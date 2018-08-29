using Sebastien.ClassManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Contract
{
    interface IUpdateRepository
    {
        bool DeleteUser(string account);
        UserCore UpdateUser(UserCore user);
        UserCore AddUser(UserCore user);
    }
}
