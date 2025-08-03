<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="FixedAssets_Categories" %>

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
            <%=Resources.Labels.Search %></div>
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
    <asp:ABFGridView runat="server" ID="gvCategories" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvCategories_RowDeleting" OnPageIndexChanging="gvCategories_PageIndexChanging"
        OnSelectedIndexChanging="gvCategories_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# !Convert.ToBoolean(Eval("IsSystem")) && Convert.ToBoolean(Eval("IsDeletable")) %>' />
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
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click">
            </asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                <label>
                    <%=Resources.Labels.Dep%></label>
                <asp:DropDownList ID="ddlDep" runat="server"  OnSelectedIndexChanged="ddlDep_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Text="<%$Resources:Labels,NoDep%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,FixedDep%>" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <label>
                    <%=Resources.Labels.ReDep%></label>
                <asp:DropDownList ID="ddlReDep" runat="server"  Enabled="false">
                    <asp:ListItem Text="<%$Resources:Labels,NoReDep%>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,CloseTheAsset%>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,SetValueToOnePound%>" Value="2"></asp:ListItem>
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
