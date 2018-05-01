using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using static System.Console;

namespace MySelector
{
    /// <summary>
    /// 选择器异常类(当选项和选项显示信息长度不匹配时引发)
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
        /// 选项
        /// </summary>
        public ImmutableList<T> Select { get; set; }
        /// <summary>
        /// 选项显示信息
        /// </summary>
        public ImmutableList<String> TheInfomationOfSelect { get; set; }
        /// <summary>
        /// 焦点
        /// </summary>
        private int _mainIndex;
        /// <summary>
        /// 背景颜色
        /// </summary>
        private readonly ConsoleColor _background;
        /// <summary>
        /// 前景颜色
        /// </summary>
        private readonly ConsoleColor _foreground;
        /// <summary>
        /// 焦点前景颜色
        /// </summary>
        private readonly ConsoleColor _mainForeground;
        /// <summary>
        /// 焦点背景颜色
        /// </summary>
        private readonly ConsoleColor _mainBackground;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">选项显示信息</param>
        /// <param name="select">选项</param>
        ///<exception cref="System.NullReferenceException">info或select为null时引发</exception>
        ///<exception cref="SelectorException">info和select长度不匹配时引发</exception>
        ///<localize><zh-CHS>中文</zh-CHS><en>English</en></localize>
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
            _mainIndex = 0;

            _mainBackground = ConsoleColor.White;
            _mainForeground = ConsoleColor.Black;
            _background = ConsoleColor.Black;
            _foreground = ConsoleColor.White;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainBg">焦点背景颜色</param>
        /// <param name="mainFg">焦点前景颜色</param>
        /// <param name="info">选项显示信息</param>
        /// <param name="select">选项</param>
        ///<exception cref="System.NullReferenceException">info或select为null时引发</exception>
        ///<exception cref="SelectorException">info和select长度不匹配时引发</exception>
        ///<localize><zh-CHS>中文</zh-CHS><en>English</en></localize>
        public Selector(ConsoleColor mainBg, ConsoleColor mainFg, List<String> info, params T[] select)
            : this(info, select)
        {
            _mainBackground = mainBg;
            _mainForeground = mainFg;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bg">背景颜色</param>
        /// <param name="fg">前景颜色</param>
        /// <param name="mainBg">焦点背景颜色</param>
        /// <param name="mainFg">焦点前景颜色</param>
        /// <param name="info">选项显示信息</param>
        /// <param name="select">选项</param>
        ///<exception cref="System.NullReferenceException">info或select为null时引发</exception>
        ///<exception cref="SelectorException">info和select长度不匹配时引发</exception>
        public Selector(ConsoleColor bg, ConsoleColor fg, ConsoleColor mainBg, ConsoleColor mainFg, List<String> info, params T[] select)
            : this(mainBg, mainFg, info, select)
        {
            _background = bg;
            _foreground = fg;
        }

        /// <summary>
        /// 显示选项信息并获取用户的选择
        /// </summary>
        /// <returns>用户的选择结果</returns>
        public T GetSubject()
        {
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
                BackgroundColor = _background;
                ForegroundColor = _foreground;
            }
            //焦点颜色设置
            void SetSelectColor()
            {
                BackgroundColor = _mainBackground;
                ForegroundColor = _mainForeground;
            }
        }
    }
}
