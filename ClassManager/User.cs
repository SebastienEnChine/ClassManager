using System;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 用于查找用户账号是否重复
    /// </summary>
    class FindAccount<T> where T : User
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
        public bool FindAccountPredicate(T other) => other?.Account == _account;
    }
    /// <summary>
    /// 用户抽象基类
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public User()
        {
        }
        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="user">用户对象</param>
        public User(User user)
            : this(user.Account, user.Passwd, user.Name, user.Sex, user.Age, user.Address, user.UserType)
        {
            //. . . 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        public User(String account, String passwd, String name, Identity userType)
        {
            Account = account;
            Passwd = passwd;
            Name = name;
            UserType = userType;
        }
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
        public User(String account, String passwd, String name, TheSex sex, int age, String address, Identity userType)
            : this(account, passwd, name, userType)
        {
            Sex = sex;
            _age = age;
            Address = address;
        }

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
        public Identity UserType { get; set; }
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
        protected int _age; 
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value > 100 || value < 10)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                _age = value;
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        protected List<Message> History { get; set; } = new List<Message>(); 

        /// <summary>
        /// 显示学生列表
        /// </summary>
        /// <param name="info">所有学生信息</param>
        public void DisplayStudentList()
        {
            if (InformationLibrary.StudentLibrary.Count > 0)
            {
                WriteLine($"{"Name",-10} {"Sex",-10} {"Age",-10}");
                foreach (var index in InformationLibrary.StudentLibrary)
                {
                    WriteLine($"{index.Name,-10} {index.Sex,-10} {index.Age,-10}");
                }
            }
            else
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.NoDisplayableInformation);
            }
        }
        /// <summary>
        /// 显示教师列表
        /// </summary>
        /// <param name="info">所有教师信息</param>
        public void DisplayTeacherList()
        {
            if (InformationLibrary.TeacherLibrary.Count > 0)
            {
                WriteLine($"{"Name",-10} {"Sex",-10} {"Age",-10} {"Since",-10}");
                foreach (var index in InformationLibrary.TeacherLibrary)
                {
                    WriteLine($"{index.Name,-10} {index.Sex,-10} {index.Age,-10} {index.YearsOfProfessional,-10}");
                }
            }
            else
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.NoDisplayableInformation);
            }
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
        public void AddHistory(Message message) => History.Add(message);
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
    }
}
