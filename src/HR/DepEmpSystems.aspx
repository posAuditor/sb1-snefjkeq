<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DepEmpSystems.aspx.cs" Inherits="HR_DepEmpSystems" %>

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
                <div class="right_col">
                    <asp:AutoComplete runat="server" ID="acEmployeeSrch" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,Employee %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acSystemSrch" ServiceMethod="GetHRSystems" LabelText="<%$Resources:Labels,System %>">
                    </asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acDepartmentSrch" ServiceMethod="GetHRDepartments"
                        LabelText="<%$Resources:Labels,Department %>"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" ValidationGroup="search" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvSystems" GridViewStyle="BlueStyle" DataKeyNames="ID,SystemName,SystemType_ID"
        OnRowDeleting="gvSystems_RowDeleting" OnPageIndexChanging="gvSystems_PageIndexChanging"
        OnSelectedIndexChanging="gvSystems_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Employee %>" />
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="SystemName" HeaderText="<%$Resources:Labels,System %>" />
         
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
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="580">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click">
            </asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList ID="ddlType" runat="server"  OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Department %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Employee %>" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acEmployeeOrDepartment" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,EmployeeOrDepartment %>" IsRequired="true" ValidationGroup="AddNew">
                    </asp:AutoComplete>
                </div>
                <div class="left_col">
                    <label>
                        <%=Resources.Labels.SystemType %></label>
                    <asp:DropDownList ID="ddlSystemType" runat="server"  OnSelectedIndexChanged="ddlSystemType_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Absence %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,AttendanceIncentives %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Allowances %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,OverTime %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Delay %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Excuse %>" Value="5"></asp:ListItem>
                          <asp:ListItem Text="<%$ Resources:Labels,GeneralExpenditure %>" Value="6"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acSystem" ServiceMethod="GetHRSystems" LabelText="<%$Resources:Labels,System %>"
                        IsRequired="true" ValidationGroup="AddNew"></asp:AutoComplete>
                </div>
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
