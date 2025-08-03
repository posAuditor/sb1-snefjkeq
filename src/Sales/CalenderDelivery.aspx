<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="CalenderDelivery.aspx.cs" Inherits="Sales_CalenderDelivery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        ul {
            list-style-type: none;
        }

        body {
            font-family: Verdana, sans-serif;
        }

        /* Month header */
        .month {
            padding: 70px 25px;
            width: 100%;
            background: #662d91;
            text-align: center;
        }

            /* Month list */
            .month ul {
                margin: 0;
                padding: 0;
            }

                .month ul li {
                    color: white;
                    font-size: 20px;
                    text-transform: uppercase;
                    letter-spacing: 3px;
                }

            /* Previous button inside month header */
            .month .prev {
                float: left;
                padding-top: 10px;
                padding-left: 50px;
                color: white !important;
            }

            /* Next button */
            .month .next {
                float: right;
                padding-top: 10px;
                color: white !important;
                padding-right: 50px;
            }

        /* Weekdays (Mon-Sun) */
        .weekdays {
            margin: 0;
            padding: 10px 0;
            background-color: #ddd;
        }

            .weekdays li {
                display: inline-block;
                width: 13.6%;
                color: #666;
                font-size: 20px;
                text-align: center;
            }

        /* Days (1-31) */
        .days {
            padding: 10px 0;
            background: #eee;
            margin: 0;
        }

            .days li {
                list-style-type: none;
                display: inline-block;
                width: 13.6%;
                text-align: center;
                margin-bottom: 5px;
                font-size: 20px;
                color: #777;
                height: 50px;
            }

                /* Highlight the "current" day */
                .days li .active {
                    padding: 5px;
                    background: #662d91;
                    color: white !important;
                }
                /* Highlight the "current" day */
                .days li .DaysInvoice {
                    padding: 15px;
                    background: red;
                    color: white !important;
                }

                .days li .DaysInvoiceActive {
                    padding: 15px;
                    background: #ff0000;
                    color: #662d91 !important;
                    border: 1px solid #1abc9c;
                }
    </style>
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
        @font-face {
            font-family: 'awesonne';
            src: url('../Fonts/NFont/font/awesonne.eot?30429');
            src: url('../Fonts/NFont/font/awesonne.eot?30429#iefix') format('embedded-opentype'), url('../Fonts/NFont/font/awesonne.woff?30429') format('woff'), url('../Fonts/NFont/font/awesonne.ttf?30429') format('truetype'), url('../Fonts/NFont/font/awesonne.svg?30429#awesonne') format('svg');
            font-weight: normal;
            font-style: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">

    <br />
    <br />
    <div class="month">
        <ul>
            <li class="prev">
                <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">السابق</asp:LinkButton>
            </li>
            <li class="next">
                <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">التالي</asp:LinkButton></li>
            <li>

                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
                <br>
                <span style="font-size: 18px">
                    <asp:Label ID="lblYear" runat="server" Text=""></asp:Label></span></li>
        </ul>
    </div>

    <ul class="weekdays">
        <li>الاثنين</li>
        <li>الثلاثاء</li>
        <li>الاربعاء</li>
        <li>الخميس</li>
        <li>الجمعة</li>
        <li>السبت</li>
        <li>الاحد</li>
    </ul>

    <ul class="days">
        <asp:Repeater ID="rptDays" runat="server">
            <ItemTemplate>
                <li>
                    <span class='<%# GetStyle(Eval("Day").ToString(),CurrentDate.Day,int.Parse( Eval("NumberInvoice").ToString())) %>'><%# Eval("Day") %>
                        <%-- <asp:Button Visible='<%# int.Parse( Eval("NumberInvoice").ToString()) !=0 %>' ID="btnViewInvoice" runat="server" Text='<%# Eval("NumberInvoice") %>' />--%>
                        <asp:LinkButton OnClick="LinkButton1_Click" Visible='<%# int.Parse( Eval("NumberInvoice").ToString()) !=0 %>'
                            ID="LinkButton1" runat="server" Text='<%# Eval("NumberInvoice") %>' CommandArgument='<%#Eval("Day").ToString() %>'></asp:LinkButton>
                        <asp:LinkButton
                            Visible='<%# GetStyleVisibility(Eval("Day").ToString(),CurrentDate.Day,int.Parse( Eval("NumberInvoice").ToString())) %>'
                            ID="lnkBtnAddInvoice"
                            runat="server"
                            OnClick="lnkBtnAddInvoice_Click"
                            CommandArgument='<%#Eval("Day").ToString() %>'>  
                             <i class="btnShartcatInvoice demo-icon icon-plus" style="font-size: 20px; padding-top: 10px; color: black!important">
                             </i>

                        </asp:LinkButton>
                    </span>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

    <asp:HiddenField ID="hiddenOffer" runat="server" />
    <asp:ModalPopupExtender ID="mpeCalender" runat="server" TargetControlID="hiddenOffer"
        PopupControlID="pnlCalender" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlCalender" CssClass="pnlPopUp" runat="server">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button3"></asp:Button>
            <span>
                <%=MyContext.PageData.PageTitle%></span>
        </div>
        <div class="content">
            <div class="form" style="width: 200px;">

                <asp:ABFGridView runat="server" ID="gvItemsInvoice" GridViewStyle="BlueStyle" DataKeyNames="ID">
                    <Columns>
                        <asp:BoundField DataField="Serial" HeaderText="<%$Resources:Labels,Serial %>" />
                        <asp:TemplateField   HeaderText="الانتقال">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkButtonMoveInvoice" CommandArgument='<%#Eval("ID") %>' OnClick="lnkButtonMoveInvoice_Click" runat="server">الانتقال</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>

            </div>
        </div>

        <div class="btnDiv">

            <asp:Button ID="btnAddOkOffer" CssClass="button default_button" runat="server"
                Text="<%$ Resources:Labels, Ok %>" />
            <asp:Button ID="Button5" runat="server" CssClass="button"
                Text="<%$ Resources:Labels, Cancel %>" />
        </div>
    </asp:Panel>

</asp:Content>

