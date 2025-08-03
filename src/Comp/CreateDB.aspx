<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="CreateDB.aspx.cs" Inherits="Comp_CreateDB" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" Runat="Server">

      <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnAdd">
        <div class="tcat">
             إنشاء قاعدة بيانات جديده
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server"></asp:ABFTextBox>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnAdd" CssClass="button" runat="server" Text="<%$ Resources:Labels, Add %>"
                    OnClick="btnAdd_Click" />
                
            </div>
        </div>
    </asp:Panel>
</asp:Content>

