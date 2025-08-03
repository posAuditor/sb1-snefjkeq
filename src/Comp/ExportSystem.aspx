<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ExportSystem.aspx.cs" Inherits="Comp_ExportSystem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style type="text/css">
        .Row {
            display: table;
            width: 90%;
            table-layout: fixed;
            border-spacing: 10px;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }

        .Column {
            display: table-cell;
            /*background-color: red;*/
        }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="InvoiceSection">
            <div class="Row">
                <div class="Column">
                    <label>
                        الملف
                    </label>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="إستخدام السطر الاول لعناوين الحقول"></asp:Label><br />
                    <asp:RadioButtonList ID="rbHDR" runat="server" RepeatLayout="Flow">
                        <asp:ListItem Text="نعم" Value="Yes" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="لا" Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="Column">
                    <label>
                        جدول البيانات
                    </label>
                    <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlIsHasBill_SelectedIndexChanged" AutoPostBack="true">

                        <asp:ListItem Text="-- إختر --" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="الفروع" Value="0"></asp:ListItem>
                        <asp:ListItem Text="المخازن" Value="1"></asp:ListItem>
                        <asp:ListItem Text="المواد" Value="2"></asp:ListItem>
                        <asp:ListItem Text="العملاء" Value="3"></asp:ListItem>
                        <asp:ListItem Text="الموردين" Value="4"></asp:ListItem>
                        <asp:ListItem Text="البنوك" Value="5"></asp:ListItem>

                        <asp:ListItem Text="الاصول" Value="6"></asp:ListItem>
                        <asp:ListItem Text="القيود" Value="7"></asp:ListItem>
                        <asp:ListItem Text="الحسابات" Value="8"></asp:ListItem>
                        <asp:ListItem Text="الوحدات" Value="9"></asp:ListItem>

                        <asp:ListItem Text="الفئات" Value="10"></asp:ListItem>




                    </asp:DropDownList>
                    <div class="Row">
                        <div class="Column">
                            <asp:DropDownList ID="ddlProperties" runat="server" ValidationGroup="AddItem">
                            </asp:DropDownList>
                        </div>
                        <div class="Column">
                            <asp:DropDownList ID="ddlPropertiesValue" runat="server" ValidationGroup="AddItem">
                            </asp:DropDownList>
                        </div>
                        <div class="Column">
                            <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_Click"
                                Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                        </div>
                    </div>
                    <div class="Row">
                        <div class="Column">
                            <asp:ABFGridView runat="server" ID="gvExportList" GridViewStyle="GrayStyle" DataKeyNames="ID" OnRowDeleting="gvExportList_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="إسم الحقل" />
                                    <asp:BoundField DataField="Value" HeaderText="الحقل المرادف" />
                                    <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgButtonDelete" ImageUrl="../images/delete_grid.gif" runat="server"
                                                CommandName="Delete" OnClientClick="return ConfirmSure();"   />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:ABFGridView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="Column">
                            <asp:Button ID="btnImport" CssClass="button" runat="server" OnClick="btnImport_Click"
                                Text="إستراد" />
                        </div>
                    </div>

                </div>
            </div>
            <div class="Row">
                <div class="Column">
                    <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="PageIndexChanging"
                        AllowPaging="true">
                    </asp:GridView>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

