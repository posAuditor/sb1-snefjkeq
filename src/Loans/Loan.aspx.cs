using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Loans_Loan : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

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

    private int Loan_ID
    {
        get
        {
            if (ViewState["Payment_ID"] == null) return 0;
            return (int)ViewState["Payment_ID"];
        }

        set
        {
            ViewState["Payment_ID"] = value;
        }
    }

    private int Installment_ID
    {
        get
        {
            if (ViewState["Installment_ID"] == null) return 0;
            return (int)ViewState["Installment_ID"];
        }

        set
        {
            ViewState["Installment_ID"] = value;
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

    private DataTable dtLoanInstallments
    {
        get
        {
            if (Session["dtLoanInstallments" + this.WinID] == null)
            {
                Session["dtLoanInstallments" + this.WinID] = dc.usp_LoansDetails_Select(0).CopyToDataTable();
            }
            return (DataTable)Session["dtLoanInstallments" + this.WinID];
        }

        set
        {
            Session["dtLoanInstallments" + this.WinID] = value;
        }
    }

    private int? ChartOfAccount_ID
    {
        get
        {
            return (int?)ViewState["ChartOfAccount_ID"];
        }

        set
        {
            ViewState["ChartOfAccount_ID"] = value;
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
                this.CheckSecurity();
                this.LoadControls();
                if (EditMode)
                {
                    this.FillLoan();
                }
                else
                {
                    this.FillDocCreditInstallment();
                }
            }

            ucNav.SourceDocTypeType_ID = DocumentsTableTypes.Loans.ToInt();
            ucNav.EntryType = 0;
            ucNav.Res_ID = this.Loan_ID;
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
        Response.Redirect(PageLinks.Loan);
    }
    void ucNav_btnHandlerSearch(string strValue)
    {
        RefillForm(strValue);
    }


    private void RefillForm(string strValue)
    {
        if (!string.IsNullOrEmpty(strValue))
        {
            this.Loan_ID = strValue.ToInt();
            // this.EditMode = strValue.ToInt();
            this.EditMode = true;

            this.DocRandomString.ToString();
            this.CheckSecurity();
            this.LoadControls();
            if (EditMode)
            {
                this.FillLoan();
            }
            else
            {
                this.FillDocCreditInstallment();
            }
        }
    }






    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void btnPay_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {

            string Serial = string.Empty;
            decimal Amount = txtInstallmentValue.Text.ToDecimalOrDefault();
            int Currency_ID = dc.DefaultCurrancy().Value;

            if (txtPayDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            DataRow r = this.dtLoanInstallments.Select("ID=" + this.Installment_ID.ToExpressString())[0];

            int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtPayDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.PayLoanInstallment.ToInt(), Currency_ID, Amount, Amount, 1, null);

            dc.usp_OperationDetails_Insert(Operation_ID, this.ChartOfAccount_ID, r["value"].ToDecimalOrDefault(), 0, r["value"].ToDecimalOrDefault(), 0, null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());

            dc.usp_OperationDetails_Insert(Operation_ID, COA.BankExpensesAndBenefits.ToInt(), r["benefit"].ToDecimalOrDefault(), 0, r["benefit"].ToDecimalOrDefault(), 0, null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());

            dc.usp_OperationDetails_Insert(Operation_ID, acOppositeAccount.Value.ToInt(), 0, r["total"].ToDecimalOrDefault(), 0, r["total"].ToDecimalOrDefault(), null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());

            dc.usp_LoansDetails_Update(this.Installment_ID, true);

            this.dtLoanInstallments = dc.usp_LoansDetails_Select(this.Loan_ID).CopyToDataTable();
            this.BindDetailsGrid();

            //CostCenter
            dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtReceivingDate.Text.ToDate(), r["benefit"].ToDecimalOrDefault(), this.Loan_ID, DocumentsTableTypes.Loans.ToInt(), txtNotes.Text);

            LogAction(Actions.Approve, "سداد قسط قرض " + " : " + txtSerial.Text + " : " + Amount + " : " + txtPayDate.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClearInstallments(object sender, EventArgs e)
    {
        try
        {
            this.dtLoanInstallments.Rows.Clear();
            this.dtLoanInstallments.AcceptChanges();
            this.BindDetailsGrid();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClacInstallments_click(object sender, EventArgs e)
    {
        try
        {

            DataRow r = null;
            this.dtLoanInstallments.Rows.Clear();


            int numberOfYears = (ddlPeriodType.SelectedValue == "2") ? txtPeriod.Text.ToInt() : Convert.ToInt32(Math.Ceiling((double)txtPeriod.Text.ToInt() / 12));
            int numberOfMonths = (ddlPeriodType.SelectedValue == "1") ? txtPeriod.Text.ToInt() : numberOfYears * 12;
            DateTime? StartDate = txtStartDate.Text.ToDate();

            if (txtPayEvery.Text.ToInt() > numberOfMonths)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PayEveryGreaterThanInstallmentsNumber, string.Empty);
                return;
            }

            if (txtStartDate.Text.ToDate() < txtReceivingDate.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.ReceDateAfterStartDate, string.Empty);
                return;
            }

            decimal installmentsNumber = Math.Ceiling((decimal)numberOfMonths / txtPayEvery.Text.ToDecimal());
            txtInstallmentsNumber.Text = installmentsNumber.ToExpressString();


            if (ddlBenefitType.SelectedValue == "0")
            {

                decimal TotalInstallmentsWithBenefit = (txtAmount.Text.ToDecimal() * txtBenefit.Text.ToDecimal() * (decimal)0.01 * numberOfYears) + txtAmount.Text.ToDecimal();
                decimal Installment = TotalInstallmentsWithBenefit / installmentsNumber;
                decimal InstallmentBenefit = (TotalInstallmentsWithBenefit - txtAmount.Text.ToDecimal()) / installmentsNumber;
                decimal InstallmentWithoutBenefit = Installment - InstallmentBenefit;

                while (installmentsNumber > 0)
                {
                    r = this.dtLoanInstallments.NewRow();
                    r["Date"] = StartDate;
                    r["Value"] = Math.Round(InstallmentWithoutBenefit, 4, MidpointRounding.AwayFromZero);
                    r["Benefit"] = Math.Round(InstallmentBenefit, 4, MidpointRounding.AwayFromZero);
                    r["Total"] = Math.Round((InstallmentWithoutBenefit + InstallmentBenefit), 4, MidpointRounding.AwayFromZero);
                    r["isPaid"] = false;
                    this.dtLoanInstallments.Rows.Add(r);
                    if (installmentsNumber != 1) StartDate = StartDate.Value.AddMonths(txtPayEvery.Text.ToInt());
                    installmentsNumber--;
                }

            }
            else // متناقصة
            {
                decimal InstallmentBenefit = 0;
                decimal InstallmentWithoutBenefit = txtAmount.Text.ToDecimal() / installmentsNumber;
                decimal MyValue = txtAmount.Text.ToDecimal();
                while (installmentsNumber > 0)
                {
                    InstallmentBenefit = (MyValue * txtBenefit.Text.ToDecimal() * (decimal)0.01) * (txtPayEvery.Text.ToDecimal() / (decimal)12);
                    r = this.dtLoanInstallments.NewRow();
                    r["Date"] = StartDate;
                    r["Value"] = Math.Round(InstallmentWithoutBenefit, 4, MidpointRounding.AwayFromZero);
                    r["Benefit"] = Math.Round(InstallmentBenefit, 4, MidpointRounding.AwayFromZero);
                    r["Total"] = Math.Round((InstallmentWithoutBenefit + InstallmentBenefit), 4, MidpointRounding.AwayFromZero);
                    r["isPaid"] = false;
                    this.dtLoanInstallments.Rows.Add(r);
                    if (installmentsNumber != 1) StartDate = StartDate.Value.AddMonths(txtPayEvery.Text.ToInt());
                    installmentsNumber--;
                    MyValue -= InstallmentWithoutBenefit;
                }

            }
            txtEndDate.Text = StartDate.Value.ToString("d/M/yyyy");
            this.BindDetailsGrid();
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
            this.Installment_ID = gvDetails.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtLoanInstallments.Select("ID=" + this.Installment_ID.ToExpressString())[0];
            txtPayDate.Clear();
            if (r["Date"].ToDate().Value <= MyContext.FiscalYearEndDate && r["Date"].ToDate().Value >= MyContext.FiscalYearStartDate)
            {
                txtPayDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            }
            txtInstallmentValue.Text = r["Total"].ToExpressString();
            mpePay.Show();
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
            dc.usp_Loans_Cancel(this.Loan_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Loan + Request.PathInfo + "?ID=" + this.Loan_ID.ToExpressString(), PageLinks.LoansList, PageLinks.Loan);
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
            int Currency_ID = dc.DefaultCurrancy().Value;
            acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + ",,true";
            acBank.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + "," + COA.Banks.ToInt() + ",false";
            acExpensesCostCenter.ContextKey = acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void LoadControls()
    {
        this.dtLoanInstallments = null;
        acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }

        this.FilterByBranchAndCurrency();
        this.BindDetailsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;


    }

    private void BindDetailsGrid()
    {
        this.Calculate();
        gvDetails.DataSource = this.dtLoanInstallments;
        gvDetails.DataBind();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Loan_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {

        if (lblTotal.Text.ToDecimalOrDefault() <= 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtReceivingDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (this.dtLoanInstallments.Rows.Count <= 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CalcInstallmentsFirst, string.Empty);
            trans.Rollback();
            return false;
        }


        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {

            this.Loan_ID = dc.usp_Loans_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), acExpensesCostCenter.Value.ToNullableInt(), ref Serial, DocSerials.Loan.ToInt(), DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, txtNotes.Text, this.DocRandomString, txtReceivingDate.Text.ToDate(), txtLoanNumber.Text, acBank.Value.ToInt(), txtAmount.Text.ToDecimal(), txtExpenses.Text.ToDecimalOrDefault(), txtPeriod.Text.ToInt(), ddlPeriodType.SelectedValue.ToByte(), txtBenefit.Text.ToDecimal(), ddlBenefitType.SelectedValue.ToByte(), txtPayEvery.Text.ToInt(), txtStartDate.Text.ToDate(), txtEndDate.Text.ToDate(), txtUserRefNo.TrimmedText, this.RelatedDoc_ID, this.RelatedDocTableType_ID);
            if (this.Loan_ID > 0)
            {
                foreach (DataRow r in this.dtLoanInstallments.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_LoansDetails_Insert(this.Loan_ID, r["Date"].ToDate(), r["value"].ToDecimal(), r["Benefit"].ToDecimal());

                }
                if (IsApproving)
                {
                    if (!this.InsertOperation())
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Loans_Update(this.Loan_ID, acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), acExpensesCostCenter.Value.ToNullableInt(), Serial, DocSerials.Loan.ToInt(), DocStatus_ID, ApproveDate, approvedBY_ID, txtNotes.Text, txtReceivingDate.Text.ToDate(), txtLoanNumber.Text, acBank.Value.ToInt(), txtAmount.Text.ToDecimal(), txtExpenses.Text.ToDecimalOrDefault(), txtPeriod.Text.ToInt(), ddlPeriodType.SelectedValue.ToByte(), txtBenefit.Text.ToDecimal(), ddlBenefitType.SelectedValue.ToByte(), txtPayEvery.Text.ToInt(), txtStartDate.Text.ToDate(), txtEndDate.Text.ToDate(), txtUserRefNo.TrimmedText);
            if (Result > 0)
            {
                dc.usp_LoansDetails_Delete(this.Loan_ID);
                foreach (DataRow r in this.dtLoanInstallments.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_LoansDetails_Insert(this.Loan_ID, r["Date"].ToDate(), r["value"].ToDecimal(), r["Benefit"].ToDecimal());
                }

                if (IsApproving)
                {
                    if (!this.InsertOperation())
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Loan + Request.PathInfo + "?ID=" + this.Loan_ID.ToExpressString(), PageLinks.LoansList, PageLinks.Loan);
        return true;
    }

    private void FillLoan()
    {
        var Loan = dc.usp_Loans_SelectByID(this.Loan_ID).FirstOrDefault();

        acBranch.Value = Loan.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        acCostCenter.Value = Loan.CostCenter_ID.ToStringOrEmpty();
        acExpensesCostCenter.Value = Loan.ExpensesCostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = Loan.UserRefNo;
        txtNotes.Text = Loan.Notes;
        txtSerial.Text = Loan.Serial;
        ucNav.SetText = Loan.Serial;

        acBank.Value = Loan.BankChartOfAccount_ID.ToExpressString();
        txtLoanNumber.Text = Loan.LoanNumber;
        txtAmount.Text = Loan.Amount.ToExpressString();
        txtExpenses.Text = Loan.Expenses.ToExpressString();
        txtPeriod.Text = Loan.Period.ToExpressString();
        ddlPeriodType.SelectedValue = Loan.PeriodType.ToExpressString();
        txtPayEvery.Text = Loan.PayEveryMonth.ToExpressString();
        txtBenefit.Text = Loan.BenefitPercentage.ToExpressString();
        ddlBenefitType.SelectedValue = Loan.BenefitType.ToExpressString();
        txtStartDate.Text = Loan.StartPayingDate.Value.ToString("d/M/yyyy");
        this.DocRandomString = Loan.DocRandomString;
        lblCreatedBy.Text = Loan.CreatedByName;
        lblApprovedBy.Text = Loan.ApprovedBYName;
        txtReceivingDate.Text = Loan.ReceiveDate.Value.ToString("d/M/yyyy");
        txtEndDate.Text = Loan.EndPayingDate.Value.ToString("d/M/yyyy");
        this.ImgStatus = ((DocStatus)Loan.DocStatus_ID).ToExpressString();
        btnPrint.Visible = MyContext.PageData.IsPrint;
        btnCancel.Visible = (Loan.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Loan.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (Loan.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        acBank.Enabled = btnClacInstallments.Enabled = (Loan.DocStatus_ID == 1);
        acBranch.Enabled = Loan.RelatedDoc_ID == null && (Loan.DocStatus_ID == 1) && (this.MyContext.UserProfile.Branch_ID == null);
        this.ChartOfAccount_ID = Loan.ChartOfAccount_ID;
        gvDetails.Columns[4].Visible = (Loan.DocStatus_ID == 2) && MyContext.PageData.IsApprove;
        this.RelatedDoc_ID = Loan.RelatedDoc_ID;
        this.RelatedDocTableType_ID = Loan.RelatedDocTableType_ID;

        this.dtLoanInstallments = dc.usp_LoansDetails_Select(this.Loan_ID).CopyToDataTable();
        txtInstallmentsNumber.Text = this.dtLoanInstallments.Rows.Count.ToExpressString();
        this.BindDetailsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
    }

    private void Calculate()
    {
        decimal Total = 0;
        foreach (DataRow r in this.dtLoanInstallments.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            Total += r["Total"].ToDecimal();
        }
        lblTotal.Text = Total.ToString("0.####");
    }

    private bool InsertOperation()
    {
        string serial = string.Empty;
        int Currency_ID = dc.DefaultCurrancy().Value;
        this.ChartOfAccount_ID = dc.usp_Loans_Approve(this.Loan_ID, txtLoanNumber.Text, acBranch.Value.ToNullableInt());

        if (this.ChartOfAccount_ID == -2)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.LoanNumberExists, string.Empty);
            return false;
        }

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtReceivingDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Loan.ToInt(), Currency_ID, txtAmount.Text.ToDecimal() + txtExpenses.Text.ToDecimalOrDefault(), txtAmount.Text.ToDecimal() + txtExpenses.Text.ToDecimalOrDefault(), 1, txtNotes.Text);

        dc.usp_OperationDetails_Insert(Result, acBank.Value.ToInt(), txtAmount.Text.ToDecimal(), 0, txtAmount.Text.ToDecimal(), 0, null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());
        dc.usp_OperationDetails_Insert(Result, this.ChartOfAccount_ID, 0, txtAmount.Text.ToDecimal(), 0, txtAmount.Text.ToDecimal(), null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());

        if (txtExpenses.Text.ToDecimalOrDefault() > 0)
        {
            dc.usp_OperationDetails_Insert(Result, COA.BanksExpensesAndBenefits.ToInt(), txtExpenses.Text.ToDecimal(), 0, txtExpenses.Text.ToDecimal(), 0, null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());
            dc.usp_OperationDetails_Insert(Result, acBank.Value.ToInt(), 0, txtExpenses.Text.ToDecimal(), 0, txtExpenses.Text.ToDecimal(), null, this.Loan_ID, DocumentsTableTypes.Loans.ToInt());
        }

        //CostCenter
        if (txtExpenses.Text.ToDecimalOrDefault() > 0) dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acExpensesCostCenter.Value.ToNullableInt(), txtReceivingDate.Text.ToDate(), txtExpenses.Text.ToDecimalOrDefault(), this.Loan_ID, DocumentsTableTypes.Loans.ToInt(), txtNotes.Text);

        if (this.RelatedDoc_ID != null)
        {
            dc.usp_DocumentryCreditInstallments_Update(this.RelatedDoc_ID, null, null, true, txtAmount.Text.ToDecimal(), 1, DocumentsTableTypes.Loans.ToInt(), this.Loan_ID);
            LogAction(Actions.Approve, "سداد بقرض لقسط اعتماد مستندى " + " : " + txtAmount.Text, dc);
        }
        return true;
    }

    private void FillDocCreditInstallment()
    {
        if (Request["RelatedDoc_ID"] == null) return;
        lblDocCreditInstallment.Text = Request["DocCreditInstallment"].ToExpressString();
        divDocCreditInstallments.Visible = true;
        this.RelatedDoc_ID = Request["RelatedDoc_ID"].ToInt();
        this.RelatedDocTableType_ID = Request["RelatedDocTableType_ID"].ToInt();
        acBranch.Value = Request["Branch_ID"].ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        acBranch.Enabled = false;

        var OldLoan_ID = (from data in dc.Loans where data.RelatedDoc_ID == this.RelatedDoc_ID && data.RelatedDocTableType_ID == this.RelatedDocTableType_ID && data.DocStatus_ID != 3 select data).FirstOrDefault();
        if (OldLoan_ID != null)
        {
            Response.Redirect(PageLinks.Loan + "?ID=" + OldLoan_ID.ID.ToExpressString(), false);
            return;
        }
    }

    private void CustomPage()
    {
        acExpensesCostCenter.Visible = acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
}