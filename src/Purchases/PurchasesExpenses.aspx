<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PurchasesExpenses.aspx.cs" Inherits="DocCredit_PurchasesExpenses" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>


<%@ Register Src="~/CustomControls/OperationsView.ascx" TagPrefix="asp" TagName="OperationsView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" OnClick="lnkAddNew_Click"
        Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
  <%--  <asp:HyperLink ID="lnkviewdocCredit" runat="server" Text="عرض فاتورة الشراء"
        CssClass="collapse_search_link" NavigateUrl="#" onclick="return ViewDocCredit(this);"></asp:HyperLink>--%>
    
    
     <asp:LinkButton ID="LinkButton1" runat="server" CssClass="collapse_add_link" OnClick="LinkButton1_Click"
        Text="<%$ Resources:Labels,Receipt %>"></asp:LinkButton>
    
    
   <%-- <asp:LinkButton ID="lnkReturnToInvoice" runat="server" CssClass="collapse_add_link" OnClick="lnkReturnToInvoice_Click"
        Text="فاتورة المشتريات"></asp:LinkButton>--%>
    <div style="clear: both">
    </div>
    <asp:ABFGridView runat="server" ID="gvExpenses" GridViewStyle="GrayStyle" DataKeyNames="ID,ExpenseName"
        OnRowDeleting="gvExpenses_RowDeleting" OnPageIndexChanging="gvExpenses_PageIndexChanging"
        OnSelectedIndexChanging="gvExpenses_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="OperationDate" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="ExpenseName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,Amount %>" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:BoundField DataField="Ratio" HeaderText="<%$Resources:Labels,Ratio %>" />
            <asp:BoundField DataField="OppsiteAccountName" HeaderText="<%$Resources:Labels,OppositeAccount %>" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Labels,Notes %>" />
            
            
            <%--  <asp:TemplateField HeaderText="القيد">
                <ItemTemplate>
                     

                      <asp:OperationsView runat="server" id="OperationsView" SourceDocTypeType_ID="19" Source_ID='<%#Eval("ID") %>' WithOutSecurity="true" />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select"
                        Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="hfmpeCreateNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="580">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList ID="ddlExpenseType" runat="server"  OnSelectedIndexChanged="ddlExpenseType_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Banking %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Other %>" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acExpenseName" ServiceMethod="GetGeneralAtt" AutoCompleteWidth="230"
                        LabelText="نوع المصروف" IsRequired="true" ValidationGroup="AddNew"></asp:AutoComplete>
                    <asp:LinkButton ID="lnkAddNewExpenseName" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                        <asp:AutoComplete runat="server" ID="acAccount" ServiceMethod="GetChartOfAccounts"
                        LabelText="حساب المصروف" IsRequired="true" ValidationGroup="AddNew"></asp:AutoComplete>


                    <asp:ABFTextBox ID="txtAmount" CssClass="field" LabelText="<%$Resources:Labels,Amount %>" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"
                        MinValue="0"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server"  OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" OnTextChanged="ddlCurrency_SelectedIndexChanged"
                        DataType="Date" AutoPostBack="true"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtRatio" CssClass="field" LabelText="<%$Resources:Labels,Ratio %>" Enabled="false"
                        runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccounts"
                        LabelText="<%$Resources:Labels,OppositeAccount %>" IsRequired="true" ValidationGroup="AddNew"></asp:AutoComplete>
                    
                       <table>
                            <tr>
                                <td><span> </span>
                                    <br />
                                  <asp:CheckBox ID="chkIsTaxFound" runat="server"   AutoPostBack="true" OnCheckedChanged="chkIsTaxFound_CheckedChanged" />   
                    

                                </td>
                                <td>

                                    <asp:ABFTextBox ID="txtAmountTax" CssClass="field" LabelText="<%$Resources:Labels,AmountTax %>"  Enabled="false"
                                     runat="server"  ValidationGroup="AddNew" DataType="Decimal"
                                      MinValue="0"></asp:ABFTextBox>

                                </td>
                            </tr>
                        </table> 
                    
                    <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                        runat="server" TextMode="MultiLine" Text=""></asp:ABFTextBox>
                </div>
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
                <asp:Button ID="btnApprove" CssClass="button" runat="server" OnClick="btnApprove_click"
                    Text="<%$ Resources:Labels, Approve %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <asp:NewAttribute ID="ucNewExpenseName" runat="server" Title="<%$Resources:Labels,Name %>"
        AttributeType_ID="12" TargetControlID="lnkAddNewExpenseName" OnNewAttributeCreated="ucNewExpenseName_NewAttributeCreated"></asp:NewAttribute>
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
</asp:content>
