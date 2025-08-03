using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Loans : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtHRLoans
    {
        get
        {
            return (DataTable)Session["dtHRLoans" + this.WinID];
        }

        set
        {
            Session["dtHRLoans" + this.WinID] = value;
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
                this.Fill();
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

    protected void acEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            var company = dc.usp_Company_Select().FirstOrDefault();
            int? Branch_ID = acEmployee.HasValue ? dc.usp_HR_Employees_SelectByID(acEmployee.Value.ToInt()).FirstOrDefault().Branch_ID : (int?)null;
            acCreditAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + Branch_ID + "," + company.Currency_ID.ToString();
            if (acEmployee.HasValue)
            {
                var contact_ID = acEmployee.Value.ToIntOrDefault();
                var emp = dc.Contacts.Where(c => c.ID == contact_ID).FirstOrDefault();
                if (emp != null)
                {
                    if (emp.Branch_ID != null)
                    {
                        acBranchEmp.Value = emp.Branch_ID.ToExpressString();
                        acBranchEmp.Enabled = false;
                    }
                    else
                    {
                        acBranchEmp.Clear();
                        acBranchEmp.Enabled = true;
                    }

                }
                else
                {
                    acBranchEmp.Clear();
                    acBranchEmp.Enabled = true;
                }


            }


            if (sender != null) mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
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
            acEmployeeSrch.ContextKey = ",," + acBranch.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            acEmployeeSrch.AutoCompleteFocus();
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
            acEmployeeSrch.Clear();
            txtFromDateSrch.Clear();
            txtToDateSrch.Clear();
            ddlStatusSrch.SelectedIndex = 0;
            chkIsFinished.Checked = false;
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterEmployees(null, null);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvLoans_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvLoans.PageIndex = e.NewPageIndex;
            gvLoans.DataSource = this.dtHRLoans;
            gvLoans.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvLoans_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtHRLoans.Select("ID=" + gvLoans.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acEmployee.Value = dr["Contact_ID"].ToExpressString();
            acCreditAccount.Value = dr["CreditChartOfAccount_ID"].ToExpressString();
            txtInstallemntsNumber.Text = dr["InstallmentsNumber"].ToExpressString();
            txtDate.Text = dr["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtStartDate.Text = dr["StartDate"].ToDate().Value.ToString("d/M/yyyy");
            txtValue.Text = dr["Value"].ToExpressString();
            this.EditID = gvLoans.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvLoans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_Loans_Delete(gvLoans.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvLoans.DataKeys[e.RowIndex]["ContactName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            this.ClearForm();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;

            if (txtDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }

            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_Loans_Insert(acEmployee.Value.ToInt(), acCreditAccount.Value.ToInt(), txtInstallemntsNumber.Text.ToInt(), txtDate.Text.ToDate(), txtStartDate.Text.ToDate(), txtValue.Text.ToDecimal(), false, ddlStatus.SelectedValue.ToByte());
            }
            else
            {
                result = dc.usp_HR_Loans_Update(this.EditID, acCreditAccount.Value.ToInt(), txtInstallemntsNumber.Text.ToInt(), txtDate.Text.ToDate(), txtStartDate.Text.ToDate(), txtValue.Text.ToDecimal(), false, ddlStatus.SelectedValue.ToByte());
            }

            if (ddlStatus.SelectedValue == DocStatus.Approved.ToByte().ToExpressString())
            {
                if (!this.InsertOperations(this.EditID == 0 ? result : this.EditID))
                {
                    trans.Rollback();
                    mpeCreateNew.Show();
                    return;
                }
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acEmployee.Text, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkFinihed_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int ID = gvLoans.DataKeys[((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex]["ID"].ToInt();
            dc.usp_HR_Loans_Update(ID, null, null, null, null, null, true, null);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        acEmployee.Clear();
        txtValue.Clear();
        txtStartDate.Clear();
        txtDate.Clear();
        acCreditAccount.Clear();
        ddlStatus.SelectedIndex = 0;
        txtInstallemntsNumber.Clear();
        this.acEmployee_SelectedIndexChanged(null, null);
    }

    private bool InsertOperations(int Loan_ID)
    {
        string serial = string.Empty;
        var emp = dc.usp_HR_Employees_SelectByID(acEmployee.Value.ToInt()).FirstOrDefault();
        var company = dc.usp_Company_Select().FirstOrDefault();

        if ((emp.Branch_ID == null && acBranchEmp.Value == null) )
        {
            UserMessages.Message(null, Resources.UserInfoMessages.EmpBranchReq, string.Empty);
            return false;
        }

        int LoanAccount_ID = dc.usp_HR_CreateLoanAccount(acEmployee.Value.ToInt());
        if (LoanAccount_ID == -2)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
            return false;
        }

        int Result = dc.usp_Operation_Insert(emp.Branch_ID, txtDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.HRLoan.ToInt(), company.Currency_ID, txtValue.Text.ToDecimal(), txtValue.Text.ToDecimal(), 1, null);

        //حساب السلف
        dc.usp_OperationDetails_Insert(Result, LoanAccount_ID, txtValue.Text.ToDecimal(), 0, txtValue.Text.ToDecimal(), 0, null, Loan_ID, DocumentsTableTypes.HR_Loans.ToInt());

        //حساب السلف
        dc.usp_OperationDetails_Insert(Result, acCreditAccount.Value.ToInt(), 0, txtValue.Text.ToDecimal(), 0, txtValue.Text.ToDecimal(), null, Loan_ID, DocumentsTableTypes.HR_Loans.ToInt());

        return true;
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        acBranchEmp.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
        this.acEmployee_SelectedIndexChanged(null, null);
        acEmployee.ContextKey = ",," + MyContext.UserProfile.Branch_ID;
    }

    private void Fill()
    {
        byte? DocStatus_ID = ddlStatusSrch.SelectedIndex == 0 ? (byte?)null : ddlStatusSrch.SelectedValue.ToByte();
        this.dtHRLoans = dc.usp_HR_Loans_Select(acEmployeeSrch.Value.ToNullableInt(), txtFromDateSrch.Text.ToDate(), txtToDateSrch.Text.ToDate(), chkIsFinished.Checked, DocStatus_ID, acBranch.Value.ToNullableInt()).CopyToDataTable();
        gvLoans.DataSource = this.dtHRLoans;
        gvLoans.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvLoans.Columns[6].Visible = MyContext.PageData.IsEdit;
        gvLoans.Columns[7].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvLoans.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion



    protected void Unnamed_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            //gvLoans.DataKeys[e.RowIndex]["ID"].ToInt()
            //GridViewRow Row = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int ID = gvLoans.DataKeys[Index]["ID"].ToInt();
            //int ID = gvLoans.DataKeys[((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex]["ID"].ToInt();
            dc.usp_CancelApprove_HR_Loans(ID);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}