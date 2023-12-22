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
using DevExpress.XtraRichEdit.Model;
using DevExpress.ExpressApp.Model;
using MyTest.Module.BusinessObjects.CRM;

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

        protected override void OnSaving()
        {
            base.OnSaving();
            UpdateBaseClassInfo();
        }

        [Browsable(false)]
        public bool IsPersistent => GetType().CustomAttributes.ToList().FirstOrDefault((CustomAttributeData x) => x.AttributeType == typeof(NonPersistentAttribute)) == null;

        [VisibleInDetailView(false)]
        public string ObjectCode => GetSystemCode();

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SystemHelper.TempIntOid--;
            base.Oid = SystemHelper.TempIntOid;
        }

        private string _UpdatedOn;
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                SetPropertyValue("UpdatedOn", ref _UpdatedOn, value);
            }
        }

        private string _CreatedOn;
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string CreatedOn
        {
            get
            {
                return _CreatedOn;
            }
            set
            {
                SetPropertyValue("CreatedOn", ref _CreatedOn, value);
            }
        }

        private void UpdateBaseClassInfo()
        {
            if (_UpdatedOn == null && _CreatedOn == null)
            {
                _UpdatedOn = "Zaktualizowano " + DateTime.Now.ToString();
                _CreatedOn = "Utworzono " + DateTime.Now.ToString();
            }
            else
            {
                _UpdatedOn = "Zaktualizowano " + DateTime.Now.ToString();
            }

        }

        //Generwowanie ObjectCode
        protected virtual string GetSystemCode()
        {
            return IsPersistent ? ("!" + base.Session.GetObjectType(base.Session.GetClassInfo(this)).Oid + "-" + base.Oid) : string.Empty;
        }
    }
}

