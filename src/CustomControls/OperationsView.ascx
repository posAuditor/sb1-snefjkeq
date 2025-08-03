<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OperationsView.ascx.cs" Inherits="CustomControls_OperationsView" %>

<asp:Button ID="btnViewOperation" CssClass="button_big shortcut_approve" runat="server" Text="القيد" OnClick="btnViewOperation_Click" />


<asp:HiddenField ID="hfFastAddNew" runat="server" />
<asp:ModalPopupExtender ID="mpeFastAddNew" runat="server" TargetControlID="hfFastAddNew"
    PopupControlID="pnlFastAddNew" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
    BehaviorID="showPopUp" Y="700">
</asp:ModalPopupExtender>
<asp:Panel ID="pnlFastAddNew" CssClass="pnlPopUp" runat="server"
    Width="690">
    <div class="tcat">
        <asp:Button runat="server" class="close-btn" ID="Button1"></asp:Button>

    </div>
    <div class="content">
        <div class="form">

            <asp:ABFGridView runat="server" ID="gvGeneralJournalList" GridViewStyle="BlueStyle"
                DataKeyNames="ID,Branch_ID" OnSelectedIndexChanging="gvGeneralJournalList_SelectedIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkbtnimgSelect" CommandName="Select" CssClass="grid-expand"
                                Text="XX"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>




                    <%--<asp:TemplateField HeaderText="<%$Resources:Labels,Hide %>">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHide" runat="server" Checked='<%# Eval("IsHidden") %>' OnCheckedChanged="chkHide_CheckedChanged" AutoPostBack="true" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>


                    <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
                    <asp:BoundField DataField="PostedDate" HeaderText="<%$Resources:Labels,Date %>" DataFormatString="{0:d/M/yyyy}" />
                    <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Labels,Amount %>" />
                    <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
                    <asp:BoundField DataField="CurrencyName" HeaderText="<%$Resources:Labels,Currency %>" />
                    <asp:BoundField DataField="OperationTypeName" HeaderText="<%$Resources:Labels,Type %>" />
                 <%--   <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="lnkPrint_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
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

        </div>
        <div class="btnDiv">

            <asp:Button ID="BtnCancelAddNew" runat="server" CssClass="button" OnClick="BtnCancelAddNew_Click"
                Text="<%$ Resources:Labels, Cancel %>" />
        </div>
    </div>
</asp:Panel>





