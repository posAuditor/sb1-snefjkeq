<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmItemCachedSelect.aspx.cs" Inherits="Inv_ItemUI_FrmItemCachedSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="../../Content/font-awesome.min.css" rel="stylesheet" />
    <!-- Bootstrap CSS -->
    <link href="../../Content/bootstrap-v5.1.3/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Content/jquery-3.5.1.min.js"></script>

    <!-- JavaScript Bundle with Popper -->
    <script type="text/javascript" src="../../Content/bootstrap-v5.1.3/js/bootstrap.bundle.min.js"></script>--%>

    <link href="../../Styles/SearchPageStyle.css" rel="stylesheet" />
    <script>
        //var $ = jQuery.noConflict();
        $(document).ready(function () {
            loadItems();

            $('#SearchText').on("keyup", function () {
                var value = $(this).val().toLowerCase();
                /*$("#searchDgvDiv .dx-row .dx-data-row .dx-row-lines .dx-column-lines").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });*/

                $("#SearchTable .tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

        function loadItems() {
            var cachedItems = localStorage.getItem("items");
            if (cachedItems) {
                let allMyItems = [];
                allMyItems = JSON.parse(cachedItems);
                //var table = document.createElement('table');    
                
                for (var i = 0; i < allMyItems.length; i++) {
                    allMyItems[i].rowNum = i + 1;
                    var tr = document.createElement('tr');
                    tr.setAttribute("class", "tr");
                    var td1 = document.createElement('td');
                    var td2 = document.createElement('td');

                    var text0 = document.createTextNode(i+1);
                    var text1 = document.createTextNode(allMyItems[i].ID);
                    var text2 = document.createTextNode(allMyItems[i].Name);

                    td1.appendChild(text0);
                    td2.appendChild(text1);
                    tr.appendChild(td1);
                    tr.appendChild(td2);

                    var td3 = document.createElement('td');
                    td3.appendChild(text2);
                    tr.appendChild(td3);

                    var td4 = document.createElement('td');
                    var itemId = allMyItems[i].ID;
                    var rowIndex = '<%=Request.QueryString["rowIndex"] %>';
                    td4.innerHTML = `<a href="#" class="btn-sm btn-success" onclick="selectMe(` + itemId + ",'" + allMyItems[i].Name+"'," + rowIndex + `)">اختيار</a>`;
                    tr.appendChild(td4);
                    //table.appendChild(tr);
                    document.getElementById("tbody").appendChild(tr);                    
                }                
            }
        }

        function selectMe(itemId,itemName, rowIndex) {
            itemSelectedF9(itemId, itemName, rowIndex);
        }
    </script>
</head>
<body>
    <form id="form0" runat="server">       

        <div class="row">
            <input id="SearchText" class="form-control" type="text" placeholder="<%=Resources.Labels.SearchHere %>" />

            <table id="SearchTable" class="table table-stripped table-bordered table-hover table-sm">
                <thead class="datagridheader">
                    <tr>
                        <th>م
                        </th>
                        <th><%=Resources.Labels.DgItemIdHeader %>
                        </th>
                        <th><%=Resources.Labels.DgItemNameHeader %>
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="tbody">
                    
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
