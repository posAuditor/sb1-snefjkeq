<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceSortie.aspx.cs" Inherits="Sales_InvoiceSortie" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>

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
            width: 16%;
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
            <asp:ABFTextBox ID="txtSerial" runat="server"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Label runat="server" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.InvoiceSortie %></asp:Label>
            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.FromSalesOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>
        </div>



        <div class="InvoiceSection">
            <div class="Row">
                <div class="Column">
                    <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label runat="server" id="lblCurrency">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acStore" LabelText="<%$Resources:Labels,Store %>" ServiceMethod="GetStores"
                        IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>


                </div>
                <div class="Column">
                    <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames"
                        IsRequired="true" OnSelectedIndexChanged="acCustomer_SelectedIndexChanged" AutoPostBack="true"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkAddNewCustomer" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>

                    <asp:Panel runat="server" ID="pnlCustomerMesure" Visible="False">
                        <asp:AutoComplete runat="server" ID="acCustomerMesure" ValidationGroup="Save" ServiceMethod="GetContactMesure" LabelText="<%$Resources:Labels,SalesRep %>"></asp:AutoComplete>
                    </asp:Panel>
                    <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                </div>

                <div class="Column">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>" Visible="false"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>
                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>
                </div>

            </div>

        </div>


        <div class="InvoiceSection" style="display: none;">


            <div class="form" style="margin: auto; width: 90%;">
                <div class="right_col">
                    <span>


                        <asp:Panel runat="server" Visible="False">
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
                            <td colspan="9">


                                <div>
                                    <asp:LinkButton ID="lnkAddNewIteme" runat="server" CssClass="PlusBtn" Style="display: none;">[+]</asp:LinkButton>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">

                                                <th class="colWLarge"><%=Resources.Labels.Item %></th>
                                                <th class="colW"><%=Resources.Labels.Unit %></th>
                                                <th class="colW"><%=Resources.Labels.Quantity %> </th>
                                                <th class="colW"><%=Resources.Labels.Price %>  </th>

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
                            <td colspan="9"> 
                                <div>
                                    <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                        <tbody>
                                            <tr class="grdWoutHeader_row">

                                                <td class="colWLarge">
                                                    <asp:AutoComplete runat="server" LabelText="<%$Resources:Labels,Item %>" ID="acItem" ServiceMethod="GetItems"
                                                        VisibleText="false" ValidationGroup="AddItem" TabIndex="1" Style="background-color: transparent; border-radius: 0px!important"
                                                        OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"></asp:AutoComplete>

                                                </td>
                                                <td class="colW">

                                                    <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem" VisibleText="false"
                                                        IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                        AutoPostBack="true"></asp:AutoComplete>
                                                </td>
                                                <td class="colW">

                                                 <asp:ABFTextBox ID="txtQty" runat="server" CssClass="cls" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"
                                                        MinValue="0.001" DataType="Decimal" Text="1" IsRequired="true" TabIndex="3" ValidationGroup="AddItem">
                                                    </asp:ABFTextBox>
                                                    
                                                     </td>
                                                <td class="colW">


                                                    <asp:ABFTextBox ID="txtCost" runat="server" VisibleText="false"
                                                        MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"></asp:ABFTextBox>

                                                </td>

                                                <td class="colW">
                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRowBeforTax" runat="server" Text=""></asp:Label>

                                                </td>
                                                <td class="colW">

                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label>


                                                </td>
                                                <td class="colSmall" style="color: black">
                                                    <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" ValidationGroup="AddItem" runat="server">
                                                     <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important"></i>

                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnAddItemException" OnClick="btnAddItemException_OnClick" ValidationGroup="AddItem" runat="server"><i class="btnShartcatInvoice demo-icon icon-plus-circle" style=" font-size: 20px; padding-top: 10px;"></i></asp:LinkButton>
                                                    <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                                                        Text="<%$ Resources:Labels, Add %>" Visible="False" />
                                                    <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" Visible="False" OnClick="btnAddItemGroup_click"
                                                        Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                                                    <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Visible="False" Text="<%$ Resources:Labels, Clear %>"
                                                        OnClick="BtnClearItem_Click" />
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
                            <td colspan="9"> 
                                <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="ItemName" ItemStyle-CssClass="colWLarge" HeaderText="<%$Resources:Labels,Item %>" />
                                        <asp:BoundField DataField="UOMName" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField DataField="Quantity" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Quantity %>"
                                            DataFormatString="{0:0.####}" />
                                        <asp:BoundField DataField="UnitCost" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Price %>" DataFormatString="{0:0.########}" />
                                        <asp:BoundField DataField="Total" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Total %>" DataFormatString="{0:0.########}" />
                                        <asp:BoundField DataField="GrossTotal" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,GrossTotal %>"
                                            DataFormatString="{0:0.########}" />
                                        <asp:TemplateField ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                                <asp:Image ID="Image4" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>" ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" Visible='<%# this.DocStatus_ID==1 %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>" ItemStyle-CssClass="colSmall">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" Visible='<%# this.DocStatus_ID==1 %>' CommandName="Delete"
                                                    OnClientClick="return ConfirmSure();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:AuditorGridView>
                            </td>
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
                                <td style="width: 11%">
                                    <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                                        OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true"></asp:ABFTextBox>
                                </td>

                                <td></td>

                                <td></td>
                                <td>
                                    <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>" IsHideable="true"></asp:AutoComplete>

                                </td>
                                <td style="width: 6%"></td>
                                <td style="width: 7%">
                                    <asp:Label runat="server" Text="<%$Resources:Labels,Quantity %>"></asp:Label>
                                    <br />


                                </td>
                                <td style="width: 12%">
                                    <asp:Label runat="server" Text="<%$Resources:Labels,Price %>"></asp:Label>
                                    <br />

                                </td>
                                <td>
                                    <div class="btnDiv" style="padding-top: 24px;">
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
                    <span>
                        <%=Resources.Labels.AvailableQty %>: </span>
                    <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
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

                            <asp:AutoComplete Visible="False" runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>

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
                            <asp:AutoComplete runat="server" ID="acPriceName" ServiceMethod="GetItemPriceNames"
                                LabelText="<%$Resources:Labels,PriceType %>" OnSelectedIndexChanged="acPriceName_SelectedIndexChanged"
                                AutoPostBack="true" IsHideable="true"></asp:AutoComplete>


                            <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>
                            <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>


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
                    <tbody>
                        <tr>
                            <td>
                                <asp:ABFTextBox ID="txtPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td>
                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td>
                                <asp:ABFTextBox ID="txtAdditionals" runat="server" LabelText="<%$Resources:Labels,Additionals %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true"></asp:ABFTextBox></td>
                            <td>
                                <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal"></asp:ABFTextBox></td>

                        </tr>
                    </tbody>
                </table>




                <div class="form" style="margin: auto;">
                    <div class="right_col">
                        <%-- <div class="form">--%>

                        <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,RecipientName %>" Style="box-sizing: border-box;"
                            TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
                        <%-- </div>--%>
                    </div>
                    <div class="left_col" style="width: 34%!important;">
                        <div class="right_col">
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

                            </table>
                        </div>
                        <div class="left_col">
                            <table class="totals">

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


                            </table>
                        </div>
                        <div class="left_col">
                            <div style="clear: both;">
                            </div>
                            <table class="totals">
                                <tr>
                                    <td>
                                        <span class="lbl">
                                            <%=Resources.Labels.GrossTotal %>: </span>
                                    </td>
                                    <td style="color: red">
                                        <asp:Label ID="lblGrossTotal" runat="server" Text="0.0000"></asp:Label>
                                    </td>
                                </tr>
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
                <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />

                <asp:Button runat="server" ID="btnCancelApprove" Text="<%$ Resources:Labels,cancelApprove %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />


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
</asp:Content>

