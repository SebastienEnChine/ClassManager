using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static System.Console;
using System.Collections.Generic;
using Sebastien.ClassManager.Enums;
using System.Windows;
using System.ComponentModel;

namespace Sebastien.ClassManager.Core
{
    /// <inheritdoc />
    /// <summary>
    /// 学生用户类
    /// </summary>
    public sealed class Student  
        : UserCore, IComparable, IComparable<Student>, IFormattable, IEnumerable, IWeakEventListener
    {
        /// <summary>
        /// 成绩
        /// </summary>
        private readonly double?[] _score = new double?[Subject.C.GetLengthOfSubject()];
        /// <summary>
        /// 成绩索引器
        /// </summary>
        /// <param name="index">科目索引</param>
        /// <returns>成绩</returns> 
        public double? this[Subject index]
        {
            get
            {
                if ((int)index > this._score.Length || index < 0)
                {
                    throw new IndexOutOfRangeException("索引越界");
                }
                return this._score[(int)index];
            }
            set
            {
                if (value > 100 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                this._score[(int)index] = value;
            }
        }
        /// <summary>
        /// 是否有新消息
        /// </summary>
        public bool HasNewMsg => this.NewMsg.Count > 0;
        /// <summary>
        /// 新消息
        /// </summary>
        private Queue<Message> NewMsg { get; } = new Queue<Message>();
        /// <summary>
        /// 消息内容
        /// </summary>
        public List<Message> AllNews { get; } = new List<Message>();
        /// <summary>
        /// 订阅状态
        /// </summary>
        private bool IsSubscription { get; set; }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Student()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="stu"></param>
        public Student(Student stu) : base(stu)
        {
            //TODO:
            for (int index = 0; index < this._score.Length; ++index)
            {
                this._score[index] = stu[(Subject)index];
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        public Student(string account, string passwd, string name)
            : base(account, passwd, name, Identity.Student)
        {
        }
        /// <inheritdoc />
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
        public Student(string account, string passwd, string name, TheSex sex, int age, string address, Identity userType = Identity.Student)
            : base(account, passwd, name, sex, age, address, userType)
        {

        }
        /// <summary>
        /// 订阅
        /// </summary>
        public async void SubscriptionToHeadTeacher(Teacher teacher)
        {
            if (this.IsSubscription)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.DuplicateSubscriptions);
            }
            else
            {
                await Task.Run(() =>
                {
                    WeakEventManager<Teacher, Message>.AddHandler(teacher, nameof(teacher.NewMsg), this.ReceiveNewCurriculum);
                    this.IsSubscription = true;
                });
                Ui.DisplayTheInformationOfSuccessfully("(订阅成功)");
            }
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        public async void UnsubscribeToHeadTeacher(Teacher teacher)
        {
            if (!this.IsSubscription)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.NotSubscribedYet);
            }
            else
            {
                await Task.Run(() => 
                        WeakEventManager<Teacher, Message>
                        .RemoveHandler(teacher, nameof(teacher.NewMsg), this.ReceiveNewCurriculum)
                    );
                Ui.DisplayTheInformationOfSuccessfully("取消订阅成功");
                this.IsSubscription = false;
            }
        }
        /// <summary>
        /// 实现IComparable接口
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(object other) => CompareTo(other as Student);
        /// <summary>
        /// 实现IComparable<Student>接口
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Student other) => GetTotalScore() < other.GetTotalScore() ? 1 : -1;
        /// <summary>
        /// 获取总成绩
        /// </summary>
        /// <returns>总成绩</returns>
        public double GetTotalScore()
        {
            double sum = default;
            foreach (double? index in this._score)
            {
                sum += index ?? 0;
            }
            return sum;
        }
        /// <summary>
        /// 显示各科目成绩和总成绩
        /// </summary>
        public void ShowMyScore()
        {
            foreach (double? index in this)
            {
                Write($"{(index == null ? "Not Set" : index.ToString()),-10}");
            }
            WriteLine($"{GetTotalScore(),-10}");
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        private void ReceiveNewCurriculum(object sender, Message msg) => this.NewMsg.Enqueue(msg);

        /// <summary>
        /// 查看新消息
        /// </summary>
        public void ViewNews()
        {
            if (this.HasNewMsg)
            {
                for (int index = 0; index < this.NewMsg.Count; ++index)
                {
                    Message msg = this.NewMsg.Dequeue();
                    Ui.PrintColorMsg(msg.ToString(), ConsoleColor.Black, ConsoleColor.DarkMagenta);
                    this.NewMsg.TrimExcess();
                    this.AllNews.Add(msg);
                    WriteLine();
                }
            }
            else
            {
                WriteLine("暂无消息");
            }
        }
        /// <summary>
        /// 查看消息记录
        /// </summary>
        public void ViewTotalNews()
        {
            foreach (Message index in this.AllNews)
            {
                WriteLine(index);
            }
        }
        /// <summary>
        /// 实现IFormattable接口
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var score = new StringBuilder();
            foreach (double? index in this._score)
            {
                score.Append($"{index,-10}");
            }
            switch (format)
            {
                case null:
                case "A":
                    return score + ToString();
                case "I":
                    return ToString();
                case "S":
                    return score.ToString();
                case "P":
                    return $"{this.Name,-10} {this.Sex,-10} {this.Age,-10}";
                default:
                    throw new FormatException("Invalid format");
            }
        }
        public string ToString(string format) => ToString(format, null);
        /// <inheritdoc />
        /// <summary>
        /// 实现IEnumerable接口
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => new StudentIEnumerator(this._score);
        /// <inheritdoc />
        /// <summary>
        /// 弱事件处理程序
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            ReceiveNewCurriculum(sender, e as Message);
            return true;
        }

        /// <summary>
        /// 学生类枚举器
        /// </summary>
        private class StudentIEnumerator : IEnumerator
        {
            /// <summary>
            /// 枚举目标
            /// </summary>
            private double?[] _iscore { get; set; }
            /// <summary>
            /// 当前索引
            /// </summary>
            private int _position;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="score"></param>
            public StudentIEnumerator(double?[] score)
            {
                this._iscore = score;
                this._position = -1;
            }
            /// <summary>
            /// 当前元素
            /// </summary>
            public object Current => this._iscore[this._position];
            /// <inheritdoc />
            /// <summary>
            /// 移动到下一元素
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() => (++this._position < this._iscore.Length);
            /// <inheritdoc />
            /// <summary>
            /// 重置当前元素
            /// </summary>
            public void Reset() => throw new NotImplementedException();
        }

        /// <summary>
        /// 学生类比较器
        /// </summary>
        public class StudentCompare : IComparer<Student>
        {
            /// <summary>
                               /// 排序方式
                               /// </summary>
            private readonly Subject _sortWay;
            public StudentCompare(Subject sortWay) => _sortWay = sortWay;
            /// <inheritdoc />
            /// <summary>
            /// 实现Compare方法
            /// </summary>
            /// <param name="x">用于比较的第一个学生对象</param>
            /// <param name="y">用于比较的第二个学生对象</param>
            /// <returns>比较结果</returns>
            public int Compare(Student x, Student y) => x[this._sortWay] > y[this._sortWay] ? 1 : -1;
        }
    }
}
