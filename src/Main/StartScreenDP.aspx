<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="StartScreenDP.aspx.cs" Inherits="Main_StartScreenDP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/droidarabickufi.css" rel="stylesheet" />
    <style>
        .center {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
     <asp:Label ID="lblVersionDay" ForeColor="Red" runat="server" Font-Size="25" style="text-align:center;" Text=""></asp:Label>
    <asp:Image ID="img" runat="server" CssClass="center" />
</asp:Content>

