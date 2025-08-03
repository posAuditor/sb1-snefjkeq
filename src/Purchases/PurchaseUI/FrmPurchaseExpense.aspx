<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="FrmPurchaseExpense.aspx.cs" Inherits="Purchases_PurchaseUI_FrmPurchaseExpense" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../../Styles/DevextremeStyle.css" rel="stylesheet" />
    <link href="../../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../../Content/font-awesome.min.css" rel="stylesheet" />

    <!-- Bootstrap CSS -->
    <link href="../../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />    

    <script type="text/javascript" src="../../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>
    <!-- DevExtreme theme -->
    <link href="../../Content/devextreme/dx.light.css" rel="stylesheet" />

    <!-- DevExtreme library -->
    <script type="text/javascript" src="../../Content/devextreme/dx.all.js"></script>
    <%--<script type="text/javascript" src="../../Content/devextreme/dx.web.js"></script>
    <script type="text/javascript" src="../../Content/devextreme/dx.viz.js"></script>--%>
    <script type="text/javascript" src="../../Content/devextreme/dx.aspnet.data.js"></script>

    <script type="text/javascript">
        var obj = <%=Convert.ToByte(this.MyContext.CurrentCulture)%>;
        console.log("obj:" + obj);
        if (obj == 0 || obj == "0") {
            if (!IsMobile())
                document.write('<link href="../../Styles/DevextremeStyleRTL.css" rel="stylesheet" />');
            document.write('<link href="../../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />');
        }
    </script>

    <!-- DataGrid & DevExtreme Scripts -->
    <script type="text/javascript">
        var currentPurchaseExpensesRowIndex = 0;
        var allPurchaseExpensesAccounts = [];
        var jqVar = jQuery.noConflict();
        jqVar(document).ready(function () {
            loadDevextremeLocalization();

            jqVar.getJSON("../../api/ChartOfAccount/GetChartOfAccountsCheledronly", null, function (exResponse) {
                for (i = 0; i < exResponse.length; i++) {
                    allPurchaseExpensesAccounts.push(exResponse[i]);
                }

                setTimeout(function () {
                    var ekd = ekdArray[<%=this.MyContext.PurchasesOptions.EnterKeyEventOnTable%>];
                    jqVar("#PurchaseExpensesGridContainer").dxDataGrid({
                        columnFixing: {
                            enabled: true
                        },
                        //repaintChangesOnly: true,
                        columnAutoWidth: true,
                        showBorders: true,
                        showRowLines: true,
                        dataSource: DevExpress.data.AspNet.createStore({
                            key: "IdCol",
                            loadUrl: "../../api/PurchaseExpense/LoadPurchaseExpenseList",
                            loadMethod: "get"
                        }),
                        allowColumnReordering: <%=this.MyContext.PageData.AllowReorderGrid.ToString().ToLower()%>,
                        columns:
                            [
                                {
                                    caption: "#",
                                    allowEditing: false,
                                    width: 50
                                },
                                {
                                    dataField: "OperationDateCol",
                                    caption: "<%=Resources.Labels.Date%>",
                                    allowEditing: false,
                                    dataType: "date",
                                    format: "dd/MM/yyyy",
                                    alignment: "center",
                                    width: 100
                                },
                                {
                                    dataField: "ExpenseTypeIdCol",
                                    caption: "<%=Resources.Labels.Type%>",
                                    dataType: "number",
                                    lookup: {
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "Id",
                                            loadUrl: "../../api/General/GetExpenseType",
                                            loadMethod: "get",
                                        }),
                                        allowClearing: true,
                                        valueExpr: "Id",
                                        displayExpr: "Name"
                                    },
                                    allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                },
                                {
                                    dataField: "ExpenseNameIdCol",
                                    caption: "<%=Resources.Labels.Name%>",
                                    dataType: "number",
                                    lookup: {
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "Id",
                                            loadUrl: "../../api/General/GetGeneralAtt",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': '<%=this.GetExpenseNameContextKey()%>' },
                                        }),
                                        allowClearing: true,
                                        valueExpr: "ID",
                                        displayExpr: "Name"
                                    },
                                    allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                },
                                {
                                    dataField: "AmountCol",
                                    caption: "<%=Resources.Labels.Amount%>",
                                    allowEditing: false,
                                    dataType: "number",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 100
                                },
                                {
                                    dataField: "CurrencyIdCol",
                                    caption: "<%=Resources.Labels.Currency%>",
                                    dataType: "number",
                                    lookup: {
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "Id",
                                            loadUrl: "../../api/General/GetCurrencies",
                                            loadMethod: "get",
                                        }),
                                        allowClearing: true,
                                        valueExpr: "ID",
                                        displayExpr: "Name"
                                    },
                                    allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                },
                                {
                                    dataField: "RatioCol",
                                    caption: "<%=Resources.Labels.Ratio%>",
                                    dataType: "number",
                                    allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                },
                                {
                                    dataField: "AccountIdCol",
                                    caption: "<%=Resources.Labels.ExpenseAccount%>",
                                    allowEditing: false,
                                    width: 100,
                                    dataType: "string",
                                    validationRules: [
                                        {
                                            type: "required",
                                            ignoreEmptyValue: false,
                                        },
                                        {
                                            type: "async",
                                            message: "<%=Resources.Messages.MsgAccountRequired%>",
                                            ignoreEmptyValue: true,
                                            validationCallback: function (params) {
                                                const d = jqVar.Deferred();
                                                if (params.value == null)
                                                    d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                else {
                                                    jqVar.getJSON("../../api/ChartOfAccount/FindById", { Source: params.value },
                                                        function (response) {
                                                            if (response != null && response.length > 0) {
                                                                d.resolve(response);
                                                            }
                                                            else {
                                                                d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                            }
                                                        }
                                                    );
                                                }
                                                return d.promise();
                                            }
                                        }
                                    ],
                                    alignment: "center",
                                    visible: <%=this.MyContext.JournalEntryOptions.ShowAccountIdByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "AccountNameCol",
                                    caption: "<%=Resources.Labels.ExpenseAccount%>",
                                    allowEditing: false,
                                    dataType: "string",
                                    validationRules: [
                                        {
                                            type: "required",
                                            ignoreEmptyValue: false,
                                        },
                                        {
                                            type: "async",
                                            message: "<%=Resources.Messages.MsgAccountRequired%>",
                                            ignoreEmptyValue: false,
                                            validationCallback: function (params) {
                                                const d = jqVar.Deferred();
                                                if (params.value == null)
                                                    d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                else {
                                                    jqVar.getJSON("../../api/ChartOfAccount/FindByName", { Source: params.value },
                                                        function (response) {
                                                            if (response != null && response.length > 0) {
                                                                d.resolve(response);
                                                            }
                                                            else {
                                                                d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                            }
                                                        }
                                                    );
                                                }
                                                return d.promise();
                                            }
                                        }
                                    ],
                                    lookup: {
                                        dataSource: {
                                            store: {
                                                type: 'array',
                                                data: allPurchaseExpensesAccounts,
                                                key: "ID"//CachedNumber
                                            },
                                            pageSize: 5,
                                            paginate: true
                                        },
                                        allowClearing: true,
                                        valueExpr: "Name",
                                        displayExpr: "Name"
                                    },
                                    alignment: "center",
                                    width: 200
                                },
                                {
                                    dataField: "OppositeAccountIdCol",
                                    caption: "<%=Resources.Labels.OppositeAccount%>",
                                    allowEditing: false,
                                    dataType: "number",
                                    validationRules: [
                                        {
                                            type: "required",
                                            ignoreEmptyValue: false,
                                        },
                                        {
                                            type: "async",
                                            message: "<%=Resources.Messages.MsgAccountRequired%>",
                                            ignoreEmptyValue: false,
                                            validationCallback: function (params) {
                                                const d = jqVar.Deferred();
                                                if (params.value == null)
                                                    d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                else {
                                                    jqVar.getJSON("../../api/ChartOfAccount/FindById", { Source: params.value },
                                                        function (response) {
                                                            if (response != null && response.length > 0) {
                                                                d.resolve(response);
                                                            }
                                                            else {
                                                                d.reject("<%=Resources.Messages.MsgAccountRequired%>");
                                                            }
                                                        }
                                                    );
                                                }
                                                return d.promise();
                                            }
                                        }
                                    ],
                                    lookup: {
                                        dataSource: {
                                            store: {
                                                type: 'array',
                                                data: allPurchaseExpensesAccounts,
                                                key: "ID"//CachedNumber
                                            },
                                            pageSize: 5,
                                            paginate: true
                                        },
                                        allowClearing: true,
                                        valueExpr: "ID",
                                        displayExpr: "Name"
                                    },
                                    alignment: "center",
                                    width: 200
                                },
                                {
                                    dataField: "DocStatusIdCol",
                                    caption: "<%=Resources.Labels.Status%>",
                                    allowEditing: false,
                                    dataType: "number",
                                    lookup: {
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "Id",
                                            loadUrl: "../../api/General/GetDocumentStatusList",
                                            loadMethod: "get",
                                        }),
                                        allowClearing: true,
                                        valueExpr: "Id",
                                        displayExpr: "Name"
                                    },
                                    value: -1,
                                    alignment: "center",
                                    width: 200
                                },
                                {
                                    dataField: "NoteCol",
                                    caption: '<%=Resources.Labels.DgNotesHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    alignment: "right",
                                    minWidth: 150,
                                    visible: <%=this.MyContext.JournalEntryOptions.UseNotesByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "DocNameCol",
                                    caption: "<%=Resources.Labels.DgDocNameHeader%>",
                                    allowEditing: false,
                                    dataType: "string",
                                    alignment: "center",
                                    width: 100,
                                    visible: <%=this.MyContext.JournalEntryOptions.UseDocNameByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "OperDateCol",
                                    caption: "<%=Resources.Labels.DgOperDateHeader%>",
                                    allowEditing: false,
                                    dataType: "date",
                                    format: "dd/MM/yyyy",
                                    alignment: "center",
                                    width: 100,
                                    visible: <%=this.MyContext.JournalEntryOptions.UseDocDateByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "IsTaxFoundCol",
                                    caption: '<%=Resources.Labels.DgTaxHeader%>',
                                    allowEditing: true,
                                    dataType: "boolean",
                                    alignment: "center",
                                    width: 60,
                                    visible: <%=this.MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "AmountTaxCol",
                                    caption: "<%=Resources.Labels.AmountTax%>",
                                    allowEditing: false,
                                    dataType: "number",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 100
                                },
                                {
                                    dataField: "IdCol",
                                    caption: '<%=Resources.Labels.DgTranIdHeader%>',
                                    allowEditing: false,
                                    visible: false
                                },
                                {
                                    fixed: true,
                                    fixedPosition: '<%=Resources.Labels.CssLeft%>',
                                    width: 80,
                                    alignment: 'center',
                                    type: "buttons",
                                    buttons: [{
                                        cssClass: 'dx-link dx-link-delete dx-icon-trash dx-link-icon',
                                        onClick: function (e) {
                                            console.log(e);
                                            jqVar('#PurchaseExpensesGridContainer').dxDataGrid('instance').deleteRow(e.row.rowIndex);
                                            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                                            grid.saveEditData();
                                        }
                                    }]
                                }
                            ],
                        remoteOperations: false,
                        onInitNewRow: function (e) { e.data.IsTaxFoundCol = false; },
                        keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: ekd /* "row"*/, editOnKeyPress: true },
                        onEditorPreparing: function OnEditorPreparing(e) {
                            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                            var component = e.component,
                                rowIndex = e.row && e.row.rowIndex;

                            if (e.dataField == "AccountIdCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        jqVar.getJSON("../../api/ChartOfAccount/FindById", { Source: e.value },
                                            function (response) {
                                                if (response != null) {
                                                    component.cellValue(rowIndex, "AccountIdCol", response[0].ID);//CachedNumber
                                                    component.cellValue(rowIndex, "AccountNameCol", response[0].Name);
                                                }
                                                else {
                                                    var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                                                    component.cellValue(rowIndex, "AccountIdCol", null);
                                                    component.cellValue(rowIndex, "AccountNameCol", null);
                                                    grid.focus(grid.getCellElement(rowIndex, "AccountNameCol"));
                                                }
                                            });
                                    }
                                    else {
                                        component.cellValue(rowIndex, "AccountIdCol", null);
                                        component.cellValue(rowIndex, "AccountNameCol", null);
                                    }
                                }
                            }
                            else if (e.dataField == "AccountNameCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        jqVar.getJSON("../../api/ChartOfAccount/FindByName", { Source: e.value },
                                            function (response) {
                                                if (response != null) {
                                                    component.cellValue(rowIndex, "AccountIdCol", response[0].ID);//CachedNumber
                                                    component.cellValue(rowIndex, "AccountNameCol", response[0].Name);
                                                }
                                                else {
                                                    var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                                                    component.cellValue(rowIndex, "AccountIdCol", null);
                                                    component.cellValue(rowIndex, "AccountNameCol", null);
                                                    grid.focus(grid.getCellElement(rowIndex, "AccountNameCol"));
                                                }
                                            });
                                    }
                                    else {
                                        component.cellValue(rowIndex, "AccountIdCol", null);
                                        component.cellValue(rowIndex, "AccountNameCol", null);
                                    }
                                }
                            }
                        },
                        onToolbarPreparing: function (e) {
                            e.toolbarOptions.visible = false;
                        },
                        editing: {
                            mode: "batch",
                            allowAdding: true,
                            //allowDeleting: true,
                            allowUpdating: true,
                            selectTextOnEditStart: true,
                            startEditAction: "click",
                            useIcons: true
                        },
                        height: 350,
                        /*height: function () {
                            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                                return 450;
                            }
                            else {
                                var PurchaseExpensesToolsPnl = jqVar("#PurchaseExpensesToolsPnl").height();
                                var btnRow = jqVar("#PurchaseExpensesBtnsRow").height();
                                return window.innerHeight - ((PurchaseExpensesToolsPnl + btnRow) - 30);
                            }
                        },*/
                        sorting: { mode: "none" },
                        customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                        onKeyDown: function (e) {
                            var keyCode = e.event.which;
                            var component = e.component;
                            if (keyCode == 13) {
                                var validateResult = ValidatePurchaseExpensesRows(false);
                                var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
                                var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                                if (currentPurchaseExpensesRowIndex == rows.length - 1 && validateResult.responseCode)
                                    AddRowBottomPurchaseExpenses(true);
                                else showErrorMessage(validateResult.responseMessage, null);
                            } else if (keyCode == 46 && currentPurchaseExpensesRowIndex >= 0) {
                                var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                                const focusedCellPosition = getPurchaseExpensesCurrentCell(grid);
                                grid.deleteRow(focusedCellPosition.rowIndex);
                            }
                        },
                        summary: {
                            totalItems: [{ column: "AccountNameCol", displayFormat: "<%=Resources.Labels.Total%>" },
                            {
                                column: "DebitAmountCol",
                                summaryType: "sum",
                                displayFormat: "{0}",
                                valueFormat: "decimal",
                                name: "TotalDebitAmountSum"
                            },
                            {
                                column: "CreditAmountCol",
                                summaryType: "sum",
                                displayFormat: "{0}",
                                valueFormat: "decimal",
                                name: "TotalCreditAmountSum"
                            }
                            ],
                            recalculateWhileEditing: true
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                        focusedRowEnabled: true,
                        onFocusedRowChanging: function OnFocusedRowChanging(e) {
                            currentPurchaseExpensesRowIndex = e.newRowIndex;
                        }
                    });
                    clearPurchaseExpensesDgv();                   
                    setTimeout(function () {
                        documentSelected(1,<%=Request.QueryString["PurchaseId"]%>);                        
                    }, 1000);
                }, 100);
                /*******************************************/                
            }).done(function () {
                setEnabled(".executeBtn", false);
                setEnabled("#PurchaseExpensesBtnSave", true);
                setEnabled("#PurchaseExpensesBtnReset", true);
                setTimeout(function () {                    
                    jqVar.getJSON('../../api/GridOrdering/FindGridOrdering', { 'GridId': '<%=DocumentKindClass.JournalEntry%>', 'UserId': '<%=MyContext.UserProfile.Contact_ID%>' }, function (response) {
                        if (response.length == 1) {
                            var array = JSON.parse(response[0].JsonVal);
                            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                            var columns = grid.option('columns');
                            for (var i = 0; i < array.length; i++) {
                                var visibleIndex = array[i].index;
                                var fieldName = array[i].fieldName;

                                for (var j = 0; j < columns.length; j++) {
                                    if (columns[j].dataField == fieldName) {
                                        grid.columnOption(j, "visibleIndex", visibleIndex);
                                    }
                                }
                            }
                        }
                    });
                }, 3000);
            });
        });

        function clearPurchaseExpensesDgv() {
            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
            var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }

            for (var i = 0; i < 7; i++) {
                grid.addRow();
            }
        }

        getPurchaseExpensesCurrentCell = function (dataGrid) {
            return (dataGrid)._controllers.keyboardNavigation._focusedCellPosition;
        }

        function ValidatePurchaseExpensesRows(skipEmptyRows) {
            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
            var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1;
            
            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].data["OperationDateCol"] == null || rows[i].data["OperationDateCol"] == '') &&
                        (rows[i].data["ExpenseTypeIdCol"] == null || rows[i].data["ExpenseTypeIdCol"] == '') &&
                        (rows[i].data["ExpenseNameIdCol"] == null || rows[i].data["ExpenseNameIdCol"] == '') &&
                        (rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') &&
                        (rows[i].data["CurrencyIdCol"] == null || rows[i].data["CurrencyIdCol"] == '') &&
                        (rows[i].data["RatioCol"] == null || rows[i].data["RatioCol"] == '') &&
                        (rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') &&
                        (rows[i].data["AccountNameCol"] == null || rows[i].data["AccountNameCol"] == '') &&
                        (rows[i].data["OppositeAccountIdCol"] == null || rows[i].data["OppositeAccountIdCol"] == '') &&
                        (rows[i].data["DocStatusIdCol"] == null || rows[i].data["DocStatusIdCol"] == '') &&
                        (rows[i].data["NoteCol"] == null || rows[i].data["NoteCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["IsTaxFoundCol"] == null || rows[i].data["IsTaxFoundCol"] == '') &&
                        (rows[i].data["AmountTaxCol"] == null || rows[i].data["AmountTaxCol"] == '') &&                       
                        (rows[i].data["IdCol"] == null || rows[i].data["IdCol"] == '')
                    )
                        isEmptyRow = true;
                }

                if (rows[i].rowType == "data" && (skipEmptyRows == false || isEmptyRow == false)) {
                    if (rows[i].data["OperationDateCol"] == null || rows[i].data["OperationDateCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocOperationDateRequired%>";
                        cellName = "OperationDateCol";
                    }
                    else if (rows[i].data["ExpenseTypeIdCol"] == null || rows[i].data["ExpenseTypeIdCol"] == '') {
                        hasError = true;
                        errorMessage = "001";
                        cellName = "ExpenseTypeIdCol";
                    }
                    else if (rows[i].data["ExpenseNameIdCol"] == null || rows[i].data["ExpenseNameIdCol"] == '') {
                        hasError = true;
                        errorMessage = "002";
                        cellName = "ExpenseNameIdCol";
                    }
                    else if (rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') {
                        hasError = true;
                        errorMessage = "003";
                        cellName = "AmountCol";
                    }
                    else if (rows[i].data["CurrencyIdCol"] == null || rows[i].data["CurrencyIdCol"] == '') {
                        hasError = true;
                        errorMessage = "004";
                        cellName = "CurrencyIdCol";
                    }
                    else if (rows[i].data["RatioCol"] == null || rows[i].data["RatioCol"] == '') {
                        hasError = true;
                        errorMessage = "005";
                        cellName = "RatioCol";
                    }
                    else if (rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgAccountRequired%>";
                        cellName = "AccountNameCol";
                    }
                    else if (rows[i].data["OppositeAccountIdCol"] == null || rows[i].data["OppositeAccountIdCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgAccountRequired%>";
                        cellName = "OppositeAccountIdCol";
                    }
                    else if (rows[i].data["DocStatusIdCol"] == null || rows[i].data["DocStatusIdCol"] == '' ||
                        rows[i].data["DocStatusIdCol"] == -1 || rows[i].data["DocStatusIdCol"] == '-1') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgAccountRequired%>";
                        cellName = "DocStatusIdCol";
                    }
                    else if (<%=MyContext.JournalEntryOptions.UseDocNameByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceDocNameByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '')) {
                        hasError = true;                        
                        errorMessage = "<%=Resources.Messages.MsgDocNameRequired%>";
                        cellName = "DocNameCol";
                    }
                    else if (<%=MyContext.JournalEntryOptions.UseDocDateByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceDocDateByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocDateRequired%>";
                        cellName = "OperDateCol";
                    }                                        
                    else hasError = false;
                }
                else if (isEmptyRow == true)
                    hasError = false;

                if (hasError == true) {
                    rowIndex = i;
                    break;
                }
            }

            /*if (hasError == true) {
                return { responseCode: false, 'rowIndex': rowIndex, responseMessage: errorMessage, 'cellName': cellName };
            }
            else return { responseCode: true };*/

            if (hasError == true) {
                jqVar("#PurchaseExpensesNotifyview").html(errorMessage);
                jqVar("#PurchaseExpensesNotifymsgdiv").removeClass("alert-success");
                jqVar("#PurchaseExpensesNotifymsgdiv").addClass("alert-danger");
                jqVar("#PurchaseExpensesNotifymsgdiv").fadeIn();
                showModal("#PurchaseExpensesNotifyModal");
                jqVar("#PurchaseExpensesNotifymsgdiv").fadeOut(3000, function () {
                    hideModal("#PurchaseExpensesNotifyModal");
                    if (rowIndex != -1 && cellName != null && cellName != '')
                        grid.focus(grid.getCellElement(rowIndex, cellName));
                })
                if (rowIndex != -1 && cellName != null && cellName != '')
                    grid.focus(grid.getCellElement(rowIndex, cellName));
                return false;
            }
            else return true;
        }

        function AddRowBottomPurchaseExpensesPurchaseExpenses(focusRow) {
            var validateResult = ValidatePurchaseExpensesRows(false);
            if (validateResult == true) {

                var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                var cols = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
                var dataRows = [];

                for (var i = 0; i < rows.length; i++) {
                    dataRows.push(rows[i].data);
                }

                var length = dataRows.length;
                for (var i = dataRows.length - 1; i >= 0; i--) {
                    grid.deleteRow(i);
                }

                for (var i = 0; i < length + 1; i++) {
                    grid.addRow(i);
                }

                rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["OperationDateCol"] = dataRows[i].OperationDateCol;
                    rows[i].data["ExpenseTypeIdCol"] = dataRows[i].ExpenseTypeIdCol;
                    rows[i].data["ExpenseNameIdCol"] = dataRows[i].ExpenseNameIdCol;
                    rows[i].data["AmountCol"] = dataRows[i].AmountCol;
                    rows[i].data["CurrencyIdCol"] = dataRows[i].CurrencyIdCol;
                    rows[i].data["RatioCol"] = dataRows[i].RatioCol;
                    rows[i].data["AccountIdCol"] = dataRows[i].AccountIdCol;
                    rows[i].data["AccountNameCol"] = dataRows[i].AccountNameCol;
                    rows[i].data["OppositeAccountIdCol"] = dataRows[i].OppositeAccountIdCol;
                    rows[i].data["DocStatusIdCol"] = dataRows[i].DocStatusIdCol;
                    rows[i].data["NoteCol"] = dataRows[i].NoteCol;
                    rows[i].data["DocNameCol"] = dataRows[i].DocNameCol;
                    rows[i].data["OperDateCol"] = dataRows[i].OperDateCol;
                    rows[i].data["IsTaxFoundCol"] = dataRows[i].IsTaxFoundCol;
                    rows[i].data["AmountTaxCol"] = dataRows[i].AmountTaxCol;
                    rows[i].data["IdCol"] = dataRows[i].IdCol;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "OperationDateCol"));
                SetPurchaseExpensesDataGridEditable(true);
            }            
        }        
    </script>

    <!-- Bill Scripts -->
    <script type="text/javascript">
        function clearPurchaseExpensesTools() {
            clearPurchaseExpensesDgv();
            jqVar("#PurchaseExpensesId").val(null);
            jqVar("#DocRandomString").val(null);
        }

        function SetPurchaseExpensesDataGridEditable(value) {
            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");            
            var columns = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleColumns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].allowEditing = value;
                if (columns[i].name == "RowNoCol")
                    columns[i].allowEditing = false;
            }
        }

        function addPurchaseExpensesItem() {
            //clearPurchaseExpensesTools();
            setEnabled(".executeBtn", false);
            setEnabled("#PurchaseExpensesBtnSave", true);
            setEnabled("#PurchaseExpensesBtnReset", true);
            jqVar("#PurchaseExpensesOperation").val("Add");
            SetPurchaseExpensesDataGridEditable(true);            
        }

        function documentSelected(requestCode, purchaseId) {
            if (requestCode == 1) {
                clearPurchaseExpensesTools();
                jqVar.getJSON("../../api/PurchaseExpense/GetPurchaseExpenseDetails", { 'PurchaseId': purchaseId },
                    function (responseTable) {
                        var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");
                        //var cols = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleColumns();
                        var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();

                        var length = rows.length;
                        for (var i = length - 1; i >= 0; i--) {
                            grid.deleteRow(i);
                        }

                        length = responseTable.length;
                        for (var i = 0; i < length + 1; i++) {
                            grid.addRow(i);
                        }

                        rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
                        console.log(responseTable);
                        for (var i = 0; i < responseTable.length; i++) {
                            rows[i].data["OperationDateCol"] = responseTable[i].OperationDate;
                            rows[i].data["ExpenseTypeIdCol"] = responseTable[i].ExpenseType;
                            rows[i].data["ExpenseNameIdCol"] = responseTable[i].ExpenseName_ID;
                            rows[i].data["AmountCol"] = responseTable[i].Amount;
                            rows[i].data["CurrencyIdCol"] = responseTable[i].Currency_ID;
                            rows[i].data["RatioCol"] = responseTable[i].Ratio;
                            rows[i].data["AccountIdCol"] = responseTable[i].Account_ID;
                            rows[i].data["AccountNameCol"] = responseTable[i].AccountName;
                            rows[i].data["OppositeAccountIdCol"] = responseTable[i].OppositeAccount_ID;
                            rows[i].data["DocStatusIdCol"] = responseTable[i].DocStatus_ID;
                            rows[i].data["NoteCol"] = responseTable[i].Notes;
                            rows[i].data["DocNameCol"] = responseTable[i].DocName;
                            rows[i].data["OperDateCol"] = responseTable[i].OperDateString;//OperDate                                        
                            rows[i].data["IsTaxFoundCol"] = responseTable[i].IsTaxFound;
                            rows[i].data["AmountTaxCol"] = responseTable[i].AmountTax;
                            rows[i].data["IdCol"] = responseTable[i].ID;
                        }
                        //grid.refresh();
                        SetPurchaseExpensesDataGridEditable(true);
                        editPurchaseExpensesItem();
                    });
            }
        }

        function editPurchaseExpensesItem() {
            setEnabled(".executeBtn", false);
            setEnabled("#PurchaseExpensesBtnSave", true);
            setEnabled("#PurchaseExpensesBtnReset", true);            
            jqVar("#PurchaseExpensesOperation").val("Edit");
            SetPurchaseExpensesDataGridEditable(true);
        }        

        function savePurchaseExpensesItem(isApproving) {
            var validateResult = ValidatePurchaseExpensesRows(true);            
            var grid = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance");

            if (validateResult == false) {                
                return false;
            }
            else {                
                var operation = jqVar("#PurchaseExpensesOperation").val();
                var debitAmount = null, creditAmount = null;
                var editMode = false;
                if (operation == "Edit")
                    editMode = true;
                
                var rows = jqVar("#PurchaseExpensesGridContainer").dxDataGrid("instance").getVisibleRows();
                var dataRows = [];

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].data["ExpenseTypeIdCol"] == null || rows[i].data["ExpenseTypeIdCol"] == '') &&
                        (rows[i].data["OperationDateCol"] == null || rows[i].data["OperationDateCol"] == '') &&
                        (rows[i].data["ExpenseNameIdCol"] == null || rows[i].data["ExpenseNameIdCol"] == '') &&
                        (rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') &&
                        (rows[i].data["CurrencyIdCol"] == null || rows[i].data["CurrencyIdCol"] == '') &&
                        (rows[i].data["RatioCol"] == null || rows[i].data["RatioCol"] == '') &&
                        (rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') &&
                        (rows[i].data["AccountNameCol"] == null || rows[i].data["AccountNameCol"] == '') &&
                        (rows[i].data["OppositeAccountIdCol"] == null || rows[i].data["OppositeAccountIdCol"] == '') &&
                        (rows[i].data["DocStatusIdCol"] == null || rows[i].data["DocStatusIdCol"] == '') &&
                        (rows[i].data["NoteCol"] == null || rows[i].data["NoteCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["IsTaxFoundCol"] == null || rows[i].data["IsTaxFoundCol"] == '') &&
                        (rows[i].data["AmountTaxCol"] == null || rows[i].data["AmountTaxCol"] == '') &&
                        (rows[i].data["IdCol"] == null || rows[i].data["IdCol"] == '')
                    )
                        isEmptyRow = true;

                    if (rows[i].rowType == "data" && isEmptyRow == false) {
                        var expenseTypeId = null, operationDate = null, expenseNameId = null, amount = null, currencyId = null, ratio = null,
                            accountId = null, oppositeAccountId = null, docStatusId = null, note = null, docName = null, operDate = null,
                            isTaxFound = null, amountTax = null, id = null;

                        if (rows[i].data["ExpenseTypeIdCol"] != null)
                            expenseTypeId = rows[i].data["ExpenseTypeIdCol"];
                        if (rows[i].data["OperationDateCol"] != null)
                            operationDate = rows[i].data["OperationDateCol"];
                        if (rows[i].data["ExpenseNameIdCol"] != null)
                            expenseNameId = rows[i].data["ExpenseNameIdCol"];
                        if (rows[i].data["AmountCol"] != null)
                            amount = rows[i].data["AmountCol"];
                        if (rows[i].data["CurrencyIdCol"] != null)
                            currencyId = rows[i].data["CurrencyIdCol"];
                        if (rows[i].data["RatioCol"] != null)
                            ratio = rows[i].data["RatioCol"];
                        if (rows[i].data["AccountIdCol"] != null)
                            accountId = rows[i].data["AccountIdCol"];
                        if (rows[i].data["OppositeAccountIdCol"] != null)
                            oppositeAccountId = rows[i].data["OppositeAccountIdCol"];
                        if (rows[i].data["DocStatusIdCol"] != null)
                            docStatusId = rows[i].data["DocStatusIdCol"];
                        if (rows[i].data["NoteCol"] != null)
                            note = rows[i].data["NoteCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocNameByRecord.ToString().ToLower()%>== true && rows[i].data["DocNameCol"] != null)
                            docName = rows[i].data["DocNameCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocDateByRecord.ToString().ToLower()%>== true && rows[i].data["OperDateCol"] != null)
                            operDate = rows[i].data["OperDateCol"];
                        if (<%=MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>== true) {
                            if (rows[i].data["IsTaxFoundCol"] != null)
                                isTaxFound = rows[i].data["IsTaxFoundCol"];
                            if (rows[i].data["AmountTaxCol"] != null)
                                amountTax = rows[i].data["AmountTaxCol"];
                        }                        
                        id = rows[i].data["IdCol"];

                        var row = {
                            'ID': id, 'OperationDate': operationDate, 'CurrencyId': currencyId, 'Ratio': ratio, //'IsApproving': isApproving,
                            'ExpenseTypeId': expenseTypeId, 'ExpenseNameId': expenseNameId, 'Amount': amount, 'OppositeAccountId': oppositeAccountId, 'Note': note,
                            'DocStatusId': docStatusId,'AmountTax': amountTax, 'AccountId': accountId, 'IsTaxFound': isTaxFound, 'DocName': docName, 'OperDate': operDate
                        };
                        dataRows.push(row);
                        //dataRows.push(rows[i].data);
                    }
                }

                dataRows = JSON.stringify(dataRows);
                var id = jqVar("#PurchaseExpensesId").val();
                jqVar.post("../../api/PurchaseExpense/SavePurchaseExpense", {
                    'ReceiptId': <%=Request.QueryString["PurchaseId"]%>, 'Source': dataRows
                },
                    function (response) {
                        if (response.Code > 0) {
                            clearPurchaseExpensesTools();
                            //jqVar("#DocRandomString").val(response[0].DocRandomString)
                            jqVar("#PurchaseExpensesOperation").val(null);
                            setReadOnly("#PurchaseExpensesToolsPnl", true);
                            setEnabled(".executeBtn", false);

                            showSuccessPrint("<%=Resources.Messages.MsgDocOperationSucessPrint%>", "<%=Resources.Messages.MsgDocSaveSuccess%>", response.Code);
                        }
                        else if (response.Code == -101)
                            showErrorMessage(response.Message);
                    });
            }
        }

        function resetPurchaseExpensesItem() {
            setEnabled(".executeBtn", false);
            clearPurchaseExpensesTools();
            setEnabled("#PurchaseExpensesBtnReset", true);
        }
    </script>

    <style type="text/css">
        * {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: normal;
        }

        /*.dx-datagrid-text-content {
            font-size: 12px;
            font-weight: normal;
        }*/
        .executeBtn{
            font-weight: bold;
        }

        .MainInvoiceStyleDiv {
            zoom: 80%;
            min-width: 0px;
        }

        /*.modal-dialog {
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0;
        }

        .modal-content {
            height: auto;
            min-height: 100%;
            border-radius: 0;
        }*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" Runat="Server">
    <div class="MainInvoiceStyleDiv">
            <input type="hidden" id="PurchaseExpensesOperation" />
            <input type="hidden" id="PurchaseExpensesId" />
            <input type="hidden" id="PurchaseExpensesTypeTax" value="<%= this.TypeTax.ToString() %>" />
            <input type="hidden" id="DocRandomString" />
            <input type="hidden" id="PurchaseExpensesEntryType" value="1" />
            <input type="hidden" id="PurchaseExpensesSourceTypeId" value="<%=DocumentKindClass.JournalEntry %>" />

            <!-- Notify Modal-->
            <div class="modal fade" id="PurchaseExpensesNotifyModal" tabindex="-1" role="dialog" aria-labelledby="PurchaseExpensesNotifyModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-bs-dismiss="modal" onclick="hideModal('#PurchaseExpensesNotifyModal')">x</button>
                        </div>
                        <div class="modal-body">
                            <div class="alert col-md-12" id="PurchaseExpensesNotifymsgdiv" style="display: none;">
                                <span id="PurchaseExpensesNotifyview"></span>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-success btn-lg" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <!-- حقول رأس الفاتور -->
                <div class="dx-viewport">
                    <div class="row" style="margin-left: 20px; margin-right: 20px;">
                        <div class="col-md-12">
                            <div id="PurchaseExpensesToolsPnl" class="row">
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:DropDownList ID="PurchaseExpensesddlCurrency" Style="display: none" runat="server"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <label runat="server" id="lblCurrencyPurchaseExpenses" style="display: none"><%=Resources.Labels.Currency %></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="row">
                        <div class="dxGridParent">
                            <div id="PurchaseExpensesGridContainer" class="dxGrid col-md-12"></div>
                        </div>
                    </div>

                    <div id="PurchaseExpensesBtnsRow" class="row" style="margin-right: 2px; padding-left: 15px;">
                        <div class="col-md-8" style="text-align: center">
                            <button type="button" id="PurchaseExpensesBtnSave" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="savePurchaseExpensesItem(false)"><%=Resources.Labels.Save %> <i class="fa fa-save" aria-hidden="true"></i></button>
                        </div>

                        <button type="button" id="PurchaseExpensesBtnReset" class="executeBtn btn bot-buffer btn-sm col-md-2" onclick="resetPurchaseExpensesItem()"><%=Resources.Labels.Clear %><i class="fa fa-undo" aria-hidden="true"></i></button>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

