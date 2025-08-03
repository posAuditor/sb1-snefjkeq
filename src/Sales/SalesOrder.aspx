<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesOrder.aspx.cs" Inherits="Sales_SalesOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" TagName="ucNewItemDescribed" Src="~/CustomControls/ucNewItemDescribed.ascx" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx" TagPrefix="asp" %>
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
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Label runat="server" Visible="false" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.SalesOrder %></asp:Label>

            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.PurchaseOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>


            <asp:Nav runat="server" ID="ucNav" />
            <asp:Favorit runat="server" ID="Favorit1" />


        </div>


        <div class="InvoiceSection">
            <div class="Row">
                <div class="Column">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label></br>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>


                    <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                        IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>

                </div>
                <div class="Column">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label></br>
                    <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames"
                        IsRequired="true" OnSelectedIndexChanged="acCustomer_SelectedIndexChanged" AutoPostBack="true" IsBtnPlus="true" ControlName="lnkAddNewCustomer"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkAddNewCustomer" runat="server" CssClass="PlusBtn" Style="display: none">[+]</asp:LinkButton>

                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                </div>
                <div class="Column">
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>
                    <asp:Panel runat="server" Style="display: none">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                </div>
            </div>


            <div class="InvoiceSection" style="display: none;">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">


                        <asp:Panel runat="server" Style="display: none">



                            <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true"></asp:AutoComplete>
                            <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                                LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                            <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                                LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                        </asp:Panel>

                    </div>
                    <div class="left_col">
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>

            <div class="InvoiceSection">

                <div class="container">

                    <div class="tb">
                        <table cellspacing="0" cellpadding="0" style="width: 100%">
                            <tr>
                                <td colspan="11">


                                    <div>

                                        <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                            <tbody>
                                                <tr class="grdWoutHeader_row">
                                                    <th class="colWLarge"><%=Resources.Labels.Item %></th>
                                                    <th class="colW"><%=Resources.Labels.Unit %></th>
                                                    <th class="colW"><%=Resources.Labels.Quantity %>  </th>
                                                    <th class="colW"><%=Resources.Labels.Price %></th>
                                                    <th class="colW"><%=Resources.Labels.PercentageDiscount %></th>
                                                    <th class="colW"><%=Resources.Labels.CashDiscount %></th>
                                                    <th class="colW">الصافي قبل الضريبة</th>
                                                    <th class="colW"><%=Resources.Labels.GrossTotal %></th>
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
                                <td colspan="11">


                                    <div>
                                        <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                            <tbody>
                                                <tr class="grdWoutHeader_row">

                                                    <td class="colWLarge">

                                                        <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Item %>"
                                                            OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                                            VisibleText="false" CssClass="cls" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                                    </td>
                                                    <td class="colW">
                                                        <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                                                            IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                            AutoPostBack="true" VisibleText="false" CssClass="cls" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                                    </td>
                                                    <td class="colW">
                                                        <asp:ABFTextBox ID="txtQty" runat="server" LabelText="<%$Resources:Labels,Quantity %>" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"
                                                            MinValue="0.001" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" VisibleText="false"
                                                            CssClass="cls" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox></td>
                                                    <td class="colW">
                                                        <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Price %>" VisibleText="false" AutoPostBack="true" OnTextChanged="txtCost_TextChanged"
                                                            MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox></td>
                                                    <td class="colW">
                                                        <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>" VisibleText="false"
                                                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                                    </td>
                                                    <td class="colW">
                                                        <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>" VisibleText="false"
                                                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                                    </td>
                                                    <td class="colW">

                                                        <span style="width: 30px!important; color: black"></span>
                                                        <asp:Label ID="lblTotalRowBeforTax" runat="server" Text=""></asp:Label>

                                                    </td>

                                                    <td class="colW">

                                                        <span style="width: 30px!important; color: black"></span>
                                                        <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label>

                                                    </td>
                                                    <td class="colSmall">
                                                        <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" TabIndex="8" ValidationGroup="AddItem" runat="server">
                                                             <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important"></i>
                                                        </asp:LinkButton>
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
                                <td colspan="11">
                                    <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                        OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                                        OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
                                        <Columns>


                                            <asp:BoundField ItemStyle-CssClass="colWLarge" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />


                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                                                DataFormatString="{0:0.####}" />



                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="UnitCost" HeaderText="<%$Resources:Labels,Price %>" DataFormatString="{0:0.####}" />

                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="Total" HeaderText="<%$Resources:Labels,Total %>" DataFormatString="{0:0.####}" />

                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="PercentageDiscount" HeaderText="<%$Resources:Labels,PercentageDiscount %>"
                                                DataFormatString="{0:0.####}" />
                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>"
                                                DataFormatString="{0:0.####}" />

                                            <asp:BoundField ItemStyle-CssClass="colW" DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                                                DataFormatString="{0:0.####}" />


                                            <asp:TemplateField ItemStyle-CssClass="colSmall">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgAdd" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Edit %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" Visible='<%# this.DocStatus_ID==1 %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete" Visible='<%# this.DocStatus_ID==1 %>'
                                                        OnClientClick="return ConfirmSure();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:AuditorGridView>




                                    <div class="validationSummary">
                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddItem" />
                                    </div>


                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <div class="InvoiceSection" style="display: none">
                <div class="container">
                    <table class="table table-hover" style="border: 0 solid black;">
                        <tbody>
                            <tr>
                                <td style="width: 11%">

                                    <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                        OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true" IsHideable="true">
                                    </asp:ABFTextBox>

                                </td>
                                <td style="width: 11%">
                                    <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                                        OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" IsHideable="true"></asp:ABFTextBox></td>

                                <td style="width: 11%"></td>
                                <td style="width: 11%">
                                    <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                        LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                        ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>
                                </td>
                                <td style="width: 11%"></td>
                                <td style="width: 11%">

                                    <table>
                                        <tr>

                                            <td>
                                                <asp:LinkButton ID="lnkGroupStoreIteme" runat="server" OnClick="lnkGroupIteme_Click" CssClass="PlusBtn2">[...]</asp:LinkButton>
                                            </td>

                                            <td></td>
                                        </tr>
                                    </table>









                                </td>
                                <td style="width: 11%"></td>
                                <td style="width: 11%">




                                    <table>
                                        <tr>

                                            <td>
                                                <asp:LinkButton ID="lnkGetPriceIteme" runat="server" OnClick="lnkGetPriceIteme_Click" CssClass="PlusBtn1">[...]</asp:LinkButton>
                                            </td>

                                            <td></td>
                                        </tr>
                                    </table>





                                </td>



                                <td style="width: 3%">

                                    <div class="btnDiv" style="padding-top: 24px;">
                                    </div>

                                </td>


                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel runat="server" ID="pnlItemdescribed">
                                        <asp:AutoComplete runat="server" ID="acItemDescribed" ServiceMethod="GetItemsDescribed"
                                            AutoPostBack="true" ValidationGroup="AddItem"
                                            LabelText="<%$Resources:Labels,Itemdescribed %>"></asp:AutoComplete>
                                        <asp:LinkButton ID="lnkNewDescribed" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>

                    <br />
                    <span>
                        <%=Resources.Labels.ActualQuantity %> : </span>
                    <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>



                    <div style="clear: both">
                    </div>




                    <div class="form" style="width: 600px; margin: auto; display: none;">
                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                            TextMode="MultiLine" Style="max-width: 600px; width: 600px;"></asp:ABFTextBox>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
            </div>

            <div class="InvoiceSection" style="display: none;">
                <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                    <div class="form" style="width: 90%; margin: auto;">
                        <div class="right_col">

                            <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>

                            <asp:ABFTextBox ID="txtCapacity" runat="server" LabelText="<%$Resources:Labels,Capacity %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>

                            <asp:ABFTextBox ID="txtCapacities" OnTextChanged="txtCapacities_OnTextChanged" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                                ValidationGroup="AddItem" AutoPostBack="true" IsHideable="true"></asp:ABFTextBox>


                            <asp:Panel runat="server" Visible="False">
                                <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                                    OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                            </asp:Panel>

                            <br />
                            <span>
                                <%=Resources.Labels.AvailableQty %>: </span>
                            <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="left_col">
                            <asp:AutoComplete runat="server" ID="acPriceName" ServiceMethod="GetItemPriceNames"
                                LabelText="<%$Resources:Labels,PriceType %>" OnSelectedIndexChanged="acPriceName_SelectedIndexChanged"
                                AutoPostBack="true" IsHideable="true"></asp:AutoComplete>


                            <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>
                            <asp:Panel runat="server" Visible="False">
                                <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                                    Enabled="false" IsHideable="true"></asp:ABFTextBox>
                                <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                                    Enabled="false" IsHideable="true"></asp:ABFTextBox>
                            </asp:Panel>

                            <br />
                            <span>
                                <%=Resources.Labels.LastCustomerPrice %>: </span>
                            <asp:Label ID="lblLastCustomerPrice" runat="server" Text=""></asp:Label>
                        </div>
                    </div>

                    <br />
                    <div class="btnDiv" style="display: none;">
                        <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                        <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" OnClick="btnAddItemGroup_click"
                            Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                        <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                            OnClick="BtnClearItem_Click" />
                    </div>
                </asp:Panel>
            </div>
            <%-- <div class="InvoiceSection">
            
        </div>--%>
            <div class="InvoiceSection" id="taxSection" runat="server" style="display: none;">
                <div class="form" style="width: 600px; margin: auto;">
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
                <div class="form">
                    <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,RecipientName %>" Style="box-sizing: border-box;"
                        TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
                </div>
                <br />
                <br />
                <div class="form" style="width: 100%; margin: auto;">

                    <table>
                        <tr style="top: 0;">
                            <td style="width: 18%;">
                                <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td style="width: 18%;">
                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td style="width: 18%;">
                                <asp:ABFTextBox ID="txtAdditionals" runat="server" LabelText="<%$Resources:Labels,Additionals %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td style="width: 18%;">
                                <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal"></asp:ABFTextBox></td>
                            <td style="width: 28%;">
                                <div class="form" style="width: 100%; margin: auto;">


                                    <table class="totals">
                                        <tr>
                                            <td>
                                                <span class="lbl">
                                                    <%=Resources.Labels.Total %>: </span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="DiscountTotal">
                                            <td>
                                                <span class="lbl">
                                                    <%=Resources.Labels.TotalDiscount %>: </span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotalDiscount" runat="server" Text="0.0000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="lbl">
                                                    <%=Resources.Labels.Additionals %>: </span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAdditionals" runat="server" Text="0.0000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="taxTotal">
                                            <td>
                                                <span class="lbl">
                                                    <%=Resources.Labels.TotalTax %>: </span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotalTax" runat="server" Text="0.0000"></asp:Label>
                                            </td>
                                        </tr>
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

                                    <div style="clear: both">
                                    </div>
                                </div>

                            </td>
                        </tr>
                    </table>


                </div>

            </div>
            <div class="InvoiceSection align_right">
                <div class="validationSummary">
                    <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                </div>
                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                    ValidationGroup="Save" OnClick="BtnSave_Click" />
                <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />



                <asp:Button runat="server" ID="btnCancelApprove" Text="<%$ Resources:Labels,cancelApprove %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click1" OnClientClick="return ConfirmSureWithValidation('Save')" />




                <asp:Button runat="server" ID="btnPrintInventoryOrder" CssClass="button_big" Text="<%$ Resources:Labels,PrintInvOrder %>"
                    Visible="false" OnClick="btnPrintOrderOut_Click" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                    OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                    onclick="window.location = window.location; return false;" />
                <div style="clear: both">
                </div>
            </div>
        </div>





        <asp:HiddenField ID="hfFastAddNew" runat="server" />
        <asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="lnkAddNewCustomer"
            PopupControlID="pnlFastAddNew" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
            BehaviorID="showPopUp" Y="300">
        </asp:ModalPopupExtender>
        <asp:ucNewItemDescribed
            OnNewItemDescribedCreated="ucNewItemDescribed_OnNewItemDescribedCreated"
            TargetControlID="cph$lnkNewDescribed"
            Title="<%$Resources:Labels,Itemdescribed %>"
            runat="server" ID="ucNewItemDescribed" />
        <asp:HiddenField ID="hfNewDescribed" runat="server" />
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
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlFastAddCurrency" runat="server">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acArea" ServiceMethod="GetAreas" LabelText="<%$Resources:Labels,Area %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acParentAccount" IsRequired="true" ServiceMethod="GetChartOfAccounts"
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



        <asp:HiddenField ID="hfCollect" runat="server" />
        <asp:ModalPopupExtender ID="mpeCollect" runat="server" TargetControlID="lnkGetPriceIteme"
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
        <asp:ModalPopupExtender ID="mpeGQiteme" runat="server" TargetControlID="lnkGroupStoreIteme"
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
</asp:Content>
