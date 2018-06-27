using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using System.Collections.Generic;
using System.Threading;
using Sebastien.ClassManager.Enums;
// ReSharper disable All

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 班主任用户类
    /// </summary>
    public sealed class HeadTeacher : Teacher, ITeacher
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object SyncRoot = new object();
        /// <summary>
        /// 此类型唯一的实例对象
        /// </summary>
        private static HeadTeacher _ht; //此类型唯一的实例对象
        /// <inheritdoc />
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private HeadTeacher() { }

        /// <inheritdoc />
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="years"></param>
        /// <param name="userType">用户类型</param>
        /// <param name="name"></param>
        private HeadTeacher(string account, string passwd, string name, int years, Identity userType = Identity.HeadTeacher) 
            : base(account, passwd, name, years, userType)
        {

        }

        /// <summary>
        /// 单例模式( 线程安全 )
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="name"></param>
        /// <param name="years"></param>
        /// <returns>如果HeadTeacher不为null，返回HeadTeacher对象， 否则创建一个新的HeadTeacher对象作为返回值</returns>
        public static HeadTeacher GetHeadTeacher(string account, string passwd, string name, int years)
        {
            #region 一般实现
            //if (_ht == null)
            //{
            //    lock (SyncRoot)
            //    {
            //        if (_ht == null)
            //        {
            //            _ht = new HeadTeacher(account, passwd, name, years);
            //        }
            //    }
            //}
            //return _ht;
            #endregion

            #region 使用NULL传播运算符
            //if (_ht == null)
            //{
            //    lock (SyncRoot)
            //    {
            //        return _ht ?? new HeadTeacher(account, passwd, name, years);
            //    }
            //}
            //return _ht;
            #endregion

            #region 使用Interlocked类
            if (_ht != null)
            {
                return _ht;
            }
            Interlocked.CompareExchange(ref _ht, new HeadTeacher(account, passwd, name, years), null);
            return _ht;

            #endregion
        }

        /// <inheritdoc />
        /// <summary>
        /// 学生成绩列表
        /// </summary>
        /// <param name="isDisplayRank">是否显示行号</param>
        /// <param name="isSort">是否排序</param>
        public void ViewScoreOfAllStudent(State isDisplayRank = State.Off, State isSort = State.Off)
        {
            if (isSort == State.On)
            {
                InformationLibrary.StudentLibrary.Sort();
            }
            uint row = 1;
            foreach (var index in InformationLibrary.StudentLibrary)
            { 
                if (isDisplayRank == State.On)
                {
                    Write($"{row++, -10} ");
                }
                Write($"{index.Name, -10}");
                index.ShowMyScore();
            }
        }

        /// <inheritdoc />
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
                WriteLine($"{"Name",-10}TotalScore");
                foreach (var index in result)
                {
                    WriteLine($"{index.Name,-10}{index.GetTotalScore(),-10}");
                }
            }
            catch (ArgumentException)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.ArgumentOutOfRange);
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
            var rd = new Random();
            return NewCurriculum();

            Curriculum NewCurriculum()
            {
                var temp = new Curriculum();
                for (var line = 0; line < temp.Week; ++line)
                {
                    for (var row = 0; row < temp.Classes; ++row)
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
        public async void AddNewCurriculum()
        {
            Curriculum cc = null;
            try
            {
                cc = CreateCurriculum();
            }catch(NullReferenceException)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.CantAdd);
                return;
            }

            await Task.Run(() =>
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
               Ui.DisplayTheInformationOfSuccessfully();
           });
        }
        /// <inheritdoc />
        /// <summary>
        /// 重写查看班主任信息方法
        /// </summary>
        public override void ViewTheInformationOfTheHeadteacher() => ViewPersonalInformation();
    }
}
