<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Main_Default" EnableEventValidation="false"
    ValidateRequest="false" %>

<%--<%@ Register TagName="Calc" Src="~/CustomControls/ucCalculatorMz1.ascx" TagPrefix="asp" %>

<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>--%>

<!doctype html>
<html lang="en" data-layout="vertical" data-topbar="light" data-sidebar="dark" data-sidebar-size="lg" data-sidebar-image="none" data-bs-theme="dark" data-body-image="img-1" data-preloader="disable">

<head runat="server">

    <title>الدقق السحابي</title>

    <%-- <link href="<%$ Resources:Resource,MasterPageStyle%>" runat="server" rel="stylesheet" type="text/css" />--%>
    <%-- <link rel="icon" href="../icon1.ico" type="image/x-icon">--%>



    <%-- <link href="../azia/lib/fontawesome-free/css/all.min.css" rel="stylesheet" />--%>
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
    <link href="../Styles/DevextremeStyle.css" rel="stylesheet" />
    <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    <link href="../azia/lib/ionicons/css/ionicons.min.css" rel="stylesheet" />
    <link href="../azia/lib/typicons.font/typicons.css" rel="stylesheet" />
    <link href="../azia/lib/morris.js/morris.css" rel="stylesheet" />
    <link href="../azia/lib/flag-icon-css/css/flag-icon.min.css" rel="stylesheet" />
    <link href="../azia/lib/jqvmap/jqvmap.min.css" rel="stylesheet" />


    <link rel="stylesheet" href="../azia/css/azia.css" />


    <link href="../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />
    <%--      <%if (this.MyContext.CurrentCulture == XPRESS.Common.ABCulture.Arabic)
      {%>
            <link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />
        <%}
    %>
    --%>

    <script type="text/javascript" src="../../Content/jquery-3.5.1.min.js"></script>


    <script type="text/javascript" src="../../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>

    <link href="../../Content/devextreme/dx.light.css" rel="stylesheet" />


    <script type="text/javascript" src="../../Content/devextreme/dx.all.js"></script>
    <script type="text/javascript" src="../../Content/devextreme/dx.aspnet.data.js"></script>

    <script src="../azia/js/azia.js"></script>

    <script src="../azia/lib/ionicons/ionicons.js"></script>
    <script src="../azia/lib/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="../azia/lib/raphael/raphael.min.js"></script>
    <script src="../azia/lib/morris.js/morris.min.js"></script>
    <script src="../azia/lib/jqvmap/jquery.vmap.min.js"></script>
    <script src="../azia/lib/jqvmap/maps/jquery.vmap.usa.js"></script>

    <script src="../Scripts/UtilityJs.js"></script>

    <script type="text/javascript">        


        var fiscalYearStart = '<%= MyLocalContext.FiscalYearStartDate.ToString("d/M/yyyy") %>';
        var fiscalYearEnd = '<%= MyLocalContext.FiscalYearEndDate.ToString("d/M/yyyy") %>';
        var CurrentLang = '<%= (int)MyLocalContext.CurrentCulture %>'
        var WinID = '<%= Guid.NewGuid().ToString() %>';
        var DatabaseName = '<%= new XpressDataContext().Connection.Database %>';
        var CurrentDocPath = "";
        var UserID = '<%=MyLocalContext.UserProfile.UserId %>';
        var UserName = '<%=MyLocalContext.UserProfile.UserName %>';
        var SessionID = '<%=Session.SessionID %>';
        var IsShowingMsg = false;
        var EnableDocumentLocks = '<%= XPRESS.Common.Settings.EnableDocumentLocks%>'.toLowerCase();

        localStorage.setItem('T', 1);
        var is_chrome;
        var is_explorer;
        var is_firefox;
        var is_safari;
        var is_Opera;

        var $ = jQuery.noConflict();
        $(document).ready(function () {
            is_chrome = navigator.userAgent.indexOf('Chrome') > -1;
            is_explorer = navigator.userAgent.indexOf('MSIE') > -1;
            is_firefox = navigator.userAgent.indexOf('Firefox') > -1;
            is_safari = navigator.userAgent.indexOf("Safari") > -1;
            is_Opera = navigator.userAgent.indexOf("Presto") > -1;

            var obj = localStorage.getItem('T');

            if (obj == 1) {
                $("#defaultThem").removeClass('theme-4-active');
                $("#defaultThem").addClass('theme-1-active');
                $("#open_right_sidebar").addClass('clsDark');
                $("#open_right_sidebar").removeClass('clsLight');
                $(".page-wrapper").css('background', '#f7f7f7');


                $("#lblTheme").text('داكن');
            }
            else {
                $("#defaultThem").removeClass('theme-1-active');
                $("#defaultThem").addClass('theme-4-active');
                $("#open_right_sidebar").removeClass('clsDark');
                $("#open_right_sidebar").addClass('clsLight');
                $(".page-wrapper").css('background', '#000');


                $("#lblTheme").text('فاتح');
            }


            'use strict'
            $('.az-sidebar .with-sub').on('click', function (e) {
                e.preventDefault();
                $(this).parent().toggleClass('show');
                $(this).parent().siblings().removeClass('show');
            })

            $(document).on('click touchstart', function (e) {
                e.stopPropagation();


                if (!$(e.target).closest('.az-header-menu-icon').length) {
                    var sidebarTarg = $(e.target).closest('.az-sidebar').length;
                    if (!sidebarTarg) {
                        $('body').removeClass('az-sidebar-show');
                    }
                }
            });

            $('#azSidebarToggle').on('click', function (e) {
                e.preventDefault();
                if (window.matchMedia('(min-width: 992px)').matches) {
                    $('body').toggleClass('az-sidebar-hide');
                } else {
                    $('body').toggleClass('az-sidebar-show');
                }
            });

            if (IsMobile() == true) {
                $('#azsTitle').on('click', function (e) {
                    e.preventDefault();
                    if (window.matchMedia('(min-width: 992px)').matches) {
                        $('body').toggleClass('az-sidebar-hide');
                    } else {
                        $('body').toggleClass('az-sidebar-show');
                    }
                });
            }
            else {
                $("#azsTitle").prop("href", "Default.aspx");
            }



            $("#launchIntoFullscreen").click(function () {
                launchIntoFullscreen(document.documentElement);
                $("#launchIntoFullscreen").css("display", "none");
                $("#launchIntoFullscreenExit").css("display", "block");

            });

            $("#launchIntoFullscreenExit").click(function () {
                exitFullscreen();
                $("#launchIntoFullscreenExit").css("display", "none");
                $("#launchIntoFullscreen").css("display", "block");
            });







            $.getJSON("../../api/Settings/LoadChatOfAccountsSettings",
                function (response) {

                    localStorage.setItem("Settings", JSON.stringify(response));

                });

            $.getJSON("../../api/Settings/LoadSettings",
                function (response) {

                    localStorage.setItem("LoadSettings", JSON.stringify(response));

                });



            $.getJSON("../../api/ChartOfAccount/GetChartOfAccountsException", { 'contextKey': <%=this.GetCurrentCulture()%> }, function (response) {
                localStorage.setItem("ChartOfAccountsException", JSON.stringify(response));
            });





            var branchId = null;
            var link = '../api/General/GetCounters';
            if (branchId != null && branchId != '')
                link = "?Branch_ID=" + branchId;
            $.getJSON(link, function (response) {
                $('#lblNotificationNumber').text(response[0]);
                $('#lblCurrentDocs').text(response[1]);
            });


            var $loading = $('.loadingiframe').hide();
            $(document)
                .ajaxStart(function () {
                    $loading.show();
                    //console.log("ajaxStart:");
                })
                .ajaxStop(function () {
                    $loading.hide();
                    //console.log("ajaxStop:");
                })
                .ajaxComplete(function () {
                    $loading.hide();
                    //console.log("ajaxComplete:");
                })
                .ajaxError(function () {
                    $loading.hide();
                    //console.log("ajaxError:");
                })
                .ajaxSuccess(function () {
                    $loading.hide();
                    //console.log("ajaxSuccess:");
                });

            var obj = localStorage.getItem('T');
            if (obj == 1) {
                $("#masterId").removeClass('theme-4-active');
                $("#masterId").addClass('theme-1-active');
                $(".page-wrapper").css('background', '#f7f7f7');
            }
            else {
                $("#masterId").removeClass('theme-1-active');
                $("#masterId").addClass('theme-4-active');
                $(".page-wrapper").css('background', '#000');
            }

            $("#form1").on("change keydown", "[date='true']", function (event) {
                if (event.type == "keydown" && event.keyCode != 13) return true;
                if (!ValidateFiscalYear(this.value)) {
                    this.value = "";
                    ShowMessageWithSingleButton('<%=  this.MyContext.PageData.PageTitle %>', '<%= Resources.UserInfoMessages.DateOutsideFiscalYear %>', "");
                    event.preventDefault();
                    event.stopPropagation();
                    return false;
                }
                return true;
            });

        });

        function IsMobile() {
            if (navigator.userAgent.match(/Android/i)
                || navigator.userAgent.match(/webOS/i)
                || navigator.userAgent.match(/iPhone/i)
                || navigator.userAgent.match(/iPad/i)
                || navigator.userAgent.match(/iPod/i)
                || navigator.userAgent.match(/BlackBerry/i)
                || navigator.userAgent.match(/Windows Phone/i)
            ) {
                return true;
            }
            else {
                return false;
            }
        }

        function resizeIframeMy() {
            window.parent.SavedScrollTop = window.parent.pageYOffset;

            $(parent.document.getElementById("MainIframe")).css("height", '600px');

            $(".close-btn").attr('tabindex', -1);
            $(".grd,.grid").parent().css("overflow", "auto");
            if (IsMobile() && is_firefox) $(".grd,.grid").addClass("grid_mobile");
            $("html,body", window.parent.document).scrollTop(window.parent.SavedScrollTop);
        }

        function onloadMainIframe(obj) {
            var h = obj.contentWindow.document.documentElement.scrollHeight;
            var oldh = h;
            if (h < 300) {
                h = screen.height - 100;
            }
            obj.style.height = h + 'px';

            if (oldh < 300)
                onloadMainIframeWait(5000);
        }

        function onloadMainIframeWait(time = 10000) {
            setTimeout(function () {
                var obj = document.getElementById("MainIframe");
                $("#MainIframe").css("min-height", "0px");

                obj.style.height = (obj.contentWindow.document.documentElement.scrollHeight + 500) + 'px';

            }, time);
        }

        function showParents(roleId) {
            if (roleId != null && roleId != '' && roleId != "0") {
                var parentId = $("#Link" + roleId).attr('data-parentid');
                $("#Node" + parentId).toggle();
                $('#SpanId' + roleId).toggleClass('fa fa-minus').toggleClass('fa fa-plus');

                showParents(parentId);
            }
        }

        function setIFrameForm(src) {

            $('html').scrollTop(0);
            document.getElementById('MainIframe').src = 'about:blank';
            $("#PageContainerDiv").html(null);
            $("#PageContainerDiv").fadeOut();
            $("#IframeDiv").fadeIn();
            document.getElementById('MainIframe').src = src;
            resizeIframeMy();
            if (IsMobile() == true) {
                $('body').removeClass('az-sidebar-show');
                $('body').addClass('az-sidebar-hide');

            }
        }

        function setDivForm(src) {

            $('html').scrollTop(0);
            document.getElementById('MainIframe').src = 'about:blank';
            $("#PageContainerDiv").html(null);
            $("#IframeDiv").fadeOut();
            $('#PageContainerDiv').load(src);
            $("#PageContainerDiv").fadeIn();

            if (IsMobile() == true) {
                $('body').removeClass('az-sidebar-show');
                $('body').addClass('az-sidebar-hide');

            }
        }

        function clickRefresh() {

        }

        function showModal(selector) {
            $(selector).modal('show');
        }

        function hideModal(selector) {
            $(selector).modal('hide');
        }

        function SetTheme() {
            var obj = localStorage.getItem('T');
            if (obj == null) {
                localStorage.setItem('T', 1);
            }
            if (obj == 1) {
                localStorage.setItem('T', 4);
            }
            else
                localStorage.setItem('T', 1);
            window.location ='<%=XPRESS.Common.PageLinks.MainPage.Replace("~/Main/",string.Empty)%>';
        }

        $(window).scroll(function () {
            //$(".contentDiv").stop().animate({ "marginTop": ($(window).scrollTop()) + "px", "marginLeft": ($(window).scrollLeft()) + "px" }, "slow");                        
        });


        //$.ajax({
        //    url: "Default.aspx/GetCurrentNotificationDocs",
        //    data: null,
        //    type: "POST",
        //    dataType: "json",
        //    contentType: "application/json; charset=utf-8",
        //    success: function (response) {
        //        var html = '';
        //        for (var i = 0; i < response.d.length; i++) {
        //            var pageLink = response.d[i].PageUrl;
        //            var pageName = response.d[i].PageName;
        //            var total = response.d[i].Total;







        //            var s = ' <div class="text-reset notification-item d-block dropdown-item position-relative"> <div class="d-flex"> <div class="avatar-xs me-3 flex-shrink-0"><span class="avatar-title bg-info-subtle text-info rounded-circle fs-16"><i class="bx bx-badge-check"></i></span></div> <div class="flex-grow-1"><p class="mb-0 fs-11 fw-medium text-uppercase text-muted"><span><i class="mdi mdi-clock-outline"></i> ' + pageName + '   </span></p></div>  </div>                    </div>';
        //            html = html + s;




        //            //    html = html + '<div class="sl-item">
        //            //    <a href="javascript:void(0)">
        //            //        <a class="btn" style="background: #01c853 !important; height: 44px; float: right; text-align: center; width: 44px; display: table;">
        //            //            <i class="fa fa-flag"></i>
        //            //        </a>
        //            //        <div class="sl-content">
        //            //            <span style="font-size: 10px; float: right; margin-right: 10px;">
        //            //                <a href='${pageLink}'>${pageName}</a></span>
        //            //            <span style="font-size: 10px; float: left; margin-right: 10px;">${total}</span>
        //            //            <p class="truncate">...</p>
        //            //        </div>
        //            //    </a>
        //            //</div>
        //            //<hr class="light-grey-hr ma-0" />';
        //        }
        //        $("#CurrentDocsDiv").html(html);

        //    },
        //    error: function (errr) {
        //        alert(errr);
        //        //$('#LoadingPanel').css('display', 'none');
        //    }
        //});

        $.ajax({
            url: "Default.aspx/GetListNotification",
            data: null,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var html = '';
                for (var i = 0; i < response.d.length; i++) {
                    html = html + `<div class="sl-item">
                        <a href="javascript:void(0)">
                            <a class="btn" style="height: 10px; float: right; text-align: center; width: 10px;">
                                <i class="ri-attachment-fill"></i>
                            </a>
                            <div class="sl-content">
                                <span style="font-size: 10px; float: right; margin-right: 10px;">${response.d[i].Title}</span>
                              
                                <p class="truncate" style="font-size: 10px; margin-right: 10px; padding-right: 10px; display: flex; margin-left: 10px;">${response.d[i].Description}></p>
                            </div>
                        </a>
                    </div>
                    <hr />`;
                }
                $("#CurrentDocsDiv1").html(html);
            },
            error: function (errr) {
                alert(errr);
                //$('#LoadingPanel').css('display', 'none');
            }
        });

        $.ajax({
            url: "Default.aspx/GetCurrentNotificationDocs",
            data: null,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var html = '';
                for (var i = 0; i < response.d.length; i++) {
                    var pageLink = response.d[i].PageUrl;
                    var pageName = response.d[i].PageName;
                    var total = response.d[i].Total;

                    html = html + `<div class="sl-item">
                        <a href="javascript:void(0)">
                            <a class="btn" style="height: 10px; float: right; text-align: center; width: 10px;">
                              <i class="ri-attachment-fill"></i>
                          </a>
                            <div class="sl-content">
                                <span style="font-size: 10px; float: right; margin-right: 10px;">
                                    <a href='${pageLink}'>${pageName}</a></span>
                                <span style="font-size: 10px; float: left; margin-right: 10px;">${total}</span>
                                <p class="truncate">...</p>
                            </div>
                        </a>
                    </div>
                    <hr class="light-grey-hr ma-0" />`;
                }
                $("#CurrentDocsDiv").html(html);
            },
            error: function (errr) {
                alert(errr);
                //$('#LoadingPanel').css('display', 'none');
            }
        });

        var branchId = null;
        var link = '../api/General/GetCounters';
        if (branchId != null && branchId != '')
            link = "?Branch_ID=" + branchId;
        $.getJSON(link, function (response) {

            var not = parseInt(response[0]) + parseInt(response[1]);


            $('#Notif_1').text(not);
            //$('#lblCurrentDocs').text();
        });

    </script>

    <style>
        .zoom80 {
            zoom: 80%;
        }

        .InvoiceHeader {
            background: #a3a2a2;
        }

        .full-width-drp {
            margin-left: 5px;
            margin-right: 5px;
        }

            .full-width-drp a {
                color: #5830C5;
            }

        .invisible {
            display: none;
        }

        /*.contentDiv {            
            width: 84%;
            position: absolute;
            top: 70px;            
        }*/
    </style>

    <style>
        .notification {
            color: white;
            text-decoration: none;
            padding: 3px 5px;
            position: relative;
            display: inline-block;
            border-radius: 2px;
        }

            .notification:hover {
                /*background: red;*/
            }

            .notification .badge {
                position: absolute;
                top: -10px;
                right: -10px;
                padding: 5px 10px;
                border-radius: 50%;
                background: red;
                color: white;
                font-size: 8px;
            }
    </style>

    <style type="text/css">
        * {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif;
            font-size: 18px;
            font-weight: normal;
        }


        .executeBtn {
            font-weight: bold;
        }
    </style>


    <style>
        .dxGridParent {
            margin-right: 5px;
            padding-left: 20px;
            text-align: center;
        }

        /* Extra small devices (phones, 600px and down) */
        @media only screen and (max-width: 600px) {
            .dxGridParent {
                zoom: 125%;
            }
        }

        /* Small devices (portrait tablets and large phones, 600px and up) */
        @media only screen and (min-width: 600px) {
        }

        /* Medium devices (landscape tablets, 768px and up) */
        @media only screen and (min-width: 768px) {
        }

        /* Large devices (laptops/desktops, 992px and up) */
        @media only screen and (min-width: 992px) {
        }

        /* Extra large devices (large laptops and desktops, 1200px and up) */
        @media only screen and (min-width: 1200px) {
        }
    </style>


    <style type="text/css">
        .heading-bg {
            height: 0px;
            margin-bottom: 20px;
            padding: 0;
        }
    </style>

    <style>
        body {
            margin: 0;
            padding: 0;
            background: #262626;
        }

        .ring {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%,-50%);
            width: 150px;
            height: 150px;
            background: transparent;
            border: 3px solid #3c3c3c;
            border-radius: 50%;
            text-align: center;
            line-height: 150px;
            font-family: sans-serif;
            font-size: 15px;
            color: #fff000;
            letter-spacing: 4px;
            text-transform: uppercase;
            text-shadow: 0 0 10px #fff000;
            box-shadow: 0 0 20px rgba(0,0,0,.5);
        }

            .ring:before {
                content: '';
                position: absolute;
                top: -3px;
                left: -3px;
                width: 100%;
                height: 100%;
                border: 3px solid transparent;
                border-top: 3px solid #fff000;
                border-right: 3px solid #fff000;
                border-radius: 50%;
                animation: animateC 2s linear infinite;
            }

        .ringspan {
            display: block;
            position: absolute;
            top: calc(50% - 2px);
            left: 50%;
            width: 50%;
            height: 4px;
            background: transparent;
            transform-origin: left;
            animation: animate 2s linear infinite;
        }

            .ringspan:before {
                content: '';
                position: absolute;
                width: 16px;
                height: 16px;
                border-radius: 50%;
                background: #fff000;
                top: -6px;
                right: -8px;
                box-shadow: 0 0 20px #fff000;
            }

        @keyframes animate {
            0% {
                transform: rotate(45deg);
            }

            100% {
                transform: rotate(405deg);
            }
        }
    </style>

    <!-- Master Scripts -->
    <script type="text/javascript">
        var ekdArray = ["none", "row", "column"];
        function CheckIsSure(keys) {
            var isSure = ConfirmSure();
            if (isSure == true) {
                for (var i = 0; i < keys.length; i++) {
                    localStorage.removeItem(keys[i]);
                }
            }
            return isSure;
        }

        function dataDiffDays(date2, date1) {
            const date1utc = Date.UTC(date1.getFullYear(), date1.getMonth(), date1.getDate());
            const date2utc = Date.UTC(date2.getFullYear(), date2.getMonth(), date2.getDate());
            day = 1000 * 60 * 60 * 24;
            return (date2utc - date1utc) / day
        }

        function numbersInt() {
            $('.numbersInt').css('text-align', 'center');
            //جعل حقول الإدخال لا تدعم الحروف
            $('.numbersInt').on('keypress', function (event) {
                var regex = new RegExp("[0-9]");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });
        }

        function numbersDouble() {
            $('.numbersDouble').css('text-align', 'center');
            $(".numbersDouble").on({
                keyup: function () {
                    formatDouble($(this));
                },
                blur: function () {
                    formatDouble($(this), "blur");
                }
            });
        }

        function formatNumber(n) {
            return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        }

        function formatDouble(input, blur) {

            var input_val = input.val();


            if (input_val === "") { return; }


            var original_len = input_val.length;


            var caret_pos = input.prop("selectionStart");


            if (input_val.indexOf(".") >= 0) {

                var decimal_pos = input_val.indexOf(".");


                var left_side = input_val.substring(0, decimal_pos);
                var right_side = input_val.substring(decimal_pos);


                left_side = formatNumber(left_side);


                right_side = formatNumber(right_side);


                if (blur === "blur") {
                    right_side += "00";
                }


                right_side = right_side.substring(0, 2);


                input_val = left_side + "." + right_side;

            } else {

                input_val = formatNumber(input_val);
                input_val = input_val;


                if (blur === "blur") {
                    input_val += ".00";
                }
            }


            input.val(input_val);


            var updated_len = input_val.length;
            caret_pos = updated_len - original_len + caret_pos;
            input[0].setSelectionRange(caret_pos, caret_pos);
        }

        function setDatePicker() {
            $(".datepicker").datepicker({
                dateFormat: 'dd/mm/yy'
            });

            jQuery(function ($) {
                $.datepicker.regional['ar-YE'] = {
                    closeText: 'إغلاق',
                    prevText: '&#x3c;السابق',
                    nextText: 'التالي&#x3e;',
                    currentText: 'اليوم',
                    monthNames: ['يناير', 'فبراير', 'مارس', 'ابريل', 'مايو', 'يونيو',
                        'يوليو', 'اغسطس', 'سبتمبر', 'أكتوبر', 'نوفمبر', 'ديسمبر'],
                    monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                    dayNames: ['أحد', 'إثنين', 'ثلاثاء', 'أربعاء', 'خميس', 'جمعة', 'سبت'],
                    dayNamesShort: ['أحد', 'إثنين', 'ثلاثاء', 'أربعاء', 'خميس', 'جمعة', 'سبت'],
                    dayNamesMin: ['أحد', 'إثنين', 'ثلاثاء', 'أربعاء', 'خميس', 'جمعة', 'سبت'],
                    weekHeader: 'أسبوع',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 6,
                    isRTL: true,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                };
                $.datepicker.setDefaults($.datepicker.regional['ar-YE']);
            });
        }

        var ask = false;
        function goLink(link, roleId, Title, img) {
            if (ask) {
                $("#linkToGo").val(link);
                $("#leaveModal").modal("show");
            }
            else {
                $("#content").load(link);

                ask = true;
            }

            var x = window.matchMedia("(min-width : 760px)")
            myFunction(x)
            x.addListener(myFunction);
        }

        function titleTemplate(itemData, itemIndex, itemElement) {

            var tabPanel = $("#tabpanel-container").dxTabPanel("instance");
            if (tabPanel.option("items").length !== 1) {
                itemElement.append(
                    $("<i>")
                        .addClass("dx-icon")
                        .addClass("dx-icon-close")
                        .click(function () { closeButtonHandler(itemIndex); })
                );
            }
        }

        function closeButtonHandler(itemData) {
            var tabPanel = $("#tabpanel-container").dxTabPanel("instance");
            var index = itemData;
            var tabPanelItems = tabPanel.option("items");
            tabPanelItems.splice(index, 1);



            tabPanel.option("dataSource", tabPanelItems);
            if (index >= tabPanelItems.length && index > 0) tabPanel.option("selectedIndex", index - 1);
        }

        function myFunction(x) {
            if (x.matches) {
            } else {

            }
        }

        function setReadOnly(selector, value) {
            if (value == true) {
                $(selector).addClass("readonly");

            }
            else if (value == false) {
                $(selector).removeClass("readonly");

            }

            $(selector).find("input").prop('readonly', value);
            $(selector).find("input").prop('disabled', value);

            $(selector).find("select").prop('readonly', value);
            $(selector).find("select").prop('disabled', value);

            setTimeout(function () {
                $(selector + " .dxComb").each(function (index) {

                    if ($("#" + this.id).dxSelectBox('instance') != null)
                        $("#" + this.id).dxSelectBox('instance').option('disabled', value);
                });

                $(selector + " .dxDatePic").each(function (index) {
                    if ($("#" + this.id).dxDateBox('instance') != null)
                        $("#" + this.id).dxDateBox('instance').option('disabled', value);
                });
            }, 500);

            //if (value == true)
            //    $(selector).css('background-color', 'azure');
            //else $(selector).css('background-color', 'white');
        }

        function setEnabled(selector, value) {
            $(selector).prop('disabled', !value);
        }

        function showSuccessMessage(message, selector) {
            var time = 1500;
            $("#notifyview").html(message);
            $("#notifymsgdiv").removeClass("alert-danger");
            $("#notifymsgdiv").addClass("alert-success");
            $("#notifymsgdiv").fadeIn();
            $("#notifyModal").modal('show');
            $("#notifymsgdiv").fadeOut(time, function () {
                $("#notifyModal").modal('hide');
                if (selector != null && $(selector).length) {
                    $(selector).focus();
                }
            })
        }

        function showErrorMessage(message, selector) {
            var time = 1500;
            $("#notifyview").html(message);
            $("#notifymsgdiv").removeClass("alert-success");
            $("#notifymsgdiv").addClass("alert-danger");
            $("#notifymsgdiv").fadeIn();
            $("#notifyModal").modal('show');
            $("#notifymsgdiv").fadeOut(time, function () {
                $("#notifyModal").modal('hide');
                if (selector != null && $(selector).length) {
                    $(selector).focus();
                }
            });
        }

        function showSuccessPrint(message, title, id) {
            var time = 1500;
            $("#printview").html(message);
            $("#printTitle").html(title);
            $("#printmsgdiv").addClass("alert-success");
            $("#printmsgdiv").fadeIn();
            $("#printModal").modal('show');
            $("#printedId").val(id);
        }

        function OpenWindowWithPost(url, name, params) {
            var form = document.createElement("form");
            form.setAttribute("method", "post");
            form.setAttribute("action", url);
            form.setAttribute("target", name);
            for (var i in params) {
                if (params.hasOwnProperty(i)) {
                    var input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = i;
                    input.value = params[i];
                    form.appendChild(input);
                }
            }
            document.body.appendChild(form);
            window.open("post.htm", name, "width=" + screen.availWidth + ",height=" + screen.availHeight);
            form.submit();
            document.body.removeChild(form);
        }

        function showModal(selector) {
            $(selector).modal('show');
        }

        function hideModal(selector) {
            $(selector).modal('hide');
        }

        function loadDevextremeLocalization() {
            DevExpress.localization.loadMessages({
                "ar": {
                    "dxDataGrid-filterRowOperationContains": "يحتوي على",
                    "dxDataGrid-filterRowOperationNotContains": "لا يحتوي على",
                    "dxDataGrid-filterRowOperationStartsWith": "يبدأ بـ",
                    "dxDataGrid-filterRowOperationEndsWith": "ينتهي بـ",
                    "dxDataGrid-filterRowOperationEquals": "يساوي",
                    "dxDataGrid-filterRowOperationNotEquals": "لا يساوي",

                    "dxDataGrid-filterRowOperationBetween": "بين",
                    "dxDataGrid-filterRowOperationGreater": "أكبر من",
                    "dxDataGrid-filterRowOperationGreaterOrEquals": "أكبر من أو يساوي",
                    "dxDataGrid-filterRowOperationLess": "أصغر من",
                    "dxDataGrid-filterRowOperationLessOrEquals": "أصغر من أو يساوي",
                }
            });

            DevExpress.localization.locale(navigator.language);

        }

        function getTimeString() {
            const d = new Date();
            var tm = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "." + d.getMilliseconds();
            return tm;
        }

        function getDateString(dateObj) {
            let month = String(dateObj.getMonth() + 1).padStart(2, '0');
            let day = String(dateObj.getDate()).padStart(2, '0');
            let year = dateObj.getFullYear();
            let output = day + '/' + month + '/' + year;
            return output;
        }

        function validateEmail(inputText) {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var regex = new RegExp(mailformat);
            if (regex.test(inputText.value)) {
                return true;
            }
            else {
                return false;
            }
        }

        function validateMobile(inputText) {

            var mobileformat = "^((?:[+?0?0?966]+)(?:\s?\d{2})(?:\s?\d{7}))$";
            var regex = new RegExp(mobileformat);
            if (regex.test(inputText.value)) {
                return true;
            }
            else {
                return false;
            }
        }


        function SetPageTitle() {
            $(".treeview li a", window.parent.document).removeClass("ActiveMenuLink");
            $("#pageID_" + '<%=this.MyContext.PageData.PageID %>', window.parent.document).addClass("ActiveMenuLink");
            $("#pageImg").attr("src", '<%=  this.Page.ResolveClientUrl(this.MyContext.PageData.ImageUrl) %>');
            $("#lblPageTitle").text('<%=  this.MyContext.PageData.PageTitle %>');
        }

        function Display(id) {

            document.getElementById(id).scrollIntoView();

        }
    </script>


    <style>
        .cls {
            font-family: "Droid Arabic Kufi", tahoma !important;
        }
    </style>


    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
    <meta content="Themesbrand" name="author" />
    <!-- App favicon -->
    <link rel="shortcut icon" href="../icon1.ico">

    <script src="../assets/js/layout.js"></script>
    <link href="../assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/app.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/custom.min.css" rel="stylesheet" type="text/css" />

