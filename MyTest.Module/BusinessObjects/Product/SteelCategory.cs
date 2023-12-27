using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class SteelCategory : BaseClass
    {
        public SteelCategory(Session session)
        : base(session)
        {
        }

        private SteelType _steelCategory;
        public SteelType SteelsCategory
        {
            get
            {
                return _steelCategory;
            }
            set
            {
                SetPropertyValue("SteelsCategory", ref _steelCategory, value);
            }
        }
    }
}

