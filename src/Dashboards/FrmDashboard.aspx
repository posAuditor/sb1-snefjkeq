<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="FrmDashboard.aspx.cs" Inherits="Dashboards_FrmDashboard" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/DevextremeStyle.css" rel="stylesheet" />
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />

    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>
    <!-- DevExtreme theme -->

    <link href="../Content/devextreme/dx.light.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn3.devexpress.com/jslib/21.1.5/css/dx.common.css" />

    <!-- DevExtreme library -->
    <script type="text/javascript" src="../Content/devextreme/dx.all.js"></script>
    <%--<script type="text/javascript" src="../Content/devextreme/dx.web.js"></script>
    <script type="text/javascript" src="../Content/devextreme/dx.viz.js"></script>--%>
    <script type="text/javascript" src="../Content/devextreme/dx.aspnet.data.js"></script>

    <%--<script type="text/javascript">
        var obj = <%=Convert.ToByte(this.MyContext.CurrentCulture)%>;
        console.log("obj:" + obj);
        if (obj == 0 || obj == "0") {
            if (!IsMobile())
                document.write('<link href="../Styles/DevextremeStyleRTL.css" rel="stylesheet" />');
            document.write('<link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />');
        }
    </script>--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.js"></script>


    <script type="text/javascript">
        var jqVar = jQuery.noConflict();
        jqVar(document).ready(function () {
            jqVar("#MainTitle").hide();
            jqVar("#pageTitleLbl").text(jqVar("#lblPageTitle").text());
            var now = new Date();
            let day = now.getDate();
            let month = now.getMonth() + 1;
            let year = now.getFullYear();

            const currentDate = '${day}-${month}-${year}';
            //console.log(currentDate);

            var beforMonth = '01/01/' +<%=DateTime.Now.Year%>; //now.setMonth(now.getMonth() - 1);

            jqVar("#txtSalesFromDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: beforMonth
            });

            jqVar("#txtSalesToDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: new Date()
            });

            jqVar("#txtPurchaseFromDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: beforMonth
            });

            jqVar("#txtPurchaseToDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: new Date()
            });

            jqVar("#txtSalesPMFromDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: beforMonth
            });

            jqVar("#txtSalesPMToDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: new Date()
            });

            jqVar('#saleBranchCbx').on('click', function () {
                var state = jqVar(this).prop('checked');
                jqVar("#saleBranch").prop('disabled', !state);
            });

            jqVar('#purchaseBranchCbx').on('click', function () {
                var state = jqVar(this).prop('checked');
                jqVar("#purchaseBranch").prop('disabled', !state);
            });

            jqVar('#salePMBranchCbx').on('click', function () {
                var state = jqVar(this).prop('checked');
                jqVar("#salePMBranch").prop('disabled', !state);
            });

            jqVar.getJSON("../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                jqVar("#saleBranch").html('');
                var s = '<option value="-1" disabled selected>-- <%=Resources.Labels.Branch%> --</option>';
                for (var i = 0; i < branchesResponse.length; i++) {
                    s += '<option value="' + branchesResponse[i].ID + '">' + branchesResponse[i].Name + '</option>';
                }

                jqVar("#saleBranch").append(s);
                jqVar("#saleBranch").val("-1");
                jqVar("#saleBranch").val(<%=this.MyContext.UserProfile.Branch_ID %>);

                jqVar("#purchaseBranch").html('');

                jqVar("#purchaseBranch").append(s);
                jqVar("#purchaseBranch").val("-1");
                jqVar("#purchaseBranch").val(<%=this.MyContext.UserProfile.Branch_ID %>);


                jqVar("#salePMBranch").html('');

                jqVar("#salePMBranch").append(s);
                jqVar("#salePMBranch").val("-1");
                jqVar("#salePMBranch").val(<%=this.MyContext.UserProfile.Branch_ID %>);

            });

            setTimeout(function () {
                loadSalesChart(2);
                loadPurchaseChart(2);

                loadSalesPMChart(2);
                loadSalesTableByDate(2);
                loadSalesTableByBranch(2);
              //  parent.onloadMainIframeWait(3000);

            }, 1000);
        });

        function loadSalesChart(typeId) {
            jqVar("#salesChartTypeId").val(typeId);
            var fromDate = null;
            var toDate = null;
            var isSaleBranch = jqVar("#saleBranchCbx").prop("checked");
            var branchId = null;
            var now = new Date();
            let day = now.getDate();
            let month = now.getMonth() + 1;
            let year = now.getFullYear();

            const currentDate = `${year}/${month}/${day}`;
            if (typeId == 2) {
                fromDate = currentDate;
                toDate = currentDate;
            }
            else if (typeId == 6) {
                fromDate = jqVar("#txtSalesFromDate").dxDateBox("instance").option('text').trim();
                toDate = jqVar("#txtSalesToDate").dxDateBox("instance").option('text').trim();
            }

            if (isSaleBranch == true && jqVar("#saleBranch").val() != -1 && jqVar("#saleBranch option:selected").val() != "-1")
                branchId = jqVar("#saleBranch option:selected").val();

            jqVar.post('../api/Dashboard/GetDashboardSales', {
                //'TypeId': typeId,
                'FromDate': fromDate, 'ToDate': toDate, 'BranchId': branchId, 'Process': 4
            }, function (response) {
                const types = ['spline', 'stackedline', 'fullstackedline'];
                jqVar(() => {

                    const chart = jqVar('#salesChart').dxChart({
                        palette: 'soft',
                        dataSource: response,
                        commonSeriesSettings: {
                            argumentField: 'BranchName',
                            type: 'bar',
                            hoverMode: 'allArgumentPoints',
                            selectionMode: 'allArgumentPoints',
                            label: {
                                visible: true,
                                format: {
                                    type: 'fixedPoint',
                                    precision: 0,
                                },
                            },
                        },
                          
                        export: {
                            enabled: true,
                        },
                        series: [
                            { valueField: 'SumItemInvoiceCost', name: '<%=Resources.Labels.Sales%>', color:'#000080' },
                            { valueField: 'SumItemReceiptCost', name: '<%=Resources.Labels.Cost%>', color: '#FF0000' },
                            { valueField: 'SumExpin', name: '<%=Resources.Labels.Profits%>', color: '#28a745', },

                        ],
                        tooltip: {
                            enabled: true,
                            customizeTooltip(arg) {
                                 var total = arg.total / 2;
                                var percent = arg.value / total *100;
                                return {
                                    
                                    text: '${percent} %',
                                };
                            },
                        },


                    }).dxChart('instance');
                });

            });
        }

        function loadPurchaseChart(typeId) {
            jqVar("#purchaseChartTypeId").val(typeId);
            var fromDate = null;
            var toDate = null;
            var now = new Date();
            let day = now.getDate();
            let month = now.getMonth() + 1;
            let year = now.getFullYear();

            const currentDate = '${year}/${month}/${day}';
            if (typeId == 2) {
                fromDate = currentDate;
                toDate = currentDate;
            }
            var isPurchaseBranch = jqVar("#purchaseBranchCbx").prop("checked");
            var branchId = null;
            if (typeId == 6) {
                fromDate = jqVar("#txtPurchaseFromDate").dxDateBox("instance").option('text').trim();
                toDate = jqVar("#txtPurchaseToDate").dxDateBox("instance").option('text').trim();
            }
            if (isPurchaseBranch == true && jqVar("#purchaseBranch").val() != -1 && jqVar("#purchaseBranch option:selected").val() != "-1")
                branchId = jqVar("#purchaseBranch option:selected").val();

            jqVar.post('../api/Dashboard/GetDashboardSales', {
                //'TypeId': typeId,
                'FromDate': fromDate, 'ToDate': toDate, 'BranchId': branchId, 'Process': 5
            }, function (response) {
                const types = ['spline', 'stackedline', 'fullstackedline'];
                jqVar(() => {
                    const types = ['spline', 'stackedline', 'fullstackedline'];
                   
                    const chart = jqVar('#purchaseChart').dxChart({
                        palette: 'soft',
                        dataSource: response,
                        commonSeriesSettings: {
                            ignoreEmptyPoints: true,
                            argumentField: 'BranchName',
                            type: 'bar', 
                            label: {
                                visible: true,
                                format: {
                                    type: 'fixedPoint',
                                    precision: 0,
                                },
                            },
                        },
                        
                        series: [
                            {
                                valueField: 'SumItemInvoiceCost',
                                name: '<%=Resources.Labels.Sales%>',
                                color: '#000080'
                            },
                            {
                                valueField: 'SumItemReceiptCost',
                                name: '<%=Resources.Labels.Cost%>',
                                color: '#FF0000'
                            },
                            {
                                valueField: 'SumExpin',
                                name: '<%=Resources.Labels.Profits%>',
                                color: '#28a745'
                            },
                        ],
 

                         
                        export: {
                            enabled: true,
                        },
                        tooltip: {
                            enabled: true,
                            customizeTooltip(arg) {
                                var total = arg.total / 2;
                                var percent = arg.value / total * 100;
                                return {

                                    text: `${percent} %`,
                                };
                            },
                        },
 


                    }).dxChart('instance');
                });
            });
        }

        function loadSalesPMChart(typeId) {
            jqVar("#salesPMChartTypeId").val(typeId);
            var fromDate = null;
            var toDate = null;
            var isSalePMBranch = jqVar("#salePMBranchCbx").prop("checked");
            var branchId = null;
            if (typeId == 6) {
                fromDate = jqVar("#txtSalesPMFromDate").dxDateBox("instance").option('text').trim();
                toDate = jqVar("#txtSalesPMToDate").dxDateBox("instance").option('text').trim();
            }

            if (isSalePMBranch == true && jqVar("#salePMBranch").val() != -1 && jqVar("#salePMBranch option:selected").val() != "-1")
                branchId = jqVar("#salePMBranch option:selected").val();

            jqVar.post('../api/Dashboard/GetDashboardSalesAndPurchase', {
                'TypeId': typeId, 'FromDate': fromDate, 'ToDate': toDate, 'BranchId': branchId, 'Process': 2
            }, function (response) {
                jqVar(() => {
                    jqVar('#salesPMBarChart').dxChart({
                        palette: 'bright',
                        dataSource: response,
                        commonSeriesSettings: {
                            argumentField: 'CurrencyIsoName',
                            valueField: 'TotalAmount',
                            type: 'bar',
                        },
                        seriesTemplate: {
                            nameField: 'PaymentMethodName',
                        },
                        legend: {
                            verticalAlignment: 'bottom',
                            horizontalAlignment: 'center',
                        },
                    }).dxChart('instance');
                    //////////////////////////////
                    /*var saDS = response.find(item => item.CurrencyId === 15);
                    console.log(saDS);*/
                    jqVar('#salesPMPieChart').dxPieChart({
                        size: {
                            width: 500,
                        },
                        palette: 'bright',
                        dataSource: response,
                        commonSeriesSettings: {
                            argumentField: 'CurrencyIsoName',
                            valueField: 'TotalAmount',
                            type: 'pie',
                        },
                        series: [
                            {
                                argumentField: 'PaymentMethodName',
                                valueField: 'TotalAmount',
                                label: {
                                    visible: true,
                                    connector: {
                                        visible: true,
                                        width: 1,
                                    },
                                },
                            },
                        ],
                        /*onPo                        intClick(e) {
                            const point = e.target;

                            toggleVisibility(point);
                        },
                        onLegendClick(e) {
                            const arg = e.target;

                            toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                        },*/
                    }).dxPieChart('instance');
                });

            });
        }

        function loadSalesTableByDate(typeId) {
            jqVar("#salesTableTypeId").val(typeId);
            var fromDate = null;
            var toDate = null;
            var now = new Date();
            let day = now.getDate();
            let month = now.getMonth() + 1;
            let year = now.getFullYear();

            const currentDate = `${year}/${month}/${day}`;
            if (typeId == 2) {
                fromDate = currentDate;
                toDate = currentDate;
            }
            var isPurchaseBranch = jqVar("#purchaseBranchCbx").prop("checked");
            var branchId = null;
            if (typeId == 6) {
                fromDate = jqVar("#txtPurchaseFromDate").dxDateBox("instance").option('text').trim();
                toDate = jqVar("#txtPurchaseToDate").dxDateBox("instance").option('text').trim();
            }
            if (isPurchaseBranch == true && jqVar("#purchaseBranch").val() != -1 && jqVar("#purchaseBranch option:selected").val() != "-1")
                branchId = jqVar("#purchaseBranch option:selected").val();

            //const date = new Date();
            //let day = date.getDate();
            //let month = date.getMonth() + 1;
            //let year = date.getFullYear();

            //let currentDate = `${day}-${month}-${year}`;
            //console.log(currentDate);  

            jqVar.post('../api/Dashboard/GetDashboardSales', {
                'FromDate': fromDate, 'ToDate': toDate, 'BranchId': branchId, 'Process': 6
            }, function (response) {
                jqVar(() => {

                    const chart = jqVar('#salesTableByDate').dxDataGrid({
                        dataSource: response,
                        keyExpr: 'ItemName',
                        allowColumnReordering: true,
                        showBorders: true,
                     
                        searchPanel: {
                            visible: true,
                        },
                        paging: {
                            pageSize: 10,
                        },
                        groupPanel: {
                            visible: true,
                        },
                        sortByGroupSummaryInfo: [{
                            summaryItem: 'count',
                        }],
                        columns: [

                            {
                                caption: '<%=Resources.Labels.Date%>',
                                dataField: 'OperationDate',
                                groupIndex: 1,
                            },
                            {
                                caption: '<%=Resources.Labels.Branch%>',
                                dataField: 'BranchName',
                                width: 160,
                                 groupIndex: 0,
                            },
                            {
                                caption: '<%=Resources.Labels.Item%>',
                                dataField: 'ItemName',
                            },
                            {
                                caption: '<%=Resources.Labels.Sales %>',
                                dataField: 'SumItemInvoiceCost',
                            },
                            {
                                caption: '<%=Resources.Labels.Cost%>',
                                dataField: 'SumItemReceiptCost',
                            },
                            {
                                caption: '<%=Resources.Labels.Profits%>',
                                dataField: 'SumExpin',
                                sortIndex: 0,
                                sortOrder: "desc"
                            },
                        ],
                        summary: {
                            groupItems: [
                                {
                                    column: 'SumItemInvoiceCost',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                },
                                {
                                    column: 'SumItemReceiptCost',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                },
                                {
                                    column: 'SumExpin',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                }
                            ],
                            totalItems: [{
                                column: 'SumItemInvoiceCost',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            },
                            {
                                column: 'SumItemReceiptCost',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            },
                            {
                                column: 'SumExpin',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            }],
                        },
                        export: {
                            enabled: true,
                             formats: ['EXCEL'],
                          
                         },
                        onExporting(e) {
                           // console.log(e);
                            const workbook = new ExcelJS.Workbook();
                            var worksheet = workbook.addWorksheet('Main sheet', { state: 'visible' });

                            DevExpress.excelExporter.exportDataGrid({
                                component: e.component,
                                worksheet,
                                autoFilterEnabled: true,
                            }).then(() => {
                                workbook.xlsx.writeBuffer().then((buffer) => {
                                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), '<%=Resources.Labels.Detailedsalesbyitems%>.xlsx');
                                });
                            });
                            e.cancel = true;
                        },
                    }).dxDataGrid('instance');

                });

            });
        }

        function loadSalesTableByBranch(typeId) {
            jqVar("#salesTableTypeId").val(typeId);
            var fromDate = null;
            var toDate = null;
            var now = new Date();
            let day = now.getDate();
            let month = now.getMonth() + 1;
            let year = now.getFullYear();

            const currentDate = `${year}/${month}/${day}`;
            if (typeId == 2) {
                fromDate = currentDate;
                toDate = currentDate;
            }
            var isPurchaseBranch = jqVar("#purchaseBranchCbx").prop("checked");
            var branchId = null;
            if (typeId == 6) {
                fromDate = jqVar("#txtPurchaseFromDate").dxDateBox("instance").option('text').trim();
                toDate = jqVar("#txtPurchaseToDate").dxDateBox("instance").option('text').trim();
            }
            if (isPurchaseBranch == true && jqVar("#purchaseBranch").val() != -1 && jqVar("#purchaseBranch option:selected").val() != "-1")
                branchId = jqVar("#purchaseBranch option:selected").val();
            jqVar.post('../api/Dashboard/GetDashboardSales', {
                'FromDate': fromDate, 'ToDate': toDate, 'BranchId': branchId, 'Process': 6
            }, function (response) {
                jqVar(() => {

                    const chart = jqVar('#salesTableByBranch').dxDataGrid({
                        dataSource: response,
                        keyExpr: 'BranchName',
                        allowColumnReordering: true,
                        showBorders: true,

                        searchPanel: {
                            visible: true,
                        },
                        paging: {
                            pageSize: 10,
                        },
                        groupPanel: {
                            visible: true,
                        },
                        columns: [
                       {

                           caption: '<%=Resources.Labels.Item%>',
                           dataField: 'ItemName',
                           width: 160,
                       }, {
                           caption: '<%=Resources.Labels.Sales%>',
                           dataField: 'SumItemInvoiceCost',
                       }, {
                           caption: '<%=Resources.Labels.Cost%>',
                           dataField: 'SumItemReceiptCost',
                       }, {
                           caption: '<%=Resources.Labels.Profits%>',
                                dataField: 'SumExpin', sortIndex: 0,
                                sortOrder: "desc"
                       },

                            {
                                caption: '<%=Resources.Labels.Branch%>',
                                dataField: 'BranchName',
                                groupIndex: 0,
                            },
                        ],

                        summary: {
                            groupItems: [
                                {
                                    column: 'SumItemInvoiceCost',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                },
                                {
                                    column: 'SumItemReceiptCost',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                },
                                {
                                    column: 'SumExpin',
                                    summaryType: 'sum',
                                    showInGroupFooter: false,
                                    alignByColumn: true,
                                }
                            ],
                            totalItems: [{
                                column: 'SumItemInvoiceCost',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            },
                            {
                                column: 'SumItemReceiptCost',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            },
                            {
                                column: 'SumExpin',
                                summaryType: 'sum',
                                showInGroupFooter: false,
                                alignByColumn: true,
                            }],
                        },
                        export: {
                            enabled: true,
                            allowExportSelectedData: true,
                        },
                        onExporting(e) {
                            const workbook = new ExcelJS.Workbook();
                            var worksheet = workbook.addWorksheet('Main sheet', { state: 'visible' });

                            DevExpress.excelExporter.exportDataGrid({
                                component: e.component,
                                worksheet,
                                autoFilterEnabled: true,
                            }).then(() => {
                                workbook.xlsx.writeBuffer().then((buffer) => {
                                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), '<%=Resources.Labels.ItemSalesByBranch%>.xlsx');
                                });
                            });
                            e.cancel = true;
                        },
                    }).dxDataGrid('instance');

                });

            });
        }

        function toggleVisibility(item) {
            if (item.isVisible()) {
                item.hide();
            } else {
                item.show();
            }
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

        .popupdialog {
            zoom: 80%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <input type="hidden" id="salesChartTypeId" />
    <input type="hidden" id="purchaseChartTypeId" />
    <input type="hidden" id="salesPMChartTypeId" />
    <input type="hidden" id="salesTableTypeId" />

    <div class="container-fluid" style="background-color: white;">
       <div class="row">
            <div class="col-md-6">
                <div class="card" style="margin: 0px;">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-3" style="display: none">
                                <select class="form-select form-select-sm" onchange="loadSalesChart(this.value)">
                                    <option value="-1" disabled="disabled">-- <%=Resources.Labels.Select %> --</option>
                                    <option value="2" selected="selected"><%=Resources.Labels.ThisWeek %></option>
                                    <option value="3"><%=Resources.Labels.ThisMonth %></option>
                                    <option value="4"><%=Resources.Labels.ThisQuarterOfTheYear %></option>
                                    <option value="5"><%=Resources.Labels.ThisYear %></option>
                                </select>
                            </div>
                            <div class="col-md-4">
                                <button class="btn btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    <%=Resources.Labels.SelectTwoDates %>
                                </button>
                                <div class="popupdialog dropdown-menu">
                                    <div style="padding-right: 10px; padding-left: 10px;">
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.From %></label>
                                            <div type="text" class="form-control col-md-8" id="txtSalesFromDate"></div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.To %></label>
                                            <div type="text" class="form-control col-md-8" id="txtSalesToDate"></div>
                                        </div>
                                        <div style="text-align: center">
                                            <a href="#" class="col-md-6 btn btn-primary" style="color: white" onclick="loadSalesChart(6);"><%=Resources.Labels.Search %></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" style="display: none">
                                <div class="row">
                                    <label class="form-check-label col-md-5" style="padding-right: 30px">
                                        <input type="checkbox" class="form-check-input" id="saleBranchCbx" checked="checked" />
                                        <%=Resources.Labels.Branch %>
                                    </label>
                                    <select id="saleBranch" class="col-md-7 form-select form-select-sm"></select>
                                </div>
                            </div>

                            <div class="col-md-1">
                                <a href="javascript:void(0);" onclick="loadSalesChart(jqVar('#salesChartTypeId').val())">
                                    <img alt="" src="../Images/Restart.png" class="img-responsive" style="width: 20px; height: 20px;" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: center">
                        <h4><%=Resources.Labels.Dailysalesreport%></h4>
                    </div>
                    <div id="salesChart">
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card" style="margin: 0px;">
                    <div class="card-header">
                        <div class="row" style="display: none">
                            <div class="col-md-3">
                                <select class="form-select form-select-sm" onchange="loadPurchaseChart(this.value)">
                                    <option value="-1" disabled="disabled">-- <%=Resources.Labels.Select %> --</option>
                                    <%--<option value="1"><%=Resources.Labels.Today %></option>--%>
                                    <option value="2" selected="selected"><%=Resources.Labels.ThisWeek %></option>
                                    <option value="3"><%=Resources.Labels.ThisMonth %></option>
                                    <option value="4"><%=Resources.Labels.ThisQuarterOfTheYear %></option>
                                    <option value="5"><%=Resources.Labels.ThisYear %></option>
                                    <%--<option value="6"><%=Resources.Labels.CustomDate %></option>--%>
                                </select>
                            </div>

                            <div class="col-md-4">
                                <button class="btn btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    <%=Resources.Labels.SelectTwoDates %>
                                </button>
                                <div class="popupdialog dropdown-menu">
                                    <div style="padding-right: 10px; padding-left: 10px;">
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.DateFrom %></label>
                                            <div type="text" class="form-control col-md-8" id="txtPurchaseFromDate"></div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.DateTo %></label>
                                            <div type="text" class="form-control col-md-8" id="txtPurchaseToDate"></div>
                                        </div>
                                        <div style="text-align: center">
                                            <a href="#" class="col-md-6 btn btn-primary" style="color: white" onclick="loadPurchaseChart(6);"><%=Resources.Labels.Search %></a>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row">
                                    <label class="form-check-label col-md-5" style="padding-right: 30px">
                                        <input type="checkbox" class="form-check-input" id="purchaseBranchCbx" checked="checked" />
                                        <%=Resources.Labels.Branch %>
                                    </label>
                                    <select id="purchaseBranch" class="col-md-7 form-select form-select-sm"></select>
                                </div>
                            </div>

                            <div class="col-md-1">
                                <a href="javascript:void(0);" onclick="loadPurchaseChart(6);">
                                    <img alt="" src="../Images/Restart.png" class="img-responsive" style="width: 20px; height: 20px;" />
                                </a>
                            </div>
                        </div>
                    </div>

                    <div style="text-align: center">
                        <h4><%=Resources.Labels.Detailedsalesreportbybranch%></h4>
                    </div>

                    <div id="purchaseChart">
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="card" style="margin: 0px;">
                    <div style="text-align: center">
                        <h4><%=Resources.Labels.Detailedsalesbyitems%></h4>
                    </div>
                    <div id="salesTableByDate">
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card" style="margin: 0px;">
                    <div style="text-align: center">
                        <h4><%=Resources.Labels.ItemSalesByBranch%></h4>
                    </div>
                    <div id="salesTableByBranch">
                    </div>
                </div>
            </div>


        </div>


        <div class="row" style="display: none">
            <div class="col-md-12">
                <div class="card" style="margin: 0px;">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-3">
                                <select class="form-select form-select-sm" onchange="loadSalesPMChart(this.value)">
                                    <option value="-1" disabled="disabled">-- <%=Resources.Labels.Select %> --</option>
                                    <%--<option value="1"><%=Resources.Labels.Today %></option>--%>
                                    <option value="2" selected="selected"><%=Resources.Labels.ThisWeek %></option>
                                    <option value="3"><%=Resources.Labels.ThisMonth %></option>
                                    <option value="4"><%=Resources.Labels.ThisQuarterOfTheYear %></option>
                                    <option value="5"><%=Resources.Labels.ThisYear %></option>
                                    <%--<option value="6"><%=Resources.Labels.CustomDate %></option>--%>
                                </select>
                            </div>

                            <div class="col-md-4">
                                <button class="btn btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    <%=Resources.Labels.SelectTwoDates %>
                                </button>
                                <div class="popupdialog dropdown-menu">
                                    <div style="padding-right: 10px; padding-left: 10px;">
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.DateFrom %></label>
                                            <div type="text" class="form-control col-md-8" id="txtSalesPMFromDate"></div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="control-label col-md-3"><%=Resources.Labels.DateTo %></label>
                                            <div type="text" class="form-control col-md-8" id="txtSalesPMToDate"></div>
                                        </div>
                                        <div style="text-align: center">
                                            <a href="#" class="col-md-6 btn btn-primary" style="color: white" onclick="loadSalesPMChart(6);"><%=Resources.Labels.Search %></a>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row">
                                    <label class="form-check-label col-md-5" style="padding-right: 30px">
                                        <input type="checkbox" class="form-check-input" id="salePMBranchCbx" checked="checked" />
                                        <%=Resources.Labels.Branch %>
                                    </label>
                                    <select id="salePMBranch" class="col-md-7 form-select form-select-sm"></select>
                                </div>
                            </div>

                            <div class="col-md-1">

                                <a href="javascript:void(0);" onclick="loadSalesPMChart(6)">
                                    <img alt="" src="../Images/Restart.png" class="img-responsive" style="width: 20px; height: 20px;" />
                                </a>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: center">
                        <h4><%=Resources.Labels.InvoiceByPaymentMethodDiagram%></h4>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div id="salesPMBarChart">
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div id="salesPMPieChart">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>