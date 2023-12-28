using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Core.Dictionaries
{
    [DefaultProperty(nameof(SystemDocumentState))]
    public class BaseDocumentState : XPObject
    {
        public BaseDocumentState(Session session):base(session) 
        {}

        SystemDocumentState _SystemDocumentState;
        public SystemDocumentState SystemDocumentState
        {
            get => _SystemDocumentState;
            set => SetPropertyValue(nameof(SystemDocumentState), ref _SystemDocumentState, value);
        }

        string _DefaultIconName;
        public string DefaultIconName
        {
            get => _DefaultIconName;
            set => SetPropertyValue(nameof(DefaultIconName), ref _DefaultIconName, value);
        }
    }
}
