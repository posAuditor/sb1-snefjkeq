<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ProductionOrder.aspx.cs" Inherits="Production_ProductionOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            <div class="form" style="margin: auto; width: 90%;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acRawStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,RawStore %>"
                        IsRequired="true" ValidationGroup="Save" OnSelectedIndexChanged="acFinalItem_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acFinalItem" ServiceMethod="GetItems" OnSelectedIndexChanged="acFinalItem_SelectedIndexChanged"
                        IsRequired="true" AutoPostBack="true"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>

                    <asp:ABFTextBox ID="txtFinalItemQty" runat="server" LabelText="<%$Resources:Labels,Quantity %>" OnTextChanged="acFinalItem_SelectedIndexChanged" AutoPostBack="true"
                        MinValue="0.001" DataType="Decimal" IsRequired="true" ValidationGroup="Save"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                        OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkNewBatch" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>

                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                        Enabled="false" IsHideable="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                        Enabled="false" IsHideable="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                <div class="form" style="margin: auto; width: 90%;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,RawStore %>"
                            IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                            AutoPostBack="true"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                            OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" IsHideable="true" ></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                            LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                            ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                            OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                            LabelText="<%$Resources:Labels,ItemMaterial %>"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtQty" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                            MinValue="0.001" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"></asp:ABFTextBox>
                        <br />
                        <span>
                            <%=Resources.Labels.AvailableQty %>: </span>
                        <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div class="form" style="margin: auto; width: 600px;">
                    <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                        TextMode="MultiLine" Style="width: 100%;"></asp:ABFTextBox>
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
        <div class="InvoiceSection">
            <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
                <Columns>
                    <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Labels,RawStore %>" />
                    <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />
                    <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,ItemMaterial %>" />
                    <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
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

        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
            </div>
            <br />
            <br />
        </div>
        <div class="InvoiceSection align_right">
            <div class="validationSummary">
                <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
            </div>
            <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                ValidationGroup="Save" OnClick="BtnSave_Click" />
            <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />

               <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />
            

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
        AttributeType_ID="70" TargetControlID="cph$lnkNewBatch" OnNewBatchCreated="ucNewBatchID_NewBatchCreated"></asp:NewBatch>
</asp:Content>
