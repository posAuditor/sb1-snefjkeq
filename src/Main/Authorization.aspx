<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Authorization.aspx.cs" Inherits="Main_Authorization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div style="padding: 20px; text-align: center; font-family: Arial;">
        <p>
            <asp:Label ID="ltNoAccessToPrevPage" Style="color: Red; font-size: 20px;" runat="server"
                Text="<%$ Resources:Resource, NoAccessToPrevPage%>"></asp:Label>
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="ltContactSupportIfRegular" Style="color: Gray;" runat="server" Text="<%$ Resources:Resource, ContactSupportIfRegular%>"></asp:Label></p>
    </div>
</asp:Content>
