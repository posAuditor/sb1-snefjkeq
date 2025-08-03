<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DatabaseBackup.aspx.cs" Inherits="Comp_DatabaseBackup" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkBackUp" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,Backup %>"
        OnClick="lnkBackUp_Click"></asp:LinkButton>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvBackups" GridViewStyle="GrayStyle" DataKeyNames="FullName"
        OnRowDeleting="gvBackups_RowDeleting" OnSelectedIndexChanging="gvBackups_SelectedIndexChanging"
        OnPageIndexChanging="gvBackups_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Labels, Name %>" />
            <asp:BoundField DataField="Date" HeaderText="<%$ Resources:Labels, Date %>" DataFormatString="{0:d/M/yyyy hh:mm tt}" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server"
                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="<%$Resources:Labels,Restore %>" runat="server"
                        CommandName="Select" OnClientClick="return ConfirmSure();"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="<%$Resources:Labels,ChangeNameFile %>" runat="server" CommandArgument='<%#Eval("FullName") %>' OnClick="Unnamed_Click"
                        OnClientClick="return ConfirmSure();"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:HyperLinkField Text="<%$ Resources:Labels, Download %>" DataNavigateUrlFields="Name" DataNavigateUrlFormatString="~/DatabaseBackups/{0}" Target="_blank" />
        </Columns>
    </asp:ABFGridView>

    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="hfmpeCreateNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="580">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="close_popup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form" style="width: 330px;">

                <asp:ABFTextBox ID="txtDateFrom" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    DataType="FreeString" runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>



            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>



    <asp:HiddenField ID="hdnValidation" runat="server" />
    <asp:ModalPopupExtender ID="mpeValidation" runat="server" TargetControlID="hdnValidation"
        PopupControlID="pnlValidation" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="305">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlValidation" CssClass="pnlPopUp" runat="server" Width="300">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="close_popup_Click"></asp:Button>
            <span>الدعم الفني هو من ينفذ هذه العملية</span>
        </div>
        <div class="content">
            <asp:ABFTextBox ID="txtPassword1" CssClass="field" LabelText="<%$Resources:Labels,Password %>" TextMode="Password" style="margin-left:50px;margin-right:50px;width: 210px;"
                DataType="FreeString" runat="server" IsRequired="true" ValidationGroup="Validation"></asp:ABFTextBox>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Validation" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnValidation" CssClass="button default_button" runat="server" OnClick="btnValidation_Click"
                    Text="<%$ Resources:Labels, Login %>" ValidationGroup="Validation" />
                <asp:Button ID="btnClear" runat="server" CssClass="button" OnClick="btnClear_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>





</asp:Content>
