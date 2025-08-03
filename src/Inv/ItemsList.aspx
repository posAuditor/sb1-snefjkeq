<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemsList.aspx.cs" Inherits="Inv_ItemsList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Style="background: Transparent" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:LinkButton ID="lnkItemsUpdateTax" runat="server" Style="background: Transparent" CssClass="collapse_search_link" Text="تعديل الضريبة على كل الاصناف"></asp:LinkButton>

    <a runat="server" id="lnkadd" href="Items.aspx" class="collapse_add_link">
        <%= Resources.Labels.AddNew %></a>

    <a runat="server" id="lnkExport" href="~/Comp/Export.aspx?Items=1" class="collapse_add_link">
        <%= Resources.Labels.Export %></a>

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
                    <asp:ABFTextBox ID="txtBarcodeSrch" CssClass="field barcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true">
                    </asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.ItemType %></label>
                    <asp:DropDownList ID="ddlItemType" runat="server" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,InventoryItem %>" Value="i"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,SerivceItem %>" Value="s"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,RawMaterial %>" Value="m"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,FinalItem %>" Value="c"></asp:ListItem>
                    </asp:DropDownList>

                    <br />
                    <asp:CheckBox ID="chkIsDescription" runat="server" Text="<%$Resources:Labels,ArticleDescribedLabel %>" />
                    <br />

                </div>
                <div class="left_col">
                    <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                        LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                        AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acNameSrch" ServiceMethod="GetItems" KeepTextWhenNoValue="true"
                        LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>



                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>" OnClientClick="Display('cph_btnSearch')"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvItemssList" GridViewStyle="BlueStyle" DataKeyNames="ID,Name"
        OnRowDeleting="gvItemssList_RowDeleting" OnPageIndexChanging="gvItemssList_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="التسلسل">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Name %>" />
            <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />
            <asp:BoundField DataField="CodeItem" HeaderText="رمز الصنف" />
            <asp:BoundField DataField="BarCode" HeaderText="<%$Resources:Labels,Barcode %>" />
            <asp:BoundField DataField="UOMName" HeaderText="<%$Resources:Labels,SmallestUnit %>" />
            <asp:BoundField DataField="SalesPrice" HeaderText="<%$Resources:Labels,Purchasingprice %>" />
            <asp:BoundField DataField="DefaultPrice" HeaderText="<%$Resources:Labels,Sellingprice %>" />

            <asp:TemplateField HeaderText="مواد خام">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkRelated" Text="الاصناف" OnClick="lnkRelated_Click" CommandArgument='<%#Eval("ID") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("Items.aspx?ID={0}", Eval("ID") ) %>'
                        Text="<img src='../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>


    <asp:ModalPopupExtender ID="mpeUpdateItemTax" runat="server" TargetControlID="lnkItemsUpdateTax"
        PopupControlID="pnlDescriptionReady" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlDescriptionReady" CssClass="pnlPopUp" runat="server" Width="400">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnCloseDescriptionReady"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">


            <asp:AutoComplete ID="acTax" ServiceMethod="GetTaxes" LabelText="<%$Resources:Labels,Tax %>"
                runat="server"></asp:AutoComplete>


            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary8" runat="server" ValidationGroup="AddNewDescriptionReady" />
            </div>
            <br />
            <div class="btnDiv">
                <asp:Button ID="btnUpdateItemTax" CssClass="button default_button" runat="server" OnClick="btnUpdateItemTax_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNewDescriptionReady" />
                <asp:Button ID="btnCloseUpdateItemTax" runat="server" CssClass="button"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="HiddenField7" runat="server" />
    <asp:ModalPopupExtender ID="mpeMaterial" runat="server" TargetControlID="HiddenField7" Drag="true"
        PopupControlID="pnlMaterial" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResizeAndScroll"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlMaterial" runat="server" CssClass="pnlPopUp" Width="480">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button17"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <asp:ABFGridView runat="server" ID="gvMaterial" GridViewStyle="GrayStyle">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="الصنف" />
                        <asp:BoundField DataField="Quantity" HeaderText="الكمية"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="AttName" HeaderText="الوحدة" />
                    </Columns>
                </asp:ABFGridView>
            </div>
            <div class="btnDiv">
            </div>
        </div>
    </asp:Panel>

</asp:Content>
