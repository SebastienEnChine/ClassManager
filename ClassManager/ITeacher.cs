using System;
using Sebastien.ClassManager.Enums;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 教师接口
    /// </summary>
    public interface ITeacher
    {
        /// <summary>
        /// 学生成绩列表
        /// </summary>
        /// <param name="IsDisplayRank">是否显示序号</param>
        /// <param name="IsSort">是否排序</param>
        void ViewScoreOfAllStudent(State IsDisplayRank = State.off, State IsSort = State.off);
        /// <summary>
        /// 显示此分数以上的所有学生
        /// </summary>
        /// <param name="score">指定分数</param>
        void GetStuHighThan(Int32 score);
    }
}
