<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InventoryTransfer.aspx.cs" Inherits="Inv_InventoryTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
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









</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <span>
                <%=Resources.Labels.Serial %></span>:<asp:ABFTextBox ID="txtSerial" runat="server" Visible="false"
                    ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Nav runat="server" ID="ucNav" />



        </div>
        <div class="InvoiceSection">







            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        DataType="Date" IsRequired="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acdrivers" ServiceMethod="Getcar" LabelText="<%$Resources:Labels,DriverNamne %>"
                        ValidationGroup="Save"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acToBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,ToBranch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <span>
                <%=Resources.Labels.AvailableQty %> -  <%=Resources.Labels.Store %>: </span>
            <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>

            <br />
            <span>
                <%=Resources.Labels.AvailableQty %>  -  <%=Resources.Labels.ToStore %>: </span>
            <asp:Label ID="lblToStoreAvailableQty" runat="server" Text=""></asp:Label>

            <div class="tb">
                <table cellspacing="0" cellpadding="0" style="width: 100%">
                    <tr>
                        <td colspan="9">


                            <div>

                                <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="grdWoutHeader_row">


                                            <th class="colW"><%=Resources.Labels.Store %></th>
                                            <th class="colW"><%=Resources.Labels.CodeItem %></th>

                                            <th class="colW"><%=Resources.Labels.Item %></th>
                                            <th class="colW"><%=Resources.Labels.Unit %></th>

                                            <th class="colW"><%=Resources.Labels.ToStore %></th>
                                            <th class="colW"><%=Resources.Labels.Quantity %></th>

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

                                            <td class="colW">

                                                <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                                                    IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged" VisibleText="false"
                                                    Style="background-color: transparent; border-radius: 0px!important"
                                                    AutoPostBack="true"></asp:AutoComplete>
                                            </td>
                                            <td class="colW">
                                                <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,CodeItem %>"
                                                    OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" VisibleText="false"></asp:ABFTextBox></td>

                                            <td class="colW">
                                                <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                                                    OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"
                                                    LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>
                                            </td>
                                            <td class="colW">
                                                <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                                                    IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged" Style="background-color: transparent; border-radius: 0px!important"
                                                    AutoPostBack="true" VisibleText="false"></asp:AutoComplete>
                                            </td>
                                            <td class="colW">
                                                <asp:AutoComplete runat="server" ID="acToStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,ToStore %>"
                                                    IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acToStore_SelectedIndexChanged" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"
                                                    AutoPostBack="true"></asp:AutoComplete>
                                            </td>
                                            <td class="colW">
                                                <asp:ABFTextBox ID="txtQty" runat="server" LabelText="<%$Resources:Labels,Quantity %>"
                                                    MinValue="0.001" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem" VisibleText="false" Style="background-color: transparent; border-radius: 0px!important"></asp:ABFTextBox></td>

                                            <td class="colSmall" style="color: black; text-align: center;">
                                                <asp:LinkButton ID="LinkButton1" OnClick="btnAddItem_click" TabIndex="8" ValidationGroup="AddItem" runat="server"> 
                                                          <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important"></i></asp:LinkButton>
                                            </td>

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
                        <td colspan="9">
                            <asp:AuditorGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                                OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
                                <Columns>
                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="StoreName" HeaderText="<%$Resources:Labels,Store %>" />

                                    <%--<asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />--%>
                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="Barcode" HeaderText="<%$Resources:Labels,CodeItem %>" />

                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="ToStoreName" HeaderText="<%$Resources:Labels,ToStore %>" />
                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>"
                                        DataFormatString="{0:0.####}" />

                                    <asp:BoundField ItemStyle-CssClass="colW" DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                                    <asp:TemplateField ItemStyle-CssClass="colSmall">
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Edit %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$ Resources:Labels, Delete %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                                OnClientClick="return ConfirmSure();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:AuditorGridView>

                        </td>
                    </tr>

                </table>
            </div>

            <table style="width: 100%; display: none;">
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td style="display: none">


                        <asp:Panel runat="server" ID="pnlItemdescribed">
                            <asp:AutoComplete runat="server" ID="acItemDescribed" OnSelectedIndexChanged="acItemDescribed_OnSelectedIndexChanged" LabelText="<%$Resources:Labels,Itemdescribed %>" ServiceMethod="GetItemsDescribed"
                                AutoPostBack="true" ValidationGroup="AddItem"></asp:AutoComplete>
                        </asp:Panel>
                    </td>
                    <td></td>


                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <div class="btnDiv">
                            <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                                Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                            <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                                OnClick="BtnClearItem_Click" />
                        </div>
                    </td>
                </tr>
            </table>
            <div style="clear: both">
            </div>



        </div>
        <div style="clear: both">
        </div>
        <div class="InvoiceSection" style="display: none;">
            <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">


                        <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                            LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                            ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>


                        <br />

                        <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                            TextMode="MultiLine"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">

                        <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                            OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                            Enabled="false" IsHideable="true"></asp:ABFTextBox>
                        <br />


                        <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumber %>"
                            MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>

                    </div>
                </div>


            </asp:Panel>
        </div>
        <div class="InvoiceSection">
        </div>
        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
            </div>
            <br />
            <br />
        </div>
        <div class="InvoiceSection align_right">
            <div class="validationSummary">
                <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
            </div>
            <asp:Button runat="server" ID="btnSave" Text="<%$Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                ValidationGroup="Save" OnClick="BtnSave_Click" />

            <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />




            <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />



            <asp:Button runat="server" ID="btnPrint" Text="<%$Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                Visible="false" OnClick="btnPrint_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
            </div>
        </div>
    </div>
</asp:Content>
