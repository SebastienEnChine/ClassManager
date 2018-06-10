using System;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 用于查找用户账号是否重复
    /// </summary>
    public class FindAccount<T> where T : User
    {
        /// <summary>
        /// 账号
        /// </summary>
        private readonly String _account; 
        /// <summary>
        /// 查找方式
        /// </summary>
        /// <param name="account">账号</param>
        public FindAccount(String account) => _account = account;
        /// <summary>
        /// 比较账号是否重复
        /// </summary>
        /// <param name="other">比较目标</param>
        /// <returns>true: 重复 false: 不重复</returns>
        public Boolean FindAccountPredicate(T other) => other?.Account == _account;
    }
    /// <summary>
    /// 用户抽象基类
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected User()
        {
        }
        /// <inheritdoc />
        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="user">用户对象</param>
        protected User(User user)
            : this(user.Account, user.Passwd, user.Name, user.Sex, user.Age, user.Address, user.UserType)
        {
            //. . . 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="name"></param>
        /// <param name="userType">用户类型</param>
        protected User(String account, String passwd, String name, Identity userType)
        {
            Account = account;
            Passwd = passwd;
            Name = name;
            UserType = userType;
        }
        /// <inheritdoc />
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        /// <param name="name">姓名</param>
        /// <param name="sex">性别</param>
        /// <param name="age">年龄</param>
        /// <param name="address">地址</param>
        protected User(String account, String passwd, String name, TheSex sex, Int32 age, String address, Identity userType)
            : this(account, passwd, name, userType)
        {
            Sex = sex;
            _age = age;
            Address = address;
        }

        /// <summary>
        /// 登录计时任务取消令牌
        /// </summary>
        protected CancellationTokenSource _cts = default(CancellationTokenSource);
        /// <summary>
        /// 登录管道输出
        /// </summary>
        public static readonly BufferBlock<User> Result = new BufferBlock<User>();
        /// <summary>
        /// 计时器
        /// </summary>
        protected Stopwatch _sw = default;

        /// <summary>
        /// 账号
        /// </summary>
        public String Account { get; }
        /// <summary>
        /// 密码
        /// </summary>
        public String Passwd { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public Identity UserType { get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; } = DateTime.Now;
        /// <summary>
        /// 姓名
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public TheSex Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        protected Int32 _age;
        public Int32 Age
        {
            get => _age;
            set
            {
                if (value > 100 || value < 10)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                _age = value;
                Ui.SaveHeadTeacher();
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public String Address { private get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        protected List<Message> History { get; } = new List<Message>();

        /// <summary>
        /// 注销登录
        /// </summary>
        public void LogOut() => _cts?.Cancel();
        /// <summary>
        /// 登录计时(异常处理)
        /// </summary>
        protected async void CallTimingAndException()
        {
            try
            {
                await Timing();
            }
            catch (OperationCanceledException)
            {
                AddHistory(new Message("此次上线时间", $"[{_sw.Elapsed.Hours:00}:{_sw.Elapsed.Minutes:00}:{_sw.Elapsed.Seconds:00}]"));
            }
        }
        /// <summary>
        /// 计时
        /// </summary>
        /// <returns></returns>
        protected Task Timing()
        {
            _cts = new CancellationTokenSource();
            _sw = new Stopwatch();
            _sw.Start();
            return new Task(() =>
            {
                while (true)
                 {
                    if (_cts.IsCancellationRequested)
                    {
                        _sw.Stop();
                        _cts = null;
                        _sw = null;
                        throw new OperationCanceledException();
                    }
                    //_cts.Token.ThrowIfCancellationRequested();
                 }
            }, TaskCreationOptions.LongRunning);
        }
        /// <summary>
        /// 登录动作管道
        /// </summary>
        public static ITargetBlock<Func<Tuple<String, String>>> LoginWithBlock()
        {
            var loginInfo = new TransformBlock<Func<Tuple<String, String>>, Tuple<String, String>>(info => info?.Invoke());
            var isAUser = new TransformBlock<Tuple<String, String>, User>(info => Client.IdentityCheck(info));

            loginInfo.LinkTo(isAUser);
            isAUser.LinkTo(Result);
            return loginInfo;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>User: sucessfully, null: faild</returns>
        public static User Login()
        {
            ITargetBlock<Func<Tuple<String, String>>> result = LoginWithBlock();
            result.Post(Ui.GetInformationForLogin);
            User user = Result.Receive();
            //User user = Client.IdentityCheck(UI.GetInformationForLogin());
            if (user == null)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.AccountOrPasswdError);
            }
            else
            {
                Ui.DisplayTheInformationOfSuccessfully("(登录成功)");
                Title = $"Student Manager Studio to [{user.Name}]";
                user.SayHello();
                user.AddHistory(new Message("登录操作", "登录成功"));
                user.CallTimingAndException();
            }
            return user;
        }
        /// <summary>
        /// 切换用户
        /// </summary>
        public User SwitchUser()
        {
            User user = Login();
            if (user != null)
            {
                LogOut();
            }
            return user;
        }
        /// <summary>
        /// 查看班主任信息
        /// </summary>
        public virtual void ViewTheInformationOfTheHeadteacher()
        {
            WriteLine($"姓名: {InformationLibrary.HeadTeacherUser.Name}");
            WriteLine($"性别: {InformationLibrary.HeadTeacherUser.Sex}");
            WriteLine($"年龄: {InformationLibrary.HeadTeacherUser.Age}");
            WriteLine($"从业年份: {InformationLibrary.HeadTeacherUser.YearsOfProfessional}");
        }
        /// <summary>
        /// 向操作记录中添加新的操作信息
        /// </summary>
        /// <param name="message"></param>
        public async void AddHistory(Message message) => await Task.Run(() => History.Add(message));

        /// <summary>
        /// 获取全部操作记录
        /// </summary>
        /// <returns></returns>
        public List<Message> GetHistory() => History;
        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override String ToString()
                        => $"账户: {Account}\n账户类型:{UserType}\n账户创建时间: {CreatedTime}\n姓名: {Name}\n性别: {Sex}\n年龄: {Age}\n地址: {Address}\n";
        /// <summary>
        /// 个人信息概览
        /// </summary>
        public void ViewPersonalInformation() => WriteLine(this);
        /// <summary>
        /// 在公共留言墙上写一条公共留言
        /// </summary>
        /// <param name="me"></param>
        public  void LeaveAMessage()
        {
            Write("请输入你想说的话: > ");
            String msg = $"[{DateTime.Now.ToLocalTime()}] {Name,-15} : {ReadLine()}{Environment.NewLine}";

            AddHistory(new Message("你", $"[公共留言墙留言]: {msg}"));
            //using (FileStream inputStream = File.OpenWrite(Client.FileName))
            //{
            //    inputStream.Seek(0, SeekOrigin.End);

            //    Byte[] buffer = Encoding.Default.GetBytes(msg);
            //    inputStream.Write(buffer, 0, buffer.Length);
            //}
            FileStream inputStream = File.OpenWrite(Client.FileName);
            using (var writer = new BinaryWriter(inputStream))
            {
                writer.Seek(0, SeekOrigin.End);
                writer.Write(msg);
            }
            Ui.DisplayTheInformationOfSuccessfully("留言成功~");
        }
        /// <summary>
        /// 浏览公共留言墙
        /// </summary>
        /// <param name="me"></param>
        public  void ViewTheLeaveMessages()
        {
            //using (FileStream outputStream = File.OpenRead(Client.FileName))
            //{
            //    Boolean isCompleted = false;
            //    Byte[] buffer = new Byte[256];
            //    do
            //    {
            //        Int32 readCount = outputStream.Read(buffer, 0, buffer.Length);
            //        if (readCount == 0)
            //        {
            //            isCompleted = true;
            //        }
            //        else if (readCount < buffer.Length)
            //        {
            //            Array.Clear(buffer, readCount, buffer.Length - readCount);
            //        }
            //        String msg = Encoding.Default.GetString(buffer, 0, buffer.Length);
            //        WriteLine(msg);
            //    } while (!isCompleted);
            //}
            BinaryReader reader = null;
            try
            {
                FileStream input = File.OpenRead(Client.FileName);
                reader = new BinaryReader(input);
                while (true)
                {
                    Write(reader.ReadString());
                }
            }
            catch (EndOfStreamException)
            {
                WriteLine("--------------END-------------");
            }
            finally
            {
                reader.Dispose();
            }
            //File.ReadLines(Client.FileName, Encoding.Default).ToList().ForEach(WriteLine);
        }

        /// <summary>
        /// 测试此计算机是否能被指定的Web地址响应
        /// </summary>
        public void UrlTest()
        {
            Write("在测试之前, 请提供Web地址(如: http://www.baidu.com):  ");
            CallGetInfoOfUrl(ReadLine());
            Ui.PrintColorMsg($"已将此任务放入后台, 任务完成之后会以窗口的形式报告结果, 在此期间你可以进行其他操作.{Environment.NewLine}", ConsoleColor.Black, ConsoleColor.DarkMagenta);
        }
        /// <summary>
        /// 发送Http请求(调用和异常处理)
        /// </summary>
        /// <param name="url"></param>
        public async void CallGetInfoOfUrl(String url)
        {
            try
            {
                await GetInfoOfUrl(url);
            }
            catch(InvalidOperationException ex)
            {
                Ui.DisplayTheInformationOfSuccessfully(ex.Message);
            }
            catch(HttpRequestException ex)
            {
                Ui.DisplayTheInformationOfSuccessfully(ex.Message);
            }
        }
        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task GetInfoOfUrl(String url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    String httpText = await response.Content.ReadAsStringAsync();
                    String savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{url.Split('.')[1]}.html");
                    File.WriteAllText(savePath, httpText);
                    await Task.Delay(5000); //模拟耗时请求
                    Ui.DisplayTheInformationOfSuccessfully($"请求已被响应! 网页已被保存到: 我的文档\\{url.Split('.')[1]}.html");
                }
            }
        }
    }
}