</head>






<body oncontextmenu="return true">

    <form id="form1" runat="server" class="cls">

        <div id="shortcutsDiv" runat="server" class="row" style="display: none;">
        </div>

        <div class="main-content">

            <div class="page-content">
                <div class="container-fluid">

                    <div class="IframeContainer">
                        <div id="IframeDiv" style="display: none">
                            <iframe id="MainIframe" name="MainIframe" scrolling="no" src="../Dashboards/FrmDashboard.aspx"
                                frameborder="0" style="width: 100%; height: 1600px" onload="onloadMainIframe(this);"></iframe>
                        </div>
                        <div id="PageContainerDiv"></div>
                    </div>

                </div>
            </div>
        </div>

        <!-- Begin page -->
        <div id="layout-wrapper">

            <header id="page-topbar">
                <div class="layout-width">
                    <div class="navbar-header">
                        <div class="d-flex">
                            <!-- LOGO -->
                            <div class="navbar-brand-box horizontal-logo">
                                <a href="index.html" class="logo logo-dark">
                                    <span class="logo-sm">
                                        <img src="../assets/images/logo-sm.png" alt="" height="22">
                                    </span>
                                    <span class="logo-lg">
                                        <img src="../assets/images/logo-dark.png" alt="" height="17">
                                    </span>
                                </a>

                                <a href="index.html" class="logo logo-light">
                                    <span class="logo-sm">
                                        <img src="../assets/images/logo-sm.png" alt="" height="22">
                                    </span>
                                    <span class="logo-lg">
                                        <img src="logo.png" alt="" height="17">
                                    </span>
                                </a>
                            </div>

                            <button type="button" class="btn btn-sm px-3 fs-16 header-item vertical-menu-btn topnav-hamburger" id="topnav-hamburger-icon">
                                <span class="hamburger-icon">
                                    <span></span>
                                    <span></span>
                                    <span></span>
                                </span>
                            </button>

                            <!-- App Search-->
                            <form class="app-search d-none d-md-block">
                                <div class="position-relative">
                                    <input type="text" class="form-control" placeholder="Search..." autocomplete="off" id="search-options" value="">
                                    <span class="mdi mdi-magnify search-widget-icon"></span>
                                    <span class="mdi mdi-close-circle search-widget-icon search-widget-icon-close d-none" id="search-close-options"></span>
                                </div>
                                <div class="dropdown-menu dropdown-menu-lg" id="search-dropdown">
                                    <div data-simplebar style="max-height: 320px;">
                                        <!-- item-->
                                        <div class="dropdown-header">
                                            <h6 class="text-overflow text-muted mb-0 text-uppercase">Recent Searches</h6>
                                        </div>

                                        <div class="dropdown-item bg-transparent text-wrap">
                                            <a href="index.html" class="btn btn-soft-primary btn-sm rounded-pill">how to setup <i class="mdi mdi-magnify ms-1"></i></a>
                                            <a href="index.html" class="btn btn-soft-primary btn-sm rounded-pill">buttons <i class="mdi mdi-magnify ms-1"></i></a>
                                        </div>
                                        <!-- item-->
                                        <div class="dropdown-header mt-2">
                                            <h6 class="text-overflow text-muted mb-1 text-uppercase">Pages</h6>
                                        </div>

                                        <!-- item-->
                                        <a href="javascript:void(0);" class="dropdown-item notify-item">
                                            <i class="ri-bubble-chart-line align-middle fs-18 text-muted me-2"></i>
                                            <span>Analytics Dashboard</span>
                                        </a>

                                        <!-- item-->
                                        <a href="javascript:void(0);" class="dropdown-item notify-item">
                                            <i class="ri-lifebuoy-line align-middle fs-18 text-muted me-2"></i>
                                            <span>Help Center</span>
                                        </a>

                                        <!-- item-->
                                        <a href="javascript:void(0);" class="dropdown-item notify-item">
                                            <i class="ri-user-settings-line align-middle fs-18 text-muted me-2"></i>
                                            <span>My account settings</span>
                                        </a>

                                        <!-- item-->
                                        <div class="dropdown-header mt-2">
                                            <h6 class="text-overflow text-muted mb-2 text-uppercase">Members</h6>
                                        </div>

                                        <div class="notification-list">
                                            <!-- item -->
                                            <a href="javascript:void(0);" class="dropdown-item notify-item py-2">
                                                <div class="d-flex">
                                                    <img src="../assets/images/users/avatar-2.jpg" class="me-3 rounded-circle avatar-xs" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="m-0">Angela Bernier</h6>
                                                        <span class="fs-11 mb-0 text-muted">Manager</span>
                                                    </div>
                                                </div>
                                            </a>
                                            <!-- item -->
                                            <a href="javascript:void(0);" class="dropdown-item notify-item py-2">
                                                <div class="d-flex">
                                                    <img src="../assets/images/users/avatar-3.jpg" class="me-3 rounded-circle avatar-xs" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="m-0">David Grasso</h6>
                                                        <span class="fs-11 mb-0 text-muted">Web Designer</span>
                                                    </div>
                                                </div>
                                            </a>
                                            <!-- item -->
                                            <a href="javascript:void(0);" class="dropdown-item notify-item py-2">
                                                <div class="d-flex">
                                                    <img src="../assets/images/users/avatar-5.jpg" class="me-3 rounded-circle avatar-xs" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="m-0">Mike Bunch</h6>
                                                        <span class="fs-11 mb-0 text-muted">React Developer</span>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                    </div>

                                    <div class="text-center pt-3 pb-1">
                                        <a href="pages-search-results.html" class="btn btn-primary btn-sm">View All Results <i class="ri-arrow-right-line ms-1"></i></a>
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="d-flex align-items-center">

                            <div class="dropdown d-md-none topbar-head-dropdown header-item">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" id="page-header-search-dropdown" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="bx bx-search fs-22"></i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-end p-0" aria-labelledby="page-header-search-dropdown">
                                    <form class="p-3">
                                        <div class="form-group m-0">
                                            <div class="input-group">
                                                <input type="text" class="form-control" placeholder="Search ..." aria-label="Recipient's username">
                                                <button class="btn btn-primary" type="submit"><i class="mdi mdi-magnify"></i></button>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>

                            <div class="dropdown ms-1 topbar-head-dropdown header-item">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <asp:ImageButton runat="server" ID="imgLanguageInit" ImageUrl="~/assets/images/flags/sa.svg" alt="Header Language" Height="20" class="rounded" />
                                    <%--<img id="header-lang-img" src="../assets/images/flags/us.svg" alt="Header Language" height="20" class="rounded">--%>
                                </button>
                                <div class="dropdown-menu dropdown-menu-end">
                                    <!-- item-->
                                    <a href="javascript:void(0);" class="dropdown-item notify-item language" data-lang="ar" title="Arabic">
                                        <asp:ImageButton runat="server" ID="Image1" ImageUrl="~/assets/images/flags/sa.svg" alt="Header Language" Height="20" class="rounded" OnClick="Image1_Click" />

                                    </a>

                                    <a href="javascript:void(0);" class="dropdown-item notify-item language" data-lang="us" title="English">
                                        <asp:ImageButton runat="server" ID="Image2" ImageUrl="~/assets/images/flags/us.svg" alt="Header Language" Height="20" class="rounded" OnClick="Image2_Click" />

                                    </a>
                                </div>
                            </div>

                            <div class="dropdown topbar-head-dropdown ms-1 header-item">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class='bx bx-category-alt fs-22'></i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-lg p-0 dropdown-menu-end">
                                    <div class="p-3 border-top-0 border-start-0 border-end-0 border-dashed border">
                                        <div class="row align-items-center">
                                            <div class="col">
                                                <h6 class="m-0 fw-semibold fs-15">معلومات البرنامج   </h6>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="p-2">
                                        <div class="row g-0">
                                            <div class="col">
                                                <a class="dropdown-icon-item" href="#!">
                                                    <img src="../assets/images/brands/github.png" alt="Github">
                                                    <span>
                                                        <asp:Label runat="server" ID="lblFiscalYearStartDate"></asp:Label>
                                                    </span>
                                                </a>
                                            </div>
                                            <div class="col">
                                                <a class="dropdown-icon-item" href="#!">
                                                    <img src="../assets/images/brands/bitbucket.png" alt="bitbucket">
                                                    <span>
                                                        <asp:Label runat="server" ID="lblFiscalYearEndDate"></asp:Label>
                                                    </span>
                                                </a>
                                            </div>
                                            <div class="col">
                                                <a class="dropdown-icon-item" onclick="document.getElementById('MainIframe').src='../Security/MyProfile.aspx'">
                                                    <img src="../assets/images/brands/dribbble.png" alt="dribbble">
                                                    <span>
                                                        <asp:Label runat="server" ID="lblProfile"></asp:Label>
                                                    </span>
                                                </a>
                                            </div>
                                        </div>

                                        <div class="row g-0">
                                            <div class="col">
                                                <a class="dropdown-icon-item" href="#!">
                                                    <img src="../assets/images/brands/dropbox.png" alt="dropbox">
                                                    <span>Dropbox</span>
                                                </a>
                                            </div>
                                            <div class="col">
                                                <a class="dropdown-icon-item" href="#!">
                                                    <img src="../assets/images/brands/mail_chimp.png" alt="mail_chimp">
                                                    <span>Mail Chimp</span>
                                                </a>
                                            </div>
                                            <div class="col">
                                                <a class="dropdown-icon-item" href="#!">
                                                    <img src="../assets/images/brands/slack.png" alt="slack">
                                                    <span>Slack</span>
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--  <div class="dropdown topbar-head-dropdown ms-1 header-item">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" id="page-header-cart-dropdown" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-haspopup="true" aria-expanded="false">
                                    <i class='bx bx-shopping-bag fs-22'></i>
                                    <span class="position-absolute topbar-badge cartitem-badge fs-10 translate-middle badge rounded-pill bg-info">5</span>
                                </button>
                                <div class="dropdown-menu dropdown-menu-xl dropdown-menu-end p-0 dropdown-menu-cart" aria-labelledby="page-header-cart-dropdown">
                                    <div class="p-3 border-top-0 border-start-0 border-end-0 border-dashed border">
                                        <div class="row align-items-center">
                                            <div class="col">
                                                <h6 class="m-0 fs-16 fw-semibold">My Cart</h6>
                                            </div>
                                            <div class="col-auto">
                                                <span class="badge bg-warning-subtle text-warning fs-13"><span class="cartitem-badge">7</span>
                                                    items</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div data-simplebar style="max-height: 300px;">
                                        <div class="p-2">
                                            <div class="text-center empty-cart" id="empty-cart">
                                                <div class="avatar-md mx-auto my-3">
                                                    <div class="avatar-title bg-info-subtle text-info fs-36 rounded-circle">
                                                        <i class='bx bx-cart'></i>
                                                    </div>
                                                </div>
                                                <h5 class="mb-3">Your Cart is Empty!</h5>
                                                <a href="apps-ecommerce-products.html" class="btn btn-success w-md mb-3">Shop Now</a>
                                            </div>
                                            <div class="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2">
                                                <div class="d-flex align-items-center">
                                                    <img src="../assets/images/products/img-1.png" class="me-3 rounded-circle avatar-sm p-2 bg-light" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="mt-0 mb-1 fs-14">
                                                            <a href="apps-ecommerce-product-details.html" class="text-reset">Branded
                                                    T-Shirts</a>
                                                        </h6>
                                                        <p class="mb-0 fs-12 text-muted">
                                                            Quantity: <span>10 x $32</span>
                                                        </p>
                                                    </div>
                                                    <div class="px-2">
                                                        <h5 class="m-0 fw-normal">$<span class="cart-item-price">320</span></h5>
                                                    </div>
                                                    <div class="ps-2">
                                                        <button type="button" class="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"><i class="ri-close-fill fs-16"></i></button>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2">
                                                <div class="d-flex align-items-center">
                                                    <img src="../assets/images/products/img-2.png" class="me-3 rounded-circle avatar-sm p-2 bg-light" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="mt-0 mb-1 fs-14">
                                                            <a href="apps-ecommerce-product-details.html" class="text-reset">Bentwood Chair</a>
                                                        </h6>
                                                        <p class="mb-0 fs-12 text-muted">
                                                            Quantity: <span>5 x $18</span>
                                                        </p>
                                                    </div>
                                                    <div class="px-2">
                                                        <h5 class="m-0 fw-normal">$<span class="cart-item-price">89</span></h5>
                                                    </div>
                                                    <div class="ps-2">
                                                        <button type="button" class="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"><i class="ri-close-fill fs-16"></i></button>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2">
                                                <div class="d-flex align-items-center">
                                                    <img src="../assets/images/products/img-3.png" class="me-3 rounded-circle avatar-sm p-2 bg-light" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="mt-0 mb-1 fs-14">
                                                            <a href="apps-ecommerce-product-details.html" class="text-reset">Borosil Paper Cup</a>
                                                        </h6>
                                                        <p class="mb-0 fs-12 text-muted">
                                                            Quantity: <span>3 x $250</span>
                                                        </p>
                                                    </div>
                                                    <div class="px-2">
                                                        <h5 class="m-0 fw-normal">$<span class="cart-item-price">750</span></h5>
                                                    </div>
                                                    <div class="ps-2">
                                                        <button type="button" class="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"><i class="ri-close-fill fs-16"></i></button>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2">
                                                <div class="d-flex align-items-center">
                                                    <img src="../assets/images/products/img-6.png" class="me-3 rounded-circle avatar-sm p-2 bg-light" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="mt-0 mb-1 fs-14">
                                                            <a href="apps-ecommerce-product-details.html" class="text-reset">Gray
                                                    Styled T-Shirt</a>
                                                        </h6>
                                                        <p class="mb-0 fs-12 text-muted">
                                                            Quantity: <span>1 x $1250</span>
                                                        </p>
                                                    </div>
                                                    <div class="px-2">
                                                        <h5 class="m-0 fw-normal">$ <span class="cart-item-price">1250</span></h5>
                                                    </div>
                                                    <div class="ps-2">
                                                        <button type="button" class="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"><i class="ri-close-fill fs-16"></i></button>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="d-block dropdown-item dropdown-item-cart text-wrap px-3 py-2">
                                                <div class="d-flex align-items-center">
                                                    <img src="../assets/images/products/img-5.png" class="me-3 rounded-circle avatar-sm p-2 bg-light" alt="user-pic">
                                                    <div class="flex-grow-1">
                                                        <h6 class="mt-0 mb-1 fs-14">
                                                            <a href="apps-ecommerce-product-details.html" class="text-reset">Stillbird Helmet</a>
                                                        </h6>
                                                        <p class="mb-0 fs-12 text-muted">
                                                            Quantity: <span>2 x $495</span>
                                                        </p>
                                                    </div>
                                                    <div class="px-2">
                                                        <h5 class="m-0 fw-normal">$<span class="cart-item-price">990</span></h5>
                                                    </div>
                                                    <div class="ps-2">
                                                        <button type="button" class="btn btn-icon btn-sm btn-ghost-secondary remove-item-btn"><i class="ri-close-fill fs-16"></i></button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="p-3 border-bottom-0 border-start-0 border-end-0 border-dashed border" id="checkout-elem">
                                        <div class="d-flex justify-content-between align-items-center pb-3">
                                            <h5 class="m-0 text-muted">Total:</h5>
                                            <div class="px-2">
                                                <h5 class="m-0" id="cart-item-total">$1258.58</h5>
                                            </div>
                                        </div>

                                        <a href="apps-ecommerce-checkout.html" class="btn btn-success text-center w-100">Checkout
                                        </a>
                                    </div>
                                </div>
                            </div>--%>

                            <div class="ms-1 header-item d-none d-sm-flex">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" data-toggle="fullscreen">
                                    <i class='bx bx-fullscreen fs-22'></i>
                                </button>
                            </div>

                            <div class="ms-1 header-item d-none d-sm-flex">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle light-dark-mode">
                                    <i class='bx bx-moon fs-22'></i>
                                </button>
                            </div>

                            <div class="dropdown topbar-head-dropdown ms-1 header-item" id="notificationDropdown">
                                <button type="button" class="btn btn-icon btn-topbar btn-ghost-secondary rounded-circle" id="page-header-notifications-dropdown" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-haspopup="true" aria-expanded="false">
                                    <i class='bx bx-bell fs-22'></i>
                                    <span class="position-absolute topbar-badge fs-10 translate-middle badge rounded-pill bg-danger" id="Notif_1">
                                        
                                        
                                       <%-- <span class="visually-hidden">unread messages</span>--%>



                                    </span>
                                </button>
                                <div class="dropdown-menu dropdown-menu-lg dropdown-menu-end p-0" aria-labelledby="page-header-notifications-dropdown">

                                    <div class="dropdown-head bg-primary bg-pattern rounded-top">
                                        <div class="p-3">
                                            <div class="row align-items-center">
                                                <div class="col">
                                                    <h6 class="m-0 fs-16 fw-semibold text-white"><%=Resources.Labels.Notifications %> </h6>
                                                </div>
                                                <div class="col-auto dropdown-tabs">
                                                    <span class="badge bg-light-subtle text-body fs-13">   </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="px-2 pt-2">
                                            <ul class="nav nav-tabs dropdown-tabs nav-tabs-custom" id="notificationItemsTab" role="tablist">

                                                <li class="nav-item active" aria-selected="true">
                                                    <a class="nav-link active" data-bs-toggle="tab" href="#messages-tab" role="tab" aria-selected="false"><%=Resources.Labels.Notifications %>
                                                    </a>
                                                </li>
                                                <li class="nav-item waves-effect waves-light">
                                                    <a class="nav-link" data-bs-toggle="tab" href="#alerts-tab" role="tab" aria-selected="false"><%=Resources.Labels.CurrentDocs %>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>

                                    </div>

                                    <div class="tab-content position-relative" id="notificationItemsTabContent">


                                        <div class="tab-pane py-2 active ps-2" id="messages-tab" role="tabpanel" aria-labelledby="messages-tab">
                                            <div style="max-height: 350px; overflow: auto;" class="pe-2" id="CurrentDocsDiv">
                                            </div>
                                            <div class="notification-actions" id="notification-actions" style="display: block;">
                                                <div class="my-3 text-center view-all" style="margin-top: 0 !important; margin-bottom: 0 !important;">
                                                    <button type="button" style="width: 100%" class="btn btn-soft-success waves-effect waves-light" onclick="$('html').scrollTop(0);document.getElementById('MainIframe').src='../Comp/Notifications.aspx'">
                                                        <%=Resources.Labels.ReadAll %>
                                                        <i class="ri-arrow-right-line align-middle"></i>
                                                    </button>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="tab-pane fade p-4" id="alerts-tab" role="tabpanel" aria-labelledby="alerts-tab">

                                            <div style="max-height: 350px; overflow: auto;" class="pe-2" id="CurrentDocsDiv1">
                                            </div>
                                            <div class="notification-actions" id="notification-actions1" style="display: block;">
                                                <div class="my-3 text-center view-all" style="margin-top: 0 !important; margin-bottom: 0 !important;">
                                                    <button type="button" style="width: 100%" class="btn btn-soft-success waves-effect waves-light" onclick="$('html').scrollTop(0);document.getElementById('MainIframe').src='../Comp/Notifications.aspx'">
                                                        <%=Resources.Labels.ReadAll %>
                                                        <i class="ri-arrow-right-line align-middle"></i>
                                                    </button>
                                                </div>
                                            </div>


                                        </div>


                                    </div>
                                </div>
                            </div>

                            <div class="dropdown ms-sm-3 header-item topbar-user">
                                <button type="button" class="btn" id="page-header-user-dropdown" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <span class="d-flex align-items-center">
                                        <img class="rounded-circle header-profile-user" src="../assets/images/users/avatar-1.jpg" alt="Header Avatar">
                                        <span class="text-start ms-xl-2"></span>
                                    </span>
                                </button>
                                <div class="dropdown-menu dropdown-menu-end">
                                    <!-- item-->
                                    <h6 class="dropdown-header">
                                        <asp:Label runat="server" ID="Label1"></asp:Label></h6>
                                    <a class="dropdown-item" onclick="document.getElementById('MainIframe').src='../Security/MyProfile.aspx'"><i class="mdi mdi-account-circle text-muted fs-16 align-middle me-1"></i><span class="align-middle">

                                        <asp:Label runat="server" ID="lblUserProfile"></asp:Label>
                                    </span></a>

                                    <%--<a class="dropdown-item" href="apps-chat.html"><i class="mdi mdi-message-text-outline text-muted fs-16 align-middle me-1"></i><span class="align-middle">Messages</span></a>--%>


                                    <a class="dropdown-item" href=""><i class="mdi mdi-calendar-check-outline text-muted fs-16 align-middle me-1"></i><span class="align-middle">
                                        <asp:Label runat="server" ID="lblDatabaseName2"></asp:Label>

                                    </span></a>



                                    <a class="dropdown-item" href="#"><i class="mdi mdi-lifebuoy text-muted fs-16 align-middle me-1"></i><span class="align-middle"><%=Resources.Labels.LangName%></span></a>
                                    <div class="dropdown-divider"></div>

                                    <a class="dropdown-item" href="pages-profile.html"><i class="mdi mdi-wallet text-muted fs-16 align-middle me-1"></i><span class="align-middle">
                                        <asp:Label ID="lblVersionDay" runat="server" Text=""></asp:Label></span></a>


                                    <a class="dropdown-item" href="pages-profile-settings.html">
                                        <span class="badge bg-success-subtle text-success mt-1 float-end">New</span><i class="mdi mdi-cog-outline text-muted fs-16 align-middle me-1"></i>
                                        <span class="align-middle">Settings</span>

                                    </a>

                                    <%-- <a class="dropdown-item" href="auth-lockscreen-basic.html"><i class="mdi mdi-lock text-muted fs-16 align-middle me-1"></i><span class="align-middle">Lock screen</span></a>--%>


                                    <asp:LinkButton class="dropdown-item" href="auth-logout-basic.html" ID="lnkLogOut" OnClick="lnkLogOut_Click" runat="server" Text="">
                                       <i class="mdi mdi-logout text-muted fs-16 align-middle me-1"></i>
                                       <span class="align-middle" data-key="t-logout"> <%=Resources.Labels.LogOut %>  </span> 
                                    </asp:LinkButton>




                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>

            <!-- removeNotificationModal -->
            <div id="removeNotificationModal" class="modal fade zoomIn" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" id="NotificationModalbtn-close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="mt-2 text-center">
                                <lord-icon src="https://cdn.lordicon.com/gsqxdxog.json" trigger="loop" colors="primary:#f7b84b,secondary:#f06548" style="width: 100px; height: 100px"></lord-icon>
                                <div class="mt-4 pt-2 fs-15 mx-4 mx-sm-5">
                                    <h4>Are you sure ?</h4>
                                    <p class="text-muted mx-4 mb-0">Are you sure you want to remove this Notification ?</p>
                                </div>
                            </div>
                            <div class="d-flex gap-2 justify-content-center mt-4 mb-2">
                                <button type="button" class="btn w-sm btn-light" data-bs-dismiss="modal">Close</button>
                                <button type="button" class="btn w-sm btn-danger" id="delete-notification">Yes, Delete It!</button>
                            </div>
                        </div>

                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
            <!-- ========== App Menu ========== -->
            <div class="app-menu navbar-menu">
                <!-- LOGO -->
                <div class="navbar-brand-box">
                    <!-- Dark Logo-->
                    <a href="index.html" class="logo logo-dark">
                        <span class="logo-sm">
                            <img src="../assets/images/logo-sm.png" alt="" height="22">
                        </span>
                        <span class="logo-lg">
                            <img src="../assets/images/logo-dark.png" alt="" height="17">
                        </span>
                    </a>
                    <!-- Light Logo-->
                    <a href="index.html" class="logo logo-light">
                        <span class="logo-sm">
                            <img src="../assets/images/logo-sm.png" alt="" height="22">
                        </span>
                        <span class="logo-lg">
                            <img src="logo.png" alt="" style="width: 219px; height: 56px;">
                        </span>
                    </a>
                    <button type="button" class="btn btn-sm p-0 fs-20 header-item float-end btn-vertical-sm-hover" id="vertical-hover">
                        <i class="ri-record-circle-line"></i>
                    </button>
                </div>

                <div id="scrollbar">
                    <div class="container-fluid">
                        <div id="two-column-menu">
                        </div>
                        <div id="myDiv" runat="server">
                        </div>






                    </div>
                    <!-- Sidebar -->
                </div>

                <div class="sidebar-background"></div>
            </div>
            <!-- Left Sidebar End -->
            <!-- Vertical Overlay-->
            <div class="vertical-overlay"></div>

            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            <div class="main-content">

                <div class="page-content">
                    <div class="container-fluid">
                    </div>
                    <!-- container-fluid -->
                </div>
                <!-- End Page-content -->


                <footer class="footer border-top">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-6">
                                <script>document.write(new Date().getFullYear())</script>
                                © Auditor.
                            </div>
                            <div class="col-sm-6">
                                <div class="text-sm-end d-none d-sm-block">
                                    Design & Develop by Dar-H
                                </div>
                            </div>
                        </div>
                    </div>
                </footer>
            </div>
            <!-- end main content-->

        </div>
        <!-- END layout-wrapper -->



        <!--start back-to-top-->
        <button onclick="topFunction()" class="btn btn-primary btn-icon" id="back-to-top">
            <i class="ri-arrow-up-line"></i>
        </button>
        <!--end back-to-top-->

        <!--preloader-->
        <div id="preloader">
            <div id="status">
                <div class="spinner-border text-primary avatar-sm" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>
        </div>

        <div class="customizer-setting d-none d-md-block">
            <div class="btn-primary btn-rounded shadow-lg btn btn-icon btn-lg p-2" data-bs-toggle="offcanvas" data-bs-target="#theme-settings-offcanvas" aria-controls="theme-settings-offcanvas">
                <i class='mdi mdi-spin mdi-cog-outline fs-22'></i>
            </div>
        </div>

        <!-- Theme Settings -->
        <div class="offcanvas offcanvas-end border-0" tabindex="-1" id="theme-settings-offcanvas">
            <div class="d-flex align-items-center bg-primary bg-gradient p-3 offcanvas-header">
                <h5 class="m-0 me-2 text-white">Theme Customizer</h5>

                <button type="button" class="btn-close btn-close-white ms-auto" id="customizerclose-btn" data-bs-dismiss="offcanvas" aria-label="Close"></button>
            </div>
            <div class="offcanvas-body p-0">
                <div data-simplebar class="h-100">
                    <div class="p-4">
                        <h6 class="mb-0 fw-semibold text-uppercase">Layout</h6>
                        <p class="text-muted">Choose your layout</p>

                        <div class="row gy-3">
                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input id="customizer-layout01" name="data-layout" type="radio" value="vertical" class="form-check-input">
                                    <label class="form-check-label p-0 avatar-md w-100" for="customizer-layout01">
                                        <span class="d-flex gap-1 h-100">
                                            <span class="flex-shrink-0">
                                                <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                    <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-grow-1">
                                                <span class="d-flex h-100 flex-column">
                                                    <span class="bg-light d-block p-1"></span>
                                                    <span class="bg-light d-block p-1 mt-auto"></span>
                                                </span>
                                            </span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Vertical</h5>
                            </div>
                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input id="customizer-layout02" name="data-layout" type="radio" value="horizontal" class="form-check-input">
                                    <label class="form-check-label p-0 avatar-md w-100" for="customizer-layout02">
                                        <span class="d-flex h-100 flex-column gap-1">
                                            <span class="bg-light d-flex p-1 gap-1 align-items-center">
                                                <span class="d-block p-1 bg-primary-subtle rounded me-1"></span>
                                                <span class="d-block p-1 pb-0 px-2 bg-primary-subtle ms-auto"></span>
                                                <span class="d-block p-1 pb-0 px-2 bg-primary-subtle"></span>
                                            </span>
                                            <span class="bg-light d-block p-1"></span>
                                            <span class="bg-light d-block p-1 mt-auto"></span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Horizontal</h5>
                            </div>
                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input id="customizer-layout03" name="data-layout" type="radio" value="twocolumn" class="form-check-input">
                                    <label class="form-check-label p-0 avatar-md w-100" for="customizer-layout03">
                                        <span class="d-flex gap-1 h-100">
                                            <span class="flex-shrink-0">
                                                <span class="bg-light d-flex h-100 flex-column gap-1">
                                                    <span class="d-block p-1 bg-primary-subtle mb-2"></span>
                                                    <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-shrink-0">
                                                <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-grow-1">
                                                <span class="d-flex h-100 flex-column">
                                                    <span class="bg-light d-block p-1"></span>
                                                    <span class="bg-light d-block p-1 mt-auto"></span>
                                                </span>
                                            </span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Two Column</h5>
                            </div>
                            <!-- end col -->

                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input id="customizer-layout04" name="data-layout" type="radio" value="semibox" class="form-check-input">
                                    <label class="form-check-label p-0 avatar-md w-100" for="customizer-layout04">
                                        <span class="d-flex gap-1 h-100">
                                            <span class="flex-shrink-0 p-1">
                                                <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                    <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-grow-1">
                                                <span class="d-flex h-100 flex-column pt-1 pe-2">
                                                    <span class="bg-light d-block p-1"></span>
                                                    <span class="bg-light d-block p-1 mt-auto"></span>
                                                </span>
                                            </span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Semi Box</h5>
                            </div>
                            <!-- end col -->
                        </div>

                        <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Color Scheme</h6>
                        <p class="text-muted">Choose Light or Dark Scheme.</p>

                        <div class="colorscheme-cardradio">
                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check card-radio">
                                        <input class="form-check-input" type="radio" name="data-bs-theme" id="layout-mode-light" value="light">
                                        <label class="form-check-label p-0 avatar-md w-100" for="layout-mode-light">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Light</h5>
                                </div>

                                <div class="col-4">
                                    <div class="form-check card-radio dark">
                                        <input class="form-check-input" type="radio" name="data-bs-theme" id="layout-mode-dark" value="dark">
                                        <label class="form-check-label p-0 avatar-md w-100 bg-dark" for="layout-mode-dark">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-white bg-opacity-10 d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-white bg-opacity-10 rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-white bg-opacity-10 d-block p-1"></span>
                                                        <span class="bg-white bg-opacity-10 d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Dark</h5>
                                </div>
                            </div>
                        </div>

                        <div id="sidebar-visibility">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Sidebar Visibility</h6>
                            <p class="text-muted">Choose show or Hidden sidebar.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-visibility" id="sidebar-visibility-show" value="show">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-visibility-show">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0 p-1">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column pt-1 pe-2">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Show</h5>
                                </div>
                                <div class="col-4">
                                    <div class="form-check card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-visibility" id="sidebar-visibility-hidden" value="hidden">
                                        <label class="form-check-label p-0 avatar-md w-100 px-2" for="sidebar-visibility-hidden">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column pt-1 px-2">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Hidden</h5>
                                </div>
                            </div>
                        </div>

                        <div id="layout-width">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Layout Width</h6>
                            <p class="text-muted">Choose Fluid or Boxed layout.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check card-radio">
                                        <input class="form-check-input" type="radio" name="data-layout-width" id="layout-width-fluid" value="fluid">
                                        <label class="form-check-label p-0 avatar-md w-100" for="layout-width-fluid">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Fluid</h5>
                                </div>
                                <div class="col-4">
                                    <div class="form-check card-radio">
                                        <input class="form-check-input" type="radio" name="data-layout-width" id="layout-width-boxed" value="boxed">
                                        <label class="form-check-label p-0 avatar-md w-100 px-2" for="layout-width-boxed">
                                            <span class="d-flex gap-1 h-100 border-start border-end">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Boxed</h5>
                                </div>
                            </div>
                        </div>

                        <div id="layout-position">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Layout Position</h6>
                            <p class="text-muted">Choose Fixed or Scrollable Layout Position.</p>

                            <div class="btn-group radio" role="group">
                                <input type="radio" class="btn-check" name="data-layout-position" id="layout-position-fixed" value="fixed">
                                <label class="btn btn-light w-sm" for="layout-position-fixed">Fixed</label>

                                <input type="radio" class="btn-check" name="data-layout-position" id="layout-position-scrollable" value="scrollable">
                                <label class="btn btn-light w-sm ms-0" for="layout-position-scrollable">Scrollable</label>
                            </div>
                        </div>
                        <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Topbar Color</h6>
                        <p class="text-muted">Choose Light or Dark Topbar Color.</p>

                        <div class="row">
                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input class="form-check-input" type="radio" name="data-topbar" id="topbar-color-light" value="light">
                                    <label class="form-check-label p-0 avatar-md w-100" for="topbar-color-light">
                                        <span class="d-flex gap-1 h-100">
                                            <span class="flex-shrink-0">
                                                <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                    <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-grow-1">
                                                <span class="d-flex h-100 flex-column">
                                                    <span class="bg-light d-block p-1"></span>
                                                    <span class="bg-light d-block p-1 mt-auto"></span>
                                                </span>
                                            </span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Light</h5>
                            </div>
                            <div class="col-4">
                                <div class="form-check card-radio">
                                    <input class="form-check-input" type="radio" name="data-topbar" id="topbar-color-dark" value="dark">
                                    <label class="form-check-label p-0 avatar-md w-100" for="topbar-color-dark">
                                        <span class="d-flex gap-1 h-100">
                                            <span class="flex-shrink-0">
                                                <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                    <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                </span>
                                            </span>
                                            <span class="flex-grow-1">
                                                <span class="d-flex h-100 flex-column">
                                                    <span class="bg-primary d-block p-1"></span>
                                                    <span class="bg-light d-block p-1 mt-auto"></span>
                                                </span>
                                            </span>
                                        </span>
                                    </label>
                                </div>
                                <h5 class="fs-13 text-center mt-2">Dark</h5>
                            </div>
                        </div>

                        <div id="sidebar-size">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Sidebar Size</h6>
                            <p class="text-muted">Choose a size of Sidebar.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-size" id="sidebar-size-default" value="lg">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-size-default">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Default</h5>
                                </div>

                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-size" id="sidebar-size-compact" value="md">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-size-compact">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Compact</h5>
                                </div>

                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-size" id="sidebar-size-small" value="sm">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-size-small">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1">
                                                        <span class="d-block p-1 bg-primary-subtle mb-2"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Small (Icon View)</h5>
                                </div>

                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar-size" id="sidebar-size-small-hover" value="sm-hover">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-size-small-hover">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1">
                                                        <span class="d-block p-1 bg-primary-subtle mb-2"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Small Hover View</h5>
                                </div>
                            </div>
                        </div>

                        <div id="sidebar-view">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Sidebar View</h6>
                            <p class="text-muted">Choose Default or Detached Sidebar view.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-layout-style" id="sidebar-view-default" value="default">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-view-default">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Default</h5>
                                </div>
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-layout-style" id="sidebar-view-detached" value="detached">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-view-detached">
                                            <span class="d-flex h-100 flex-column">
                                                <span class="bg-light d-flex p-1 gap-1 align-items-center px-2">
                                                    <span class="d-block p-1 bg-primary-subtle rounded me-1"></span>
                                                    <span class="d-block p-1 pb-0 px-2 bg-primary-subtle ms-auto"></span>
                                                    <span class="d-block p-1 pb-0 px-2 bg-primary-subtle"></span>
                                                </span>
                                                <span class="d-flex gap-1 h-100 p-1 px-2">
                                                    <span class="flex-shrink-0">
                                                        <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                            <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                            <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                            <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        </span>
                                                    </span>
                                                </span>
                                                <span class="bg-light d-block p-1 mt-auto px-2"></span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Detached</h5>
                                </div>
                            </div>
                        </div>
                        <div id="sidebar-color">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Sidebar Color</h6>
                            <p class="text-muted">Choose a color of Sidebar.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio" data-bs-toggle="collapse" data-bs-target="#collapseBgGradient.show">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-light" value="light">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-color-light">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-white border-end d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Light</h5>
                                </div>
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio" data-bs-toggle="collapse" data-bs-target="#collapseBgGradient.show">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-dark" value="dark">
                                        <label class="form-check-label p-0 avatar-md w-100" for="sidebar-color-dark">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-primary d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-white bg-opacity-10 rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-white bg-opacity-10"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Dark</h5>
                                </div>
                            </div>
                            <!-- end row -->

                            <div class="collapse" id="collapseBgGradient">
                                <div class="d-flex gap-2 flex-wrap img-switch p-2 px-3 bg-light rounded">

                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-gradient" value="gradient">
                                        <label class="form-check-label p-0 avatar-xs rounded-circle" for="sidebar-color-gradient">
                                            <span class="avatar-title rounded-circle bg-vertical-gradient"></span>
                                        </label>
                                    </div>
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-gradient-2" value="gradient-2">
                                        <label class="form-check-label p-0 avatar-xs rounded-circle" for="sidebar-color-gradient-2">
                                            <span class="avatar-title rounded-circle bg-vertical-gradient-2"></span>
                                        </label>
                                    </div>
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-gradient-3" value="gradient-3">
                                        <label class="form-check-label p-0 avatar-xs rounded-circle" for="sidebar-color-gradient-3">
                                            <span class="avatar-title rounded-circle bg-vertical-gradient-3"></span>
                                        </label>
                                    </div>
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-sidebar" id="sidebar-color-gradient-4" value="gradient-4">
                                        <label class="form-check-label p-0 avatar-xs rounded-circle" for="sidebar-color-gradient-4">
                                            <span class="avatar-title rounded-circle bg-vertical-gradient-4"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="preloader-menu">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Preloader</h6>
                            <p class="text-muted">Choose a preloader.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-preloader" id="preloader-view-custom" value="enable">
                                        <label class="form-check-label p-0 avatar-md w-100" for="preloader-view-custom">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                            <!-- <div id="preloader"> -->
                                            <div id="status" class="d-flex align-items-center justify-content-center">
                                                <div class="spinner-border text-primary avatar-xxs m-auto" role="status">
                                                    <span class="visually-hidden">Loading...</span>
                                                </div>
                                            </div>
                                            <!-- </div> -->
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Enable</h5>
                                </div>
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-preloader" id="preloader-view-none" value="disable">
                                        <label class="form-check-label p-0 avatar-md w-100" for="preloader-view-none">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Disable</h5>
                                </div>
                            </div>

                        </div>
                        <!-- end preloader-menu -->

                        <div id="body-img">
                            <h6 class="mt-4 mb-0 fw-semibold text-uppercase">Background Image</h6>
                            <p class="text-muted">Choose a body background image.</p>

                            <div class="row">
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-body-image" id="body-img-none" value="none">
                                        <label class="form-check-label p-0 avatar-md w-100" data-body-image="none" for="body-img-none">
                                            <span class="d-flex gap-1 h-100">
                                                <span class="flex-shrink-0">
                                                    <span class="bg-light d-flex h-100 flex-column gap-1 p-1">
                                                        <span class="d-block p-1 px-2 bg-primary-subtle rounded mb-2"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                        <span class="d-block p-1 px-2 pb-0 bg-primary-subtle"></span>
                                                    </span>
                                                </span>
                                                <span class="flex-grow-1">
                                                    <span class="d-flex h-100 flex-column">
                                                        <span class="bg-light d-block p-1"></span>
                                                        <span class="bg-light d-block p-1 mt-auto"></span>
                                                    </span>
                                                </span>
                                            </span>
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">None</h5>
                                </div>
                                <!-- end col -->
                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-body-image" id="body-img-one" value="img-1">
                                        <label class="form-check-label p-0 avatar-md w-100" data-body-image="img-1" for="body-img-one">
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">One</h5>
                                </div>
                                <!-- end col -->

                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-body-image" id="body-img-two" value="img-2">
                                        <label class="form-check-label p-0 avatar-md w-100" data-body-image="img-2" for="body-img-two">
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Two</h5>
                                </div>
                                <!-- end col -->

                                <div class="col-4">
                                    <div class="form-check sidebar-setting card-radio">
                                        <input class="form-check-input" type="radio" name="data-body-image" id="body-img-three" value="img-3">
                                        <label class="form-check-label p-0 avatar-md w-100" data-body-image="img-3" for="body-img-three">
                                        </label>
                                    </div>
                                    <h5 class="fs-13 text-center mt-2">Three</h5>
                                </div>
                                <!-- end col -->
                            </div>
                            <!-- end row -->
                        </div>
                    </div>
                </div>

            </div>
            <div class="offcanvas-footer border-top p-3 text-center">
                <div class="row">
                    <div class="col-6">
                        <button type="button" class="btn btn-light w-100" id="reset-layout">Reset</button>
                    </div>
                    <div class="col-6">
                        <a href="#" target="_blank" class="btn btn-primary w-100"></a>
                    </div>
                </div>
            </div>
        </div>

        <!-- JAVASCRIPT -->
        <%-- <script src="../assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>--%>
        <%--  <script src="../assets/libs/simplebar/simplebar.min.js"></script>--%>
        <%--  <script src="../assets/libs/node-waves/waves.min.js"></script>
        <script src="../assets/libs/feather-icons/feather.min.js"></script>
        <script src="../assets/js/pages/plugins/lord-icon-2.1.0.js"></script>
        <script src="../assets/js/plugins.js"></script>
        <script src="../assets/libs/prismjs/prism.js"></script>--%>
        <script src="../assets/js/app.js"></script>
    </form>
</body>
</html>

