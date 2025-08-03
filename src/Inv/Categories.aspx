<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="Inv_Categories" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>
    <script type="text/javascript">           
        $(document).ready(function () {
            $('#chkIsUseTax').change(function () {
                //console.log(this.checked);                
                setEnabled("#txtTaxPercentage", this.checked);
                setEnabled("#chkIsTaxIncludedInPurchase", this.checked);
                setEnabled("#chkIsTaxIncludedInSale", this.checked);
            });
        });
    </script>
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
    <asp:ABFGridView runat="server" ID="gvCategories" GridViewStyle="BlueStyle" DataKeyNames="ID,MonoName"
        OnRowDeleting="gvCategories_RowDeleting" OnPageIndexChanging="gvCategories_PageIndexChanging"
        OnSelectedIndexChanging="gvCategories_SelectedIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="التسلسل">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="ParentName" HeaderText="<%$Resources:Labels,ParentCategory %>" />
             <%-- <asp:BoundField DataField="Sts" HeaderText="المخازن" />--%>
            <%--            <asp:BoundField DataField="PrinterName" HeaderText="<%$Resources:Labels,PrinterName %>" />--%>

            <asp:TemplateField HeaderText="ربط بالمخازن">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkRelatedBranch" runat="server" CssClass="collapse_add_link" OnClick="lnkRelatedBranch_Click" Text='<%#Eval("Sts") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

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
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                    runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>
                <asp:AutoComplete ID="acParentCategory" ServiceMethod="GetItemsCategories" LabelText="<%$Resources:Labels,ParentCategory %>"
                    runat="server"></asp:AutoComplete>


                <asp:AutoComplete ID="acPrinterName" ServiceMethod="GetPrintersName" LabelText="<%$Resources:Labels,PrinterName %>"
                    runat="server" ValidationGroup="NewItem"></asp:AutoComplete>




                <br />                
                <asp:CheckBox ID="chkPos" runat="server" /><span>إظهار في نقطة البيع</span>
                <br />                
                <asp:CheckBox ID="chkIsNotViewInInvoice" runat="server" /><span>   عدم الظهور في  فاتورة البيع    </span>
                <br />                
                <asp:CheckBox ID="chkIsNotViewInReceipt" runat="server" /><span>      عدم الظهور في  فاتورة الشراء     </span>
                <br />
                <asp:CheckBox ID="chkIsUseTax" runat="server" ClientIDMode="Static"/><span>استخدام ضريبة القيمة المضافة</span>
                <br />
                <asp:AutoComplete ID="acTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                            runat="server"></asp:AutoComplete>
                <br />                
                <asp:CheckBox ID="chkIsTaxIncludedInPurchase" runat="server" ClientIDMode="Static" Enabled="false"/><span>سعر الشراء شامل الضريبة</span>
                <br />
                <asp:CheckBox ID="chkIsTaxIncludedInSale" runat="server" ClientIDMode="Static" Enabled="false"/><span>سعر البيع شامل الضريبة</span>
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

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
        PopupControlID="Panel1" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="400">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" CssClass="pnlPopUp" runat="server" Width="480">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                
                 <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"  ValidationGroup="AddNew1"  
                   ></asp:AutoComplete>  <br />
                 <asp:ABFGridView runat="server" GridViewStyle="GrayStyle" ID="gvStoresCatergory"  DataKeyNames="ID,Name" OnRowDeleting="gvStoresCatergory_RowDeleting" >
                    <Columns>
                        <asp:TemplateField HeaderText="التسلسل">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>

               
              
               

            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddNew1" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveStoresItemsCategory" CssClass="button default_button" runat="server" OnClick="btnSaveStoresItemsCategory_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew1" />
                
            </div>
        </div>
    </asp:Panel>


</asp:Content>
