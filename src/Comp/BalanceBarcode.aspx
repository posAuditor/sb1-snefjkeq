<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="BalanceBarcode.aspx.cs" Inherits="Comp_BalanceBarcode" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" Style="min-width: 800px;">



        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="إعدادات  الميزان  ">
            <ContentTemplate>



                <div class="form" style="width: 600px;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtFirstCode" runat="server" LabelText="الرقم الثابت"
                            IsRequired="true" ValidationGroup="NewItem">
                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtLegthWeight" runat="server" LabelText="طول الوزن" CssClass="barcode"
                            IsRequired="true" ValidationGroup="NewItem"> </asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="طول الباركود" CssClass="barcode"
                            IsRequired="true" ValidationGroup="NewItem">

                        </asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtDecimal" runat="server" LabelText="طول الاعداد العشرية للوزن" CssClass="barcode"
                            IsRequired="true" ValidationGroup="NewItem"> </asp:ABFTextBox>
                    </div>
                </div>

                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="NewItem" />
                </div>

                <br />
                <div class="btnDiv">
                    <asp:Button ID="btnSave" CssClass="button shortcut_save" runat="server" OnClick="btnSave_Click"
                        Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewItem" />
                    <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
                        OnClick="BtnReturn_Click" />
                </div>

            </ContentTemplate>
        </asp:TabPanel>



        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="إعدادات الخصم">
            <ContentTemplate>
                <asp:ABFTextBox ID="txtSerialPrefix" runat="server" Width="150" ValidationGroup="NewDocSerial"
                    LabelText="<%$Resources:Labels,PercentageDiscount %>" IsRequired="true"></asp:ABFTextBox>


                <asp:Button ID="btnSaveSettingsPOss" CssClass="button default_button" runat="server" OnClick="btnSaveSettingsPOss_Click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewDocSerial" />

                <br />
                <br />
                <br />
                <br />
            </ContentTemplate>
        </asp:TabPanel>


    </asp:TabContainer>



</asp:Content>

