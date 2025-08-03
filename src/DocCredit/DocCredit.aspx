<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DocCredit.aspx.cs" Inherits="DocCredit_DocCredits" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" TagName="ucNewItemDescribed" Src="~/CustomControls/ucNewItemDescribed.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        @font-face {
            font-family: 'awesonne';
            src: url('../Fonts/NFont/font/awesonne.eot?30429');
            src: url('../Fonts/NFont/font/awesonne.eot?30429#iefix') format('embedded-opentype'), url('../Fonts/NFont/font/awesonne.woff?30429') format('woff'), url('../Fonts/NFont/font/awesonne.ttf?30429') format('truetype'), url('../Fonts/NFont/font/awesonne.svg?30429#awesonne') format('svg');
            font-weight: normal;
            font-style: normal;
        }

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
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
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
                    <asp:ABFTextBox ID="txtDocCreditName" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DocCreditName %>"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acVendor" ServiceMethod="GetContactNames" IsRequired="true"
                        OnSelectedIndexChanged="acVendor_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>

                    <asp:Panel ID="Panel1" runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>" Enabled="false"
                        DataType="Decimal" IsRequired="true" MinValue="0.0001"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCoverValue" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,CoverValue %>"
                        DataType="Decimal" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acBank" ServiceMethod="GetChartOfAccounts" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,AccountName %>" IsRequired="true"></asp:AutoComplete>
                    <asp:Panel ID="Panel2" runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>

        <div class="InvoiceSection">
            <div class="container">
                <table class="table table-hover" style="border: 0 solid black;">
                    <tbody>
                        <tr>
                            <td>
                                <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                                    IsRequired="true" ValidationGroup="AddItem"></asp:AutoComplete>


                            </td>
                            <td style="width: 11%">


                                <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                                    OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" IsHideable="true"></asp:ABFTextBox>


                            </td>
                            <td style="width: 7%">
                                <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                    OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true" IsHideable="true">
                                </asp:ABFTextBox>
                            </td>
                            <td>
                                <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                                    OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                    LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>
                            </td>
                            <td>
                                <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>
                            </td>
                            <td style="width: 7%">
                                <asp:Label runat="server" Text="<%$Resources:Labels,Quantity %>"></asp:Label>
                                <br />

                                <asp:ABFTextBox ID="txtQty" VisibleText="False" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                                    DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" MinValue="0.001"></asp:ABFTextBox>

                            </td>
                            <td style="width: 12%">
                                <asp:Label runat="server" Text="<%$Resources:Labels,Price %>"></asp:Label>
                                <br />
                                <asp:ABFTextBox ID="txtCost" VisibleText="False" runat="server" LabelText="<%$Resources:Labels,Cost %>"
                                    MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"></asp:ABFTextBox>
                            </td>
                            <td>


                                <div class="btnDiv" style="padding-top: 24px;">
                                    <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" ValidationGroup="AddItem" runat="server"><i class="demo-icon icon-plus" style="color: white; font-size: 20px; padding-top: 10px;"></i></asp:LinkButton>

                                </div>


                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel runat="server" ID="pnlItemdescribed">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:AutoComplete runat="server" ID="acItemDescribed" ServiceMethod="GetItemsDescribed"
                                                    AutoPostBack="true" ValidationGroup="AddItem"
                                                    LabelText="<%$Resources:Labels,Itemdescribed %>"></asp:AutoComplete>

                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkNewDescribed" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>

                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                            </td>
                            <td>
                                <asp:ABFTextBox ID="txtCapacity" runat="server" LabelText="<%$Resources:Labels,Capacity %>"
                                    MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                                </asp:ABFTextBox>
                            </td>
                            <td colspan="2">

                                <table class="table table-hover forHide" style="border: 0 solid black;">
                                    <tr>
                                        <td style="width: 80px;">
                                            <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,CapacityDistribution %>"></asp:Label></td>
                                        <td>
                                            <span><%=Resources.Labels.ActualQuantity %> : </span>
                                            <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>
                                    </tr>
                                    <tr>


                                        <td colspan="2">

                                            <asp:ABFTextBox ID="txtCapacities" OnTextChanged="txtCapacities_OnTextChanged" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                                                ValidationGroup="AddItem" AutoPostBack="true" IsHideable="true" VisibleText="False"></asp:ABFTextBox>

                                        </td>
                                    </tr>
                                </table>


                            </td>
                            <td>

                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Policy %>"></asp:Label>

                                <asp:ABFTextBox ID="txtPolicy" IsRequired="True" runat="server" VisibleText="False"
                                    MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Policy %>" IsHideable="true"></asp:ABFTextBox>


                            </td>
                            <td>

                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Code %>"></asp:Label>

                                <asp:ABFTextBox ID="txtCode" IsRequired="True" runat="server"
                                    MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Code %>" VisibleText="False" IsHideable="true"></asp:ABFTextBox>


                            </td>
                            <td>
                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,DateRequest %>"></asp:Label>
                                <asp:ABFTextBox ID="txtInvoiceDate" runat="server" VisibleText="False" LabelText="<%$Resources:Labels,DateRequest %>" ValidationGroup="AddItem"
                                    DataType="Date" IsRequired="true" IsHideable="true"></asp:ABFTextBox>

                            </td>
                            <td></td>

                        </tr>
                    </tbody>
                </table>

                <div style="clear: both">
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                </div>

                <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
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
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="TaxPercentageValue" HeaderText="<%$Resources:Labels,Tax %>" ItemStyle-CssClass="TaxCol"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                            DataFormatString="{0:0.####}" />
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
        </div>

        <div class="InvoiceSection" style="display: none;">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">

                        <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                            LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                            ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                            IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                            AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                            OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkNewBatch" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    </div>
                    <div class="left_col">

                        <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                            TextMode="MultiLine"></asp:ABFTextBox>
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
                    <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearItem_Click" />
                </div>
            </asp:Panel>
        </div>
        <div class="InvoiceSection" style="display: none;">
        </div>
        <div class="InvoiceSection" id="taxSection" runat="server" style="display: none;">
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
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
            </div>
            <br />
            <br />
            <div class="form" style="width: 700px; margin: auto;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                        MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                        ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                        MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                        ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtReceivingDate" runat="server" LabelText="<%$Resources:Labels,ReceivingDate %>"
                        ValidationGroup="Receive" DataType="Date" IsRequired="true"></asp:ABFTextBox>
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

            <asp:Button runat="server" ID="btnReceive" Text="<%$ Resources:Labels,Receive %>" CssClass="button_big" OnClientClick="return ConfirmSureWithValidation('Receive')"
                ValidationGroup="Receive" OnClick="btnReceive_Click" />
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
    
    
    <asp:ucNewItemDescribed
        OnNewItemDescribedCreated="ucNewItemDescribed_OnNewItemDescribedCreated"
        TargetControlID="cph$lnkNewDescribed"
        Title="<%$Resources:Labels,Itemdescribed %>"
        runat="server" ID="ucNewItemDescribed" />
    <asp:HiddenField ID="hfNewDescribed" runat="server" />
   

    <asp:HiddenField ID="hfNewBatch" runat="server" />
    <asp:NewBatch ID="ucNewBatchID" runat="server" Title="<%$Resources:Labels,BatchID %>"
        AttributeType_ID="14" TargetControlID="cph$lnkNewBatch" OnNewBatchCreated="ucNewBatchID_NewBatchCreated"></asp:NewBatch>
</asp:Content>
