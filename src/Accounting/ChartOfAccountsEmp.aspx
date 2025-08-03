<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ChartOfAccountsEmp.aspx.cs" Inherits="Accounting_ChartOfAccountsEmp" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .treeNode {
            cursor: pointer;
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }

            .treeNode:hover {
                text-decoration: underline;
            }

        .treeNode2 {
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNewAccount" runat="server" CssClass="collapse_add_link"
        Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkDeleteAccount" runat="server" CssClass="collapse_delete_link"
        OnClick="btnDelete_Click" OnClientClick="return ConfirmSure()" Text="<%$ Resources:Labels,Delete %>"></asp:LinkButton>


    <asp:LinkButton ID="btnCollapse" runat="server" CssClass="collapse_add_link"
        OnClick="btnCollapse_Click" OnClientClick="return CollapseAll()" Text="<%$ Resources:Labels,Close1 %>"></asp:LinkButton>

    <asp:LinkButton ID="btnExpand" runat="server" CssClass="collapse_add_link"
        OnClick="btnExpand_Click" OnClientClick="return ExpandAll()" Text="<%$ Resources:Labels,Open %>"></asp:LinkButton>
    <br />
    <br />
    <div style="clear: both;">
        <asp:TreeView ID="tvAccounts" runat="server" ShowLines="True">
        </asp:TreeView>
    </div>
    <asp:HiddenField runat="server" ID="hfEditAccount" />
    <asp:ModalPopupExtender ID="mpeCreateAccount" runat="server" TargetControlID="lnkAddNewAccount"
        PopupControlID="pnlAddNewAccount" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeCreateAccount" Y="600">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAddNewAccount" CssClass="pnlPopUp" runat="server" Width="588">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosePopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 98%; margin: auto;">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtIdParentAccount" Style="display: none;" runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtBranch" Style="display: none;" runat="server"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtAccountName" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,AccountName %>"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAccountNameEN" runat="server" ValidationGroup="NewAccount"
                        LabelText="<%$ Resources:Labels,AccountNameEN %>"  ></asp:ABFTextBox>

                    <asp:AutoComplete runat="server" IsException="True" ID="acBranch" ServiceMethod="GetBranchs" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,Branch %>" AutoPostBack="true" OnSelectedIndexChanged="acBranch_SelectedIndexChanged"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acParentAccount" ServiceMethod="GetChartOfAccountsWithoutBankAssetEmployeVendorCustomer"
                        ValidationGroup="NewAccount" IsException="True" IsRequired="true" LabelText="<%$Resources:Labels,ParentAccount %>" OnSelectedIndexChanged="acParentAccount_OnSelectedIndexChanged"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <div class="addOnlyPart"></div>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="txtStartFrom_TextChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,OpenBalance %>"
                        OnTextChanged="txtOpenBalance_TextChanged" AutoPostBack="true" DataType="Decimal"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,StartFrom %>"
                        OnTextChanged="txtStartFrom_TextChanged" AutoPostBack="true" DataType="Date"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsException"
                        LabelText="<%$Resources:Labels,OppositeAccount %>" Enabled="false"></asp:AutoComplete>

                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewAccount" />
            </div>
            <div style="clear: both">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveAccount" CssClass="button default_button" runat="server" OnClick="btnSaveAccount_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewAccount" />
                <asp:Button ID="BtnClearAccount" runat="server" CssClass="button" OnClick="BtnClearAccount_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">

        function ExpandAll() {
            $("#cph_tvAccounts td a > img[alt*=Expand]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            $("#cph_tvAccounts td a > img[alt*=توسيع]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;

        }

        function CollapseAll() {
            $("#cph_tvAccounts td a > img[alt*=Collapse]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            $("#cph_tvAccounts td a > img[alt*=طي]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;
        }

        function postBackByObject(accountNameAR, accountNameEN, id, branch, parentAccount, Currency, openBalance, StartFrom) {
            //if (id != 0) {
            $("#cph_txtAccountName").val(accountNameAR);
            $("#cph_txtAccountNameEN").val(accountNameEN);
            $("#cph_txtOpenBalance").val(openBalance);

            $('#cph_acParentAccount_txtAutoCompleteText').focus();
            $('#cph_acParentAccount_txtAutoCompleteText').change();
            $('#cph_acParentAccount_txtAutoCompleteText').blur();

            $("#cph_acParentAccount_txtAutoCompleteText").val('');
            $("#cph_acParentAccount_hfAutocomplete").val('');
            $("#cph_txtIdParentAccount").val('');

            //refille parent account
            if ($('#cph_acParentAccount_AutocompleteListContainer > ul >li').length == 0) {
                $('#cph_acParentAccount_txtAutoCompleteText').focus();
                $('#cph_acParentAccount_txtAutoCompleteText').change();
                $('#cph_acParentAccount_txtAutoCompleteText').blur();
            }
            $('#cph_acParentAccount_AutocompleteListContainer > ul >li').each(function (i) {
                if ($(this).attr('autocompleteid') === parentAccount) {
                    $("#cph_acParentAccount_txtAutoCompleteText").val($(this)[0].innerHTML);
                    $("#cph_acParentAccount_hfAutocomplete").val(parentAccount);
                    $("#cph_txtIdParentAccount").val(parentAccount);
                    $('#cph_acBranch_txtAutoCompleteText').focus();
                    $('#cph_acBranch_txtAutoCompleteText').change();
                    $('#cph_acBranch_txtAutoCompleteText').blur();
                }
            });
            $('#cph_acBranch_txtAutoCompleteText').focus();
            $('#cph_acBranch_txtAutoCompleteText').change();
            $('#cph_acBranch_txtAutoCompleteText').blur();

            $("#cph_acBranch_hfAutocomplete").val('');
            $("#cph_txtBranch").val('');
            $("#cph_acBranch_txtAutoCompleteText").val('');
            $('#cph_acBranch_AutocompleteListContainer > ul >li').each(function (i) {
                if (branch === "") {
                   // branch = "2";
                   // $(this)[0].innerHTML = "الرئيسي";
                }
                if ($(this).attr('autocompleteid') === branch) {
                    $("#cph_acBranch_txtAutoCompleteText").val($(this)[0].innerHTML);
                    $("#cph_acBranch_hfAutocomplete").val(branch);
                    $("#cph_txtBranch").val(branch);

                } else {
                    $('#cph_acBranch_txtAutoCompleteText').focus();
                }
            });

            $("#cph_hfEditAccount").val(id);
            HideAddOnlyPart();
            $find("mpeCreateAccount").show();
            // }
            <%-- else {
                alert("<%=Resources.UserInfoMessages.AccountNameCantBeChanged %>");--%>
            //}

        }

        function HideAddOnlyPart() {
            $("#cph_acParentAccount_hfAutocomplete").val('0');
            $(".addOnlyPart").hide();
            $("#cph_BtnClearAccount").click(function () {
                $("#cph_txtAccountName").val('');
                $("#cph_txtAccountNameEN").val('');
                return false;
            });
            SetPopUpBottom();
        }
    </script>
</asp:Content>
