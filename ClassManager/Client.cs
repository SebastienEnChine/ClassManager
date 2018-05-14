using System;
using static System.Console;
using Sebastien.ClassManager.Enums;
using System.Reflection;
#region 其他需求
//其他: 
//          2: 切换用户之后 窗口标题显示对应用户类型
#endregion

#region 扩展待实现
//学生：
//          3：查看班主任信息
//教师：
//          3：查看班主任信息
//班主任
//          1：
#endregion

#region 待解决问题
//中英文对齐问题   
#endregion

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed class Client
    {
        /// <summary>
        /// 程序状态
        /// </summary>
        private static State ApplicationState { get; set; } = State.on;

        /// <summary>
        /// 程序主逻辑
        /// </summary>
        /// <param name="args">命令行参数</param>
        static void Main(string[] args)
        {
            UI.DefaultSetting();
            UI.AboutThisApplication();

            User currentUser = Login();

            while (ApplicationState == State.on)
             {
                currentUser.Prompt();
                switch (currentUser.UserType)
                {
                    case Identity.Student:
                        currentUser = RunForStudent(currentUser as Student) ?? currentUser;
                        break;
                    case Identity.Instructor:
                        currentUser = RunForTeacher(currentUser as Instructor) ?? currentUser;
                        break;
                    case Identity.HeadTeacher:
                        currentUser = RunForHeadTeacher(currentUser as HeadTeacher) ?? currentUser;
                        break;
                    default:
                        throw new ArgumentException();
                }
             }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>User: sucessfully, null: faild</returns>
        public static User Login()
        {
            User user = null;
            while (true)
            {
                user = IdentityCheck(UI.GetInformationForLogin());
                if (user == null)
                {
                    UI.DisplayTheInformationOfErrorCode(ErrorCode.AccountOrPasswdError);
                }
                else
                {
                    UI.DisplayTheInformationOfSuccessfully("(登录成功)");
                    user.SayHello();
                    break;
                }
            }
            return user;
        }
        /// <summary>
        /// 加载选择器
        /// </summary>
        /// <returns>选择器对象</returns>
        public static object GetSelectorObject() //TODO:
        {
            Assembly asm = Assembly.LoadFrom(@"D:\Document\Workspace\C_SHARP\ConsoleApps\ClassManager\ClassManager\bin\Debug\SelectorLib.dll");
            return asm.CreateInstance("MySelector.Selector`1");

        }
        /// <summary>
        /// 获取由用户输入的命令
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <returns></returns>
        public static String GetCmd(String input, out Command cmd)
        {
            if (!Enum.TryParse(input, true, out cmd))
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.NotACommand, input);
                return null;
            }
            return input;
        }
        /// <summary>
        /// 交互(针对于所有用户)
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="cmd">可执行命令</param>
        /// <returns></returns>
        public static User RunForUser(User currentUser, Command cmd)
        {
            User user = null;
            switch (cmd)
            {
                case Command.GetHelp:
                    currentUser.GetHelp();
                    break;
                case Command.SwitchUser:
                    user = IdentityCheck(UI.GetInformationForLogin());
                    if (user == null)
                    {
                        UI.DisplayTheInformationOfErrorCode(ErrorCode.AccountOrPasswdError);
                    }
                    else
                    {
                        UI.DisplayTheInformationOfSuccessfully("(登录成功)");
                        user.SayHello();
                    }
                    break;
                case Command.ShowMe:
                    currentUser.ViewPersonalInformation();
                    break;
                case Command.ChangePasswd:
                    currentUser.CallChangeMyPasswd();
                    break;
                case Command.ChangeAge:
                    currentUser.CallChangeMyAge();
                    break;
                case Command.ChangeAddress:
                    currentUser.ChangeMyAddress();
                    break;
                case Command.ChangeSex:
                    currentUser.ChangeMySex();
                    break;
                case Command.ViewMyHistory:
                    currentUser.ViewMyHistory();
                    break;
                case Command.ViewCurriculums:
                    user.ViewCurriculum();
                    break;
                case Command.ViewHeadTeacher:
                    user.ViewTheInformationOfTheHeadteacher();
                    break;
                case Command.Exit:
                    ApplicationState = State.off;
                    break;
                default:
                    UI.DisplayTheInformationOfErrorCode(ErrorCode.NotACommand, cmd.ToString());
                    break;
            }
            return user;
        }
        /// <summary>
        /// 交互(针对于学生用户)
        /// </summary>
        /// <param name="stu">学生用户</param>
        /// <returns>用户对象</returns>
        public static User RunForStudent(Student stu)
        {
            stu.TheTipsOfNews();
            ForegroundColor = ConsoleColor.Yellow;
            String input = ReadLine();
            ForegroundColor = ConsoleColor.Blue;
            if (input.Equals(String.Empty))
            {
                return null;
            }

            if (GetCmd(input, out Command cmd) == null)
            {
                return null;
            }
            User result = null;
            switch (cmd)
            {
                case Command.Exit:
                case Command.SwitchUser:
                case Command.GetHelp:
                case Command.ShowMe:
                case Command.ChangePasswd:
                case Command.ChangeAge:
                case Command.ChangeAddress:
                case Command.ChangeSex:
                case Command.ViewMyHistory:
                case Command.ViewCurriculums:
                case Command.ViewHeadTeacher:
                    result = RunForUser(stu, cmd);
                    break;
                case Command.ViewNews:
                    stu.ViewNews();
                    break;
                case Command.ViewAllNews:
                    stu.ViewTotalNews();
                    break;
                case Command.MyScore:
                    stu.ShowMyScore();
                    break;
                case Command.StudentsPreview:
                    stu.DisplayStudentList();
                    break;
                case Command.TeachersPreview:
                    stu.DisplayTeacherList();
                    break;
                case Command.SubscriptionToHeadTeacher:
                    stu.SubscriptionToHeadTeacher(InformationLibrary.HeadTeacherUser);
                    break;
                case Command.UnsubscribeToHeadTeacher:
                    stu.UnsubscribeToHeadTeacher(InformationLibrary.HeadTeacherUser);
                    break;
                default:
                    UI.DisplayTheInformationOfErrorCode(ErrorCode.NotACommand, input);
                    break;
            }
            return result;
        }
        ///         /// <summary>
        /// 交互(针对于各科目任课老师用户)
        /// </summary>
        /// <param name="headTeacher">班主任用户</param>
        /// <returns>用户对象</returns>
        public static User RunForTeacher(Instructor teacher)
        {
            ForegroundColor = ConsoleColor.Yellow;
            String input = ReadLine();
            ForegroundColor = ConsoleColor.Blue;
            if (input.Equals(String.Empty))
            {
                return null;
            }

            if (GetCmd(input, out Command cmd) == null)
            {
                return null;
            }
            User result = null;
            switch (cmd)
            {
                case Command.Exit:
                case Command.SwitchUser:
                case Command.GetHelp:
                case Command.ShowMe:
                case Command.ChangePasswd:
                case Command.ChangeAge:
                case Command.ChangeAddress:
                case Command.ChangeSex:
                case Command.ViewMyHistory:
                case Command.ViewCurriculums:
                case Command.ViewHeadTeacher:
                    result = RunForUser(teacher, cmd);
                    break;
                case Command.StudentsPreview:
                    teacher.DisplayStudentList();
                    break;
                case Command.TeachersPreview:
                    teacher.DisplayTeacherList();
                    break;
                case Command.AllScore:
                    teacher.DisplayAllScoreOfStudent();
                    break;
                case Command.AllScoreAndRank:
                    teacher.DisplayAllScoreOfStudent(State.on, State.on);
                    break;
                case Command.ChangeScore:
                    teacher.CallChangeScore();
                    break;
                case Command.HighThan:
                    teacher.DisplayScoreHighThan();
                    break;
                case Command.ReleaseAMsg:
                    teacher.ReleaseNewMessage();
                    break;
                default:
                    UI.DisplayTheInformationOfErrorCode(ErrorCode.NotACommand, input);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 交互(班主任用户)
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>用户对象</returns>
        public static User RunForHeadTeacher(HeadTeacher headTeacher)
        {
            ForegroundColor = ConsoleColor.Yellow;
            String input = ReadLine();
            ForegroundColor = ConsoleColor.Blue;
            if (input.Equals(String.Empty))
            {
                return null;
            }

            if (GetCmd(input, out Command cmd) == null)
            {
                return null;
            }
            User result = null;
            switch (cmd)
            {
                case Command.Exit:
                case Command.SwitchUser:
                case Command.GetHelp:
                case Command.ShowMe:
                case Command.ChangePasswd:
                case Command.ChangeAge:
                case Command.ChangeAddress:
                case Command.ChangeSex:
                case Command.ViewMyHistory:
                case Command.ViewCurriculums:
                //case Command.ViewHeadTeacher:
                    result = RunForUser(headTeacher, cmd);
                    break;
                case Command.ChangeName:
                    headTeacher.ChangeMyName();
                    break;
                case Command.ChangeNameOfThisUser:
                    headTeacher.ChangeNameOfOtherUser();
                    break;
                case Command.AddStudent:
                    headTeacher.CallAddStudent();
                    break;
                case Command.AddTeacher:
                    headTeacher.CallAddTeacher();
                    break;
                case Command.Remove:
                    headTeacher.RemoveAccount();
                    break;
                case Command.StudentsPreview:
                    headTeacher.DisplayStudentList();
                    break;
                case Command.TeachersPreview:
                    headTeacher.DisplayTeacherList();
                    break;
                case Command.AllScore:
                    headTeacher.DisplayAllScoreOfStudent();
                    break;
                case Command.AllScoreAndRank:
                    headTeacher.DisplayAllScoreOfStudent(State.on, State.on);
                    break;
                case Command.HighThan:
                    headTeacher.DisplayScoreHighThan();
                    break;
                case Command.ReleaseNewCurriculum:
                    headTeacher.AddNewCurriculum();
                    break;
                case Command.ReleaseAMsg:
                    headTeacher.ReleaseNewMessage();
                    break;
                default:
                    UI.DisplayTheInformationOfErrorCode(ErrorCode.NotACommand, input);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 检查账户是否存在(旧版本)
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns>true: 不存在， false: 已存在</returns>
        [Obsolete("此方法依赖User.cs文件中的FindAccount <T> 类, 推荐使用基于本地函数实现的新版本")]
        public static User CheckAccountAvailabilityOldVersionAndNeedOtherClass(String account) //依赖于User.cs文件中的FindAccount <T> 类
        {
            int index1 = InformationLibrary.StudentLibrary.FindIndex(new FindAccount<Student>(account).FindAccountPredicate);
            if (index1 != -1)
            {
                return InformationLibrary.StudentLibrary[index1];
            }
            int index2 = InformationLibrary.TeacherLibrary.FindIndex(new FindAccount<Teacher>(account).FindAccountPredicate);
            if (index2 != -1)
            {
                return InformationLibrary.StudentLibrary[index2];
            }
            return account == InformationLibrary.HeadTeacherUser.Account ? InformationLibrary.HeadTeacherUser : null;
        }
        /// <summary>
        /// 检查账户是否存在(旧版本)
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns>true: 不存在， false: 已存在</returns>
        [Obsolete("此方法使用Lambda表达式, 但同一表达式使用多次")]
        public static User CheckAccountAvailabilityOldVersionLambda(String account)
        {
            int index1 = InformationLibrary.StudentLibrary.FindIndex(u => u.Account == account);
            if (index1 != -1)
            {
                return InformationLibrary.StudentLibrary[index1];
            }
            int index2 = InformationLibrary.TeacherLibrary.FindIndex(u => u.Account.Equals(account));
            if (index2 != -1)
            {
                return InformationLibrary.StudentLibrary[index2];
            }
            return account == InformationLibrary.HeadTeacherUser.Account ? InformationLibrary.HeadTeacherUser : null;
        }
        /// <summary>
        /// 检查账户是否存在(旧版本)
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns>true: 不存在， false: 已存在</returns>
        [Obsolete("使用传统for循环的旧版本")]
        public static User CheckAccountAvailabilityOldVersionNormal(String account)
        {
            foreach (var index in InformationLibrary.StudentLibrary)
            {
                if (account.Equals(index.Account))
                {
                    return index;
                }
            }
            foreach (var index in InformationLibrary.TeacherLibrary)
            {
                if (account.Equals(index.Account))
                {
                    return index;
                }
            }
            return account == InformationLibrary.HeadTeacherUser.Account ? InformationLibrary.HeadTeacherUser : null;
        }
        /// <summary>
        /// 检查账户是否存在
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns>true: 不存在， false: 已存在</returns>
        public static User CheckAccountAvailability(String account)
        {
            Student student = InformationLibrary.StudentLibrary.Find(IsEquals);
            if (student != default(Student))
            {
                return student;
            }
            Instructor instructor = InformationLibrary.TeacherLibrary.Find(IsEquals);
            if (instructor != default(Instructor))
            {
                return instructor;
            }

            return (IsEquals(InformationLibrary.HeadTeacherUser)) ? InformationLibrary.HeadTeacherUser : null;

            //Local Function For The Find() Method 
            bool IsEquals(User u) => u.Account.Equals(account);
        }
        /// <summary>
        /// 检查登录信息
        /// </summary>
        /// <param name="LoginInformation">登录信息</param>
        /// <returns>用户对象</returns>
        public static User IdentityCheck(Tuple<String, String> LoginInformation)
        {
            User result = CheckAccountAvailability(LoginInformation.Item1);
            if ((result != null) && LoginInformation.Item2.Equals(result.Passwd))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// 检查能否发布新课表
        /// </summary>
        /// <returns></returns>
        public static bool CanAddNewCurriculum()
        {
            UpdateCurriculum();
            return InformationLibrary._curriculums[1] == null;
        }
        /// <summary>
        /// 检查已失效的课表并删除(没有使用" ?. "条件运算符的旧方法)
        /// </summary>
        [Obsolete("此方法没有使用 ?. 运算符,  代码过于冗长, 请使用此方法的新版本")]
        public static void UpdateCurriculumOldVersion()
        {
            if (InformationLibrary._curriculums[0] != null)
            {
                if (InformationLibrary._curriculums[1] != null)
                {
                    if (DateTime.Now > InformationLibrary._curriculums[0].OverTime)
                    {
                        if (DateTime.Now > InformationLibrary._curriculums[1].OverTime)
                        {
                            InformationLibrary._curriculums[0] = InformationLibrary._curriculums[1] = null;
                        }
                        InformationLibrary._curriculums[0] = InformationLibrary._curriculums[1];
                        InformationLibrary._curriculums[1] = null;
                    }
                }
                else
                {
                    if (DateTime.Now > InformationLibrary._curriculums[0].OverTime)
                    {
                        InformationLibrary._curriculums[0] = null;
                    }
                }
            }
        }
        /// <summary>
        /// 检查已失效的课表并删除(使用" ?. "条件运算符的新方法)
        /// </summary>
        public static void UpdateCurriculum()
        {
            //<!>DateTime结构体变量于null比较始终为false</!>
            if (DateTime.Now > InformationLibrary._curriculums[0]?.OverTime)
            {
                if (DateTime.Now > InformationLibrary._curriculums[1]?.OverTime)
                {
                    InformationLibrary._curriculums[0] = InformationLibrary._curriculums[1] = null;
                }
                else
                {
                    InformationLibrary._curriculums[0] = InformationLibrary._curriculums[1];
                    InformationLibrary._curriculums[1] = null;
                }
            }
        }
    }
}
