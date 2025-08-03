<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UnderRequestEmployees.aspx.cs" Inherits="HR_UnderRequestEmployees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <div class="form" style="width: 90%">
        <div class="right_col">
            <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
            <asp:AutoComplete runat="server" ID="acDegree" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,Degree %>"></asp:AutoComplete>
            <asp:ABFTextBox ID="txtNationalID" CssClass="field" LabelText="<%$Resources:Labels,NationalID %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtTestDate" CssClass="field" LabelText="<%$Resources:Labels,TestDate %>"
                runat="server" DataType="Date" ValidationGroup="AddNew" IsRequired="true" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtTestDegreeSpeed" CssClass="field" LabelText="<%$Resources:Labels,TestDegreeSpeed %>"
                runat="server"></asp:ABFTextBox>
            <asp:AutoComplete runat="server" ID="acDepartment" ServiceMethod="GetHRDepartments"
                LabelText="<%$Resources:Labels,Department %>" OnSelectedIndexChanged="acDepartment_SelectedIndexChanged"
                AutoPostBack="true"></asp:AutoComplete>
            <asp:AutoComplete runat="server" ID="acPosition" ServiceMethod="GetHRPositions" LabelText="<%$Resources:Labels,Position %>"></asp:AutoComplete>
            <asp:ABFTextBox ID="txtEmail" CssClass="field" LabelText="<%$Resources:Labels,Email %>"
                DataType="Email" ValidationGroup="AddNew" runat="server"></asp:ABFTextBox>
            <label>
                <%=Resources.Labels.EmploymentStatus %></label>
            <asp:DropDownList ID="ddlEmploymentStatus" runat="server">
                <asp:ListItem Text="<%$Resources:Labels,UnderRequest %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$Resources:Labels,Denied %>" Value="1"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="left_col">
            <asp:AutoComplete runat="server" ID="acQualification" ServiceMethod="GetGeneralAtt"
                LabelText="<%$Resources:Labels,Qual %>"></asp:AutoComplete>
            <asp:ABFTextBox ID="txtUniversity" CssClass="field" LabelText="<%$Resources:Labels,University %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtPassportID" CssClass="field" LabelText="<%$Resources:Labels,PassportID %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtTestDegreeQuality" CssClass="field" LabelText="<%$Resources:Labels,TestDegreeQuality %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtTel1" CssClass="field" LabelText="<%$Resources:Labels,Telephone %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtTel2" CssClass="field" LabelText="<%$Resources:Labels,Telephone2 %>"
                runat="server"></asp:ABFTextBox>
            <asp:ABFTextBox ID="txtAddress" CssClass="field" LabelText="<%$Resources:Labels,Address %>"
                TextMode="MultiLine" Height="100" runat="server"></asp:ABFTextBox>
        </div>
        <div style="clear: both;">
        </div>

        <br />
        <div class="right_col">
            <label>
                <%=Resources.Labels.Picture %></label>
            <asp:FileUpload ID="fpLogo" runat="server" Width="200" accept="image/jpeg"></asp:FileUpload>
            <asp:Button ID="btnUploadImage" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>" Style="min-width:0px;"
                OnClick="btnUploadImage_Click" />
        </div>
        <div class="left_col">
            <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                ImageUrl="~/Images/no_photo.png" />
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <br />
    <br />
    <div class="validationSummary">
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddNew" />
    </div>
    <div class="btnDiv">
        <asp:Button ID="btnSaveEmp" CssClass="button shortcut_save" runat="server" OnClick="btnSaveEmp_click"
            Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>

</asp:Content>
