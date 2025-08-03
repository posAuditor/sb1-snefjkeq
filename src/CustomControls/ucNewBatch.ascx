<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNewBatch.ascx.cs" Inherits="CustomControls_ucNewBatch" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:ModalPopupExtender ID="mpeCreateBatch" runat="server" TargetControlID="" PopupControlID="pnlAddNewAtt"
    BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="showPopUp"
    Y="700">
</asp:ModalPopupExtender>
<asp:Panel ID="pnlAddNewAtt" CssClass="pnlPopUp" runat="server"  
    Width="280">
    <div class="tcat">
        <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClientClick="$find('mpeCreateBatch').hide(); return false;" OnClick="ClosePopup_Click">
        </asp:Button>
        <span>
            <%=this.Title%></span>
    </div>
    <div class="content">
        <div class="form">
            <asp:ABFTextBox ID="txtBatchID" CssClass="field" runat="server" Width="250px" IsRequired="true"
                LabelText="<%$Resources:Labels,BatchID %>">
            </asp:ABFTextBox>
            <asp:ABFTextBox ID="txtProductionDate" CssClass="field" runat="server" Width="250px"
                LabelText="<%$Resources:Labels,ProductionDate %>" DataType="Date" IsDateFiscalYearRestricted="false">
            </asp:ABFTextBox>
            <asp:ABFTextBox ID="txtExirationDate" CssClass="field" runat="server" Width="250px"
                LabelText="<%$Resources:Labels,ExpirationDate %>" DataType="Date" IsDateFiscalYearRestricted="false">
            </asp:ABFTextBox>
            <div style="display:none;">
                <asp:ABFTextBox ID="txtItemID" IsRequired="true" CssClass="field" runat="server" Width="250px" LabelText="<%$Resources:Labels,Item %>"
                    DataType="Int">
                </asp:ABFTextBox>
            </div>
        </div>
        <div class="validationSummary">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
        <div class="btnDiv">
            <asp:Button ID="btnSaveAtt" CssClass="button default_button" runat="server" OnClick="btnSaveAtt_click"
                Text="<%$ Resources:Labels, Save %>" />
            <asp:Button ID="BtnClearAtt" runat="server" CssClass="button" OnClick="BtnClearAtt_click"
                Text="<%$ Resources:Labels, Clear %>" />
        </div>
    </div>
</asp:Panel>
