<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Damages.aspx.cs" Inherits="Production_Damages" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<%@ Import Namespace="XPRESS.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <div runat="server" id="divOrderNumber" style="display: inline-block;">
        <span>
            <%=Resources.Labels.ProductionOrderNumber %></span>:
                <asp:Label ID="lblOrderNumber" runat="server" Font-Bold="true" Text=""></asp:Label>
    </div>
    <div style="clear: both">
    </div>
    <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID" AllowPaging="false">
        <Columns>
            <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Labels,RawStore %>" />
            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,ItemMaterial %>" />
            <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                DataFormatString="{0:0.####}" />
            <asp:TemplateField HeaderText="<%$Resources:Labels,Damages %>">
                <ItemTemplate>
                    <asp:ABFTextBox runat="server" ID="txtDamageQty" DataType="Decimal" Text='<%# Eval("DamageQty").ToExpressString() %> ' Style="min-width: 100px;"></asp:ABFTextBox>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:ABFGridView>
    <div class="align_right">
        <asp:Button runat="server" ID="btnSave" Text="<%$Resources:Labels,Save %>" CssClass="button_big shortcut_save"
            OnClick="btnSave_click" />
        <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
            OnClick="btnApprove_click" OnClientClick="return ConfirmSure();" />
    </div>
</asp:Content>
