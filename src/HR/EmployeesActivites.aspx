<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="EmployeesActivites.aspx.cs" Inherits="HR_EmployeesActivites" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <span>
        <%=Resources.Labels.Name %></span>:
    <asp:Label runat="server" ID="lblEmpName" Text="" Font-Bold="true"></asp:Label>
    <br />
    <br />
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
        AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
        ExpandDirection="Vertical" SuppressPostBack="true">
    </asp:CollapsiblePanelExtender>
    <div style="clear: both;">
    </div>
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
        <div class="tcat">
            <%=Resources.Labels.Search %></div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtDateFromSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Accepted %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,NotAccepted %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
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
    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" style="min-width:600px;">
        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$Resources:Labels,Complaints %>">
            <ContentTemplate>
                <div class="form" style="width: 550px;">
                    <asp:AutoComplete runat="server" ID="acComplaintAgainst" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,Against %>" ValidationGroup="AddComplaint" IsRequired="true">
                    </asp:AutoComplete>
                    <asp:ABFTextBox ID="txtComplaintDate" runat="server" ValidationGroup="AddComplaint"
                        IsRequired="true" DataType="Date" LabelText="<%$Resources:Labels,Date %>" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtComplaintReason" TextMode="MultiLine" Height="50" runat="server"
                        LabelText="<%$Resources:Labels,Reason %>"></asp:ABFTextBox>
                    <div style="clear: both;">
                    </div>
                    <div style="clear: both; text-align: center; width: 550px;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddComplaint" CssClass="button" runat="server" OnClick="btnAddComplaint_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddComplaint" />
                        <asp:Button ID="btnClearComplaint" CssClass="button" runat="server" OnClick="btnClearComplaint_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                    <asp:ABFGridView runat="server" ID="gvComplaints" GridViewStyle="GrayStyle" DataKeyNames="ID"
                        PageSize="10" OnRowDeleting="gvComplaints_RowDeleting" OnPageIndexChanging="gvComplaints_PageIndexChanging"
                        OnSelectedIndexChanging="gvComplaints_SelectedIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="AgainstName" HeaderText="<%$Resources:Labels,Against %>" />
                            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                            <asp:BoundField DataField="Reason" HeaderText="<%$Resources:Labels,Reason %>" />
                            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                                <ItemTemplate>
                                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
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
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,Vacations %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acVacationType" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,Type %>" IsRequired="true" ValidationGroup="AddVacation">
                        </asp:AutoComplete>
                        <asp:ABFTextBox ID="txtVacationFromDate" runat="server" LabelText="<%$Resources:Labels,DateFrom %>"
                            DataType="Date" IsRequired="true" ValidationGroup="AddVacation" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtVacationToDate" runat="server" LabelText="<%$Resources:Labels,DateTo %>"
                            DataType="Date" IsRequired="true" ValidationGroup="AddVacation" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtVacationNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddVacation"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acVacationApprovedBY" ServiceMethod="GetContactNames"
                            LabelText="<%$Resources:Labels,ApprovedBy %>" ValidationGroup="AddVacation" IsRequired="true">
                        </asp:AutoComplete>
                        <br />
                        <asp:CheckBox ID="chkVacationIsAccepted" runat="server" /><span><%=Resources.Labels.IsAccepted %></span><br />
                        <asp:ABFTextBox ID="txtVacationReason" runat="server" LabelText="<%$Resources:Labels,Reason %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddVacation"></asp:ABFTextBox>
                    </div>
                    <div style="clear: both; text-align: center; width: 100%;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddVacations" CssClass="button" runat="server" OnClick="btnAddVacations_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddVacation" />
                        <asp:Button ID="btnClearVacations" CssClass="button" runat="server" OnClick="btnClearVacations_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvVacations" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvVacations_RowDeleting" OnPageIndexChanging="gvVacations_PageIndexChanging"
                    OnSelectedIndexChanging="gvVacations_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="VacationTypeName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="FromDate" HeaderText="<%$Resources:Labels,DateFrom %>"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="ToDate" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="Reason" HeaderText="<%$Resources:Labels,Reason %>" />
                        <asp:CheckBoxField DataField="IsAccepted" HeaderText="<%$Resources:Labels,IsAccepted %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$Resources:Labels,Sancations %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acSancationType" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,Type %>" IsRequired="true" ValidationGroup="AddSancation">
                        </asp:AutoComplete>
                        <asp:ABFTextBox ID="txtSancationDate" runat="server" LabelText="<%$Resources:Labels,Date %>"
                            DataType="Date" IsRequired="true" ValidationGroup="AddSancation" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtSancationReason" runat="server" LabelText="<%$Resources:Labels,Reason %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddSancation"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acSancationTakenProcedure" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,TakenProcedure %>" IsRequired="true" ValidationGroup="AddSancation">
                        </asp:AutoComplete>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acSanctionApprovedBy" ServiceMethod="GetContactNames"
                            LabelText="<%$Resources:Labels,ApprovedBy %>" ValidationGroup="AddSancation"
                            IsRequired="true"></asp:AutoComplete>
                        <br />
                        <asp:ABFTextBox ID="txtSancationEmployeeComment" runat="server" LabelText="<%$Resources:Labels,EmployeeComment %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddSancation"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtSancationSalaryDeduction" runat="server" LabelText="<%$Resources:Labels,DeductionFromSalary %>"
                            DataType="Decimal" ValidationGroup="AddSancation" MinValue="0"></asp:ABFTextBox>
                        <label>
                            <%=Resources.Labels.ValueType %></label>
                        <asp:DropDownList ID="ddlSancationSalaryDeducationValueType" runat="server" >
                            <asp:ListItem Text="<%$ Resources:Labels,FixedValue %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Percentage %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Minutes %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Hours %>" Value="3"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Days %>" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="clear: both; text-align: center; width: 100%;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddSancations" CssClass="button" runat="server" OnClick="btnAddSancations_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddSancation" />
                        <asp:Button ID="btnClearSancation" CssClass="button" runat="server" OnClick="btnClearSancations_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvSancations" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvSancations_RowDeleting" OnPageIndexChanging="gvSancations_PageIndexChanging"
                    OnSelectedIndexChanging="gvSancations_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="SancationTypeName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="Reason" HeaderText="<%$Resources:Labels,Reason %>" />
                        <asp:BoundField DataField="ProcedureName" HeaderText="<%$Resources:Labels,TakenProcedure %>" />
                        <asp:BoundField DataField="SalaryDeducation" HeaderText="<%$Resources:Labels,DeductionFromSalary %>" />
                        <asp:BoundField DataField="ValueTypeName" HeaderText="<%$Resources:Labels,ValueType %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$Resources:Labels,Incentives %>">
            <ContentTemplate>
                <div class="form" style="width: 550px;">
                    <asp:AutoComplete runat="server" ID="acIncentive" ServiceMethod="GetHRIncentives" LabelText="<%$Resources:Labels,Incentive %>"
                        IsRequired="true" ValidationGroup="AddIncentive"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtIncentiveDate" runat="server" LabelText="<%$Resources:Labels,Date %>"
                        DataType="Date" IsRequired="true" ValidationGroup="AddIncentive" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtIncentiveNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                        TextMode="MultiLine" Height="50" ValidationGroup="AddIncentive"></asp:ABFTextBox>
                    <div style="clear: both; text-align: center; width: 550px;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddIncentives" CssClass="button" runat="server" OnClick="btnAddIncentives_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddIncentive" />
                        <asp:Button ID="btnClearIncentives" CssClass="button" runat="server" OnClick="btnClearIncentives_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvIncentives" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvIncentives_RowDeleting" OnPageIndexChanging="gvIncentives_PageIndexChanging"
                    OnSelectedIndexChanging="gvIncentives_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="IncentiveName" HeaderText="<%$Resources:Labels,Incentive %>" />
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="<%$Resources:Labels,AbsesnceInMission %>">
            <ContentTemplate>
                <div class="form" style="width: 550px;">
                    <asp:ABFTextBox ID="txtAbsenceInMissionFromDate" runat="server" LabelText="<%$Resources:Labels,DateFrom %>"
                        DataType="Date" IsRequired="true" ValidationGroup="AddAbasenceInMission" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAbsenceInMissionToDate" runat="server" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" IsRequired="true" ValidationGroup="AddAbasenceInMission" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAbasenceInMissionReason" runat="server" LabelText="<%$Resources:Labels,Reason %>"
                        TextMode="MultiLine" Height="50" ValidationGroup="AddAbasenceInMission"></asp:ABFTextBox>
                    <div style="clear: both; text-align: center; width: 550px;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddAbasenceInMission" CssClass="button" runat="server" OnClick="btnAddAbasenceInMission_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddAbasenceInMission" />
                        <asp:Button ID="btnClearAbasenceInMission" CssClass="button" runat="server" OnClick="btnClearAbasenceInMission_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvAbasenceInMission" GridViewStyle="GrayStyle"
                    DataKeyNames="ID" OnRowDeleting="gvAbasenceInMission_RowDeleting" OnPageIndexChanging="gvAbasenceInMission_PageIndexChanging"
                    OnSelectedIndexChanging="gvAbasenceInMission_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="FromDate" HeaderText="<%$Resources:Labels,DateFrom %>"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="ToDate" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="<%$Resources:Labels,Excuses %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtExcuseDate" runat="server" LabelText="<%$Resources:Labels,Date %>"
                            DataType="Date" IsRequired="true" ValidationGroup="AddExcuse" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExcuseFrom" CssClass="field" LabelText="<%$Resources:Labels,From %>"
                            runat="server" IsRequired="true" ValidationGroup="AddExcuse" DataType="Time"
                            Width="170"></asp:ABFTextBox>
                        <asp:DropDownList ID="ddlExcuseFrom" runat="server" Width="50" Style="display: inline;">
                            <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtExcuseTo" CssClass="field" LabelText="<%$Resources:Labels,To %>"
                            runat="server" IsRequired="true" ValidationGroup="AddExcuse" DataType="Time"
                            Width="170"></asp:ABFTextBox>
                        <asp:DropDownList ID="ddlExcuseTo" runat="server" Width="50" Style="display: inline;">
                            <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtExcuseNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddExcuse"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acExcuseApprovedBy" ServiceMethod="GetContactNames"
                            LabelText="<%$Resources:Labels,ApprovedBy %>" ValidationGroup="AddExcuse" IsRequired="true">
                        </asp:AutoComplete>
                        <br />
                        <asp:CheckBox ID="chkExcuseIsAccepted" runat="server" /><span><%=Resources.Labels.IsAccepted%></span><br />
                        <asp:CheckBox ID="chkExcuseInMission" runat="server" /><span><%=Resources.Labels.InMission%></span><br />
                        <asp:ABFTextBox ID="txtExcuseReason" runat="server" LabelText="<%$Resources:Labels,Reason %>"
                            TextMode="MultiLine" Height="50" ValidationGroup="AddExcuse"></asp:ABFTextBox>
                    </div>
                    <div style="clear: both; text-align: center; width: 100%;">
                        <br />
                        <br />
                        <asp:Button ID="btnAddExcuse" CssClass="button" runat="server" OnClick="btnAddExcuse_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddExcuse" />
                        <asp:Button ID="btnClearExcuse" CssClass="button" runat="server" OnClick="btnClearExcuse_click"
                            Text="<%$ Resources:Labels, Clear %>" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvExcuses" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvExcuses_RowDeleting" OnPageIndexChanging="gvExcuses_PageIndexChanging"
                    OnSelectedIndexChanging="gvExcuses_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="FromTime" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:hh:mm tt}" />
                        <asp:BoundField DataField="ToTime" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:hh:mm tt}" />
                        <asp:BoundField DataField="Reason" HeaderText="<%$Resources:Labels,Reason %>" />
                        <asp:CheckBoxField DataField="IsAccepted" HeaderText="<%$Resources:Labels,IsAccepted %>" />
                        <asp:CheckBoxField DataField="IsMission" HeaderText="<%$Resources:Labels,InMission %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <br />
    <br />
    <div class="btnDiv">
        <asp:Button ID="btnSave" CssClass="button shortcut_save" runat="server" OnClick="btnSave_click"
            Text="<%$ Resources:Labels, Save %>" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>
</asp:Content>
