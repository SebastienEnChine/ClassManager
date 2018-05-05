using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 班主任用户类
    /// </summary>
    public sealed class HeadTeacher : Teacher, ITeacher
    {
        /// <summary>
        /// 此类型唯一的实例对象
        /// </summary>
        private static HeadTeacher _ht = null; //此类型唯一的实例对象
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private HeadTeacher() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        private HeadTeacher(String account, String passwd, String name, int years, Identity userType = Identity.HeadTeacher) 
            : base(account, passwd, name, years, userType)
        {

        }
        
        /// <summary>
        /// 限制HeadTeacher对象的唯一性
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <returns>如果HeadTeacher不为null，返回HeadTeacher对象， 否则创建一个新的HeadTeacher对象作为返回值</returns>
        public static HeadTeacher GetHeadTeacher(String account, String passwd, String name, int years) 
            => _ht ?? ( _ht = new HeadTeacher(account, passwd, name, years));
        /// <summary>
        /// 学生成绩列表
        /// </summary>
        /// <param info>所有学生信息</param>
        /// <param name="IsDisplayRank">是否显示行号</param>
        /// <param name="IsSort">是否排序</param>
        public void ViewScoreOfAllStudent(State IsDisplayRank = State.off, State IsSort = State.off)
        {
            if (IsSort == State.on)
            {
                InformationLibrary.StudentLibrary.Sort();
            }
            uint row = 1;
            foreach (var index in InformationLibrary.StudentLibrary)
            { 
                if (IsDisplayRank == State.on)
                {
                    Write($"{row++, -10} ");
                }
                Write($"{index.Name, -10}");
                index.ShowMyScore();
            }
        }
        /// <summary>
        /// 显示此分数以上的所有学生
        /// </summary>
        /// <param name="score">指定分数</param>
        public void GetStuHighThan(int score)
        {
            try
            {
                if (score < 0 || score > Enum.GetValues(typeof(Subject)).Length * 100)
                {
                    throw new ArgumentException();
                }
                IEnumerable<Student> result = from s in InformationLibrary.StudentLibrary
                                              where s.GetTotalScore() >= score
                                              select s;
                WriteLine($"{"Name",-10}{"TotalScore"}");
                foreach (Student index in result)
                {
                    WriteLine($"{index.Name,-10}{index.GetTotalScore(),-10}");
                }
            }
            catch (ArgumentException)
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
            }
        }
        /// <summary>
        /// 创建临时课表 (由于测试需要  暂时自动随机填充课表)
        /// </summary>
        public static Curriculum CreateCurriculum()
        {
            if (!Client.CanAddNewCurriculum())
            {
                throw new NullReferenceException();
            }
            Random rd = new Random();
            return NewCurriculum();

            Curriculum NewCurriculum()
            {
                Curriculum temp = new Curriculum();
                for (int line = 0; line < temp.Week; ++line)
                {
                    for (int row = 0; row < temp.Classes; ++row)
                    {
                        temp[line, row] = new CurriculumContant(line.ToString(), row.ToString(), (ConsoleColor)rd.Next(14) + 1);
                    }
                }
                return temp;
            }
        }
        /// <summary>
        /// 发布新课表
        /// </summary>
        /// <param name="cc">新课表</param>
        public void AddNewCurriculum()
        {
            Curriculum cc = null;
            try
            {
                cc = CreateCurriculum();
            }catch(NullReferenceException)
            {
                UI.DisplayTheInformationOfErrorCode(ErrorCode.CantAdd);
                return;
            }

            Task.Run(() =>
            {
               if (InformationLibrary._curriculums[0] == null)
               {
                   InformationLibrary._curriculums[0] = cc;
               }
               else
               {
                   InformationLibrary._curriculums[1] = cc;
               }
               ReleaseNewMsg(new Message("班主任", "发布了新课表, 快去看看吧~"));
               InformationLibrary.HeadTeacherUser.AddHistory(new Message("你", "发布了新课表"));
               UI.DisplayTheInformationOfSuccessfully();
           });
        }
        /// <summary>
        /// 重写查看班主任信息方法
        /// </summary>
        public override void ViewTheInformationOfTheHeadteacher() => ViewPersonalInformation();
    }
}
