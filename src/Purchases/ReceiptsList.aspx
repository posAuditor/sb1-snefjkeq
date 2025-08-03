<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReceiptsList.aspx.cs" Inherits="Purchases_ReceiptsList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text=" <%$ Resources:Labels,AddNew %>"></asp:HyperLink>
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
                <div class="right_col">
                    <asp:ABFTextBox ID="txtDateFromSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>

                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="FilterVendors"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterVendors" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acVendorName" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,Vendor %>"></asp:AutoComplete>

                    <div runat="server" id="divHasBill">
                        <label>
                            <%=Resources.Labels.Status %></label>
                        <asp:DropDownList ID="ddlIsHasBill" runat="server">
                            <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="All"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,HasBill %>" Value="true"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,HasNoBill %>" Value="false"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>

                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"  OnClientClick="Display('cph_btnSearch')" 
                    OnClick="btnSearch_click" ValidationGroup="search" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvReceiptsList" GridViewStyle="GrayStyle" DataKeyNames="ID,GrossTotalAmount,PaidAmount,Serial,Branch_ID"
        OnSelectedIndexChanging="gvReceiptsList_SelectedIndexChanging" OnPageIndexChanging="gvReceiptsList_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,NbInvoice %>" />
            <asp:BoundField DataField="OperationDate" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="GrossTotalAmount" HeaderText="<%$Resources:Labels,GrossTotal %>" />
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Vendor %>" />
            <asp:BoundField DataField="user_created" HeaderText="المستخدم "/>

            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />

            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />

            <asp:TemplateField HeaderText="<%$ Resources:Labels, Expenses %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLinkxxxx1" runat="server" NavigateUrl='<%# String.Format("PurchasesExpenses.aspx?Receipt_ID={0}", Eval("ID") ) %>'
                        Text="<%$ Resources:Labels, Expenses %>" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("{1}?ID={0}", Eval("ID"),Eval("PageName") ) %>'
                        Text="<img src='../../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="btnPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Receipt %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("{0}", Eval("ToReceiptPage")) %>'
                        Text="<%$ Resources:Labels, Receipt %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, ReturnRecipt %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("ReturnReceipt.aspx?FromReceiptID={0}", Eval("ID")) %>'
                        Text="<%$ Resources:Labels, ReturnRecipt %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Pay %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPay" Text="<%$ Resources:Labels, Pay %>" runat="server" OnClick="lnkPay_Click"
                        Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") && (Convert.ToDecimal(Eval("GrossTotalAmount")) > Convert.ToDecimal(Eval("PaidAmount"))) %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

               <asp:TemplateField HeaderText="توليد فاتورة">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkGenerate" Text="توليد فاتورة" runat="server" CommandArgument='<%# Eval("ID").ToString() %>'
                        OnClick="lnkGenerate_Click"></asp:LinkButton>

                </ItemTemplate>
            </asp:TemplateField>




            <asp:BoundField DataField="FirstPaid" HeaderText="<%$Resources:Labels,FirstPaid %>" />

        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfPay" runat="server" />
    <asp:ModalPopupExtender ID="mpePay" runat="server" TargetControlID="hfPay" PopupControlID="pnlPay"
        BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="showPopUp"
        Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlPay" CssClass="pnlPopUp" runat="server"
        Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <br />
                <span>
                    <%=Resources.Labels.Serial %></span>:
                <asp:Label ID="lblReceiptSerial" Text="" runat="server"></asp:Label>
                <br />
                <span>
                    <%=Resources.Labels.Paid %></span>:
                <asp:Label ID="lblCollected" Text="" runat="server"></asp:Label>
                <span>
                    <%=Resources.Labels.From %></span>
                <asp:Label ID="lblGrossTotal" Text="" runat="server"></asp:Label>
                <label>
                    <%=Resources.Labels.PayWith %></label>
                <asp:DropDownList ID="ddlPayWith" runat="server">
                    <asp:ListItem Text="<%$ Resources:Labels,Cash %>" Value="~/Payments/Payments.aspx/CashOutVendor"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Bank %>" Value="~/Payments/Payments.aspx/BankWithdrawVendor"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Check %>" Value="~/Payments/Checks.aspx/CheckOut"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnOkPay" CssClass="button default_button" runat="server" OnClick="btnOkPay_click"
                    Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelPay" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
