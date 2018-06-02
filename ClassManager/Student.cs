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
        : User, IComparable, IComparable<Student>, IFormattable, IEnumerable, IWeakEventListener
    {
        /// <summary>
        /// 成绩
        /// </summary>
        private readonly Double?[] _score = new Double?[Subject.C.GetLengthOfSubject()];
        /// <summary>
        /// 成绩索引器
        /// </summary>
        /// <param name="index">科目索引</param>
        /// <returns>成绩</returns> 
        public Double? this[Subject index]
        {
            get
            {
                if ((Int32)index > _score.Length || index < 0)
                {
                    throw new IndexOutOfRangeException("索引越界");
                }
                return _score[(Int32)index];
            }
            set
            {
                if (value > 100 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("参数超出范围");
                }
                _score[(Int32)index] = value;

            }
        }
        /// <summary>
        /// 是否有新消息
        /// </summary>
        public Boolean HasNewMsg => NewMsg.Count > 0;
        /// <summary>
        /// 新消息
        /// </summary>
        private Queue<Message> NewMsg { get; } = new Queue<Message>();
        /// <summary>
        /// 消息内容
        /// </summary>
        private List<Message> AllNews { get; } = new List<Message>();
        /// <summary>
        /// 订阅状态
        /// </summary>
        private Boolean IsSubscription { get; set; }


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
            for (Int32 index = 0; index < _score.Length; ++index)
            {
                _score[index] = stu[(Subject)index];
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        public Student(String account, String passwd, String name)
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
        public Student(String account, String passwd, String name, TheSex sex, Int32 age, String address, Identity userType = Identity.Student)
            : base(account, passwd, name, sex, age, address, userType)
        {

        }
        /// <summary>
        /// 订阅
        /// </summary>
        public async void SubscriptionToHeadTeacher(Teacher teacher)
        {
            if (IsSubscription)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.DuplicateSubscriptions);
            }
            else
            {
                await Task.Run(() =>
                {
                    WeakEventManager<Teacher, Message>.AddHandler(teacher, nameof(teacher.NewMsg), ReceiveNewCurriculum);
                    IsSubscription = true;
                });
                Ui.DisplayTheInformationOfSuccessfully("(订阅成功)");
            }
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        public async void UnsubscribeToHeadTeacher(Teacher teacher)
        {
            if (! IsSubscription)
            {
                Ui.DisplayTheInformationOfErrorCode(ErrorCode.NotSubscribedYet);
            }
            else
            {
                await Task.Run(() => 
                        WeakEventManager<Teacher, Message>
                        .RemoveHandler(teacher, nameof(teacher.NewMsg), ReceiveNewCurriculum)
                    );
                Ui.DisplayTheInformationOfSuccessfully("取消订阅成功");
                IsSubscription = false;
            }
        }
        /// <summary>
        /// 实现IComparable接口
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Int32 CompareTo(Object other) => CompareTo(other as Student);
        /// <summary>
        /// 实现IComparable<Student>接口
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Int32 CompareTo(Student other) => GetTotalScore() < other.GetTotalScore() ? 1 : -1;
        /// <summary>
        /// 获取总成绩
        /// </summary>
        /// <returns>总成绩</returns>
        public Double GetTotalScore()
        {
            Double sum = default(Double);
            foreach (Double? index in _score)
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
            foreach (Double? index in this)
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
        private void ReceiveNewCurriculum(Object sender, Message msg) => NewMsg.Enqueue(msg);

        /// <summary>
        /// 查看新消息
        /// </summary>
        public void ViewNews()
        {
            if (HasNewMsg)
            {
                for (Int32 index = 0; index < NewMsg.Count; ++index)
                {
                    Message msg = NewMsg.Dequeue();
                    Ui.PrintColorMsg(msg.ToString(), ConsoleColor.Black, ConsoleColor.DarkMagenta);
                    NewMsg.TrimExcess();
                    AllNews.Add(msg);
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
            foreach (Message index in AllNews)
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
        public String ToString(String format, IFormatProvider formatProvider)
        {
            var score = new StringBuilder();
            foreach (Double? index in _score)
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
                    return $"{Name,-10} {Sex,-10} {Age,-10}";
                default:
                    throw new FormatException("Invalid format");
            }
        }
        public String ToString(String format) => ToString(format, null);
        /// <inheritdoc />
        /// <summary>
        /// 实现IEnumerable接口
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => new StudentIEnumerator(_score);
        /// <inheritdoc />
        /// <summary>
        /// 弱事件处理程序
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public Boolean ReceiveWeakEvent(Type managerType, Object sender, EventArgs e)
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
            private Double?[] _iscore { get; set; }
            /// <summary>
            /// 当前索引
            /// </summary>
            private Int32 _position;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="score"></param>
            public StudentIEnumerator(Double?[] score)
            {
                _iscore = score;
                _position = -1;
            }
            /// <summary>
            /// 当前元素
            /// </summary>
            public Object Current => _iscore[_position];
            /// <inheritdoc />
            /// <summary>
            /// 移动到下一元素
            /// </summary>
            /// <returns></returns>
            public Boolean MoveNext() => (++_position < _iscore.Length);
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
            public Int32 Compare(Student x, Student y) => x[_sortWay] > y[_sortWay] ? 1 : -1;
        }
    }
}
