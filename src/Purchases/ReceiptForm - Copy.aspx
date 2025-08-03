<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ReceiptForm - Copy.aspx.cs" Inherits="Purchases_ReceiptForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ucNewItemDescribed.ascx" TagPrefix="asp" TagName="ucNewItemDescribed" %>

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
            width: 22%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colW1Large {
            width: 31%;
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
                ReadOnly="true" Width="200" Style="text-align: center;">
            </asp:ABFTextBox>

            <asp:Label runat="server" Visible="false" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.Receipt %></asp:Label>

            <div runat="server" visible="false" id="divPurchaseOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.PurchaseOrderNo %></span>:
                <asp:Label ID="lblPurchaseOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>

            <asp:Nav runat="server" ID="ucNav" />
            <asp:Favorit runat="server" ID="Favorit1" />
        </div>
        <div class="InvoiceSection">
            <%--<div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
            --%>
            <div class="Row">
                <div class="Column">
                    <span>


                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                    <label style="display: none">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" Style="display: none" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" TabIndex="-1" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" AutoPostBack="true" OnSelectedIndexChanged="acStore_SelectedIndexChanged" LabelText="<%$Resources:Labels,Store %>"
                        IsRequired="true" ValidationGroup="AddItem"></asp:AutoComplete>

                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>

                </div>
                <div class="Column">
                    <br />
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>

                    <asp:LinkButton ID="lnkAddNewVendor" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    <asp:AutoComplete runat="server" ID="acVendor" ServiceMethod="GetContactNames" IsRequired="true"
                        OnSelectedIndexChanged="acVendor_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acdrivers" ServiceMethod="Getcar" LabelText="<%$Resources:Labels,DriverNamne %>"
                        ValidationGroup="Save"></asp:AutoComplete>
                </div>
                <div class="Column">

                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" Style="display: none" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true" VisibleText="false">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true">
                    </asp:ABFTextBox>

                    <asp:AutoComplete runat="server" ID="acCashAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,CashAccount %>"></asp:AutoComplete>

                    <asp:Panel runat="server" Visible="False">
                        <asp:AutoComplete runat="server" ID="acPaymentAddress" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,PaymentAddress %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acTelephone" ServiceMethod="GetContactDetails"
                            LabelText="<%$Resources:Labels,Telephone %>" IsHideable="true"></asp:AutoComplete>
                    </asp:Panel>





                   <%-- <label>
                        .
                    </label>--%>
                    <asp:DropDownList runat="server" ID="ddlTvae" AutoPostBack="true" OnSelectedIndexChanged="ddlTvae_SelectedIndexChanged">

                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>

                    </asp:DropDownList>

                </div>
            </div>





            <span></span>
            <asp:Panel runat="server" Visible="False">
                <asp:AutoComplete runat="server" ID="acAddress" ServiceMethod="GetContactDetails"
                    LabelText="<%$Resources:Labels,Address %>" IsHideable="true"></asp:AutoComplete>
                <asp:AutoComplete runat="server" ID="acShipAddress" ServiceMethod="GetContactDetails"
                    LabelText="<%$Resources:Labels,ShipAddress %>" IsHideable="true"></asp:AutoComplete>
            </asp:Panel>




            <%-- </div>
                <div class="left_col">--%>




            <%--  </div>
            </div>--%>





            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">

            <div class="container">
                <asp:ABFTextBox ID="txtBarcode" runat="server" TabIndex="0" LabelText="<%$Resources:Labels,Barcode %>" Style="background-color: #eadbf5; text-align: center; font-size: 22px; font-weight: bold"
                    OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" CssClass="barcode">
                </asp:ABFTextBox>
                <span>
                    <%=Resources.Labels.LastCustomerRecipt %>: </span>
                <asp:Label ID="Lb_LastCustomerRecipt" runat="server" Text=""></asp:Label>

                <div class="tb">  <asp:LinkButton ID="lnkAddNewIteme" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                    <table cellspacing="0" cellpadding="0" style="width: 100%">
                        <tr>
                            <td colspan="12">
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
                                                <th class="colW XSN" runat="server" id="ViewSN" >SerialNumber</th>
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
                                                    <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItemsExceptItemsCategories" ValidationGroup="AddItem" TabIndex="1"
                                                        OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                                        LabelText="<%$Resources:Labels,Item %>" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>

                                                </td>
                                                <td class="colW">
                                                    <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem" TabIndex="2"
                                                        IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                                        AutoPostBack="true" VisibleText="false" CssClass="barcode cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                                </td>

                                                <td class="colW">

                                                       <table>
                                                    <tr>
                                        
                                                        <td>
                                                                <asp:LinkButton ID="lnkGroupStoreIteme" runat="server" OnClick="lnkGroupIteme_Click" CssClass="PlusBtn2">[...]</asp:LinkButton>  
                                                        </td>

                                                        <td>
                                                             
                                                            
                                                    <asp:ABFTextBox ID="txtQty" runat="server" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" TabIndex="3"
                                                        MinValue="0.001" DataType="Decimal" Text="1" IsRequired="true" ValidationGroup="AddItem" VisibleText="false" CssClass="cls"
                                                        onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                               
                                 

                                                        </td>
                                                    </tr>
                                                </table>
                                                
               
                                                </td>
                                                
                                                
                                                
                                                
                                                
                                                
                                                
                                                
                                                
                                                 <td class="colW">
                                                   
                                                     
                                                     
                                                 <table>
                                                        <tr>
                                        
                                                            <td>
                                                                      <asp:LinkButton ID="lnkGetPriceIteme" runat="server" OnClick="lnkGetPriceIteme_Click" CssClass="PlusBtn1">[...]</asp:LinkButton>
                                                            </td>

                                                            <td>
                                                                 <asp:ABFTextBox ID="txtCost" runat="server" AutoPostBack="true" OnTextChanged="txtCost_TextChanged" TabIndex="4"
                                                        MinValue="0.00000001" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"
                                                        VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox>
                                               
                                                            </td>
                                                        </tr>
                                                    </table>
                                                     
                                                      </td>
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                     
                                                      </td>
                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtItemPercentageDiscount" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>" AutoPostBack="true" OnTextChanged="txtItemPercentageDiscount_TextChanged" TabIndex="5 "
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important">
                                                    </asp:ABFTextBox>
                                                </td>
                                                <td class="colW">
                                                    <asp:ABFTextBox ID="txtItemCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>" AutoPostBack="true" OnTextChanged="txtItemCashDiscount_TextChanged" TabIndex="6"
                                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important">
                                                    </asp:ABFTextBox></td>
                                                <td class="colW">
                                                    <span style="width: 30px!important; color: black"></span>
                                                    <asp:Label ID="lblTotalRow" runat="server" Text=""></asp:Label>


                                                </td>
                                                <td class="colW XSN" runat="server" id="EditXSN">

                                                    <asp:Label ID="lblSerialNumber" Visible="false" runat="server" Text=""></asp:Label>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" CssClass="PlusBtn">[+]</asp:LinkButton>

                                                </td>
                                                <td class="colSmall" style="color: black; text-align: center; ">
                                                    <asp:LinkButton ID="lnkAddItem" OnClick="btnAddItem_click" ValidationGroup="AddItem" runat="server" TabIndex="7">
                                                      <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px;color:black!important;width:34px;"></i>
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
                            <td colspan="12">
                                <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID,Item_ID"
                                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging" OnPreRender="gvItems_OnPreRender">
                                    <Columns>
                                         <asp:TemplateField ItemStyle-CssClass="colSmall"   HeaderText="<%$Resources:Labels,Serial %>"> 
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField  ItemStyle-CssClass="colWLarge" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />

                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" DataFormatString="{0:0.####}" />
                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="UnitCost" HeaderText="<%$Resources:Labels,Cost %>" DataFormatString="{0:0.########}" />

                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="PercentageDiscount" HeaderText="<%$Resources:Labels,PercentageDiscount %>"
                                            DataFormatString="{0:0.########}" />
                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="CashDiscount" HeaderText="<%$Resources:Labels,CashDiscount %>"
                                            DataFormatString="{0:0.########}" />

                                        <asp:BoundField  ItemStyle-CssClass="colW" DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                                            DataFormatString="{0:0.########}" />
                                        <asp:TemplateField ItemStyle-CssClass="colW XSN">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSerialNumberView" Visible='<%# !this.IsViewSerialNumber %>' CommandArgument='<%# Eval("ID").ToString() + ":" + Eval("Item_ID").ToString() %>' OnClick="lnkSerialNumberView_Click" runat="server">إضفط عنا</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  ItemStyle-CssClass="colSmall" FooterStyle-Width="80">
                                            <ItemTemplate>
                                               <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField  ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Edit %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="34" Visible='<%# this.DocStatus_ID==1 %>' ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
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
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                                </asp:ABFTextBox>






                            </th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtCashDiscount" runat="server" LabelText="<%$Resources:Labels,CashDiscount %>"
                                    MinValue="0" OnTextChanged="txtPercentageDiscount_TextChanged" AutoPostBack="true"
                                    ValidationGroup="Save" DataType="Decimal" IsHideable="true">
                                </asp:ABFTextBox>

                            </th>
                            <th class="colWx" colspan="2">
                                <asp:ABFTextBox ID="txtFirstPaid" runat="server" LabelText="<%$Resources:Labels,FirstPaid %>"
                                    MinValue="0" OnTextChanged="txtFirstPaid_TextChanged" AutoPostBack="true" ValidationGroup="Save"
                                    DataType="Decimal">
                                </asp:ABFTextBox></th>

                            <th class="colWx"></th>
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





                    <table class="table table-hover" style="border: 0 solid black; display: none;">
                        <tbody>
                            <tr>
                                <td style="width: 7%;"></td>
                                <td style="width: 11%"></td>
                                <td></td>

                                <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                                    VisibleText="false" CssClass="barcode cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')"
                                    Style="background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                <td></td>
                                <td style="width: 5%"></td>
                                <td></td>
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
                                <td>
                                    <asp:Panel runat="server" ID="pnlItemdescribed">
                                        <asp:AutoComplete runat="server" ID="acItemDescribed" ServiceMethod="GetItemsDescribed"
                                            AutoPostBack="true" ValidationGroup="AddItem"
                                            LabelText="<%$Resources:Labels,Itemdescribed %>"></asp:AutoComplete>
                                        <asp:LinkButton ID="lnkNewDescribed" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:ABFTextBox ID="txtCapacity" runat="server" LabelText="<%$Resources:Labels,Capacity %>"
                                        MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                                    </asp:ABFTextBox>
                                </td>
                                <td>

                                    <table class="table table-hover forHide" style="border: 0 solid black;">
                                        <tr>
                                            <td style="width: 80px;">
                                                <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,CapacityDistribution %>"></asp:Label></td>
                                            <td>
                                                <span><%=Resources.Labels.ActualQuantity %> : </span>
                                                <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>
                                        </tr>
                                        <tr>


                                            <td colspan="2">

                                                <asp:ABFTextBox ID="txtCapacities" OnTextChanged="txtCapacities_OnTextChanged" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                                                    ValidationGroup="AddItem" AutoPostBack="true" IsHideable="true" VisibleText="False"></asp:ABFTextBox>
                                            </td>
                                        </tr>
                                    </table>


                                </td>
                                <%-- <td></td>--%>
                                <td>

                                    <asp:Label runat="server" CssClass="forHide" Text="<%$Resources:Labels,Policy %>"></asp:Label>

                                    <asp:ABFTextBox ID="txtPolicy" runat="server" VisibleText="False"
                                        MinValue="0" DataType="FreeString" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,Policy %>" IsHideable="true"></asp:ABFTextBox>



                                    <%--  <asp:ABFTextBox ID="txtPolicy" runat="server" LabelText="<%$Resources:Labels,Policy %>"
                                    MinValue="0" IsRequired="True" DataType="FreeString" ValidationGroup="AddItem" IsHideable="true">
                                </asp:ABFTextBox>--%>

                                </td>

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
                <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                    <div class="form" style="width: 90%; margin: auto;">
                        <div class="right_col">


                            <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>


                            <asp:ABFTextBox ID="txtCItem" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                OnTextChanged="txtCodeItem_TextChanged" AutoPostBack="true" VisibleText="false" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtAmount')" Style="background-color: transparent; border-radius: 0px!important">
                            </asp:ABFTextBox>




                            <%--  <asp:ABFTextBox ID="txtQty" Text="1" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                            DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" MinValue="0.001">
                        </asp:ABFTextBox>--%>


                            <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true">
                            </asp:ABFTextBox>


                            <%--  <asp:ABFTextBox ID="txtCapacities" runat="server" LabelText="<%$Resources:Labels,CapacityDistribution %>"
                            ValidationGroup="AddItem" OnTextChanged="txtCapacities_OnTextChanged" AutoPostBack="true"></asp:ABFTextBox>
                        <br />--%>
                            <%-- <span><%=Resources.Labels.ActualQuantity %> : </span>
                        <asp:Label ID="lblQTyterminal" runat="server" Text=""></asp:Label>--%>


                            <%--   <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                            IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                            AutoPostBack="true" IsHideable="true"></asp:AutoComplete>--%>
                            <span>

                                <%-- LabelText="<%$Resources:Labels,BatchID %>"--%>
                                <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" CssClass="hiddencol"
                                    OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>

                                <asp:LinkButton ID="lnkNewBatch" runat="server" CssClass="PlusBtn hiddencol">[+]</asp:LinkButton>

                            </span>

                        </div>
                        <div class="left_col">
                            <%-- <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Cost %>"
                            MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem">
                        </asp:ABFTextBox>--%>


                            <%--  <asp:ABFTextBox ID="txtCode" IsRequired="True" runat="server" LabelText="<%$Resources:Labels,Code %>"
                            MinValue="0" DataType="FreeString" ValidationGroup="AddItem" IsHideable="true">
                        </asp:ABFTextBox>--%>

                            <%-- <asp:ABFTextBox ID="txtInvoiceDate" IsHideable="true" runat="server" ValidationGroup="AddItem" LabelText="<%$Resources:Labels,DateRequest %>"
                            DataType="Date" IsRequired="true">
                        </asp:ABFTextBox>--%>


                            <asp:Panel runat="server" ID="Panel1" Visible="False">
                                <%-- LabelText="<%$Resources:Labels,ProductionDate %>"--%>
                                <asp:ABFTextBox ID="txtProductionDate" CssClass="hiddencol" runat="server"
                                    Enabled="false" IsHideable="true">
                                </asp:ABFTextBox>

                                <%-- LabelText="<%$Resources:Labels,ExpirationDate %>"--%>
                                <asp:ABFTextBox ID="txtExpirationDate" CssClass="hiddencol" runat="server"
                                    Enabled="false" IsHideable="true">
                                </asp:ABFTextBox>
                            </asp:Panel>
                            <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                                TextMode="MultiLine">
                            </asp:ABFTextBox>
                        </div>
                    </div>

                    <br />
                    <%--<div class="btnDiv">
                    <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                    <asp:Button ID="btnAddItemGroup" CssClass="button" runat="server" OnClick="btnAddItemGroup_click"
                        Text="<%$ Resources:Labels, AddGroup %>" ValidationGroup="AddItem" />
                    <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearItem_Click" />
                </div>--%>
                </asp:Panel>
            </div>

            <div class="InvoiceSection" id="taxSection" runat="server" style="display: none">
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
                        TextMode="MultiLine" Width="100%">
                    </asp:ABFTextBox>
                </div>
                <br />
                <br />
                <div class="form" style="width: 700px; margin: auto;">
                    <div class="right_col">
                    </div>
                    <div class="left_col">
                        <%-- <table class="totals">
                            <tr>
                                <td>
                                    <span class="lbl">
                                        <%=Resources.Labels.Total %>: </span>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                                </td>
                            </tr>



                        </table>--%>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
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
                    Visible="false" OnClick="btnPrintOrderIn_Click" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                    OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                    onclick="window.location = window.location; return false;" />
                <asp:OperationsView runat="server" ID="OperationsView" />
                <asp:Button runat="server" ID="btnExpense" Text="<%$ Resources:Labels,Expenses %>" CssClass="button_big shortcut_save"
                    ValidationGroup="Save" OnClick="btnExpense_Click" />
                <%-- <asp:HyperLink ID="HL" runat="server" NavigateUrl='<%# String.Format("PurchasesExpenses.aspx?Receipt_ID={0}", this.Receipt_ID ) %>'
                        Text="<%$ Resources:Labels, Expenses %>" />--%>

                <div style="clear: both">
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfNewBatch" runat="server" />

        <asp:NewBatch ID="ucNewBatchID" runat="server" Title="<%$Resources:Labels,BatchID %>"
            AttributeType_ID="14" TargetControlID="cph$lnkNewBatch" OnNewBatchCreated="ucNewBatchID_NewBatchCreated"></asp:NewBatch>
        <asp:HiddenField ID="hfFastAddNew" runat="server" />


        <asp:ucNewItemDescribed
            OnNewItemDescribedCreated="ucNewItemDescribed_OnNewItemDescribedCreated"
            TargetControlID="cph$lnkNewDescribed"
            Title="<%$Resources:Labels,Itemdescribed %>"
            runat="server" ID="ucNewItemDescribed" />
        <asp:HiddenField ID="hfNewDescribed" runat="server" />
        <asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="lnkAddNewVendor"
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
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlFastAddCurrency" runat="server">
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtTaxNumber" runat="server" LabelText="الرقم الضريبي"></asp:ABFTextBox>
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
                            IsRequired="true" ValidationGroup="NewItem"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtCodeItemeCard" runat="server" LabelText="<%$Resources:Labels,CodeItem %>" CssClass="barcode"
                            ValidationGroup="NewItem"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtNameItemeCard" runat="server" LabelText="<%$Resources:Labels,Name %>"
                            IsRequired="true" ValidationGroup="NewItem"></asp:ABFTextBox>

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
                            IsRequired="true" ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtDefaultPriceItemeCard" runat="server" LabelText="<%$Resources:Labels,Price %>" MinValue="0.00000001"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>
                    </div>

                    <div class="left_col">
                        <asp:ABFTextBox ID="txtMinQtyItemeCard" runat="server" LabelText="<%$Resources:Labels,MinQty %>"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtMaxQtyItemeCard" runat="server" LabelText="<%$Resources:Labels,MaxQty %>"
                            ValidationGroup="NewItem" DataType="Decimal"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acSmallestUnit" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,SmallestUnit %>" IsRequired="true" ValidationGroup="NewItem"></asp:AutoComplete>
                        <asp:AutoComplete ID="acTaxItemeCard" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                            runat="server"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtPercentageDiscountItemeCard" runat="server" LabelText="<%$Resources:Labels,PercentageDiscount %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="NewItem"></asp:ABFTextBox>

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
        <asp:ModalPopupExtender ID="mpeSerialNumberView" runat="server" TargetControlID="HiddenField2"
            PopupControlID="pnlSerialNumberView" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
            BehaviorID="showPopUp" Y="300">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlSerialNumberView" runat="server" CssClass="pnlPopUp" Width="280">
            <div class="tcat">
                <asp:Button runat="server" class="close-btn" ID="Button6"></asp:Button>
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

        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:ModalPopupExtender ID="mpeSerialNumber" runat="server" TargetControlID="HiddenField1"
            PopupControlID="pnlSerialNumber" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
            BehaviorID="showPopUp" Y="300">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlSerialNumber" runat="server" CssClass="pnlPopUp" Width="280">
            <div class="tcat">
                <asp:Button runat="server" class="close-btn" ID="Button2"></asp:Button>
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
                    <asp:Button ID="Button5" runat="server" CssClass="button"
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

            <asp:ABFGridView runat="server" ID="gvPriceList1" GridViewStyle="GrayStyle"  >
                <Columns>
                      <asp:BoundField DataField="AttName" HeaderText="الاسم"
                        />
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

            <asp:ABFGridView runat="server" ID="gvQtyStoreList1" GridViewStyle="GrayStyle"  >
                <Columns>
                      <asp:BoundField DataField="StoreName" HeaderText="المستودع"
                        />
                    <asp:BoundField DataField="ItemeBalanceStore" HeaderText="الكمية"
                        DataFormatString="{0:0.####}" />
                     
                      
                </Columns>
            </asp:ABFGridView>


        </div>
    </asp:Panel>

</asp:Content>

