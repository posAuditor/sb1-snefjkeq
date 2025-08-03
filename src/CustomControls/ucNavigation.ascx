<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNavigation.ascx.cs" Inherits="CustomControls_ucNavigation" %>
<style>
    .xx {
    }

        .xx a {
            text-decoration: none;
            display: inline-block;
            /*padding: 8px 16px;*/
            padding: 2px 19px;
            font-size: 29px;
            color: white !important;
        }

            .xx a:hover {
                background-color: #b579e2;
                color: black;
            }

        .xx .xxx a:hover {
            background-color: transparent !important;
            color: red;
        }

    .previous {
        /*background-color: #f1f1f1;*/
        background-color: transparent !important;
        color: black;
    }

    .next {
        /*background-color: #f1f1f1;*/
        background-color: transparent !important;
        color: white;
    }

    .round {
        border-radius: 50%;
    }


</style>

<span class="xx">

    <asp:TextBox ID="txtSerialSearch" runat="server"  AutoCompleteType="None" Width="130px" Style="background-color: transparent; color: red; font-size: 15px;"></asp:TextBox>
    <span class="xxx">
        <asp:LinkButton ID="lnkSearchInvoice" runat="server" Style="padding: 0px;" OnClick="lnkSearchInvoice_Click">
           <i class="fa fa-search" ></i>
        </asp:LinkButton>
    </span>
    <span class="xxx">
        <asp:LinkButton ID="lnkAddNewItem" runat="server" CssClass=" previous round" OnClick="lnkAddNewItem_Click">
     <i class="fa fa-plus"></i>
        </asp:LinkButton>
    </span>
    <span class="xxx">
        <asp:LinkButton ID="lnkFirst" OnClick="lnkFirst_Click" CssClass="xx previous round" runat="server">
         <i class="fa fa-fast-forward"></i>
        </asp:LinkButton>
    </span>
    <span class="xxx">
        <asp:LinkButton ID="lnkPrev" CssClass="xx previous round" runat="server" OnClick="lnkPrev_Click">

        <i class="fa fa-arrow-right"></i>
        </asp:LinkButton>
    </span>

    <span class="xxx">
        <asp:LinkButton ID="lnkNext" CssClass="xx next round" runat="server" OnClick="lnkNext_Click">
           <i class="fa fa-arrow-left"></i>
        </asp:LinkButton>
    </span>
    <span class="xxx">
        <asp:LinkButton ID="lnkLast" CssClass="xx next round" runat="server" OnClick="lnkLast_Click">
   <i class="fa fa-fast-backward"></i>
        </asp:LinkButton>
    </span>
</span>