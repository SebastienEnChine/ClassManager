namespace Sebastien.ClassManager.Enums
{
    /// <summary>
    /// 用户可执行命令
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// 退出程序        
        /// </summary>
        Exit,
        /// <summary>
        /// 获取帮助
        /// </summary>
        GetHelp,
        /// <summary>
        /// 修改地址
        /// </summary>
        ChangeAddress,
        /// <summary>
        /// 修改年龄
        /// </summary>
        ChangeAge,
        /// <summary>
        /// 修改性别
        /// </summary>
        ChangeSex,
        /// <summary>
        /// 个人信息概览
        /// </summary>
        ShowMe,
        /// <summary>
        /// 切换用户
        /// </summary>
        SwitchUser,
        /// <summary>
        /// 查看本周和下周课表
        /// </summary>
        ViewCurriculums,
        /// <summary>
        /// 学生列表
        /// </summary>
        StudentsPreview,
        /// <summary>
        /// 任课教师列表
        /// </summary>
        TeachersPreview,
        /// <summary>
        /// 查看班主任信息
        /// </summary>
        ViweInformatinOfHeadTeacher,
        /// <summary>
        /// 查看操作记录
        /// </summary>
        ViewMyHistory,
        /// <summary>
        /// 修改密码
        /// </summary>
        ChangePasswd,
        /// <summary>
        /// 个人成绩概览
        /// </summary>
        MyScore,
        /// <summary>
        /// 查看新消息
        /// </summary>
        ViewNews,
        /// <summary>
        /// 查看消息/通知记录
        /// </summary>
        ViewAllNews,
        /// <summary>
        /// 订阅班主任
        /// </summary>
        SubscriptionToHeadTeacher,
        /// <summary>
        /// 取消订阅班主任
        /// </summary>
        UnsubscribeToHeadTeacher,
        /// <summary>
        /// 查看班主任信息
        /// </summary>
        ViewHeadTeacher,
        /// <summary>
        /// 显示本班学生的成绩(不排序)
        /// </summary>
        AllScore,
        /// <summary>
        /// 显示本班学生的成绩(排序)
        /// </summary>
        AllScoreAndRank,
        /// <summary>
        /// 修改/设置成绩
        /// </summary>
        ChangeScore,
        /// <summary>
        /// 修改学生或老师的姓名
        /// </summary>
        ChangeNameOfThisUser,
        /// <summary>
        /// 修改姓名
        /// </summary>
        ChangeName,
        /// <summary>
        /// 添加新生
        /// </summary>
        AddStudent,
        /// <summary>
        /// 添加新任课老师
        /// </summary>
        AddTeacher,
        /// <summary>
        /// 从本程序移除一个学生/教师用户
        /// </summary>
        Remove,
        /// <summary>
        /// 发布新课表
        /// </summary>
        ReleaseNewCurriculum,
        /// <summary>
        /// 广播一条消息
        /// </summary>
        ReleaseAMsg,
        /// <summary>
        /// 查看高于指定分数的所有学生
        /// </summary>
        HighThan,
        /// <summary>
        /// 留言墙
        /// </summary>
        LeaveAMessage,
        /// <summary>
        /// 查看留言墙
        /// </summary>
        ViewLeaveMessages,
        /// <summary>
        /// 测试计算机能否被指定Web地址响应
        /// </summary>
        UrlTest
    }
}
