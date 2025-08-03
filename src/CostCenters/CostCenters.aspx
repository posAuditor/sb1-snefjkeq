<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CostCenters.aspx.cs" Inherits="CostCenters_CostCenters" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .treeNode
        {
            cursor: pointer;
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }

            .treeNode:hover
            {
                text-decoration: underline;
            }

        .treeNode2
        {
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNewCostCenter" runat="server" CssClass="collapse_add_link"
        Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkDeleteCostCenter" runat="server" CssClass="collapse_delete_link"
        OnClick="btnDelete_Click" OnClientClick="return ConfirmSure()" Text="<%$ Resources:Labels,Delete %>"></asp:LinkButton>
   
    
      <asp:LinkButton ID="btnCollapse" runat="server" CssClass="collapse_add_link"
        OnClick="btnCollapse_Click" OnClientClick="return CollapseAll()" Text="<%$ Resources:Labels,Close1 %>"></asp:LinkButton>
   
        <asp:LinkButton ID="btnExpand" runat="server" CssClass="collapse_add_link"
        OnClick="btnExpand_Click" OnClientClick="return ExpandAll()" Text="<%$ Resources:Labels,Open %>"></asp:LinkButton>
   

   <%--  <asp:Button ID="btnCollapse" runat="server" CssClass="collapse_add_link" Text=" " OnClientClick="return CollapseAll()"
        OnClick="btnCollapse_Click" />--%>
   <%-- <asp:Button ID="btnExpand" runat="server" CssClass="collapse_add_link" Text=" " OnClientClick="return ExpandAll()"
        OnClick="btnExpand_Click" />--%>
    <br />
    <br />
    <div style="clear: both;">
        <asp:TreeView ID="tvCostCenters" runat="server" ShowLines="True">
        </asp:TreeView>
    </div>
    <asp:HiddenField runat="server" ID="hfEditCostCenter" />
    <asp:ModalPopupExtender ID="mpeCreateCostCenter" runat="server" TargetControlID="lnkAddNewCostCenter"
        PopupControlID="pnlAddNewCostCenter" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeCreateCostCenter" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAddNewCostCenter" CssClass="pnlPopUp" runat="server" Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosePopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtCostCenterName" runat="server" ValidationGroup="NewAccount"
                    LabelText="<%$Resources:Labels,CostCenterName %>" IsRequired="true"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtCostCenterNameEN" runat="server" ValidationGroup="NewAccount"
                    LabelText="<%$ Resources:Labels,CostCenterNameEN %>" IsRequired="true"></asp:ABFTextBox>
                <div class="addOnlyPart">
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,Branch %>" OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acParentCostCenter" ServiceMethod="GetCostCenters"
                        ValidationGroup="NewAccount" IsRequired="true" LabelText="<%$Resources:Labels,ParentCostCenter %>"></asp:AutoComplete>

                    <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList ID="ddlType" runat="server"  OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Child  %>" Value="False"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Parent  %>" Value="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,OpenBalance %>"
                        DataType="Decimal" OnTextChanged="txtOpenBalance_TextChanged" AutoPostBack="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="NewAccount" LabelText="<%$Resources:Labels,StartFrom %>"
                        DataType="Date"></asp:ABFTextBox>
                </div>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewAccount" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveCostCenter" CssClass="button default_button" runat="server" OnClick="btnSaveCostCenter_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewAccount" />
                <asp:Button ID="BtnClearCostCenter" runat="server" CssClass="button" OnClick="BtnClearCostCenter_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">

        function ExpandAll() {
            $("#cph_tvCostCenters td a > img[alt*=Expand]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;

        }

        function CollapseAll() {
            $("#cph_tvCostCenters td a > img[alt*=Collapse]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;
        }

        function postBackByObject(accountNameAR, accountNameEN, id) {
            if (id != 0) {
                $("#cph_txtCostCenterName").val(accountNameAR);
                $("#cph_txtCostCenterNameEN").val(accountNameEN);
                $("#cph_hfEditCostCenter").val(id);
                HideAddOnlyPart();
                $find("mpeCreateCostCenter").show();
            }
            else {
                alert("<%=Resources.UserInfoMessages.CostCenterNameCantBeChanged %>");
        }
    }


    function HideAddOnlyPart() {
        $("#cph_acParentCostCenter_hfAutocomplete").val('0');
        $(".addOnlyPart").hide();
        $("#cph_BtnClearCostCenter").click(function () {
            $("#cph_txtCostCenterName").val('');
            $("#cph_txtCostCenterNameEN").val('');
            return false;
        });
        SetPopUpBottom();
    }
    </script>
</asp:Content>
