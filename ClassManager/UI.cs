using System;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 控制台应用程序界面
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly Object localLock = new Object();
        /// <summary>
        /// 任务取消令牌
        /// </summary>
        public static CancellationTokenSource _cts = new CancellationTokenSource();
        #region 用户，学生，教师， 班主任类的扩展方法
        /// <summary>
        /// 获取帮助(指定用户)
        /// </summary>
        /// <param name="currentIdentity">用户类型</param>
        public static void GetHelp(this User me)
        {
            switch (me.UserType)
            {
                case Identity.Student:
                    GetHelpForStudent();
                    break;
                case Identity.Instructor:
                    GetHelpForInstructor();
                    break;
                case Identity.HeadTeacher:
                    GetHelpForHeadTeacher();
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        /// <summary>
        /// 获取基本帮助
        /// </summary>
        public static void GetHelpForUser()
        {
            WriteLine("SwitchUser: 切换用户");
            WriteLine("ChangePasswd: 修改密码");
            WriteLine("ChangeAge: 修改年龄");
            WriteLine("ChangeAddress: 修改地址");
            WriteLine("ChangeSex: 修改性别");
            WriteLine("ShowMe: 个人信息概览");
            WriteLine("StudentsPreview: 学生列表预览");
            WriteLine("TeachersPreview: 教师列表预览");
            WriteLine("ViewCurriculums: 查看课表");
            WriteLine("ViewMyHistory: 查看我的操作记录");
            WriteLine("Exit: 退出程序");
        }
        /// <summary>
        /// 获取帮助(学生用户)
        /// </summary>
        public static void GetHelpForStudent()
        {
            GetHelpForUser();
            WriteLine("MyScore: 查看各科目成绩");
            WriteLine("ViewNews: 查看我收到的新消息");
            WriteLine("ViewAllNews: 查看消息记录");
            WriteLine("SubscriptionToHeadTeacher: 订阅班主任");
            WriteLine("UnsubscribeToHeadTeacher: 取消订阅班主任");
        }
        /// <summary>
        /// 获取帮助(教师用户)
        /// </summary>
        public static void GetHelpForInstructor()
        {
            GetHelpForUser();
            WriteLine("AllSocre: 显示本班学生的本科目成绩(不排序)");
            WriteLine("AllSocreAndRank: 显示本班学生的本科目成绩(排序)");
            WriteLine("ChangeScore: 设置或修改学生的成绩");
            WriteLine("HighThan: 查看高于指定分数的所有学生");
            WriteLine("ReleaseAMsg: 广播一条消息");
        }
        /// <summary>
        /// 获取帮助(班主任用户)
        /// </summary>
        public static void GetHelpForHeadTeacher()
        {
            GetHelpForUser();
            WriteLine("AddStudent: 新生注册");
            WriteLine("AddTeacher: 新老师注册");
            WriteLine("Remove: 永久删除一位学生或老师账号");
            WriteLine("AllSocre: 显示本班学生的成绩(不排序)");
            WriteLine("AllSocreAndRank: 显示本班学生的成绩(排序)");
            WriteLine("HighThan: 查看总分高于指定分数的所有学生");
            WriteLine("ChangeName: 修改姓名");
            WriteLine("ReleaseNewCurriculum: 发布新课表");
            WriteLine("ReleaseAMsg: 广播一条消息");
        }
        
        /// <summary>
        /// 用户选择界面(未实现)
        /// </summary>
        /// <returns></returns>
        public static void TheSelectOfUser()
        {
            BackgroundColor = ConsoleColor.Green;
            ForegroundColor = ConsoleColor.White;
            Clear();
            SetCursorPosition(50, 10);
            WriteLine("Who are you?");
            dynamic dm = Client.GetSelectorObject<Identity>(
                new List<String>
                {
                    "Student",
                    "Teacher",
                    "HeadTeacher"
                },
                new Identity[]
                {
                    Identity.Student,
                    Identity.Instructor,
                    Identity.HeadTeacher
                }
                );
            dm.UnselectedBackground = BackgroundColor;
            dm.UnselectedForeground = ForegroundColor;

            Identity result = dm.GetSelect();
            Clear();
            WriteLine($"your selector: {result}");
            ReadKey();
        }
        /// <summary>
        /// 显示命令提示符
        /// </summary>
        /// <param name="me"></param>
        public static void Prompt(this User me)
        {
            switch (me.UserType)
            {
                case Identity.Student:
                    ForegroundColor = ConsoleColor.Cyan;
                    break;
                case Identity.Instructor:
                    ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case Identity.HeadTeacher:
                    ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Write($"[ {DateTime.Now.ToLongTimeString()} ] {me.UserType}@{me.Name}$> ");
            DefaultColor();
        }
        /// <summary>
        /// 显示登录成功问候语
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void SayHello(this User me)
        {
            switch (DateTime.Now.Hour)
            {
                case 7:
                case 8:
                case 9:
                    WriteLine($"早上好，{me.Name}~");
                    break;
                case 10:
                case 11:
                case 12:
                    WriteLine($"上午好，{me.Name}~");
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    WriteLine($"下午好，{me.Name}~");
                    break;
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                    WriteLine($"晚上好，{me.Name}~");
                    break;
                default:
                    WriteLine($"晚上好，{me.Name}~");
                    WriteLine("天很晚了，早点休息");
                    break;
            }
        }
        /// <summary>
        /// 显示学生列表
        /// </summary>
        /// <param name="info">所有学生信息</param>
        public static void DisplayStudentList(this User me)
        {
            if (InformationLibrary.StudentLibrary.Count > 0)
            {
                Boolean IsWhite = true;
                WriteLine($"{"Name",-10} {"Sex",-10} {"Age",-10}");
                Parallel.ForEach(InformationLibrary.StudentLibrary, stu =>
                {
                    lock (localLock)
                    {
                        if (IsWhite)
                        {
                            UI.PrintColorMsg($"{stu.Name,-10} {stu.Sex,-10} {stu.Age,-10}", ConsoleColor.White, ConsoleColor.Black);
                            WriteLine();
                            IsWhite = false;
                        }
                        else
                        {
                            UI.PrintColorMsg($"{stu.Name,-10} {stu.Sex,-10} {stu.Age,-10}", ConsoleColor.Black, ConsoleColor.White);
                            WriteLine();
                            IsWhite = true;
                        }
                    }
                });
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
        public static void DisplayTeacherList(this User me)
        {
            if (InformationLibrary.TeacherLibrary.Count > 0)
            {
                Boolean IsWhite = true;
                WriteLine($"{"Name",-10} {"Sex",-10} {"Age",-10} {"Since",-10}");
                Parallel.ForEach(InformationLibrary.TeacherLibrary, teacher =>
                {
                    lock (localLock)
                    {
                        if (IsWhite)
                        {
                            UI.PrintColorMsg($"{teacher.Name,-10} {teacher.Sex,-10} {teacher.Age,-10} {teacher.YearsOfProfessional,-10}", ConsoleColor.White, ConsoleColor.Black);
                            WriteLine();
                            IsWhite = false;
                        }
                        else
                        {
                            UI.PrintColorMsg($"{teacher.Name,-10} {teacher.Sex,-10} {teacher.Age,-10} {teacher.YearsOfProfessional,-10}", ConsoleColor.Black, ConsoleColor.White);
                            WriteLine();
                            IsWhite = true;
                        }
                    }
                });
            }
            else
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.NoDisplayableInformation);
            }
        }
        /// <summary>
        /// 修改密码(调用和异常处理)
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void CallChangeMyPasswd(this User me)
        {
            try
            {
                me.ChangeMyPasswd();
            }
            catch (FormatException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentError);
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void ChangeMyPasswd(this User me)
        {
            Write("请输入当前密码: ");
            String currentPasswd = ReadLine();
            Write("请输入新密码: ");
            String firstPasswd = ReadLine();
            Write("请再次输入新密码: ");
            String secondPasswd = ReadLine();
            if (!firstPasswd.Equals(secondPasswd))
            {
                DisplayTheInformationOfErrorCode(ErrorCode.InconsistentPassword);
            }
            else
            {
                me.Passwd = secondPasswd;
                me.AddHistory(new Message("你", "重新设置了密码"));
                DisplayTheInformationOfSuccessfully();
            }
        }
        /// <summary>
        /// 修改年龄(调用和异常处理)
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void CallChangeMyAge(this User me)
        {
            try
            {
                me.ChangeMyAge();
            }
            catch (ArgumentOutOfRangeException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
            catch (ArgumentException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentError);
            }
        }
        /// <summary>
        /// 修改年龄
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void ChangeMyAge(this User me)
        {
            Write("请输入你当前的年龄: ");
            if (Int32.TryParse(ReadLine(), out Int32 age))
            {
                me.Age = age;
                me.AddHistory(new Message("你", $"重新设置了年龄({me.Age})"));
                DisplayTheInformationOfSuccessfully();
            }
            else
            {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void ChangeMyAddress(this User me)
        {
            Write("地址: ");
            String address = ReadLine();
            me.AddHistory(new Message("你", $"重新设置了地址({me.Address = address})"));
            DisplayTheInformationOfSuccessfully();
        }
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void ChangeMySex(this User me)
        {
            WriteLine("设置新性别: (选择: 上/下方向键   确定: 回车键) ");
            dynamic dm = Client.GetSelectorObject(new List<String> { "男", "女" }, TheSex.Male, TheSex.Frame);
            TheSex result = dm.GetSubject();
            //不使用动态加载
            //TheSex result = new Selector<TheSex>(new List<String> { "男", "女" }, TheSex.Male, TheSex.Frame).GetSubject();
            me.AddHistory(new Message("你", $"重新设置了性别({me.Sex = result})"));
            DisplayTheInformationOfSuccessfully();
        }
        /// <summary>
        /// 查看我的操作记录
        /// </summary>
        /// <param name="me">当前用户</param>
        public static void ViewMyHistory(this User me)
        {
            for(var historyIndex = me.GetHistory().Count - 1; historyIndex >= 0; --historyIndex)
            {
                WriteLine(me.GetHistory()[historyIndex]);
            }
        }
        /// <summary>
        /// 查看课表
        /// </summary>
        public static void ViewCurriculum(this User me)
        {
            Client.UpdateCurriculum();
            if (InformationLibrary._curriculums[0] != null)
            {
                WriteLine($"本周课表：\n于{InformationLibrary._curriculums[0].OverTime}失效");
                InformationLibrary._curriculums[0].Draw();
                if (InformationLibrary._curriculums[1] != null)
                {
                    WriteLine($"下周课表：\n于{InformationLibrary._curriculums[1].OverTime}失效");
                    InformationLibrary._curriculums[1].Draw();
                }
            }
            else
            {
                DisplayTheInformationOfErrorCode(ErrorCode.NoDisplayableInformation);
            }
        }

        /// <summary>
        /// 新消息提示
        /// </summary>
        /// <param name="stu"></param>
        public static void TheTipsOfNews(this Student stu)
        {
            if (stu.HasNewMsg)
            {
                SetCursorPosition(0, CursorTop);
                PrintColorMsg($"[ {DateTime.Now.ToLongTimeString()} ] {stu.UserType}@{stu.Name}", ConsoleColor.Black, ConsoleColor.Cyan);
                PrintColorMsg("[新消息]", ConsoleColor.Black, ConsoleColor.DarkMagenta);
                PrintColorMsg(">", ConsoleColor.Black, ConsoleColor.Cyan);
            }
        }

        /// <summary>
        /// 显示成绩列表
        /// </summary>
        /// <param name="teacher"></param>
        public static void DisplayAllScoreOfStudent(this ITeacher teacher, State IsDisplayRank = State.off, State IsSort = State.off)
        {
            if (InformationLibrary.StudentLibrary.Count <= 0)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.NoDisplayableInformation);
                return;
            }
            DisplayAllScoreTitle(IsSort);
            teacher.ViewScoreOfAllStudent(IsDisplayRank, IsSort);

            //用于显示成绩的列的标题
            void DisplayAllScoreTitle(State IsSortAndRank)
            {
                if (IsSortAndRank == State.on)
                {
                    Write($"{"No.",-10}");
                }
                WriteLine($"{"Name",-10}{"C",-10}{"Java",-10}{"C++",-10}{"C#",-10}{"Html&Css",-10}{"SQL",-10}{"Total",-10}");
            }
        }
        /// <summary>
        /// 显示高于指定分数的所有学生
        /// </summary>
        /// <param name="teacher"></param>
        public static void DisplayScoreHighThan(this ITeacher teacher)
        {
            try
            {
                Write("你想将分数指定为: ");
                teacher.GetStuHighThan(Int32.Parse(ReadLine()));
            }
            catch (FormatException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentError);
            }
            catch (ArgumentOutOfRangeException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
        }
        /// <summary>
        /// 广播一条消息
        /// </summary>
        /// <param name="teacher"></param>
        public static void ReleaseNewMessage(this Teacher teacher)
        {
            Write("在此处输入将要广播的消息> ");
            Message msg = new Message("班主任", ReadLine());
            teacher.ReleaseNewMsg(msg);
            teacher.AddHistory(new Message("你", $"广播了一条消息: {msg.Content}"));
            DisplayTheInformationOfSuccessfully("(广播已发布)");
        }

        /// <summary>
        /// 修改姓名(针对于班主任用户)
        /// </summary>
        /// <param name="headteacher">当前用户</param>
        public static void ChangeMyName(this HeadTeacher ht)
        {
            Write("新姓名: ");
            String name = ReadLine();

            InformationLibrary.HeadTeacherUser.Name = name;

            InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", $"重新设置了你的姓名({name})"));
            DisplayTheInformationOfSuccessfully();
        }
        /// <summary>
        /// 修改学生或教师的姓名(针对于班主任用户)
        /// </summary>
        /// <param name="me">执行该命令的对象</param>
        public static void ChangeNameOfOtherUser(this HeadTeacher ht)
        {
            Write("请输入将要更改姓名的账户：");
            String account = ReadLine();
            for (Int32 index = 0; index < InformationLibrary.StudentLibrary.Count; ++index)
            {
                if (account == InformationLibrary.StudentLibrary[index].Account)
                {
                    Write("新姓名: ");
                    String name = ReadLine();
                    InformationLibrary.StudentLibrary[index].Name = name;
                    InformationLibrary.HeadTeacherUser.AddHistory(
                        new Message("你", $"将{InformationLibrary.StudentLibrary[index].Account}的姓名重新设置为({name})")
                        );
                    DisplayTheInformationOfSuccessfully();
                    return;
                }
            }
            for (Int32 index = 0; index < InformationLibrary.TeacherLibrary.Count; ++index)
            {
                if (account == InformationLibrary.TeacherLibrary[index].Account)
                {
                    Write("新姓名: ");
                    String name = ReadLine();

                    InformationLibrary.TeacherLibrary[index].Name = name;
                    InformationLibrary.HeadTeacherUser.AddHistory(
                        new Message("你", $"将{InformationLibrary.TeacherLibrary[index].Account}的姓名重新设置为({name})")
                        );
                    DisplayTheInformationOfSuccessfully();
                    return;
                }
            }
        }

        /// <summary>
        /// 添加新生
        /// </summary>
        /// <param name="info">所有用户信息库</param>
        public static void CallAddStudent(this HeadTeacher ht)
        {
            try
            {
                ht.AddStudent();
            }
            catch (NullReferenceException ex) when (ex.Message.Equals("Account is Empty"))
            {
                DisplayTheInformationOfErrorCode(ErrorCode.BadAccount);
            }
            catch (NullReferenceException ex) when (ex.Message.Equals("Passwd is Empty"))
            {
                DisplayTheInformationOfErrorCode(ErrorCode.BadPasswd);
            }
        }
        /// <summary>
        /// 添加新生
        /// </summary>
        /// <param name="info">所有用户信息库</param>
        public static void AddStudent(this HeadTeacher ht)
        {
            Write("账号: ");
            String account = ReadLine();
            if (account.Contains(" ") || account.Equals(String.Empty))
            {
                throw new NullReferenceException("Account is Empty");
            }

            if (Client.CheckAccountAvailability(account) != null)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.AccountAlreadyExists, account);
                return;
            }

            Write("密码: ");
            String passwd = EnterPasswd();
            if (passwd.Contains(" ") || passwd.Equals(String.Empty))
            {
                throw new NullReferenceException("Passwd is Empty");
            }

            Write("姓名: ");
            String name = ReadLine();
            InformationLibrary.StudentLibrary.Add(new Student(account, passwd, name));
            InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", $"注册了一个学生账户({account})"));
            ht.ReleaseNewMsg(new Message("班主任", $"班里来了一位新同学({account}), 快去看看吧~"));
            DisplayTheInformationOfSuccessfully();
        }
        /// <summary>
        /// 添加/注册新老师(调用和异常处理)
        /// </summary>
        /// <param name="info">所有用户信息库</param>
        public static void CallAddTeacher(this HeadTeacher ht)
        {
            try
            {
                ht.AddTeacher();
            }
            catch (NullReferenceException ex) when (ex.Message.Equals("Account is Empty"))
            {
                DisplayTheInformationOfErrorCode(ErrorCode.BadAccount);
            }
            catch (NullReferenceException ex) when (ex.Message.Equals("Passwd is Empty"))
            {
                DisplayTheInformationOfErrorCode(ErrorCode.BadAccount);
            }
            catch (ArgumentException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentError);
            }
        }
        /// <summary>
        /// 添加/注册新老师
        /// </summary>
        /// <param name="info">所有用户信息库</param>
        public static void AddTeacher(this HeadTeacher ht)
        {
            WriteLine("你想创建哪一科目的任课教师? (选择: 上/下方向键   确定: 回车键) ");
            #region 不使用动态加载
            //Subject result = new Selector<Subject>(
            //                              new List<String>()
            //                               {
            //                                    "C语言任课老师",
            //                                    "C++任课老师",
            //                                    "C#任课老师",
            //                                    "HTML/Css任课老师",
            //                                    "Java任课老师",
            //                                    "SQL数据库任课老师"
            //                               },
            //                              new Subject[]
            //                              {
            //                                     Subject.C,
            //                                     Subject.CPlusPlus,
            //                                     Subject.CSharp,
            //                                     Subject.HtmlAndCss,
            //                                     Subject.Java,
            //                                     Subject.SQL
            //                              }
            //                           ).GetSubject();
            #endregion
            dynamic dm = Client.GetSelectorObject(
                                            new List<String>()
                                            {
                                                "C语言任课老师",
                                                "C++任课老师",
                                                "C#任课老师",
                                                "HTML/Css任课老师",
                                                "Java任课老师",
                                                "SQL数据库任课老师"
                                            },
                                          new Subject[]
                                          {
                                                 Subject.C,
                                                 Subject.CPlusPlus,
                                                 Subject.CSharp,
                                                 Subject.HtmlAndCss,
                                                 Subject.Java,
                                                 Subject.SQL
                                          }) ?? throw new NullReferenceException();
            Subject result = dm.GetSubject();
            Write("账号: ");
            String account = ReadLine();
            if (account.Contains(" ") || account.Equals(String.Empty))
            {
                throw new NullReferenceException("Account is Empty");
            }
            if (Client.CheckAccountAvailability(account) != null)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.AccountAlreadyExists, account);
                return;
            }

            Write("密码: ");
            String passwd = EnterPasswd();
            if (passwd.Contains(" ") || passwd.Equals(String.Empty))
            {
                throw new NullReferenceException("Passwd is Empty");
            }

            Write("姓名: ");
            String name = ReadLine();

            WriteLine("此用户从哪一年开始从事该行业？");
            Write("> ");
            if (!Int32.TryParse(ReadLine(), out Int32 years))
            {
                throw new ArgumentException();
            }

            InformationLibrary.TeacherLibrary.Add(new Instructor(account, passwd, name, years, result));
            InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", $"注册了一个教师账户({account})"));
            ht.ReleaseNewMsg(new Message("班主任", $"班里来了一位新老师({account}), 快去看看吧~"));
            DisplayTheInformationOfSuccessfully();
        }
        /// <summary>
        /// 修改学生的成绩(调用和异常处理)
        /// </summary>
        /// <param name="curr">执行此命令的老师</param>
        public static void CallChangeScore(this Instructor curr)
        {
            try
            {
                curr.ChangeScore();
            }
            catch (NullReferenceException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.CantFindThisAccount);
            }
            catch (ArgumentOutOfRangeException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
            catch (ArgumentException)
            {
                DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }
        /// <summary>
        /// 修改学生的成绩
        /// </summary>
        /// <param name="curr">执行此命令的老师</param>
        public static void ChangeScore(this Instructor curr)
        {
            Write("请输入将要修改的学生的账户: ");
            String account = ReadLine();
            Student temp = InformationLibrary.StudentLibrary
                .Find(s => s.Account == account) as Student
                ?? throw new NullReferenceException();
            Write("分数: ");
            temp[curr.TeachingRange] = Double.Parse(ReadLine());
            curr.AddHistory(new Message("你", $"修改了{account}的分数: {temp[curr.TeachingRange]}"));
        }
        /// <summary>
        /// 移除账户   TODO: 需要更优解决方案
        /// </summary>
        public static void RemoveAccount(this HeadTeacher ht)
        {
            Write("请输入将要移除的账户：");
            String account = ReadLine();
            for (Int32 index = 0; index < InformationLibrary.StudentLibrary.Count; ++index)
            {
                if (account == InformationLibrary.StudentLibrary[index].Account)
                {
                    Write("确定要删除吗？(Y/N) : ");
                    InformationLibrary.StudentLibrary[index].ViewPersonalInformation();
                    String result = ReadLine();
                    if (result.Equals("y") || result.Equals("Y") || result.Equals(String.Empty))
                    {
                        InformationLibrary.StudentLibrary.RemoveAt(index);
                        InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", $"删除了一个学生账户({account})"));
                        DisplayTheInformationOfSuccessfully();
                    }
                    return;
                }
            }
            for (Int32 index = 0; index < InformationLibrary.TeacherLibrary.Count; ++index)
            {
                if (account == InformationLibrary.TeacherLibrary[index].Account)
                {
                    Write("确定要删除吗？(Y/N) : ");
                    InformationLibrary.TeacherLibrary[index].ViewPersonalInformation();
                    String result = ReadLine();
                    if (result.Equals("y") || result.Equals("Y") || result.Equals(String.Empty))
                    {
                        InformationLibrary.TeacherLibrary.RemoveAt(index);
                        InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", $"删除了一个教师账户({account})"));
                        DisplayTheInformationOfSuccessfully();
                    }
                    return;
                }
            }
            DisplayTheInformationOfErrorCode(ErrorCode.CantFindThisAccount, account);
        }
        #endregion

        /// <summary>
        /// 关于
        /// </summary>
        public static void AboutThisApplication()
        {
            ForegroundColor = ConsoleColor.White;
            WriteLine("-------------------------------------------------------------------------");
            WriteLine("  [ Student Manager Studio <SMS> ]");
            WriteLine("  Application Version：V1.0.0");
            WriteLine("  Developer: Sébastien Muller(sebastienenchine@outlook.com)");
            WriteLine("  Please Login to use this program");
            WriteLine("  Enter GetHelp to get help");
            WriteLine("-------------------------------------------------------------------------");
            DefaultColor();
        }
        /// <summary>
        /// 程序默认设置
        /// </summary>
        public static void DefaultSetting()
        {
            DefaultSize();
            DefaultColor();
            DefaultTitle();
        }
        /// <summary>
        /// 程序窗口大小默认设置
        /// </summary>
        public static void DefaultSize()
        {
            SetWindowSize(74, 41);
        }
        /// <summary>
        /// 程序窗口颜色默认设置
        /// </summary>
        public static void DefaultColor()
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Blue;
        }
        /// <summary>
        /// 程序窗口标题默认设置
        /// </summary>
        public static void DefaultTitle()
        {
            Title = "StudentManagerStudio";
        }

        /// <summary>
        /// 获取用户输入的密码
        /// </summary>
        /// <returns>密码</returns>
        public static String EnterPasswd()
        {
            String key = String.Empty;
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) //按下回车，结束
                    break;
                else if (keyInfo.Key == ConsoleKey.Backspace && key.Length > 0) //如果是退格键并且字符没有删光
                {
                    Write("\b \b"); //输出一个退格（此时光标向左走了一位），然后输出一个空格取代最后一个星号，然后再往前走一位，也就是说其实后面有一个空格但是你看不见= =
                    key = key.Substring(0, key.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar)) //过滤掉功能按键等
                {
                    key += keyInfo.KeyChar.ToString();
                    Write("*");
                }
            }
            WriteLine();
            return key;
        }
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns>登录信息</returns>
        public static Tuple<String, String> GetInformationForLogin()
        {
            Write("账号: ");
            String account = ReadLine();
            Write("密码: ");
            String passwd = EnterPasswd();
            return Tuple.Create(account, passwd);
        }

        /// <summary>
        /// 显示错误代码信息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="parameter">错误参数信息</param>
        public static void DisplayTheInformationOfErrorCode(ErrorCode code, String parameter = "parameter")
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.DarkRed;
            switch (code)
            {
                case ErrorCode.NotACommand:
                    WriteLine($"{parameter} : 无法将 \"{parameter}\" 识别为可执行命令的名称。请检查名称的拼写，然后再试一次。");
                    break;
                case ErrorCode.AccountOrPasswdError:
                    WriteLine("账号或密码错误, 请检查账号和密码的正确性，然后再试一次");
                    break;
                case ErrorCode.AccountAlreadyExists:
                    WriteLine($"{parameter} : 该账户已存在, 请检查此项， 然后再试一次");
                    break;
                case ErrorCode.CantFindThisAccount:
                    WriteLine($"{parameter}: 找不到此账户, 请检查此项, 然后再试一次");
                    break;
                case ErrorCode.ArgumentError:
                    WriteLine("参数错误, 请检查该项，然后再试一次");
                    break;
                case ErrorCode.InconsistentPassword:
                    WriteLine("两次输入密码不一致, 请检查该项，然后再试一次");
                    break;
                case ErrorCode.PasswdError:
                    WriteLine("密码错误, 请检查该项，然后再试一次");
                    break;
                case ErrorCode.NoDisplayableInformation:
                    WriteLine("目前还没有可显示信息");
                    break;
                case ErrorCode.CantAdd:
                    WriteLine("目前已经有两张课表， 请在第一张课表失效后重试");
                    break;
                case ErrorCode.NotSubscribedYet:
                    WriteLine("你没有订阅班主任, 无法取消订阅");
                    break;
                case ErrorCode.DuplicateSubscriptions:
                    WriteLine("你已经订阅了班主任, 无法重复订阅");
                    break;
                case ErrorCode.ArgumentOutOfRange:
                    WriteLine("参数过大或过小");
                    break;
                case ErrorCode.BadAccount:
                    WriteLine("账号不能为空或者包含空格");
                    break;
                case ErrorCode.BadPasswd:
                    WriteLine("密码不能为空或者包含空格");
                    break;
                default:
                    break;
            }
            DefaultColor();
        }
        /// <summary>
        /// 操作成功提示
        /// </summary>
        public static void DisplayTheInformationOfSuccessfully(String msg = "(已完成)")
        {
            //BackgroundColor = ConsoleColor.Black;
            //ForegroundColor = ConsoleColor.Green;
            //Beep();
            //WriteLine(msg);
            //DefaultColor();
            Process.Start(@"D:\Document\Workspace\C_SHARP\ConsoleApps\ClassManager\DisplayInfo\bin\Debug\DisplayInfo.exe", msg);
        }
        /// <summary>
        /// 将字符串以指定颜色显示
        /// </summary>
        /// <param name="msg">字符串</param>
        /// <param name="bg">背景颜色</param>
        /// <param name="fg">前景颜色</param>
        public static void PrintColorMsg(String msg, ConsoleColor bg, ConsoleColor fg)
        {
            BackgroundColor = bg;
            ForegroundColor = fg;
            Write(msg);
            DefaultColor();
        }
    }
}
