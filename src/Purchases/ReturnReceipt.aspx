<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReturnReceipt.aspx.cs" Inherits="Purchases_ReturnReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>

<%@ Register Src="~/CustomControls/OperationsView.ascx" TagPrefix="asp" TagName="OperationsView" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />



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
            width: 31%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colWLarge {
            width: 16%;
            color: black;
            border: 2px solid black;
            text-align: center;
            height: 30px;
        }

        .colW {
            width: 10%;
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

        .colW {
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server" Visible="false"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Label Visible="false" runat="server" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.ReturnReceipt %></asp:Label>

            <div runat="server" visible="false" id="divPurchaseOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.FromReceiptSerial%></span>:
                <asp:Label ID="lblFromReceiptNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>

            <asp:Nav runat="server" ID="ucNav" />
            <asp:Favorit runat="server" ID="Favorit1" />


        </div>
        <div class="InvoiceSection">


            <%--   <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">--%>



            <div class="Row">
                <div class="Column">

                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <br />
                    <label style="display: none;">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" Style="display: none;"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" TabIndex="-1" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                        IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>

                </div>
                <div class="Column">

                    <br />
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acVendor" ServiceMethod="GetContactNames" IsRequired="true"
                        OnSelectedIndexChanged="acVendor_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>
                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                    <label>
                        .
                    </label>
                    <asp:DropDownList runat="server" ID="ddlTvae" AutoPostBack="true" OnSelectedIndexChanged="ddlTvae_SelectedIndexChanged">

                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>

                    </asp:DropDownList>


                </div>
                <div class="Column">

                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <br />
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>" Style="display: none;" VisibleText="false"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>

                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>
                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>

                </div>
            </div>







            <%--                      </div>
                <div class="left_col">--%>




            <%--   
                     </div>
            </div>--%>



            <div style="clear: both">
            </div>



        </div>



        <div class="InvoiceSection">

            <div class="container">
                <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true"></asp:ABFTextBox>
                <br />
                <span>
                    <%=Resources.Labels.AvailableQty %>: </span>
                <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>


                <div class="tb">

                    <table cellspacing="0" cellpadding="0" style="width: 100%">
                        <tr>
                            <td colspan="11">
                                <div>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">
                                                <th class="colSmall"></th>
                                                <th class="colWLarge"><%=Resources.Labels.Item %></th>
                                                <th class="colW"><%=Resources.Labels.Unit %></th>
                                                <th class="colW"><%=Resources.Labels.Quantity %></th>
                                                <th class="colW"><%=Resources.Labels.Price %></th>
                                                <th class="colW"><%=Resources.Labels.PercentageDiscount %></th>
                                                <th class="colW"><%=Resources.Labels.CashDiscount %></th>
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
                                                <td class="colSmall"></td>
                                                <td class="colWLarge">

                                                    <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem" TabIndex="1"
                                                        OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                                        LabelText="<%$Resources:Labels,Item %>" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                                </td>
                                                <td class="colW">

                                                    <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem" TabIndex="2"
                                                        IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                        AutoPostBack="true" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>

                                                </td>

                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtQty" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                                                        MinValue="0.001" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" VisibleText="false" TabIndex="3"
                                                        CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox></td>

                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtCost" runat="server" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" LabelText="<%$Resources:Labels,Cost %>" TabIndex="4"
                                                        MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>

                                                </td>
                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtItemPercentageDiscount" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>" TabIndex="5"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                                </td>

                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtItemCashDiscount" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>" TabIndex="6"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                                </td>
                                                <td class="colW"><span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label></td>
                                                <td class="colSmall" style="color: black; text-align: center;">
                                                    <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" ValidationGroup="AddItem" runat="server" TabIndex="7">
                                                        <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px;color:black!important"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" /></td>
                                                <td class="colSmall" style="color: black">
                                                    <asp:Image ID="Image2" runat="server" Width="34" ImageUrl="../images/black.gif" /></td>
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
                                        <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$Resources:Labels,Serial %>">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ControlStyle-CssClass="colWLarge" ItemStyle-CssClass="colWLarge" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="UnitCost" HeaderText="<%$Resources:Labels,Cost %>" DataFormatString="{0:0.####}" />
                                        <%--  <asp:BoundField DataField="Total" HeaderText="<%$Resources:Labels,Total %>" DataFormatString="{0:0.####}" />--%>
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="PercentageDiscount" HeaderText="<%$Resources:Labels,PercentageDiscount %>"
                                            DataFormatString="{0:0.####}" />
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>"
                                            DataFormatString="{0:0.####}" />
                                        <%--   <asp:BoundField DataField="TaxPercentageValue" HeaderText="<%$Resources:Labels,Tax %>" ItemStyle-CssClass="TaxCol"
                                            DataFormatString="{0:0.####}" />--%>
                                        <asp:BoundField ControlStyle-CssClass="colW" ItemStyle-CssClass="colW" DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                                            DataFormatString="{0:0.####}" />

                                        <asp:TemplateField ControlStyle-CssClass="colSmall" ItemStyle-CssClass="colSmall" FooterStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:Image ID="Image3" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ControlStyle-CssClass="colSmall" ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Edit %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="34" Visible='<%# this.DocStatus_ID==1 %>' ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-CssClass="colSmall" ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
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
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal"></asp:ABFTextBox></th>

                            <th class="colWx"></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 35px!important; color: black"></th>
                            <th style="width: 30px!important; color: black"></th>
                        </tr>
                        <tr style="height: 20px; background-color: lightgrey;">
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
                            <th class="colWx"></th>
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
        </div>




        <div class="InvoiceSection" style="display: none;">
            <table class="table table-hover" style="border: 0 solid black;">
                <tbody>
                    <tr>
                        <td style="width: 7%;">
                            <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true" IsHideable="true">
                            </asp:ABFTextBox>
                        </td>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>
                        </td>



                        <td></td>
                        <td></td>
                        <td></td>

                        <td></td>
                        <td>
                            <div class="btnDiv" style="padding-top: 24px;">
                            </div>
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
            <br />


        </div>

        <div class="InvoiceSection" style="display: none;">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">





                        <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>

                        <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                            OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>

                    </div>
                    <div class="left_col">


                        <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                            TextMode="MultiLine"></asp:ABFTextBox>
                    </div>
                </div>

                <div class="btnDiv">
                    <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                    <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" OnClick="btnAddItemGroup_click"
                        Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                    <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearItem_Click" />
                </div>
            </asp:Panel>
        </div>

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


            <div class=" align_right">
                <div class="validationSummary">
                    <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                </div>
                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                    ValidationGroup="Save" OnClick="BtnSave_Click" />
                <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />


                <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />




                <asp:Button runat="server" ID="btnPrintInventoryOrder" CssClass="button_big" Text="<%$ Resources:Labels,PrintInvOrder %>"
                    Visible="false" OnClick="btnPrintOrderOut_Click" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                    OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                    onclick="window.location = window.location; return false;" />

                <asp:OperationsView runat="server" ID="OperationsView" />

                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
</asp:Content>

