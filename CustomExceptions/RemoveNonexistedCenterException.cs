using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QA_APP_Test_Project.EmploymentCenterModule.EmploymentCenterService;

namespace QA_APP_Test_Project.CustomExceptions
{
    internal class RemoveNonexistedCenterException: EcException
    {
        public RemoveNonexistedCenterException(string msg, Center center): base(msg, center)
        {
        }
    }
}
