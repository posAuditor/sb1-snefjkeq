<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GroupPermissions.aspx.cs" Inherits="Security_GroupPermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <span>
        <%= Resources.Labels.GroupName %></span>:
    <asp:Label ID="lblGroupName" Font-Bold="true" runat="server"></asp:Label><br />
    <br />
    <div class="MainGrayDiv">
        <asp:Button ID="btnCollapse" runat="server" CssClass="collapse_tree" Text=" " OnClientClick="return CollapseAll()" />
        <asp:Button ID="btnExpand" runat="server" CssClass="expand_tree" Text=" " OnClientClick="return ExpandAll()" />
        <div id="SelectAllDiv">
            <asp:CheckBox ID="chkSelectAll" runat="server" Text="<%$ Resources:Labels,SelectAll %>" />
            <asp:CheckBox ID="chkAllAdd" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,Add %>" />
            <asp:CheckBox ID="chkAllEdit" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,Edit %>" />
            <asp:CheckBox ID="chkAllApprove" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,Approve %>" />
            <asp:CheckBox ID="chkAllNotApprove" CssClass="selectall" runat="server" Text="الغاء الاعتماد" />
            <asp:CheckBox ID="chkAllViewDoc" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,ViewDoc %>" />
            <asp:CheckBox ID="chkAllViewList" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,ViewList %>" />
            <asp:CheckBox ID="chkAllDelete" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,DeleteCancel %>" />
            <asp:CheckBox ID="chkAllPrint" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,Print %>" />
            <asp:CheckBox ID="chkAllAttach" CssClass="selectall" runat="server" Text="<%$ Resources:Labels,Attach %>" />
            <asp:CheckBox ID="chkAllDeleteAttach" CssClass="selectall" runat="server" Text="حذف المرفق" />
            <asp:CheckBox ID="chkAllViewAttach" CssClass="selectall" runat="server" Text="مشاهدة المرفق" />
            <asp:CheckBox ID="chkUseApproveAccounting" CssClass="selectall" runat="server" Text="إعتماد مالي" />
             
        </div>
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
                $(obj).addClass(($(obj).parent().siblings(".EntityNode,.PermissionNode").attr('id')));
                $(obj).addClass(($(obj).parent().siblings(".EntityNode,.PermissionNode").attr('class')));
            });


            $("#cph_tvPages input.EntityNode").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='" + $(this).attr('class').split(' ')[0] + "']").each(function (index, obj) {
                    obj.checked = checked;
                });

            });

            $("#cph_chkSelectAll").change(function () {
                var checked = this.checked;

                $("#cph_tvPages input[type=checkbox], .selectall input").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllAdd").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-0'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllEdit").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-1'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllApprove").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-2'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllViewDoc").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-3'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllViewList").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-4'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllDelete").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-5'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllPrint").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-6'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllAttach").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-7'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllNotApprove").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-8'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllDeleteAttach").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-9'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            });

            $("#cph_chkAllViewAttach").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-10'].PermissionNode").each(function (index, obj) {
                    obj.checked = checked;
                });
            }); $("#cph_UseApproveAccounting").change(function () {
                var checked = this.checked;
                $("#cph_tvPages input[class*='-11'].PermissionNode").each(function (index, obj) {
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
            window.location = '<%= Page.ResolveClientUrl( XPRESS.Common.PageLinks.Users) %>';
        }

    </script>
</asp:Content>
