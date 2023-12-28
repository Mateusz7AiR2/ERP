using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class Element : BaseClass
    {
        public Element(Session session)
        : base(session)
        {
        }

        private string _elementName;
        public string ElementName
        {
            get
            {
                return _elementName;
            }
            set
            {
                SetPropertyValue(nameof(ElementName), ref _elementName, value);
            }
        }
        bool _Completed;
        public bool Completed
        {
            get => _Completed;
            set => SetPropertyValue(nameof(Completed), ref _Completed, value);
        }

        ProductionTaskItem _ProductionTaskItem;
        [Association]
        public ProductionTaskItem ProductionTaskItem
        {
            get => _ProductionTaskItem;
            set => SetPropertyValue(nameof(ProductionTaskItem), ref _ProductionTaskItem, value);
        }
    }

}
