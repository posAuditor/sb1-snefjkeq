<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNewItemDescribed.ascx.cs" Inherits="CustomControls_ucNewItemDescribed" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:ModalPopupExtender ID="mpeCreateItemDescribed" runat="server" TargetControlID="" PopupControlID="pnlAddNewDescribed"
    BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize" BehaviorID="showPopUp"
    Y="700">
</asp:ModalPopupExtender>
<asp:Panel ID="pnlAddNewDescribed" CssClass="pnlPopUp" runat="server"  
    Width="280">
    <div class="tcat">
        <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClientClick="$find('mpeCreateItemDescribed').hide(); return false;" OnClick="btnClosepopup_OnClick">
        </asp:Button>
        <span>
            <%=this.Title%></span>
    </div>
    <div class="content">
        <div class="form">
            <asp:ABFTextBox ID="txtDescribed"   CssClass="field" runat="server" Width="250px" IsRequired="true"
                LabelText="<%$Resources:Labels,Itemdescribed %>">
            </asp:ABFTextBox>
           
             <asp:ABFTextBox ID="txtPrice" CssClass="field"   runat="server" Width="250px" 
                LabelText="<%$Resources:Labels,Price %>">
            </asp:ABFTextBox>
             
            <div style="display:none;">
                <asp:ABFTextBox ID="txtItemID" IsRequired="true"   CssClass="field" runat="server" Width="250px" LabelText="<%$Resources:Labels,Item %>"
                    DataType="Int">
                </asp:ABFTextBox>
            </div>
        </div>
        <div class="validationSummary">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server"  />
        </div>
        <div class="btnDiv">
            <asp:Button ID="btnSaveDescribed" CssClass="button default_button"   runat="server" OnClick="btnSaveAtt_OnClick"
                Text="<%$ Resources:Labels, Save %>" />
            <asp:Button ID="BtnClearDescribed" runat="server" CssClass="button" OnClick="BtnClearAtt_OnClick"
                Text="<%$ Resources:Labels, Clear %>" />
        </div>
    </div>
</asp:Panel>
