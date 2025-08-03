<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyDefault.aspx.cs" Inherits="Main_MyDefault" %>--%>

<%@ Page Async="true" Language="C#" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="MyDefault.aspx.cs" Inherits="Main_MyDefault" ValidateRequest="false"%>

<%@ Register TagName="Calc" Src="~/CustomControls/ucCalculatorMz1.ascx" TagPrefix="asp" %>

<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhml">
<head runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    
    <meta name="description" content="Responsive Bootstrap 4 Dashboard Template" />
    <meta name="author" content="BootstrapDash" />

    <title></title>

    <link href="<%$ Resources:Resource,MasterPageStyle%>" runat="server" rel="stylesheet" type="text/css" />
    <link rel="icon" href="../icon.ico" type="image/x-icon">

    <!-- Morris Charts CSS -->
    <%if (this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic)
        {%>
            <link href="../Styles/RTL_Theme/vendors/bower_components/morris.js/morris.css" rel="stylesheet" type="text/css" />
            <style>
                .az-sidebar{
                    right:0px;
                }
            </style>
        <%} 
    %>
    <%else
        {
            {%>
            <link href="../Styles/LTR_Theme/vendors/bower_components/morris.js/morris.css" rel="stylesheet" type="text/css" />
            <%}
        }
     %>
    

    <%--<link href="../Styles/Loading.css" rel="stylesheet" />--%>

    <link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, StyleInternel  %>" />

    <!-- vendor css -->
    <link href="../azia/lib/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
    <link href="../azia/lib/ionicons/css/ionicons.min.css" rel="stylesheet" />
    <link href="../azia/lib/typicons.font/typicons.css" rel="stylesheet" />
    <link href="../azia/lib/morris.js/morris.css" rel="stylesheet" />
    <link href="../azia/lib/flag-icon-css/css/flag-icon.min.css" rel="stylesheet" />
    <link href="../azia/lib/jqvmap/jqvmap.min.css" rel="stylesheet" />

    <!-- azia CSS -->
    <link rel="stylesheet" href="../azia/css/azia.css" />

    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />
    <%if (this.MyContext.CurrentCulture==XPRESS.Common.ABCulture.Arabic)
        {%>
            <link href="../Content/twitter-bootstrap/css/bootstrap-rtl.min.css" rel="stylesheet" />
        <%} 
    %>
    <script type="text/javascript" src="../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>


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

                //document.write('<link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, Style  %>" />');
                $("#lblTheme").text('داكن');
            }
            else {
                $("#defaultThem").removeClass('theme-1-active');
                $("#defaultThem").addClass('theme-4-active');
                $("#open_right_sidebar").removeClass('clsDark');
                $("#open_right_sidebar").addClass('clsLight');
                $(".page-wrapper").css('background', '#000');

                //document.write('<link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, StyleDark  %>" />');
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

                // closing of sidebar menu when clicking outside of it
                if (!$(e.target).closest('.az-header-menu-icon').length) {
                    var sidebarTarg = $(e.target).closest('.az-sidebar').length;
                    if (!sidebarTarg) {
                        $('body').removeClass('az-sidebar-show');
                    }
                }
            });

            $('#azSidebarToggle').on('click', function (e) {
                //console.log(hi);
                e.preventDefault();
                if (window.matchMedia('(min-width: 992px)').matches) {
                    $('body').toggleClass('az-sidebar-hide');
                } else {
                    $('body').toggleClass('az-sidebar-show');
                }
            });

            if (IsMobile() == true) {
                $('#azsTitle').on('click', function (e) {
                    //console.log(hi);
                    e.preventDefault();
                    if (window.matchMedia('(min-width: 992px)').matches) {
                        $('body').toggleClass('az-sidebar-hide');
                    } else {
                        $('body').toggleClass('az-sidebar-show');
                    }
                });
            }
            else {              
                $("#azsTitle").prop("href", "MyDefault.aspx");
            }
            
            /* ----------------------------------- */

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
            
            $.ajax({
                url: "MyDefault.aspx/GetCurrentNotificationDocs",
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
                        
                        html = html +`<div class="sl-item">
                        <a href="javascript:void(0)">
                            <a class="btn" style="background: #01c853 !important; height: 44px; float: right; text-align: center; width: 44px; display: table;">
                                <i class="fa fa-flag"></i>
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
            
            $.ajax({
                url: "MyDefault.aspx/GetListNotification",
                data: null,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (response) {                    
                    var html = '';
                    for (var i = 0; i < response.d.length; i++) {                        
                        html = html + `<div class="sl-item">
                        <a href="javascript:void(0)">
                            <a class="btn" style="background: #01c853 !important; height: 44px; float: right; text-align: center; width: 44px; display: table;">
                                <i class="fa fa-flag"></i>
                            </a>
                            <div class="sl-content">
                                <span style="font-size: 10px; float: right; margin-right: 10px;">${response.d[i].Title}</span>
                                <span style="font-size: 10px; margin-right: 10px; margin-left: 10px;">.</span>
                                <p class="truncate" style="font-size: 10px; margin-right: 10px; padding-right: 10px; display: flex; margin-left: 10px;">${response.d[i].Description}></p>
                            </div>
                        </a>
                    </div>
                    <hr />`;
                    }                    
                    $("#NotificationsDiv").html(html);
                },
                error: function (errr) {
                    alert(errr);
                    //$('#LoadingPanel').css('display', 'none');
                }
            });

            <%--var branchId =<%=MyLocalContext.UserProfile.Branch_ID%>;--%>
            var branchId = null;
            var link = '../api/General/GetCounters';
            if (branchId != null && branchId != '')
                link = "?Branch_ID=" + branchId;
            $.getJSON(link, function (response) {
                $('#lblNotificationNumber').text(response[0]);
                $('#lblCurrentDocs').text(response[1]);
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
            /*$(parent.document.getElementById("MainIframe")).css("min-height", 800 + 'px');
            $(parent.document.getElementById("MainIframe")).css("min-height", $(document).height() + 300 + 'px');*/

            //$(parent.document.getElementById("MainIframe")).css("min-height", 500 + 'px');
            //$(parent.document.getElementById("MainIframe")).css("min-height", $(document).height() + 100 + 'px');
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
            console.log("onloadMainIframe:" + h);
            if (oldh < 300)
                onloadMainIframeWait(5000);
        }

        function onloadMainIframeWait(time=10000) {            
            setTimeout(function () {
                var obj = document.getElementById("MainIframe");                
                $("#MainIframe").css("min-height", "0px");
                console.log("min:" + $("#MainIframe").css("min-height"));
                obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';                
                console.log("onloadMainIframeWait");
            }, time);
        }

        function showParents(roleId) {
            if (roleId != null && roleId != '' && roleId != "0") {                
                var parentId = $("#Link" + roleId).attr('data-parentid');
                $("#Node" + parentId).toggle();
                $('#SpanId' + roleId).toggleClass('fa fa-minus').toggleClass('fa fa-plus');
                console.log("parentId:" + parentId);
                showParents(parentId);
            }
        }

        function setForm(src) {
            $('html').scrollTop(0);
            document.getElementById('MainIframe').src = src;
            resizeIframeMy();                        
            if (IsMobile() == true) {
                $('body').removeClass('az-sidebar-show');
                $('body').addClass('az-sidebar-hide');
                /*if (window.matchMedia('(min-width: 992px)').matches) {
                    $('body').toggleClass('az-sidebar-hide');
                } else {
                    $('body').toggleClass('az-sidebar-show');
                }*/
            }
        }

        function clickRefresh() {
            document.getElementById('MainIframe').src = document.getElementById('MainIframe').src;
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

        /*function goToLink(link) {
            $('html').scrollTop(0);
            document.getElementById('MainIframe').src = link;
            resizeIframeMy();
            if (IsMobile() == true) {

            }
        }*/
    </script>

    <style>
        .full-width-drp {
            margin-left: 5px;
            margin-right: 5px;
        }

            .full-width-drp a {
                color: #5830C5;
            }

        .invisible {
            display:none;
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
            font-size: 14px;
            font-weight: normal;
        }

        /*.dx-datagrid-text-content {
            font-size: 12px;
            font-weight: normal;
        }*/
        .executeBtn {
            font-weight: bold;
        }        
    </style>    







</head>
<body>
    <form id="form1" runat="server">
        <!-- Calculator Modal -->
        <div class="modal fade" id="calculatorModal" tabindex="-1" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <%--<div class="modal-header">
                        <strong id="calculatorTitle" class="modal-title">تم الحفظ</strong>
                        <button type="button" class="close" data-bs-dismiss="modal">x</button>
                    </div>--%>
                    <div class="modal-body">
                        <asp:Calc runat="server" ID="Calc"/>
                    </div>
                    <div class="modal-footer">                        
                        <button type="button" id="btnCancelcalculator" class="btn btn-primary btn-lg" data-bs-dismiss="modal"><%=Resources.Labels.Close%></button>
                    </div>
                </div>
            </div>
        </div>

        <div class="az-body az-body-sidebar">
            <%--<div class="loadingiframe">
                <span class="WaitWord">
                    <div class="loading" style="color: black"><%= Resources.UserInfoMessages.PleaseWait %></div>
                </span>
            </div>

            <div class="loadingiframe">
                <span class="WaitWord">
                    <div class="base_ouro" style="z-index: 100; opacity: 1;">
                        <p class="xalicircle" style="display: block;">
                            <span class="ouro">
                                <span class="left">
                                    <span class="anim"></span>
                                </span>
                                <span class="right">
                                    <span class="anim"></span>
                                </span>
                            </span>
                            <span>
                                <%= Resources.UserInfoMessages.PleaseWait %>
                            </span>
                        </p>
                    </div>
                    <br />
                </span>
            </div>--%>

            <div class="az-sidebar">
                <div class="az-sidebar-header">
                    <div class="logo-wrap">
                        <a href="#" id="azsTitle">
                            <%--<img class="brand-img img-responsive" src="../Images/logo.png" alt="Auditor Erp" style="width: 200px; height: 60px;" />--%>
                            <span class="brand-text TitleText" style="font-size: 20px;font-weight:bold">
                                <%=Resources.Labels.AuditorERP %>
                            </span>
                        </a>
                    </div>
                </div>
                <!-- az-sidebar-loggedin #226bb2-->
                <div class="az-sidebar-body" id="myDiv" runat="server" style="background-color:#5830C5; text-align: right; border-radius: 10px; zoom: 80%;">
                    
                </div>                
            </div>
            <!-- az-sidebar -->

            <div class="az-content az-content-dashboard-two">
                <div class="az-header">
                    <div class="container-fluid">
                        <div class="az-header-left">
                            <a href="" id="azSidebarToggle" class="az-header-menu-icon"><span></span></a>
                        </div>
                        <!-- az-header-left -->
                        <div class="az-header-center">
                            <input type="search" class="col-md-6 form-control" placeholder="Search for anything...">
                            <%--<button class="btn"><i class="fa fa-search"></i></button>--%>
                        </div>
                        <!-- az-header-center -->
                        <div class="az-header-right">                                                     
                            <!-- az-header-message -->
                            <div class="dropdown az-header-notification" style="min-width:50px;width:50px;">
                                <a href="" class="notification new">
                                    <i class="typcn typcn-bell"><asp:Label ID="lblNotificationNumber" ClientIDMode="Static" runat="server" class="badge"></asp:Label></i>
                                </a>                                
                                <div class="dropdown-menu">
                                    <div class="az-dropdown-header mg-b-20 d-sm-none">
                                        <a href="" class="az-header-arrow"><i class="icon ion-md-arrow-back"></i></a>
                                    </div>
                                    <h6 class="az-notification-title"><%=Resources.Labels.Notifications %></h6>                                    
                                    <div class="az-notification-list">
                                        <div id="NotificationsDiv" style="overflow-y: scroll; max-height:400px;">
                                            
                                        </div>                                        
                                    </div>
                                    <!-- az-notification-list -->
                                    <div class="dropdown-footer"><a href="" onclick="$('html').scrollTop(0);document.getElementById('MainIframe').src='../Comp/Notifications.aspx'"><%=Resources.Labels.ReadAll %></a></div>
                                </div>
                                <!-- dropdown-menu -->
                            </div>
                              
                            <!-- az-header-message -->
                            <div class="dropdown az-header-notification" style="min-width:50px;width:50px;">
                                <a href="" class="notification new">
                                    <i class="typcn typcn-messages"><asp:Label ID="lblCurrentDocs" ClientIDMode="Static" runat="server" class="badge"></asp:Label></i>
                                </a>                                
                                <div class="dropdown-menu">
                                    <div class="az-dropdown-header mg-b-20 d-sm-none">
                                        <a href="" class="az-header-arrow"><i class="icon ion-md-arrow-back"><%=Resources.Labels.CurrentDocs %></i></a>
                                    </div>
                                    <h6 class="az-notification-title"><%=Resources.Labels.CurrentDocs %></h6>
                                    <div class="az-notification-list">
                                        <div id="CurrentDocsDiv" style="overflow-y: scroll; max-height:400px;">
                                            
                                        </div>
                                    </div>   
                                    <!-- az-notification-list -->
                                    <div class="dropdown-footer"><a href="" onclick="$('html').scrollTop(0);document.getElementById('MainIframe').src='../Comp/CurrenctDocs.aspx'"><%=Resources.Labels.ReadAll %></a></div>
                                </div>
                                <!-- dropdown-menu -->
                            </div>

                            <!-- az-header-notification -->
                            <div class="dropdown az-profile-menu">
                                <a href="" class="az-img-user">
                                    <%--<img src="../../azia/img/faces/face1.jpg" alt="">--%>
                                    <img src="../Images/prof.png" alt="">
                                </a>
                                <div class="dropdown-menu">
                                    <div class="az-dropdown-header d-sm-none">
                                        <a href="" class="az-header-arrow"><i class="icon ion-md-arrow-back"></i></a>
                                    </div>
                                    <div class="az-header-profile">
                                        <div class="az-img-user">
                                            <%--<img src="../../azia/img/faces/face1.jpg" alt="">--%>
                                            <img src="../Images/prof.png" alt="">
                                        </div>
                                        <!-- az-img-user -->
                                        <h6><asp:LoginName ID="LoginName2" runat="server" /></h6>
                                        <%--<span>Premium Member</span>--%>
                                    </div>
                                    <!-- az-header-profile -->

                                    <%--<a href="" class="dropdown-item"><i class="typcn typcn-user-outline"></i>My Profile</a>
                                    <a href="" class="dropdown-item"><i class="typcn typcn-edit"></i>Edit Profile</a>
                                    <a href="" class="dropdown-item"><i class="typcn typcn-time"></i>Activity Logs</a>
                                    <a href="" class="dropdown-item"><i class="typcn typcn-cog-outline"></i>Account Settings</a>--%>
                                    
                                    <a class="dropdown-item">
                                        <i class="fa fa-calendar" style="font-size: 16px;"></i><span>
                                            <asp:Label ID="lblFiscalYearStartDate" runat="server" Text=""></asp:Label>
                                        </span></a>

                                    <a class="dropdown-item" onclick="SetTheme()" data-toggle="tooltip" data-placement="bottom" data-original-title="<%=Resources.Labels.Theme %>" aria-describedby="tooltip502315">
                                        <i class="ti-palette" style="font-size: 16px;"></i>
                                        <span>
                                            <asp:Label ID="lblTheme" runat="server"></asp:Label>
                                        </span>
                                    </a>

                                    <a class="dropdown-item" onclick="document.getElementById('MainIframe').src='../Security/MyProfile.aspx'" href="javascript:void(0);">
                                        <i class="fa fa-user" style="font-size: 16px;"></i><span>
                                            <asp:LoginName ID="LoginName1" runat="server" />
                                        </span>
                                    </a>

                                    <a class="dropdown-item" href="javascript:void(0);">
                                        <i class="fa fa-database" style="font-size: 16px;"></i><span>
                                            <asp:Label ID="lblDatabaseName" runat="server" Text=""></asp:Label>
                                        </span></a>

                                    <a id="A1" runat="server" class="dropdown-item">
                                        <i class="fa fa-map-pin" style="font-size: 16px;"></i><span>
                                            <asp:Label ID="lblVersionDay" runat="server" Text=""></asp:Label>
                                        </span></a>

                                        <a id="lnkBranchName" runat="server" class="dropdown-item">
                                            <i class="fa fa-map-marker" style="font-size: 16px;"></i>
                                            <span><asp:Label ID="lblBranchName" runat="server" Text="sdfsfsd"></asp:Label></span>
                                        </a>

                                    <a class="dropdown-item" onclick="document.getElementById('MainIframe').src='../Security/MyProfile.aspx'" 
                                        href="javascript:void(0);">
                                        <asp:LinkButton ID="lnkChangeLang" CssClass="headerButton" OnClick="lnkChangeLang_Click" runat="server"
                                            style="margin-left:30px;padding-right:15px;">
                                              <i class="fa fa-language" style="font-size: 16px;"></i><span><%=Resources.Labels.LangName%></span></asp:LinkButton>
                                    </a>

                                    <asp:LinkButton ID="lnkLogOut" CssClass="dropdown-item" OnClick="lnkLogOut_Click" runat="server">
                                        <i class="fa fa-sign-out" style="font-size: 16px;"></i>
                                        <span><%=Resources.Labels.LogOut%></span>
                                    </asp:LinkButton>
                                </div>
                                <!-- dropdown-menu -->
                            </div>

                            <div class="full-width-drp d-none d-md-block" id="xsds">
                                <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="font-size: 15px; padding-top: 3px; color: #5830C5;"><i class="fa fa-calendar-plus-o"></i>
                                    <asp:Label ID="lblFiscalYearStartDate2" CssClass="awesomeDataBase" runat="server" Text=""></asp:Label></a>
                            </div>

                            <div class="full-width-drp d-none d-md-block" id="xsds">
                                <a href="#" class="dropdown-toggle" data-toggle="dropdown" style="font-size: 15px; padding-top: 3px; color: #5830C5;"><i class="fa fa-database"></i>
                                    <asp:Label ID="lblDatabaseName2" CssClass="awesome" runat="server" Text=""></asp:Label></a>
                            </div>

                            <div class="full-width-drp" id="l" style="display:none">
                                <a href="#" style="font-size: 23px; padding-top: 3px; color: #5830C5;">
                                    <i data-toggle="tooltip" data-placement="bottom" data-original-title="تنشيط" onclick="clickRefresh()" aria-describedby="tooltip502315" class="fa fa-refresh"></i>
                                </a>
                            </div>

                            <div class="full-width-drp" id="launchIntoFullscreen">
                                <a href="#" data-toggle="dropdown" style="font-size: 20px; padding-top: 3px; color: #5830C5;">
                                    <i id="launchIntoFullscreenIcon" data-toggle="tooltip" data-placement="bottom" data-original-title="<%=Resources.Labels.FullScreenTooltip %>" aria-describedby="tooltip502315" class="fa fa-desktop"></i></a>
                            </div>
                            <div class="full-width-drp" id="launchIntoFullscreenExit" style="display: none">
                                <a href="#" data-toggle="dropdown" style="font-size: 20px; padding-top: 3px; color: #5830C5;">
                                    <i id="launchIntoFullscreenIcon" data-toggle="tooltip" data-placement="bottom" data-original-title="<%=Resources.Labels.FullScreenTooltip %>" aria-describedby="tooltip502315" class="fa fa-desktop"></i></a>
                            </div>

                            <div class="full-width-drp">
                                <a id="open_right_sidebar" href="#" onclick="showModal('#calculatorModal')"><i class="fa fa-gear" style="color: #5830C5;"></i></a>
                            </div>

                            <div class="full-width-drp">
                                <%--<a id="open_right_sidebar" href="#"><i class="fa fa-gear" ></i></a>--%>
                                <a href="#" id="azshortcutsDivToggle" class="az-header-menu-icon" style="color: #5830C5;"
                                    onclick="$('#<%=shortcutsDiv.ClientID%>').toggle();onloadMainIframeWait(1);"><span></span></a>
                            </div>                            
                            <!--Here-->
                        </div>
                        <!-- az-header-right -->
                    </div>
                    <!-- container -->
                </div>
                
                <!-- az-content-header -->
                <div class="contentDiv az-content-body">                    
                    <div class="wrapper theme-1-active pimary-color-red" id="masterId">
                        <div class="page-wrapper1" style="margin-right: 0!important; margin-left: 0!important; padding: 0px 0px!important; background-color: transparent">
                            <div id="MainTitle" style="display: none">
                                <div style="float: right;" class="PageTitle">
                                    <img width="32" height="32" src="" alt="" style="display: none;" id="pageImg" />
                                    <span id="lblPageTitle"></span>&nbsp;&nbsp;&nbsp;
                                </div>
                                <hr style="clear: both;" />
                            </div>
                            <div id="shortcutsDiv" runat="server" class="row" style="display:none;">
                                
                            </div>
                            <div class="IframeContainer">
                                <iframe id="MainIframe" name="MainIframe" scrolling="no" src="../Dashboards/FrmDashboard.aspx"
                                    frameborder="0" style="width: 100%;" onload="onloadMainIframe(this);"></iframe>
                            </div>
                            <%--<asp:UpdatePanel runat="server" ID="upnMainUpdatePanel" UpdateMode="Conditional" style="padding-top: 0px; padding-left: 0px; padding-right: 0;">
                                <ContentTemplate>
                                    <asp:ContentPlaceHolder ID="cph" runat="server">
                                    </asp:ContentPlaceHolder>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="font-size: 35px;display:none" class="PageTitle">
                                <asp:HyperLink ID="lnkAttachments" NavigateUrl="javascript:;" runat="server" Text="<%$Resources:Labels,Attachments %>"></asp:HyperLink>
                            </div>--%>
                        </div>
                    </div>
                </div>

                <!-- az-content-body -->
                <div class="az-footer ht-40">
                    <div style="text-align:center">
                        <span id="LblCopyright">Copyright</span> © <span id="lblCopyrightYear">
                            <%=DateTime.Now.ToString("yyyy") %></span> - <a target="_blank" tabindex="-1" title="Xpress Erp" href="http://www.auditorerp.cloud"><span id="LblXpress">Auditor Erp</span></a>
                    </div>                  
                </div>
                <!-- az-footer -->
            </div>
            <!-- az-content -->
        </div>
    </form>    
</body>
</html>
