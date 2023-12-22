using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Import.EPub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using System.Text.RegularExpressions;
using MyTest.Module.BusinessObjects.Enum;
using MyTest.Module.BusinessObjects.Core;
using DevExpress.CodeParser;

namespace MyTest.Module.BusinessObjects.CRM
{
    public class Person : BaseClass
    {
        public Person(Session session)
            : base(session)
        {
        }

        private string _FirstName;
        [Size(30)] 
        [RuleRequiredField("Imie","Pole Imie nie może być puste.")] 
        [ImmediatePostData]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                SetPropertyValue("FirstName", ref _FirstName, value);
            }
        }

        private string _LastName;
        [Size(40)]
        [RuleRequiredField("Nazwisko", "Pole Nazwisko nie może być puste.")]
        [ImmediatePostData]
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                SetPropertyValue("LastName", ref _LastName, value);
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

                SetPropertyValue("NumberPhone", ref _phoneNumber, value);
            }
        }
        private EmployeeRole employeeRole;
        public EmployeeRole EmployeeRole
        {
            get { return employeeRole; }
            set { SetPropertyValue(nameof(EmployeeRole), ref employeeRole, value); }
        }
        private Gender _Gender;
        [ImmediatePostData]
        public Gender Gender
        {
            get { return _Gender; }
            set
            {
                if (SetPropertyValue("Gender", ref _Gender, value) && !base.IsLoading)
                {
                    OnChanged("Image"); // Wywołaj zmianę, aby zaktualizować wyświetlane zdjęcie
                }
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

                SetPropertyValue("DefaultEmailPerson", ref _DefaultEmail, value);
            }
        }



    }
}





