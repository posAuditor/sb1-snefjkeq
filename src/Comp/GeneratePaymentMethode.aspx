<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="GeneratePaymentMethode.aspx.cs" Inherits="Comp_GeneratePaymentMethode" %>


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
    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" Style="min-width: 800px;">
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="تعريف طرق الدفع">
            <ContentTemplate>
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
                                OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvGPaymentMethode" GridViewStyle="BlueStyle" DataKeyNames="ID,NamePayment,Branch_ID"
                    OnRowDeleting="gvGPaymentMethode_RowDeleting" OnPageIndexChanging="gvGPaymentMethode_PageIndexChanging"
                    OnSelectedIndexChanging="gvGPaymentMethode_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NamePayment" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="SalesAccountName" HeaderText="<%$Resources:Labels,SalesAccount %>" />
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Labels,Branch %>" />
                        <%--<asp:BoundField DataField="PurchaseAccountName" HeaderText="حساب النسبة" />--%>
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
             
                   <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="320">
                    <div class="tcat">
                        <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="close_popup_Click"></asp:Button>
                        <span>
                            <%=this.MyContext.PageData.PageTitle %></span>
                    </div>
                    <div class="content">
                        <div class="form">
                            <div class="">

                       <asp:ABFTextBox ID="txtName" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew"></asp:ABFTextBox>

                                   <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="AddNew"></asp:AutoComplete>


                               
                                <asp:AutoComplete runat="server" ID="acSalesAccount" ServiceMethod="GetChartOfAccounts"
                                    ValidationGroup="AddNew" IsRequired="true" LabelText="<%$Resources:Labels,SalesAccount %>"></asp:AutoComplete>
                                 


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
                            <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>


            </ContentTemplate>
        </asp:TabPanel>
 





    </asp:TabContainer>

</asp:Content>

