namespace Sebastien.ClassManager.Enums
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 不是一个命令
        /// </summary>
        NotACommand,
        /// <summary>
        /// 账号格式错误
        /// </summary>
        BadAccount,
        /// <summary>
        /// 密码格式错误
        /// </summary>
        BadPasswd,
        /// <summary>
        /// 账户或密码不匹配
        /// </summary>
        AccountOrPasswdError,
        /// <summary>
        /// 账户已经存在
        /// </summary>
        AccountAlreadyExists,
        /// <summary>
        /// 找不到此账户
        /// </summary>
        CantFindThisAccount,
        /// <summary>
        /// 参数错误
        /// </summary>
        ArgumentError,
        /// <summary>
        /// 密码不一致
        /// </summary>
        InconsistentPassword,
        /// <summary>
        /// 密码错误
        /// </summary>
        PasswdError,
        /// <summary>
        /// 无可显示信息
        /// </summary>
        NoDisplayableInformation,
        /// <summary>
        /// 重复订阅
        /// </summary>
        DuplicateSubscriptions,
        /// <summary>
        /// 无法添加
        /// </summary>
        CantAdd,
        /// <summary>
        /// 还未订阅
        /// </summary>
        NotSubscribedYet,
        /// <summary>
        /// 参数超出范围
        /// </summary>
        ArgumentOutOfRange
    }
}
