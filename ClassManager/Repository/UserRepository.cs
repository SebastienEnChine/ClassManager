using ClassManager.Contract;
using System.Collections.Generic;
using System.Linq;

namespace Sebastien.ClassManager.Core
{
    /// <summary>
    /// 用户库
    /// </summary>
    public class UserRepository : IUserRepository
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
        public static readonly Curriculum[] _curriculums = new Curriculum[2];

        private List<UserCore> _users;
        public UserCore GetUser(string account) => _users.Find(s => s.Account == account);

        public IEnumerable<UserCore> GetUsers() => _users;
        public bool DeleteUser(string account)
        {
            UserCore userToDelete = _users.Find(s => s.Account == account);
            if (userToDelete == null)
            {
                return false;
            }
            return _users.Remove(userToDelete);
        }
        public UserCore UpdateUser(UserCore user)
        {
            UserCore userToUpdate = _users.Find(s => s.Account == user.Account);
            int ix = _users.IndexOf(userToUpdate);
            _users[ix] = user;
            return _users[ix];
        }

        public UserCore AddUser(UserCore user)
        {
            _users.Add(user);
            return user;
        }
        //public static Queue<Curriculum> Curriculums = new Queue<Curriculum>(2);
    }
}
