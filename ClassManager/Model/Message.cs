using System;
using static System.Console;

namespace Sebastien.ClassManager.Core
{
    public class Message : EventArgs
    {
        /// <summary>
        /// 消息来源
        /// </summary>
        public string Source { get; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; } = DateTime.Now;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time">消息的时间</param>
        /// <param name="msg">消息的内容</param>
        public Message(string source, string msg)
        {
            this.Source = source;
            this.Content = msg;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="msg">内容</param>
        public Message(string source, DateTime time, string msg) : this(source, msg) => Time = time;
        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{this.Time}]<{this.Source}>: {this.Content}";
    }
}
