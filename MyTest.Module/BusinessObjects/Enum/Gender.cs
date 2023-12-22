using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Enum
{
    public enum Gender
    {
        [ImageName("Business_Businessman")]
        Male,
        [ImageName("Business_Businesswoman")]
        Female,
        [ImageName("BO_Customer")]
        Unknown
    }
}
