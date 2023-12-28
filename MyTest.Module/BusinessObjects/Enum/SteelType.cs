using MyTest.Module.BusinessObjects.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Enum
{
    public enum SteelType
    {
        [Display(Name = "Blacha kwasowa", ResourceType = typeof(SteelCategoryResources))]
        acidResistantPlate = 10,
        [Display(Name = "Blacha czarna", ResourceType = typeof(SteelCategoryResources))]
        blackPlate = 20,
    }

    public class SteelCategoryResources
    {
        public static string acidResistantPlate => "Blacha kwasowa";
        public static string blackPlate => "Blacha czarna";
    }
}
