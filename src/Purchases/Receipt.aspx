<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Receipt.aspx.cs" Inherits="Purchases_Receipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucNewItemDescribed.ascx" TagPrefix="asp" TagName="ucNewItemDescribed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server"
                ReadOnly="true" Width="200" Style="text-align: center;">
            </asp:ABFTextBox>

            <asp:Label runat="server" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.Receipt %></asp:Label>

            <div runat="server" visible="false" id="divPurchaseOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.PurchaseOrderNo %></span>:
                <asp:Label ID="lblPurchaseOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                    <span>
                        <asp:AutoComplete runat="server" ID="acVendor" ServiceMethod="GetContactNames" IsRequired="true"
                            OnSelectedIndexChanged="acVendor_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"
                            LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkAddNewVendor" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    </span>
                     <asp:AutoComplete runat="server" ID="acdrivers" ServiceMethod="Getcar" LabelText="<%$Resources:Labels,DriverNamne %>"
                     ValidationGroup="Save"  ></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                        LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                        LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                        LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                        LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">

                        <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                            IsRequired="true" ValidationGroup="AddItem"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                            LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                            ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                            OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" IsHideable="true">
                        </asp:ABFTextBox>

                         <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>" 
                            OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true" IsHideable="true">
                        </asp:ABFTextBox>

                        <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                            OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                            LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>

                        <asp:Panel runat="server" ID="pnlItemdescribed">
                            <asp:AutoComplete runat="server" ID="acItemDescribed" ServiceMethod="GetItemsDescribed"
                                AutoPostBack="true" ValidationGroup="AddItem"
                                LabelText="<%$Resources:Labels,Itemdescribed %>"></asp:AutoComplete>
                            <asp:LinkButton ID="lnkNewDescribed" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                        </asp:Panel>

                        <asp:ABFTextBox ID="txtQty" Text="1" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                            DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" MinValue="0.001">
                        </asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtCapacity" runat="server" LabelText="<%$Resources:Labels,Capacity %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtCapacities" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                            ValidationGroup="AddItem" OnTextChanged="txtCapacities_OnTextChanged"  AutoPostBack="true"></asp:ABFTextBox>
                        <br />
                        <span><%=Resources.Labels.ActualQuantity %> : </span>
                        <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>


                        <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                            IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                            AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                        <span>

                            <%-- LabelText="<%$Resources:Labels,BatchID %>"--%>
                            <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" CssClass="hiddencol"
                                OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>

                            <asp:LinkButton ID="lnkNewBatch" runat="server" CssClass="PlusBtn hiddencol">[+]</asp:LinkButton>

                        </span>

                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Cost %>"
                            MinValue="0.00000000" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem">
                        </asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                            MinValue="0.00000000" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                            MinValue="0.00000000" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtPolicy" runat="server" LabelText="<%$Resources:Labels,Policy %>"
                            MinValue="0" IsRequired="True" DataType="FreeString" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtCode" IsRequired="True" runat="server" LabelText="<%$Resources:Labels,Code %>"
                            MinValue="0" DataType="FreeString" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtInvoiceDate" IsHideable="true" runat="server" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,DateRequest %>"
                            DataType="Date" IsRequired="true">
                        </asp:ABFTextBox>

                        <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>

                        <asp:Panel runat="server" ID="Panel1" Visible="False">
                            <%-- LabelText="<%$Resources:Labels,ProductionDate %>"--%>
                            <asp:ABFTextBox ID="txtProductionDate" CssClass="hiddencol" runat="server"
                                Enabled="false" IsHideable="true">
                            </asp:ABFTextBox>

                            <%-- LabelText="<%$Resources:Labels,ExpirationDate %>"--%>
                            <asp:ABFTextBox ID="txtExpirationDate" CssClass="hiddencol" runat="server"
                                Enabled="false" IsHideable="true">
                            </asp:ABFTextBox>
                        </asp:Panel>
                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                            TextMode="MultiLine">
                        </asp:ABFTextBox>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddItem" />
                </div>
                <br />
                <div class="btnDiv">
                    <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                    <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" OnClick="btnAddItemGroup_click"
                        Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                    <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearItem_Click" />
                </div>
            </asp:Panel>
        </div>
        <div class="InvoiceSection">
            <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                OnSelectedIndexChanging="gvItems_SelectedIndexChanging" OnPreRender="gvItems_OnPreRender">
                <Columns>
                    <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Labels,Store %>" />
                    <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />
                    <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
                    <asp:BoundField DataField="DescribedName" HeaderText="<%$Resources:Labels,Itemdescribed %>" />
                    <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                        DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="IDCodeOperation" HeaderText="<%$Resources:Labels,Code %>" />
                    <asp:BoundField DataField="Policy" HeaderText="<%$Resources:Labels,Policy %>" />
                    <asp:BoundField DataField="QtyInNumber" HeaderText="<%$Resources:Labels,QtyInNumber %>"
                        DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="Capacity" HeaderText="<%$Resources:Labels,Capacity %>"
                        DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="Capacities" HeaderText="<%$Resources:Labels,CapacityDistribution %>" />

                    <asp:BoundField DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                    <%-- <asp:BoundField  DataField="BatchName" HeaderText="<%$Resources:Labels,BatchID %>" ItemStyle-CssClass="BatchCol" />
                    <asp:BoundField DataField="ProductionDate" HeaderText="<%$Resources:Labels,ProductionDate %>" ItemStyle-CssClass="BatchCol"
                        DataFormatString="{0:d/M/yyyy}" />
                    <asp:BoundField DataField="ExpirationDate" HeaderText="<%$Resources:Labels,ExpirationDate %>" ItemStyle-CssClass="BatchCol"
                        DataFormatString="{0:d/M/yyyy}" />--%>
                    <asp:BoundField DataField="UnitCost" HeaderText="<%$Resources:Labels,Cost %>" DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Labels,Total %>" DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="PercentageDiscount" HeaderText="<%$Resources:Labels,PercentageDiscount %>"
                        DataFormatString="{0:0.########}" />
                    <asp:BoundField DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>"
                        DataFormatString="{0:0.########}" />
                    <asp:BoundField DataField="TaxPercentageValue" HeaderText="<%$Resources:Labels,Tax %>" ItemStyle-CssClass="TaxCol"
                        DataFormatString="{0:0.########}" />
                    <asp:BoundField DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                        DataFormatString="{0:0.########}" />
                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                OnClientClick="return ConfirmSure();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:ABFGridView>
        </div>
        <div class="InvoiceSection" id="taxSection" runat="server">
            <div class="form" style="width: 600px; margin: auto;">
                <asp:AutoComplete runat="server" ID="acTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" AutoCompleteWidth="300"
                    IsRequired="true" ValidationGroup="AddTax"></asp:AutoComplete>
                <asp:Button ID="btnAddTax" CssClass="button" runat="server" OnClick="btnAddTax_click"
                    Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddTax" />
            </div>
            <asp:ABFGridView runat="server" ID="gvTaxes" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnRowDeleting="gvTaxes_RowDeleting" OnPageIndexChanging="gvTaxes_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
                    <asp:BoundField DataField="PercentageValue" HeaderText="<%$Resources:Labels,Tax %>" />
                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                OnClientClick="return ConfirmSure();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:ABFGridView>
        </div>
        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,RecipientName %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%">
                </asp:ABFTextBox>
            </div>
            <br />
            <br />
            <div class="form" style="width: 700px; margin: auto;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                        MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                        ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                        MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                        ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                        MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                        DataType="Decimal">
                    </asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <table class="totals">
                        <tr>
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.Total %>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="DiscountTotal">
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.TotalDiscount %>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblTotalDiscount" runat="server" Text="0.0000"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="taxTotal">
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.TotalTax %>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblTotalTax" runat="server" Text="0.0000"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.GrossTotal %>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblGrossTotal" runat="server" Text="0.0000"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <div class="InvoiceSection align_right">
            <div class="validationSummary">
                <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
            </div>
            <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                ValidationGroup="Save" OnClick="BtnSave_Click" />
            <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />
            <asp:Button runat="server" ID="btnPrintInventoryOrder" CssClass="button_big" Text="<%$ Resources:Labels,PrintInvOrder %>"
                Visible="false" OnClick="btnPrintOrderIn_Click" />
            <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                Visible="false" OnClick="btnPrint_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfNewBatch" runat="server" />

    <asp:NewBatch ID="ucNewBatchID" runat="server" Title="<%$Resources:Labels,BatchID %>"
        AttributeType_ID="14" TargetControlID="cph$lnkNewBatch" OnNewBatchCreated="ucNewBatchID_NewBatchCreated"></asp:NewBatch>
    <asp:HiddenField ID="hfFastAddNew" runat="server" />


    <asp:ucNewItemDescribed
        OnNewItemDescribedCreated="ucNewItemDescribed_OnNewItemDescribedCreated"
        TargetControlID="cph$lnkNewDescribed"
        Title="<%$Resources:Labels,Itemdescribed %>"
        runat="server" ID="ucNewItemDescribed" />
    <asp:HiddenField ID="hfNewDescribed" runat="server" />
    <asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="lnkAddNewVendor"
        PopupControlID="pnlFastAddNew" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>




    <asp:Panel ID="pnlFastAddNew" CssClass="pnlPopUp" runat="server"
        Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="CloseFastAddNewPopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtFastAddName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                    IsRequired="true" ValidationGroup="FastAddNew">
                </asp:ABFTextBox>
                <label>
                    <%=Resources.Labels.Currency %></label>
                <asp:DropDownList ID="ddlFastAddCurrency" runat="server">
                </asp:DropDownList>
                <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" ServiceMethod="GetChartOfAccounts"
                    LabelText="<%$Resources:Labels,ParentAccount %>" ValidationGroup="FastAddNew"></asp:AutoComplete>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnFastAddNew" CssClass="button default_button" runat="server" OnClick="btnFastAddNew_click"
                    ValidationGroup="FastAddNew" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelAddNew" runat="server" CssClass="button" OnClick="CloseFastAddNewPopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
