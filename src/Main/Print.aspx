<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="Main_Print" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <center>
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Main/Default.aspx" Text="<%$Resources:Labels,Back %>" Font-Size="20"></asp:HyperLink></center>
        <br /><br />
    <iframe id="ifViewer" name="ifViewer" src="../Reports/TempReports/2015-05-25-11-46-27-GeneralLedger.pdf"
        scrolling="yes" width="100%" runat="server" height="1400" frameborder="0"></iframe>
</asp:Content>
