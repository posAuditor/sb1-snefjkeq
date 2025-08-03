<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="NewInvoice.aspx.cs" Inherits="Sales_NewInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <title>Free Invoice Generator by Invoiced</title>


    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta name="description" content="Make beautiful invoices straight from your web browser. No account necessary.">

    <!-- Favicons -->

    <link rel="alternate" media="print" type="application/pdf" title="Click Download Invoice to print" href="https://d1m57r24ku159u.cloudfront.net/print.pdf">

    <link href="tempFiles/vendor.583690e0.css" type="text/css" rel="stylesheet">
    <link href="tempFiles/app.e3ed3638.css" type="text/css" rel="stylesheet">

    <script id="facebook-jssdk" src="tempFiles/sdk.js"></script>
    <script id="twitter-wjs" src="tempFiles/widgets.js"></script>
    <script  src="tempFiles/analytics.js"></script>
    <script type="text/javascript" src="tempFiles/jquery.min.js"></script>
    <script type="text/javascript" src="tempFiles/jquery-ui.min.js"></script>
    <script type="text/javascript" src="tempFiles/angular.min.js"></script>
    <script type="text/javascript" src="tempFiles/vendor.18f8c06b.js"></script>
    <script type="text/javascript" src="tempFiles/app.494f3c22.js"></script>

    <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } }(document, 'script', 'twitter-wjs');</script>

    <script type="text/javascript">
        window.defaultInvoice = { "header": "INVOICE", "to_title": "Bill To", "date_title": "Date", "payment_terms_title": "Payment Terms", "due_date_title": "Due Date", "purchase_order_title": "Purchase Order", "item_header": "Item", "quantity_header": "Quantity", "unit_cost_header": "Rate", "amount_header": "Amount", "subtotal_title": "Subtotal", "discounts_title": "Discounts", "tax_title": "Tax", "shipping_title": "Shipping", "total_title": "Total", "amount_paid_title": "Amount Paid", "balance_title": "Balance Due", "terms_title": "Terms", "notes_title": "Notes", "version": 4, "logo": null, "from": null, "to": null, "number": null, "purchase_order": null, "date": null, "payment_terms": null, "due_date": null, "items": [], "currency": "USD", "fields": { "discounts": false, "tax": "%", "shipping": false }, "discounts": 0, "tax": 0, "shipping": 0, "amount_paid": 0, "notes": null, "terms": null };
		</script>
    <style type="text/css">
        .fb_hidden {
            position: absolute;
            top: -10000px;
            z-index: 10001;
        }

        .fb_reposition {
            overflow: hidden;
            position: relative;
        }

        .fb_invisible {
            display: none;
        }

        .fb_reset {
            background: none;
            border: 0;
            border-spacing: 0;
            color: #000;
            cursor: auto;
            direction: ltr;
            font-family: "lucida grande", tahoma, verdana, arial, sans-serif;
            font-size: 11px;
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            letter-spacing: normal;
            line-height: 1;
            margin: 0;
            overflow: visible;
            padding: 0;
            text-align: left;
            text-decoration: none;
            text-indent: 0;
            text-shadow: none;
            text-transform: none;
            visibility: visible;
            white-space: normal;
            word-spacing: normal;
        }

            .fb_reset > div {
                overflow: hidden;
            }

        .fb_link img {
            border: none;
        }

        @keyframes fb_transform {
            from {
                opacity: 0;
                transform: scale(.95);
            }

            to {
                opacity: 1;
                transform: scale(1);
            }
        }

        .fb_animate {
            animation: fb_transform .3s forwards;
        }

        .fb_dialog {
            background: rgba(82, 82, 82, .7);
            position: absolute;
            top: -10000px;
            z-index: 10001;
        }

        .fb_reset .fb_dialog_legacy {
            overflow: visible;
        }

        .fb_dialog_advanced {
            padding: 10px;
            -moz-border-radius: 8px;
            -webkit-border-radius: 8px;
            border-radius: 8px;
        }

        .fb_dialog_content {
            background: #fff;
            color: #333;
        }

        .fb_dialog_close_icon {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/yq/r/IE9JII6Z1Ys.png) no-repeat scroll 0 0 transparent;
            cursor: pointer;
            display: block;
            height: 15px;
            position: absolute;
            right: 18px;
            top: 17px;
            width: 15px;
        }

        .fb_dialog_mobile .fb_dialog_close_icon {
            top: 5px;
            left: 5px;
            right: auto;
        }

        .fb_dialog_padding {
            background-color: transparent;
            position: absolute;
            width: 1px;
            z-index: -1;
        }

        .fb_dialog_close_icon:hover {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/yq/r/IE9JII6Z1Ys.png) no-repeat scroll 0 -15px transparent;
        }

        .fb_dialog_close_icon:active {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/yq/r/IE9JII6Z1Ys.png) no-repeat scroll 0 -30px transparent;
        }

        .fb_dialog_loader {
            background-color: #f6f7f9;
            border: 1px solid #606060;
            font-size: 24px;
            padding: 20px;
        }

        .fb_dialog_top_left, .fb_dialog_top_right, .fb_dialog_bottom_left, .fb_dialog_bottom_right {
            height: 10px;
            width: 10px;
            overflow: hidden;
            position: absolute;
        }

        .fb_dialog_top_left {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/ye/r/8YeTNIlTZjm.png) no-repeat 0 0;
            left: -10px;
            top: -10px;
        }

        .fb_dialog_top_right {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/ye/r/8YeTNIlTZjm.png) no-repeat 0 -10px;
            right: -10px;
            top: -10px;
        }

        .fb_dialog_bottom_left {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/ye/r/8YeTNIlTZjm.png) no-repeat 0 -20px;
            bottom: -10px;
            left: -10px;
        }

        .fb_dialog_bottom_right {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/ye/r/8YeTNIlTZjm.png) no-repeat 0 -30px;
            right: -10px;
            bottom: -10px;
        }

        .fb_dialog_vert_left, .fb_dialog_vert_right, .fb_dialog_horiz_top, .fb_dialog_horiz_bottom {
            position: absolute;
            background: #525252;
            filter: alpha(opacity=70);
            opacity: .7;
        }

        .fb_dialog_vert_left, .fb_dialog_vert_right {
            width: 10px;
            height: 100%;
        }

        .fb_dialog_vert_left {
            margin-left: -10px;
        }

        .fb_dialog_vert_right {
            right: 0;
            margin-right: -10px;
        }

        .fb_dialog_horiz_top, .fb_dialog_horiz_bottom {
            width: 100%;
            height: 10px;
        }

        .fb_dialog_horiz_top {
            margin-top: -10px;
        }

        .fb_dialog_horiz_bottom {
            bottom: 0;
            margin-bottom: -10px;
        }

        .fb_dialog_iframe {
            line-height: 0;
        }

        .fb_dialog_content .dialog_title {
            background: #6d84b4;
            border: 1px solid #365899;
            color: #fff;
            font-size: 14px;
            font-weight: bold;
            margin: 0;
        }

            .fb_dialog_content .dialog_title > span {
                background: url(https://static.xx.fbcdn.net/rsrc.php/v3/yd/r/Cou7n-nqK52.gif) no-repeat 5px 50%;
                float: left;
                padding: 5px 0 7px 26px;
            }

        body.fb_hidden {
            -webkit-transform: none;
            height: 100%;
            margin: 0;
            overflow: visible;
            position: absolute;
            top: -10000px;
            left: 0;
            width: 100%;
        }

        .fb_dialog.fb_dialog_mobile.loading {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/ya/r/3rhSv5V8j3o.gif) white no-repeat 50% 50%;
            min-height: 100%;
            min-width: 100%;
            overflow: hidden;
            position: absolute;
            top: 0;
            z-index: 10001;
        }

            .fb_dialog.fb_dialog_mobile.loading.centered {
                width: auto;
                height: auto;
                min-height: initial;
                min-width: initial;
                background: none;
            }

                .fb_dialog.fb_dialog_mobile.loading.centered #fb_dialog_loader_spinner {
                    width: 100%;
                }

                .fb_dialog.fb_dialog_mobile.loading.centered .fb_dialog_content {
                    background: none;
                }

        .loading.centered #fb_dialog_loader_close {
            color: #fff;
            display: block;
            padding-top: 20px;
            clear: both;
            font-size: 18px;
        }

        #fb-root #fb_dialog_ipad_overlay {
            background: rgba(0, 0, 0, .45);
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            top: 0;
            width: 100%;
            min-height: 100%;
            z-index: 10000;
        }

            #fb-root #fb_dialog_ipad_overlay.hidden {
                display: none;
            }

        .fb_dialog.fb_dialog_mobile.loading iframe {
            visibility: hidden;
        }

        .fb_dialog_content .dialog_header {
            -webkit-box-shadow: white 0 1px 1px -1px inset;
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#738ABA), to(#2C4987));
            border-bottom: 1px solid;
            border-color: #1d4088;
            color: #fff;
            font: 14px Helvetica, sans-serif;
            font-weight: bold;
            text-overflow: ellipsis;
            text-shadow: rgba(0, 30, 84, .296875) 0 -1px 0;
            vertical-align: middle;
            white-space: nowrap;
        }

            .fb_dialog_content .dialog_header table {
                -webkit-font-smoothing: subpixel-antialiased;
                height: 43px;
                width: 100%;
            }

            .fb_dialog_content .dialog_header td.header_left {
                font-size: 12px;
                padding-left: 5px;
                vertical-align: middle;
                width: 60px;
            }

            .fb_dialog_content .dialog_header td.header_right {
                font-size: 12px;
                padding-right: 5px;
                vertical-align: middle;
                width: 60px;
            }

        .fb_dialog_content .touchable_button {
            background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#4966A6), color-stop(.5, #355492), to(#2A4887));
            border: 1px solid #29487d;
            -webkit-background-clip: padding-box;
            -webkit-border-radius: 3px;
            -webkit-box-shadow: rgba(0, 0, 0, .117188) 0 1px 1px inset, rgba(255, 255, 255, .167969) 0 1px 0;
            display: inline-block;
            margin-top: 3px;
            max-width: 85px;
            line-height: 18px;
            padding: 4px 12px;
            position: relative;
        }

        .fb_dialog_content .dialog_header .touchable_button input {
            border: none;
            background: none;
            color: #fff;
            font: 12px Helvetica, sans-serif;
            font-weight: bold;
            margin: 2px -12px;
            padding: 2px 6px 3px 6px;
            text-shadow: rgba(0, 30, 84, .296875) 0 -1px 0;
        }

        .fb_dialog_content .dialog_header .header_center {
            color: #fff;
            font-size: 16px;
            font-weight: bold;
            line-height: 18px;
            text-align: center;
            vertical-align: middle;
        }

        .fb_dialog_content .dialog_content {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/y9/r/jKEcVPZFk-2.gif) no-repeat 50% 50%;
            border: 1px solid #555;
            border-bottom: 0;
            border-top: 0;
            height: 150px;
        }

        .fb_dialog_content .dialog_footer {
            background: #f6f7f9;
            border: 1px solid #555;
            border-top-color: #ccc;
            height: 40px;
        }

        #fb_dialog_loader_close {
            float: left;
        }

        .fb_dialog.fb_dialog_mobile .fb_dialog_close_button {
            text-shadow: rgba(0, 30, 84, .296875) 0 -1px 0;
        }

        .fb_dialog.fb_dialog_mobile .fb_dialog_close_icon {
            visibility: hidden;
        }

        #fb_dialog_loader_spinner {
            animation: rotateSpinner 1.2s linear infinite;
            background-color: transparent;
            background-image: url(https://static.xx.fbcdn.net/rsrc.php/v3/yD/r/t-wz8gw1xG1.png);
            background-repeat: no-repeat;
            background-position: 50% 50%;
            height: 24px;
            width: 24px;
        }

        @keyframes rotateSpinner {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        .fb_iframe_widget {
            display: inline-block;
            position: relative;
        }

            .fb_iframe_widget span {
                display: inline-block;
                position: relative;
                text-align: justify;
            }

            .fb_iframe_widget iframe {
                position: absolute;
            }

        .fb_iframe_widget_fluid_desktop, .fb_iframe_widget_fluid_desktop span, .fb_iframe_widget_fluid_desktop iframe {
            max-width: 100%;
        }

            .fb_iframe_widget_fluid_desktop iframe {
                min-width: 220px;
                position: relative;
            }

        .fb_iframe_widget_lift {
            z-index: 1;
        }

        .fb_hide_iframes iframe {
            position: relative;
            left: -10000px;
        }

        .fb_iframe_widget_loader {
            position: relative;
            display: inline-block;
        }

        .fb_iframe_widget_fluid {
            display: inline;
        }

            .fb_iframe_widget_fluid span {
                width: 100%;
            }

        .fb_iframe_widget_loader iframe {
            min-height: 32px;
            z-index: 2;
            zoom: 1;
        }

        .fb_iframe_widget_loader .FB_Loader {
            background: url(https://static.xx.fbcdn.net/rsrc.php/v3/y9/r/jKEcVPZFk-2.gif) no-repeat;
            height: 32px;
            width: 32px;
            margin-left: -16px;
            position: absolute;
            left: 50%;
            z-index: 4;
        }

        .fb_invisible_flow {
            display: inherit;
            height: 0;
            overflow-x: hidden;
            width: 0;
        }

        .fb_mobile_overlay_active {
            height: 100%;
            overflow: hidden;
            position: fixed;
            width: 100%;
        }

        .fb_shrink_active {
            opacity: 1;
            transform: scale(1, 1);
            transition-duration: 200ms;
            transition-timing-function: ease-out;
        }

            .fb_shrink_active:active {
                opacity: .5;
                transform: scale(.75, .75);
            }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <div id="fb-root" class=" fb_reset">
        <div style="position: absolute; top: -10000px; height: 0px; width: 0px;">
            <div>
                <iframe name="fb_xdm_frame_https" frameborder="0" allowtransparency="true" allowfullscreen="true" scrolling="no" id="fb_xdm_frame_https" aria-hidden="true" title="Facebook Cross Domain Communication Frame" tabindex="-1" src="tempFiles/lY4eZXm_YWu.html" style="border: none;"></iframe>
            </div>
        </div>
        <div style="position: absolute; top: -10000px; height: 0px; width: 0px;">
            <div></div>
        </div>
    </div>
    <script>(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=635157686501071";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>

    <div class="background"></div>



    <div class="container scrollable">


        <div class="">
            <!-- ngView:  -->
            <div ng-view="" class="ng-scope">
                <form name="invoiceForm" class="ng-scope ng-invalid ng-invalid-required ng-dirty ng-valid-parse ng-valid-number">
                    <div class="invoice-holder clearfix">



                        <div class="papers">
                            <div class="invoice">
                                <div class="two-col clearfix">
                                    <div class="title">
                                        <input class="form-control input-label ng-pristine ng-untouched ng-valid" ng-model="invoice.header" tabindex="10">
                                        <div class="subtitle">
                                            <div class="input-group">
                                                <span class="input-group-addon">#</span>
                                                <input class="form-control ng-pristine ng-untouched ng-valid" tabindex="11" ng-model="invoice.number">
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col">

   

                                        <div class="contact from">
                                            <div class="value">
                                                <div class="expandingText" style="position: relative;">
                                                    <textarea class="form-control ng-pristine ng-untouched expanding-init ng-invalid ng-invalid-required" placeholder="Who is this invoice from? (required)" ng-model="invoice.from" tabindex="15" required="" expanding-textarea="" style="position: absolute; height: 100%; resize: none;"></textarea>
                                                    <pre class="textareaClone" style="visibility: hidden; border: 1px solid; white-space: pre-wrap; line-height: 20px; text-decoration: none solid rgb(85, 85, 85); font-size: 14px; font-family: &quot; open sans&quot; , &quot; helvetica neue&quot; , helvetica, arial, sans-serif; text-align: start; word-break: normal; padding: 8px 12px; margin-bottom: 0px;"><div> </div></pre>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="contact to">
                                            <div class="field">
                                                <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.to_title" tabindex="16">
                                            </div>
                                            <div class="value">
                                                <div class="expandingText" style="position: relative;">
                                                    <textarea class="form-control ng-pristine ng-untouched expanding-init ng-invalid ng-invalid-required" placeholder="Who is this invoice to? (required)" ng-model="invoice.to" tabindex="17" required="" expanding-textarea="" style="position: absolute; height: 100%; resize: none;"></textarea>
                                                    <pre class="textareaClone" style="visibility: hidden; border: 1px solid; white-space: pre-wrap; line-height: 20px; text-decoration: none solid rgb(85, 85, 85); font-size: 14px; font-family: &quot; open sans&quot; , &quot; helvetica neue&quot; , helvetica, arial, sans-serif; text-align: start; word-break: normal; padding: 8px 12px; margin-bottom: 0px;"><div> </div></pre>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col fields">
                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.date_title" tabindex="19">
                                            <div class="input-group-addon">
                                                <input class="form-control datepicker date ng-pristine ng-untouched ng-valid hasDatepicker" ng-model="invoice.date" tabindex="20" date-picker="" id="dp1510886536569">
                                            </div>
                                        </div>

                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.payment_terms_title" tabindex="21">
                                            <div class="input-group-addon">
                                                <input class="form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.payment_terms" tabindex="22">
                                            </div>
                                        </div>

                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.due_date_title" tabindex="23">
                                            <div class="input-group-addon">
                                                <input class="form-control datepicker due-date ng-pristine ng-untouched ng-valid hasDatepicker" ng-model="invoice.due_date" tabindex="24" date-picker="" id="dp1510886536570">
                                            </div>
                                        </div>

                                        <div class="input-group addon-input highlight">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.balance_title" tabindex="25">
                                            <div class="input-group-addon value highlight ng-binding" ng-bind-html="invoice.balance|currencyFormat:invoice.currency"><span class="currency-amount"><span class="currency-symbol">$</span>130</span></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="items-holder">
                                    <div class="items-table-header">
                                        <div class="amount">
                                            <div class="field bordered">
                                                <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.amount_header" tabindex="29">
                                            </div>
                                        </div>
                                        <div class="unit_cost">
                                            <div class="field bordered">
                                                <input class="input-label form-control ng-pristine ng-untouched ng-valid ng-valid-required" required="" ng-model="invoice.unit_cost_header" tabindex="28">
                                            </div>
                                        </div>
                                        <div class="quantity">
                                            <div class="field bordered">
                                                <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.quantity_header" tabindex="27">
                                            </div>
                                        </div>
                                        <div class="name">
                                            <div class="field bordered">
                                                <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.item_header" tabindex="26">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="items-table">
                                        <!-- ngRepeat: (k, item) in invoice.items -->
                                        <div class="item-row ng-scope" ng-repeat="(k, item) in invoice.items">
                                            <div class="main-row">
                                                <div class="delete ng-hide" ng-hide="invoice.items.length==1">
                                                    <button type="button" class="delete-row" ng-click="deleteLineItem(item)" tabindex="35">
                                                        <i class="ion-close-round"></i>
                                                    </button>
                                                </div>
                                                <div class="amount value ng-binding" ng-bind-html="item.amount|currencyFormat:invoice.currency"><span class="currency-amount"><span class="currency-symbol">$</span>130</span></div>
                                                <div class="unit_cost">
                                                    <div ng-tabindex="30+8*k+3" ng-model="item.unit_cost" currency="invoice.currency" ng-required="true" input-amount="" class="ng-pristine ng-untouched ng-valid ng-isolate-scope ng-valid-required" required="required">
                                                        <div class="input-amount dropdown" ng-class="{&#39;is-percent&#39;:!!isRate,&#39;with-selector&#39;:hasSelector}">
                                                            <div class="addon currency-sign ng-binding">$</div>
                                                            <input class="form-control ng-valid ng-valid-required ng-dirty ng-valid-number ng-touched" type="number" step="any" autocomplete="off" tabindex="33" ng-required="ngRequired" ng-model="value" ng-change="change()" required="required"><div class="addon percent">%</div>
                                                            <div class="addon selector">
                                                                <button type="button" class="btn dropdown-toggle" aria-haspopup="true" aria-expanded="false"><span class="ion-chevron-down"></span></button>
                                                            </div>
                                                            <ul class="dropdown-menu dropdown-menu-right">
                                                                <li ng-class="{active:!isRate}" class="active"><a href="https://invoice-generator.com/#" ng-click="isRate=false;$event.preventDefault()" class="ng-binding">Flat ($)</a></li>
                                                                <li ng-class="{active:isRate}"><a href="https://invoice-generator.com/#" ng-click="isRate=true;$event.preventDefault()">Percent (%)</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="quantity">
                                                    <input type="number" step="any" class="form-control ng-valid ng-valid-required ng-dirty ng-valid-number ng-touched" autocomplete="off" ng-model="item.quantity" tabindex="32" placeholder="Quantity" required="">
                                                </div>
                                                <div class="name">
                                                    <div class="expandingText" style="position: relative;">
                                                        <textarea class="form-control ng-valid expanding-init ng-dirty ng-valid-parse ng-touched" ng-model="item.name" tabindex="31" placeholder="Description of service or product..." expanding-textarea="" style="position: absolute; height: 100%; resize: none;"></textarea>
                                                        <pre class="textareaClone" style="visibility: hidden; border: 1px solid; white-space: pre-wrap; line-height: 20px; text-decoration: none solid rgb(85, 85, 85); font-size: 14px; font-family: &quot; open sans&quot; , &quot; helvetica neue&quot; , helvetica, arial, sans-serif; word-break: normal; padding: 8px 12px; margin-bottom: 0px;"><div>wwqw </div></pre>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end ngRepeat: (k, item) in invoice.items -->
                                    </div>
                                    <div class="new-line">
                                        <button type="button" class="btn btn-primary" tabindex="1000" ng-click="addLineItem()">
                                            <span class="ion-plus-round"></span>
                                            Line Item
					
                                        </button>
                                    </div>
                                </div>

                                <div class="two-col clearfix rates">
                                    <div class="col col-offset">
                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.subtotal_title" tabindex="1001">
                                            <div class="input-group-addon value deleteable ng-binding" ng-bind-html="invoice.subtotal|currencyFormat:invoice.currency"><span class="currency-amount"><span class="currency-symbol">$</span>130</span></div>
                                        </div>

                                        <div class="input-group addon-input ng-hide" ng-show="!!invoice.fields.discounts">
                                            <div class="delete">
                                                <button type="button" class="btn btn-link" ng-click="removeDiscount()">
                                                    <span class="ion-close-round"></span>
                                                </button>
                                            </div>
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.discounts_title" tabindex="1002">
                                            <div class="input-group-addon input deleteable">
                                                <div input-amount="" is-rate="discountIsRate" currency="invoice.currency" ng-model="invoice.discounts" ng-tabindex="1003" has-selector="true" class="ng-pristine ng-untouched ng-valid ng-isolate-scope">
                                                    <div class="input-amount dropdown with-selector" ng-class="{&#39;is-percent&#39;:!!isRate,&#39;with-selector&#39;:hasSelector}">
                                                        <div class="addon currency-sign ng-binding">$</div>
                                                        <input class="form-control ng-pristine ng-untouched ng-valid ng-valid-required" type="number" step="any" autocomplete="off" tabindex="1003" ng-required="ngRequired" ng-model="value" ng-change="change()"><div class="addon percent">%</div>
                                                        <div class="addon selector">
                                                            <button type="button" class="btn dropdown-toggle" aria-haspopup="true" aria-expanded="false"><span class="ion-chevron-down"></span></button>
                                                        </div>
                                                        <ul class="dropdown-menu dropdown-menu-right">
                                                            <li ng-class="{active:!isRate}" class="active"><a href="https://invoice-generator.com/#" ng-click="isRate=false;$event.preventDefault()" class="ng-binding">Flat ($)</a></li>
                                                            <li ng-class="{active:isRate}"><a href="https://invoice-generator.com/#" ng-click="isRate=true;$event.preventDefault()">Percent (%)</a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-group addon-input" ng-show="!!invoice.fields.tax">
                                            <div class="delete">
                                                <button type="button" class="btn btn-link" ng-click="removeTax()">
                                                    <span class="ion-close-round"></span>
                                                </button>
                                            </div>
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.tax_title" tabindex="1004">
                                            <div class="input-group-addon input deleteable">
                                                <div input-amount="" is-rate="taxIsRate" currency="invoice.currency" ng-model="invoice.tax" ng-tabindex="1005" has-selector="true" class="ng-pristine ng-untouched ng-valid ng-isolate-scope">
                                                    <div class="input-amount dropdown is-percent with-selector" ng-class="{&#39;is-percent&#39;:!!isRate,&#39;with-selector&#39;:hasSelector}">
                                                        <div class="addon currency-sign ng-binding">$</div>
                                                        <input class="form-control ng-pristine ng-untouched ng-valid ng-valid-required" type="number" step="any" autocomplete="off" tabindex="1005" ng-required="ngRequired" ng-model="value" ng-change="change()"><div class="addon percent">%</div>
                                                        <div class="addon selector">
                                                            <button type="button" class="btn dropdown-toggle" aria-haspopup="true" aria-expanded="false"><span class="ion-chevron-down"></span></button>
                                                        </div>
                                                        <ul class="dropdown-menu dropdown-menu-right">
                                                            <li ng-class="{active:!isRate}"><a href="https://invoice-generator.com/#" ng-click="isRate=false;$event.preventDefault()" class="ng-binding">Flat ($)</a></li>
                                                            <li ng-class="{active:isRate}" class="active"><a href="https://invoice-generator.com/#" ng-click="isRate=true;$event.preventDefault()">Percent (%)</a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-group addon-input ng-hide" ng-show="!!invoice.fields.shipping">
                                            <div class="delete">
                                                <button type="button" class="btn btn-link" ng-click="removeShipping()">
                                                    <span class="ion-close-round"></span>
                                                </button>
                                            </div>
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.shipping_title" tabindex="1006">
                                            <div class="input-group-addon input deleteable">
                                                <div input-amount="" is-rate="false" currency="invoice.currency" ng-model="invoice.shipping" ng-tabindex="1007" class="ng-pristine ng-untouched ng-valid ng-isolate-scope">
                                                    <div class="input-amount dropdown" ng-class="{&#39;is-percent&#39;:!!isRate,&#39;with-selector&#39;:hasSelector}">
                                                        <div class="addon currency-sign ng-binding">$</div>
                                                        <input class="form-control ng-pristine ng-untouched ng-valid ng-valid-required" type="number" step="any" autocomplete="off" tabindex="1007" ng-required="ngRequired" ng-model="value" ng-change="change()"><div class="addon percent">%</div>
                                                        <div class="addon selector">
                                                            <button type="button" class="btn dropdown-toggle" aria-haspopup="true" aria-expanded="false"><span class="ion-chevron-down"></span></button>
                                                        </div>
                                                        <ul class="dropdown-menu dropdown-menu-right">
                                                            <li ng-class="{active:!isRate}" class="active"><a href="https://invoice-generator.com/#" ng-click="isRate=false;$event.preventDefault()" class="ng-binding">Flat ($)</a></li>
                                                            <li ng-class="{active:isRate}"><a href="https://invoice-generator.com/#" ng-click="isRate=true;$event.preventDefault()">Percent (%)</a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="add-rates">
                                            <button type="button" class="btn btn-link btn-sm" ng-click="addDiscount()" ng-hide="!!invoice.fields.discounts" tabindex="1008">
                                                <span class="ion-plus-round"></span>
                                                Discount
						
                                            </button>
                                            <button type="button" class="btn btn-link btn-sm ng-hide" ng-click="addTax()" ng-hide="!!invoice.fields.tax" tabindex="1009">
                                                <span class="ion-plus-round"></span>
                                                Tax
						
                                            </button>
                                            <button type="button" class="btn btn-link btn-sm" ng-click="addShipping()" ng-hide="!!invoice.fields.shipping" tabindex="1010">
                                                <span class="ion-plus-round"></span>
                                                Shipping
						
                                            </button>
                                        </div>

                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.total_title" tabindex="1011">
                                            <div class="input-group-addon value deleteable ng-binding" ng-bind-html="invoice.total|currencyFormat:invoice.currency"><span class="currency-amount"><span class="currency-symbol">$</span>130</span></div>
                                        </div>

                                        <div class="input-group addon-input">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.amount_paid_title" tabindex="1012">
                                            <div class="input-group-addon input deleteable">
                                                <div input-amount="" currency="invoice.currency" ng-model="invoice.amount_paid" ng-tabindex="1013" class="ng-pristine ng-untouched ng-valid ng-isolate-scope">
                                                    <div class="input-amount dropdown" ng-class="{&#39;is-percent&#39;:!!isRate,&#39;with-selector&#39;:hasSelector}">
                                                        <div class="addon currency-sign ng-binding">$</div>
                                                        <input class="form-control ng-pristine ng-untouched ng-valid ng-valid-required" type="number" step="any" autocomplete="off" tabindex="1013" ng-required="ngRequired" ng-model="value" ng-change="change()"><div class="addon percent">%</div>
                                                        <div class="addon selector">
                                                            <button type="button" class="btn dropdown-toggle" aria-haspopup="true" aria-expanded="false"><span class="ion-chevron-down"></span></button>
                                                        </div>
                                                        <ul class="dropdown-menu dropdown-menu-right">
                                                            <li ng-class="{active:!isRate}" class="active"><a href="https://invoice-generator.com/#" ng-click="isRate=false;$event.preventDefault()" class="ng-binding">Flat ($)</a></li>
                                                            <li ng-class="{active:isRate}"><a href="https://invoice-generator.com/#" ng-click="isRate=true;$event.preventDefault()">Percent (%)</a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="footer">
                                    <div class="notes-holder">
                                        <div class="field">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.notes_title" tabindex="1014">
                                        </div>
                                        <div class="value">
                                            <div class="expandingText" style="position: relative;">
                                                <textarea class="notes form-control ng-pristine ng-valid expanding-init ng-touched" placeholder="Notes - any relevant information not already covered" ng-model="invoice.notes" tabindex="1015" expanding-textarea="" style="position: absolute; height: 100%; resize: none;"></textarea>
                                                <pre class="textareaClone" style="visibility: hidden; border: 1px solid; white-space: pre-wrap; line-height: 20px; text-decoration: none solid rgb(85, 85, 85); font-size: 14px; font-family: &quot; open sans&quot; , &quot; helvetica neue&quot; , helvetica, arial, sans-serif; word-break: normal; padding: 8px 12px; margin-bottom: 0px;"><div> </div></pre>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="terms-holder">
                                        <div class="field">
                                            <input class="input-label form-control ng-pristine ng-untouched ng-valid" ng-model="invoice.terms_title" tabindex="1016">
                                        </div>
                                        <div class="value">
                                            <div class="expandingText" style="position: relative;">
                                                <textarea class="terms form-control ng-pristine ng-valid expanding-init ng-touched" placeholder="Terms and conditions - late fees, payment methods, delivery schedule" ng-model="invoice.terms" tabindex="1017" expanding-textarea="" style="position: absolute; height: 100%; resize: none;"></textarea>
                                                <pre class="textareaClone" style="visibility: hidden; border: 1px solid; white-space: pre-wrap; line-height: 20px; text-decoration: none solid rgb(85, 85, 85); font-size: 14px; font-family: &quot; open sans&quot; , &quot; helvetica neue&quot; , helvetica, arial, sans-serif; word-break: normal; padding: 8px 12px; margin-bottom: 0px;"><div> </div></pre>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </form>
            </div>
        </div>


    </div>


</asp:Content>

