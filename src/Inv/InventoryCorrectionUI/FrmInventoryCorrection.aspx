<%@ Page Title="" Language="C#" 
    AutoEventWireup="true" CodeFile="FrmInventoryCorrection.aspx.cs" Inherits="Inv_InventoryCorrectionUI_FrmInventoryCorrection" %>

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
            if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== true) {
                $("#acStoreDiv").hide();
            }            
            $(".hiddenField").hide();
        }

        $(document).ready(function () {
            $("button").css({ "background-image": "unset" });
            $("#imgStatusDiv").css("background-image", getImgStatus(0));
            loadDevextremeLocalization();
            $("#MainTitle").hide();
            $("#pageTitleLbl").text('<%=this.MyContext.PageData.PageTitle%>');

            loadDivs();

            $("#itemInfoDiv").load('../../Inv/ItemUI/FrmItemDbSelect.aspx');
            $("#customerInfoDiv").load('../../Accounting/ChartOfAccountsUI/FrmAddVendor.aspx');

            $('#saveNewItem').click(function () {
                saveItemInfo();
            });

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
            continueDone();
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
            $.getJSON("../../api/ChartOfAccount/GetChartOfAccountsException", { 'contextKey': '<%=this.GetOppositeAccountContextKey()%>' }, function (Response) {
                $("#acOppositeAccount").dxSelectBox({
                    dataSource: Response,
                    displayExpr: "Name",
                    valueExpr: "ID",
                    searchEnabled: true,
                    disabled: true,
                    placeholder: '<%=Resources.Labels.OppositeAccount%>',
                    onValueChanged: function (data) {

                    }
                });
            });

            $.getJSON("../../api/Structure/GetBranchs", { 'contextKey': null }, function (branchesResponse) {
                $("#cmbxBranch").dxSelectBox({
                    dataSource: branchesResponse,
                    displayExpr: "Name",
                    valueExpr: "ID",
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.Branch%>',
                    onValueChanged: function (data) {
                        $("#BranchIdHidden").val(data);
                        $.getJSON("../../api/Structure/GetStorPermissionsGroup", { 'contextKey': $("#cmbxBranch").dxSelectBox('instance').option('value') + "," + '<%=this.MyContext.UserProfile.UserId%>' }, function (storesResponse) {
                            var selectedStoreId = null;
                            if (<%=MyContext.InventoryCorrectionOptions.KeepStore.ToString().ToLower()%>== true &&
                                <%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== false && storesResponse.length > 0)
                                selectedStoreId = storesResponse[0].ID;
                            $("#acStore").dxSelectBox({
                                dataSource: storesResponse,
                                displayExpr: "Name",
                                valueExpr: "ID",
                                value: selectedStoreId,
                                searchEnabled: true,
                                placeholder: '<%=Resources.Labels.Store%>',
                                visible: '<%=!MyContext.InventoryCorrectionOptions.UseStoreByRecord%>',
                                onValueChanged: function (data) {

                                },
                                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                            }).dxSelectBox("instance");
                        });
                    },
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
                }).dxSelectBox("instance");

                var ekd = ekdArray[<%=this.MyContext.InventoryCorrectionOptions.EnterKeyEventOnTable%>];
                $("#inventoryCorrectionGridContainer").dxDataGrid({
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
                                width: 50,
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
                                dataField: "StoreIdCol",
                                caption: '<%=Resources.Labels.DgStoreHeader%>',
                                allowEditing: false,
                                dataType: "string",
                                lookup: {
                                    dataSource: function getStores(options) {
                                        return {
                                            store: DevExpress.data.AspNet.createStore({
                                                key: "ID",
                                                loadUrl: "../../api/Structure/GetStores",
                                                loadMethod: "get",
                                                loadParams: { 'contextKey': $("#cmbxBranch").dxSelectBox('instance').option('value') },
                                                displayExpr: "Name",
                                                valueExpr: "ID"
                                            })
                                        };
                                    },
                                    allowClearing: true,
                                    displayExpr: "Name",
                                    valueExpr: "ID"
                                },
                                alignment: "center",
                                width: 150,
                                visible: <%=this.MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>,
                            },
                            {
                                dataField: "CategoryNameCol",
                                caption: "<%=Resources.Labels.Category%>",
                                allowEditing: false,
                                dataType: "string",
                                lookup: {
                                    dataSource: function getCategories(options) {
                                        return {
                                            store: DevExpress.data.AspNet.createStore({
                                                key: "ID",
                                                loadUrl: "../../api/ItemCategory/GetItemsCategories",
                                                loadMethod: "get",
                                                displayExpr: "Name",
                                                valueExpr: "ID"
                                            })
                                        };
                                    },
                                    allowClearing: true,
                                    displayExpr: "Name",
                                    valueExpr: "ID"
                                },
                                alignment: "center",
                                width: 130,
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
                                visible: false
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
                                width: 250,
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
                                dataField: "ActualQtyCol",
                                caption: '<%=Resources.Labels.DgQuantityHeader%>',
                                allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 80
                            },
                            {
                                dataField: "QtyInNumberCol",
                                caption: "<%=Resources.Labels.QtyInNumber%>",
                                allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 80
                            },
                            {
                                dataField: "DifferrenceCol",
                                caption: "<%=Resources.Labels.Difference%>",
                                allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 80/*,
                                    calculateCellValue: function (rowData) {
                                        var Differrence=null,ActualQty=0.00,QtyInNumber=0.00;
                                        if (rowData.ActualQtyCol != null)
                                            ActualQty=rowData.ActualQtyCol;
                                        if (rowData.QtyInNumberCol != null) 
                                            QtyInNumber=rowData.QtyInNumberCol;
                                        if(ActualQty!=0.00 && QtyInNumber!=0.00)
                                            Differrence=ActualQty-QtyInNumber;
                                        return Differrence;
                                    }*/
                            },
                            {
                                dataField: "UnitNameCol",
                                caption: '<%=Resources.Labels.DgUnitNameHeader%>',
                                allowEditing: false,
                                dataType: "string",
                                lookup: {
                                    dataSource: function getUnits(options) {
                                        var rows = $("#inventoryCorrectionGridContainer").dxDataGrid("instance").getVisibleRows();
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
                                width: 120
                            },
                            {
                                dataField: "BatchIdCol",
                                caption: "<%=Resources.Labels.DgBatchNoHeader%>",
                                allowEditing: false,
                                dataType: "string",
                                format: "decimal",
                                alignment: "center",
                                width: 100
                            },
                            {
                                dataField: "ProductionDateCol",
                                caption: "<%=Resources.Labels.ProductionDate%>",
                                allowEditing: false,
                                dataType: "date",
                                format: "dd/MM/yyyy",
                                alignment: "center",
                                width: 100
                            },
                            {
                                dataField: "ExpirationDateCol",
                                caption: "<%=Resources.Labels.ExpirationDate%>",
                                allowEditing: false,
                                dataType: "date",
                                format: "dd/MM/yyyy",
                                alignment: "center",
                                width: 100
                            },
                            {
                                dataField: "UnitCostCol",
                                caption: "<%=Resources.Labels.DgUnitCostHeader%>",
                                allowEditing: false,
                                width: 100,
                                dataType: "number",
                                alignment: "center"
                            },
                            {
                                dataField: "TotalCol",
                                caption: "<%=Resources.Labels.Total%>",
                                allowEditing: false,
                                width: 150,
                                dataType: "number",
                                alignment: "center",
                                calculateCellValue: function (rowData) {
                                    return getRowTotal(rowData);
                                }
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
                                        $('#inventoryCorrectionGridContainer').dxDataGrid('instance').deleteRow(e.row.rowIndex);
                                        var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
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
                                    $.getJSON("../../api/Item/FindByBarcode", { contextKey: ",,,true,1", Source: e.value },
                                        function (response) {
                                            if (response != null) {
                                                component.cellValue(rowIndex, "BarcodeCol", response[0].Barcode);
                                                component.cellValue(rowIndex, "ItemIdCol", response[0].ID);
                                                component.cellValue(rowIndex, "ItemNameCol", response[0].Name);
                                                component.cellValue(rowIndex, "UnitIdCol", response[0].UOM_ID);
                                                component.cellValue(rowIndex, "UnitNameCol", response[0].UOMName);
                                                component.cellValue(rowIndex, "UnitCostCol", response[0].Cost);
                                                component.cellValue(rowIndex, "QtyInNumberCol", null);
                                                component.cellValue(rowIndex, "ActualQtyCol", null);
                                                component.cellValue(rowIndex, "DifferrenceCol", null);
                                                refreshItemInfo(rowIndex, component);
                                            }
                                            else {
                                                var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
                                                component.cellValue(rowIndex, "ItemIdCol", null);
                                                component.cellValue(rowIndex, "ItemNameCol", null);
                                                component.cellValue(rowIndex, "UnitIdCol", null);
                                                component.cellValue(rowIndex, "UnitNameCol", null);
                                                component.cellValue(rowIndex, "UnitCostCol", null);
                                                component.cellValue(rowIndex, "QtyInNumberCol", null);
                                                component.cellValue(rowIndex, "ActualQtyCol", null);
                                                component.cellValue(rowIndex, "DifferrenceCol", null);
                                                grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                                            }
                                        });
                                }
                                else {
                                    component.cellValue(rowIndex, "ItemIdCol", null);
                                    component.cellValue(rowIndex, "ItemNameCol", null);
                                    component.cellValue(rowIndex, "UnitIdCol", null);
                                    component.cellValue(rowIndex, "UnitNameCol", null);
                                    component.cellValue(rowIndex, "UnitCostCol", null);
                                    component.cellValue(rowIndex, "QtyInNumberCol", null);
                                    component.cellValue(rowIndex, "ActualQtyCol", null);
                                    component.cellValue(rowIndex, "DifferrenceCol", null);
                                }
                            }
                        }
                        else if (e.dataField == "ItemNameCol") {
                            var onValueChanged = e.editorOptions.onValueChanged;
                            e.editorOptions.onValueChanged = function (e) {
                                onValueChanged.call(this, e);
                                if (e.value != null && e.value != '') {
                                    $.getJSON("../../api/Item/FindByName", { contextKey: ",,,true,1", Source: e.value },
                                        function (response) {
                                            if (response != null) {
                                                component.cellValue(rowIndex, "BarcodeCol", response[0].Barcode);
                                                component.cellValue(rowIndex, "ItemIdCol", response[0].ID);
                                                component.cellValue(rowIndex, "ItemNameCol", response[0].Name);
                                                component.cellValue(rowIndex, "UnitIdCol", response[0].UOM_ID);
                                                component.cellValue(rowIndex, "UnitNameCol", response[0].UOMName);
                                                component.cellValue(rowIndex, "UnitCostCol", response[0].Cost);
                                                component.cellValue(rowIndex, "QtyInNumberCol", null);
                                                component.cellValue(rowIndex, "ActualQtyCol", null);
                                                component.cellValue(rowIndex, "DifferrenceCol", null);
                                                refreshItemInfo(rowIndex, component);
                                            }
                                            else {
                                                var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
                                                component.cellValue(rowIndex, "ItemIdCol", null);
                                                component.cellValue(rowIndex, "ItemNameCol", null);
                                                component.cellValue(rowIndex, "UnitIdCol", null);
                                                component.cellValue(rowIndex, "UnitNameCol", null);
                                                component.cellValue(rowIndex, "UnitCostCol", null);
                                                component.cellValue(rowIndex, "QtyInNumberCol", null);
                                                component.cellValue(rowIndex, "ActualQtyCol", null);
                                                component.cellValue(rowIndex, "DifferrenceCol", null);
                                                grid.focus(grid.getCellElement(rowIndex, "ItemNameCol"));
                                            }
                                        });
                                }
                                else {
                                    component.cellValue(rowIndex, "ItemIdCol", null);
                                    component.cellValue(rowIndex, "ItemNameCol", null);
                                    component.cellValue(rowIndex, "UnitIdCol", null);
                                    component.cellValue(rowIndex, "UnitNameCol", null);
                                    component.cellValue(rowIndex, "UnitCostCol", null);
                                    component.cellValue(rowIndex, "QtyInNumberCol", null);
                                    component.cellValue(rowIndex, "ActualQtyCol", null);
                                    component.cellValue(rowIndex, "DifferrenceCol", null);
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
                    //height: 400,
                    height: function () {
                        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                            return 400;
                        }
                        else {
                            var toolsPnl = $("#ToolsPnl").height();
                            var btnRow = $("#BtnsRow").height();
                            return window.innerHeight - ((toolsPnl + btnRow) - 30);
                        }
                    },
                    sorting: { mode: "none" },
                    customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                    onKeyDown: function (e) {
                        var keyCode = e.event.which;
                        var component = e.component;
                        var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
                        if (keyCode == 13) {
                            grid.saveEditData();
                            var items = grid.getDataSource().store()._array;
                            if (currentRowIndex == items.length - 1 && ValidateRows(false, false)) {
                                AddRowBottom(true);
                            }
                        } else if (keyCode == 46 && currentRowIndex >= 0) {
                            const focusedCellPosition = getCurrentCell(grid);
                            grid.deleteRow(focusedCellPosition.rowIndex);
                        }
                    },
                    summary: {
                        totalItems: [/*{
                        column: "ItemNameCol",
                            displayFormat: "الإجمالي"
                        },*/
                            {
                                column: "TotalCol",
                                summaryType: "sum",
                                displayFormat: "<%=Resources.Labels.Total%> {0}",
                                valueFormat: "decimal",
                                name: "TotalSum"
                            }],
                        recalculateWhileEditing: true,
                    },
                    onCellClick: function (e) {
                        var operation = $("#Operation").val();
                        if ((operation == "Add" || operation == "Edit"))
                            SetDataGridEditable(true);                        
                        else SetDataGridEditable(false);
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
                         
                    }
                });

                clearDgv();                
            });
            /********************************************/
            $("#acStore").dxSelectBox({
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Store%>',
                visible: '<%=!MyContext.InventoryCorrectionOptions.UseStoreByRecord%>',
                onValueChanged: function (data) {
                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
            }).dxSelectBox("instance");
            /********************************************/
            $("#acAddress").dxSelectBox({
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Address%>',
                onValueChanged: function (data) {

                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>
            }).dxSelectBox("instance");
            /*******************************************/
            $("#acShipAddress").dxSelectBox({
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.ShipAddress%>',
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
            $("#cmbxTemplate").dxSelectBox({
                dataSource: DevExpress.data.AspNet.createStore({
                    key: "Id",
                    loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                    loadMethod: "get",
                    loadParams: { 'KindId': '<%=DocumentKindClass.InventoryCorrection%>', 'EntryType': null},
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

            /*******************************************/
            $.getJSON("../../api/PrintTemplate/GetDefaultTemplateByKindId", { KindId: '<%=DocumentKindClass.InventoryCorrection%>', 'EntryType': null },
                function (response) {
                    if (response.length > 0) {
                        $("#cmbxTemplate").dxSelectBox({
                            dataSource: DevExpress.data.AspNet.createStore({
                                key: "Id",
                                loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                loadMethod: "get",
                                loadParams: { 'KindId': '<%=DocumentKindClass.InventoryCorrection%>', 'EntryType': null },
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
                        $("#cmbxTemplate").dxSelectBox({
                            dataSource: DevExpress.data.AspNet.createStore({
                                key: "Id",
                                loadUrl: "../../api/PrintTemplate/GetPrintTemplatesByKindId",
                                loadMethod: "get",
                                loadParams: { 'KindId': '<%=DocumentKindClass.InventoryCorrection%>', 'EntryType': null },
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
            /******************************************/
        }

        function continueDone() {
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnAdd", true);
            setEnabled("#btnSearch", true);
            setEnabled("#btnPrint", true);
            setEnabled("#txtOperationDate", true);
            
                setReadOnly("#ToolsPnl", true);
                $.getJSON('../../api/GridOrdering/FindGridOrdering', { 'GridId': '<%=DocumentKindClass.InventoryCorrection%>', 'UserId': '<%=MyContext.UserProfile.Contact_ID%>' }, function (response) {
                        if (response.length == 1) {
                            var array = JSON.parse(response[0].JsonVal);
                            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
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
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
            var rows = $("#inventoryCorrectionGridContainer").dxDataGrid("instance").getVisibleRows();
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

        /**************************** استرجاع الصافي قبل الضريبة = ((السعر-الضريبة)*العدد)-الخصم ********************/
        function getRowTotal(rowData) {              
            if (rowData.UnitCostCol != null && rowData.DifferrenceCol != null) {                
                var ItemTotalCostEvaluate = 0.00;
                ItemTotalCostEvaluate = rowData.UnitCostCol * Math.abs(rowData.DifferrenceCol);
                ItemTotalCostEvaluate = Math.round((ItemTotalCostEvaluate + Number.EPSILON) * 100,<%=MyContext.NumberDecimal%>) / 100;                
                if (ItemTotalCostEvaluate != 0.00)                                    
                {
                    //grid.cellValue(rowIndex, "TotalCol",ItemTotalCostEvaluate)
                    return ItemTotalCostEvaluate;                
                }
                else return null;                
            }
            else
            {                
                return null;
            }
        }       

        function refreshItemInfo(rowIndex,component){
            var ItemId=null,StoreId=null,UnitId=null,BatchId=null;
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
            var rows = $("#inventoryCorrectionGridContainer").dxDataGrid("instance").getVisibleRows();
            if (rows != null && rows.length > 0 && rowIndex > -1) {
                if(rows[rowIndex].data["ItemIdCol"] != null)
                    ItemId = rows[rowIndex].data["ItemIdCol"];
                if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== true)
                    StoreId = rows[rowIndex].data["StoreIdCol"];
                else StoreId = $("#acStore").dxSelectBox('instance').option('value');

                if(rows[rowIndex].data["UnitIdCol"] != null)
                    UnitId=rows[rowIndex].data["UnitIdCol"];
                if(rows[rowIndex].data["BatchIdCol"] != null)
                    BatchId=rows[rowIndex].data["BatchIdCol"];
            }
            $.post('../../api/Item/FindItemQty', {'ItemId':ItemId, 'StoreId': StoreId, 'BatchId': BatchId, 'UnitId':UnitId},function (response) {                
                if(response!=-1)
                {                    
                    if(component!=null)
                        component.cellValue(rowIndex, "ActualQtyCol", response);
                }
                else {                    
                    if(component!=null)
                        component.cellValue(rowIndex, "ActualQtyCol", null);
                }
            });
        }

        function ValidateRows(skipEmptyRows, toSave) {
            return true;
            var UseStoreByRecord = $("#UseStoreByRecord").prop("checked");
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
            var rows = grid.getDataSource().store()._array;
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1,
                operation = $("#Operation").val(), documentId = $("#Id").val(),
                docDate = $("#DocDate").dxDateBox("instance").option('text').trim();
            var TransactionDict = new Object();
            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&                        
                        (rows[i].QtyInNumberCol == null || rows[i].QtyInNumberCol == '') &&
                        (rows[i].ActualQtyCol == null || rows[i].ActualQtyCol == '') &&
                        (rows[i].DifferrenceCol == null || rows[i].DifferrenceCol == '') &&
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostCol == null || rows[i].UnitCostCol == '') &&
                        (grid.cellValue(i, "TotalCol") == null || grid.cellValue(i, "TotalCol") == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '')                    
                    )
                        isEmptyRow = true;
                }

                if (rows[i].rowType == "data" && (skipEmptyRows == false || isEmptyRow == false)) {
                    if (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocItemRequired%>";
                        cellName = "ItemNameCol";
                    }
                    else if (rows[i].QtyInNumberCol == null || rows[i].QtyInNumberCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocQuanityRequired%>";
                        cellName = "QtyInNumberCol";
                    }
                    else if (rows[i].ActualQtyCol == null || rows[i].ActualQtyCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgActualQtyRequired%>";
                        cellName = "ActualQtyCol";
                    }
                    else if (rows[i].DifferrenceCol == null || rows[i].DifferrenceCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDifferrenceRequired%>";
                        cellName = "DifferrenceCol";
                    }
                    else if (rows[i].UnitCostCol == null || rows[i].UnitCostCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitCostRequired%>";
                        cellName = "UnitCostCol";
                    }
                    else if (UseStoreByRecord == true && (rows[i].StoreIdCol == null || rows[i].StoreIdCol == '')) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocStoreRequired%>";
                        cellName = "StoreIdCol";
                    }
                    else if (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitRequired%>";
                        cellName = "UnitNameCol";
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
                var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
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
    
        function clearTools() {
            $("#Id").val(null);
            $("#Operation").val(null);

            clearDgv();

            if (<%=MyContext.InventoryCorrectionOptions.KeepStore.ToString().ToLower()%>!= true)
                $("#acStore").dxSelectBox('instance').option({ value: null });

            $("#txtSerialSearch").val(null);
            setReadOnly("#ToolsPnl", true);
        }

        function SetDataGridEditable(value) {
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");            
            var columns = $("#inventoryCorrectionGridContainer").dxDataGrid("instance").getVisibleColumns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].allowEditing = value;
                if (columns[i].name == "RowNoCol")
                    columns[i].allowEditing = false;
            }        
        }

        function addItem() {            
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);

            $("#Operation").val("Add");
            SetDataGridEditable(true);            
        }

        function findItem() {
            $("#searchDiv").load("../../Inv/InventoryCorrectionUI/FrmInventoryCorrectionSelect.aspx?requestCode=1");
            showModal('#searchModal');
        }

        function documentSelected(requestCode, selectedDocumentId) {
            if (requestCode == 1) {
                clearTools();
                hideModal('#searchModal');
                $.getJSON("../../api/InventoryCorrection/FindDocumentById", { InventoryDocumentID: selectedDocumentId },
                    function (response) {
                        if (response.length == 1) {                            
                            if (response[0].DocStatus_ID == 1)
                                setEnabled("#btnEdit", true);
                            else setEnabled("#btnEdit", false);
                            setEnabled("#btnApprove", true);
                            setEnabled("#btnReset", true);
                            setEnabled("#btnAdd", false);
                            $("#cmbxBranch").dxSelectBox('instance').option({ value: response[0].Branch_ID });                                                        
                            $("#txtUserRefNo").val(response[0].UserRefNo);
                            $("#txtSerialSearch").val(response[0].Serial);

                           
                                $("#txtOperationDate").dxDateBox("instance").option({ value: new Date(response[0].OperationDate) });
                           

                            if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== false) {
                                
                                    $("#acStore").dxSelectBox('instance').option({ value: response.StoreId });
                                 
                            }

                            $("#acAddress").dxSelectBox('instance').option({ value: response[0].DefaultAddress_ID });
                            $("#acShipAddress").dxSelectBox('instance').option({ value: response[0].ShipToAddress_ID });
                            $("#acOppositeAccount").dxSelectBox('instance').option({ value: response[0].OppositeAccount_ID });
                            $("#imgStatusDiv").css("background-image", getImgStatus(response[0].DocStatus_ID));
                            $("#Id").val(selectedDocumentId);
                            setEnabled("#btnPrint", true);
                            $.getJSON("../../api/InventoryCorrection/GetDocumentItems", { InventoryDocumentID: selectedDocumentId, BranchId: response[0].Branch_ID },
                                function (responseTable) {
                                    if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== false) {
                                       
                                            $("#acStore").dxSelectBox('instance').option({ value: responseTable[0].StoreIdCol });
                                   
                                    }

                                    var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
                                    for (var i = 0; i < responseTable.length; i++) {
                                        responseTable[i].KeyCol = responseTable[i].RowNoCol.toString();
                                        rowIdentifier = responseTable[i].RowNoCol                                        
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
                                    //grid.refresh();                                    
                                });
                        }
                    });
            }
            else if (requestCode == 2) {
                hideModal('#searchModal');
                var printTemplateId = $("#cmbxTemplate").dxSelectBox('instance').option('value');
                window.open('../../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&InvoiceID=' + selectedDocumentId + "&DocKindId=" + '<%=DocumentKindClass.InventoryCorrection%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
            }
        }

        function showOperationDetials(SourceDocumentId) {
            $("#operationDetailsDiv").load("../../OperationDetails/FrmOperationDetials.aspx?SourceDocId=" + SourceDocumentId + "&SourceTableId=" +<%=DocumentKindClass.InventoryCorrection%>);
            showModal('#operationDetailsModal');
        }

        function editItem() {
            setEnabled(".executeBtn", false);
            setEnabled(".divBtnAdd", false);
            setEnabled("#btnSave", true);
            setEnabled("#btnApprove", true);
            setEnabled("#btnReset", true);
            setReadOnly("#ToolsPnl", false);
            $("#Operation").val("Edit");
            SetDataGridEditable(true);
        }

        function saveItem(isApproving) {
            var dateBox = $("#txtOperationDate").dxDateBox("instance");
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
            grid.saveEditData();

            if (dateBox.option('text') == null || dateBox.option('text') == '') {
                showErrorMessage("<%=Resources.Messages.MsgDocOperationDateRequired%>", "#txtOperationDate");
                return false;
            }
            else if ($("#cmbxBranch").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocBranchRequired%>", "#cmbxBranch");
                return false;
            }
            else if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== false && $("#acStore").dxSelectBox('instance').option('value') == null) {
                showErrorMessage("<%=Resources.Messages.MsgDocStoreRequired%>", "#txtOperationDate");
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

                var branchId = $("#cmbxBranch").dxSelectBox('instance').option('value'), storeId = null,
                    userRefNo = $("#txtUserRefNo").val(),
                    docDate = dateBox.option('text').trim(),
                    oppsiteAccount = $("#acOppositeAccount").dxSelectBox('instance').option('value');
                var totalSum = grid.getTotalSummaryValue('TotalSum');
                var totalCostEvaluateSum = grid.getTotalSummaryValue('TotalSum');

                var dataRows = [];
                var rows = grid.getDataSource().store()._array;
                if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== false)
                    storeId = $("#acStore").dxSelectBox('instance').option('value');

                for (i = 0; i < rows.length; i++) {
                    var isEmptyRow = false;
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].ActualQtyCol == null || rows[i].ActualQtyCol == '') &&
                        (rows[i].QtyInNumberCol == null || rows[i].QtyInNumberCol == '') &&
                        (rows[i].DifferrenceCol == null || rows[i].DifferrenceCol == '') &&
                        (rows[i].UnitNameCol == null || rows[i].UnitNameCol == '') &&
                        (rows[i].UnitCostCol == null || rows[i].UnitCostCol == '') &&
                        (grid.cellValue(i, "TotalCol") == null || grid.cellValue(i, "TotalCol") == '') &&
                        (rows[i].ItemIdCol == null || rows[i].ItemIdCol == '') &&
                        (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '')
                    )
                        isEmptyRow = true;

                    if (/*rows[i].rowType == "data" &&*/ isEmptyRow == false) {
                        var idCol = null, storeIdCol = storeId, unitCostCol = null, totalCol = null, itemIdCol = null,
                            unitIdCol = null, actualQtyCol = null, qtyInNumberCol = null, differrenceCol = null;

                        if (rows[i].IdCol != null && rows[i].IdCol != '')
                            idCol = rows[i].IdCol;

                        if (<%=MyContext.InventoryCorrectionOptions.UseStoreByRecord.ToString().ToLower()%>== true && rows[i].StoreIdCol != null && rows[i].StoreIdCol != '')
                            storeIdCol = rows[i].StoreIdCol;
                        
                        if (rows[i].ActualQtyCol != null && rows[i].ActualQtyCol != '')
                            actualQtyCol = rows[i].ActualQtyCol;
                        if (rows[i].QtyInNumberCol != null && rows[i].QtyInNumberCol != '')
                            qtyInNumberCol = rows[i].QtyInNumberCol;
                        if (rows[i].DifferrenceCol != null && rows[i].DifferrenceCol != '')
                            differrenceCol = rows[i].DifferrenceCol;
                        if (rows[i].UnitCostCol != null && rows[i].UnitCostCol != '')
                            unitCostCol = rows[i].UnitCostCol;
                        if (grid.cellValue(i, "TotalCol") != null && grid.cellValue(i, "TotalCol") != '')
                            totalCol = grid.cellValue(i, "TotalCol");
                        itemIdCol = rows[i].ItemIdCol;
                        unitIdCol = rows[i].UnitIdCol;
                        var row = {
                            'ID': idCol, 'Quantity': actualQtyCol, 'ActualQty': actualQtyCol, 'QtyInNumber': qtyInNumberCol, 'Differrence': differrenceCol,
                            'UnitCost': unitCostCol, 'Total': totalCol, 'Item_ID': itemIdCol, 'Uom_ID': unitIdCol, 'Store_ID': storeIdCol
                        };
                        dataRows.push(row);
                        //dataRows.push(rows[i].data);
                    }
                }

                dataRows = JSON.stringify(dataRows);                
                var id = $("#Id").val();
                $.post("../../api/InventoryCorrection/SaveInventoryCorrection", {
                    'Id': id, 'BranchId': branchId, 'UserRefNo': userRefNo, 'OperationDate': docDate,
                    'acOppositeAccount': oppsiteAccount, 'Total': totalSum, 'GrossTotal': totalCostEvaluateSum,
                    'EditMode': editMode, 'UserProfileContact_ID': '<%=MyContext.UserProfile.Contact_ID%>',
                    'IsApproving': isApproving, 'StoreId': storeId, 'Source': dataRows
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
                                window.open('../../PrintTemplate/PrintDocumentHtml.aspx?PrintTemplateId=' + printTemplateId + '&InvoiceID=' + response.Code + "&DocKindId=" + '<%=DocumentKindClass.InventoryCorrection%>', "PopupWindow", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                            });

                            //------------------------ btnSendEmail ---------------------------//
                            $("#btnSendEmail").click(function () {
                                $.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateEmail(queryResponse) == true) {
                                        $.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>', "SendType": 1 }, function (sendResponse) {
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
                                                $.post('../../api/General/SendEmail', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>', "SendType": 2, "Data": $('#txtEmailTo').val() }, function (sendResponse) {
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
                                $.post('../../api/General/GetContactDetialData', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>' }, function (queryResponse) {
                                    if (queryResponse != "" && queryResponse.length != 0 && validateMobile(queryResponse) == true) {
                                        $.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>', "SendType": 1 }, function (sendResponse) {
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
                                                $.post('../../api/General/SendSms', { "DocumentId": response.Code, "KindId": '<%=DocumentKindClass.InventoryCorrection%>', "SendType": 2, "Data": $('#txtMobileTo').val() }, function (sendResponse) {
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
                $("#searchDiv").load("../../Inv/InventoryCorrectionUI/FrmInventoryCorrectionSelect.aspx?requestCode=2");
                showModal('#searchModal');
            }
        }

        function saveGridOrdering() {
            var grid = $("#inventoryCorrectionGridContainer").dxDataGrid("instance");
            var colCount = grid.columnCount();
            var columnIndicies = [];
            for (var i = 0; i < colCount; i++) {
                var visibleIndex = grid.columnOption(i, "visibleIndex");
                if (visibleIndex >= 0)
                    columnIndicies.push({ index: visibleIndex, fieldName: grid.columnOption(i, "dataField") });
            }

            $.post("../../api/GridOrdering/SaveGridOrdering", {
                'GridId': '<%=DocumentKindClass.InventoryCorrection%>',
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
            font-size: 18px;
             
        }

        /*.dx-datagrid-text-content {
            font-size: 12px;
            font-weight: normal;
        }*/
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
    </style>
    </head>
<body runat="server">
    <form id="form1" runat="server">
    <div class="MainInvoiceStyleDiv">
        <input type="hidden" id="Operation" />
        <input type="hidden" id="Id" />        
        <input type="hidden" id="COAVendors" value="<%=XPRESS.Common.COA.Vendors.ToString()%>" />        
        <input type="hidden" id="EntryType" value="1" />        
        <input type="hidden" id="SourceTypeId" value="<%=DocumentKindClass.InventoryCorrection %>" />

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
                        <h5 class="modal-title" id="exampleModalLabel"><%=Resources.Labels.SelectInventoryCorrection %></h5>
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
            <!-- حقول رأس الفاتور -->
            <div class="row">
                <div class="col-lg-4 col-md-4 col-sm-4">
                </div>
                <div class="col-md-4">
                </div>
            </div>

            <div class="dx-viewport">
                <div id="imgStatusDiv" class="notch_label" style="background: url('/images/new-ar.png') no-repeat no-repeat;">

                </div>
                <div class="InvoiceHeader row" style="height:auto;">
                    <asp:Nav runat="server" ID="ucNav" />                   
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
                            <div class="col-md-2">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span><%=Resources.Labels.CreatedBy %></span>:
                                        <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        <label runat="server" id="Label1" style="display: none">
                                            <%=Resources.Labels.Currency %></label>
                                        <asp:DropDownList ID="ddlCurrency" Style="display: none" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <label runat="server" id="lblCurrency" style="display: none"><%=Resources.Labels.Currency %></label>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label2"><%=Resources.Labels.Branch %></label>
                                        <div id="cmbxBranch" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2" id="acStoreDiv">
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label4"><%=Resources.Labels.Store %></label>
                                        <div id="acStore" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span><%=Resources.Labels.ApprovedBy %></span>:
                                        <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                                        <label runat="server" id="Label5"><%=Resources.Labels.UserRefNo %></label>
                                        <input id="txtUserRefNo" type="text" class="form-control" placeholder="<%=Resources.Labels.UserRefNo %>" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label3"><%=Resources.Labels.OppositeAccount %></label>
                                        <div id="acOppositeAccount" class="dxComb"></div>
                                    </div>
                                </div>

                                <div class="row" style="display: none">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label17"><%=Resources.Labels.Address %></label>
                                        <div id="acAddress" class="dxComb"></div>
                                    </div>
                                </div>

                                <div class="row" style="display: none">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label18"><%=Resources.Labels.ShipAddress %></label>
                                        <div id="acShipAddress" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <br />

                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label24"><%=Resources.Labels.PrintTemplate %></label>
                                        <div id="cmbxTemplate" class="dxComb"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <label runat="server" id="Label14"><%=Resources.Labels.Date %></label>
                                        <div id="txtOperationDate" class="dxDatePic"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="dxGridParent">
                        <div id="inventoryCorrectionGridContainer" class="dxGrid col-md-12"></div>
                    </div>
                </div>

                <div id="BtnsRow" class="row" style="margin-right: 2px; padding-left: 15px;">     
                    
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

                    <button type="button" id="btnReset" class="executeBtn btn bot-buffer btn-sm col-md-1" onclick="resetItem()"><%=Resources.Labels.Clear %><i class="fa fa-undo" aria-hidden="true"></i></button>
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
