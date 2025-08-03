<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Taxes.aspx.cs" Inherits="Accounting_Taxes" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
        AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
        ExpandDirection="Vertical" SuppressPostBack="true">
    </asp:CollapsiblePanelExtender>
    <div style="clear: both;">
    </div>
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
        <div class="tcat">
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server"></asp:ABFTextBox>

                <div runat="server" id="divHasBill" style="display: none;">
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlActiveNotActive" runat="server">
                        <asp:ListItem Text="نشط" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="غير نشط" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvTaxes" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvTaxes_RowDeleting" OnPageIndexChanging="gvTaxes_PageIndexChanging"
        OnSelectedIndexChanging="gvTaxes_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="PercentageValue" HeaderText="<%$Resources:Labels,PercentageValue %>" />
            <asp:BoundField DataField="OnInvoiceTypeName" HeaderText="<%$Resources:Labels,OnSales %>" />
            <asp:BoundField DataField="OnReceiptTypeName" HeaderText="<%$Resources:Labels,OnPurchases %>" />
            <asp:BoundField DataField="OnDocCreditTypeName" HeaderText="<%$Resources:Labels,OnDocCredit %>" />
            <asp:BoundField DataField="PurchaseAccountName" HeaderText="<%$Resources:Labels,TaxPurchaseAccount %>" />
            <asp:BoundField DataField="SalesAccountName" HeaderText="<%$Resources:Labels,TaxSalesAccount %>" />
            <asp:TemplateField HeaderText="تعديل النسبة ">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkActive" CommandArgument='<%#Eval("ID") %>' runat="server" OnClick="lnkActive_Click">تعديل النسبة </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" Visible='<%# !Convert.ToBoolean(Eval("IsLocked")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete" Visible='<%# !Convert.ToBoolean(Eval("IsLocked")) %>'
                        OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">

                    <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtPercentageValue" CssClass="field" LabelText="<%$Resources:Labels,PercentageValue %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.OnSales %></label>
                    <asp:DropDownList ID="ddlOnSales" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                    <label>
                        <%=Resources.Labels.OnPurchases %></label>
                    <asp:DropDownList ID="ddlOnPurchases" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">

                    <label>
                        <%=Resources.Labels.OnDocCredit %></label>
                    <asp:DropDownList ID="ddlOnDocCredit" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Debit %>" Value="D"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Credit %>" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acSalesAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,TaxSalesAccount %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acPurchaseAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,TaxPurchaseAccount %>"></asp:AutoComplete>
                </div>


            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <br />
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ModalPopupExtender ID="mpeEditParcent" runat="server" TargetControlID="HiddenField1"
        PopupControlID="pnlEditParcent" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlEditParcent" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <div>


                    <asp:ABFTextBox ID="txtPar" CssClass="field" LabelText="<%$Resources:Labels,PercentageValue %>"
                        runat="server" IsRequired="true" ValidationGroup="EditNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>

                </div>
                <asp:Label ID="lblID" runat="server" Style="display: none;" Text="Label"></asp:Label>



            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="EditNew" />
            </div>
            <br />
            <div class="btnDiv">
                <asp:Button ID="btnEditParcent" CssClass="button default_button" runat="server" OnClick="btnEditParcent_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="EditNew" />
                <asp:Button ID="BtnClearEdit" runat="server" CssClass="button" OnClick="BtnClearEdit_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>
