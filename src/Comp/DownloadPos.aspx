<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DownloadPos.aspx.cs" Inherits="Comp_DownloadPos" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div style="direction: rtl; font-size: 15px; color: black; font-family: Tahoma;" class="MainGrayDiv">
            <center>
                Download </center>
            <ul>
                <li>
                    <p>
                        قم بانزال نقطة البيع من  <a target="_blank" href="../Pos.zip">هنا</a>
                    </p>
                </li>
               

            </ul>
        </div>
    </div>
   
</asp:Content>
