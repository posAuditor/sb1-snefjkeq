using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;

public partial class HR_Employees : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtContactData
    {
        get
        {
            if (Session["dtEmpContactData" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Att_ID", typeof(int));
                dt.Columns.Add("AttName", typeof(string));
                dt.Columns.Add("Data", typeof(string));
                Session["dtEmpContactData" + this.WinID] = dt;
            }
            return (DataTable)Session["dtEmpContactData" + this.WinID];
        }

        set
        {
            Session["dtEmpContactData" + this.WinID] = value;
        }
    }

    private DataTable dtExperienceData
    {
        get
        {
            if (Session["dtExperienceData" + this.WinID] == null)
            {
                Session["dtExperienceData" + this.WinID] = dc.usp_HR_Experiences_Select(0).CopyToDataTable();
            }
            return (DataTable)Session["dtExperienceData" + this.WinID];
        }

        set
        {
            Session["dtExperienceData" + this.WinID] = value;
        }
    }

    private DataTable dtEducation
    {
        get
        {
            if (Session["dtEducation" + this.WinID] == null)
            {
                Session["dtEducation" + this.WinID] = dc.usp_HR_Education_Select(0).CopyToDataTable();
            }
            return (DataTable)Session["dtEducation" + this.WinID];
        }

        set
        {
            Session["dtEducation" + this.WinID] = value;
        }
    }

    private DataTable dtSkills
    {
        get
        {
            if (Session["dtSkills" + this.WinID] == null)
            {
                Session["dtSkills" + this.WinID] = dc.usp_HR_Skills_Select(0).CopyToDataTable();
            }
            return (DataTable)Session["dtSkills" + this.WinID];
        }

        set
        {
            Session["dtSkills" + this.WinID] = value;
        }
    }

    private string ImageUrl
    {
        get
        {
            return (string)ViewState["ImageUrl"];
        }

        set
        {
            ViewState["ImageUrl"] = value;
        }
    }

    private int Contact_ID
    {
        get
        {
            if (ViewState["Contact_ID"] == null) return 0;
            return (int)ViewState["Contact_ID"];
        }

        set
        {
            ViewState["Contact_ID"] = value;
        }
    }

    private int EmployeeUnderRequestID
    {
        get
        {
            if (ViewState["EmployeeUnderRequestID"] == null) return 0;
            return (int)ViewState["EmployeeUnderRequestID"];
        }

        set
        {
            ViewState["EmployeeUnderRequestID"] = value;
        }
    }

    private bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null) return false;
            return (bool)ViewState["EditMode"];
        }

        set
        {
            ViewState["EditMode"] = value;
        }
    }

    private int ExperienceEditID
    {
        get
        {
            if (ViewState["ExperienceEditID"] == null) return 0;
            return (int)ViewState["ExperienceEditID"];
        }

        set
        {
            ViewState["ExperienceEditID"] = value;
        }
    }

    private int EducationEditID
    {
        get
        {
            if (ViewState["EducationEditID"] == null) return 0;
            return (int)ViewState["EducationEditID"];
        }

        set
        {
            ViewState["EducationEditID"] = value;
        }
    }

    private int SkillEditID
    {
        get
        {
            if (ViewState["SkillEditID"] == null) return 0;
            return (int)ViewState["SkillEditID"];
        }

        set
        {
            ViewState["SkillEditID"] = value;
        }
    }




    private DataTable dtGoalCommission
    {
        get
        {
            if (Session["dtGoalCommission" + this.WinID] == null)
            {
                Session["dtGoalCommission" + this.WinID] = dc.usp_GoalCommission_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtGoalCommission" + this.WinID];
        }

        set
        {
            Session["dtGoalCommission" + this.WinID] = value;
        }
    }



    private DataTable dtAllowance
    {
        get
        {
            if (Session["dtAllowance" + this.WinID] == null)
            {
                Session["dtAllowance" + this.WinID] = dc.usp_EmployeAllowance_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtAllowance" + this.WinID];
        }

        set
        {
            Session["dtAllowance" + this.WinID] = value;
        }
    }

    private DataTable dtEmployeExpense
    {
        get
        {
            if (Session["dtEmployeExpense" + this.WinID] == null)
            {
                Session["dtEmployeExpense" + this.WinID] = dc.usp_EmployeExpense_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtEmployeExpense" + this.WinID];
        }

        set
        {
            Session["dtEmployeExpense" + this.WinID] = value;
        }
    }

    #endregion

    #region Conrtol Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ifDownload.Attributes.Remove("src");
            fpFile.Attributes.Add("multiple", "multiple");
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUploadImage);

            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpload);
            this.SetEditMode();
            this.CheckSecurity();

            if (!Page.IsPostBack)
            {
                ChangeDate();
                this.LoadControls();
                if (this.EditMode) this.FillEmployeeData();
                if (!this.EditMode) this.FillFromEmployeeUnderRequest();

                //"~/HR/Employees.aspx?ID=5"

                var context = dc.usp_MyContext_select("~/HR/Employees.aspx", Request["DocumentPathInfo"].ToExpressString(), new Guid(Membership.GetUser().ProviderUserKey.ToExpressString())).FirstOrDefault();

                this.Fill();
                if (Request["guid"] != null && Request["filename"] != null) this.Download();
            }


            ucNav.SourceDocTypeType_ID = 90;
            ucNav.EntryType = 3;
            ucNav.Res_ID = this.Contact_ID;
            ucNav.btnHandler += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandler);
            ucNav.btnHandlerPrev += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerPrev);
            ucNav.btnHandlerFirst += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerFirst);
            ucNav.btnHandlerLast += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerLast);
            ucNav.btnHandlerAddNew += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerAddNew);
            ucNav.btnHandlerSearch += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerSearch);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }


    void ucNav_btnHandler(string strValue)
    {

        RefillForm(strValue);
    }
    void ucNav_btnHandlerPrev(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerFirst(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerLast(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerAddNew(string strValue)
    {
        Response.Redirect(PageLinks.Employees);
    }
    void ucNav_btnHandlerSearch(string strValue)
    {
        RefillForm(strValue);
    }

    private void RefillForm(string strValue)
    {
        if (!string.IsNullOrEmpty(strValue))
        {
            this.Contact_ID = strValue.ToInt();
            // this.EditMode = strValue.ToInt();
            this.EditMode = true;

            ChangeDate();
            this.LoadControls();
            if (this.EditMode) this.FillEmployeeData();
            if (!this.EditMode) this.FillFromEmployeeUnderRequest();

            //"~/HR/Employees.aspx?ID=5"

            var context = dc.usp_MyContext_select("~/HR/Employees.aspx", Request["DocumentPathInfo"].ToExpressString(), new Guid(Membership.GetUser().ProviderUserKey.ToExpressString())).FirstOrDefault();

            this.Fill();
            if (Request["guid"] != null && Request["filename"] != null) this.Download();
        }
    }




    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    protected void txtStartFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtStartFrom.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOpenBalance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acBranch.IsRequired = txtRatio.IsRequired = txtStartFrom.IsRequired = txtOpenBalance.IsNotEmpty;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acPosition.Clear();
            acPosition.Enabled = acDepartment.HasValue;
            acPosition.ContextKey = string.Empty + acDepartment.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveEmployee_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        int ChartofAccount_ID = 0;
        try
        {
            var company = dc.usp_Company_Select().FirstOrDefault();

            if (txtStartFrom.Text.ToDate() > DateTime.Now.Date && txtOpenBalance.Text.ToDecimalOrDefault() > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                return;
            }

            if (txtHiringDate.Text.ToDate().Value.Subtract(txtDateOfBirth.Text.ToDate().Value).TotalDays < 6570)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.HiringDateLimit, string.Empty);
                trans.Rollback();
                return;
            }

            if (!this.EditMode) //insert
            {
                this.Contact_ID = dc.usp_Contact_Insert(acBranch.Value.ToNullableInt(), company.Currency_ID, DocSerials.Employee.ToInt(), txtName.TrimmedText, 'E', txtNotes.Text, this.ImageUrl);
                string AccountName = txtName.TrimmedText + " - راتب مستحق";
                ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(AccountName, AccountName, company.HRAccruedSalariesAccountID, true, acBranch.Value.ToNullableInt(), company.Currency_ID, 1, txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt());

                if (ChartofAccount_ID == -2 || this.Contact_ID == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    return;
                }
                int result = dc.usp_HR_Employees_Insert(this.Contact_ID, ddlGender.SelectedValue.ToByte(), txtNationalID.TrimmedText, txtDateOfBirth.Text.ToDate(), acMaritalStatus.Value.ToNullableInt(),
                     acMilitaryStatus.Value.ToNullableInt(), acNationality.Value.ToNullableInt(), acReligion.Value.ToNullableInt(), txtMachineID.TrimmedText, acPosition.Value.ToNullableInt(),
                     acJobDegree.Value.ToNullableInt(), acEmploymentStatus.Value.ToNullableInt(), acShift.Value.ToNullableInt(), txtHiringDate.Text.ToDate(), txtVisaStartDate.Text.ToDate(),
                     txtVisaEndDate.Text.ToDate(), txtRegualrVacations.Text.ToNullableInt(), txtCasualVacations.Text.ToNullableInt(), chkIsSystmeUser.Checked, txtBasicSalary.Text.ToDecimalOrDefault(),
                     txtAccountNumber.TrimmedText, txtInsurance.Text.ToDecimalOrDefault(),
                     ChartofAccount_ID, acSuperVisor.Value.ToNullableInt(), txtPassportID.TrimmedText,
                     txtTerminationDate.Text.ToDate(), txtTerminationReason.TrimmedText,
                     acCashierAccount_ID.Value.ToNullableInt(), txtComPercentage.Text.ToDecimalOrDefault(),
                     txtTarget.Text.ToDecimalOrDefault(), acCostCenter.Value.ToNullableInt(),
                     txtInvAdsComission.Text.ToDecimalOrDefault(),
                     acStore.Value.ToNullableInt(), acCity.TrimmedText, acguarantor.TrimmedText,
                       acBorderNumber.TrimmedText, txtDateBorderIn.Text.ToDate(), txtStatuessubscription.TrimmedText, txtNumberpassport.TrimmedText,
                            txtinsuranceNumber.TrimmedText, txtJopNumber.TrimmedText, txtCodeEmployee.TrimmedText, txtBankNumber.TrimmedText, txtNameE.TrimmedText);
                if (result == -3)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.MachineIDExists, string.Empty);
                    trans.Rollback();
                    return;
                }
            }
            else // update
            {
                int result = dc.usp_HR_Employees_Update(this.Contact_ID, txtName.TrimmedText, company.Currency_ID, 1, acBranch.Value.ToNullableInt(), txtNotes.Text, this.ImageUrl, ddlGender.SelectedValue.ToByte(), txtNationalID.TrimmedText, txtDateOfBirth.Text.ToDate(), acMaritalStatus.Value.ToNullableInt(),
                    acMilitaryStatus.Value.ToNullableInt(), acNationality.Value.ToNullableInt(), acReligion.Value.ToNullableInt(), txtMachineID.TrimmedText, acPosition.Value.ToNullableInt(),
                    acJobDegree.Value.ToNullableInt(), acEmploymentStatus.Value.ToNullableInt(), acShift.Value.ToNullableInt(), txtHiringDate.Text.ToDate(), txtVisaStartDate.Text.ToDate(),
                    txtVisaEndDate.Text.ToDate(), txtRegualrVacations.Text.ToNullableInt(), txtCasualVacations.Text.ToNullableInt(), chkIsSystmeUser.Checked, txtBasicSalary.Text.ToDecimalOrDefault(),
                    txtAccountNumber.TrimmedText, txtInsurance.Text.ToDecimalOrDefault(), acSuperVisor.Value.ToNullableInt(), txtPassportID.TrimmedText, txtTerminationDate.Text.ToDate(), txtTerminationReason.TrimmedText, txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt(), acCashierAccount_ID.Value.ToNullableInt(), txtComPercentage.Text.ToDecimalOrDefault(), txtTarget.Text.ToDecimalOrDefault(), acCostCenter.Value.ToNullableInt(),
                    txtInvAdsComission.Text.ToDecimalOrDefault(), acStore.Value.ToNullableInt(),
                    acCity.TrimmedText, acguarantor.TrimmedText, acBorderNumber.TrimmedText
                    , txtDateBorderIn.Text.ToDate(), txtStatuessubscription.TrimmedText, txtNumberpassport.TrimmedText,
                            txtinsuranceNumber.TrimmedText, txtJopNumber.TrimmedText, txtCodeEmployee.TrimmedText, txtBankNumber.TrimmedText, txtNameE.TrimmedText);


                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    return;
                }
                if (result == -3)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.MachineIDExists, string.Empty);
                    trans.Rollback();
                    return;
                }
            }

            var emp = dc.HR_Employees.Where(x => x.Contact_ID == this.Contact_ID);
            if (emp != null)
            {
                if (emp.Any())
                {
                    var ep = emp.FirstOrDefault();
                    ep.PrinterName = txtPrinterName.Text.ToExpressString();
                    ep.HasPrintCuisin = CheckBox1.Checked;
                    ep.IsPermissionShow = chkIsPermissionView.Checked;
                    ep.TypeOfSalaryAdd = ddlTypeSalaryAdd.SelectedValue.ToInt();
                }
            }
            //Contact Data
            foreach (DataRow r in this.dtContactData.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ContactDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ContactDetails_insert(this.Contact_ID, r["Att_ID"].ToInt(), r["Data"].ToExpressString());
                }
            }
            //Expericens
            foreach (DataRow r in this.dtExperienceData.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Experiences_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_Experiences_Insert(this.Contact_ID, r["ExperienceType"].ToByte(), r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["companyName"].ToExpressString(), r["jobName"].ToExpressString(), r["Salary"].ToDecimal(), r["TerminationReason"].ToExpressString());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_Experiences_Update(r["ID"].ToInt(), r["ExperienceType"].ToByte(), r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["companyName"].ToExpressString(), r["jobName"].ToExpressString(), r["Salary"].ToDecimal(), r["TerminationReason"].ToExpressString());
                }
            }

            //Skills
            foreach (DataRow r in this.dtSkills.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Skills_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_Skills_Insert(this.Contact_ID, r["SkillType"].ToByte(), r["SkillName"].ToExpressString(), r["Degree_ID"].ToNullableInt());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_Skills_Update(r["ID"].ToInt(), r["SkillType"].ToByte(), r["SkillName"].ToExpressString(), r["Degree_ID"].ToNullableInt());
                }
            }

            //Education
            foreach (DataRow r in this.dtEducation.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Education_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_Education_Insert(this.Contact_ID, r["Qual_ID"].ToInt(), r["FieldName"].ToExpressString(), r["Degree_ID"].ToNullableInt(), r["FromDate"].ToDate(), r["ToDate"].ToDate());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_Education_Update(r["ID"].ToInt(), r["Qual_ID"].ToInt(), r["FieldName"].ToExpressString(), r["Degree_ID"].ToNullableInt(), r["FromDate"].ToDate(), r["ToDate"].ToDate());
                }
            }
            //goal Commision
            foreach (DataRow r in this.dtGoalCommission.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsItemCommission_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_GoalCommission_Insert(this.Contact_ID, r["FromValue"].ToInt(), r["ToValue"].ToInt(), r["Parcent"].ToDecimal(), r["FromDate"].ToDate(), r["ToDate"].ToDate());
                }
                if (r.RowState != DataRowState.Deleted)
                {
                    if (r["FromValue"].ToInt() > r["ToValue"].ToInt())
                    {
                        UserMessages.Message(null, "الرقم الاول اصغر من الرقم الثاني", string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }
            //Allowance
            foreach (DataRow r in this.dtAllowance.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_EmployeAllowance_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_EmployeAllowance_Insert(this.Contact_ID, r["TypeAllowance"].ToInt(), r["DailyAllowance"].ToDecimal(), r["MonthlyAllowance"].ToDecimal(), r["Account_ID"].ToInt());
                }

            }
            //Allowance
            foreach (DataRow r in this.dtEmployeExpense.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_EmployeExpense_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_EmployeExpense_Insert(this.Contact_ID, r["TypeEmployeExpense"].ToInt(), 0, r["MonthlyEmployeExpense"].ToDecimal(), r["TypeExpense"].ToInt(), r["TypeStatusExpense"].ToInt(), r["DateExpense"].ToDate(), r["DateExpire"].ToDate(), r["Note"].ToExpressString());



                }
            }
            //Delete under request employee
            dc.usp_HR_EmployeesUnderRequest_Delete(this.EmployeeUnderRequestID);

            LogAction(this.EditMode ? Actions.Edit : Actions.Add, txtName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.Employees + "?ID=" + this.Contact_ID.ToExpressString(), PageLinks.EmployeesList, PageLinks.Employees);
            trans.Commit();
            dc.SubmitChanges();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(PageLinks.EmployeesList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvContactData.PageIndex = e.NewPageIndex;
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvContactData.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtContactData.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddContactDetail_click(object sender, EventArgs e)
    {
        try
        {
            this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), acContactDataType.Value, acContactDataType.Text, txtContactData.TrimmedText);
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
            acContactDataType.Clear();
            txtContactData.Clear();
            acContactDataType.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnUploadImage_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fpLogo.HasFile) return;
            string fileName = null;
            do
            {
                fileName = Guid.NewGuid() + Path.GetExtension(fpLogo.PostedFile.FileName);
            }
            while (File.Exists(Server.MapPath("~\\uploads\\" + fileName)));

            Bitmap originalBMP = new Bitmap(fpLogo.FileContent);
            Bitmap newBMP = new Bitmap(originalBMP, 200, 150);
            Graphics objGraphics = Graphics.FromImage(newBMP);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            objGraphics.DrawImage(originalBMP, 0, 0, 200, 150);
            newBMP.Save(Server.MapPath("~\\uploads\\" + fileName)); ;
            imgLogo.ImageUrl = "~/uploads/" + fileName;
            this.ImageUrl = fileName;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExperiences_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExperiences.PageIndex = e.NewPageIndex;
            gvExperiences.DataSource = this.dtExperienceData;
            gvExperiences.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExperiences_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.ExperienceEditID = gvExperiences.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtExperienceData.Select("ID=" + this.ExperienceEditID.ToExpressString())[0];

            ddlExpType.SelectedValue = r["ExperienceType"].ToExpressString();
            if (r["FromDate"].ToExpressString() != string.Empty) txtExpFromDate.Text = r["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            if (r["ToDate"].ToExpressString() != string.Empty) txtExpToDate.Text = r["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            txtExpSalary.Text = r["Salary"].ToExpressString();
            txtEmployerOrTrainer.Text = r["CompanyName"].ToExpressString();
            txtJobOrTrainingName.Text = r["JobName"].ToExpressString();
            txtExpTerminationReason.Text = r["TerminationReason"].ToExpressString();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExperiences_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvExperiences.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtExperienceData.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvExperiences.DataSource = this.dtExperienceData;
            gvExperiences.DataBind();
            this.ClearExpForm();
            this.ExperienceEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddExp_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.ExperienceEditID == 0)
            {
                r = this.dtExperienceData.NewRow();
                r["ID"] = this.dtExperienceData.GetID("ID");

            }
            else
            {
                r = this.dtExperienceData.Select("ID=" + this.ExperienceEditID)[0];
            }

            r["ExperienceType"] = ddlExpType.SelectedValue.ToByte();
            r["ExperienceTypeName"] = ddlExpType.SelectedItem.Text;
            r["FromDate"] = txtExpFromDate.Text.ToDateOrDBNULL();
            r["ToDate"] = txtExpToDate.Text.ToDateOrDBNULL();
            r["Salary"] = txtExpSalary.Text.ToDecimalOrDefault();
            r["CompanyName"] = txtEmployerOrTrainer.Text;
            r["JobName"] = txtJobOrTrainingName.Text;
            r["TerminationReason"] = txtExpTerminationReason.Text;
            if (this.ExperienceEditID == 0)
            {
                this.dtExperienceData.Rows.Add(r);
            }
            gvExperiences.DataSource = this.dtExperienceData;
            gvExperiences.DataBind();
            this.ClearExpForm();
            this.ExperienceEditID = 0;
            ddlExpType.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearExp_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearExpForm();
            this.ExperienceEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void gvSkills_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSkills.PageIndex = e.NewPageIndex;
            gvSkills.DataSource = this.dtSkills;
            gvSkills.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSkills_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.SkillEditID = gvSkills.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtSkills.Select("ID=" + this.SkillEditID.ToExpressString())[0];

            ddlSkillType.SelectedValue = r["SkillType"].ToExpressString();
            txtSkillName.Text = r["SkillName"].ToExpressString();
            acSkillDegree.Value = r["degree_ID"].ToStringOrEmpty();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSkills_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvSkills.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtSkills.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvSkills.DataSource = this.dtSkills;
            gvSkills.DataBind();
            this.ClearSkillsForm();
            this.SkillEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddSkill_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.SkillEditID == 0)
            {
                r = this.dtSkills.NewRow();
                r["ID"] = this.dtSkills.GetID("ID");

            }
            else
            {
                r = this.dtSkills.Select("ID=" + this.SkillEditID)[0];
            }

            r["SkillType"] = ddlSkillType.SelectedValue.ToByte();
            r["SkillTypeName"] = ddlSkillType.SelectedItem.Text;
            r["SkillName"] = txtSkillName.TrimmedText;
            r["Degree_ID"] = acSkillDegree.Value.ToIntOrDBNULL();
            r["DegreeName"] = acSkillDegree.Text;
            if (this.SkillEditID == 0)
            {
                this.dtSkills.Rows.Add(r);
            }
            gvSkills.DataSource = this.dtSkills;
            gvSkills.DataBind();
            this.ClearSkillsForm();
            this.SkillEditID = 0;
            ddlSkillType.Focus();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSkill_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearSkillsForm();
            this.SkillEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void gvEducation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvEducation.PageIndex = e.NewPageIndex;
            gvEducation.DataSource = this.dtEducation;
            gvEducation.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEducation_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EducationEditID = gvEducation.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtEducation.Select("ID=" + this.EducationEditID.ToExpressString())[0];

            if (r["FromDate"].ToExpressString() != string.Empty) txtEduDateFrom.Text = r["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            if (r["ToDate"].ToExpressString() != string.Empty) txtEduDateTo.Text = r["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            txtEduField.Text = r["FieldName"].ToExpressString();
            acEduQualification.Value = r["Qual_ID"].ToExpressString();
            acEduDegree.Value = r["Degree_ID"].ToStringOrEmpty();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEducation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvEducation.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtEducation.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvEducation.DataSource = this.dtEducation;
            gvEducation.DataBind();
            this.ClearEduForm();
            this.EducationEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddEdu_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.EducationEditID == 0)
            {
                r = this.dtEducation.NewRow();
                r["ID"] = this.dtEducation.GetID("ID");

            }
            else
            {
                r = this.dtEducation.Select("ID=" + this.EducationEditID)[0];
            }

            r["Qual_ID"] = acEduQualification.Value.ToInt();
            r["FieldName"] = txtEduField.TrimmedText;
            r["Degree_ID"] = acEduDegree.Value.ToIntOrDBNULL();
            r["FromDate"] = txtEduDateFrom.Text.ToDateOrDBNULL();
            r["ToDate"] = txtEduDateTo.Text.ToDateOrDBNULL();
            r["DegreeName"] = acEduDegree.Text;
            r["QualName"] = acEduQualification.Text;
            if (this.EducationEditID == 0)
            {
                this.dtEducation.Rows.Add(r);
            }
            gvEducation.DataSource = this.dtEducation;
            gvEducation.DataBind();
            this.ClearEduForm();
            this.EducationEditID = 0;
            acEduQualification.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearEdu_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearEduForm();
            this.EducationEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acBranch_SelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        acCashierAccount_ID.ContextKey = this.MyContext.CurrentCulture.ToInt().ToExpressString() + "," + acBranch.Value + ",," + COA.CashOnHand.ToInt().ToExpressString() + ",true";
        acStore.ContextKey = string.Empty + acBranch.Value;
        acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
        if (sender != null) this.FocusNextControl(sender);
    }

    #endregion

    #region Private Methods

    private void FillFromEmployeeUnderRequest()
    {
        if (this.EmployeeUnderRequestID == 0) return;
        var employee = dc.usp_HR_EmployeesUnderRequest_Select(this.EmployeeUnderRequestID, string.Empty, null, null, null).FirstOrDefault();

        txtName.Text = employee.Name;
        //acQualification.Value = employee.Qual_ID.ToStringOrEmpty();
        //acDegree.Value = employee.Degree_ID.ToStringOrEmpty();
        //txtUniversity.Text = employee.University;
        txtNationalID.Text = employee.NationalID;
        txtPassportID.Text = employee.PassportID;

        acDepartment.Value = employee.Department_ID.ToStringOrEmpty();
        this.acDepartment_SelectedIndexChanged(null, null);
        acPosition.Value = employee.Position_ID.ToStringOrEmpty();

        if (employee.Tel1 != string.Empty) this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), 4, "التليفون الافتراضى", employee.Tel1);
        if (employee.Tel2 != string.Empty) this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), 4, "التليفون الافتراضى", employee.Tel2);
        if (employee.Email != string.Empty) this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), 11, "بريد الكترونى", employee.Email);
        if (employee.Address != string.Empty) this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), 3, "العنوان الافتراضى", employee.Address);
        gvContactData.DataSource = this.dtContactData;
        gvContactData.DataBind();

        if (employee.Qual_ID != null)
        {
            DataRow r = this.dtEducation.NewRow();
            r["ID"] = this.dtEducation.GetID("ID");
            r["Qual_ID"] = employee.Qual_ID;
            r["QualName"] = dc.usp_GeneralAttributes_Select(MyContext.CurrentCulture.ToByte(), 18).Where(x => x.ID == employee.Qual_ID).FirstOrDefault().Name;
            this.dtEducation.Rows.Add(r);
            gvEducation.DataSource = this.dtEducation;
            gvEducation.DataBind();
        }


        if (File.Exists(Server.MapPath("~/Uploads/" + employee.PhotoUrl)))
        {
            imgLogo.ImageUrl = "~/Uploads/" + employee.PhotoUrl;
            this.ImageUrl = employee.PhotoUrl;
        }
    }

    private void ClearExpForm()
    {
        txtExpFromDate.Clear();
        txtExpToDate.Clear();
        txtJobOrTrainingName.Clear();
        txtExpTerminationReason.Clear();
        txtEmployerOrTrainer.Clear();
        txtExpSalary.Clear();
    }

    private void ClearEduForm()
    {
        acEduQualification.Clear();
        txtEduDateFrom.Clear();
        txtEduDateTo.Clear();
        txtEduField.Clear();
        acEduDegree.Clear();
    }

    private void ClearSkillsForm()
    {
        acSkillDegree.Clear();
        txtSkillName.Clear();
    }

    private void LoadControls()
    {
        this.dtContactData = null;
        this.dtExperienceData = null;
        this.dtSkills = null;
        this.dtEducation = null;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        acDepartment.ContextKey = string.Empty;
        acShift.ContextKey = string.Empty;
        acSuperVisor.ContextKey = "E," + MyContext.UserProfile.Branch_ID.ToNullableInt() + ",,";
        acJobDegree.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.JobDegree.ToInt().ToExpressString();
        acEmploymentStatus.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.EmploymentStatus.ToInt().ToExpressString();
        acMaritalStatus.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.MaritalStatus.ToInt().ToExpressString();
        acMilitaryStatus.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.MilitaryStatus.ToInt().ToExpressString();
        acReligion.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Religion.ToInt().ToExpressString();
        acNationality.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Nationality.ToInt().ToExpressString();
        acContactDataType.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ContactData.ToInt().ToExpressString();
        acEduDegree.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Degree.ToInt().ToExpressString();
        acEduQualification.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Qualification.ToInt().ToExpressString();
        acSkillDegree.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Degree.ToInt().ToExpressString();
        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        acAccountAllowance.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,,,true";
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        this.txtStartFrom_TextChanged(null, null);
        this.acBranch_SelectedIndexChanged(null, null);

        var listHrSystemAllowance = dc.usp_HR_Systems_Select(null, string.Empty, 2).ToList();
        ddlAllowances.DataSource = listHrSystemAllowance;
        ddlAllowances.DataTextField = "Name";
        ddlAllowances.DataValueField = "ID";

        ddlAllowances.DataBind();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Contact_ID = Request["ID"].ToInt();
        }
        if (Request["UREmployee_ID"] != null)
        {
            this.EmployeeUnderRequestID = Request["UREmployee_ID"].ToInt();
        }
    }

    private void FillEmployeeData()
    {
        var Employee = dc.usp_HR_Employees_SelectByID(this.Contact_ID).FirstOrDefault();
        lblEmpName.Text = txtName.Text = Employee.Name;
        ddlGender.SelectedValue = Employee.Gender.ToExpressString();
        txtNationalID.Text = Employee.NationalID;
        txtPassportID.Text = Employee.PassprtID;
        txtDateOfBirth.Text = Employee.DateOfBirth.Value.ToString("d/M/yyyy");
        acMaritalStatus.Value = Employee.MaritalStatus_ID.ToStringOrEmpty();
        acMilitaryStatus.Value = Employee.MilitaryStatus_ID.ToStringOrEmpty();
        acNationality.Value = Employee.Nationality_ID.ToStringOrEmpty();
        acReligion.Value = Employee.Religion_ID.ToStringOrEmpty();
        acBranch.Value = Employee.Branch_ID.ToExpressString();
        this.acBranch_SelectedIndexChanged(null, null);
        acCashierAccount_ID.Value = Employee.CashierAccount_ID.ToStringOrEmpty();
        acStore.Value = Employee.Store_ID.ToStringOrEmpty();
        ddlCurrency.SelectedValue = Employee.Currency_ID.ToExpressString();
        if (File.Exists(Server.MapPath("~/Uploads/" + Employee.Photo))) imgLogo.ImageUrl = "~/Uploads/" + Employee.Photo;
        txtNotes.Text = Employee.Description;
        txtMachineID.Text = Employee.MachineID;
        acDepartment.Value = Employee.Department_ID.ToExpressString();
        this.acDepartment_SelectedIndexChanged(null, null);
        acPosition.Value = Employee.Position_ID.ToExpressString();
        acJobDegree.Value = Employee.JobDegree_ID.ToExpressString();
        acEmploymentStatus.Value = Employee.EmploymentStatus_ID.ToExpressString();
        acSuperVisor.Value = Employee.Supervisor_ID.ToExpressString();
        acShift.Value = Employee.Shift_ID.ToExpressString();
        txtHiringDate.Text = Employee.HiringDate.Value.ToString("d/M/yyyy");
        if (Employee.VisaStartDate.HasValue) txtVisaStartDate.Text = Employee.VisaStartDate.Value.ToString("d/M/yyyy");
        if (Employee.VisaEndDate.HasValue) txtVisaEndDate.Text = Employee.VisaEndDate.Value.ToString("d/M/yyyy");
        txtCasualVacations.Text = Employee.CasualVacations.ToExpressString();
        txtRegualrVacations.Text = Employee.RegularVacations.ToExpressString();
        chkIsSystmeUser.Checked = Employee.IsSystemUser.Value;
        txtBasicSalary.Text = Employee.BasicSalary.ToExpressString();
        txtComPercentage.Text = Employee.ComissionPercentage.ToExpressString();
        txtInvAdsComission.Text = Employee.InvAddsPercentage.ToExpressString();
        txtTarget.Text = Employee.TargetQty.ToExpressString();
        acCostCenter.Value = Employee.CostCenter_ID.ToStringOrEmpty();
        txtInsurance.Text = Employee.Insurance.ToExpressString();
        txtAccountNumber.Text = Employee.AccountNumber;
        chkIsPermissionView.Checked = Employee.HasPermissionShow;
        ddlTypeSalaryAdd.SelectedValue = Employee.TypeOfSalaryAdd.ToExpressString();
        acCity.Text = Employee.City;
        acguarantor.Text = Employee.guarantor;

        acBorderNumber.Text = Employee.BorderNumber;
        txtDateBorderIn.Text = Employee.DateBorderIn != null ? Employee.DateBorderIn.Value.ToString("d/M/yyyy") : string.Empty;


        txtStatuessubscription.Text = Employee.Statuessubscription;
        txtNumberpassport.Text = Employee.Numberpassport;

        txtinsuranceNumber.Text = Employee.insuranceNumber;
        txtJopNumber.Text = Employee.JopNumber;
        txtCodeEmployee.Text = Employee.CodeEmployee;
        txtBankNumber.Text = Employee.BankNumber;
        txtNameE.Text = Employee.NameE;



        if (Employee.OpenBalance.HasValue) txtOpenBalance.Text = Employee.OpenBalance.ToExpressString();
        this.txtOpenBalance_TextChanged(null, null);
        if (Employee.OpenBalanceDate.HasValue) txtStartFrom.Text = Employee.OpenBalanceDate.Value.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
        if (Employee.Ratio.HasValue) txtRatio.Text = Employee.Ratio.Value.ToExpressString();
        ddlCurrency.Enabled = txtStartFrom.Enabled = txtOpenBalance.Enabled = !Employee.LockEmployee.Value;
        acBranch.Enabled = MyContext.UserProfile.Branch_ID == null && !Employee.LockEmployee.Value;

        if (Employee.ServiceEndDate.HasValue) txtTerminationDate.Text = Employee.ServiceEndDate.Value.ToString("d/M/yyyy");
        txtTerminationReason.Text = Employee.ServiceEndReason;


        var allowance = dc.usp_EmployeAllowance_Select(this.Contact_ID);

        var valuer = allowance.FirstOrDefault(x => x.TypeAllowance == 1);

        //txtInsurance.Text = (txtParcent.Text.ToDecimalOrDefault() * (txtBasicSalary.Text.ToDecimalOrDefault() + (valuer != null ? valuer.MonthlyAllowance.Value : 0)) / 100).ToExpressString();
        try
        {
            txtParcent.Text = Math.Round(((txtInsurance.Text.ToDecimalOrDefault() * 100) / (txtBasicSalary.Text.ToDecimalOrDefault() + valuer.MonthlyAllowance.Value))).ToString();

        }
        catch
        {

        }

        var emp = dc.HR_Employees.Where(x => x.Contact_ID == this.Contact_ID);
        if (emp != null)
        {
            if (emp.Any())
            {
                var ep = emp.FirstOrDefault();
                txtPrinterName.Text = ep.PrinterName;
                CheckBox1.Checked = ep.HasPrintCuisin != null ? ep.HasPrintCuisin.Value : false;
            }
        }



        this.dtContactData = dc.usp_ContactDetails_Select(this.Contact_ID, this.MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        gvContactData.DataSource = this.dtContactData;
        gvContactData.DataBind();

        this.dtExperienceData = dc.usp_HR_Experiences_Select(this.Contact_ID).CopyToDataTable();
        gvExperiences.DataSource = this.dtExperienceData;
        gvExperiences.DataBind();

        this.dtSkills = dc.usp_HR_Skills_Select(this.Contact_ID).CopyToDataTable();
        gvSkills.DataSource = this.dtSkills;
        gvSkills.DataBind();

        this.dtEducation = dc.usp_HR_Education_Select(this.Contact_ID).CopyToDataTable();
        gvEducation.DataSource = this.dtEducation;
        gvEducation.DataBind();

        this.dtGoalCommission = dc.usp_GoalCommission_Select(this.Contact_ID).CopyToDataTable();

        gvCommission.DataSource = this.dtGoalCommission;
        gvCommission.DataBind();

        this.dtAllowance = dc.usp_EmployeAllowance_Select(this.Contact_ID).CopyToDataTable();

        gvAllowance.DataSource = this.dtAllowance;
        gvAllowance.DataBind();

        this.dtEmployeExpense = dc.usp_EmployeExpense_Select(this.Contact_ID).CopyToDataTable();

        gvEmployeExpense.DataSource = this.dtEmployeExpense;
        gvEmployeExpense.DataBind();

    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc && this.EditMode) Response.Redirect(PageLinks.Authorization, true);
        btnSaveEmployee.Visible = (this.EditMode && this.MyContext.PageData.IsEdit) || (!this.EditMode && this.MyContext.PageData.IsAdd);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acCashierAccount_ID.Visible = MyContext.Features.WorkingMode != WorkingMode.HR.ToByte();
    }

    #endregion

    protected void btnSaveGoalCommission_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtGoalCommission.NewRow();
            r["ID"] = this.dtGoalCommission.GetID("ID");
            r["ContactName"] = txtName.Text;
            r["FromValue"] = txtFirstValue.Text;
            r["ToValue"] = txtLastValue.Text;
            r["Parcent"] = txtPercent.Text;
            r["FromDate"] = txtFromDate.Text.ToDate();
            r["ToDate"] = txtToDate.Text.ToDate(); ;

            this.dtGoalCommission.Rows.Add(r);
            gvCommission.DataSource = this.dtGoalCommission;
            gvCommission.DataBind();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }



    protected void BtnClearNew_OnClick(object sender, EventArgs e)
    {


    }

    protected void btnAllowanceSaveNew_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtAllowance.NewRow();
            r["ID"] = this.dtAllowance.GetID("ID");
            r["ContactName"] = txtName.Text;
            r["TypeAllowance"] = ddlAllowances.SelectedValue;
            r["DailyAllowance"] = txtDailyAllowance.Text;
            r["MonthlyAllowance"] = txtMonthlyAllowance.Text;
            r["Account_ID"] = acAccountAllowance.Value;
            r["NameAccount"] = acAccountAllowance.Text;

            r["TypeAllowanceName"] = ddlAllowances.SelectedItem.ToString();




            //if (ddlAllowances.SelectedValue == "1")
            //{
            //    r["TypeAllowanceName"] = "بدل سكن";
            //}
            //else if (ddlAllowances.SelectedValue == "2")
            //{
            //    r["TypeAllowanceName"] = "بدل معيشة";
            //}
            //else if (ddlAllowances.SelectedValue == "3")
            //{
            //    r["TypeAllowanceName"] = "بدل مواصلات";
            //}
            //else
            //{
            //    r["TypeAllowanceName"] = "بدلات أخرى";
            //}


            this.dtAllowance.Rows.Add(r);
            gvAllowance.DataSource = this.dtAllowance;
            gvAllowance.DataBind();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    protected void btnEmployeExpenseAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtEmployeExpense.NewRow();
            r["ID"] = this.dtEmployeExpense.GetID("ID");
            r["ContactName"] = txtName.Text;
            r["TypeEmployeExpense"] = ddlTypeEmployeExpense.SelectedValue;
            //r["DailyEmployeExpense"] = txtDailyEmployeExpense.Text;
            r["MonthlyEmployeExpense"] = txtMonthlyEmployeExpense.Text;

            r["TypeExpense"] = ddlTypeExpense.SelectedValue;
            r["TypeStatusExpense"] = ddlType.SelectedValue;
            r["DateExpense"] = txtDate.Text.ToDate();
            r["DateExpire"] = txtDateFinish.Text.ToDateOrDBNULL();
            r["Note"] = txtNote.Text;


            switch (ddlTypeEmployeExpense.SelectedValue)
            {
                case "0":
                    r["TypeEmployeExpenseName"] = "إقامة";
                    break;
                case "1":
                    r["TypeEmployeExpenseName"] = "تذكرة طيران";
                    break;
                case "2":
                    r["TypeEmployeExpenseName"] = "مصروفات مقدمة";
                    break;
                case "3":
                    r["TypeEmployeExpenseName"] = "بطاقة صراف";
                    break;
                case "4":
                    r["TypeEmployeExpenseName"] = "رخصة عمل";
                    break;
                case "5":
                    r["TypeEmployeExpenseName"] = "شهادة صحية";
                    break;
                case "6":
                    r["TypeEmployeExpenseName"] = "تأمين صحي";
                    break;
                case "7":
                    r["TypeEmployeExpenseName"] = "رسوم اصدارخروج وعودة";
                    break;
                case "8":
                    r["TypeEmployeExpenseName"] = "رسوم رخصة قيادة";
                    break;
                case "9":
                    r["TypeEmployeExpenseName"] = "تأمينات أجتماعية";
                    break;
                case "10":
                    r["TypeEmployeExpenseName"] = "مكافئات";
                    break;
                case "11":
                    r["TypeEmployeExpenseName"] = "علاج";
                    break;
                case "12":
                    r["TypeEmployeExpenseName"] = "مصروفات أخرى";

                    break;
                case "13":
                    r["TypeEmployeExpenseName"] = "رسوم تعليم قيادة";

                    break;
                case "14":
                    r["TypeEmployeExpenseName"] = "رسوم إصدار تأشيرة";

                    break;
            }


            switch (ddlType.SelectedValue)
            {
                case "0":
                    r["TypeExpenseName"] = "اصدار جديد";
                    break;
                case "1":
                    r["TypeExpenseName"] = "تجديد";
                    break;


            }


            switch (ddlTypeExpense.SelectedValue)
            {
                case "0":
                    r["TypeStatusExpenseName"] = "حالي";
                    break;
                case "1":
                    r["TypeStatusExpenseName"] = "مقدم";
                    break;


            }


            this.dtEmployeExpense.Rows.Add(r);
            gvEmployeExpense.DataSource = this.dtEmployeExpense;
            gvEmployeExpense.DataBind();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    protected void ntnEmployeExpenseClear_OnClick(object sender, EventArgs e)
    {

    }













    bool render = true;

    private DataTable dtAttachments
    {
        get
        {
            return (DataTable)Session["dtAttachments" + this.WinID];
        }

        set
        {
            Session["dtAttachments" + this.WinID] = value;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {

            string Guid = string.Empty;
            for (int index = 0; index < Request.Files.Count; index++)
            {
                Guid = this.GenerateGuid();
                HttpPostedFile f = Request.Files[index];
                if (f.ContentLength <= 0) continue;
                f.SaveAs(Server.MapPath("~/Uploads/Attachments/" + Guid));
                dc.usp_Attachments_Insert("~/HR/Employees.aspx?ID=" + this.Contact_ID, f.FileName, Guid, (decimal?)f.ContentLength / (decimal?)1024.00);
                LogAction(Actions.Add, f.FileName, dc);
            }
            this.Fill();
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0) UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected void gvAttachments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAttachments.PageIndex = e.NewPageIndex;
            gvAttachments.DataSource = this.dtAttachments;
            gvAttachments.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected void gvAttachments_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            ifDownload.Attributes.Add("src", Request.Url.ToExpressString() + "&guid=" + gvAttachments.DataKeys[e.NewSelectedIndex]["Guid"].ToExpressString() + "&filename=" + gvAttachments.DataKeys[e.NewSelectedIndex]["FileName"].ToExpressString());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected void gvAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            File.Delete(Server.MapPath("~/Uploads/Attachments/" + gvAttachments.DataKeys[e.RowIndex]["Guid"]));
            dc.usp_Attachments_Delete(gvAttachments.DataKeys[e.RowIndex]["ID"].ToInt());
            UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);
            LogAction(Actions.Delete, gvAttachments.DataKeys[e.RowIndex]["FileName"].ToExpressString(), dc);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    private void Fill()
    {
        this.dtAttachments = dc.usp_Attachments_Select("~/HR/Employees.aspx?ID=" + this.Contact_ID).CopyToDataTable();
        gvAttachments.DataSource = this.dtAttachments;
        gvAttachments.DataBind();
    }

    private string GenerateGuid()
    {
        try
        {
            string guid = string.Empty;
            do
            {
                guid = Guid.NewGuid().ToExpressString();
            } while (File.Exists(Server.MapPath("~/Uploads/Attachments/" + guid)));
            return guid;
        }
        catch
        {
            throw;
        }
    }

    private void Download()
    {
        render = false;
        Response.Clear();
        Response.Buffer = true;
        Response.HeaderEncoding = System.Text.Encoding.GetEncoding("windows-1256");
        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Request["FileName"] + "\"");
        Response.ContentType = "application/octet-stream";
        Response.TransmitFile(Server.MapPath("~/Uploads/Attachments/" + Request["Guid"]));
    }

    protected void txtParcent_OnTextChanged(object sender, EventArgs e)
    {

        var allowance = dc.usp_EmployeAllowance_Select(this.Contact_ID);

        var valuer = allowance.FirstOrDefault(x => x.TypeAllowance == 1);
        txtInsurance.Text = (txtParcent.Text.ToDecimalOrDefault() * (txtBasicSalary.Text.ToDecimalOrDefault() + (valuer != null ? valuer.MonthlyAllowance.Value : 0)) / 100).ToExpressString();

    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {


        XpressDataContext dataContext = new XpressDataContext();
        var databaseName = dataContext.Connection.Database;

        ReportDocument doc = new ReportDocument();
        doc.Load(Server.MapPath("~\\Reports\\EmployeCosts.rpt"));
        doc.SetParameterValue("@Contact_ID", this.Contact_ID);
        Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "EmployeCosts"), false);


    }
    protected void gvAllowance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvAllowance.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtAllowance.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvAllowance.DataSource = this.dtAllowance;
            gvAllowance.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvCommission_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvCommission.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtGoalCommission.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvCommission.DataSource = this.dtGoalCommission;
            gvCommission.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvEmployeExpense_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvEmployeExpense.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtEmployeExpense.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvEmployeExpense.DataSource = this.dtEmployeExpense;
            gvEmployeExpense.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
        ChangeDate();
    }

    private void ChangeDate()
    {
        // txtNbrYear.Visible = false;

        switch (ddlTypeEmployeExpense.SelectedValue)
        {

            case "0":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":

            case "9":
                if (!string.IsNullOrEmpty(txtDate.Text))
                    txtDateFinish.Text = txtDate.Text.ToDate().Value.AddDays(int.Parse(txtNbrYear.Text)).ToExpressString();
                break;
            case "1":
            case "7":
            case "10":

            case "11":
            case "12":
            case "13":
            case "14":
                txtDateFinish.Text = "";
                break;
            case "8":
                // txtNbrYear.Visible = true;
                if (!string.IsNullOrEmpty(txtDate.Text))
                    txtDateFinish.Text = txtDate.Text.ToDate().Value.AddDays(int.Parse(txtNbrYear.Text)).ToExpressString();
                break;
        }
    }

    protected void ddlTypeEmployeExpense_OnTextChanged(object sender, EventArgs e)
    {

        ChangeDate();
    }

    protected void lnkCommissionPrint_OnClick(object sender, EventArgs e)
    {

    }
    protected void txtNbrYear_TextChanged(object sender, EventArgs e)
    {
        ChangeDate();
    }
}