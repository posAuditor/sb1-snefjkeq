<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Stores.aspx.cs" Inherits="Inv_Stores" %>

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
                    <asp:AutoComplete ID="acBranchSrch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" OnSelectedIndexChanged="acBranchSrch_SelectedIndexChanged" AutoPostBack="true"
                        runat="server"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:AutoComplete ID="acEmployeeNameSrch" ServiceMethod="GetContactNames" LabelText="<%$Resources:Labels,Employee %>"
                        runat="server"></asp:AutoComplete>
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
    <asp:ABFGridView runat="server" ID="gvStores" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvStores_RowDeleting" OnPageIndexChanging="gvStores_PageIndexChanging"
        OnSelectedIndexChanging="gvStores_SelectedIndexChanging">
        <Columns>
             <asp:TemplateField HeaderText="التسلسل">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="EmployeeName" HeaderText="<%$Resources:Labels,Employee %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# !Convert.ToBoolean(Eval("IsSystem")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="350">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                <asp:AutoComplete ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" ValidationGroup="AddNew" OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true"
                    runat="server"></asp:AutoComplete>
                <asp:AutoComplete ID="acEmployeeName" ServiceMethod="GetContactNames" LabelText="<%$Resources:Labels,Employee %>"
                    runat="server"></asp:AutoComplete>
                <asp:ABFTextBox ID="txtAddress" CssClass="field" LabelText="<%$Resources:Labels,Address %>"
                    runat="server" ValidationGroup="AddNew" TextMode="MultiLine" Height="100"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                    runat="server" ValidationGroup="AddNew" TextMode="MultiLine" Height="100"></asp:ABFTextBox>
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
