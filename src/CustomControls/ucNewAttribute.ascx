<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNewAttribute.ascx.cs"
    Inherits="CustomControls_ucNewAttribute" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:ModalPopupExtender ID="mpeCreateAtt" runat="server" TargetControlID="" PopupControlID="pnlAddNewAtt"
    BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="showPopUp"
    Y="200">
</asp:ModalPopupExtender>
<asp:Panel ID="pnlAddNewAtt" CssClass="pnlPopUp" runat="server" 
    Width="280">
    <div class="tcat">
        <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClick="ClosePopup_Click">
        </asp:Button>
        <span>
            <%=this.Title%></span>
    </div>
    <div class="content">
        <div class="form">
            <label>
                <%=Resources.Labels.Name %></label>
            <asp:ABFTextBox ID="txtAttName" CssClass="field" runat="server" Width="250px" IsRequired="true"
                ValidationGroup="NewAtt" ErrorMessage="<%$Resources:Validations,rfvName %>">
            </asp:ABFTextBox>
        </div>
        <div class="validationSummary">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewAtt" />
        </div>
        <div class="btnDiv">
            <asp:Button ID="btnSaveAtt" CssClass="button default_button" runat="server" OnClick="btnSaveAtt_click"
                Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewAtt" />
            <asp:Button ID="BtnClearAtt" runat="server" CssClass="button" OnClick="BtnClearAtt_click"
                Text="<%$ Resources:Labels, Clear %>" />
        </div>
    </div>
</asp:Panel>
