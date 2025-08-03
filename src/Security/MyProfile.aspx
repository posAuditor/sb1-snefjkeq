<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="MyProfile.aspx.cs" Inherits="Security_MyProfile" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainGrayDiv" id="myProfile" style="min-width: 650px;">
        <div class="form" style="width: 600px;">
            <div dir="auto">
                <span>
                    <%=Resources.Labels.Welcome%></span> :
                <asp:LoginName ID="LoginName1" runat="server" />
            </div>
            <div class="right_col">
                <asp:ABFTextBox ID="txtOldPassword" runat="server" TextMode="Password" LabelText="<%$ Resources:Labels,OldPassword %>"
                    IsRequired="true" ValidationGroup="SaveUserData"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtNewPassword" runat="server" TextMode="Password" LabelText="<%$ Resources:Labels,NewPassword %>"></asp:ABFTextBox>
            </div>
            <div class="left_col">
                <label>
                    <%=Resources.Labels.Email %></label>
                <asp:ABFTextBox ID="txtEmail" CssClass="field" runat="server" IsRequired="true"
                    DataType="Email" ValidationGroup="SaveUserData" ErrorMessage="<%$Resources:Validations,rfvEmail %>">
                </asp:ABFTextBox>
                <label>
                    <%=Resources.Labels.FavLang %></label>
                <asp:DropDownList ID="ddlFavLang" CssClass="field" runat="server" >
                    <asp:ListItem Text="<%$Resources:Labels,Arabic %>" Value="0">
                    </asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Labels,English %>" Value="1">
                    </asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
           <div style="clear: both">
        </div><br /><br />
        <div class="validationSummary">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="SaveUserData" />
        </div>

        <div class="btnDiv">
            <asp:Button ID="btnSave" CssClass="button shortcut_save" runat="server" OnClick="btnSave_click"
                Text="<%$ Resources:Labels, Save %>" ValidationGroup="SaveUserData" />
            <asp:Button ID="btnClear" runat="server" CssClass="button shortcut_clear" OnClick="btnClear_click"
                Text="<%$ Resources:Labels, Clear %>" />
        </div>
     
    </div>
</asp:Content>
