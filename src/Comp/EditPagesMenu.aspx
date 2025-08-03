<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="EditPagesMenu.aspx.cs" Inherits="Comp_EditPagesMenu" %>


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


    <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID,PageName,PageName_en,DisplayOrder"
        OnPageIndexChanging="gvItems_PageIndexChanging" AllowPaging="false" OnSelectedIndexChanging="gvItems_SelectedIndexChanging">
        <Columns>

            <asp:BoundField DataField="PageName" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="PageName_en" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="DisplayOrder" HeaderText="الترتيب" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton Width="34" ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:ABFGridView>




    <asp:HiddenField ID="hfmpeConfirm" runat="server" />
    <asp:ModalPopupExtender ID="mpeConfirm" runat="server" TargetControlID="hfmpeConfirm"
        PopupControlID="pnlConfirm" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="mpeConfirm" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlConfirm" CssClass="pnlPopUp" runat="server"
        Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClientClick="$find('mpeConfirm').hide(); return false;"></asp:Button>
            <span>
                <%=Resources.Labels.Edit%></span>
        </div>
        <div class="content">
            <div style="max-height: 400px; overflow: auto;" class="pnlPopupMessage">
                <asp:ABFTextBox ID="txtName" runat="server" IsRequired="true" ValidationGroup="Edit"
                    Width="200">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtNameEn" runat="server" IsRequired="true" ValidationGroup="Edit"
                    Width="200">
                </asp:ABFTextBox>
                <asp:ABFTextBox ID="txtDisplayOrder" runat="server" IsRequired="true" DataType="Int" MinValue="1" ValidationGroup="Edit"
                    Width="200">
                </asp:ABFTextBox>
                <div style="clear: both">
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Edit" />
                </div>
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnYes" CssClass="button default_button" ValidationGroup="Edit" runat="server" OnClick="btnYes_Click" Text="<%$ Resources:Labels, Save %>" />
                <asp:Button ID="BtnNo" runat="server" CssClass="button" OnClientClick="$find('mpeConfirm').hide(); return false;"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>


</asp:Content>

