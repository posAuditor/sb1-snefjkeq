<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PeriodicMaintain.aspx.cs" Inherits="FixedAssets_PeriodicMaintain" %>

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
                    <asp:AutoComplete runat="server" ID="acParentAsset" ServiceMethod="GetAssets" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,Asset %>" IsRequired="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acExpenseName" ServiceMethod="GetPeriodicMaintenanceExpenses"
                        LabelText="<%$Resources:Labels,ExpenseName %>" IsRequired="true" ValidationGroup="Save"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkAddNewExpense" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" IsRequired="true" Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCost" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Amount %>"
                        DataType="Decimal" IsRequired="true" MinValue="1"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,OppositeAccount %>" IsRequired="true"></asp:AutoComplete>
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
            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
            </div>
        </div>
    </div>
    <asp:ModalPopupExtender ID="mpeCreateExpense" runat="server" TargetControlID="lnkAddNewExpense"
        PopupControlID="pnlAddNewEX" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAddNewEX" CssClass="pnlPopUp" runat="server"
        Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtExpenseName" CssClass="field" runat="server" Width="250px"
                    IsRequired="true" LabelText="<%$Resources:Labels,ExpenseName %>" ValidationGroup="SaveExpense">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtPeriodicFollowUp" CssClass="field" runat="server" Width="250px"
                    LabelText="<%$Resources:Labels,FollowUpEvery %>" DataType="Int" IsRequired="true"
                    ValidationGroup="SaveExpense">
                </asp:ABFTextBox>
                <label>
                    <%= Resources.Labels.PeriodType %>
                </label>
                <asp:DropDownList ID="ddlPeriodType" runat="server">
                    <asp:ListItem Text="<%$ Resources:Labels,Day %>" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Month %>" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="SaveExpense" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveEX" CssClass="button default_button" runat="server" OnClick="btnSaveEX_click"
                    ValidationGroup="SaveExpense" Text="<%$ Resources:Labels, Save %>" />
                <asp:Button ID="BtnClearEX" runat="server" CssClass="button" OnClick="BtnClearEX_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
