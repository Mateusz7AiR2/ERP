using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    public class ProductionTaskKind : BaseClass
    {
        public ProductionTaskKind(Session session) : base(session)
        {
        }

        string _TaskKindName;
        public string TaskKindName
        {
            get => _TaskKindName;
            set => SetPropertyValue(nameof(TaskKindName), ref _TaskKindName, value);
        }
    }
}
