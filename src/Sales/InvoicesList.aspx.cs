using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Script.Serialization;
using System.Web.Security;

public partial class Sales_InvoicesList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtInvoicesList
    {
        get
        {
            return (DataTable)Session["dtInvoicesList" + this.WinID];
        }

        set
        {
            Session["dtInvoicesList" + this.WinID] = value;
        }
    }

    #endregion

    #region ViewState

    private int Invoice_ID
    {
        get
        {
            if (ViewState["Invoice_ID"] == null) return 0;
            return (int)ViewState["Invoice_ID"];
        }

        set
        {
            ViewState["Invoice_ID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvInvoicesList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvInvoicesList.Columns[8].Visible = this.MyContext.PageData.IsViewDoc;
                gvInvoicesList.Columns[9].Visible = this.MyContext.PageData.IsPrint;
                btnPrintList.Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillInvoicesList();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "OpenModalDialog", "<script type='text/javascript'>document.getElementById('" + lnkSearch.ClientID + "').scrollIntoView('cph_btnSearch');</script>", false);
          
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

            #region Set Saved Search
            InvoicesSearchable = new InvoiceListSearchable();
            InvoicesSearchable.acBranch = acBranch.Value.ToNullableInt();
            InvoicesSearchable.DocStatus = (ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte());
            InvoicesSearchable.FromDate = (txtDateFromSrch.Text.ToDate() == DateTime.MinValue ? null : txtDateFromSrch.Text.ToDate());
            InvoicesSearchable.ToDate = (txtDateToSrch.Text.ToDate() == DateTime.MinValue ? null : txtDateToSrch.Text.ToDate());

            #endregion


            this.FillInvoicesList();
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "ClickScript", "var elmnt = document.getElementById('cph_btnSearch'); elmnt.scrollIntoView('cph_btnSearch');", true);
           // ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "OpenModalDialog", "<script type='text/javascript'>document.getElementById('" + btnSearch.ClientID + "').scrollIntoView('cph_btnSearch');</script>", false);
           
            ddlStatus.Focus();
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
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            ddlStatus.SelectedIndex = 0;
            ddlIsHasBill.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            acCustomerName.Clear();
            this.FilterCustomers(null, null);
            this.FillInvoicesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterCustomers(object sender, EventArgs e)
    {
        try
        {
            acCustomerName.ContextKey = "C," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInvoicesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvInvoicesList.PageIndex = e.NewPageIndex;
            gvInvoicesList.DataSource = this.dtInvoicesList;
            gvInvoicesList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInvoicesList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvInvoicesList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    private int IndexSelected
    {
        get
        {
            if (ViewState["IndexSelected"] == null) return 0;
            return (int)ViewState["IndexSelected"];
        }

        set
        {
            ViewState["IndexSelected"] = value;
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


    protected void lnkCollect_Click(object sender, EventArgs e)
    {


        #region New Code

        try
        {
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            IndexSelected = Index;
            if (Math.Abs(gvInvoicesList.DataKeys[Index]["CollectedAmount"].ToDecimal() - gvInvoicesList.DataKeys[Index]["GrossTotalAmount"].ToDecimal()) < 0.0001m)
            {
                UserMessages.Message("تحصيل الفاتورة قد اكتمل");
                return;
            }

            this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
            lblInvoiceSerial.Text = gvInvoicesList.DataKeys[Index]["Serial"].ToExpressString();
            lblCollected.Text = gvInvoicesList.DataKeys[Index]["CollectedAmount"].ToExpressString();
            lblGrossTotal.Text = gvInvoicesList.DataKeys[Index]["GrossTotalAmount"].ToExpressString();

            var invoice = dc.usp_Invoice_SelectByID(this.Invoice_ID).FirstOrDefault();
            var iddd = dc.fun_getContactAccountID(invoice.Contact_ID).Value.ToExpressString();
            iddd = dc.fun_getContactAccountID(invoice.Contact_ID).Value.ToExpressString();



            acCreditAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + iddd;

            acCreditAccount.Value = iddd;

            var invoiceAmountRest =
                Math.Abs(gvInvoicesList.DataKeys[Index]["CollectedAmount"].ToDecimal() -
                         gvInvoicesList.DataKeys[Index]["GrossTotalAmount"].ToDecimal());

            lblDebitBalance.Text = dc.fun_GetAccountBalanceInForeign(COA.CashOnHand.ToInt(), txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value.ToExpressString();

            lblCreditBalance.Text = invoiceAmountRest.ToString("0.##");
            txtVisa.Text = "";
            txtMaster.Text = "";
            txtAtm.Text = "";
            txtBalanceNow.Text = "";
            acCreditAccount.Enabled = false;

            if (this.MyContext.UserProfile.Branch_ID != null)
            {
                acBranchPayment.Value = invoice.Branch_ID.ToExpressString();
                acBranchPayment.Enabled = false;
            }
            else
            {
                try
                {
                    acBranchPayment.Value = invoice.Branch_ID.ToExpressString();

                    acBranchPayment.Enabled = false;
                }
                catch
                {

                }
            }
            this.RelatedDoc_ID = Invoice_ID.ToInt();
            this.RelatedDocTableType_ID = 1;
            mpePayment.Show();
            btnApprove.Visible = true;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

        #endregion




        #region Old Code

        //try
        //{
        //    int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
        //    this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
        //    lblInvoiceSerial.Text = gvInvoicesList.DataKeys[Index]["Serial"].ToExpressString();
        //    lblCollected.Text = gvInvoicesList.DataKeys[Index]["CollectedAmount"].ToExpressString();
        //    lblGrossTotal.Text = gvInvoicesList.DataKeys[Index]["GrossTotalAmount"].ToExpressString();
        //    mpeCollect.Show();
        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        //} 
        #endregion




    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            mpeCollect.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnOkCollect_click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(ddlCollectWith.SelectedValue + "?RelatedDocTableType_ID=1&RelatedDoc_ID=" + Invoice_ID.ToExpressString(), false);
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
            string SaveName = Request.PathInfo == "/Invoice" ? "Invoice" : "SalesOrder";
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
            Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1");


            //string SaveName = Request.PathInfo == "/Invoice" ? "Invoice" : "SalesOrder";
            //int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            //this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
            //int? Branch_ID = gvInvoicesList.DataKeys[Index]["Branch_ID"].ToNullableInt();

            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());



            //ReportDocument doc = new ReportDocument();

            //doc.Load(Server.MapPath("~\\Reports\\InvoicePrint.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            //doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            //doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            //doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            // doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
           //////// Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrintList_Click(object sender, EventArgs e)
    {
        try
        {
            int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
            byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
            bool? HasInvoice = ddlIsHasBill.SelectedIndex == 0 ? (bool?)null : ddlIsHasBill.SelectedValue.ToBoolean();

            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\InvoiceList_Print.rpt"));
            doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            doc.SetParameterValue("@Currency_ID", Currency_ID);
            doc.SetParameterValue("@Serial", txtSerialsrch.Text);
            doc.SetParameterValue("@Contact_ID", acCustomerName.Value.ToNullableInt());
            doc.SetParameterValue("@FromDate", txtDateFromSrch.Text.ToDate());
            doc.SetParameterValue("@ToDate", txtDateToSrch.Text.ToDate());
            doc.SetParameterValue("@UserRefNo", txtUserRefNo.Text);
            doc.SetParameterValue("@DocStatus_ID", DocStatus_ID);
            doc.SetParameterValue("@Culture", MyContext.CurrentCulture.ToByte());
            doc.SetParameterValue("@HasInvoice", HasInvoice);
            doc.SetParameterValue("@EntryType", 2);
            doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            doc.SetParameterValue("ByEmp", MyContext.UserProfile.EmployeeName);


            doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            //doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(null, "InvoiceList"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillInvoicesList()
    {
        byte EntryType = Request.PathInfo == "/Invoice" ? (byte)2 : (byte)0;
        lnkadd.NavigateUrl = Request.PathInfo == "/Invoice" ? PageLinks.InvoiceShortcut : PageLinks.SalesOrder;
        gvInvoicesList.Columns[10].Visible = Request.PathInfo != "/Invoice";
        gvInvoicesList.Columns[11].Visible = Request.PathInfo == "/Invoice";
        gvInvoicesList.Columns[12].Visible = Request.PathInfo == "/Invoice";
        divHasBill.Visible = Request.PathInfo != "/Invoice";
        btnPrintList.Visible = Request.PathInfo == "/Invoice" && MyContext.PageData.IsPrint;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        bool? HasInvoice = ddlIsHasBill.SelectedIndex == 0 ? (bool?)null : ddlIsHasBill.SelectedValue.ToBoolean();
        int? CollectType = ddlCollect.SelectedValue.ToInt();

         
        int? CreatedBy_ID = (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID);
        if (acnameEmp.HasValue)
        {
            CreatedBy_ID = acnameEmp.Value.ToInt();
        }
        var lstInvoices =
            dc.usp_InvoiceRep_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText,
                acCustomerName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(),
                txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(),
                HasInvoice, EntryType, CollectType, CreatedBy_ID,
                acnameEmp.Value.ToNullableInt(), txtCustomerName.TrimmedText, 
                txtCustomerMobile.TrimmedText, txtNotess.TrimmedText, 
                txtGrossTotal.Text.ToDecimalOrDefault(), ddlPaymentMethode.SelectedValue.ToIntOrDefault()).ToList().Where(x => x.IsPos == null);
        foreach (var item in lstInvoices)
        {
            
            item.PageName = item.PageName.Replace("Invoice", "InvoiceForm");
            item.ToInvoicePage = item.ToInvoicePage.Replace("Invoice", "InvoiceForm");
        }
        this.dtInvoicesList = lstInvoices.CopyToDataTable();

        gvInvoicesList.DataSource = this.dtInvoicesList;
        gvInvoicesList.DataBind();
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        acBranchPayment.ContextKey = string.Empty;

        acnameEmp.ContextKey = ",,";
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
        this.FilterCustomers(null, null);


        var listSos = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList().Select(c => new PaymentMethodeCls { Name = c.Name, ID = c.ID }).ToList();

        listSos.Add(new PaymentMethodeCls { Name = "الكل", ID = -2 });
        listSos.Add(new PaymentMethodeCls { Name = "اجـل", ID = -1 });
        listSos.Add(new PaymentMethodeCls { Name = "نقدي", ID = 0 });

        ddlPaymentMethode.DataSource = listSos.OrderBy(c => c.ID).ToList();
        ddlPaymentMethode.DataTextField = "Name";
        ddlPaymentMethode.DataValueField = "ID";
        ddlPaymentMethode.DataBind();
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;

        foreach (DataControlField col in gvInvoicesList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }




    public bool GetTypeCollect(string status, decimal gross, decimal total)
    {
        if ((status == "2" && gross > total) || gross == 0)
        {
            return true;
        } return false;
    }


    public string GetTypeCado(string status, decimal gross, decimal total)
    {
        if (status == "2" && gross > total)
        {
            return Resources.Labels.Collect;
        }
        return "هدية";
    }



    #endregion



    #region Pay

    List<PaymentsOperationDetail> OperationDetailsList = new List<PaymentsOperationDetail>();

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
    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        OperationDetailsList.Clear();
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        var brnID = int.Parse(acBranchPayment.Value);
        var lst = dc.PaymentMethodes.Where(x => x.Branch_ID == brnID).ToList();
        if (!lst.Any())
        {
            UserMessages.Message(null, "إعدادات حسابات غير معرفة", string.Empty);
            return false;
        }
        // btnPrintPay.Enabled = false;
        //btnInvoicePrintWithoutItems.Enabled = false;
        var objPm = lst.First();

        var DebitAccount_Atm_ID = objPm.Account_Atm_Id;
        var DebitAccount_Master_ID = objPm.Account_Master_Id;
        var DebitAccount_Visa_ID = objPm.Account_Visa_Id;
        var DebitAccount_Treasury_ID = objPm.Account_Treasury_Id;
        if (MyContext.UserProfile.CashierAccount_ID > 0)
        {
            DebitAccount_Treasury_ID = MyContext.UserProfile.CashierAccount_ID;
        }

        if (DebitAccount_Atm_ID == null || DebitAccount_Master_ID == null || DebitAccount_Visa_ID == null || DebitAccount_Treasury_ID == null)
        {
            UserMessages.Message(null, "إعدادات حسابات الدفع غير مكتملة", string.Empty);
            return false;
        }


        var creditAccount = acCreditAccount.Value.ToInt();
        if (creditAccount == null || creditAccount == 0)
        {
            UserMessages.Message(null, "حساب العميل غير موجود", string.Empty);
            return false;
        }


        if (!this.EditMode)
        {
            this.Total = txtAtm.Text.ToDecimalOrDefault() + txtMaster.Text.ToDecimalOrDefault() + txtVisa.Text.ToDecimalOrDefault() +
                         txtBalanceNow.Text.ToDecimalOrDefault();
            ViewState["DocRandomString"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            this.Payment_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, approvedBY_ID, ApproveDate, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), "", this.Total, null, DocStatus_ID, PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), 1, 15, this.RelatedDoc_ID, this.RelatedDocTableType_ID, this.DocRandomString);
            if (this.Payment_ID > 0)
            {
                if (this.Payment_ID == null || this.Payment_ID == 0)
                {
                    UserMessages.Message(null, "العملية فشلت أرجو الاعادة", string.Empty);
                    return false;
                }


                if (txtAtm.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(this.Payment_ID, DebitAccount_Atm_ID, creditAccount, txtAtm.Text.ToDecimalOrDefault(), null, "", null);
                if (txtMaster.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(this.Payment_ID, DebitAccount_Master_ID, creditAccount, txtMaster.Text.ToDecimalOrDefault(), null, "", null);
                if (txtVisa.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(this.Payment_ID, DebitAccount_Visa_ID, creditAccount, txtVisa.Text.ToDecimalOrDefault(), null, "", null);
                if (txtBalanceNow.Text.ToDecimalOrDefault() > 0) dc.usp_PaymentsDetails_Insert(this.Payment_ID, DebitAccount_Treasury_ID, creditAccount, txtBalanceNow.Text.ToDecimalOrDefault(), null, "", null);
                if (IsApproving)
                {

                    if (txtAtm.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Atm_ID).FirstOrDefault().Name,
                            Account_ID = DebitAccount_Atm_ID,
                            DebitAmount = txtAtm.Text.ToDecimalOrDefault(),
                            CreditAmount = 0,
                            CostCenter_ID = null,
                            Notes = ""
                        });
                    if (txtAtm.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                            Account_ID = creditAccount,
                            DebitAmount = 0,
                            CreditAmount = txtAtm.Text.ToDecimalOrDefault(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = null
                        });

                    if (txtMaster.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName =
                                dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Master_ID).FirstOrDefault().Name,
                            Account_ID = DebitAccount_Master_ID,
                            DebitAmount = txtMaster.Text.ToDecimalOrDefault(),
                            CreditAmount = 0,
                            CostCenter_ID = null,
                            Notes = ""
                        });
                    if (txtMaster.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                            Account_ID = creditAccount,
                            DebitAmount = 0,
                            CreditAmount = txtMaster.Text.ToDecimalOrDefault(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = null
                        });
                    if (txtVisa.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName =
                                dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Visa_ID).FirstOrDefault().Name,
                            Account_ID = DebitAccount_Visa_ID,
                            DebitAmount = txtVisa.Text.ToDecimalOrDefault(),
                            CreditAmount = 0,
                            CostCenter_ID = null,
                            Notes = ""
                        });
                    if (txtVisa.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                            Account_ID = creditAccount,
                            DebitAmount = 0,
                            CreditAmount = txtVisa.Text.ToDecimalOrDefault(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = null
                        });

                    if (txtBalanceNow.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName =
                                dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Treasury_ID).FirstOrDefault().Name,
                            Account_ID = DebitAccount_Treasury_ID,
                            DebitAmount = txtBalanceNow.Text.ToDecimalOrDefault(),
                            CreditAmount = 0,
                            CostCenter_ID = null,
                            Notes = ""
                        });
                    if (txtBalanceNow.Text.ToDecimalOrDefault() > 0)
                        OperationDetailsList.Add(new PaymentsOperationDetail()
                        {
                            AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                            Account_ID = creditAccount,
                            DebitAmount = 0,
                            CreditAmount = txtBalanceNow.Text.ToDecimalOrDefault(),
                            CostCenter_ID = null, // Note This Null to prevent dubplicate
                            Notes = null
                        });

                }
                if (IsApproving) InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Payments_Update(this.Payment_ID, txtOperationDate.Text.ToDate(), approvedBY_ID, ApproveDate, txtUserRefNo.TrimmedText, ref Serial, null, "", this.Total, null, DocStatus_ID, acBranch.Value.ToNullableInt(), 1, ddlCurrency.SelectedValue.ToInt());
            if (Result > 0)
            {

                if (IsApproving)
                {
                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName = dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Atm_ID).FirstOrDefault().Name,
                        Account_ID = DebitAccount_Atm_ID,
                        DebitAmount = txtAtm.ToDecimal(),
                        CreditAmount = 0,
                        CostCenter_ID = null,
                        Notes = ""
                    });
                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                        Account_ID = creditAccount,
                        DebitAmount = 0,
                        CreditAmount = txtAtm.ToDecimal(),
                        CostCenter_ID = null, // Note This Null to prevent dubplicate
                        Notes = null
                    });


                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName =
                            dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Master_ID).FirstOrDefault().Name,
                        Account_ID = DebitAccount_Master_ID,
                        DebitAmount = txtMaster.ToDecimal(),
                        CreditAmount = 0,
                        CostCenter_ID = null,
                        Notes = ""
                    });
                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                        Account_ID = creditAccount,
                        DebitAmount = 0,
                        CreditAmount = txtMaster.ToDecimal(),
                        CostCenter_ID = null, // Note This Null to prevent dubplicate
                        Notes = null
                    });

                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName =
                            dc.ChartOfAccounts.Where(x => x.ID == DebitAccount_Visa_ID).FirstOrDefault().Name,
                        Account_ID = DebitAccount_Visa_ID,
                        DebitAmount = txtVisa.ToDecimal(),
                        CreditAmount = 0,
                        CostCenter_ID = null,
                        Notes = ""
                    });
                    OperationDetailsList.Add(new PaymentsOperationDetail()
                    {
                        AccountName = dc.ChartOfAccounts.Where(x => x.ID == creditAccount).FirstOrDefault().Name,
                        Account_ID = creditAccount,
                        DebitAmount = 0,
                        CreditAmount = txtVisa.ToDecimal(),
                        CostCenter_ID = null, // Note This Null to prevent dubplicate
                        Notes = null
                    });
                }
                // }

                if (IsApproving) InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, "Error Serial", dc);
            }
        }
        if (this.ConfirmationMessage != string.Empty)
        {

            mpePayment.Show();
            trans.Rollback();
            return false;
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(string.Empty, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        return true;
    }
    private void InsertOperation()
    {
        if (OperationDetailsList.Count == 0) return;
        decimal ratio = 1;
        string serial = string.Empty;

        int Result = dc.usp_Operation_Insert(acBranchPayment.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), 15, this.Total * ratio, this.Total, ratio, "");

        var GroupedOperationDetails = from OperationDetails in this.OperationDetailsList
                                      group OperationDetails by new { OperationDetails.Account_ID, OperationDetails.AccountName, IsDebit = (OperationDetails.CreditAmount == 0), OperationDetails.Notes } into groupedDetails
                                      select new { Key = groupedDetails.Key, DebitAmount = groupedDetails.Sum(x => x.DebitAmount), CreditAmount = groupedDetails.Sum(x => x.CreditAmount) };

        foreach (var Detail in GroupedOperationDetails)
        {
            if ((this.OperationType_ID == OperationTypes.CashOut.ToInt() || this.OperationType_ID == OperationTypes.BankWithdraw.ToInt()) && !this.ConfirmationAnswered)
            {
                if (Detail.CreditAmount > 0 && Detail.CreditAmount > dc.fun_GetAccountBalanceInForeign(Detail.Key.Account_ID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value)
                {
                    this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BalanceNotEnough + " (" + Detail.Key.AccountName + ")";
                }
            }
            dc.usp_OperationDetails_Insert(Result, Detail.Key.Account_ID, Detail.DebitAmount * ratio, Detail.CreditAmount * ratio, Detail.DebitAmount, Detail.CreditAmount, Detail.Key.Notes, this.Payment_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        }

        //CostCenter
        foreach (var Detail in OperationDetailsList)
        {
            dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), Detail.CostCenter_ID, txtOperationDate.Text.ToDate(), (Detail.DebitAmount + Detail.CreditAmount) * ratio, this.Payment_ID, DocumentsTableTypes.Payment_CashIn.ToInt(), Detail.Notes);
        }
        // dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), null, txtOperationDate.Text.ToDate(), this.Total * ratio, this.Payment_ID, DocumentsTableTypes.Payment_CashIn.ToInt(), "");
    }
    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        if ((txtAtm.Text.ToDecimalOrDefault() + txtVisa.Text.ToDecimalOrDefault() + txtMaster.Text.ToDecimalOrDefault() + txtBalanceNow.Text.ToDecimalOrDefault()) == 0)
        {
            UserMessages.Message(null, "المبالغ غير صحيحة", string.Empty);
            mpePayment.Show();
            return;

        }


        if ((lblCreditBalance.Text.ToDecimalOrDefault() - (txtAtm.Text.ToDecimalOrDefault() + txtVisa.Text.ToDecimalOrDefault() + txtMaster.Text.ToDecimalOrDefault() + txtBalanceNow.Text.ToDecimalOrDefault())) < 0)
        {

            UserMessages.Message("راجع المبالغ ");
            mpePayment.Show(); return;

        }
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;



        try
        {
            if (this.Save(true, trans))
            {
                trans.Commit();
                lblDebitBalance.Text =
                    dc.fun_GetAccountBalanceInForeign(COA.CashOnHand.ToInt(), txtOperationDate.Text.ToDate(),
                        acBranch.Value.ToNullableInt()).Value.ToString("0.##");


                lblCreditBalance.Text = dc.fun_GetAccountBalanceInForeign(acCreditAccount.Value.ToNullableInt(), txtOperationDate.Text.ToDate(),
                    acBranch.Value.ToNullableInt()).Value.ToString("0.##"); ;


            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            btnApprove.Visible = true;
            //btnPrintPay.Enabled = false;
            // btnInvoicePrintWithoutItems.Enabled = false;
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }


        FillInvoicesList();


        var invoiceAmountRest =
              Math.Abs(gvInvoicesList.DataKeys[IndexSelected]["CollectedAmount"].ToDecimal() -
                       gvInvoicesList.DataKeys[IndexSelected]["GrossTotalAmount"].ToDecimal());
        btnApprove.Visible = false;
        mpePayment.Hide();




    }
    protected void txtOperationDate_TextChanged(object sender, EventArgs e)
    {
        mpePayment.Show();
    }
    protected void txtAtm_TextChanged(object sender, EventArgs e)
    {
        mpePayment.Show();
    }
    protected void lnkGenerate_Click(object sender, EventArgs e)
    {

        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            var ID = (sender as LinkButton).CommandArgument;


            Response.Redirect("~/Sales/InvoiceForm.aspx?GenerateInoice_ID=" + ID, false);

        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }






    } 
    #endregion




    //#region Invoice
    ////---------------------------------------Invoice---------------------------------------


    //private bool IsUserInUse(CashierAppLoggedInUser u, bool force)
    //{
    //    try
    //    {
    //        //User Locks
    //        if (HttpRuntime.Cache.Get("_LoggedUsers") == null)
    //        {
    //            List<LoggedUser> lst = new List<LoggedUser>();
    //            HttpRuntime.Cache.Add("_LoggedUsers", lst, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60), System.Web.Caching.CacheItemPriority.NotRemovable, null);
    //        }

    //        List<LoggedUser> LoggedUsersList = (List<LoggedUser>)HttpRuntime.Cache.Get("_LoggedUsers");


    //        var LoggedUsers = from data in LoggedUsersList
    //                          where data.UserID.ToLower().Trim() == u.UserGuid.ToLower().Trim()
    //                          select data;

    //        if (force) //set Lock
    //        {
    //            LoggedUsers.ToList().ForEach(x => LoggedUsersList.Remove(x));
    //            LoggedUser LoggedUser = new LoggedUser() { SessionID = u.SecurityTokenID, UserID = u.UserGuid, LastRefresh = DateTime.Now };
    //            LoggedUsersList.Add(LoggedUser);
    //            return false;
    //        }

    //        if (LoggedUsers.Count() != 1 || LoggedUsers.FirstOrDefault().SessionID.ToLower().Trim() != u.SecurityTokenID.ToLower().Trim())
    //        {
    //            HttpRuntime.Cache.Remove(u.SecurityTokenID);
    //            return true;
    //        }
    //        return false;

    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.LogError(ex);
    //        return true;
    //    }
    //}

    //public CashierAppLoggedInUser Authenticate(string SecurityTokenID)
    //{
    //    CashierAppLoggedInUser u = (CashierAppLoggedInUser)HttpContext.Current.Cache.Get(SecurityTokenID);
    //    if (u != null && !this.IsUserInUse(u, false))
    //    {
    //        return u;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}


    //public CashierAppBaseResponseObject SaveInvoice(
    //    string SecurityTokenID,
    //    DateTime InvoiceDate,
    //    int CustomerID,
    //    decimal Total,
    //    decimal discount,
    //    decimal additionals,
    //    decimal grossTotal,
    //    List<CashierAppInvoiceDetails> Details,
    //    bool ConfirmationAnswered,
    //    decimal CashDiscount,
    //    decimal PercentageDiscount,
    //    decimal TaxAmount,
    //    decimal totalInit,
    //    decimal grossTotalInit,
    //    string usrRef,
    //    DateTime usrRefDate,
    //    string newSerialLocal,
    //    int typePayment,
    //    int? SalesRep_ID,
    //    string CustomerName,
    //    string CustomerMobile,
    //    decimal PaidRest,
    //    string CustomerNbrTax,
    //    List<PaymentSelected> PayDetails
    //    )
    //{

    //    CashierAppBaseResponseObject r = new CashierAppBaseResponseObject();
    //    System.Data.Common.DbTransaction trans;
    //    dc.Connection.Open();
    //    trans = dc.Connection.BeginTransaction();
    //    dc.Transaction = trans;

    //    try
    //    {

    //        var listPayPos = dc.SettingPointOs.Where(c => c.IsActive == true).ToList();
    //        InvoiceDate = InvoiceDate.Date;
    //        System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-eg", false);
    //        CashierAppLoggedInUser u = this.Authenticate(SecurityTokenID);

    //        //Get PercentValue 
    //        var taxPercentValue = dc.Taxes.Where(x => x.IsActive.Value).OrderByDescending(x => x.ID).FirstOrDefault();

    //        if (u != null && u.IsAddNewInvoice)
    //        {
    //            MyContext InvoiceContext = new MyContext(Membership.GetUser(u.UserName), "~/Sales/invoiceForm.aspx", string.Empty);
    //            int Detail_ID = 0;
    //            string Serial = string.Empty;
    //            decimal CalculatedSalesCost = 0;
    //            this.ConfirmationMessage = string.Empty;

    //            decimal ValueTax = 0;
    //            var company = dc.usp_Company_Select().FirstOrDefault();
    //            var taxTable = dc.Taxes.Where(x => x.IsActive.Value).OrderByDescending(x => x.ID).FirstOrDefault();
    //            if (company.TypeTax == 0)
    //            {
    //                ValueTax = 0;
    //            }
    //            else if (company.TypeTax == 1)
    //            {
    //                ValueTax = decimal.Divide(taxTable.PercentageValue.Value, 100);
    //            }
    //            else if (company.TypeTax == 2)
    //            {
    //                ValueTax = decimal.Divide(taxTable.PercentageValue.Value, 100 + taxTable.PercentageValue.Value);
    //            }



    //            var defaults = dc.usp_CashBillDefaults_Select(u.Branch_ID, u.UserID).First();
    //            string docRandomString = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString() + "_CashierInvoice";



    //            var listItem = dc.Items.Where(c => Details.Select(v => v.ItemID).ToList().Contains(c.ID)).ToList();


    //            decimal TaxV = 0;


    //            decimal GTA = 0;
    //            decimal TaxValue = 0;
    //            if (company.TypeTax == 1)
    //            {
    //                TaxV = (Total - (Total * PercentageDiscount / 100)) * ValueTax;
    //                TaxValue = Math.Round(TaxV, 2);
    //                GTA = Total - discount + TaxValue;
    //            }
    //            else if (company.TypeTax == 2)
    //            {
    //                TaxValue = TaxAmount;
    //                GTA = grossTotal;


    //            }
    //            decimal TotalInitcalculate = 0;
    //            decimal TotalTaxCalculate = 0;
    //            decimal TotalGrossCalculate = 0;


    //            decimal totalFinal = 0;
    //            decimal grossTotalFinal = 0;


    //            foreach (var item in Details)
    //            {
    //                if (company.TypeTax == 1)
    //                {
    //                    var t = item.Quantity.ToDecimalOrDefault() * item.Price.ToDecimalOrDefault();
    //                    TotalInitcalculate += t;
    //                    var isExistTax = listItem.Where(c => c.ID == item.ItemID).FirstOrDefault().Tax_ID != null ? true : false;
    //                    TotalTaxCalculate += (t - (t * PercentageDiscount / 100)) * (isExistTax ? ValueTax : 0);
    //                    TaxValue = Math.Round(TotalTaxCalculate, 2);
    //                    GTA = TotalInitcalculate - discount + TaxValue;
    //                }
    //                else if (company.TypeTax == 2)
    //                {
    //                    var t = item.Quantity.ToDecimalOrDefault() * item.Price.ToDecimalOrDefault();
    //                    TotalInitcalculate += t;
    //                    TotalTaxCalculate += t;

    //                    TaxValue = TaxAmount;
    //                    GTA = grossTotal;
    //                }

    //            }





    //            if (company.TypeTax == 1)
    //            {
    //                totalFinal = grossTotal - TaxValue;
    //            }
    //            else if (company.TypeTax == 2)
    //            {
    //                totalFinal = grossTotal + discount - TaxValue;
    //            }

    //            int invoiceId = dc.usp_InvoiceDateDelivery_Insert(InvoiceContext.UserProfile.Branch_ID, company.Currency_ID, 1,
    //                InvoiceDate, CustomerID, ref Serial, DocSerials.Invoice.ToInt(),
    //                DocStatus.Approved.ToByte(), DateTime.Now, u.UserID,
    //                DateTime.Now, u.UserID, null, null, null, null, null, null, null, string.Empty, TotalInitcalculate, PercentageDiscount, CashDiscount, additionals, discount,
    //                TaxValue, GTA, (typePayment == 1 ? GTA : 0), null, usrRef, docRandomString, 1, SalesRep_ID,
    //                null, defaults.DefaultCashAccount_ID, 1111, null, null, 1, 0, CustomerName, CustomerMobile, string.Empty, string.Empty, CustomerNbrTax, string.Empty, typePayment);

                

    //            if (invoiceId == -50)
    //            {
    //                trans.Rollback();

    //                r.StatusCode = -50;
    //                r.ResponseMessage = "رقم المرجع متكرر";
    //                r.CurrentUser = null;
    //                return r;
    //            }
    //            if (invoiceId <= 0) throw new Exception("Error occured during invoice saving");
    //            var invObject = dc.Invoices.Where(x => x.ID == invoiceId).FirstOrDefault();

    //            int AccountCustomer_ID = 0;
    //            var customer = dc.usp_CusomerAccount(CustomerID).FirstOrDefault();
    //            if (customer != null)
    //            {
    //                AccountCustomer_ID = customer.ChartOfAccount_ID.Value;
    //            }
    //            if (invObject != null)
    //            {
    //                invObject.usrRefDate = usrRefDate;
    //                invObject.TypeDeliveryName = newSerialLocal;
    //            }
    //            foreach (CashierAppInvoiceDetails d in Details)
    //            {
    //                //TODO INVOICE DEATIL : CAPACITY
    //                // Detail_ID = dc.usp_InvoiceDetails_Insert(invoiceId, defaults.DefaultStore_ID, d.ItemID, d.Price, d.Quantity, d.UOM, null, null, 0, 0, 0, string.Empty, 0, "");



    //                decimal uniteCostInit = d.Price;

    //                if (company.TypeTax == 1)
    //                {
    //                    uniteCostInit = d.Price;
    //                }
    //                else if (company.TypeTax == 2)
    //                {
    //                    uniteCostInit = d.Price - d.Price * ValueTax;
    //                }



    //                var TaxID = listItem.Where(c => c.ID == d.ItemID).FirstOrDefault();

    //                Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(invoiceId, defaults.DefaultStore_ID, d.ItemID, uniteCostInit, d.Quantity, d.UOM, null, TaxID.Tax_ID, 0, 0, 0, string.Empty, 0, "", string.Empty, string.Empty, string.Empty, null, string.Empty, null, d.Price);


    //                if (!this.InsertICJ(Detail_ID, InvoiceDate, defaults.DefaultStore_ID, InvoiceContext.UserProfile.Branch_ID, invoiceId, d, ConfirmationAnswered, CalculatedSalesCost, true))
    //                {
    //                    r.StatusCode = -6;
    //                    r.ResponseMessage = this.ConfirmationMessage;
    //                    trans.Rollback();
    //                    return r;
    //                }
    //            }
               

    //            if (InvoiceContext.UserProfile.Branch_ID == null && InvoiceContext.Features.BranchesEnabled)
    //            {
    //                r.StatusCode = -3;
    //                r.ResponseMessage = string.Empty;
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else if (defaults.DefaultStore_ID == null || defaults.DefaultCashAccount_ID == null || CustomerID == null)
    //            {
    //                r.StatusCode = -4;
    //                r.ResponseMessage = string.Empty;
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else if (InvoiceDate > DateTime.Now.Date)
    //            {
    //                r.StatusCode = -7;
    //                r.ResponseMessage = Resources.UserInfoMessages.DateBiggerThanToday;
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else if (InvoiceDate > InvoiceContext.FiscalYearEndDate || InvoiceDate < InvoiceContext.FiscalYearStartDate)
    //            {
    //                r.StatusCode = -7;
    //                r.ResponseMessage = Resources.UserInfoMessages.DateOutsideFiscalYear;
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else if (Details.Count() <= 0)
    //            {
    //                r.StatusCode = -8;
    //                r.ResponseMessage = Resources.UserInfoMessages.OneItemRequired;
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else if (invoiceId == -50)
    //            {
    //                r.StatusCode = -50;
    //                r.ResponseMessage = "رقم المرجع متكرر";
    //                r.CurrentUser = null;
    //                trans.Rollback();
    //            }
    //            else
    //            {
    //                this.InsertOperation(InvoiceContext.UserProfile.Branch_ID, Total, grossTotal,
    //                    discount, additionals, defaults.DefaultCashAccount_ID.Value, InvoiceDate,
    //                    company.Currency_ID, invoiceId, TaxAmount, typePayment, AccountCustomer_ID,
    //                    PaidRest, docRandomString, u.UserID, PayDetails, listPayPos, CalculatedSalesCost);
    //                r.StatusCode = 0;
    //                r.ResponseMessage = Serial;
    //                r.CurrentUser = u;
                    
    //                //dc.SubmitChanges();
    //                trans.Commit();

    //                //var invSaved = dc.Invoices.Where(c => c.ID == invoiceId).FirstOrDefault();
    //                //if (invSaved != null)
    //                //{
    //                //    invSaved.typePayment_ID = typePayment;
    //                //    dc.SubmitChanges();
    //                //}
    //            }
    //        }
    //        else
    //        {
    //            r.StatusCode = -2;
    //            r.ResponseMessage = string.Empty;
    //            r.CurrentUser = null;
    //            trans.Rollback();
    //        }

    //        return r;
    //    }
    //    catch (Exception ex)
    //    {
    //        trans.Rollback();
    //        Logger.LogError(ex);
    //        r.StatusCode = -1;
    //        r.ResponseMessage = ex.Message;
    //        r.CurrentUser = null;
    //        return r;
    //    }
    //}

    //private bool InsertICJ(int Detail_ID, DateTime InvoiceDate, int? Store_ID, int? Branch_ID, int Invoice_ID, 
    //    CashierAppInvoiceDetails row, bool ConfirmationAnswered,
    //    decimal CalculatedSalesCost, bool ConfirmationMessageIsAsk)
    //{
    //    decimal? SalesCost = 0;
    //    decimal? QtyAvailble = 0;
    //    int result = dc.usp_ICJ_InvoiceWithQtyAvailble(InvoiceDate, row.Quantity, row.ItemID, row.UOM, row.Quantity * row.Price, null, Store_ID, Branch_ID, DocumentsTableTypes.Invoice.ToInt(), Invoice_ID, Detail_ID, null, ref SalesCost, ref QtyAvailble);
    //    CalculatedSalesCost += SalesCost.Value;
       
    //    if ((result & ICJStoredProcFlags.QtyReserved.ToInt()) == ICJStoredProcFlags.QtyReserved.ToInt())
    //    {
    //        this.ConfirmationMessage += Resources.UserInfoMessages.QtyReserved + " (" + row.ItemName + ")";
    //        return false;
    //    }

    //    if ((result & ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt())
    //    {
    //        this.ConfirmationMessage += Resources.UserInfoMessages.QtyNotEnough + " (" + row.ItemName + ")";
    //        return false;
    //    }
    //    if ((result & ICJStoredProcFlags.BatchOrderWarning.ToInt()) == ICJStoredProcFlags.BatchOrderWarning.ToInt())
    //    {
    //        this.ConfirmationMessage += Resources.UserInfoMessages.BatchOrderWarning + " (" + row.ItemName + ")";
    //        return false;
    //    }

    //    if ((result & ICJStoredProcFlags.BatchQtyNotEnough.ToInt()) == ICJStoredProcFlags.BatchQtyNotEnough.ToInt())
    //    {
    //        this.ConfirmationMessage += Resources.UserInfoMessages.BatchQtyNotEnough + " (" + row.ItemName + ")";
    //        return false;
    //    }

    //    if ((result & ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt() && !ConfirmationAnswered)
    //    {
    //        this.ConfirmationMessage += "\r\n \u2022 " + Resources.UserInfoMessages.QtyNotEnough + " (" + row.ItemName + ")";
    //        ConfirmationMessageIsAsk = true;
    //    }
    //    if ((result & ICJStoredProcFlags.PriceLessThanCost.ToInt()) == ICJStoredProcFlags.PriceLessThanCost.ToInt() && !ConfirmationAnswered)
    //    {
    //        this.ConfirmationMessage += "\r\n \u2022 " + Resources.UserInfoMessages.PriceLessThanCost + " (" + row.ItemName + ")";
    //        ConfirmationMessageIsAsk = true;
    //    }
    //    return true;
    //}

    //private void InsertOperation(int? Branch_ID,
    //                             decimal Total,
    //                             decimal GrossTotal,
    //                             decimal TotalDiscount,
    //                             decimal Additionals,
    //                             int DefaultCashAccount_ID,
    //                             DateTime InvoiceDate,
    //                             int Currency_ID,
    //                             int Invoice_ID,
    //                             decimal TaxAmount,
    //                             int TypePayment,
    //                             int AccountCustomer_ID,
    //                             decimal PaidRest,
    //                             string docRandomString,
    //                             int CreatedBy_ID,
    //                             List<PaymentSelected> PayDetails,
    //                             List<SettingPointO> listPayPos,decimal CalculatedSalesCost )
    //{
    //    decimal ratio = 1;

    //    string serial = string.Empty;
    //    var company = dc.usp_Company_Select().FirstOrDefault();

    //    int Result = dc.usp_Operation_Insert(Branch_ID, InvoiceDate, ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), Currency_ID, Math.Round(Total + Additionals + TaxAmount, 2) * ratio, Math.Round(Total + Additionals + TaxAmount, 2), ratio, string.Empty);

    //    //ايراد المبيعات
    //    dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, Math.Round((Total) * ratio, 2), 0, Math.Round((Total), 2), null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    //ايراد الاضافات
    //    if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, (Additionals) * ratio, 0, (Additionals), null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    //tax
    //    if (Math.Round(TaxAmount, 2) > 0)
    //        dc.usp_OperationDetails_Insert(Result, 83, 0, Math.Round(TaxAmount, 2), 0, Math.Round(TaxAmount, 2), null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


    //    if ((TypePayment == 0 || TypePayment == 1 || TypePayment == 2 || TypePayment == 3) && Math.Round(Total + Additionals + TaxAmount, 2) == Math.Round(PaidRest, 2))
    //    {
    //        var objectCash = PayDetails.Where(c => c.ID == 0).FirstOrDefault();
    //        if (objectCash != null)
    //        {
    //            dc.usp_OperationDetails_Insert(Result, DefaultCashAccount_ID, Math.Round((objectCash.Amount) * ratio, 2), 0, Math.Round((objectCash.Amount), 2), 0, null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //        }
    //        foreach (var item in PayDetails.Where(c => c.ID != 0).ToList())
    //        {
    //            var objpay = listPayPos.Where(c => c.ID == item.ID).FirstOrDefault();
    //            //الخزنة على طول عشان نقدية
    //            dc.usp_OperationDetails_Insert(Result, objpay.SalesAccountID, Math.Round((item.Amount) * ratio, 2), 0, Math.Round((item.Amount), 2), 0, null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //        }
    //        if (!PayDetails.Any())
    //        {
    //            dc.usp_OperationDetails_Insert(Result, DefaultCashAccount_ID, Math.Round((GrossTotal) * ratio, 2), 0, Math.Round((GrossTotal), 2), 0, null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //        }
    //    }
    //    else
    //    {
    //        dc.usp_OperationDetails_Insert(Result, AccountCustomer_ID, Math.Round((GrossTotal) * ratio, 2), 0, Math.Round((GrossTotal), 2), 0, null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }


    //    //Discount
    //    if (TotalDiscount > 0)
    //    {
    //        dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, Math.Round(TotalDiscount * ratio, 2), 0, Math.Round(TotalDiscount, 2), 0, null, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }

    //    if (CalculatedSalesCost > 0)
    //    {
    //        dc.usp_SalesCostOperation_Insert(Branch_ID, InvoiceDate, CalculatedSalesCost, Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }
    //    if (Math.Round(Total + Additionals + TaxAmount, 2) != Math.Round(PaidRest, 2))
    //    {
    //        InsertCashIn(PaidRest, AccountCustomer_ID, InvoiceDate, CreatedBy_ID, string.Empty, string.Empty, Branch_ID.ToInt(), DefaultCashAccount_ID, docRandomString, Invoice_ID);
    //    }
    //}



    //private void InsertCashIn(decimal PaidRest, int customer_ID, DateTime OperationDate, int Contact_ID, string UserRefNo, string Notes, int Branch_ID, int cashAccount_ID, string DocRandomString, int Invoice_ID)
    //{
    //    string Serial = string.Empty;
    //    decimal ratio = 1;
    //    int? CashIn_ID = null;
    //    if (PaidRest <= 0) return;
    //    int ContactChartOfAccount_ID = customer_ID; //dc.fun_getContactAccountID(customer_ID).Value;
    //    CashIn_ID = dc.usp_Payments_Insert(OperationDate, DateTime.Now, Contact_ID, Contact_ID, DateTime.Now, UserRefNo, ref Serial, DocSerials.CashIn.ToInt(), Notes, PaidRest, null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), Branch_ID, 1, 15, Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), DocRandomString + "_FromInvoice");
    //    if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
    //    dc.usp_PaymentsDetails_Insert(CashIn_ID, cashAccount_ID, ContactChartOfAccount_ID, PaidRest, null, string.Empty, null);

    //    int Operation_ID = dc.usp_Operation_Insert(Branch_ID, OperationDate, ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), 15, PaidRest * ratio, PaidRest, ratio, null);
    //    dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, PaidRest * ratio, 0, PaidRest, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
    //    dc.usp_OperationDetails_Insert(Operation_ID, cashAccount_ID, PaidRest * ratio, 0, PaidRest, 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
    //    dc.usp_SetCashDocForBills(Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    //}



    //#endregion
}