using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_PayrollPayment : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtPayrollPayment
    {
        get
        {
            return (DataTable)Session["dtPayrollPayment" + this.WinID];
        }

        set
        {
            Session["dtPayrollPayment" + this.WinID] = value;
        }
    }

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
            acCreditAccount.AutoCompleteFocus();
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
            gvPayRoll.DataSource = this.dtPayrollPayment;
            gvPayRoll.DataBind();
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
            var company = dc.usp_Company_Select().FirstOrDefault();
            acName.ContextKey = acDepartment.Value + ",," + acBranch.Value;
            acCreditAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToString() + "," + acBranch.Value + "," + company.Currency_ID.ToString();
            if (this.dtPayrollPayment != null)
            {
                this.dtPayrollPayment.Rows.Clear();
                gvPayRoll.DataSource = this.dtPayrollPayment;
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
            if (txtDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                return;
            }
            string serial = string.Empty;
            var company = dc.usp_Company_Select().FirstOrDefault();
            decimal? PaidAmount = 0;

            foreach (GridViewRow gvRow in gvPayRoll.Rows)
            {
                if (!((CheckBox)gvRow.FindControl("chkPay")).Checked) continue;

                PaidAmount = gvPayRoll.Columns[5].Visible ? ((TextBox)gvRow.FindControl("txtPaidAmount")).Text.ToDecimalOrDefault() : gvPayRoll.DataKeys[gvRow.RowIndex]["Salary"].ToDecimalOrDefault();
                if (PaidAmount <= 0) continue;

                if (PaidAmount > gvPayRoll.DataKeys[gvRow.RowIndex]["Salary"].ToDecimalOrDefault())
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.PaidAmountGreaterThanSalary, string.Empty);
                    trans.Rollback();
                    return;
                }
                int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.SalaryPayment.ToInt(), company.Currency_ID, PaidAmount, PaidAmount, 1, txtNotes.Text);

                //راتب مستحق
                dc.usp_OperationDetails_Insert(Result, gvPayRoll.DataKeys[gvRow.RowIndex]["ChartOfAccount_ID"].ToNullableInt(), PaidAmount, 0, PaidAmount, 0, null, null, null);
                //الحساب الدائن
                dc.usp_OperationDetails_Insert(Result, acCreditAccount.Value.ToInt(), 0, PaidAmount, 0, PaidAmount, null, null, null);
                LogAction(Actions.Approve, gvPayRoll.DataKeys[gvRow.RowIndex]["ContactName"] + " :" + PaidAmount.ToExpressString(), dc);
            }
            this.Fill();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }




    #endregion

    #region Private Methods

    private void LoadControls()
    {
        acDepartment.ContextKey = string.Empty;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
        gvPayRoll.DataSource = this.dtPayrollPayment;
        gvPayRoll.DataBind();
    }

    private void Fill()
    {
        this.dtPayrollPayment = dc.usp_HR_PayrollPayment_Select(acName.Value.ToNullableInt(), acDepartment.Value.ToNullableInt(), acBranch.Value.ToNullableInt()).CopyToDataTable();
        gvPayRoll.DataSource = this.dtPayrollPayment;
        gvPayRoll.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvPayRoll.Columns[5].Visible = MyContext.PageData.IsEdit;
        btnApprove.Visible = MyContext.PageData.IsApprove;
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvPayRoll.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion
}