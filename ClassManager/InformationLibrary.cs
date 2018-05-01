using System.Collections.Generic;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 用户库
    /// </summary>
    public static class InformationLibrary
    {
        /// <summary>
        /// 学生用户库
        /// </summary>
        public static List<Student> StudentLibrary { get; } = new List<Student>();
        /// <summary>
        /// 教师用户库
        /// </summary>
        public static List<Instructor> TeacherLibrary { get; } = new List<Instructor>();
        /// <summary>
        /// 班主任用户
        /// </summary>
        public static HeadTeacher HeadTeacherUser { get; } = HeadTeacher.GetHeadTeacher("headteacher", "1234", "Teacher He", 2000);
        /// <summary>
        /// 课表信息
        /// </summary>
        public static Curriculum[] _curriculums = new Curriculum[2];
        //public static Queue<Curriculum> Curriculums = new Queue<Curriculum>(2);
    }
}
