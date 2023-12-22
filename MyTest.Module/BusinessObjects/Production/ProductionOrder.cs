using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    internal class ProductionOrder : BaseClass
    {
        public ProductionOrder(Session session)
            : base(session)
        {
        }
    }
}
