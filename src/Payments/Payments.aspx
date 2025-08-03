<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Payments.aspx.cs" Inherits="Payments_Payments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>


<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>
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

        .colW {
            width: 18%;
            color: black;
            border: 1px solid black;
            text-align: center;
        }

        .colWx {
            width: 18%;
            color: black;
            border: 1px solid white;
            text-align: center;
        }

        .colW1 {
            width: 18%;
            color: black;
            border: 2px solid black;
            text-align: center;
            height: 30px;
        }

        /*Small gridWoutHeader view grdWoutHeader*/
        .grdWoutHeader {
            -border-style: solid;
            -border-color: #444;
            -border-width: 1px;
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
        }

        .gridWoutHeader_mobile {
            table-layout: fixed;
            word-wrap: break-word;
        }

        .grdWoutHeader_header th {
            background: #f9f9f9;
            height: 24px;
            border-bottom: solid 1px silver;
            padding: 2px 3px 1px 3px;
            font-size: 12px;
            display: none;
        }

        .grdWoutHeader_fixedheader {
            position: absolute;
            background-color: #D0D0D0;
            margin-top: -12px;
            margin-right: -2px;
            border-top: solid 1px;
            border-right: solid 1px;
            border-left: solid 1px;
            width: 662px;
            height: 40px;
        }

        .grdWoutHeader_empty td {
            border: solid 1px #D0D0D0;
            color: Red;
            padding: 10px;
            background: #F7F7F7;
            font-weight: bold;
        }


        .grdWoutHeader_pager {
            background: #ffffff;
        }

            .grdWoutHeader_pager td {
                border: 0px;
            }

            .grdWoutHeader_pager table {
                margin: auto;
            }

                .grdWoutHeader_pager table td {
                    border: 1px solid #557C93;
                    margin: 2px;
                    border-spacing: 10px;
                    font-family: "Droid Arabic Kufi",Arial;
                    padding: 5px;
                    text-align: center;
                    color: #575757;
                    background-color: #F5F5FF;
                    font-size: 20px;
                }

                    .grdWoutHeader_pager table td a {
                        font-weight: bold;
                        color: #4c9ccc;
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



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server" Visible="false"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>

            <asp:Label runat="server" ID="lblTitle" Visible="false" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"></asp:Label>


            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.PurchaseOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>

            <asp:Nav runat="server" ID="ucNav" />

            <asp:Favorit runat="server" ID="ucFavorit" />
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label style="display: none">
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" Style="display: none"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" Style="display: none" Visible="false" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
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
            <asp:Panel ID="pnlAddDetail" runat="server" DefaultButton="btnAddDetail">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acDebitAccount" ServiceMethod="GetChartOfAccounts"
                            ValidationGroup="AddDetail" IsRequired="true" LabelText="<%$Resources:Labels,DebitAccount %>"
                            OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                        <br />
                        <span>
                            <%=Resources.Labels.Balance %>: </span>
                        <asp:Label ID="lblDebitBalance" runat="server" Text=""></asp:Label>
                        <asp:ABFTextBox ID="txtAmount" runat="server" LabelText="<%$Resources:Labels,Amount %>"
                            MinValue="0.0001" DataType="Decimal" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged" IsRequired="true" ValidationGroup="AddDetail"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtDiscount" runat="server" AutoPostBack="true" OnTextChanged="txtDiscount_TextChanged" LabelText="<%$Resources:Labels,CashDiscount %>"
                            MinValue="0.0001" DataType="Decimal" ValidationGroup="AddDetail"></asp:ABFTextBox>


                        <asp:AutoComplete runat="server" ID="acVendorsSecond" ServiceMethod="GetVendorsSecond" LabelText="مورد المصروفات"
                            ValidationGroup="Save"></asp:AutoComplete>
                        <asp:LinkButton ID="lnkAddNewCustomer" runat="server" CssClass="PlusBtn" OnClick="lnkAddNewCustomer_Click">[+]</asp:LinkButton>



                        <table>
                            <tr>
                                <td><span></span>
                                    <br />
                                    <asp:CheckBox ID="chkIsTaxFound" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="chkIsTaxFound_CheckedChanged" />

                                </td>
                                <td>

                                    <asp:ABFTextBox ID="txtTaxFound" runat="server" LabelText="مبلغ الضريبة" Visible="false"
                                        MinValue="0" DataType="Decimal" ValidationGroup="AddDetail" Enabled="false"></asp:ABFTextBox>

                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acCreditAccount" ServiceMethod="GetChartOfAccounts"
                            ValidationGroup="AddDetail" IsRequired="true" LabelText="<%$Resources:Labels,CreditAccount %>"
                            OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                        <br />
                        <span>
                            <%=Resources.Labels.Balance %>: </span>
                        <asp:Label ID="lblCreditBalance" runat="server" Text=""></asp:Label>
                        <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acDetailCostCenter" ServiceMethod="GetCostCenters"
                            LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true"></asp:AutoComplete>



                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div class="form" style="width: 600px; margin: auto;">
                    <asp:ABFTextBox ID="txtDetailNotes" runat="server" LabelText="<%$Resources:Labels,Statement %>"
                        TextMode="MultiLine" Style="width: 100%;"></asp:ABFTextBox>
                </div>
                <div style="clear: both">
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddDetail" />
                </div>
                <br />
                <div class="btnDiv">
                    <asp:Button ID="btnAddDetail" CssClass="button" runat="server" OnClick="btnAddDetail_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddDetail" />
                    <asp:Button ID="BtnClearDetail" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                        OnClick="BtnClearDetail_Click" />
                </div>
            </asp:Panel>
        </div>
        <div class="InvoiceSection">
            <asp:ABFGridView runat="server" ID="gvDetails" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnRowDeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvDetails_PageIndexChanging"
                OnSelectedIndexChanging="gvDetails_SelectedIndexChanging">
                <Columns>
                    <asp:BoundField DataField="DebitAccountName" HeaderText="<%$Resources:Labels,DebitAccount %>" />
                    <asp:BoundField DataField="CreditAccountName" HeaderText="<%$Resources:Labels,CreditAccount %>" />
                    <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,Amount %>" DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="SalesRepName" HeaderText="<%$Resources:Labels,SalesRep %>" ItemStyle-CssClass="SalesRepCol" />
                    <asp:BoundField DataField="CostCenterName" HeaderText="<%$Resources:Labels,CostCenter%>" />
                    <asp:BoundField DataField="Discount" HeaderText="خصم" />
                    <asp:BoundField DataField="Tax" HeaderText="الضريبة" />
                     <asp:BoundField DataField="VendorsSecondname" HeaderText="مورد المصروفات" />

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
        </div>
        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,RecipientName %>" Style="box-sizing: border-box;"
                    TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
            </div>
            <br />
            <br />
            <div class="form" style="width: 500px; margin: auto;">
                <div class="right_col">
                </div>
                <div class="left_col">
                    <table class="totals">
                        <tr>
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.Total%>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                            </td>
                        </tr>
                    </table>
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
            <asp:Button runat="server" ID="btnCancelApprove" Text="<%$ Resources:Labels,NotApprove %>" CssClass="button_big shortcut_approve"
                ValidationGroup="Save" Visible="false" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />









            <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                Visible="false" OnClick="btnPrint_Click" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
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
    </div>
    <asp:HiddenField ID="hfFastAddNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="hfFastAddNew"
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
                <asp:ABFTextBox ID="txtVatNumber" runat="server" LabelText="الرقم الضريبي"
                    IsRequired="true" ValidationGroup="FastAddNew"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtNoteVender" runat="server" LabelText=" ملاحظات"
                    ValidationGroup="FastAddNew"></asp:ABFTextBox>

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
