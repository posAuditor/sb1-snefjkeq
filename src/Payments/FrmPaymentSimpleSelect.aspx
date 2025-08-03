<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPaymentSimpleSelect.aspx.cs" Inherits="Payments_FrmPaymentSimpleSelect" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/SearchPageStyle.css" rel="stylesheet" />
    <script>
        jqVar(document).ready(function () {
            jqVar('#SearchText').on("keyup", function () {
                var value = jqVar(this).val().toLowerCase();
                jqVar("#searchDgvDiv .dx-row .dx-data-row .dx-row-lines .dx-column-lines").filter(function () {
                    jqVar(this).toggle(jqVar(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            jqVar("#txtSerial").on("input", function () {                
            });

            jqVar(<%=ddlStatus.ClientID%>).change(function () {                
            });
            const date = new Date();
            let day = date.getDate();
            let month = date.getMonth();
            let year = date.getFullYear();

            var currentDate = new Date(year, month, day);
            jqVar("#txtDateFrom").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: currentDate
            });


            jqVar("#txtDateTo").dxDateBox({
                displayFormat: "dd/MM/yyyy"
            });
           
            jqVar("#Currency").dxSelectBox({
                dataSource: DevExpress.data.AspNet.createStore({
                    key: "ID",
                    loadUrl: "../api/General/GetCurrencies",
                    loadMethod: "get"
                }),
                displayExpr: "Name",
                valueExpr: "ID",
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Currency%>',
                onValueChanged: function (data) {
                    filterAccounts();
                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
            }).dxSelectBox("instance");

            jqVar.getJSON("../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                    jqVar("#Branch").dxSelectBox({
                        dataSource: branchesResponse,
                        displayExpr: "Name",
                        valueExpr: "ID",
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.Branch%>',
                        onValueChanged: function (data) {
                            filterAccounts();
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                    }).dxSelectBox("instance");
            });

            setTimeout(function () {
                filterAccounts();
            }, 1000);            
        });

        function filterAccounts() {
            var DebitParent_ID = '<%=string.Empty%>';
            var CreditParent_ID = '<%=string.Empty%>';
            var Currency_ID = '<%=string.Empty%>';
            var branchId = '<%=string.Empty%>';
            var p='/<%=Request.QueryString["p"]%>';            
            console.log("p:" + p);

            switch(p){
                case "/CashIn":
                    DebitParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    jqVar("#txtBillNo").hide();
                    break;

                case "/CashInCustomer":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Customers.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    jqVar("#txtBillNo").show();
                    break;

                case "/CashOut":
                    CreditParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    jqVar("#txtBillNo").hide();
                    break;

                case "/CashOutVendor":
                    CreditParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Vendors.ToString()%>';
                    jqVar("#txtBillNo").show();
                    break;

                case "/BankDeposit":
                    DebitParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    jqVar("#txtBillNo").hide();
                    break;
                case "/BankDepositCustomer":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Customers.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    jqVar("#txtBillNo").show();
                    break;
                case "/BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    jqVar("#txtBillNo").hide();
                    break;

                case "/BankWithdrawVendor":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Vendors.ToString()%>';
                    jqVar("#txtBillNo").show();
                    break;
            }

            if (jqVar("#Currency").dxSelectBox('instance').option('value') != null)
                Currency_ID = jqVar("#Currency").dxSelectBox('instance').option('value');
            if (jqVar("#Branch").dxSelectBox('instance').option('value') != null)
                branchId = jqVar("#Branch").dxSelectBox('instance').option('value');

            var CreditAccountContextKey = '<%=this.GetCurrentCulture()%>' + "," + branchId + "," + Currency_ID + "," + CreditParent_ID + ",true";
            var DebitAccountContextKey = '<%=this.GetCurrentCulture()%>' + "," + branchId + "," + Currency_ID + "," + DebitParent_ID + ",true";            
                        
            jqVar.getJSON("../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': DebitAccountContextKey }, function (exResponse) {
                var debitAccounts = [];
                for (i = 0; i < exResponse.length; i++) {
                    debitAccounts.push(exResponse[i]);
                }

                jqVar("#DebitAccount").dxSelectBox({
                    dataSource: {
                        store: {
                            type: 'array',
                            data: debitAccounts,
                            key: "ID"
                        },
                        pageSize: 5,
                        paginate: true
                    },
                    displayExpr: "Name",
                    valueExpr: "ID",
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.DebitAccount%>',
                    onValueChanged: function (data) {
                    },
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");
            });

            jqVar.getJSON("../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': CreditAccountContextKey }, function (exResponse) {
                var creditAccounts = [];
                for (i = 0; i < exResponse.length; i++) {
                    creditAccounts.push(exResponse[i]);
                }

                jqVar("#CreditAccount").dxSelectBox({
                    dataSource: {
                        store: {
                            type: 'array',
                            data: creditAccounts,
                            key: "ID"
                        },
                        pageSize: 5,
                        paginate: true
                    },
                    displayExpr: "Name",
                    valueExpr: "ID",
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.CreditAccount%>',
                    onValueChanged: function (data) {
                    },
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");
            });

            jqVar("#btnSearchFind").show();
        }

        function refreshData() {
            var p = null, currency = null, status = null, branch = null,
                searchText = '', debitAccount = null, creditAccount = null,
                dateFrom = null, dateTo = null, userRefNo = null, billNo = null,
                preferedCulture =<%=this.GetCurrentCulture()%>,
                requestCode =<%=Request.QueryString["requestCode"] %>,
                userProfile = '<%=this.GetContactID()%>';

            if ('<%=Request.QueryString["p"]%>' != null && '<%=Request.QueryString["p"]%>' != '')
                p = '/<%=Request.QueryString["p"]%>';
            if (jqVar("#Currency").dxSelectBox('instance').option('value') != null)
                currency = jqVar("#Currency").dxSelectBox('instance').option('value');
            if (jqVar(<%=ddlStatus.ClientID%>).val() != null)
                status = jqVar(<%=ddlStatus.ClientID%>).val();
            if (jqVar("#Branch").dxSelectBox('instance').option('value') != null)
                branch = jqVar("#Branch").dxSelectBox('instance').option('value');
            if (jqVar("#SearchText").val() != null && jqVar("#SearchText").val() != '')
                searchText = jqVar("#SearchText").val();
            if (jqVar("#DebitAccount").dxSelectBox('instance').option('value') != null)
                debitAccount = jqVar("#DebitAccount").dxSelectBox('instance').option('value');
            if (jqVar("#CreditAccount").dxSelectBox('instance').option('value') != null)
                creditAccount = jqVar("#CreditAccount").dxSelectBox('instance').option('value');
            if (jqVar("#txtDateFrom").dxDateBox("instance").option('text').trim() != null && jqVar("#txtDateFrom").dxDateBox("instance").option('text').trim() != '')
                dateFrom = jqVar("#txtDateFrom").dxDateBox("instance").option('text').trim();
            if (jqVar("#txtDateTo").dxDateBox("instance").option('text').trim() != null && jqVar("#txtDateTo").dxDateBox("instance").option('text').trim() != '')
                dateTo = jqVar("#txtDateTo").dxDateBox("instance").option('text').trim();
            if (jqVar("#txtUserRefNo").val() != null && jqVar("#txtUserRefNo").val() != '')
                userRefNo = jqVar("#txtUserRefNo").val();
            if (jqVar("#txtBillNo").val() != null && jqVar("#txtBillNo").val() != '')
                billNo = jqVar("#txtBillNo").val();            

            var customDataSource = new DevExpress.data.CustomStore({
                key: "ID",
                load: function (loadOptions) {
                    var d = jqVar.Deferred();
                    var params = {};
                    [
                        "filter",
                        "group",
                        "groupSummary",
                        "parentIds",
                        "requireGroupCount",
                        "requireTotalCount",
                        "searchExpr",
                        "searchOperation",
                        "searchValue",
                        "select",
                        "sort",
                        "skip",
                        "take",
                        "totalSummary",
                        "userData"
                    ].forEach(function (i) {
                        if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                            params[i] = JSON.stringify(loadOptions[i]);
                        }
                    });

                    jqVar.post('../../api/PaymentSimple/Search', {
                        'p': p, 'Currency': currency, 'Status': status, 'Branch': branch, 'SearchText': searchText,
                        'DebitAccount': debitAccount, 'CreditAccount': creditAccount, 'DateFrom': dateFrom, 'DateTo': dateTo,
                        'UserRefNo': userRefNo, 'BillNo': billNo, 'PreferedCulture': preferedCulture, 'requestCode': requestCode,
                        'UserProfile': userProfile, 'loadOptions': JSON.stringify(loadOptions)
                    })
                        .done(function (response) {
                            d.resolve(response.data, {
                                totalCount: response.totalCount,
                                summary: response.summary,
                                groupCount: response.groupCount
                            });
                        })
                        .fail(function () { throw "Data loading error" });
                    return d.promise();
                },
            });

            jqVar("#searchDgvDiv").dxDataGrid({
                columnFixing: {
                    enabled: true
                },
                columnAutoWidth: true,
                showBorders: true,
                showRowLines: true,
                dataSource: customDataSource,
                filterRow: {
                    visible: true,
                    applyFilter: 'auto',
                },
                headerFilter: {
                    visible: true,
                },
                columns:
                    [
                        {
                            dataField: "ID",
                            caption: "ID",
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                            visible: false
                        },
                        {
                            dataField: "RowNum",
                            caption: '<%=Resources.Labels.Serial%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "Serial",
                            caption: '<%=Resources.Labels.NbInvoice%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                            calculateCellValue: function (rowData) {
                                if (rowData.Serial === null || rowData.Serial == null || rowData.Serial === '' || rowData.Serial == '')
                                    return '<%=Resources.Labels.Draft%>';
                                return rowData.Serial;
                            }
                        },
                        {
                            dataField: "OperationDate",
                            caption: '<%=Resources.Labels.Date%>',
                            allowEditing: false,
                            dataType: "date",
                            alignment: "center",
                            format: "dd-MM-yyyy"
                        },
                        {
                            dataField: "TotalAmount",
                            caption: '<%=Resources.Labels.Total%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                            format: "decimal",
                        },
                        {
                            dataField: "DebitAccountNames",
                            caption: '<%=Resources.Labels.Debit%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "CreditAccountNames",
                            caption: '<%=Resources.Labels.Credit%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "CeartedByName",
                            caption: '<%=Resources.Labels.UserName%>',
                            allowEditing: false,                            
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "BranchName",
                            caption: '<%=Resources.Labels.Branch%>',
                            allowEditing: false,                            
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "DocStatus",
                            caption: '<%=Resources.Labels.Status%>',
                            allowEditing: false,                            
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "RealtedBillSerial",
                            caption: '<%=Resources.Labels.BillNo%>',
                            allowEditing: false,                            
                            dataType: "string",
                            alignment: "center",
                        },                        
                        {
                            type: "buttons",
                            width: 50,
                            cssClass: 'selectDoc',
                            buttons: [{
                                text: '<%=Resources.Labels.Select%>',                                
                                onClick(e) {
                                    documentSelected('<%=Request.QueryString["requestCode"] %>', e.row.data.ID);
                                }
                            }]
                        },
                        {
                            type: "buttons",
                            width: 50,
                            cssClass: 'qaidDoc',
                            buttons: [{
                                text: '<%=Resources.Labels.Qaid%>',
                                visible: function (e) {
                                    return !e.row.isEditing && (e.row.data.DocStatus_ID == 2);
                                },
                                onClick(e) {
                                    showOperationDetials(e.row.data.ID);
                                }
                            }]
                        }, {
                            type: "buttons",
                            width: 100,
                            cssClass: 'btnDelete',
                            buttons: [{
                                text: '<%=Resources.Labels.cancelApprove%>',
                                visible: function (e) {
                                    return !e.row.isEditing && (e.row.data.DocStatus_ID == 2);
                                },
                                onClick(e) {
                                    console.log(e);
                                    CancelPaymentSimpleApprovel(e.row.data.ID);
                                    $(e.event.currentTarget).hide();
                                    //refreshData();
                                }
                            }]
                        },

                        {
                            type: "buttons",
                            width: 70,
                            cssClass: 'printDoc',
                            buttons: [{
                                text: '<%=Resources.Labels.Print%>',
                                visible: function (e) {
                                    //return !e.row.isEditing && (e.row.data.DocStatus_ID == 2);
                                    return true;
                                },
                                onClick(e) {
                                    documentSelected(2, e.row.data.ID);
                                }
                            }]
                        }
                    ],
                paging: {
                    pageSize: 10,
                },
                pager: {
                    visible: true,
                    allowedPageSizes: [5, 10, 15, 20, 100, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                remoteOperations: { groupPaging: true },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
            });
        }

        function isNotEmpty(value) {
            return value !== undefined && value !== null && value !== "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">        
        <div class="row" >
            <div class="col-md-4"></div>
            <a href="javascript:void(0)" class="col-md-2 btn searchBtn" onclick="jqVar('#pnlSearch').toggle();"><%=Resources.Labels.SearchOptions %></a>
            <a href="javascript:void(0)" id="btnSearchFind" class="col-md-2 btn searchBtn" style="display:none;" onclick="refreshData();"><%=Resources.Labels.Search%></a>
        </div>        
        <div id="pnlSearch" class="dx-viewport" style="display:block">
            <div class="row">
                <div class="col-md-3">
                    <label runat="server" id="Label1"><%=Resources.Labels.DateFrom %></label>
                    <div id="txtDateFrom"></div>
                </div>

                <div class="col-md-3">
                    <label runat="server" id="Label2"><%=Resources.Labels.DateTo %></label>
                    <div id="txtDateTo"></div>
                </div>

                <div class="col-md-3">
                    <label runat="server" id="Label3"><%=Resources.Labels.Currency %></label>
                    <div id="Currency" class="dxComb"></div>
                </div>
                
                <div class="col-md-3">
                    <label><%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-3">
                    <label runat="server" id="Label4"><%=Resources.Labels.Branch %></label>
                    <div id="Branch" class="dxComb"></div>
                </div>
                
                <div class="col-md-3">
                    <label runat="server" id="Label5"><%=Resources.Labels.Serial %></label>
                    <input id="txtSerial" type="text" class="form-control" placeholder="<%=Resources.Labels.Serial %>" />
                </div>
                
                <div class="col-md-3">
                    <label runat="server" id="Label6"><%=Resources.Labels.DebitAccount %></label>
                    <div id="DebitAccount" class="dxComb"></div>
                </div>

                <div class="col-md-3">
                    <label runat="server" id="Label7"><%=Resources.Labels.CreditAccount %></label>
                    <div id="CreditAccount" class="dxComb"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-3">
                    <label runat="server" id="Label8"><%=Resources.Labels.UserRefNo %></label>
                    <input id="txtUserRefNo" type="text" class="form-control" placeholder="<%=Resources.Labels.UserRefNo %>" />
                </div>

                <div class="col-md-3">
                    <label runat="server" id="Label9"><%=Resources.Labels.BillNo %></label>
                    <input id="txtBillNo" type="text" class="form-control" placeholder="<%=Resources.Labels.BillNo %>" />
                </div>
            </div>
        </div>

        <div id="mydivid">
            <div id="pr" class="inProgressDiv" style="display:none">
                <span class="WaitWord">                   
                    <div class="base_ouro" style="z-index: 100; opacity: 1;">
                        <p class="xalicircle" style="display: block;">
                            <span class="ouro">
                                <span class="left">
                                    <span class="anim"></span>
                                </span>
                                <span class="right">
                                    <span class="anim"></span>
                                </span>
                            </span>
                            <span>
                                <%= Resources.UserInfoMessages.PleaseWait %>
                            </span>
                        </p>
                    </div>
                </span>
            </div>
            <input id="SearchText" class="form-control" type="text" placeholder="<%=Resources.Labels.SearchHere %>" />
            <div id="searchDgvDiv" class="row">
            </div>
        </div>
    </form>
</body>
</html>