<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmItemDbSelect.aspx.cs" Inherits="Inv_ItemUI_FrmItemDbSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/SearchPageStyle.css" rel="stylesheet" />

    <script type="text/javascript">
        var $ = jQuery.noConflict();
        function ValidateRows(skipEmptyRows, toSave) {
            return true;
            var UseStoreByRecord = <%=MyContext.SalesOptions.UseStoreByRecord.ToString().ToLower()%>;
            var grid = $("#pricesGridContainer").dxDataGrid("instance");
            var rows = grid.getDataSource().store()._array;
            var hasError = false, errorMessage = null, cellName = null, rowIndex = -1,
                operation = $("#ItemOperation").val(), documentId = $("#ItemId").val(),
                docDate = $("#txtOperationDate").dxDateBox("instance").option('text').trim();
            var TransactionDict = new Object();
            for (i = 0; i < rows.length; i++) {
                var isEmptyRow = false;
                if (skipEmptyRows == true) {
                    if ((rows[i].BarcodeCol == null || rows[i].BarcodeCol == '') &&
                        (rows[i].ItemNameCol == null || rows[i].ItemNameCol == '') &&
                        (rows[i].AmountCol == null || rows[i].AmountCol == '')
                        //(rows[i].BounusCol == null || rows[i].BounusCol == '')
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
                        <%=MyContext.SalesOptions.AllowZeroInSale.ToString().ToLower()%>!= true) {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocZeroQuanityRejected%>";
                        cellName = "AmountCol";
                    }
                    else if (rows[i].UnitCostEvaluateCol == null || rows[i].UnitCostEvaluateCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitCostRequired%>";
                        cellName = "UnitCostEvaluateCol";
                    }
                    else if (rows[i].UnitIdCol == null || rows[i].UnitIdCol == '') {
                        hasError = true;
                        errorMessage = "<%=Resources.Messages.MsgDocUnitRequired%>";
                        cellName = "UnitNameCol";
                    }
                    else if ( <%=MyContext.SalesOptions.UseStoreByRecord.ToString().ToLower()%> == true &&
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

        function AddPricesRowBottom(focusRow) {
            if (ValidateRows(false, false) == true) {

                var grid = $("#pricesGridContainer").dxDataGrid("instance");
                var cols = $("#pricesGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();
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

                rows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["Id"] = dataRows[i].Id;
                    rows[i].data["PriceTypeId"] = dataRows[i].PriceTypeId;
                    rows[i].data["Price"] = dataRows[i].Price;
                    rows[i].data["UnitId"] = dataRows[i].UnitId;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "BarcodeCol"));
                //SetDataGridEditable(true);
            }
        }

        function AddUnitsRowBottom(focusRow) {
            if (ValidateRows(false, false) == true) {

                var grid = $("#unitsGridContainer").dxDataGrid("instance");
                var cols = $("#unitsGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();
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

                rows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["Id"] = dataRows[i].Id;
                    rows[i].data["PriceTypeId"] = dataRows[i].PriceTypeId;
                    rows[i].data["Price"] = dataRows[i].Price;
                    rows[i].data["UnitId"] = dataRows[i].UnitId;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "BarcodeCol"));
                //SetDataGridEditable(true);
            }
        }

        function AddRawMatRowBottom(focusRow) {
            if (ValidateRows(false, false) == true) {

                var grid = $("#rawMatGridContainer").dxDataGrid("instance");
                var cols = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleColumns();
                var rows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();
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

                rows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();

                for (var i = 0; i < dataRows.length; i++) {
                    rows[i].data["Id"] = dataRows[i].Id;
                    rows[i].data["PriceTypeId"] = dataRows[i].PriceTypeId;
                    rows[i].data["Price"] = dataRows[i].Price;
                    rows[i].data["UnitId"] = dataRows[i].UnitId;
                }

                //grid.refresh();
                if (focusRow)
                    grid.focus(grid.getCellElement(dataRows.length, "BarcodeCol"));
                //SetDataGridEditable(true);
            }
        }
    </script>
    <script type="text/javascript">
        var defaultUnitId = null, defaultCategoryId = null;

        $(document).ready(function () {
            loadDevextremeLocalization();
            /*******************************************/
            $.getJSON("../../api/ItemCategory/GetItemsCategories", function (response) {
                var allCategories = [];
                for (i = 0; i < response.length; i++) {
                    allCategories.push(response[i]);
                }
                if (response.length > 0)
                    defaultCategoryId = response[0].ID;
                $("#acParentCategory").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: {
                            type: "array",
                            key: "ID",
                            data: allCategories,
                        },
                        key: 'ID',
                        paginate: true,
                        pageSize: 10
                    }),
                    displayExpr: "Name",
                    valueExpr: "ID",
                    value: defaultCategoryId,
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.ParentCategory%>',
                    onValueChanged: function (data) {
                    },
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                }).dxSelectBox("instance");
            });
            /*******************************************/
            $.post("../../api/ItemUnit/GetGeneralAtt", { 'ContextKey':'<%=this.GeneralAttributesUOM()%>' },
                function (response) {
                    var allUnits = [];
                    for (i = 0; i < response.length; i++) {
                        allUnits.push(response[i]);
                    }
                    if (response.length > 0) {
                        defaultUnitId = response[0].ID;
                    }

                    $("#acSmallestUnit").dxSelectBox({
                        dataSource: new DevExpress.data.DataSource({
                            store: {
                                type: "array",
                                key: "ID",
                                data: allUnits,
                            },
                            key: 'ID',
                            paginate: true,
                            pageSize: 10
                        }),
                        displayExpr: "Name",
                        valueExpr: "ID",
                        value: defaultUnitId,
                        searchEnabled: true,
                        placeholder: '<%=Resources.Labels.SmallestUnit%>',
                        onValueChanged: function (data) {
                        },
                        rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                    }).dxSelectBox("instance");
                });
            /******************************************/
            $("#acTaxItemeCard").dxSelectBox({
                dataSource: DevExpress.data.AspNet.createStore({
                    key: "ID",
                    loadUrl: "../../api/Tax/GetTaxes",
                    loadMethod: "get",
                    loadParams: { 'contextKey': '' },
                    displayExpr: "Name",
                    valueExpr: "ID"
                }),
                displayExpr: "Name",
                valueExpr: "ID",
                searchEnabled: true,
                placeholder: '<%=Resources.Labels.Tax%>',
                onValueChanged: function (data) {
                },
                rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
            }).dxSelectBox("instance");
            /*******************************************/
            $.getJSON("../../api/ChartOfAccount/GetChartOfAccountsCheledronly"<%--, { 'contextKey': '<%=this.GetAccountRelatedContext()%>'}--%>, function (exResponse) {
                let allMyItems = [];
                for (i = 0; i < exResponse.length; i++) {
                    allMyItems.push(exResponse[i]);
                }

                $("#acAccountRelated").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: {
                            type: "array",
                            key: "ID",
                            data: allMyItems,
                        },
                        key: 'ID',
                        paginate: true,
                        pageSize: 50
                    }),
                    ////////////////
                    displayExpr: "Name",
                    valueExpr: "ID",
                    value: null,
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.AccountName%>',
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                }).dxSelectBox("instance");
            });
            /*******************************************/
            $.getJSON("../../api/General/GetPrintersName", function (exResponse) {
                let allMyItems = [];
                for (i = 0; i < exResponse.length; i++) {
                    allMyItems.push(exResponse[i]);
                }

                $("#acPrinterName").dxSelectBox({
                    dataSource: new DevExpress.data.DataSource({
                        store: {
                            type: "array",
                            key: "Id",
                            data: allMyItems,
                        },
                        key: 'Id',
                        paginate: true,
                        pageSize: 50
                    }),
                    ////////////////
                    displayExpr: "PrinterName",
                    valueExpr: "Id",
                    value: null,
                    searchEnabled: true,
                    placeholder: '<%=Resources.Labels.PrinterName%>',
                    rtlEnabled:<%=(this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic).ToString().ToLower()%>,
                }).dxSelectBox("instance");
            });
            /*******************************************/
            setTimeout(function () {
                refreshItemList();
                /*******************************************/
                $("#pricesGridContainer").dxDataGrid({
                    columnFixing: {
                        enabled: true
                    },
                    //repaintChangesOnly: true,
                    columnAutoWidth: true,
                    showBorders: true,
                    showRowLines: true,
                    dataSource: DevExpress.data.AspNet.createStore({
                        key: "Id",
                        loadUrl: "../../api/Item/LoadItemPrice",
                        loadMethod: "get"
                    }),
                    allowColumnReordering: true,
                    columns:
                        [
                            {
                                caption: "#",
                                allowEditing: false,
                                width: 30
                            },
                            {
                                dataField: "PriceTypeId",
                                caption: '<%=Resources.Labels.PriceType%>',
                                dataType: "string",
                                lookup: {
                                    dataSource: DevExpress.data.AspNet.createStore({
                                        key: "ID",
                                        loadUrl: "../../api/General/GetGeneralAtt",
                                        loadMethod: "get",
                                        loadParams: { 'contextKey': '<%=this.GetPriceNameContextKey()%>' },
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        pageSize: 5,
                                        paginate: true
                                    }),
                                    allowClearing: true,
                                    valueExpr: "ID",
                                    displayExpr: "Name"
                                },
                                alignment: "center",
                                width: 200
                            },
                            {
                                dataField: "Price",
                                caption: '<%=Resources.Labels.Price%>',
                                //allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "UnitId",
                                caption: '<%=Resources.Labels.Unit%>',
                                dataType: "string",
                                lookup: {
                                    dataSource: DevExpress.data.AspNet.createStore({
                                        key: "ID",
                                        loadUrl: "../../api/General/GetGeneralAtt",
                                        loadMethod: "get",
                                        loadParams: { 'contextKey': '<%=this.GetUOMContextKey()%>' },
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        pageSize: 5,
                                        paginate: true
                                    }),
                                    allowClearing: true,
                                    valueExpr: "ID",
                                    displayExpr: "Name"
                                },
                                //allowEditing: false,
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Id",
                                caption: "رقم الحركة",
                                allowEditing: false,
                                width: 150,
                                alignment: "center",
                                visible: false
                            }
                        ],
                    remoteOperations: false,
                    keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: "row", editOnKeyPress: true },
                    onEditorPreparing: function OnEditorPreparing(e) {
                        var component = e.component,
                            rowIndex = e.row && e.row.rowIndex;
                    },
                    onToolbarPreparing: function (e) {
                        e.toolbarOptions.visible = false;
                    },
                    editing: {
                        mode: "batch",
                        allowAdding: true,
                        allowDeleting: true,
                        allowUpdating: true,
                        selectTextOnEditStart: true,
                        startEditAction: "click",
                        useIcons: true
                    },
                    height: 150,
                    sorting: { mode: "none" },
                    customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                    rtlEnabled: true,
                    focusedRowEnabled: true,
                    onFocusedRowChanging: function OnFocusedRowChanging(e) {
                        currentRowIndex = e.newRowIndex;
                    }/*,
                selection: { mode: "single" },
                onSelectionChanged: function (e) {
                    e.component.byKey(e.currentSelectedRowKeys[0]).done(employee => {
                        if (employee) {
                            $("#selected-employee").text(`Selected employee: ${employee.FullName}`);
                        }
                    });
                },*/
                });
                clearItemPriceDgv(3);

                $("#unitsGridContainer").dxDataGrid({
                    columnFixing: {
                        enabled: true
                    },
                    //repaintChangesOnly: true,
                    columnAutoWidth: true,
                    showBorders: true,
                    showRowLines: true,
                    dataSource: DevExpress.data.AspNet.createStore({
                        key: "Id",
                        loadUrl: "../../api/Item/LoadIUnit",
                        loadMethod: "get"
                    }),
                    allowColumnReordering: true,
                    columns:
                        [
                            {
                                caption: "#",
                                allowEditing: false,
                                width: 30
                            },
                            {
                                dataField: "UnitId",
                                caption: '<%=Resources.Labels.Unit%>',
                                dataType: "string",
                                lookup: {
                                    dataSource: DevExpress.data.AspNet.createStore({
                                        key: "ID",
                                        loadUrl: "../../api/General/GetGeneralAtt",
                                        loadMethod: "get",
                                        loadParams: { 'contextKey': '<%=this.GetUOMContextKey()%>' },
                                        displayExpr: "Name",
                                        valueExpr: "ID",
                                        pageSize: 5,
                                        paginate: true
                                    }),
                                    allowClearing: true,
                                    valueExpr: "ID",
                                    displayExpr: "Name"
                                },
                                //allowEditing: false,
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Ratio",
                                caption: '<%=Resources.Labels.Ratio%>',
                                //allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Price",
                                caption: '<%=Resources.Labels.Price%>',
                                //allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Barcode",
                                caption: '<%=Resources.Labels.Barcode%>',
                                //allowEditing: false,
                                dataType: "string",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "IsFavorite",
                                caption: 'الوحدة المفضلة',
                                //allowEditing: false,
                                dataType: "boolean",
                                format: "decimal",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Id",
                                caption: "رقم الحركة",
                                allowEditing: false,
                                width: 150,
                                alignment: "center",
                                visible: false
                            }
                        ],
                    remoteOperations: false,
                    keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: "row", editOnKeyPress: true },
                    onEditorPreparing: function OnEditorPreparing(e) {
                        var component = e.component,
                            rowIndex = e.row && e.row.rowIndex;
                    },
                    onToolbarPreparing: function (e) {
                        e.toolbarOptions.visible = false;
                    },
                    editing: {
                        mode: "batch",
                        allowAdding: true,
                        allowDeleting: true,
                        allowUpdating: true,
                        selectTextOnEditStart: true,
                        startEditAction: "click",
                        useIcons: true
                    },
                    height: 150,
                    sorting: { mode: "none" },
                    customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                    rtlEnabled: true,
                    focusedRowEnabled: true,
                    onFocusedRowChanging: function OnFocusedRowChanging(e) {
                        currentRowIndex = e.newRowIndex;
                    }/*,
                selection: { mode: "single" },
                onSelectionChanged: function (e) {
                    e.component.byKey(e.currentSelectedRowKeys[0]).done(employee => {
                        if (employee) {
                            $("#selected-employee").text(`Selected employee: ${employee.FullName}`);
                        }
                    });
                },*/
                });
                clearItemUnitDgv(3);

                $("#rawMatGridContainer").dxDataGrid({
                    columnFixing: {
                        enabled: true
                    },
                    //repaintChangesOnly: true,
                    columnAutoWidth: true,
                    showBorders: true,
                    showRowLines: true,
                    dataSource: DevExpress.data.AspNet.createStore({
                        key: "Id",
                        loadUrl: "../../api/Item/LoadRawMat",
                        loadMethod: "get"
                    }),
                    allowColumnReordering: true,
                    columns:
                        [
                            {
                                caption: "#",
                                allowEditing: false,
                                width: 30
                            },
                            {
                                dataField: "MaterialId",
                                caption: '<%=Resources.Labels.RawItem%>',
                                dataType: "string",
                                lookup: {
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
                                    valueExpr: "ID",
                                    displayExpr: "Name"
                                },
                                //allowEditing: false,
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Quantity",
                                caption: '<%=Resources.Labels.Quantity%>',
                                //allowEditing: false,
                                dataType: "number",
                                format: "decimal",
                                alignment: "center",
                                width: 150
                            },
                            {
                                dataField: "Id",
                                caption: "رقم الحركة",
                                allowEditing: false,
                                width: 150,
                                alignment: "center",
                                visible: false
                            }
                        ],
                    remoteOperations: false,
                    keyboardNavigation: { enterKeyAction: "moveFocus", enterKeyDirection: "row", editOnKeyPress: true },
                    onEditorPreparing: function OnEditorPreparing(e) {
                        var component = e.component,
                            rowIndex = e.row && e.row.rowIndex;
                    },
                    onToolbarPreparing: function (e) {
                        e.toolbarOptions.visible = false;
                    },
                    editing: {
                        mode: "batch",
                        allowAdding: true,
                        allowDeleting: true,
                        allowUpdating: true,
                        selectTextOnEditStart: true,
                        startEditAction: "click",
                        useIcons: true
                    },
                    height: 150,
                    sorting: { mode: "none" },
                    customizeColumns: function (columns) { /*columns[0].width = 90;*/ },
                    rtlEnabled: true,
                    focusedRowEnabled: true,
                    onFocusedRowChanging: function OnFocusedRowChanging(e) {
                        currentRowIndex = e.newRowIndex;
                    }/*,
                selection: { mode: "single" },
                onSelectionChanged: function (e) {
                    e.component.byKey(e.currentSelectedRowKeys[0]).done(employee => {
                        if (employee) {
                            $("#selected-employee").text(`Selected employee: ${employee.FullName}`);
                        }
                    });
                },*/
                });
                clearRawMatDgv(3);
            }, 500);
            /*******************************************/
            $("#btnItemSearch").click(function () {
                refreshItemList();
            });
            /*******************************************/
            $("#butUpload").click(function () {
                var fd = new FormData();
                var files = $('#file')[0].files;
                // Check file selected or not
                if (files.length > 0) {
                    fd.append('file', files[0]);
                    $.ajax({
                        url: '../../api/File/UploadImage',
                        type: 'post',
                        data: fd,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            if (response != 0) {
                                $("#img").attr("src", response);
                                $(".preview img").show();
                                $("#LogoUrl").val(response);
                                $('#file')[0].files = null;
                            } else {
                                alert('file not uploaded');
                            }
                        },
                        error: function (e) {
                            console.log("error:" + e);
                        }
                    });
                } else {
                    alert("Please select a file.");
                }
            });
            /*******************************************/
            $("#btnSaveItemData").click(function () {
                saveItemfun();
            });
            /******************************************/
            $("#btnItemDelete").click(function () {
                var id = $("#ItemId").val();
                $.post("../../api/Item/DeleteItem", { Id: id }, function (response) {
                    if (response.Code == 100) {
                        refreshItemList();
                        clearItemTools();
                        $("#deleteitemviewerror").html('<%=Resources.Messages.MsgDeleteSuccess%>');
                        $("#deleteitemerrordiv").addClass("alert-success");
                        $("#deleteitemerrordiv").removeClass("alert-danger");
                        $("#deleteitemerrordiv").fadeIn();
                        $("#deleteitemerrordiv").fadeOut('slow', function () {
                            hideModal("#deleteItemModal");
                            $("#deleteitemerrordiv").fadeOut();
                        });
                    }
                    else if (response.Code == -101) {
                        $("#deleteitemviewerror").html(response.Message);
                        $("#deleteitemerrordiv").removeClass("alert-success");
                        $("#deleteitemerrordiv").addClass("alert-danger");
                        $("#deleteitemerrordiv").fadeIn();
                    }
                });
            });
            /******************************************/
            $('#txtBarcodeSearch').on("keyup", function () {
                refreshItemList();
            });
            /******************************************/
            $("#btnGenerateBarcode").click(function () {
                $.getJSON('../../api/Item/GenerateBarcode', null, function (response) {
                    $('#txtBarcode').val(response);
                });
            });
            setTimeout(function () {
                refreshItemList();
            }, 500);
        });

        function refreshItemList() {
            var barcode = null, category = null, ddlItemType = null, isDescription = null;
            if ($("#txtBarcodeSearch").val() != null && $("#txtBarcodeSearch").val() != '')
                barcode = $("#txtBarcodeSearch").val();

            if ($('#acCategorySearch option:selected').val() != null && $('#acCategorySearch option:selected').val() != '-1')
                category = $('#acCategorySearch option:selected').val();

            if ($('#ddlItemTypeSearch option:selected').val() != null)
                ddlItemType = $('#ddlItemTypeSearch option:selected').val();

            isDescription = $("#chkIsDescription").prop("checked");

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

                    $.post('../../api/item/Search', {
                        'Barcode': barcode, 'Category': category, 'ddlItemType': ddlItemType, 'IsDescription': isDescription,
                        'loadOptions': JSON.stringify(loadOptions)
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

            $("#searchProductsDgvDiv").dxDataGrid({
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
                            dataField: "CategoryName",
                            caption: '<%=Resources.Labels.Category%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "Name",
                            caption: '<%=Resources.Labels.Name%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "CodeItem",
                            caption: 'رمز الصنف',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center"
                        },
                        {
                            dataField: "Barcode",
                            caption: '<%=Resources.Labels.Barcode%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center"
                        },
                        {
                            dataField: "UOMName",
                            caption: '<%=Resources.Labels.SmallestUnit%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "SalesPrice",
                            caption: '<%=Resources.Labels.Purchasingprice%>',
                            allowEditing: false,
                            dataType: "string",
                            alignment: "center",
                        },
                        {
                            dataField: "DefaultPrice",
                            caption: '<%=Resources.Labels.Sellingprice%>',
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
                                    var rowIndex = '<%=Request.QueryString["rowIndex"] %>';
                                    selectMe(e.row.data.ID, e.row.data.Name, rowIndex, false);
                                }
                            }]
                        },
                        {
                            type: "buttons",
                            width: 70,
                            cssClass: 'btnEdit',
                            buttons: [{
                                text: '<%=Resources.Labels.Edit%>',
                                onClick(e) {
                                    editItemfun(e.row.data.ID);
                                }
                            }]
                        },
                        {
                            type: "buttons",
                            width: 70,
                            buttons: [{
                                text: '<%=Resources.Labels.Delete%>',
                                onClick(e) {
                                    deleteItemfun(e.row.data.ID, e.row.data.Name + "\n" + e.row.data.NameEN);
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

        function clearItemPriceDgv(rowCount) {
            var grid = $("#pricesGridContainer").dxDataGrid("instance");
            var rows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }

            for (var i = 0; i < rowCount; i++) {
                grid.addRow();
            }
        }

        function clearItemUnitDgv(rowCount) {
            var grid = $("#unitsGridContainer").dxDataGrid("instance");
            var rows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }

            for (var i = 0; i < rowCount; i++) {
                grid.addRow();
            }
        }

        function clearRawMatDgv(rowCount) {
            var grid = $("#rawMatGridContainer").dxDataGrid("instance");
            var rows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();
            for (i = rows.length - 1; i >= 0; i--) {
                grid.deleteRow(i);
            }

            for (var i = 0; i < rowCount; i++) {
                grid.addRow();
            }
        }

        function isNotEmpty(value) {
            return value !== undefined && value !== null && value !== "";
        }

        function clearItemTools() {
            $("#ItemOperation").val(null);
            $('#ItemId').val(null);
            $('#txtBarcode').val(null);
            $('#txtNameAr').val(null);
            $('#txtNameEn').val(null);
            $('#txtCodeItem').val(null);
            $("#acParentCategory").dxSelectBox('instance').option({ value: null });
            $("#ItemTypeItemeCardDDL").val('0');
            $('#txtCost').val(null);
            $('#txtPriceUnite').val(null);
            $('#txtMinQty').val(null);
            $('#txtMaxQty').val(null);
            $('#txtMaxDiscountCash').val(null);
            $('#txtMaxDiscountPercent').val(null);
            $("#acTaxItemeCard").dxSelectBox('instance').option({ value: null });
            $('#txtNotes').val(null);
            $('#txtPercentageDiscount').val(null);
            $("#acPrinterName").dxSelectBox('instance').option({ value: null });
            $("#txtQuantityProductRaw").val(null);
            $("#chkItemCuisine").prop("checked", false);
            $("#ChkHideItem").prop("checked", false);
            $("#chkIsUseTax").prop("checked", false);
            $("#chkIsTaxIncludedInPurchase").prop("checked", false);
            $("#chkIsTaxIncludedInSale").prop("checked", false);
            $("#acSmallestUnit").dxSelectBox('instance').option({ value: null });
            $('#txtMiSalePrice').val(null);
            $('#acAccountRelated').dxSelectBox('instance').option({ value: null });
            $("#chkIsBalance").prop("checked", false);

            $('#txtAveragePurchasePrice').val(null);
            $('#txtLastPurchasePrice').val(null);

            $("#img").attr("src", null);
            $("#img").attr("src", '/Images/no_photo.png');
            $("#LogoUrl").val('/Images/no_photo.png');
            $('#file')[0].files = null;

            clearItemPriceDgv(3);
            clearItemUnitDgv(3);
            clearRawMatDgv(3);
        }

        function addItemfun() {
            clearItemTools();
            $.getJSON('../../api/Item/GenerateBarcode', null, function (response) {
                $('#txtBarcode').val(response);
            });
            $("#btnSaveItemData").css({ "display": "block" });
            $("#ItemOperation").val("Add");
            $("#acSmallestUnit").dxSelectBox('instance').option({ value: defaultUnitId });
            $("#acParentCategory").dxSelectBox('instance').option({ value: defaultCategoryId });
            showModal('#UpdateItemDataModal');
            setTimeout(function () {
                $('#txtNameAr').focus();
            }, 1000);
        }

        function editItemfun(id) {
            clearItemTools();
            $.getJSON('../../api/Item/FindById', { Id: id },
                function (response) {
                    $('#ItemId').val(response[0].ID);
                    $('#txtBarcode').val(response[0].Barcode);
                    $('#txtNameAr').val(response[0].Name);
                    $('#txtNameEn').val(response[0].NameEN);
                    $('#txtCodeItem').val(response[0].CodeItem);
                    $("#acParentCategory").dxSelectBox('instance').option({ value: response[0].Category_ID });
                    $('#ItemTypeItemeCardDDL option:selected').val(response[0].Type);
                    $('#txtCost').val(response[0].Cost);
                    $('#txtPriceUnite').val(response[0].DefaultPrice);
                    $('#txtMinQty').val(response[0].MiniQty);
                    $('#txtMaxQty').val(response[0].MaxQty);
                    $('#txtMaxDiscountCash').val(response[0].MaxDicountCash);
                    $('#txtMaxDiscountPercent').val(response[0].MaxDicountParcent);
                    $("#acTaxItemeCard").dxSelectBox('instance').option({ value: response[0].Tax_ID });
                    $('#txtNotes').val(response[0].Description);
                    $('#txtPercentageDiscount').val(response[0].DiscountPercentage);
                    $("#acPrinterName").dxSelectBox('instance').option({ value: response[0].PrinterName_ID });
                    if (response[0].LogoUrl != null && response[0].LogoUrl != '')
                        $('#LogoUrl').val(response[0].LogoUrl);
                    $("#txtQuantityProductRaw").val(response[0].QuantityProductRaw);
                    $("#chkItemCuisine").prop("checked", response[0].IsCuisine);
                    $("#ChkHideItem").prop("checked", response[0].HideItem);
                    $("#chkIsUseTax").prop("checked", response[0].IsUseTax);
                    $("#chkIsTaxIncludedInPurchase").prop("checked", response[0].IsTaxIncludedInPurchase);
                    $("#chkIsTaxIncludedInSale").prop("checked", response[0].IsTaxIncludedInSale);
                    $("#acSmallestUnit").dxSelectBox('instance').option({ value: response[0].UOM_ID });
                    $('#txtMiSalePrice').val(response[0].MiniLevelPrice);
                    $('#acAccountRelated').dxSelectBox('instance').option({ value: response[0].Account_ID });
                    $("#chkIsBalance").prop("checked", response[0].IsItemBalance);

                    $.getJSON('../../api/Item/FindItemPrices', { Id: id, UnitId: response[0].UOM_ID },
                        function (inResponse) {
                            $('#txtAveragePurchasePrice').val(inResponse[0].AvergePurchsePrice);
                            $('#txtLastPurchasePrice').val(inResponse[0].LastPurchsePrice);
                        });

                    $.getJSON("../../api/Item/GetItemPrices", { Id: id },
                        function (responseTable) {
                            var grid = $("#pricesGridContainer").dxDataGrid("instance");
                            //var cols = $("#pricesGridContainer").dxDataGrid("instance").getVisibleColumns();
                            var rows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();

                            var length = rows.length;
                            for (var i = length - 1; i >= 0; i--) {
                                grid.deleteRow(i);
                            }

                            length = responseTable.length;
                            for (var i = 0; i < length + 1; i++) {
                                grid.addRow(i);
                            }

                            rows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();

                            for (var i = 0; i < responseTable.length; i++) {
                                rows[i].data["PriceTypeId"] = responseTable[i].PriceName_ID;
                                rows[i].data["Price"] = responseTable[i].Price;
                                rows[i].data["UnitId"] = responseTable[i].Uom_ID;
                                rows[i].data["Id"] = responseTable[i].ID;
                            }
                        });

                    $.getJSON("../../api/Item/GetItemUnits", { Id: id },
                        function (responseTable) {
                            var grid = $("#unitsGridContainer").dxDataGrid("instance");
                            var rows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();

                            var length = rows.length;
                            for (var i = length - 1; i >= 0; i--) {
                                grid.deleteRow(i);
                            }

                            length = responseTable.length;
                            for (var i = 0; i < length + 1; i++) {
                                grid.addRow(i);
                            }

                            rows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();

                            for (var i = 0; i < responseTable.length; i++) {
                                rows[i].data["UnitId"] = responseTable[i].Unit_ID;
                                rows[i].data["Ratio"] = responseTable[i].Ratio;
                                rows[i].data["Price"] = responseTable[i].Price;
                                rows[i].data["Barcode"] = responseTable[i].Barcode;
                                rows[i].data["IsFavorite"] = responseTable[i].IsFavorite;
                                rows[i].data["Id"] = responseTable[i].ID;
                            }
                        });

                    $.getJSON("../../api/Item/GetItemMaterials", { Id: id },
                        function (responseTable) {
                            var grid = $("#rawMatGridContainer").dxDataGrid("instance");
                            var rows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();

                            var length = rows.length;
                            for (var i = length - 1; i >= 0; i--) {
                                grid.deleteRow(i);
                            }

                            length = responseTable.length;
                            for (var i = 0; i < length + 1; i++) {
                                grid.addRow(i);
                            }

                            rows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();

                            for (var i = 0; i < responseTable.length; i++) {
                                rows[i].data["MaterialId"] = responseTable[i].Material_ID;
                                rows[i].data["Quantity"] = responseTable[i].Quantity;
                                rows[i].data["Id"] = responseTable[i].ID;
                            }
                        });
                    $("#btnSaveItemData").css({ "display": "block" });
                    $("#ItemOperation").val("Edit");
                    showModal('#UpdateItemDataModal');
                }
            );
        }

        function deleteItemfun(id, name) {
            $("#itemName").html(name);
            $("#ItemId").val(id);
            $("#deleteitemerrordiv").fadeOut();
            showModal('#deleteItemModal');
        }

        function saveItemfun() {
            /*var pricesGrid = $("#pricesGridContainer").dxDataGrid("instance");
            var unitsGrid = $("#unitsGridContainer").dxDataGrid("instance");
            var rawMatGrid = $("#rawMatGridContainer").dxDataGrid("instance");*/

            if ($('#txtBarcode').val() == "" || $('#txtBarcode').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemBarcodeRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtBarcode').focus();
                return false;
            }
            else if ($('#txtNameAr').val() == "" || $('#txtNameAr').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemNameRequred%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtNameAr').focus();
                return false;
            }
            <%--else if ($('#txtNameEn').val() == "" || $('#txtNameEn').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemNameRequred%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtNameEn').focus();                
                return false;
            }
            else if ($('#txtCodeItem').val() == "" || $('#txtCodeItem').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemCodeRequred%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtCodeItem').focus();
                return false;
            }--%>
            else if ($("#acParentCategory").dxSelectBox('instance').option('value') == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemParentCategoryRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#acParentCategory').focus();
                return false;
            }
            else if ($('#ItemTypeItemeCardDDL option:selected').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemTypeItemCardRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#ItemTypeItemeCardDDL').focus();
                return false;
            }
            else if ($('#txtCost').val() == "" || $('#txtCost').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgDocUnitCostRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtCost').focus();
                return false;
            }
            <%--else if ($('#txtLastPurchasePrice').val() == "" || $('#txtLastPurchasePrice').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemLastPurchasePriceRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtLastPurchasePrice').focus();
                return false;
            }
            else if ($('#txtAveragePurchasePrice').val() == "" || $('#txtAveragePurchasePrice').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemAveragePurchasePriceRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtAveragePurchasePrice').focus();
                return false;
            }
            else if ($('#txtPriceUnite').val() == "" || $('#txtPriceUnite').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemPriceUnitRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtPriceUnite').focus();
                return false;
            }
            else if ($('#txtMiSalePrice').val() == "" || $('#txtMiSalePrice').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemMiSalePriceRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtMiSalePrice').focus();
                return false;
            }
            else if ($('#txtMinQty').val() == "" || $('#txtMinQty').val() == null) {
                $("#itemerrordiv").fadeIn();
                $("#itemviewerror").html("<%=Resources.Messages.MsgItemMinQtyRequired%>");
                $("#itemerrordiv").fadeOut('slow');
                $('#txtMinQty').focus();
                return false;
            }--%>
            else {
                var itemId = null;
                var operation = $("#ItemOperation").val();
                if (operation == "Edit") {
                    itemId = $("#ItemId").val();
                }
                var barcode = $('#txtBarcode').val();
                var name = $('#txtNameAr').val();
                var nameEn = $('#txtNameEn').val();
                var codeItem = $('#txtCodeItem').val();
                var parentCategory = $("#acParentCategory").dxSelectBox('instance').option('value');
                var itemType = $('#ItemTypeItemeCardDDL option:selected').val();
                var cost = $('#txtCost').val();
                var lastPurchasePrice = $('#txtLastPurchasePrice').val();
                var averagePurchasePrice = $('#txtAveragePurchasePrice').val();
                var priceUnit = $('#txtPriceUnite').val();
                var miSalePrice = $('#txtMiSalePrice').val();
                var minQty = $('#txtMinQty').val();
                var maxQty = $('#txtMaxQty').val();
                var smallestUnitId = $("#acSmallestUnit").dxSelectBox('instance').option('value');
                var taxId = $("#acTaxItemeCard").dxSelectBox('instance').option('value');
                var percentageDiscount = $('#txtPercentageDiscount').val();
                var maxDiscountCash = $('#txtMaxDiscountCash').val();
                var maxDiscountPercent = $('#txtMaxDiscountPercent').val();
                var notes = $('#txtNotes').val();
                var printerName = $("#acPrinterName").dxSelectBox('instance').option('value');// $('#txtPrinterName').val();                
                var accountRelated = $('#acAccountRelated').val();
                var logoUrl = $('#LogoUrl').val();
                var quantityProductRaw = $("#txtQuantityProductRaw").val();

                var isBalance = $("#chkIsBalance").prop("checked");
                var isItemCuisine = $("#chkItemCuisine").prop("checked");
                var isHideItem = $("#ChkHideItem").prop("checked");
                var isUseTax = $("#chkIsUseTax").prop("checked");
                var isTaxIncludedInPurchase = $("#chkIsTaxIncludedInPurchase").prop("checked");
                var isTaxIncludedInSale = $("#chkIsTaxIncludedInSale").prop("checked");

                var priceRows = $("#pricesGridContainer").dxDataGrid("instance").getVisibleRows();
                var priceDataRows = [];
                for (i = 0; i < priceRows.length; i++) {
                    var isEmptyRow = false;
                    if ((priceRows[i].data["Id"] == null || priceRows[i].data["Id"] == '') &&
                        (priceRows[i].data["PriceTypeId"] == null || priceRows[i].data["PriceTypeId"] == '') &&
                        (priceRows[i].data["Price"] == null || priceRows[i].data["Price"] == '') &&
                        (priceRows[i].data["UnitId"] == null || priceRows[i].data["UnitId"] == '')
                    )
                        isEmptyRow = true;

                    if (priceRows[i].rowType == "data" && isEmptyRow == false) {
                        var id = null, priceTypeId = null, price = null, unitId = null;
                        if (priceRows[i].data["Id"] != null && priceRows[i].data["Id"] != '')
                            id = priceRows[i].data["Id"];
                        if (priceRows[i].data["PriceTypeId"] != null && priceRows[i].data["PriceTypeId"] != '')
                            priceTypeId = priceRows[i].data["PriceTypeId"];
                        if (priceRows[i].data["Price"] != null && priceRows[i].data["Price"] != '')
                            price = priceRows[i].data["Price"];
                        if (priceRows[i].data["UnitId"] != null && priceRows[i].data["UnitId"] != '')
                            unitId = priceRows[i].data["UnitId"];
                        var priceRow = {
                            'ID': id, 'PriceName_ID': priceTypeId, 'Price': price, 'UOM_ID': unitId
                        };
                        priceDataRows.push(priceRow);
                        //priceDataRows.push(rows[i].data);
                    }
                }
                priceDataRows = JSON.stringify(priceDataRows);

                var unitRows = $("#unitsGridContainer").dxDataGrid("instance").getVisibleRows();
                var unitDataRows = [];
                for (i = 0; i < unitRows.length; i++) {
                    var isEmptyRow = false;
                    if ((unitRows[i].data["Id"] == null || unitRows[i].data["Id"] == '') &&
                        (unitRows[i].data["UnitId"] == null || unitRows[i].data["UnitId"] == '') &&
                        (unitRows[i].data["Ratio"] == null || unitRows[i].data["Ratio"] == '') &&
                        (unitRows[i].data["Price"] == null || unitRows[i].data["Price"] == '') &&
                        (unitRows[i].data["Barcode"] == null || unitRows[i].data["Barcode"] == '')
                    )
                        isEmptyRow = true;

                    if (unitRows[i].rowType == "data" && isEmptyRow == false) {
                        var id = null, unitId = null, ratio = null, price = null, rowBarcode = null, isFavorite = null;
                        if (unitRows[i].data["Id"] != null && unitRows[i].data["Id"] != '')
                            id = unitRows[i].data["Id"];
                        if (unitRows[i].data["UnitId"] != null && unitRows[i].data["UnitId"] != '')
                            unitId = unitRows[i].data["UnitId"];
                        if (unitRows[i].data["Ratio"] != null && unitRows[i].data["Ratio"] != '')
                            ratio = unitRows[i].data["Ratio"];
                        if (unitRows[i].data["Price"] != null && unitRows[i].data["Price"] != '')
                            price = unitRows[i].data["Price"];
                        if (unitRows[i].data["Barcode"] != null && unitRows[i].data["Barcode"] != '')
                            rowBarcode = unitRows[i].data["Barcode"];
                        if (unitRows[i].data["IsFavorite"] != null && unitRows[i].data["IsFavorite"] != '')
                            isFavorite = unitRows[i].data["IsFavorite"];

                        var unitRow = {
                            'ID': id, 'Unit_ID': unitId, 'Ratio': ratio, 'Price': price, 'Barcode': rowBarcode, IsFavorite: isFavorite
                        };
                        unitDataRows.push(unitRow);
                        //unitDataRows.push(rows[i].data);
                    }
                }
                unitDataRows = JSON.stringify(unitDataRows);

                var rawMatRows = $("#rawMatGridContainer").dxDataGrid("instance").getVisibleRows();
                var rawMatDataRows = [];
                for (i = 0; i < rawMatRows.length; i++) {
                    var isEmptyRow = false;
                    if ((rawMatRows[i].data["Id"] == null || rawMatRows[i].data["Id"] == '') &&
                        (rawMatRows[i].data["MaterialId"] == null || rawMatRows[i].data["MaterialId"] == '') &&
                        (rawMatRows[i].data["Quantity"] == null || rawMatRows[i].data["Quantity"] == '')
                    )
                        isEmptyRow = true;

                    if (rawMatRows[i].rowType == "data" && isEmptyRow == false) {
                        var id = null, materialId = null, quantity = null;
                        if (rawMatRows[i].data["Id"] != null && rawMatRows[i].data["Id"] != '')
                            id = rawMatRows[i].data["Id"];
                        if (rawMatRows[i].data["MaterialId"] != null && rawMatRows[i].data["MaterialId"] != '')
                            materialId = rawMatRows[i].data["MaterialId"];
                        if (rawMatRows[i].data["Quantity"] != null && rawMatRows[i].data["Quantity"] != '')
                            quantity = rawMatRows[i].data["Quantity"];
                        var rawMatRow = {
                            'ID': id, 'Material_ID': materialId, 'Quantity': quantity
                        };
                        rawMatDataRows.push(rawMatRow);
                        //rawMatDataRows.push(rows[i].data);
                    }
                }
                rawMatDataRows = JSON.stringify(rawMatDataRows);

                $.post('../../api/Item/SaveItem', {
                    ItemId: itemId, Barcode: barcode, Name: name, NameEn: nameEn, CodeIteme: codeItem, CategoryId: parentCategory, ItemType: itemType, Cost: cost,
                    LastPurchasePrice: lastPurchasePrice, AveragePurchasePrice: averagePurchasePrice, Price: priceUnit, MinSalePrice: miSalePrice,
                    MaxQty: maxQty, MinQty: minQty, SmallestUnitId: smallestUnitId, TaxId: taxId, PercentageDiscount: percentageDiscount, MaxDiscountCash: maxDiscountCash,
                    MaxDiscountPercent: maxDiscountPercent, Description: notes, PrinterNameId: printerName, AccountRelated: accountRelated, LogoUrl: logoUrl,
                    IsBalance: isBalance, IsCuisine: isItemCuisine, IsHideItem: isHideItem, IsUseTax: isUseTax, IsTaxIncludedInPurchase: isTaxIncludedInPurchase,
                    IsTaxIncludedInSale: isTaxIncludedInSale, QuantityProductRaw: quantityProductRaw,
                    PriceSource: priceDataRows, UnitSource: unitDataRows, RawMatSource: rawMatDataRows
                },
                    function (response) {
                        if (response.Code != 100) {
                            $("#itemerrordiv").removeClass("alert-success");
                            $("#itemerrordiv").addClass("alert-danger");
                            $("#itemerrordiv").fadeIn();
                            $("#itemviewerror").html(response.Message);
                            $("#itemerrordiv").fadeOut('slow');
                            //hideModal('#UpdateItemDataModal');
                        }
                        else {
                            $("#itemerrordiv").removeClass("alert-danger");
                            $("#itemerrordiv").addClass("alert-success");
                            $("#itemerrordiv").fadeIn();
                            $("#itemviewerror").html(response.Message);
                            $("#itemerrordiv").fadeOut('slow', function () {
                                hideModal("#UpdateItemDataModal");
                                //showModal('#UpdateItemDataModal');
                                var rowIndex = '<%=Request.QueryString["rowIndex"] %>';
                                selectMe(response.itemId, name, rowIndex, true);
                            });
                            refreshItemList();
                        }
                    }
                );
            }
        }

        function selectMe(itemId, itemName, rowIndex, isNewItem) {
            itemSelectedF9(itemId, itemName, rowIndex, isNewItem);
        }
    </script>

    <style type="text/css">
        /* * {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif;
            font-size: 12px;
            font-weight: normal;            
        }*/

        a.dx-link {
            /*color:red !important;*/
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="ItemId" />
        <input type="hidden" id="ItemOperation" />
        <%-- UpdateItemDataModal --%>
        <div id="UpdateItemDataModal" class="modal fade">
            <div class="modal-dialog modal-xl">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <strong><%=Resources.Labels.ItemData %></strong>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body" style="overflow: auto;">
                        <div class="alert alert-danger col-md-12" id="itemerrordiv" style="display: none;">
                            <span id="itemviewerror"></span>
                        </div>
                        <ul class="nav nav-tabs" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="btnPrimaryData" data-bs-toggle="tab" data-bs-target="#divPrimaryData" type="button" role="tab" aria-controls="AccountDgv" aria-selected="true"><%=Resources.Labels.BasicData %></button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="BtnSecondData" data-bs-toggle="tab" data-bs-target="#divSecondData" type="button" role="tab" aria-controls="AccountTree" aria-selected="false">بيانات اضافية</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="BtnPricesData" data-bs-toggle="tab" data-bs-target="#divPricesData" type="button" role="tab" aria-controls="AccountTree" aria-selected="false"><%=Resources.Labels.Prices %></button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="BtnUnitsData" data-bs-toggle="tab" data-bs-target="#divUnitsData" type="button" role="tab" aria-controls="AccountTree" aria-selected="false"><%=Resources.Labels.Units %></button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="BtnRawMatData" data-bs-toggle="tab" data-bs-target="#divRawMatData" type="button" role="tab" aria-controls="AccountTree" aria-selected="false"><%=Resources.Labels.RawItems1 %></button>
                            </li>
                        </ul>

                        <div class="tab-content">
                            <div id="divPrimaryData" class="tab-pane active" role="tabpanel" aria-labelledby="home-tab" style="padding: 5px;">
                                <div class="row grid-divider">
                                    <div class="col-md-1" style="margin-left: 5px;"></div>
                                    <div class="col-md-3" style="margin-left: 5px;">
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Barcode %><span style="color: red">*</span></label>
                                            <input id="txtBarcode" type="text" placeholder="<%=Resources.Labels.Barcode %>" class="form-control col-md-10" />
                                            <input type="button" id="btnGenerateBarcode" value="+" class="col-md-2" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Name %><span style="color: red">*</span></label>
                                            <input id="txtNameAr" type="text" placeholder="<%=Resources.Labels.Name %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.NameEN %></label>
                                            <input id="txtNameEn" type="text" placeholder="<%=Resources.Labels.NameEN %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.CodeItem %><span style="color: red"></span></label>
                                            <input id="txtCodeItem" type="text" placeholder="<%=Resources.Labels.CodeItem %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.ParentCategory %><span style="color: red">*</span></label>
                                            <div id="acParentCategory" class="dxComb"></div>
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.SmallestUnit %><span style="color: red">*</span></label>
                                            <div id="acSmallestUnit" class="dxComb"></div>
                                        </div>
                                    </div>
                                    <div class="col-md-3" style="margin-left: 5px;">
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Cost %></label>
                                            <input id="txtCost" type="text" placeholder="<%=Resources.Labels.Cost %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Price %></label>
                                            <input id="txtPriceUnite" type="text" placeholder="<%=Resources.Labels.Price %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.MiSalePrice %></label>
                                            <input id="txtMiSalePrice" type="text" placeholder="<%=Resources.Labels.MiSalePrice %>" class="form-control" />
                                        </div>
                                        <%--<div class="row col-padding">
                                        <label>عدد الاصناف المسموح بها للعرض</label>
                                        <input id="txtQtyItems" type="text" placeholder="عدد الاصناف المسموح بها للعرض" class="form-control" />
                                    </div> --%>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.ItemType %><span style="color: red">*</span></label>
                                            <asp:DropDownList ID="ItemTypeItemeCardDDL" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Text="<%$ Resources:Labels,SelectProductType %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Tax %></label>
                                            <div id="acTaxItemeCard" class="dxComb"></div>
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.AccountName%></label>
                                            <div id="acAccountRelated" class="dxComb"></div>
                                        </div>
                                    </div>
                                    <div class="col-md-4" style="margin-left: 5px;">
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Logo %></label>
                                            <input id="LogoUrl" value="/Images/no_photo.png" type="hidden" />
                                            <input type="button" class="button col-md-3" value="Upload" id="butUpload" />
                                            <input type="file" id="file" name="file" value="" class="col-md-5" />
                                        </div>
                                        <div class="row col-padding">
                                            <div class="col-md-8">
                                                <div class='preview'>
                                                    <img id="img" alt="" src="/Images/no_photo.png" style="max-width: 200px; max-height: 150px" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-1"></div>
                                    <div class="col-md-2">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="chkIsBalance">
                                                <input type="checkbox" class="form-check-input" id="chkIsBalance" />
                                                صنف ميزان
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="chkItemCuisine">
                                                <input type="checkbox" class="form-check-input" id="chkItemCuisine" />
                                                صنف مطبخ
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="ChkHideItem">
                                                <input type="checkbox" class="form-check-input" id="ChkHideItem" />
                                                اخفاء
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="chkIsUseTax">
                                                <input type="checkbox" class="form-check-input" id="chkIsUseTax" />
                                                استخدام الضريبة
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="chkIsTaxIncludedInPurchase">
                                                <input type="checkbox" class="form-check-input" id="chkIsTaxIncludedInPurchase" />
                                                سعر الشراء شامل الضريبة
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-check row">
                                            <label class="form-check-label" for="chkIsTaxIncludedInSale">
                                                <input type="checkbox" class="form-check-input" id="chkIsTaxIncludedInSale" />
                                                سعر البيع شامل الضريبة
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divSecondData" class="tab-pane" role="tabpanel" aria-labelledby="home-tab" style="padding: 5px;">
                                <div class="row grid-divider">
                                    <div class="col-md-3"></div>
                                    <div class="col-md-3" style="margin-left: 5px;">
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.LastPurchasePrice %></label>
                                            <input id="txtLastPurchasePrice" type="text" placeholder="<%=Resources.Labels.LastPurchasePrice %>" class="form-control" readonly />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.AveragePurchasePrice %></label>
                                            <input id="txtAveragePurchasePrice" type="text" placeholder="<%=Resources.Labels.AveragePurchasePrice %>" class="form-control" readonly />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.MinQty %></label>
                                            <input id="txtMinQty" type="text" placeholder="<%=Resources.Labels.MinQty %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.MaxQty %></label>
                                            <input id="txtMaxQty" type="text" placeholder="<%=Resources.Labels.MaxQty %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.PrinterName %></label>
                                            <div id="acPrinterName" class="dxComb"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-3" style="margin-left: 5px;">
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.PercentageDiscount %></label>
                                            <input id="txtPercentageDiscount" type="text" placeholder="<%=Resources.Labels.PercentageDiscount %>" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label>أعلى خصم نقدي</label>
                                            <input id="txtMaxDiscountCash" type="text" placeholder="أعلى خصم نقدي" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label>أعلى خصم نسبة</label>
                                            <input id="txtMaxDiscountPercent" type="text" placeholder="أعلى خصم نسبة" class="form-control" />
                                        </div>
                                        <div class="row col-padding">
                                            <label><%=Resources.Labels.Notes %></label>
                                            <input id="txtNotes" type="text" placeholder="<%=Resources.Labels.Notes %>" class="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divUnitsData" class="tab-pane" role="tabpanel" aria-labelledby="home-tab" style="padding: 5px;">
                                <div class="row grid-divider">
                                    <div class="dxGridParent">
                                        <div id="unitsGridContainer" class="dxGrid col-md-12"></div>
                                        <br />
                                    </div>
                                </div>
                            </div>

                            <div id="divPricesData" class="tab-pane" role="tabpanel" aria-labelledby="profile-tab" style="padding: 5px;">
                                <div class="row grid-divider">
                                    <div class="dxGridParent">
                                        <div id="pricesGridContainer" class="dxGrid col-md-12"></div>
                                        <br />
                                    </div>
                                </div>
                            </div>

                            <div id="divRawMatData" class="tab-pane" role="tabpanel" aria-labelledby="home-tab" style="padding: 5px;">
                                <div class="row">
                                    <label class="col-md-1"></label>
                                    <label class="col-md-1"><%=Resources.Labels.Quantity %></label>
                                    <input id="txtQuantityProductRaw" type="text" placeholder="<%=Resources.Labels.Quantity %>" class="col-md-2 form-control" />
                                </div>
                                <div class="row grid-divider">
                                    <div class="dxGridParent">
                                        <div id="rawMatGridContainer" class="dxGrid col-md-12"></div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnSaveItemData" style="font-size: 20px; border-radius: 5px; margin: 10px; display: none;" class="btn btn-primary btn-lg"><%=Resources.Labels.Save %></button>
                        <button type="button" data-bs-dismiss="modal" class="btn btn-danger btn-lg" style="font-size: 20px; border-radius: 5px; margin: 10px;"><%=Resources.Labels.Close %></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="deleteItemModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <strong><%=Resources.Labels.DeleteConfirm %></strong>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>
                    <div class="modal-body">
                        <h3><%=Resources.Labels.DeleteConfirmAsk %></h3>
                        <h4 id="itemName"></h4>
                        <h4 id="itemId"></h4>
                        <div class="alert col-md-12" id="deleteitemerrordiv" style="display: none;">
                            <span id="deleteitemviewerror"></span>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnItemDelete" class="btn btn-danger btn-lg"><%=Resources.Labels.Delete%></button>
                        <button type="button" class="btn btn-success btn-lg" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <div class="MainInvoiceStyleDiv">
            <div class="container-fluid">

                <div class="dx-viewport" style="width: 100%">
                    <div class="row" style="margin-left: 20px; margin-right: 20px;">
                        <div class="col-md-12">
                            <div class="row">
                                <a href="#" class="col-md-2 btn btn-info" onclick="addItemfun()"><%=Resources.Labels.AddNew %></a>

                            </div>
                            <div class="row">
                                <input id="txtBarcodeSearch" class="col-md-1 form-control" type="text" placeholder="<%=Resources.Labels.Barcode %>" />

                                <div class="col-md-3">
                                    <asp:DropDownList ID="acCategorySearch" runat="server" AutoPostBack="false" ClientIDMode="Static" />
                                </div>

                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlItemTypeSearch" runat="server" AutoPostBack="false" ClientIDMode="Static">
                                        <asp:ListItem Text="<%$ Resources:Labels,SelectProductType %>" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-2">
                                    <div class="form-check row">
                                        <label class="form-check-label" for="chkIsDescription">
                                            <input type="checkbox" class="form-check-input" id="chkIsDescription" />
                                            <%=Resources.Labels.ArticleDescribedLabel %>
                                        </label>
                                    </div>
                                </div>

                                <input type="button" id="btnItemSearch" class="col-md-1 btn btn-primary" value="<%=Resources.Labels.Search%>" />
                            </div>
                        </div>

                        <div class="row" style="width: 100%">
                            <div id="searchProductsDgvDiv" class=" col-md-12">
                            </div>
                        </div>
                    </div>
                </div>
                 
            </div> 
        <%--<div class="container-fluid">
                <div class="row">
                    <a href="#" class="col-md-2 btn btn-info" onclick="addItemfun()"><%=Resources.Labels.AddNew %></a>                    
                </div>
                <br />
                <div class="row">
                    <input id="txtBarcodeSearch" class="col-md-1 form-control" type="text" placeholder="<%=Resources.Labels.Barcode %>" />

                    <div class="col-md-3">                        
                        <asp:DropDownList ID="acCategorySearch" runat="server" AutoPostBack="false" ClientIDMode="Static"/>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlItemTypeSearch" runat="server" AutoPostBack="false" ClientIDMode="Static">
                            <asp:ListItem Text="<%$ Resources:Labels,SelectProductType %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                        </asp:DropDownList>
                    </div>                    

                    <div class="col-md-2">
                        <div class="form-check row">
                            <label class="form-check-label" for="chkIsDescription">
                                <input type="checkbox" class="form-check-input" id="chkIsDescription" />
                                <%=Resources.Labels.ArticleDescribedLabel %>
                            </label>
                        </div>
                    </div>

                    <input type="button" id="btnItemSearch" class="col-md-1 btn btn-primary" value="<%=Resources.Labels.Search%>" />
                </div>
                <br />
                <div id="searchProductsDgvDiv" class="row">
                </div>
            </div>--%>
        </div>
    </form>
</body>
</html>
