<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SettingPointOS.aspx.cs" Inherits="Accounting_SettingPointOS" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ShowModalPopup() {
            $find("mpeCreateNew").show();
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" Style="min-width: 800px;">
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="تعريف طرق الدفع">
            <ContentTemplate>

                <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
                <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
                    CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
                    AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
                    ExpandDirection="Vertical" SuppressPostBack="true">
                </asp:CollapsiblePanelExtender>
                <div style="clear: both;">
                </div>
                <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
                    <div class="tcat">
                        <%=Resources.Labels.Search %>
                    </div>
                    <div class="content">
                        <div class="form" style="width: 550px;">
                            <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                runat="server"></asp:ABFTextBox>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="btnDiv">
                            <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                                OnClick="btnSearch_click" />
                            <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvSettingPointOS" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
                    OnRowDeleting="gvSettingPointOS_RowDeleting" OnPageIndexChanging="gvSettingPointOS_PageIndexChanging"
                    OnSelectedIndexChanging="gvSettingPointOS_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />

                        <asp:BoundField DataField="OnInvoiceTypeName" HeaderText="<%$Resources:Labels,OnSales %>" />
                        <asp:BoundField DataField="SalesAccountName" HeaderText="<%$Resources:Labels,SalesAccount %>" />
                        <asp:BoundField DataField="PercentageValue" HeaderText="<%$Resources:Labels,PercentageValue %>" />
                        <asp:BoundField DataField="PurchaseAccountName" HeaderText="حساب النسبة" />
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
                <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
                <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
                    PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="200">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">
                            <div class="right_col">

                                <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>

                                <asp:AutoComplete runat="server" ID="acSalesAccount" ServiceMethod="GetChartOfAccounts"
                                    ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,SalesAccount %>"></asp:AutoComplete>


                                <label>
                                    <%=Resources.Labels.OnSales %></label>
                                <asp:DropDownList ID="ddlOnSales" runat="server">
                                    <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <label style="display: none">
                                    <%=Resources.Labels.OnPurchases %></label>
                                <asp:DropDownList ID="ddlOnPurchases" runat="server" Style="display: none">
                                    <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                                </asp:DropDownList>


                                <label>
                                    <%=Resources.Labels.Logo %></label>
                                <asp:FileUpload ID="fpLogo" runat="server" Width="250"></asp:FileUpload>
                                <asp:Button ID="btnUploadImage" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>"
                                    OnClick="btnUploadImage_Click1" />


                            </div>
                            <div class="left_col">

                                <label style="display: none">
                                    <%=Resources.Labels.OnDocCredit %></label>
                                <asp:DropDownList ID="ddlOnDocCredit" runat="server" Style="display: none">
                                    <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:ABFTextBox ID="txtPercentageValue" CssClass="field" LabelText="<%$Resources:Labels,PercentageValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>



                                <asp:AutoComplete runat="server" ID="acPurchaseAccount" ServiceMethod="GetChartOfAccounts"
                                    ValidationGroup="AddNew" LabelText="حساب النسبة"></asp:AutoComplete>

                                <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                                    ImageUrl="~/Images/no_photo.png" />
                                 <br />
                                <br />
                                <asp:CheckBox ID="chkIsCash" runat="server" /><span> كاش  </span>
                                <br />

                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
                        </div>
                        <br />
                        <div class="btnDiv">
                            <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                            <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>


            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="عروض نقطة البيع">
            <ContentTemplate>
                <asp:LinkButton ID="lnkOfferAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvOffer" GridViewStyle="BlueStyle" DataKeyNames="ID,NameOffer"
                    OnRowDeleting="gvOffer_RowDeleting"
                    OnPageIndexChanging="gvOffer_PageIndexChanging"
                    OnSelectedIndexChanging="gvOffer_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NameOffer" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Discount" HeaderText="<%$Resources:Labels,CashDiscount %>" />
                        <asp:BoundField DataField="DiscountParcentage" HeaderText="<%$Resources:Labels,PercentageDiscount %>" />
                        <asp:BoundField DataField="FromDate" HeaderText="<%$Resources:Labels,DateFrom %>" />
                        <asp:BoundField DataField="ToDate" HeaderText="<%$Resources:Labels,DateTo %>" />
                        <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" />

                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDiscountCustomer" CommandArgument='<%#Eval("ID") %>' OnClick="lnkDiscountCustomer_Click" runat="server" Text="خصم على العملاء"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDiscountItems" CommandArgument='<%#Eval("ID") %>' OnClick="lnkDiscountItems_Click" runat="server" Text="خصم على الاصناف"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:ModalPopupExtender ID="mpeOffer" runat="server" TargetControlID="lnkOfferAddNew"
                    PopupControlID="pnlOffer" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="500">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOffer" CssClass="pnlPopUp" runat="server" Width="500">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="btnCloseOffer"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">
                            <div class="right_col">
                                <asp:ABFTextBox ID="txtNameOffer" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                    runat="server" IsRequired="true" ValidationGroup="OfferAddNew"></asp:ABFTextBox>

                                <asp:ABFTextBox ID="txtDateFromOffer" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                                    DataType="Date" runat="server" ValidationGroup="OfferAddNew"></asp:ABFTextBox>

                                <asp:ABFTextBox ID="txtOfferPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" MaxValue="100" DataType="Decimal" ValidationGroup="OfferAddNew"></asp:ABFTextBox>

                            </div>
                            <div class="left_col">

                                <asp:ABFTextBox ID="txtQuantityOffer" CssClass="field" LabelText="<%$Resources:Labels,Quantity %>"
                                    runat="server" IsRequired="true" ValidationGroup="OfferAddNew"></asp:ABFTextBox>
                                <asp:ABFTextBox ID="txtDateToOffer" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                                    DataType="Date" runat="server" ValidationGroup="OfferAddNew"></asp:ABFTextBox>

                                <asp:ABFTextBox ID="txtOfferCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" DataType="Decimal" ValidationGroup="OfferAddNew"></asp:ABFTextBox>

                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="OfferAddNew" />
                        </div>
                        <br />
                        <div class="btnDiv">
                            <asp:Button ID="btnSaveOffer" CssClass="button default_button" runat="server" OnClick="btnSaveOffer_Click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="OfferAddNew" />
                            <asp:Button ID="btnClearOffer" runat="server" CssClass="button" OnClick="btnClearOffer_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>




                <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:ModalPopupExtender ID="mpeOfferCustomer" runat="server" TargetControlID="HiddenField2"
                    PopupControlID="pnlOfferCustomer" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="500">
                </asp:ModalPopupExtender>
                <%--offer  customer--%>
                <asp:Panel ID="pnlOfferCustomer" CssClass="pnlPopUp" runat="server" Width="500">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="Button1"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">
                            <div class="right_col">

                                <asp:ABFTextBox ID="txtCoupon" runat="server" LabelText="Coupon"
                                    DataType="FreeString" ValidationGroup="OfferCustomerAddNew"></asp:ABFTextBox>


                                <asp:AutoComplete runat="server" ID="acName" ServiceMethod="GetContactNames" LabelText="<%$Resources:Labels,Name %>"
                                    KeepTextWhenNoValue="true"></asp:AutoComplete>
                            </div>
                            <div class="left_col">

                                <asp:ABFTextBox ID="txtParcentDiscountOfferCustomer" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    DataType="Int" ValidationGroup="OfferCustomerAddNew"></asp:ABFTextBox>

                                <asp:ABFTextBox ID="txtCashDiscountOfferCustomer" Style="display: none" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" DataType="Decimal" ValidationGroup="OfferCustomerAddNew"></asp:ABFTextBox>
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="OfferCustomerAddNew" />
                        </div>
                        <br />
                        <asp:ABFGridView runat="server" ID="gvCustomerOffer" GridViewStyle="BlueStyle" DataKeyNames="ID"
                            OnPageIndexChanging="gvCustomerOffer_PageIndexChanging"
                            OnSelectedIndexChanging="gvCustomerOffer_SelectedIndexChanging"
                            OnRowDeleting="gvCustomerOffer_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
                                <%--  <asp:BoundField DataField="CashDiscount" HeaderText="<%$Resources:Labels,PercentageValue %>" />--%>
                                <asp:BoundField DataField="ParcentageDiscount" HeaderText="<%$Resources:Labels,PercentageValue %>" />
                                <asp:BoundField DataField="Coupon" HeaderText="Coupon" />
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
                        <div class="btnDiv">
                            <asp:Button ID="btnOfferCustomer" CssClass="button default_button" runat="server" OnClick="btnOfferCustomer_Click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="OfferCustomerAddNew" />
                            <asp:Button ID="Button3" runat="server" CssClass="button"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>


                <asp:HiddenField ID="HiddenField3" runat="server" />
                <asp:ModalPopupExtender ID="mpeOfferItems" runat="server" TargetControlID="HiddenField3"
                    PopupControlID="pnlOfferItems" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="500">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOfferItems" CssClass="pnlPopUp" runat="server" Width="500">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="Button2"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">
                            <div class="right_col">

                                <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                    LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                    AutoPostBack="true"></asp:AutoComplete>

                                <asp:AutoComplete runat="server" ID="acItemsame" ServiceMethod="GetItems" KeepTextWhenNoValue="true"
                                    LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>

                            </div>
                            <div class="left_col">

                                <asp:ABFTextBox ID="txtParcentDiscountOfferItems" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" MaxValue="100" DataType="Decimal" ValidationGroup="OfferItemsAddNew"></asp:ABFTextBox>

                                <asp:ABFTextBox ID="txtCashDiscountOfferItems" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" DataType="Decimal" ValidationGroup="OfferItemsAddNew"></asp:ABFTextBox>
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="OfferItemsAddNew" />
                        </div>
                        <br />
                        <asp:ABFGridView runat="server" ID="gvItemsOffer" GridViewStyle="BlueStyle" DataKeyNames="ID"
                            OnPageIndexChanging="gvItemsOffer_PageIndexChanging"
                            OnSelectedIndexChanging="gvItemsOffer_SelectedIndexChanging"
                            OnRowDeleting="gvItemsOffer_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
                                <asp:BoundField DataField="CashDiscount" HeaderText="<%$Resources:Labels,PercentageValue %>" />
                                <asp:BoundField DataField="ParcentageDiscount" HeaderText="<%$Resources:Labels,PercentageValue %>" />
                                <asp:BoundField DataField="NameItem" HeaderText="اسم العميل" />
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
                        <div class="btnDiv">
                            <asp:Button ID="btnOfferItems" CssClass="button default_button" runat="server" OnClick="btnOfferItems_Click1"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="OfferItemsAddNew" />
                            <asp:Button ID="Button5" runat="server" CssClass="button"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>

            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="طرق التوصيل">
            <ContentTemplate>




                <asp:LinkButton ID="lnkAddNewTypeTran" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <br />
                <asp:ABFGridView runat="server" ID="gvTypeTranTypeTransporter" GridViewStyle="GrayStyle" DataKeyNames="ID,NameType"
                    OnRowDeleting="gvTypeTranTypeTransporter_RowDeleting" OnPageIndexChanging="gvTypeTranTypeTransporter_PageIndexChanging"
                    OnSelectedIndexChanging="gvTypeTranTypeTransporter_SelectedIndexChanging">
                    <Columns>

                        <asp:BoundField DataField="NameType" HeaderText="<%$Resources:Labels,Name %>" />





                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">


                            <ItemTemplate>
                                <asp:CheckBox runat="server" Checked='<%# bool.Parse( Eval("Favorite").ToString()) %>' Width="15" Height="15"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">


                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" Visible='<%# Convert.ToBoolean(Eval("IsActive")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
                <asp:HiddenField ID="hfmpeCreateNewTypeTransporter" runat="server" />
                <asp:ModalPopupExtender ID="mpeCreateNewTypeTransporter" runat="server" TargetControlID="lnkAddNewTypeTran"
                    PopupControlID="pnlAddTypeTransporter" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="200">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlAddTypeTransporter" CssClass="pnlPopUp" runat="server" Width="300">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="Button4" OnClick="ClosePopup_Click"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">


                            <asp:ABFTextBox ID="txtNameTypeTransporter" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                runat="server" IsRequired="true" ValidationGroup="AddNewTypeTransporter"></asp:ABFTextBox>

                            <asp:CheckBox runat="server" ID="chkIsFavorit" />
                            المفضلة
                
               
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary1TypeTransporter" runat="server" ValidationGroup="AddNewTypeTransporter" />
                        </div>
                        <div class="btnDivTypeTransporter">
                            <asp:Button ID="btnSaveNewTypeTransporter" CssClass="button default_button" runat="server" OnClick="btnSaveNewTypeTransporter_click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNewTypeTransporter" />
                            <asp:Button ID="BtnClearNewTypeTransporter" runat="server" CssClass="button" OnClick="BtnClearNewTypeTransporter_click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>







            </ContentTemplate>
        </asp:TabPanel>


        <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="إعدادات الطابعة">
            <ContentTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:HiddenField ID="HiddenField4" runat="server" />
                <br />
                <asp:ABFGridView runat="server" ID="gvPrinter" GridViewStyle="BlueStyle" DataKeyNames="ID,PrinterName"
                    OnRowDeleting="gvPrinter_RowDeleting" OnPageIndexChanging="gvPrinter_PageIndexChanging"
                    OnSelectedIndexChanging="gvPrinter_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="PrinterName" HeaderText="<%$Resources:Labels,Name %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>


                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="LinkButton1"
                    PopupControlID="Panel1" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="200">
                </asp:ModalPopupExtender>
                <asp:Panel ID="Panel1" CssClass="pnlPopUp" runat="server" Width="400">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="Button6" OnClick="Button1_Click"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">


                        <asp:ABFTextBox ID="txtPrinterName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                            runat="server" IsRequired="true" ValidationGroup="AddNewPrinter"></asp:ABFTextBox>



                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="AddNewPrinter" />
                        </div>
                        <br />
                        <div class="btnDiv">
                            <asp:Button ID="Button7" CssClass="button default_button" runat="server" OnClick="Button2_Click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNewPrinter" />
                            <asp:Button ID="Button8" runat="server" CssClass="button" OnClick="Button3_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:TabPanel>



        <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="إعدادات عامة لنقطة البيع">
            <ContentTemplate>
                <div class="form" style="width: 70%;">
                    <div class="right_col">
                        <br />
                        <asp:CheckBox ID="chkItemDeleted" runat="server" /><span>السماح حذف صنف من القائمة</span><br />
                        <br />
                        <asp:CheckBox ID="chkDiscount" runat="server" /><span>إظهار الخصم</span><br />
                        <br />
                        <asp:CheckBox ID="chkClogingDays" runat="server" /><span> إعتماد الاغلاق الاجباري </span>
                        <br />
                        <br />
                        <label>الضريبة</label>
                        <asp:DropDownList ID="ddlTypeTax" runat="server">
                            <asp:ListItem Text="بدون ضريبة" Value="0"></asp:ListItem>
                            <asp:ListItem Text="شامل ضريبة" Value="1"></asp:ListItem>
                            <asp:ListItem Text="ضريبة" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames"
                            IsRequired="true"
                            ValidationGroup="SaveCompanyData" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>
                        <br />
                        <span>ربط نقطة البيع بي قاعدة بيانات       </span>
                        <asp:DropDownList ID="ddlDatabase" runat="server" Width="120">
                        </asp:DropDownList>



                    </div>
                    <div style="clear: both;">
                    </div>
                </div>

                <br />
                <br />
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="SaveCompanyData" />
                </div>
                <div class="btnDiv">
                    <asp:Button ID="btnSaveCompanyData" CssClass="button shortcut_save" runat="server"
                        OnClick="btnSaveCompanyData_Click" Text="<%$ Resources:Labels, Save %>" ValidationGroup="SaveCompanyData" />
                    <br />
                    <br />
                </div>
                <br />
                <br />



            </ContentTemplate>
        </asp:TabPanel>







        <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="توصيف الاصناف">
            <ContentTemplate>
                <asp:LinkButton ID="lnkDescription" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:HiddenField ID="HiddenField5" runat="server" />
                <br />
                <asp:ABFGridView runat="server" ID="gvDescription" GridViewStyle="BlueStyle" DataKeyNames="Id,Description"
                    OnRowDeleting="gvDescription_RowDeleting"
                    OnPageIndexChanging="gvDescription_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="<%$Resources:Labels,Name %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>


                <asp:ModalPopupExtender ID="mpeDescription" runat="server" TargetControlID="lnkDescription"
                    PopupControlID="pnlDescription" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="200">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlDescription" CssClass="pnlPopUp" runat="server" Width="400">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="Button9"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">


                        <asp:ABFTextBox ID="txtDescription" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                            runat="server" IsRequired="true" ValidationGroup="AddNewDescription"></asp:ABFTextBox>



                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="AddNewDescription" />
                        </div>
                        <br />
                        <div class="btnDiv">
                            <asp:Button ID="btnSaveDescription" CssClass="button default_button" runat="server" OnClick="btnSaveDescription_Click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNewDescription" />
                            <asp:Button ID="btnClearDescription" runat="server" CssClass="button"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:TabPanel>



        <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="الوصف">
            <ContentTemplate>
                <asp:LinkButton ID="lnkDescriptionReady" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:HiddenField ID="HiddenField6" runat="server" />
                <br />
                <asp:ABFGridView runat="server" ID="gvDescriptionReady" GridViewStyle="BlueStyle" DataKeyNames="Id,Description"
                    OnRowDeleting="gvDescriptionReady_RowDeleting"
                    OnPageIndexChanging="gvDescriptionReady_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="<%$Resources:Labels,Name %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>


                <asp:ModalPopupExtender ID="mpeDescriptionReady" runat="server" TargetControlID="lnkDescriptionReady"
                    PopupControlID="pnlDescriptionReady" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
                    BehaviorID="showPopUp" Y="200">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlDescriptionReady" CssClass="pnlPopUp" runat="server" Width="400">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="btnCloseDescriptionReady"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">


                        <asp:ABFTextBox ID="txtDescriptionReady" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                            runat="server" IsRequired="true" ValidationGroup="AddNewDescriptionReady"></asp:ABFTextBox>



                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary8" runat="server" ValidationGroup="AddNewDescriptionReady" />
                        </div>
                        <br />
                        <div class="btnDiv">
                            <asp:Button ID="btnDescriptionReady" CssClass="button default_button" runat="server" OnClick="btnDescriptionReady_Click"
                                Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNewDescriptionReady" />
                            <asp:Button ID="btnCancelDescriptionReady" runat="server" CssClass="button"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:TabPanel>










    </asp:TabContainer>

</asp:Content>
