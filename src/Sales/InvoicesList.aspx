  <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InvoicesList.aspx.cs" Inherits="Sales_InvoicesList" %>

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
                    <asp:DropDownList ID="ddlCurrency" runat="server" OnSelectedIndexChanged="FilterCustomers"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="FilterCustomers" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acCustomerName" ServiceMethod="GetContactNames"
                        LabelText="<%$Resources:Labels,Customer %>"></asp:AutoComplete>
                    <div runat="server" id="divHasBill">
                        <label>
                            <%=Resources.Labels.Status %></label>
                        <asp:DropDownList ID="ddlIsHasBill" runat="server">
                            <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="All"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,HasBill %>" Value="true"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:Labels,HasNoBill %>" Value="false"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:AutoComplete runat="server" ID="acnameEmp" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"
                        KeepTextWhenNoValue="true"></asp:AutoComplete>
                    <label runat="server" id="Label2">
                        <%=Resources.Labels.PaymentMethod %></label>
                    <asp:DropDownList ID="ddlPaymentMethode" runat="server">
                    </asp:DropDownList>
                       <asp:ABFTextBox ID="txtCustomerName" CssClass="field" runat="server" LabelText="<%$Resources:Labels,CustomerName %>">
                    </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtGrossTotal" runat="server"   LabelText="<%$Resources:Labels,GrossTotal %>" 
                                                        MinValue="0" DataType="Decimal" text="0"   ></asp:ABFTextBox>
                   

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



                    <label>التحصيلات</label>
                    <asp:DropDownList ID="ddlCollect" runat="server">
                        <asp:ListItem Text="إختر" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="محصل" Value="0"></asp:ListItem>
                        <asp:ListItem Text="غير محصل" Value="1"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>
                    
                         <asp:ABFTextBox ID="txtCustomerMobile" CssClass="field" runat="server" LabelText="<%$Resources:Labels,CustomerMobile %>">
                    </asp:ABFTextBox>
                      <asp:ABFTextBox ID="txtNotess" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                        runat="server"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" ValidationGroup="search" OnClientClick="Display('cph_btnSearch')"   />
                <asp:Button ID="btnPrintList" CssClass="button" runat="server" Text="<%$ Resources:Labels, Print %>"
                    OnClick="btnPrintList_Click" ValidationGroup="search" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvInvoicesList" GridViewStyle="GrayStyle" DataKeyNames="ID,GrossTotalAmount,CollectedAmount,Serial,Branch_ID"
        OnSelectedIndexChanging="gvInvoicesList_SelectedIndexChanging" OnPageIndexChanging="gvInvoicesList_PageIndexChanging">
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
            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Customer %>" />
            <asp:BoundField DataField="user_created" HeaderText="المستخدم "/>
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />

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
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Invoice %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("{0}", Eval("ToInvoicePage")) %>'
                        Text="<%$ Resources:Labels, Invoice %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, ReturnInvoice %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("ReturnInvoice.aspx?FromInvoiceID={0}", Eval("ID")) %>'
                        Text="<%$ Resources:Labels, ReturnInvoice %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Collect %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCollect" Text='<%# GetTypeCado(Eval("DocStatus_ID").ToString(),  Convert.ToDecimal(Eval("GrossTotalAmount")),Convert.ToDecimal(Eval("CollectedAmount"))) %>' runat="server"
                        OnClick="lnkCollect_Click" Visible='<%# GetTypeCollect(Eval("DocStatus_ID").ToString(),  Convert.ToDecimal(Eval("GrossTotalAmount")),Convert.ToDecimal(Eval("CollectedAmount"))) && Convert.ToInt16( Eval("typePayment_ID").ToString())<0  %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>



            <asp:TemplateField HeaderText="حالة التسليم ">
                <ItemTemplate>
                    <asp:HyperLink ID="hlinkOutput" Width="20" Height="20" runat="server" AlternateText="تسليم" NavigateUrl='<%# String.Format("InvoiceOutput.aspx?ID={0}", Eval("ID")) %>'
                        Text="تسليم" Visible='<%#  Convert.ToBoolean(Eval("NotStocked").ToString()!="0") &&  Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")  &&   Eval("ApprovedComplete").ToString()=="0" %>' />

                    <asp:Label ID="lblOutput" runat="server" Text="تم التسليم" Visible='<%#  Convert.ToBoolean(Eval("NotStocked").ToString()=="0") &&  Eval("ApprovedComplete").ToString()=="0" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>



            <asp:TemplateField HeaderText="توليد فاتورة">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkGenerate" Text="توليد فاتورة" runat="server" CommandArgument='<%# Eval("ID").ToString() %>'
                        OnClick="lnkGenerate_Click"></asp:LinkButton>

                </ItemTemplate>
            </asp:TemplateField>
             <asp:BoundField DataField="namepayment" HeaderText="طريقة الدفع "/>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfCollect" runat="server" />
    <asp:ModalPopupExtender ID="mpeCollect" runat="server" TargetControlID="hfCollect"
        PopupControlID="pnlCollectRefuse" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="300">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlCollectRefuse" CssClass="pnlPopUp" runat="server"
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
                <asp:Label ID="lblInvoiceSerial" Text="" runat="server"></asp:Label>
                <br />
                <span>
                    <%=Resources.Labels.Collected %></span>:
                <asp:Label ID="lblCollected" Text="" runat="server"></asp:Label>
                <span>
                    <%=Resources.Labels.From %></span>
                <asp:Label ID="lblGrossTotal" Text="" runat="server"></asp:Label>
                <label>
                    <%=Resources.Labels.CollectWith %></label>
                <asp:DropDownList ID="ddlCollectWith" runat="server">
                    <asp:ListItem Text="<%$ Resources:Labels,Cash %>" Value="~/Payments/Payments.aspx/CashInCustomer"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Bank %>" Value="~/Payments/Payments.aspx/BankDepositCustomer"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels,Check %>" Value="~/Payments/Checks.aspx/CheckIn"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnOkCollect" CssClass="button default_button" runat="server" OnClick="btnOkCollect_click"
                    ValidationGroup="Collect" Text="<%$ Resources:Labels, OK %>" />
                <asp:Button ID="BtnCancelCollect" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>




    <asp:ModalPopupExtender ID="mpePayment" runat="server" TargetControlID="hfCollect"
        PopupControlID="pnlPayment" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlPayment" CssClass="pnlPopUp" runat="server"
        Width="100%" Height="55%">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1"></asp:Button>
            <span>تحصيل
            </span>
        </div>
        <div class="content">
            <div class="form">
                <asp:AutoComplete runat="server" ID="acBranchPayment" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                    ValidationGroup="Collect"></asp:AutoComplete>

                <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Collect" LabelText="<%$Resources:Labels,Date %>"
                    AutoPostBack="true" OnTextChanged="txtOperationDate_TextChanged" DataType="Date"
                    IsRequired="true"></asp:ABFTextBox>

                <asp:AutoComplete runat="server" ID="acCreditAccount" ServiceMethod="GetChartOfAccountsOnly"
                    ValidationGroup="Collect" IsRequired="true" LabelText="<%$Resources:Labels,CreditAccount %>"
                    AutoPostBack="true"></asp:AutoComplete>


                <div class="form" style="width: 100%; margin: auto;">
                    <div class="right_col">

                        <span>مبلغ الفاتورة: </span>
                        <asp:Label ID="lblCreditBalance" runat="server" Text="" Style="border: 2px dashed red; width: 100px; font-weight: bold; padding-left: 20px; padding-right: 20px; text-align: center"></asp:Label>


                        <br />
                        <span>الشبكة:  ATM</span>
                        <asp:ABFTextBox ID="txtAtm" runat="server"
                            MinValue="0.0001" DataType="Decimal" ValidationGroup="AddDetail"></asp:ABFTextBox>
                        <br />
                        <span>فيزا: VISA </span>
                        <asp:ABFTextBox ID="txtVisa" runat="server"
                            MinValue="0.0001" DataType="Decimal" ValidationGroup="AddDetail"></asp:ABFTextBox>
                        <br />
                        <span>ماستركارد: MASTER CARD</span>
                        <asp:ABFTextBox ID="txtMaster" runat="server"
                            MinValue="0.0001" DataType="Decimal" ValidationGroup="AddDetail"></asp:ABFTextBox>

                        <br />
                        <span>كاش: CASH</span>
                        <asp:ABFTextBox ID="txtBalanceNow" runat="server"
                            MinValue="0.0001" DataType="Decimal" ValidationGroup="AddDetail"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">


                        <span style="display: none;">الصندوق: </span>
                        <asp:Label ID="lblDebitBalance" runat="server" Text="" Style="display: none;"></asp:Label>
                        <br />
                        <br />
                        <table style="text-align: left">

                            <tr>
                                <td>
                                    <asp:Button ID="btnApprove" CssClass="button_big shortcut_print" Style="width: 100%; background-color: burlywood" OnClick="btnApprove_OnClick" runat="server"
                                        ValidationGroup="Collect" Text="موافق - OK" />
                                </td>
                            </tr>


                            <tr>
                                <td>

                                    <asp:Button ID="Button3" runat="server" CssClass="button" Style="width: 100%"
                                        Text="<%$ Resources:Labels, Cancel %>" />
                                    <asp:Button ID="Button4" runat="server" CssClass="button" Visible="False"
                                        Text="<%$ Resources:Labels, Clear %>" />

                                </td>
                            </tr>
                        </table>





                    </div>
                </div>

            </div>


        </div>
        <div class="btnDiv">
        </div>
    </asp:Panel>


</asp:Content>
