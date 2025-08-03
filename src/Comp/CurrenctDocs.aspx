<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CurrenctDocs.aspx.cs" Inherits="Comp_CurrentDocs" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="">

        <div id="cph_UnderMinQty" class="InvoiceSection">
           
                <h3>

                    <asp:Label runat="server" ID="lblTitle" Text="<%$Resources:Labels,CurrentDocs %>"></asp:Label>

                </h3>
            
        </div>
    </div>

    <asp:ABFGridView runat="server" ID="gvDocs" GridViewStyle="BlueStyle" OnPageIndexChanging="gvDocs_PageIndexChanging">
        <Columns>
            <asp:HyperLinkField DataTextField="PageName" DataNavigateUrlFields="PageUrl" DataNavigateUrlFormatString="{0}"
                HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Total" HeaderText="<%$Resources:Labels,Total %>" />
        </Columns>
    </asp:ABFGridView>
</asp:Content>
