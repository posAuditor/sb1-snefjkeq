<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BranchesTree.aspx.cs" Inherits="Accounting_BranchesTree" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .treeNode {
            cursor: pointer;
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }

            .treeNode:hover {
                text-decoration: underline;
            }

        .treeNode2 {
            border-top: 1px solid rgb(255, 255, 255);
            border-right: 1px solid rgb(255, 255, 255);
            padding: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
   
    <br />
    <div style="clear: both;">
        <asp:TreeView ID="tvAccounts" runat="server" ShowLines="True">
        </asp:TreeView>
    </div>
  
    <script type="text/javascript">

        function ExpandAll() {
            $("#cph_tvAccounts td a > img[alt*=Expand]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            $("#cph_tvAccounts td a > img[alt*=توسيع]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;

        }

        function CollapseAll() {
            $("#cph_tvAccounts td a > img[alt*=Collapse]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            $("#cph_tvAccounts td a > img[alt*=طي]").each(function (index, element) {
                if ($(element).parent().attr("href") != undefined) {
                    var str = $(element).parent().attr("href").replace("javascript:", "");
                    eval(str);
                }
            });
            resizeIframe();
            return false;
        }

      
 
    </script>
</asp:Content>
