using MyTest.Module.BusinessObjects.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Core
{
    public static class SystemHelper
    {
        public const string NonPersistentContext = "NonPersistentContext";
        public static int TempIntOid { get; set; }
    }
}
