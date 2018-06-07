using System;
using System.Linq;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;
using System.Threading.Tasks;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 授课教师类
    /// </summary>
    public class Instructor : Teacher, ITeacher
    {
        /// <summary>
        /// 所教科目
        /// </summary>
        public Subject TeachingRange { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Instructor()
        {
        }
        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="user">用户对象</param>
        public Instructor(Instructor teacher) : base(teacher)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        public Instructor(String account, String passwd, String name, Int32 years, Subject range, Identity userType = Identity.Instructor)
            : base(account, passwd, name, years, userType) => TeachingRange = range;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        /// <param name="name">姓名</param>
        /// <param name="sex">性别</param>
        /// <param name="age">年龄</param>
        /// <param name="address">地址</param>
        public Instructor(String account, String passwd, String name, Subject range, TheSex sex, Int32 age, String address, Int32 years, Identity userType = Identity.Instructor)
            : base(account, passwd, name, sex, age, address, years, userType) => TeachingRange = range;

        /// <summary>
        /// 重写基类的ToString()方法
        /// </summary>
        /// <returns></returns>
        public override String ToString() => $"{base.ToString()}\n所教科目: {TeachingRange}\n";
        /// <summary>
        /// 获取此学生某单科目的成绩
        /// </summary>
        /// <param name="stu">学生</param>
        /// <returns>成绩</returns>
        public Double? GetScoreOfThisStudent(Student stu) => stu[TeachingRange];
        /// <summary>
        /// 显示学生成绩列表
        /// </summary>
        /// <param info>所有学生信息</param>
        /// <param name="subject">科目</param>
        /// <param name="IsSort">是否排序</param>
        /// <param name="IsDisplayRank">是否显示行号</param>
        public virtual void ViewScoreOfAllStudent(State IsSort = State.Off, State IsDisplayRank = State.Off)
        {
            if (IsSort == State.On)
            {
                //(依赖于Student.cs文件中的StudentCompare类)
                //InformationLibrary.StudentLibrary.Sort(new Student.StudentCompare(TeachingRange));
                InformationLibrary.StudentLibrary.Sort((x, y) => x[TeachingRange] < y[TeachingRange] ? 1 : -1);
            }
            UInt32 row = 1;
            foreach (Student index in InformationLibrary.StudentLibrary)
            {
                if (row % 2 == 1)
                {
                    BackgroundColor = ConsoleColor.White;
                    ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    BackgroundColor = ConsoleColor.Black;
                    ForegroundColor = ConsoleColor.White;
                }
                if (IsDisplayRank == State.On)
                {
                    Write($"{row++} ");
                }
                WriteLine($"{index.Name, -10} {index[TeachingRange]}");
            }
            Ui.DefaultColor();
        }

        /// <summary>
        /// 显示此分数以上的所有学生
        /// </summary>
        /// <param name="score">指定分数</param>
        /// <exception cref="ArgumentOutOfRangeException">score大于100或者小于0时引发此异常</exception>
        public void GetStuHighThan(Int32 score)
        {
            try
            {
                if (score < 0 || score > 100)
                {
                    throw new ArgumentOutOfRangeException();
                }

                WriteLine($"{"Name",-10}Score");
                Parallel.ForEach(InformationLibrary.StudentLibrary.Where(stu => stu[TeachingRange] >= score), student =>
                {
                    WriteLine($"{student.Name,-10}{student[TeachingRange],-10}");
                });
                //IEnumerable<Student> result = from stu in InformationLibrary.StudentLibrary
                //                              where stu[TeachingRange] >= score
                //                              select stu;
            }
            catch(ArgumentOutOfRangeException)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
        }
    }
}
