<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Payment_Generate.aspx.cs" Inherits="Payments_Payment_Generate" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="false" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
        AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
        ExpandDirection="Vertical" SuppressPostBack="true">
    </asp:CollapsiblePanelExtender>
    <div style="clear: both;">
    </div>
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnShow">
        <div class="tcat">
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                       AutoPostBack="true"></asp:AutoComplete>
                        <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList runat="server" ID="ddlDocType">
                        <asp:ListItem Value="0" Text="اذن استلام نقدية"></asp:ListItem>
                        <asp:ListItem Value="1" Text=" استلام نقدية من عميل"></asp:ListItem>
                         <asp:ListItem Value="4" Text=" ايداع بنكى"></asp:ListItem>
                        <asp:ListItem Value="5" Text="ايداع بنكي من عميل"></asp:ListItem>
                         <asp:ListItem Value="2" Text="اذن صرف نقدية"></asp:ListItem>
                        <asp:ListItem Value="3" Text=" اذن صرف نقدية لمورد"></asp:ListItem>
                        <asp:ListItem Value="6" Text="سحب بنكى"></asp:ListItem>
                        <asp:ListItem Value="7" Text="سحب بنكى لمورد"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="left_col">
                    <asp:ABFTextBox ID="txtDateFrom" CssClass="field" LabelText="<%$Resources:Labels,DateFrom %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtDateTo" CssClass="field" LabelText="<%$Resources:Labels,DateTo %>"
                        DataType="Date" runat="server" ValidationGroup="search"></asp:ABFTextBox>
                      


                 
                     
                   
                     <asp:AutoComplete runat="server" ID="acnameEmp" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"
                        KeepTextWhenNoValue="true"></asp:AutoComplete>
                     
                 
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnShow" CssClass="button" runat="server" Text="<%$ Resources:Labels, Show %>"
                    ValidationGroup="search" OnClick="btnShow_click" />
              
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <iframe id="ifViewer" name="ifViewer" src="" scrolling="yes" width="100%" runat="server"
        height="1400" frameborder="0"></iframe>
</asp:Content>
