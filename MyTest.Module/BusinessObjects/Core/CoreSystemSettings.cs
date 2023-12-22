using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Core
{
    [RuleObjectExists("AnotherSystemSettingsExists", DefaultContexts.Save, "True", InvertResult = true, CustomMessageTemplate = "Another SystemSettings already exists.")]
    [RuleCriteria("CannotDeleteSystemSettings", DefaultContexts.Delete, "False", CustomMessageTemplate = "Cannot delete SystemSettings.")]
    [Persistent("SystemSettings")]
    public abstract class CoreSystemSettings: BaseClass, ICoreObject
    {
        public CoreSystemSettings(Session session)
           : base(session)
        {
        }
        private string _SystemCode;
        [Browsable(false)]
        public string SystemCode
        {
            get
            {
                return _SystemCode;
            }
            set
            {
                SetPropertyValue("SystemCode", ref _SystemCode, value);
            }
        }
    }
}
