using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QA_APP_Test_Project.EmploymentCenterModule.EmploymentCenterService;

namespace QA_APP_Test_Project.CustomExceptions
{
    internal class EcException: Exception
    {
        public Center _center { get; }
        public EcException(string msg, Center center): base(msg)
        {
            this._center = center;
        }
    }
}
