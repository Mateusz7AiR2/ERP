using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Enum;
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
        private string _ShortName;
        [RuleRequiredField]
        [ImmediatePostData]
        [Size(255)]
        public string ShortName
        {
            get
            {
                return _ShortName;
            }
            set
            {
                if (!base.IsLoading)
                {
                    value = value?.Trim();
                }

                SetPropertyValue("ShortName", ref _ShortName, value);
            }
        }

        private string _VATIN;
        [ImmediatePostData]
        public string VATIN
        {
            get
            {
                return _VATIN;
            }
            set
            {
                if (value != null && !base.IsLoading && !base.IsSaving && !base.IsDeleted)
                {
                    value = value.Trim();
                }

                SetPropertyValue("VATIN", ref _VATIN, value);
            }
        }

        private string _DefaultEmail;
        [ImmediatePostData]
        public string DefaultEmail
        {
            get
            {
                return _DefaultEmail;
            }
            set
            {
                if (value != null && !base.IsLoading && !base.IsSaving && !base.IsDeleted)
                {
                    value = value.Trim();
                }

                SetPropertyValue("DefaultEmail", ref _DefaultEmail, value);
            }
        }

        private string _phoneNumber;
        [Size(10)]
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (!base.IsLoading)
                {
                    value = value?.Trim();
                }

                SetPropertyValue("Number", ref _phoneNumber, value);
            }
        }
        private string _street;
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                SetPropertyValue("Street", ref _street, value);
            }
        }

        private string _streetNumber;
        [Size(9)]
        [ImmediatePostData]
        public string StreetNumber
        {
            get
            {
                return _streetNumber;
            }
            set
            {
                if (!base.IsLoading)
                {
                    value = value?.Trim();
                }

                SetPropertyValue("StreetNumber", ref _streetNumber, value);
            }
        }

        private string _locality;
        public string Locality
        {
            get
            {
                return _locality;
            }
            set
            {
                SetPropertyValue("Locality", ref _locality, value);
            }
        }

        private string _countryRegion;
        [ImmediatePostData]
        public string CountryRegion
        {
            get
            {
                return _countryRegion;
            }
            set
            {
                SetPropertyValue("CountryRegion", ref _countryRegion, value);
            }
        }
    

        private string _country;
        [ImmediatePostData]
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                SetPropertyValue("Country", ref _country, value);
            }
        }

        private EntityRole entityRole;
        public EntityRole EntityRole
        {
            get { return entityRole; }
            set { SetPropertyValue(nameof(EntityRole), ref entityRole, value); }
        }
    }
}
