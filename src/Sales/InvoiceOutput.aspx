<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceOutput.aspx.cs" Inherits="Sales_InvoiceOutput" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />

    <style>
        ::selection {
            background: #f7a494;
        }

        ::-moz-selection {
            background: #f7a494;
        }
    </style>

    <style type="text/css">
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
            width: 21%;
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
            width: 15%;
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
            width: 5%;
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


        /*.grdWoutHeader_pager {
            background: #ffffff;
        }

            .grdWoutHeader_pager td {
                border: 0px;
            }

            .grdWoutHeader_pager table {
               
            }

                .grdWoutHeader_pager table td {
                    
                    font-family: "Droid Arabic Kufi",Arial;
                    
                    text-align: center;
                    color: #575757;
                    background-color: #F5F5FF;
                    font-size: 20px;
                }

                    .grdWoutHeader_pager table td a {
                        font-weight: bold;
                        color: #4c9ccc;
                    }*/

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
    </script>

    <script type="text/javascript">
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


        //$(document).on('keyup', '#tree', function (e, sender) {
        //    if (e.which == 13 || e.which == 9)
        //        document.getElementById(getNextControl(sender)).focus();
        //});




    </script>

    <style type="text/css">
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


    <script>
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
            <asp:ABFTextBox ID="txtSerial" runat="server"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Label runat="server" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.Invoice %></asp:Label>
            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.FromSalesOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>
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

                    <asp:AutoComplete runat="server" ID="acStore" LabelText="<%$Resources:Labels,Store %>" ServiceMethod="GetStores"
                        IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acdrivers" ServiceMethod="Getcar" LabelText="<%$Resources:Labels,DriverNamne %>"
                        ValidationGroup="Save"></asp:AutoComplete>

                        <asp:ABFTextBox ID="ABFTextBox1" runat="server" ValidationGroup="Save" LabelText="تاريخ التسليم" DataType="Date" IsHideable="true"></asp:ABFTextBox>

                     <asp:ABFTextBox ID="txtCustomerRepresentative" runat="server" LabelText="اسم ممثل العميل" IsHideable="true"></asp:ABFTextBox>

                </div>
                <div class="Column">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames"
                        IsRequired="true" OnSelectedIndexChanged="acCustomer_SelectedIndexChanged" AutoPostBack="true"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkAddNewCustomer" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    <asp:LinkButton ID="lnkAccountstatement" runat="server" CssClass="PlusBtn1">[...]</asp:LinkButton>


                    <asp:Panel runat="server" ID="pnlCustomerMesure" Visible="False">
                        <asp:AutoComplete runat="server" ID="acCustomerMesure" ValidationGroup="Save" ServiceMethod="GetContactMesure" LabelText="<%$Resources:Labels,SalesRep %>"></asp:AutoComplete>
                    </asp:Panel>

                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                    <asp:ABFTextBox ID="txtDeliveryDate" runat="server" ValidationGroup="Save" LabelText="تاريخ التسليم" DataType="Date" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtProjectRef" runat="server" LabelText="بيانات المشروع وموقع التسليم" IsHideable="true"></asp:ABFTextBox>
                </div>
                <div class="Column">
                    <br />
                    <asp:ABFTextBox ID="txtRatio" Style="display: none" VisibleText="false" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>

                      <asp:ABFTextBox ID="txtContactPerson" runat="server" LabelText="بيانات المستلم" IsHideable="true"></asp:ABFTextBox>
                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                    <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccountsCheledronly" Visible="false"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>

                </div>
            </div>









            <%-- </div>
                <div class="left_col">--%>



            <%--  </div>
            </div>
            <div style="clear: both">
            </div>--%>
        </div>


        <div class="InvoiceSection" >

            <div class="container" >
                <asp:ABFTextBox ID="txtBarcode" runat="server" TabIndex="0" LabelText="<%$Resources:Labels,Barcode %>" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    OnTextChanged="txtBarcode_TextChanged" Visible="false"  AutoPostBack="true" CssClass="barcode">
                </asp:ABFTextBox>


                <asp:AutoComplete runat="server" ID="acPriceName" Visible="false" ServiceMethod="GetItemPriceNames" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    LabelText="<%$Resources:Labels,PriceType %>" OnSelectedIndexChanged="acPriceName_SelectedIndexChanged"
                    AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                <asp:AutoComplete Visible="false" runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                    LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                    ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true" IsHideable="true" ></asp:AutoComplete>


                <span>
                    </span>

                <asp:LinkButton ID="lnkViewQty"  Visible="false"  runat="server" OnClick="lnkViewQty_Click">
                    <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
                </asp:LinkButton>




                <br />
                <span>
                    <%=Resources.Labels.LastCustomerPrice %>: </span>
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                <div class="tb">
                    <table cellspacing="0" cellpadding="0" style="width: 100%">
                        <tr>
                            <td colspan="6">


                                <div>
                                     
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">


                                                <th class="colWLarge"><%=Resources.Labels.Item %></th>
                                                <th class="colW"><%=Resources.Labels.Unit %></th>

                                                <th class="colW"><%=Resources.Labels.Quantity %></th>
                                                <th class="colW">الكمية المتوقع تسليمها</th>
                                                <th class="colW">الكمية المستلمة</th>



                                                <th class="colSmall" style="color: black"><%=Resources.Labels.Delete %></th>
                                                <th class="colSmall">تسليم</th>



                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            </td>
                        </tr>

                        
                        <tr>
                            <td colspan="6">
                                <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging" OnRowDataBound="gvItems_RowDataBound"
                                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging" OnRowCommand="gvItems_RowCommand">
                                    <Columns>

                                        <asp:BoundField ItemStyle-CssClass="colWLarge" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />

                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                                            DataFormatString="{0:0.####}" />
                                        <asp:BoundField ItemStyle-CssClass="colW" DataField="QuantityRecived" HeaderText="المتوقع استلامها"
                                            DataFormatString="{0:0.####}" />
                                         <asp:BoundField ItemStyle-CssClass="colW" DataField="QuantityInflamation2" HeaderText="الكمية المستلمة"
                                            DataFormatString="{0:0.####}" />

                                       <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="34" Visible='<%# this.DocStatus_ID==1 %>' ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                                    OnClientClick="return ConfirmSure();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                               <asp:Button runat="server" ID="btnSave" Text="تسليم" CssClass="button_big shortcut_save" CommandName="AddInflamation"
                                                   CommandArgument='<%# Eval("ID") %>'  OnClientClick="return ConfirmSure();" style="background: #9c9c9d;
                                                                                                                                    background-image: -webkit-linear-gradient(top, #9c9c9d, #707171);
                                                                                                                                    background-image: -moz-linear-gradient(top, #9c9c9d, #707171);
                                                                                                                                    background-image: -ms-linear-gradient(top, #9c9c9d, #707171);
                                                                                                                                    background-image: -o-linear-gradient(top, #9c9c9d, #707171);
                                                                                                                                    background-image: linear-gradient(to bottom, #9c9c9d, #707171);
                                                                                                                                    -webkit-border-radius: 7;
                                                                                                                                    -moz-border-radius: 7;
                                                                                                                                     border-radius:0;  
    
                                                                                                                                    color: #ffffff;
                                                                                                                                    font-size: 12px;
                                                                                                                                    padding: 5px 20px 7px 20px;
                                                                                                                                    text-decoration: none;
                                                                                                                                    cursor: pointer;
                                                                                                                                      min-width: 30px; 
                                                                                                                                    border: 1px solid #abd3eb;"/>


                                                <%--<asp:ImageButton CommandArgument='<%# Eval("ID") %>' ImageUrl="../images/logout_img.png" Width="20" Height="20" runat="server" AlternateText="استلام"
                                                    CommandName="AddInflamation"
                                                    OnClientClick="return ConfirmSure();" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:AuditorGridView>
                            </td>
                        </tr>

                      
                    </table>
                    <div  style="display: none;">
                          <tr>
                            <td colspan="6">


                                <div>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">

                                                <td class="colWLarge">
                                                    <asp:AutoComplete runat="server" LabelText="<%$Resources:Labels,Item %>" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                                                        OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                                        VisibleText="false" CssClass="cls" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>

                                                </td>
                                                <td class="colW">
                                                    <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" Style="background-color: transparent; border-radius: 0px!important"
                                                        IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" TabIndex="2" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                        AutoPostBack="true"></asp:AutoComplete>
                                                </td>
                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtQty" runat="server" CssClass="cls" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"
                                                        MinValue="0.001" DataType="Decimal" Text="1" IsRequired="true" TabIndex="3" onfocus="javascript:this.select();" ValidationGroup="AddItem"></asp:ABFTextBox></td>
                                               

                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image2" runat="server" Width="34" ImageUrl="../images/black.gif" /></td>
                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            </td>
                        </tr>


                        <tr>
                            <th class="colWx"></th>
                            <th class="colWx"></th>
                            <th class="colWx">

                                <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox>


                            </th>
                            <th class="colWx">


                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox>


                            </th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtAdditionals" runat="server" LabelText="<%$Resources:Labels,Additionals %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox>
                            </th>

                            <th class="colWx">
                                <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal"></asp:ABFTextBox></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 30px!important; color: black"></th>
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
                                                 <td class="colW" style="display: none;">
                                                    <asp:ABFTextBox ID="txtCost" runat="server" CssClass="cls" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" Style="background-color: transparent; border-radius: 0px!important"
                                                        MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" onfocus="javascript:this.select();" TabIndex="4"></asp:ABFTextBox></td>
                                                <td class="colW" style="display: none;">


                                                    <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" onfocus="javascript:this.select();" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" LabelText="<%$Resources:Labels,PercentageDiscount %>" VisibleText="false" CssClass="cls"
                                                        Style="background-color: transparent; border-radius: 0px!important" TabIndex="5"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem"></asp:ABFTextBox>


                                                </td>
                                                <td class="colW" style="display: none;">

                                                    <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" onfocus="javascript:this.select();" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" LabelText="<%$Resources:Labels,CashDiscount %>" VisibleText="false" CssClass="cls"
                                                        Style="background-color: transparent; border-radius: 0px!important" TabIndex="6"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem"></asp:ABFTextBox>




                                                </td>
                                                <td class="colW" style="display: none;">
                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label>


                                                </td>
                                                <td class="colSmall" style="color: black; display: none;">
                                                    <asp:CheckBox ID="chkCado" TabIndex="7" Width="34" runat="server" /></td>

                                                <td class="colSmall" style="color: black; display: none;">
                                                    <asp:Image ID="imgEmpty" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                                </td>


                                                <td class="colSmall" style="color: black; text-align: center;">

                                                    <asp:LinkButton ID="LinkButton1" CausesValidation="false" OnClick="btnAddItem_click" TabIndex="8" ValidationGroup="AddItem" runat="server"> 
                                                          <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important"></i></asp:LinkButton>

                                                  
                                                </td>
                    </div>
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
                                    <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" ></asp:AutoComplete>

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
                                        <asp:LinkButton ID="btnAddItemException" OnClick="btnAddItemException_OnClick" ValidationGroup="AddItem" runat="server"><i class="btnShartcatInvoice demo-icon icon-plus-circle" style=" font-size: 20px; padding-top: 10px;"></i></asp:LinkButton>
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
                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>



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
                                                    ValidationGroup="AddItem" AutoPostBack="true" IsHideable="true" VisibleText="False"></asp:ABFTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</td>

                            <td>--%>
                               
                                </td>
                                <td>



                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Policy %>"></asp:Label>

                                    <asp:ABFTextBox ID="txtPolicy" runat="server" VisibleText="False"
                                        MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Policy %>" IsHideable="true"></asp:ABFTextBox>


                                </td>
                                <td></td>
                                <td>
                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Code %>"></asp:Label>

                                    <asp:ABFTextBox ID="txtCode" runat="server"
                                        MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Code %>" VisibleText="False" IsHideable="true"></asp:ABFTextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,DateRequest %>"></asp:Label>
                                    <asp:ABFTextBox ID="txtInvoiceDate" runat="server" VisibleText="False" LabelText="<%$Resources:Labels,DateRequest %>" ValidationGroup="AddItem"
                                        DataType="Date" IsHideable="true"></asp:ABFTextBox>
                                </td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>

                    <div style="clear: both">
                    </div>
                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddItem" />
                    </div>


                </div>
            </div>
            <div class="InvoiceSection" style="display: none;">
                <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem" Style="display: none;">
                    <div class="form" style="margin: auto; width: 90%;">
                        <div class="right_col">



                            <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>
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
                                    Enabled="false" IsHideable="true"></asp:ABFTextBox>

                                <%-- LabelText="<%$Resources:Labels,ExpirationDate %>"--%>
                                <asp:ABFTextBox ID="txtExpirationDate" CssClass="hiddencol" runat="server"
                                    Enabled="false" IsHideable="true"></asp:ABFTextBox>

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
                            TextMode="MultiLine" Style="width: 100%;"></asp:ABFTextBox>
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
                            TextMode="MultiLine" Width="100%"></asp:ABFTextBox>

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
                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save" style="display:none"
                    ValidationGroup="Save" OnClick="BtnSave_Click" />

                <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />


                <asp:Button runat="server" ID="btnCancelApprove" Text="<%$ Resources:Labels,cancelApprove %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />



                <asp:Button runat="server" ID="btnPrintInventoryOrder" style="display: none" CssClass="button_big" Text="<%$ Resources:Labels,PrintInvOrder %>"
                    Visible="false" OnClick="btnPrintOrderOut_Click" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
             
               

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
                    IsRequired="true" ValidationGroup="FastAddNew"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtMobileNumner" runat="server" LabelText="<%$Resources:Labels,Mobile %>"
                    IsRequired="true" ValidationGroup="FastAddNew"></asp:ABFTextBox>
                <label>
                    <%=Resources.Labels.Currency %></label>
                <asp:DropDownList ID="ddlFastAddCurrency" runat="server">
                </asp:DropDownList>

                <asp:AutoComplete runat="server" ID="acArea" ServiceMethod="GetAreas" LabelText="<%$Resources:Labels,Area %>"></asp:AutoComplete>
                <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" ServiceMethod="GetChartOfAccountsOld"
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


                <asp:CheckBox ID="chkSame" runat="server" /><span>الهدية</span>

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
    <asp:LinkButton ID="lbInflamation" CommandName="AddInflamation" Style="display: none;"
        runat="server">استلام</asp:LinkButton>
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:ModalPopupExtender ID="mpeInflamation" runat="server" TargetControlID="lbInflamation"
        PopupControlID="Panel2" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel2" CssClass="pnlPopUp" runat="server"
        Width="280">

        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="xxxxxx"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtQuatityInflamation" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                    IsRequired="true" ValidationGroup="InflamationAddNew">
                </asp:ABFTextBox>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnInflamationAddNewAddNew" CssClass="button default_button" runat="server" OnClick="btnInflamationAddNewAddNew_Click"
                    ValidationGroup="InflamationAddNew" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelInflamationAddNew" runat="server" CssClass="button" OnClick="BtnCancelInflamationAddNew_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

