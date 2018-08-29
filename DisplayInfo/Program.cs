using System;
using static System.Console;
using System.Threading.Tasks;

namespace DisplayInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            SetWindowSize(40, 5);
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Green;
            Beep();
            foreach(string index in args)
            {
                Write($"{index} ");
            }
            WriteLine();
            ForegroundColor = ConsoleColor.White;
            WriteLine("-----------------------------------------");
            WriteLine("按任意键关闭窗口");
            ReadKey(true);
        }
    }
}
