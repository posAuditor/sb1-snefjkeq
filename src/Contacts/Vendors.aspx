<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Vendors.aspx.cs" Inherits="Contacts_Vendors" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <div class="InvoiceHeader">


        <asp:Nav runat="server" ID="ucNav" VisibleText="false" />
         <asp:Favorit runat="server" ID="ucFavorit" />

    </div>
    <div style="clear: both;">
    </div>




    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue">
        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$Resources:Labels,BasicData %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                            IsRequired="true" ValidationGroup="NewVendor"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" ValidationGroup="NewVendor"></asp:AutoComplete>
                        <label>
                            <%=Resources.Labels.Currency %></label>
                        <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="txtStartFrom_TextChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:AutoComplete runat="server" ID="acArea" ServiceMethod="GetAreas" LabelText="<%$Resources:Labels,Area %>"
                            Visible="false"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipVia" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,ShipVia %>"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkAddNewAtt" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                        <%--<label>
                            <%=Resources.Labels.ApplyCreditLimit %></label>--%>
                        <asp:DropDownList ID="ddlApplyCreditLimit" runat="server" Visible="false">
                            <asp:ListItem Text="<%$ Resources:Labels,Yes %>" Value="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,No %>" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtTaxNumber" runat="server" LabelText="الرقم الضريبي"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" Enabled="false" ServiceMethod="GetChartOfAccountsException"
                            LabelText="<%$Resources:Labels,ParentAccount %>" ValidationGroup="NewVendor"></asp:AutoComplete>
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtCreditLimitValue" runat="server" ValidationGroup="NewVendor"
                            LabelText="<%$Resources:Labels,DefaultCustomersCreditLimitValue %>" IsRequired="true"
                            Visible="false" DataType="Decimal"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="NewVendor" LabelText="<%$Resources:Labels,Ratio %>"
                            DataType="Decimal" Enabled="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="NewVendor" LabelText="<%$Resources:Labels,OpenBalance %>"
                            OnTextChanged="txtOpenBalance_TextChanged" AutoPostBack="true" DataType="Decimal"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="NewVendor" LabelText="<%$Resources:Labels,StartFrom %>"
                            OnTextChanged="txtStartFrom_TextChanged" AutoPostBack="true" DataType="Date"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsException"
                            LabelText="<%$Resources:Labels,OppositeAccount %>" Enabled="false"></asp:AutoComplete>
                        <br />
                        <asp:CheckBox ID="chkCustomer" runat="server" /><span>المورد يعامل كعميل</span>
                    </div>
                    <div style="clear: both;">
                    </div>
                    <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                        TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
                    <br />
                    <div class="right_col">
                        <label>
                            <%=Resources.Labels.Logo %></label>
                        <asp:FileUpload ID="fpLogo" runat="server" Width="200" accept="image/jpeg"></asp:FileUpload>
                        <asp:Button ID="btnUploadImage" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>" Style="min-width: 0px;"
                            OnClick="btnUploadImage_Click" />
                    </div>
                    <div class="left_col">
                        <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                            ImageUrl="~/Images/no_photo.png" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,ContactData %>">
            <ContentTemplate>
                <div class="form" style="width: 600px;">
                    <asp:AutoComplete runat="server" ID="acContactDataType" ServiceMethod="GetGeneralAtt"
                        LabelText="<%$Resources:Labels,Type %>" IsRequired="true" ValidationGroup="NewContactData"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtContactData" runat="server" LabelText="<%$Resources:Labels,Data %>"
                        TextMode="MultiLine" Height="50" IsRequired="true" ValidationGroup="NewContactData"></asp:ABFTextBox>

                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewContactData" />
                    </div>
                    <br />
                    <div class="align_right">
                        <asp:Button ID="btnAddContactDetail" CssClass="button" runat="server" OnClick="btnAddContactDetail_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewContactData" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvContactData" GridViewStyle="GrayStyle" DataKeyNames="ID,Att_ID"
                    OnRowDeleting="gvContactData_RowDeleting" OnPageIndexChanging="gvContactData_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="AttName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="Data" HeaderText="<%$Resources:Labels,Data %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel9" runat="server" HeaderText="<%$Resources:Labels,EInvoiceHeaderSettings %>">
            <ContentTemplate>
                <div class="form">
                    <div class="row">
                        <div class="col-md-6">
                            <label style="text-align:center">بيانات الشركة بالعربي</label>
                        </div>

                        <div class="col-md-6">
                            <label style="text-align:center">Company Data In Other Language</label>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-4">
                            <label>رقم السجل التجاري : </label>
                        </div>
                        <div class="col-md-4">                            
                            <asp:TextBox ID="TxtRegisterNo" runat="server" style="text-align:center"/>
                        </div>
                        <div class="col-md-4">
                            <label style="text-align: left">: Register No </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <label>رقم المبنى : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtBuildingNo" runat="server" style="text-align:center"/>
                        </div>
                        <div class="col-md-4">
                            <label style="text-align:left">: Building No </label>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-2">
                            <label>اسم الشركة : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtNameAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtNameOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: Company Name </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label>عنوان الشركة : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtAddressAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtAddressOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: Company Address </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label>اسم الشارع : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtStreetNameAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtStreetNameOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: Street Name </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label>اسم الحي : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtDistrictAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtDistrictOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: District </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label>اسم المدينة : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtCityAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtCityOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: City </label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label>البلد : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtCountryAr" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtCountryOther" runat="server" style="text-align:left"/>
                        </div>

                        <div class="col-md-2" style="text-align:left;">
                            <label>: Country </label>
                        </div>
                    </div>                    
                    
                    <%--<br />--%>
                    <div class="row">
                        <div class="col-md-4">
                            <label>الرمز البريدي : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtPostalCode" runat="server" style="text-align:center"/>
                        </div>
                        <div class="col-md-4">
                            <label style="text-align:left">: Postal Code </label>
                        </div>
                    </div>

                    <%--<br />--%>
                     <div class="row">
                        <div class="col-md-4">
                            <label>الرقم الإضافي للعنوان : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtAdditonalNo" runat="server" style="text-align:center"/>
                        </div>
                        <div class="col-md-4">
                            <label style="text-align:left">: Additional No </label>
                        </div>
                    </div>

                    <%--<br />--%>
                     <div class="row">
                        <div class="col-md-4">
                            <label>معرف آخر : </label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtOtherBuyerId" runat="server" style="text-align:center"/>
                        </div>
                        <div class="col-md-4">
                            <label style="text-align:left">: Other Buyer ID </label>
                        </div>
                    </div>
                </div>
                <br />
               <%-- <div class="btnDiv">
                    <asp:Button ID="BtnSaveEInvoiceHeaderSettings" CssClass="button shortcut_save" runat="server"
                        OnClick="BtnSaveEInvoiceHeaderSettings_Click" Text="<%$ Resources:Labels, Save %>" ValidationGroup="SaveCompanyData" />
                </div>--%>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <br />
    <br />
    <div class="validationSummary">
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="NewVendor" />
    </div>
    <div class="btnDiv">
        <asp:Button ID="btnSaveVendor" CssClass="button shortcut_save" runat="server" OnClick="btnSaveVendor_click"
            Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewVendor" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>
    <asp:NewAttribute ID="ucNewAtt" runat="server" Title="<%$Resources:Labels,ShipVia %>"
        AttributeType_ID="10" TargetControlID="cph$TabContainer1$TabPanel2$lnkAddNewAtt"
        OnNewAttributeCreated="ucNewAtt_NewAttributeCreated"></asp:NewAttribute>
</asp:Content>
