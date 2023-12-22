using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyTest.Module.BusinessObjects.Core.Interfaces;

namespace MyTest.Module.BusinessObjects.Core
{
    [NonPersistent]
    [VisibleInReports(false)]
    [VisibleInDashboards(false)]
    public abstract class BaseClass : XPObject, IObjectCode
    {
        public BaseClass(Session session)
        : base(session)
        {
        }

        [Browsable(false)]
        public bool IsPersistent => GetType().CustomAttributes.ToList().FirstOrDefault((CustomAttributeData x) => x.AttributeType == typeof(NonPersistentAttribute)) == null;

        [VisibleInDetailView(false)]
        public string ObjectCode => GetSystemCode();//

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SystemHelper.TempIntOid--;
            base.Oid = SystemHelper.TempIntOid;
        }

        //Generwowanie ObjectCode
        protected virtual string GetSystemCode()
        {
            return IsPersistent ? ("!" + base.Session.GetObjectType(base.Session.GetClassInfo(this)).Oid + "-" + base.Oid) : string.Empty;
        }
    }
}

