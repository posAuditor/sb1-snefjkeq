<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Items.aspx.cs" Inherits="Inv_Items" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">           
        $(document).ready(function () {
            $('#chkIsUseTax').change(function () {
                //console.log(this.checked);                                
                setEnabled("#chkIsTaxIncludedInPurchase", this.checked);
                setEnabled("#chkIsTaxIncludedInSale", this.checked);
            })
        });

        function mzFun() {
            console.log(this);
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cph" runat="Server">



    <div class="InvoiceHeader">


        <asp:Nav runat="server" ID="ucNav" VisibleText="false" />


    </div>

    <div style="clear: both;">
    </div>
    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" Style="min-width: 700px;">

        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$Resources:Labels,BasicData %>">
            <ContentTemplate>
                <div class="form" style="width: 600px;">
                    <div class="right_col">

                        <asp:ABFTextBox ID="txtName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                            IsRequired="true" ValidationGroup="NewItem"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtNameEn" runat="server" LabelText="<%$Resources:Labels,NameEN %>"
                            ValidationGroup="NewItem"></asp:ABFTextBox>
                        <asp:AutoComplete ID="acCategory" ServiceMethod="GetItemsCategories" LabelText="<%$Resources:Labels,ParentCategory %>"
                            runat="server" IsRequired="true" ValidationGroup="NewItem" on></asp:AutoComplete>
                       
                      <%--  
                         <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Cost %>" MinValue="0.00000001"
                            IsRequired="true" ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtDefaultPrice" runat="server" LabelText="<%$Resources:Labels,Price %>" MinValue="0.00000001"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>
                      --%>
                           <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Cost %>" MinValue="0.00000001"
                            IsRequired="true" ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtlastPurchasePrice" runat="server" LabelText="<%$Resources:Labels,LastPurchasePrice %>" MinValue="0"
                            ValidationGroup="NewItem" DataType="Decimal" Enabled="false"></asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtAveragePurchasePrice" runat="server" LabelText="<%$Resources:Labels,AveragePurchasePrice %>" MinValue="0"
                            ValidationGroup="NewItem" DataType="Decimal" Enabled="false"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtDefaultPrice" runat="server" LabelText="<%$Resources:Labels,Price %>" MinValue="0.00000001"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>



                        <asp:ABFTextBox ID="txtMiSalePrice" runat="server" LabelText="<%$Resources:Labels,MiSalePrice %>" MinValue="0"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>

                        
                          <asp:ABFTextBox ID="txtMinQty" runat="server" LabelText="<%$Resources:Labels,MinQty %>"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtMaxQty" runat="server" LabelText="<%$Resources:Labels,MaxQty %>"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>

                        <label>
                            <%=Resources.Labels.Logo %></label>
                        <asp:FileUpload ID="fpLogo" runat="server" Width="200" accept="image/jpeg"></asp:FileUpload>
                        <asp:Button ID="btnUploadImage" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>"
                            OnClick="btnUploadImage_Click" Style="min-width: 0px" />
                        <br />
                        <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                            ImageUrl="~/Images/no_photo.png" />
                    </div>
                    <div class="left_col">
                        <%-- <asp:AutoComplete ID="acItemsNotCategory"  ValidationGroup="NewItem"  ServiceMethod="GetItems" LabelText="<%$Resources:Labels,ArticleDescribedLabel %>"
                            runat="server" ></asp:AutoComplete>--%>
                        <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                            IsRequired="true" ValidationGroup="NewItem"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtCodeItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>" CssClass="barcode"
                            ValidationGroup="NewItem"></asp:ABFTextBox>

                        <label>
                            <%=Resources.Labels.ItemType %></label>
                        <asp:DropDownList ID="ddlItemType" runat="server">
                            <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                        </asp:DropDownList>


                        <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="NewItem"></asp:ABFTextBox>

                        <asp:AutoComplete ID="acTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                            runat="server"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtMaxDicountCash" runat="server" LabelText="أعلى خصم نقدي"
                            ValidationGroup="NewItem" DataType="Decimal" MinValue="0"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtMaxDuscountParcent" runat="server" LabelText="أعلى خصم نسبة"
                            ValidationGroup="NewItem" MinValue="0" DataType="Decimal"></asp:ABFTextBox>




                        <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                            TextMode="MultiLine" Height="100"></asp:ABFTextBox>

                        <asp:AutoComplete ID="acPrinterName" ServiceMethod="GetPrintersName" LabelText="<%$Resources:Labels,PrinterName %>"
                            runat="server" ValidationGroup="NewItem"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtQtyItems" runat="server" LabelText="عدد الاصناف المسموح بها للعرض " MinValue="0" MaxValue="10" DataType="Int"></asp:ABFTextBox>
                          <asp:AutoComplete runat="server" ID="acAccountRelated" ServiceMethod="GetChartOfAccountsCheledronly"
                                LabelText="اسم الحساب"></asp:AutoComplete>
                        <br />

                         <br />
                        <asp:CheckBox ID="chkIsBalance" runat="server" /><span>   صنف ميزان   </span>
                        <br />
                        <asp:CheckBox ID="chkItemCuisine" runat="server" /><span> صنف مطبخ   </span>
                         <br />
                         <asp:CheckBox ID="CheHideItem" runat="server" /><span>   اخفاء  </span>
                        <br />
                        <asp:CheckBox ID="chkIsUseTax" runat="server" ClientIDMode="Static" /><span>استخدام ضريبة القيمة المضافة</span>
                        <br />                        
                        <asp:CheckBox ID="chkIsTaxIncludedInPurchase" runat="server" ClientIDMode="Static" Enabled="false" /><span>سعر الشراء شامل الضريبة</span>
                        <br />
                        <asp:CheckBox ID="chkIsTaxIncludedInSale" runat="server" ClientIDMode="Static" Enabled="false" /><span>سعر البيع شامل الضريبة</span>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,Prices %>">
            <ContentTemplate>
                <div class="form" style="width: 710px;">
                    <asp:Panel runat="server" ID="Panel1" DefaultButton="btnAddaddPrice" CssClass="right_col">
                        <asp:AutoComplete runat="server" ID="acPriceName" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,PriceType %>" IsBtnPlus="true" ControlName="TabContainer1_TabPanel1_lnkAddNewPriceName"
                            IsRequired="true" ValidationGroup="addPrice"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkAddNewPriceName" runat="server" Style="display: none" CssClass="PlusBtn">[+]</asp:LinkButton>
                        <asp:ABFTextBox ID="txtPrice" runat="server" LabelText="<%$Resources:Labels,Price %>" MinValue="0"
                            DataType="Decimal" IsRequired="true" ValidationGroup="addPrice"></asp:ABFTextBox>


                        <asp:AutoComplete runat="server" ID="acUom_ID" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,BiggerUnit %>" IsRequired="true" ValidationGroup="addPrice"></asp:AutoComplete>

                        <div style="clear: both"></div>
                        <asp:Button ID="btnAddaddPrice" CssClass="button" runat="server" OnClick="btnaddPrice_click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="addPrice" />
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="addPrice" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvPrices" GridViewStyle="GrayStyle" DataKeyNames="ID,PriceName_ID,Uom_ID"
                    OnRowDeleting="gvPrices_RowDeleting" OnPageIndexChanging="gvPrices_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="PriceName" HeaderText="<%$Resources:Labels,PriceType %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Price %>" />
                        <asp:BoundField DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" />
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

        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$Resources:Labels,Units %>">
            <ContentTemplate>
                <div class="form" style="width: 700px;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acSmallestUnit" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,SmallestUnit %>" IsRequired="true" ValidationGroup="NewItem" IsBtnPlus="true" ControlName="TabContainer1_TabPanel3_lnkAddNewBiggerUnit"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkbtnAddNewUnit" runat="server" Style="display: none;" CssClass="PlusBtn">[+]</asp:LinkButton>
                    </div>
                    <div style="clear: both;"></div>
                    <hr />
                    <asp:Panel runat="server" ID="pnlBiggerUnit" DefaultButton="btnAddUnit" CssClass="right_col">
                        <div class="form" style="width: 600px;">
                            <div class="right_col">



                                <%--      <div style="display: flex;">
                                    <div style="flex: 1; width: 90%">--%>

                                <asp:AutoComplete runat="server" ID="acBiggerUnit" ServiceMethod="GetGeneralAtt" IsBtnPlus="true" ControlName="TabContainer1_TabPanel3_lnkAddNewBiggerUnit"
                                    LabelText="<%$Resources:Labels,BiggerUnit %>" IsRequired="true" ValidationGroup="addUnit"></asp:AutoComplete>
                                <%-- </div>
                                    <div style="width: 10%; padding-top: 14%;">--%>
                                <asp:LinkButton ID="lnkAddNewBiggerUnit" Style="display: none;" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                                <%--  </div>

                                </div>--%>

                                <asp:ABFTextBox ID="txtUnitRatio" runat="server" LabelText="<%$Resources:Labels,Ratio %>" Width="96%"
                                    DataType="Decimal" IsRequired="true" ValidationGroup="addUnit" MinValue="0"></asp:ABFTextBox>

                            </div>
                            <div class="left_col">

                                <asp:ABFTextBox ID="txtPriceUnite" runat="server" LabelText="<%$Resources:Labels,Price %>" Width="70%"
                                    DataType="Decimal" IsRequired="true" ValidationGroup="addUnit" MinValue="0.0001"></asp:ABFTextBox>


                                <asp:ABFTextBox ID="txtBarcodeUnite" runat="server" LabelText="<%$Resources:Labels,Barcode %>" Width="70%"
                                    DataType="Decimal" IsRequired="true" ValidationGroup="addUnit" MinValue="0.0001"></asp:ABFTextBox>
                                <br />
                                <br />
                                <asp:CheckBox ID="chkIsFavorit" runat="server" /><span>  المفضلة   </span>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>

                        <asp:Button ID="btnAddUnit" CssClass="button" runat="server" OnClick="btnAddUnit_click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="addUnit" />
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="addUnit" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvUnits" GridViewStyle="GrayStyle" DataKeyNames="ID,Unit_ID"
                    OnRowDeleting="gvUnits_RowDeleting" OnPageIndexChanging="gvUnits_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="UnitName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Ratio" HeaderText="<%$Resources:Labels,Ratio %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Price %>" />
                        <asp:BoundField DataField="Barcode" HeaderText="<%$Resources:Labels,Barcode %>" />
                        <asp:BoundField DataField="IsFavorite" HeaderText="الوحدة المفضلة" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditUnit" Text="<%$ Resources:Labels, Edit %>" OnClick="lnkEditUnit_OnClick" CommandArgument='<%#Eval("ID") %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>

        <%-- <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$Resources:Labels,RawItems %>">--%>
        <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$Resources:Labels,RawItems1 %>">
            <ContentTemplate>
                <div class="form" style="width: 600px;">
                    <div class="form" style="width: 700px;">
                        <div class="right_col">
                                <asp:ABFTextBox ID="txtQuantityProductRaw" runat="server" LabelText="<%$Resources:Labels,Quantity %>" Width="20%"
                                    DataType="Decimal" IsRequired="true" ValidationGroup="addRawMat" MinValue="0.0001"></asp:ABFTextBox>
                        </div>
                    </div>

                    <div style="clear: both;"></div>
                    <hr />
                    <asp:Panel runat="server" ID="pnlRawMat" DefaultButton="btnAddRawMat" CssClass="right_col">
                        <asp:AutoComplete runat="server" ID="acRawMat" ServiceMethod="GetItems"
                            LabelText="<%$Resources:Labels,RawItem %>" IsRequired="true" ValidationGroup="addRawMat"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtRawMatQuantity" runat="server" LabelText="<%$Resources:Labels,Quantity %>" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="addRawMat" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:Button ID="btnAddRawMat" CssClass="button" runat="server" OnClick="btnAddRawMat_click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="addRawMat" />

                        <asp:Button ID="btnEdit" CssClass="button" runat="server" OnClick="btnEdit_OnClick" Style="min-width: 0px" Visible="False"
                            Text="<%$ Resources:Labels, Edit %>" ValidationGroup="addRawMat" />


                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="addRawMat" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvRawMats" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvRawMats_RowDeleting" OnPageIndexChanging="gvRawMats_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="المسلسل">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" Text="<%$ Resources:Labels, Edit %>" OnClick="lnkEdit_OnClick" CommandArgument='<%#Eval("ID") %>' runat="server" />

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


        <asp:TabPanel ID="TabPanel8" runat="server" HeaderText="التركيبة">
            <ContentTemplate>
                <div class="form" style="width: 600px;">
                    <asp:Panel runat="server" ID="Panel3" DefaultButton="btnAddRawCompose" CssClass="right_col">

                        <asp:AutoComplete runat="server" ID="acItemRawCompose" ServiceMethod="GetItems"
                            LabelText="<%$Resources:Labels,Name %>" IsRequired="true" ValidationGroup="addRawCompose"></asp:AutoComplete>

                        <asp:ABFTextBox ID="txtQtyRawCompose" runat="server" LabelText="<%$Resources:Labels,Quantity %>" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="addRawCompose" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:Button ID="btnAddRawCompose" CssClass="button" runat="server" OnClick="btnRawCompose_Click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="addRawCompose" />

                        <asp:Button ID="btnEditCompose" CssClass="button" runat="server" OnClick="btnEditCompose_Click" Style="min-width: 0px" Visible="False"
                            Text="<%$ Resources:Labels, Edit %>" ValidationGroup="addRawCompose" />

                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="addRawCompose" />
                        </div>

                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvRawCompose" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvRawCompose_RowDeleting" OnPageIndexChanging="gvRawCompose_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="المسلسل">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditRawCompose" Text="<%$ Resources:Labels, Edit %>" OnClick="lnkEditRawCompose_Click" CommandArgument='<%#Eval("ID") %>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnImageRowDelete" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>


            </ContentTemplate>
        </asp:TabPanel>


        <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="شرائح العمولات">
            <ContentTemplate>
                <div class="form" style="width: 600px;">

                    <asp:Panel runat="server" ID="Panel33" DefaultButton="btnSaveCommission" CssClass="right_col">
                        <%-- <asp:AutoComplete runat="server" ID="acItemCommission" ServiceMethod="GetItems"
                            LabelText="<%$Resources:Labels,RawItem %>" IsRequired="true" ValidationGroup="Commission"></asp:AutoComplete>--%>
                        <asp:ABFTextBox ID="txtFirstValue" runat="server" LabelText="من" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtLastValue" runat="server" LabelText="إالى" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtPercent" runat="server" LabelText="النسبة" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:Button ID="btnSaveCommission" CssClass="button" runat="server" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="Commission" OnClick="btnSaveCommission_OnClick" />


                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummaryCommission" runat="server" ValidationGroup="Commission" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvCommission" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvCommission_OnRowDeleting" OnPageIndexChanging="gvCommission_OnPageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="المسلسل">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="FirstValue" HeaderText="من" />
                        <asp:BoundField DataField="LastValue" HeaderText="الى" />
                        <asp:BoundField DataField="ParcentValue" HeaderText="النسبة" />

                        <%--<asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCommissionEdit" OnClick="lnkCommissionEdit_OnClick" Text="<%$ Resources:Labels, Edit %>"  CommandArgument='<%#Eval("ID") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

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

        <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="<%$Resources:Labels,MinQty %>">
            <ContentTemplate>
                <div class="form" style="width: 600px;">

                    <asp:Panel runat="server" ID="Panel2" DefaultButton="btnAddMinQty" CssClass="right_col">
                        <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores"
                            LabelText="<%$Resources:Labels,Store %>" IsRequired="true" ValidationGroup="addMinQty"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtMinQty_Multi" runat="server" LabelText="<%$Resources:Labels,MinQty %>" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="addMinQty"></asp:ABFTextBox>

                        <asp:Button ID="btnAddMinQty" CssClass="button" runat="server" OnClick="btnAddMinQty_click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="addMinQty" />
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="addMinQty" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvMinQtys" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvMinQtys_RowDeleting" OnPageIndexChanging="gvMinQtys_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Labels,Store %>" />
                        <asp:BoundField DataField="MinQty" HeaderText="<%$Resources:Labels,Quantity %>" />
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





        <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="<%$Resources:Labels,Itemdescribed %>" Style="display: none;" Visible="false">
            <ContentTemplate>
                <div class="form" style="width: 600px;">

                    <asp:Panel runat="server" ID="Panel6" DefaultButton="btnSaveDescribred" CssClass="right_col">

                        <asp:ABFTextBox ID="txtItemDescribed" runat="server" LabelText="<%$Resources:Labels,Name %>" Width="70%"
                            IsRequired="true" ValidationGroup="Itemdescribed"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtPriceItemDescribed" runat="server" LabelText="<%$Resources:Labels,Price %>" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Itemdescribed"></asp:ABFTextBox>

                        <asp:Button ID="btnSaveDescribred" CssClass="button" runat="server" OnClick="btnSaveDescribred_Click" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="Itemdescribed" />
                        <asp:Label runat="server" ID="lblExplain" ForeColor="Red" Font-Size="12" Text="يجب حفظ الصنف قبل انشاء مادة موصوفة له"></asp:Label>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="Itemdescribed" />
                        </div>
                    </asp:Panel>
                </div>
                <div style="clear: both"></div>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvItemdescribed" GridViewStyle="GrayStyle" DataKeyNames="ID" OnPageIndexChanged="gvItemdescribed_PageIndexChanged" OnPageIndexChanging="gvItemdescribed_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="DescribedName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Price %>" />

                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>


    </asp:TabContainer>
    <div class="validationSummary">
        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="NewItem" />
    </div>
    <br />
    <div class="btnDiv">
        <asp:Button ID="btnSave" CssClass="button shortcut_save" runat="server" OnClick="btnSave_click"
            Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewItem" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>
    <asp:NewAttribute ID="ucNewPrice" runat="server" Title="<%$Resources:Labels,Prices %>"
        AttributeType_ID="2" TargetControlID="cph$TabContainer1$TabPanel1$lnkAddNewPriceName"
        OnNewAttributeCreated="ucNewPrice_NewAttributeCreated"></asp:NewAttribute>
    <asp:NewAttribute ID="ucNewUnit" runat="server" Title="<%$Resources:Labels,Units %>"
        AttributeType_ID="14" TargetControlID="cph$TabContainer1$TabPanel3$lnkbtnAddNewUnit"
        OnNewAttributeCreated="ucNewUnit_NewAttributeCreated"></asp:NewAttribute>
    <asp:NewAttribute ID="ucNewBiggerUnit" runat="server" Title="<%$Resources:Labels,Units %>"
        AttributeType_ID="14" TargetControlID="cph$TabContainer1$TabPanel3$lnkAddNewBiggerUnit"
        OnNewAttributeCreated="ucNewBiggerUnit_NewAttributeCreated"></asp:NewAttribute>
</asp:content>
