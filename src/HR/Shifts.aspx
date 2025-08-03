<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Shifts.aspx.cs" Inherits="HR_Shifts" %>

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
                <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server"></asp:ABFTextBox>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvShifts" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvShifts_RowDeleting" OnPageIndexChanging="gvShifts_PageIndexChanging"
        OnSelectedIndexChanging="gvShifts_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="WorkFrom" HeaderText="<%$Resources:Labels,WorkFrom %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="WorkTo" HeaderText="<%$Resources:Labels,WorkTo %>" DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="BreakFrom" HeaderText="<%$Resources:Labels,BreakFrom %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="BreakTo" HeaderText="<%$Resources:Labels,BreakTo %>"  DataFormatString="{0:hh:mm tt}"  />
            <asp:BoundField DataField="DelayFrom" HeaderText="<%$Resources:Labels,DelayFrom %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="AbsenceFrom" HeaderText="<%$Resources:Labels,AbsenceFrom %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="DefaultCheckIn" HeaderText="<%$Resources:Labels,DefaultWorkFrom %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:BoundField DataField="DefaultCheckOut" HeaderText="<%$Resources:Labels,DefaultWorkTo %>"
                DataFormatString="{0:hh:mm tt}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# !Convert.ToBoolean(Eval("IsSystem")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click">
            </asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 100%;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtWorkFrom" CssClass="field" LabelText="<%$Resources:Labels,WorkFrom %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlWorkFrom" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtWorkTo" CssClass="field" LabelText="<%$Resources:Labels,WorkTo %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlWorkTo" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtDelayFrom" CssClass="field" LabelText="<%$Resources:Labels,DelayFrom %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlDelayFrom" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtAbsenceFrom" CssClass="field" LabelText="<%$Resources:Labels,AbsenceFrom %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlAbsenceFrom" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtBreakFrom" CssClass="field" LabelText="<%$Resources:Labels,BreakFrom %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlBreakFrom" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtBreakTo" CssClass="field" LabelText="<%$Resources:Labels,BreakTo %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlBreakTo" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtDefaultWorkFrom" CssClass="field" LabelText="<%$Resources:Labels,DefaultWorkFrom %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170" IsRequired="true"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlDefaultWorkFrom" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtDefaultWorkTo" CssClass="field" LabelText="<%$Resources:Labels,DefaultWorkTo %>"
                        runat="server" ValidationGroup="AddNew" DataType="Time" Width="170" IsRequired="true"></asp:ABFTextBox>
                    <asp:DropDownList ID="ddlDefaultWorkTo" runat="server" Width="50" Style="display: inline;">
                        <asp:ListItem Text="<%$Resources:Labels,AM %>" Value="AM"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PM %>" Value="PM"></asp:ListItem>
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
