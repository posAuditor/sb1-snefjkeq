<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="StartScreen.aspx.cs" Inherits="Main_StartScreen" %>
<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/droidarabickufi.css" rel="stylesheet" />
    <style>
        .heading-bg {
            height: 0px !important;
            margin-bottom: 10px;
            padding: 5px 2px !important;
        }

        .MoreDetail {
            padding-top: 44px !important;
            color: white !important;
            /* font-weight: bold; */
        }


        .fontUsedDashBoard {
            font-family: "Droid Arabic Kufi",Tahoma, "Helvetica Neue",Arial,Helvetica,sans-serif !important;
            font-size: 16px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <p class="ConErr" runat="server" id="ConErr">
        <%=Resources.UserInfoMessages.ConErr %>
    </p>
     <asp:Favorit runat="server" ID="ucFavorit" />


    

    <asp:Panel runat="server" ID="StartScreen">


        <style>
            .sm-data-box-3 .easypiechart .percentVal {
                font-size: 30px;
                line-height: 30px;
                margin-top: 56px;
            }
        </style>

      <%--  <div class="row fontUsedDashBoard">
            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-red" style="background: #8f02fe!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="TheFund"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block">
                                                <%= Resources.Labels.TheFund %>    </span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-shopping-basket"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">

                                            <asp:HyperLink ID="lnkPund" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-yellow" style="background: #06c7ff!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="lblBank"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.Bank %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-bar-chart-o"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkBanks" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-green" style="background: #d9d33f!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="valueCustomer"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.Customersowe %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-credit-card"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkValueCustomer" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-blue" style="background: #ff885d!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="SuppliersPayables"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.SuppliersPayables %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 pt-25  data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-user"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkVendors" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row fontUsedDashBoard">

            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-red" style="background: #7aca33!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="nbrSales1Today"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.SalesToday %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-shopping-basket"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkSales" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-red" style="background: #ff3b3d!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="nbrSalesToday"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.PurchasesToday %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-shopping-basket"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkPurshase" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-red" style="background: #d907fe!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="Clientsintheservice"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.Clientsintheservice %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-shopping-basket"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkClientService" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view pa-0">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body pa-0">
                            <div class="sm-data-box bg-red" style="background: #feb028!important">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-xs-5 text-center pl-0 pr-0 data-wrap-left">
                                            <span class="txt-light block counter"><span class="counter-anim" id="venderintheservice"></span></span>
                                            <span class="weight-500 uppercase-font txt-light block"><%=Resources.Labels.VenderInTheService %></span>
                                        </div>
                                        <div class="col-xs-5 text-center  pl-0 pr-0 data-wrap-right">
                                            <i style="font-size: 60px; color: white;" class="fa fa-shopping-basket"></i>
                                        </div>
                                        <div class="col-xs-2 text-center  pl-0 pr-0 data-wrap-right MoreDetail">
                                            <asp:HyperLink ID="lnkVendorsServices" runat="server" CssClass="" Style="color: white" Text=" <%$ Resources:Labels,More %>"></asp:HyperLink>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>


        <div class="row fontUsedDashBoard">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body sm-data-box-1">
                            <span class="uppercase-font weight-500 font-14 block text-center txt-dark"><%=Resources.Labels.SalesTrafficIndicator %></span>
                            <canvas id="chart_2" height="250" style="max-height:346px!important"></canvas>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-3" style="display:none">
                <div class="panel panel-default card-view">
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body sm-data-box-1">
                            <span class="uppercase-font weight-500 font-14 block text-center txt-dark"><%=Resources.Labels.BestSellingProducts %></span>
                            <div class="cus-sat-stat weight-500 txt-success text-center mt-5">
                                <span class="counter-anim" id="BestSellingProducts"></span><span>%</span>
                            </div>
                            <div class="progress-anim mt-20">
                                <div class="progress">
                                    <div class="progress-bar progress-bar-success wow animated progress-animated" role="progressbar" id="bestSellingProductsProg" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                            </div>
                            <span class="pull-left inline-block capitalize-font txt-dark" id="nameItem"></span>
                            <span class="label label-warning pull-right" id="qtyIn"></span>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default card-view">
                    <div class="panel-heading">
                        <div class="">
                            <h6 class="uppercase-font weight-500 font-14 block text-center txt-dark"><%=Resources.Labels.ProductsAndCategories %>  </h6>
                        </div>
                        <div class="pull-right">
                            <a href="#" class="pull-left inline-block mr-15">
                                <i class="zmdi zmdi-download"></i>
                            </a>


                            <a href="#" class="pull-left inline-block full-screen mr-15">
                                <i class="zmdi zmdi-fullscreen"></i>
                            </a>


                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body">
                            <div id="cost">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6"  >
                <div class="panel panel-default card-view panel-refresh">
                    <div class="refresh-container">
                        <div class="la-anim-1"></div>
                    </div>
                    <div class="panel-heading">
                        <div class="">
                            <h6 class="uppercase-font weight-500 font-14 block text-center txt-dark">

                                <%=Resources.Labels.TopExpenses %>
                            </h6>
                        </div>
                        <div class="pull-right">

                            <a href="#" class="pull-left inline-block full-screen mr-15">
                                <i class="zmdi zmdi-fullscreen"></i>
                            </a>

                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body">
                            <div>
                                <canvas id="chart_6" height="151"></canvas>
                            </div>
                            <hr class="light-grey-hr row mt-10 mb-15" style="display: none" id="first1" />
                            <div class="label-chatrs" style="" id="first">
                                <div class="">
                                    <span class="clabels clabels-lg inline-block bg-blue mr-10 pull-left"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font pull-left"><span class="block font-15 weight-500 mb-5" id="firstPercName"></span><span class="block txt-grey"></span></span>
                                    <br />
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <hr class="light-grey-hr row mt-10 mb-15" style="" id="second1" />
                            <div class="label-chatrs" style="display: none" id="second">
                                <div class="">
                                    <span class="clabels clabels-lg inline-block bg-green mr-10 pull-left"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font pull-left"><span class="block font-15 weight-500 mb-5" id="secondPercName"></span><span class="block txt-grey"></span></span>
                                    <br />
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                            <hr class="light-grey-hr row mt-10 mb-15" style="" id="third1" />
                            <div class="label-chatrs" style="display: none" id="third">
                                <div class="">
                                    <span class="clabels clabels-lg inline-block bg-yellow mr-10 pull-left"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font pull-left"><span class="block font-15 weight-500 mb-5" id="thirdPercName"></span><span class="block txt-grey"></span></span>
                                    <br />
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row fontUsedDashBoard">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view panel-refresh">
                    <div class="refresh-container">
                        <div class="la-anim-1"></div>
                    </div>
                    <div class="panel-heading">
                        <div class="">
                            <h6 class="puppercase-font weight-500 font-14 block text-center txt-dark"><%=Resources.Labels.balanceAccount %>  </h6>
                        </div>
                        <div class="pull-right">
                            <a href="#" class="pull-left inline-block refresh mr-15">
                                <i class="zmdi zmdi-replay"></i>
                            </a>
                            <a href="#" class="pull-left inline-block full-screen mr-15">
                                <i class="zmdi zmdi-fullscreen"></i>
                            </a>

                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body row pa-0">
                            <div class="table-wrap">
                                <div class="table-responsive">
                                    <table class="table table-hover mb-0">
                                        <thead>
                                            <tr>
                                                <th><%=Resources.Labels.AccountName %></th>

                                                <th><%=Resources.Labels.AccountNumber %></th>

                                                <th><%=Resources.Labels.Balance %></th>
                                            </tr>
                                        </thead>
                                        <tbody id="Accounts">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                <div class="panel panel-default card-view panel-refresh">
                    <div class="refresh-container">
                        <div class="la-anim-1"></div>
                    </div>
                    <div class="panel-heading">
                        <div class="">
                            <h6 class="uppercase-font weight-500 font-14 block text-center txt-dark"><%=Resources.Labels.MovProj %>  </h6>
                        </div>
                        <div class="pull-right">
                            <a href="#" class="pull-left inline-block refresh mr-15">
                                <i class="zmdi zmdi-replay"></i>
                            </a>
                            <a href="#" class="pull-left inline-block full-screen mr-15">
                                <i class="zmdi zmdi-fullscreen"></i>
                            </a>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-wrapper collapse in">
                        <div class="panel-body">
                            <div>
                                <canvas id="chart_3" height="253"></canvas>
                            </div>
                            <div class="label-chatrs mt-30">
                                <div class="inline-block mr-15">
                                    <span class="clabels inline-block bg-yellow mr-5"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font">debit</span>
                                </div>
                                <div class="inline-block mr-15">
                                    <span class="clabels inline-block bg-red mr-5"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font">credit</span>
                                </div>

                                <div class="inline-block">
                                    <span class="clabels inline-block bg-green mr-5"></span>
                                    <span class="clabels-text font-12 inline-block txt-dark capitalize-font">balance</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row fontUsedDashBoard" style="display: none;">

            <div class="col-md-12 col-lg-12 col-xs-12">
                <div class="panel panel-default card-view">

                    <div class="panel-wrapper collapse in">
                        <div class="panel-body">


                            <div class="col-sm-2 ">
                                <div class="sm-data-box-3">

                                    <div class=" ">
                                        <div class=" ">
                                            <div class="col-sm-12 ">
                                                <span id="pie_chart_Invoices" class="easypiechart" data-percent="100">

                                                    <span class="percentVal block txt-dark weight-500"></span>
                                                    <span class="clsValue"></span>
                                                    <span class="block txt-success text-center">
                                                        <span class="weight-500"><%=Resources.Labels.InvoicesDashboard %></span>
                                                    </span>







                                                </span>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-2 col-xs-6" style="padding-top: 42px; text-align: center;">
                                <span class="pie" data-peity='{ "fill": ["#fec107", "#e91e63"]}' id="draft"></span>
                                <div id="draftValue"></div>
                                <div>متأخرة</div>
                            </div>
                            <div class="col-sm-2 col-xs-6" style="padding-top: 42px; text-align: center;">
                                <span class="pie" data-peity='{ "fill": ["#fec107", "#e91e63"]}' id="delivered">0/0</span>
                                <div id="deliveredValue"></div>
                                <div>تم شحنها</div>
                            </div>
                            <div class="col-sm-2 col-xs-6" style="padding-top: 42px; text-align: center;">
                                <span class="pie" data-peity='{ "fill": ["#fec107", "#e91e63"]}' id="approvel"></span>
                                <div id="approvelValue"></div>
                                <div>تم تسليمها</div>
                            </div>
                            <div class="col-sm-2 col-xs-6" style="padding-top: 42px; text-align: center;">
                                <span class="pie" data-peity='{ "fill": ["#fec107", "#e91e63"]}' id="return"></span>
                                <div id="returnValue"></div>
                                <div>تم إرجاعها</div>
                            </div>
                            <div class="col-sm-2 col-xs-6" style="padding-top: 42px; text-align: center;">
                                <span class="pie" data-peity='{ "fill": ["#fec107", "#e91e63"]}' id="cancel"></span>

                                <div id="cancelValue"></div>
                                <div>تم إلغائها</div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>


        </div>--%>



      <%--  <script src="../Styles/LTR_Theme/vendors/bower_components/jquery/dist/jquery.min.js"></script>
        <script src="../Styles/LTR_Theme/vendors/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

        <script src="../Styles/LTR_Theme/vendors/chart.js/Chart.min.js"></script>--%>
     <%--   <script src="../Scripts/DashBoardJs.js"></script>--%>

       <%-- <script src="../Styles/LTR_Theme/vendors/bower_components/peity/jquery.peity.min.js"></script>
        <script src="../Styles/LTR_Theme/dist/js/peity-data.js"></script>

        <script src="../Styles/LTR_Theme/vendors/bower_components/jquery.easy-pie-chart/dist/jquery.easypiechart.min.js"></script>


        <script src="../Styles/LTR_Theme/vendors/bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
        <script src="../Styles/LTR_Theme/dist/js/jquery.slimscroll.js"></script>
        <script src="../Styles/LTR_Theme/vendors/bower_components/moment/min/moment.min.js"></script>

        <script src="../Styles/LTR_Theme/vendors/bower_components/waypoints/lib/jquery.waypoints.min.js"></script>
        <script src="../Styles/LTR_Theme/vendors/bower_components/jquery.counterup/jquery.counterup.min.js"></script>



        <script src="../Styles/LTR_Theme/vendors/bower_components/morris.js/morris.min.js"></script>
        <script src="../Styles/LTR_Theme/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.js"></script>
        <script src="../Styles/LTR_Theme/vendors/bower_components/switchery/dist/switchery.min.js"></script>

        <!-- Owl JavaScript -->
        <script src="../Styles/LTR_Theme/vendors/bower_components/owl.carousel/dist/owl.carousel.min.js"></script>

        <script src="../Styles/LTR_Theme/dist/js/init.js"></script>

        <script src="../Styles/LTR_Theme/dist/js/widgets-data.js"></script>--%>

        <p class="ConErr" runat="server" id="ConErr2">
            <%=Resources.UserInfoMessages.ConErr %>
        </p>




<%--        <script>

            $(document).ready(function () {
                var obj = localStorage.getItem('T');
                if (obj == 1) {

                    $('head').append('<link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, Style  %>" />')


                }
                else {



                    $('head').append('<link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, StyleDark  %>" />')




                }
            });
        </script>--%>
    </asp:Panel>
</asp:Content>

