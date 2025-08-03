<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DocCreditList.aspx.cs" Inherits="DocCredit_DocCreditList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:HyperLink>
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
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server"  OnSelectedIndexChanged="FilterVendors"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterVendors" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acVendorName" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acBank" ServiceMethod="GetChartOfAccounts" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,Bank %>" IsRequired="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtDocCreditName" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DocCreditName %>"
                        IsRequired="true"></asp:ABFTextBox>
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
    <asp:ABFGridView runat="server" ID="gvReceiptsList" GridViewStyle="GrayStyle" DataKeyNames="ID,Branch_ID"
        OnSelectedIndexChanging="gvReceiptsList_SelectedIndexChanging" OnPageIndexChanging="gvReceiptsList_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,NbInvoice %>" />
            <asp:BoundField DataField="OperationDate" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="BankName" HeaderText="<%$Resources:Labels,Bank %>" />
            <asp:BoundField DataField="GrossTotalAmount" HeaderText="<%$Resources:Labels,GrossTotal %>" />
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Vendor %>" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="OpenStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="ReceiveStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("DocCredit.aspx?ID={0}", Eval("ID") ) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="btnPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Expenses %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("Expenses.aspx?Receipt_ID={0}", Eval("ID") ) %>'
                        Text="<%$ Resources:Labels, Expenses %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Installments %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# String.Format("Installments.aspx?Receipt_ID={0}", Eval("ID") ) %>'
                        Text="<%$ Resources:Labels, Installments %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Close %>">
                <ItemTemplate>
                    <asp:LinkButton runat="server" OnClick="lnkCLose_Click" Text="<%$ Resources:Labels, Close %>" OnClientClick="return ConfirmSure();"
                        Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") && !Convert.ToBoolean(Eval("IsClosed"))%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
</asp:Content>
