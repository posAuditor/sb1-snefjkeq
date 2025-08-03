<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Payroll2.aspx.cs" Inherits="HR_Payroll2" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
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
                    <asp:AutoComplete runat="server" ID="acName" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <label>
                        <%=Resources.Labels.Month %></label>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,January %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,February %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,March %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,April %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,May %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,June %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,July %>" Value="7"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,August %>" Value="8"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,September %>" Value="9"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,October %>" Value="10"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,November %>" Value="11"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,December %>" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                    <label>
                        <%=Resources.Labels.Year %></label>
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Calculate %>" ValidationGroup="Search"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <div class="form" style="width: 550px;">
        <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
            runat="server" ValidationGroup="Approve" DataType="Date" IsRequired="true"></asp:ABFTextBox>
    </div>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvPayRoll" GridViewStyle="BlueStyle" DataKeyNames="ID"
        OnPageIndexChanging="gvPayRoll_PageIndexChanging" OnSelectedIndexChanging="gvPayRoll_SelectedIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Approve %>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkApprove" runat="server" Checked='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>'
                        Enabled='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1")%>' />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox ID="chkApproveAll" runat="server" Text="<%$ Resources:Labels, Approve %>"
                        onchange="SelectAll();" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Month" HeaderText="<%$Resources:Labels,Month %>" />
            <asp:BoundField DataField="Year" HeaderText="<%$Resources:Labels,Year %>" />
            <asp:BoundField DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                DataFormatString="{0:0.####}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <br />
    <br />
    <div class="align_right">
        <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve" ValidationGroup="Approve" OnClientClick="return ConfirmSureWithValidation('Approve');"
            OnClick="BtnApprove_Click" />
    </div>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="hfmpeCreateNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <asp:Label runat="server" ID="lblHeader" Text=""></asp:Label></span>
        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtOverTime" CssClass="field" LabelText="<%$Resources:Labels,OverTime %>"
                        OnTextChanged="txtOverTime_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                        ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDayOffWork" CssClass="field" LabelText="<%$Resources:Labels,DayOffWork %>"
                        OnTextChanged="txtDayOffWork_TextChanged" AutoPostBack="true" runat="server"
                        IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDelay" CssClass="field" LabelText="<%$Resources:Labels,Delay %>"
                        OnTextChanged="txtDelay_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                        ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAbsence" CssClass="field" LabelText="<%$Resources:Labels,Absence %>"
                        OnTextChanged="txtAbsence_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                        ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtExcuse" CssClass="field" LabelText="<%$Resources:Labels,Excuse %>"
                        OnTextChanged="txtExcuse_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                        ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDeducatedWorkDays" CssClass="field" LabelText="<%$Resources:Labels,DeducatedWorkDays %>"
                        OnTextChanged="txtDeducatedWorkDays_TextChanged" AutoPostBack="true" runat="server"
                        IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDeducatedDaysOff" CssClass="field" LabelText="<%$Resources:Labels,DeducatedDaysOff %>"
                        OnTextChanged="txtDeducatedDaysOff_TextChanged" AutoPostBack="true" runat="server"
                        IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtIncentives" CssClass="field" LabelText="<%$Resources:Labels,Incentives %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtTaxes" CssClass="field" LabelText="<%$Resources:Labels,Taxes %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtLoans" CssClass="field" LabelText="<%$Resources:Labels,HRLoan %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOtherAdditions" CssClass="field" LabelText="مكافئات - زيادات أخرى"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtOverTimeValue" CssClass="field" LabelText="<%$Resources:Labels,OverTimeValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDayOffWorkValue" CssClass="field" LabelText="<%$Resources:Labels,DayOffWorkValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDelayValue" CssClass="field" LabelText="<%$Resources:Labels,DelayValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAbsenceValue" CssClass="field" LabelText="<%$Resources:Labels,AbsenceValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtExcuseValue" CssClass="field" LabelText="<%$Resources:Labels,ExcuseValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDeducatedWorkDaysValue" CssClass="field" LabelText="<%$Resources:Labels,DeducatedWorkDaysValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDeducatedDaysOffValue" CssClass="field" LabelText="<%$Resources:Labels,DeducatedDaysOffValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAllowances" Enabled="False" CssClass="field" LabelText="<%$Resources:Labels,Allowances %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtInsurance" CssClass="field" LabelText="<%$Resources:Labels,Insurance %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" Enabled="False" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtSancations" CssClass="field" LabelText="<%$Resources:Labels,Sancations %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOtherDeducations" CssClass="field" LabelText="<%$Resources:Labels,OtherDeducations %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0"></asp:ABFTextBox>
                </div>
                <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                    runat="server" ValidationGroup="AddNew" Height="50" TextMode="MultiLine"></asp:ABFTextBox>

                <span>
                   الاجمالي </span>:
                <asp:Label runat="server" ID="lblGrossTotal2" Text="0" ForeColor="Red"></asp:Label>
                <br />
                <span>
                   الخصومات</span>:
                <asp:Label runat="server" ID="lblGrossTotal1" Text="0" ForeColor="Green"></asp:Label>
                <br />
                <span>
                    <%=Resources.Labels.GrossTotal %></span>:
                <asp:Label runat="server" ID="lblGrossTotal" Text="0" ForeColor="Black"></asp:Label>

            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnCloseNew" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Close %>" />
            </div>
        </div>
    </asp:Panel>
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
