<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" 
    AutoEventWireup="true" CodeFile="InvoiceForm.aspx.cs" Inherits="Sales_InvoiceForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/OperationsView.ascx" TagPrefix="asp" TagName="OperationsView" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
        ::selection {
            background: #f7a494;
        }

        ::-moz-selection {
            background: #f7a494;
        }



        @font-face {
            font-family: 'awesonne';
            src: url('../Fonts/NFont/font/awesonne.eot?30429');
            src: url('../Fonts/NFont/font/awesonne.eot?30429#iefix') format('embedded-opentype'), url('../Fonts/NFont/font/awesonne.woff?30429') format('woff'), url('../Fonts/NFont/font/awesonne.ttf?30429') format('truetype'), url('../Fonts/NFont/font/awesonne.svg?30429#awesonne') format('svg');
            font-weight: normal;
            font-style: normal;
        }

        .hiddencol {
            display: none;
        }


        .CellTable {
            font-size: 13px;
            font-weight: bold;
            border-bottom: 1px solid black;
            border-right: 1px solid black;
            border-left: 1px solid black;
        }

        .CellTableHeader {
            font-size: 13px;
            font-weight: bold;
            border-bottom: 1px solid black;
            border-right: 1px solid black;
            border-left: 1px solid black;
        }

        .tb {
            margin: 4px;
            background-color: white !important;
        }

        .cls {
            border: 0px !important;
        }


        .colWLarge {
            width: 24%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colW1Large {
            width: 20%;
            color: black;
            border: 2px solid black;
            text-align: center;
            height: 30px;
        }

        .colW {
            width: 9%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colWS {
            width: 6%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colWx {
            width: 10%;
            color: black;
            border: 1px solid white;
            text-align: center;
        }

        .colW1 {
            width: 10%;
            color: black;
            border: 2px solid black;
            text-align: center;
            height: 30px;
        }

        .colSmall {
            width: 3%;
            color: black;
            /*border: 2px solid transparent;*/
            text-align: center;
            height: 34px;
        }



        /*Small gridWoutHeader view grdWoutHeader*/
        .grdWoutHeader {
            -border-style: solid;
            -border-color: #444;
            -border-width: 0px;
            background: #d0d0d0;
            width: 100%;
            font-size: 15px;
        }


        .grdWoutHeader11 {
            /*-border-style: solid;
            -border-color: #444;
            -border-width: 1px;
            background: #d0d0d0;*/
            width: 100%;
            /*font-size: 15px;*/
            /*border-collapse: separate !important;*/
        }

        .gridWoutHeader_mobile {
            table-layout: fixed;
            word-wrap: break-word;
        }

        .grdWoutHeader_header th {
            background: #f9f9f9;
            height: 24px;
            border-bottom: solid 1px silver;
            /*padding: 2px 3px 1px 3px;*/
            font-size: 12px;
            display: none;
        }

        .grdWoutHeader_fixedheader {
            position: absolute;
            background-color: #D0D0D0;
            /*margin-top: -12px;
            margin-right: -2px;
            border-top: solid 1px;
            border-right: solid 1px;
            border-left: solid 1px;
            width: 662px;
            height: 40px;*/
        }

        .grdWoutHeader_empty td {
            /*border: solid 1px #D0D0D0;*/
            color: Red;
            /*padding: 10px;*/
            background: #F7F7F7;
            font-weight: bold;
        }

        .hide_column {
            display: none;
        }

        .align_column {
            text-align: right;
        }

        .grdWoutHeader input[type=image], .gridWoutHeader input[type=image] {
            border: 0px;
            background: transparent;
            box-shadow: none;
        }

        a.gridWoutHeader-expand, a:focus.gridWoutHeader-expand {
            background: url(../images/gridWoutHeaderview_expand-ar.gif) no-repeat !important;
            width: 16px;
            height: 16px;
            border: 0px;
            cursor: pointer;
            color: transparent;
        }

        a.gridWoutHeader-collapse, a:focus.gridWoutHeader-collapse {
            background: url(../images/gridWoutHeaderview_collapse-ar.gif) no-repeat !important;
            width: 16px;
            height: 16px;
            border: 0px;
            cursor: pointer;
            color: transparent;
        }

        .AddControl {
            width: 75px !important;
            color: black;
        }

        .xx {
        }

            .xx a {
                text-decoration: none;
                display: inline-block;
                padding: 8px 16px;
            }

                .xx a:hover {
                    background-color: #b579e2;
                    color: black;
                }

        .previous {
            background-color: #f1f1f1;
            color: black;
        }

        .next {
            background-color: #842dc5;
            color: white;
        }

        .round {
            border-radius: 50%;
        }

        .Row {
            display: table;
            width: 70%;
            table-layout: fixed;
            border-spacing: 10px;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }

        .Column {
            display: table-cell;
            /*background-color: red;*/
        }
    </style>

    <script type="text/javascript">

        function focusOnNext(e, nextControl) {

            if (e.keyCode == 13) {
                $("#" + nextControl).focus();
                return false;
            }
        }

        function DeleteItem(id) {

        }

        function getNextControl(sender) {
            var waitOne = false;
            for (i = 0; i <= form1.elements.length; i++) {
                //  if (waitOne && form1.elements[i].type == 'text') return form1.elements[i].id;
                waitOne = sender.id == form1.elements[i].id;
            }


        }
        function setFocus(event, sender) {
            if (event.keyCode == 13 || event.keyCode == 9) {
                document.getElementById(getNextControl(sender)).focus();
            }
        };

        function setSeriaFocus(e) {
            if (e.keyCode == 13) {
                document.getElementById('<%= LinkButton1.ClientID %>').click();
                document.getElementById('<%= txtQty.ClientID %>').select();
            }
        }

    </script>

</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <div class="MainInvoiceStyleDiv">

        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>

        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server" Visible="false"
                ReadOnly="true" Width="200" Style="text-align: center;">
            </asp:ABFTextBox>
            <asp:Label runat="server" Visible="false" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.Invoice %></asp:Label>

            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.FromSalesOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>

            <asp:Nav runat="server" ID="ucNav" />
            <asp:Favorit runat="server" ID="ucFavorit" />


        </div>

        <div class="InvoiceSection">
            <div class="Row">
                <div class="Column">

                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <br />
                    <label runat="server" id="lblCurrency" style="display: none">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" Style="display: none" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save" TabIndex="-1"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acStore" LabelText="<%$Resources:Labels,Store %>" ServiceMethod="GetStorPermissionsGroup"
                        IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>"></asp:AutoComplete>


                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtDeliveryDate" runat="server" ValidationGroup="Save" LabelText="تاريخ التسليم" DataType="Date" IsHideable="true"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtCustomerRepresentative" runat="server" LabelText="اسم ممثل العميل" IsHideable="true"></asp:ABFTextBox>



                </div>
                <div class="Column">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />


                    <asp:LinkButton ID="lnkAccountstatement" runat="server" CssClass="PlusBtn1">[...]</asp:LinkButton>


                    <asp:LinkButton ID="lnkAddNewCustomer" Style="display: none;" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>

                    <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames"
                        IsRequired="true" OnSelectedIndexChanged="acCustomer_SelectedIndexChanged" AutoPostBack="true" IsBtnPlus="true" ControlName="lnkAddNewCustomer"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>


                    <asp:Panel runat="server" ID="pnlCustomerMesure" Visible="False">
                        <asp:AutoComplete runat="server" ID="acCustomerMesure" ValidationGroup="Save" ServiceMethod="GetContactMesure" LabelText="<%$Resources:Labels,SalesRep %>"></asp:AutoComplete>
                    </asp:Panel>

                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>


                    <asp:ABFTextBox ID="txtCustomerName" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,CustomerName %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCustomerMobile" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,CustomerMobile %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acdrivers" ServiceMethod="Getcar" LabelText="<%$Resources:Labels,DriverNamne %>"
                        ValidationGroup="Save"></asp:AutoComplete>




                    <asp:ABFTextBox ID="txtProjectRef" runat="server" LabelText="بيانات المشروع وموقع التسليم" IsHideable="true"></asp:ABFTextBox>



                </div>
                <div class="Column">
                    <br />
                    <asp:ABFTextBox ID="txtRatio" Style="display: none" VisibleText="false" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true">
                    </asp:ABFTextBox>


                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>

                    <label runat="server" id="Label2">
                        <%=Resources.Labels.PaymentMethod %></label>
                    <asp:DropDownList ID="ddlPaymentMethode" runat="server">
                    </asp:DropDownList>

                    <span style="display: none">
                        <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccounts"
                            ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>
                    </span>
                    <asp:ABFTextBox ID="txtContactPerson" runat="server" LabelText="بيانات المستلم" IsHideable="true"></asp:ABFTextBox>
                    <label>
                        .
                    </label>
                    <asp:DropDownList runat="server" ID="ddlTvae" AutoPostBack="true" OnSelectedIndexChanged="ddlTvae_SelectedIndexChanged">

                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>

                    </asp:DropDownList>

                </div>

            </div>

        </div>

        <div class="InvoiceSection">

            <div class="container">
                <asp:ABFTextBox ID="txtBarcode" runat="server" TabIndex="0" LabelText="<%$Resources:Labels,Barcode %>" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" CssClass="barcode" IsHideable="true">
                </asp:ABFTextBox>


                <asp:AutoComplete runat="server" ID="acPriceName" ServiceMethod="GetItemPriceNames" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    LabelText="<%$Resources:Labels,PriceType %>" OnSelectedIndexChanged="acPriceName_SelectedIndexChanged"
                    AutoPostBack="true" IsHideable="true"></asp:AutoComplete>



                <span>
                    <%=Resources.Labels.AvailableQty %>: </span>

                <asp:LinkButton ID="lnkViewQty" runat="server" OnClick="lnkViewQty_Click">
                    <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
                </asp:LinkButton>




                <br />
                <span>
                    <%=Resources.Labels.LastCustomerPrice %>: </span>
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                <div class="tb">
                    <table cellspacing="0" cellpadding="0" style="width: 100%">
                        <tr>
                            <td colspan="13">


                                <div>
                                    <asp:LinkButton ID="lnkAddNewIteme" runat="server" CssClass="PlusBtn" Style="display: none;">[+]</asp:LinkButton>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">

                                                <th class="colSmall"></th>
                                                <th class="colWLarge"><%=Resources.Labels.Item %></th>
                                                <th class="colW"><%=Resources.Labels.Unit %></th>
                                                <th class="colW">
                                                    <asp:LinkButton ID="lnkGroupStoreIteme" runat="server" OnClick="lnkGroupIteme_Click"
                                                        CssClass="PlusBtn2"><%=Resources.Labels.Quantity %> </asp:LinkButton>

                                                    <asp:Label ID="lblQty" runat="server"><%=Resources.Labels.Quantity %> </asp:Label>


                                                </th>
                                                <th class="colW">
                                                    <asp:LinkButton ID="lnkGetPriceIteme" runat="server" OnClick="lnkGetPriceIteme_Click" CssClass="PlusBtn1"><%=Resources.Labels.Price %> </asp:LinkButton>
                                                    <asp:Label ID="lblPrice" runat="server"><%=Resources.Labels.Price %> </asp:Label>




                                                </th>
                                                <th class="colW"><%=Resources.Labels.PercentageDiscount %></th>
                                                <th class="colW"><%=Resources.Labels.CashDiscount %></th>
                                                <th class="colW">الصافي قبل الضريبة</th>
                                                <th class="colW"><%=Resources.Labels.GrossTotal %>
                                                    <asp:LinkButton ID="lnkInfo" runat="server">
                                                        <i class="fa fa-info" style="font-size: 15px;   color: black!important"></i>
                                                    </asp:LinkButton>
                                                </th>
                                                <th class="colW XSN" runat="server" id="ViewSN">SerialNumber</th>
                                                <th class="colSmall" runat="server" id="col_gift_Header">هدية</th>
                                                <th class="colSmall" style="color: black"><%=Resources.Labels.Add %></th>
                                                <th class="colSmall" style="color: black"><%=Resources.Labels.Edit %></th>
                                                <th class="colSmall" style="color: black"><%=Resources.Labels.Delete %></th>




                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="12">


                                <div>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">
                                                <td class="colSmall"></td>
                                                <td class="colWLarge">
                                                    <asp:AutoComplete runat="server" LabelText="<%$Resources:Labels,Item %>" ID="acItem" ServiceMethod="GetItemsExceptItemsCategories" ValidationGroup="AddItem"
                                                        OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true" IsBtnPlus="true" ControlName="lnkAddNewIteme"
                                                        VisibleText="false" CssClass="cls" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>

                                                </td>
                                                <td class="colW">
                                                    <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" Style="background-color: transparent; border-radius: 0px!important"
                                                        IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" TabIndex="2" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                        AutoPostBack="true"></asp:AutoComplete>
                                                </td>
                                                <td class="colW">

                                                    <asp:ABFTextBox ID="txtQty" runat="server" CssClass="cls" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"
                                                        MinValue="0.001" DataType="Decimal" Text="1" IsRequired="true" TabIndex="3" ValidationGroup="AddItem">
                                                    </asp:ABFTextBox>


                                                </td>

                                                <td class="colW">


                                                    <asp:ABFTextBox ID="txtCost" runat="server" CssClass="cls" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" Style="background-color: transparent; border-radius: 0px!important"
                                                        MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" TabIndex="4"  Text="السعر" VisibleText="false">
                                                    </asp:ABFTextBox>

                                                </td>
                                                <td class="colW">


                                                    <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" onfocus="javascript:this.select();" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" LabelText="<%$Resources:Labels,PercentageDiscount %>" VisibleText="false" CssClass="cls"
                                                        Style="background-color: transparent; border-radius: 0px!important" TabIndex="5"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem">
                                                    </asp:ABFTextBox>


                                                </td>
                                                <td class="colW">

                                                    <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" onfocus="javascript:this.select();" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" LabelText="<%$Resources:Labels,CashDiscount %>" VisibleText="false" CssClass="cls"
                                                        Style="background-color: transparent; border-radius: 0px!important" TabIndex="6"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem">
                                                    </asp:ABFTextBox>




                                                </td>
                                                <td class="colW">
                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRowBeforTax" runat="server" Text=""></asp:Label>


                                                </td>

                                                <td class="colW">
                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label>


                                                </td>

                                                <td class="colW XSN" runat="server" id="EditXSN">

                                                    <asp:Label ID="lblSerialNumber" Visible="false" runat="server" Text=""></asp:Label>
                                                    <asp:LinkButton ID="lnkSelectSerialNumber" OnClick="lnkSelectSerialNumber_Click" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>

                                                </td>

                                                <td class="colSmall" style="color: black" runat="server" id="col_gift_Edit">
                                                    <asp:CheckBox ID="chkCado" TabIndex="7" Width="34" runat="server" />
                                                </td>

                                                <td class="colSmall" style="color: black; text-align: center;">

                                                    <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" TabIndex="8" ValidationGroup="AddItem" runat="server">
                                                        <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important"></i>
                                                    </asp:LinkButton>

                                                    <%--   <asp:Button ID="Button7"  OnClick="btnAddItem_click"  runat="server" Text="Button" />--%>

                                                </td>

                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image2" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                                </td>
                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="10">
                                <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDataBound="gvItems_RowDataBound"
                                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$Resources:Labels,Serial %>">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-CssClass="colWLarge" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />

                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                                            DataFormatString="{0:0.####}" />
                                        <%--  <asp:BoundField  ItemStyle-CssClass="colW" DataField="UnitCost" HeaderText="<%$Resources:Labels,Price %>" DataFormatString="{0:0.########}" />
                                        --%>
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="UnitCostEvaluate" HeaderText="<%$Resources:Labels,Price %>" DataFormatString="{0:0.########}" />

                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="PercentageDiscount" HeaderText="<%$Resources:Labels,PercentageDiscount %>"
                                            DataFormatString="{0:0.########}" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="TotalCostBeforTax"
                                            DataFormatString="{0:0.########}" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="GrossTotalVirtual" HeaderText="<%$Resources:Labels,GrossTotal %>"
                                            DataFormatString="{0:0.########}" />

                                        <asp:TemplateField ItemStyle-CssClass="colW XSN">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSerialNumberView" Visible='<%# this.IsViewSerialNumber %>' CommandArgument='<%# Eval("ID").ToString() + ":" + Eval("Item_ID").ToString() %>' OnClick="lnkSerialNumberView_Click" runat="server">إضفط عنا</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGift" Style="display: none;" runat="server" Text='<%# Eval("IsGift") %>'></asp:Label>
                                                <asp:Image Width="25" Height="25" runat="server" ID="imgGift" ImageUrl="../images/git.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Edit %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="34" Visible='<%# this.DocStatus_ID==1 %>' ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="34" Visible='<%# this.DocStatus_ID==1 %>' ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                                    OnClientClick="return ConfirmSure();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:AuditorGridView>
                            </td>
                        </tr>
                        <tr>
                            <th class="colWx"></th>
                            <th class="colWx"></th>
                            <th class="colWx">
                                <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                                </asp:ABFTextBox>


                            </th>
                            <th class="colWx">
                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                                </asp:ABFTextBox>

                            </th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtAdditionals" runat="server" LabelText="<%$Resources:Labels,Additionals %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                                </asp:ABFTextBox>
                            </th>

                            <th class="colWx">
                                <asp:ABFTextBox ID="txtFirstPaid1" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>" Visible="false"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal">
                                </asp:ABFTextBox></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 30px!important; color: black"></th>
                        </tr>

                        <tr>
                            <td colspan="12">
                                <div style="clear: both">
                                </div>
                                <div class="validationSummary">
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddItem" />
                                </div>
                            </td>
                        </tr>

                        <tr style="height: 20px;">
                            <td colspan="10">
                                <br />
                            </td>
                        </tr>

                        <tr>


                            <th class="colWx"></th>
                            <th class="colWx">

                                <table>
                                    <tr>
                                        <td>
                                            <span class="lbl">
                                                <%=Resources.Labels.Total %>: </span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                                        </td>
                                    </tr>

                                </table>
                            </th>
                            <th class="colWx">
                                <table>
                                    <tr runat="server" id="taxTotal">
                                        <td>
                                            <span class="lbl">
                                                <%=Resources.Labels.TotalTax %>: </span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTotalTax" runat="server" Text="0.0000"></asp:Label>
                                        </td>
                                    </tr>
                                </table>


                            </th>
                            <th class="colWx">

                                <table>
                                    <tr>
                                        <td>
                                            <span class="lbl">
                                                <%=Resources.Labels.Additionals %>: </span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAdditionals" runat="server" Text="0.0000"></asp:Label>
                                        </td>

                                    </tr>
                                </table>
                            </th>
                            <th class="colWx" colspan="2">
                                <table>
                                    <tr runat="server" id="DiscountTotal">
                                        <td>
                                            <span class="lbl">
                                                <%=Resources.Labels.TotalDiscount %>: </span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTotalDiscount" runat="server" Text="0.0000"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                            <th class="colWx">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="lbl">
                                                <%=Resources.Labels.GrossTotal %>: </span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGrossTotal" runat="server" Text="0.0000"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 30px!important; color: black"></th>
                        </tr>
                    </table>

                </div>
            </div>

            <div class="InvoiceSection" style="display: none;">
                <div class="container">
                    <table class="table table-hover" style="border: 0 solid black;">
                        <tbody>
                            <tr>
                                <td style="width: 7%">
                                    <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                        OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true">
                                    </asp:ABFTextBox></td>
                                <td style="width: 11%"></td>

                                <td></td>

                                <td></td>
                                <td>
                                    <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>

                                </td>
                                <td style="width: 6%"></td>
                                <td style="width: 7%">

                                    <br />


                                </td>
                                <td style="width: 12%">
                                    <asp:Label runat="server" Text="<%$Resources:Labels,Price %>"></asp:Label>
                                    <br />

                                </td>
                                <td>
                                    <div class="btnDiv" style="padding-top: 24px;">
                                        <asp:LinkButton ID="btnAddItemException" OnClick="btnAddItemException_OnClick" ValidationGroup="AddItem" runat="server"><i class="btnShartcatInvoice demo-icon icon-plus-circle" style="font-size: 20px; padding-top: 10px;"></i></asp:LinkButton>
                                        <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                                            Text="<%$ Resources:Labels, Add %>" Visible="False" />
                                        <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" Visible="False" OnClick="btnAddItemGroup_click"
                                            Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                                        <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Visible="False" Text="<%$ Resources:Labels, Clear %>"
                                            OnClick="BtnClearItem_Click" />
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <asp:Panel runat="server" ID="pnlItemdescribed">
                                        <asp:AutoComplete runat="server" ID="acItemDescribed" OnSelectedIndexChanged="acItemDescribed_OnSelectedIndexChanged" LabelText="<%$Resources:Labels,Itemdescribed %>" ServiceMethod="GetItemsDescribedSales"
                                            AutoPostBack="true" ValidationGroup="AddItem"></asp:AutoComplete>
                                    </asp:Panel>
                                    <%-- </td>


                            <td>--%>

                                    <asp:Label ID="lblCapacity" CssClass="forHide" runat="server" Text="<%$Resources:Labels,Capacity %>"></asp:Label>
                                    <asp:ABFTextBox ID="txtCapacity" Enabled="False" runat="server" VisibleText="False" LabelText="<%$Resources:Labels,Capacity %>"
                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                                    </asp:ABFTextBox>



                                </td>
                                <td colspan="2">
                                    <table class="table table-hover forHide" style="border: 0 solid black;">
                                        <tr>
                                            <td style="width: 80px;">
                                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,CapacityDistribution %>"></asp:Label></td>
                                            <td>
                                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,ActualQuantity %>"></asp:Label>
                                                :<asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label></td>

                                        </tr>
                                        <tr>


                                            <td colspan="2">

                                                <asp:ABFTextBox ID="txtCapacities" OnTextChanged="txtCapacities_OnTextChanged" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                                                    ValidationGroup="AddItem" AutoPostBack="true" IsHideable="true" VisibleText="False">
                                                </asp:ABFTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</td>

                            <td>--%>
                               
                                </td>
                                <td>



                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Policy %>"></asp:Label>

                                    <asp:ABFTextBox ID="txtPolicy" runat="server" VisibleText="False"
                                        MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Policy %>" IsHideable="true">
                                    </asp:ABFTextBox>


                                </td>
                                <td></td>
                                <td>
                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Code %>"></asp:Label>

                                    <asp:ABFTextBox ID="txtCode" runat="server"
                                        MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Code %>" VisibleText="False" IsHideable="true">
                                    </asp:ABFTextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,DateRequest %>"></asp:Label>
                                    <asp:ABFTextBox ID="txtInvoiceDate" runat="server" VisibleText="False" LabelText="<%$Resources:Labels,DateRequest %>" ValidationGroup="AddItem"
                                        DataType="Date" IsHideable="true">
                                    </asp:ABFTextBox>
                                </td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="InvoiceSection" style="display: none;">
                <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem" Style="display: none;">
                    <div class="form" style="margin: auto; width: 90%;">
                        <div class="right_col">

                            <asp:AutoComplete Visible="False" runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>

                            <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                            </asp:ABFTextBox>
                            <br />
                            <%--    <span><%=Resources.Labels.ActualQuantity %> : </span>
                        <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>--%>

                            <%--  <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                            IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                            AutoPostBack="true" IsHideable="true"></asp:AutoComplete>--%>
                            <%-- LabelText="<%$Resources:Labels,BatchID %>"--%>
                            <asp:AutoComplete CssClass="hiddencol" runat="server" ID="acBatchID" ServiceMethod="GetBatches"
                                OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                            <br />
                            <%--   <span>
                                <%=Resources.Labels.AvailableQty %>: </span>
                            <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>--%>
                        </div>
                        <div class="left_col">




                            <asp:Panel runat="server" ID="Panel1" Visible="False">
                                <%--LabelText="<%$Resources:Labels,ProductionDate %>"--%>
                                <asp:ABFTextBox ID="txtProductionDate" CssClass="hiddencol" runat="server"
                                    Enabled="false" IsHideable="true">
                                </asp:ABFTextBox>

                                <%-- LabelText="<%$Resources:Labels,ExpirationDate %>"--%>
                                <asp:ABFTextBox ID="txtExpirationDate" CssClass="hiddencol" runat="server"
                                    Enabled="false" IsHideable="true">
                                </asp:ABFTextBox>

                            </asp:Panel>
                            <br />
                            <span>
                                <%=Resources.Labels.LastCustomerPrice %>: </span>
                            <asp:Label ID="lblLastCustomerPrice" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div class="form" style="margin: auto; width: 600px;">
                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                            TextMode="MultiLine" Style="width: 100%;">
                        </asp:ABFTextBox>
                    </div>
                </asp:Panel>
            </div>

            <div class="InvoiceSection" id="taxSection" runat="server" style="display: none">
                <div class="form" style="margin: auto; width: 600px;">
                    <asp:AutoComplete runat="server" ID="acTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" AutoCompleteWidth="300"
                        IsRequired="true" ValidationGroup="AddTax"></asp:AutoComplete>
                    <asp:Button ID="btnAddTax" CssClass="button" runat="server" OnClick="btnAddTax_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddTax" />
                </div>
                <asp:ABFGridView runat="server" ID="gvTaxes" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvTaxes_RowDeleting" OnPageIndexChanging="gvTaxes_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="PercentageValue" HeaderText="<%$Resources:Labels,Tax %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </div>

            <div class="InvoiceSection">
                <table class="table table-hover" style="border: 0 solid black;">
                </table>
                <div class="form" style="margin: auto;">
                    <div class="right_col">
                        <%-- <div class="form">--%>

                        <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                            TextMode="MultiLine" Width="100%">
                        </asp:ABFTextBox>
                        <%-- </div>--%>
                    </div>
                    <div class="left_col" style="width: 34%!important;">
                        <div class="right_col">
                        </div>
                        <div class="left_col">
                        </div>
                        <div class="left_col">
                            <div style="clear: both;">
                            </div>
                            <table class="totals">

                                <tr>
                                    <td>
                                        <span class="lbl">
                                            <%=Resources.Labels.CustomerBalanceBefore %>: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustomerBalanceBefore" runat="server" Text="0.0000"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="lbl">
                                            <%=Resources.Labels.CustomerBalanceAfter%>: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustomerBalanceAfter" runat="server" Text="0.0000"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
            </div>

            <div class="InvoiceSection align_right">
                <div class="validationSummary">
                    <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                </div>
                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                    ValidationGroup="Save" OnClick="BtnSave_Click" />


                <asp:Button runat="server" ID="btnApproveAccounting" Text="إعتماد مالي" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnApproveAccounting_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />





                <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />



                <asp:Button runat="server" ID="btnBtnApproveAndPay" Text="إعتماد و دفع" Style="display: none;" CssClass="button_big shortcut_save"
                    OnClick="btnBtnApproveAndPay_Click" />


                <asp:Button runat="server" ID="btnCancelApprove" Text="<%$ Resources:Labels,cancelApprove %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />



                <asp:Button runat="server" ID="btnPrintInventoryOrder" CssClass="button_big" Text="<%$ Resources:Labels,PrintInvOrder %>"
                    Visible="false" OnClick="btnPrintOrderOut_Click" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
                <asp:Button runat="server" ID="btnPrint_Dev" Text="طباعة تصميم" CssClass="button_big shortcut_print"
                    Visible="true" OnClick="btnPrint_Dev_Click" />
                <asp:Button runat="server" ID="btnPrint_Design" Text=" تصميم" CssClass="button_big shortcut_print"
                    Visible="true" OnClick="btnPrint_Design_Click" />

                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                    OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                    onclick="window.location = window.location; return false;" />
                <label>
                </label>
                <asp:DropDownList runat="server" ID="DropDownList1" CssClass="button_big shortcut_print" Width="40">
                    <%--<asp:ListItem Value="-1" Text="<%$Resources:Labels,Select  %>"></asp:ListItem>--%>
                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                    <asp:ListItem Value="3" Text="3"></asp:ListItem>

                </asp:DropDownList>


                <asp:OperationsView runat="server" ID="OperationsView" />


                <asp:LinkButton ID="lnkSearch" runat="server" CssClass="button_big shortcut_cancel" Text="استيراد"></asp:LinkButton>

                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
                    CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
                    AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
                    ExpandDirection="Vertical" SuppressPostBack="true">
                </asp:CollapsiblePanelExtender>


                <asp:Panel ID="pnlSearch" CssClass="pnlSearch" Style="background-color: #640ab7" runat="server" DefaultButton="btnImportFromFile">
                    <div class="tcat">
                        استيراد
                    </div>
                    <div class="content">
                        <div class="form" style="width: 600px;">
                            <div class="right_col">

                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />

                                <br />
                                <asp:Label ID="Label3" runat="server" Text="إستخدام السطر الاول لعناوين الحقول"></asp:Label><br />
                                <asp:RadioButtonList ID="rbHDR" runat="server" RepeatLayout="OrderedList">
                                    <asp:ListItem Text="نعم" Value="Yes" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="لا" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="GridView1_PageIndexChanging"
                                    AllowPaging="true">
                                </asp:GridView>


                            </div>
                            <div class="left_col">


                                <div class="Row">
                                    <div class="Column">
                                    </div>

                                    <div class="Column">
                                        <asp:Button ID="Button17" CssClass="button" runat="server" OnClick="Button17_Click"
                                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem1" />
                                    </div>
                                </div>
                                <div class="Row">
                                    <div class="Column">
                                        <asp:DropDownList ID="ddlPropertiesValue" runat="server" ValidationGroup="AddItem1">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="Column">
                                        <asp:DropDownList ID="ddlProperties" runat="server" ValidationGroup="AddItem1">
                                            <asp:ListItem Text="الباركود" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="الكمية" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="الوحدة" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="السعر" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="Row">
                                    <div class="Column">
                                        <asp:ABFGridView runat="server" ID="gvExportList" GridViewStyle="GrayStyle" DataKeyNames="ID">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="إسم الحقل" />
                                                <asp:BoundField DataField="Value" HeaderText="الحقل المرادف" />
                                            </Columns>
                                        </asp:ABFGridView>
                                    </div>
                                </div>











                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="btnDiv">
                            <asp:Button ID="btnImportFromFile" CssClass="button" runat="server" Text="استراد من ملف" OnClick="btnImportFromFile_Click"
                                ValidationGroup="search" />



                        </div>
                    </div>
                </asp:Panel>

                <div style="clear: both">
                </div>
            </div>
        </div>

    </div>


    <asp:HiddenField ID="hfmpeConfirm" runat="server" />
    <asp:ModalPopupExtender ID="mpeConfirm" runat="server" TargetControlID="hfmpeConfirm"
        PopupControlID="pnlConfirm" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeConfirm" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlConfirm" CssClass="pnlPopUp" runat="server"
        Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClientClick="$find('mpeConfirm').hide(); return false;"></asp:Button>
            <span>
                <%=Resources.UserInfoMessages.DoContinue%></span>
        </div>
        <div class="content">
            <div style="max-height: 400px; overflow: auto;" class="pnlPopupMessage">
                <asp:Literal runat="server" ID="ltConfirmationMessage"></asp:Literal>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnYes" CssClass="button default_button" runat="server" OnClick="btnYes_click" Text="<%$ Resources:Labels, Yes %>" />
                <asp:Button ID="BtnNo" runat="server" CssClass="button" OnClientClick="$find('mpeConfirm').hide(); return false;"
                    Text="<%$ Resources:Labels, No %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hfmpeConfirmCollection" runat="server" />
    <asp:ModalPopupExtender ID="mpeConfirmCollection" runat="server" TargetControlID="hfmpeConfirmCollection"
        PopupControlID="pnlConfirmCollection" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeConfirm" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmCollection" CssClass="pnlPopUp" runat="server"
        Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button16" OnClientClick="$find('mpeConfirmCollection').hide(); return false;"></asp:Button>
            <span>
                <%=Resources.UserInfoMessages.DoContinue%></span>
        </div>
        <div class="content">
            <div style="max-height: 400px; overflow: auto;" class="pnlPopupMessage">
                <asp:Literal runat="server" ID="Literal1"></asp:Literal>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnYesCollection" CssClass="button default_button" runat="server" OnClick="btnYesCollection_Click" Text="<%$ Resources:Labels, Yes %>" />
                <asp:Button ID="Button18" runat="server" CssClass="button" OnClientClick="$find('mpeConfirmCollection').hide(); return false;"
                    Text="<%$ Resources:Labels, No %>" />
            </div>
        </div>
    </asp:Panel>


    <asp:HiddenField ID="hfFastAddNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="lnkAddNewCustomer"
        PopupControlID="pnlFastAddNew" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlFastAddNew" CssClass="pnlPopUp" runat="server"
        Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="CloseFastAddNewPopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtFastAddName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                    IsRequired="true" ValidationGroup="FastAddNew">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtMobileNumner" runat="server" LabelText="<%$Resources:Labels,Mobile %>"
                    IsRequired="true" ValidationGroup="FastAddNew">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtTaxNumber" runat="server" LabelText="الرقم الضريبي"></asp:ABFTextBox>
                <label>
                    <%=Resources.Labels.Currency %></label>
                <asp:DropDownList ID="ddlFastAddCurrency" runat="server">
                </asp:DropDownList>

                <asp:AutoComplete runat="server" ID="acArea" ServiceMethod="GetAreas" LabelText="<%$Resources:Labels,Area %>"></asp:AutoComplete>
                <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" ServiceMethod="GetChartOfAccountsFatheronly"
                    LabelText="<%$Resources:Labels,ParentAccount %>" ValidationGroup="FastAddNew"></asp:AutoComplete>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnFastAddNew" CssClass="button default_button" runat="server" OnClick="btnFastAddNew_click"
                    ValidationGroup="FastAddNew" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelAddNew" runat="server" CssClass="button" OnClick="CloseFastAddNewPopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>



    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lnkAccountstatement"
        PopupControlID="pnlAccountstatement1" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="900">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAccountstatement1" CssClass="pnlPopUp" runat="server"
        Width="250">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button2" OnClick="CloseFastAddNewPopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">

                <asp:Button ID="BtnAccountstatement" CssClass="button default_button" runat="server" OnClick="BtnAccountstatement_Click"
                    Text="كشف        حساب        عميل" />
                <br />
                <br />
                <asp:Button ID="BtnAccountstatementIteme" CssClass="button default_button" runat="server" OnClick="BtnAccountstatementIteme_Click"
                    Text="كشف حساب عميل بالاصناف" />

            </div>
        </div>
    </asp:Panel>



    <asp:HiddenField ID="hiddenOffer" runat="server" />
    <asp:ModalPopupExtender ID="mpeOffer" runat="server" TargetControlID="hiddenOffer"
        PopupControlID="pnlOffer" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlOffer" CssClass="pnlPopUp" runat="server">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button3"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 200px;">


                <asp:CheckBox ID="chkSame" runat="server" />
                <span>الهدية</span>

                <asp:ABFTextBox ID="txtQtyOffer" Width="150" runat="server" TabIndex="0" LabelText="<%$Resources:Labels,Quantity %>">
                </asp:ABFTextBox>

            </div>
        </div>

        <div class="btnDiv">

            <asp:Button ID="btnAddOkOffer" CssClass="button default_button" runat="server" OnClick="btnAddOkOffer_Click"
                Text="<%$ Resources:Labels, Ok %>" />
            <asp:Button ID="Button5" runat="server" CssClass="button" OnClick="CloseFastAddNewPopup_Click"
                Text="<%$ Resources:Labels, Cancel %>" />
        </div>
    </asp:Panel>



    <asp:HiddenField ID="hfFastAddNewIteme" runat="server" />
    <asp:ModalPopupExtender ID="mpeFastAddNewIteme" runat="server" TargetControlID="lnkAddNewIteme"
        PopupControlID="pnlFastAddNewIteme" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="800">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlFastAddNewIteme" CssClass="pnlPopUp" runat="server"
        Width="588">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button4" OnClick="BtnCancelAddNewItemeCard_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 98%; margin: auto;">

                <div class="right_col">


                    <asp:ABFTextBox ID="txtBarcodeItemeCard" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                        IsRequired="true" ValidationGroup="NewItem">
                    </asp:ABFTextBox>



                    <asp:ABFTextBox ID="txtCodeItemeCard" runat="server" LabelText="<%$Resources:Labels,CodeItem %>" CssClass="barcode"
                        ValidationGroup="NewItem">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtNameItemeCard" runat="server" LabelText="<%$Resources:Labels,Name %>"
                        IsRequired="true" ValidationGroup="NewItem">
                    </asp:ABFTextBox>

                    <asp:AutoComplete ID="acCategoryItemeCard" ServiceMethod="GetItemsCategories" LabelText="<%$Resources:Labels,ParentCategory %>"
                        runat="server" IsRequired="true" ValidationGroup="NewItem"></asp:AutoComplete>
                    <label>
                        <%=Resources.Labels.ItemType %></label>
                    <asp:DropDownList ID="ddlItemTypeItemeCard" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtCostItemeCard" runat="server" LabelText="<%$Resources:Labels,Cost %>" MinValue="0.00000001"
                        IsRequired="true" ValidationGroup="NewItem" DataType="Decimal">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDefaultPriceItemeCard" runat="server" LabelText="<%$Resources:Labels,Price %>" MinValue="0.00000001"
                        ValidationGroup="NewItem" DataType="Decimal">
                    </asp:ABFTextBox>
                </div>

                <div class="left_col">
                    <asp:ABFTextBox ID="txtMinQtyItemeCard" runat="server" LabelText="<%$Resources:Labels,MinQty %>"
                        ValidationGroup="NewItem" DataType="Decimal">
                    </asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtMaxQtyItemeCard" runat="server" LabelText="<%$Resources:Labels,MaxQty %>"
                        ValidationGroup="NewItem" DataType="Decimal">
                    </asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acSmallestUnit" ServiceMethod="GetGeneralAtt"
                        LabelText="<%$Resources:Labels,SmallestUnit %>" IsRequired="true" ValidationGroup="NewItem"></asp:AutoComplete>
                    <asp:AutoComplete ID="acTaxItemeCard" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                        runat="server"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtPercentageDiscountItemeCard" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                        MinValue="0" DataType="Decimal" ValidationGroup="NewItem">
                    </asp:ABFTextBox>

                </div>

                <br />
                <br />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnFastAddNewItemeCard" CssClass="button default_button" runat="server" OnClick="btnFastAddNewItemeCard_Click"
                    ValidationGroup="NewItem" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelAddNewItemeCard" runat="server" CssClass="button" OnClick="BtnCancelAddNewItemeCard_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>


    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:ModalPopupExtender ID="mpeViewQty" runat="server" TargetControlID="HiddenField2"
        PopupControlID="pnlViewQty" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlViewQty" CssClass="pnlPopUp" runat="server"
        Width="499">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button6"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 85%; margin: auto;">


                <asp:ABFGridView runat="server" ID="gvViewQty" GridViewStyle="BlueStyle" DataKeyNames="NameStore" Width="">
                    <Columns>
                        <asp:BoundField DataField="NameStore" HeaderText="<%$Resources:Labels,Store %>" />

                        <asp:BoundField DataField="NameBranch" HeaderText="<%$Resources:Labels,Branch %>" />
                        <asp:BoundField DataField="Qty" HeaderText="<%$Resources:Labels,Quantity %>" />
                    </Columns>
                </asp:ABFGridView>


                <%-- <div class="right_col">


                  
                </div>

                <div class="left_col">
                    

                </div>--%>

                <br />
                <br />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnViewQty" CssClass="button default_button" runat="server" OnClick="btnViewQty_Click"
                    ValidationGroup="NewItem" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="btnViewQtyClose" runat="server" CssClass="button" OnClick="btnViewQtyClose_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>



    <%--Serial Number--%>

    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:ModalPopupExtender ID="mpeSerialNumberView" runat="server" TargetControlID="HiddenField3"
        PopupControlID="pnlSerialNumberView" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlSerialNumberView" runat="server" CssClass="pnlPopUp" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button7"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">

                <asp:Label ID="lblSerialNumberView" runat="server" Text="Label"></asp:Label>



            </div>
            <div class="btnDiv">

                <asp:Button ID="Button8" runat="server" CssClass="button"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="HiddenField4" runat="server" />
    <asp:ModalPopupExtender ID="mpeSerialNumber" runat="server" TargetControlID="HiddenField4"
        PopupControlID="pnlSerialNumber" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlSerialNumber" runat="server" CssClass="pnlPopUp" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button9"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtSerialNumber" runat="server" LabelText="Serial Number" Width="250" Height="200"
                    IsRequired="true" ValidationGroup="FastSerialNumber" TextMode="MultiLine">
                </asp:ABFTextBox>




            </div>
            <div class="btnDiv">
                <asp:Button ID="btnAddSerial" CssClass="button default_button" runat="server" OnClick="btnAddSerial_Click"
                    ValidationGroup="FastSerialNumber" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="Button10" runat="server" CssClass="button"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="HiddenField5" runat="server" />
    <asp:ModalPopupExtender ID="mpeInvoiceDistributePay" runat="server" TargetControlID="HiddenField5"
        PopupControlID="pnlInvoiceDistributePay" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResizeAndScroll"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlInvoiceDistributePay" CssClass="pnlPopUp" runat="server"
        Width="480">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button11" OnClick="CloseFastAddNewPopup_Click"></asp:Button>
            <span>إعتماد ودفع</span>
        </div>
        <div class="content">
            <div class="form">

                <asp:EmptyGridView runat="server" ID="gvPay" DataKeyNames="ID,Posted_ID" AllowPaging="false">
                    <Columns>
                        <asp:BoundField DataField="NamePayment" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125" HeaderText="طريقة الدفع" />
                        <asp:TemplateField HeaderText="مبلغ الذي سيدفع">
                            <ItemTemplate>
                                <asp:ABFTextBox runat="server" ID="txtAmountPay"
                                    DataType="Decimal" Style="min-width: 100px;"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:EmptyGridView>

            </div>
            <div class="btnDiv">

                <asp:Button ID="btnPay" CssClass="button default_button" runat="server" OnClick="BtnApprove_Click"
                    Text="<%$ Resources:Labels, Approve %>" />
                <asp:Button ID="Button12" runat="server" CssClass="button" OnClick="Button12_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>


    <asp:HiddenField ID="hfCollect" runat="server" />
    <asp:ModalPopupExtender ID="mpeCollect" runat="server" TargetControlID="hfCollect"
        PopupControlID="pnlCollectRefuse" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlCollectRefuse" CssClass="pnlPopUp" runat="server"
        Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button13" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">

            <asp:ABFGridView runat="server" ID="gvPriceList1" GridViewStyle="GrayStyle">
                <Columns>
                    <asp:BoundField DataField="AttName" HeaderText="الاسم" />
                    <asp:BoundField DataField="Price" HeaderText="السعر"
                        DataFormatString="{0:0.####}" />


                </Columns>
            </asp:ABFGridView>


        </div>
    </asp:Panel>


    <asp:HiddenField ID="hfGQiteme" runat="server" />
    <asp:ModalPopupExtender ID="mpeGQiteme" runat="server" TargetControlID="hfGQiteme"
        PopupControlID="pnlOGQiteme" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlOGQiteme" CssClass="pnlPopUp" runat="server"
        Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button14" OnClick="Button6_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">

            <asp:ABFGridView runat="server" ID="gvQtyStoreList1" GridViewStyle="GrayStyle">
                <Columns>
                    <asp:BoundField DataField="StoreName" HeaderText="المستودع" />
                    <asp:BoundField DataField="ItemeBalanceStore" HeaderText="الكمية"
                        DataFormatString="{0:0.####}" />


                </Columns>
            </asp:ABFGridView>


        </div>
    </asp:Panel>




    <asp:HiddenField ID="HiddenField6" runat="server" />
    <asp:ModalPopupExtender ID="mpeInfo" runat="server" TargetControlID="lnkInfo" Drag="true"
        PopupControlID="pnlInfo" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResizeAndScroll"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlInfo" runat="server" CssClass="pnlPopUp" Width="180">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button15"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtInfo" runat="server" LabelText="القيمة النهائية"
                    IsRequired="true" ValidationGroup="Info">
                </asp:ABFTextBox>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnInfoChange" CssClass="button" runat="server" OnClick="btnInfoChange_Click"
                    ValidationGroup="Info" Text="<%$ Resources:Labels, OK %>" />

            </div>
        </div>
    </asp:Panel>

</asp:Content>

