<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Branchs.aspx.cs" Inherits="Comp_Branchs" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
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
                <asp:ABFTextBox ID="txtNameSrch" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server"></asp:ABFTextBox>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvBranches" GridViewStyle="GrayStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvBranches_RowDeleting" OnPageIndexChanging="gvBranches_PageIndexChanging"
        OnSelectedIndexChanging="gvBranches_SelectedIndexChanging">
        <Columns>
            <asp:ImageField DataImageUrlField="LogoURL" DataImageUrlFormatString="{0}" ControlStyle-Width="64"
                HeaderStyle-Width="64" ControlStyle-Height="64" HeaderText="<%$ Resources:Labels, Logo %>" />
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Labels,Telephone %>" />
            <asp:BoundField DataField="Mobile" HeaderText="<%$Resources:Labels,Mobile %>" />
            <asp:BoundField DataField="Address" HeaderText="<%$Resources:Labels,Address %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                        OnClientClick="return ConfirmSure();" Visible='<%# !Convert.ToBoolean(Eval("IsSystem")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="lnkAddNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="580">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">
                    <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                        runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtMobile" CssClass="field" LabelText="<%$Resources:Labels,Mobile %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtTelephone" CssClass="field" LabelText="<%$Resources:Labels,Telephone %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtFax" CssClass="field" LabelText="<%$Resources:Labels,Fax %>"
                        runat="server"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtAddress" CssClass="field" LabelText="<%$Resources:Labels,Address %>"
                        runat="server" Height="100" TextMode="MultiLine"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtPrinterName" CssClass="field" Visible="false" LabelText="إسم الطابعة"
                        runat="server" ValidationGroup="AddNew"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtNbrPrint" CssClass="field" Visible="false" VisibleText="false" LabelText="عدد مرات الطباعة"
                        runat="server" DataType="Int" MinValue="1" ValidationGroup="AddNew"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtPassword" CssClass="field" LabelText="الباسورد"
                        runat="server" DataType="FreeString" ValidationGroup="AddNew"></asp:ABFTextBox>

                    <asp:ABFTextBox ID="txtConfirmPassword" Visible="false" CssClass="field" LabelText="تأكيد الباسورد"
                        runat="server" DataType="FreeString" ValidationGroup="AddNew"></asp:ABFTextBox>






                    <%--   <asp:CompareValidator ID="CompareValidator1" runat="server" 
                         ControlToValidate="txtConfirmPassword"
                         CssClass="ValidationError"
                         ControlToCompare="txtPassword"
                         ErrorMessage="No Match" 
                         ValidationGroup="AddNew"
                         ToolTip="Password must be the same" />--%>
                </div>
                <div class="left_col">

                      <asp:AutoComplete runat="server" ID="acBrancheGroup" ServiceMethod="GetBrancheGroupName"  LabelText="<%$Resources:Labels,GroupBranch %>"></asp:AutoComplete>
                   
                    <asp:AutoComplete runat="server" ID="acDefaultStore" ServiceMethod="GetStores" Enabled="false" LabelText="<%$Resources:Labels,DefaultStore %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acDefaultCashAccount_ID" ServiceMethod="GetChartOfAccounts" Enabled="false"
                        LabelText="<%$Resources:Labels,DefaultCashAccount %>"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acDefaultCustomer" ServiceMethod="GetContactNames" Enabled="false"
                        LabelText="<%$Resources:Labels,DefaultCustomer %>"></asp:AutoComplete>


                    <asp:ABFTextBox ID="txtBranchNbrTax" CssClass="field"   LabelText="<%$Resources:Labels,BranchTaxNumber %>" runat="server" DataType="FreeString" ValidationGroup="AddNew"></asp:ABFTextBox>
                    
                    <asp:ABFTextBox ID="txtTradeRegistration" CssClass="field"   LabelText="<%$Resources:Labels,TradeRegister %>" runat="server" DataType="FreeString" ValidationGroup="AddNew"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Logo %></label>
                    <asp:FileUpload ID="fpLogo" runat="server" Width="200" accept="image/jpeg"></asp:FileUpload>
                    <asp:Button ID="btnUploadImage" runat="server" Style="min-width: 0px;" CssClass="button" Text="<%$ Resources:Labels, Upload %>"
                        OnClick="btnUploadImage_Click" />
                    <div class="align_right">
                        <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                            ImageUrl="~/Images/no_photo.png" />
                        <asp:Button ID="btnDeleteImage" runat="server" Style="min-width: 0px;" CssClass="button" Text="<%$ Resources:Labels, Delete %>"
                            OnClick="btnDeleteImage_Click" />
                    </div>

                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
