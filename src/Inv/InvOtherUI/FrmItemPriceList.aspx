<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmItemPriceList.aspx.cs" Inherits="Inv_InvOtherUI_FrmItemPriceList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView runat="server" ID="gvPriceList1"
                CssClass="table table-responsive-sm table-hover table-outline mb-0"
                AllowCustomPaging="true" AutoGenerateColumns="false" CellPadding="4" HorizontalAlign="Center" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                <EditRowStyle BackColor="#999999"></EditRowStyle>

                <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" CssClass="tr" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>

                <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>

                <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>

                <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                <Columns>
                    <asp:BoundField DataField="AttName" HeaderText="<%$Resources:Labels,Name %>" />
                    <asp:BoundField DataField="Price" HeaderText="<%$Resources:Labels,Price %>"
                        DataFormatString="{0:0.####}" />

                    <asp:TemplateField HeaderText="<%$Resources:Labels,Select %>" HeaderStyle-HorizontalAlign="Center" ControlStyle-Width="50px" ControlStyle-Height="50px">
                        <ItemTemplate>                            
                                
                                    <a href="#" onclick="itemPriceSelected('<%=Request.QueryString["rowIndex"] %>','<%#Eval("Price")%>')">
                                        <img src="../../images/trueimg.png" style="width: 40px; height: 40px" />
                                    </a>
                                </ContentTemplate>                           
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
