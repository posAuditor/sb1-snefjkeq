<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GeneralJournalList.aspx.cs" Inherits="Accounting_GeneralJournalList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
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
                    <asp:AutoComplete runat="server" ID="acOperationType" ServiceMethod="GetOperationTypes"
                        LabelText="<%$Resources:Labels,Type %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>

                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Currency %></label>
                    <asp:DropDownList ID="ddlCurrency" runat="server">
                    </asp:DropDownList>

                     <asp:AutoComplete runat="server" ID="acBrancheGroup" ServiceMethod="GetBrancheGroupName"  
                          LabelText="<%$Resources:Labels,GroupBranch %>" AutoPostBack="true" OnSelectedIndexChanged="acBrancheGroup_SelectedIndexChanged"></asp:AutoComplete>


                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"></asp:AutoComplete>

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
    <asp:ABFGridView runat="server" ID="gvGeneralJournalList" GridViewStyle="BlueStyle"
        DataKeyNames="ID,Branch_ID" OnSelectedIndexChanging="gvGeneralJournalList_SelectedIndexChanging"
        OnPageIndexChanging="gvGeneralJournalList_PageIndexChanging">
        <Columns>
           
             <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkbtnimgSelect" CommandName="Select" CssClass="grid-expand"
                        Text="XX"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

             


            <asp:TemplateField HeaderText="<%$Resources:Labels,Hide %>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkHide" runat="server" Checked='<%# Eval("IsHidden") %>' OnCheckedChanged="chkHide_CheckedChanged" AutoPostBack="true" />
                </ItemTemplate>
            </asp:TemplateField>


            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
            <asp:BoundField DataField="PostedDate" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,Amount %>" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
            <asp:BoundField DataField="OperationTypeName" HeaderText="<%$Resources:Labels,Type %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="lnkPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <tr runat="server" id="tdOperationDetails" style="margin-left: 25px;" visible="false">
                        <td colspan="8" style="margin-left: 35px; padding: 0px">
                            <table cellpadding="2" cellspacing="2" border="0" style="width: 450px; padding: 0px;" class="grid">
                                <tr>
                                    <td colspan="2" class="tcat">
                                        <asp:Label ID="lblOperationDetails" runat="server" Text="<%$ Resources:Labels, OperationDetails %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100">
                                        <asp:Label ID="lblRef" runat="server" Text="<%$ Resources:Labels, Serial %>"></asp:Label>
                                    </td>
                                    <td width="350">
                                        <asp:Label runat="server" ID="lblOperationNo" Text='<%# Bind("Serial") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblam" runat="server" Text="<%$ Resources:Labels, Amount %>"></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblAmount" Text='<%# Bind("amount","{0:0.####}") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:Labels, DocumentTpeshowe %> "></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblCURENCY_NAME" Text='<%# Bind("DocumentsTableTypesDescription") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRatio" runat="server" Text="<%$ Resources:Labels, Serial %> "></asp:Label>:
                                    </td>
                                    <td>


                                        <asp:HyperLink runat="server" ID="lnkDocument" Target="_blank" 
                                            NavigateUrl='<%# BetLinkPage(Eval("SourceDoc_ID"),Eval("SourceDocTableType_ID")) %>' 
                                            Text='<%# Eval("SerialDocument") %>' />

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldesc" runat="server" Text="<%$ Resources:Labels, Notes %> "></asp:Label>:
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblopdesc" Text='<%# Bind("Description") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:ABFGridView runat="server" ID="gvOperationDetails" GridViewStyle="BlueStyle"
                                AllowPaging="false">
                                <Columns>
                                    <asp:BoundField DataField="AccountName" HeaderText="<%$ Resources:Labels, AccountName %>" />
                                    <asp:BoundField DataField="DebitAmount" ItemStyle-ForeColor="Green" HeaderText="<%$ Resources:Labels, Debit %>" DataFormatString="{0:0.####}" />
                                    <asp:BoundField DataField="CreditAmount" ItemStyle-ForeColor="Red" HeaderText="<%$ Resources:Labels, Credit %>" DataFormatString="{0:0.####}" />
                                </Columns>
                            </asp:ABFGridView>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
</asp:Content>
