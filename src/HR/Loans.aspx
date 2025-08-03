<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Loans.aspx.cs" Inherits="HR_Loans" %>

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
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterEmployees" AutoPostBack="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtFromDateSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtToDateSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox><br />
                    <asp:CheckBox ID="chkIsFinished" runat="server" Text="" /><span><%=Resources.Labels.Finished %></span>
                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acEmployeeSrch" ServiceMethod="GetEmployeesNames"
                        LabelText="<%$Resources:Labels,Employee %>" ValidationGroup="search"></asp:AutoComplete>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatusSrch" runat="server" >
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
    <asp:ABFGridView runat="server" ID="gvLoans" GridViewStyle="BlueStyle" DataKeyNames="ID,ContactName"
        OnRowDeleting="gvLoans_RowDeleting" OnPageIndexChanging="gvLoans_PageIndexChanging"
        OnSelectedIndexChanging="gvLoans_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Value" HeaderText="<%$Resources:Labels,Value %>" />
            <asp:BoundField DataField="InstallmentsNumber" HeaderText="<%$Resources:Labels,InstallmentsNumber %>" />
            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="StartDate" HeaderText="<%$Resources:Labels,StartDate %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Finished %>">
                <ItemTemplate>
                    <asp:LinkButton runat="server" Text="<%$ Resources:Labels, Finished %>" Visible='<%# !Convert.ToBoolean( Eval("IsFinished"))  && Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>'  OnClick="lnkFinihed_CheckedChanged" OnClientClick="return ConfirmSure();"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
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
            <asp:TemplateField HeaderText="الغاء الاعتماد">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" 
                       OnClick="Unnamed_Click" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>' />
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
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                        runat="server" ValidationGroup="AddNew" DataType="Date" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtStartDate" CssClass="field" LabelText="<%$Resources:Labels,StartDate %>"
                        runat="server" ValidationGroup="AddNew" DataType="Date" IsRequired="true" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtValue" CssClass="field" LabelText="<%$Resources:Labels,Value %>"
                        runat="server" ValidationGroup="AddNew" DataType="Decimal" MinValue="1" IsRequired="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acEmployee" ServiceMethod="GetEmployeesNames"
                        LabelText="<%$Resources:Labels,Employee %>" ValidationGroup="AddNew" IsRequired="true" OnSelectedIndexChanged="acEmployee_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acCreditAccount" ServiceMethod="GetChartOfAccountsCashAndBanks"
                        LabelText="<%$Resources:Labels,CreditAccount %>" ValidationGroup="AddNew" IsRequired="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtInstallemntsNumber" CssClass="field" LabelText="<%$Resources:Labels,InstallmentsNumber %>"
                        runat="server" ValidationGroup="AddNew" DataType="Int" MinValue="1" IsRequired="true"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                      <asp:AutoComplete runat="server" ID="acBranchEmp" ValidationGroup="AddNew" IsRequired="true" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                         ></asp:AutoComplete>

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
