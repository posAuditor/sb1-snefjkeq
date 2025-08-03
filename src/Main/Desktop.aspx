<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="Desktop.aspx.cs" Inherits="Main_Desktop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <link href="../Styles/Auditor.min.css" rel="stylesheet" />
    <link href="../Styles/Auditor-rtl.css" rel="stylesheet" />
     <link href="../Fonts/NFont/css/awesonne.css" rel="stylesheet" />
    
   <link runat="server" rel="stylesheet" type="text/css" href="<%$ Resources:ResourceThem, Style  %>" />
    <link href="../Styles/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        .clsDark {
            color: white;
        }

        .clsLight {
            color: black;
        }


        .clearfix {
            *zoom: 1;
        }

            .clearfix:before,
            .clearfix:after {
                display: table;
                content: "";
                line-height: 0;
            }

            .clearfix:after {
                clear: both;
            }

        @font-face {
            font-family: 'awesonne';
            src: url('../Fonts/NFont/font/awesonne.eot?30429');
            src: url('../Fonts/NFont/font/awesonne.eot?30429#iefix') format('embedded-opentype'), url('../Fonts/NFont/font/awesonne.woff?30429') format('woff'), url('../Fonts/NFont/font/awesonne.ttf?30429') format('truetype'), url('../Fonts/NFont/font/awesonne.svg?30429#awesonne') format('svg');
            font-weight: normal;
            font-style: normal;
        }
    </style>

    <script type="text/javascript" src="../Scripts/Descktop.js"></script>
    <script type="text/javascript">
        var tags = [];
        var pageUrl = [];
        var isArabic = false;
        $(document).ready(function () {
            LoadMenu(<%=this.MyLocalContext.CurrentCulture == XPRESS.Common.ABCulture.Arabic ? "true" : "false"%>);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
   
     <div class="content page-container" id="chat_list_scroll" data-page-route="desktop" style="min-height: 90vh!important; height: 90vh!important">
        <div style="text-align: center; padding-top: calc(4px + 1%)">
            <div id="icon-grid">
            </div>
            <div class="help-message-wrapper hidden">
                <div class="help-message-container">
                    <a class="close pull-right" style="margin-right: -7px;">
                        <i class="octicon octicon-x"></i>
                    </a>
                    <div class="help-messages"></div>
                    <a class="left-arrow octicon octicon-chevron-left"></a><a class="right-arrow octicon octicon-chevron-right"></a>
                </div>
            </div>
        </div>
        <div style="clear: both"></div>
    </div>
     
  
    <script src="../Scripts/jquery-ui.js"></script>
    <script src="../Styles/RTL_Theme/vendors/bower_components/bootstrap/dist/js/bootstrap.min.js"></script> 
    
    
     
</asp:Content>

