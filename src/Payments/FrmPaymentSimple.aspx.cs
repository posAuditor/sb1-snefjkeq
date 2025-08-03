using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Payments_FrmPaymentSimple : UICulturePage
{
    public XpressDataContext dc = new XpressDataContext();
 
    public int DocKindId
    {
        get
        {
            if (Request.QueryString["p"] == "ReceiptNots")
                return DocumentKindClass.PaymentReceiptNote.ToByte();
            else if (Request.QueryString["p"] == "CashIn")
                return DocumentKindClass.CashIn;
            else if (Request.QueryString["p"] == "CashInCustomer")
                return DocumentKindClass.CashInCustomer;
            else if (Request.QueryString["p"] == "CashOut")
                return DocumentKindClass.CashOut;
            else if (Request.QueryString["p"] == "CashOutVendor")
                return DocumentKindClass.CashOutVendor;
            else if (Request.QueryString["p"] == "BankDeposit")
                return DocumentKindClass.PaymentBankDeposit;
            else if (Request.QueryString["p"] == "BankWithdraw")
                return DocumentKindClass.PaymentBankWithdraw;
            else if (Request.QueryString["p"] == "BankDepositCustomer")
                return DocumentKindClass.PaymentBankDeposit.ToByte();
            else if (Request.QueryString["p"] == "BankWithdrawVendor")
                return DocumentKindClass.PaymentBankWithdraw.ToByte();
            return 0;
        }
    }

    public int TypeTax
    {
        get
        {
            if (Session["TypeTax_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["TypeTax_Invoice" + this.WinID] = dc.usp_Company_Select().FirstOrDefault().TypeTax;
            }
            return (int)Session["TypeTax_Invoice" + this.WinID];
        }

        set
        {
            Session["TypeTax_Invoice" + this.WinID] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadControls();                        
        }

    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    public string GetCostCenterContextKey()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false,";
    }

    public string GetDebitAccountContextKey()
    {
        try
        {
            string DebitParent_ID = string.Empty;
            string CreditParent_ID = string.Empty;
            bool IncludeParent_debit = true;
            bool IncludeParent_credit = true;
            switch (Request.QueryString["p"])
            {
                case "CashIn":
                    DebitParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "CashInCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = (MyContext.UserProfile.CashierAccount_ID != null) ? MyContext.UserProfile.CashierAccount_ID.ToExpressString() : COA.CashOnHand.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "CashOut":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "CashOutVendor":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;

                case "BankDeposit":
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;
                case "BankDepositCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    IncludeParent_credit = false;
                    break;
                case "BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "BankWithdrawVendor":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    IncludeParent_debit = false;
                    break;
            }

            string acDebitAccountContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + BranchIdHidden.Value + "," + ddlCurrency.SelectedValue + "," + DebitParent_ID + ",false," + IncludeParent_debit.ToExpressString();
            //string acDetailCostCenterContextKey = acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + BranchIdHidden.Value;
            return acDebitAccountContextKey;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
            return string.Empty;
        }
    }

    public string GetCreditAccountContextKey()
    {
        try
        {
            string DebitParent_ID = string.Empty;
            string CreditParent_ID = string.Empty;
            bool IncludeParent_debit = true;
            bool IncludeParent_credit = true;
            switch (Request.QueryString["p"])
            {
                case "CashIn":
                    DebitParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "CashInCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = (MyContext.UserProfile.CashierAccount_ID != null) ? MyContext.UserProfile.CashierAccount_ID.ToExpressString() : COA.CashOnHand.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "CashOut":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "CashOutVendor":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;

                case "BankDeposit":
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    break;
                case "BankDepositCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_debit = false;
                    IncludeParent_credit = false;
                    break;
                case "BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    break;

                case "BankWithdrawVendor":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    IncludeParent_credit = false;
                    IncludeParent_debit = false;
                    break;
            }

            string acCreditAccountContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + BranchIdHidden.Value + "," + ddlCurrency.SelectedValue + "," + CreditParent_ID + ",false," + IncludeParent_credit.ToExpressString();
            // acCreditAccount.ContextKey = "" + acBranch.Value + "";

            //string acDebitAccountContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + BranchIdHidden.Value + "," + ddlCurrency.SelectedValue + "," + DebitParent_ID + ",false," + IncludeParent_debit.ToExpressString();
            //string acDetailCostCenterContextKey = acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + BranchIdHidden.Value;
            return acCreditAccountContextKey;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
            return string.Empty;
        }
    }

    private void LoadControls()
    {
        var currency = dc.usp_Currency_Select(false).ToList();
        ddlCurrency.DataSource = currency;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }

    public int? GetCustomerContactID()
    {
        if (!string.IsNullOrWhiteSpace(CreditAccountIdHidden.Value))
            return this.dc.fun_GetCustomerContactID(CreditAccountIdHidden.Value.ToNullableInt());
        else return null;
    }
}