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
using DevExpress.XtraPrinting.Native;
using System.Drawing;
using MyTest.Module.BusinessObjects.Core.Dictionaries;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Utils;

namespace MyTest.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        InitializeProductCategories();
        InitializeProductionTaskKind();
        InitializeBaseBaseDocumentStates();
        GenerateProductionTaskStateMachine();

        ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
    }

    private void InitializeBaseBaseDocumentStates()
    {
        BaseDocumentState createdStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Created));
        if (createdStatus == null)
            createdStatus = GenerateBaseDocumentState(SystemDocumentState.Created,"Created");

        BaseDocumentState sentStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Ordered));
        if (sentStatus == null)
            sentStatus = GenerateBaseDocumentState(SystemDocumentState.Ordered, "Ordered");

        BaseDocumentState inProgressStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.InProgress));
        if (inProgressStatus == null)
            inProgressStatus = GenerateBaseDocumentState(SystemDocumentState.InProgress, "progress_bar");

        BaseDocumentState closedStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Closed));
        if (closedStatus == null)
            closedStatus = GenerateBaseDocumentState(SystemDocumentState.Closed, "");

        BaseDocumentState paidStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Paid));
        if (paidStatus == null)
            paidStatus = GenerateBaseDocumentState(SystemDocumentState.Paid, "Business_Cash");

        BaseDocumentState notPaidStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.NotPaid));
        if (notPaidStatus == null)
            notPaidStatus = GenerateBaseDocumentState(SystemDocumentState.NotPaid, "Payment");

        BaseDocumentState partiallyPaidStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.PartiallyPaid));
        if (partiallyPaidStatus == null)
            partiallyPaidStatus = GenerateBaseDocumentState(SystemDocumentState.PartiallyPaid, "Business_Money");

        BaseDocumentState overPaidStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.OverPaid));
        if (overPaidStatus == null)
            overPaidStatus = GenerateBaseDocumentState(SystemDocumentState.OverPaid, "Business_Money");

        BaseDocumentState cancelledStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Cancelled));
        if (cancelledStatus == null)
            cancelledStatus = GenerateBaseDocumentState(SystemDocumentState.Cancelled, "Cancelled");

        BaseDocumentState acceptedStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Accepted));
        if (acceptedStatus == null)
            acceptedStatus = GenerateBaseDocumentState(SystemDocumentState.Accepted, "");

        BaseDocumentState commisionedStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Commissioned));
        if (commisionedStatus == null)
            commisionedStatus = GenerateBaseDocumentState(SystemDocumentState.Commissioned, "Electronics_TV");

        BaseDocumentState inCorrectionStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.InCorrection));
        if (inCorrectionStatus == null)
            inCorrectionStatus = GenerateBaseDocumentState(SystemDocumentState.InCorrection, "InCorrection");

        BaseDocumentState suspendedStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Suspended));
        if (suspendedStatus == null)
            suspendedStatus = GenerateBaseDocumentState(SystemDocumentState.Suspended);

        BaseDocumentState completedStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Completed));
        if (completedStatus == null)
            completedStatus = GenerateBaseDocumentState(SystemDocumentState.Completed);

        BaseDocumentState deliveredStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.Delivered));
        if (deliveredStatus == null)
            deliveredStatus = GenerateBaseDocumentState(SystemDocumentState.Delivered);

        BaseDocumentState partiallyDeliveredStatus = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", SystemDocumentState.PartiallyDelivered));
        if (partiallyDeliveredStatus == null)
            partiallyDeliveredStatus = GenerateBaseDocumentState(SystemDocumentState.PartiallyDelivered);

    }

    private BaseDocumentState GenerateBaseDocumentState(SystemDocumentState status, string iconName = null)
    {
        BaseDocumentState BaseDocumentState = ObjectSpace.CreateObject<BaseDocumentState>();
        BaseDocumentState.SystemDocumentState = status;
        if (string.IsNullOrEmpty(iconName))
            iconName = "BO_Task";
        BaseDocumentState.DefaultIconName = iconName;
        return BaseDocumentState;
    }

    void GenerateProductionTaskStateMachine()
    {
        XpoStateMachine document = ObjectSpace.FindObject<XpoStateMachine>(new BinaryOperator("Name", "Zlecenie produkcyjne"));
        if (document == null)
        {
            document = ObjectSpace.CreateObject<XpoStateMachine>();
            document.Name = "Zlecenie produkcyjne";
            document.Active = true;
            document.TargetObjectType = typeof(ProductionTask);
            document.StatePropertyName = new StringObject(nameof(ProductionTask.DocumentState));

            XpoState created = GenerateXpoState("Utworzony", document, GetMarkerValue(SystemDocumentState.Created));
            XpoState commisioned = GenerateXpoState("Na produkcji", document, GetMarkerValue(SystemDocumentState.Commissioned));
            XpoState inProgress = GenerateXpoState("W realizacji", document, GetMarkerValue(SystemDocumentState.InProgress));
            XpoState suspended = GenerateXpoState("Wstrzymany", document, GetMarkerValue(SystemDocumentState.Suspended));
            XpoState completed = GenerateXpoState("Zakończono", document, GetMarkerValue(SystemDocumentState.Completed));
            XpoState closed = GenerateXpoState("Zamknięto", document, GetMarkerValue(SystemDocumentState.Closed));
            document.StartState = created;

            XpoTransition commisionedCreated = GenerateTransition("Zwolnij na produkcję", created, commisioned);
            XpoTransition completedInProgress = GenerateTransition("Zakończ", inProgress, completed);
            XpoTransition completedSuspended = GenerateTransition("Zakończ", suspended, completed);
            XpoTransition completedCommisioned = GenerateTransition("Zakończ", commisioned, completed);
            XpoTransition resumeCompleted = GenerateTransition("Wznów", completed, commisioned);
            XpoTransition closeCompleted = GenerateTransition("Zamknij", completed, closed);

            created.Transitions.Add(commisionedCreated);
            commisioned.Transitions.Add(completedCommisioned);
            inProgress.Transitions.Add(completedInProgress);
            suspended.Transitions.Add(completedSuspended);
            completed.Transitions.Add(resumeCompleted);
            completed.Transitions.Add(closeCompleted);

        }
    }

    private XpoState GenerateXpoState(string caption, XpoStateMachine stateMachine, string markerValue, string targetObjectCriteria = null)
    {
        XpoState state = ObjectSpace.CreateObject<XpoState>();
        state.Caption = caption;
        state.StateMachine = stateMachine;
        state.MarkerValue = markerValue;
        state.TargetObjectCriteria = targetObjectCriteria;
        return state;
    }

    private XpoTransition GenerateTransition(string caption, XpoState sourceState, XpoState targetState)
    {
        XpoTransition transition = ObjectSpace.CreateObject<XpoTransition>();
        transition.Caption = caption;
        transition.SourceState = sourceState;
        transition.TargetState = targetState;
        return transition;
    }

    private string GetMarkerValue(SystemDocumentState documentState)
    {
        BaseDocumentState state = ObjectSpace.FindObject<BaseDocumentState>(new BinaryOperator("SystemDocumentState", documentState), true);
        return state.GetType().FullName + "(" + state.Oid + ")";
    }

    private void InitializeProductCategories()
    {
        ProductCategory NSP = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NSP"));
        if (NSP == null)
        {
            NSP = ObjectSpace.CreateObject<ProductCategory>();
            NSP.ProductCategoryName = "NSP";
            NSP.SteelsCategory = SteelType.blackPlate;
        }
        
        ProductCategory NK = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NK"));
        if (NK == null)
        {
            NK = ObjectSpace.CreateObject<ProductCategory>();
            NK.ProductCategoryName = "NK";
            NK.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory RB = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "RB"));
        if (RB == null)
        {
            RB = ObjectSpace.CreateObject<ProductCategory>();
            RB.ProductCategoryName = "RB";
            RB.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory MVL = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MVL"));
        if (MVL == null)
        {
            MVL = ObjectSpace.CreateObject<ProductCategory>();
            MVL.ProductCategoryName = "MVL";
            MVL.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory MBWL = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MBWL"));
        if (MBWL == null)
        {
            MBWL = ObjectSpace.CreateObject<ProductCategory>();
            MBWL.ProductCategoryName = "MBWL";
            MBWL.SteelsCategory = SteelType.blackPlate;
        }

        ProductCategory NSPacid = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NSP S235"));
        if (NSPacid == null)
        {
            NSPacid = ObjectSpace.CreateObject<ProductCategory>();
            NSPacid.ProductCategoryName = "NSP S235";
            NSPacid.SteelsCategory = SteelType.acidResistantPlate;
        }

        ProductCategory NKacid = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "NK S235"));
        if (NKacid == null)
        {
            NKacid = ObjectSpace.CreateObject<ProductCategory>();
            NKacid.ProductCategoryName = "NK S235";
            NKacid.SteelsCategory = SteelType.acidResistantPlate;
        }

        ProductCategory RBacid = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "RB S235"));
        if (RBacid == null)
        {
            RBacid = ObjectSpace.CreateObject<ProductCategory>();
            RBacid.ProductCategoryName = "RB S235";
            RBacid.SteelsCategory = SteelType.acidResistantPlate;
        }

        ProductCategory MVLacid = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MVL S235"));
        if (MVLacid == null)
        {
            MVLacid = ObjectSpace.CreateObject<ProductCategory>();
            MVLacid.ProductCategoryName = "MVL S235";
            MVLacid.SteelsCategory = SteelType.acidResistantPlate;
        }

        ProductCategory MBWLacid = ObjectSpace.FindObject<ProductCategory>(new BinaryOperator(nameof(ProductCategory.ProductCategoryName), "MBWL S235"));
        if (MBWLacid == null)
        {
            MBWLacid = ObjectSpace.CreateObject<ProductCategory>();
            MBWLacid.ProductCategoryName = "MBWL S235";
            MBWLacid.SteelsCategory = SteelType.acidResistantPlate;
        }
    }

    void InitializeProductionTaskKind()
    {
        ProductionTaskKind creatingATechnicalDwawing = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Stworzenie rysunku technicznego"));
        if (creatingATechnicalDwawing == null)
        {
            creatingATechnicalDwawing = ObjectSpace.CreateObject<ProductionTaskKind>();
            creatingATechnicalDwawing.TaskKindName = "Stworzenie rysunku technicznego";
        }

        ProductionTaskKind materialCuttingProductionTaskKind = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Cięcie piłą"));
        if (materialCuttingProductionTaskKind == null)
        {
            materialCuttingProductionTaskKind = ObjectSpace.CreateObject<ProductionTaskKind>();
            materialCuttingProductionTaskKind.TaskKindName = "Cięcie piłą";
        }
        ProductionTaskKind cleaningSteel = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Śrutowanie"));
        if (cleaningSteel == null)
        {
            cleaningSteel = ObjectSpace.CreateObject<ProductionTaskKind>();
            cleaningSteel.TaskKindName = "Śrutowanie";
        }

        ProductionTaskKind bending = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Gięcie na prasie"));
        if (bending == null)
        {
            bending = ObjectSpace.CreateObject<ProductionTaskKind>();
            bending.TaskKindName = "Gięcie na prasie";
        }

        ProductionTaskKind painting = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Malowanie"));
        if (painting == null)
        {
            painting = ObjectSpace.CreateObject<ProductionTaskKind>();
            painting.TaskKindName = "Malowanie";
        }

        ProductionTaskKind CNC = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "CNC"));
        if (CNC == null)
        {
            CNC = ObjectSpace.CreateObject<ProductionTaskKind>();
            CNC.TaskKindName = "CNC";
        }

        ProductionTaskKind laser = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Cięcie laserem"));
        if (laser == null)
        {
            laser = ObjectSpace.CreateObject<ProductionTaskKind>();
            laser.TaskKindName = "Cięcie laserem";
        }

        ProductionTaskKind acidizing = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Kwasowanie"));
        if (acidizing == null)
        {
            acidizing = ObjectSpace.CreateObject<ProductionTaskKind>();
            acidizing.TaskKindName = "Kwasowanie";
        }

        ProductionTaskKind stockEditionMechanic = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Wydanie komponentó na montaż"));
        if (stockEditionMechanic == null)
        {
            stockEditionMechanic = ObjectSpace.CreateObject<ProductionTaskKind>();
            stockEditionMechanic.TaskKindName = "Wydanie komponentów na montaż";
        }

        ProductionTaskKind stockEditionElectric = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Wydanie komponentów na dział elektryczny"));
        if (stockEditionElectric == null)
        {
            stockEditionElectric = ObjectSpace.CreateObject<ProductionTaskKind>();
            stockEditionElectric.TaskKindName = "Wydanie komponentów na dział elektryczny";
        }

        ProductionTaskKind mechanicalAssembly = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Montaż konstrukcji"));
        if (mechanicalAssembly == null)
        {
            mechanicalAssembly = ObjectSpace.CreateObject<ProductionTaskKind>();
            mechanicalAssembly.TaskKindName = "Montaż konstrukcji";
        }

        ProductionTaskKind createTheElectricalCabinet = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Szycie szafy elektrycznej"));
        if (createTheElectricalCabinet == null)
        {
            createTheElectricalCabinet = ObjectSpace.CreateObject<ProductionTaskKind>();
            createTheElectricalCabinet.TaskKindName = "Szycie szafy elektrycznej";
        }

        ProductionTaskKind createTheElectricalDiagram = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Przygotowanie schematu elektrycznego"));
        if (createTheElectricalDiagram == null)
        {
            createTheElectricalDiagram = ObjectSpace.CreateObject<ProductionTaskKind>();
            createTheElectricalDiagram.TaskKindName = "Przygotowanie schematu elektrycznego";
        }

        ProductionTaskKind electicalAssembly = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Montaż elektryki"));
        if (electicalAssembly == null)
        {
            electicalAssembly = ObjectSpace.CreateObject<ProductionTaskKind>();
            electicalAssembly.TaskKindName = "Montaż elektryki";
        }

        ProductionTaskKind packing = ObjectSpace.FindObject<ProductionTaskKind>(new BinaryOperator(nameof(ProductionTaskKind.TaskKindName), "Pakowanie na tira"));
        if (packing == null)
        {
            packing = ObjectSpace.CreateObject<ProductionTaskKind>();
            packing.TaskKindName = "Pakowanie na tira";
        }

    }

    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
}
