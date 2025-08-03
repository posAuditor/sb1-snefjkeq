<%@ Page Title="" Language="C#" 
    AutoEventWireup="true" CodeFile="FrmPurchaseReturnInvoice.aspx.cs" Inherits="Purchases_PurchaseReturnUI_FrmPurchaseReturnInvoice" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigationJS.ascx" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhml">
<head runat="server">    
     

    <!-- DataGrid & DevExtreme Scripts -->
    <script type="text/javascript">
        var rowIdentifier = 0;
        var currentRowIndex = 0;
        var parentAccounts = [];
        var $ = jQuery.noConflict();

        function loadDivs() {
            if (<%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== true) {
                $("#acStoreDiv").hide();
            }

            if (<%=MyContext.PurchasesReturnOptions.UseRefNo.ToString().ToLower()%>== true) {
                $("#refNoDiv").show();
            }
            else $("#refNoDiv").hide();

            if (<%=MyContext.PurchasesReturnOptions.UseCostCenter.ToString().ToLower()%>== true) {
                $("#costCenterDiv").show();
            }
            else $("#costCenterDiv").hide();
        }

        $(document).ready(function () {

            SetDataGridEditable(false);
            $("button").css({ "background-image": "unset" });
            $("#CustomerItemNavDiv").show();
            $("#imgStatusDiv").css("background-image", getImgStatus(0));
            loadDevextremeLocalization();

            $("#MainTitle").hide();
            $("#pageTitleLbl").text('<%=this.MyContext.PageData.PageTitle%>');

            $('#cbxInvPerDiscount').on('click', function () {
                var state = $(this).prop('checked');
                if (state == true) {
                    document.getElementById("txtInvPerDiscount").disabled = false;
                }
                else {
                    document.getElementById("txtInvPerDiscount").disabled = true;
                    $("#txtInvPerDiscount").val(null);
                    $("#txtInvCashDiscount").val(null);
                    calcDocTotals();
                }
            });
            $("#txtInvPerDiscount").on('input', function (e) {
                var operation = $("#Operation").val();
                if ((operation == "Add" || operation == "Edit")) {
                    calcDocTotals();
                }
            });

            $("#txtInvCashDiscount").on('input', function (e) {
                var operation = $("#Operation").val();
                if ((operation == "Add" || operation == "Edit")) {
                    calcDocTotals();
                }
            });

            loadDivs();

            if (<%=MyContext.PurchasesReturnOptions.DiscountPerRow.ToString().ToLower()%>== true) {
                $(".discount").css('display', 'none');
            }

            $('#saveNewCustomer').click(function () {
                saveCustomerInfo();
            });

            //جعل حقل الإدخال لا تدعم الحروف
            /*$('.number').on('keypress', function (event) {
                var regex = new RegExp("[0-9]");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });*/

            //clearTools();

            loadItems();
            loadItemUnits();
            /*setTimeout(function () {
                loadCustomers();
            }, 1000);*/

            initializeAll();
        });

        function loadItemUnits(ItemId) {
            var cachedUnits = localStorage.getItem("units");
            if (cachedUnits) {
                let allUnits = [];
                allUnits = JSON.parse(cachedUnits);
                var lastUnitDate = localStorage.getItem("unitLastDate");
                if (!lastUnitDate) {
                    lastUnitDate = new Date();
                }
                $.post("../../api/ItemUnit/GetLastModifiedItemUnits", { 'ItemId': null, 'LastDate': lastUnitDate }, function (exResponse) {
                    lastUnitDate = '<%=DateTime.Now.ToString() %>';
                    for (var i = 0; i < exResponse.length; i++) {
                        var oldItem = allUnits.find(item => item.ID === exResponse[i].ID);
                        if (oldItem) {
                            const index = allUnits.indexOf(oldItem);
                            if (index > -1) { // only splice array when item is found                                
                                allUnits.splice(index, 1); // 2nd parameter means remove one item only                                
                            }
                        }
                        allUnits.push(exResponse[i]);
                    }

                    localStorage.setItem('units', JSON.stringify(allUnits));
                    localStorage.setItem('unitLastDate', lastUnitDate);
                });
            }
            else {
                let allUnits = [];
                var lastUnitDate = '<%=DateTime.Now.ToString() %>';
                $.getJSON("../../api/ItemUnit/GetItemUnits", { 'contextKey': null }, function (exResponse) {
                    for (i = 0; i < exResponse.length; i++) {
                        allUnits.push(exResponse[i]);
                    }
                    localStorage.setItem('units', JSON.stringify(allUnits));
                    localStorage.setItem('unitLastDate', lastUnitDate);
                });
            }
        }

        function loadItems() {
            var cachedItems = localStorage.getItem("items");
            if (cachedItems) {
                let allMyItems = [];
                allMyItems = JSON.parse(cachedItems);
                var lastDate = localStorage.getItem("itemLastDate");
                if (!lastDate) {
                    lastDate = new Date();
                }

                $.post("../../api/Item/GetLastModifiedItems", { 'contextKey': ',,,true,1', 'LastDate': lastDate }, function (exResponse) {
                    lastDate = '<%=DateTime.Now.ToString() %>';
                    for (var i = 0; i < exResponse.length; i++) {
                        var oldItem = allMyItems.find(item => item.ID === exResponse[i].ID);
                        if (oldItem) {
                            const index = allMyItems.indexOf(oldItem);
                            if (index > -1) { // only splice array when item is found
                                allMyItems.splice(index, 1); // 2nd parameter means remove one item only
                            }
                        }
                        allMyItems.push(exResponse[i]);
                    }

                    localStorage.setItem('items', JSON.stringify(allMyItems));
                    localStorage.setItem('itemLastDate', lastDate);
                });
            }
            else {
                let allMyItems = [];
                var lastDate = '<%=DateTime.Now.ToString() %>';
                $.getJSON("../../api/Item/GetItems", { 'contextKey': ',,,true,1' }, function (exResponse) {
                    for (i = 0; i < exResponse.length; i++) {
                        allMyItems.push(exResponse[i]);
                    }
                    localStorage.setItem('items', JSON.stringify(allMyItems));
                    localStorage.setItem('itemLastDate', lastDate);
                });
            }
        }

        function initializeAll(allItems) {
            $.getJSON("../../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                $("#cmbxBranch").dxSelectBox({
                    dataSource: branchesResponse,
                    displayExpr: "Name",
                    valueExpr: "ID",
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.Branch%>',
                    onValueChanged: function (data) {
                        $.getJSON("../../api/Structure/GetStorPermissionsGroup", { 'contextKey': $("#cmbxBranch").dxSelectBox('instance').option('value') + "," + '<%=this.MyContext.UserProfile.UserId%>' }, function (storesResponse) {
                            var selectedStoreId = null;
                            if (<%=MyContext.PurchasesReturnOptions.KeepStore.ToString().ToLower()%>== true &&
                                    <%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== false && storesResponse.length > 0)
                                selectedStoreId = storesResponse[0].ID;
                            $("#acStore").dxSelectBox({
                                dataSource: storesResponse,
                                displayExpr: "Name",
                                valueExpr: "ID",
                                value: selectedStoreId,
                                searchEnabled: true,
                                placeholder: '<%=Resources.Labels.Store%>',
                                visible: '<%=!MyContext.PurchasesReturnOptions.UseStoreByRecord%>',
                                onValueChanged: function (data) {

                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                }).dxSelectBox("instance")
                        });
                        ///////////////////////////////////////////////                                                
                        $.getJSON("../../api/CostCenter/GetCostCenters", { 'contextKey': '<%=this.GetCostCenterContextKey()%>' + $("#cmbxBranch").dxSelectBox('instance').option('value') }, function (costCentersResponse) {
                            var selectedCostCenterId = null;
                            if (<%=MyContext.PurchasesReturnOptions.KeepCostCenter.ToString().ToLower()%>== true && costCentersResponse.length > 0)
                                selectedCostCenterId = costCentersResponse[0].ID;
                            $("#cmbxCostCenter").dxSelectBox({
                                dataSource: costCentersResponse,
                                displayExpr: "Name",
                                valueExpr: "ID",
                                value: selectedCostCenterId,
                                searchEnabled: true,
                                placeholder: '<%=Resources.Labels.CostCenter%>',
                                visible: '<%=!MyContext.PurchasesReturnOptions.UseCostCenterByRecord%>',
                                onValueChanged: function (data) {

                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                }).dxSelectBox("instance")
                        });
                        ///////////////////////////////////////////////
                        $.getJSON("../../api/General/GetContact", { 'contextKey': "V," + $("#cmbxBranch").dxSelectBox('instance').option('value') + "," + <%=ddlCurrency.SelectedValue%> + "," }, function (vendorsResponse) {
                            var defaultVendorId = null;
                            if (<%=MyContext.PurchasesReturnOptions.KeepDefaultSupplier.ToString().ToLower()%>== true && vendorsResponse.length > 0)
                                defaultVendorId = vendorsResponse[0].ID;
                            $("#acVendor").dxSelectBox({
                                /*dataSource: new DevExpress.data.ArrayStore({
                                    data: vendorsResponse,
                                    key: 'ID',
                                    paginate: true,
                                    pageSize: 10
                                }),*/
                                dataSource: new DevExpress.data.DataSource({
                                    store: {
                                        type: "array",
                                        key: "ID",
                                        data: vendorsResponse,
                                    },
                                    key: 'ID',
                                    paginate: true,
                                    pageSize: 10
                                }),
                                ////////////////
                                displayExpr: "Name",
                                valueExpr: "ID",
                                searchEnabled: true,
                                value: defaultVendorId,
                                placeholder: '<%=Resources.Labels.Vendor%>',
                                <%--buttons: [{
                                    name: 'BtnAddCustomer',
                                    location: 'after',
                                    options: {
                                        text: '+ ',
                                        stylingMode: 'text',
                                        width: 32,
                                        elementAttr: {
                                            class: 'btn btn-primary btnAdd',
                                        },
                                        onClick(e) {
                                            var bol = '<%=this.GetContext().PageData.IsAdd%>';
                                            $("#acParentAccount").dxSelectBox('instance').option({ value: parseInt($("#COAVendors").val()) });
                                            //if (bol==true)
                                            showModal("#customerInfoModal");
                                        },
                                    },
                                }, 'clear'],--%>
                                onValueChanged: function (data) {
                                    $("#acAddress").dxSelectBox({
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "ID",
                                            loadUrl: "../../api/General/GetContactDetails",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': $("#acVendor").dxSelectBox('instance').option('value') + ',' + '<%=GetAcAddressContextKey()%>' },
                                            displayExpr: "Name",
                                            valueExpr: "ID"
                                        }),
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        searchEnabled: true,
                                        placeholder: '<%=Resources.Labels.Address%>',
                                        onValueChanged: function (data) {

                                        },
                                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                        }).dxSelectBox("instance");

                                    $("#acShipAddress").dxSelectBox({
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "ID",
                                            loadUrl: "../../api/General/GetContactDetails",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': $("#acVendor").dxSelectBox('instance').option('value') + ',' + '<%=GetAcAddressContextKey()%>' },
                                            displayExpr: "Name",
                                            valueExpr: "ID"
                                        }),
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        searchEnabled: true,
                                        placeholder: '<%=Resources.Labels.ShipAddress%>',
                                        onValueChanged: function (data) {

                                        },
                                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                        }).dxSelectBox("instance");

                                    $("#acPaymentAddress").dxSelectBox({
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "ID",
                                            loadUrl: "../../api/General/GetContactDetails",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': $("#acVendor").dxSelectBox('instance').option('value') + ',' + '<%=GetAcAddressContextKey()%>' },
                                            displayExpr: "Name",
                                            valueExpr: "ID"
                                        }),
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        searchEnabled: true,
                                        placeholder: '<%=Resources.Labels.PaymentAddress%>',
                                        onValueChanged: function (data) {

                                        },
                                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                        }).dxSelectBox("instance");

                                    $("#acTelephone").dxSelectBox({
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "ID",
                                            loadUrl: "../../api/General/GetContactDetails",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': $("#acVendor").dxSelectBox('instance').option('value') + ',' + '<%=GetAcAddressContextKey()%>' },
                                            displayExpr: "Name",
                                            valueExpr: "ID"
                                        }),
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        searchEnabled: true,
                                        placeholder: '<%=Resources.Labels.Telephone%>',
                                        onValueChanged: function (data) {

                                        },
                                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                        }).dxSelectBox("instance");
                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                                }).dxSelectBox("instance");
                        });
                        ///////////////////////////////////////////////
                        $.getJSON("../../api/General/GetChartOfAccounts", { 'contextKey': <%=this.GetCurrentCulture()%>+ "," + $("#cmbxBranch").dxSelectBox('instance').option('value') + "," + <%=ddlCurrency.SelectedValue%> + "," + <%=Convert.ToInt32(XPRESS.Common.COA.CashOnHand).ToString()%> + ",false,false" }, function (cashAccountResponse) {
                            var selectedCashAccountId = null;
                            if (<%=MyContext.SalesOptions.KeepCostCenter.ToString().ToLower()%>== true && cashAccountResponse.length > 0)
                                selectedCashAccountId = cashAccountResponse[0].ID;
                            $("#acCashAccount").dxSelectBox({
                                dataSource: cashAccountResponse,
                                displayExpr: "Name",
                                valueExpr: "ID",
                                value: selectedCashAccountId,
                                searchEnabled: true,

                                placeholder: '<%=Resources.Labels.CashAccount%>',
                                onValueChanged: function (data) {

                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                            }).dxSelectBox("instance");
                        });
                    },
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                }).dxSelectBox("instance");


                var ekd = ekdArray[<%=this.MyContext.PurchasesReturnOptions.EnterKeyEventOnTable%>];
                $("#purchaseReturnGridContainer").dxDataGrid({
                    columnFixing: {
                        enabled: true
                    },
                    columnAutoWidth: true,
                    showBorders: true,
                    showRowLines: true,
                    dataSource: {
                        store: DevExpress.data.AspNet.createStore({
                            key: "KeyCol",
                            loadUrl: "../../api/Item/LoadStoreTranList",
                            loadMethod: "get",
                        }),
                        paginate: false
                    },
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
                                    dataField: "KeyCol",
                                    caption: "KeyCol",
                                    allowEditing: false,
                                    dataType: "string",
                                    width: 150,
                                    visible: false
                                },
                                {
                                    dataField: "RowNoCol",
                                    caption: '<%=Resources.Labels.DgRowNoHeader%>',
                                    allowEditing: false,
                                    dataType: "number",
                                    alignment: "center",
                                    width: 70,
                                    visible: false
                                },
                                {
                                    dataField: "BarcodeCol",
                                    caption: '<%=Resources.Labels.DgBarcodeHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    /*validationRules: [
                                        {
                                            type: "required",
                                            ignoreEmptyValue: false,
                                        },
                                        {
                                            type: "async",
                                            message: "يجب ادخال الصنف",
                                            ignoreEmptyValue: false,
                                            validationCallback: function (params) {
                                                const d = $.Deferred();
                                                if (params.value == null)
                                                    d.reject("يجب ادخال الصنف");
                                                else {
                                                    $.getJSON("../../api/Item/FindByBarcode", { contextKey: ",,,true,1", Source: params.value },
                                                        function (response) {
                                                            if (response != null && response.length > 0) {
                                                                d.resolve(response);
                                                            }
                                                            else {
                                                                d.reject("يجب ادخال الصنف");
                                                            }
                                                        }
                                                    );
                                                }
                                                return d.promise();
                                            }
                                        }
                                    ],*/
                                    alignment: "center",
                                    visible: <%=this.MyContext.SalesReturnOptions.UseBarcode.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "ItemNameCol",
                                    caption: '<%=Resources.Labels.DgItemNameHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    /*validationRules: [
                                        {
                                            type: "required",
                                            ignoreEmptyValue: false,
                                        },
                                        {
                                            type: "async",
                                            message: "يجب ادخال الصنف",
                                            ignoreEmptyValue: false,
                                            validationCallback: function (params) {
                                                const d = $.Deferred();
                                                if (params.value == null)
                                                    d.reject("يجب ادخال الصنف");
                                                else {
                                                    $.getJSON("../../api/Item/FindByName", { contextKey: ",,,true,1", Source: params.value },
                                                        function (response) {
                                                            if (response != null && response.length > 0) {
                                                                d.resolve(response);
                                                            }
                                                            else {
                                                                d.reject("يجب ادخال الصنف");
                                                            }
                                                        }
                                                    );
                                                }
                                                return d.promise();
                                            }
                                        }
                                    ],*/
                                    lookup: {
                                        /*dataSource: {
                                            store: {
                                                type: 'array',
                                                data: allItems,
                                                key: "ID"
                                            },
                                            paginate: true,
                                            pageSize: 100
                                        },*/
                                        dataSource: function getItems() {
                                            var cachedItems = localStorage.getItem("items");
                                            allMyItems = JSON.parse(cachedItems);
                                            return {
                                                store: {
                                                    type: "array",
                                                    key: "ID",
                                                    data: allMyItems,
                                                },
                                                paginate: true,
                                                pageSize: 100
                                            };
                                        },
                                        allowClearing: true,
                                        valueExpr: "Name",
                                        displayExpr: "Name"
                                    },
                                    allowEditing: false,
                                    alignment: "center",
                                    width: 420,
                                    calculateDisplayValue: function (rowData) {
                                        var cachedItems = localStorage.getItem("items");
                                        var ds = JSON.parse(cachedItems);
                                        var idx = ds.findIndex(i => i.ID === rowData.ItemIdCol);

                                        if (idx === -1) {
                                            return rowData.ItemNameCol;
                                        } else {
                                            return ds[idx].Name;
                                        }
                                    }
                                },
                                {
                                    dataField: "UnitNameCol",
                                    caption: '<%=Resources.Labels.DgUnitNameHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    lookup: {
                                        dataSource: function getUnits(options) {
                                            var rows = $("#purchaseReturnGridContainer").dxDataGrid("instance").getVisibleRows();
                                            if (rows != null && rows.length > 0 && options.rowIndex > -1 && rows[options.rowIndex].data["ItemIdCol"] != null) {
                                                var UnitId = null;
                                                if (rows[options.rowIndex].data["UnitIdCol"] != null)
                                                    UnitId = rows[options.rowIndex].data["UnitIdCol"];
                                                var ItemId = rows[options.rowIndex].data["ItemIdCol"];
                                                //options.value = UnitId;
                                                return {
                                                    store: DevExpress.data.AspNet.createStore({
                                                        key: "UOMName",
                                                        loadUrl: "../../api/ItemUnit/GetItemUnits",
                                                        loadParams: { 'contextKey': ItemId },
                                                        loadMethod: "get",
                                                        valueExpr: "UOMName",
                                                        displayExpr: "UOMName"
                                                    })
                                                };
                                            }
                                            else {
                                                var cachedUnits = localStorage.getItem("units");
                                                if (cachedUnits) {
                                                    let allUnits = [];
                                                    allUnits = JSON.parse(cachedUnits);
                                                    return {
                                                        store: {
                                                            type: "array",
                                                            key: "UOMName",
                                                            data: allUnits,
                                                            valueExpr: "UOMName",
                                                            displayExpr: "UOMName"
                                                        }
                                                    };
                                                }
                                                else {
                                                    return {
                                                        store: DevExpress.data.AspNet.createStore({
                                                            key: "UOMName",
                                                            loadUrl: "../../api/ItemUnit/GetItemUnits",
                                                            loadParams: { 'contextKey': null },
                                                            loadMethod: "get",
                                                            valueExpr: "UOMName",
                                                            displayExpr: "UOMName"
                                                        })
                                                    };
                                                }
                                            }
                                        },
                                        allowClearing: true,
                                        valueExpr: "UOMName",
                                        displayExpr: "UOMName"
                                    },
                                    //allowEditing: false,
                                    alignment: "center",
                                    width: 100
                                },
                                {
                                    dataField: "AmountCol",
                                    caption: '<%=Resources.Labels.DgQuantityHeader%>',
                                    allowEditing: false,
                                    dataType: "number",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 80
                                },
                                {
                                    dataField: "BounusCol",
                                    caption: '<%=Resources.Labels.DgBounusHeader%>',
                                    allowEditing: false,
                                    dataType: "number",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 80,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseBounus.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "UnitCostEvaluateCol",
                                    caption: '<%=Resources.Labels.DgUnitCostEvaluateHeader%>',
                                    allowEditing: false,
                                    width: 120,
                                    dataType: "number",
                                    alignment: "center"
                                },
                                {
                                    dataField: "StoreIdCol",
                                    caption: '<%=Resources.Labels.DgStoreHeader%>',
                                    allowEditing: false,
                                    dataType: "number",
                                    lookup: {
                                        dataSource: function getStores(options) {
                                            var branchId = $("#cmbxBranch").dxSelectBox('instance').option('value');
                                            return {
                                                store: DevExpress.data.AspNet.createStore({
                                                    key: "ID",
                                                    loadUrl: "../../api/Structure/GetStorPermissionsGroup",
                                                    loadMethod: "get",
                                                    loadParams: { 'contextKey': branchId + "," + '<%=this.MyContext.UserProfile.UserId%>' },
                                                        displayExpr: "Name",
                                                        valueExpr: "ID"
                                                    })
                                            };
                                        },
                                        allowClearing: true,
                                        valueExpr: "ID",
                                        displayExpr: "Name"
                                    },
                                    //allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseStoreByRecord.ToString().ToLower()%>,
                                },

                                {
                                    dataField: "TotalBeforTaxCol",
                                    caption: '<%=Resources.Labels.DgRowTotalBeforTaxHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "number",
                                    alignment: "center",
                                    /*visible: false,
                                    calculateCellValue: function (rowData) {
                                        return getRowTotalBeforTaxVal(rowData);
                                    }*/
                                },
                                {
                                    dataField: "SafeBeforTaxCol",
                                    caption: '<%=Resources.Labels.DgRowSafeBeforTaxHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "number",
                                    alignment: "center",
                                    /*visible: false,
                                    calculateCellValue: function (rowData) {
                                        return getRowSafeBeforTaxVal(rowData);
                                    }*/
                                },
                                {
                                    dataField: "TotalCostEvaluateCol",
                                    caption: '<%=Resources.Labels.DgRowTotalCostEvaluateHeader%>',
                                    allowEditing: false,
                                    width: 120,
                                    dataType: "number",
                                    alignment: "center",
                                    /*calculateCellValue: function (rowData) {
                                        return getRowTotal(rowData);
                                    }*/
                                },
                                {
                                    dataField: "RowTotalTaxCol",
                                    caption: '<%=Resources.Labels.DgRowTotalTaxHeader%>',
                                    allowEditing: false,
                                    width: 100,
                                    dataType: "number",
                                    alignment: "center",
                                    visible: true,
                                    <%--calculateCellValue: function (rowData) {
                                        var DiscountTotal = 0.00, Amount = 0.00;
                                        if (rowData.AmountCol != null && rowData.AmountCol != '' && rowData.AmountCol != 0.00)
                                            Amount = rowData.AmountCol;
                                        if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != '' && rowData.CashDiscountCol != 0.00)
                                            DiscountTotal = rowData.CashDiscountCol;
                                        DiscountTotal = DiscountTotal * Amount;

                                        var TotalBeforTaxCol = getRowTotalBeforTaxVal(rowData);
                                        if (TotalBeforTaxCol != null && TotalBeforTaxCol != null) {
                                            var result = TotalBeforTaxCol;
                                            result = result - DiscountTotal;
                                            result = result * rowData.TaxIncludeCol;
                                            result = Math.round((result + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                                            return result;
                                        }
                                        else return null;
                                    }--%>
                                },
                                {
                                    dataField: "PercentageDiscountCol",
                                    caption: '<%=Resources.Labels.DgPercentageDiscountHeader%>',
                                    allowEditing: false,
                                    width: 100,
                                    dataType: "number",
                                    alignment: "center",
                                    visible: true
                                },
                                {
                                    dataField: "CashDiscountCol",
                                    caption: '<%=Resources.Labels.DgCashDiscountHeader%>',
                                    allowEditing: false,
                                    width: 100,
                                    dataType: "number",
                                    alignment: "center"
                                },
                                {
                                    dataField: "DiscountTotalCol",
                                    caption: '<%=Resources.Labels.DgDiscountTotalHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "number",
                                    alignment: "center",
                                    visible: true,
                                    /*calculateCellValue: function (rowData) {
                                        var DiscountTotal = 0.00, Amount = 0.00;
                                        if (rowData.AmountCol != null && rowData.AmountCol != '' && rowData.AmountCol != 0.00)
                                            Amount = rowData.AmountCol;
                                        if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != '' && rowData.CashDiscountCol != 0.00)
                                            DiscountTotal = rowData.CashDiscountCol;
                                        return DiscountTotal * Amount;
                                    }*/
                                },
                                {
                                    dataField: "BatchIdCol",
                                    caption: '<%=Resources.Labels.DgBatchNoHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 80,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseBatchByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "TruckIdCol",
                                    caption: '<%=Resources.Labels.DgTruckNoHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 80,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseTruckByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "ShelfIdCol",
                                    caption: '<%=Resources.Labels.DgShelfNoHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 80,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseShelf.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "CostCenterIdCol",
                                    caption: '<%=Resources.Labels.CostCenter%>',
                                    allowEditing: false,
                                    dataType: "number",
                                    lookup: {
                                        dataSource: function getStores(options) {
                                            var branchId = $("#cmbxBranch").dxSelectBox('instance').option('value');
                                            return {
                                                store: DevExpress.data.AspNet.createStore({
                                                    key: "ID",
                                                    loadUrl: "../../api/CostCenter/GetCostCenters",
                                                    loadMethod: "get",
                                                    loadParams: { 'contextKey': '<%=this.GetCostCenterContextKey()%>' + $("#cmbxBranch").dxSelectBox('instance').option('value') },
                                                        displayExpr: "Name",
                                                        valueExpr: "ID"
                                                    })
                                            };
                                        },
                                        allowClearing: true,
                                        valueExpr: "ID",
                                        displayExpr: "Name"
                                    },
                                    //allowEditing: false,
                                    alignment: "center",
                                    width: 120,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseCostCenterByRecord.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "NotesCol",
                                    caption: '<%=Resources.Labels.DgNotesHeader%>',
                                    allowEditing: false,
                                    dataType: "string",
                                    format: "decimal",
                                    alignment: "center",
                                    width: 250,
                                    visible: <%=this.MyContext.SalesReturnOptions.UseNote.ToString().ToLower()%>,
                                },
                                {
                                    dataField: "ItemIdCol",
                                    caption: '<%=Resources.Labels.DgItemIdHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "UnitIdCol",
                                    allowEditing: false,
                                    caption: '<%=Resources.Labels.DgUnitIdHeader%>',
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxPercentageValueCol",
                                    caption: '<%=Resources.Labels.DgTaxPercentageValueHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxIncludeCol",
                                    caption: '<%=Resources.Labels.DgTaxIncludeHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "number",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxIdCol",
                                    caption: '<%=Resources.Labels.DgTaxHeader%>',
                                    dataType: "string",
                                    lookup: {
                                        dataSource: DevExpress.data.AspNet.createStore({
                                            key: "ID",
                                            loadUrl: "../../api/Tax/GetTaxes",
                                            loadMethod: "get",
                                            loadParams: { 'contextKey': '' },
                                            displayExpr: "Name",
                                            valueExpr: "ID"
                                        }),
                                        allowClearing: true,
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
                                    width: 150,
                                },
                                {
                                    dataField: "TaxOnInvoiceTypeCol",
                                    caption: '<%=Resources.Labels.DgTaxOnInvoiceTypeHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxOnReceiptTypeCol",
                                    caption: '<%=Resources.Labels.DgTaxOnReceiptTypeHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxOnDocCreditTypeCol",
                                    caption: '<%=Resources.Labels.DgTaxOnDocCreditTypeHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxSalesAccountIDCol",
                                    caption: '<%=Resources.Labels.DgTaxSalesAccountIDHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "TaxPurchaseAccountIDCol",
                                    caption: '<%=Resources.Labels.DgTaxPurchaseAccountIDHeader%>',
                                    allowEditing: false,
                                    width: 150,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "IsUseTaxCol",
                                    caption: '<%=Resources.Labels.DgIsUseTaxHeader%>',
                                    allowEditing: true,
                                    dataType: "boolean",
                                    alignment: "center",
                                    width: 100,
                                    visible: false
                                },
                                {
                                    dataField: "TaxPercentageCol",
                                    caption: '<%=Resources.Labels.DgTaxPercentageHeader%>',
                                    allowEditing: true,
                                    dataType: "string",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "IsTaxIncludedInPurchaseCol",
                                    caption: '<%=Resources.Labels.DgIsTaxIncludedInPurchaseHeader%>',
                                    allowEditing: true,
                                    dataType: "boolean",
                                    alignment: "center",
                                    visible: false
                                },
                                {
                                    dataField: "IsTaxIncludedInSaleCol",
                                    caption: '<%=Resources.Labels.DgIsTaxIncludedInSaleHeader%>',
                                    allowEditing: true,
                                    dataType: "boolean",
                                    alignment: "center",
                                    visible: true
                                },
                                {
                                    dataField: "InvoiceDetailIdCol",
                                    caption: "<%=Resources.Labels.DgInvoiceDetailId%>",
                                    allowEditing: false,
                                    visible: false
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
                                            $('#purchaseReturnGridContainer').dxDataGrid('instance').deleteRow(e.row.rowIndex);
                                            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                            grid.saveEditData();
                                        }
                                    }]
                                }
                            ],
                        keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: ekd /* "row"*/, editOnKeyPress: true },
                        onEditorPreparing: function OnEditorPreparing(e) {
                            var component = e.component,
                                rowIndex = e.row && e.row.rowIndex;

                            if (e.dataField == "BarcodeCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                        $.getJSON("../../api/Item/FindByBarcode", { contextKey: ",,,true,1", Source: e.value },
                                            function (response) {
                                                if (response != null) {
                                                    component.cellValue(rowIndex, "BarcodeCol", response[0].Barcode);
                                                    component.cellValue(rowIndex, "ItemIdCol", response[0].ID);
                                                    component.cellValue(rowIndex, "ItemNameCol", response[0].Name);
                                                    component.cellValue(rowIndex, "UnitIdCol", response[0].UOM_ID);
                                                    component.cellValue(rowIndex, "UnitNameCol", response[0].UOMName);
                                                    if (<%=MyContext.SalesReturnOptions.SetAmountToOne.ToString().ToLower()%>== true)
                                                        component.cellValue(rowIndex, "AmountCol", 1);
                                                    component.cellValue(rowIndex, "UnitCostEvaluateCol", response[0].DefaultPrice);
                                                    component.cellValue(rowIndex, "PercentageDiscountCol", response[0].PercentageDiscount);
                                                    component.cellValue(rowIndex, "TaxPercentageValueCol", response[0].PercentageValue);
                                                    component.cellValue(rowIndex, "TaxIdCol", response[0].Tax_ID);
                                                    component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", response[0].OnInvoiceType);
                                                    component.cellValue(rowIndex, "TaxOnReceiptTypeCol", response[0].TaxOnReceiptType);
                                                    component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", response[0].OnDocCreditType);
                                                    component.cellValue(rowIndex, "TaxSalesAccountIDCol", response[0].SalesAccountID);
                                                    component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", response[0].PurchaseAccountID);

                                                    if (response[0].IsUseTax != null && response[0].IsUseTax == true) {
                                                        component.cellValue(rowIndex, "IsUseTaxCol", response[0].IsUseTax);
                                                        component.cellValue(rowIndex, "TaxPercentageCol", response[0].TaxPercentage);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", response[0].IsTaxIncludedInPurchase);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", response[0].IsTaxIncludedInSale);

                                                        if (response[0].IsTaxIncludedInSale != null && response[0].IsTaxIncludedInSale == true) {
                                                            if (response[0].TaxPercentage) {
                                                                var result = (response[0].TaxPercentage / (100 + response[0].TaxPercentage));
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                        }
                                                        else {
                                                            if (response[0].TaxPercentage != null) {
                                                                var result = (response[0].TaxPercentage / 100);
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                        }
                                                    }
                                                    else {
                                                        component.cellValue(rowIndex, "IsUseTaxCol", false);
                                                        component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                                    }

                                                    calcDocTotals();
                                                    //grid.focus(grid.getCellElement(rowIndex, "AmountCol"));
                                                }
                                                else {
                                                    component.cellValue(rowIndex, "ItemIdCol", null);
                                                    component.cellValue(rowIndex, "ItemNameCol", null);
                                                    component.cellValue(rowIndex, "UnitIdCol", null);
                                                    component.cellValue(rowIndex, "UnitNameCol", null);
                                                    if (<%=MyContext.SalesReturnOptions.SetAmountToOne.ToString().ToLower()%>== true)
                                                        component.cellValue(rowIndex, "AmountCol", null);
                                                    component.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                                                    component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                                    component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                                    component.cellValue(rowIndex, "TaxIdCol", null);
                                                    component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxOnReceiptTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxSalesAccountIDCol", null);
                                                    component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", null);
                                                    component.cellValue(rowIndex, "IsUseTaxCol", false);
                                                    component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                                    component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                                    component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                                    calcDocTotals();
                                                    //grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                                                }
                                            });
                                    }
                                    else {
                                        component.cellValue(rowIndex, "ItemIdCol", null);
                                        component.cellValue(rowIndex, "ItemNameCol", null);
                                        component.cellValue(rowIndex, "UnitIdCol", null);
                                        component.cellValue(rowIndex, "UnitNameCol", null);
                                        component.cellValue(rowIndex, "AmountCol", null);
                                        component.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                                        component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                        component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                        component.cellValue(rowIndex, "TaxIdCol", null);
                                        component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", null);
                                        component.cellValue(rowIndex, "TaxOnReceiptTypeCol", null);
                                        component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", null);
                                        component.cellValue(rowIndex, "TaxSalesAccountIDCol", null);
                                        component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", null);
                                        component.cellValue(rowIndex, "IsUseTaxCol", false);
                                        component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                        calcDocTotals();
                                    }
                                }
                            }
                            else if (e.dataField == "ItemNameCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = async function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                        $.getJSON("../../api/Item/FindByName", { contextKey: ",,,true,1", Source: e.value },
                                            async function (response) {
                                                if (response != null) {
                                                    component.cellValue(rowIndex, "BarcodeCol", response[0].Barcode);
                                                    component.cellValue(rowIndex, "ItemIdCol", response[0].ID);
                                                    component.cellValue(rowIndex, "ItemNameCol", response[0].Name);
                                                    component.cellValue(rowIndex, "UnitIdCol", response[0].UOM_ID);
                                                    component.cellValue(rowIndex, "UnitNameCol", response[0].UOMName);
                                                    if (<%=MyContext.SalesReturnOptions.SetAmountToOne.ToString().ToLower()%>== true)
                                                        component.cellValue(rowIndex, "AmountCol", 1);
                                                    component.cellValue(rowIndex, "UnitCostEvaluateCol", response[0].DefaultPrice);
                                                    component.cellValue(rowIndex, "PercentageDiscountCol", response[0].PercentageDiscount);
                                                    component.cellValue(rowIndex, "TaxPercentageValueCol", response[0].PercentageValue);
                                                    component.cellValue(rowIndex, "TaxIdCol", response[0].Tax_ID);
                                                    component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", response[0].OnInvoiceType);
                                                    component.cellValue(rowIndex, "TaxOnReceiptTypeCol", response[0].TaxOnReceiptType);
                                                    component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", response[0].OnDocCreditType);
                                                    component.cellValue(rowIndex, "TaxSalesAccountIDCol", response[0].SalesAccountID);
                                                    component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", response[0].PurchaseAccountID);

                                                    if (response[0].IsUseTax != null && response[0].IsUseTax == true) {
                                                        component.cellValue(rowIndex, "IsUseTaxCol", response[0].IsUseTax);
                                                        component.cellValue(rowIndex, "TaxPercentageCol", response[0].TaxPercentage);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", response[0].IsTaxIncludedInPurchase);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", response[0].IsTaxIncludedInSale);

                                                        if (response[0].IsTaxIncludedInSale != null && response[0].IsTaxIncludedInSale == true) {
                                                            if (response[0].TaxPercentage) {
                                                                var result = (response[0].TaxPercentage / (100 + response[0].TaxPercentage));
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                        }
                                                        else {
                                                            if (response[0].TaxPercentage != null) {
                                                                var result = (response[0].TaxPercentage / 100);
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                        }
                                                    }
                                                    else {
                                                        component.cellValue(rowIndex, "IsUseTaxCol", false);
                                                        component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                                    }

                                                    calcDocTotals();
                                                    grid.focus(grid.getCellElement(rowIndex, "AmountCol"));
                                                    calcDocTotals();
                                                }
                                                else {
                                                    component.cellValue(rowIndex, "BarcodeCol", null);
                                                    component.cellValue(rowIndex, "ItemIdCol", null);
                                                    component.cellValue(rowIndex, "ItemNameCol", null);
                                                    component.cellValue(rowIndex, "UnitIdCol", null);
                                                    component.cellValue(rowIndex, "UnitNameCol", null);
                                                    component.cellValue(rowIndex, "AmountCol", null);
                                                    component.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                                                    component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                                    component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                                    component.cellValue(rowIndex, "TaxIdCol", null);
                                                    component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxOnReceiptTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", null);
                                                    component.cellValue(rowIndex, "TaxSalesAccountIDCol", null);
                                                    component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", null);
                                                    component.cellValue(rowIndex, "IsUseTaxCol", false);
                                                    component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                                    component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                                    component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                                    calcDocTotals();
                                                    grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                                                }
                                            });
                                    }
                                    else {
                                        component.cellValue(rowIndex, "BarcodeCol", null);
                                        component.cellValue(rowIndex, "ItemIdCol", null);
                                        component.cellValue(rowIndex, "ItemNameCol", null);
                                        component.cellValue(rowIndex, "UnitIdCol", null);
                                        component.cellValue(rowIndex, "UnitNameCol", null);
                                        component.cellValue(rowIndex, "AmountCol", null);
                                        component.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                                        component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                        component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                        component.cellValue(rowIndex, "TaxIdCol", null);
                                        component.cellValue(rowIndex, "TaxOnInvoiceTypeCol", null);
                                        component.cellValue(rowIndex, "TaxOnReceiptTypeCol", null);
                                        component.cellValue(rowIndex, "TaxOnDocCreditTypeCol", null);
                                        component.cellValue(rowIndex, "TaxSalesAccountIDCol", null);
                                        component.cellValue(rowIndex, "TaxPurchaseAccountIDCol", null);
                                        component.cellValue(rowIndex, "IsUseTaxCol", false);
                                        component.cellValue(rowIndex, "TaxPercentageCol", 0);
                                        component.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                                        component.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                                        calcDocTotals();
                                    }
                                }

                                /*if (e.lookup.items.length > 0 && e.lookup.items[0].template == null) {
                                    var advancedSearchItem = {
                                        template: function () {
                                            return $("<div class='btn btn-primary btnAdd'>").dxButton({
                                                text: "+ اضافة صنف",
                                                onClick: function (args) {
                                                    showModal("#itemInfoModal");
                                                }
                                            });
                                        }
                                    };
 
                                    var src = e.lookup.items;
                                    src.splice(0, 0, advancedSearchItem);
                                    e.editorOptions.dataSource = e.lookup.items;
                                    //e.editorOptions.dataSource.push(advancedSearchItem);
                                }
                                else {
                                    var src = e.lookup.items;
                                    //src.splice(0, 0, advancedSearchItem);
                                    e.editorOptions.dataSource = e.lookup.items;
                                }*/
                            }
                            else if (e.dataField == "TaxIdCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        if (component.cellValue(rowIndex, "IsUseTaxCol") == true) {
                                            $.getJSON("../../api/Tax/FindItem", { Id: e.value },
                                                function (response) {
                                                    if (response != null) {
                                                        if (component.cellValue(rowIndex, "IsTaxIncludedInSaleCol") == true) {
                                                            if (response[0].PercentageValue) {
                                                                var result = (response[0].PercentageValue / (100 + response[0].PercentageValue));
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else {
                                                                component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                            }
                                                        }
                                                        else {
                                                            if (response[0].PercentageValue != null) {
                                                                var result = (response[0].PercentageValue / 100);
                                                                component.cellValue(rowIndex, "TaxIncludeCol", result);
                                                            }
                                                            else {
                                                                component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                            }
                                                        }
                                                        component.cellValue(rowIndex, "TaxPercentageCol", response[0].PercentageValue);
                                                        component.cellValue(rowIndex, "TaxPercentageValueCol", response[0].PercentageValue);
                                                    }
                                                    else {
                                                        var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                                        component.cellValue(rowIndex, "TaxPercentageCol", null);
                                                        component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                                        component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                                        grid.focus(grid.getCellElement(rowIndex, "TaxPercentageCol"));
                                                    }

                                                    calcDocTotals();
                                                });
                                        }
                                        else {
                                            component.cellValue(rowIndex, "TaxPercentageCol", null);
                                            component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                            component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                            calcDocTotals();
                                        }
                                    }
                                    else {
                                        component.cellValue(rowIndex, "TaxPercentageCol", null);
                                        component.cellValue(rowIndex, "TaxPercentageValueCol", null);
                                        component.cellValue(rowIndex, "TaxIncludeCol", 0);
                                        calcDocTotals();
                                    }
                                }
                            }
                            else if (e.dataField == "PercentageDiscountCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var percent = e.value;
                                        var UnitCost = 0.00, Discount = 0.00;
                                        UnitCost = component.cellValue(rowIndex, "UnitCostEvaluateCol");
                                        Discount = UnitCost * (percent / 100);
                                        component.cellValue(rowIndex, "CashDiscountCol", Discount);
                                    }
                                    else {
                                        component.cellValue(rowIndex, "CashDiscountCol", null);
                                    }

                                    calcDocTotals();
                                }
                            }
                            else if (e.dataField == "CashDiscountCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var UnitCost = 0.00, Amount = 0.00, Discount = 0.00, PercentageDiscount = 0, DiscountTotal = 0.00;
                                        Amount = component.cellValue(rowIndex, "AmountCol");
                                        Discount = component.cellValue(rowIndex, "CashDiscountCol");
                                        UnitCost = component.cellValue(rowIndex, "UnitCostEvaluateCol");
                                        PercentageDiscount = (Discount / UnitCost) * 100;
                                        component.cellValue(rowIndex, "PercentageDiscountCol", PercentageDiscount);
                                        DiscountTotal = Amount * Discount;
                                        //component.cellValue(rowIndex, "DiscountTotalCol", DiscountTotal);                                        
                                    }
                                    else {
                                        component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                        component.cellValue(rowIndex, "DiscountTotalCol", null);
                                    }

                                    calcDocTotals();
                                }
                            }
                            else if (e.dataField == "DiscountTotalCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var UnitCost = 0.00, Amount = 0.00, Discount = 0.00, PercentageDiscount = 0, DiscountTotal = 0.00;
                                        Amount = component.cellValue(rowIndex, "AmountCol");
                                        DiscountTotal = component.cellValue(rowIndex, "DiscountTotalCol");
                                        if (Amount != null && Amount != '' && Amount > 0)
                                            Discount = DiscountTotal / Amount;
                                        UnitCost = component.cellValue(rowIndex, "UnitCostEvaluateCol");
                                        PercentageDiscount = (Discount / UnitCost) * 100;
                                        component.cellValue(rowIndex, "PercentageDiscountCol", PercentageDiscount);
                                        component.cellValue(rowIndex, "CashDiscountCol", Discount);
                                    }
                                    else {
                                        component.cellValue(rowIndex, "PercentageDiscountCol", null);
                                        component.cellValue(rowIndex, "CashDiscountCol", null);
                                    }

                                    calcDocTotals();
                                }
                            }
                            else if (e.dataField == "TotalBeforTaxCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    if (e.value != null && e.value != '') {
                                        var UnitCost = 0.00, Amount = 0.00, TotalBeforTax = 0.00;
                                        Amount = component.cellValue(rowIndex, "AmountCol");
                                        TotalBeforTax = component.cellValue(rowIndex, "TotalBeforTaxCol");
                                        if (Amount != null && Amount != '' && Amount > 0)
                                            UnitCost = TotalBeforTax / Amount;
                                        component.cellValue(rowIndex, "UnitCostEvaluateCol", UnitCost);
                                    }
                                    else {
                                        component.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                                    }

                                    calcDocTotals();
                                }
                            }
                            else if (e.dataField == "AmountCol" || e.dataField == "UnitCostEvaluateCol" || e.dataField == "TotalCostEvaluateCol" ||
                                e.dataField == "BounusCol" || e.dataField == "RowTotalTaxCol" || e.dataField == "SafeBeforTaxCol" || e.dataField == "UnitIdCol" ||
                                e.dataField == "UnitNameCol" || e.dataField == "TaxPercentageValueCol" || e.dataField == "IsUseTaxCol" ||
                                e.dataField == "TaxPercentageCol" || e.dataField == "IsTaxIncludedInSaleCol") {
                                var onValueChanged = e.editorOptions.onValueChanged;
                                e.editorOptions.onValueChanged = function (e) {
                                    onValueChanged.call(this, e);
                                    calcDocTotals();
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
                                var toolsPnl = $("#ToolsPnl").height();
                                var btnRow = $("#BtnsRow").height();
                                return window.innerHeight - ((toolsPnl + btnRow) - 30);
                            }
                        },
                        width: function () {
                            return window.innerWidth * 0.78;
                        },
                        sorting: { mode: "none" },
                        customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                        onKeyDown: function (e) {
                            var keyCode = e.event.which, key = e.event.key, code = e.event.code;
                            var component = e.component;
                            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                            const focusedCellPosition = getCurrentCell(grid);
                            var isEditing = grid.getVisibleRows()[focusedCellPosition.rowIndex].isEditing;
                            if (key == "Enter" || keyCode == 13) {// Enter
                                //grid.saveEditData();
                                var items = grid.getDataSource().store()._array;
                                if (currentRowIndex == items.length - 1 && ValidateRows(false, false)) {
                                    AddRowBottom(true);
                                    grid.focus(grid.getCellElement(items.length, "ItemNameCol"));
                                }
                                /*else {                                    
                                    grid.focus(grid.getCellElement(focusedCellPosition.rowIndex+1, "ItemNameCol"));
                                }*/
                            }
                            else if ((key == "Delete" || keyCode == 46) && currentRowIndex >= 0) {// Delete
                                grid.deleteRow(focusedCellPosition.rowIndex);
                            }
                            else if ((key == "End" || keyCode == 35) /*&& isEditing==false*/) { // End
                                var columns = grid.getVisibleColumns();
                                grid.focus(grid.getCellElement(focusedCellPosition.rowIndex, columns[columns.length - 2].dataField));
                            }
                            else if ((key == "Home" || keyCode == 36) /*&& isEditing == false*/) { // Home
                                var columns = grid.getVisibleColumns();
                                grid.focus(grid.getCellElement(focusedCellPosition.rowIndex, columns[1].dataField));
                            }
                            else if (key == "F8" || keyCode == 119) { // F8
                                var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                grid.saveEditData();
                                var items = grid.getDataSource().store()._array;
                                var rowIndex = focusedCellPosition.rowIndex;
                                var columnIndex = focusedCellPosition.columnIndex;
                                var columns = $("#purchaseReturnGridContainer").dxDataGrid("instance").getVisibleColumns();
                                var column = columns[columnIndex];
                                var row = items[rowIndex];
                                if (column.dataField == "BarcodeCol" || column.dataField == "ItemNameCol") {
                                    $("#itemInfoDiv").load("../../Inv/ItemUI/FrmItemCachedSelect.aspx?rowIndex=" + rowIndex);
                                    showModal('#itemInfoModal');
                                }
                            }
                            else if (key == "F9" || keyCode == 120) { // F9
                                var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                grid.saveEditData();
                                var items = grid.getDataSource().store()._array;
                                var rowIndex = focusedCellPosition.rowIndex;
                                var columnIndex = focusedCellPosition.columnIndex;
                                var columns = $("#purchaseReturnGridContainer").dxDataGrid("instance").getVisibleColumns();
                                var column = columns[columnIndex];
                                var row = items[rowIndex];
                                if (column.dataField == "AvailabelQuantityCol" || column.dataField == "AmountCol") {
                                    showAvailableQuantity(row.ItemIdCol, row.ItemNameCol, rowIndex);
                                }
                                else if (column.dataField == "BarcodeCol" || column.dataField == "ItemNameCol") {
                                    $("#itemInfoDiv").load("../../Inv/ItemUI/FrmItemDbSelect.aspx?rowIndex=" + rowIndex);
                                    showModal('#itemInfoModal');
                                }
                            }
                        },

                        summary: {
                            totalItems: [
                                /////////////////////////////
                                {
                                    column: "UnitCostEvaluateCol",
                                    displayFormat: "<%=Resources.Labels.DgFooterTotalVATExcluded%>",
                                    showInColumn: 'ItemNameCol'
                                },
                                {
                                    column: "TotalBeforTaxCol",
                                    summaryType: "sum",
                                    //displayFormat: "ج قبل الضريبة {0}",
                                    displayFormat: "{0}",
                                    valueFormat: "#,##0.##",
                                    name: "TotalBeforTaxSum",
                                    showInColumn: 'UnitNameCol',
                                },
                                /////////////////////////////
                                {
                                    column: "UnitCostEvaluateCol",
                                    displayFormat: "<%=Resources.Labels.DgFooterTotalDiscountsSummation%>",
                                    showInColumn: 'ItemNameCol'
                                },
                                {
                                    column: "DiscountTotalCol",
                                    summaryType: "sum",
                                    //displayFormat: "اجمالي الخصم {0}",
                                    displayFormat: "{0}",
                                    valueFormat: "#,##0.##",
                                    name: "TotalCashDiscountSum",
                                    showInColumn: 'UnitNameCol',
                                },
                                /////////////////////////////
                                {
                                    column: "UnitCostEvaluateCol",
                                    displayFormat: "<%=Resources.Labels.DgFooterTotalAfterDiscountVATExcluded%>",
                                    showInColumn: 'ItemNameCol'
                                },
                                {
                                    column: "SafeBeforTaxCol",
                                    summaryType: "sum",
                                    //displayFormat: "ج قبل الضريبة {0}",
                                    displayFormat: "{0}",
                                    valueFormat: "#,##0.##",
                                    name: "SafeBeforTaxSum",
                                    showInColumn: 'UnitNameCol',
                                },
                                /////////////////////////////
                                {
                                    column: "UnitCostEvaluateCol",
                                    displayFormat: "<%=Resources.Labels.DgFooterTotalVatTax%>",
                                    showInColumn: 'ItemNameCol'
                                },
                                {
                                    column: "RowTotalTaxCol",
                                    summaryType: "sum",
                                    //displayFormat: "اجمالي الضريبة {0}",
                                    displayFormat: "{0}",
                                    valueFormat: "#,##0.##",
                                    name: "TotalTaxSum",
                                    showInColumn: 'UnitNameCol',
                                },
                                /////////////////////////////
                                {
                                    column: "UnitCostEvaluateCol",
                                    displayFormat: "<%=Resources.Labels.DgFooterTotalFinal%>",
                                    showInColumn: 'ItemNameCol'
                                },
                                {
                                    column: "TotalCostEvaluateCol",
                                    summaryType: "sum",
                                    //displayFormat: "الصافي {0}",
                                    displayFormat: "{0}",
                                    valueFormat: "#,##0.##",
                                    name: "TotalCostEvaluateSum",
                                    showInColumn: 'UnitNameCol',
                                }
                                /////////////////////////////                                                                                                                                               
                            ],
                            recalculateWhileEditing: true,
                        },
                        onCellPrepared: function (e) {
                            if (e.rowType == "totalFooter") {
                                let si = e.summaryItems.find(i => i.column == "TotalCostEvaluateCol");
                                if (si) {
                                    e.cellElement.find(".dx-datagrid-summary-item")
                                        .css({
                                            //"color": si.value > 10 ? "red" : "green",
                                            "color": "black",
                                            /*"border-bottom": "1px solid blue",
                                            "border-top": "1px solid blue",*/
                                            "border-bottom": "1px solid black",
                                            "border-top": "1px solid black",
                                            /*"font-size": "14px"*/
                                        });
                                }

                                si = e.summaryItems.find(i => i.showInColumn == "UnitNameCol");
                                if (si) {
                                    e.cellElement.find(".dx-datagrid-summary-item")
                                        .css({
                                            //"color": "blue",
                                            "color": "#394659",
                                            /*"border-bottom": "1px solid blue",
                                            "border-top": "1px solid blue",*/
                                            "border-bottom": "1px solid black",
                                            "border-top": "1px solid black",
                                            /*"font-size": "14px"*/
                                        });
                                }

                                si = e.summaryItems.find(i => i.column == "UnitCostEvaluateCol");
                                if (si) {
                                    e.cellElement.find(".dx-datagrid-summary-item")
                                        .css({
                                            //"color": "blue",
                                            "color": "#1D97E0",
                                            "border-bottom": "1px solid transparent",
                                            "border-top": "1px solid transparent",
                                            /*"font-size": "14px"*/
                                        });
                                }
                            }
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                        focusedRowEnabled: true,
                        onFocusedRowChanging: function OnFocusedRowChanging(e) {
                            currentRowIndex = e.newRowIndex;
                        },
                        remoteOperations: false,
                        paging: {
                            enabled: false,
                        },
                        selection: {
                            mode: "multiple",
                            showCheckBoxesMode: 'none',
                            selectAllMode: "allPages"//"page" // or "allPages
                        },
                        scrolling: {
                            mode: 'virtual',
                        },
                        onContentReady(e) {
                            
                                let scroll = e.component.getScrollable();
                                scroll.on("scroll", function () {
                                    var operation = $("#Operation").val();
                                    if ((operation == "Add" || operation == "Edit")) {
                                       
                                            SetDataGridEditable(true);
                                       
                                    }
                                });
                                scroll.on("updated", function () {

                                });
                          
                        },
                        onFocusedCellChanging: function OnFocusedCellChanging(e) {
                            var fieldName = e.columns[e.newColumnIndex].dataField
                            if (fieldName === "RowTotalTaxCol" || fieldName === "SafeBeforTaxCol") {
                                if (e.prevColumnIndex < e.newColumnIndex && e.newColumnIndex < e.columns.length - 1)
                                    e.newColumnIndex += 1;
                                else if (e.prevColumnIndex > e.newColumnIndex && e.newColumnIndex > 0)
                                    e.newColumnIndex -= 1;
                            }
                        },
                    });

                    clearDgv();
              
            }).done(function () {
                $("#itemInfoDiv").load("../../Inv/ItemUI/FrmItemDbSelect.aspx?rowIndex=-1");
               // $("#customerInfoDiv").load("../../Accounting/ChartOfAccountsUI/FrmChartOfAccountDbSelect.aspx?requestCode=1");

                $("#customerInfoDiv").load("../../Accounting/ChartOfAccountsUI/FrmAddVendor.aspx");
                $('#saveNewCustomer').click(function () {
                    saveCustomerInfo();
                });



                $("#acStore").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.Store%>',
                onValueChanged: function (data) {
                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

                $("#cmbxCostCenter").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.CostCenter%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");
                /********************************************/
                $("#acVendor").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.Vendor%>',
                <%--buttons: [{
                    name: 'BtnAddCustomer',
                    location: 'after',
                    options: {
                        text: '+ ',
                        stylingMode: 'text',
                        width: 32,
                        elementAttr: {
                            class: 'btn btn-primary btnAdd',
                        },
                        onClick(e) {
                            var bol = '<%=this.GetContext().PageData.IsAdd%>';
                            $("#acParentAccount").dxSelectBox('instance').option({ value: parseInt($("#COAVendors").val()) });
                            //if (bol==true)
                            showModal("#customerInfoModal");
                        },
                    },
                }, 'clear'],--%>
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

                $("#acAddress").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.Address%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

                $("#acShipAddress").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.ShipAddress%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

                $("#acPaymentAddress").dxSelectBox({
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.PaymentAddress%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

            $("#acTelephone").dxSelectBox({
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Telephone%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

            $("#acCashAccount").dxSelectBox({
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.CashAccount%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");
            /*******************************************/
            $("#txtOperationDate").dxDateBox({
                displayFormat: "dd/MM/yyyy",
                value: new Date()
            });

            /*******************************************/
            $.getJSON("../../api/PrintTemplate/GetDefaultTemplateByKindId", { KindId: '<%=DocumentKindClass.PurchaseReturnInvoice%>', 'EntryType': null },
                function (response) {
                    if (response.length > 0) {
                        $("#cmbxTemplate").dxSelectBox({
                            dataSource: DevExpress.data.AspNet.createStore({
                                key: "Id",
                                loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                loadMethod: "get",
                                loadParams: { 'KindId': '<%=DocumentKindClass.PurchaseReturnInvoice%>', 'EntryType': null },
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
                            rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                            }).dxSelectBox("instance");
                    }
                    else {
                        $("#cmbxTemplate").dxSelectBox({
                            dataSource: DevExpress.data.AspNet.createStore({
                                key: "Id",
                                loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                loadMethod: "get",
                                loadParams: { 'KindId': '<%=DocumentKindClass.PurchaseReturnInvoice%>', 'EntryType': null },
                                displayExpr: "Name",
                                valueExpr: "Id"
                            }),
                            displayExpr: "Name",
                            valueExpr: "Id",
                            searchEnabled: true,
                            placeholder: '<%=Resources.Labels.PrintTemplate%>',
                            onValueChanged: function (data) {
                            },
                            rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                            }).dxSelectBox("instance");
                    }
                });
            /*******************************************/
                continueDone();
            });
        }

        function continueDone() {
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnAdd", true);
            setEnabled("#btnSearch", true);
            setEnabled("#btnPrint", true);
            setEnabled("#txtOperationDate", true);
         
                setReadOnly("#ToolsPnl", true);
                $.getJSON('../../api/GridOrdering/FindGridOrdering', { 'GridId': '<%=DocumentKindClass.PurchaseReturnInvoice%>', 'UserId': '<%=MyContext.UserProfile.Contact_ID%>' }, function (response) {
                    if (response.length == 1) {
                        var array = JSON.parse(response[0].JsonVal);
                        var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
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
            
        }

        function clearDgv() {
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            var rows = $("#purchaseReturnGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }
            var dataSource = new DevExpress.data.DataSource({
                store: {
                    type: "array",
                    key: "KeyCol",
                    data: [],
                },
            });

            grid.resetOption('dataSource');
            rowIdentifier = 0;
            for (var i = 0; i < 10; i++) {
                rowIdentifier++;
                dataSource.store().push([
                    { type: "insert", key: rowIdentifier, data: { 'RowNoCol': rowIdentifier, 'KeyCol': rowIdentifier.toString() } }
                ]);
            }

            grid.option("dataSource", dataSource);
            try {
                grid.saveEditData();
            }
            catch {

            }
        }

        getCurrentCell = function (dataGrid) {
            return (dataGrid)._controllers.keyboardNavigation._focusedCellPosition;
        }

        /**************************** استرجاع الصافي قبل الضريبة = ((السعر-الضريبة)*العدد) ********************/
        function getRowTotalBeforTaxVal(rowData) {
            if (rowData.IsUseTaxCol != null && rowData.IsUseTaxCol == true) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    if (rowData.TaxPercentageCol) {
                        var result = (rowData.TaxPercentageCol / (100 + Number(rowData.TaxPercentageCol)));
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
                else {
                    if (rowData.TaxPercentageCol != null) {
                        var result = (rowData.TaxPercentageCol / 100);
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
            }
            else rowData.TaxIncludeCol = 0;
            ////////////////////////////////////////////////
            if (rowData.UnitCostEvaluateCol != null && rowData.AmountCol != null) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    var ItemTotalCostEvaluate = 0.00, unitCost = 0.00;
                    if (rowData.TaxIncludeCol != null) {
                        unitCost = rowData.UnitCostEvaluateCol;
                        unitCost = unitCost - (unitCost * rowData.TaxIncludeCol);
                        unitCost = rowData.AmountCol * unitCost;
                        ItemTotalCostEvaluate = unitCost;
                        ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                    }

                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
                else {
                    var ItemTotalCostEvaluate = 0.00;
                    ItemTotalCostEvaluate = rowData.UnitCostEvaluateCol * rowData.AmountCol;
                    ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;

                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
            }
            else return null;
        }
        /**************************** استرجاع الصافي قبل الضريبة = ((السعر-الضريبة)*العدد-الخصم ********************/
        function getRowSafeBeforTaxVal(rowData) {
            if (rowData.IsUseTaxCol != null && rowData.IsUseTaxCol == true) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    if (rowData.TaxPercentageCol) {
                        var result = (rowData.TaxPercentageCol / (100 + Number(rowData.TaxPercentageCol)));
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
                else {
                    if (rowData.TaxPercentageCol != null) {
                        var result = (rowData.TaxPercentageCol / 100);
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
            }
            else rowData.TaxIncludeCol = 0;
            ////////////////////////////////////////////////
            if (rowData.UnitCostEvaluateCol != null && rowData.AmountCol != null) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    var ItemTotalCostEvaluate = 0.00, unitCost = 0.00;
                    if (rowData.TaxIncludeCol != null) {
                        unitCost = rowData.UnitCostEvaluateCol;
                        if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != 0.00) {
                            unitCost = unitCost - rowData.CashDiscountCol;
                        }
                        unitCost = unitCost - (unitCost * rowData.TaxIncludeCol);
                        unitCost = rowData.AmountCol * unitCost;
                        ItemTotalCostEvaluate = unitCost;
                        ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                    }

                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
                else {
                    var ItemTotalCostEvaluate = 0.00, Amount = 0.00;
                    if (rowData.UnitCostEvaluateCol != null && rowData.UnitCostEvaluateCol != '')
                        ItemTotalCostEvaluate = rowData.UnitCostEvaluateCol;
                    if (rowData.AmountCol != null && rowData.AmountCol != '')
                        Amount = rowData.AmountCol;

                    ItemTotalCostEvaluate = rowData.UnitCostEvaluateCol;
                    if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != 0.00) {
                        ItemTotalCostEvaluate = ItemTotalCostEvaluate - rowData.CashDiscountCol;
                    }

                    ItemTotalCostEvaluate = ItemTotalCostEvaluate * Amount;
                    ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;

                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
            }
            else return null;
        }
        /**************************** استرجاع الصافي بعد الضريبة = (العدد*السعر)-الخصم ********************/
        function getRowTotal(rowData) {
            if (rowData.IsUseTaxCol != null && rowData.IsUseTaxCol == true) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    if (rowData.TaxPercentageCol) {
                        var result = (rowData.TaxPercentageCol / (100 + Number(rowData.TaxPercentageCol)));
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
                else {
                    if (rowData.TaxPercentageCol != null) {
                        var result = (rowData.TaxPercentageCol / 100);
                        rowData.TaxIncludeCol = result;
                    }
                    else rowData.TaxIncludeCol = 0;
                }
            }
            else rowData.TaxIncludeCol = 0;
            ////////////////////////////////////////////////
            if (rowData.UnitCostEvaluateCol != null && rowData.AmountCol != null) {
                if (rowData.IsTaxIncludedInPurchaseCol != null && rowData.IsTaxIncludedInPurchaseCol == true) {
                    var ItemTotalCostEvaluate = 0.00, unitCost = 0.00, totalDicount = 0.00;
                    if (rowData.TaxIncludeCol != null) {
                        unitCost = rowData.UnitCostEvaluateCol;
                        if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != 0.00)
                            totalDicount = totalDicount + rowData.CashDiscountCol;
                        unitCost = unitCost - totalDicount;
                        unitCost = rowData.AmountCol * unitCost;
                        ItemTotalCostEvaluate = unitCost;
                        ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                    }

                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
                else {
                    var ItemTotalCostEvaluate = 0.00, Amount = 0.00;
                    if (rowData.UnitCostEvaluateCol != null && rowData.UnitCostEvaluateCol != '')
                        ItemTotalCostEvaluate = rowData.UnitCostEvaluateCol;
                    if (rowData.AmountCol != null && rowData.AmountCol != '')
                        Amount = rowData.AmountCol;

                    ItemTotalCostEvaluate = rowData.UnitCostEvaluateCol;
                    if (rowData.CashDiscountCol != null && rowData.CashDiscountCol != 0.00) {
                        ItemTotalCostEvaluate = ItemTotalCostEvaluate - rowData.CashDiscountCol;
                    }

                    ItemTotalCostEvaluate = ItemTotalCostEvaluate * Amount;
                    ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
                    ItemTotalCostEvaluate = ItemTotalCostEvaluate + (ItemTotalCostEvaluate * rowData.TaxIncludeCol);
                    if (ItemTotalCostEvaluate != 0.00)
                        return ItemTotalCostEvaluate;
                    else return null;
                }
            }
            else return null;
        }

        function ValidateRows(skipEmptyRows, toSave) {
            var UseStoreByRecord = <%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>;
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            var rows = grid.getDataSource().store()._array;
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1,
                operation = $("#Operation").val(), documentId = $("#Id").val(),
                docDate = $("#txtOperationDate").dxDateBox("instance").option('text').trim();
            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].AmountCol == null || rows[i].AmountCol == '') &&
                        //(rows[i].BounusCol == null || rows[i].BounusCol == '') &&                                
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') &&
                        (rows[i].PercentageDiscountCol == null || rows[i].PercentageDiscountCol == '') &&
                        (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '') &&
                        (rows[i].CashDiscountCol == null || rows[i].CashDiscountCol == '') &&
                        (grid.cellValue(i, "RowTotalTaxCol") == null || grid.cellValue(i, "RowTotalTaxCol") == '') &&
                        (grid.cellValue(i, "TotalBeforTaxCol") == null || grid.cellValue(i, "TotalBeforTaxCol") == '') &&
                        (grid.cellValue(i, "TotalCostEvaluateCol") == null || grid.cellValue(i, "TotalCostEvaluateCol") == '') &&
                        (rows[i].BatchIdCol == null || rows[i].BatchIdCol == '') &&
                        (rows[i].TruckIdCol == null || rows[i].TruckIdCol == '') &&
                        (rows[i].ShelfIdCol == null || rows[i].ShelfIdCol == '') &&
                        (rows[i].CostCenterIdCol == null || rows[i].CostCenterIdCol == '') &&
                        (rows[i].NotesCol == null || rows[i].NotesCol == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') &&
                        (rows[i].TaxPercentageValueCol == null || rows[i].TaxPercentageValueCol == '') &&
                        (rows[i].TaxIncludeCol == null || rows[i].TaxIncludeCol == '')
                    )
                        isEmptyRow = true;
                }

                if (skipEmptyRows == false || isEmptyRow == false) {
                    if (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocItemRequired%>";
                        cellName = "ItemNameCol";
                    }
                    else if (rows[i].AmountCol == null || rows[i].AmountCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocQuanityRequired%>";
                        cellName = "AmountCol";
                    }
                    else if ((rows[i].AmountCol == 0 || rows[i].AmountCol == '0') &&
                    <%=MyContext.PurchasesReturnOptions.AllowZeroInPurchase.ToString().ToLower()%>!= true) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocZeroQuanityRejected%>";
                        cellName = "AmountCol";
                    }
                    else if (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitCostRequired%>";
                        cellName = "UnitCostEvaluateCol";
                    }
                    else if (UseStoreByRecord == true && (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocStoreRequired%>";
                        cellName = "StoreNameCol";
                    }
                    else if (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitRequired%>";
                        cellName = "UnitNameCol";
                    }
                    else if ( <%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%> == true &&
                        (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocStoreRequired%>";
                        cellName = "StoreIdCol";
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
                $("#notifyview").html(errorMessage);
                $("#notifymsgdiv").removeClass("alert-success");
                $("#notifymsgdiv").addClass("alert-danger");
                $("#notifymsgdiv").fadeIn();
                showModal("#notifyModal");
                $("#notifymsgdiv").fadeOut(3000, function () {
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
            if (ValidateRows(false, false) == true) {
                var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                rowIdentifier++;

                var items = JSON.stringify(grid.getDataSource().store()._array);
                var array = JSON.parse(items);
                array.push({ 'RowNoCol': rowIdentifier, 'KeyCol': rowIdentifier.toString() });

                grid.option("dataSource", array);
                grid.option("keyExpr", "KeyCol");
                 
                    SetDataGridEditable(true);
                    if (focusRow)
                        grid.focus(grid.getCellElement(array.length, "BarcodeCol"));
                
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

        function itemSelectedF9(itemId, itemName, rowIndex, isNewItem) {
            if (isNewItem == true) {
                loadItems();
                loadItemUnits();
            }
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            hideModal('#itemInfoModal');

            $.getJSON("../../api/Item/FindByName", { contextKey: ",,,true,1", Source: itemName },
                async function (response) {
                    //var rows = grid.getDataSource().store()._array;
                    //rows[i].ItemNameCol = response[0].Name;
                    //grid.cellValue(rowIndex, "ItemNameCol", response[0].Name);                    
                    if (response != null) {
                        grid.cellValue(rowIndex, "BarcodeCol", response[0].Barcode);
                        grid.cellValue(rowIndex, "ItemIdCol", response[0].ID);
                        grid.cellValue(rowIndex, "ItemNameCol", response[0].Name);
                        grid.cellValue(rowIndex, "UnitIdCol", response[0].UOM_ID);
                        grid.cellValue(rowIndex, "UnitNameCol", response[0].UOMName);
                        if (<%=MyContext.SalesOptions.SetAmountToOne.ToString().ToLower()%>== true)
                            grid.cellValue(rowIndex, "AmountCol", 1);
                        grid.cellValue(rowIndex, "UnitCostEvaluateCol", response[0].DefaultPrice);
                        grid.cellValue(rowIndex, "PercentageDiscountCol", response[0].PercentageDiscount);
                        grid.cellValue(rowIndex, "TaxPercentageValueCol", response[0].PercentageValue);
                        grid.cellValue(rowIndex, "TaxIdCol", response[0].Tax_ID);
                        grid.cellValue(rowIndex, "TaxOnInvoiceTypeCol", response[0].OnInvoiceType);
                        grid.cellValue(rowIndex, "TaxOnReceiptTypeCol", response[0].TaxOnReceiptType);
                        grid.cellValue(rowIndex, "TaxOnDocCreditTypeCol", response[0].OnDocCreditType);
                        grid.cellValue(rowIndex, "TaxSalesAccountIDCol", response[0].SalesAccountID);
                        grid.cellValue(rowIndex, "TaxPurchaseAccountIDCol", response[0].PurchaseAccountID);

                        if (response[0].IsUseTax != null && response[0].IsUseTax == true) {
                            grid.cellValue(rowIndex, "IsUseTaxCol", response[0].IsUseTax);
                            grid.cellValue(rowIndex, "TaxPercentageCol", response[0].TaxPercentage);
                            grid.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", response[0].IsTaxIncludedInPurchase);
                            grid.cellValue(rowIndex, "IsTaxIncludedInSaleCol", response[0].IsTaxIncludedInSale);

                            if (response[0].IsTaxIncludedInSale != null && response[0].IsTaxIncludedInSale == true) {
                                if (response[0].TaxPercentage) {
                                    var result = (response[0].TaxPercentage / (100 + response[0].TaxPercentage));
                                    grid.cellValue(rowIndex, "TaxIncludeCol", result);
                                }
                                else grid.cellValue(rowIndex, "TaxIncludeCol", 0);
                            }
                            else {
                                if (response[0].TaxPercentage != null) {
                                    var result = (response[0].TaxPercentage / 100);
                                    grid.cellValue(rowIndex, "TaxIncludeCol", result);
                                }
                                else grid.cellValue(rowIndex, "TaxIncludeCol", 0);
                            }
                        }
                        else {
                            grid.cellValue(rowIndex, "IsUseTaxCol", false);
                            grid.cellValue(rowIndex, "TaxPercentageCol", 0);
                            grid.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                            grid.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                        }

                        calcDocTotals();
                        grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                    }
                    else {
                        grid.cellValue(rowIndex, "BarcodeCol", null);
                        grid.cellValue(rowIndex, "ItemIdCol", null);
                        grid.cellValue(rowIndex, "ItemNameCol", null);
                        grid.cellValue(rowIndex, "UnitIdCol", null);
                        grid.cellValue(rowIndex, "UnitNameCol", null);
                        grid.cellValue(rowIndex, "AmountCol", null);
                        grid.cellValue(rowIndex, "UnitCostEvaluateCol", null);
                        grid.cellValue(rowIndex, "PercentageDiscountCol", null);
                        grid.cellValue(rowIndex, "TaxPercentageValueCol", null);
                        grid.cellValue(rowIndex, "TaxIdCol", null);
                        grid.cellValue(rowIndex, "TaxOnInvoiceTypeCol", null);
                        grid.cellValue(rowIndex, "TaxOnReceiptTypeCol", null);
                        grid.cellValue(rowIndex, "TaxOnDocCreditTypeCol", null);
                        grid.cellValue(rowIndex, "TaxSalesAccountIDCol", null);
                        grid.cellValue(rowIndex, "TaxPurchaseAccountIDCol", null);
                        grid.cellValue(rowIndex, "IsUseTaxCol", false);
                        grid.cellValue(rowIndex, "TaxPercentageCol", 0);
                        grid.cellValue(rowIndex, "IsTaxIncludedInPurchaseCol", false);
                        grid.cellValue(rowIndex, "IsTaxIncludedInSaleCol", false);
                        calcDocTotals();
                        grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                    }

                    grid.saveEditData();
                });
        }
     
        function clearTools() {
            $("#Id").val(null);
            $("#FromReceiptId").val(null);
            clearDgv();

            if (<%=MyContext.PurchasesReturnOptions.KeepDefaultSupplier.ToString().ToLower()%>!= true)
                $("#acVendor").dxSelectBox('instance').option({ value: null });

            if (<%=MyContext.PurchasesReturnOptions.KeepStore.ToString().ToLower()%>!= true)
                $("#acStore").dxSelectBox('instance').option({ value: null });

            if (<%=MyContext.PurchasesReturnOptions.KeepCostCenter.ToString().ToLower()%>!= true)
                $("#cmbxCostCenter").dxSelectBox('instance').option({ value: null });

            if (<%=MyContext.PurchasesReturnOptions.KeepDocDate.ToString().ToLower()%>!= true) {
                $.getJSON("../../api/General/GetServerDate", null, function (response) {
                    $("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response) });
                });
            }

            $("#txtSerialSearch").val(null);
            $("#cbxInvPerDiscount").prop('checked', false);
            $("#txtInvPerDiscount").val(null);
            setEnabled('#txtInvPerDiscount', false);
            $("#txtInvCashDiscount").val(null);
            setReadOnly("#ToolsPnl", true);
        }

        function SetDataGridEditable(value) {
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            var columns = $("#purchaseReturnGridContainer").dxDataGrid("instance").getVisibleColumns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].allowEditing = value;
                if (columns[i].name == "RowNoCol")
                    columns[i].allowEditing = false;
            }
        }

        function addItem() {
            //clearTools();
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            setEnabled('#txtInvPerDiscount', false);

            $("#Operation").val("Add");
            //SetDataGridEditable(true);
        }

        function findItem() {
            $("#searchDiv").load("../../Purchases/PurchaseReturnUI/FrmPurchaseReturnInvoiceSelect.aspx?requestCode=1");
            showModal('#searchModal');
        }

        function importFromSales() {
            $("#searchDiv").load("../../Purchases/PurchaseUI/FrmPurchaseInvoiceSelect.aspx?requestCode=3&StatusId=2");
            showModal('#searchModal');
        }

        function documentSelected(requestCode, selectedDocumentId) {
            if (requestCode == 1) {
                clearTools();
                hideModal('#searchModal');
                $.getJSON("../../api/PurchaseReturn/FindDocumentById", { InvoiceID: selectedDocumentId },
                    function (response) {
                        if (response.length == 1) {
                            if (response[0].DocStatus_ID == 1)
                                setEnabled("#btnEdit", true);
                            else setEnabled("#btnEdit", false);
                            setEnabled("#btnApprove", true);
                            setEnabled("#btnReset", true);
                            setEnabled("#btnAdd", false);
                            $("#txtRatio").val(response[0].Ratio);
                            $("#cmbxBranch").dxSelectBox('instance').option({ value: response[0].Branch_ID });
                            $("#txtUserRefNo").val(response[0].UserRefNo);
                            $("#txtSerialSearch").val(response[0].Serial);

                            
                                $("#cmbxCostCenter").dxSelectBox('instance').option({ value: response[0].CostCenter_ID });
                                $("#acVendor").dxSelectBox('instance').option({ value: response[0].Contact_ID });
                                $("#acCashAccount").dxSelectBox('instance').option({ value: response[0].CashAccount_ID });
                                $("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response[0].OperationDate) });
                           

                            $("#acTelephone").dxSelectBox('instance').option({ value: response[0].Telephone_ID });
                            $("#acAddress").dxSelectBox('instance').option({ value: response[0].DefaultAddress_ID });
                            $("#acShipAddress").dxSelectBox('instance').option({ value: response[0].ShipToAddress_ID });
                            $("#acPaymentAddress").dxSelectBox('instance').option({ value: response[0].PaymentAddress_ID });
                            $('#' + '<%=this.ddlPaymentMethod.ClientID.ToString() %>').val(response[0].typePayment_ID)
                            $("#imgStatusDiv").css("background-image", getImgStatus(response[0].DocStatus_ID));

                            if (response[0].PercentageDiscount != null && response[0].PercentageDiscount != '' && response[0].PercentageDiscount != 0
                                && response[0].PercentageDiscount != '0') {
                                $('#cbxInvPerDiscount').prop('checked', true);
                                $('#txtInvPerDiscount').val(response[0].PercentageDiscount);
                            }

                            $("#txtInvCashDiscount").val(response[0].CashDiscount);
                            //$("#txtInvCashDiscount").val(response[0].TotalDiscount);

                            $("#Id").val(selectedDocumentId);
                            setEnabled("#btnPrint", true);

                            $.getJSON("../../api/PurchaseReturn/GetDocumentItems", { InvoiceID: selectedDocumentId },
                                function (responseTable) {
                                    if (<%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== false) {                                        
                                            $("#acStore").dxSelectBox('instance').option({ value: responseTable[0].StoreIdCol });                                       
                                    }

                                    console.log("responseTable:");
                                    console.log(responseTable);
                                    var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                    for (var i = 0; i < responseTable.length; i++) {
                                        responseTable[i].KeyCol = responseTable[i].RowNoCol.toString();
                                        rowIdentifier = responseTable[i].RowNoCol
                                        responseTable[i].UnitCostEvaluateCol = responseTable[i].UnitCostCol;
                                        if (responseTable[i].IsUseTaxCol != null && responseTable[i].IsUseTaxCol == true) {
                                            if (responseTable[i].TaxPercentageCol) {
                                                var result = (responseTable[i].TaxPercentageCol / (100 + responseTable[i].TaxPercentageCol));
                                                responseTable[i].TaxIncludeCol = result;
                                            }
                                            else {
                                                responseTable[i].TaxIncludeCol = 0;
                                            }
                                        }
                                        else {
                                            responseTable[i].TaxIncludeCol = 0;
                                        }
                                    }

                                    var dataSource = new DevExpress.data.DataSource({
                                        store: {
                                            type: "array",
                                            key: "KeyCol",
                                            data: responseTable,
                                            // Other ArrayStore properties go here
                                        },
                                        paginate: false
                                        // Other DataSource properties go here
                                    });

                                    grid.option("dataSource", dataSource);
                                    calcDocTotals();
                                });
                        }
                    });
            }
            else if (requestCode == 2) {
                hideModal('#searchModal');
                var printTemplateId = $("#cmbxTemplate").dxSelectBox('instance').option('value');
                if (printTemplateId != null && printTemplateId != '') {
                    window.open('../../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&InvoiceID=' + selectedDocumentId + "&DocKindId=" + '<%=DocumentKindClass.PurchaseReturnInvoice%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                }
                else {
                    showErrorMessage("<%=Resources.Messages.MsgDocPrintTemplateRequired%>", "#cmbxTemplate");
                }
            }
            else if (requestCode == 3) {
                hideModal('#searchModal');
                $.getJSON("../../api/Purchase/FindDocumentById", { InvoiceID: selectedDocumentId },
                    function (response) {
                        if (response.length == 1) {
                            /*setEnabled("#btnEdit", true);
                            setEnabled("#btnApprove", true);
                            setEnabled("#btnReset", true);
                            setEnabled("#btnAdd", false);*/
                            $("#txtRatio").val(response[0].Ratio);
                            $("#cmbxBranch").dxSelectBox('instance').option({ value: response[0].Branch_ID });
                            $("#txtUserRefNo").val(response[0].UserRefNo);

                           
                                $("#cmbxCostCenter").dxSelectBox('instance').option({ value: response[0].CostCenter_ID });
                                $("#acVendor").dxSelectBox('instance').option({ value: response[0].Contact_ID });
                                $("#acCashAccount").dxSelectBox('instance').option({ value: response[0].CashAccount_ID });
                                //$("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response[0].OperationDate) });
                             

                            $("#acTelephone").dxSelectBox('instance').option({ value: response[0].Telephone_ID });
                            $("#acAddress").dxSelectBox('instance').option({ value: response[0].DefaultAddress_ID });
                            $("#acShipAddress").dxSelectBox('instance').option({ value: response[0].ShipToAddress_ID });
                            $("#acPaymentAddress").dxSelectBox('instance').option({ value: response[0].PaymentAddress_ID });
                            //$("#imgStatusDiv").css("background-image", getImgStatus(response[0].DocStatus_ID));
                            $("#FromReceiptId").val(selectedDocumentId);

                            if (response[0].PercentageDiscount > 0) {
                                $("#cbxInvPerDiscount").prop('checked', true);
                                setEnabled('#txtInvPerDiscount', true);
                                $("#txtInvPerDiscount").val(response[0].PercentageDiscount);
                            }
                            $("#txtInvCashDiscount").val(response[0].CashDiscount);

                            $.getJSON("../../api/Purchase/GetDocumentItems", { InvoiceID: selectedDocumentId },
                                function (responseTable) {
                                    if (<%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== false) {
                                        
                                            $("#acStore").dxSelectBox('instance').option({ value: responseTable[0].StoreIdCol });
                                         
                                    }

                                    var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                                    for (var i = 0; i < responseTable.length; i++) {
                                        responseTable[i].KeyCol = responseTable[i].RowNoCol.toString();
                                        rowIdentifier = responseTable[i].RowNoCol
                                        responseTable[i].ReceiptDetailIdCol = responseTable[i].IdCol;
                                        responseTable[i].IdCol = null;

                                        if (responseTable[i].IsUseTaxCol != null && responseTable[i].IsUseTaxCol == true) {
                                            if (responseTable[i].TaxPercentageCol) {
                                                var result = (responseTable[i].TaxPercentageCol / (100 + responseTable[i].TaxPercentageCol));
                                                responseTable[i].TaxIncludeCol = result;
                                            }
                                            else {
                                                responseTable[i].TaxIncludeCol = 0;
                                            }
                                        }
                                        else {
                                            responseTable[i].TaxIncludeCol = 0;
                                        }
                                    }

                                    var dataSource = new DevExpress.data.DataSource({
                                        store: {
                                            type: "array",
                                            key: "KeyCol",
                                            data: responseTable,
                                            // Other ArrayStore properties go here
                                        },
                                        paginate: false
                                        // Other DataSource properties go here
                                    });

                                    grid.option("dataSource", dataSource);
                                    calcDocTotals();
                                });
                        }
                    });
            }
        }

        function showOperationDetials(SourceDocumentId) {
            $("#operationDetailsDiv").load("../../OperationDetails/FrmOperationDetials.aspx?SourceDocId=" + SourceDocumentId + "&SourceTableId=" +<%=DocumentKindClass.PurchaseReturnInvoice%>);
            showModal('#operationDetailsModal');
        }

        function getDocTotalBeforTax() {
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            grid.saveEditData();
            var rows = grid.getDataSource().store()._array;
            var total = 0.00;

            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                    (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                    (rows[i].AmountCol == null || rows[i].AmountCol == '') &&
                    //(rows[i].BounusCol == null || rows[i].BounusCol == '') &&                                
                    (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                    (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') &&
                    (rows[i].PercentageDiscountCol == null || rows[i].PercentageDiscountCol == '') &&
                    (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '') &&
                    (rows[i].CashDiscountCol == null || rows[i].CashDiscountCol == '') &&
                    (rows[i].RowTotalTaxCol == null || rows[i].RowTotalTaxCol == '') &&
                    (rows[i].TotalBeforTaxCol == null || rows[i].TotalBeforTaxCol == '') &&
                    (rows[i].TotalCostEvaluateCol == null || rows[i].TotalCostEvaluateCol == '') &&
                    (rows[i].BatchIdCol == null || rows[i].BatchIdCol == '') &&
                    (rows[i].TruckIdCol == null || rows[i].TruckIdCol == '') &&
                    (rows[i].ShelfIdCol == null || rows[i].ShelfIdCol == '') &&
                    (rows[i].CostCenterIdCol == null || rows[i].CostCenterIdCol == '') &&
                    (rows[i].NotesCol == null || rows[i].NotesCol == '') &&
                    (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                    (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') &&
                    (rows[i].TaxPercentageValueCol == null || rows[i].TaxPercentageValueCol == '') &&
                    (rows[i].TaxIncludeCol == null || rows[i].TaxIncludeCol == '')
                )
                    isEmptyRow = true;

                if (isEmptyRow == false) {
                    var TotalBeforTax = getRowTotalBeforTaxVal(rows[i]);
                    rows[i].TotalBeforTaxCol = TotalBeforTax;
                    total += TotalBeforTax;
                }
            }

            grid.saveEditData();
            return total;
        }

        function calcDocTotals() {
            if (<%=MyContext.PurchasesReturnOptions.DiscountPerRow.ToString().ToLower()%>== false) {
                var isPercent = $('#cbxInvPerDiscount').prop('checked');
                var billDiscount = 0.00, totalBeforTaxSum = 0.00;
                var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");

                totalBeforTaxSum = getDocTotalBeforTax();
                //totalBeforTaxSum = grid.getTotalSummaryValue('TotalBeforTaxSum');
                if (isPercent == true) {
                    var percent = $("#txtInvPerDiscount").val();
                    /*var totalCashDiscountSum = grid.getTotalSummaryValue('TotalCashDiscountSum');
                    var totalTaxSum = grid.getTotalSummaryValue('TotalTaxSum');                
                    var totalCostEvaluateSum = grid.getTotalSummaryValue('TotalCostEvaluateSum');*/
                    billDiscount = (percent / 100) * totalBeforTaxSum;
                    $("#txtInvCashDiscount").val(billDiscount);
                }
                else if ($("#txtInvCashDiscount").val() != null && $("#txtInvCashDiscount").val() != '')
                    billDiscount = $("#txtInvCashDiscount").val();
                else billDiscount = 0;

                var rows = grid.getDataSource().store()._array;

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].AmountCol == null || rows[i].AmountCol == '') &&
                        //(rows[i].BounusCol == null || rows[i].BounusCol == '') &&                                
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') &&
                        (rows[i].PercentageDiscountCol == null || rows[i].PercentageDiscountCol == '') &&
                        (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '') &&
                        (rows[i].CashDiscountCol == null || rows[i].CashDiscountCol == '') &&
                        (rows[i].RowTotalTaxCol == null || rows[i].RowTotalTaxCol == '') &&
                        (rows[i].TotalBeforTaxCol == null || rows[i].TotalBeforTaxCol == '') &&
                        (rows[i].TotalCostEvaluateCol == null || rows[i].TotalCostEvaluateCol == '') &&
                        (rows[i].BatchIdCol == null || rows[i].BatchIdCol == '') &&
                        (rows[i].TruckIdCol == null || rows[i].TruckIdCol == '') &&
                        (rows[i].ShelfIdCol == null || rows[i].ShelfIdCol == '') &&
                        (rows[i].CostCenterIdCol == null || rows[i].CostCenterIdCol == '') &&
                        (rows[i].NotesCol == null || rows[i].NotesCol == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') &&
                        (rows[i].TaxPercentageValueCol == null || rows[i].TaxPercentageValueCol == '') &&
                        (rows[i].TaxIncludeCol == null || rows[i].TaxIncludeCol == '')
                    )
                        isEmptyRow = true;

                    if (isEmptyRow == false) {
                        var rowDiscountPercentage = 0.00, rowDiscountTotal = 0.00, rowTotalBeforTax = 0.00, Amount = 0.00;

                        if (rows[i].AmountCol != null && rows[i].AmountCol != '' && rows[i].AmountCol != 0.00)
                            Amount = rows[i].AmountCol;

                        if (rows[i].TotalBeforTaxCol != null && rows[i].TotalBeforTaxCol != '' && rows[i].TotalBeforTaxCol != 0.00)
                            rowTotalBeforTax = rows[i].TotalBeforTaxCol;

                        if (totalBeforTaxSum != 0 && totalBeforTaxSum != '' && totalBeforTaxSum != null)
                            rowDiscountPercentage = rowTotalBeforTax / totalBeforTaxSum;

                        rowDiscountTotal = rowDiscountPercentage * billDiscount;
                        rows[i].DiscountTotalCol = formatToText(rowDiscountTotal);
                        if (Amount != 0 && Amount != '' && Amount != null) {
                            rows[i].CashDiscountCol = formatToText(rowDiscountTotal / Amount);
                        }
                        else rows[i].CashDiscountCol = null;

                        if (rowTotalBeforTax != 0 && rowTotalBeforTax != '' && rowTotalBeforTax != null)
                            rowDiscountPercentage = (rowDiscountTotal / rowTotalBeforTax) * 100;
                        rows[i].PercentageDiscountCol = formatToText(rowDiscountPercentage);
                    }
                }
                grid.saveEditData();
                grid.refresh();
            }

            if (true) {
                var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
                grid.saveEditData();
                var rows = grid.getDataSource().store()._array;
                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].AmountCol == null || rows[i].AmountCol == '') &&
                        //(rows[i].BounusCol == null || rows[i].BounusCol == '') &&                                
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') &&
                        (rows[i].PercentageDiscountCol == null || rows[i].PercentageDiscountCol == '') &&
                        (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '') &&
                        (rows[i].CashDiscountCol == null || rows[i].CashDiscountCol == '') &&
                        (rows[i].RowTotalTaxCol == null || rows[i].RowTotalTaxCol == '') &&
                        (rows[i].TotalBeforTaxCol == null || rows[i].TotalBeforTaxCol == '') &&
                        (rows[i].TotalCostEvaluateCol == null || rows[i].TotalCostEvaluateCol == '') &&
                        (rows[i].BatchIdCol == null || rows[i].BatchIdCol == '') &&
                        (rows[i].TruckIdCol == null || rows[i].TruckIdCol == '') &&
                        (rows[i].ShelfIdCol == null || rows[i].ShelfIdCol == '') &&
                        (rows[i].CostCenterIdCol == null || rows[i].CostCenterIdCol == '') &&
                        (rows[i].NotesCol == null || rows[i].NotesCol == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') &&
                        (rows[i].TaxPercentageValueCol == null || rows[i].TaxPercentageValueCol == '') &&
                        (rows[i].TaxIncludeCol == null || rows[i].TaxIncludeCol == '')
                    )
                        isEmptyRow = true;

                    if (isEmptyRow == false) {
                        var DiscountTotal = 0.00, Amount = 0.00, TotalBeforTax = 0.00, SafeBeforTax = 0.00, TotalCostEvaluate = 0.00;
                        if (rows[i].AmountCol != null && rows[i].AmountCol != '' && rows[i].AmountCol != 0.00)
                            Amount = rows[i].AmountCol;
                        if (rows[i].CashDiscountCol != null && rows[i].CashDiscountCol != '' && rows[i].CashDiscountCol != 0.00)
                            DiscountTotal = rows[i].CashDiscountCol;
                        DiscountTotal = Amount * DiscountTotal;
                        rows[i].DiscountTotalCol = formatToText(DiscountTotal);

                        TotalBeforTax = getRowTotalBeforTaxVal(rows[i]);
                        rows[i].TotalBeforTaxCol = TotalBeforTax;
                        if (TotalBeforTax != null && TotalBeforTax != null) {
                            TotalBeforTax = TotalBeforTax - DiscountTotal;
                            TotalBeforTax = TotalBeforTax * rows[i].TaxIncludeCol;
                            TotalBeforTax = formatToText(TotalBeforTax);
                            rows[i].RowTotalTaxCol = TotalBeforTax;
                        }
                        else rows[i].RowTotalTaxCol = null;

                        SafeBeforTax = getRowSafeBeforTaxVal(rows[i]);
                        rows[i].SafeBeforTaxCol = formatToText(SafeBeforTax);

                        TotalCostEvaluate = getRowTotal(rows[i]);
                        rows[i].TotalCostEvaluateCol = formatToText(TotalCostEvaluate);
                    }
                }

                grid.saveEditData();
                grid.refresh();
            }
        }

        function formatToText(n) {
            var x = Math.round((n + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;
            return x;
        }

        function editItem() {
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            $("#Operation").val("Edit");
            SetDataGridEditable(false);
        }

        function saveItem(isApproving) {
            var dateBox = $("#txtOperationDate").dxDateBox("instance");
            var paymentMethod = $('#' + '<%=this.ddlPaymentMethod.ClientID.ToString() %>' + ' option:selected').val();
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            grid.saveEditData();

            <%--if (paymentMethod == "" || paymentMethod == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocPaymentMethodRequired%>", "#PaymentMethod");
                return false;
            }
            else--%>
            if (/*paymentMethod == -1 &&*/$("#acVendor").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocVendorRequired%>", "#acVendor");
                return false;
            }
            else if (dateBox.option('text') == null || dateBox.option('text') == '') {
                showErrorMessage("<%=Resources.Messages.MsgDocOperationDateRequired%>", "#txtOperationDate");
                return false;
            }
            else if ($("#cmbxBranch").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocBranchRequired%>", "#cmbxBranch");
                return false;
            }
            else if (<%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== false && $("#acStore").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocStoreRequired%>", "#txtOperationDate");
                return false;
            }
            else if (<%=MyContext.PurchasesReturnOptions.UseCostCenter.ToString().ToLower()%>== true &&
                <%=MyContext.PurchasesReturnOptions.ForceCostCenter.ToString().ToLower()%>== true &&
                $("#cmbxCostCenter").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocCostCenterRequired%>", "#cmbxCostCenter");
                return false;
            }
            else if (<%=MyContext.PurchasesReturnOptions.UseRefNo.ToString().ToLower()%>== true &&
                <%=MyContext.PurchasesReturnOptions.ForceRefNo.ToString().ToLower()%>== true &&
                ($("#txtUserRefNo").val() == null || $("#txtUserRefNo").val() == '')) {
                showErrorMessage("<%=Resources.Messages.MsgDoctUserRefNoRequired%>", "#txtUserRefNo");
                return false;
            }
            else if (!ValidateRows(true, true)) {
                return false;
            }
            else {
                var operation = $("#Operation").val();
                var amount = null;
                var editMode = false;
                if (operation == "Edit")
                    editMode = true;

                var branchId = $("#cmbxBranch").dxSelectBox('instance').option('value'),
                    storeId = $("#acStore").dxSelectBox('instance').option('value'),
                    costCenterId = $("#cmbxCostCenter").dxSelectBox('instance').option('value'),
                    userRefNo = $("#txtUserRefNo").val(),
                    vendorId = $("#acVendor").dxSelectBox('instance').option('value'),
                    acPaymentAddress = $("#acPaymentAddress").dxSelectBox('instance').option('value'),
                    acTelephone = $("#acTelephone").dxSelectBox('instance').option('value'),
                    ratio = $("#txtRatio").val(),
                    docDate = dateBox.option('text').trim(),
                    acCashAccount = $("#acCashAccount").dxSelectBox('instance').option('value'),
                    currencyId = '<%=ddlCurrency.SelectedValue%>';
                isPercentDiscount = $('#cbxInvPerDiscount').prop('checked'),
                    invPercentageDiscount = $("#txtInvPerDiscount").val(),
                    invCashDiscount = $("#txtInvCashDiscount").val();

                var totalCashDiscountSum = grid.getTotalSummaryValue('TotalCashDiscountSum');
                var totalTaxSum = grid.getTotalSummaryValue('TotalTaxSum');
                var totalBeforTaxSum = grid.getTotalSummaryValue('TotalBeforTaxSum');
                var totalCostEvaluateSum = grid.getTotalSummaryValue('TotalCostEvaluateSum');

                var dataRows = [];
                var rows = grid.getDataSource().store()._array;

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].AmountCol == null || rows[i].AmountCol == '') &&
                        //(rows[i].BounusCol == null || rows[i].BounusCol == '') &&                                
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') &&
                        (rows[i].PercentageDiscountCol == null || rows[i].PercentageDiscountCol == '') &&
                        (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '') &&
                        (rows[i].CashDiscountCol == null || rows[i].CashDiscountCol == '') &&
                        (grid.cellValue(i, "RowTotalTaxCol") == null || grid.cellValue(i, "RowTotalTaxCol") == '') &&
                        (grid.cellValue(i, "TotalBeforTaxCol") == null || grid.cellValue(i, "TotalBeforTaxCol") == '') &&
                        (grid.cellValue(i, "TotalCostEvaluateCol") == null || grid.cellValue(i, "TotalCostEvaluateCol") == '') &&
                        (rows[i].BatchIdCol == null || rows[i].BatchIdCol == '') &&
                        (rows[i].TruckIdCol == null || rows[i].TruckIdCol == '') &&
                        (rows[i].ShelfIdCol == null || rows[i].ShelfIdCol == '') &&
                        (rows[i].CostCenterIdCol == null || rows[i].CostCenterIdCol == '') &&
                        (rows[i].NotesCol == null || rows[i].NotesCol == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') &&
                        (rows[i].TaxPercentageValueCol == null || rows[i].TaxPercentageValueCol == '') &&
                        (rows[i].TaxIncludeCol == null || rows[i].TaxIncludeCol == '')
                    )
                        isEmptyRow = true;

                    if (isEmptyRow == false) {
                        var idCol = null, amountCol = null, bounusCol = null, unitCostEvaluateCol = null, percentageDiscountCol = null, cashDiscountCol = null, rowTotalTaxCol = null,
                            totalBeforTaxCol = null, totalCostEvaluateCol = null, itemIdCol = null, unitIdCol = null, taxPercentageValueCol = null, taxIncludeCol = null,
                            taxIdCol = null, taxOnInvoiceTypeCol = null, taxOnReceiptTypeCol = null, taxOnDocCreditTypeCol = null, taxSalesAccountIDCol = null, taxPurchaseAccountIDCol = null,
                            rowStoreIdCol = storeId, rowCostCenterIdCol = costCenterId, batchIdCol = null, truckIdCol = null, shelfIdCol = null, notesCol = null,
                            isUseTaxCol = null, taxPercentageCol = null, isTaxIncludedInPurchaseCol = null, isTaxIncludedInSaleCol = null, receiptDetailIdCol = null;

                        if (rows[i].IdCol != null && rows[i].IdCol != '')
                            idCol = rows[i].IdCol;
                        if (rows[i].ReceiptDetailIdCol != null && rows[i].ReceiptDetailIdCol != '')
                            receiptDetailIdCol = rows[i].ReceiptDetailIdCol;
                        console.log("receiptDetailIdCol:" + receiptDetailIdCol);
                        if (rows[i].AmountCol != null && rows[i].AmountCol != '')
                            amountCol = rows[i].AmountCol;
                        if (rows[i].BounusCol != null && rows[i].BounusCol != '')
                            bounusCol = rows[i].BounusCol;
                        if (rows[i].UnitCostEvaluateCol != null && rows[i].UnitCostEvaluateCol != '')
                            unitCostEvaluateCol = rows[i].UnitCostEvaluateCol;
                        if (rows[i].PercentageDiscountCol != null && rows[i].PercentageDiscountCol != '')
                            percentageDiscountCol = rows[i].PercentageDiscountCol;
                        if (rows[i].CashDiscountCol != null && rows[i].CashDiscountCol != '')
                            cashDiscountCol = rows[i].CashDiscountCol;
                        /*if (rows[i].RowTotalTaxCol != null && rows[i].RowTotalTaxCol != '')
                            rowTotalTaxCol = rows[i].RowTotalTaxCol;*/
                        if (grid.cellValue(i, "RowTotalTaxCol") != null && grid.cellValue(i, "RowTotalTaxCol") != '')
                            rowTotalTaxCol = grid.cellValue(i, "RowTotalTaxCol");
                        if (grid.cellValue(i, "TotalBeforTaxCol") != null && grid.cellValue(i, "TotalBeforTaxCol") != '')
                            totalBeforTaxCol = grid.cellValue(i, "TotalBeforTaxCol");
                        if (grid.cellValue(i, "TotalCostEvaluateCol") != null && grid.cellValue(i, "TotalCostEvaluateCol") != '')
                            totalCostEvaluateCol = grid.cellValue(i, "TotalCostEvaluateCol");
                        itemIdCol = rows[i].ItemIdCol;
                        unitIdCol = rows[i].UnitIdCol;
                        if (rows[i].TaxPercentageValueCol != null && rows[i].TaxPercentageValueCol != '')
                            taxPercentageValueCol = rows[i].TaxPercentageValueCol;
                        if (rows[i].TaxIncludeCol != null && rows[i].TaxIncludeCol != '')
                            taxIncludeCol = rows[i].TaxIncludeCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseStoreByRecord.ToString().ToLower()%>== false)
                            rowStoreIdCol = rows[i].StoreIdCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseCostCenterByRecord.ToString().ToLower()%>== false)
                            rowCostCenterIdCol = rows[i].CostCenterIdCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseBatchByRecord.ToString().ToLower()%>== false)
                            batchIdCol = rows[i].BatchIdCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseTruckByRecord.ToString().ToLower()%>== false)
                            truckIdCol = rows[i].TruckIdCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseShelf.ToString().ToLower()%>== false)
                            shelfIdCol = rows[i].ShelfIdCol;

                        if (<%=MyContext.PurchasesReturnOptions.UseNote.ToString().ToLower()%>== false)
                            notesCol = rows[i].NotesCol;

                        if (rows[i].TaxIdCol != null && rows[i].TaxIdCol != '')
                            taxIdCol = rows[i].TaxIdCol;
                        if (rows[i].TaxOnInvoiceTypeCol != null && rows[i].TaxOnInvoiceTypeCol != '')
                            taxOnInvoiceTypeCol = rows[i].TaxOnInvoiceTypeCol;
                        if (rows[i].TaxOnReceiptTypeCol != null && rows[i].TaxOnReceiptTypeCol != '')
                            taxOnReceiptTypeCol = rows[i].TaxOnReceiptTypeCol;
                        if (rows[i].TaxOnDocCreditTypeCol != null && rows[i].TaxOnDocCreditTypeCol != '')
                            taxOnDocCreditTypeCol = rows[i].TaxOnDocCreditTypeCol;
                        if (rows[i].TaxSalesAccountIDCol != null && rows[i].TaxSalesAccountIDCol != '')
                            taxSalesAccountIDCol = rows[i].TaxSalesAccountIDCol;
                        if (rows[i].TaxPurchaseAccountIDCol != null && rows[i].TaxPurchaseAccountIDCol != '')
                            taxPurchaseAccountIDCol = rows[i].TaxPurchaseAccountIDCol;

                        if (rows[i].IsUseTaxCol != null && rows[i].IsUseTaxCol != '')
                            isUseTaxCol = rows[i].IsUseTaxCol;
                        else isUseTaxCol = false;

                        if (isUseTaxCol == true) {
                            if (rows[i].TaxPercentageCol != null && rows[i].TaxPercentageCol != '')
                                taxPercentageCol = rows[i].TaxPercentageCol;
                            if (rows[i].IsTaxIncludedInPurchaseCol != null && rows[i].IsTaxIncludedInPurchaseCol != '')
                                isTaxIncludedInPurchaseCol = rows[i].IsTaxIncludedInPurchaseCol;
                            if (rows[i].IsTaxIncludedInSaleCol != null && rows[i].IsTaxIncludedInSaleCol != '')
                                isTaxIncludedInSaleCol = rows[i].IsTaxIncludedInSaleCol;
                        }
                        else {
                            taxPercentageCol = null;
                            isTaxIncludedInPurchaseCol = null;
                            isTaxIncludedInSaleCol = null;
                        }

                        var row = {
                            'ID': idCol, 'Quantity': amountCol, 'Bounus': bounusCol, 'UnitCost': unitCostEvaluateCol, 'UnitCostEvaluate': unitCostEvaluateCol, 'PercentageDiscount': percentageDiscountCol,
                            'CashDiscount': cashDiscountCol, 'TotalTax': rowTotalTaxCol, 'TotalBeforTax': totalBeforTaxCol, 'Total': totalCostEvaluateCol, 'Item_ID': itemIdCol, 'Uom_ID': unitIdCol,
                            'TaxPercentageValue': taxPercentageValueCol, 'TaxInclude': taxIncludeCol, 'Tax_ID': taxIdCol, 'OnInvoiceType': taxOnInvoiceTypeCol, 'TaxOnReceiptType': taxOnReceiptTypeCol,
                            'TaxOnDocCreditType': taxOnDocCreditTypeCol, 'TaxSalesAccountID': taxSalesAccountIDCol, 'TaxPurchaseAccountID': taxPurchaseAccountIDCol, 'Store_ID': rowStoreIdCol, 'CostCenter_ID': rowCostCenterIdCol,
                            'Batch_ID': batchIdCol, 'TruckId': truckIdCol, 'ShelfId': shelfIdCol, 'Notes': notesCol, 'IsUseTax': isUseTaxCol, 'TaxPercentage': taxPercentageCol, 'IsTaxIncludedInPurchase': isTaxIncludedInPurchaseCol,
                            'IsTaxIncludedInSale': isTaxIncludedInSaleCol, 'ReceiptDetail_ID': receiptDetailIdCol
                        };
                        dataRows.push(row);
                        //dataRows.push(rows[i].data);
                    }
                }

                dataRows = JSON.stringify(dataRows);
                var id = $("#Id").val();
                var fromReceiptId = $("#FromReceiptId").val();
                $.post("../../api/PurchaseReturn/SavePurchaseReturnInvoice", {
                    'Id': id, 'BranchId': branchId, 'StoreId': storeId, 'PaymentMethod': paymentMethod, 'CostCenterId': costCenterId, 'UserRefNo': userRefNo,
                    'VendorId': vendorId, 'AcPaymentAddress': acPaymentAddress,
                    'CurrencyId': currencyId, 'AcTelephone': acTelephone, 'AcCashAccount': acCashAccount,
                    'OperationDate': docDate, 'Ratio': ratio, 'TotalCashDiscountSum': totalCashDiscountSum,
                    'TotalTaxSum': totalTaxSum, 'Total': totalBeforTaxSum, 'GrossTotal': totalCostEvaluateSum,
                    'EditMode': editMode, 'UserProfileContact_ID': '<%=MyContext.UserProfile.Contact_ID%>',
                    'IsApproving': isApproving, 'FromReceiptId': fromReceiptId,/*'IsPercentDiscount':isPercentDiscount,*/'PercentageDiscount': invPercentageDiscount, 'Source': dataRows
                },
                    function (response) {
                        if (response.Code > 0) {
                            clearTools();
                            $("#Operation").val(null);
                            setReadOnly("#ToolsPnl", true);
                            setEnabled(".executeBtn", false);
                            setEnabled(".divBtnAdd", false);
                            setEnabled("#btnAdd", true);
                            setEnabled("#btnSearch", true);
                            setEnabled("#btnPrint", true);
                            showSuccessPrint("<%=Resources.Messages.MsgDocOperationSucessPrint%>", "<%=Resources.Messages.MsgDocSaveSuccess%>", response.Code);

                            $("#btnLoad").click(function () {
                                hideModal('#printModal');
                                documentSelected(1, response.Code);
                            });

                            $("#btnConfirmPrint").click(function () {
                                hideModal('#printModal');
                                var printTemplateId = $("#cmbxTemplate").dxSelectBox('instance').option('value');
                                window.open('../../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&InvoiceID=' + response.Code + "&DocKindId=" + '<%=DocumentKindClass.PurchaseReturnInvoice%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);

                                if (<%=MyContext.PurchasesReturnOptions.AddNewAfterSave.ToString().ToLower()%>== true)
                                    addItem();
                            });

                            //------------------------ btnSendEmail ---------------------------//
                            $("#btnSendEmail").click(function () {
                                $.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateEmail(queryResponse) == true) {
                                        $.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>', "SendType": 1 }, function (sendResponse) {
                                            if (sendResponse.Code == 100)
                                                hideModal('#printModal');
                                            else {
                                                $("#sendview").html(sendResponse.Message);
                                                $("#sendmsgdiv").removeClass("alert-success");
                                                $("#sendmsgdiv").addClass("alert-danger");
                                                $("#sendmsgdiv").fadeIn();
                                                $("#sendmsgdiv").fadeOut(5000);
                                            }
                                        });
                                    }
                                    else {
                                        hideModal('#printModal');
                                        showModal('#emailModal');
                                        //------------------------ Confirm Email ---------------------------//                                                                                
                                        $("#btnConfirmEmail").click(function () {
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
                                            var emailVal = $('#txtEmailTo').val();
                                            var emailResult = emailRGEX.test(emailVal);

                                            if ($('#txtEmailTo').val() == "") {
                                                $("#emailmsgdiv").removeClass("alert-success");
                                                $("#emailmsgdiv").addClass("alert-danger");
                                                $("#emailmsgdiv").fadeIn();
                                                $("#emailview").html("<%=Resources.Messages.MsgEmailRequired%>");
                                                $("#emailmsgdiv").fadeOut(5000);
                                                $('#txtEmailTo').focus();
                                                return false;
                                            }
                                            else if (emailResult == false) {
                                                $("#emailmsgdiv").removeClass("alert-success");
                                                $("#emailmsgdiv").addClass("alert-danger");
                                                $("#emailmsgdiv").fadeIn();
                                                $("#emailview").html("<%=Resources.Messages.MsgEmailInvalid%>");
                                                $("#emailmsgdiv").fadeOut(5000);
                                                $('#txtEmailTo').focus();

                                            }
                                            else {
                                                $.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>', "SendType": 2, "Data": $('#txtEmailTo').val() }, function (sendResponse) {
                                                    if (sendResponse.Code == 100)
                                                        hideModal('#printModal');
                                                    else {
                                                        $("#sendview").html(sendResponse.Message);
                                                        $("#sendmsgdiv").removeClass("alert-success");
                                                        $("#sendmsgdiv").addClass("alert-danger");
                                                        $("#sendmsgdiv").fadeIn();
                                                        $("#sendmsgdiv").fadeOut(5000);
                                                    }
                                                });
                                                //hideModal('#emailModal');
                                            }
                                        });

                                        //------------------------ Back ---------------------------//
                                        $("#btnBackEmail").click(function () {
                                            hideModal('#emailModal');
                                            showModal('#printModal');
                                        });
                                    }
                                });
                            });

                            //------------------------ btnSendSms ---------------------------//
                            $("#btnSendSms").click(function () {
                                $.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateMobile(queryResponse) == true) {
                                        $.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>', "SendType": 1 }, function (sendResponse) {
                                            if (sendResponse.Code == 100)
                                                hideModal('#printModal');
                                            else {
                                                $("#sendview").html(sendResponse.Message);
                                                $("#sendmsgdiv").removeClass("alert-success");
                                                $("#sendmsgdiv").addClass("alert-danger");
                                                $("#sendmsgdiv").fadeIn();
                                                $("#sendmsgdiv").fadeOut(5000);
                                            }
                                        });
                                    }
                                    else {
                                        hideModal('#printModal');
                                        showModal('#mobileModal');
                                        //------------------------ Confirm Mobile ---------------------------//
                                        $("#btnConfirmMobile").click(function () {
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
                                            var mobileVal = $('#txtMobileTo').val();
                                            var mobileResult = mobileRGEX.test(mobileVal);

                                            if ($('#txtMobileTo').val() == "") {
                                                $("#mobilemsgdiv").removeClass("alert-success");
                                                $("#mobilemsgdiv").addClass("alert-danger");
                                                $("#mobilemsgdiv").fadeIn();
                                                $("#mobileview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                                $("#mobilemsgdiv").fadeOut(5000);
                                                $('#txtMobileTo').focus();
                                                return false;
                                            }
                                            else if (mobileResult == false) {
                                                $("#mobilemsgdiv").removeClass("alert-success");
                                                $("#mobilemsgdiv").addClass("alert-danger");
                                                $("#mobilemsgdiv").fadeIn();
                                                $("#mobileview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                                $("#mobilemsgdiv").fadeOut(5000);
                                                $('#txtMobileTo').focus();
                                            }
                                            else {
                                                hideModal('#mobileModal');
                                                $.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.PurchaseReturnInvoice%>', "SendType": 2, "Data": $('#txtMobileTo').val() }, function (sendResponse) {
                                                    if (sendResponse.Code == 100)
                                                        hideModal('#printModal');
                                                    else {
                                                        $("#sendview").html(sendResponse.Message);
                                                        $("#sendmsgdiv").removeClass("alert-success");
                                                        $("#sendmsgdiv").addClass("alert-danger");
                                                        $("#sendmsgdiv").fadeIn();
                                                        $("#sendmsgdiv").fadeOut(5000);
                                                    }
                                                });
                                                //showModal('#printModal');
                                            }
                                        });

                                        //------------------------ Back ---------------------------//
                                        $("#btnBackMobile").click(function () {
                                            hideModal('#mobileModal');
                                            showModal('#printModal');
                                        });
                                    }
                                });
                            });

                            //------------------------ btnSendWhatsapp ---------------------------//
                            $("#btnSendWhatsapp").click(function () {
                                hideModal('#printModal');
                                showModal('#whatsappModal');
                                //------------------------ Back ---------------------------//
                                $("#btnConfirmWhatsapp").click(function () {
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
                                    var whatsappVal = $('#txtWhatsappTo').val();
                                    var whatsappResult = whatsappRGEX.test(whatsappVal);

                                    if ($('#txtWhatsappTo').val() == "") {
                                        $("#whatsappmsgdiv").removeClass("alert-success");
                                        $("#whatsappmsgdiv").addClass("alert-danger");
                                        $("#whatsappmsgdiv").fadeIn();
                                        $("#whatsappview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                        $("#whatsappmsgdiv").fadeOut(5000);
                                        $('#txtWhatsappTo').focus();
                                        return false;
                                    }
                                    else if (whatsappResult == false) {
                                        $("#whatsappmsgdiv").removeClass("alert-success");
                                        $("#whatsappmsgdiv").addClass("alert-danger");
                                        $("#whatsappmsgdiv").fadeIn();
                                        $("#whatsappview").html("<%=Resources.Messages.MsgMobileRequired%>");
                                        $("#whatsappmsgdiv").fadeOut(5000);
                                        $('#txtWhatsappTo').focus();
                                    }
                                    else {
                                        hideModal('#whatsappModal');
                                        //showModal('#printModal');
                                    }
                                });
                                //------------------------ Back ---------------------------//
                                $("#btnBackWhatsapp").click(function () {
                                    hideModal('#whatsappModal');
                                    showModal('#printModal');
                                });
                            });

                            $("#btnCancelPrint").click(function () {
                                if (<%=MyContext.PurchasesReturnOptions.AddNewAfterSave.ToString().ToLower()%>== true)
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
            setEnabled(".divBtnAdd", false);
            clearTools();
            setEnabled("#btnReset", true);
            setEnabled("#btnAdd", true);
            setEnabled("#btnSearch", true);
            setEnabled("#btnPrint", true);
        }

        function printItem() {
            if ($("#Id").val() != null && $("#Id").val() != '' && $("#Id").val().length > 0) {
                var id = $("#Id").val();
                documentSelected(2, id);
            }
            else { 
                $("#searchDiv").load("../../Purchases/PurchaseReturnUI/FrmPurchaseReturnInvoiceSelect.aspx?requestCode=2");
                showModal('#searchModal');
            }
        }

        function saveGridOrdering() {
            var grid = $("#purchaseReturnGridContainer").dxDataGrid("instance");
            var colCount = grid.columnCount();
            var columnIndicies = [];
            for (var i = 0; i < colCount; i++) {
                var visibleIndex = grid.columnOption(i, "visibleIndex");
                if (visibleIndex >= 0)
                    columnIndicies.push({ index: visibleIndex, fieldName: grid.columnOption(i, "dataField") });
            }

            $.post("../../api/GridOrdering/SaveGridOrdering", {
                'GridId': '<%=DocumentKindClass.PurchaseReturnInvoice%>',
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
        function CancelReturnReceiptApprovel(SourceDocumentId) {
            $.post("../../api/PurchaseReturn/CancelReturnReceiptApprovel", { ID: SourceDocumentId },
                function (response) {
                   
                });

        }
    </script>

    <style type="text/css">
          * {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif;
            font-size: 18px;
            /*font-weight: normal;*/            
        }

        .executeBtn {
            font-weight: bold;    
            
          min-height: 20px; 
        }

        .MainInvoiceStyleDiv {   
            /*zoom:80%;*/
            min-width: 0px;
        }        
     .dx-texteditor-input{
        min-height: 20px; 
     }
        /** {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: normal;
        }*/

        /*.dx-datagrid-text-content {
            font-size: 12px;
            font-weight: normal;
        }*/
        /*.executeBtn {
            font-weight: bold;
        }

        .MainInvoiceStyleDiv {
            zoom: 80%;
            min-width: 0px;
        }*/
    </style>
</head>

<body runat="server">
    <form id="form1" runat="server">
    <div class="MainInvoiceStyleDiv">
        <input type="hidden" id="Operation" />
        <input type="hidden" id="Id" />
        <input type="hidden" id="FromReceiptId" />
        <input type="hidden" id="COAVendors" value="<%=XPRESS.Common.COA.Vendors.ToString()%>" />
        <input type="hidden" id="EntryType" value="1" />
        <input type="hidden" id="IsPermShow" value="<%=(MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID) %>" />
        <input type="hidden" id="SourceTypeId" value="<%=DocumentKindClass.PurchaseReturnInvoice %>" />

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
                        <h5 class="modal-title" id="exampleModalLabel"><%=Resources.Labels.SelectReturnPurchase %></h5>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
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

        <!-- Item Info Modal -->
        <div class="modal fade" id="itemInfoModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="itemInfoModalLabel"><%=Resources.Labels.ItemData %></h5>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body" style="max-height: 380px; overflow: auto;">
                        <div id="itemInfoDiv">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-lg btn-danger" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- customerInfoModal -->
        <div class="modal fade" id="customerInfoModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="customerInfoModalLabel"><%=Resources.Labels.VendorData %></h5>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body" style="max-height: 380px; overflow: auto;">
                        <div id="customerInfoDiv">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="saveNewCustomer" class="btn btn-primary btn-lg"><%=Resources.Labels.Save %></button>
                        <button type="button" class="btn btn-lg btn-danger" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Notify Modal -->
        <div class="modal fade" id="notifyModal" tabindex="-1" role="dialog" aria-labelledby="notifyModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
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

        <div class="container-fluid">
            <div class="dx-viewport">
                <div id="imgStatusDiv" class="notch_label" style="background: url('/images/new-ar.png') no-repeat no-repeat;">
                </div>
                <div class="InvoiceHeader row" style="height: auto;">
                    <asp:Nav runat="server" ID="ucNav" />
                    <%--<asp:Favorit runat="server" ID="ucFavorit" />--%>
                </div>

                <div class="row" id="divBtnAdd" style="margin:5px">
                     <%if (MyContext.PageData.IsAdd)
                        {%>
                        <button type="button" id="btnAdd" class="executeBtn btn bot-buffer btn-sm col-md-1 offset-md-5" onclick="addItem()"><%=Resources.Labels.AddNew %> <i class="fa fa-plus-square" aria-hidden="true"></i></button>
                    <%}
                    %>
                </div>

                <div class="row" style="margin-left: 20px; margin-right: 20px;">
                    <div class="col-md-12">
                        <div id="ToolsPnl" class="row">
                            <div class="row">
                                <div class="col-md-2">
                                    <span><%=Resources.Labels.CreatedBy %></span>:
                                        <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                                    <label runat="server" id="Label1" style="display: none">
                                        <%=Resources.Labels.Currency %></label>
                                    <asp:DropDownList ID="ddlCurrency" Style="display: none" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <label runat="server" id="lblCurrency" style="display: none"><%=Resources.Labels.Currency %></label>
                                </div>

                                <div class="col-md-2">
                                    <span><%=Resources.Labels.ApprovedBy %></span>:
                                        <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </div>

                                <div class="col-md-3">
                                    <asp:LinkButton ID="lnkAccountstatement" runat="server" CssClass="PlusBtn1" Visible="false">[...]</asp:LinkButton>
                                    <asp:LinkButton ID="lnkAddNewCustomer" Style="display: none;" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <label runat="server" id="Label2"><%=Resources.Labels.Branch %></label>
                                    <div id="cmbxBranch" class="dxComb"></div>
                                </div>

                                <div class="col-md-2" id="acStoreDiv">
                                    <label runat="server" id="Label3"><%=Resources.Labels.Store %></label>
                                    <div id="acStore" class="dxComb"></div>
                                </div>

                                <div class="col-md-2" id="costCenterDiv">
                                    <label runat="server" id="Label4"><%=Resources.Labels.CostCenter %></label>
                                    <div id="cmbxCostCenter" class="dxComb"></div>
                                </div>

                                <div class="col-md-2" id="refNoDiv">
                                    <label runat="server" id="Label5"><%=Resources.Labels.UserRefNo %></label>
                                    <input id="txtUserRefNo" type="text" class="form-control" placeholder="<%=Resources.Labels.UserRefNo %>" />
                                </div>

                                <div class="col-md-2">
                                    <label runat="server" id="Label8"><%=Resources.Labels.Vendor %></label>
                                    <div id="acVendor" class="dxComb"></div>
                                </div>

                                <div class="col-md-3" style="display: none">
                                    <label runat="server" id="Label17"><%=Resources.Labels.Address %></label>
                                    <div id="acAddress" class="dxComb"></div>
                                </div>

                                <div class="col-md-3" style="display: none">
                                    <label runat="server" id="Label18"><%=Resources.Labels.ShipAddress %></label>
                                    <div id="acShipAddress" class="dxComb"></div>
                                </div>

                                <div class="col-md-2">
                                    <label runat="server" id="Label24"><%=Resources.Labels.PrintTemplate %></label>
                                    <div id="cmbxTemplate" class="dxComb"></div>
                                </div>

                                <div class="col-md-2" style="display: none">
                                    <label runat="server" id="Label20"><%=Resources.Labels.Ratio %></label>
                                    <input type="text" id="txtRatio" disabled="disabled" />
                                </div>

                                <div class="col-md-2">
                                    <label runat="server" id="Label14"><%=Resources.Labels.Date %></label>
                                    <div id="txtOperationDate" class="dxDatePic"></div>
                                </div>

                                <div class="col-md-2" style="display: none;">
                                    <label runat="server" id="Label21"><%=Resources.Labels.PaymentAddress %></label>
                                    <div id="acPaymentAddress" class="dxComb"></div>
                                </div>

                                <div class="col-md-2" style="display: none;">
                                    <label runat="server" id="Label22"><%=Resources.Labels.Telephone %></label>
                                    <div id="acTelephone" class="dxComb"></div>
                                </div>

                                <div class="col-md-2">
                                    <label runat="server" id="Label15"><%=Resources.Labels.PaymentMethod %></label>
                                    <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-2">
                                    <label runat="server" id="Label23"><%=Resources.Labels.CashAccount %></label>
                                    <div id="acCashAccount" class="dxComb"></div>
                                </div>

                                <div class="col-md-2 discount">
                                    <label runat="server" id="Label6">
                                        <input type="checkbox" id="cbxInvPerDiscount" class="form-check-input" /><%=Resources.Labels.PercentageDiscount %>
                                    </label>
                                    <input type="number" id="txtInvPerDiscount" disabled="disabled" min="0" max="100" step="1" class="form-control inputFont numbersInt" />
                                </div>

                                <div class="col-md-2 discount">
                                    <label runat="server" id="Label7"><%=Resources.Labels.CashDiscount %></label>
                                    <input type="text" id="txtInvCashDiscount" class="form-control inputFont numbersDouble" />
                                </div>
                                <div class="col-md-4">
                                    <label runat="server"></label>
                                    <br />
                                    
                                    <input type="button" id="btnImport" class="form-control btn btn-primary" value="<%=Resources.Labels.ImportFromPurchase %>"
                                        onclick="importFromSales();" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="dxGridParent">
                        <div id="purchaseReturnGridContainer" class="dxGrid col-md-12" <%--style="zoom:125%"--%>></div>
                    </div>
                </div>


                <div id="BtnsRow" class="row" style="margin-right: 2px; padding-left: 15px;">
                   

                    <div class="col-md-8" style="text-align: center">
                        <button type="button" id="btnSave" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="saveItem(false)"><%=Resources.Labels.SaveasDraft %> <i class="fa fa-save" aria-hidden="true"></i></button>

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
 </form>
</body>
</html>