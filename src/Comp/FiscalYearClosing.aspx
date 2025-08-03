<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="FiscalYearClosing.aspx.cs" Inherits="Comp_FiscalYearClosing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div style="direction: rtl; font-size: 15px; color: Red; font-family: Tahoma;" class="MainGrayDiv">
        <center>
            قبل اغلاق السنه يرجى التأكد من النقاط التالية</center>
        <ul>
            <li>
                <p>
                    يرجى عدم استخدام النظام من اى جهاز اخر اثناء الاغلاق
                </p>
            </li>
            <li>
                <p>
                    اعتمد جميع المستندات حيث ان المستندات الحالية سيتم حذفها ولن ثؤثر فى الحسابات
                </p>
            </li>
            <li>
                <p>
                    يرجى تسوية ارصدة الاصناف السالبة ان وجدت
                </p>
            </li>
            <li>
                <p style="font-weight: bold">
                    يجب عمل نسخه احتياطية من قاعدة البيانات قبل البدء فى الاغلاق -- هااام!
                </p>
            </li>
            <li>
                <p>
                    يرجى حساب واعتماد المرتبات للموظفين
                </p>
            </li>
            <li>
                <p>
                    يرجى مراجعة اهلاكات الاصول
                </p>
            </li>
            <li>
                <p>
                    قد يستغرق الاغلاق بعض الوقت حسب حجم البيانات فيرجى الانتظار وعدم اغلاق الصفحة
                </p>
            </li>
            <li>
                <p>
                    يجب ادخال تاريخ بداية السنه المالية الجديدة بعد الاغلاق والضغط على حفظ فى شاشة اعدادات الشركة!
                </p>
            </li>


            <li>
                <p style="font-weight: bold">
                    بعد الاغلاق راجع ميزاينة السنة الجديدة وقارنها بالسنه القديمة يجب ان يكونا متطابقتان تماما --- هااام
                </p>
            </li>
        </ul>
    </div>
    <center>
        <asp:Button runat="server" ID="btnClose" CssClass="button" Text="<%$ Resources:Labels,CloseFiscalYear %>"
            OnClick="btnClose_Click" OnClientClick="return ConfirmSure()" />
    </center>
</asp:Content>
