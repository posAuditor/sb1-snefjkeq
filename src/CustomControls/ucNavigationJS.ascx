<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNavigationJS.ascx.cs" Inherits="CustomControls_ucNavigationJS" %>

<%@ Register Src="~/CustomControls/ucLover.ascx" TagPrefix="asp" TagName="Favorit" %>
<style>
    /*    @media screen and (max-width : 320px)
{
  .xx a
  {
    font-size: 8px;
  }
}
@media screen and (max-width : 1204px)
{
  .xx a
  {
    font-size: 29px;
  }
}*/

    .xx {
        zoom:90%
    }

        .xx a {
            text-decoration: none;
            display: inline-block;
            /*padding: 8px 16px;*/
            padding: 2px 19px;
            font-size: 29px;
            color: white !important;
        }

            .xx a:hover {
                background-color: #b579e2;
                color: black;
            }

        .xx .xxx a:hover {
            background-color: transparent !important;
            color: red;
        }

    .previous {
        /*background-color: #f1f1f1;*/
        background-color: transparent !important;
        color: black;
    }

    .next {
        /*background-color: #f1f1f1;*/
        background-color: transparent !important;
        color: yellow;
    }

    .round {
        border-radius: 50%;
    }
</style>

<script type="text/javascript">
    function navigate(navTypeId) {
        var id = jqVar("#Id").val();
        var entryType = jqVar("#EntryType").val();
        var isPermShow = jqVar("#IsPermShow").val();
        var sourceTypeId = jqVar("#SourceTypeId").val();
        var searchText = jqVar("#txtSerialSearch").val();

        jqVar.post("../../api/General/Navigate", {
            'EntryType': entryType, 'SourceTypeId': sourceTypeId, 'NavTypeId': navTypeId, 'Id': id,
            'IsPermShow': isPermShow, 'SearchText': searchText,
        }, function (response) {

            if (response > 0) {
                documentSelected(1, response);
            }
        });
    }

    function toggelFav() {
        var filePath = '<%=Request.AppRelativeCurrentExecutionFilePath%>';
        jqVar.post('../../api/General/ToggleFav', { 'FilePath': filePath }, function (response) {
            //alert(response);
            if (response == 1) {
                //alert("<%=idfav.ClientID%>");
                jqVar("#<%=idfav.ClientID%>").css({ "font-size": "30px", "padding-top": "0", "color": "red" });
            }
            else {
                //alert("<%=idfav.ClientID%>");
                jqVar("#<%=idfav.ClientID%>").css({ "font-size": "30px", "padding-top": "0", "color": "blue" });
            }
        });
    }
</script>

<div class="xx col-md-12" >
    <div class="row">
        <input type="text" class="col-auto" id="txtSerialSearch" autocompletetype="None" style="width: 130px; background-color: transparent; color: red; font-size: 15px;" />
        <a href="javascript:void(0)" class="col-auto" onclick="toggelFav()">
            <i class="fa fa-heart" runat="server" id="idfav"></i>
        </a>

        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkSearchInvoice" style="padding: 0px;" onclick="navigate(5)">
                <i class="fa fa-search"></i>
            </a>
        </div>
        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkAddNewItem" class="previous round" onclick="addItem()">
                <i class="fa fa-plus"></i>
            </a>
        </div>
        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkFirst" class="xx previous round" onclick="navigate(1)">
                <i class="<%=Resources.Labels.NavFastForward %>"></i>
            </a>
        </div>
        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkPrev" class="xx previous round" onclick="navigate(4)">
                <i class="<%=Resources.Labels.NavRight%>"></i>
            </a>
        </div>

        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkNext" class="xx next round" onclick="navigate(3)">
                <i class="<%=Resources.Labels.NavLeft%>"></i>
            </a>
        </div>
        <div class="col-auto">
            <a href="javascript:void(0)" id="lnkLast" class="xx next round" onclick="navigate(2)">
                <i class="<%=Resources.Labels.NavFastBackward%>"></i>
            </a>
        </div>

        <label class="col-auto" id="pageTitleLbl" style="color: blue; font-size: 16px; font-weight: bold;"></label>

        <div id="CustomerItemNavDiv" class="col-auto" style="display: none">
            <a href="javascript:void(0)" onclick="showModal('#itemInfoModal');" title="<%=Resources.Labels.AddItem %>"><i class="fa fa-plus-square" aria-hidden="true"></i></a>
            <a href="javascript:void(0)" onclick="showModal('#customerInfoModal');" title="<%=Resources.Labels.AddAccount %>"><i class="fa fa-plus-square" aria-hidden="true"></i></a>
        </div>
    </div>
</div>
