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

namespace MyTest.Module.BusinessObjects.CRM
{
    public class Person : BaseClass
    {
     //   public Person() : base() { }
        public Person(Session session)
            : base(session)
        {
        }
        //[Key]
        //public int Oid
        //{
        //    get { return GetPropertyValue<int>("Oid"); }
        //    set { SetPropertyValue("Oid", value); }
        //}

        private string _FirstName;
        [Size(30)] // rozmiar
        [RuleRequiredField("Imie","Pole Imie nie może być puste.")] // nie może być pusta
        [ImmediatePostData] // od razu zapisuje do bazy bez wciśkania ok/ zapisz
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


        private string _Address;

        public static Type TargetType { get; set; }

        [Size(254)]
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                if (!base.IsLoading)
                {
                    value = value?.Trim();
                }

                SetPropertyValue("Address", ref _Address, value);
            }
        }
    }
}





