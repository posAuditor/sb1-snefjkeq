<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="License.aspx.cs" Inherits="Main_License" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div style="font-size: 17px;color:red" runat="server" id="msgArabic">
        نأسف لايمكن استمرار العمل على نسخة البرنامج
        <br />
        السبب:
        <asp:Label runat="server" ID="lblReason" Text="" ForeColor="Red"></asp:Label>
        <br />
        رقم الخطأ:
        <asp:Label runat="server" ID="lblReasonNumber" Text="" ForeColor="Red"></asp:Label>
        <br />
        <br />
        لقد تم اعلام الشركة المنتجة بالخطأ المذكور
        <br />
        عند تكرار حدوث الخطأ يرجى التواصل مع الدعم الفنى
        <br />
        <br />
        اذا كنت قد قمت بشراء البرنامج بالفعل وسددت كافة المستحقات فبرجاء الضغط على  زر "اعادة الترخيص"
          <br />
        <br />
        موقع الشركة المنتجة للبرنامج 
        <br />
        <a href="http://www.auditorerp.cloud/" target="_blank">www.auditorerp.cloud</a>

    </div>
    <div style="font-size: 17px;color:red" runat="server" id="msgEng">
        Sorry! You can NOT keep working on the current copy
        <br />
        Reason:
        <asp:Label runat="server" ID="lblReasonEng" Text="" ForeColor="Red"></asp:Label>
        <br />
        Error No:
        <asp:Label runat="server" ID="lblReasonNumberEng" Text="" ForeColor="Red"></asp:Label>
        <br />
        <br />
        The Company has been informed about this error.
        <br />
        If this error is permenant please contact customer service.
           <br />
        <br />
        If you have already purchased this ERP system please click "ReLicense"
          <br />
        <br />
        Produced by:
        <br />
        <a href="http://www.auditorerp.cloud/" target="_blank">www.auditorerp.cloud</a>
    </div>
    <br />
    <center>
        <asp:Button ID="lnkReLicense" runat="server" Text="<%$ Resources:Labels,ReLicense %>" ForeColor="Green" Style="padding: 10px; cursor: pointer; font-family: Droid Arabic Kufi, Arial;"
            Font-Size="20" OnClientClick="return ConfirmSure();" OnClick="lnkReLicense_Click"></asp:Button></center>
</asp:Content>
