using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using static System.Console;

namespace MySelector
{
    /// <summary>
    /// 选择器异常类
    /// </summary>
    public class SelectorException : Exception
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SelectorException()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        public SelectorException(String message)
            : base(message)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="innerException">源异常</param>
        public SelectorException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    /// <summary>
    /// 选择器类
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    public sealed class Selector<T>
    {
        /// <summary>
        /// 背景颜色(未选中项)
        /// </summary>
        public ConsoleColor UnselectedBackground { get; set; } = ConsoleColor.Black;
        /// <summary>
        /// 前景颜色(未选中项)
        /// </summary>
        public ConsoleColor UnselectedForeground { get; set; } = ConsoleColor.White;
        /// <summary>
        /// 背景颜色(选中项)
        /// </summary>
        public ConsoleColor SelectedBackground { get; set; } = ConsoleColor.White;
        /// <summary>
        /// 前景颜色(选中项)
        /// </summary>
        public ConsoleColor SelectedForeground { get; set; } = ConsoleColor.Black;
        /// <summary>
        /// 选项
        /// </summary>
        public ImmutableList<T> Select { get; set; }
        /// <summary>
        /// 选项显示信息
        /// </summary>
        public ImmutableList<String> TheInfomationOfSelect { get; set; }
        /// <summary>
        /// 选中项索引
        /// </summary>
        private int _mainIndex = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">选项显示信息</param>
        /// <param name="select">选项</param>
        ///<exception cref="System.NullReferenceException">info或select为null时引发</exception>
        ///<exception cref="SelectorException">info和select长度不匹配时引发</exception>
        public Selector(List<String> info, params T[] select)
        {
            if (info == null || select == null)
            {
                throw new NullReferenceException();
            }
            if (info.Count != select.Length)
            {
                throw new SelectorException();
            }

            Select = select.ToImmutableList();
            TheInfomationOfSelect = info.ToImmutableList();
        }

        /// <summary>
        /// 显示选项信息并获取用户的选择
        /// </summary>
        /// <returns>用户的选择结果</returns>
        public T GetSubject()
        {
            //保存控制台原有颜色
            ConsoleColor oldbg = BackgroundColor;
            ConsoleColor oldfg = ForegroundColor;

            do
            {
                DisplayTheInfomationOfSelect();
                ConsoleKeyInfo info = ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_mainIndex > 0)
                        {
                            --_mainIndex;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_mainIndex < Select.Count - 1)
                        {
                            ++_mainIndex;
                        }
                        break;
                    case ConsoleKey.Enter:
                        //恢复控制台原有颜色
                        BackgroundColor = oldbg;
                        ForegroundColor = oldfg;

                        return Select[_mainIndex];
                    default:
                        break;
                }
                SetCursorPosition(0, CursorTop - Select.Count);
            } while (true);

        }
        /// <summary>
        /// 显示选项信息
        /// </summary>
        private void DisplayTheInfomationOfSelect()
        {
            for (var index = 0; index < TheInfomationOfSelect.Count; ++index)
            {
                if (_mainIndex == index)
                {
                    SetSelectColor();
                    Write($"{" ==>",-10}{TheInfomationOfSelect[index]}");
                    SetColor();
                    WriteLine(".");
                }
                else
                {
                    WriteLine($"{" ",-10}{TheInfomationOfSelect[index]}");
                }
            }

            //默认颜色设置
            void SetColor()
            {
                BackgroundColor = UnselectedBackground;
                ForegroundColor = UnselectedForeground;
            }
            //焦点颜色设置
            void SetSelectColor()
            {
                BackgroundColor = SelectedBackground;
                ForegroundColor = SelectedForeground;
            }
        }
    }
}
