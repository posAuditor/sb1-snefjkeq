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

public partial class Payments_Payments : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    List<PaymentsOperationDetail> OperationDetailsList = new List<PaymentsOperationDetail>();

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

    private int Payment_ID
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

    private DataTable dtPaymentDetails
    {
        get
        {
            if (Session["dtPaymentDetails" + this.WinID] == null)
            {
                Session["dtPaymentDetails" + this.WinID] = dc.usp_PaymentsDetails_Select(null, 0).CopyToDataTable();
            }
            return (DataTable)Session["dtPaymentDetails" + this.WinID];
        }

        set
        {
            Session["dtPaymentDetails" + this.WinID] = value;
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
                this.SetDefaults();
                if (EditMode)
                {
                    this.FillPayment();
                }
                else
                {
                    this.FillFromBill();
                }
            }


            switch (Request.PathInfo)
            {
                case "/CashIn":
                    ucNav.SourceDocTypeType_ID = 10;
                    break;
                case "/CashOut":
                    ucNav.SourceDocTypeType_ID = 11;
                    break;
                case "/BankDeposit":
                    ucNav.SourceDocTypeType_ID = 12;
                    break;
                case "/BankWithdraw":
                    ucNav.SourceDocTypeType_ID = 13;
                    break;
            }
            ucNav.Res_ID = this.Payment_ID;
            ucNav.EntryType = this.EntryType;
            //ucNav.btnHandler += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandler);
            ucNav.IsPermShow = (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID);


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
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtOperationDate.Text.ToDate());
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


            var AcDCount = dc.fun_AccountHaveChild(acDebitAccount.Value.ToInt());
            var AcCCount = dc.fun_AccountHaveChild(acCreditAccount.Value.ToInt());
            if (AcDCount > 0)
            {
                UserMessages.Message(null, "حساب المدين لا يمكن ان يكون حساب اب", string.Empty);

                return;
            }
            if (AcCCount > 0)
            {
                UserMessages.Message(null, "حساب الدائن لا يمكن ان يكون حساب اب", string.Empty);

                return;
            }

            if (acDebitAccount.Value == acCreditAccount.Value)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DebitSameCredit, string.Empty);
                return;
            }
            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtPaymentDetails.NewRow();
                r["ID"] = this.dtPaymentDetails.GetID("ID");

            }
            else
            {
                r = this.dtPaymentDetails.Select("ID=" + this.EditID)[0];
            }

            r["DebitAccount_ID"] = acDebitAccount.Value.ToInt();
            r["CreditAccount_ID"] = acCreditAccount.Value.ToInt();
            r["Notes"] = txtDetailNotes.Text;
            r["CostCenter_ID"] = acDetailCostCenter.Value.ToIntOrDBNULL();
            r["CreditAccountName"] = acCreditAccount.Text;
            r["DebitAccountName"] = acDebitAccount.Text;
            r["CostCenterName"] = acDetailCostCenter.Text;
            r["SalesRepName"] = acSalesRep.Text;
            r["SalesRep_ID"] = acSalesRep.Value.ToIntOrDBNULL();
            r["Amount"] = txtAmount.Text.ToDecimal();
            r["Discount"] = txtDiscount.Text.ToDecimalOrDefault();
            r["tax"] = txtTaxFound.Text.ToDecimalOrDefault();
            r["VendorsSecond_ID"] = acVendorsSecond.Value.ToIntOrDBNULL();
            r["VendorsSecondname"] = acVendorsSecond.Text;


            if (this.EditID == 0) this.dtPaymentDetails.Rows.Add(r);

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
            DataRow r = this.dtPaymentDetails.Select("ID=" + this.EditID.ToExpressString())[0];

            acCreditAccount.Value = r["CreditAccount_ID"].ToExpressString();
            acDebitAccount.Value = r["DebitAccount_ID"].ToExpressString();
            this.ShowBalances();
            this.FilterSalesReps();
            acSalesRep.Value = r["SalesRep_ID"].ToExpressString();
            acDetailCostCenter.Value = r["CostCenter_ID"].ToExpressString();
            txtAmount.Text = r["Amount"].ToDecimal().ToExpressString();
            txtDetailNotes.Text = r["Notes"].ToExpressString();
            txtDiscount.Text = r["Discount"].ToExpressString();
            txtTaxFound.Text = r["tax"].ToExpressString();
            acVendorsSecond.Value = r["VendorsSecond_ID"].ToExpressString();
            chkIsTaxFound.Checked = false;

            if (Convert.ToDouble(txtTaxFound.Text) > 0)
            {
                chkIsTaxFound.Checked = true;
            }



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
            DataRow dr = this.dtPaymentDetails.Select("ID=" + ID.ToExpressString())[0];
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
            dc.usp_Payments_Cancel(this.Payment_ID, MyContext.UserProfile.Contact_ID, DateTime.Now);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Payments + Request.PathInfo + "?ID=" + this.Payment_ID.ToExpressString(), PageLinks.PaymentsList + Request.PathInfo, PageLinks.Payments + Request.PathInfo);
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
            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\Payment_Print.rpt"));
            //doc.SetParameterValue("@Payments_ID", this.Payment_ID);
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Payment"), false);
            string UserPrintName = MyContext.UserProfile.EmployeeName;
            int PaymentsID = this.Payment_ID;
          

            switch (Request.PathInfo)
            {
                case "/CashIn":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                        "&UserPrintName=" + UserPrintName + "&IsMaterla=1", false);
 
                    break;

                case "/CashInCustomer":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                          "&UserPrintName=" + UserPrintName + "&IsMaterla=2", false);

                    break;

                case "/CashOut":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                         "&UserPrintName=" + UserPrintName + "&IsMaterla=3", false);
                    break;

                case "/CashOutVendor":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                         "&UserPrintName=" + UserPrintName + "&IsMaterla=4", false);
                    break;

                case "/BankDeposit":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                         "&UserPrintName=" + UserPrintName + "&IsMaterla=5", false);
                    break;
                case "/BankDepositCustomer":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                          "&UserPrintName=" + UserPrintName + "&IsMaterla=6", false);
                    break;
                case "/BankWithdraw":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                         "&UserPrintName=" + UserPrintName + "&IsMaterla=7", false);

                    break;

                case "/BankWithdrawVendor":
                    Response.Redirect("~/Report_Dev/PrintPaymentsDev.aspx?Payments_ID=" + this.Payment_ID +
                        "&UserPrintName=" + UserPrintName + "&IsMaterla=8", false);
                    break;
            }
        
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
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        try
        {
            dc.usp_CancelApprove_Payment(this.DocumentTableType_ID, this.Payment_ID);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Payments + Request.PathInfo + "?ID=" + this.Payment_ID.ToExpressString(), PageLinks.PaymentsList + Request.PathInfo, PageLinks.Payments + Request.PathInfo);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
        }
        catch (Exception)
        {


        }
    }
    protected void chkIsTaxFound_CheckedChanged(object sender, EventArgs e)
    {


        this.ChangeTax();

    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        this.ChangeTax();

    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        this.ChangeTax();
    }
    protected void CloseFastAddNewPopup_Click(object sender, EventArgs e)
    {
        try
        {
            txtFastAddName.Clear();
            txtVatNumber.Clear();
            txtNoteVender.Clear();
            mpeFastAddNew.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnFastAddNew_click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            int result = 0;


            result = dc.usp_VendorsSecond_Insert(txtFastAddName.TrimmedText, txtVatNumber.TrimmedText, txtNoteVender.TrimmedText, DateTime.Now, MyContext.UserProfile.Contact_ID, true);
            LogAction(Actions.Add, "مورد المصروفات: " + txtFastAddName.TrimmedText, dc);

            acVendorsSecond.Clear();
            acVendorsSecond.ContextKey = string.Empty;
            acVendorsSecond.Value = result.ToExpressString();
            this.FocusNextControl(lnkAddNewCustomer);


        }
        catch (Exception ex)
        {
            //trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void lnkAddNewCustomer_Click(object sender, EventArgs e)
    {
        mpeFastAddNew.Show();
    }


    #endregion

    #region Private Methods
    private void FilterByBranchAndCurrency()
    {
        try
        {
            string DebitParent_ID = string.Empty;
            string CreditParent_ID = string.Empty;
            bool IncludeParent_debit = true;
            bool IncludeParent_credit = true;
            switch (Request.PathInfo)
            {
                case "/CashIn":
                    DebitParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "/CashInCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = (MyContext.UserProfile.CashierAccount_ID != null) ? MyContext.UserProfile.CashierAccount_ID.ToExpressString() : COA.CashOnHand.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "/CashOut":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "/CashOutVendor":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;

                case "/BankDeposit":
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;
                case "/BankDepositCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    IncludeParent_credit = false;
                    break;
                case "/BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "/BankWithdrawVendor":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    IncludeParent_debit = false;
                    break;
            }

            acCreditAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + CreditParent_ID + ",false," + IncludeParent_credit.ToExpressString();
            // acCreditAccount.ContextKey = "" + acBranch.Value + "";

            acDebitAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + DebitParent_ID + ",false," + IncludeParent_debit.ToExpressString();
            acDetailCostCenter.ContextKey = acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void LoadControls()
    {
        this.dtPaymentDetails = null;
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

        acVendorsSecond.ContextKey = string.Empty;
    }

    private void BindDetailsGrid()
    {
        this.Calculate();
        gvDetails.DataSource = this.dtPaymentDetails;
        gvDetails.DataBind();
        // acBranch.Enabled = (gvDetails.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
        ddlCurrency.Enabled = (gvDetails.Rows.Count == 0);
    }

    private void ClearDetailForm()
    {
        switch (Request.PathInfo)
        {
            case "/CashIn":
                acCreditAccount.Clear();
                txtAmount.Focus();
                break;

            case "/CashInCustomer":
                acCreditAccount.Clear();
                txtAmount.Focus();
                break;

            case "/CashOut":
                acDebitAccount.Clear();
                acDebitAccount.AutoCompleteFocus();
                break;

            case "/CashOutVendor":
                acDebitAccount.Clear();
                acDebitAccount.AutoCompleteFocus();
                break;

            case "/BankDeposit":
                acCreditAccount.Clear();
                txtAmount.Focus();
                break;
            case "/BankDepositCustomer":
                acCreditAccount.Clear();
                txtAmount.Focus();
                break;
            case "/BankWithdraw":
                acDebitAccount.Clear();
                acDebitAccount.AutoCompleteFocus();
                break;

            case "/BankWithdrawVendor":
                acDebitAccount.Clear();
                acDebitAccount.AutoCompleteFocus();
                break;
        }
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
            this.Payment_ID = Request["ID"].ToInt();
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

        if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
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

            this.Payment_ID = dc.usp_PaymentsVendorsSecond_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, approvedBY_ID, ApproveDate, txtUserRefNo.TrimmedText, ref Serial, this.Serial_ID, txtNotes.Text, this.Total, acCostCenter.Value.ToNullableInt(), DocStatus_ID, this.EntryType, acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.RelatedDoc_ID, this.RelatedDocTableType_ID, this.DocRandomString, acVendorsSecond.Value.ToNullableInt());
            if (this.Payment_ID > 0)
            {
                foreach (DataRow r in this.dtPaymentDetails.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_PaymentsDetailsVendorsSecond_Insert(this.Payment_ID, r["DebitAccount_ID"].ToInt(),
                        r["CreditAccount_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(),
                        r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt(), r["Discount"].ToDecimal(),
                        r["Tax"].ToDecimal(), r["PaymentType"].ToNullableInt(), r["VendorsSecond_ID"].ToNullableInt()
                        ,null,null);
                    if (IsApproving)
                    {
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = r["DebitAccountName"].ToExpressString(),
                            Account_ID = r["DebitAccount_ID"].ToInt(),
                            DebitAmount = r["Amount"].ToDecimal(),
                            CreditAmount = 0,
                            CostCenter_ID = r["CostCenter_ID"].ToNullableInt(),
                            Notes = r["Notes"].ToExpressString(),
                            Discount = r["Discount"].ToDecimalOrDefault(),
                            tax = r["Tax"].ToDecimalOrDefault(),
                            VendorsSecond = r["VendorsSecond_ID"].ToNullableInt()

                        });
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = r["CreditAccountName"].ToExpressString(),
                            Account_ID = r["CreditAccount_ID"].ToInt(),
                            DebitAmount = 0,
                            CreditAmount = r["Amount"].ToDecimal(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = r["Notes"].ToExpressString(),
                            Discount = r["Discount"].ToDecimalOrDefault(),
                            tax = r["Tax"].ToDecimalOrDefault(),
                            VendorsSecond = r["VendorsSecond_ID"].ToNullableInt()

                        });


                    }
                }


                var ListOpAccount = OperationDetailsList.Where(c => dc.fun_AccountHaveChild(c.Account_ID) > 0).ToList();
                if (ListOpAccount.Count > 0)
                {
                    UserMessages.Message(null, "هناك حسابات آباء لا يمكن اختيارها", string.Empty);
                    trans.Rollback();
                    return false;
                }


                if (IsApproving) InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_PaymentsVendorsSecon_Update(this.Payment_ID, txtOperationDate.Text.ToDate(), approvedBY_ID, ApproveDate, txtUserRefNo.TrimmedText, ref Serial, this.Serial_ID, txtNotes.Text, this.Total, acCostCenter.Value.ToNullableInt(), DocStatus_ID, acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), acVendorsSecond.Value.ToNullableInt());
            if (Result > 0)
            {
                foreach (DataRow r in this.dtPaymentDetails.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        dc.usp_PaymentsDetailsVendorsSecond_Insert(this.Payment_ID, r["DebitAccount_ID"].ToInt(), r["CreditAccount_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt(), r["Discount"].ToDecimal(), r["Tax"].ToDecimal(), r["PaymentType"].ToNullableInt(), r["VendorsSecond_ID"].ToNullableInt(), null, null);

                        //dc.usp_PaymentsDetails_Insert(this.Payment_ID, r["DebitAccount_ID"].ToInt(), r["CreditAccount_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt());
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        dc.usp_PaymentsDetailsVendorsSecond_Update(r["ID"].ToInt(), r["DebitAccount_ID"].ToInt(), r["CreditAccount_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt(), r["Discount"].ToDecimal(), r["Tax"].ToDecimal(), r["PaymentType"].ToNullableInt(), r["VendorsSecond_ID"].ToNullableInt(), null, null);
                        //dc.usp_PaymentsDetails_Update(r["ID"].ToInt(), r["DebitAccount_ID"].ToInt(), r["CreditAccount_ID"].ToInt(), r["Amount"].ToDecimal(), r["SalesRep_ID"].ToNullableInt(), r["Notes"].ToExpressString(), r["CostCenter_ID"].ToNullableInt());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_PaymentsDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                    if (r.RowState != DataRowState.Deleted && IsApproving)
                    {
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = r["DebitAccountName"].ToExpressString(),
                            Account_ID = r["DebitAccount_ID"].ToInt(),
                            DebitAmount = r["Amount"].ToDecimal(),
                            CreditAmount = 0,
                            CostCenter_ID = r["CostCenter_ID"].ToNullableInt(),
                            Notes = r["Notes"].ToExpressString(),
                            Discount = r["Discount"].ToDecimalOrDefault(),
                            tax = r["Tax"].ToDecimalOrDefault(),
                            VendorsSecond = r["VendorsSecond_ID"].ToNullableInt()


                        });
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = r["CreditAccountName"].ToExpressString(),
                            Account_ID = r["CreditAccount_ID"].ToInt(),
                            DebitAmount = 0,
                            CreditAmount = r["Amount"].ToDecimal(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = r["Notes"].ToExpressString(),
                            Discount = r["Discount"].ToDecimalOrDefault(),
                            tax = r["Tax"].ToDecimalOrDefault(),
                            VendorsSecond = r["VendorsSecond_ID"].ToNullableInt()

                        });
                    }
                }
                var ListOpAccount = OperationDetailsList.Where(c => dc.fun_AccountHaveChild(c.Account_ID) > 0).ToList();
                if (ListOpAccount.Count > 0)
                {
                    UserMessages.Message(null, "هناك حسابات آباء لا يمكن اختيارها", string.Empty);
                    trans.Rollback();
                    return false;
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
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Payments + Request.PathInfo + "?ID=" + this.Payment_ID.ToExpressString(), PageLinks.PaymentsList + Request.PathInfo, PageLinks.Payments + Request.PathInfo);
        return true;
    }

    private void FillPayment()
    {
        var Payment = dc.usp_PaymentsVendorsSecond_SelectByID(this.Payment_ID).FirstOrDefault();
        ddlCurrency.SelectedValue = Payment.Currency_ID.ToExpressString();
        txtRatio.Text = Payment.Ratio.ToExpressString();
        acBranch.Value = Payment.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        acCostCenter.Value = Payment.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = Payment.UserRefNumber;
        txtNotes.Text = Payment.Notes;
        txtSerial.Text = Payment.Serial;
        ucNav.SetText = Payment.Serial;
        this.DocRandomString = Payment.DocRandomString;
        lblCreatedBy.Text = Payment.CeartedByName;
        lblApprovedBy.Text = Payment.ApprovedByName;
        acVendorsSecond.Value = Payment.VendorsSecond_ID.ToStringOrEmpty();


        txtOperationDate.Text = Payment.OperationDate.Value.ToString("d/M/yyyy");
        this.ImgStatus = ((DocStatus)Payment.DocStatus_ID).ToExpressString();
        this.RelatedDoc_ID = Payment.RelatedDoc_ID;
        this.RelatedDocTableType_ID = Payment.RelatedDocTableType_ID;
        btnPrint.Visible = MyContext.PageData.IsPrint;
        pnlAddDetail.Visible = (Payment.DocStatus_ID == 1);
        btnCancel.Visible = (Payment.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Payment.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = !btnApprove.Visible && (Payment.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
        //btnCancelApprove.Visible =  (Payment.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove ;
        btnSave.Visible = (Payment.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        gvDetails.Columns[gvDetails.Columns.Count - 1].Visible = gvDetails.Columns[gvDetails.Columns.Count - 2].Visible = (Payment.DocStatus_ID == 1);

        this.dtPaymentDetails = dc.usp_PaymentsDetails_Select(this.Payment_ID, MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        this.BindDetailsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        //btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        //btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
    }

    private void Calculate()
    {
        this.Total = 0;
        foreach (DataRow r in this.dtPaymentDetails.Rows)
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
        lblDebitBalance.Text = dc.fun_GetAccountBalanceInForeign(acDebitAccount.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value.ToExpressString();
        lblCreditBalance.Text = dc.fun_GetAccountBalanceInForeign(acCreditAccount.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value.ToExpressString();
    }

    private void FilterSalesReps()
    {
        int? Contact_ID = dc.fun_GetCustomerContactID(acCreditAccount.Value.ToNullableInt());
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
        chkIsTaxFound.Visible = false;
        txtTaxFound.Visible = false;

        lnkAddNewCustomer.Visible = false;
        acVendorsSecond.Visible = false;

        switch (Request.PathInfo)
        {
            case "/CashIn":
                this.Serial_ID = DocSerials.CashIn.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_CashIn.ToInt();
                this.EntryType = PaymentsTypes.CashIn.ToByte();
                this.OperationType_ID = OperationTypes.CashIn.ToInt();
                this.lblTitle.Text = Resources.Labels.CashIn;
                break;

            case "/CashInCustomer":
                this.Serial_ID = DocSerials.CashIn.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_CashIn.ToInt();
                this.EntryType = PaymentsTypes.CashInCustomer.ToByte();
                this.OperationType_ID = OperationTypes.CashIn.ToInt();

                break;

            case "/CashOut":
                this.Serial_ID = DocSerials.CashOut.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payament_CashOut.ToInt();
                this.EntryType = PaymentsTypes.CashOut.ToByte();
                this.OperationType_ID = OperationTypes.CashOut.ToInt();
                this.lblTitle.Text = Resources.Labels.CashOut;
                chkIsTaxFound.Visible = true;
                txtTaxFound.Visible = true;
                lnkAddNewCustomer.Visible = true;
                acVendorsSecond.Visible = true;
                break;

            case "/CashOutVendor":
                this.Serial_ID = DocSerials.CashOut.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payament_CashOut.ToInt();
                this.EntryType = PaymentsTypes.CashOutVendor.ToByte();
                this.OperationType_ID = OperationTypes.CashOut.ToInt();
                this.lblTitle.Text = Resources.Labels.CashVendorPayment;
                break;

            case "/BankDeposit":
                this.Serial_ID = DocSerials.BankDeposit.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_BankDeposit.ToInt();
                this.EntryType = PaymentsTypes.BankDeposit.ToByte();
                this.OperationType_ID = OperationTypes.BankDeposit.ToInt();
                break;
            case "/BankDepositCustomer":
                this.Serial_ID = DocSerials.BankDeposit.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_BankDeposit.ToInt();
                this.EntryType = PaymentsTypes.BankDepositCustomer.ToByte();
                this.OperationType_ID = OperationTypes.BankDeposit.ToInt();
                break;
            case "/BankWithdraw":
                this.Serial_ID = DocSerials.BankWithdraw.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_BankWithdraw.ToInt();
                this.EntryType = PaymentsTypes.BankWithDraw.ToByte();
                this.OperationType_ID = OperationTypes.BankWithdraw.ToInt();

                chkIsTaxFound.Visible = true;
                txtTaxFound.Visible = true;
                lnkAddNewCustomer.Visible = true;
                acVendorsSecond.Visible = true;

                break;

            case "/BankWithdrawVendor":
                this.Serial_ID = DocSerials.BankWithdraw.ToInt();
                this.DocumentTableType_ID = DocumentsTableTypes.Payment_BankWithdraw.ToInt();
                this.EntryType = PaymentsTypes.BankWithdrawVendor.ToByte();
                this.OperationType_ID = OperationTypes.BankWithdraw.ToInt();
                chkIsTaxFound.Visible = true;
                txtTaxFound.Visible = true;
                lnkAddNewCustomer.Visible = true;
                acVendorsSecond.Visible = true;
                break;
        }
        //gvDetails.Columns[3].Visible = acSalesRep.Visible = (Request.PathInfo == "/CashInCustomer" || Request.PathInfo == "/BankDepositCustomer");
    }

    //private void InsertOperation()
    //{
    //    if (OperationDetailsList.Count == 0) return;
    //    decimal ratio = txtRatio.Text.ToDecimal();
    //    string serial = string.Empty;
    //    var company = dc.usp_Company_Select().FirstOrDefault();
    //    int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), this.Total * ratio, this.Total, ratio, txtNotes.Text);

    //    var GroupedOperationDetails = this.OperationDetailsList.ToList();
    //    // group OperationDetails by new { OperationDetails.Account_ID, OperationDetails.AccountName, IsDebit = (OperationDetails.CreditAmount == 0), OperationDetails.Notes } into groupedDetails
    //    // select new { Key = groupedDetails.Key, DebitAmount = groupedDetails.Sum(x => x.DebitAmount), CreditAmount = groupedDetails.Sum(x => x.CreditAmount) };

    //    foreach (var Detail in GroupedOperationDetails)
    //    {
    //        if ((this.OperationType_ID == OperationTypes.CashOut.ToInt() || this.OperationType_ID == OperationTypes.BankWithdraw.ToInt()) && !this.ConfirmationAnswered)
    //        {
    //            if (Detail.CreditAmount > 0 && Detail.CreditAmount > dc.fun_GetAccountBalanceInForeign(Detail.Account_ID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value)
    //            {
    //                this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BalanceNotEnough + " (" + Detail.AccountName + ")";
    //            }
    //        }
    //        dc.usp_OperationDetails_Insert(Result, Detail.Account_ID, Detail.DebitAmount * ratio, Detail.CreditAmount * ratio, Detail.DebitAmount, Detail.CreditAmount, Detail.Notes, this.Payment_ID, this.DocumentTableType_ID);
    //    }
    //    int i = 0;
    //    //CostCenter



    //    foreach (var Detail in OperationDetailsList)
    //    {
    //        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), Detail.CostCenter_ID, txtOperationDate.Text.ToDate(), (Detail.DebitAmount + Detail.CreditAmount) * ratio, this.Payment_ID, this.DocumentTableType_ID, Detail.Notes);
    //        i++;
    //        if (Detail.Discount > 0 && i % 2 == 0)
    //        {
    //            if (this.EntryType == PaymentsTypes.CashIn.ToByte() || this.EntryType == PaymentsTypes.CashInCustomer.ToByte() || this.EntryType == PaymentsTypes.BankDepositCustomer.ToByte())
    //            //حسم الممنوح
    //            {
    //                string serial1 = string.Empty;
    //                int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم ممنوح");

    //                dc.usp_OperationDetails_Insert(Result1, company.SalesDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
    //                dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
    //            }
    //            if (this.EntryType == PaymentsTypes.CashOut.ToByte() || this.EntryType == PaymentsTypes.CashOutVendor.ToByte() || this.EntryType == PaymentsTypes.BankWithdrawVendor.ToByte())
    //            //الحسم المكتسب
    //            {
    //                string serial1 = string.Empty;
    //                int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم مكتسب");


    //                dc.usp_OperationDetails_Insert(Result1, company.PurchaseDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);
    //                dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);

    //            }
    //        }

    //    }
    //    dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.Total * ratio, this.Payment_ID, this.DocumentTableType_ID, txtNotes.Text);
    //}

    private void InsertOperation()
    {
        if (OperationDetailsList.Count == 0) return;
        decimal ratio = txtRatio.Text.ToDecimal();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), this.Total * ratio, this.Total, ratio, txtNotes.Text);

        var GroupedOperationDetails = this.OperationDetailsList.ToList();
        // group OperationDetails by new { OperationDetails.Account_ID, OperationDetails.AccountName, IsDebit = (OperationDetails.CreditAmount == 0), OperationDetails.Notes } into groupedDetails
        // select new { Key = groupedDetails.Key, DebitAmount = groupedDetails.Sum(x => x.DebitAmount), CreditAmount = groupedDetails.Sum(x => x.CreditAmount) };

        //foreach (var Detail in GroupedOperationDetails)
        //{
        //    if ((this.OperationType_ID == OperationTypes.CashOut.ToInt() || this.OperationType_ID == OperationTypes.BankWithdraw.ToInt()) && !this.ConfirmationAnswered)
        //    {
        //        if (Detail.CreditAmount > 0 && Detail.CreditAmount > dc.fun_GetAccountBalanceInForeign(Detail.Account_ID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value)
        //        {
        //            this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BalanceNotEnough + " (" + Detail.AccountName + ")";
        //        }
        //    }
        //    dc.usp_OperationDetails_Insert(Result, Detail.Account_ID, Detail.DebitAmount * ratio, Detail.CreditAmount * ratio, Detail.DebitAmount, Detail.CreditAmount, Detail.Notes, this.Payment_ID, this.DocumentTableType_ID);
        //}
        int i = 0;
        //CostCenter



        foreach (var Detail in OperationDetailsList)
        {
            dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), Detail.CostCenter_ID, txtOperationDate.Text.ToDate(), (Detail.DebitAmount + Detail.CreditAmount) * ratio, this.Payment_ID, this.DocumentTableType_ID, Detail.Notes);
            //    i++;
            //    if (Detail.Discount > 0 && i % 2 == 0)
            //    {
            //        if (this.EntryType == PaymentsTypes.CashIn.ToByte() || this.EntryType == PaymentsTypes.CashInCustomer.ToByte() || this.EntryType == PaymentsTypes.BankDepositCustomer.ToByte())
            //        //حسم الممنوح
            //        {
            //            string serial1 = string.Empty;
            //            int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم ممنوح");

            //            dc.usp_OperationDetails_Insert(Result1, company.SalesDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
            //            dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
            //        }
            //        if (this.EntryType == PaymentsTypes.CashOut.ToByte() || this.EntryType == PaymentsTypes.CashOutVendor.ToByte() || this.EntryType == PaymentsTypes.BankWithdrawVendor.ToByte())
            //        //الحسم المكتسب
            //        {
            //            string serial1 = string.Empty;
            //            int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم مكتسب");


            //            dc.usp_OperationDetails_Insert(Result1, company.PurchaseDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);
            //            dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);

            //        }
            //    }

        }

        if (this.EntryType == PaymentsTypes.CashIn.ToByte() || this.EntryType == PaymentsTypes.CashInCustomer.ToByte() || this.EntryType == PaymentsTypes.BankDepositCustomer.ToByte() || this.EntryType == PaymentsTypes.BankDeposit.ToByte())
        //حسم الممنوح
        {
            dc.usp_OperationDetailsCashIN_Insert(Result, this.Payment_ID, this.DocumentTableType_ID);

        }
        if (this.EntryType == PaymentsTypes.CashOut.ToByte() || this.EntryType == PaymentsTypes.CashOutVendor.ToByte() || this.EntryType == PaymentsTypes.BankWithdrawVendor.ToByte() || this.EntryType == PaymentsTypes.BankWithDraw.ToByte())
        //الحسم المكتسب
        {
            dc.usp_OperationDetailsCashOut_Insert(Result, this.Payment_ID, this.DocumentTableType_ID);


        }

        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.Total * ratio, this.Payment_ID, this.DocumentTableType_ID, txtNotes.Text);
    }

    //private void InsertOperation()
    //{
    //    if (OperationDetailsList.Count == 0) return;
    //    decimal ratio = txtRatio.Text.ToDecimal();
    //    string serial = string.Empty;
    //    var company = dc.usp_Company_Select().FirstOrDefault();
    //    int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), this.Total * ratio, this.Total, ratio, txtNotes.Text);

    //    var GroupedOperationDetails = from OperationDetails in this.OperationDetailsList
    //                                  group OperationDetails by new { OperationDetails.Account_ID, OperationDetails.AccountName, IsDebit = (OperationDetails.CreditAmount == 0), OperationDetails.Notes } into groupedDetails
    //                                  select new { Key = groupedDetails.Key, DebitAmount = groupedDetails.Sum(x => x.DebitAmount), CreditAmount = groupedDetails.Sum(x => x.CreditAmount) };

    //    foreach (var Detail in GroupedOperationDetails)
    //    {
    //        if ((this.OperationType_ID == OperationTypes.CashOut.ToInt() || this.OperationType_ID == OperationTypes.BankWithdraw.ToInt()) && !this.ConfirmationAnswered)
    //        {
    //            if (Detail.CreditAmount > 0 && Detail.CreditAmount > dc.fun_GetAccountBalanceInForeign(Detail.Key.Account_ID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value)
    //            {
    //                this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BalanceNotEnough + " (" + Detail.Key.AccountName + ")";
    //            }
    //        }
    //        dc.usp_OperationDetails_Insert(Result, Detail.Key.Account_ID, Detail.DebitAmount * ratio, Detail.CreditAmount * ratio, Detail.DebitAmount, Detail.CreditAmount, Detail.Key.Notes, this.Payment_ID, this.DocumentTableType_ID);
    //    }
    //    int i = 0;
    //    //CostCenter



    //    foreach (var Detail in OperationDetailsList)
    //    {
    //        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), Detail.CostCenter_ID, txtOperationDate.Text.ToDate(), (Detail.DebitAmount + Detail.CreditAmount) * ratio, this.Payment_ID, this.DocumentTableType_ID, Detail.Notes);
    //        i++;
    //        if (Detail.Discount > 0 && i % 2 == 0)
    //        {
    //            if (this.EntryType == PaymentsTypes.CashIn.ToByte() || this.EntryType == PaymentsTypes.CashInCustomer.ToByte() || this.EntryType == PaymentsTypes.BankDepositCustomer.ToByte())
    //            //حسم الممنوح
    //            {
    //                string serial1 = string.Empty;
    //                int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم ممنوح");

    //                dc.usp_OperationDetails_Insert(Result1, company.SalesDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
    //                dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم ممنوح", this.Payment_ID, this.DocumentTableType_ID);
    //            }
    //            if (this.EntryType == PaymentsTypes.CashOut.ToByte() || this.EntryType == PaymentsTypes.CashOutVendor.ToByte() || this.EntryType == PaymentsTypes.BankWithdrawVendor.ToByte())
    //            //الحسم المكتسب
    //            {
    //                string serial1 = string.Empty;
    //                int Result1 = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial1, DocStatus.Approved.ToByte(), this.OperationType_ID, ddlCurrency.SelectedValue.ToInt(), Detail.Discount * ratio, Detail.Discount, ratio, "حسم مكتسب");


    //                dc.usp_OperationDetails_Insert(Result1, company.PurchaseDiscountAccountID, Detail.Discount * ratio, 0, Detail.Discount, 0, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);
    //                dc.usp_OperationDetails_Insert(Result1, Detail.Account_ID, 0, Detail.Discount * ratio, 0, Detail.Discount, "حسم مكتسب", this.Payment_ID, this.DocumentTableType_ID);

    //            }
    //        }

    //    }
    //    dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.Total * ratio, this.Payment_ID, this.DocumentTableType_ID, txtNotes.Text);
    //}

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
            acCreditAccount.Value = dc.fun_getContactAccountID(invoice.Contact_ID).Value.ToExpressString();
            this.FilterSalesReps();
            acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
        }
        else if (this.RelatedDocTableType_ID == 2)
        {
            var Receipt = dc.usp_Receipt_SelectByID(this.RelatedDoc_ID).FirstOrDefault();
            ddlCurrency.SelectedValue = Receipt.Currency_ID.ToExpressString();
            acBranch.Value = Receipt.Branch_ID.ToStringOrEmpty();
            this.FilterByBranchAndCurrency();
            acDebitAccount.Value = dc.fun_getContactAccountID(Receipt.Contact_ID).Value.ToExpressString();
        }
    }

    private void CustomPage()
    {
        acDetailCostCenter.Visible = acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;

        acSalesRep.Visible = MyContext.Features.SalesRepEnabled && (Request.PathInfo == "/CashInCustomer" || Request.PathInfo == "/BankDepositCustomer");
        foreach (DataControlField col in gvDetails.Columns)
        {
            if (col.ItemStyle.CssClass == "SalesRepCol") col.Visible = MyContext.Features.SalesRepEnabled && (Request.PathInfo == "/CashInCustomer" || Request.PathInfo == "/BankDepositCustomer");
        }
    }

    private void SetDefaults()
    {
        if (Page.IsPostBack) return;
        var company = dc.usp_Company_Select().FirstOrDefault();

        if (company.AutoDate.Value) txtOperationDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");
        if (txtOperationDate.IsNotEmpty) this.ddlCurrency_SelectedIndexChanged(null, null);



        txtOperationDate.Enabled = !company.LockAutoDate.Value;
    }
    private void ChangeTax()
    {
        if (chkIsTaxFound.Checked && txtAmount.Text.ToDecimalOrDefault() > 0 && txtAmount.Text.ToDecimalOrDefault() > txtDiscount.Text.ToDecimalOrDefault())
        {
            var typeTax = dc.usp_Company_Select().FirstOrDefault().TypeTaxExpenses.Value;
            var tx = dc.Taxes.Where(x => x.IsActive == true).OrderByDescending(c => c.ID).FirstOrDefault();
            txtTaxFound.Text = string.Format("{0:0.##}", (typeTax == 1 ? (((txtAmount.Text.ToDecimalOrDefault() - txtDiscount.Text.ToDecimalOrDefault()) * tx.PercentageValue / 100))
                                          : ((((txtAmount.Text.ToDecimalOrDefault() - txtDiscount.Text.ToDecimalOrDefault()) * tx.PercentageValue) / (100 + tx.PercentageValue)))));
        }
        else
        {
            txtTaxFound.Text = string.Empty;
        }
    }
    #endregion

}