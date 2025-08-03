<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmInventoryTransferSelect.aspx.cs" Inherits="Inv_InventoryTransferUI_FrmInventoryTransferSelect" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/SearchPageStyle.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('#SearchText').on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#searchDgvDiv .dx-row .dx-data-row .dx-row-lines .dx-column-lines").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#txtSerial").on("input", function () {                
            });

            $(<%=ddlStatus.ClientID%>).change(function () {                
            });
            const date = new Date();
            let day = date.getDate();
            let month = date.getMonth();
            let year = date.getFullYear();

            var currentDate = new Date(year, month, day);
            $("#txtDateFrom").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: currentDate
            }); 

            $("#txtDateTo").dxDateBox({
                displayFormat: "dd/MM/yyyy"
            });
           
            $("#Currency").dxSelectBox({
                dataSource: DevExpress.data.AspNet.createStore({
                    key: "ID",
                    loadUrl: "../../api/General/GetCurrencies",
                    loadMethod: "get"
                }),
                displayExpr: "Name",
                valueExpr: "ID",
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Currency%>',
                onValueChanged: function (data) {
                    filterAccounts();
                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
            }).dxSelectBox("instance");

            $.getJSON("../../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                $("#Branch").dxSelectBox({
                        dataSource: branchesResponse,
                        displayExpr: "Name",
                        valueExpr: "ID",
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.Branch%>',
                        onValueChanged: function (data) {
                            filterAccounts();
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                    }).dxSelectBox("instance");
            });

            
                filterAccounts();
                       
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
                    $("#txtBillNo").hide();
                    break;

                case "/CashInCustomer":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Customers.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    $("#txtBillNo").show();
                    break;

                case "/CashOut":
                    CreditParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    $("#txtBillNo").hide();
                    break;

                case "/CashOutVendor":
                    CreditParent_ID = '<%=XPRESS.Common.COA.CashOnHand.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Vendors.ToString()%>';
                    $("#txtBillNo").show();
                    break;

                case "/BankDeposit":
                    DebitParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    $("#txtBillNo").hide();
                    break;
                case "/BankDepositCustomer":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Customers.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    $("#txtBillNo").show();
                    break;
                case "/BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    $("#txtBillNo").hide();
                    break;

                case "/BankWithdrawVendor":
                    CreditParent_ID = '<%=XPRESS.Common.COA.Banks.ToString()%>';
                    DebitParent_ID = '<%=XPRESS.Common.COA.Vendors.ToString()%>';
                    $("#txtBillNo").show();
                    break;
            }

            if ($("#Currency").dxSelectBox('instance').option('value') != null)
                Currency_ID = $("#Currency").dxSelectBox('instance').option('value');
            if ($("#Branch").dxSelectBox('instance').option('value') != null)
                branchId = $("#Branch").dxSelectBox('instance').option('value');

            var CreditAccountContextKey = '<%=this.GetCurrentCulture()%>' + "," + branchId + "," + Currency_ID + "," + CreditParent_ID + ",true";
            var DebitAccountContextKey = '<%=this.GetCurrentCulture()%>' + "," + branchId + "," + Currency_ID + "," + DebitParent_ID + ",true";            
                        
            $.getJSON("../../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': DebitAccountContextKey }, function (exResponse) {
                var debitAccounts = [];
                for (i = 0; i < exResponse.length; i++) {
                    debitAccounts.push(exResponse[i]);
                }

                $("#DebitAccount").dxSelectBox({
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

            $.getJSON("../../api/ChartOfAccount/GetChartOfAccounts", { 'contextKey': CreditAccountContextKey }, function (exResponse) {
                var creditAccounts = [];
                for (i = 0; i < exResponse.length; i++) {
                    creditAccounts.push(exResponse[i]);
                }

                $("#CreditAccount").dxSelectBox({
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

            $("#btnSearchFind").show();
        }

        function refreshData() {
            var p = null, currency = null, status = null, branch = null,
                searchText = '', txtSerial='', debitAccount = null, creditAccount = null,
                dateFrom = null, dateTo = null, userRefNo = null, billNo = null,
                preferedCulture =<%=this.GetCurrentCulture()%>,
                requestCode =<%=Request.QueryString["requestCode"] %>,
                userProfile = '<%=this.GetContactID()%>';

            if ('<%=Request.QueryString["p"]%>' != null && '<%=Request.QueryString["p"]%>' != '')
                p = '/<%=Request.QueryString["p"]%>';
            if ($("#Currency").dxSelectBox('instance').option('value') != null)
                currency = $("#Currency").dxSelectBox('instance').option('value');
            if ($(<%=ddlStatus.ClientID%>).val() != null)
                status = $(<%=ddlStatus.ClientID%>).val();
            if ($("#Branch").dxSelectBox('instance').option('value') != null)
                branch = $("#Branch").dxSelectBox('instance').option('value');
            if ($("#SearchText").val() != null && $("#SearchText").val() != '')
                searchText = $("#SearchText").val();
            if ($("#DebitAccount").dxSelectBox('instance').option('value') != null)
                debitAccount = $("#DebitAccount").dxSelectBox('instance').option('value');
            if ($("#CreditAccount").dxSelectBox('instance').option('value') != null)
                creditAccount = $("#CreditAccount").dxSelectBox('instance').option('value');
            if ($("#txtDateFrom").dxDateBox("instance").option('text').trim() != null && $("#txtDateFrom").dxDateBox("instance").option('text').trim() != '')
                dateFrom = $("#txtDateFrom").dxDateBox("instance").option('text').trim();
            if ($("#txtDateTo").dxDateBox("instance").option('text').trim() != null && $("#txtDateTo").dxDateBox("instance").option('text').trim() != '')
                dateTo = $("#txtDateTo").dxDateBox("instance").option('text').trim();
            if ($("#txtUserRefNo").val() != null && $("#txtUserRefNo").val() != '')
                userRefNo = $("#txtUserRefNo").val();
            if ($("#txtBillNo").val() != null && $("#txtBillNo").val() != '')
                billNo = $("#txtBillNo").val();
            if ($("#txtSerial").val() != null && $("#txtSerial").val() != '')
                txtSerial = $("#txtSerial").val();
            
            var customDataSource = new DevExpress.data.CustomStore({
                key: "ID",
                load: function (loadOptions) {
                    var d = $.Deferred();
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

                    $.post('../../api/InventoryTransfer/Search', {
                        'p': p, 'Currency': currency, 'Status': status, 'Branch': branch, 'SearchText': searchText,
                        'DebitAccount': debitAccount, 'CreditAccount': creditAccount, 'DateFrom': dateFrom, 'DateTo': dateTo,
                        'UserRefNo': userRefNo, 'BillNo': billNo, 'PreferedCulture': preferedCulture, 'requestCode': requestCode,
                        'UserProfile': userProfile, 'TxtSerial': txtSerial, 'loadOptions': JSON.stringify(loadOptions)
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

            $("#searchDgvDiv").dxDataGrid({
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
                            caption: 'S.N',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "Serial",
                            caption: '<%=Resources.Labels.Serial%>',
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
                            dataField: "BranchName",
                            caption: '<%=Resources.Labels.Branch%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "fromstore",
                            caption: '<%=Resources.Labels.FromStor%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "ToBranchName",
                            caption: '<%=Resources.Labels.toBransh%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "toomstore",
                            caption: '<%=Resources.Labels.TooStore%>',
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
            <a href="javascript:void(0)" class="col-md-2 btn searchBtn" onclick="$('#pnlSearch').toggle();"><%=Resources.Labels.SearchOptions %></a>
            <a href="javascript:void(0)" id="btnSearchFind" class="col-md-2 btn searchBtn"  onclick="refreshData();"><%=Resources.Labels.Search%></a>
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