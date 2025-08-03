<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="SettingsPayment.aspx.cs" Inherits="Sales_SettingsPayment" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ShowModalPopup() {
            $find("mpeCreateNew").show();
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>

    <br />
    <asp:ABFGridView runat="server" ID="gvSettingPointOS" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"  
        OnRowDeleting="gvSettingPointOS_RowDeleting" 
        OnPageIndexChanging="gvSettingPointOS_PageIndexChanging"
        OnSelectedIndexChanging="gvSettingPointOS_SelectedIndexChanging">
        <Columns>
             
             <asp:BoundField DataField="NameTreasury" HeaderText="كاش" />
             <asp:BoundField DataField="NameATM" HeaderText="شبكة" />
             <asp:BoundField DataField="NameMaster" HeaderText="ماستر" />
             <asp:BoundField DataField="NAmeVisa" HeaderText="فيزا" />
              <asp:BoundField DataField="Name" HeaderText="الفرع" />

            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup"></asp:Button>

        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">




                    <asp:AutoComplete runat="server" ID="acCash" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="كاش"></asp:AutoComplete>


                    <asp:AutoComplete runat="server" ID="acVisa" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="فيزا"></asp:AutoComplete>

                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        ValidationGroup="AddNew"></asp:AutoComplete>

                </div>
                <div class="left_col">



                    <asp:AutoComplete runat="server" ID="acMaster" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="ماستر"></asp:AutoComplete>


                    <asp:AutoComplete runat="server" ID="acAtm" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="AddNew" IsRequired="true" LabelText="شبكة"></asp:AutoComplete>




                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <br />
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>



</asp:Content>

