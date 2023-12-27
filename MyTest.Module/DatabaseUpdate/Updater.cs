using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using MyTest.Module.BusinessObjects.Product;
using MyTest.Module.BusinessObjects.Enum;
using MyTest.Module.BusinessObjects.Production;

namespace MyTest.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        InitializeProductCategories();


        ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
    }

    private void InitializeProductCategories()
    {
        ProductCategory NSP = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NSP"));
        if (NSP == null)
        {
            NSP = ObjectSpace.CreateObject<ProductCategory>();
            NSP.ProductCategoryName = "NSP";
            //productCategory.SteelsCategory = SteelType.blackPlate;
        }
        
        ProductCategory NK = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NK"));
        if (NK == null)
        {
            NK = ObjectSpace.CreateObject<ProductCategory>();
            NK.ProductCategoryName = "NK";
            //productCategory.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory RB = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "RB"));
        if (RB == null)
        {
            RB = ObjectSpace.CreateObject<ProductCategory>();
            RB.ProductCategoryName = "RB";
            //productCategory.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory MVL = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MVL"));
        if (MVL == null)
        {
            MVL = ObjectSpace.CreateObject<ProductCategory>();
            MVL.ProductCategoryName = "MVL";
            //productCategory.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory MBWL = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MBWL"));
        if (MBWL == null)
        {
            MBWL = ObjectSpace.CreateObject<ProductCategory>();
            MBWL.ProductCategoryName = "MBWL";
            //productCategory.SteelsCategory = SteelType.blackPlate;
        }

    }

    void InitializeProductionTaskKind()
    {
        ProductionTaskKind materialCuttingProductionTaskKind = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Krojenie"));
        if (materialCuttingProductionTaskKind == null)
        {
            materialCuttingProductionTaskKind = ObjectSpace.CreateObject<ProductionTaskKind>();
            materialCuttingProductionTaskKind.TaskKindName = "Krojenie";
        }
        ProductionTaskKind folding = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Falcowanie"));
        if (folding == null)
        {
            folding = ObjectSpace.CreateObject<ProductionTaskKind>();
            folding.TaskKindName = "Falcowanie";
        }

        ProductionTaskKind packing = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Pakowanie"));
        if (packing == null)
        {
            packing = ObjectSpace.CreateObject<ProductionTaskKind>();
            packing.TaskKindName = "Pakowanie";
        }

    }

    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
}
