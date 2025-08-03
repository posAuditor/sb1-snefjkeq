<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InventoryCorrection.aspx.cs" Inherits="Inv_InventoryCorrection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewBatch" Src="~/CustomControls/ucNewBatch.ascx" TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>

<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <div class="MainInvoiceStyleDiv">
        <div class="notch_label" style="background: url('<%=this.ImgStatus%>.png') no-repeat;">
        </div>
        <div class="InvoiceHeader">
            <asp:ABFTextBox ID="txtSerial" runat="server" Visible="false"
                ReadOnly="true" Width="200" Style="text-align: center;"></asp:ABFTextBox>
            <asp:Nav runat="server" ID="ucNav" />
        </div>
        <div class="InvoiceSection">
            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <span>
                        <%=Resources.Labels.CreatedBy %></span>:
                    <asp:Label ID="lblCreatedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtOperationDate" runat="server" ValidationGroup="Save" LabelText="<%$Resources:Labels,Date %>"
                        DataType="Date" IsRequired="true"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>"
                        OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true" ValidationGroup="Save"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <span>
                        <%=Resources.Labels.ApprovedBy %></span>:
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Text=""></asp:Label>
                    <asp:ABFTextBox ID="txtUserRefNo" runat="server" LabelText="<%$Resources:Labels,UserRefNo %>"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acOppositeAccount" ServiceMethod="GetChartOfAccounts"
                        ValidationGroup="Save" LabelText="<%$Resources:Labels,OppositeAccount %>" IsRequired="true"></asp:AutoComplete>
                </div>
            </div>
            <div style="clear: both">
            </div>

            <div class="form" style="width: 90%; margin: auto;">
                <div class="right_col">
                    <label>إستراد من ملف ماكينة الجرد</label>
                    <div class="InvoiceSection">
                        <div class="container">
                            <asp:FileUpload ID="FileUpload2" runat="server" />
                            <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="FileUpload2"
                                ErrorMessage="Only TXT Allowed"
                                ValidationExpression="^.*\.(?:txt|TXT)$"
                                ValidationGroup="add"></asp:RegularExpressionValidator>
                            <asp:Button ID="Button2" ValidationGroup="add" runat="server" Text="Upload" OnClick="Button2_Click" />
                        </div>
                    </div>
                </div>
                <div class="left_col">
                </div>
            </div>
            <div style="clear: both">
            </div>

            <div class="InvoiceSection">
                <asp:Panel ID="pnlAddItem" runat="server" DefaultButton="btnAddItem">
                    <center>
                        <asp:Label runat="server" ID="lblCostWarn" Text="<%$Resources:UserInfoMessages,InvCorrectionCostWarn %>"
                            ForeColor="Red"></asp:Label>
                    </center>
                    <div class="form" style="width: 90%; margin: auto;">
                        <div class="right_col">
                            <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"
                                IsRequired="true" ValidationGroup="AddItem" OnSelectedIndexChanged="acStore_SelectedIndexChanged"
                                AutoPostBack="true"></asp:AutoComplete>
                            <asp:ABFTextBox ID="txtBarcode" runat="server" LabelText="<%$Resources:Labels,Barcode %>" CssClass="barcode"
                                OnTextChanged="txtBarcode_TextChanged" AutoPostBack="true" IsHideable="true"></asp:ABFTextBox>
                            <asp:AutoComplete runat="server" ID="acCategory" ServiceMethod="GetItemsCategories"
                                LabelText="<%$Resources:Labels,Category %>" OnSelectedIndexChanged="acCategory_SelectedIndexChanged"
                                ValidationGroup="AddItem" IsRequired="true" AutoPostBack="true"></asp:AutoComplete>
                            <asp:AutoComplete runat="server" ID="acItem" ServiceMethod="GetItems" ValidationGroup="AddItem"
                                OnSelectedIndexChanged="acItem_SelectedIndexChanged" AutoPostBack="true" IsRequired="true"
                                LabelText="<%$Resources:Labels,Item %>"></asp:AutoComplete>
                            <asp:AutoComplete runat="server" ID="acUnit" ServiceMethod="GetItemUnits" ValidationGroup="AddItem"
                                IsRequired="true" LabelText="<%$Resources:Labels,Unit %>" OnSelectedIndexChanged="acUnit_SelectedIndexChanged"
                                AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                            <asp:ABFTextBox ID="txtItemNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>"
                                TextMode="MultiLine"></asp:ABFTextBox>
                        </div>
                        <div class="left_col">
                            <asp:ABFTextBox ID="txtCost" runat="server" LabelText="<%$Resources:Labels,Cost %>"
                                MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"></asp:ABFTextBox>
                            <asp:AutoComplete runat="server" ID="acBatchID" ServiceMethod="GetBatches" LabelText="<%$Resources:Labels,BatchID %>"
                                OnSelectedIndexChanged="acBatchID_SelectedIndexChanged" AutoPostBack="true" IsHideable="true"></asp:AutoComplete>
                            <asp:LinkButton ID="lnkNewBatch" runat="server" CssClass="PlusBtn">[+]</asp:LinkButton>
                            <asp:ABFTextBox ID="txtProductionDate" runat="server" LabelText="<%$Resources:Labels,ProductionDate %>"
                                IsHideable="true"></asp:ABFTextBox>
                            <asp:ABFTextBox ID="txtExpirationDate" runat="server" LabelText="<%$Resources:Labels,ExpirationDate %>"
                                IsHideable="true"></asp:ABFTextBox>
                            <br />
                            <asp:ABFTextBox ID="txtQty" runat="server" LabelText="<%$Resources:Labels,ActualQty %>"
                                MinValue="0" DataType="Decimal" IsRequired="true" ValidationGroup="AddItem"></asp:ABFTextBox>
                            <span>
                                <%=Resources.Labels.AvailableQty %>: </span>
                            <asp:Label ID="lblAvailableQty" runat="server" Text=""></asp:Label>
                            <asp:ABFTextBox ID="txtQtyInNumber" runat="server" LabelText="<%$Resources:Labels,QtyInNumberDiff %>"
                                MinValue="0" DataType="Decimal" ValidationGroup="AddItem" IsHideable="true"></asp:ABFTextBox>
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddItem" />
                    </div>
                    <br />
                    <div class="btnDiv">
                        <asp:Button ID="btnAddItem" CssClass="button" runat="server" OnClick="btnAddItem_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem" />
                        <asp:Button ID="BtnClearItem" runat="server" CssClass="button" Text="<%$ Resources:Labels, Clear %>"
                            OnClick="BtnClearItem_Click" />
                        <asp:LinkButton ID="lnkSearch" runat="server" CssClass="button" Text="استراد"></asp:LinkButton>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearch"
                            CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearch" CollapseControlID="lnkSearch"
                            AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
                            ExpandDirection="Vertical" SuppressPostBack="true">
                        </asp:CollapsiblePanelExtender>

                        <asp:Panel ID="pnlSearch" CssClass="pnlSearch" Style="background-color: #640ab7" runat="server" DefaultButton="btnImportFromFile">
                            <div class="tcat">
                                استراد
                            </div>
                            <div class="content">
                                <div class="form" style="width: 550px;">
                                    <div class="right_col">

                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                        <br />
                                        <asp:Label ID="Label1" runat="server" Text="إستخدام السطر الاول لعناوين الحقول"></asp:Label><br />
                                        <asp:RadioButtonList ID="rbHDR" runat="server" RepeatLayout="OrderedList">
                                            <asp:ListItem Text="نعم" Value="Yes" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="لا" Value="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:GridView ID="GridView1" runat="server" OnPageIndexChanging="PageIndexChanging"
                                            AllowPaging="true">
                                        </asp:GridView>
                                    </div>
                                    <div class="left_col">
                                        <div class="Row">
                                            <div class="Column">
                                                <asp:DropDownList ID="ddlPropertiesValue" runat="server" ValidationGroup="AddItem1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="Column">
                                                <asp:DropDownList ID="ddlProperties" runat="server" ValidationGroup="AddItem1">
                                                    <asp:ListItem Text="الباركود" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="الكمية" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="Column">
                                                <asp:Button ID="Button1" CssClass="button" runat="server" OnClick="Button1_Click"
                                                    Text="<%$ Resources:Labels, Add %>" ValidationGroup="AddItem1" />
                                            </div>
                                        </div>
                                        <div class="Row">
                                            <div class="Column">
                                                <asp:ABFGridView runat="server" ID="gvExportList" GridViewStyle="GrayStyle" DataKeyNames="ID">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$Resources:Labels,Serial %>">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Name" HeaderText="إسم الحقل" />
                                                        <asp:BoundField DataField="Value" HeaderText="الحقل المرادف" />
                                                    </Columns>
                                                </asp:ABFGridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;">
                                </div>
                                <div class="btnDiv">
                                    <asp:Button ID="btnImportFromFile" CssClass="button" runat="server" Text="استراد من ملف" OnClick="btnImportFromFile_Click"
                                        ValidationGroup="search" />
                                    <asp:Button ID="btnGetItemsDataNegative" CssClass="button" runat="server" OnClick="btnGetItemsDataNegative_Click"
                                        Text="استراد الكميات السالبة" />
                                    <asp:Button ID="btnGetItemsDataIn" CssClass="button" runat="server" OnClick="btnGetItemsDatain_Click"
                                        Text="تصفير الكميات الموجبة" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
            <div class="InvoiceSection">
                <asp:ABFGridView runat="server" ID="gvItems" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    OnRowDeleting="gvItems_RowDeleting" OnPageIndexChanging="gvItems_PageIndexChanging"
                    OnSelectedIndexChanging="gvItems_SelectedIndexChanging" OnRowDataBound="gvItems_RowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="colSmall" HeaderText="<%$Resources:Labels,Serial %>">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Labels,Store %>" />
                        <asp:BoundField DataField="CategoryName" HeaderText="<%$Resources:Labels,Category %>" />
                        <asp:BoundField DataField="ItemName" HeaderText="<%$Resources:Labels,Item %>" />
                        <asp:BoundField DataField="ActualQty" HeaderText="<%$Resources:Labels,Quantity %>"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="Differrence" HeaderText="<%$Resources:Labels,Difference %>"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="QtyInNumber" HeaderText="<%$Resources:Labels,QtyInNumber %>"
                            DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="UOMName" HeaderText="<%$Resources:Labels,Unit %>" DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="BatchName" HeaderText="<%$Resources:Labels,BatchID %>" ItemStyle-CssClass="BatchCol" />
                        <asp:BoundField DataField="ProductionDate" HeaderText="<%$Resources:Labels,ProductionDate %>" ItemStyle-CssClass="BatchCol"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="ExpirationDate" HeaderText="<%$Resources:Labels,ExpirationDate %>" ItemStyle-CssClass="BatchCol"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="UnitCost" HeaderText="<%$Resources:Labels,Cost %>" DataFormatString="{0:0.####}" />
                        <asp:BoundField DataField="Total" HeaderText="<%$Resources:Labels,Total %>" DataFormatString="{0:0.####}" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </div>
            <div class="InvoiceSection">
                <div class="form">
                    <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                        TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
                </div>
                <br />
                <br />
                <div class="form" style="width: 500px; margin: auto;">
                    <div class="right_col">
                    </div>
                    <div class="left_col">
                        <table class="totals">
                            <tr>
                                <td>
                                    <span class="lbl">
                                        <%=Resources.Labels.Total %>: </span>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotal" runat="server" Text="0.0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="clear: both">
                    </div>
                </div>
            </div>
            <div class="InvoiceSection align_right">
                <div class="validationSummary">
                    <asp:ValidationSummary ID="vsPage" runat="server" ValidationGroup="Save" />
                </div>
                <asp:Button runat="server" ID="btnSave" Text="<%$Resources:Labels,Save %>" CssClass="button_big shortcut_save"
                    ValidationGroup="Save" OnClick="BtnSave_Click" />
                <asp:Button runat="server" ID="btnApprove" Text="<%$Resources:Labels,Approve %>" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="BtnApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />
                <asp:Button runat="server" ID="btnCancelApprove" Text="الغاء الاعتماد" CssClass="button_big shortcut_approve"
                    ValidationGroup="Save" OnClick="btnCancelApprove_Click" OnClientClick="return ConfirmSureWithValidation('Save')" />
                <asp:Button runat="server" ID="btnPrint" Text="<%$Resources:Labels,Print %>" CssClass="button_big shortcut_print"
                    Visible="false" OnClick="btnPrint_Click" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$Resources:Labels,Cancel %>" CssClass="button_big shortcut_cancel"
                    OnClick="btnCancel_Click" OnClientClick="return ConfirmSure()" />
                <input type="button" id="btnRefresh" class="button_big" value='<%= Request.QueryString.Count==0 ? Resources.Labels.Clear: Resources.Labels.Reset %>'
                    onclick="window.location = window.location; return false;" />
                <div style="clear: both">
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfNewBatch" runat="server" />
        <asp:NewBatch ID="ucNewBatchID" runat="server" Title="<%$Resources:Labels,BatchID %>"
            Y="400" AttributeType_ID="14" TargetControlID="cph$lnkNewBatch" OnNewBatchCreated="ucNewBatchID_NewBatchCreated"></asp:NewBatch>
        <asp:HiddenField ID="hfxxx" runat="server" />
        <asp:ModalPopupExtender ID="mpexxx" runat="server" TargetControlID="hfxxx"
            PopupControlID="xxx" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
            BehaviorID="showPopUp" Y="250">
        </asp:ModalPopupExtender>
        <asp:Panel ID="xxx" CssClass="pnlPopUp" runat="server"
            Width="249">
            <div class="tcat">
                <asp:Button runat="server" class="close-btn" ID="Button13"></asp:Button>
                <span>
                    <%=MyContext.PageData.PageTitle%></span>
            </div>
            <div class="content">
                <asp:ABFTextBox ID="BarcodeNC" runat="server" LabelText="الباركودات التي لم يتم استرادها" Width="249" Height="200"
                    TextMode="MultiLine">
                </asp:ABFTextBox>
            </div>
            <div class="btnDiv">
                <asp:Button ID="Button8" runat="server" CssClass="button"
                    Text="<%$ Resources:Labels, Cancel %>" />
            </div>
        </asp:Panel>
</asp:Content>
