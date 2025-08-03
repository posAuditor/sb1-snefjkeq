using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using XPRESS.ServerControls;
using Newtonsoft.Json;

public partial class HR_Payroll : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtPayroll
    {
        get
        {
            return (DataTable)Session["dtPayroll" + this.WinID];
        }

        set
        {
            Session["dtPayroll" + this.WinID] = value;
        }
    }

    private int EditID
    {
        get
        {
            if (ViewState["EditID"] == null) return 0;
            return (int)ViewState["EditID"];
        }

        set
        {
            ViewState["EditID"] = value;
        }
    }
    private List<clsShowHide> Lst
    {
        get
        {
            if (ViewState["Lst_HideShow"] == null) return new List<clsShowHide>();
            return (List<clsShowHide>)ViewState["Lst_HideShow"];
        }

        set
        {
            ViewState["Lst_HideShow"] = value;
        }
    }
    //private decimal BasicSalary
    //{
    //    get
    //    {
    //        if (ViewState["BasicSalary"] == null) return 0;
    //        return (decimal)ViewState["BasicSalary"];
    //    }

    //    set
    //    {
    //        ViewState["BasicSalary"] = value;
    //    }
    //}

    //private int Contact_ID
    //{
    //    get
    //    {
    //        if (ViewState["Contact_ID"] == null) return 0;
    //        return (int)ViewState["Contact_ID"];
    //    }

    //    set
    //    {
    //        ViewState["Contact_ID"] = value;
    //    }
    //}

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtDate.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            acDepartment.Clear();
            acName.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterEmployees(null, null);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPayRoll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPayRoll.PageIndex = e.NewPageIndex;
            gvPayRoll.DataSource = this.dtPayroll;
            gvPayRoll.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPayRoll_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            //this.EditID = gvPayRoll.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            //var payroll = dc.usp_HR_PayRoll_SelectByID(this.EditID).FirstOrDefault();

            //this.Contact_ID = payroll.Contact_ID.Value;
            //this.BasicSalary = payroll.BasicSalary.Value;
            //lblHeader.Text = payroll.ContactName + " - " + payroll.Month.ToExpressString() + "/" + payroll.Year.ToExpressString() + " - " + this.BasicSalary.ToExpressString();

            //  txtOverTime.Text = payroll.OvertimePeriod.ToExpressString();
            // txtOverTimeValue.Text = payroll.OvertimeValue.ToExpressString();

            //  txtDayOffWork.Text = payroll.DayOffWorkPeriod.ToExpressString();
            //  txtDayOffWorkValue.Text = payroll.DayOffWorkValue.ToExpressString();

            //txtDelay.Text = payroll.DelayPeriod.ToExpressString();
            //txtDelayValue.Text = payroll.DelayValue.ToExpressString();

            ////txtAbsence.Text = payroll.AbsencePeriod.ToExpressString();
            ////txtAbsenceValue.Text = payroll.AbsenceValue.ToExpressString();

            ////txtExcuse.Text = payroll.ExcusePeriod.ToExpressString();
            ////txtExcuseValue.Text = payroll.ExcuseValue.ToExpressString();

            ////txtDeducatedDaysOff.Text = payroll.VacationWithDeducationPeriod.ToExpressString();
            ////txtDeducatedDaysOffValue.Text = payroll.VacationWithDeducationValue.ToExpressString();

            ////txtDeducatedWorkDays.Text = payroll.WorkWithDeductionPeriod.ToExpressString();
            ////txtDeducatedWorkDaysValue.Text = payroll.WorkWithDeductionValue.ToExpressString();

            ////txtIncentives.Text = payroll.Incentives.ToExpressString();
            ////// txtAllowances.Text = payroll.Allowances.ToExpressString();
            ////txtAllowances.Text = dc.usp_EmployeAllowance_Select(payroll.Contact_ID).ToList().Sum(x => x.MonthlyAllowance).ToExpressString();

            // var listEmployeeExpence = dc.usp_EmployeExpense_Select(payroll.Contact_ID).ToList();



            ////txtTaxes.Text = payroll.Taxes.ToExpressString();
            ////txtInsurance.Text = payroll.Insurance.ToExpressString();
            //////payroll.Insurance.ToExpressString();


            //////txtTaxes.Text = payroll.Taxes.ToExpressString();
            //////txtInsurance.Text = "0";//listEmployeeExpence.Where(x => x.TypeEmployeExpense == 9).Sum(x => x.MonthlyEmployeExpense).ToExpressString();
            ////////payroll.Insurance.ToExpressString();

            ////txtLoans.Text = payroll.Loans.ToExpressString();
            ////txtSancations.Text = payroll.Sancations.ToExpressString();


            ////// txtOtherAdditions.Text = listEmployeeExpence.Where(x => x.TypeEmployeExpense == 10).Sum(x => x.MonthlyEmployeExpense).ToExpressString();

            //////txtOtherAdditions.Text = payroll.OtherAdditions.ToExpressString();
            ////txtOtherDeducations.Text = payroll.OtherDeducations.ToExpressString();

            //  lblGrossTotal.Text = payroll.GrossTotal.ToExpressString();

            // txtNotes.Text = payroll.Notes;





            //this.CalcGrossTotal(sender, null);
            // btnSaveNew.Visible = payroll.DocStatus_ID == DocStatus.Current.ToByte() && MyContext.PageData.IsEdit;
            //mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            this.EditID = 0;
            mpeCreateNew.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            //  int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;


            LinkButton tb = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int Index = gvr.RowIndex;



            var Item_ID = gvPayRoll.DataKeys[Index]["ID"].ToInt();
            // var PurshasePrice_ID = gvItemssList.DataKeys[Index]["PurshasePrice_ID"].ToInt();

            var txtOverTime = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtOverTime"));
            var txtOverTimeValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtOverTimeValue"));


            var txtDayOffWork = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDayOffWork"));
            var txtDayOffWorkValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDayOffWorkValue"));

            var txtDelay = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDelay"));
            var txtDelayValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDelayValue"));

            var txtAbsence = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtAbsence"));
            var txtAbsenceValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtAbsenceValue"));


            var txtExcuse = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtExcuse"));
            var txtExcuseValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtExcuseValue"));



            var txtDeducatedDaysOff = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDeducatedDaysOff"));
            var txtDeducatedDaysOffValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDeducatedDaysOffValue"));


            var txtDeducatedWorkDays = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDeducatedWorkDays"));
            var txtDeducatedWorkDaysValue = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtDeducatedWorkDaysValue"));

            var txtIncentives = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtIncentives"));
            var txtAllowances = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtAllowances"));

            var txtTaxes = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtTaxes"));
            var txtInsurance = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtInsurance"));


            var txtLoans = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtLoans"));
            var txtSancations = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtSancations"));

            var txtOtherDeducations = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtOtherDeducations"));
            var txtOtherAdditions = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtOtherAdditions"));
            var lblGrossTotal = ((Label)gvPayRoll.Rows[Index].FindControl("lblGrossTotal"));

            var txtNotes = ((ABFTextBox)gvPayRoll.Rows[Index].FindControl("txtNotes"));


            result = dc.usp_HR_PayRoll_Update(
                Item_ID, DocStatus.Current.ToByte(),
                txtAbsence.Text.ToIntOrDefault(),
                txtAbsenceValue.Text.ToDecimalOrDefault(),


                txtOverTime.Text.ToIntOrDefault(),

                txtOverTimeValue.Text.ToDecimalOrDefault(),
                txtDayOffWork.Text.ToIntOrDefault(),
                txtDayOffWorkValue.Text.ToDecimalOrDefault(),
                txtDelay.Text.ToIntOrDefault(),
                txtDelayValue.Text.ToDecimalOrDefault(),
                txtExcuse.Text.ToIntOrDefault(),
                txtExcuseValue.Text.ToDecimalOrDefault(),
                txtDeducatedWorkDays.Text.ToIntOrDefault(),
                txtDeducatedWorkDaysValue.Text.ToDecimalOrDefault(),
                txtDeducatedDaysOff.Text.ToIntOrDefault(),
                txtDeducatedDaysOffValue.Text.ToDecimalOrDefault(),
                txtIncentives.Text.ToDecimalOrDefault(),
                txtAllowances.Text.ToDecimalOrDefault(),
                txtTaxes.Text.ToDecimalOrDefault(),
                txtLoans.Text.ToDecimalOrDefault(),
                txtSancations.Text.ToDecimalOrDefault(),
                txtInsurance.Text.ToDecimalOrDefault(),
                txtOtherAdditions.Text.ToDecimalOrDefault(),
                txtOtherDeducations.Text.ToDecimalOrDefault(),
                  lblGrossTotal.Text.ToDecimalOrDefault(),
                  txtNotes.TrimmedText
                );
            DataRow r = this.dtPayroll.Select("ID=" + this.EditID)[0];
            r["GrossTotal"] = lblGrossTotal.Text.ToDecimalOrDefault();
            gvPayRoll.DataSource = this.dtPayroll;
            gvPayRoll.DataBind();

            LogAction(Actions.Edit, lblHeader.Text, dc);
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterEmployees(object sender, EventArgs e)
    {
        try
        {
            acName.ContextKey = acDepartment.Value + ",," + acBranch.Value;
            if (this.dtPayroll != null)
            {
                this.dtPayroll.Rows.Clear();
                gvPayRoll.DataSource = this.dtPayroll;
                gvPayRoll.DataBind();
            }
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            string serial = string.Empty;
            var company = dc.usp_Company_Select().FirstOrDefault();
            decimal? TotalSalary = 0;
            decimal? TotalSalaryOperation = 0;

            if (txtDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                return;
            }

            foreach (GridViewRow gvRow in gvPayRoll.Rows)
            {
                if (!((CheckBox)gvRow.FindControl("chkApprove")).Checked) continue;

                var payroll = dc.usp_HR_PayRoll_SelectByID(gvPayRoll.DataKeys[gvRow.RowIndex]["ID"].ToInt()).FirstOrDefault();

                if (payroll.DocStatus_ID == DocStatus.Approved.ToInt()) continue;

                TotalSalary = payroll.GrossTotal + payroll.Loans + payroll.Taxes + payroll.Insurance;

                TotalSalaryOperation = payroll.Loans + payroll.Taxes + payroll.Insurance + (payroll.GrossTotal >= 0 ? payroll.GrossTotal : 0) + (TotalSalary < 0 ? TotalSalary * -1 : 0);

                if (TotalSalaryOperation <= 0) continue;
                int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Salary.ToInt(), company.Currency_ID, TotalSalaryOperation, TotalSalaryOperation, 1, payroll.Notes);


                //Allowance


                var AllowanceValues = dc.usp_EmployeAllowance_Select(payroll.Contact_ID).Where(c => c.Account_ID != null).ToList();
                decimal TotalMonthlyAllowance = 0;
                foreach (var item in AllowanceValues)
                {
                    TotalMonthlyAllowance += item.MonthlyAllowance.ToDecimalOrDefault();
                    //راتب مستحق
                    if (item.MonthlyAllowance != 0) dc.usp_OperationDetails_Insert(Result, item.Account_ID, item.MonthlyAllowance, 0, item.MonthlyAllowance, 0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());


                }

                //رواتب واجور
                dc.usp_OperationDetails_Insert(Result, company.HRSalariesAccountID, TotalSalary - TotalMonthlyAllowance, 0, TotalSalary - TotalMonthlyAllowance, 0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());


                //راتب مستحق
                if (payroll.GrossTotal != 0) dc.usp_OperationDetails_Insert(Result, payroll.SalaryChartOfAccount_ID, 0, payroll.GrossTotal, 0, payroll.GrossTotal, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());







                ////رواتب واجور
                //dc.usp_OperationDetails_Insert(Result, company.HRSalariesAccountID, TotalSalary, 0, TotalSalary, 0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());






                ////راتب مستحق
                //if (payroll.GrossTotal != 0) dc.usp_OperationDetails_Insert(Result, payroll.SalaryChartOfAccount_ID, 0, payroll.GrossTotal, 0, payroll.GrossTotal, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                //تامينات
                if (payroll.Insurance != 0) dc.usp_OperationDetails_Insert(Result, company.HRSocialSecurityAccountID, 0, payroll.Insurance, 0, payroll.Insurance, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                //ضرائب
                if (payroll.Taxes != 0) dc.usp_OperationDetails_Insert(Result, company.HRWorkTaxAccountID, 0, payroll.Taxes, 0, payroll.Taxes, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                if (payroll.Loans != 0)
                {
                    if (payroll.LoanChartOfAccount_ID == null)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.EmpHasNoLoansAccount + " (" + payroll.ContactName + ")", string.Empty);
                        trans.Rollback();
                        return;
                    }

                    dc.usp_OperationDetails_Insert(Result, payroll.LoanChartOfAccount_ID, 0, payroll.Loans, 0, payroll.Loans, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());
                }

                dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), payroll.CostCenter_ID, txtDate.Text.ToDate(), TotalSalary, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt(), payroll.Notes);

                dc.usp_HR_PayRoll_Update(payroll.ID, DocStatus.Approved.ToByte(), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                ((CheckBox)gvRow.FindControl("chkApprove")).Enabled = false;
                LogAction(Actions.Approve, payroll.ContactName + " " + payroll.Year.ToExpressString() + "-" + payroll.Month.ToExpressString(), dc);
            }
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void CalcGrossTotal(object sender, EventArgs e)
    {
        try
        {


            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;


            var txtDeducatedDaysOffValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedDaysOffValue"));
            var txtOverTimeValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtOverTimeValue"));
            var txtDayOffWorkValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDayOffWorkValue"));
            var txtIncentives = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtIncentives"));
            var txtAllowances = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAllowances"));
            var txtOtherAdditions = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtOtherAdditions"));

            var txtDelayValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDelayValue"));
            var txtExcuseValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtExcuseValue"));
            var txtAbsenceValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAbsenceValue"));

            var txtDeducatedWorkDaysValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedWorkDaysValue"));
            var txtTaxes = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtTaxes"));

            var txtInsurance = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtInsurance"));
            var txtLoans = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtLoans"));
            var txtSancations = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtSancations"));
            var txtOtherDeducations = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtOtherDeducations"));
            var lblGrossTotal = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblGrossTotal"));
            var lblGrossTotal1 = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblGrossTotal1"));

            var lblGrossTotal2 = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblGrossTotal2"));
            var lblBasicSalary = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblBasicSalary"));



            decimal additions = lblBasicSalary.Text.ToDecimalOrDefault() +


                txtOverTimeValue.Text.ToDecimalOrDefault() +
                txtDayOffWorkValue.Text.ToDecimalOrDefault() +
                txtIncentives.Text.ToDecimalOrDefault() +
                txtAllowances.Text.ToDecimalOrDefault() +
                txtOtherAdditions.Text.ToDecimalOrDefault();

            decimal deducations = txtDelayValue.Text.ToDecimalOrDefault() +
                txtExcuseValue.Text.ToDecimalOrDefault() +
                txtAbsenceValue.Text.ToDecimalOrDefault() +
                txtDeducatedDaysOffValue.Text.ToDecimalOrDefault() +
                txtDeducatedWorkDaysValue.Text.ToDecimalOrDefault() +
                txtTaxes.Text.ToDecimalOrDefault() +
                txtInsurance.Text.ToDecimalOrDefault() +
                txtLoans.Text.ToDecimalOrDefault() +
                txtSancations.Text.ToDecimalOrDefault() +
                txtOtherDeducations.Text.ToDecimalOrDefault();

            lblGrossTotal.Text = (additions - deducations).ToExpressString();
            lblGrossTotal1.Text = (deducations).ToExpressString();
            lblGrossTotal2.Text = (additions).ToExpressString();



            // mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOverTime_TextChanged(object sender, EventArgs e)
    {
        try
        {


            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtOverTimeValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtOverTimeValue"));
            var txtOverTime = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtOverTime"));
            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();




            txtOverTimeValue.Text = dc.fun_HR_OverTime(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtOverTime.Text.ToIntOrDefault()).FirstOrDefault().OverTimeValue.ToExpressString();
            // txtOverTimeValue.Text = dc.fun_HR_OverTime(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), 0).FirstOrDefault().OverTimeValue.ToExpressString();
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDayOffWork_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtDayOffWorkValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDayOffWorkValue"));
            var txtDayOffWork = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDayOffWork"));
            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();




            txtDayOffWorkValue.Text = dc.fun_HR_DayOffWork(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDayOffWork.Text.ToIntOrDefault()).FirstOrDefault().DayOffWorkValue.ToExpressString();
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDelay_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtDelayValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDelayValue"));
            var txtDelay = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDelay"));

            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();



            txtDelayValue.Text = dc.fun_HR_Delay(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDelay.Text.ToIntOrDefault()).FirstOrDefault().DelayValue.ToExpressString();
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtAbsence_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtAbsenceValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAbsenceValue"));
            var txtAbsence = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAbsence"));

            ABFTextBox txtIncentives = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtIncentives"));

            ABFTextBox txtDeducatedDaysOff = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedDaysOff"));

            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();



            txtAbsenceValue.Text = dc.fun_HR_Absence(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtAbsence.Text.ToIntOrDefault()).FirstOrDefault().AbsenceValue.ToExpressString();
            this.CalcIncentives(txtIncentives, txtAbsence, txtDeducatedDaysOff, lblContact_ID);
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtExcuse_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtExcuseValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtExcuseValue"));
            var txtExcuse = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtExcuse"));


            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();



            txtExcuseValue.Text = dc.fun_HR_Excuse(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtExcuse.Text.ToIntOrDefault()).FirstOrDefault().ExcuseValue.ToExpressString();
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDeducatedWorkDays_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtDeducatedWorkDaysValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedWorkDaysValue"));
            var txtDeducatedWorkDays = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedWorkDays"));

            var txtAbsence = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAbsence"));
            ABFTextBox txtIncentives = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtIncentives"));
            ABFTextBox txtDeducatedDaysOff = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedDaysOff"));

            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();


            txtDeducatedWorkDaysValue.Text = dc.fun_HR_DeducatedWorkDays(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDeducatedWorkDays.Text.ToIntOrDefault()).FirstOrDefault().DeducatedWorkDaysValue.ToExpressString();
            this.CalcIncentives(txtIncentives, txtAbsence, txtDeducatedDaysOff, lblContact_ID);


            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDeducatedDaysOff_TextChanged(object sender, EventArgs e)
    {
        try
        {

            ABFTextBox tb = (ABFTextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            var txtDeducatedDaysOffValue = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedDaysOffValue"));
            var txtDeducatedDaysOff = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtDeducatedDaysOff"));


            ABFTextBox txtAbsence = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtAbsence"));
            ABFTextBox txtIncentives = ((ABFTextBox)gvPayRoll.Rows[rowindex].FindControl("txtIncentives"));

            var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();



            txtDeducatedDaysOffValue.Text = dc.fun_HR_DeducatedDaysOff(lblContact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDeducatedDaysOff.Text.ToIntOrDefault()).FirstOrDefault().DeducatedDaysOffValue.ToExpressString();
            this.CalcIncentives(txtIncentives, txtAbsence, txtDeducatedDaysOff, lblContact_ID);
            this.CalcGrossTotal(sender, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void LoadControls()
    {
        ddlYear.Items.Insert(0, new ListItem((DateTime.Now.Year - 1).ToExpressString(), (DateTime.Now.Year - 1).ToExpressString()));
        ddlYear.Items.Insert(1, new ListItem((DateTime.Now.Year).ToExpressString(), (DateTime.Now.Year).ToExpressString()));
        ddlYear.Items.Insert(2, new ListItem((DateTime.Now.Year + 1).ToExpressString(), (DateTime.Now.Year + 1).ToExpressString()));
        ddlYear.SelectedValue = (DateTime.Now.Year).ToExpressString();

        acDepartment.ContextKey = string.Empty;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
        gvPayRoll.DataSource = this.dtPayroll;
        gvPayRoll.DataBind();


        try
        {
            var lst = (new List<clsShowHide>());
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 1, IndexColumn = 1, NameColumn = Resources.Labels.Department });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 2, IndexColumn = 2, NameColumn = Resources.Labels.Name });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 3, IndexColumn = 3, NameColumn = Resources.Labels.Month });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 4, IndexColumn = 4, NameColumn = Resources.Labels.Year });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 5, IndexColumn = 5, NameColumn = Resources.Labels.GrossTotal });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 6, IndexColumn = 6, NameColumn = Resources.Labels.OverTime });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 7, IndexColumn = 7, NameColumn = Resources.Labels.OverTimeValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 8, IndexColumn = 8, NameColumn = Resources.Labels.DayOffWork });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 9, IndexColumn = 9, NameColumn = Resources.Labels.DayOffWorkValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 10, IndexColumn = 10, NameColumn = Resources.Labels.Delay });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 11, IndexColumn = 11, NameColumn = Resources.Labels.DelayValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 12, IndexColumn = 12, NameColumn = Resources.Labels.Absence });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 13, IndexColumn = 13, NameColumn = Resources.Labels.AbsenceValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 15, IndexColumn = 15, NameColumn = Resources.Labels.Excuse });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 16, IndexColumn = 16, NameColumn = Resources.Labels.ExcuseValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 17, IndexColumn = 17, NameColumn = Resources.Labels.DeducatedWorkDays });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 18, IndexColumn = 18, NameColumn = Resources.Labels.DeducatedWorkDaysValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 19, IndexColumn = 19, NameColumn = Resources.Labels.DeducatedDaysOff });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 20, IndexColumn = 20, NameColumn = Resources.Labels.DeducatedDaysOffValue });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 21, IndexColumn = 21, NameColumn = Resources.Labels.Incentives });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 22, IndexColumn = 22, NameColumn = Resources.Labels.Allowances });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 23, IndexColumn = 23, NameColumn = Resources.Labels.Taxes });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 24, IndexColumn = 24, NameColumn = Resources.Labels.Insurance });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 25, IndexColumn = 25, NameColumn = Resources.Labels.HRLoan });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 26, IndexColumn = 26, NameColumn = Resources.Labels.Sancations });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 27, IndexColumn = 27, NameColumn = "مكافئات - زيادات أخرى" });
            lst.Add(new clsShowHide() { checkedIndex = false, ID = 28, IndexColumn = 28, NameColumn = Resources.Labels.OtherDeducations });


            gvShowHide.DataSource = lst.CopyToDataTable();
            gvShowHide.DataBind();
        }
        catch
        {


        }




        //  txtOverTimeValue.ForeColor =
        //txtDayOffWorkValue.ForeColor =
        // txtIncentives.ForeColor =
        // txtAllowances.ForeColor =
        // txtOtherAdditions.ForeColor = System.Drawing.Color.Green;

        // txtDelayValue.ForeColor =
        //         txtExcuseValue.ForeColor =
        //         txtAbsenceValue.ForeColor =
        //         txtDeducatedDaysOffValue.ForeColor =
        //         txtDeducatedWorkDaysValue.ForeColor =
        //         txtTaxes.ForeColor =
        //         txtInsurance.ForeColor =
        //         txtLoans.ForeColor =
        //         txtSancations.ForeColor =
        //         txtOtherDeducations.ForeColor = System.Drawing.Color.Red;
    }

    private void Fill()
    {
        if (new DateTime(ddlYear.SelectedValue.ToInt(), ddlMonth.SelectedValue.ToInt(), 1) > DateTime.Now.Date.AddDays(2))
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            return;
        }
        if (new DateTime(ddlYear.SelectedValue.ToInt(), ddlMonth.SelectedValue.ToInt(), 1) > MyContext.FiscalYearEndDate)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateOutsideFiscalYear, string.Empty);
            return;
        }
        var EmpList = dc.usp_HR_PayRoll_Select(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), acName.Value.ToNullableInt(), acDepartment.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), false).ToList();
        foreach (var Emp in EmpList)
        {
            dc.usp_HR_PayRoll_Calc(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), Emp.Contact_ID);
        }
        this.dtPayroll = dc.usp_HR_PayRoll_Select(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), acName.Value.ToNullableInt(), acDepartment.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), true).CopyToDataTable();
        gvPayRoll.DataSource = this.dtPayroll;
        gvPayRoll.DataBind();
        var ObjectFormatJson = dc.HiddenGrids.Where(c => c.Contact_ID == MyContext.UserProfile.Contact_ID && c.Page_ID == MyContext.PageData.PageID).FirstOrDefault();
        if (ObjectFormatJson != null)
        {
            var str = ObjectFormatJson.JsonFormControl.ToExpressString();
            if (!string.IsNullOrEmpty(str))
            {
                List<clsShowHide> data = JsonConvert.DeserializeObject<List<clsShowHide>>(str);

                foreach (var item in data)
                {
                    gvPayRoll.Columns[item.IndexColumn].Visible = item.checkedIndex;
                }

            }
        }

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvPayRoll.Columns[6].Visible = MyContext.PageData.IsEdit;
        btnSearch.Visible = MyContext.PageData.IsAdd;
        btnApprove.Visible = MyContext.PageData.IsApprove;
    }

    private void CalcIncentives(ABFTextBox txtIncentives, ABFTextBox txtAbsence, ABFTextBox txtDeducatedDaysOff, int contact_ID)
    {
        // var lblContact_ID = ((Label)gvPayRoll.Rows[rowindex].FindControl("lblContact_ID")).Text.ToIntOrDefault();



        txtIncentives.Text = (dc.fun_HR_OtherSalaryValues(contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt()).FirstOrDefault().Incentives +
            dc.fun_HR_AttendanceIncentives(contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), (txtAbsence.Text.ToIntOrDefault() + txtDeducatedDaysOff.Text.ToIntOrDefault()))).ToExpressString();

    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
    protected void gvPayRoll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // TextBox txt = e.Row.FindControl("txtID") as TextBox;


            this.EditID = gvPayRoll.DataKeys[e.Row.RowIndex]["ID"].ToInt();
            var payroll = dc.usp_HR_PayRoll_SelectByID(this.EditID).FirstOrDefault();




            // this.Contact_ID = payroll.Contact_ID.Value;
            // this.BasicSalary = payroll.BasicSalary.Value;
            lblHeader.Text = payroll.ContactName + " - " + payroll.Month.ToExpressString() + "/" + payroll.Year.ToExpressString() + " - " + payroll.BasicSalary.Value.ToExpressString();


            var txtOverTime = ((ABFTextBox)e.Row.FindControl("txtOverTime"));
            var txtOverTimeValue = ((ABFTextBox)e.Row.FindControl("txtOverTimeValue"));


            var txtDayOffWork = ((ABFTextBox)e.Row.FindControl("txtDayOffWork"));
            var txtDayOffWorkValue = ((ABFTextBox)e.Row.FindControl("txtDayOffWorkValue"));

            var txtDelay = ((ABFTextBox)e.Row.FindControl("txtDelay"));
            var txtDelayValue = ((ABFTextBox)e.Row.FindControl("txtDelayValue"));

            var txtAbsence = ((ABFTextBox)e.Row.FindControl("txtAbsence"));
            var txtAbsenceValue = ((ABFTextBox)e.Row.FindControl("txtAbsenceValue"));


            var txtExcuse = ((ABFTextBox)e.Row.FindControl("txtExcuse"));
            var txtExcuseValue = ((ABFTextBox)e.Row.FindControl("txtExcuseValue"));



            var txtDeducatedDaysOff = ((ABFTextBox)e.Row.FindControl("txtDeducatedDaysOff"));
            var txtDeducatedDaysOffValue = ((ABFTextBox)e.Row.FindControl("txtDeducatedDaysOffValue"));


            var txtDeducatedWorkDays = ((ABFTextBox)e.Row.FindControl("txtDeducatedWorkDays"));
            var txtDeducatedWorkDaysValue = ((ABFTextBox)e.Row.FindControl("txtDeducatedWorkDaysValue"));

            var txtIncentives = ((ABFTextBox)e.Row.FindControl("txtIncentives"));
            var txtAllowances = ((ABFTextBox)e.Row.FindControl("txtAllowances"));

            var txtTaxes = ((ABFTextBox)e.Row.FindControl("txtTaxes"));
            var txtInsurance = ((ABFTextBox)e.Row.FindControl("txtInsurance"));


            var txtLoans = ((ABFTextBox)e.Row.FindControl("txtLoans"));
            var txtSancations = ((ABFTextBox)e.Row.FindControl("txtSancations"));

            var txtOtherDeducations = ((ABFTextBox)e.Row.FindControl("txtOtherDeducations"));
            var txtOtherAdditions = ((ABFTextBox)e.Row.FindControl("txtOtherAdditions"));
            var lblGrossTotal = ((Label)e.Row.FindControl("lblGrossTotal"));

            var txtNotes = ((ABFTextBox)e.Row.FindControl("txtNotes"));

            var lblBasicSalary = ((Label)e.Row.FindControl("lblBasicSalary"));

            var lblContact_ID = ((Label)e.Row.FindControl("lblContact_ID"));


            lblBasicSalary.Text = payroll.BasicSalary.ToExpressString();
            lblContact_ID.Text = payroll.Contact_ID.ToExpressString();
            txtOverTime.Text = payroll.OvertimePeriod.ToExpressString();
            txtOverTimeValue.Text = payroll.OvertimeValue.ToExpressString();

            txtDayOffWork.Text = payroll.DayOffWorkPeriod.ToExpressString();
            txtDayOffWorkValue.Text = payroll.DayOffWorkValue.ToExpressString();

            txtDelay.Text = payroll.DelayPeriod.ToExpressString();
            txtDelayValue.Text = payroll.DelayValue.ToExpressString();

            txtAbsence.Text = payroll.AbsencePeriod.ToExpressString();
            txtAbsenceValue.Text = payroll.AbsenceValue.ToExpressString();

            txtExcuse.Text = payroll.ExcusePeriod.ToExpressString();
            txtExcuseValue.Text = payroll.ExcuseValue.ToExpressString();

            txtDeducatedDaysOff.Text = payroll.VacationWithDeducationPeriod.ToExpressString();
            txtDeducatedDaysOffValue.Text = payroll.VacationWithDeducationValue.ToExpressString();

            txtDeducatedWorkDays.Text = payroll.WorkWithDeductionPeriod.ToExpressString();
            txtDeducatedWorkDaysValue.Text = payroll.WorkWithDeductionValue.ToExpressString();

            txtIncentives.Text = payroll.Incentives.ToExpressString();





            // txtAllowances.Text = payroll.Allowances.ToExpressString();
            txtAllowances.Text = dc.usp_EmployeAllowance_Select(payroll.Contact_ID).ToList().Sum(x => x.MonthlyAllowance).ToExpressString();

            var listEmployeeExpence = dc.usp_EmployeExpense_Select(payroll.Contact_ID).ToList();



            txtTaxes.Text = payroll.Taxes.ToExpressString();
            txtInsurance.Text = payroll.Insurance.ToExpressString();
            //payroll.Insurance.ToExpressString();


            //txtTaxes.Text = payroll.Taxes.ToExpressString();
            //txtInsurance.Text = "0";//listEmployeeExpence.Where(x => x.TypeEmployeExpense == 9).Sum(x => x.MonthlyEmployeExpense).ToExpressString();
            ////payroll.Insurance.ToExpressString();

            txtLoans.Text = payroll.Loans.ToExpressString();
            txtSancations.Text = payroll.Sancations.ToExpressString();


            //   txtOtherAdditions.Text = listEmployeeExpence.Where(x => x.TypeEmployeExpense == 10).Sum(x => x.MonthlyEmployeExpense).ToExpressString();

            txtOtherAdditions.Text = payroll.OtherAdditions.ToExpressString();
            txtOtherDeducations.Text = payroll.OtherDeducations.ToExpressString();

            lblGrossTotal.Text = payroll.GrossTotal.ToExpressString();

            txtNotes.Text = payroll.Notes;


            txtOverTimeValue.ForeColor =
            txtDayOffWorkValue.ForeColor =
            txtIncentives.ForeColor =
            txtAllowances.ForeColor =
            txtOtherAdditions.ForeColor = System.Drawing.Color.Green;

            txtDelayValue.ForeColor =
                    txtExcuseValue.ForeColor =
                    txtAbsenceValue.ForeColor =
                    txtDeducatedDaysOffValue.ForeColor =
                    txtDeducatedWorkDaysValue.ForeColor =
                    txtTaxes.ForeColor =
                    txtInsurance.ForeColor =
                    txtLoans.ForeColor =
                    txtSancations.ForeColor =
                    txtOtherDeducations.ForeColor = System.Drawing.Color.Red;
        }
        //  var payroll = dc.usp_HR_PayRoll_SelectByID(this.EditID).FirstOrDefault();
    }
    protected void bvtnShowHide_Click(object sender, EventArgs e)
    {
        var lst = (new List<clsShowHide>());
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 1, IndexColumn = 1, NameColumn = Resources.Labels.Department });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 2, IndexColumn = 2, NameColumn = Resources.Labels.Name });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 3, IndexColumn = 3, NameColumn = Resources.Labels.Month });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 4, IndexColumn = 4, NameColumn = Resources.Labels.Year });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 5, IndexColumn = 5, NameColumn = Resources.Labels.GrossTotal });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 6, IndexColumn = 6, NameColumn = Resources.Labels.OverTime });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 7, IndexColumn = 7, NameColumn = Resources.Labels.OverTimeValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 8, IndexColumn = 8, NameColumn = Resources.Labels.DayOffWork });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 9, IndexColumn = 9, NameColumn = Resources.Labels.DayOffWorkValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 10, IndexColumn = 10, NameColumn = Resources.Labels.Delay });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 11, IndexColumn = 11, NameColumn = Resources.Labels.DelayValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 12, IndexColumn = 12, NameColumn = Resources.Labels.Absence });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 13, IndexColumn = 13, NameColumn = Resources.Labels.AbsenceValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 15, IndexColumn = 15, NameColumn = Resources.Labels.Excuse });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 16, IndexColumn = 16, NameColumn = Resources.Labels.ExcuseValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 17, IndexColumn = 17, NameColumn = Resources.Labels.DeducatedWorkDays });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 18, IndexColumn = 18, NameColumn = Resources.Labels.DeducatedWorkDaysValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 19, IndexColumn = 19, NameColumn = Resources.Labels.DeducatedDaysOff });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 20, IndexColumn = 20, NameColumn = Resources.Labels.DeducatedDaysOffValue });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 21, IndexColumn = 21, NameColumn = Resources.Labels.Incentives });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 22, IndexColumn = 22, NameColumn = Resources.Labels.Allowances });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 23, IndexColumn = 23, NameColumn = Resources.Labels.Taxes });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 24, IndexColumn = 24, NameColumn = Resources.Labels.Insurance });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 25, IndexColumn = 25, NameColumn = Resources.Labels.HRLoan });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 26, IndexColumn = 26, NameColumn = Resources.Labels.Sancations });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 27, IndexColumn = 27, NameColumn = "مكافئات - زيادات أخرى" });
        lst.Add(new clsShowHide() { checkedIndex = false, ID = 28, IndexColumn = 28, NameColumn = Resources.Labels.OtherDeducations });

        var ObjectFormatJson = dc.HiddenGrids.Where(c => c.Contact_ID == MyContext.UserProfile.Contact_ID && c.Page_ID == MyContext.PageData.PageID).FirstOrDefault();
        if (ObjectFormatJson != null)
        {
            var str = ObjectFormatJson.JsonFormControl.ToExpressString();
            if (!string.IsNullOrEmpty(str))
            {
                List<clsShowHide> data = JsonConvert.DeserializeObject<List<clsShowHide>>(str);
                foreach (var item in lst)
                {
                    var obj = data.Where(c => c.ID == item.ID).FirstOrDefault();
                    if (obj != null)
                    {
                        item.checkedIndex = obj.checkedIndex;
                    }

                }

            }
        }

        gvShowHide.DataSource = lst.CopyToDataTable();
        gvShowHide.DataBind();
        //foreach (GridViewRow itemGv in gvShowHide.Rows)
        //{



        //    var chk = gvShowHide.DataKeys[itemGv.RowIndex]["checkedIndex"].ToBooleanOrDefault();
        //    var chkBox = ((CheckBox)itemGv.FindControl("chkApproveShowHide"));
        //    var txt = ((TextBox)itemGv.FindControl("txtCh"));
        //    chkBox.Checked = chk;
        //    txt.Text = gvShowHide.DataKeys[itemGv.RowIndex]["checkedIndex"].ToExpressString();

        //}





        ModalPopupExtender1.Show();
    }
    protected void btnSaveShowHide_Click(object sender, EventArgs e)
    {
        var lst = (new List<clsShowHide>());
        foreach (GridViewRow gvRow in gvShowHide.Rows)
        {
            var ID1 = gvShowHide.DataKeys[gvRow.RowIndex]["ID"].ToInt();
            var IndexColumn1 = gvShowHide.DataKeys[gvRow.RowIndex]["IndexColumn"].ToInt();
            var NameColumn1 = gvShowHide.DataKeys[gvRow.RowIndex]["NameColumn"].ToExpressString();
            lst.Add(new clsShowHide() { checkedIndex = ((CheckBox)gvRow.FindControl("chkApproveShowHide")).Checked, ID = ID1, IndexColumn = IndexColumn1, NameColumn = NameColumn1 });
        }
        var ObjectFormatJson = dc.HiddenGrids.Where(c => c.Contact_ID == MyContext.UserProfile.Contact_ID && c.Page_ID == MyContext.PageData.PageID).FirstOrDefault();
        if (ObjectFormatJson != null)
        {
            // ObjectFormatJson
            var objEdit = dc.HiddenGrids.Where(c => c.ID == ObjectFormatJson.ID).FirstOrDefault();
            objEdit.JsonFormControl = JsonConvert.SerializeObject(lst);
            dc.SubmitChanges();
        }
        else
        {
            var hG = new HiddenGrid()
            {
                JsonFormControl = JsonConvert.SerializeObject(lst),
                Contact_ID = MyContext.UserProfile.Contact_ID,
                Page_ID = MyContext.PageData.PageID,

            };
            dc.HiddenGrids.InsertOnSubmit(hG);
            dc.SubmitChanges();

        }
    }
    protected void gvShowHide_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var chk = gvShowHide.DataKeys[e.Row.RowIndex]["checkedIndex"].ToBooleanOrDefault();
            var chkBox = ((CheckBox)e.Row.FindControl("chkApproveShowHide"));
          
            chkBox.Checked = chk;
            
        }



    }
}