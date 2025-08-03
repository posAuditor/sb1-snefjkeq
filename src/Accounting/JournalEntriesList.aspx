<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="JournalEntriesList.aspx.cs" Inherits="Accounting_journalEntriesList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text=" <%$ Resources:Labels,AddNew %>"
        NavigateUrl="~/Accounting/journalEntry.aspx"></asp:HyperLink>

    
      <asp:LinkButton ID="lnkExportc" style="padding-bottom: 13px; padding-top: 2px; padding-right: 27px; height: 10px; display: block; float: right;        vertical-align: middle;      font-size: 14px;      font-weight: bold;     color: #6b6d6f !important;     margin-left: 5px;     margin-right: 5px;" runat="server" OnClick="lnkExport_Click"  Text="تصدير"></asp:LinkButton>

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
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>
                     <asp:ABFTextBox ID="txtDebitFrom" runat="server"    LabelText="من مدين"  
                                                        MinValue="0" DataType="Decimal"  text="0"  ></asp:ABFTextBox>

                       <asp:ABFTextBox ID="txtDebitto" runat="server"    LabelText="الى مدين"  
                                                        MinValue="0" DataType="Decimal"  text="0"  ></asp:ABFTextBox>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server" >
                    </asp:DropDownList>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"></asp:AutoComplete>
                    
                    

                      <asp:ABFTextBox ID="txtCreditFrom" runat="server"   LabelText="من دائن"  
                                                        MinValue="0" DataType="Decimal" text="0"   ></asp:ABFTextBox>

                      <asp:ABFTextBox ID="txtCreditto" runat="server"   LabelText="الى دائن"  
                                                        MinValue="0" DataType="Decimal" text="0"   ></asp:ABFTextBox>
                     <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>"
                        runat="server"></asp:ABFTextBox>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" ValidationGroup="search" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvJournalEntriesList" GridViewStyle="GrayStyle"
        DataKeyNames="ID,Branch_ID" OnSelectedIndexChanging="gvJournalEntriesList_SelectedIndexChanging"
        OnPageIndexChanging="gvJournalEntriesList_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
            <asp:BoundField DataField="EntryDate" HeaderText="تاريخ الانشاء" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="PostedDate" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="ForeignAmount" HeaderText="<%$Resources:Labels,Amount %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
              <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkGenerateOperation"  OnClick="lnkGenerateOperation_Click"  CommandArgument='<%#Eval("ID") %>'
                        Text="توليد"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("JournalEntry.aspx?ID={0}", Eval("ID") ) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="btnPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>


                 <asp:TemplateField HeaderText="تصدير">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkExportLine" OnClick="lnkExportLine_OnClick" CommandArgument='<%#Eval("ID") %>' runat="server">تصدير</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Attachments %>" >
                <ItemTemplate>

                    <asp:LinkButton ID="lnkAttachments" runat="server" Text="<%#Resources.Labels.Attachments %>" CommandArgument='<%#Eval("ID") %>'
                        OnClick="lnkAttachments_Click">   </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>


        </Columns>
    </asp:ABFGridView>


     <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenField3"
        PopupControlID="Panel3" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel3" CssClass="pnlPopUp" runat="server" Width="700" Height="550">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button13"></asp:Button>
            <span>ملفاتي
            </span>
        </div>
        <div class="content">
            <iframe id="Iframe3" name="ifViewer1"
                scrolling="yes" width="100%" runat="server" height="500" frameborder="0"></iframe>
        </div>
        <div class="btnDiv">
        </div>
    </asp:Panel>



</asp:content>
