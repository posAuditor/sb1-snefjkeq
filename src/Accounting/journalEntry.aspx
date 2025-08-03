<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="journalEntry.aspx.cs" Inherits="Accounting_JournalEntry" %>

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
            width: 10%;
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


    <script>
        function setSeriaFocus(e) {
          <%--  if (e.keyCode == 13) {
                document.getElementById('<%= LinkButton1.ClientID %>').click();
                document.getElementById('<%= txtQty.ClientID %>').select();
            }--%>
        }

    </script>

    <style>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" Visible="false" runat="server"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>

            <asp:Nav runat="server" ID="ucNav" />


        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label style="display: none">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" Style="display: none" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" TabIndex="0" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acOperationType" ServiceMethod="GetOperationTypes"
                        LabelText="<%$Resources:Labels,Type %>" IsRequired="true" ValidationGroup="Save" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <%-- <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddDetail">--%>
            <div class="tb">
                <table cellspacing="0" cellpadding="0" style="width: 100%">
                    <tr>


                        <td colspan="13">


                            <div>

                                <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="grdWoutHeader_row">
                                            <th class="colW"><%=Resources.Labels.AccountName %></th>
                                            <th class="colW"><%=Resources.Labels.Debit %></th>
                                            <th class="colW"><%=Resources.Labels.Credit %></th>
                                            <th class="colW"><%=Resources.Labels.Notes %></th>
                                            <th class="colW"><%=Resources.Labels.CostCenter %></th>
                                            <th class="colW"><%=Resources.Labels.Date %></th>
                                            <th class="colW"><%=Resources.Labels.Tax %></th>
                                            <th class="colW"><%=Resources.Labels.Tax %></th>
                                            <th class="colW"><%=Resources.Labels.Percentage %></th>




                                            <th class="colW">مورد مصروفات</th>
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
                        <td colspan="13">
                            <div>
                                <table class="grdWoutHeader11" cellspacing="0" cellpadding="0" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="grdWoutHeader_row">
                                            <td class="colW">
                                                <label style="display: none">
                                                    <%=Resources.Labels.Type %></label>
                                                <asp:DropDownList ID="ddlType" runat="server" onkeydown="return focusOnNext(event, 'cph_acAccount')" Style="background-color: transparent; display: none; box-sizing: content-box; border-radius: 0px" CssClass="cls">
                                                    <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="1"></asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:AutoComplete runat="server" ID="acAccount" AutoPostBack="true" VisibleText="false" TabIndex="1" ServiceMethod="GetChartOfAccountsCheledronly" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtDebit')" style="background-color: transparent; border-radius: 0px!important"
                                                    ValidationGroup="AddDetail" IsRequired="true" LabelText="<%$Resources:Labels,AccountName %>"></asp:AutoComplete>


                                            </td>
                                            <td class="colW">
                                                <asp:ABFTextBox ID="txtDebit" runat="server" AutoPostBack="true" TabIndex="2" OnTextChanged="txtDebit_TextChanged" LabelText="<%$Resources:Labels,Debit %>" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtCredit')" Style="text-align: center; background-color: transparent; border-radius: 0px!important"
                                                    MinValue="0" DataType="Decimal" VisibleText="false" ValidationGroup="AddDetail"></asp:ABFTextBox>


                                            </td>
                                            <td class="colW">

                                                <asp:ABFTextBox ID="txtCredit" runat="server" AutoPostBack="true" OnTextChanged="txtCredit_TextChanged" TabIndex="3" LabelText="<%$Resources:Labels,Credit %>" CssClass="cls" Style="text-align: center; background-color: transparent; border-radius: 0px!important"
                                                    MinValue="0" DataType="Decimal" VisibleText="false" ValidationGroup="AddDetail"></asp:ABFTextBox>


                                            </td>
                                            <td class="colW">
                                                <asp:ABFTextBox ID="txtDetailNotes" CssClass="cls" Style="background-color: transparent; border-radius: 0px!important"
                                                    runat="server" VisibleText="false" LabelText="<%$Resources:Labels,Notes %>"></asp:ABFTextBox>
                                            </td>

                                            <td class="colW">
                                                <asp:AutoComplete runat="server" ID="acDetailCostCenter" AutoPostBack="true" TabIndex="4"
                                                    VisibleText="false" ServiceMethod="GetCostCenters" CssClass="cls"
                                                    onkeyup="return focusOnNext(event, 'cph_btnAddDetail')" style="background-color: transparent; border-radius: 0px!important"
                                                    LabelText="<%$Resources:Labels,CostCenter %>"></asp:AutoComplete>

                                            </td>
                                            <td class="colW">

                                                <asp:ABFTextBox ID="txtDate" runat="server" LabelText="<%$Resources:Labels,Date %>"
                                                    DataType="Date" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_btnAddDetail')" Style="background-color: transparent; border-radius: 0px!important" VisibleText="false"
                                                    IsRequired="true"></asp:ABFTextBox>
                                            </td>


                                            <td class="colW">

                                                <asp:AutoComplete runat="server" ID="acItemTax" ServiceMethod="GetTaxes" 
                                                    LabelText="<%$Resources:Labels,Tax %>" VisibleText="false" 
                                                    Style="background-color: transparent; display: none; box-sizing: content-box; border-radius: 0px" CssClass="cls"></asp:AutoComplete>
                                            </td>


                                            <td class="colW">
                                                <asp:CheckBox ID="chkIsTaxFound" runat="server" OnCheckedChanged="chkIsTaxFound_CheckedChanged" />

                                            </td>

                                            <td class="colW"></td>




                                            <td class="colW">

                                                <asp:AutoComplete runat="server" ID="acVendorsSecond" VisibleText="false" ServiceMethod="GetVendorsSecond" LabelText="مورد المصروفات" CssClass="cls" onkeyup="return focusOnNext(event, 'cph_txtCredit')" Style="text-align: center; background-color: transparent; border-radius: 0px!important"></asp:AutoComplete>
                                            </td>







                                            <td style="color: black; text-align: center;" class="colSmall">
                                                <asp:LinkButton ID="btnAddDetail" TabIndex="5" OnClick="btnAddDetail_click" ValidationGroup="AddDetail" runat="server">
                                                               <i class="demo-icon icon-plus" style="font-size: 20px; padding-top: 10px;color:black!important"></i>
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
                            <asp:AuditorGridView runat="server" ID="gvDetails" GridViewStyle="GrayStyle" DataKeyNames="ID"
                                OnRowDeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvDetails_PageIndexChanging"
                                OnSelectedIndexChanging="gvDetails_SelectedIndexChanging1">
                                <Columns>
                                    <asp:BoundField DataField="AccountName" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,AccountName %>" />
                                    <asp:BoundField DataField="DebitAmount" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Debit %>"
                                        DataFormatString="{0:0.####}" />
                                    <asp:BoundField DataField="CreditAmount" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Credit %>"
                                        DataFormatString="{0:0.####}" />
                                    <asp:BoundField DataField="Description" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Notes%>" />
                                    <asp:BoundField DataField="CostCenterName" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,CostCenter%>" />
                                    <asp:BoundField DataField="OperDate" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Notes%>" />

                                    <asp:BoundField DataField="PercentTaxValue" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Tax%>" />

                                    <asp:TemplateField FooterStyle-Width="80" ItemStyle-CssClass="colW">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkApprove" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsTaxFound").ToString())%>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PercentTaxValue" ItemStyle-CssClass="colW" HeaderText="<%$Resources:Labels,Tax%>" />

                                    <asp:BoundField DataField="VendorsSecondname" ItemStyle-CssClass="colW" HeaderText="مورد المصروفات" />

                                    <asp:TemplateField FooterStyle-Width="80" ItemStyle-CssClass="colSmall">
                                        <ItemTemplate>
                                            <asp:Image ID="Image3" runat="server" Width="34" ImageUrl="../images/black.gif" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>" ItemStyle-CssClass="colSmall">
                                        <ItemTemplate>
                                            <asp:ImageButton Visible='<%# this.DocStatus_ID==1 && Eval("TypeTax").ToString()!="-1" %>' ValidationGroup="AddDetail1" Width="34" ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>" ItemStyle-CssClass="colSmall">
                                        <ItemTemplate>
                                            <asp:ImageButton Visible='<%# this.DocStatus_ID==1 && Eval("TypeTax").ToString()!="-1"%>' Width="34" ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                                OnClientClick="return ConfirmSure();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:AuditorGridView>
                        </td>
                    </tr>
                   

                    <tr>
                        <th class="colWx"></th>
                        <th class="colWx">
                            <table class="totals">
                                <tr>
                                    <td>
                                        <span class="lbl">
                                            <%=Resources.Labels.TotalDebit %>: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalDebit" runat="server" Text="0.0000"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </th>
                        <th class="colWx">

                            <table class="totals">
                                <tr>
                                    <td>
                                        <span class="lbl">
                                            <%=Resources.Labels.TotalCredit%>: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalCredit" runat="server" Text="0.0000"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </th>
                        <th class="colWx">


                            <table class="totals">
                                <td>
                                    <span class="lbl">
                                        <%=Resources.Labels.Difference %>: </span>
                                </td>
                                <td>
                                    <asp:Label ID="lblDifference" runat="server" Text="0.0000"></asp:Label>
                                </td>
                            </table>


                        </th>
                        <th class="colWx"></th>
                        <th style="width: 35px!important; color: black"></th>
                        <th style="width: 35px!important; color: black"></th>
                        <th style="width: 30px!important; color: black"></th>
                    </tr>



                </table>
            </div>


            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddDetail" />
            </div>

            <div class="btnDiv">
                <%--  <asp:Button ID="btnAddDetail" CssClass="button" runat="server" OnClick="btnAddDetail_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddDetail" />--%>
                <asp:Button ID="BtnClearDetail" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>" Style="display: none;"
                    OnClick="BtnClearDetail_Click" />
            </div>
            <%--</asp:Panel>--%>
        </div>

        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                    TextMode="MultiLine" Width="50%"></asp:ABFTextBox>

                <div class=" align_right">
                    <div class="validationSummary">
                        <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                    </div>

                    <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                        ValidationGroup="Save" OnClick="BtnSave_Click" />
                    <asp:Button runat="server" ID="btnApprove" Text="<%$ Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                        ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />

                    <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                        OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />


                    <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                        Visible="false" OnClick="btnPrint_Click" ValidationGroup="Save" />
                    <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                        OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                    <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                        onclick="window.location = window.location; return false;" />
                    <div style="clear: both">
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
