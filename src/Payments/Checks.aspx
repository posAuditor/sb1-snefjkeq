<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Checks.aspx.cs" Inherits="Payments_Checks" %>

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
            <div runat="server" visible="false" id="divSalesOrderNo" style="display: inline-block;">
                <span>
                    <%=Resources.Labels.PurchaseOrderNo %></span>:
                <asp:Label ID="lblSalesOrderNo" runat="server" Font-Bold="true" Text=""></asp:Label>
            </div>
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server"  OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                    <div id="divBeginingBalanceCheck" runat="server">
                        <label>
                            <%=Resources.Labels.BeginingBalanceCheck %></label>
                        <asp:DropDownList ID="ddlBeginingBalanceCheck" runat="server"  OnSelectedIndexChanged="ddlBeginingBalanceCheck_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Labels,No %>" Value="False"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Labels,Yes %>" Value="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="Save" IsRequired="true" LabelText="<%$Resources:Labels,StartFrom %>"
                            DataType="Date" Visible="false"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                            LabelText="<%$Resources:Labels,OppositeAccount %>" Enabled="false" Visible="false"></asp:AutoComplete>
                    </div>
                    <asp:ABFTextBox ID="txtCheckNo" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,CheckNo %>"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtIssueDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,IssueDate %>"
                        OnTextChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true" DataType="Date"
                        IsRequired="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtMaturityDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,MaturityDate %>"
                        DataType="Date" IsRequired="true" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Ratio %>"
                        DataType="Decimal" Enabled="false" IsRequired="true"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.CheckStatus %></label>
                    <asp:DropDownList ID="ddlCheckStatus" runat="server"  OnSelectedIndexChanged="ddlCheckStatus_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$Resources:Labels,UnderCollection %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,PaidCollected %>" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acIssueBank" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" IsRequired="true" LabelText="<%$Resources:Labels,IssueBank %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acIntermediateAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" IsRequired="true" LabelText="<%$Resources:Labels,IntermediateAccount %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                        LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true" ></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>" IsHideable="true" ></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtCollectingDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,CollectingDate %>"
                        DataType="Date" IsRequired="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acDepositAccount" ServiceMethod="GetChartOfAccountsCashAndBanks"
                        ValidationGroup="Save" IsRequired="true" LabelText="<%$Resources:Labels,DepositAccount %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtCollectingExpenses" runat="server" ValidationGroup="Save"
                        MinValue="0" LabelText="<%$Resources:Labels,Expenses %>" DataType="Decimal"
                        OnTextChanged="txtCollectingExpenses_TextChanged" AutoPostBack="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acExpensesAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,ExpensesAccount %>"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div class="InvoiceSection">
            <asp:Panel ID="pnlAddDetail" runat="server" DefaultButton="btnAddDetail">
                <div class="form" style="width: 90%; margin: auto;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acAccount" ServiceMethod="GetChartOfAccountsCheledronly"
                            ValidationGroup="AddDetail" IsRequired="true" LabelText="<%$Resources:Labels,AccountName %>"
                            OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                        <br />
                        <span>
                            <%=Resources.Labels.Balance %>: </span>
                        <asp:Label ID="lblAccountBalance" runat="server" Text=""></asp:Label>
                        <asp:ABFTextBox ID="txtAmount" runat="server" LabelText="<%$Resources:Labels,Amount %>"
                            MinValue="0.0001" DataType="Decimal" IsRequired="true" ValidationGroup="AddDetail"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acSalesRep" ServiceMethod="GetSalesReps" LabelText="<%$Resources:Labels,SalesRep %>" IsHideable="true" ></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acDetailCostCenter" ServiceMethod="GetCostCenters"
                            LabelText="<%$Resources:Labels,CostCenter %>" IsHideable="true" ></asp:AutoComplete>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div class="form" style="width: 600px; margin: auto;">
                    <asp:ABFTextBox ID="txtDetailNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
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
                    <asp:BoundField DataField="AccountName" HeaderText="<%$Resources:Labels,AccountName %>" />
                    <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,Amount %>" DataFormatString="{0:0.####}" />
                    <asp:BoundField DataField="SalesRepName" HeaderText="<%$Resources:Labels,SalesRep %>" ItemStyle-CssClass="SalesRepCol" />
                    <asp:BoundField DataField="CostCenterName" HeaderText="<%$Resources:Labels,CostCenter%>" />
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
             
            <asp:Button runat="server" ID="btnCancelcollection" Text="<%$ Resources:Labels,NotApprove %>"  CssClass="button_big shortcut_approve"
                Visible="false" OnClick="btnCancelcollection_Click" />


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
            BehaviorID="mpeConfirm" Y="1000">
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
</asp:Content>
