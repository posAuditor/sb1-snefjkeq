<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UnderRequestEmployeesList.aspx.cs" Inherits="HR_UnderRequestEmployeesList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
        <a runat="server" id="lnkadd" href="UnderRequestEmployees.aspx" class="collapse_add_link">
        <%= Resources.Labels.AddNew %></a>
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
                    <asp:AutoComplete ID="acPositionSrch" ServiceMethod="GetHRPositions" LabelText="<%$Resources:Labels,Position %>"
                        runat="server"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateFrom" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        runat="server" DataType="Date" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDateTo" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        runat="server" DataType="Date" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
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
    <asp:ABFGridView runat="server" ID="gvEmployees" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvEmployees_RowDeleting" OnPageIndexChanging="gvEmployees_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="PositionName" HeaderText="<%$Resources:Labels,Position %>" />
            <asp:BoundField DataField="TestDate" HeaderText="<%$Resources:Labels,TestDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="EmploymentStatus" HeaderText="<%$Resources:Labels,EmploymentStatus %>" />
            <asp:HyperLinkField DataNavigateUrlFields="ID" Text="<%$Resources:Labels,ToEmployee %>" DataNavigateUrlFormatString="Employees.aspx?UREmployee_ID={0}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("UnderRequestEmployees.aspx?ID={0}", Eval("ID") ) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
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

</asp:Content>
