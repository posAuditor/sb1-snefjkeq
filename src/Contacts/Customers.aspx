<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Customers.aspx.cs" Inherits="Contacts_Customers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>


<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />
    <%if (this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic)
        {%>
            <link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />
        <%} 
    %>
    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>
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
                            IsRequired="true" ValidationGroup="NewCustomer"></asp:ABFTextBox>
                    
            
                        
                        <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" ValidationGroup="NewCustomer"></asp:AutoComplete>
                        <label>
                            <%=Resources.Labels.Currency %></label>
                        <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="txtStartFrom_TextChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:AutoComplete runat="server" ID="acArea" ServiceMethod="GetAreas" LabelText="<%$Resources:Labels,Area %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipVia" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,ShipVia %>"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkAddNewAtt" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                        <label>
                            <%=Resources.Labels.ApplyCreditLimit %></label>
                        <asp:DropDownList ID="ddlApplyCreditLimit" runat="server">
                            <asp:ListItem Text="<%$ Resources:Labels,Yes %>" Value="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,No %>" Value="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtTaxNumber" runat="server" LabelText="الرقم الضريبي"></asp:ABFTextBox>
                         <br />
                        <asp:CheckBox ID="chkIsCashCustomer" runat="server" /><span>    عميل نقدي  </span>
                        <br />
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtCreditLimitValue" runat="server" ValidationGroup="NewCustomer"
                            LabelText="<%$Resources:Labels,DefaultCustomersCreditLimitValue %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" ServiceMethod="GetChartOfAccountsException"
                            LabelText="<%$Resources:Labels,ParentAccount %>" ValidationGroup="NewCustomer"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="NewCustomer" LabelText="<%$Resources:Labels,Ratio %>"
                            DataType="Decimal" Enabled="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="NewCustomer"
                            LabelText="<%$Resources:Labels,OpenBalance %>" OnTextChanged="txtOpenBalance_TextChanged"
                            AutoPostBack="true" DataType="DecimalNegative"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="NewCustomer" LabelText="<%$Resources:Labels,StartFrom %>"
                            OnTextChanged="txtStartFrom_TextChanged" AutoPostBack="true" DataType="Date"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsException"
                            LabelText="<%$Resources:Labels,OppositeAccount %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acPriceName" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,PriceType %>"
                            IsRequired="true" ValidationGroup="addPrice"></asp:AutoComplete>
                         
                        <br />
                        <asp:CheckBox ID="chkCustomer" runat="server" /><span>العميل يعامل كمورد  </span>
                        <br />
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


                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

                    </div>
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,ContactData %>">
            <ContentTemplate>
                <div class="form" style="width: 60%;">
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
        <asp:TabPanel ID="TabPanelPrices" runat="server" HeaderText="<%$Resources:Labels,SpecialPrices %>">
            <ContentTemplate>
                 <div class="form" style="width: 60%;">
                 
                        <asp:AutoComplete runat="server" ID="acNameSrch" ServiceMethod="GetItems"  
                        LabelText="<%$Resources:Labels,Item %>" ValidationGroup="PriceSpec" IsRequired="true"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtPrice" runat="server" LabelText="<%$Resources:Labels,Price %>"
                         IsRequired="true" ValidationGroup="PriceSpec"></asp:ABFTextBox>

                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary4" runat="server"  ValidationGroup="PriceSpec" />
                    </div>
                    <br />
                    <div class="align_right">
                        <asp:Button ID="btnPriceSpec" CssClass="button" runat="server" OnClick="btnPriceSpec_Click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="PriceSpec" />
                    </div>
                </div>

                <asp:ABFGridView runat="server" ID="gvPriceSpec" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvPriceSpec_RowDeleting" OnPageIndexChanging="gvPriceSpec_PageIndexChanging" OnSelectedIndexChanging="gvPriceSpec_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Item %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Price %>" />
                         <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="قياسات العميل" Visible="False">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtLength" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,Length %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtLengthSleeve" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,LengthSleeve %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtNeek" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,Neek %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtCupLength" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,CupLength %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>
                    </div>

                    <div class="left_col">
                        <asp:ABFTextBox ID="txtShoulder" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,Shoulder %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtSizeChest" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,SizeChest %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtSizeHand" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,SizeHand %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtElbow" runat="server" ValidationGroup="NewContactMesure"
                            LabelText="<%$Resources:Labels,Elbow %>" IsRequired="true"
                            DataType="Decimal"></asp:ABFTextBox>
                    </div>
                    <br />
                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="NewContactMesure" />
                    </div>
                    <div class="align_right">

                        <br />
                        <asp:Button ID="btnAddMesures" CssClass="button" runat="server" OnClick="btnAddMesures_OnClick"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewContactMesure" />
                    </div>
                </div>
                <br />
                <asp:ABFGridView runat="server" ID="gvContactMesure" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnPageIndexChanging="gvContactMesure_OnPageIndexChanging" OnSelectedIndexChanging="gvContactMesure_OnSelectedIndexChanging"
                    OnRowDeleting="gvContactMesure_OnRowDeleting" OnSelectedIndexChanged="gvContactMesure_OnSelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Length" HeaderText="<%$Resources:Labels,Length %>" />
                        <asp:BoundField DataField="Shoulder" HeaderText="<%$Resources:Labels,Shoulder %>" />
                        <asp:BoundField DataField="LengthSleeve" HeaderText="<%$Resources:Labels,LengthSleeve %>" />
                        <asp:BoundField DataField="SizeChest" HeaderText="<%$Resources:Labels,SizeChest %>" />

                        <asp:BoundField DataField="Neek" HeaderText="<%$Resources:Labels,Neek %>" />
                        <asp:BoundField DataField="SizeHand" HeaderText="<%$Resources:Labels,SizeHand %>" />
                        <asp:BoundField DataField="CupLength" HeaderText="<%$Resources:Labels,CupLength %>" />
                        <asp:BoundField DataField="Elbow" HeaderText="<%$Resources:Labels,Elbow %>" />



                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
                <br />
                <br />
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
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="NewCustomer" />
    </div>
    <div class="btnDiv">
        <asp:Button ID="btnSaveCustomer" CssClass="button shortcut_save" runat="server" OnClick="btnSaveCustomer_click"
            Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewCustomer" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>
    <asp:NewAttribute ID="ucNewAtt" runat="server" Title="<%$Resources:Labels,ShipVia %>"
        AttributeType_ID="10" TargetControlID="cph$TabContainer1$TabPanel2$lnkAddNewAtt"
        OnNewAttributeCreated="ucNewAtt_NewAttributeCreated"></asp:NewAttribute>
</asp:Content>
