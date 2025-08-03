using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Sales_Installment : UICulturePage
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

    private int InstallmentDetails_ID
    {
        get
        {
            if (ViewState["InstallmentDetails_ID"] == null) return 0;
            return (int)ViewState["InstallmentDetails_ID"];
        }

        set
        {
            ViewState["InstallmentDetails_ID"] = value;
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
    private DataTable dtCusInstallments
    {
        get
        {
            if (Session["dtCusInstallments" + this.WinID] == null)
            {
                Session["dtCusInstallments" + this.WinID] = dc.usp_InstallmentsDetails_Select(0).CopyToDataTable();
            }
            return (DataTable)Session["dtCusInstallments" + this.WinID];
        }

        set
        {
            Session["dtCusInstallments" + this.WinID] = value;
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
                if (EditMode) this.FillInstallment();
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

    protected void btnPay_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {

            string Serial = string.Empty;
            decimal ratio = txtRatio.Text.ToDecimal();
            int? CashIn_ID = null;

            if (txtPayDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
            int? Currency_ID = dc.usp_Customer_SelectByID(acCustomer.Value.ToInt()).First().Currency_ID;
            CashIn_ID = dc.usp_Payments_Insert(txtPayDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, txtInstallmentValue.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), Currency_ID, null, null, this.DocRandomString + "_" + this.InstallmentDetails_ID.ToString());
            if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
            dc.usp_PaymentsDetails_Insert(CashIn_ID, acOppositeAccount.Value.ToInt(), ContactChartOfAccount_ID, txtInstallmentValue.Text.ToDecimal(), null, string.Empty, null);

            int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtPayDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), Currency_ID, txtInstallmentValue.Text.ToDecimal() * ratio, txtInstallmentValue.Text.ToDecimal(), ratio, null);
            dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtInstallmentValue.Text.ToDecimal() * ratio, 0, txtInstallmentValue.Text.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
            dc.usp_OperationDetails_Insert(Operation_ID, acOppositeAccount.Value.ToInt(), txtInstallmentValue.Text.ToDecimal() * ratio, 0, txtInstallmentValue.Text.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());

            dc.usp_InstallmentsDetails_Update(this.InstallmentDetails_ID, true);

            this.dtCusInstallments = dc.usp_InstallmentsDetails_Select(this.Installment_ID).CopyToDataTable();
            this.BindDetailsGrid();

            LogAction(Actions.Approve, "سداد قسط عميل " + " : " + txtSerial.Text + " : " + txtInstallmentValue.Text + " : " + txtPayDate.Text, dc);
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
            this.dtCusInstallments.Rows.Clear();
            this.dtCusInstallments.AcceptChanges();
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
            DateTime? StartDate = txtStartDate.Text.ToDate();
            this.dtCusInstallments.Rows.Clear();
            decimal NumberOfInstallments = 0;
            decimal Total = 0;
            decimal value = 0;

            switch (ddlPayEveryType.SelectedValue)
            {
                case "0":
                    NumberOfInstallments = Math.Ceiling(txtPeriod.Text.ToDecimal() * 30.00m / txtPayEvery.Text.ToDecimal());
                    break;

                case "1":
                    NumberOfInstallments = Math.Ceiling(txtPeriod.Text.ToDecimal() / txtPayEvery.Text.ToDecimal());
                    break;
                case "2":
                    NumberOfInstallments = Math.Ceiling(txtPeriod.Text.ToDecimal() / (txtPayEvery.Text.ToDecimal() * 12.00m));
                    break;
            }

            for (int i = 0; i < NumberOfInstallments; i++)
            {
                r = this.dtCusInstallments.NewRow();
                r["Date"] = StartDate;

                if (i == NumberOfInstallments - 1)
                {
                    value = txtAmount.Text.ToDecimal() - Total;
                }
                else
                {
                    value = Math.Ceiling(txtAmount.Text.ToDecimal() / NumberOfInstallments);
                    Total += value;
                }
                if (value <= 0) continue;
                r["Value"] = value;
                r["isPaid"] = false;
                this.dtCusInstallments.Rows.Add(r);

                switch (ddlPayEveryType.SelectedValue)
                {
                    case "0":
                        StartDate = StartDate.Value.AddDays(txtPayEvery.Text.ToInt());
                        break;

                    case "1":
                        StartDate = StartDate.Value.AddMonths(txtPayEvery.Text.ToInt());
                        break;
                    case "2":
                        StartDate = StartDate.Value.AddYears(txtPayEvery.Text.ToInt());
                        break;
                }
            }
            txtEndDate.Text = StartDate.Value.ToString("d/M/yyyy");
            txtInstallmentsNumber.Text = NumberOfInstallments.ToExpressString();
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
            this.FilterByBranchAndCurrency();
            this.InstallmentDetails_ID = gvDetails.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtCusInstallments.Select("ID=" + this.InstallmentDetails_ID.ToExpressString())[0];
            txtPayDate.Clear();
            if (r["Date"].ToDate().Value <= MyContext.FiscalYearEndDate && r["Date"].ToDate().Value >= MyContext.FiscalYearStartDate)
            {
                txtPayDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            }
            this.txtPaydate_TextChnaged(null, null);
            txtInstallmentValue.Text = r["Value"].ToExpressString();
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
            dc.usp_Installments_Cancel(this.Installment_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Installment + Request.PathInfo + "?ID=" + this.Installment_ID.ToExpressString(), PageLinks.InstallmentsList, PageLinks.Installment);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtPaydate_TextChnaged(object sender, EventArgs e)
    {
        try
        {
            int? Currency_ID = dc.usp_Customer_SelectByID(acCustomer.Value.ToInt()).First().Currency_ID;
            var ratio = dc.fun_GetCurrentRatio(Currency_ID, txtPayDate.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            mpePay.Show();
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
            int? Currency_ID = acCustomer.HasValue ? dc.usp_Customer_SelectByID(acCustomer.Value.ToInt()).First().Currency_ID : (int?)null;
            acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + "," + COA.CashOnHand.ToInt() + ",true";
            acCustomer.ContextKey = "C," + acBranch.Value + ",,";
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void LoadControls()
    {
        this.dtCusInstallments = null;
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
        gvDetails.DataSource = this.dtCusInstallments;
        gvDetails.DataBind();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Installment_ID = Request["ID"].ToInt();
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

        if (this.dtCusInstallments.Rows.Count <= 0)
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

            this.Installment_ID = dc.usp_Installments_Insert(acBranch.Value.ToNullableInt(), ref Serial, DocSerials.Installment.ToInt(), DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, txtNotes.Text, this.DocRandomString, txtStartDate.Text.ToDate(), acCustomer.Value.ToInt(), txtAmount.Text.ToDecimal(), txtPayEvery.Text.ToInt(), ddlPayEveryType.SelectedValue.ToByte(), txtPeriod.Text.ToInt(), txtEndDate.Text.ToDate(), txtUserRefNo.TrimmedText);
            if (this.Installment_ID > 0)
            {
                foreach (DataRow r in this.dtCusInstallments.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_InstallmentsDetails_Insert(this.Installment_ID, r["Date"].ToDate(), r["value"].ToDecimal());

                }

                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Installments_Update(this.Installment_ID, acBranch.Value.ToNullableInt(), Serial, DocSerials.Installment.ToInt(), DocStatus_ID, ApproveDate, approvedBY_ID, txtNotes.Text, txtStartDate.Text.ToDate(), acCustomer.Value.ToInt(), txtAmount.Text.ToDecimal(), txtPayEvery.Text.ToInt(), ddlPayEveryType.SelectedValue.ToByte(), txtPeriod.Text.ToInt(), txtEndDate.Text.ToDate(), txtUserRefNo.TrimmedText);
            if (Result > 0)
            {
                dc.usp_InstallmentsDetails_Delete(this.Installment_ID);
                foreach (DataRow r in this.dtCusInstallments.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_InstallmentsDetails_Insert(this.Installment_ID, r["Date"].ToDate(), r["value"].ToDecimal());
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Installment + Request.PathInfo + "?ID=" + this.Installment_ID.ToExpressString(), PageLinks.InstallmentsList, PageLinks.Installment);
        return true;
    }

    private void FillInstallment()
    {
        var Installment = dc.usp_Installments_SelectByID(this.Installment_ID).FirstOrDefault();

        acBranch.Value = Installment.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtUserRefNo.Text = Installment.UserRefNo;
        txtNotes.Text = Installment.Notes;
        txtSerial.Text = Installment.Serial;
        acCustomer.Value = Installment.Contact_ID.ToExpressString();
        txtAmount.Text = Installment.Amount.ToExpressString();
        txtPayEvery.Text = Installment.PayEvery.ToExpressString();
        ddlPayEveryType.SelectedValue = Installment.PayEveryType.ToExpressString();
        txtPeriod.Text = Installment.Period.ToExpressString();
        txtStartDate.Text = Installment.StartDate.Value.ToString("d/M/yyyy");
        this.DocRandomString = Installment.DocRandomString;
        lblCreatedBy.Text = Installment.CreatedByName;
        lblApprovedBy.Text = Installment.ApprovedBYName;
        txtStartDate.Text = Installment.StartDate.Value.ToString("d/M/yyyy");
        txtEndDate.Text = Installment.EndDate.Value.ToString("d/M/yyyy");
        this.ImgStatus = ((DocStatus)Installment.DocStatus_ID).ToExpressString();
        btnPrint.Visible = MyContext.PageData.IsPrint;
        btnCancel.Visible = (Installment.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Installment.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (Installment.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        acCustomer.Enabled = btnClacInstallments.Enabled = (Installment.DocStatus_ID == 1);
        acBranch.Enabled = (Installment.DocStatus_ID == 1) && (this.MyContext.UserProfile.Branch_ID == null);
        gvDetails.Columns[2].Visible = (Installment.DocStatus_ID == 2) && MyContext.PageData.IsApprove;

        this.dtCusInstallments = dc.usp_InstallmentsDetails_Select(this.Installment_ID).CopyToDataTable();
        txtInstallmentsNumber.Text = this.dtCusInstallments.Rows.Count.ToExpressString();
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
        decimal Paid = 0;
        foreach (DataRow r in this.dtCusInstallments.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            Total += r["Value"].ToDecimal();
            if (r["IsPaid"].ToBoolean()) Paid += r["Value"].ToDecimal();
        }
        lblTotal.Text = Total.ToString("0.####");
        lblRemaining.Text = (Total - Paid).ToString("0.####");
    }

    private void CustomPage()
    {
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
}