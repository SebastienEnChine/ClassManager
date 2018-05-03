using System;
using System.Linq;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;

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
        public Instructor(String account, String passwd, String name, int years, Subject range, Identity userType = Identity.Instructor)
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
        public Instructor(String account, String passwd, String name, Subject range, TheSex sex, int age, String address, int years, Identity userType = Identity.Instructor)
            : base(account, passwd, name, sex, age, address, years, userType) => TeachingRange = range;

        /// <summary>
        /// 重写基类的ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{base.ToString()}\n所教科目: {TeachingRange}\n";
        /// <summary>
        /// 获取此学生某单科目的成绩
        /// </summary>
        /// <param name="stu">学生</param>
        /// <returns>成绩</returns>
        public double? GetScoreOfThisStudent(Student stu) => stu[TeachingRange];
        /// <summary>
        /// 显示学生成绩列表
        /// </summary>
        /// <param info>所有学生信息</param>
        /// <param name="subject">科目</param>
        /// <param name="IsSort">是否排序</param>
        /// <param name="IsDisplayRank">是否显示行号</param>
        public virtual void ViewScoreOfAllStudent(State IsSort = State.off, State IsDisplayRank = State.off)
        {
            if (IsSort == State.on)
            {
                //(依赖于Student.cs文件中的StudentCompare类)
                //InformationLibrary.StudentLibrary.Sort(new Student.StudentCompare(TeachingRange));
                InformationLibrary.StudentLibrary.Sort((x, y) => x[TeachingRange] < y[TeachingRange] ? 1 : -1);
            }
            uint row = 1;
            foreach (var index in InformationLibrary.StudentLibrary)
            {
                if (IsDisplayRank == State.on)
                {
                    Write($"{row++} ");
                }
                WriteLine($"{index.Name, -10} {index[TeachingRange]}");
            }
        }
        /// <summary>
        /// 显示此分数以上的所有学生
        /// </summary>
        /// <param name="score">指定分数</param>
        /// <exception cref="ArgumentOutOfRangeException">score大于100或者小于0时引发此异常</exception>
        public void GetStuHighThan(int score)
        {
            try
            {
                if(score < 0 || score > 100)
                {
                    throw new ArgumentOutOfRangeException();
                }
                IEnumerable<Student> result = from s in InformationLibrary.StudentLibrary
                                              where s[TeachingRange] >= score
                                              select s;
                WriteLine($"{"Name", -10}{"Score"}");
                foreach(Student index in result)
                {
                    WriteLine($"{index.Name, -10}{index[TeachingRange], -10}");
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
        }
    }
}
