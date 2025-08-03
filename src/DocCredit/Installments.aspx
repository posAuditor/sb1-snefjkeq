<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Installments.aspx.cs" Inherits="DocCredit_Installments" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkviewdocCredit" runat="server" Text="<%$ Resources:Labels,ViewDocCredit %>"
        CssClass="collapse_search_link" NavigateUrl="#" onclick="return ViewDocCredit(this);"></asp:HyperLink>
    <div style="clear: both">
    </div>
    <asp:Label runat="server" ID="lblMSG" Text="<%$Resources:UserInfoMessages,PayFromCover %>" ForeColor="Red"></asp:Label>
    <asp:ABFGridView runat="server" ID="gvInstallments" GridViewStyle="GrayStyle" DataKeyNames="ID,Amount,InstDate"
        OnRowDeleting="gvInstallments_RowDeleting" OnPageIndexChanging="gvInstallments_PageIndexChanging"
        OnSelectedIndexChanging="gvInstallments_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="InstDate" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,InstallmentValue %>" />
            <asp:BoundField DataField="PaidStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="AmountInDefaultCurr" HeaderText="<%$Resources:Labels,PaidWithDefaultCurrency %>" />
            <asp:BoundField DataField="Ratio" HeaderText="<%$Resources:Labels,Ratio %>" />
            <asp:BoundField DataField="PaymentMethod" HeaderText="<%$Resources:Labels,PaymentMethod %>" />
            <asp:BoundField DataField="PaymentDocSerial" HeaderText="<%$Resources:Labels,PaymentDocSerial %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, PayWithBankW %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPayWithBank" runat="server" Text="<%$ Resources:Labels, PayWithBankW %>" OnClick="lnkPayWithBank_Click"
                        Visible='<%# !Convert.ToBoolean(Eval("IsPaid"))%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, PayWithLoan %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPayWithLoan" runat="server" Text="<%$ Resources:Labels, PayWithLoan %>" OnClick="lnkPayWithBank_Click"
                        Visible='<%# !Convert.ToBoolean(Eval("IsPaid"))%>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select"
                        Visible='<%# !Convert.ToBoolean(Eval("IsLocked"))%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# !Convert.ToBoolean(Eval("IsLocked"))%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <table class="totals">
        <tr>
            <td>
                <span class="lbl">
                    <%=Resources.Labels.InstallmentsTotal%>: </span>
            </td>
            <td>
                <asp:Label ID="lblInstallmentsTotal" runat="server" Text="0.0000"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lbl">
                    <%=Resources.Labels.DocCreditGrossTotal%>: </span>
            </td>
            <td>
                <asp:Label ID="lblDocCreditGrossTotal" runat="server" Text="0.0000"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="lbl">
                    <%=Resources.Labels.Difference%>: </span>
            </td>
            <td>
                <asp:Label ID="lblDifference" runat="server" Text="0.0000"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 250px;">
                <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                    IsDateFiscalYearRestricted="false" runat="server" IsRequired="true" ValidationGroup="AddNew"
                    DataType="Date"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtAmount" CssClass="field" LabelText="<%$Resources:Labels,InstallmentValue %>"
                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                <div style="clear: both">
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSave_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hfPay" runat="server" />
    <asp:ModalPopupExtender ID="mpePay" runat="server" TargetControlID="hfPay" PopupControlID="pnlPay"
        BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="mpePayBeH"
        Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlPay" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClientClick="$find('mpePayBeH').hide(); return false;"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 250px;">
                <asp:ABFTextBox ID="txtPayDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>" OnTextChanged="txtPayDate_TextChanged" AutoPostBack="true"
                    runat="server" DataType="Date" ValidationGroup="Pay" IsRequired="true"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtInstallmentValue" CssClass="field" LabelText="<%$Resources:Labels,InstallmentValue %>"
                    runat="server" Enabled="false" DataType="Decimal" ValidationGroup="Pay"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtPayAmount" CssClass="field" LabelText="<%$Resources:Labels,Amount %>"
                    runat="server" IsRequired="true" ValidationGroup="Pay" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtPayFromCover" CssClass="field" LabelText="<%$Resources:Labels,AmountFromCover %>"
                    runat="server" IsRequired="true" ValidationGroup="Pay" DataType="Decimal" MinValue="0"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtRatio" CssClass="field" LabelText="<%$Resources:Labels,Ratio %>" Enabled="false"
                    runat="server" IsRequired="true" ValidationGroup="Pay" DataType="Decimal" MinValue="0.0001"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                    runat="server" ValidationGroup="AddNew" TextMode="MultiLine" Height="80"></asp:ABFTextBox>
                <div style="clear: both">
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Pay" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnPay" CssClass="button default_button" runat="server" OnClick="btnPay_click" Text="<%$ Resources:Labels, OK %>"
                    ValidationGroup="Pay" />
                <asp:Button ID="btnCancelPay" runat="server" CssClass="button" OnClientClick="$find('mpePayBeH').hide(); return false;"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        function ViewDocCredit(obj) {

            var fileUploadsWin = window.open(obj.getAttribute("href"), 'ViewDocCredit', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=no,width=800,height=600,top=100,left=10');
            fileUploadsWin.status = 0;
            fileUploadsWin.menubar = 0;
            fileUploadsWin.toolbar = 0;
            fileUploadsWin.focus();
            return false;
        }
    </script>
</asp:Content>
