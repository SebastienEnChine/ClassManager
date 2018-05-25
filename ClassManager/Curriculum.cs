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
        public String Person { get; }
        /// <summary>
        /// 课程主题
        /// </summary>
        public String Theme { get; }
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
        public CurriculumContant(String teacher, String contant, ConsoleColor color)
        {
            Person = teacher;
            Theme = contant;
            CurriculumColor = color;
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
        public Int32 Week { get; } = 7;
        /// <summary>
        /// 课表节数
        /// </summary>
        public Int32 Classes { get; } = 3;
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime OverTime { get; }
        /// <summary>
        /// 课表内容
        /// </summary> 
        private CurriculumContant[,] _lessons;
        /// <summary>
        /// 课表内容
        /// </summary>
        /// <param name="days">节索引</param>
        /// <param name="classes">天数索引</param>
        /// <returns>课程内容</returns>
        /// <exception cref="ArgumentOutOfRangeException">days小于0或大于6时引发, classes小于0或大于2时引发</exception>
        public CurriculumContant this[Int32 days, Int32 classes]
        {
            get
            {
                if (days < 0 || days > 6 || classes < 0 || classes > 2)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                return _lessons[days, classes];
            }
            set
            {
                if (days < 0 || days > 6 || classes < 0 || classes > 2)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                _lessons[days, classes] = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Curriculum()
        {
            DateTime temp = DateTime.Now.AddDays(DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 0 : 7 - (Int32)DateTime.Now.DayOfWeek);
            if (InformationLibrary._curriculums[0] != null)
            {
                temp.AddDays(7);
            }
            OverTime = new DateTime(temp.Year, temp.Month, temp.Day, 23, 59, 59);
            _lessons = new CurriculumContant[Week, Classes];
        }

        /// <summary>
        /// 显示整张课表
        /// </summary>
        public void Draw()
        {
            UI.PrintColorMsg($"{"", -6}{"Mon",-15}{"Tue",-15}{"Wen",-15}{"Thu",-15}{"Fri",-15}{"Sat",-15}{"Sun",-15}", ConsoleColor.White, ConsoleColor.Black);
            WriteLine(".");
            for (Int32 row = 0; row < Classes; ++row)
            {
                UI.PrintColorMsg(row == 0 ? "上午" : (row == 1 ? "下午" : "晚上"), ConsoleColor.White, ConsoleColor.Black);
                UI.PrintColorMsg($"{" ",-2}", ConsoleColor.White, ConsoleColor.Black);
                for (Int32 line = 0; line < Week; ++line)
                {
                    PrintMsg(_lessons[line, row].Person, _lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
                UI.PrintColorMsg($"{"",-6}", ConsoleColor.White, ConsoleColor.Black);
                for (Int32 line = 0; line < Week; ++line)
                {
                    PrintMsg(_lessons[line, row].Theme, _lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
                UI.PrintColorMsg($"{"",-6}", ConsoleColor.White, ConsoleColor.Black);
                for (Int32 line = 0; line < Week; ++line)
                {
                    PrintMsg(_lessons[line, row].CurriculumColor.ToString(), _lessons[line, row].CurriculumColor);
                }
                WriteLine(".");
            }
        }
        /// <summary>
        /// 显示课表内容
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cc"></param>
        private void PrintMsg(String msg, ConsoleColor cc)
        {
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = cc;
            Write($"{msg,-15}");
            UI.DefaultColor();
        }
    }
}
