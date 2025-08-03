<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Payroll.aspx.cs" Inherits="HR_Payroll" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style>
        /*Grid View Style*/
        .grid {
            background: #E8E8E8 none repeat scroll 0 0;
            width: 100%;
        }

            .grid td {
                border-bottom: 1px solid white;
                font-family: "Droid Arabic Kufi",Arial;
                font-weight: bold;
                font-size: 14px;
                text-align: center;
                padding: 5px;
            }

        .grid_header {
            background: #215173 url(../images/thead_bg.gif) repeat-x scroll left top;
            color: #FFFFFF;
            font-family: "Droid Arabic Kufi",tahoma,verdana,geneva,lucida, 'lucida grande',arial,helvetica,sans-serif;
            font-size: 11px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            text-align: center;
            padding-left: 8px;
        }

        .grid_empty td {
            border: solid 2px #26990E;
            color: #6f0b0b;
            font-weight: bold;
            text-align: center;
        }

        .grid_header th {
            height: 38px;
            border: 0px !important;
            border-right: solid 1px !important;
            padding-left: 10px;
            padding-right: 10px;
        }

        .grid_header a {
            color: #FFFFFF;
        }

            .grid_header a:hover {
                color: #ECEFF2;
            }

        .grid_align_left_column {
            text-align: right !important;
            font-family: "Droid Arabic Kufi", Arial;
            padding: 5px 5px 5px 5px;
        }

        .grid_align_left_header {
            height: 38px;
            border: 0px !important;
            border-right: solid 1px # !important;
            padding-left: 10px;
            padding-right: 15px;
            text-align: right !important;
        }

        .grid_align_left_footer {
            height: 38px;
            border: 0px !important;
            border-right: solid 1px # !important;
            padding-left: 10px;
            padding-right: 15px;
            text-align: right !important;
        }

        .grid_Row {
            background: white url(../images/gv_col_bg.gif) repeat-x scroll left bottom;
            color: #575757;
            font-family: "Droid Arabic Kufi",tahoma;
            font-size: 11px;
            -font-weight: bold;
        }

        .grid_alt_Row {
            background: #eef5f9 url(../images/gv_alt_col_bg.png) repeat-x scroll left bottom;
            color: #575757;
            font-family: "Droid Arabic Kufi",tahoma;
            font-size: 11px;
            -font-weight: bold;
        }

        .grid_edit_Row {
            background-color: #000000;
        }

        .grid_selected_Row {
            background-color: #dcecfb;
        }

        .grid_pager {
            text-align: -moz-center;
            background: #ffffff;
        }

            .grid_pager td {
                text-align: -moz-center;
                border: 0px;
            }

            .grid_pager table {
                text-align: -moz-center;
            }

                .grid_pager table td {
                    border: 1px solid #557C93;
                    margin: 3px 3px 3px 3px;
                    border-spacing: 10px;
                    font-family: "Droid Arabic Kufi",Arial;
                    padding: 5px 5px 5px 5px;
                    text-align: center;
                    color: #575757;
                    background-color: #F5F5FF;
                }

                    .grid_pager table td a {
                        font-weight: bold;
                        color: #5FBE38;
                    }
        /*Small Grid view grd*/
        .grd {
            margin: 10px auto;
            background: #d0d0d0;
            width: 100%;
            font-size: 15px;
        }

        .grid_mobile {
            table-layout: fixed;
            word-wrap: break-word;
        }

        .grd_header th {
            background: #f9f9f9;
            height: 24px;
            padding: 3px;
            font-size: 12px;
        }

        .grd_fixedheader {
            position: absolute;
            background-color: #D0D0D0;
            margin-top: -12px;
            margin-right: -2px;
            border-top: solid 1px;
            border-right: solid 1px;
            border-left: solid 1px;
            width: 662px;
            height: 40px;
        }

        .grd_empty td {
            border: solid 1px #D0D0D0;
            color: #6f0b0b;
            padding: 10px;
            background: #F7F7F7;
            font-weight: bold;
            text-align: center;
        }

        .grd_row {
            background: #fff;
        }

            .grd_row td, .grd_alt_row td, .grd_marked_row td {
                padding: 3px;
                text-align: center;
                overflow: visible;
            }

        .grd_alt_row {
            background: #f1f2f4;
        }

        .grd_marked_row {
            background: #f5ff5d;
        }



            .grd_row:hover, .grd_alt_row:hover, .grd_row:hover a, .grd_alt_row:hover a, .grd_marked_row:hover, .grd_marked_row:hover a {
                background-color: #b33131;
                color: #fff;
            }

        .DocMarker_New .grd_row:hover, .DocMarker_New .grd_alt_row:hover, .DocMarker_New .grd_row:hover a, .DocMarker_New .grd_alt_row:hover a {
            background-color: #0b6f13;
        }

        .DocMarker_Current .grd_row:hover, .DocMarker_Current .grd_alt_row:hover, .DocMarker_Current .grd_row:hover a, .DocMarker_Current .grd_alt_row:hover a {
            background-color: #9e7528;
        }

        .DocMarker_Canceled .grd_row:hover, .DocMarker_Canceled .grd_alt_row:hover, .DocMarker_Canceled .grd_row:hover a, .DocMarker_Canceled .grd_alt_row:hover a {
            background-color: #736e6e;
        }


        .grd_row td a, .grd_alt_row td a {
            display: block;
        }

            .grd_row td a img, .grd_alt_row td a img {
                display: block;
                margin: auto;
            }

        .grd_pager {
            background: #ffffff;
        }

            .grd_pager td {
                border: 0px;
            }

            .grd_pager table {
                margin: auto;
            }

                .grd_pager table td {
                    border: 1px solid #557C93;
                    margin: 2px;
                    border-spacing: 10px;
                    font-family: "Droid Arabic Kufi",Arial;
                    padding: 5px;
                    text-align: center;
                    color: #575757;
                    background-color: #F5F5FF;
                    font-size: 20px;
                }

                    .grd_pager table td a {
                        font-weight: bold;
                        color: #4c9ccc;
                    }
        /* Hide Grid Column */
        .hide_column {
            display: none;
        }

        .align_column {
            text-align: right;
        }

        .grd input[type=image], .grid input[type=image] {
            border: 0px;
            background: transparent;
            box-shadow: none;
            cursor: pointer;
            display: block;
            margin: auto;
        }

        a.grid-expand, a:focus.grid-expand {
            background: url(../images/gridview_expand-ar.gif) no-repeat !important;
            width: 16px;
            height: 16px;
            border: 0px;
            cursor: pointer;
            color: transparent;
        }

        a.grid-collapse, a:focus.grid-collapse {
            background: url(../images/gridview_collapse-ar.gif) no-repeat !important;
            width: 16px;
            height: 16px;
            border: 0px;
            cursor: pointer;
            color: transparent;
        }


        h1 {
            color: Green;
        }

        div.scroll {
            margin: 4px, 4px;
            padding: 4px;
            background-color: #4c0481;
            width: 100%;
            overflow-x: auto;
            overflow-y: hidden;
            white-space: nowrap;
        }
    </style>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:LinkButton ID="lnkSearch" runat="server" CssClass="collapse_search_link" Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
        CollapsedSize="0" Collapsed="false" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
        AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
        ExpandDirection="Vertical" SuppressPostBack="true">
    </asp:CollapsiblePanelExtender>
    <div style="clear: both;">
    </div>
    <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
        <div class="tcat">
            <%=Resources.Labels.Search %>
        </div>
        <div class="content">
            <div class="form" style="width: 550px;">
                <div class="right_col">
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" IsRequired="true" ValidationGroup="Search"
                        OnSelectedIndexChanged="FilterEmployees" AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete ID="acDepartment" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>"
                        runat="server" IsRequired="true" ValidationGroup="NewEmployee" OnSelectedIndexChanged="FilterEmployees"
                        AutoPostBack="true"></asp:AutoComplete>
                    <asp:AutoComplete runat="server" ID="acName" ServiceMethod="GetEmployeesNames" LabelText="<%$Resources:Labels,Name %>"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <label>
                        <%=Resources.Labels.Month %></label>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Text="<%$ Resources:Labels,January %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,February %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,March %>" Value="3"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,April %>" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,May %>" Value="5"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,June %>" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,July %>" Value="7"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,August %>" Value="8"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,September %>" Value="9"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,October %>" Value="10"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,November %>" Value="11"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Labels,December %>" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                    <label>
                        <%=Resources.Labels.Year %></label>
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Calculate %>" ValidationGroup="Search"
                    OnClick="btnSearch_click" />
                <asp:Button ID="btnClearSrch" runat="server" CssClass="button" OnClick="btnClearSrch_Click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
    <div class="form" style="width: 550px;">
        <asp:ABFTextBox ID="txtDate" CssClass="field" LabelText="<%$Resources:Labels,Date %>"
            runat="server" ValidationGroup="Approve" DataType="Date" IsRequired="true"></asp:ABFTextBox>
    </div>
    <br />
    <br /> 
    <asp:Button ID="bvtnShowHide" OnClick="bvtnShowHide_Click" runat="server" Text="إظهار/إخفاء الحقول" />
    <div class="scroll">

 

        <asp:UpdatePanel ID="updatepnl123" runat="server">
            <ContentTemplate>
              
                <asp:ABFGridView runat="server" ID="gvPayRoll" GridViewStyle="BlueStyle" DataKeyNames="ID,Contact_ID,ContactName,Department_ID,DepartmentName,GrossTotal,Month,Year,DocStatus_ID"
                    OnPageIndexChanging="gvPayRoll_PageIndexChanging" OnSelectedIndexChanging="gvPayRoll_SelectedIndexChanging" OnRowDataBound="gvPayRoll_RowDataBound">
                    <Columns>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Approve %>">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkApprove" runat="server" Checked='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="2")%>'
                                    Enabled='<%# Convert.ToBoolean(Eval("DocStatus_ID").ToString()=="1")%>' />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkApproveAll" runat="server" Text="<%$ Resources:Labels, Approve %>"
                                    onchange="SelectAll();" />
                            </HeaderTemplate>
                        </asp:TemplateField>


                        <asp:BoundField DataField="DepartmentName" HeaderText="<%$Resources:Labels,Department %>" />
                        <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="Month" HeaderText="<%$Resources:Labels,Month %>" />
                        <asp:BoundField DataField="Year" HeaderText="<%$Resources:Labels,Year %>" />
                        <asp:BoundField DataField="GrossTotal" HeaderText="<%$Resources:Labels,GrossTotal %>"
                            DataFormatString="{0:0.####}" />



                        <asp:TemplateField HeaderText="<%$ Resources:Labels, OverTime %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtOverTime" CssClass="field" LabelText="<%$Resources:Labels,OverTime %>" Width="100"
                                    OnTextChanged="txtOverTime_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                                    ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, OverTimeValue %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtOverTimeValue" CssClass="field" LabelText="<%$Resources:Labels,OverTimeValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, DayOffWork %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDayOffWork" CssClass="field" LabelText="<%$Resources:Labels,DayOffWork %>"
                                    OnTextChanged="txtDayOffWork_TextChanged" AutoPostBack="true" runat="server" Width="100"
                                    IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,DayOffWorkValue   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDayOffWorkValue" CssClass="field" LabelText="<%$Resources:Labels,DayOffWorkValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delay  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDelay" CssClass="field" LabelText="<%$Resources:Labels,Delay %>"
                                    OnTextChanged="txtDelay_TextChanged" AutoPostBack="true" runat="server" IsRequired="true" Width="100"
                                    ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, DelayValue  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDelayValue" CssClass="field" LabelText="<%$Resources:Labels,DelayValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,Absence   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtAbsence" CssClass="field" LabelText="<%$Resources:Labels,Absence %>"
                                    OnTextChanged="txtAbsence_TextChanged" AutoPostBack="true" runat="server" IsRequired="true" Width="100"
                                    ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,AbsenceValue   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtAbsenceValue" CssClass="field" LabelText="<%$Resources:Labels,AbsenceValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Excuse  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtExcuse" CssClass="field" LabelText="<%$Resources:Labels,Excuse %>" Width="100"
                                    OnTextChanged="txtExcuse_TextChanged" AutoPostBack="true" runat="server" IsRequired="true"
                                    ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, ExcuseValue  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtExcuseValue" CssClass="field" LabelText="<%$Resources:Labels,ExcuseValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, DeducatedWorkDays  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDeducatedWorkDays" CssClass="field" LabelText="<%$Resources:Labels,DeducatedWorkDays %>"
                                    OnTextChanged="txtDeducatedWorkDays_TextChanged" AutoPostBack="true" runat="server" Width="100"
                                    IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,DeducatedWorkDaysValue   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDeducatedWorkDaysValue" CssClass="field" LabelText="<%$Resources:Labels,DeducatedWorkDaysValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, DeducatedDaysOff  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDeducatedDaysOff" CssClass="field" LabelText="<%$Resources:Labels,DeducatedDaysOff %>"
                                    OnTextChanged="txtDeducatedDaysOff_TextChanged" AutoPostBack="true" runat="server" Width="100"
                                    IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, DeducatedDaysOffValue  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtDeducatedDaysOffValue" CssClass="field" LabelText="<%$Resources:Labels,DeducatedDaysOffValue %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Incentives  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtIncentives" CssClass="field" LabelText="<%$Resources:Labels,Incentives %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Allowances  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtAllowances" Enabled="False" CssClass="field" LabelText="<%$Resources:Labels,Allowances %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,Taxes   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtTaxes" CssClass="field" LabelText="<%$Resources:Labels,Taxes %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Insurance  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtInsurance" CssClass="field" LabelText="<%$Resources:Labels,Insurance %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" Enabled="False" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels,HRLoan   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtLoans" CssClass="field" LabelText="<%$Resources:Labels,HRLoan %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels,Sancations   %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtSancations" CssClass="field" LabelText="<%$Resources:Labels,Sancations %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="مكافئات - زيادات أخرى">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtOtherAdditions" CssClass="field" LabelText="مكافئات - زيادات أخرى"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, OtherDeducations  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtOtherDeducations" CssClass="field" LabelText="<%$Resources:Labels,OtherDeducations %>"
                                    runat="server" IsRequired="true" ValidationGroup="AddNew" DataType="Decimal" Width="100"
                                    OnTextChanged="CalcGrossTotal" AutoPostBack="true" MinValue="0" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--   <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                    <ItemTemplate>
                        <asp:Button ID="btnSaveNew" CssClass="button default_button" runat="server" OnClick="btnSaveNew_click"
                            Text="<%$ Resources:Labels, Save %>" ValidationGroup="AddNew" />
                    </ItemTemplate>
                </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Notes  %>">
                            <ItemTemplate>
                                <asp:ABFTextBox ID="txtNotes" CssClass="field" LabelText="<%$Resources:Labels,Notes %>" Width="100"
                                    runat="server" ValidationGroup="AddNew" VisibleText="false"></asp:ABFTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="الاجمالي">
                            <ItemTemplate>

                                <asp:Label runat="server" ID="lblGrossTotal2" Text="0" ForeColor="Red"></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="الخصومات">
                            <ItemTemplate>

                                <asp:Label runat="server" ID="lblGrossTotal1" Text="0" ForeColor="Green"></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, GrossTotal  %>">
                            <ItemTemplate>

                                <asp:Label runat="server" ID="lblGrossTotal" Text="0" ForeColor="Black"></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSave" Text="حفظ" runat="server" CommandArgument='<%#Eval("Contact_ID") %>' OnClick="btnSaveNew_click"></asp:LinkButton>
                                <%-- <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblBasicSalary" Text="0" ForeColor="Black"></asp:Label>
                                <asp:Label runat="server" ID="lblContact_ID" Text="0" Visible="false" ForeColor="Black"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>


            </ContentTemplate>
        </asp:UpdatePanel>


    </div>


    <br />
    <br />
    <div class="align_right">
        <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve" ValidationGroup="Approve" OnClientClick="return ConfirmSureWithValidation('Approve');"
            OnClick="BtnApprove_Click" />
    </div>

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
        PopupControlID="Panel1" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <asp:Label runat="server" ID="Label1" Text=""></asp:Label></span>
        </div>
        <div class="content">
            <div class="form" style="height: 400px; overflow-x: hidden; overflow-y: auto; white-space: nowrap;">
                <asp:ABFGridView runat="server" ID="gvShowHide" GridViewStyle="BlueStyle" DataKeyNames="ID,IndexColumn,NameColumn,checkedIndex" OnRowDataBound="gvShowHide_RowDataBound" PageSize="30" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Approve %>">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkApproveShowHide" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                        <asp:BoundField DataField="NameColumn" HeaderText="العمود" />
                    </Columns>
                </asp:ABFGridView>
            </div>
            <div style="clear: both;">
            </div>

            <div class="btnDiv">
                <asp:LinkButton ID="btnSaveShowHide" Text="حفظ" runat="server" OnClick="btnSaveShowHide_Click"></asp:LinkButton>
                <%-- <asp:Button ID="Button2" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Close %>" />--%>
            </div>
        </div>
    </asp:Panel>






    <asp:HiddenField ID="hfmpeCreateNew" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateNew" runat="server" TargetControlID="hfmpeCreateNew"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <asp:Label runat="server" ID="lblHeader" Text=""></asp:Label></span>
        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">
                </div>
                <div class="left_col">
                </div>
            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddNew" />
            </div>
            <div class="btnDiv">

                <asp:Button ID="BtnCloseNew" runat="server" CssClass="button" OnClick="ClosePopup_Click"
                    Text="<%$ Resources:Labels, Close %>" />
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">

        function SelectAll() {
            $("input[type=checkbox]:enabled").each(function (index, obj) {
                if (obj.id != "cph_gvPayRoll_chkApproveAll") {
                    obj.checked = document.getElementById("cph_gvPayRoll_chkApproveAll").checked;
                }
            });

        }
    </script>
</asp:Content>
