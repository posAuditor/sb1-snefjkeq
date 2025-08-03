<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Banks.aspx.cs" Inherits="Accounting_Banks" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
        AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
        ExpandDirection="Vertical" SuppressPostBack="true">
    </asp:CollapsiblePanelExtender>
    <div style="clear: both;">
    </div>
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
        <div class="tcat">
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:AutoComplete ID="acBranchSrch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        runat="server"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtAccountNumberSrch" CssClass="field" LabelText="<%$Resources:Labels,AccountNumber %>"
                        runat="server"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrencySrch" runat="server" >
                    </asp:DropDownList>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvBanks" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvBanks_RowDeleting" OnPageIndexChanging="gvBanks_PageIndexChanging"
        OnSelectedIndexChanging="gvBanks_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="AccountNumber" HeaderText="<%$Resources:Labels,AccountNumber %>" />
            <asp:BoundField DataField="OpenBalance" HeaderText="<%$Resources:Labels,OpenBalance %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# Convert.ToBoolean(Eval("IsDeletable")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="580">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col" style="width: 250px;">
                    <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                    <asp:AutoComplete ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true"
                        runat="server" ValidationGroup="AddNew"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acParentAccount" ServiceMethod="GetChartOfAccountsOld"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,ParentAccount %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtAccountNumber" CssClass="field" LabelText="<%$Resources:Labels,AccountNumber %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtTelephone" CssClass="field" LabelText="<%$Resources:Labels,Telephone %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAddress" CssClass="field" LabelText="<%$Resources:Labels,Address %>"
                        runat="server" ValidationGroup="AddNew" TextMode="MultiLine" Height="100"></asp:ABFTextBox>
                </div>
                <div class="left_col" style="width: 250px;">
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server"  OnSelectedIndexChanged="txtStartFrom_TextChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="AddNew" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="AddNew" LabelText="<%$Resources:Labels,OpenBalance %>"
                        OnTextChanged="txtOpenBalance_TextChanged" AutoPostBack="true" DataType="Decimal"
                        MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="AddNew" LabelText="<%$Resources:Labels,StartFrom %>"
                        OnTextChanged="txtStartFrom_TextChanged" AutoPostBack="true" DataType="Date"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsOld"
                        LabelText="<%$Resources:Labels,OppositeAccount %>" Enabled="false"></asp:AutoComplete>
                </div>
                <div style="clear: both;">
                </div>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
