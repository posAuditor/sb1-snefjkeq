<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PayrollPayment.aspx.cs" Inherits="HR_PayrollPayment" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Import Namespace="XPRESS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="false" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
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
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" IsRequired="true" ValidationGroup="Search"
                        OnSelectedIndexChanged="FilterEmployees" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete ID="acDepartment" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>"
                        runat="server" IsRequired="true" ValidationGroup="NewEmployee" OnSelectedIndexChanged="FilterEmployees"
                        AutoPostBack="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acName" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>" ValidationGroup="Search"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <div class="form" style="width: 550px;">
        <div class="right_col">
            <asp:AutoComplete runat="server" ID="acCreditAccount" ServiceMethod="GetChartOfAccountsCashAndBanks"
                LabelText="<%$Resources:Labels,CreditAccount %>" ValidationGroup="Approve" IsRequired="true"></asp:AutoComplete>
        </div>
        <div class="left_col">
            <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                runat="server" ValidationGroup="Approve" DataType="Date" IsRequired="true"></asp:ABFTextBox>
        </div>
        <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
            runat="server" TextMode="MultiLine" Height="50" Width="542"></asp:ABFTextBox>
    </div>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvPayRoll" GridViewStyle="BlueStyle" DataKeyNames="ChartOfAccount_ID,Salary,Branch_ID,ContactName"
        OnPageIndexChanging="gvPayRoll_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Pay %>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkPay" runat="server" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkApproveAll" runat="server" Text="<%$ Resources:Labels, Pay %>"
                        onchange="SelectAll();" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Salary" HeaderText="<%$Resources:Labels,Salary %>" DataFormatString="{0:0.####}" />
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:ABFTextBox runat="server" ID="txtPaidAmount" Text='<%# Eval("Salary").ToExpressString() %>'></asp:ABFTextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <br />
    <br />
    <div class="align_right">
        <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
            ValidationGroup="Approve" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Approve');" />
    </div>
    <script type="text/javascript">

        function SelectAll() {
            $("input[type=checkbox]:enabled").each(function (index, obj) {
                if (obj.id != "cph_gvPayRoll_chkApproveAll") {
                    obj.checked = document.getElementById("cph_gvPayRoll_chkApproveAll").checked;
                }
            });

        }
    </script>
</asp:Content>
