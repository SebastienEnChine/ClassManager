using System;
using System.Threading.Tasks;
using Sebastien.ClassManager.Enums;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 老师抽象基类
    /// </summary>
    public abstract class Teacher : User
    {
        /// <summary>
        /// 从业年份
        /// </summary>
        public Int32 YearsOfProfessional { get; }
        public event EventHandler<Message> NewMsg;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Teacher() { }
        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="user">用户对象</param>
        public Teacher(Teacher teacher) : base(teacher) => YearsOfProfessional = teacher.YearsOfProfessional;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="passwd">密码</param>
        /// <param name="userType">用户类型</param>
        public Teacher(String account, String passwd, String name, Int32 years, Identity userType = Identity.Instructor)
            : base(account, passwd, name, userType) => YearsOfProfessional = years;
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
        public Teacher(String account, String passwd, String name, TheSex sex, Int32 age, String address, Int32 years, Identity userType = Identity.Instructor)
            : base(account, passwd, name, sex, age, address, userType) => YearsOfProfessional = years;
        /// <summary>
        /// 发布新通知
        /// </summary>
        /// <param name="msg">消息</param>       
        //await Task.Factory.FromAsync(NewMsg.BeginInvoke, NewMsg.EndInvoke, this, msg, null);
        public async void ReleaseNewMsg(Message msg) => await Task.Run(() => NewMsg?.Invoke(this, msg));

        /// <summary>
        /// 重写用户基类的ToString()方法
        /// </summary>
        /// <returns></returns>
        public override String ToString() => $"{base.ToString()}从业年份: {YearsOfProfessional}\n";
    }
}
