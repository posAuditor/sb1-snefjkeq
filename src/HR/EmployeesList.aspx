<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="EmployeesList.aspx.cs" Inherits="HR_EmployeesList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <a runat="server" id="lnkadd" href="Employees.aspx" class="collapse_add_link">
        <%= Resources.Labels.AddNew %></a>
    <asp:Favorit runat="server" ID="ucFavorit" />

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
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtNationalIDSearsh" runat="server" LabelText="<%$Resources:Labels,NationalID %>"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlTerminationStatus" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Terminated %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:AutoComplete ID="acDepartment" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>"
                        runat="server" IsRequired="true" ValidationGroup="NewEmployee" OnSelectedIndexChanged="acDepartment_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acPosition" ServiceMethod="GetHRPositions" LabelText="<%$Resources:Labels,Position %>"
                        IsRequired="true" ValidationGroup="NewEmployee" Enabled="false"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acName" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"
                        KeepTextWhenNoValue="true"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    ValidationGroup="search" OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvEmployeesList" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvEmployeesList_RowDeleting" OnPageIndexChanging="gvEmployeesList_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,CareerNumber %>" />
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="PositionName" HeaderText="<%$Resources:Labels,Position %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:TemplateField HeaderText="حالة الموظف">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkChangeState"  CommandArgument='<%# Eval("ID") %>' OnClick="lnkChangeState_Click" runat="server"><%# this.SetNameStoped(Eval("IsStoped").ToString() )%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Activities %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("EmployeesActivites.aspx?ID={0}", Eval("ID") ) %>' Visible='<%# !(Eval("Branch_ID").ToString()==string.Empty && MyContext.UserProfile.Branch_ID!=null) %>'
                        Text="<%$ Resources:Labels, Activities %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("Employees.aspx?ID={0}", Eval("ID") ) %>' Visible='<%# !(Eval("Branch_ID").ToString()==string.Empty && MyContext.UserProfile.Branch_ID!=null) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server" Visible='<%# !(Eval("Branch_ID").ToString()==string.Empty && MyContext.UserProfile.Branch_ID!=null) %>'
                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
</asp:Content>
