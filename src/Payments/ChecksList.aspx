<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ChecksList.aspx.cs" Inherits="Checks_ChecksList" %>

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
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtMatDateFrom" CssClass="field" LabelText="<%$Resources:Labels,MatDateFrom %>"
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>

                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server"  OnSelectedIndexChanged="FilterAccounts"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterAccounts" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acIssueBank" ServiceMethod="GetChartOfAccounts"
                        LabelText="<%$Resources:Labels,IssueBank %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.CheckStatus %></label>
                    <asp:DropDownList ID="ddlCheckStatus" runat="server" >
                        <asp:ListItem Text="<%$Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,UnderCollection %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PaidCollected %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Refused %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtMatDateTo" CssClass="field" LabelText="<%$Resources:Labels,MatDateTo %>"
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
                    <asp:AutoComplete runat="server" ID="acDepositAccountSrch" ServiceMethod="GetChartOfAccountsCashAndBanks"
                        LabelText="<%$Resources:Labels,DepositAccount %>"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acAccount" ServiceMethod="GetChartOfAccounts"
                        LabelText="<%$Resources:Labels,AccountName %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtBillNo" CssClass="field" LabelText="<%$Resources:Labels,BillNo %>"
                        runat="server"></asp:ABFTextBox>
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
    <asp:ABFGridView runat="server" ID="gvChecksList" GridViewStyle="GrayStyle" DataKeyNames="ID,Branch_ID,Currency_ID,IssueDate,Serial"
        OnSelectedIndexChanging="gvChecksList_SelectedIndexChanging" OnPageIndexChanging="gvChecksList_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
            <asp:BoundField DataField="IssueDate" HeaderText="<%$Resources:Labels,IssueDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="MaturityDate" HeaderText="<%$Resources:Labels,MaturityDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="TotalAmount" HeaderText="<%$Resources:Labels,Total %>" />
            <asp:BoundField DataField="FromBankName" HeaderText="<%$Resources:Labels,IssueBank %>" />
            <asp:BoundField DataField="AccountNames" HeaderText="" />
            <asp:BoundField DataField="ToBankName" HeaderText="<%$Resources:Labels,DepositAccount %>" />
            <asp:BoundField DataField="CeartedByName" HeaderText="المستخدم "/>
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="CheckStatusName" HeaderText="<%$Resources:Labels,CheckStatus %>" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="RealtedBillSerial" HeaderText="<%$Resources:Labels,BillNo %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("Checks.aspx/{1}?ID={0}", Eval("ID"),Eval("PathInfo") ) %>'
                        Text="<img src='../../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, CollectPay %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCollect" Text="<%$ Resources:Labels, CollectPay %>" runat="server"
                        OnClick="lnkCollect_Click" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2" && Eval("CheckStatus").ToString()=="0") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Refuse %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkRefuse" Text="<%$ Resources:Labels, Refuse %>" runat="server"
                        OnClick="lnkRefuse_Click" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2" && Eval("CheckStatus").ToString()=="0") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="btnPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfCollect" runat="server" />
    <asp:ModalPopupExtender ID="mpeCollectRefuse" runat="server" TargetControlID="hfCollect"
        PopupControlID="pnlCollectRefuse" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlCollectRefuse" CssClass="pnlPopUp" runat="server" Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <br />
                <span>
                    <%=Resources.Labels.Serial %></span>:
                <asp:Label ID="lblCheckSerial" Text="" runat="server"></asp:Label>
                <asp:ABFTextBox ID="txtCollectingOrRefuseDate" CssClass="field" runat="server" Width="250px"
                    IsRequired="true" LabelText="<%$Resources:Labels,Date %>" DataType="Date" ValidationGroup="Collect">
                </asp:ABFTextBox>
                <asp:AutoComplete runat="server" ID="acDepositAccount" ServiceMethod="GetChartOfAccountsCashAndBanks"
                    ValidationGroup="Collect" IsRequired="true" LabelText="<%$Resources:Labels,DepositAccount %>"></asp:AutoComplete>
                <asp:ABFTextBox ID="txtCollectingExpenses" runat="server" ValidationGroup="Collect"
                    LabelText="<%$Resources:Labels,Expenses %>" DataType="Decimal" OnTextChanged="txtCollectingExpenses_TextChanged"
                    AutoPostBack="true"></asp:ABFTextBox>
                <asp:AutoComplete runat="server" ID="acExpensesAccount" ServiceMethod="GetChartOfAccounts"
                    ValidationGroup="Collect" LabelText="<%$Resources:Labels,ExpensesAccount %>"></asp:AutoComplete>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Collect" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnOkCollect" CssClass="button default_button" runat="server" OnClick="btnOkCollect_click"
                    ValidationGroup="Collect" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelCollect" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hfmpeConfirm" runat="server" />
    <asp:ModalPopupExtender ID="mpeConfirm" runat="server" TargetControlID="hfmpeConfirm"
        PopupControlID="pnlConfirm" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeConfirm" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlConfirm" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClientClick="$find('mpeConfirm').hide(); return false;"></asp:Button>
            <span>
                <%=Resources.UserInfoMessages.DoContinue%></span>
        </div>
        <div class="content">
            <div style="max-height: 400px; overflow: auto;" class="pnlPopupMessage">
                <asp:Literal runat="server" ID="ltConfirmationMessage"></asp:Literal>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnYes" CssClass="button default_button" runat="server" OnClick="btnYes_click" Text="<%$ Resources:Labels, Yes %>" />
                <asp:Button ID="BtnNo" runat="server" CssClass="button" OnClientClick="$find('mpeConfirm').hide(); return false;"
                    Text="<%$ Resources:Labels, No %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
