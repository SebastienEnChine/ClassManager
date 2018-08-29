using System;
using static System.Console;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 单节课 内容结构
    /// </summary>
    public class CurriculumContant
    {
        /// <summary>
        /// 讲课人
        /// </summary>
        public string Person { get; }
        /// <summary>
        /// 课程主题
        /// </summary>
        public string Theme { get; }
        /// <summary>
        /// 在课表上的颜色
        /// </summary>
        public ConsoleColor CurriculumColor { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="teacher">讲课老师</param>
        /// <param name="contant">课程主要焦点</param>
        /// <param name="color">颜色</param>
        public CurriculumContant(string teacher, string contant, ConsoleColor color)
        {
            this.Person = teacher;
            this.Theme = contant;
            this.CurriculumColor = color;
        }
    }

    /// <summary>
    /// 课表类
    /// </summary>
    public class Curriculum
    {
        /// <summary>
        /// 课表天数
        /// </summary>
        public int Week { get; } = 7;
        /// <summary>
        /// 课表节数
        /// </summary>
        public int Classes { get; } = 3;
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime OverTime { get; }
        private readonly CurriculumContant[,] _lessons;
        /// <summary>
        /// 课表内容
        /// </summary>
        /// <param name="days">节索引</param>
        /// <param name="classes">天数索引</param>
        /// <returns>课程内容</returns>
        /// <exception cref="ArgumentOutOfRangeException">days小于0或大于6时引发, classes小于0或大于2时引发</exception>
        public CurriculumContant this[int days, int classes]
        {
            get
            {
                if (days < 0 || days > 6 || classes < 0 || classes > 2)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                return this._lessons[days, classes];
            }
            set
            {
                if (days < 0 || days > 6 || classes < 0 || classes > 2)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                this._lessons[days, classes] = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Curriculum()
        {
            DateTime temp = DateTime.Now.AddDays(DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 0 : 7 - (int)DateTime.Now.DayOfWeek);
            if (UserRepository._curriculums[0] != null)
            {
                temp = temp.AddDays(7);
            }
            this.OverTime = new DateTime(temp.Year, temp.Month, temp.Day, 23, 59, 59);
            this._lessons = new CurriculumContant[this.Week, this.Classes];
        }

        /// <summary>
        /// 显示整张课表
        /// </summary>
        public void Draw()
        {
            Ui.PrintColorMsg($"{"",-6}{"Mon",-15}{"Tue",-15}{"Wen",-15}{"Thu",-15}{"Fri",-15}{"Sat",-15}{"Sun",-15}", ConsoleColor.White, ConsoleColor.Black);
            WriteLine(".");
            for (int row = 0; row < this.Classes; ++row)
            {
                Ui.PrintColorMsg(row == 0 ? "上午" : (row == 1 ? "下午" : "晚上"), ConsoleColor.White, ConsoleColor.Black);
                Ui.PrintColorMsg($"{" ",-2}", ConsoleColor.White, ConsoleColor.Black);
                for (int line = 0; line < this.Week; ++line)
                {
                    PrintMsg(this._lessons[line, row].Person, this._lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
                Ui.PrintColorMsg($"{"",-6}", ConsoleColor.White, ConsoleColor.Black);
                for (int line = 0; line < this.Week; ++line)
                {
                    PrintMsg(this._lessons[line, row].Theme, this._lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
                Ui.PrintColorMsg($"{"",-6}", ConsoleColor.White, ConsoleColor.Black);
                for (int line = 0; line < this.Week; ++line)
                {
                    PrintMsg(this._lessons[line, row].CurriculumColor.ToString(), this._lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
            }
        }
        /// <summary>
        /// 显示课表内容
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cc"></param>
        private void PrintMsg(string msg, ConsoleColor cc)
        {
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = cc;
            Write($"{msg,-15}");
            Ui.DefaultColor();
        }
    }
}
