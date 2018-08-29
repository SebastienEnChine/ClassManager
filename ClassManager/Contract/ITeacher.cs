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
        /// <param name="isDisplayRank">是否显示序号</param>
        /// <param name="isSort">是否排序</param>
        void ViewScoreOfAllStudent(State isDisplayRank = State.Off, State isSort = State.Off);

        /// <summary>
        /// 显示此分数以上的所有学生
        /// </summary>
        /// <param name="score">指定分数</param>
        void GetStuHighThan(int score);
    }
}
