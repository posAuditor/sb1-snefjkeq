<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="FrmJournalEntry.aspx.cs" Inherits="Accounting_FrmJournalEntry" %>

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
        var allAccounts = [];
        var jqVar = jQuery.noConflict();
        jqVar(document).ready(function () {
            jqVar("#imgStatusDiv").css("background-image", getImgStatus(0));
            loadDevextremeLocalization();

            jqVar("#MainTitle").hide();
            jqVar("#pageTitleLbl").text(jqVar("#lblPageTitle").text());
            jqVar.getJSON("../api/ChartOfAccount/GetChartOfAccountsCheledronly", null, function (exResponse) {
                for (i = 0; i < exResponse.length; i++) {
                    allAccounts.push(exResponse[i]);
                }

                jqVar.getJSON("../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                    jqVar("#acBranch").dxSelectBox({
                        dataSource: branchesResponse,
                        displayExpr: "Name",
                        valueExpr: "ID",
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.Branch%>',
                        onValueChanged: function (data) {
                            jqVar.getJSON("../api/CostCenter/GetCostCenters", { 'contextKey': '<%=this.GetCostCenterContextKey()%>' + jqVar("#acBranch").dxSelectBox('instance').option('value') }, function (costCentersResponse) {
                                var selectedCostCenterId = null;
                                if (<%=MyContext.JournalEntryOptions.KeepCostCenter.ToString().ToLower()%>== true && costCentersResponse.length > 0)
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
                                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                }).dxSelectBox("instance")
                            });
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                    }).dxSelectBox("instance");

                    setTimeout(function () {
                        var ekd = ekdArray[<%=this.MyContext.JournalEntryOptions.EnterKeyEventOnTable%>];
                        jqVar("#journalEntryGridContainer").dxDataGrid({
                            columnFixing: {
                                enabled: true
                            },
                            //repaintChangesOnly: true,
                            columnAutoWidth: true,
                            showBorders: true,
                            showRowLines: true,
                            dataSource: DevExpress.data.AspNet.createStore({
                                key: "IdCol",
                                loadUrl: "../api/JournalEntry/LoadJournalEntryList",
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
                                        dataField: "AccountIdCol",
                                        caption: "<%=Resources.Labels.DgAccountNoHeader%>",
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
                                                        jqVar.getJSON("../api/ChartOfAccount/FindById", { Source: params.value },
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
                                        caption: "<%=Resources.Labels.AccountName%>",
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
                                                        jqVar.getJSON("../api/ChartOfAccount/FindByName", { Source: params.value },
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
                                                    data: allAccounts,
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
                                        dataField: "DebitAmountCol",
                                        caption: "<%=Resources.Labels.Debit%>",
                                        allowEditing: false,
                                        dataType: "number",
                                        format: "decimal",
                                        alignment: "center",
                                        width: 100
                                    },
                                    {
                                        dataField: "CreditAmountCol",
                                        caption: "<%=Resources.Labels.Credit%>",
                                        allowEditing: false,
                                        dataType: "number",                                       
                                        format: "decimal",
                                        alignment: "center",
                                        width: 100
                                    },
                                    {
                                        dataField: "DescriptionCol",
                                        caption: '<%=Resources.Labels.DgNotesHeader%>',
                                        allowEditing: false,
                                        dataType: "string",
                                        alignment: "right",
                                        minWidth: 150,
                                        visible: <%=this.MyContext.JournalEntryOptions.UseNotesByRecord.ToString().ToLower()%>,
                                    },
                                    {
                                        dataField: "CostCenterNameCol",
                                        caption: "<%=Resources.Labels.CostCenter%>",
                                        dataType: "string",
                                        validationRules: [
                                            {
                                                type: "async",
                                                message: "<%=Resources.Messages.MsgDocCostCenterRequired%>",
                                                ignoreEmptyValue: true,
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
                                        width: 100,
                                        visible: <%=this.MyContext.JournalEntryOptions.UseCostCenterByRecord.ToString().ToLower()%>,
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
                                                type: "async",
                                                message: "<%=Resources.Messages.MsgVendorRequired%>",
                                                ignoreEmptyValue: true,
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
                                        width: 100,
                                        visible: <%=this.MyContext.JournalEntryOptions.UseVendorsSecondByRecord.ToString().ToLower()%>,
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
                                        dataField: "IsTaxFoundCol",
                                        caption: '<%=Resources.Labels.DgTaxHeader%>',
                                        allowEditing: true,
                                        dataType: "boolean",
                                        alignment: "center",
                                        width: 60,
                                        visible: <%=this.MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>,
                                    },
                                    {
                                        dataField: "TaxIdCol",
                                        caption: '<%=Resources.Labels.DgTaxHeader%>',
                                        dataType: "string",
                                        lookup: {
                                            dataSource: DevExpress.data.AspNet.createStore({
                                                key: "ID",
                                                loadUrl: "../api/Tax/GetTaxes",
                                                loadMethod: "get",
                                                loadParams: { 'contextKey': '' },
                                                displayExpr: "Name",
                                                valueExpr: "ID"
                                            }),
                                            displayExpr: "Name",
                                            valueExpr: "ID",
                                            searchEnabled: true,
                                            placeholder: '<%=Resources.Labels.Tax%>',
                                            onValueChanged: function (e) {
                                            },
                                            rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                        },
                                        allowEditing: false,
                                        alignment: "center",
                                        width: 120,
                                        visible: <%=this.MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>,
                                    },
                                    {
                                        dataField: "PercentTaxValueCol",
                                        caption: "<%=Resources.Labels.DgPercentTaxValueHeader%>",
                                        allowEditing: false,
                                        width: 100,
                                        dataType: "decimal",
                                        visible: <%=this.MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>,
                                    },
                                    {
                                        dataField: "ParentIdCol",
                                        caption: "ParentId",                                        
                                        allowEditing: false,
                                        visible: false,
                                        width: 100
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
                                                jqVar('#journalEntryGridContainer').dxDataGrid('instance').deleteRow(e.row.rowIndex);
                                                var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                                grid.saveEditData();
                                            }
                                        }]
                                    }                             
                                ],
                            remoteOperations: false,
                            onInitNewRow: function (e) { e.data.IsTaxFoundCol = false; },
                            keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: ekd /* "row"*/, editOnKeyPress: true },
                            onEditorPreparing: function OnEditorPreparing(e) {
                                var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                var component = e.component,
                                    rowIndex = e.row && e.row.rowIndex;

                                if (e.dataField == "AccountIdCol") {
                                    var onValueChanged = e.editorOptions.onValueChanged;
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (e.value != null && e.value != '') {
                                            jqVar.getJSON("../api/ChartOfAccount/FindById", { Source: e.value },
                                                function (response) {
                                                    if (response != null) {
                                                        component.cellValue(rowIndex, "AccountIdCol", response[0].ID);//CachedNumber
                                                        component.cellValue(rowIndex, "AccountNameCol", response[0].Name);
                                                    }
                                                    else {
                                                        var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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
                                            jqVar.getJSON("../api/ChartOfAccount/FindByName", { Source: e.value },
                                                function (response) {
                                                    if (response != null) {
                                                        component.cellValue(rowIndex, "AccountIdCol", response[0].ID);//CachedNumber
                                                        component.cellValue(rowIndex, "AccountNameCol", response[0].Name);
                                                    }
                                                    else {
                                                        var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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
                                else if (e.dataField == "DebitAmountCol") {
                                    var onValueChanged = e.editorOptions.onValueChanged;
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (e.value != null && e.value != '') {
                                            setTimeout(function () {
                                                component.cellValue(rowIndex, "CreditAmountCol", null);
                                            }, 5);
                                            //grid.cellValue(rowIndex, "CreditAmountCol", null);
                                        }
                                    }
                                }
                                else if (e.dataField == "CreditAmountCol") {
                                    var onValueChanged = e.editorOptions.onValueChanged;
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (e.value != null && e.value != '') {
                                            setTimeout(function () {
                                                component.cellValue(rowIndex, "DebitAmountCol", null);
                                            }, 5);
                                            //grid.cellValue(rowIndex, "DebitAmountCol", null);
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
                                                        var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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
                                                        var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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
                                else if (e.dataField == "IsTaxFoundCol") {
                                    var onValueChanged = e.editorOptions.onValueChanged;
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (e.value != null) {
                                            if (e.value == true) {
                                                if (component.cellValue(rowIndex, "TaxIdCol") != null && component.cellValue(rowIndex, "TaxIdCol") != '') {
                                                    jqVar.getJSON("../api/Tax/FindItem", { Id: component.cellValue(rowIndex, "TaxIdCol") },
                                                        function (response) {
                                                            if (response != null) {
                                                                var debit = 0.00, credit = 0.00, percentTaxValue = 0.00;
                                                                var taxInclude = 0.00;
                                                                var typeTax = jqVar("#TypeTax").val();
                                                                if (typeTax == 2) {
                                                                    if (response[0].PercentageValue) {
                                                                        var result = (response[0].PercentageValue / (100 + response[0].PercentageValue));
                                                                        taxInclude = result;
                                                                    }
                                                                    else {
                                                                        taxInclude = 0.00;
                                                                    }
                                                                }
                                                                else {
                                                                    if (response[0].PercentageValue != null) {
                                                                        var result = (response[0].PercentageValue / 100);
                                                                        taxInclude = result;
                                                                    }
                                                                    else {
                                                                        taxInclude = 0.00;
                                                                    }
                                                                }

                                                                if (component.cellValue(rowIndex, "DebitAmountCol"))
                                                                    debit = component.cellValue(rowIndex, "DebitAmountCol");

                                                                if (component.cellValue(rowIndex, "CreditAmountCol"))
                                                                    credit = component.cellValue(rowIndex, "CreditAmountCol");

                                                                if (debit > 0) {
                                                                    percentTaxValue = debit * taxInclude;
                                                                }
                                                                else if (credit > 0) {
                                                                    percentTaxValue = credit * taxInclude;
                                                                }
                                                                percentTaxValue = Math.round((percentTaxValue + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                                                                component.cellValue(rowIndex, "PercentTaxValueCol", percentTaxValue);
                                                            }
                                                            else {
                                                                var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                                                component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                                                grid.focus(grid.getCellElement(rowIndex, "PercentTaxValueCol"));
                                                            }
                                                        });
                                                }
                                                else component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                            }
                                            else {
                                                component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                                component.cellValue(rowIndex, "TaxIdCol", null);
                                            }
                                        }
                                        else {
                                            component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                            component.cellValue(rowIndex, "TaxIdCol", null);
                                        }
                                    }
                                }
                                else if (e.dataField == "TaxIdCol") {
                                    var onValueChanged = e.editorOptions.onValueChanged;
                                    e.editorOptions.onValueChanged = function (e) {
                                        onValueChanged.call(this, e);
                                        if (e.value != null && e.value != '') {
                                            if (component.cellValue(rowIndex, "IsTaxFoundCol") == true) {
                                                jqVar.getJSON("../api/Tax/FindItem", { Id: e.value },
                                                    function (response) {
                                                        if (response != null) {
                                                            var debit = 0.00, credit = 0.00, percentTaxValue = 0.00;
                                                            var taxInclude = 0.00;
                                                            var typeTax = jqVar("#TypeTax").val();
                                                            if (typeTax == 2) {
                                                                if (response[0].PercentageValue) {
                                                                    var result = (response[0].PercentageValue / (100 + response[0].PercentageValue));
                                                                    taxInclude = result;
                                                                }
                                                                else {
                                                                    taxInclude = 0.00;
                                                                }
                                                            }
                                                            else {
                                                                if (response[0].PercentageValue != null) {
                                                                    var result = (response[0].PercentageValue / 100);
                                                                    taxInclude = result;
                                                                }
                                                                else {
                                                                    //component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                                    taxInclude = 0.00;
                                                                }
                                                            }

                                                            if (component.cellValue(rowIndex, "DebitAmountCol"))
                                                                debit = component.cellValue(rowIndex, "DebitAmountCol");

                                                            if (component.cellValue(rowIndex, "CreditAmountCol"))
                                                                credit = component.cellValue(rowIndex, "CreditAmountCol");

                                                            if (debit > 0) {
                                                                percentTaxValue = debit * taxInclude;
                                                            }
                                                            else if (credit > 0) {
                                                                percentTaxValue = credit * taxInclude;
                                                            }
                                                            percentTaxValue = Math.round((percentTaxValue + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                                                            component.cellValue(rowIndex, "PercentTaxValueCol", percentTaxValue);
                                                        }
                                                        else {
                                                            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                                            component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                                            grid.focus(grid.getCellElement(rowIndex, "PercentTaxValueCol"));
                                                        }
                                                    });
                                            }
                                            else {
                                                component.cellValue(rowIndex, "PercentTaxValueCol", null);
                                            }
                                        }
                                        else {
                                            component.cellValue(rowIndex, "PercentTaxValueCol", null);
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
                                    var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();
                                    var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                    if (currentRowIndex == rows.length - 1 && validateResult.responseCode)
                                        AddRowBottom(true);
                                    else showErrorMessage(validateResult.responseMessage, null);
                                } else if (keyCode == 46 && currentRowIndex >= 0) {
                                    var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                    const focusedCellPosition = getCurrentCell(grid);
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
                /********************************************/
                jqVar.getJSON("../api/Operation/GetOperationTypes", null, function (operationTypeResponse) {
                    jqVar("#acOperationType").dxSelectBox({
                        dataSource: operationTypeResponse,
                        displayExpr: "Description",
                        valueExpr: "ID",
                        value: 8,
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.Type%>',
                        onValueChanged: function (data) {

                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                    }).dxSelectBox("instance");
                });
                /*******************************************/
                jqVar("#txtOperationDate").dxDateBox({
                    displayFormat: "dd/MM/yyyy",
                    value: new Date()
                });
            }).done(function () {               
                setEnabled(".executeBtn", false);
                setEnabled("#btnAdd", true);
                setEnabled("#btnSearch", true);
                setEnabled("#btnPrint", true);
                setEnabled("#txtOperationDate", true);
                setTimeout(function () {
                    setReadOnly("#ToolsPnl", true);
                    jqVar.getJSON('../api/GridOrdering/FindGridOrdering', { 'GridId': '<%=DocumentKindClass.JournalEntry%>', 'UserId': '<%=MyContext.UserProfile.Contact_ID%>' }, function (response) {
                        if (response.length == 1) {
                            var array = JSON.parse(response[0].JsonVal);
                            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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

        function clearDgv() {
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
            var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();
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
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
            var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1,
                operation = jqVar("#Operation").val(), documentId = jqVar("#Id").val(),
                docDate = jqVar("#txtOperationDate").dxDateBox("instance").option('text').trim();

            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') &&
                        (rows[i].data["AccountNameCol"] == null || rows[i].data["AccountNameCol"] == '') &&
                        (rows[i].data["DebitAmountCol"] == null || rows[i].data["DebitAmountCol"] == '') &&
                        (rows[i].data["CreditAmountCol"] == null || rows[i].data["CreditAmountCol"] == '') &&
                        (rows[i].data["DescriptionCol"] == null || rows[i].data["DescriptionCol"] == '') &&
                        (rows[i].data["CostCenterNameCol"] == null || rows[i].data["CostCenterNameCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["VendorsSecondnameCol"] == null || rows[i].data["VendorsSecondnameCol"] == '') &&
                        (rows[i].data["CostCenterIdCol"] == null || rows[i].data["CostCenterIdCol"] == '') &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '') &&
                        (rows[i].data["IsTaxFoundCol"] == null || rows[i].data["IsTaxFoundCol"] == '') &&
                        (rows[i].data["PercentTaxValueCol"] == null || rows[i].data["PercentTaxValueCol"] == '') &&
                        (rows[i].data["TaxIdCol"] == null || rows[i].data["TaxIdCol"] == '') &&
                        (rows[i].data["ParentIdCol"] == null || rows[i].data["ParentIdCol"] == '')
                    )
                        isEmptyRow = true;
                }

                if (rows[i].rowType == "data" && (skipEmptyRows == false || isEmptyRow == false)) {
                    if (rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgAccountRequired%>";
                        cellName = "AccountNameCol";
                    }
                    else if ((rows[i].data["DebitAmountCol"] == null || rows[i].data["DebitAmountCol"] == '') &&
                        (rows[i].data["CreditAmountCol"] == null || rows[i].data["CreditAmountCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDebitOrCreditRequired%>";
                        cellName = "DebitAmountCol";
                    }                    
                    else if (<%=MyContext.JournalEntryOptions.UseCostCenterByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceCostCenterByRecord.ToString().ToLower()%>== true &&
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
                    else if (<%=MyContext.JournalEntryOptions.UseVendorsSecondByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceVendorsSecondByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocVendorRequired%>";
                        cellName = "VendorsSecondIdCol";
                    }
                    else if (<%=MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceVatByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["TaxIdCol"] == null || rows[i].data["TaxIdCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgTaxRequired%>";
                        errorMessage = "<%=Resources.Messages.MsgTaxRequired%>";
                        cellName = "TaxIdCol";
                    }
                    else if (<%=MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>== true &&
                             <%=MyContext.JournalEntryOptions.ForceVatByRecord.ToString().ToLower()%>== true &&
                        (rows[i].data["PercentTaxValueCol"] == null || rows[i].data["PercentTaxValueCol"] == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgTaxRequired%>";
                        cellName = "PercentTaxValueCol";
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
                jqVar("#notifyview").html(errorMessage);
                jqVar("#notifymsgdiv").removeClass("alert-success");
                jqVar("#notifymsgdiv").addClass("alert-danger");
                jqVar("#notifymsgdiv").fadeIn();
                showModal("#notifyModal");
                jqVar("#notifymsgdiv").fadeOut(3000, function () {
                    hideModal("#notifyModal");
                    if (rowIndex != -1 && cellName != null && cellName != '')
                        grid.focus(grid.getCellElement(rowIndex, cellName));
                })
                if (rowIndex != -1 && cellName != null && cellName != '')
                    grid.focus(grid.getCellElement(rowIndex, cellName));
                return false;
            }
            else return true;

        }

        function AddRowBottom(focusRow) {
            var validateResult = ValidateRows(false);
            if (validateResult == true) {

                var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                var cols = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();
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

                rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["AccountIdCol"] = dataRows[i].AccountIdCol;
                    rows[i].data["AccountNameCol"] = dataRows[i].AccountNameCol;
                    rows[i].data["DebitAmountCol"] = dataRows[i].DebitAmountCol;
                    rows[i].data["CreditAmountCol"] = dataRows[i].CreditAmountCol;
                    rows[i].data["DescriptionCol"] = dataRows[i].DescriptionCol;
                    rows[i].data["CostCenterNameCol"] = dataRows[i].CostCenterNameCol;
                    rows[i].data["DocNameCol"] = dataRows[i].DocNameCol;
                    rows[i].data["OperDateCol"] = dataRows[i].OperDateCol;
                    rows[i].data["VendorsSecondnameCol"] = dataRows[i].VendorsSecondnameCol;
                    rows[i].data["CostCenterIdCol"] = dataRows[i].CostCenterIdCol;
                    rows[i].data["VendorsSecondIdCol"] = dataRows[i].VendorsSecondIdCol;
                    rows[i].data["IsTaxFoundCol"] = dataRows[i].IsTaxFoundCol;
                    rows[i].data["PercentTaxValueCol"] = dataRows[i].PercentTaxValueCol;
                    rows[i].data["TaxIdCol"] = dataRows[i].TaxIdCol;
                    rows[i].data["ParentIdCol"] = dataRows[i].ParentIdCol;
                    rows[i].data["IdCol"] = dataRows[i].IdCol;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "AccountIdCol"));
                SetDataGridEditable(true);
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
            if (<%=MyContext.JournalEntryOptions.KeepCostCenter.ToString().ToLower()%>!= true)
                jqVar("#acCostCenter").dxSelectBox('instance').option({ value: null });

            if (<%=MyContext.JournalEntryOptions.KeepDocDate.ToString().ToLower()%>!= true) {
                jqVar.getJSON("../api/General/GetServerDate", null, function (response) {
                    jqVar("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response) });
                });
            }

            jqVar("#DocRandomString").val(null);
            setReadOnly("#ToolsPnl", true);
        }

        function SetDataGridEditable(value) {
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");            
            var columns = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleColumns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].allowEditing = value;
                if (columns[i].name == "RowNoCol")
                    columns[i].allowEditing = false;
            }
        }

        function addItem() {
            //clearTools();
            setEnabled(".executeBtn", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            jqVar("#Operation").val("Add");
            SetDataGridEditable(true);            
        }

        function findItem() {
            jqVar("#searchDiv").load("../Accounting/FrmJournalEntrySelect.aspx?requestCode=1");
            showModal('#searchModal');
        }

        function documentSelected(requestCode, selectedDocumentId) {                        
            if (requestCode == 1) {
                clearTools();
                hideModal('#searchModal');
                jqVar.getJSON("../api/JournalEntry/FindOperationById", { Operation_ID: selectedDocumentId },
                    function (response) {
                        if (response.length == 1) {
                            if (response[0].DocStatus_ID == 1)
                                setEnabled("#btnEdit", true);
                            else setEnabled("#btnEdit", false);
                            setEnabled("#btnApprove", true);
                            setEnabled("#btnReset", true);
                            setEnabled("#btnAdd", false);
                            //jqVar("#LocalId").val(response[0].LocalId);
                            //jqVar("#AccountId").val(response[0].AccountId);   
                            jqVar("#txtRatio").val(response[0].Ratio);
                            jqVar("#acBranch").dxSelectBox('instance').option({ value: response[0].Branch_ID });
                            
                            jqVar("#acCostCenter").dxSelectBox('instance').option({ value: response[0].CostCenter_ID });
                            jqVar("#txtUserRefNo").val(response[0].UserRefNo);

                            setTimeout(function () {
                                jqVar("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response[0].EntryDate) });
                            }, 100);

                            jqVar("#DocRandomString").val(response[0].DocRandomString);
                            jqVar("#imgStatusDiv").css("background-image", getImgStatus(response[0].DocStatus_ID));
                            jqVar("#Id").val(selectedDocumentId);
                            setEnabled("#btnPrint", true);
                            jqVar.getJSON("../api/JournalEntry/GetEntryDetails", { Operation_ID: selectedDocumentId },
                                function (responseTable) {
                                    var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
                                    //var cols = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleColumns();
                                    var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();

                                    var length = rows.length;
                                    for (var i = length - 1; i >= 0; i--) {
                                        grid.deleteRow(i);
                                    }

                                    length = responseTable.length;
                                    for (var i = 0; i < length + 1; i++) {
                                        grid.addRow(i);
                                    }

                                    rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();

                                    for (var i = 0; i < responseTable.length; i++) {
                                        rows[i].data["AccountIdCol"] = responseTable[i].Account_ID;
                                        rows[i].data["AccountNameCol"] = responseTable[i].AccountName;
                                        rows[i].data["DebitAmountCol"] = responseTable[i].DebitForeignAmount;//DebitAmount;
                                        rows[i].data["CreditAmountCol"] = responseTable[i].CreditForeignAmount;//CreditAmount;

                                        rows[i].data["DescriptionCol"] = responseTable[i].Description;
                                        rows[i].data["CostCenterNameCol"] = responseTable[i].CostCenterName;
                                        rows[i].data["DocNameCol"] = responseTable[i].DocName;
                                        rows[i].data["OperDateCol"] = responseTable[i].OperDateString;//OperDate
                                        rows[i].data["VendorsSecondnameCol"] = responseTable[i].VendorsSecondname;
                                        rows[i].data["CostCenterIdCol"] = responseTable[i].CostCenter_ID;
                                        rows[i].data["VendorsSecondIdCol"] = responseTable[i].VendorsSecond_ID;
                                        rows[i].data["IsTaxFoundCol"] = responseTable[i].IsTaxFound;
                                        rows[i].data["PercentTaxValueCol"] = responseTable[i].PercentTaxValue;
                                        rows[i].data["TaxIdCol"] = responseTable[i].Tax_ID;
                                        rows[i].data["ParentIdCol"] = responseTable[i].Parent_ID;

                                        rows[i].data["IdCol"] = responseTable[i].ID;
                                    }
                                    grid.refresh();
                                });
                        }
                    });
            }
            else if (requestCode == 2) {
                hideModal('#searchModal');
                var printTemplateId = jqVar("#PrintTemplateId option:selected").val();
                window.open('../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&DocumentId=' + selectedDocumentId + "&DocKindId=" + '<%=DocumentKindClass.JournalEntry%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
            }
        }

        function showOperationDetials(SourceDocumentId) {
            jqVar("#operationDetailsDiv").load("../OperationDetails/FrmOperationDetials.aspx?SourceDocId=" + SourceDocumentId + "&SourceTableId=" +<%=DocumentKindClass.JournalEntry%>);
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
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
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
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");

            if (dateBox.option('text') == null || dateBox.option('text') == '') {                
                showErrorMessage("<%=Resources.Messages.MsgDocOperationDateRequired%>", "#txtOperationDate");
                return false;
            }
            else if (jqVar("#acBranch").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocBranchRequired%>", "#acBranch");
                return false;
            }
            else if (validateResult == false) {
                return false;
            }
            else {
                var operation = jqVar("#Operation").val();
                var debitAmount = null, creditAmount = null;
                var editMode = false;
                if (operation == "Edit")
                    editMode = true;

                var branchId = jqVar("#acBranch").dxSelectBox('instance').option('value'),
                    costCenterId = jqVar("#acCostCenter").dxSelectBox('instance').option('value'),
                    userRefNo = jqVar("#txtUserRefNo").val(),
                    ratio = jqVar("#txtRatio").val(),
                    docDate = dateBox.option('text').trim(),
                    operationType = jqVar("#acOperationType").dxSelectBox('instance').option('value'),
                    currencyId = '<%=ddlCurrency.SelectedValue%>', docRandomString = $("#DocRandomString").val();

                var totalDebitAmountSum = grid.getTotalSummaryValue('TotalDebitAmountSum');
                var totalCreditAmountSum = grid.getTotalSummaryValue('TotalCreditAmountSum');

                var rows = jqVar("#journalEntryGridContainer").dxDataGrid("instance").getVisibleRows();
                var dataRows = [];

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].data["AccountIdCol"] == null || rows[i].data["AccountIdCol"] == '') &&
                        (rows[i].data["AccountNameCol"] == null || rows[i].data["AccountNameCol"] == '') &&
                        (rows[i].data["DebitAmountCol"] == null || rows[i].data["DebitAmountCol"] == '') &&
                        (rows[i].data["CreditAmountCol"] == null || rows[i].data["CreditAmountCol"] == '') &&
                        (rows[i].data["DescriptionCol"] == null || rows[i].data["DescriptionCol"] == '') &&
                        (rows[i].data["CostCenterNameCol"] == null || rows[i].data["CostCenterNameCol"] == '') &&
                        (rows[i].data["DocNameCol"] == null || rows[i].data["DocNameCol"] == '') &&
                        (rows[i].data["OperDateCol"] == null || rows[i].data["OperDateCol"] == '') &&
                        (rows[i].data["VendorsSecondnameCol"] == null || rows[i].data["VendorsSecondnameCol"] == '') &&
                        (rows[i].data["CostCenterIdCol"] == null || rows[i].data["CostCenterIdCol"] == '') &&
                        (rows[i].data["VendorsSecondIdCol"] == null || rows[i].data["VendorsSecondIdCol"] == '') &&
                        (rows[i].data["IsTaxFoundCol"] == null || rows[i].data["IsTaxFoundCol"] == '') &&
                        (rows[i].data["PercentTaxValueCol"] == null || rows[i].data["PercentTaxValueCol"] == '') &&
                        (rows[i].data["TaxIdCol"] == null || rows[i].data["TaxIdCol"] == '') &&
                        (rows[i].data["ParentIdCol"] == null || rows[i].data["ParentIdCol"] == '')
                    )
                        isEmptyRow = true;

                    if (rows[i].rowType == "data" && isEmptyRow == false) {
                        var accountId = null, accountName = null, debitAmount = null, creditAmount = null, description = null, costCenterName = null,
                            docName = null, operDate = null, vendorsSecondname = null, vendorsSecondId = null, isTaxFound = null,
                            typeTax = jqVar("#TypeTax").val(), percentTaxValue = null, taxId = null, parentId = null,
                            rowCostCenterId = costCenterId;

                        if (rows[i].data["AccountIdCol"] != null)
                            accountId = rows[i].data["AccountIdCol"];
                        if (rows[i].data["DebitAmountCol"] != null)
                            debitAmount = rows[i].data["DebitAmountCol"];
                        if (rows[i].data["CreditAmountCol"] != null)
                            creditAmount = rows[i].data["CreditAmountCol"];

                        if (<%=MyContext.JournalEntryOptions.UseNotesByRecord.ToString().ToLower()%>== true && rows[i].data["DescriptionCol"] != null)
                            description = rows[i].data["DescriptionCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocNameByRecord.ToString().ToLower()%>== true && rows[i].data["DocNameCol"] != null)
                            docName = rows[i].data["DocNameCol"];
                        if (<%=MyContext.JournalEntryOptions.UseDocDateByRecord.ToString().ToLower()%>== true && rows[i].data["OperDateCol"] != null)
                            operDate = rows[i].data["OperDateCol"];
                        if (<%=MyContext.JournalEntryOptions.UseVendorsSecondByRecord.ToString().ToLower()%>== true && rows[i].data["VendorsSecondIdCol"] != null)
                            vendorsSecondId = rows[i].data["VendorsSecondIdCol"];

                        if (<%=MyContext.JournalEntryOptions.UseVatByRecord.ToString().ToLower()%>== true) {
                            if (rows[i].data["IsTaxFoundCol"] != null)
                                isTaxFound = rows[i].data["IsTaxFoundCol"];
                            if (rows[i].data["PercentTaxValueCol"] != null)
                                percentTaxValue = rows[i].data["PercentTaxValueCol"];
                            if (rows[i].data["TaxIdCol"] != null)
                                taxId = rows[i].data["TaxIdCol"];
                        }

                        if (rows[i].data["ParentIdCol"] != null)
                            parentId = rows[i].data["ParentIdCol"];

                        if (<%=MyContext.JournalEntryOptions.UseCostCenterByRecord.ToString().ToLower()%>== true)
                            rowCostCenterId = rows[i].data["CostCenterIdCol"];

                        var idCol = rows[i].data["IdCol"];

                        var row = {
                            'ID': idCol, 'Account_ID': accountId, 'DebitAmount': debitAmount, 'CreditAmount': creditAmount,
                            'DebitForeignAmount': debitAmount, 'CreditForeignAmount': creditAmount, 'Description': description,
                            'DocName': docName, 'OperDate': operDate, 'CostCenter_ID': rowCostCenterId, 'VendorsSecond_ID': vendorsSecondId,
                            'IsTaxFound': isTaxFound, 'TypeTax': typeTax, 'PercentTaxValue': percentTaxValue, 'TaxId': taxId, 'ParentId': parentId
                        };
                        dataRows.push(row);
                        //dataRows.push(rows[i].data);
                    }
                }

                dataRows = JSON.stringify(dataRows);
                var id = jqVar("#Id").val();
                jqVar.post("../api/JournalEntry/SaveJournalEntry", {
                    'Id': id, 'BranchId': branchId, 'CostCenterId': costCenterId, 'UserRefNo': userRefNo,
                    'CurrencyId': currencyId, 'OperationDate': docDate, 'Ratio': ratio, 'OperationType': operationType, 'DocRandomString': docRandomString,
                    'TotalDebit': totalDebitAmountSum, 'TotalCredit': totalCreditAmountSum,
                    'EditMode': editMode, 'UserProfileContact_ID': '<%=MyContext.UserProfile.Contact_ID%>',
                    'IsApproving': isApproving, 'Source': dataRows
                },
                    function (response) {
                        if (response.Code > 0) {
                            clearTools();
                            //jqVar("#DocRandomString").val(response[0].DocRandomString)
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
                                var printTemplateId = jqVar("#PrintTemplateId option:selected").val();
                                window.open('../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&DocumentId=' + response.Code + "&DocKindId=" + '<%=DocumentKindClass.JournalEntry%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                                if (<%=MyContext.JournalEntryOptions.AddNewAfterSave.ToString().ToLower()%>== true)
                                    addItem();
                            });

                            //------------------------ btnSendEmail ---------------------------//
                            jqVar("#btnSendEmail").click(function () {
                                jqVar.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateEmail(queryResponse) == true) {
                                        jqVar.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>', "SendType": 1 }, function (sendResponse) {
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
                                                jqVar.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>', "SendType": 2, "Data": jqVar('#txtEmailTo').val() }, function (sendResponse) {
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
                                jqVar.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateMobile(queryResponse) == true) {
                                        jqVar.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>', "SendType": 1 }, function (sendResponse) {
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
                                                jqVar.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.JournalEntry%>', "SendType": 2, "Data": jqVar('#txtMobileTo').val() }, function (sendResponse) {
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
                                if (<%=MyContext.JournalEntryOptions.AddNewAfterSave.ToString().ToLower()%>== true)
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
                jqVar("#searchDiv").load("../Accounting/FrmJournalEntrySelect.aspx?requestCode=2");
                showModal('#searchModal');
            }
        }

        function saveGridOrdering() {
            var grid = jqVar("#journalEntryGridContainer").dxDataGrid("instance");
            var colCount = grid.columnCount();
            var columnIndicies = [];
            for (var i = 0; i < colCount; i++) {
                var visibleIndex = grid.columnOption(i, "visibleIndex");
                if (visibleIndex >= 0)
                    columnIndicies.push({ index: visibleIndex, fieldName: grid.columnOption(i, "dataField") });
            }

            jqVar.post("../api/GridOrdering/SaveGridOrdering", {
                'GridId': '<%=DocumentKindClass.JournalEntry%>',
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <input type="hidden" id="Operation" />
        <input type="hidden" id="Id" />       
        <input id="TypeTax" type="hidden" value="<%= this.TypeTax.ToString() %>" />
        <input id="DocRandomString" type="hidden" />
        <input type="hidden" id="EntryType" value="1" />        
        <input type="hidden" id="SourceTypeId" value="<%=DocumentKindClass.JournalEntry %>" />

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
                        <button type="button" id="btnLoad" class="printResult btn btn-success btn-lg" style="position:absolute;float:right;right:5px;"><i class="fa fa-folder-open" aria-hidden="true"></i> <%=Resources.Labels.Load%></button>
                        <button type="button" id="btnConfirmPrint" class="printResult btn btn-secondary btn-lg"><i class="fa fa-print" aria-hidden="true"></i> <%=Resources.Labels.Print%></button>
                        <button type="button" id="btnSendEmail" class="printResult btn btn-primary btn-lg"><i class="fa fa-solid fa-envelope"></i> <%=Resources.Labels.SendEmail%></button>
                        <button type="button" id="btnSendSms" class="printResult btn btn-info btn-lg"><i class="fa fa-solid fa-sms"></i> <%=Resources.Labels.SendSms%></button>
                        <button type="button" id="btnSendWhatsapp" style="display:none" class="printResult btn btn-success btn-lg"><i class="fa fa-brands fa-whatsapp"></i> <%=Resources.Labels.SendWhatsapp%></button>
                        <button type="button" id="btnCancelPrint" class="printResult btn btn-danger btn-lg" data-bs-dismiss="modal"><i class="fa fa-close"></i> <%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Search Modal -->
        <div class="modal fade" id="searchModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel"><%=Resources.Labels.SelectJournalEntry %></h5>
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
                <div class="InvoiceHeader row" style="height:auto;">
                    <asp:Nav runat="server" ID="ucNav" />
                    <%--<asp:Favorit runat="server" ID="ucFavorit" />--%>
                </div>

                <div class="row" style="margin-left: 20px; margin-right: 20px;">
                    <div class="col-md-12">
                        <div id="ToolsPnl" class="row">
                            <div class="col-md-4">
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

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label4"><%=Resources.Labels.CostCenter %></label>
                                        <div id="acCostCenter" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span><%=Resources.Labels.ApprovedBy %></span>:
                                        <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                                        <label runat="server" id="Label20"><%=Resources.Labels.Ratio %></label>
                                        <input type="text" id="txtRatio" disabled="disabled" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label14"><%=Resources.Labels.Date %></label>
                                        <div id="txtOperationDate" class="dxDatePic"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />
                                        <label runat="server" id="Label5"><%=Resources.Labels.UserRefNo %></label>
                                        <input id="txtUserRefNo" type="text" class="form-control" placeholder="<%=Resources.Labels.UserRefNo %>" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label3"><%=Resources.Labels.Type %></label>
                                        <div id="acOperationType" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="dxGridParent">
                        <div id="journalEntryGridContainer" class="dxGrid col-md-12"></div>
                    </div>
                </div>

                <div id="BtnsRow" class="row" style="margin-right: 2px; padding-left: 15px;">     
                    <%if (MyContext.PageData.IsAdd)
                        {%>
                            <button type="button" id="btnAdd" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="addItem()"><%=Resources.Labels.AddNew %> <i class="fa fa-plus-square" aria-hidden="true"></i></button>
                        <%}
                    %>

                    <div class="col-md-8" style="text-align: center">
                        <button type="button" id="btnSave" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="saveItem(false)"><%=Resources.Labels.Save %> <i class="fa fa-save" aria-hidden="true"></i></button>

                        <%if (MyContext.PageData.IsApprove)
                            {%>
                                <button type="button" id="btnApprove" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="saveItem(true)"><%=Resources.Labels.Approve %> <i class="fa fa-check" aria-hidden="true"></i></button>
                            <%}
                        %>

                        <%if (MyContext.PageData.IsPrint)
                            {%>
                                <button type="button" id="btnPrint" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="printItem()"><%=Resources.Labels.Print %> <i class="fa fa-print" aria-hidden="true"></i></button>
                            <%}
                        %>

                        <button type="button" id="btnSearch" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="findItem()"><%=Resources.Labels.Search %><i class="fa fa-search" aria-hidden="true"></i></button>

                        <%if (MyContext.PageData.IsEdit)
                            {%>
                                <button type="button" id="btnEdit" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="editItem()"><%=Resources.Labels.Edit %><i class="fa fa-edit" aria-hidden="true"></i></button>
                            <%}
                        %>

                        <%if (MyContext.PageData.IsDelete)
                            {%>
                                <button type="button" id="btnDelete" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="deleteItem()"><%=Resources.Labels.Delete %><i class="fa fa-remove" aria-hidden="true"></i></button>
                            <%}
                        %>
                    </div>

                    <button type="button" id="btnReset" class="executeBtn btn bot-buffer btn-sm col-md-2" onclick="resetItem()"><%=Resources.Labels.Clear %><i class="fa fa-undo" aria-hidden="true"></i></button>
                    <%if (MyContext.PageData.AllowReorderGrid)
                        {%>
                            <button type="button" id="btnSaveGrid" class="btn bot-buffer btn-sm col-md-1" onclick="saveGridOrdering()"><%=Resources.Labels.SaveOrdering %></button>
                        <%}
                    %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
