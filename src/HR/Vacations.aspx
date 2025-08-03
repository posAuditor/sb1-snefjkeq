<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Vacations.aspx.cs" Inherits="HR_Vacations" %>

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
            <%=Resources.Labels.Search %></div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:AutoComplete ID="acDepartmentSrch" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>" IsDateFiscalYearRestricted="false"
                        runat="server"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtDateFromSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>" IsDateFiscalYearRestricted="false"
                        runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.DayFrom %></label>
                    <asp:DropDownList ID="ddlWeekDayFromSrch" runat="server">
                        <asp:ListItem Text="<%$Resources:Labels,Sunday %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Monday %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Tuesday %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Wednesday %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Thursday %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Friday %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Saturday %>" Value="7"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <label>
                        <%=Resources.Labels.VacationType %></label>
                    <asp:DropDownList ID="ddlVacationTypeSrch" runat="server">
                        <asp:ListItem Text="<%$Resources:Labels,Weekly %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Annually %>" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        runat="server" ValidationGroup="search" DataType="Date"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.DayTo %></label>
                    <asp:DropDownList ID="ddlWeekDayToSrch" runat="server">
                        <asp:ListItem Text="<%$Resources:Labels,Sunday %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Monday %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Tuesday %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Wednesday %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Thursday %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Friday %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Saturday %>" Value="7" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
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
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvVacations" GridViewStyle="BlueStyle" DataKeyNames="ID"
        OnRowDeleting="gvVacations_RowDeleting" OnPageIndexChanging="gvVacations_PageIndexChanging"
        OnSelectedIndexChanging="gvVacations_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
            <asp:BoundField DataField="DateFrom" HeaderText="<%$Resources:Labels,DateFrom %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="DateTo" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="WeekDayFromName" HeaderText="<%$Resources:Labels,DayFrom %>" />
            <asp:BoundField DataField="WeekDayToName" HeaderText="<%$Resources:Labels,DayTo %>" />
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
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click">
            </asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <label>
                    <%=Resources.Labels.Department %></label>
                <asp:ListBox ID="lstDepartments" CssClass="field" runat="server" SelectionMode="Multiple"
                    Height="200"></asp:ListBox>
                <label>
                    <%=Resources.Labels.VacationType %></label>
                <asp:DropDownList ID="ddlVacationType" runat="server" OnSelectedIndexChanged="ddlVacationType_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Text="<%$Resources:Labels,Weekly %>" Value="0"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,Annually %>" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <div runat="server" id="divAnnulally" visible="false">
                    <asp:ABFTextBox ID="txtDateFrom" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Date" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDateTo" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Date" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                </div>
                <div runat="server" id="divWeekly">
                    <label>
                        <%=Resources.Labels.DayFrom %></label>
                    <asp:DropDownList ID="ddlWeekDayFrom" runat="server" >
                        <asp:ListItem Text="<%$Resources:Labels,Sunday %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Monday %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Tuesday %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Wednesday %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Thursday %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Friday %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Saturday %>" Value="7"></asp:ListItem>
                    </asp:DropDownList>
                    <label>
                        <%=Resources.Labels.DayTo %></label>
                    <asp:DropDownList ID="ddlWeekDayTo" runat="server" >
                        <asp:ListItem Text="<%$Resources:Labels,Sunday %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Monday %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Tuesday %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Wednesday %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Thursday %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Friday %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Saturday %>" Value="7"></asp:ListItem>
                    </asp:DropDownList>
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
