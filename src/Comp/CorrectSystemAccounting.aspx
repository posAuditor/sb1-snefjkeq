<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="CorrectSystemAccounting.aspx.cs" Inherits="Comp_CorrectSystemAccounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div style="direction: rtl; font-size: 15px; color: Red; font-family: Tahoma;" class="MainGrayDiv">
        <center>
        <asp:Button runat="server" ID="btnClose" CssClass="button" Text="صيانة المخزون والقيود"
            OnClick="btnClose_Click" OnClientClick="return ConfirmSure()" />
    </center>
    </div>
</asp:Content>

