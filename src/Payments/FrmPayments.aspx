<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="FrmPayments.aspx.cs" Inherits="Payments_FrmPayments" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigationJS.ascx" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/DevextremeStyle.css" rel="stylesheet" />
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>
    <!-- DevExtreme theme -->
    <link href="../Content/devextreme/dx.light.css" rel="stylesheet" />

    <!-- DevExtreme library -->
    <script type="text/javascript" src="../Content/devextreme/dx.all.js"></script>
    <%--<script type="text/javascript" src="../Content/devextreme/dx.web.js"></script>
    <script type="text/javascript" src="../Content/devextreme/dx.viz.js"></script>--%>
    <script type="text/javascript" src="../Content/devextreme/dx.aspnet.data.js"></script>

    <script type="text/javascript">
        var obj = <%=Convert.ToByte(this.MyContext.CurrentCulture)%>;
        console.log("obj:" + obj);
        if (obj == 0 || obj == "0") {
            if (!IsMobile())
                document.write('<link href="../Styles/DevextremeStyleRTL.css" rel="stylesheet" />');
            document.write('<link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />');
        }
    </script>

    <!-- DataGrid & DevExtreme Scripts -->
    <script type="text/javascript">
        var currentRowIndex = 0;
        var creditAccounts = [];
        var debitAccounts = [];

        var jqVar = jQuery.noConflict();
        jqVar(document).ready(function () {
            jqVar("#imgStatusDiv").css("background-image", getImgStatus(0));
            loadDevextremeLocalization();

            jqVar("#lblPageTitle").text('<%= Request.QueryString["Title"] %>');
            jqVar("#MainTitle").hide();
            jqVar("#pageTitleLbl").text(jqVar("#lblPageTitle").text());
            jqVar.getJSON("../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': '<%=this.GetDebitAccountContextKey()%>' }, function (exResponse) {
                for (i = 0; i < exResponse.length; i++) {
                    debitAccounts.push(exResponse[i]);
                }

                jqVar.getJSON("../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': '<%=this.GetCreditAccountContextKey()%>' }, function (exResponse) {
                    for (i = 0; i < exResponse.length; i++) {
                        creditAccounts.push(exResponse[i]);
                    }

                    jqVar.getJSON("../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                        jqVar("#acBranch").dxSelectBox({
                            dataSource: branchesResponse,
                            displayExpr: "Name",
                            valueExpr: "ID",
                            searchEnabled: true,
                            placeholder: '<%=Resources.Labels.Branch%>',
                            onValueChanged: function (data) {
                                jqVar("#BranchIdHidden").val(data);
                                jqVar.getJSON("../api/CostCenter/GetCostCenters", { 'contextKey': '<%=this.GetCostCenterContextKey()%>' + jqVar("#acBranch").dxSelectBox('instance').option('value') }, function (costCentersResponse) {
                                    var selectedCostCenterId = null;
                                    if (<%=this.PaymentOptions.KeepCostCenter.ToString().ToLower()%>== true && costCentersResponse.length > 0)
                                        selectedCostCenterId = costCentersResponse[0].ID;
                                    jqVar("#acCostCenter").dxSelectBox({
                                        dataSource: costCentersResponse,
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        value: selectedCostCenterId,
                                        searchEnabled: true,
                                        placeholder: '<%=Resources.Labels.CostCenter%>',
                                        onValueChanged: function (data) {

                                        },
                                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                                    }).dxSelectBox("instance")
                                });
                            },
                            rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                        }).dxSelectBox("instance");

                        setTimeout(function () {
                            var ekd = ekdArray[<%=this.PaymentOptions.EnterKeyEventOnTable%>];
                            jqVar("#paymentGridContainer").dxDataGrid({
                                columnFixing: {
                                    enabled: true
                                },
                                //repaintChangesOnly: true,
                                columnAutoWidth: true,
                                showBorders: true,
                                showRowLines: true,
                                dataSource: DevExpress.data.AspNet.createStore({
                                    key: "IdCol",
                                    loadUrl: "../api/Payment/LoadPaymentList",
                                    loadMethod: "get"
                                }),
                                allowColumnReordering: <%=this.MyContext.PageData.AllowReorderGrid.ToString().ToLower()%>,
                                columns:
                                    [
                                        {
                                            dataField: "RowIndexCol",
                                            caption: "#",
                                            allowEditing: false,
                                            width: 50
                                        },
                                        {
                                            dataField: "DebitAccountNameCol",
                                            caption: "<%=Resources.Labels.DgDebitAccountNameHeader%>",
                                            allowEditing: false,
                                            dataType: "string",
                                            validationRules: [
                                                {
                                                    type: "required",
                                                    ignoreEmptyValue: false,
                                                },
                                                {
                                                    type: "async",
                                                    message: "<%=Resources.Messages.MsgDebitAccountRequired%>",
                                                    ignoreEmptyValue: false,
                                                    validationCallback: function (params) {
                                                        const d = jqVar.Deferred();
                                                        if (params.value == null)
                                                            d.reject("<%=Resources.Messages.MsgDebitAccountRequired%>");
                                                        else {
                                                            d.resolve(params.value);
                                                        }
                                                        return d.promise();
                                                    }
                                                }
                                            ],
                                            lookup: {
                                                dataSource: {
                                                    store: {
                                                        type: 'array',
                                                        data: debitAccounts,
                                                        key: "ID"
                                                    },
                                                    pageSize: 5,
                                                    paginate: true
                                                },
                                                allowClearing: true,
                                                valueExpr: "ID",
                                                displayExpr: "Name"
                                            },
                                            //allowEditing: false,
                                            alignment: "center",
                                            width: 200
                                          
                                        },
                                        {
                                            dataField: "CreditAccountNameCol",
                                            caption: "<%=Resources.Labels.DgCreditAccountNameHeader%>",
                                            allowEditing: false,
                                            dataType: "string",
                                            validationRules: [
                                                {
                                                    type: "required",
                                                    ignoreEmptyValue: false,
                                                },
                                                {
                                                    type: "async",
                                                    message: "<%=Resources.Messages.MsgCreditAccountRequired%>",
                                                    ignoreEmptyValue: false,
                                                    validationCallback: function (params) {
                                                        const d = jqVar.Deferred();
                                                        if (params.value == null)
                                                            d.reject("<%=Resources.Messages.MsgCreditAccountRequired%>");
                                                        else {
                                                            d.resolve(params.value);
                                                        }
                                                        return d.promise();
                                                    }
                                                }
                                            ],
                                            lookup: {
                                                dataSource: {
                                                    store: {
                                                        type: 'array',
                                                        data: creditAccounts,
                                                        key: "ID"
                                                    },
                                                    pageSize: 5,
                                                    paginate: true
                                                },
                                                allowClearing: true,
                                                valueExpr: "ID",
                                                displayExpr: "Name"
                                            },
                                            //allowEditing: false,
                                            alignment: "center",
                                            width: 200
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
                                            dataField: "SalesRepNameCol",
                                            caption: "<%=Resources.Labels.DgSalesRepNameHeader%>",
                                            allowEditing: false,
                                            dataType: "string",
                                            validationRules: [
                                                {
                                                    type: "async",
                                                    message: "<%=Resources.Messages.MsgDocSalesRepRequired%>",
                                                    ignoreEmptyValue: false,
                                                    validationCallback: function (params) {
                                                        const d = jqVar.Deferred();
                                                        if (params.value == null)
                                                            d.reject("<%=Resources.Messages.MsgDocSalesRepRequired%>");
                                                        else {
                                                            jqVar.getJSON("../api/General/FindSalesRepByName", { Source: params.value },
                                                                function (response) {
                                                                    if (response != null && response.length > 0) {
                                                                        d.resolve(response);
                                                                    }
                                                                    else {
                                                                        d.reject("<%=Resources.Messages.MsgDocSalesRepRequired%>");
                                                                    }
                                                                }
                                                            );
                                                        }
                                                        return d.promise();
                                                    }
                                                }
                                            ],
                                            lookup: {
                                                dataSource: function getUnits(options) {
                                                    var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
                                                    if (rows != null && rows.length > 0 && options.rowIndex > -1 && rows[options.rowIndex].data["CreditAccountIdCol"] != null &&
                                                        rows[options.rowIndex].data["CreditAccountIdCol"] != '') {
                                                        var acSalesRepContextKey = null, CreditAccountId = null;
                                                        if (rows[options.rowIndex].data["CreditAccountIdCol"] != null)
                                                            CreditAccountId = rows[options.rowIndex].data["CreditAccountIdCol"];
                                                        jqVar("#CreditAccountIdHidden").val(CreditAccountId);
                                                        var Contact_ID = '<%=this.GetCustomerContactID() %>';
                                                        if (Contact_ID == null || jqVar("#acBranch").dxSelectBox('instance').option('value') == null)
                                                            acSalesRepContextKey = "0,";
                                                        else acSalesRepContextKey = Contact_ID + ",," + jqVar("#acBranch").dxSelectBox('instance').option('value');

                                                        return {
                                                            store: DevExpress.data.AspNet.createStore({
                                                                key: "Name",
                                                                loadUrl: "../api/General/GetSalesReps",
                                                                loadParams: { 'contextKey': acSalesRepContextKey },
                                                                loadMethod: "get",
                                                                valueExpr: "Name",
                                                                displayExpr: "Name"
                                                            })
                                                        };
                                                    }
                                                    else return {
                                                        store: DevExpress.data.AspNet.createStore({
                                                            key: "Name",
                                                            loadUrl: "../api/General/GetSalesReps",
                                                            loadParams: { 'contextKey': "0," },
                                                            loadMethod: "get",
                                                            valueExpr: "Name",
                                                            displayExpr: "Name"
                                                        })
                                                    };
                                                },
                                                allowClearing: true,
                                                valueExpr: "Name",
                                                displayExpr: "Name"
                                            },
                                            //allowEditing: false,
                                            alignment: "center",
                                            width: 200,
                                            visible: <%=this.PaymentOptions.UseSalesRepByRecored.ToString().ToLower()%>,
                                        },
                                        {
                                            dataField: "CostCenterNameCol",
                                            caption: "<%=Resources.Labels.CostCenter%>",
                                            dataType: "string",
                                            validationRules: [
                                                {
                                                    type: "required",
                                                    ignoreEmptyValue: false,
                                                },
                                                {
                                                    type: "async",
                                                    message: "<%=Resources.Messages.MsgDocCostCenterRequired%>",
                                                    ignoreEmptyValue: false,
                                                    validationCallback: function (params) {
                                                        const d = jqVar.Deferred();
                                                        if (params.value == null)
                                                            d.reject("<%=Resources.Messages.MsgDocCostCenterRequired%>");
                                                        else {
                                                            jqVar.getJSON("../api/CostCenter/FindByName", { Source: params.value },
                                                                function (response) {
                                                                    if (response != null && response.length > 0) {
                                                                        d.resolve(response);
                                                                    }
                                                                    else {
                                                                        d.reject("<%=Resources.Messages.MsgDocCostCenterRequired%>");
                                                                    }
                                                                }
                                                            );
                                                        }
                                                        return d.promise();
                                                    }
                                                }
                                            ],
                                            lookup: {
                                                dataSource: DevExpress.data.AspNet.createStore({
                                                    key: "ID",
                                                    loadUrl: "../api/CostCenter/GetCostCenters",
                                                    loadMethod: "get",
                                                    loadParams: {
                                                        'contextKey': '<%=this.GetCostCenterContextKey()%>'
                                                        //+ jqVar("#acBranch").dxSelectBox('instance').option('value')
                                                    },
                                                }),
                                                allowClearing: true,
                                                valueExpr: "Name",
                                                displayExpr: "Name"
                                            },
                                            allowEditing: false,
                                            alignment: "center",
                                            width: 150,
                                            visible: <%=this.PaymentOptions.UseCostCenterByRecord.ToString().ToLower()%>,
                                        },
                                        {
                                            dataField: "DiscountCol",
                                            caption: "<%=Resources.Labels.CashDiscount%>",
                                            allowEditing: false,
                                            dataType: "number",
                                            format: "decimal",
                                            alignment: "center",
                                            width: 100,
                                            visible: <%=this.PaymentOptions.UseDiscountByRecord.ToString().ToLower()%>,
                                        },
                                        {
                                            dataField: "TaxCol",
                                            caption: '<%=Resources.Labels.DgTaxHeader%>',
                                            allowEditing: false,
                                            dataType: "number",
                                            format: "decimal",
                                            alignment: "center",
                                            width: 100,
                                            visible: <%=this.PaymentOptions.UseVatByRecord.ToString().ToLower()%>,
                                        },
                                        {
                                            dataField: "DescriptionCol",
                                            caption: "<%=Resources.Labels.DgByanHeader%>",
                                            allowEditing: false,
                                            dataType: "string",
                                            alignment: "right",
                                            width: 250,
                                            visible: <%=this.PaymentOptions.UseNotesByRecord.ToString().ToLower()%>,
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
                                            dataField: "VendorsSecondnameCol",
                                            caption: "<%=Resources.Labels.DgVendorsSecondnameHeader%>",
                                            dataType: "string",
                                            validationRules: [
                                                {
                                                    type: "required",
                                                    ignoreEmptyValue: false,
                                                },
                                                {
                                                    type: "async",
                                                    message: "<%=Resources.Messages.MsgVendorRequired%>",
                                                    ignoreEmptyValue: false,
                                                    validationCallback: function (params) {
                                                        const d = jqVar.Deferred();
                                                        if (params.value == null)
                                                            d.reject("<%=Resources.Messages.MsgVendorRequired%>");
                                                        else {
                                                            jqVar.getJSON("../api/Vendor/FindByName", { Source: params.value },
                                                                function (response) {
                                                                    if (response != null && response.length > 0) {
                                                                        d.resolve(response);
                                                                    }
                                                                    else {
                                                                        d.reject("<%=Resources.Messages.MsgVendorRequired%>");
                                                                    }
                                                                }
                                                            );
                                                        }
                                                        return d.promise();
                                                    }
                                                }
                                            ],
                                            lookup: {
                                                dataSource: DevExpress.data.AspNet.createStore({
                                                    key: "ID",
                                                    loadUrl: "../api/Vendor/GetVendorsSecond",
                                                    loadMethod: "get"
                                                }),
                                                allowClearing: true,
                                                valueExpr: "Name",
                                                displayExpr: "Name"
                                            },
                                            allowEditing: false,
                                            alignment: "center",
                                            width: 150,
                                            visible: <%=this.PaymentOptions.UseVendorsSecondByRecord.ToString().ToLower()%>,
                                        },
                                        {
                                            dataField: "IdCol",
                                            caption: '<%=Resources.Labels.DgTranIdHeader%>',
                                            allowEditing: false,
                                            visible: false
                                        },
                                        {
                                            dataField: "CostCenterIdCol",
                                            caption: "CostCenterIdCol",
                                            allowEditing: false,
                                            visible: false
                                        },
                                        {
                                            dataField: "VendorsSecondIdCol",
                                            caption: "VendorsSecondIdCol",
                                            allowEditing: false,
                                            visible: false
                                        },
                                        {
                                            dataField: "DebitAccountIdCol",
                                            caption: "DebitAccountIdCol",
                                            allowEditing: false,
                                            width: 100,
                                            dataType: "string",
                                            alignment: "center",
                                            visible: false
                                        },
                                        {
                                            dataField: "CreditAccountIdCol",
                                            caption: "CreditAccountIdCol",
                                            allowEditing: false,
                                            width: 100,
                                            dataType: "string",
                                            alignment: "center",
                                            visible: false
                                        },
                                        {
                                            dataField: "SalesRepIdCol",
                                            caption: "SalesRepIdCol",
                                            allowEditing: false,
                                            width: 100,
                                            dataType: "string",
                                            alignment: "center",
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
                                                    jqVar('#paymentGridContainer').dxDataGrid('instance').deleteRow(e.row.rowIndex);
                                                    var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                                    grid.saveEditData();
                                                }
                                            }]
                                        }
                                    ],
                                remoteOperations: false,
                                keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: ekd /* "row"*/, editOnKeyPress: true },
                                onEditorPreparing: function OnEditorPreparing(e) {
                                    var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                    var component = e.component,
                                        rowIndex = e.row && e.row.rowIndex;

                                    if (e.dataField == "DebitAccountNameCol") {
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            if (e.value != null && e.value != '') {
                                                component.cellValue(rowIndex, "DebitAccountIdCol", e.value);
                                                component.cellValue(rowIndex, "DebitAccountNameCol", e.value);
                                            }
                                            else {
                                                component.cellValue(rowIndex, "DebitAccountIdCol", null);
                                                component.cellValue(rowIndex, "DebitAccountNameCol", null);
                                            }
                                        }
                                    }
                                    else if (e.dataField == "CreditAccountNameCol") {
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            if (e.value != null && e.value != '') {
                                                component.cellValue(rowIndex, "CreditAccountIdCol", e.value);
                                                component.cellValue(rowIndex, "DebitAccountNameCol", e.value);
                                                jqVar("#CreditAccountIdHidden").val(e.value);
                                            }
                                            else {
                                                component.cellValue(rowIndex, "CreditAccountIdCol", null);
                                                component.cellValue(rowIndex, "DebitAccountNameCol", null);
                                                jqVar("#CreditAccountIdHidden").val(null);
                                            }
                                        }
                                    }
                                    else if (e.dataField == "SalesRepNameCol") {
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            if (e.value != null && e.value != '') {
                                                jqVar.getJSON("../api/General/FindSalesRepByName", { Source: e.value },
                                                    function (response) {
                                                        if (response != null) {
                                                            component.cellValue(rowIndex, "SalesRepIdCol", response[0].ID);
                                                            //component.cellValue(rowIndex, "SalesRepNameCol", response[0].Name);
                                                        }
                                                        else {
                                                            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                                            component.cellValue(rowIndex, "SalesRepIdCol", null);
                                                            component.cellValue(rowIndex, "SalesRepNameCol", null);
                                                            grid.focus(grid.getCellElement(rowIndex, "SalesRepNameCol"));
                                                        }
                                                    });
                                            }
                                            else {
                                                component.cellValue(rowIndex, "SalesRepIdCol", null);
                                                component.cellValue(rowIndex, "SalesRepNameCol", null);
                                            }
                                        }
                                    }
                                    else if (e.dataField == "CostCenterNameCol") {
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            if (e.value != null && e.value != '') {
                                                jqVar.getJSON("../api/CostCenter/FindByName", { Source: e.value },
                                                    function (response) {
                                                        if (response != null) {
                                                            component.cellValue(rowIndex, "CostCenterIdCol", response[0].ID);
                                                            component.cellValue(rowIndex, "CostCenterNameCol", response[0].Name);
                                                        }
                                                        else {
                                                            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                                            component.cellValue(rowIndex, "CostCenterIdCol", null);
                                                            component.cellValue(rowIndex, "CostCenterNameCol", null);
                                                            grid.focus(grid.getCellElement(rowIndex, "CostCenterNameCol"));
                                                        }
                                                    });
                                            }
                                            else {
                                                component.cellValue(rowIndex, "CostCenterIdCol", null);
                                                component.cellValue(rowIndex, "CostCenterNameCol", null);
                                            }
                                        }
                                    }
                                    else if (e.dataField == "VendorsSecondnameCol") {
                                        var onValueChanged = e.editorOptions.onValueChanged;
                                        e.editorOptions.onValueChanged = function (e) {
                                            onValueChanged.call(this, e);
                                            if (e.value != null && e.value != '') {
                                                jqVar.getJSON("../api/Vendor/FindByName", { Source: e.value },
                                                    function (response) {
                                                        if (response != null) {
                                                            component.cellValue(rowIndex, "VendorsSecondIdCol", response[0].ID);
                                                            component.cellValue(rowIndex, "VendorsSecondnameCol", response[0].Name);
                                                        }
                                                        else {
                                                            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                                            component.cellValue(rowIndex, "VendorsSecondIdCol", null);
                                                            component.cellValue(rowIndex, "VendorsSecondnameCol", null);
                                                            grid.focus(grid.getCellElement(rowIndex, "VendorsSecondnameCol"));
                                                        }
                                                    });
                                            }
                                            else {
                                                component.cellValue(rowIndex, "VendorsSecondIdCol", null);
                                                component.cellValue(rowIndex, "VendorsSecondnameCol", null);
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
                                //height: 450,
                                height: function () {
                                    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                                        return 450;
                                    }
                                    else {
                                        var toolsPnl = jqVar("#ToolsPnl").height();
                                        var btnRow = jqVar("#BtnsRow").height();
                                        return window.innerHeight - ((toolsPnl + btnRow) - 30);
                                    }
                                },
                                sorting: { mode: "none" },
                                customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                                onKeyDown: function (e) {
                                    var keyCode = e.event.which;
                                    var component = e.component;
                                    if (keyCode == 13) {
                                        var validateResult = ValidateRows(false);
                                        var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
                                        var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                        if (currentRowIndex == rows.length - 1 && validateResult.responseCode)
                                            AddRowBottom(true);
                                        else showErrorMessage(validateResult.responseMessage, null);
                                    } else if (keyCode == 46 && currentRowIndex >= 0) {
                                        var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                        const focusedCellPosition = getCurrentCell(grid);
                                        grid.deleteRow(focusedCellPosition.rowIndex);
                                    }
                                },
                                summary: {
                                    totalItems: [
                                        { column: "DebitAccountNameCol", displayFormat: "<%=Resources.Labels.Total%>" },
                                        {
                                            column: "AmountCol",
                                            summaryType: "sum",
                                            displayFormat: "{0}",
                                            valueFormat: "decimal",
                                            name: "TotalAmountSum"
                                        },
                                        {
                                            column: "DiscountCol",
                                            summaryType: "sum",
                                            displayFormat: "{0}",
                                            valueFormat: "decimal",
                                            name: "TotalDiscountSum"
                                        },
                                        {
                                            column: "TaxCol",
                                            summaryType: "sum",
                                            displayFormat: "{0}",
                                            valueFormat: "decimal",
                                            name: "TotalTaxSum"
                                        }
                                    ],
                                    recalculateWhileEditing: true
                                },
                                onCellClick: function (e) {
                                    var operation = jqVar("#Operation").val();
                                    if ((operation == "Add" || operation == "Edit"))
                                        SetDataGridEditable(true);
                                    else SetDataGridEditable(false);
                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                                focusedRowEnabled: true,
                                onFocusedRowChanging: function OnFocusedRowChanging(e) {
                                    currentRowIndex = e.newRowIndex;
                                }
                            });
                            clearDgv();

                        }, 100);
                    }).done(function () {
                    });

                    jqVar("#acCostCenter").dxSelectBox({
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.CostCenter%>',
                        onValueChanged: function (data) {

                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                    }).dxSelectBox("instance");
                    /*******************************************/
                    jqVar("#txtOperationDate").dxDateBox({
                        displayFormat: "dd/MM/yyyy",
                        value: new Date()
                    });
                    /*******************************************/
                    jqVar.getJSON("../../api/PrintTemplate/GetDefaultTemplateByKindId", { KindId: '<%=this.DocKindId%>', 'EntryType': '<%=this.EntryType%>' },
                        function (response) {
                            if (response.length > 0) {
                                jqVar("#cmbxTemplate").dxSelectBox({
                                    dataSource: DevExpress.data.AspNet.createStore({
                                        key: "Id",
                                        loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                        loadMethod: "get",
                                        loadParams: { 'KindId': '<%=this.DocKindId%>', 'EntryType': '<%=this.EntryType%>' },
                                        displayExpr: "Name",
                                        valueExpr: "Id"
                                    }),
                                    displayExpr: "Name",
                                    valueExpr: "Id",
                                    value: response[0].Id,
                                    searchEnabled: true,
                                    placeholder: '<%=Resources.Labels.PrintTemplate%>',
                                    onValueChanged: function (data) {
                                    },
                                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                                }).dxSelectBox("instance");
                            }
                            else {
                                jqVar("#cmbxTemplate").dxSelectBox({
                                    dataSource: DevExpress.data.AspNet.createStore({
                                        key: "Id",
                                        loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                        loadMethod: "get",
                                        loadParams: { 'KindId': '<%=this.DocKindId%>', 'EntryType': '<%=this.EntryType%>' },
                                        displayExpr: "Name",
                                        valueExpr: "Id"
                                    }),
                                    displayExpr: "Name",
                                    valueExpr: "Id",
                                    searchEnabled: true,
                                    placeholder: '<%=Resources.Labels.PrintTemplate%>',
                                    onValueChanged: function (data) {
                                    },
                                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                                }).dxSelectBox("instance");
                            }
                        });
                }).done(function () {
                    setEnabled(".executeBtn", false);
                    setEnabled("#btnAdd", true);
                    setEnabled("#btnSearch", true);
                    setEnabled("#btnPrint", true);
                    setEnabled("#txtOperationDate", true);
                    setTimeout(function () {
                        setReadOnly("#ToolsPnl", true);
                        jqVar.getJSON('../../api/GridOrdering/FindGridOrdering', { 'GridId': '<%=this.DocKindId%>', 'UserId': '<%=MyContext.UserProfile.Contact_ID%>' }, function (response) {
                            if (response.length == 1) {
                                var array = JSON.parse(response[0].JsonVal);
                                var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
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
        });

        function clearDgv() {
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
            var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }

            for (var i = 0; i < 10; i++) {
                grid.addRow();
            }
        }

        getCurrentCell = function (dataGrid) {
            return (dataGrid)._controllers.keyboardNavigation._focusedCellPosition;
        }

        function ValidateRows(skipEmptyRows) {
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
            var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1,
                operation = jqVar("#Operation").val(), documentId = jqVar("#Id").val(),
                docDate = jqVar("#txtOperationDate").dxDateBox("instance").option('text').trim();

            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].data["DebitAccountIdCol"] == null || rows[i].data["DebitAccountIdCol"] == '') &&
                        (rows[i].data["DebitAccountNameCol"] == null || rows[i].data["DebitAccountNameCol"] == '') &&
                        (rows[i].data["CreditAccountIdCol"] == null || rows[i].data["CreditAccountIdCol"] == '') &&
                        (rows[i].data["CreditAccountNameCol"] == null || rows[i].data["CreditAccountNameCol"] == '') &&
                        (rows[i].data["SalesRepIdCol"] == null || rows[i].data["SalesRepIdCol"] == '') &&
                        (rows[i].data["SalesRepNameCol"] == null || rows[i].data["SalesRepNameCol"] == '') &&
                        (rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') &&
                        (rows[i].data["DiscountCol"] == null || rows[i].data["DiscountCol"] == '') &&
                        (rows[i].data["TaxCol"] == null || rows[i].data["TaxCol"] == '') &&
                        (rows[i].data["DescriptionCol"] == null || rows[i].data["DescriptionCol"] == '') &&
                        (rows[i].data["CostCenterNameCol"] == null || rows[i].data["CostCenterNameCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["VendorsSecondnameCol"] == null || rows[i].data["VendorsSecondnameCol"] == '') &&
                        (rows[i].data["CostCenterIdCol"] == null || rows[i].data["CostCenterIdCol"] == '') &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '')
                    )
                        isEmptyRow = true;
                }

                if (rows[i].rowType == "data" && (skipEmptyRows == false || isEmptyRow == false)) {
                    if (rows[i].data["DebitAccountIdCol"] == null || rows[i].data["DebitAccountIdCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDebitAccountRequired%>";
                        cellName = "DebitAccountNameCol";
                    }
                    else if (rows[i].data["CreditAccountIdCol"] == null || rows[i].data["CreditAccountIdCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgCreditAccountRequired%>";
                        cellName = "CreditAccountNameCol";
                    }
                    else if ((rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') &&
                        (rows[i].data["DiscountCol"] == null || rows[i].data["DiscountCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDebitOrCreditRequired%>";
                        cellName = "AmountCol";
                    }
                    else if (<%=this.PaymentOptions.UseCostCenterByRecord.ToString().ToLower()%>== true &&
                        <%=this.PaymentOptions.ForceCostCenterByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["CostCenterIdCol"] == null || rows[i].data["CostCenterIdCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocCostCenterRequired%>";
                        cellName = "CostCenterIdCol";
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
                    else if (<%=this.PaymentOptions.UseVendorsSecondByRecord.ToString().ToLower()%>== true &&
                        <%=this.PaymentOptions.ForceVendorsSecondByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgVendorRequired%>";
                        cellName = "VendorsSecondIdCol";
                    }
                    else if (<%=this.PaymentOptions.UseVatByRecord.ToString().ToLower()%>== true &&
                        <%=this.PaymentOptions.ForceVendorsSecondByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["TaxCol"] == null || rows[i].data["TaxCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgTaxRequired%>";
                        cellName = "TaxCol";
                    }
                    else if (<%=this.PaymentOptions.UseSalesRepByRecored.ToString().ToLower()%>== true &&
                        <%=this.PaymentOptions.ForceSalesRepRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["SalesRepIdCol"] == null || rows[i].data["SalesRepIdCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocSalesRepRequired%>";
                        cellName = "SalesRepNameCol";
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

            if (hasError == true) {
                return { responseCode: false, 'rowIndex': rowIndex, responseMessage: errorMessage, 'cellName': cellName };
            }
            else return { responseCode: true };
        }

        function AddRowBottom(focusRow) {
            var validateResult = ValidateRows(false);
            if (validateResult.responseCode == true) {

                var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                var cols = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
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

                rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["DebitAccountIdCol"] = dataRows[i].DebitAccountIdCol;
                    rows[i].data["DebitAccountNameCol"] = dataRows[i].DebitAccountNameCol;
                    rows[i].data["CreditAccountIdCol"] = dataRows[i].CreditAccountIdCol;
                    rows[i].data["CreditAccountNameCol"] = dataRows[i].CreditAccountNameCol;
                    rows[i].data["SalesRepIdCol"] = dataRows[i].SalesRepIdCol;
                    rows[i].data["SalesRepNameCol"] = dataRows[i].SalesRepNameCol;
                    rows[i].data["AmountCol"] = dataRows[i].AmountCol;
                    rows[i].data["DiscountCol"] = dataRows[i].DiscountCol;
                    rows[i].data["TaxCol"] = dataRows[i].TaxCol;
                    rows[i].data["DescriptionCol"] = dataRows[i].DescriptionCol;
                    rows[i].data["CostCenterNameCol"] = dataRows[i].CostCenterNameCol;
                    rows[i].data["DocNameCol"] = dataRows[i].DocNameCol;
                    rows[i].data["OperDateCol"] = dataRows[i].OperDateCol;
                    rows[i].data["VendorsSecondnameCol"] = dataRows[i].VendorsSecondnameCol;
                    rows[i].data["CostCenterIdCol"] = dataRows[i].CostCenterIdCol;
                    rows[i].data["VendorsSecondIdCol"] = dataRows[i].VendorsSecondIdCol;
                    rows[i].data["IdCol"] = dataRows[i].IdCol;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "DebitAccountIdCol"));
                SetDataGridEditable(true);
            }
            else {
                showErrorMessage(validateResult.responseMessage, null);
            }
        }

        function getImgStatus(status) {
            var result = '';
            if (status == null)
                result += "/Images/new";
            else if (status == 0)
                result += "/Images/new";
            else if (status == 1)
                result += "/Images/current";
            else if (status == 2)
                result += "/Images/Approved";
            else if (status == 3)
                result += "/Images/Canceled";
            else result += status;
            result += '<%=this.MyContext.CurrentCulture == XPRESS.Common.ABCulture.Arabic ? "-ar" : string.Empty%>';
            return "url('" + result + ".png" + "')";
        }
    </script>

    <!-- Bill Scripts -->
    <script type="text/javascript">
        function clearTools() {
            jqVar("#Id").val(null);
            jqVar("#Operation").val(null);
            clearDgv();

            if (<%=this.PaymentOptions.KeepCostCenter.ToString().ToLower()%>!= true)
                jqVar("#acCostCenter").dxSelectBox('instance').option({ value: null });

            if (<%=this.PaymentOptions.KeepDocDate.ToString().ToLower()%>!= true) {
                jqVar.getJSON("../../api/General/GetServerDate", null, function (response) {
                    jqVar("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response) });
                });
            }

            setReadOnly("#ToolsPnl", true);
        }

        function SetDataGridEditable(value) {
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
            var columns = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleColumns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].allowEditing = value;
                if (columns[i].name == "RowNoCol")
                    columns[i].allowEditing = false;
            }
        }

        function addItem() {
            setEnabled(".executeBtn", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            jqVar("#Operation").val("Add");
            SetDataGridEditable(true);
        }

        function findItem() {
            jqVar("#searchDiv").load("../Payments/FrmPaymentSelect.aspx?p=<%=Request.QueryString["p"]%>&requestCode=1");
            showModal('#searchModal');
        }

        function documentSelected(requestCode, selectedDocumentId) {
            if (requestCode == 1) {
                clearTools();
                hideModal('#searchModal');
                jqVar.getJSON("../api/Payment/FindOperationById", { Payment_ID: selectedDocumentId },
                    function (response) {
                        if (response.length == 1) {
                            if (response[0].DocStatus_ID == 1)
                                setEnabled("#btnEdit", true);
                            else setEnabled("#btnEdit", false);
                            setEnabled("#btnApprove", true);
                            setEnabled("#btnReset", true);
                            setEnabled("#btnAdd", false);
                            //jqVar("#LocalId").val(response[0].LocalId);
                            //jqVar("#DebitAccountId").val(response[0].DebitAccountId);
                            jqVar("#acBranch").dxSelectBox('instance').option({ value: response[0].Branch_ID });
                            jqVar("#acCostCenter").dxSelectBox('instance').option({ value: response[0].CostCenter_ID });
                            jqVar("#txtUserRefNo").val(response[0].UserRefNo);
                            setTimeout(function () {
                                //jqVar("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response[0].OperationDate) });
                            }, 100);

                            jqVar("#DocRandomString").val(response[0].DocRandomString);
                            jqVar("#imgStatusDiv").css("background-image", getImgStatus(response[0].DocStatus_ID));
                            jqVar("#Id").val(selectedDocumentId);
                            setEnabled("#btnPrint", true);

                            jqVar.getJSON("../api/Payment/GetEntryDetails", { Payment_ID: selectedDocumentId },
                                function (responseTable) {
                                    var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
                                    //var cols = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleColumns();
                                    var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();

                                    var length = rows.length;
                                    for (var i = length - 1; i >= 0; i--) {
                                        grid.deleteRow(i);
                                    }

                                    length = responseTable.length;
                                    for (var i = 0; i < length + 1; i++) {
                                        grid.addRow(i);
                                    }

                                    rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();

                                    for (var i = 0; i < responseTable.length; i++) {
                                        rows[i].data["DebitAccountIdCol"] = responseTable[i].DebitAccount_ID;
                                        rows[i].data["DebitAccountNameCol"] = responseTable[i].DebitAccount_ID;
                                        rows[i].data["CreditAccountIdCol"] = responseTable[i].CreditAccount_ID;
                                        rows[i].data["CreditAccountNameCol"] = responseTable[i].CreditAccount_ID;
                                        rows[i].data["SalesRepIdCol"] = responseTable[i].SalesRep_ID;
                                        rows[i].data["SalesRepNameCol"] = responseTable[i].SalesRepName;
                                        rows[i].data["AmountCol"] = responseTable[i].Amount;
                                        rows[i].data["DiscountCol"] = responseTable[i].Discount;
                                        rows[i].data["TaxCol"] = responseTable[i].Tax;

                                        rows[i].data["DescriptionCol"] = responseTable[i].Description;
                                        rows[i].data["CostCenterNameCol"] = responseTable[i].CostCenterName;
                                        rows[i].data["DocNameCol"] = responseTable[i].DocName;
                                        rows[i].data["OperDateCol"] = responseTable[i].OperDateString;//OperDate
                                        rows[i].data["VendorsSecondnameCol"] = responseTable[i].VendorsSecondname;
                                        rows[i].data["CostCenterIdCol"] = responseTable[i].CostCenter_ID;
                                        rows[i].data["VendorsSecondIdCol"] = responseTable[i].VendorsSecond_ID;

                                        rows[i].data["IdCol"] = responseTable[i].ID;
                                    }

                                    grid.refresh();
                                });
                        }
                    });
            }
            else if (requestCode == 2) {
                hideModal('#searchModal');
                var printTemplateId = jqVar("#cmbxTemplate").dxSelectBox('instance').option('value');
                window.open('../PrintTemplate/CashTemplateUI/CashPrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&DocumentId=' + selectedDocumentId + "&DocKindId=" + '<%=this.DocKindId%>' + "&EntryType=" + '<%=this.EntryType%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
            }
        }

        function showOperationDetials(SourceDocumentId) {
            jqVar("#operationDetailsDiv").load("../OperationDetails/FrmOperationDetials.aspx?SourceDocId=" + SourceDocumentId + "&SourceTableId=" +<%=this.DocKindId%>);
            showModal('#operationDetailsModal');
        }

        function editItem() {
            setEnabled(".executeBtn", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            jqVar("#Operation").val("Edit");
            SetDataGridEditable(true);
        }

        function toastError(validateResult) {
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
            jqVar("#notifyview").html(validateResult.responseMessage);
            jqVar("#notifymsgdiv").removeClass("alert-success");
            jqVar("#notifymsgdiv").addClass("alert-danger");
            jqVar("#notifymsgdiv").fadeIn();
            showModal("#notifyModal");
            jqVar("#notifymsgdiv").fadeOut(3000, function () {
                hideModal("#notifyModal");
                if (validateResult.rowIndex != -1 && validateResult.cellName != null && validateResult.cellName != '')
                    grid.focus(grid.getCellElement(validateResult.rowIndex, validateResult.cellName));
            });

            if (validateResult.rowIndex != -1 && validateResult.cellName != null && validateResult.cellName != '')
                grid.focus(grid.getCellElement(validateResult.rowIndex, validateResult.cellName));
        }

        function saveItem(isApproving) {
            var validateResult = ValidateRows(true);
            var dateBox = jqVar("#txtOperationDate").dxDateBox("instance");
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");

            if (dateBox.option('text') == null || dateBox.option('text') == '') {
                showErrorMessage("<%=Resources.Messages.MsgDocOperationDateRequired%>", "#txtOperationDate");
                return false;
            }
            else if (jqVar("#acBranch").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocBranchRequired%>", "#acBranch");
                return false;
            }
            else if (!validateResult.responseCode) {
                toastError(validateResult);
                return false;
            }
            else {
                var operation = jqVar("#Operation").val();
                var amount = null, discount = null, tax = null;
                var editMode = false;
                if (operation == "Edit")
                    editMode = true;

                var branchId = jqVar("#acBranch").dxSelectBox('instance').option('value'),
                    costCenterId = jqVar("#acCostCenter").dxSelectBox('instance').option('value'),
                    userRefNo = jqVar("#txtUserRefNo").val(),
                    docDate = dateBox.option('text').trim(),
                    currencyId = '<%=ddlCurrency.SelectedValue%>',
                    docRandomString = $("#DocRandomString").val();

                var totalAmountSum = grid.getTotalSummaryValue('TotalAmountSum');
                var totalDiscountSum = grid.getTotalSummaryValue('TotalDiscountSum');
                var totalTaxSum = grid.getTotalSummaryValue('TotalTaxSum');

                var rows = jqVar("#paymentGridContainer").dxDataGrid("instance").getVisibleRows();
                var dataRows = [];

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].data["DebitAccountIdCol"] == null || rows[i].data["DebitAccountIdCol"] == '') &&
                        (rows[i].data["DebitAccountNameCol"] == null || rows[i].data["DebitAccountNameCol"] == '') &&
                        (rows[i].data["CreditAccountIdCol"] == null || rows[i].data["CreditAccountIdCol"] == '') &&
                        (rows[i].data["CreditAccountNameCol"] == null || rows[i].data["CreditAccountNameCol"] == '') &&
                        (rows[i].data["SalesRepIdCol"] == null || rows[i].data["SalesRepIdCol"] == '') &&
                        (rows[i].data["SalesRepNameCol"] == null || rows[i].data["SalesRepNameCol"] == '') &&
                        (rows[i].data["AmountCol"] == null || rows[i].data["AmountCol"] == '') &&
                        (rows[i].data["DiscountCol"] == null || rows[i].data["DiscountCol"] == '') &&
                        (rows[i].data["TaxCol"] == null || rows[i].data["TaxCol"] == '') &&
                        (rows[i].data["DescriptionCol"] == null || rows[i].data["DescriptionCol"] == '') &&
                        (rows[i].data["CostCenterNameCol"] == null || rows[i].data["CostCenterNameCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["VendorsSecondnameCol"] == null || rows[i].data["VendorsSecondnameCol"] == '') &&
                        (rows[i].data["CostCenterIdCol"] == null || rows[i].data["CostCenterIdCol"] == '') &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '')
                    )
                        isEmptyRow = true;

                    if (rows[i].rowType == "data" && isEmptyRow == false) {
                        var debitAccountId = null, debitAccountName = null, creditAccountId = null, creditAccountName = null, amount = null, discount = null, tax = null,
                            description = null, costCenterName = null, vendorsSecondname = null, vendorsSecondId = null, salesRepId = null, salesRepName = null,
                            docName = null, operDate = null, rowCostCenterId = costCenterId;

                        if (rows[i].data["DebitAccountIdCol"] != null)
                            debitAccountId = rows[i].data["DebitAccountIdCol"];
                        if (rows[i].data["CreditAccountIdCol"] != null)
                            creditAccountId = rows[i].data["CreditAccountIdCol"];
                        if (rows[i].data["SalesRepIdCol"] != null)
                            salesRepId = rows[i].data["SalesRepIdCol"];
                        if (rows[i].data["AmountCol"] != null)
                            amount = rows[i].data["AmountCol"];
                        if (rows[i].data["DiscountCol"] != null)
                            discount = rows[i].data["DiscountCol"];
                        if (rows[i].data["TaxCol"] != null)
                            tax = rows[i].data["TaxCol"];
                        if (rows[i].data["DescriptionCol"] != null)
                            description = rows[i].data["DescriptionCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocNameByRecord.ToString().ToLower()%>== true && rows[i].data["DocNameCol"] != null)
                            docName = rows[i].data["DocNameCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocDateByRecord.ToString().ToLower()%>== true && rows[i].data["OperDateCol"] != null)
                            operDate = rows[i].data["OperDateCol"];
                        if (rows[i].data["CostCenterIdCol"] != null)
                            rowCostCenterId = rows[i].data["CostCenterIdCol"];
                        if (rows[i].data["VendorsSecondIdCol"] != null)
                            vendorsSecondId = rows[i].data["VendorsSecondIdCol"];
                        var idCol = rows[i].data["IdCol"];

                        var row = {
                            'ID': idCol, 'DebitAccount_ID': debitAccountId, 'CreditAccount_ID': creditAccountId, 'Amount': amount, 'Discount': discount, 'Tax': tax, 'Description': description,
                            'DocName': docName, 'OperDate': operDate, 'CostCenter_ID': costCenterId, 'VendorsSecond_ID': vendorsSecondId, 'SalesRep_ID': salesRepId,
                        };
                        dataRows.push(row);
                        //dataRows.push(rows[i].data);
                    }
                }

                dataRows = JSON.stringify(dataRows);

                var id = jqVar("#Id").val();
                jqVar.post("../api/Payment/SavePayment", {
                    'Id': id, 'BranchId': branchId, 'CostCenterId': rowCostCenterId, 'UserRefNo': userRefNo,
                    'CurrencyId': currencyId, 'OperationDate': docDate, 'DocRandomString': docRandomString,
                    'TotalAmount': totalAmountSum, 'TotalDiscount': totalDiscountSum, 'TotalTax': totalTaxSum,
                    'EditMode': editMode, 'UserProfileContact_ID': '<%=MyContext.UserProfile.Contact_ID%>',
                    'PathInfo': '/' + '<%=Request.QueryString["p"]%>',
                    'IsApproving': isApproving, 'Source': dataRows
                },
                    function (response) {
                        if (response.Code > 0) {
                            clearTools();
                            jqVar("#Operation").val(null);
                            setReadOnly("#ToolsPnl", true);
                            setEnabled(".executeBtn", false);
                            setEnabled("#btnAdd", true);
                            setEnabled("#btnSearch", true);
                            setEnabled("#btnPrint", true);
                            showSuccessPrint("<%=Resources.Messages.MsgDocOperationSucessPrint%>", "<%=Resources.Messages.MsgDocSaveSuccess%>", response.Code);

                            jqVar("#btnLoad").click(function () {
                                hideModal('#printModal');
                                documentSelected(1, response.Code);
                            });

                            jqVar("#btnConfirmPrint").click(function () {
                                hideModal('#printModal');
                                var printTemplateId = jqVar("#cmbxTemplate").dxSelectBox('instance').option('value');
                                window.open('../PrintTemplate/CashTemplateUI/CashPrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&DocumentId=' + response.Code + "&DocKindId=" + '<%=this.DocKindId%>' + "&EntryType=" + '<%=this.EntryType%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                                if (<%=this.PaymentOptions.AddNewAfterSave.ToString().ToLower()%>== true)
                                    addItem();
                            });

                            //------------------------ btnSendEmail ---------------------------//
                            jqVar("#btnSendEmail").click(function () {
                                jqVar.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateEmail(queryResponse) == true) {
                                        jqVar.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>', "SendType": 1 }, function (sendResponse) {
                                            if (sendResponse.Code == 100)
                                                hideModal('#printModal');
                                            else {
                                                jqVar("#sendview").html(sendResponse.Message);
                                                jqVar("#sendmsgdiv").removeClass("alert-success");
                                                jqVar("#sendmsgdiv").addClass("alert-danger");
                                                jqVar("#sendmsgdiv").fadeIn();
                                                jqVar("#sendmsgdiv").fadeOut(5000);
                                            }
                                        });
                                    }
                                    else {
                                        hideModal('#printModal');
                                        showModal('#emailModal');
                                        //------------------------ Confirm Email ---------------------------//                                                                                
                                        jqVar("#btnConfirmEmail").click(function () {
                                            /*var forms = document.querySelectorAll('.needs-validation');
                                            Array.prototype.slice.call(forms)
                                                .forEach(function (form) {
                                                        if (!form.checkValidity()) {
                                                            event.preventDefault();
                                                            event.stopPropagation();
                                                            console.log("btnConfirmEmail");
                                                        }
                                                        form.classList.add('was-validated')
                                                });*/

                                            var emailRGEX = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
                                            var emailVal = jqVar('#txtEmailTo').val();
                                            var emailResult = emailRGEX.test(emailVal);

                                            if (jqVar('#txtEmailTo').val() == "") {
                                                jqVar("#emailmsgdiv").removeClass("alert-success");
                                                jqVar("#emailmsgdiv").addClass("alert-danger");
                                                jqVar("#emailmsgdiv").fadeIn();
                                                jqVar("#emailview").html("<%=Resources.Messages.MsgEmailRequired%>");
                                                jqVar("#emailmsgdiv").fadeOut(5000);
                                                jqVar('#txtEmailTo').focus();
                                                return false;
                                            }
                                            else if (emailResult == false) {
                                                jqVar("#emailmsgdiv").removeClass("alert-success");
                                                jqVar("#emailmsgdiv").addClass("alert-danger");
                                                jqVar("#emailmsgdiv").fadeIn();
                                                jqVar("#emailview").html("<%=Resources.Messages.MsgEmailInvalid%>");
                                                jqVar("#emailmsgdiv").fadeOut(5000);
                                                jqVar('#txtEmailTo').focus();

                                            }
                                            else {
                                                jqVar.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>', "SendType": 2, "Data": jqVar('#txtEmailTo').val() }, function (sendResponse) {
                                                    if (sendResponse.Code == 100)
                                                        hideModal('#printModal');
                                                    else {
                                                        jqVar("#sendview").html(sendResponse.Message);
                                                        jqVar("#sendmsgdiv").removeClass("alert-success");
                                                        jqVar("#sendmsgdiv").addClass("alert-danger");
                                                        jqVar("#sendmsgdiv").fadeIn();
                                                        jqVar("#sendmsgdiv").fadeOut(5000);
                                                    }
                                                });
                                                //hideModal('#emailModal');
                                            }
                                        });

                                        //------------------------ Back ---------------------------//
                                        jqVar("#btnBackEmail").click(function () {
                                            hideModal('#emailModal');
                                            showModal('#printModal');
                                        });
                                    }
                                });
                            });

                            //------------------------ btnSendSms ---------------------------//
                            jqVar("#btnSendSms").click(function () {
                                jqVar.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateMobile(queryResponse) == true) {
                                        jqVar.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>', "SendType": 1 }, function (sendResponse) {
                                            if (sendResponse.Code == 100)
                                                hideModal('#printModal');
                                            else {
                                                jqVar("#sendview").html(sendResponse.Message);
                                                jqVar("#sendmsgdiv").removeClass("alert-success");
                                                jqVar("#sendmsgdiv").addClass("alert-danger");
                                                jqVar("#sendmsgdiv").fadeIn();
                                                jqVar("#sendmsgdiv").fadeOut(5000);
                                            }
                                        });
                                    }
                                    else {
                                        hideModal('#printModal');
                                        showModal('#mobileModal');
                                        //------------------------ Confirm Mobile ---------------------------//
                                        jqVar("#btnConfirmMobile").click(function () {
                                            /*var forms = document.querySelectorAll('.needs-validation');                                    
                                            Array.prototype.slice.call(forms)
                                                .forEach(function (form) {                                            
                                                        if (!form.checkValidity()) {
                                                            event.preventDefault();
                                                            event.stopPropagation();
                                                            console.log("btnConfirmMobile");
                                                        }
                                                        form.classList.add('was-validated')                                            
                                                });*/

                                            var mobileRGEX = /^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$/;
                                            var mobileVal = jqVar('#txtMobileTo').val();
                                            var mobileResult = mobileRGEX.test(mobileVal);

                                            if (jqVar('#txtMobileTo').val() == "") {
                                                jqVar("#mobilemsgdiv").removeClass("alert-success");
                                                jqVar("#mobilemsgdiv").addClass("alert-danger");
                                                jqVar("#mobilemsgdiv").fadeIn();
                                                jqVar("#mobileview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                                jqVar("#mobilemsgdiv").fadeOut(5000);
                                                jqVar('#txtMobileTo').focus();
                                                return false;
                                            }
                                            else if (mobileResult == false) {
                                                jqVar("#mobilemsgdiv").removeClass("alert-success");
                                                jqVar("#mobilemsgdiv").addClass("alert-danger");
                                                jqVar("#mobilemsgdiv").fadeIn();
                                                jqVar("#mobileview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                                jqVar("#mobilemsgdiv").fadeOut(5000);
                                                jqVar('#txtMobileTo').focus();
                                            }
                                            else {
                                                hideModal('#mobileModal');
                                                jqVar.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=this.DocKindId%>', "SendType": 2, "Data": jqVar('#txtMobileTo').val() }, function (sendResponse) {
                                                    if (sendResponse.Code == 100)
                                                        hideModal('#printModal');
                                                    else {
                                                        jqVar("#sendview").html(sendResponse.Message);
                                                        jqVar("#sendmsgdiv").removeClass("alert-success");
                                                        jqVar("#sendmsgdiv").addClass("alert-danger");
                                                        jqVar("#sendmsgdiv").fadeIn();
                                                        jqVar("#sendmsgdiv").fadeOut(5000);
                                                    }
                                                });
                                                //showModal('#printModal');
                                            }
                                        });

                                        //------------------------ Back ---------------------------//
                                        jqVar("#btnBackMobile").click(function () {
                                            hideModal('#mobileModal');
                                            showModal('#printModal');
                                        });
                                    }
                                });
                            });

                            //------------------------ btnSendWhatsapp ---------------------------//
                            jqVar("#btnSendWhatsapp").click(function () {
                                hideModal('#printModal');
                                showModal('#whatsappModal');
                                //------------------------ Back ---------------------------//
                                jqVar("#btnConfirmWhatsapp").click(function () {
                                    /*var forms = document.querySelectorAll('.needs-validation');                                    
                                    Array.prototype.slice.call(forms)
                                        .forEach(function (form) {                                            
                                                if (!form.checkValidity()) {
                                                    event.preventDefault();
                                                    event.stopPropagation();
                                                    console.log("btnConfirmWhatsapp");
                                                }
                                                form.classList.add('was-validated')                                            
                                        });*/

                                    var whatsappRGEX = /^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$/;
                                    var whatsappVal = jqVar('#txtWhatsappTo').val();
                                    var whatsappResult = whatsappRGEX.test(whatsappVal);

                                    if (jqVar('#txtWhatsappTo').val() == "") {
                                        jqVar("#whatsappmsgdiv").removeClass("alert-success");
                                        jqVar("#whatsappmsgdiv").addClass("alert-danger");
                                        jqVar("#whatsappmsgdiv").fadeIn();
                                        jqVar("#whatsappview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                        jqVar("#whatsappmsgdiv").fadeOut(5000);
                                        jqVar('#txtWhatsappTo').focus();
                                        return false;
                                    }
                                    else if (whatsappResult == false) {
                                        jqVar("#whatsappmsgdiv").removeClass("alert-success");
                                        jqVar("#whatsappmsgdiv").addClass("alert-danger");
                                        jqVar("#whatsappmsgdiv").fadeIn();
                                        jqVar("#whatsappview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                        jqVar("#whatsappmsgdiv").fadeOut(5000);
                                        jqVar('#txtWhatsappTo').focus();
                                    }
                                    else {
                                        hideModal('#whatsappModal');
                                        //showModal('#printModal');
                                    }
                                });
                                //------------------------ Back ---------------------------//
                                jqVar("#btnBackWhatsapp").click(function () {
                                    hideModal('#whatsappModal');
                                    showModal('#printModal');
                                });
                            });

                            jqVar("#btnCancelPrint").click(function () {
                                if (<%=this.PaymentOptions.AddNewAfterSave.ToString().ToLower()%>== true)
                                    addItem();
                            });
                        }
                        else if (response.Code == -101)
                            showErrorMessage(response.Message);
                    });
            }
        }

        function deleteItem() {

        }

        function resetItem() {
            setEnabled(".executeBtn", false);
            clearTools();
            setEnabled("#btnReset", true);
            setEnabled("#btnAdd", true);
            setEnabled("#btnSearch", true);
            setEnabled("#btnPrint", true);
        }

        function printItem() {
            if (jqVar("#Id").val() != null && jqVar("#Id").val() != '' && jqVar("#Id").val().length > 0) {
                var id = jqVar("#Id").val();
                documentSelected(2, id);
            }
            else {
                jqVar("#searchDiv").load("../Payments/FrmPaymentSelect.aspx?p=<%=Request.QueryString["p"]%>&requestCode=2");
                showModal('#searchModal');
            }
        }

        function saveGridOrdering() {
            var grid = jqVar("#paymentGridContainer").dxDataGrid("instance");
            var colCount = grid.columnCount();
            var columnIndicies = [];
            for (var i = 0; i < colCount; i++) {
                var visibleIndex = grid.columnOption(i, "visibleIndex");
                if (visibleIndex >= 0)
                    columnIndicies.push({ index: visibleIndex, fieldName: grid.columnOption(i, "dataField") });
            }

            jqVar.post("../../api/GridOrdering/SaveGridOrdering", {
                'GridId': '<%=this.DocKindId%>',
                'UserId': '<%=MyContext.UserProfile.Contact_ID%>',
                'JsonVal': JSON.stringify(columnIndicies)
            }, function (response) {
                if (response == 100) {
                    showSuccessMessage("<%=Resources.Messages.MsgDocSaveSuccess%>", null);
                }
                else {
                    showErrorMessage("<%=Resources.Messages.MsgDocSaveFaild%>", null);
                }
            });
        }


        function CancelPaymentApprovel(SourceDocumentId) {

            jqVar.post("../../api/Payment/CancelPaymentApprovel", { ID: SourceDocumentId, PathInfo: '/' + '<%=Request.QueryString["p"]%>' },
                function (response) {
                    if (response.Code > 0) {
                        //clearTools();
                        //jqVar("#Operation").val(null);
                        //setReadOnly("#ToolsPnl", true);
                        //setEnabled(".executeBtn", false);
                        //setEnabled("#btnAdd", true);
                        //setEnabled("#btnSearch", true);
                        //setEnabled("#btnPrint", true);
                        //setEnabled("#btnReset", true);
                    }
                    else if (response.Code == -101) {
                        //  showErrorMessage(response.Message);
                    }
                });

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
        .executeBtn {
            font-weight: bold;
        }

        .MainInvoiceStyleDiv {
            zoom: 80%;
            min-width: 0px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <input type="hidden" id="Operation" />
        <input type="hidden" id="Id" />
        <input type="hidden" id="BranchIdHidden" runat="server" />
        <input type="hidden" id="CreditAccountIdHidden" runat="server" />
        <input id="TypeTax" type="hidden" value="<%= this.TypeTax.ToString() %>" />
        <input id="DocRandomString" type="hidden" />
        <input type="hidden" id="EntryType" value="<%=this.EntryType %>" />
        <input type="hidden" id="IsPermShow" value="<%=(MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID) %>" />
        <input type="hidden" id="SourceTypeId" value="<%=this.DocKindId %>" />

        <!-- Notify Modal-->
        <div class="modal fade" id="notifyModal" tabindex="-1" role="dialog" aria-labelledby="notifyModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-bs-dismiss="modal" onclick="hideModal('#notifyModal')">x</button>
                    </div>
                    <div class="modal-body">
                        <div class="alert col-md-12" id="notifymsgdiv" style="display: none;">
                            <span id="notifyview"></span>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success btn-lg" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Print Modal -->
        <div class="modal fade" id="printModal" tabindex="-1" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <strong id="printTitle" class="modal-title"><%=Resources.Messages.MsgDocSaveSuccess %></strong>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body">
                        <div class="alert col-md-12" id="printmsgdiv" style="display: none;">
                            <span id="printview">Do you want to print?
                            </span>
                            <input type="hidden" id="printedId" />
                        </div>

                        <div class="alert col-md-12" id="sendmsgdiv" style="display: none;">
                            <span id="sendview">Do you want to print?</span>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnLoad" class="printResult btn btn-success btn-lg" style="position: absolute; float: right; right: 5px;"><i class="fa fa-folder-open" aria-hidden="true"></i><%=Resources.Labels.Load%></button>
                        <button type="button" id="btnConfirmPrint" class="printResult btn btn-secondary btn-lg"><i class="fa fa-print" aria-hidden="true"></i><%=Resources.Labels.Print%></button>
                        <button type="button" id="btnSendEmail" class="printResult btn btn-primary btn-lg"><i class="fa fa-solid fa-envelope"></i><%=Resources.Labels.SendEmail%></button>
                        <button type="button" id="btnSendSms" class="printResult btn btn-info btn-lg"><i class="fa fa-solid fa-sms"></i><%=Resources.Labels.SendSms%></button>
                        <button type="button" id="btnSendWhatsapp" style="display: none" class="printResult btn btn-success btn-lg"><i class="fa fa-brands fa-whatsapp"></i><%=Resources.Labels.SendWhatsapp%></button>
                        <button type="button" id="btnCancelPrint" class="printResult btn btn-danger btn-lg" data-bs-dismiss="modal"><i class="fa fa-close"></i><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Search Modal -->
        <div class="modal fade" id="searchModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel"><%=Resources.Labels.SelectDocument %></h5>
                        <button type="button" class="close" data-bs-dismiss="modal" onclick="hideModal('#searchModal')">x</button>
                    </div>
                    <div class="modal-body" style="max-height: 500px; overflow: auto;">
                        <div id="searchDiv">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-lg btn-danger" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Operation Detials Modal -->
        <div class="modal fade" id="operationDetailsModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="operationDetailsModalLabel"><%=Resources.Labels.OperationDetails %></h5>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body" style="max-height: 500px; overflow: auto;">
                        <div id="operationDetailsDiv">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-lg btn-danger" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <div class="container-fluid">
            <!-- حقول رأس الفاتور -->
            <div class="dx-viewport">
                <div id="imgStatusDiv" class="notch_label" style="background: url('/images/new-ar.png') no-repeat no-repeat;">
                </div>
                <div class="InvoiceHeader row" style="height: auto;">
                    <asp:Nav runat="server" ID="ucNav" />
                    <%--<asp:Favorit runat="server" ID="ucFavorit" />--%>
                </div>
                <div class="row" id="divBtnAdd" style="margin: 5px">
                    <%if (MyContext.PageData.IsAdd)
                        {%>
                    <button type="button" id="btnAdd" class="executeBtn btn bot-buffer btn-sm col-md-1 offset-md-5" onclick="addItem()"><%=Resources.Labels.AddNew %> <i class="fa fa-plus-square" aria-hidden="true"></i></button>
                    <%}
                    %>
                </div>
                <div class="row" style="margin-left: 20px; margin-right: 20px;">
                    <div class="col-md-12">
                        <div id="ToolsPnl" class="row">
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span><%=Resources.Labels.CreatedBy %></span>:
                                        <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        <label runat="server" id="Label1" style="display: none">
                                            <%=Resources.Labels.Currency %></label>
                                        <asp:DropDownList ID="ddlCurrency" Style="display: none" runat="server"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <label runat="server" id="lblCurrency" style="display: none"><%=Resources.Labels.Currency %></label>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label2"><%=Resources.Labels.Branch %></label>
                                        <div id="acBranch" class="dxComb"></div>
                                    </div>
                                </div>

                                <%--<div class="row">
                                <div class="col-md-12">
                                    <label runat="server" id="Label3"><%=Resources.Labels.Branch %></label>
                                    <div id="acBranch" class="dxComb"></div>
                                </div>
                            </div>  --%>
                            </div>

                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span><%=Resources.Labels.ApprovedBy %></span>:
                                        <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                                        <label runat="server" id="Label14"><%=Resources.Labels.Date %></label>
                                        <div id="txtOperationDate" class="dxDatePic"></div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label4"><%=Resources.Labels.CostCenter %></label>
                                        <div id="acCostCenter" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />
                                        <label runat="server" id="Label5"><%=Resources.Labels.UserRefNo %></label>
                                        <input id="txtUserRefNo" type="text" class="form-control" placeholder="<%=Resources.Labels.UserRefNo %>" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />
                                        <label runat="server" id="Label24"><%=Resources.Labels.PrintTemplate %></label>
                                        <div id="cmbxTemplate" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="dxGridParent">
                        <div id="paymentGridContainer" class="dxGrid col-md-12"></div>
                    </div>
                </div>

                <div id="BtnsRow" class="row" style="margin-right: 2px; padding-left: 15px;">


                    <div class="col-md-8" style="text-align: center">
                        <button type="button" id="btnSave" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="saveItem(false)"><%=Resources.Labels.SaveasDraft %> <i class="fa fa-save" aria-hidden="true"></i></button>

                        <button type="button" id="btnApprove" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="saveItem(true)"><%=Resources.Labels.Approve %> <i class="fa fa-check" aria-hidden="true"></i></button>

                        <button type="button" id="btnPrint" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="printItem()"><%=Resources.Labels.Print %> <i class="fa fa-print" aria-hidden="true"></i></button>

                        <button type="button" id="btnSearch" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="findItem()"><%=Resources.Labels.Search %><i class="fa fa-search" aria-hidden="true"></i></button>

                        <button type="button" id="btnEdit" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="editItem()"><%=Resources.Labels.Edit %><i class="fa fa-edit" aria-hidden="true"></i></button>

                        <button type="button" id="btnDelete" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="deleteItem()"><%=Resources.Labels.Delete %><i class="fa fa-remove" aria-hidden="true"></i></button>
                    </div>

                    <button type="button" id="btnReset" class="executeBtn btn bot-buffer btn-sm col-md-2" onclick="resetItem()"><%=Resources.Labels.Clear %><i class="fa fa-undo" aria-hidden="true"></i></button>
                    <button type="button" id="btnSaveGrid" class="btn bot-buffer btn-sm col-md-1" onclick="saveGridOrdering()"><%=Resources.Labels.SaveOrdering %></button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
