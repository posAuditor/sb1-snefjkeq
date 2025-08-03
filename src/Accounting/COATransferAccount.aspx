<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="COATransferAccount.aspx.cs" Inherits="Accounting_COATransferAccount" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">




    <asp:Panel ID="pnlMerge" CssClass="pnlPopUp" runat="server" Width="590">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnCls"></asp:Button>
            <span>دمج حسابين</span>
        </div>
        <div class="content">
            <div class="form" style="width: 98%; margin: auto;">
                <div class="right_col">

                    <asp:AutoComplete runat="server" ID="acAccountFrom" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="TransferAccount" IsException="True" IsRequired="true" LabelText="الحساب "></asp:AutoComplete>

                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acAccountTo" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="TransferAccount" IsException="True" IsRequired="true" LabelText="حساب الاب"></asp:AutoComplete>


                </div>

            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="validationSummary">
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="TransferAccount" />
        </div>
        <div style="clear: both">
        </div>
        <div class="btnDiv">
            <asp:Button ID="btnMerge" CssClass="button default_button" runat="server" OnClick="btnMerge_Click"
                Text="<%$ Resources:Labels, Save %>" ValidationGroup="TransferAccount" />
            <asp:Button ID="Button4" runat="server" CssClass="button"
                Text="<%$ Resources:Labels, Clear %>" />
        </div>

    </asp:Panel>


</asp:Content>



