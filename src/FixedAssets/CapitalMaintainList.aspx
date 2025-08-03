<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CapitalMaintainList.aspx.cs" Inherits="FixedAssets_CapitalMaintainList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text=" <%$ Resources:Labels,AddNew %>"></asp:HyperLink>
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
                    <asp:ABFTextBox ID="txtDateFromSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        IsDateFiscalYearRestricted="false" DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="FilterAccounts"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterAccounts" AutoPostBack="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acParentAsset" ServiceMethod="GetAssets"
                        LabelText="<%$Resources:Labels,Asset %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtName" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Name %>">
                    </asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        IsDateFiscalYearRestricted="false" DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccounts"
                        LabelText="<%$Resources:Labels,OppositeAccount %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>

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
    <asp:ABFGridView runat="server" ID="gvAssetsList" GridViewStyle="GrayStyle" DataKeyNames="ID"
        OnPageIndexChanging="gvAssetsList_PageIndexChanging">
        <Columns>
              <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
            <asp:BoundField DataField="Serial" HeaderText="الرقم " />
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="PurchaseDate" HeaderText="<%$Resources:Labels,PurchaseDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="StartWorkDate" HeaderText="<%$Resources:Labels,StartWorkDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Cost %>" />
            <asp:BoundField DataField="ProductionAge" HeaderText="<%$Resources:Labels,ProductionAge %>" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("CapitalMaintain.aspx?ID={0}", Eval("ID")) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
</asp:Content>
