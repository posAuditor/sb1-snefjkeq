<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GenerateVendorNotHaveCard.aspx.cs" Inherits="Contacts_GenerateVendorNotHaveCard" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <asp:ABFGridView runat="server" ID="gvCutomersList" GridViewStyle="BlueStyle" DataKeyNames="ID,Name">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:TemplateField HeaderText="توليد">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%#Eval("ID") %>' OnClick="lnk_Click" runat="server">توليد</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
</asp:Content>
