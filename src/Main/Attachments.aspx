<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Attachments.aspx.cs" Inherits="Main_Attachments" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainGrayDiv">
        <div class="form">
            <label>
                <%=Resources.Labels.File %></label>
            <asp:FileUpload ID="fpFile" runat="server" Width="400"></asp:FileUpload>
            <asp:Button ID="btnUpload" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>"
                OnClick="btnUpload_Click" />
        </div>
    </div>
    <br />
    <br />
    <asp:ABFGridView runat="server" ID="gvAttachments" GridViewStyle="GrayStyle" DataKeyNames="ID,Guid,FileName"
        OnRowDeleting="gvAttachments_RowDeleting" OnPageIndexChanging="gvAttachments_PageIndexChanging"
        PageSize="10" OnSelectedIndexChanging="gvAttachments_SelectedIndexChanging">
        <Columns>
            <asp:BoundField DataField="FileName" HeaderText="<%$ Resources:Labels, FileName %>" />
            <asp:BoundField DataField="FileSize" HeaderText="<%$ Resources:Labels, FileSize %>"



                DataFormatString="{0:F2}" />

             <asp:TemplateField HeaderText="عرض">
                <ItemTemplate>
                    <asp:ImageButton CommandName="View" ID="ImageButton2" ImageUrl="../images/search_20.png" runat="server"
                        OnClientClick='<%#GetUrl(Eval("FileName").ToString(),Eval("Guid").ToString()) %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="<%$Resources:Labels,Download %>" runat="server" CommandName="Select"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" ImageUrl="../images/delete_grid.gif" runat="server"
                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:ABFGridView>
    <iframe id="ifDownload" runat="server" src="" style="display: none;"></iframe>
</asp:Content>
