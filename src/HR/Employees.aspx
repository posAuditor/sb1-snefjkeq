<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Employees.aspx.cs" Inherits="HR_Employees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<%@ Register TagName="NewAttribute" Src="~/CustomControls/ucNewAttribute.ascx" TagPrefix="asp" %>


<%@ Register TagName="Nav" Src="~/CustomControls/ucNavigation.ascx"
    TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">


    <div class="InvoiceHeader">


        <asp:Nav runat="server" ID="ucNav" VisibleText="false" />


    </div>
    <div style="clear: both;">
    </div>



    <span>
        <%=Resources.Labels.Name %></span>:
    <asp:Label runat="server" ID="lblEmpName" Text="" Font-Bold="true"></asp:Label><br />
    <br />
    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue" Style="min-width: 900px;">
        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$Resources:Labels,BasicData %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                            IsRequired="true" ValidationGroup="NewEmployee"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtNameE" runat="server" LabelText="الاسم E"
                            ValidationGroup="NewEmployee"></asp:ABFTextBox>
                        <label>
                            <%=Resources.Labels.Gender %></label>
                        <asp:DropDownList ID="ddlGender" runat="server">
                            <asp:ListItem Text="<%$Resources:Labels,Male %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Labels,Female %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtNationalID" runat="server" LabelText="<%$Resources:Labels,NationalID %>"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtPassportID" runat="server" LabelText="<%$Resources:Labels,PassportID %>"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtDateOfBirth" runat="server" ValidationGroup="NewEmployee"
                            IsRequired="true" IsDateFiscalYearRestricted="false" DataType="Date" LabelText="<%$Resources:Labels,DateOfBirth %>"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtinsuranceNumber" runat="server" LabelText="رقم الاشتراك بالتامينات"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtJopNumber" runat="server" LabelText="الرقم الوظيفي"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtCodeEmployee" runat="server" LabelText="كود حساب الموظف"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtBankNumber" runat="server" LabelText="رقم الحساب البنكي"></asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtStatuessubscription" runat="server" LabelText="حالة مدة الإشتراك"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtNumberpassport" runat="server" LabelText="رقم الجواز"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtPrinterName" runat="server" LabelText="اسم الطابعه"></asp:ABFTextBox>

                        <br />

                        <label>نوع احتساب الراتب </label>
                        <asp:DropDownList ID="ddlTypeSalaryAdd" runat="server">
                            <asp:ListItem Text="قيمة" Value="0"></asp:ListItem>
                            <asp:ListItem Text="نسبة" Value="1"></asp:ListItem>
                        </asp:DropDownList>

                        <br />
                        <asp:CheckBox ID="CheckBox1" runat="server" Visible="false" /><span>   </span>
                        <br />
                        <asp:CheckBox ID="chkIsPermissionView" runat="server" /><span> عدم السماح بمشاهدة عمليات الاخرين    </span>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acMaritalStatus" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,MaritalStatus %>"></asp:AutoComplete>
                        <asp:Panel runat="server" Visible="false">
                            <asp:AutoComplete runat="server" ID="acMilitaryStatus" ServiceMethod="GetGeneralAtt"
                                LabelText="<%$Resources:Labels,MilitaryStatus %>"></asp:AutoComplete>
                        </asp:Panel>


                        <asp:AutoComplete runat="server" ID="acNationality" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,Nationality %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acReligion" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,Religion %>"></asp:AutoComplete>

                        <asp:ABFTextBox ID="acCity" runat="server" LabelText="<%$Resources:Labels,HRCity %>"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="acguarantor" runat="server" LabelText="<%$Resources:Labels,HRguarantor %>"></asp:ABFTextBox>



                        <asp:ABFTextBox ID="acBorderNumber" runat="server" LabelText="<%$Resources:Labels,HRBorderNumber %>"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtDateBorderIn" runat="server" ValidationGroup="NewEmployee"
                            IsDateFiscalYearRestricted="false" DataType="Date" LabelText="<%$Resources:Labels,HRDateBorderIn %>"></asp:ABFTextBox>


                    </div>
                    <div style="clear: both;">
                    </div>
                    <asp:ABFTextBox ID="txtNotes" runat="server" LabelText="<%$Resources:Labels,Notes %>" Style="box-sizing: border-box;"
                        TextMode="MultiLine" Width="100%"></asp:ABFTextBox>
                    <br />
                    <div class="right_col">
                        <label>
                            <%=Resources.Labels.Picture %></label>
                        <asp:FileUpload ID="fpLogo" runat="server" Width="200" accept="image/jpeg"></asp:FileUpload>
                        <asp:Button ID="btnUploadImage" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>" Style="min-width: 0px;"
                            OnClick="btnUploadImage_Click" />
                    </div>
                    <div class="left_col">
                        <asp:Image ID="imgLogo" Style="max-width: 200px; max-height: 150px" runat="server"
                            ImageUrl="~/Images/no_photo.png" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,ContactData %>">
            <ContentTemplate>
                <div class="form" style="width: 60%;">
                    <asp:AutoComplete runat="server" ID="acContactDataType" ServiceMethod="GetGeneralAtt"
                        LabelText="<%$Resources:Labels,Type %>" IsRequired="true" ValidationGroup="NewContactData"></asp:AutoComplete>
                    <asp:ABFTextBox ID="txtContactData" runat="server" LabelText="<%$Resources:Labels,Data %>"
                        TextMode="MultiLine" Height="50" IsRequired="true" ValidationGroup="NewContactData"></asp:ABFTextBox>

                    <div class="validationSummary">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewContactData" />
                    </div>
                    <br />
                    <div class="align_right">
                        <asp:Button ID="btnAddContactDetail" CssClass="button" runat="server" OnClick="btnAddContactDetail_click"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewContactData" />
                    </div>
                </div>
                <asp:ABFGridView runat="server" ID="gvContactData" GridViewStyle="GrayStyle" DataKeyNames="ID,Att_ID"
                    OnRowDeleting="gvContactData_RowDeleting" OnPageIndexChanging="gvContactData_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="AttName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="Data" HeaderText="<%$Resources:Labels,Data %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="<%$Resources:Labels,JobData %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtMachineID" runat="server" LabelText="<%$Resources:Labels,MachineID %>"></asp:ABFTextBox>
                        <asp:AutoComplete ID="acDepartment" ServiceMethod="GetHRDepartments" LabelText="<%$Resources:Labels,Department %>"
                            runat="server" IsRequired="true" ValidationGroup="NewEmployee" OnSelectedIndexChanged="acDepartment_SelectedIndexChanged"
                            AutoPostBack="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acPosition" ServiceMethod="GetHRPositions" LabelText="<%$Resources:Labels,Position %>"
                            IsRequired="true" ValidationGroup="NewEmployee" Enabled="false"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acJobDegree" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,JobDegree %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acEmploymentStatus" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,EmploymentStatus %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acSuperVisor" ServiceMethod="GetContactNames"
                            ValidationGroup="Save" LabelText="<%$Resources:Labels,Supervisor %>"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acBranch" ServiceMethod="GetBranchs" LabelText="<%$Resources:Labels,Branch %>" ValidationGroup="NewEmployee" OnSelectedIndexChanged="acBranch_SelectedIndexChanged" AutoPostBack="true"></asp:AutoComplete>
                        <asp:AutoComplete runat="server" ID="acCashierAccount_ID" ServiceMethod="GetChartOfAccounts"
                            LabelText="<%$Resources:Labels,DefaultCashAccount %>"></asp:AutoComplete>
                    </div>
                    <div class="left_col">
                        <asp:AutoComplete runat="server" ID="acShift" ServiceMethod="GetHRShifts" LabelText="<%$Resources:Labels,Shift %>"
                            IsRequired="true" ValidationGroup="NewEmployee"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtHiringDate" runat="server" LabelText="<%$Resources:Labels,HiringDate %>"
                            ValidationGroup="NewEmployee" IsRequired="true" DataType="Date" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtVisaStartDate" runat="server" LabelText="<%$Resources:Labels,VisaStartDate %>" IsDateFiscalYearRestricted="false"
                            ValidationGroup="NewEmployee" DataType="Date"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtVisaEndDate" runat="server" LabelText="<%$Resources:Labels,VisaEndDate %>" IsDateFiscalYearRestricted="false"
                            ValidationGroup="NewEmployee" DataType="Date"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtCasualVacations" runat="server" LabelText="<%$Resources:Labels,CasualVacations %>"
                            ValidationGroup="NewEmployee" IsRequired="true" DataType="Int"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtRegualrVacations" runat="server" LabelText="<%$Resources:Labels,RegualrVacations %>"
                            ValidationGroup="NewEmployee" IsRequired="true" DataType="Int"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acStore" ServiceMethod="GetStores" LabelText="<%$Resources:Labels,Store %>"></asp:AutoComplete>

                        <br />
                        <br />
                        <br />
                        <asp:CheckBox ID="chkIsSystmeUser" runat="server" /><span><%=Resources.Labels.SystemUser %></span>
                    </div>
                    <div style="clear: both;">
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="<%$Resources:Labels,FinancialData %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:ABFTextBox ID="txtBasicSalary" runat="server" IsRequired="true" ValidationGroup="NewEmployee"
                            DataType="Decimal" LabelText="<%$Resources:Labels,BasicSalary %>" MinValue="0.01"></asp:ABFTextBox>
                        <div class="" style="width: 96%;">
                            <div class="right_col">
                                <asp:ABFTextBox ID="txtParcent" runat="server" AutoPostBack="True" OnTextChanged="txtParcent_OnTextChanged" ValidationGroup="NewEmployee" DataType="Decimal" MinValue="0"
                                    LabelText="نسبة التأمين"></asp:ABFTextBox>
                            </div>
                            <div class="left_col">
                                <asp:ABFTextBox ID="txtInsurance" Enabled="False" runat="server" ValidationGroup="NewEmployee" DataType="Decimal" MinValue="0"
                                    LabelText="<%$Resources:Labels,Insurance %>"></asp:ABFTextBox>
                            </div>
                        </div>
                        <asp:ABFTextBox ID="txtAccountNumber" runat="server" LabelText="<%$Resources:Labels,AccountNumber %>"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtTarget" runat="server" ValidationGroup="NewEmployee" DataType="Decimal" MinValue="0" Visible="false"
                            LabelText="<%$Resources:Labels,Target %>"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acCostCenter" ServiceMethod="GetCostCenters"
                            LabelText="<%$Resources:Labels,CostCenter %>"></asp:AutoComplete>
                    </div>
                    <div class="left_col">
                        <%--<label>
                            <%=Resources.Labels.Currency %></label>--%>
                        <asp:DropDownList ID="ddlCurrency" runat="server" Visible="false">
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtRatio" runat="server" ValidationGroup="NewEmployee" LabelText="<%$Resources:Labels,Ratio %>"
                            DataType="Decimal" Enabled="false" Visible="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtOpenBalance" runat="server" ValidationGroup="NewEmployee"
                            LabelText="<%$Resources:Labels,OpenBalance %>" OnTextChanged="txtOpenBalance_TextChanged"
                            AutoPostBack="true" DataType="Decimal"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtStartFrom" runat="server" ValidationGroup="NewEmployee" LabelText="<%$Resources:Labels,StartFrom %>"
                            OnTextChanged="txtStartFrom_TextChanged" AutoPostBack="true" DataType="Date"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acOppsiteAccount" ServiceMethod="GetChartOfAccountsException"
                            LabelText="<%$Resources:Labels,OppositeAccount %>" Enabled="false"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtComPercentage" runat="server" ValidationGroup="NewEmployee" DataType="Decimal" MinValue="0"
                            LabelText="<%$Resources:Labels,ComissionPercentage %>"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtInvAdsComission" runat="server" ValidationGroup="NewEmployee" DataType="Decimal" MinValue="0"
                            LabelText="<%$Resources:Labels,InvAdsComission %>"></asp:ABFTextBox>
                    </div>
                    <div style="clear: both;">
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel9" runat="server" HeaderText="البدلات">
            <ContentTemplate>
                <div class="form" style="width: 90%;">


                    <div class="content">
                        <div class="form">
                            <%--  <asp:ABFTextBox ID="ABFTextBox1" CssClass="field" LabelText="<%$Resources:Labels,Name %>"
                                runat="server" IsRequired="true" ValidationGroup="Allowance"></asp:ABFTextBox>--%>
                            <label>
                                <%=Resources.Labels.Type %></label>

                            <asp:DropDownList ID="ddlAllowances" runat="server">
                            </asp:DropDownList>
                            <%--  <asp:DropDownList ID="ddlAllowances" runat="server">
                                <asp:ListItem Text="بدل سكن" Value="1"></asp:ListItem>
                                <asp:ListItem Text="بدل معيشة" Value="2"></asp:ListItem>
                                <asp:ListItem Text="بدل مواصلات" Value="3"></asp:ListItem>
                                <asp:ListItem Text="بدلات اخرى" Value="4"></asp:ListItem>
                            </asp:DropDownList>--%>
                            <asp:ABFTextBox ID="txtDailyAllowance" CssClass="field" LabelText="<%$Resources:Labels,DailyAllowance %>"
                                runat="server" IsRequired="true" ValidationGroup="Allowance"></asp:ABFTextBox>
                            <asp:ABFTextBox ID="txtMonthlyAllowance" CssClass="field" LabelText="<%$Resources:Labels,MonthlyAllowance %>"
                                runat="server" IsRequired="true" ValidationGroup="Allowance"></asp:ABFTextBox>

                            <asp:AutoComplete runat="server" ID="acAccountAllowance" ServiceMethod="GetChartOfAccounts"
                                LabelText="اسم الحساب"></asp:AutoComplete>

                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="Allowance" />
                        </div>
                        <div class="btnDiv">
                            <asp:Button ID="btnAllowanceSaveNew" CssClass="button default_button" runat="server" OnClick="btnAllowanceSaveNew_OnClick"
                                Text="<%$ Resources:Labels, Add %>" ValidationGroup="Allowance" />
                            <asp:Button ID="BtnClearNew" runat="server" CssClass="button" OnClick="BtnClearNew_OnClick"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                    <div style="clear: both"></div>
                    <br />
                    <br />
                    <asp:ABFGridView runat="server" ID="gvAllowance" GridViewStyle="GrayStyle" DataKeyNames="ID" OnRowDeleting="gvAllowance_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
                            <asp:BoundField DataField="TypeAllowanceName" HeaderText="نوع البدل" />
                            <asp:BoundField DataField="DailyAllowance" HeaderText="بدل يومي" />
                            <asp:BoundField DataField="MonthlyAllowance" HeaderText="بدل شهري" />
                            <asp:BoundField DataField="NameAccount" HeaderText="اسم الحساب" />
                            <%--<asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCommissionEdit" OnClick="lnkCommissionEdit_OnClick" Text="<%$ Resources:Labels, Edit %>"  CommandArgument='<%#Eval("ID") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:ABFGridView>

                </div>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel10" runat="server" HeaderText="الأهداف">
            <ContentTemplate>
                <div class="form" style="width: 90%;">



                    <asp:Panel runat="server" ID="Panel33" DefaultButton="btnSaveGoalCommission" CssClass="right_col">
                        <%-- <asp:AutoComplete runat="server" ID="acItemCommission" ServiceMethod="GetItems"
                            LabelText="<%$Resources:Labels,RawItem %>" IsRequired="true" ValidationGroup="Commission"></asp:AutoComplete>--%>
                        <asp:ABFTextBox ID="txtFirstValue" runat="server" LabelText="من" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtLastValue" runat="server" LabelText="إلى" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtPercent" runat="server" LabelText="النسبة" Width="70%"
                            DataType="Decimal" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>


                        <asp:ABFTextBox ID="txtFromDate" runat="server" LabelText="من تاريخ" Width="70%"
                            DataType="Date" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>

                        <asp:ABFTextBox ID="txtToDate" runat="server" LabelText=" الى تاريخ" Width="70%"
                            DataType="Date" IsRequired="true" ValidationGroup="Commission" MinValue="0.0001"></asp:ABFTextBox>


                        <asp:Button ID="btnSaveGoalCommission" CssClass="button" runat="server" Style="min-width: 0px"
                            Text="<%$ Resources:Labels, Add %>" ValidationGroup="Commission" OnClick="btnSaveGoalCommission_OnClick" />


                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummaryCommission" runat="server" ValidationGroup="Commission" />
                        </div>
                    </asp:Panel>

                    <div style="clear: both"></div>
                    <br />
                    <br />
                    <asp:ABFGridView runat="server" ID="gvCommission" GridViewStyle="GrayStyle" DataKeyNames="ID" OnRowDeleting="gvCommission_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
                            <asp:BoundField DataField="FromValue" HeaderText="من" />
                            <asp:BoundField DataField="ToValue" HeaderText="الى" />
                            <asp:BoundField DataField="FromDate" HeaderText="من تاريخ" />
                            <asp:BoundField DataField="ToDate" HeaderText="الى تاريخ" />
                            <asp:BoundField DataField="Parcent" HeaderText="النسبة" />

                            <%--<asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCommissionEdit" OnClick="lnkCommissionEdit_OnClick" Text="<%$ Resources:Labels, Edit %>"  CommandArgument='<%#Eval("ID") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:ABFGridView>



                </div>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel11" runat="server" HeaderText="الوثائق">
            <ContentTemplate>
                <div class="form" style="width: 90%;">


                    <div class="MainGrayDiv">
                        <div class="form">
                            <label>
                                <%=Resources.Labels.File %></label>
                            <asp:FileUpload ID="fpFile" runat="server" Width="400"></asp:FileUpload>
                            <asp:Button ID="btnUpload" runat="server" CssClass="button" Text="<%$ Resources:Labels, Upload %>"
                                OnClick="btnUpload_Click" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <asp:ABFGridView runat="server" ID="gvAttachments" GridViewStyle="GrayStyle" DataKeyNames="ID,Guid,FileName"
                        OnRowDeleting="gvAttachments_RowDeleting" OnPageIndexChanging="gvAttachments_PageIndexChanging"
                        PageSize="10" OnSelectedIndexChanging="gvAttachments_SelectedIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="FileName" HeaderText="<%$ Resources:Labels, FileName %>" />
                            <asp:BoundField DataField="FileSize" HeaderText="<%$ Resources:Labels, FileSize %>"
                                DataFormatString="{0:F2}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton Text="<%$Resources:Labels,Download %>" runat="server" CommandName="Select"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="../images/delete_grid.gif" runat="server"
                                        CommandName="Delete" OnClientClick="return ConfirmSure();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:ABFGridView>
                    <iframe id="ifDownload" runat="server" src="" style="display: none;"></iframe>




                </div>
            </ContentTemplate>
        </asp:TabPanel>


        <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="<%$Resources:Labels,Experiences %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <label>
                            <%=Resources.Labels.Type %></label>
                        <asp:DropDownList ID="ddlExpType" runat="server">
                            <asp:ListItem Text="<%$Resources:Labels,Training %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Labels,Work %>" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ABFTextBox ID="txtExpFromDate" runat="server" LabelText="<%$Resources:Labels,DateFrom %>"
                            DataType="Date" ValidationGroup="NewExp" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpToDate" runat="server" LabelText="<%$Resources:Labels,DateTo %>"
                            DataType="Date" ValidationGroup="NewExp" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtEmployerOrTrainer" runat="server" LabelText="<%$Resources:Labels,EmployerOrTrainer %>"
                            IsRequired="true" ValidationGroup="NewExp"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtJobOrTrainingName" runat="server" LabelText="<%$Resources:Labels,JobOrTrainingName %>"
                            IsRequired="true" ValidationGroup="NewExp"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpSalary" runat="server" LabelText="<%$Resources:Labels,Salary %>"
                            DataType="Decimal" ValidationGroup="NewExp"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtExpTerminationReason" runat="server" LabelText="<%$Resources:Labels,TerminationReason %>"
                            TextMode="MultiLine" Height="50"></asp:ABFTextBox>
                    </div>
                </div>
                <div style="clear: both; text-align: center; width: 90%;">
                    <br />
                    <br />
                    <asp:Button ID="btnAddExp" CssClass="button" runat="server" OnClick="btnAddExp_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewExp" />
                    <asp:Button ID="btnClearExp" CssClass="button" runat="server" OnClick="btnClearExp_click"
                        Text="<%$ Resources:Labels, Clear %>" />
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="NewExp" />
                </div>
                <asp:ABFGridView runat="server" ID="gvExperiences" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    PageSize="10" OnRowDeleting="gvExperiences_RowDeleting" OnPageIndexChanging="gvExperiences_PageIndexChanging"
                    OnSelectedIndexChanging="gvExperiences_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ExperienceTypeName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="FromDate" HeaderText="<%$Resources:Labels,DateFrom %>"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="ToDate" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Labels,EmployerOrTrainer %>" />
                        <asp:BoundField DataField="JobName" HeaderText="<%$Resources:Labels,JobOrTrainingName %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="<%$Resources:Labels,SkillsAndCertificates %>">
            <ContentTemplate>
                <div class="form" style="width: 550px;">
                    <label>
                        <%=Resources.Labels.Type %></label>
                    <asp:DropDownList ID="ddlSkillType" runat="server">
                        <asp:ListItem Text="<%$Resources:Labels,Language %>" Value="0"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Certificate %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,Skill %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ABFTextBox ID="txtSkillName" runat="server" LabelText="<%$Resources:Labels,Name %>"
                        IsRequired="true" ValidationGroup="NewSkill"></asp:ABFTextBox>
                    <asp:AutoComplete runat="server" ID="acSkillDegree" ServiceMethod="GetGeneralAtt"
                        LabelText="<%$Resources:Labels,Degree %>"></asp:AutoComplete>
                </div>
                <div style="clear: both; text-align: center; width: 550px;">
                    <br />
                    <br />
                    <asp:Button ID="btnAddSkill" CssClass="button" runat="server" OnClick="btnAddSkill_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewSkill" />
                    <asp:Button ID="btnClearSkill" CssClass="button" runat="server" OnClick="btnClearSkill_click"
                        Text="<%$ Resources:Labels, Clear %>" />
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="NewExp" />
                </div>
                <asp:ABFGridView runat="server" ID="gvSkills" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    PageSize="10" OnRowDeleting="gvSkills_RowDeleting" OnPageIndexChanging="gvSkills_PageIndexChanging"
                    OnSelectedIndexChanging="gvSkills_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="SkillTypeName" HeaderText="<%$Resources:Labels,Type %>" />
                        <asp:BoundField DataField="SkillName" HeaderText="<%$Resources:Labels,Name %>" />
                        <asp:BoundField DataField="DegreeName" HeaderText="<%$Resources:Labels,Degree %>" />
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
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel8" runat="server" HeaderText="<%$Resources:Labels,Education %>">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div class="right_col">
                        <asp:AutoComplete runat="server" ID="acEduQualification" ServiceMethod="GetGeneralAtt"
                            LabelText="<%$Resources:Labels,Qual %>" IsRequired="true" ValidationGroup="NewEdu"></asp:AutoComplete>
                        <asp:ABFTextBox ID="txtEduDateFrom" runat="server" LabelText="<%$Resources:Labels,DateFrom %>"
                            DataType="Date" ValidationGroup="NewEdu" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                        <asp:ABFTextBox ID="txtEduDateTo" runat="server" LabelText="<%$Resources:Labels,DateTo %>"
                            DataType="Date" ValidationGroup="NewEdu" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    </div>
                    <div class="left_col">
                        <asp:ABFTextBox ID="txtEduField" runat="server" LabelText="<%$Resources:Labels,Field %>"></asp:ABFTextBox>
                        <asp:AutoComplete runat="server" ID="acEduDegree" ServiceMethod="GetGeneralAtt" LabelText="<%$Resources:Labels,Degree %>"></asp:AutoComplete>
                    </div>
                </div>
                <div style="clear: both; text-align: center; width: 100%">
                    <br />
                    <br />
                    <asp:Button ID="btnAddEdu" CssClass="button" runat="server" OnClick="btnAddEdu_click"
                        Text="<%$ Resources:Labels, Add %>" ValidationGroup="NewEdu" />
                    <asp:Button ID="btnClearEdu" CssClass="button" runat="server" OnClick="btnClearEdu_click"
                        Text="<%$ Resources:Labels, Clear %>" />
                </div>
                <div class="validationSummary">
                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="NewExp" />
                </div>
                <asp:ABFGridView runat="server" ID="gvEducation" GridViewStyle="GrayStyle" DataKeyNames="ID"
                    PageSize="10" OnRowDeleting="gvEducation_RowDeleting" OnPageIndexChanging="gvEducation_PageIndexChanging"
                    OnSelectedIndexChanging="gvEducation_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="QualName" HeaderText="<%$Resources:Labels,Qual %>" />
                        <asp:BoundField DataField="FromDate" HeaderText="<%$Resources:Labels,DateFrom %>"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="ToDate" HeaderText="<%$Resources:Labels,DateTo %>" DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="FieldName" HeaderText="<%$Resources:Labels,Field %>" />
                        <asp:BoundField DataField="DegreeName" HeaderText="<%$Resources:Labels,Degree %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="<%$Resources:Labels,Termination %>">
            <ContentTemplate>
                <div class="form" style="width: 550px;">
                    <asp:ABFTextBox ID="txtTerminationDate" runat="server" LabelText="<%$Resources:Labels,TerminationDate %>"
                        ValidationGroup="NewEmployee" DataType="Date" IsDateFiscalYearRestricted="false"></asp:ABFTextBox>
                    <asp:ABFTextBox ID="txtTerminationReason" runat="server" LabelText="<%$Resources:Labels,TerminationReason %>"
                        TextMode="MultiLine" Height="60"></asp:ABFTextBox>
                </div>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="TabPanel12" runat="server" HeaderText="تكلفة الموظف">
            <ContentTemplate>
                <div class="form" style="width: 90%;">
                    <div>

                        <div class="right_col">


                            <label>
                                اسم المصروف</label>
                            <asp:DropDownList ID="ddlTypeEmployeExpense" runat="server" AutoPostBack="True" OnTextChanged="ddlTypeEmployeExpense_OnTextChanged">

                                <asp:ListItem Text="أقامة" Value="0"></asp:ListItem>
                                <asp:ListItem Text="تذكرة طيران" Value="1"></asp:ListItem>
                                <asp:ListItem Text="مصروفات مقدمة" Value="2"></asp:ListItem>
                                <asp:ListItem Text="بطاقة صراف" Value="3"></asp:ListItem>
                                <asp:ListItem Text="رخصة عمل" Value="4"></asp:ListItem>
                                <asp:ListItem Text="شهادة صحية" Value="5"></asp:ListItem>
                                <asp:ListItem Text="تأمين صحي" Value="6"></asp:ListItem>
                                <asp:ListItem Text="رسوم اصدارخروج وعودة" Value="7"></asp:ListItem>
                                <asp:ListItem Text="رسوم رخصة قيادة" Value="8"></asp:ListItem>
                                <asp:ListItem Text="رسوم تعليم قيادة" Value="13"></asp:ListItem>
                                <asp:ListItem Text="رسوم إصدار تأشيرة" Value="14"></asp:ListItem>
                                <asp:ListItem Text="تأمينات أجتماعية" Value="9"></asp:ListItem>
                                <asp:ListItem Text="مكافئات" Value="10"></asp:ListItem>
                                <asp:ListItem Text="علاج" Value="11"></asp:ListItem>
                                <asp:ListItem Text="مصروفات أخرى" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <label>
                                حالة المصروف</label>
                            <asp:DropDownList ID="ddlTypeExpense" runat="server">
                                <asp:ListItem Text="اصدار جديد" Value="0"></asp:ListItem>
                                <asp:ListItem Text="تجديد" Value="1"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:ABFTextBox ID="txtDailyEmployeExpense" CssClass="field" Visible="False" LabelText="مصروف يومي"
                                runat="server" IsRequired="true" ValidationGroup="EmployeExpense"></asp:ABFTextBox>
                            <label>
                                نوع المصروف</label>
                            <asp:DropDownList ID="ddlType" runat="server">

                                <asp:ListItem Text="حالي" Value="0"></asp:ListItem>
                                <asp:ListItem Text="مقدم" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ABFTextBox ID="txtNote" CssClass="field" LabelText="الملاحظات"
                                runat="server" ValidationGroup="EmployeExpense" DataType="FreeString"></asp:ABFTextBox>
                        </div>
                        <div class="left_col">

                            <asp:ABFTextBox ID="txtMonthlyEmployeExpense" DataType="Decimal" CssClass="field" LabelText="المبلغ"
                                runat="server" IsRequired="true" ValidationGroup="EmployeExpense"></asp:ABFTextBox>
                            <asp:ABFTextBox ID="txtDate" OnTextChanged="txtDate_OnTextChanged" AutoPostBack="True" CssClass="field" LabelText="بتاريخ"
                                runat="server" IsRequired="true" ValidationGroup="EmployeExpense" DataType="Date"></asp:ABFTextBox>

                            <asp:ABFTextBox ID="txtNbrYear" AutoPostBack="true" OnTextChanged="txtNbrYear_TextChanged" DataType="Int" CssClass="field" LabelText="عدد الايام"
                                runat="server" IsRequired="true" ValidationGroup="EmployeExpense" Text="1" Width="50"></asp:ABFTextBox>

                            <asp:ABFTextBox ID="txtDateFinish" Enabled="False" CssClass="field" LabelText="تاريخ الانتهاء"
                                runat="server" ValidationGroup="EmployeExpense" DataType="Date"></asp:ABFTextBox>

                        </div>
                        <div style="clear: both;">
                            <br />
                        </div>
                        <div class="validationSummary">
                            <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="EmployeExpense" />
                        </div>
                        <div class="btnDiv">
                            <asp:Button ID="btnEmployeExpenseAdd" CssClass="button default_button" runat="server" OnClick="btnEmployeExpenseAdd_OnClick"
                                Text="<%$ Resources:Labels, Add %>" ValidationGroup="EmployeExpense" />
                            <asp:Button ID="ntnEmployeExpenseClear" runat="server" CssClass="button" OnClick="ntnEmployeExpenseClear_OnClick"
                                Text="<%$ Resources:Labels, Clear %>" />
                            <asp:Button ID="btnPrint" runat="server" CssClass="button" OnClick="btnPrint_OnClick"
                                Text="<%$ Resources:Labels, Print %>" />
                        </div>

                        <div style="clear: both"></div>
                        <br />
                        <br />
                        <asp:ABFGridView runat="server" ID="gvEmployeExpense" GridViewStyle="GrayStyle" DataKeyNames="ID" OnRowDeleting="gvEmployeExpense_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ContactName" HeaderText="<%$Resources:Labels,Name %>" />
                                <asp:BoundField DataField="TypeEmployeExpenseName" HeaderText="نوع المصروف" />
                                <asp:BoundField DataField="DateExpense" HeaderText="التاريخ" />
                                <asp:BoundField DataField="DateExpire" HeaderText="تاريخ الانتهاء" />
                                <asp:BoundField DataField="TypeExpenseName" HeaderText="النوع" />
                                <asp:BoundField DataField="TypeStatusExpenseName" HeaderText="الحالة" />
                                <asp:BoundField DataField="MonthlyEmployeExpense" HeaderText="مصروف شهري" />
                                <%--    <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCommissionPrint" OnClick="lnkCommissionPrint_OnClick" Text="<%$ Resources:Labels, Edit %>" CommandArgument='<%#Eval("ID") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                            CommandName="Delete" OnClientClick="return ConfirmSure();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:ABFGridView>



                    </div>
            </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>
    <br />
    <br />
    <div class="validationSummary">
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="NewEmployee" />
    </div>
    <div class="btnDiv">
        <asp:Button ID="btnSaveEmployee" CssClass="button shortcut_save" runat="server" OnClick="btnSaveEmployee_click"
            Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewEmployee" />
        <asp:Button ID="BtnReturn" runat="server" CssClass="button" Text="<%$ Resources:Labels, Return %>"
            OnClick="BtnReturn_Click" />
    </div>
</asp:Content>
