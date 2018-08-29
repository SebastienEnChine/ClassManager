using Sebastien.ClassManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Contract
{
    public interface IQueryRepository
    {
        UserCore GetUser(string account);
        IEnumerable<UserCore> GetUsers();
    }
}
