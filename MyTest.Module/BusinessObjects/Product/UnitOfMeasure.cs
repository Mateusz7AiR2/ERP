using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class UnitOfMeasure : BaseClass
    {
        public UnitOfMeasure(Session session)
        : base(session)
        {
        }

        private string _beaseUnitOfMeasure;
        public string BeaseUnitOfMeasure
        {
            get
            {
                return _beaseUnitOfMeasure;
            }
            set
            {
                SetPropertyValue("BeaseUnitOfMeasure", ref _beaseUnitOfMeasure, value);
            }
        }
        
    }
}
