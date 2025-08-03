<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CustomPages.aspx.cs" Inherits="Security_CustomPages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <br />
    <div class="MainGrayDiv">
        <asp:Button ID="btnCollapse" runat="server" CssClass="collapse_tree" Text=" " OnClientClick="return CollapseAll()" />
        <asp:Button ID="btnExpand" runat="server" CssClass="expand_tree" Text=" " OnClientClick="return ExpandAll()" />
        <br />
        <br />
        <div>
            <asp:CheckBox ID="chkBatchID" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,BatchID %>" />
            <asp:CheckBox ID="chkCashDiscount" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,CashDiscount %>" />
            <asp:CheckBox ID="chkPercentageDiscount" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,PercentageDiscount %>" />
          &nbsp;&nbsp;  <label>
                <%=Resources.Labels.WorkingMode %></label>
            <asp:DropDownList ID="ddlWorkingMode" runat="server"  OnSelectedIndexChanged="ddlWorkingMode_SelectedIndexChanged" AutoPostBack="true" Width="250">
                <asp:ListItem Text="<%$ Resources:Labels,Companies %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Labels,SmallComapnies %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Labels,MegaCompanies %>" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <asp:TreeView runat="server" ID="tvPages" ShowLines="true" ExpandDepth="FullyExpand" />
    </div>

    <div class="align_right">
        <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Labels,Save %>" CssClass="button_big shortcut_save"
            OnClick="BtnSave_Click" />
        <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Labels,Return %>" CssClass="button_big" OnClientClick="RedirectToList();" />
    </div>
    <br />
    <br />
    <script type="text/javascript">

        $(document).ready(function () {

            $("#cph_tvPages input[type=checkbox]").each(function (index, obj) {
                $(obj).addClass(($(obj).parent().siblings(".checkme").attr('class')));
            });


            $("#cph_tvPages input.checkme").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='" + $(this).attr('class').split(' ')[1] + "']").each(function (index, obj) {
                    obj.checked = checked;
                });

            });

        });

        function ExpandAll() {

            $("#cph_tvPages td a > img[alt*=Expand]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            return false;

        }

        function CollapseAll() {
            $("#cph_tvPages td a > img[alt*=Collapse]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            return false;
        }

        function RedirectToList() {
            window.location = '<%= Page.ResolveClientUrl( XPRESS.Common.PageLinks.StartScreen) %>';
        }

    </script>
</asp:Content>
