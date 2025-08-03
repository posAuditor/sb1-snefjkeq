<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListAll.aspx.cs" Inherits="Inv_ItemUI_ListAll" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        jqVar(document).ready(function () {
            jqVar('#ListInput').on("keyup", function () {
                var value = jqVar(this).val().toLowerCase();
                jqVar("#" + '<%=gvItemssList.ClientID%>' + " .tr").filter(function () {
                    jqVar(this).toggle(jqVar(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
        <%--<input id="ListInput" class="form-control" type="text" placeholder="<%=Resources.Labels.SearchHere %>" />--%>

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
                        <asp:LinkButton ID="lnkRelated" Text="الاصناف" CommandArgument='<%#Eval("ID") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("Items.aspx?ID={0}", Eval("ID") ) %>'
                            Text="<img src='../../images/edit_grid.gif' />" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton2" ImageUrl="../../images/delete_grid.gif" runat="server"
                            CommandName="Delete" OnClientClick="return ConfirmSure();" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:ABFGridView>
    </form>
</body>
</html>
