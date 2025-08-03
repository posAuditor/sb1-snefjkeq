<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Assets.aspx.cs" Inherits="FixedAssets_Assets" %>

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
                    <asp:AutoComplete runat="server" ID="acAssetsCategories" ServiceMethod="GetAssetsCateogries"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Category %>" IsRequired="true"
                        OnSelectedIndexChanged="acCategory_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtName" runat="server" ValidationGroup="Save" IsRequired="true"
                        LabelText="<%$Resources:Labels,Name %>"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtPurchaseDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,PurchaseDate %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsDateFiscalYearRestricted="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCost" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Cost %>"
                        DataType="Decimal" IsRequired="true" MinValue="1" OnTextChanged="CalculateDepValue"
                        AutoPostBack="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccountsException"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,OppositeAccount %>" IsRequired="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" IsRequired="true" MinValue="0.0001"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtScrapValue" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,ScrapValue %>"
                        DataType="Decimal" IsRequired="true" MinValue="0" OnTextChanged="CalculateDepValue"
                        AutoPostBack="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtProductionAge" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,ProductionAge %>"
                        DataType="Int" IsRequired="true" MinValue="1" OnTextChanged="CalculateDepValue"
                        AutoPostBack="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtStartWorkDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,StartWorkDate %>"
                        DataType="Date" IsRequired="true" OnTextChanged="CalculateDepValue" AutoPostBack="true"
                        IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDepRate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DepRatio %>"
                        DataType="Decimal" IsRequired="true" MinValue="0.001" Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDepValue" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DepValue %>"
                        DataType="Decimal" IsRequired="true" MinValue="0" Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDepDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DepDate %>"
                        DataType="Date" IsRequired="true" Enabled="false" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                     <br /><br />
                    <asp:CheckBox ID="chkNotDepAuto" runat="server" /><span>إهلاك غير اوتوماتيكي</span>
                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
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
              <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />
          
            
            
              <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
            </div>
        </div>
    </div>
</asp:Content>
