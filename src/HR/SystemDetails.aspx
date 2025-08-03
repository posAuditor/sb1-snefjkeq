<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SystemDetails.aspx.cs" Inherits="HR_SystemDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">

        <div class="InvoiceHeader">
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 600px; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.Name %></span>:
                    <asp:Label ID="lblName" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList ID="ddlSystemTypeSrch" runat="server"  Enabled="false">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Absence %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,AttendanceIncentives %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Allowances %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,OverTime %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Delay %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Excuse %>" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtFromalVacationAs" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,FormalVacationAs %>"
                        DataType="Decimal" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDailyAllowance" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,DailyAllowance %>"
                        DataType="Decimal" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtMonthlyAllowance" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,MonthlyAllowance %>"
                        DataType="Decimal" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddDetail">
                <div class="form" style="width: 600px; margin: auto;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtValueFrom" runat="server" ValidationGroup="AddDetail" LabelText="<%$Resources:Labels,From %>"
                            DataType="int" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtValueTo" runat="server" ValidationGroup="AddDetail" LabelText="<%$Resources:Labels,To %>"
                            DataType="int" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtResultValue" runat="server" ValidationGroup="AddDetail" LabelText="<%$Resources:Labels,Value %>"
                            DataType="Decimal" IsRequired="true" MinValue="0"></asp:ABFTextBox>
                        <label>
                            <%=Resources.Labels.ValueType %></label>
                        <asp:DropDownList ID="ddlResultResultValueType" runat="server" >
                            <asp:ListItem Text="<%$ Resources:Labels,FixedValue %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Percentage %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Minutes %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Hours %>" Value="3"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,Days %>" Value="4"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,ByActualValue %>" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div style="clear: both">
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddDetail" />
                </div>
                <br />
                <div class="btnDiv">
                    <asp:Button ID="btnAddDetail" CssClass="button" runat="server" OnClick="btnAddDetail_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddDetail" />
                    <asp:Button ID="BtnClearDetail" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearDetail_Click" />
                </div>
            </asp:Panel>
        </div>
        <div class="InvoiceSection">
            <asp:ABFGridView runat="server" ID="gvDetails" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnRowDeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvDetails_PageIndexChanging"
                OnSelectedIndexChanging="gvDetails_SelectedIndexChanging">
                <Columns>
                    <asp:BoundField DataField="FromValue" HeaderText="<%$Resources:Labels,From %>" />
                    <asp:BoundField DataField="ToValue" HeaderText="<%$Resources:Labels,To %>" />
                    <asp:BoundField DataField="ResultValue" HeaderText="<%$Resources:Labels,Value %>"
                        DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="ResultValueTypeName" HeaderText="<%$Resources:Labels,ValueType%>" />
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
        <div class="InvoiceSection align_right">
            <div class="validationSummary">
                <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
            </div>
            <asp:Button runat="server" ID="btnSave" Text="<%$Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                ValidationGroup="Save" OnClick="BtnSave_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" />
            <div style="clear: both">
            </div>
        </div>
    </div>
</asp:Content>
