<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="JournalEntriesGeneralList.aspx.cs" Inherits="Accounting_JournalEntriesGeneralList" %>


<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text=" <%$ Resources:Labels,AddNew %>"
        NavigateUrl="~/Accounting/journalEntry.aspx"></asp:HyperLink>
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
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="رقم تسلسل القيد العام">
                    </asp:ABFTextBox>
                     <asp:ABFTextBox ID="txtSerialsrchNG" CssClass="field" runat="server" LabelText="رقم تسلسل المستند">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label style="display:none;">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList  style="display:none;" ID="ddlCurrency" runat="server">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"></asp:AutoComplete>
                     
                    <asp:AutoComplete runat="server" ID="acDocNameSrch" ServiceMethod="GetOperationTypes"
                                    AutoCompleteWidth="250" InlineRender="true" LabelText="نوع المستند"></asp:AutoComplete>
                     <asp:AutoComplete runat="server" ID="acAccount" ServiceMethod="GetChartOfAccountsGroupUser" LabelText="<%$Resources:Labels,AccountName %>"
                        IsRequired="true"  ></asp:AutoComplete>
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
    <asp:ABFGridView runat="server" ID="gvJournalEntriesList" GridViewStyle="GrayStyle"
        DataKeyNames="ID,Branch_ID" OnSelectedIndexChanging="gvJournalEntriesList_SelectedIndexChanging"
        OnPageIndexChanging="gvJournalEntriesList_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Serial" HeaderText="تسلسل القيد العام" />
            <asp:BoundField DataField="PostedDate" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="ForeignAmount" HeaderText="<%$Resources:Labels,Amount %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />

            <asp:BoundField DataField="SerialExt" HeaderText=" تسلسل مستند" />
            <asp:BoundField DataField="Description" HeaderText="نوع القيد" />

            <asp:TemplateField HeaderText="ألغاء الاعتماد">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/delete_grid.gif" Visible='<%#IsVisible(Eval("OperationType_ID").ToString(),int.Parse(Eval("DocStatus_ID").ToString())) %>' OnClientClick="return ConfirmSureWithValidation('Save')" runat="server" OnClick="btnCancel_Click" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:ABFGridView>
</asp:Content>


