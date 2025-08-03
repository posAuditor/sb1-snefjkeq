<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="MachineAttendance.aspx.cs" Inherits="HR_MachineAttendance" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkSync" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,SyncWithMachine %>"
        OnClick="lnkSync_Click" OnClientClick="return ConfirmSure();"></asp:LinkButton>
    <asp:DropDownList ID="ddlMachines" runat="server" Width="250">
    </asp:DropDownList>
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
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterEmployees" AutoPostBack="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtMachineIDSrch" CssClass="field" LabelText="<%$Resources:Labels,MachineID %>"
                        runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtFromDateSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        IsDateFiscalYearRestricted="false" runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtToDateSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        IsDateFiscalYearRestricted="false" runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:AutoComplete ID="acDepartmentSrch" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>"
                        OnSelectedIndexChanged="FilterEmployees" AutoPostBack="true"
                        runat="server"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acEmployeeSrch" ServiceMethod="GetEmployeesNames"
                        LabelText="<%$Resources:Labels,Employee %>" ValidationGroup="search"></asp:AutoComplete>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatusSrch" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
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
    <asp:ABFGridView runat="server" ID="gvAttendance" GridViewStyle="BlueStyle" DataKeyNames="ID,ContactName"
        OnRowDeleting="gvAttendance_RowDeleting" OnPageIndexChanging="gvAttendance_PageIndexChanging"
        OnSelectedIndexChanging="gvAttendance_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="MachineID" HeaderText="<%$Resources:Labels,MachineID %>" />
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Employee %>" />
            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="Time" HeaderText="<%$Resources:Labels,Time %>" DataFormatString="{0:hh:mm tt}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select"
                        Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <br />
    <br />
    <div class="align_right">
        <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels, Approve %>" CssClass="button_big shortcut_approve"
            OnClick="BtnApprove_Click" OnClientClick="return ConfirmSure();" />
    </div>
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
                <asp:AutoComplete runat="server" ID="acEmployee" ServiceMethod="GetContactNames"
                    LabelText="<%$Resources:Labels,Employee %>" IsRequired="true" ValidationGroup="AddNew"></asp:AutoComplete>
                <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                    IsDateFiscalYearRestricted="false" runat="server" IsRequired="true" ValidationGroup="AddNew"
                    DataType="Date"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtTime" CssClass="field" LabelText="<%$Resources:Labels,Time %>"
                    Width="155" runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Time"></asp:ABFTextBox>
                <asp:DropDownList ID="ddlTime" runat="server" Width="50" Style="display: inline;">
                    <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                </asp:DropDownList>
                <label>
                    <%=Resources.Labels.Status %></label>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                </asp:DropDownList>
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
    </asp:Panel>
</asp:Content>
