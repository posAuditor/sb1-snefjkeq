<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PeriodicMaintainenceExpenses.aspx.cs" Inherits="FixedAssets_PeriodicMaintainenceExpenses" %>

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
    <input type="password" style="display: none;" />
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
        <div class="tcat">
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">

                <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server"></asp:ABFTextBox>
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
    <asp:ABFGridView runat="server" ID="gvExpenses" GridViewStyle="BlueStyle" DataKeyNames="ID,ExpenseName"
        OnRowDeleting="gvExpenses_RowDeleting" OnPageIndexChanging="gvExpenses_PageIndexChanging"
        OnSelectedIndexChanging="gvExpenses_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="ExpenseName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="AssetName" HeaderText="<%$Resources:Labels,Asset %>" />
            <asp:BoundField DataField="PeriodicFollowUp" HeaderText="<%$Resources:Labels,FollowUpEvery %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblPeriodType" Text='<%# Eval("PeriodType").ToString()=="0"? Resources.Labels.Day:Resources.Labels.Month %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
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
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">

            <div class="form">
                <asp:AutoComplete runat="server" ID="acAsset" ServiceMethod="GetAssets" ValidationGroup="AddNew"
                    LabelText="<%$Resources:Labels,Asset %>" IsRequired="true"></asp:AutoComplete>
                <asp:ABFTextBox ID="txtName" CssClass="field" runat="server" 
                    IsRequired="true" LabelText="<%$Resources:Labels,ExpenseName %>" ValidationGroup="AddNew">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtPeriodicFollowUp" CssClass="field" runat="server"
                    LabelText="<%$Resources:Labels,FollowUpEvery %>" DataType="Int" IsRequired="true"
                    ValidationGroup="AddNew">
                </asp:ABFTextBox>
                <label>
                    <%= Resources.Labels.PeriodType %>
                </label>
                <asp:DropDownList ID="ddlPeriodType" runat="server">
                    <asp:ListItem Text="<%$ Resources:Labels,Day %>" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Month %>" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="clear: both;">
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
