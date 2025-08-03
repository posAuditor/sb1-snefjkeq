<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Installment.aspx.cs" Inherits="Sales_Installment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
             <asp:Label runat="server" Style="padding-left: 10px; padding-right: 10px; font-weight: bold;"><%=Resources.Labels.Installments %></asp:Label>

            <div runat="server" visible="false" id="divDocCreditInstallments" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.DocCreditInstallment%></span>:
                <asp:Label ID="lblDocCreditInstallment" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acCustomer" ServiceMethod="GetContactNames" ValidationGroup="Save"
                        IsRequired="true" LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>

                    <asp:ABFTextBox ID="txtAmount" runat="server" ValidationGroup="Calc" LabelText="<%$Resources:Labels,Amount %>"
                        DataType="Decimal" IsRequired="true" OnTextChanged="ClearInstallments" AutoPostBack="true"
                        MinValue="1"></asp:ABFTextBox>


                    <asp:ABFTextBox ID="txtPayEvery" runat="server" ValidationGroup="Calc" LabelText="<%$Resources:Labels,PayEvery %>"
                        DataType="Int" IsRequired="true" OnTextChanged="ClearInstallments" AutoPostBack="true" MinValue="1"></asp:ABFTextBox>

                    <label>
                        <%= Resources.Labels.PeriodType %>
                    </label>
                    <asp:DropDownList ID="ddlPayEveryType" runat="server" OnSelectedIndexChanged="ClearInstallments"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Day %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Month %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Year %>" Value="2" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtPeriod" runat="server" ValidationGroup="Calc" LabelText="<%$Resources:Labels,PayingPeriodInMonths %>"
                        DataType="Int" IsRequired="true" OnTextChanged="ClearInstallments" AutoPostBack="true" MinValue="1"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtStartDate" runat="server" ValidationGroup="Calc" LabelText="<%$Resources:Labels,StartDate %>"
                        DataType="Date" IsRequired="true" OnTextChanged="ClearInstallments" AutoPostBack="true" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtEndDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,EndDate %>"
                        DataType="Date" IsRequired="true" Enabled="false" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtInstallmentsNumber" runat="server" ValidationGroup="Save"
                        LabelText="<%$Resources:Labels,InstallmentsNumber %>" DataType="Int" IsRequired="true"
                        Enabled="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Calc" />
            </div>
            <br />
            <div style="text-align: center; padding-top: 30px;">
                <asp:Button ID="btnClacInstallments" CssClass="button" runat="server" OnClick="btnClacInstallments_click"
                    Text="<%$ Resources:Labels, CalcInstallments %>" ValidationGroup="Calc" />
            </div>
        </div>
        <div class="InvoiceSection">
            <asp:ABFGridView runat="server" ID="gvDetails" GridViewStyle="GrayStyle" DataKeyNames="ID"
                OnPageIndexChanging="gvDetails_PageIndexChanging" OnSelectedIndexChanging="gvDetails_SelectedIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Date" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                    <asp:BoundField DataField="Value" HeaderText="<%$Resources:Labels,Amount %>" DataFormatString="{0:0.####}" />
                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Pay %>" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Select" Text="<%$Resources:Labels,Pay %>"
                                Visible='<%# !Convert.ToBoolean(Eval("IsPaid")) %>' Enabled='<%# Convert.ToBoolean(Eval("RowNo").ToString()=="1") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:ABFGridView>
        </div>
        <div class="InvoiceSection">
            <div class="form">
                <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
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

                        <tr>
                            <td>
                                <span class="lbl">
                                    <%=Resources.Labels.Remaining%>: </span>
                            </td>
                            <td>
                                <asp:Label ID="lblRemaining" runat="server" Text="0.0000"></asp:Label>
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
            <asp:Button runat="server" ID="btnPrint" Text="<%$ Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                Visible="false" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
            <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                onclick="window.location = window.location; return false;" />
            <div style="clear: both">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfPay" runat="server" />
    <asp:ModalPopupExtender ID="mpePay" runat="server" TargetControlID="hfPay" PopupControlID="pnlPay"
        BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="mpePayBeH"
        Y="700">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlPay" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClientClick="$find('mpePayBeH').hide(); return false;"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 250px;">
                <asp:ABFTextBox ID="txtPayDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                    runat="server" DataType="Date" ValidationGroup="Pay" IsRequired="true" OnTextChanged="txtPaydate_TextChnaged" AutoPostBack="true"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtRatio" CssClass="field" LabelText="<%$Resources:Labels,Ratio %>"
                    runat="server" Enabled="false" DataType="Decimal" ValidationGroup="Pay" IsRequired="true"></asp:ABFTextBox>
                <asp:ABFTextBox ID="txtInstallmentValue" CssClass="field" LabelText="<%$Resources:Labels,InstallmentValue %>"
                    runat="server" Enabled="false" DataType="Decimal" ValidationGroup="Pay" IsRequired="true"></asp:ABFTextBox>
                <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccounts"
                    ValidationGroup="Pay" IsRequired="true" LabelText="<%$Resources:Labels,OppositeAccount %>"></asp:AutoComplete>
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
</asp:Content>
