using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.CRM
{
    public class Entity : BaseClass
    {
        public Entity(Session session)
        : base(session)
        {
        }

        private string _LongName;
        [Size(255)]
        [ImmediatePostData]
        [RuleRequiredField]
        public string LongName
        {
            get
            {
                return _LongName;
            }
            set
            {
                if (!base.IsLoading)
                {
                    value = value?.Trim();
                }

                SetPropertyValue("LongName", ref _LongName, value);
            }
        }


    }
}
