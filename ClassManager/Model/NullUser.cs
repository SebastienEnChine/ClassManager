using Sebastien.ClassManager.Core;
using Sebastien.ClassManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager
{
    public class NullUser : IUser
    {
        public void ViewTheInformationOfTheHeadteacher() 
            => Ui.DisplayTheInformationOfErrorCode(ErrorCode.CantFindThisAccount);
    }
}
