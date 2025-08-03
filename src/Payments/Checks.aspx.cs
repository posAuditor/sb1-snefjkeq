using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Payments_Checks : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    List<ChecksOperationDetail> OperationDetailsList = new List<ChecksOperationDetail>();

    #region Properties

    protected string ImgStatus
    {
        get
        {
            string result = Page.ResolveClientUrl("~/images/");
            if (ViewState["ImgStatus"] == null) result += "new"; else result += ViewState["ImgStatus"].ToExpressString();
            result += this.MyContext.CurrentCulture == XPRESS.Common.ABCulture.Arabic ? "-ar" : string.Empty;
            return result;
        }

        set
        {
            ViewState["ImgStatus"] = value;
        }
    }

    private int Check_ID
    {
        get
        {
            if (ViewState["Check_ID"] == null) return 0;
            return (int)ViewState["Check_ID"];
        }

        set
        {
            ViewState["Check_ID"] = value;
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

    private DataTable dCheckDetails
    {
        get
        {
            if (Session["dCheckDetails" + this.WinID] == null)
            {
                Session["dCheckDetails" + this.WinID] = dc.usp_ChecksDetails_Select(null, 0).CopyToDataTable();
            }
            return (DataTable)Session["dCheckDetails" + this.WinID];
        }

        set
        {
            Session["dCheckDetails" + this.WinID] = value;
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

    private decimal Total
    {
        get
        {
            if (ViewState["Total"] == null) return 0;
            return (decimal)ViewState["Total"];
        }

        set
        {
            ViewState["Total"] = value;
        }
    }

    private int Serial_ID
    {
        get
        {
            if (ViewState["Serial_ID"] == null) return 0;
            return (int)ViewState["Serial_ID"];
        }

        set
        {
            ViewState["Serial_ID"] = value;
        }
    }

    private byte EntryType
    {
        get
        {
            if (ViewState["EntryType"] == null) return 0;
            return (byte)ViewState["EntryType"];
        }

        set
        {
            ViewState["EntryType"] = value;
        }
    }

    private int DocumentTableType_ID
    {
        get
        {
            if (ViewState["DocumentTableType_ID"] == null) return 0;
            return (int)ViewState["DocumentTableType_ID"];
        }

        set
        {
            ViewState["DocumentTableType_ID"] = value;
        }
    }

    private int OperationType_ID
    {
        get
        {
            if (ViewState["OperationType_ID"] == null) return 0;
            return (int)ViewState["OperationType_ID"];
        }

        set
        {
            ViewState["OperationType_ID"] = value;
        }
    }

    private string DocRandomString
    {
        get
        {
            if (ViewState["DocRandomString"] == null)
            {
                ViewState["DocRandomString"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString"];
        }

        set
        {
            ViewState["DocRandomString"] = value;
        }
    }

    private int? RelatedDoc_ID
    {
        get
        {
            if (ViewState["RelatedDoc_ID"] == null) return null;
            return (int?)ViewState["RelatedDoc_ID"];
        }

        set
        {
            ViewState["RelatedDoc_ID"] = value;
        }
    }

    private int? RelatedDocTableType_ID
    {
        get
        {
            if (ViewState["RelatedDocumentTableType_ID"] == null) return null;
            return (int?)ViewState["RelatedDocumentTableType_ID"];
        }

        set
        {
            ViewState["RelatedDocumentTableType_ID"] = value;
        }
    }

    private bool ConfirmationAnswered
    {
        get
        {
            if (ViewState["ConfirmationAnswered"] == null) return false;
            return (bool)ViewState["ConfirmationAnswered"];
        }

        set
        {
            ViewState["ConfirmationAnswered"] = value;
        }
    }

    private string ConfirmationMessage
    {
        get
        {
            if (ViewState["ConfirmationMessage"] == null)
            {
                ViewState["ConfirmationMessage"] = string.Empty;
            }
            return (string)ViewState["ConfirmationMessage"];
        }

        set
        {
            ViewState["ConfirmationMessage"] = value;
        }
    }

    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.SetEditMode();
            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.SetPageWorkingMode();
                this.CheckSecurity();
                this.LoadControls();
                if (EditMode)
                {
                    this.FillCheck();
                }
                else
                {
                    this.FillFromBill();
                }
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

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtIssueDate.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            this.FilterByBranchAndCurrency();
            this.ShowBalances();
            this.FilterSalesReps();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddDetail_click(object sender, EventArgs e)
    {
        try
        {

            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dCheckDetails.NewRow();
                r["ID"] = this.dCheckDetails.GetID("ID");

            }
            else
            {
                r = this.dCheckDetails.Select("ID=" + this.EditID)[0];
            }

            r["Account_ID"] = acAccount.Value.ToInt();
            r["Notes"] = txtDetailNotes.Text;
            r["CostCenter_ID"] = acDetailCostCenter.Value.ToIntOrDBNULL();
            r["AccountName"] = acAccount.Text;
            r["CostCenterName"] = acDetailCostCenter.Text;
            r["SalesRepName"] = acSalesRep.Text;
            r["SalesRep_ID"] = acSalesRep.Value.ToIntOrDBNULL();
            r["Amount"] = txtAmount.Text.ToDecimal();

            if (this.EditID == 0) this.dCheckDetails.Rows.Add(r);

            this.ClearDetailForm();
            this.BindDetailsGrid();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearDetail_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearDetailForm();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDetails.PageIndex = e.NewPageIndex;
            this.BindDetailsGrid();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditID = gvDetails.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dCheckDetails.Select("ID=" + this.EditID.ToExpressString())[0];

            acAccount.Value = r["Account_ID"].ToExpressString();
            this.ShowBalances();
            this.FilterSalesReps();
            acSalesRep.Value = r["SalesRep_ID"].ToExpressString();
            acDetailCostCenter.Value = r["CostCenter_ID"].ToExpressString();
            txtAmount.Text = r["Amount"].ToDecimal().ToExpressString();
            txtDetailNotes.Text = r["Notes"].ToExpressString();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvDetails.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dCheckDetails.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            this.ClearDetailForm();
            this.BindDetailsGrid();
            this.EditID = 0;
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FilterByBranchAndCurrency();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(false, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
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
            if (this.Save(true, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            dc.usp_Checks_Cancel(this.Check_ID, MyContext.UserProfile.Contact_ID, DateTime.Now);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Checks + Request.PathInfo + "?ID=" + this.Check_ID.ToExpressString(), PageLinks.ChecksList + Request.PathInfo, PageLinks.Checks + Request.PathInfo);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlCheckStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtCollectingExpenses.Clear();
            txtCollectingDate.Clear();
            this.txtCollectingExpenses_TextChanged(null, null);
            if (ddlCheckStatus.SelectedValue == "2" && sender != null)
            {
                ddlCheckStatus.SelectedValue = "0";
                UserMessages.Message(null, Resources.UserInfoMessages.CantRefuseBeforeApprove, string.Empty);
            }
            this.SetPageWorkingMode();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtCollectingExpenses_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acExpensesAccount.IsRequired = (txtCollectingExpenses.Text.ToDecimalOrDefault() > 0);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlBeginingBalanceCheck_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtStartFrom.Clear();
            if (ddlBeginingBalanceCheck.SelectedValue.ToBoolean()) txtStartFrom.Text = MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
            txtStartFrom.Visible = acOppsiteAccount.Visible = ddlBeginingBalanceCheck.SelectedValue.ToBoolean();
            if (txtIssueDate.IsDateFiscalYearRestricted == ddlBeginingBalanceCheck.SelectedValue.ToBoolean())
            {
                txtIssueDate.IsDateFiscalYearRestricted = !ddlBeginingBalanceCheck.SelectedValue.ToBoolean();
                txtIssueDate.Clear();
            }
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\Check_Print.rpt"));
            doc.SetParameterValue("@Check_ID", this.Check_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Check"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnYes_click(object sender, EventArgs e)
    {
        try
        {
            this.ConfirmationAnswered = true;
            this.BtnApprove_Click(null, null);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods
    private void FilterByBranchAndCurrency()
    {
        try
        {

            acIssueBank.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + COA.Banks.ToInt().ToExpressString() + ",false";
            acDepositAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue;
            acIntermediateAccount.ContextKey = acExpensesAccount.ContextKey = acAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",,true";
            acDetailCostCenter.ContextKey = acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
            acExpensesAccount.Value = COA.BankExpensesAndBenefits.ToInt().ToExpressString();
            acIntermediateAccount.Value = Request.PathInfo == "/CheckIn" ? COA.NotesReceivable.ToInt().ToExpressString() : COA.NotesPayable.ToInt().ToExpressString();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void LoadControls()
    {
        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        this.dCheckDetails = null;
        acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        this.FilterByBranchAndCurrency();
        this.BindDetailsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }

    private void BindDetailsGrid()
    {
        this.Calculate();
        gvDetails.DataSource = this.dCheckDetails;
        gvDetails.DataBind();
        acBranch.Enabled = (gvDetails.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
        ddlCurrency.Enabled = (gvDetails.Rows.Count == 0);
    }

    private void ClearDetailForm()
    {
        acAccount.Clear();
        acAccount.AutoCompleteFocus();
        acSalesRep.Clear();
        txtAmount.Clear();
        acDetailCostCenter.Clear();
        txtDetailNotes.Clear();
        this.ShowBalances();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Check_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {

        if (this.Total <= 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
            trans.Rollback();
            return false;
        }

        if (ddlCheckStatus.SelectedValue == "1" && txtCollectingDate.Text.ToDate() < txtIssueDate.Text.ToDate())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CollectDateLessIssus, string.Empty);
            trans.Rollback();
            return false;
        }

        if (ddlBeginingBalanceCheck.SelectedValue.ToBoolean() && txtIssueDate.Text.ToDate() >= MyContext.FiscalYearStartDate)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateMustBeforeFiscalYear, string.Empty);
            trans.Rollback();
            return false;
        }

        if (ddlBeginingBalanceCheck.SelectedValue.ToBoolean() && txtStartFrom.Text.ToDate() < txtIssueDate.Text.ToDate())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.StartFromDateLessIssus, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtIssueDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (ddlCheckStatus.SelectedValue == "1" && txtCollectingDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (IsApproving && this.RelatedDoc_ID != null && !dc.fun_CanCollectMore(this.RelatedDoc_ID, this.RelatedDocTableType_ID, this.Total).Value)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CanNOTCollectMore, string.Empty);
            trans.Rollback();
            return false;
        }

        this.ConfirmationMessage = string.Empty;
        OperationDetailsList.Clear();
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {

            this.Check_ID = dc.usp_Checks_Insert(DateTime.Now, MyContext.UserProfile.Contact_ID, approvedBY_ID, ApproveDate, txtUserRefNo.Text, this.Serial_ID, ref Serial, txtNotes.Text, acCostCenter.Value.ToNullableInt(), DocStatus_ID, acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), txtIssueDate.Text.ToDate(), txtMaturityDate.Text.ToDate(), txtCollectingDate.Text.ToDate(), acIssueBank.Value.ToNullableInt(), acDepositAccount.Value.ToNullableInt(), acIntermediateAccount.Value.ToNullableInt(), acExpensesAccount.Value.ToNullableInt(), this.Total, ddlCheckStatus.SelectedValue.ToByte(), txtCollectingExpenses.Text.ToDecimalOrDefault(), txtCheckNo.Text, this.EntryType, this.RelatedDoc_ID, this.RelatedDocTableType_ID, ddlBeginingBalanceCheck.SelectedValue.ToBoolean(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt(), this.DocRandomString);
            if (this.Check_ID > 0)
            {
                foreach (DataRow r in this.dCheckDetails.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_ChecksDetails_Insert(this.Check_ID, r["Account_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt());
                    if (IsApproving)
                    {
                        OperationDetailsList.Add(new ChecksOperationDetail()
                        {
                            Account_ID = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? acOppsiteAccount.Value.ToInt() : r["Account_ID"].ToInt(),
                            Amount = r["Amount"].ToDecimal(),
                            CostCenter_ID = r["CostCenter_ID"].ToNullableInt(),
                            Notes = r["Notes"].ToExpressString()
                        });
                    }
                }
                if (IsApproving) InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Checks_Update(this.Check_ID, approvedBY_ID, ApproveDate, txtUserRefNo.Text, this.Serial_ID, Serial, txtNotes.Text, acCostCenter.Value.ToNullableInt(), DocStatus_ID, acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), txtIssueDate.Text.ToDate(), txtMaturityDate.Text.ToDate(), txtCollectingDate.Text.ToDate(), acIssueBank.Value.ToNullableInt(), acDepositAccount.Value.ToNullableInt(), acIntermediateAccount.Value.ToNullableInt(), acExpensesAccount.Value.ToNullableInt(), txtCheckNo.Text, this.Total, ddlCheckStatus.SelectedValue.ToByte(), txtCollectingExpenses.Text.ToDecimalOrDefault(), ddlBeginingBalanceCheck.SelectedValue.ToBoolean(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());
            if (Result > 0)
            {
                foreach (DataRow r in this.dCheckDetails.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        dc.usp_ChecksDetails_Insert(this.Check_ID, r["Account_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt());
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        dc.usp_ChecksDetails_Update(r["ID"].ToInt(), r["Account_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_ChecksDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                    if (r.RowState != DataRowState.Deleted && IsApproving)
                    {
                        OperationDetailsList.Add(new ChecksOperationDetail()
                        {
                            Account_ID = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? acOppsiteAccount.Value.ToInt() : r["Account_ID"].ToInt(),
                            Amount = r["Amount"].ToDecimal(),
                            CostCenter_ID = r["CostCenter_ID"].ToNullableInt(),
                            Notes = r["Notes"].ToExpressString()
                        });
                    }
                }

                if (IsApproving) InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        if (this.ConfirmationMessage != string.Empty)
        {
            ltConfirmationMessage.Text = this.ConfirmationMessage;
            mpeConfirm.Show();
            trans.Rollback();
            return false;
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Checks + Request.PathInfo + "?ID=" + this.Check_ID.ToExpressString(), PageLinks.ChecksList + Request.PathInfo, PageLinks.Checks + Request.PathInfo);
        return true;
    }

    private void FillCheck()
    {
        var Check = dc.usp_Checks_SelectByID(this.Check_ID).FirstOrDefault();
        ddlCurrency.SelectedValue = Check.Currency_ID.ToExpressString();
        txtRatio.Text = Check.Ratio.ToExpressString();
        acBranch.Value = Check.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        if (Check.CheckStatus == 2) ddlCheckStatus.Items.Add(new ListItem(Resources.Labels.Refused, "2"));
        ddlCheckStatus.SelectedValue = Check.CheckStatus.ToExpressString();
        this.ddlCheckStatus_SelectedIndexChanged(null, null);
        ddlBeginingBalanceCheck.SelectedValue = Check.BeginingBalanceCheck.ToExpressString();
        this.ddlBeginingBalanceCheck_SelectedIndexChanged(null, null);
        if (Check.StartFromDate.HasValue) txtStartFrom.Text = Check.StartFromDate.Value.ToString("d/M/yyyy");
        if (Check.OppositeAccount_ID.HasValue) acOppsiteAccount.Value = Check.OppositeAccount_ID.ToExpressString();
        acCostCenter.Value = Check.CostCenter_ID.ToStringOrEmpty();
        acIntermediateAccount.Value = Check.IntermediateAccount_ID.ToStringOrEmpty();
        acExpensesAccount.Value = Check.ExpensesAccount_ID.ToStringOrEmpty();
        acIssueBank.Value = Check.FromBankChartOfAccount_ID.ToStringOrEmpty();
        acDepositAccount.Value = Check.ToBankChartOfAccount_ID.ToStringOrEmpty();
        txtUserRefNo.Text = Check.UserRefNumber;
        txtNotes.Text = Check.Notes;
        txtSerial.Text = Check.Serial;
        txtCheckNo.Text = Check.CheckNo;
        txtCollectingExpenses.Text = Check.CollectingExpenses.ToExpressString();
        this.txtCollectingExpenses_TextChanged(null, null);
        this.DocRandomString = Check.DocRandomString;
        lblCreatedBy.Text = Check.CeartedByName;
        lblApprovedBy.Text = Check.ApprovedByName;
        txtIssueDate.Text = Check.IssueDate.Value.ToString("d/M/yyyy");
        txtMaturityDate.Text = Check.MaturityDate.Value.ToString("d/M/yyyy");
        if (Check.CollectedOrRefusedDate.HasValue) txtCollectingDate.Text = Check.CollectedOrRefusedDate.Value.ToString("d/M/yyyy");
        this.ImgStatus = ((DocStatus)Check.DocStatus_ID).ToExpressString();
        this.RelatedDoc_ID = Check.RelatedDoc_ID;
        this.RelatedDocTableType_ID = Check.RelatedDocTableType_ID;
        btnPrint.Visible = MyContext.PageData.IsPrint;
        btnCancelcollection.Visible = (Check.DocStatus_ID == 2) && (Check.CheckStatus == 1) && (Request.PathInfo == "/CheckIn") && (btnPrint.Visible = true);
        pnlAddDetail.Visible = (Check.DocStatus_ID == 1);
        btnCancel.Visible = (Check.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Check.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (Check.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        gvDetails.Columns[gvDetails.Columns.Count - 1].Visible = gvDetails.Columns[gvDetails.Columns.Count - 2].Visible = (Check.DocStatus_ID == 1);


        btnCancelcollection.Visible = (Check.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;


        this.dCheckDetails = dc.usp_ChecksDetails_Select(this.Check_ID, MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        this.BindDetailsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        btnCancelcollection.Visible =   MyContext.PageData.IsNotApprove;

    }

    private void Calculate()
    {
        this.Total = 0;
        foreach (DataRow r in this.dCheckDetails.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            Total += r["Amount"].ToDecimal();
        }
        lblTotal.Text = this.Total.ToString("0.####");
        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;
    }

    private void ShowBalances()
    {
        lblAccountBalance.Text = dc.fun_GetAccountBalanceInForeign(acAccount.Value.ToNullableInt(), txtIssueDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value.ToExpressString();
    }

    private void FilterSalesReps()
    {
        int? Contact_ID = dc.fun_GetCustomerContactID(acAccount.Value.ToNullableInt());
        if (Contact_ID == null)
        {
            acSalesRep.ContextKey = "0,";
            return;
        }
        acSalesRep.ContextKey = Contact_ID.ToExpressString() + ",," + acBranch.Value;
        if (!this.EditMode && this.RelatedDoc_ID == null)
        {
            int? DefaultRep_ID = dc.fun_GetDefaultSalesRep_ID(Contact_ID, null);
            if (DefaultRep_ID.HasValue) acSalesRep.Value = DefaultRep_ID.ToExpressString();
        }
    }

    private void SetPageWorkingMode()
    {
        switch (Request.PathInfo)
        {
            case "/CheckIn":
                this.Serial_ID = DocSerials.ReceivedCheck.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Checks_Received.ToInt();
                this.EntryType = ChecksTypes.ReceivedCheck.ToByte();
                acIssueBank.IsRequired = false;
                acAccount.LabelText = Resources.Labels.Drawer;
                break;

            case "/CheckOut":
                this.Serial_ID = DocSerials.IssusedCheck.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Checks_Issued.ToInt();
                this.EntryType = ChecksTypes.IssuedCheck.ToByte();
                acIssueBank.IsRequired = true;
                acAccount.LabelText = Resources.Labels.Beneficiary;
                break;
        }
        acExpensesAccount.Visible = txtCollectingDate.Visible = txtCollectingExpenses.Visible = (ddlCheckStatus.SelectedValue != "0");
        divBeginingBalanceCheck.Visible = acIntermediateAccount.Visible = (ddlCheckStatus.SelectedValue == "0");
        acDepositAccount.Visible = (Request.PathInfo == "/CheckIn") && ddlCheckStatus.SelectedValue != "0";
        if (ddlCheckStatus.SelectedValue == "1")
        {
            ddlBeginingBalanceCheck.SelectedValue = "False";
            this.ddlBeginingBalanceCheck_SelectedIndexChanged(null, null);
        }
    }

    private void InsertOperation()
    {
        if (OperationDetailsList.Count == 0) return;
        decimal ratio = txtRatio.Text.ToDecimal();

        if (Request.PathInfo == "/CheckIn")
        {
            this.InsertOperationReceivedChecks();
        }
        else
        {
            this.InsertOperationIssuedChecks();
        }
        if (ddlBeginingBalanceCheck.SelectedValue.ToBoolean() == false)
        {
            //CostCenter
            foreach (var Detail in OperationDetailsList)
            {
                dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), Detail.CostCenter_ID, txtMaturityDate.Text.ToDate(), (Detail.Amount) * ratio, this.Check_ID, this.DocumentTableType_ID, Detail.Notes);
            }
            dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtMaturityDate.Text.ToDate(), this.Total * ratio, this.Check_ID, this.DocumentTableType_ID, txtNotes.Text);
        }
    }

    private void InsertOperationIssuedChecks()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        decimal Expenses = ddlCheckStatus.SelectedValue == "0" ? 0 : txtCollectingExpenses.Text.ToDecimalOrDefault();
        string serial = string.Empty;
        int? BankAccount_ID = ddlCheckStatus.SelectedValue == "0" ? acIntermediateAccount.Value.ToInt() : acIssueBank.Value.ToInt();
        this.OperationType_ID = ddlCheckStatus.SelectedValue == "0" ? OperationTypes.IssuedCheck.ToInt() : OperationTypes.CollectIssusedCheck.ToInt();
        DateTime? OperationDate = ddlCheckStatus.SelectedValue == "0" ? txtIssueDate.Text.ToDate() : txtCollectingDate.Text.ToDate();
        OperationDate = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? txtStartFrom.Text.ToDate() : OperationDate;
        this.OperationType_ID = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? OperationTypes.OpenBalance.ToInt() : this.OperationType_ID;

        if (!this.ConfirmationAnswered && ddlCheckStatus.SelectedValue == "1" && this.Total > dc.fun_GetAccountBalanceInForeign(acIssueBank.Value.ToInt(), txtCollectingDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value)
        {
            this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BankBalanceNotEnough;
        }

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), OperationDate, ref serial, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), (this.Total + Expenses) * ratio, (this.Total + Expenses), ratio, txtNotes.Text);

        //الدائن
        dc.usp_OperationDetails_Insert(Result, BankAccount_ID, 0, this.Total * ratio, 0, this.Total, txtNotes.Text, this.Check_ID, this.DocumentTableType_ID);
        //دائن حساب البنك
        if (Expenses > 0) dc.usp_OperationDetails_Insert(Result, BankAccount_ID, 0, Expenses * ratio, 0, Expenses, "مصروفات تحصيل", this.Check_ID, this.DocumentTableType_ID);

        //المدين
        foreach (var Detail in OperationDetailsList)
        {
            dc.usp_OperationDetails_Insert(Result, Detail.Account_ID, Detail.Amount * ratio, 0, Detail.Amount, 0, Detail.Notes, this.Check_ID, this.DocumentTableType_ID);
        }
        //مدين حساب المصروفات
        if (Expenses > 0) dc.usp_OperationDetails_Insert(Result, acExpensesAccount.Value.ToInt(), Expenses * ratio, 0, Expenses, 0, "مصروفات تحصيل", this.Check_ID, this.DocumentTableType_ID);

    }

    private void InsertOperationReceivedChecks()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        decimal Expenses = ddlCheckStatus.SelectedValue == "0" ? 0 : txtCollectingExpenses.Text.ToDecimalOrDefault();
        string serial = string.Empty;
        int? BankAccount_ID = ddlCheckStatus.SelectedValue == "0" ? acIntermediateAccount.Value.ToInt() : acDepositAccount.Value.ToInt();
        this.OperationType_ID = ddlCheckStatus.SelectedValue == "0" ? OperationTypes.ReceivedCheck.ToInt() : OperationTypes.CollectReceivedCheck.ToInt();
        DateTime? OperationDate = ddlCheckStatus.SelectedValue == "0" ? txtIssueDate.Text.ToDate() : txtCollectingDate.Text.ToDate();
        OperationDate = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? txtStartFrom.Text.ToDate() : OperationDate;
        this.OperationType_ID = ddlBeginingBalanceCheck.SelectedValue.ToBoolean() ? OperationTypes.OpenBalance.ToInt() : this.OperationType_ID;

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), OperationDate, ref serial, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), (this.Total + Expenses) * ratio, (this.Total + Expenses), ratio, txtNotes.Text);

        //المدين
        dc.usp_OperationDetails_Insert(Result, BankAccount_ID, this.Total * ratio, 0, this.Total, 0, txtNotes.Text, this.Check_ID, this.DocumentTableType_ID);
        //دائن حساب البنك
        if (Expenses > 0) dc.usp_OperationDetails_Insert(Result, BankAccount_ID, 0, Expenses * ratio, 0, Expenses, "مصروفات تحصيل", this.Check_ID, this.DocumentTableType_ID);

        //الدائن
        foreach (var Detail in OperationDetailsList)
        {
            dc.usp_OperationDetails_Insert(Result, Detail.Account_ID, 0, Detail.Amount * ratio, 0, Detail.Amount, Detail.Notes, this.Check_ID, this.DocumentTableType_ID);
        }
        //مدين حساب المصروفات
        if (Expenses > 0) dc.usp_OperationDetails_Insert(Result, acExpensesAccount.Value.ToInt(), Expenses * ratio, 0, Expenses, 0, "مصروفات تحصيل", this.Check_ID, this.DocumentTableType_ID);
    }

    private void FillFromBill()
    {
        if (Request["RelatedDoc_ID"] == null) return;

        this.RelatedDoc_ID = Request["RelatedDoc_ID"].ToInt();
        this.RelatedDocTableType_ID = Request["RelatedDocTableType_ID"].ToInt();

        if (this.RelatedDocTableType_ID == 1)
        {
            var invoice = dc.usp_Invoice_SelectByID(this.RelatedDoc_ID).FirstOrDefault();
            ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
            acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
            this.FilterByBranchAndCurrency();
            acAccount.Value = dc.fun_getContactAccountID(invoice.Contact_ID).Value.ToExpressString();
            this.FilterSalesReps();
            acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
        }
        else if (this.RelatedDocTableType_ID == 2)
        {
            var Receipt = dc.usp_Receipt_SelectByID(this.RelatedDoc_ID).FirstOrDefault();
            ddlCurrency.SelectedValue = Receipt.Currency_ID.ToExpressString();
            acBranch.Value = Receipt.Branch_ID.ToStringOrEmpty();
            this.FilterByBranchAndCurrency();
            acAccount.Value = dc.fun_getContactAccountID(Receipt.Contact_ID).Value.ToExpressString();
        }
    }

    private void CustomPage()
    {
        acDetailCostCenter.Visible = acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;

        acSalesRep.Visible = MyContext.Features.SalesRepEnabled && (Request.PathInfo == "/CheckIn");
        foreach (DataControlField col in gvDetails.Columns)
        {
            if (col.ItemStyle.CssClass == "SalesRepCol") col.Visible = MyContext.Features.SalesRepEnabled && (Request.PathInfo == "/CheckIn");
        }
    }

    #endregion
    protected void btnCancelcollection_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Request.PathInfo == "/CheckIn")
            //{
            dc.usp_Check_Cancel(this.Check_ID);
                
                  UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess  , PageLinks.Checks + Request.PathInfo + "?ID=" + this.Check_ID.ToExpressString(), PageLinks.ChecksList + Request.PathInfo, PageLinks.Checks + Request.PathInfo);
                  LogAction(Actions.NotApprove, txtSerial.Text, dc);
            
            //}
            //else
            //{
            //    this.InsertOperationIssuedChecks();
            //}
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
      
    }
}