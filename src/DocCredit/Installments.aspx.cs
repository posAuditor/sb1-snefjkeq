using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public partial class DocCredit_Installments : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtDocCreditInstallments
    {
        get
        {
            return (DataTable)Session["dtDocCreditInstallments" + this.WinID];
        }

        set
        {
            Session["dtDocCreditInstallments" + this.WinID] = value;
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

    private int Receipt_ID
    {
        get
        {
            if (ViewState["Receipt_ID"] == null) return 0;
            return (int)ViewState["Receipt_ID"];
        }

        set
        {
            ViewState["Receipt_ID"] = value;
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

    private int? Branch_ID
    {
        get
        {
            return (int?)ViewState["Branch_ID"];
        }

        set
        {
            ViewState["Branch_ID"] = value;
        }
    }

    private int? BankChartOfAccount_ID
    {
        get
        {
            return (int?)ViewState["BankChartOfAccount_ID"];
        }

        set
        {
            ViewState["BankChartOfAccount_ID"] = value;
        }
    }

    private int? SubBankChartOfAccount_ID
    {
        get
        {
            return (int?)ViewState["SubBankChartOfAccount_ID"];
        }

        set
        {
            ViewState["SubBankChartOfAccount_ID"] = value;
        }
    }

    private int? Currency_ID
    {
        get
        {
            return (int?)ViewState["Currency_ID"];
        }

        set
        {
            ViewState["Currency_ID"] = value;
        }
    }

    private int PayingInstID
    {
        get
        {
            if (ViewState["PayingInstID"] == null) return 0;
            return (int)ViewState["PayingInstID"];
        }

        set
        {
            ViewState["PayingInstID"] = value;
        }
    }

    private int Vendor_ID
    {
        get
        {
            if (ViewState["Vendor_ID"] == null) return 0;
            return (int)ViewState["Vendor_ID"];
        }

        set
        {
            ViewState["Vendor_ID"] = value;
        }
    }

    private bool IsLastInstallment
    {
        get
        {
            if (ViewState["IsLastInstallment"] == null) return false;
            return (bool)ViewState["IsLastInstallment"];
        }

        set
        {
            ViewState["IsLastInstallment"] = value;
        }
    }

    private decimal ReceiptGrossTotalInDefaultCurr
    {
        get
        {
            if (ViewState["ReceiptGrossTotalInDefaultCurr"] == null) return 0;
            return (decimal)ViewState["ReceiptGrossTotalInDefaultCurr"];
        }

        set
        {
            ViewState["ReceiptGrossTotalInDefaultCurr"] = value;
        }
    }

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                this.Fill();
            }
            mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
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
            int? BankWithdraw_ID = null;
            decimal Amount = txtPayFromCover.Text.ToDecimalOrDefault() + txtPayAmount.Text.ToDecimalOrDefault();

            if (txtPayDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            if (Amount != txtInstallmentValue.Text.ToDecimalOrDefault())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.AmountNotEqualToInstallment, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            if (txtPayFromCover.Text.ToDecimalOrDefault() > dc.fun_GetAccountBalanceInForeign(this.SubBankChartOfAccount_ID, null, this.Branch_ID).Value)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.AmountFromCoverGreaterThanCover, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            if (txtPayAmount.Text.ToDecimalOrDefault() > dc.fun_GetAccountBalanceInForeign(this.BankChartOfAccount_ID, null, this.Branch_ID).Value)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.BankBalanceNotEnough, string.Empty);
                trans.Rollback();
                mpePay.Show();
                return;
            }

            int ContactChartOfAccount_ID = dc.fun_getContactAccountID(this.Vendor_ID).Value;

            BankWithdraw_ID = dc.usp_Payments_Insert(txtPayDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, string.Empty, ref Serial, DocSerials.BankWithdraw.ToInt(), txtNotes.Text, Amount, null, DocStatus.Approved.ToByte(), PaymentsTypes.BankWithdrawVendor.ToByte(), this.Branch_ID, txtRatio.Text.ToDecimal(), this.Currency_ID, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt(), this.DocRandomString + "_FromDocCreditInstallment");

            if (!BankWithdraw_ID.HasValue || BankWithdraw_ID.Value <= 0) throw new Exception("Error Occured During inserting the Bankwithdraw document");

            if (txtPayAmount.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(BankWithdraw_ID, ContactChartOfAccount_ID, this.BankChartOfAccount_ID, txtPayAmount.Text.ToDecimalOrDefault(), null, string.Empty, null);
            //الغطا
            if (txtPayFromCover.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(BankWithdraw_ID, ContactChartOfAccount_ID, this.SubBankChartOfAccount_ID, txtPayFromCover.Text.ToDecimalOrDefault(), null, string.Empty, null);

            int Operation_ID = dc.usp_Operation_Insert(this.Branch_ID, txtPayDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.BankWithdraw.ToInt(), this.Currency_ID, Amount * ratio, Amount, ratio, txtNotes.Text);

            if (txtPayAmount.Text.ToDecimalOrDefault() > 0) dc.usp_OperationDetails_Insert(Operation_ID, this.BankChartOfAccount_ID, 0, txtPayAmount.Text.ToDecimalOrDefault() * ratio, 0, txtPayAmount.Text.ToDecimalOrDefault(), null, BankWithdraw_ID, DocumentsTableTypes.Payment_BankWithdraw.ToInt());
            //الغطا
            if (txtPayFromCover.Text.ToDecimalOrDefault() > 0) dc.usp_OperationDetails_Insert(Operation_ID, this.SubBankChartOfAccount_ID, 0, txtPayFromCover.Text.ToDecimalOrDefault() * ratio, 0, txtPayFromCover.Text.ToDecimalOrDefault(), null, BankWithdraw_ID, DocumentsTableTypes.Payment_BankWithdraw.ToInt());

            dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, Amount * ratio, 0, Amount, 0, null, BankWithdraw_ID, DocumentsTableTypes.Payment_BankWithdraw.ToInt());

            dc.usp_DocumentryCreditInstallments_Update(this.PayingInstID, null, null, true, Amount * ratio, ratio, DocumentsTableTypes.Payment_BankWithdraw.ToInt(), BankWithdraw_ID);

            this.Fill();

            if (this.IsLastInstallment)
            {

                decimal CurrencyDiff = this.ReceiptGrossTotalInDefaultCurr - this.dtDocCreditInstallments.Compute("sum(AmountInDefaultCurr)", string.Empty).ToDecimal();

                if (Math.Round(Math.Abs(CurrencyDiff), 4, MidpointRounding.AwayFromZero) > 0)
                {
                    Operation_ID = dc.usp_Operation_Insert(this.Branch_ID, txtPayDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CurrenciesDiff.ToInt(), dc.DefaultCurrancy(), Math.Abs(CurrencyDiff), 0, 1, null);

                    if (CurrencyDiff > 0) //ربح
                    {
                        //debit
                        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, CurrencyDiff, 0, 0, 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());

                        //credit
                        dc.usp_OperationDetails_Insert(Operation_ID, COA.CurrencyDifferencesProfits.ToInt(), 0, CurrencyDiff, 0, 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());
                    }
                    else
                    {
                        //debit
                        dc.usp_OperationDetails_Insert(Operation_ID, COA.CurrencyDifferencesLosses.ToInt(), Math.Abs(CurrencyDiff), 0, 0, 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());

                        //credit
                        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, Math.Abs(CurrencyDiff), 0, 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());
                    }
                }

            }
            LogAction(Actions.Approve, "سداد بنكى لقسط اعتماد مستندى " + " : " + txtPayAmount.Text + " : " + txtPayDate.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInstallments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvInstallments.PageIndex = e.NewPageIndex;
            gvInstallments.DataSource = this.dtDocCreditInstallments;
            gvInstallments.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInstallments_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtDocCreditInstallments.Select("ID=" + gvInstallments.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtAmount.Text = dr["Amount"].ToExpressString();
            txtRatio.Text = dr["Ratio"].ToExpressString();
            txtDate.Text = dr["InstDate"].ToDate().Value.ToString("d/M/yyyy");
            this.EditID = gvInstallments.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInstallments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_DocumentryCreditInstallments_Delete(gvInstallments.DataKeys[e.RowIndex]["ID"].ToInt());
            LogAction(Actions.Delete, string.Empty, dc);
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

    protected void btnSave_click(object sender, EventArgs e)
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

    protected void btnApprove_click(object sender, EventArgs e)
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

    protected void lnkPayWithBank_Click(object sender, EventArgs e)
    {
        try
        {
            this.DocRandomString = null;
            if (lblDifference.Text.ToDecimalOrDefault() != 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.TotalInstallmentsNotEqualToGrossTotal, string.Empty);
                return;
            }

            txtPayFromCover.Enabled = true;
            txtPayAmount.Clear();
            txtPayFromCover.Clear();
            this.IsLastInstallment = false;
            txtNotes.Clear();

            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.PayingInstID = gvInstallments.DataKeys[Index]["ID"].ToInt();
            txtInstallmentValue.Text = gvInstallments.DataKeys[Index]["Amount"].ToExpressString();
            txtPayDate.Clear();
            if (gvInstallments.DataKeys[Index]["InstDate"].ToDate().Value <= MyContext.FiscalYearEndDate && gvInstallments.DataKeys[Index]["InstDate"].ToDate().Value >= MyContext.FiscalYearStartDate)
            {
                txtPayDate.Text = gvInstallments.DataKeys[Index]["InstDate"].ToDate().Value.ToString("d/M/yyyy");
            }
            this.SetRatio(gvInstallments.DataKeys[Index]["InstDate"].ToDate().Value);

            if (((LinkButton)sender).ID == "lnkPayWithLoan")
            {
                Response.Redirect(PageLinks.Loan + string.Format("?RelatedDoc_ID={0}&RelatedDocTableType_ID={1}&DocCreditInstallment={2}&Branch_ID={3}", this.PayingInstID, DocumentsTableTypes.DocumentryCreditInstallments.ToInt(), txtInstallmentValue.Text, this.Branch_ID), false);
                return;
            }


            if (this.dtDocCreditInstallments.Select("isPaid=0").Length <= 1)
            {
                this.IsLastInstallment = true;
                txtPayFromCover.Enabled = false;
                txtPayFromCover.Text = dc.fun_GetAccountBalanceInForeign(this.SubBankChartOfAccount_ID, null, this.Branch_ID).Value.ToExpressString();
            }
            mpePay.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtPayDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(this.Currency_ID, txtPayDate.Text.ToDate());
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

    private void SetRatio(DateTime date)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(this.Currency_ID, date);
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearForm()
    {
        txtDate.Clear();
        txtAmount.Clear();
    }

    private void LoadControls()
    {
        this.Receipt_ID = Request["Receipt_ID"].ToInt();
        var Receipt = dc.usp_Receipt_SelectByID(this.Receipt_ID).FirstOrDefault();
        this.Branch_ID = Receipt.Branch_ID;
        this.Currency_ID = Receipt.Currency_ID;
        this.Vendor_ID = Receipt.Contact_ID.Value;
        this.ReceiptGrossTotalInDefaultCurr = Receipt.GrossTotalAmount.Value * Receipt.Ratio.Value;
        lblDocCreditGrossTotal.Text = Receipt.GrossTotalAmount.ToExpressString();
        var DocCredit = dc.usp_DocumentryCredit_SelectByReceipt_ID(this.Receipt_ID).FirstOrDefault();
        this.BankChartOfAccount_ID = DocCredit.BankChartOfAccount_ID;
        this.SubBankChartOfAccount_ID = DocCredit.SubBankChartOfAccount_ID;
        lnkviewdocCredit.NavigateUrl = PageLinks.DocumentryCredit + "?ID=" + this.Receipt_ID.ToExpressString() + "&ViewInPopUpMode=1";
    }

    private void Fill()
    {
        this.dtDocCreditInstallments = dc.usp_DocumentryCreditInstallments_Select(null, this.Receipt_ID).CopyToDataTable();
        gvInstallments.DataSource = this.dtDocCreditInstallments;
        gvInstallments.DataBind();

        lblInstallmentsTotal.Text = this.dtDocCreditInstallments.Compute("sum(amount)", string.Empty).ToExpressString();
        lblDifference.Text = (lblDocCreditGrossTotal.Text.ToDecimalOrDefault() - lblInstallmentsTotal.Text.ToDecimalOrDefault()).ToString("0.####");

        if (this.dtDocCreditInstallments.Select("isPaid=1").Length > 0)
        {
            lnkAddNew.Visible = false;
            mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
        }
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvInstallments.Columns[7].Visible = MyContext.PageData.IsApprove;
        gvInstallments.Columns[8].Visible = MyContext.PageData.IsApprove;
        gvInstallments.Columns[9].Visible = MyContext.PageData.IsEdit;
        gvInstallments.Columns[10].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        int result = 0;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        if (this.EditID == 0) //insert
        {
            result = dc.usp_DocumentryCreditInstallments_Insert(this.Receipt_ID, txtDate.Text.ToDate(), txtAmount.Text.ToDecimal(), null, null);
            LogAction(IsApproving ? Actions.Approve : Actions.Add, txtDate.Text + " : " + txtAmount.Text, dc);
        }
        else
        {
            result = dc.usp_DocumentryCreditInstallments_Update(this.EditID, txtDate.Text.ToDate(), txtAmount.Text.ToDecimal(), false, null, null, null, null);
            LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtDate.Text + " : " + txtAmount.Text, dc);
        }

        this.Fill();
        this.ClosePopup_Click(null, null);
        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        return true;
    }

    #endregion
}