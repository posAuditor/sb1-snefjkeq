<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Expenses.aspx.cs" Inherits="Production_Expenses" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" OnClick="lnkAddNew_Click"
        Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>

    <div style="clear: both">
    </div>
    <br />
    <div runat="server" id="divOrderNumber" style="display: inline-block;">
        <span>
            <%=Resources.Labels.ProductionOrderNumber %></span>:
                <asp:Label ID="lblOrderNumber" runat="server" Font-Bold="true" Text=""></asp:Label>
    </div>
    <asp:ABFGridView runat="server" ID="gvExpenses" GridViewStyle="GrayStyle" DataKeyNames="ID,ExpenseName"
        OnRowDeleting="gvExpenses_RowDeleting" OnPageIndexChanging="gvExpenses_PageIndexChanging"
        OnSelectedIndexChanging="gvExpenses_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="OperationDate" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="ExpenseName" HeaderText="<%$Resources:Labels,ExpenseAccount %>" />
            <asp:BoundField DataField="OppositeAccountName" HeaderText="<%$Resources:Labels,OppositeAccount %>" />
            <asp:BoundField DataField="ExpenseValue" HeaderText="<%$Resources:Labels,Amount %>" DataFormatString="{0:0.####}" />
            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Labels,Notes %>" />
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
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="hfmpeCreateNew"
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
                        runat="server" IsRequired="true" ValidationGroup="AddNew"
                        DataType="Date"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acExpense" ServiceMethod="GetGeneralAtt"
                        LabelText="<%$Resources:Labels,Name %>" IsRequired="true" ValidationGroup="AddNew" AutoCompleteWidth="230"></asp:AutoComplete>
                            <asp:LinkButton ID="lnkAddNewExpenseName" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                        runat="server" TextMode="MultiLine"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,OppositeAccount %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtAmount" CssClass="field" LabelText="<%$Resources:Labels,Amount %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        MinValue="0"></asp:ABFTextBox>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSave_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
        <asp:NewAttribute ID="ucNewExpenseName" runat="server" Title="<%$Resources:Labels,Name %>"
        AttributeType_ID="70" TargetControlID="lnkAddNewExpenseName" OnNewAttributeCreated="ucNewExpenseName_NewAttributeCreated"></asp:NewAttribute>
</asp:Content>
