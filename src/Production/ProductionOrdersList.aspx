<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ProductionOrdersList.aspx.cs" Inherits="Production_ProductionOrdersList" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:HyperLink ID="lnkadd" runat="server" CssClass="collapse_add_link" Text=" <%$ Resources:Labels,AddNew %>"></asp:HyperLink>
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
                    <asp:ABFTextBox ID="txtDateFromSrch" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Status %></label>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,Select %>" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Draft %>" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Approved %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,Canceled %>" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSerialsrch" CssClass="field" runat="server" LabelText="<%$Resources:Labels,Serial %>">
                    </asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems"
                        IsRequired="true"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateToSrch" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtUserRefNo" CssClass="field" LabelText="<%$Resources:Labels,UserRefNo %>"
                        runat="server"></asp:ABFTextBox>

                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                    OnClick="btnSearch_click" ValidationGroup="search" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvProductionOrdersList" GridViewStyle="GrayStyle"
        DataKeyNames="ID,Branch_ID" OnSelectedIndexChanging="gvProductionOrdersList_SelectedIndexChanging"
        OnPageIndexChanging="gvProductionOrdersList_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
            <asp:BoundField DataField="OperationDate" HeaderText="<%$Resources:Labels,Date %>"
                DataFormatString="{0:d/M/yyyy}" />
            <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
            <asp:BoundField DataField="Quantity" HeaderText="<%$Resources:Labels,Quantity %>" DataFormatString="{0:0.####}" />
            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Labels,Branch %>" ItemStyle-CssClass="BranchCol" />
            <asp:BoundField DataField="DocStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:BoundField DataField="ReceivedStatus" HeaderText="<%$Resources:Labels,Status %>" />
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("ProductionOrder.aspx?ID={0}", Eval("ID") ) %>'
                        Text="<img src='../../images/edit_grid.gif' />" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Print %>">
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/print-icon.png" runat="server" OnClick="btnPrint_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Expenses %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("Expenses.aspx?ProductionOrder_ID={0}", Eval("ID") ) %>'
                        Text="<%$ Resources:Labels, Expenses %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Damages %>">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# String.Format("Damages.aspx?ProductionOrder_ID={0}", Eval("ID") ) %>'
                        Text="<%$ Resources:Labels, Damages %>" Visible='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Receive %>">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkReceive" Text="<%$ Resources:Labels, Receive %>" runat="server"
                        OnClick="lnkReceive_Click" Visible='<%# !Convert.ToBoolean(Eval("IsReceived"))  && Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2") %>'></asp:LinkButton>
              <%-- && Convert.ToBoolean(Eval("IsDamagedApproved"))--%>
                     </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <asp:HiddenField ID="hfReceive" runat="server" />
    <asp:ModalPopupExtender ID="mpeReceive" runat="server" TargetControlID="hfReceive"
        PopupControlID="pnlReceive" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="500">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlReceive" CssClass="pnlPopUp" runat="server" Width="288">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="btnClosepopup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form">
                <br />
                <span>
                    <%=Resources.Labels.Serial %></span>:
                <asp:Label ID="lblProductionOrderSerial" Text="" runat="server"></asp:Label>
                <asp:ABFTextBox ID="txtReceiveDate" CssClass="field" runat="server" Width="250px"
                    IsRequired="true" LabelText="<%$Resources:Labels,Date %>" DataType="Date" ValidationGroup="Receive">
                </asp:ABFTextBox>
                <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores"
                    ValidationGroup="Receive" IsRequired="true" LabelText="<%$Resources:Labels,Store %>"></asp:AutoComplete>
                <asp:ABFTextBox ID="txtQuantity" runat="server" ValidationGroup="Receive" IsRequired="true" MinValue="0.001"
                    LabelText="<%$Resources:Labels,Quantity %>" DataType="Decimal"></asp:ABFTextBox>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Receive" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnOkCollect" CssClass="button default_button" runat="server" OnClick="btnOkReceive_click"
                    ValidationGroup="Receive" Text="<%$ Resources:Labels, Receive %>" />
                <asp:Button ID="BtnCancelCollect" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
