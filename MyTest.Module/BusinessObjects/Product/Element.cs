using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
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

    }
}
