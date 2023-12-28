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
