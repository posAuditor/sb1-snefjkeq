using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Comp_CorrectSystemAccounting : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {

        System.Data.Common.DbTransaction trans = null;

        try
        {
            var listInv = dc.Invoices.Where(x => x.DocStatus_ID == 2).ToList().Select(y => new { Type = "INV", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate , RandomVal = float.Parse(y.DocRandomString.Split('_')[0])});
            var listReceipt = dc.Receipts.Where(x => x.DocStatus_ID == 2).ToList().Select(y => new { Type = "Rec", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate, RandomVal = float.Parse(y.DocRandomString.Split('_')[0]) });
            var listReturnReceipt = dc.ReturnReceipts.Where(x => x.DocStatus_ID == 2).ToList().Select(y => new { Type = "RRec", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate, RandomVal = float.Parse(y.DocRandomString.Split('_')[0]) });
            var listReturnInv = dc.ReturnInvoices.Where(x => x.DocStatus_ID == 2).ToList().Select(y => new { Type = "RINV", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate, RandomVal = float.Parse(y.DocRandomString.Split('_')[0]) });
            var listTransfer = dc.InventoryDocuments.Where(x => x.DocStatus_ID == 2 && x.EntryType == 1).ToList().Select(y => new { Type = "Trans", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate, RandomVal = float.Parse(y.DocRandomString.Split('_')[0]) });
            var listCorrect = dc.InventoryDocuments.Where(x => x.DocStatus_ID == 2 && x.EntryType == 0).ToList().Select(y => new { Type = "Corr", ID = y.ID, OpDate = y.OperationDate, CrDate = y.CreateDate, RandomVal = float.Parse(y.DocRandomString.Split('_')[0]) });
            var lst = listInv.Union(listReceipt).Union(listReturnReceipt).Union(listReturnInv).Union(listTransfer).Union(listCorrect).ToList();

            var listFinal = lst.OrderBy(x => x.OpDate).ThenBy(y => y.RandomVal).ToList();
            foreach (var item in listFinal)
            {
                if (item.Type == "INV")
                {
                    SaveInvoice(true, item.ID);
                }
                else if (item.Type == "Rec")
                {
                    SaveReceipt(true, item.ID);
                }
                else if (item.Type == "RRec")
                {
                    // SaveReturnReceipt(true, item.ID);
                }
                else if (item.Type == "RINV")
                {
                    SaveReturnInvoice(true, item.ID);
                }
                else if (item.Type == "Trans")
                {
                    SaveTransfer(true, item.ID);
                }
                else if (item.Type == "Corr")
                {
                    SaveCorrect(true, item.ID);
                }
            }

        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #region Invoice

    #region Properties



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

    private DataTable dtItems
    {
        get
        {
            if (Session["dtItems_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetailsWithDescription_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_Invoice" + this.WinID];
        }

        set
        {
            Session["dtItems_Invoice" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_Invoice" + this.WinID] == null)
            {
                Session["dtTaxes_Invoice" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, false).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_Invoice" + this.WinID];
        }

        set
        {
            Session["dtTaxes_Invoice" + this.WinID] = value;
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

    private decimal GrossTotal
    {
        get
        {
            if (ViewState["GrossTotal"] == null) return 0;
            return (decimal)ViewState["GrossTotal"];
        }

        set
        {
            ViewState["GrossTotal"] = value;
        }
    }

    private decimal TotalDiscount
    {
        get
        {
            if (ViewState["TotalDiscount"] == null) return 0;
            return (decimal)ViewState["TotalDiscount"];
        }

        set
        {
            ViewState["TotalDiscount"] = value;
        }
    }

    private decimal TotalTax
    {
        get
        {
            if (ViewState["TotalTax"] == null) return 0;
            return (decimal)ViewState["TotalTax"];
        }

        set
        {
            ViewState["TotalTax"] = value;
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

    private int SalesOrderID
    {
        get
        {
            if (ViewState["SalesOrderID"] == null) return 0;
            return (int)ViewState["SalesOrderID"];
        }

        set
        {
            ViewState["SalesOrderID"] = value;
        }
    }

    private decimal TotalDebitTax
    {
        get
        {
            if (ViewState["TotalDebitTax"] == null) return 0;
            return (decimal)ViewState["TotalDebitTax"];
        }

        set
        {
            ViewState["TotalDebitTax"] = value;
        }
    }

    private decimal TotalCreditTax
    {
        get
        {
            if (ViewState["TotalCreditTax"] == null) return 0;
            return (decimal)ViewState["TotalCreditTax"];
        }

        set
        {
            ViewState["TotalCreditTax"] = value;
        }
    }

    private DataTable dtAllTaxes
    {
        get
        {
            if (Session["dtAllTaxes_Invoice" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnInvoiceType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_Invoice" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_Invoice" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Invoice" + this.WinID] = value;
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

    private byte QuantityWarning
    {
        get
        {
            if (ViewState["QuantityWarning"] == null) return 0;
            return (byte)ViewState["QuantityWarning"];
        }

        set
        {
            ViewState["QuantityWarning"] = value;
        }
    }

    private decimal CalculatedSalesCost
    {
        get
        {
            if (ViewState["CalculatedSalesCost"] == null) return 0;
            return (decimal)ViewState["CalculatedSalesCost"];
        }

        set
        {
            ViewState["CalculatedSalesCost"] = value;
        }
    }

    private byte DocStatus_ID
    {
        get
        {
            if (ViewState["DocStatus_ID"] == null) return 0;
            return (byte)ViewState["DocStatus_ID"];
        }

        set
        {
            ViewState["DocStatus_ID"] = value;
        }
    }

    private bool IsCashInvoice
    {
        get
        {
            return Request.PathInfo == "/Cash";
        }
    }

    private bool SumFirstPaid
    {
        get
        {
            if (ViewState["SumFirstPaid"] == null)
            {
                ViewState["SumFirstPaid"] = dc.usp_Company_Select().First().SumFirstPaid;
            }
            return (bool)ViewState["SumFirstPaid"];
        }

        set
        {
            ViewState["SumFirstPaid"] = value;
        }
    }

    #endregion

    private bool SaveInvoice(bool IsApproving, int Invoice_ID)
    {
        //if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}
        //if (this.Total <= 0 || this.GrossTotal < 0)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}



        //if (this.dtItems.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted && x.Field<int?>("Store_ID") == null).Any())
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.StoresRequired, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}


        this.Invoice_ID = Invoice_ID;
        var invoice = dc.usp_Invoice_SelectByID(Invoice_ID).FirstOrDefault();
        string txtRatio = invoice.Ratio.ToExpressString();
        string acBranch = invoice.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = invoice.OperationDate.ToExpressString();
        string acCostCenter = invoice.CostCenter_ID.ToStringOrEmpty();
        string txtUserRefNo = invoice.UserRefNo;
        string acCustomer = invoice.Contact_ID.ToExpressString();
        string acTelephone = invoice.Telephone_ID.ToStringOrEmpty();
        string acAddress = invoice.DefaultAddress_ID.ToStringOrEmpty();
        string acShipAddress = invoice.ShipToAddress_ID.ToStringOrEmpty();
        string acPaymentAddress = invoice.PaymentAddress_ID.ToStringOrEmpty();
        string acCashAccount = invoice.CashAccount_ID.ToStringOrEmpty();
        string acSalesRep = invoice.SalesRep_ID.ToStringOrEmpty();
        string txtCashDiscount = invoice.CashDiscount.ToExpressString();
        string txtAdditionals = invoice.Additionals.ToExpressString();
        string txtPercentageDiscount = invoice.PercentageDiscount.ToExpressString();
        string txtFirstPaid = invoice.FirstPaid.ToExpressString();
        string lblTotal = invoice.TotalAmount.ToExpressString();
        string lblTotalDiscount = invoice.TotalDiscount.ToExpressString();
        string lblTotalTax = invoice.TotalTax.ToExpressString();
        string lblGrossTotal = invoice.GrossTotalAmount.ToExpressString();
        string txtNotes = invoice.Notes;
        if (invoice.SalesOrder_ID.HasValue) this.SalesOrderID = invoice.SalesOrder_ID.Value;

        this.dtItems = dc.usp_InvoiceDetailsWithDescription_Select(this.Invoice_ID).CopyToDataTable();

        this.CalculatedSalesCost = 0;
        this.ConfirmationMessage = string.Empty;
        //if (IsApproving && !this.CheckCreditLmit())
        //{
        //    trans.Rollback();
        //    return false;
        //}

        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal Additionals = 0;
        decimal DocTaxValue = 0;
        this.Total = 0;
        this.GrossTotal = 0;
        this.TotalDiscount = 0;
        this.TotalTax = 0;
        this.TotalDebitTax = 0;
        this.TotalCreditTax = 0;
        this.dtAllTaxes.Rows.Clear();
        this.dtAllTaxes.AcceptChanges();
        foreach (DataRow r in this.dtItems.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax += r["TotalTax"].ToDecimal();
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                this.TotalTax += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotal += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal * txtPercentageDiscount.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.ToDecimalOrDefault();




        //if (acTaxInvoice.HasValue)
        //{
        //    var Tax = dc.usp_Taxes_Select(acTaxInvoice.ToInt(), string.Empty).FirstOrDefault();

        //    DocTaxValue = ((this.GrossTotal - DocDiscount) * Tax.PercentageValue.ToDecimal() * 0.01m);

        //    if (Tax.OnInvoiceType.ToExpressString() == "C")
        //    {
        //        this.dtAllTaxes.Rows.Add(Tax.SalesAccountID.ToInt(), Tax.OnInvoiceType.ToExpressString(), 0,
        //            DocTaxValue);
        //        this.TotalCreditTax += DocTaxValue;
        //    }
        //    else
        //    {
        //        this.dtAllTaxes.Rows.Add(Tax.SalesAccountID, Tax.OnInvoiceType.ToExpressString(), DocTaxValue,
        //            0);
        //        this.TotalDebitTax += DocTaxValue;
        //        DocTaxValue *= -1m;
        //    }
        //    DocTax += DocTaxValue;

        //}

        Additionals = txtAdditionals.ToDecimalOrDefault();
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax + Additionals;
        lblTotal = this.Total.ToString("0.####");
        lblTotalDiscount = this.TotalDiscount.ToString("0.####");
        // lblAdditionals.Text = Additionals.ToString("0.####");
        lblTotalTax = this.TotalTax.ToString("0.####");
        lblGrossTotal = this.GrossTotal.ToString("0.####");


        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;





        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = this.IsCashInvoice ? (byte)2 : (byte)1;
        int Serial_ID = DocSerials.Invoice.ToInt();
        int? SalesOrderID = this.SalesOrderID == 0 ? (int?)null : this.SalesOrderID;
        int Detail_ID = 0;
        //if (!this.EditMode)
        //{

        //    this.Invoice_ID = dc.usp_Invoice_Insert(acBranch.ToNullableInt(), 15, txtRatio.ToDecimalOrDefault(), txtOperationDate.ToDate(),
        //                                             acCustomer.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
        //                                             approvedBY_ID, null, null, acAddress.ToNullableInt(), acShipAddress.ToNullableInt(), acPaymentAddress.ToNullableInt(),
        //                                             acTelephone.ToNullableInt(), acCostCenter.ToNullableInt(), txtNotes, lblTotal.ToDecimal(), txtPercentageDiscount.ToDecimalOrDefault(),
        //                                             txtCashDiscount.ToDecimalOrDefault(), txtAdditionals.ToDecimalOrDefault(), lblTotalDiscount.ToDecimalOrDefault(), lblTotalTax.ToDecimalOrDefault(), lblGrossTotal.ToDecimalOrDefault(), txtFirstPaid.ToDecimalOrDefault(), null, txtUserRefNo, this.DocRandomString, EntryType, SalesOrderID, acSalesRep.ToNullableInt(), acCashAccount.ToNullableInt(), null);
        //    if (this.Invoice_ID > 0)
        //    {
        //        foreach (DataRow r in this.dtItems.Rows)
        //        {
        //            if (r.RowState == DataRowState.Deleted) continue;
        //             Detail_ID = dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString());

        //            if (IsApproving)
        //                if (!this.InsertICJInvoice(Detail_ID, r, txtOperationDate, acBranch))
        //                {
        //                   // trans.Rollback();
        //                    //return false;
        //                }
        //        }
        //        foreach (DataRow r in this.dtTaxes.Rows)
        //        {
        //            if (r.RowState == DataRowState.Deleted) continue;
        //            dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
        //        }
        //        if (IsApproving) this.InsertOperationInvoice(txtAdditionals, acCustomer, acBranch, txtOperationDate,
        //                                              txtNotes, acCostCenter, txtFirstPaid, acCashAccount, txtUserRefNo);
        //        LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
        //    }
        //}
        //else
        //{
        //int Result = dc.usp_Invoice_Update(this.Invoice_ID, acBranch.ToNullableInt(), 15, txtRatio.ToDecimalOrDefault(), txtOperationDate.ToDate(),
        //                                        acCustomer.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
        //                                        null, null, acAddress.ToNullableInt(), acShipAddress.ToNullableInt(), acPaymentAddress.ToNullableInt(), acTelephone.ToNullableInt(),
        //                                        acCostCenter.ToNullableInt(), txtNotes, lblTotal.ToDecimal(), txtPercentageDiscount.ToDecimalOrDefault(), txtCashDiscount.ToDecimalOrDefault(), txtAdditionals.ToDecimalOrDefault(),
        //                                        lblTotalDiscount.ToDecimalOrDefault(), lblTotalTax.ToDecimalOrDefault(), lblGrossTotal.ToDecimalOrDefault(), txtFirstPaid.ToDecimalOrDefault(), txtUserRefNo, this.DocRandomString, acSalesRep.ToNullableInt(), acCashAccount.ToNullableInt());
        //if (Result > 0)
        //{
            foreach (DataRow r in this.dtItems.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    // Detail_ID = dc.usp_InvoiceDetails_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                //    Detail_ID = dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString());

                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_InvoiceDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_InvoiceDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}
                if (IsApproving && r.RowState != DataRowState.Deleted)
                {
                    Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                    if (!this.InsertICJInvoice(Detail_ID, r, txtOperationDate, acBranch))
                    {
                        //trans.Rollback();
                        //return false;
                    }
                }
            }

            //foreach (DataRow r in this.dtTaxes.Rows)
            //{
            //    if (r.RowState == DataRowState.Added)
            //    {
            //        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
            //    }
            //    if (r.RowState == DataRowState.Deleted)
            //    {
            //        dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
            //    }
            //}
            if (IsApproving) this.InsertOperationInvoice(txtAdditionals, acCustomer, acBranch, txtOperationDate,
                                                  txtNotes, acCostCenter, txtFirstPaid, acCashAccount, txtUserRefNo);
            // LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
       // }
        // }
        if (this.ConfirmationMessage != string.Empty)
        {
            // ltConfirmationMessage.Text = this.ConfirmationMessage;
            // mpeConfirm.Show();
            // trans.Rollback();
            //return false;
        }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        // UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InvoiceShortcut + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut + Request.PathInfo);
        return true;
    }

    private void InsertOperationInvoice(string txtAdditionals, string acCustomer,
        string acBranch, string txtOperationDate,
        string txtNotes, string acCostCenter, string txtFirstPaid,
        string acCashAccount, string txtUserRefNo)
    {
        decimal ratio = 1;
        decimal Additionals = txtAdditionals.ToDecimalOrDefault();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acCustomer.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), 15, (this.Total + this.TotalCreditTax + Additionals) * ratio, (this.Total + this.TotalCreditTax + Additionals), ratio, txtNotes);

        //ايراد المبيعات
        dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, this.Total * ratio, 0, this.Total, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

        //ايراد الاضافات
        if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, Additionals * ratio, 0, Additionals, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


        //Customer
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

        //Discount
        if (this.TotalDiscount > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }

        if (this.CalculatedSalesCost > 0)
        {
            dc.usp_SalesCostOperation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), this.CalculatedSalesCost, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }

        //CostCenter Debit
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), this.CalculatedSalesCost + (this.TotalDiscount * ratio), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes);
        //CostCenter Credit
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), (this.Total + Additionals) * ratio * -1, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes);
        //this.InsertCashInInvoice(txtFirstPaid, acCustomer, txtOperationDate, txtNotes, acCashAccount, acBranch, txtUserRefNo);
    }

    private bool InsertICJInvoice(int Detail_ID, DataRow row, string txtOperationDate, string acBranch)
    {
        decimal? SalesCost = 0;
        int result = dc.usp_ICJ_Invoice(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * 1, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, Detail_ID, this.SalesOrderID, ref SalesCost);
        this.CalculatedSalesCost += SalesCost.Value;

        if ((result & ICJStoredProcFlags.QtyReserved.ToInt()) == ICJStoredProcFlags.QtyReserved.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }
        if ((result & ICJStoredProcFlags.BatchOrderWarning.ToInt()) == ICJStoredProcFlags.BatchOrderWarning.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.BatchOrderWarning + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.BatchQtyNotEnough.ToInt()) == ICJStoredProcFlags.BatchQtyNotEnough.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.BatchQtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt() && !this.ConfirmationAnswered)
        {
            this.ConfirmationMessage += "<br>\u2022 " + Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")";
        }
        if ((result & ICJStoredProcFlags.PriceLessThanCost.ToInt()) == ICJStoredProcFlags.PriceLessThanCost.ToInt() && !this.ConfirmationAnswered)
        {
            this.ConfirmationMessage += "<br><span style='color:red'> \u2022 " + Resources.UserInfoMessages.PriceLessThanCost + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")</span>";
        }
        return true;
    }

    private void InsertCashInInvoice(string txtFirstPaid, string acCustomer,
        string txtOperationDate, string txtNotes,
        string acCashAccount, string acBranch,
        string txtUserRefNo)
    {
        string Serial = string.Empty;
        decimal ratio = 1;
        int? CashIn_ID = null;
        if (txtFirstPaid.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.ToInt()).Value;
       // CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo, ref Serial, DocSerials.CashIn.ToInt(), txtNotes, txtFirstPaid.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.ToNullableInt(), 1, 15, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
       CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo, ref Serial, DocSerials.CashIn.ToInt(), txtNotes, txtFirstPaid.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.ToNullableInt(), 1, 15, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
      
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, acCashAccount.ToInt(), ContactChartOfAccount_ID, txtFirstPaid.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), 15, txtFirstPaid.ToDecimal() * ratio, txtFirstPaid.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.ToInt(), txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    }
    #endregion

    #region Receipt


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

    private bool EditModeReceipt
    {
        get
        {
            if (ViewState["EditModeReceipt"] == null) return false;
            return (bool)ViewState["EditModeReceipt"];
        }

        set
        {
            ViewState["EditModeReceipt"] = value;
        }
    }

    private DataTable dtItems_Receipt
    {
        get
        {
            if (Session["dtItems_Receipt" + this.WinID] == null)
            {
                //Session["dtItems_Receipt" + this.WinID] = dc.usp_ReceiptDetails_Select(null).CopyToDataTable();
                Session["dtItems_Receipt" + this.WinID] = dc.usp_ReceiptDetailsWithDescription_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_Receipt" + this.WinID];
        }

        set
        {
            Session["dtItems_Receipt" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes_Receipt
    {
        get
        {
            if (Session["dtTaxes_Receipt" + this.WinID] == null)
            {
                Session["dtTaxes_Receipt" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, false).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_Receipt" + this.WinID];
        }

        set
        {
            Session["dtTaxes_Receipt" + this.WinID] = value;
        }
    }

    private int EditID_Receipt
    {
        get
        {
            if (ViewState["EditID_Receipt"] == null) return 0;
            return (int)ViewState["EditID_Receipt"];
        }

        set
        {
            ViewState["EditID"] = value;
        }
    }

    private string DocRandomStringReceipt
    {
        get
        {
            if (ViewState["DocRandomStringReceipt"] == null)
            {
                ViewState["DocRandomStringReceipt"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomStringReceipt"];
        }

        set
        {
            ViewState["DocRandomStringReceipt"] = value;
        }
    }

    private int PurchaseOrderID
    {
        get
        {
            if (ViewState["PurchaseOrderID"] == null) return 0;
            return (int)ViewState["PurchaseOrderID"];
        }

        set
        {
            ViewState["PurchaseOrderID"] = value;
        }
    }

    private decimal TotalReceipt
    {
        get
        {
            if (ViewState["TotalReceipt"] == null) return 0;
            return (decimal)ViewState["TotalReceipt"];
        }

        set
        {
            ViewState["TotalReceipt"] = value;
        }
    }

    private decimal GrossTotalReceipt
    {
        get
        {
            if (ViewState["GrossTotalReceipt"] == null) return 0;
            return (decimal)ViewState["GrossTotalReceipt"];
        }

        set
        {
            ViewState["GrossTotalReceipt"] = value;
        }
    }

    private decimal TotalReceiptDiscount
    {
        get
        {
            if (ViewState["TotalReceiptDiscount"] == null) return 0;
            return (decimal)ViewState["TotalReceiptDiscount"];
        }

        set
        {
            ViewState["TotalReceiptDiscount"] = value;
        }
    }

    private decimal TotalReceiptTax
    {
        get
        {
            if (ViewState["TotalReceiptTax"] == null) return 0;
            return (decimal)ViewState["TotalReceiptTax"];
        }

        set
        {
            ViewState["TotalReceiptTax"] = value;
        }
    }

    private decimal TotalReceiptDebitTax
    {
        get
        {
            if (ViewState["TotalReceiptDebitTax"] == null) return 0;
            return (decimal)ViewState["TotalReceiptDebitTax"];
        }

        set
        {
            ViewState["TotalReceiptDebitTax"] = value;
        }
    }

    private decimal TotalReceiptCreditTax
    {
        get
        {
            if (ViewState["TotalReceiptCreditTax"] == null) return 0;
            return (decimal)ViewState["TotalReceiptCreditTax"];
        }

        set
        {
            ViewState["TotalReceiptCreditTax"] = value;
        }
    }


    private bool SumFirstPaidReceipt
    {
        get
        {
            if (ViewState["SumFirstPaidReceipt"] == null)
            {
                ViewState["SumFirstPaidReceipt"] = dc.usp_Company_Select().First().SumFirstPaid;
            }
            return (bool)ViewState["SumFirstPaidReceipt"];
        }

        set
        {
            ViewState["SumFirstPaidReceipt"] = value;
        }
    }

    private decimal TotalReceiptServices
    {
        get
        {
            if (ViewState["TotalReceiptServices"] == null) return 0;
            return (decimal)ViewState["TotalReceiptServices"];
        }

        set
        {
            ViewState["TotalReceiptServices"] = value;
        }
    }

    #endregion


    private bool SaveReceipt(bool IsApproving, int Receipt_ID)
    {
        this.Receipt_ID = Receipt_ID;

        var receipt = dc.usp_Receipt_SelectByID(this.Receipt_ID).FirstOrDefault();

        string txtRatio = receipt.Ratio.ToExpressString();
        string acBranch = receipt.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = receipt.OperationDate.Value.ToString("d/M/yyyy");
        string acCostCenter = receipt.CostCenter_ID.ToStringOrEmpty();
        string txtUserRefNo = receipt.UserRefNo;
        string acVendor = receipt.Contact_ID.ToExpressString();
        string acTelephone = receipt.Telephone_ID.ToStringOrEmpty();
        string acAddress = receipt.DefaultAddress_ID.ToStringOrEmpty();
        string acShipAddress = receipt.ShipToAddress_ID.ToStringOrEmpty();
        string acPaymentAddress = receipt.PaymentAddress_ID.ToStringOrEmpty();
        string acCashAccount = receipt.CashAccount_ID.ToStringOrEmpty();
        string txtCashDiscount = receipt.CashDiscount.ToExpressString();
        string txtPercentageDiscount = receipt.PercentageDiscount.ToExpressString();
        string txtFirstPaid = receipt.FirstPaid.ToExpressString();
        string lblTotalReceipt = receipt.TotalAmount.ToExpressString();
        string lblTotalReceiptDiscount = receipt.TotalDiscount.ToExpressString();
        string lblTotalReceiptTax = receipt.TotalTax.ToExpressString();
        string lblGrossTotalReceipt = receipt.GrossTotalAmount.ToExpressString();
        string txtNotes = receipt.Notes;
        string txtTaxAmount = receipt.TotalTax.ToExpressString();
        string txtTaxParcent = string.Empty;
        if (receipt.TotalTax > 0)
        {
            txtTaxAmount = receipt.TotalTax.ToExpressString();
            txtTaxParcent = ((receipt.TotalTax / (receipt.TotalAmount - receipt.TotalDiscount)) * 100).ToExpressString();
        }



        this.DocRandomStringReceipt = receipt.DocRandomString;
        string lblCreatedBy = receipt.CreatedByName;
        string lblApprovedBy = receipt.ApprovedBYName;



        this.dtItems_Receipt = dc.usp_ReceiptDetailsWithDescription_Select(this.Receipt_ID).CopyToDataTable();



        this.TotalReceiptServices = 0;
        string Serial = string.Empty;



        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal DocTaxValue = 0;
        this.TotalReceipt = 0;
        this.GrossTotalReceipt = 0;
        this.TotalReceiptDiscount = 0;
        this.TotalReceiptTax = 0;
        this.TotalReceiptDebitTax = 0;
        this.TotalReceiptCreditTax = 0;
        this.dtAllTaxes.Rows.Clear();
        this.dtAllTaxes.AcceptChanges();
        foreach (DataRow r in this.dtItems.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            TotalReceipt += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalReceiptDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnReceiptType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalReceiptCreditTax += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalReceiptDebitTax += r["TotalTax"].ToDecimal();
                }
                this.TotalReceiptTax += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotalReceipt += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotalReceipt * txtPercentageDiscount.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.ToDecimalOrDefault();

        this.TotalReceiptDiscount += DocDiscount;
        this.TotalReceiptTax += DocTax;
        this.GrossTotalReceipt = this.GrossTotalReceipt - DocDiscount + DocTax;
        lblTotalReceipt = this.TotalReceipt.ToString("0.####");
        lblTotalReceiptDiscount = this.TotalReceiptDiscount.ToString("0.####");
        lblTotalReceiptTax = this.TotalReceiptTax.ToString("0.####");
        lblGrossTotalReceipt = this.GrossTotalReceipt.ToString("0.####");

        // txtFirstPaid = lblGrossTotalReceipt;















        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 1;
        int Serial_ID = DocSerials.Receipt.ToInt();
        int? PurchaseOrderID = this.PurchaseOrderID == 0 ? (int?)null : this.PurchaseOrderID;
        int Detail_ID = 0;


        //int Result = dc.usp_Receipt_Update(this.Receipt_ID, acBranch.ToNullableInt(), 15, txtRatio.ToDecimalOrDefault(), txtOperationDate.ToDate(),
        //                                        acVendor.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
        //                                        null, null, acAddress.ToNullableInt(), acShipAddress.ToNullableInt(), acPaymentAddress.ToNullableInt(), acTelephone.ToNullableInt(),
        //                                        acCostCenter.ToNullableInt(), txtNotes, lblTotalReceipt.ToDecimal(), txtPercentageDiscount.ToDecimalOrDefault(), txtCashDiscount.ToDecimalOrDefault(),
        //                                        lblTotalReceiptDiscount.ToDecimalOrDefault(), lblTotalReceiptTax.ToDecimalOrDefault(), lblGrossTotalReceipt.ToDecimalOrDefault(), txtFirstPaid.ToDecimalOrDefault(), txtUserRefNo, this.DocRandomStringReceipt, acCashAccount.ToNullableInt());
        //if (Result > 0)
        //{


            foreach (DataRow r in this.dtItems_Receipt.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    //Detail_ID = dc.usp_ReceiptDetails_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalReceiptTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                //    Detail_ID = dc.usp_ReceiptDetailsWithDesciption_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalReceiptTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["ReceiptDate"].ToDate(), r["Capacities"].ToString());
                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_ReceiptDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalReceiptTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_ReceiptDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}
                if (IsApproving && r.RowState != DataRowState.Deleted)
                {
                    Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                    if (!this.InsertICJReceipt(Detail_ID, r, txtOperationDate, acBranch))
                    {

                    }
                }
            }

            //foreach (DataRow r in this.dtTaxes_Receipt.Rows)
            //{
            //    if (r.RowState == DataRowState.Added)
            //    {
            //        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID, r["Tax_ID"].ToInt());
            //    }
            //    if (r.RowState == DataRowState.Deleted)
            //    {
            //        dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
            //    }
            //}
            if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.ToDate(), acBranch.ToNullableInt(), DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID);
            if (IsApproving) this.InsertOperationReceipt(Detail_ID, txtOperationDate, acBranch,
         acVendor, acCostCenter, txtNotes, txtFirstPaid, acCashAccount, txtUserRefNo);

       // }

        //Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        //UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReceiptShortcut + "?ID=" + this.Receipt_ID.ToExpressString(), PageLinks.ReceiptsList, PageLinks.ReceiptShortcut);
        return true;
    }

    private void InsertOperationReceipt(int Detail_ID, string txtOperationDate, string acBranch,
        string acVendor, string acCostCenter, string txtNotes, string txtFirstPaid, string acCashAccount, string txtUserRefNo)
    {
        decimal ratio = 1;
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acVendor.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Purchases.ToInt(), 15, (this.TotalReceipt + this.TotalReceiptDebitTax) * ratio, (this.TotalReceipt + this.TotalReceiptDebitTax), ratio, txtNotes);

        //Vendor
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotalReceipt) * ratio, 0, (this.GrossTotalReceipt), null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());

        //Inventory
        dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, (this.TotalReceipt - this.TotalReceiptServices) * ratio, 0, (this.TotalReceipt - this.TotalReceiptServices), 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());

        //Discount
        if (this.TotalReceiptDiscount > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.PurchaseDiscountAccountID, 0, this.TotalReceiptDiscount * ratio, 0, this.TotalReceiptDiscount, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());
        }

        //Services
        if (this.TotalReceiptServices > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.ServicesExpAccount_ID, this.TotalReceiptServices * ratio, 0, this.TotalReceiptServices, 0, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnReceiptType = tax.Field<string>("OnReceiptType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt());
        }

        //CostCenter 
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), this.GrossTotalReceipt * ratio, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt(), txtNotes);
        //this.InsertCashOut(txtFirstPaid, txtOperationDate, acVendor, acCashAccount, acBranch, txtUserRefNo, txtNotes);
    }

    private bool InsertICJReceipt(int Detail_ID, DataRow row, string txtOperationDate, string acBranch)
    {
        int result = 0;
        bool? IsService = false;
        decimal ItemTotalReceipt = row["Total"].ToDecimal() * 1;
        result = dc.usp_ICJ_Receipt(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), ItemTotalReceipt, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID, Detail_ID, ref IsService);
        if (IsService.Value) this.TotalReceiptServices += row["Total"].ToDecimal();

        return true;
    }

    private void InsertCashOut(string txtFirstPaid, string txtOperationDate,
                               string acVendor, string acCashAccount,
                               string acBranch, string txtUserRefNo, string txtNotes)
    {
        string Serial = string.Empty;
        decimal ratio = 1;
        int? CashOut_ID = null;
        if (txtFirstPaid.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acVendor.ToInt()).Value;
        CashOut_ID = dc.usp_Payments_Insert(txtOperationDate.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo, ref Serial, DocSerials.CashOut.ToInt(), txtNotes, txtFirstPaid.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashOutVendor.ToByte(), acBranch.ToNullableInt(), 1, 15, this.Receipt_ID, DocumentsTableTypes.Receipt.ToInt(), this.DocRandomStringReceipt + "_FromReturnInvoice");

        if (!CashOut_ID.HasValue || CashOut_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");

        dc.usp_PaymentsDetails_Insert(CashOut_ID, ContactChartOfAccount_ID, acCashAccount.ToInt(), txtFirstPaid.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashOut.ToInt(), 15, txtFirstPaid.ToDecimal() * ratio, txtFirstPaid.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.ToInt(), 0, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), 0, null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_SetCashDocForBills(this.Receipt_ID, CashOut_ID, DocumentsTableTypes.Receipt.ToInt());
    }
    #endregion

    #region ReturnReceipt
    #region Properties

    private int ReturnReceipt_ID
    {
        get
        {
            if (ViewState["ReturnReceipt_ID"] == null) return 0;
            return (int)ViewState["ReturnReceipt_ID"];
        }

        set
        {
            ViewState["ReturnReceipt_ID"] = value;
        }
    }



    private DataTable dtItems_ReturnReceipt
    {
        get
        {
            if (Session["dtItems_ReturnReceipt" + this.WinID] == null)
            {
                Session["dtItems_ReturnReceipt" + this.WinID] = dc.usp_ReturnReceiptDetails_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtItems_ReturnReceipt" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes_ReturnReceipt
    {
        get
        {
            if (Session["dtTaxes_ReturnReceipt" + this.WinID] == null)
            {
                Session["dtTaxes_ReturnReceipt" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, true).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtTaxes_ReturnReceipt" + this.WinID] = value;
        }
    }



    private string DocRandomString_ReturnReceipt
    {
        get
        {
            if (ViewState["DocRandomString_ReturnReceipt"] == null)
            {
                ViewState["DocRandomString_ReturnReceipt"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString_ReturnReceipt"];
        }

        set
        {
            ViewState["DocRandomString_ReturnReceipt"] = value;
        }
    }

    private int? FromReceipt_ID
    {
        get
        {
            return (int?)ViewState["FromReceipt_ID"];
        }

        set
        {
            ViewState["FromReceipt_ID"] = value;
        }
    }

    private decimal Total_ReturnReceipt
    {
        get
        {
            if (ViewState["Total_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["Total_ReturnReceipt"];
        }

        set
        {
            ViewState["Total_ReturnReceipt"] = value;
        }
    }

    private decimal GrossTotal_ReturnReceipt
    {
        get
        {
            if (ViewState["GrossTotal_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["GrossTotal_ReturnReceipt"];
        }

        set
        {
            ViewState["GrossTotal_ReturnReceipt"] = value;
        }
    }

    private decimal TotalDiscount_ReturnReceipt
    {
        get
        {
            if (ViewState["TotalDiscount_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["TotalDiscount_ReturnReceipt"];
        }

        set
        {
            ViewState["TotalDiscount_ReturnReceipt"] = value;
        }
    }

    private decimal TotalTax_ReturnReceipt
    {
        get
        {
            if (ViewState["TotalTax_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["TotalTax_ReturnReceipt"];
        }

        set
        {
            ViewState["TotalTax_ReturnReceipt"] = value;
        }
    }

    private decimal TotalDebitTax_ReturnReceipt
    {
        get
        {
            if (ViewState["TotalDebitTax_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["TotalDebitTax_ReturnReceipt"];
        }

        set
        {
            ViewState["TotalDebitTax_ReturnReceipt"] = value;
        }
    }

    private decimal TotalCreditTax_ReturnReceipt
    {
        get
        {
            if (ViewState["TotalCreditTax_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["TotalCreditTax_ReturnReceipt"];
        }

        set
        {
            ViewState["TotalCreditTax"] = value;
        }
    }

    private DataTable dtAllTaxes_ReturnReceipt
    {
        get
        {
            if (Session["dtAllTaxes_ReturnReceipt" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnReceiptType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_ReturnReceipt" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_ReturnReceipt" + this.WinID] = value;
        }
    }

    private bool SumFirstPaid_ReturnReceipt
    {
        get
        {
            if (ViewState["SumFirstPaid_ReturnReceipt"] == null)
            {
                ViewState["SumFirstPaid_ReturnReceipt"] = dc.usp_Company_Select().First().SumFirstPaid;
            }
            return (bool)ViewState["SumFirstPaid_ReturnReceipt"];
        }

        set
        {
            ViewState["SumFirstPaid_ReturnReceipt"] = value;
        }
    }

    private decimal TotalServices_ReturnReceipt
    {
        get
        {
            if (ViewState["TotalServices_ReturnReceipt"] == null) return 0;
            return (decimal)ViewState["TotalServices_ReturnReceipt"];
        }

        set
        {
            ViewState["TotalServices_ReturnReceipt"] = value;
        }
    }

    #endregion

    private bool SaveReturnReceipt(bool IsApproving, int ID)
    {
        //if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}
        //if (this.Total <= 0 || this.GrossTotal < 0)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}

        //if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}

        //if (txtFirstPaid.Text.ToDecimalOrDefault() > this.GrossTotal)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}


        this.ReturnReceipt_ID = ID;
        var receipt = dc.usp_ReturnReceipt_SelectByID(this.ReturnReceipt_ID).FirstOrDefault();
        //var receipt = dc.usp_ReturnReceipt_SelectByID(this.ReturnReceipt_ID).FirstOrDefault();

        string acBranch = receipt.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = receipt.OperationDate.Value.ToString("d/M/yyyy");
        string acCostCenter = receipt.CostCenter_ID.ToStringOrEmpty();
        string txtUserRefNo = receipt.UserRefNo;
        string acVendor = receipt.Contact_ID.ToExpressString();
        string acTelephone = receipt.Telephone_ID.ToStringOrEmpty();
        string acAddress = receipt.DefaultAddress_ID.ToStringOrEmpty();
        string acShipAddress = receipt.ShipToAddress_ID.ToStringOrEmpty();
        string acPaymentAddress = receipt.PaymentAddress_ID.ToStringOrEmpty();
        string acCashAccount = receipt.CashAccount_ID.ToStringOrEmpty();
        string txtCashDiscount = receipt.CashDiscount.ToExpressString();
        string txtPercentageDiscount = receipt.PercentageDiscount.ToExpressString();
        string txtFirstPaid = receipt.FirstPaid.ToExpressString();
        string lblTotal = receipt.TotalAmount.ToExpressString();
        string lblTotalDiscount = receipt.TotalDiscount.ToExpressString();
        string lblTotalTax = receipt.TotalTax.ToExpressString();
        string lblGrossTotal = receipt.GrossTotalAmount.ToExpressString();
        string txtNotes = receipt.Notes;




        // this.dtItems_ReturnReceipt = dc.usp_ReceiptDetailsForReturn_Select(this.ReturnReceipt_ID).CopyToDataTable();
        this.dtItems_ReturnReceipt = dc.usp_ReturnReceiptDetails_Select(this.ReturnReceipt_ID).CopyToDataTable();


        this.CalculateReturnReceipt(txtPercentageDiscount, txtCashDiscount);




        this.TotalServices_ReturnReceipt = 0;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        int Serial_ID = DocSerials.ReturnReceipt.ToInt();

        int Detail_ID = 0;

        //int Result = dc.usp_ReturnReceipt_Update(this.ReturnReceipt_ID, acBranch.ToNullableInt(), 15, 1, txtOperationDate.ToDate(),
        //                                        acVendor.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
        //                                        null, null, acAddress.ToNullableInt(), acShipAddress.ToNullableInt(), acPaymentAddress.ToNullableInt(), acTelephone.ToNullableInt(),
        //                                        acCostCenter.ToNullableInt(), txtNotes, lblTotal.ToDecimal(), txtPercentageDiscount.ToDecimalOrDefault(), txtCashDiscount.ToDecimalOrDefault(),
        //                                        lblTotalDiscount.ToDecimalOrDefault(), lblTotalTax.ToDecimalOrDefault(), lblGrossTotal.ToDecimalOrDefault(), txtFirstPaid.ToDecimalOrDefault(), txtUserRefNo, this.DocRandomString_ReturnReceipt, acCashAccount.ToNullableInt());
        //if (Result >= 0)
      //  {
            foreach (DataRow r in this.dtItems_ReturnReceipt.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    Detail_ID = dc.usp_ReturnReceiptDetails_Insert(this.ReturnReceipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ReceiptDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_ReturnReceiptDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_ReturnReceiptDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}
                if (IsApproving && r.RowState != DataRowState.Deleted)
                {
                    if (!this.InsertICJReturnReceipt(Detail_ID, r, txtOperationDate, acBranch))
                    {
                        Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                    }
                }
            }

            //foreach (DataRow r in this.dtTaxes_ReturnReceipt.Rows)
            //{
            //    if (r.RowState == DataRowState.Added)
            //    {
            //        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, r["Tax_ID"].ToInt());
            //    }
            //    if (r.RowState == DataRowState.Deleted)
            //    {
            //        dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
            //    }
            //}
            if (IsApproving) this.InsertOperationReturnReceipt(acVendor, acBranch, txtOperationDate, txtNotes, acCostCenter, txtFirstPaid, acCashAccount, txtUserRefNo);

       // }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        // UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReturnReceipt + "?ID=" + this.ReturnReceipt_ID.ToExpressString(), PageLinks.ReturnReceiptsList, PageLinks.ReturnReceipt);
        return true;
    }

    private void InsertOperationReturnReceipt(string acVendor, string acBranch, string txtOperationDate, string txtNotes, string acCostCenter, string txtFirstPaid, string acCashAccount, string txtUserRefNo)
    {
        decimal ratio = 1;
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acVendor.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.RetrunPurchases.ToInt(), 15, (this.Total_ReturnReceipt + this.TotalCreditTax_ReturnReceipt) * ratio, (this.Total_ReturnReceipt + this.TotalCreditTax_ReturnReceipt), ratio, txtNotes);

        //Inventory
        dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, (this.Total_ReturnReceipt - this.TotalServices_ReturnReceipt) * ratio, 0, (this.Total_ReturnReceipt - this.TotalServices_ReturnReceipt), null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        //Vendor
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal_ReturnReceipt) * ratio, 0, (this.GrossTotal_ReturnReceipt), 0, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        //Discount
        if (this.TotalDiscount_ReturnReceipt > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.PurchaseDiscountAccountID, this.TotalDiscount_ReturnReceipt * ratio, 0, this.TotalDiscount_ReturnReceipt, 0, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
        }

        //Services
        if (this.TotalServices_ReturnReceipt > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.ServicesExpAccount_ID, 0, this.TotalServices_ReturnReceipt * ratio, 0, this.TotalServices_ReturnReceipt, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes_ReturnReceipt.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnReceiptType = tax.Field<string>("OnReceiptType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        }

        //CostCenter
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), this.GrossTotal_ReturnReceipt * ratio, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt(), txtNotes);
        this.InsertCashInReturnReceipt(txtFirstPaid, acBranch, txtOperationDate, acVendor, acCashAccount, txtUserRefNo, txtNotes);
    }

    private bool InsertICJReturnReceipt(int Detail_ID, DataRow row, string txtOperationDate, string acBranch)
    {
        int result = 0;
        bool? IsService = false;
        result = dc.usp_ICJ_ReturnReceipt(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * 1, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, Detail_ID, DocumentsTableTypes.Receipt.ToInt(), row["ReceiptDetail_ID"].ToNullableInt(), ref IsService);
        if (IsService.Value) this.TotalServices_ReturnReceipt += row["Total"].ToDecimal();

        if (result == -32)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -5)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantReturnMoreOriginal + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -4)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        return true;
    }
    private void CalculateReturnReceipt(string txtPercentageDiscount, string txtCashDiscount)
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal DocTaxValue = 0;
        this.Total_ReturnReceipt = 0;
        this.GrossTotal_ReturnReceipt = 0;
        this.TotalDiscount_ReturnReceipt = 0;
        this.TotalTax_ReturnReceipt = 0;
        this.TotalDebitTax_ReturnReceipt = 0;
        this.TotalCreditTax_ReturnReceipt = 0;
        this.dtAllTaxes_ReturnReceipt.Rows.Clear();
        this.dtAllTaxes_ReturnReceipt.AcceptChanges();
        foreach (DataRow r in this.dtItems_ReturnReceipt.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total_ReturnReceipt += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount_ReturnReceipt += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnReceiptType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnReceipt.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax_ReturnReceipt += r["TotalTax"].ToDecimal();
                }
                else
                {
                    this.dtAllTaxes_ReturnReceipt.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax_ReturnReceipt += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                this.TotalTax_ReturnReceipt += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotal_ReturnReceipt += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal_ReturnReceipt * txtPercentageDiscount.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.ToDecimalOrDefault();
        foreach (DataRow r in this.dtTaxes_ReturnReceipt.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (r["OnReceiptType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal_ReturnReceipt - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnReceipt.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax_ReturnReceipt += DocTaxValue;
                }
                else
                {
                    this.dtAllTaxes_ReturnReceipt.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax_ReturnReceipt += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                DocTax += DocTaxValue;
            }
        }
        this.TotalDiscount_ReturnReceipt += DocDiscount;
        this.TotalTax_ReturnReceipt += DocTax;
        this.GrossTotal_ReturnReceipt = this.GrossTotal_ReturnReceipt - DocDiscount + DocTax;
        string lblTotal = this.Total_ReturnReceipt.ToString("0.####");
        string lblTotalDiscount = this.TotalDiscount_ReturnReceipt.ToString("0.####");
        string lblTotalTax = this.TotalTax_ReturnReceipt.ToString("0.####");
        string lblGrossTotal = this.GrossTotal_ReturnReceipt.ToString("0.####");
        // if (acCashAccount && this.SumFirstPaid)
        //{
        //    txtFirstPaid = lblGrossTotal;

        //}
    }
    private void InsertCashInReturnReceipt(string txtFirstPaid, string acBranch, string txtOperationDate, string acVendor, string acCashAccount, string txtUserRefNo, string txtNotes)
    {
        string Serial = string.Empty;
        decimal ratio = 1;
        int? CashIn_ID = null;
        if (txtFirstPaid.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acVendor.ToInt()).Value;
        CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo, ref Serial, DocSerials.CashIn.ToInt(), txtNotes, txtFirstPaid.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashIn.ToByte(), acBranch.ToNullableInt(), 1, 15, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt(), this.DocRandomString_ReturnReceipt + "_FromInvoice");
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, acCashAccount.ToInt(), ContactChartOfAccount_ID, txtFirstPaid.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), 15, txtFirstPaid.ToDecimal() * ratio, txtFirstPaid.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.ToInt(), txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.ReturnReceipt_ID, CashIn_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
    }
    #endregion

    #region ReturnInvoice


    #region Properties

    private int ReturnInvoice_ID
    {
        get
        {
            if (ViewState["ReturnInvoice_ID"] == null) return 0;
            return (int)ViewState["ReturnInvoice_ID"];
        }

        set
        {
            ViewState["ReturnInvoice_ID"] = value;
        }
    }



    private DataTable dtItems_ReturnInvoice
    {
        get
        {
            if (Session["dtItems_ReturnInvoice" + this.WinID] == null)
            {
                Session["dtItems_ReturnInvoice" + this.WinID] = dc.usp_ReturnInvoiceDetails_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtItems_ReturnInvoice" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes_ReturnInvoice
    {
        get
        {
            if (Session["dtTaxes_ReturnInvoice" + this.WinID] == null)
            {
                Session["dtTaxes_ReturnInvoice" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, true).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtTaxes_ReturnInvoice" + this.WinID] = value;
        }
    }



    private string DocRandomString_ReturnInvoice
    {
        get
        {
            if (ViewState["DocRandomString_ReturnInvoice"] == null)
            {
                ViewState["DocRandomString_ReturnInvoice"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString_ReturnInvoice"];
        }

        set
        {
            ViewState["DocRandomString-ReturnInvoice"] = value;
        }
    }

    private int? FromInvoice_IDRTIn
    {
        get
        {
            return (int?)ViewState["FromInvoice_IDRTIn"];
        }

        set
        {
            ViewState["FromInvoice_IDRTIn"] = value;
        }
    }

    private decimal Total_ReturnInvoice
    {
        get
        {
            if (ViewState["Total_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["Total_ReturnInvoice"];
        }

        set
        {
            ViewState["Total_ReturnInvoice"] = value;
        }
    }

    private decimal GrossTotal_ReturnInvoice
    {
        get
        {
            if (ViewState["GrossTotal_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["GrossTotal_ReturnInvoice"];
        }

        set
        {
            ViewState["GrossTotal_ReturnInvoice"] = value;
        }
    }

    private decimal TotalDiscount_ReturnInvoice
    {
        get
        {
            if (ViewState["TotalDiscount_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["TotalDiscount_ReturnInvoice"];
        }

        set
        {
            ViewState["TotalDiscount_ReturnInvoice"] = value;
        }
    }

    private decimal TotalTax_ReturnInvoice
    {
        get
        {
            if (ViewState["TotalTax_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["TotalTax_ReturnInvoice"];
        }

        set
        {
            ViewState["TotalTax_ReturnInvoice"] = value;
        }
    }

    private decimal TotalDebitTax_ReturnInvoice
    {
        get
        {
            if (ViewState["TotalDebitTax_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["TotalDebitTax_ReturnInvoice"];
        }

        set
        {
            ViewState["TotalDebitTax_ReturnInvoice"] = value;
        }
    }

    private decimal TotalCreditTax_ReturnInvoice
    {
        get
        {
            if (ViewState["TotalCreditTax_ReturnInvoice"] == null) return 0;
            return (decimal)ViewState["TotalCreditTax_ReturnInvoice"];
        }

        set
        {
            ViewState["TotalCreditTax_ReturnInvoice"] = value;
        }
    }

    private DataTable dtAllTaxes_ReturnInvoice
    {
        get
        {
            if (Session["dtAllTaxes_ReturnInvoice" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnInvoiceType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_ReturnInvoice" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_ReturnInvoice" + this.WinID] = value;
        }
    }

    private decimal CalculatedReturnSalesCost_ReturnInvoice
    {
        get
        {
            if (ViewState["CalculatedReturnSalesCost"] == null) return 0;
            return (decimal)ViewState["CalculatedReturnSalesCost"];
        }

        set
        {
            ViewState["CalculatedReturnSalesCost"] = value;
        }
    }

    private bool ConfirmationAnswered_ReturnInvoice
    {
        get
        {
            if (ViewState["ConfirmationAnswered_ReturnInvoice"] == null) return false;
            return (bool)ViewState["ConfirmationAnswered_ReturnInvoice"];
        }

        set
        {
            ViewState["ConfirmationAnswered_ReturnInvoice"] = value;
        }
    }

    private string ConfirmationMessage_ReturnInvoice
    {
        get
        {
            if (ViewState["ConfirmationMessage_ReturnInvoice"] == null)
            {
                ViewState["ConfirmationMessage_ReturnInvoice"] = string.Empty;
            }
            return (string)ViewState["ConfirmationMessage_ReturnInvoice"];
        }

        set
        {
            ViewState["ConfirmationMessage_ReturnInvoice"] = value;
        }
    }

    private bool SumFirstPaid_ReturnInvoice
    {
        get
        {
            if (ViewState["SumFirstPaid_ReturnInvoice"] == null)
            {
                ViewState["SumFirstPaid_ReturnInvoice"] = dc.usp_Company_Select().First().SumFirstPaid;
            }
            return (bool)ViewState["SumFirstPaid_ReturnInvoice"];
        }

        set
        {
            ViewState["SumFirstPaid_ReturnInvoice"] = value;
        }
    }

    #endregion

    private bool SaveReturnInvoice(bool IsApproving, int ID)
    {
        this.ReturnInvoice_ID = ID;
        var invoice = dc.usp_ReturnInvoice_SelectByID(this.ReturnInvoice_ID).FirstOrDefault();

        string ddlCurrency = invoice.Currency_ID.ToExpressString();
        string txtRatio = invoice.Ratio.ToExpressString();
        string acBranch = invoice.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = invoice.OperationDate.Value.ToString("d/M/yyyy");
        string acCostCenter = invoice.CostCenter_ID.ToStringOrEmpty();
        string txtUserRefNo = invoice.UserRefNo;
        string acCustomer = invoice.Contact_ID.ToExpressString();
        string acTelephone = invoice.Telephone_ID.ToStringOrEmpty();
        string acAddress = invoice.DefaultAddress_ID.ToStringOrEmpty();
        string acShipAddress = invoice.ShipToAddress_ID.ToStringOrEmpty();
        string acPaymentAddress = invoice.PaymentAddress_ID.ToStringOrEmpty();
        string acCashAccount = invoice.CashAccount_ID.ToStringOrEmpty();
        string txtCashDiscount = invoice.CashDiscount.ToExpressString();
        string txtAdditionals = invoice.Additionals.ToExpressString();
        string txtPercentageDiscount = invoice.PercentageDiscount.ToExpressString();
        string txtFirstPaid = invoice.FirstPaid.ToExpressString();
        string lblTotal = invoice.TotalAmount.ToExpressString();
        string lblTotalDiscount = invoice.TotalDiscount.ToExpressString();
        string lblTotalTax = invoice.TotalTax.ToExpressString();
        string lblGrossTotal = invoice.GrossTotalAmount.ToExpressString();
        string txtNotes = invoice.Notes;

        this.dtItems_ReturnInvoice = dc.usp_ReturnInvoiceDetails_Select(this.ReturnInvoice_ID).CopyToDataTable();
        //TODO acSalesRep
        string acSalesRep = null;
        this.FromInvoice_IDRTIn = invoice.FromInvoiceID;

        ////////////////////////Calculate



        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal DocTaxValue = 0;
        decimal Additionals = 0;
        this.Total_ReturnInvoice = 0;
        this.GrossTotal_ReturnInvoice = 0;
        this.TotalDiscount_ReturnInvoice = 0;
        this.TotalTax_ReturnInvoice = 0;
        this.TotalDebitTax_ReturnInvoice = 0;
        this.TotalCreditTax_ReturnInvoice = 0;
        this.dtAllTaxes_ReturnInvoice.Rows.Clear();
        this.dtAllTaxes_ReturnInvoice.AcceptChanges();
        foreach (DataRow r in this.dtItems_ReturnInvoice.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total_ReturnInvoice += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount_ReturnInvoice += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax_ReturnInvoice += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                else
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax_ReturnInvoice += r["TotalTax"].ToDecimal();
                }
                this.TotalTax_ReturnInvoice += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotal_ReturnInvoice += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal_ReturnInvoice * txtPercentageDiscount.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.ToDecimalOrDefault();
        foreach (DataRow r in this.dtTaxes_ReturnInvoice.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (r["OnInvoiceType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal_ReturnInvoice - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax_ReturnInvoice += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                else
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax_ReturnInvoice += DocTaxValue;
                }
                DocTax += DocTaxValue;
            }
        }
        Additionals = txtAdditionals.ToDecimalOrDefault();
        this.TotalDiscount_ReturnInvoice += DocDiscount;
        this.TotalTax_ReturnInvoice += DocTax;
        this.GrossTotal_ReturnInvoice = this.GrossTotal_ReturnInvoice - DocDiscount + DocTax + Additionals;
        //lblTotal.Text = this.Total.ToString("0.####");
        //lblTotalDiscount.Text = this.TotalDiscount.ToString("0.####");
        //lblAdditionals.Text = Additionals.ToString("0.####");
        //lblTotalTax.Text = this.TotalTax.ToString("0.####");
        //lblGrossTotal.Text = this.GrossTotal.ToString("0.####");
        //if (acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid)
        //{
        //    txtFirstPaid.Text = lblGrossTotal.Text;
        //    this.txtFirstPaid_TextChanged(null, null);
        //}
        //this.ConfirmationAnswered = false;
        //this.ConfirmationMessage = string.Empty;

        ///////////////////////



        if (!string.IsNullOrEmpty(invoice.FromInvoiceSerial))
        {
            string lblFromInvoiceNo = invoice.FromInvoiceSerial;

        }


        string txtSerial = invoice.Serial;
        this.DocRandomString_ReturnInvoice = invoice.DocRandomString;
        string lblCreatedBy = invoice.CreatedByName;
        string lblApprovedBy = invoice.ApprovedBYName;




        this.dtItems_ReturnInvoice = dc.usp_ReturnInvoiceDetails_Select(this.ReturnInvoice_ID).CopyToDataTable();


        this.CalculatedReturnSalesCost_ReturnInvoice = 0;
        this.ConfirmationMessage_ReturnInvoice = string.Empty;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        int Serial_ID = DocSerials.ReturnInvoice.ToInt();
        int Detail_ID = 0;


        //int Result = dc.usp_ReturnInvoice_Update(this.ReturnInvoice_ID, acBranch.ToNullableInt(), 15, txtRatio.ToDecimalOrDefault(), txtOperationDate.ToDate(),
        //                                        acCustomer.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
        //                                        null, null, acAddress.ToNullableInt(), acShipAddress.ToNullableInt(), acPaymentAddress.ToNullableInt(), acTelephone.ToNullableInt(),
        //                                        acCostCenter.ToNullableInt(), txtNotes, lblTotal.ToDecimal(), txtPercentageDiscount.ToDecimalOrDefault(), txtCashDiscount.ToDecimalOrDefault(), txtAdditionals.ToDecimalOrDefault(),
        //                                        lblTotalDiscount.ToDecimalOrDefault(), lblTotalTax.ToDecimalOrDefault(), lblGrossTotal.ToDecimalOrDefault(), txtFirstPaid.ToDecimalOrDefault(), txtUserRefNo, this.DocRandomString_ReturnInvoice, acSalesRep.ToNullableInt(), acCashAccount.ToNullableInt());
        //if (Result > 0)
        //{
            foreach (DataRow r in this.dtItems_ReturnInvoice.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    Detail_ID = dc.usp_ReturnInvoiceDetails_Insert(this.ReturnInvoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["InvoiceDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_ReturnInvoiceDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_ReturnInvoiceDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}
                if (IsApproving && r.RowState != DataRowState.Deleted)
                {
                    Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                    if (!this.InsertICJReturnInvoice(Detail_ID, r, txtOperationDate, acBranch))
                    {

                    }
                }
            }

            foreach (DataRow r in this.dtTaxes_ReturnInvoice.Rows)
            {
                if (r.RowState == DataRowState.Added)
                {
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, r["Tax_ID"].ToInt());
                }
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
            }
            if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.ToDate(), acBranch.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID);
            if (IsApproving) this.InsertOperationReturnInvoice(acCustomer, txtAdditionals, txtOperationDate, acBranch, txtNotes, acCostCenter, txtFirstPaid, acCashAccount, txtUserRefNo);

        //}




        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        // UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReturnInvoice + "?ID=" + this.ReturnInvoice_ID.ToExpressString(), PageLinks.ReturnInvoicesList, PageLinks.ReturnInvoice);
        return true;
    }
    private void InsertOperationReturnInvoice(string acCustomer, string txtAdditionals, string txtOperationDate, string acBranch, string txtNotes, string acCostCenter, string txtFirstPaid, string acCashAccount, string txtUserRefNo)
    {
        decimal ratio = 1;
        decimal Additionals = txtAdditionals.ToDecimalOrDefault();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acCustomer.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.ReturnSales.ToInt(), 15, (this.Total_ReturnInvoice + this.TotalDebitTax_ReturnInvoice + Additionals) * ratio, (this.Total_ReturnInvoice + this.TotalDebitTax_ReturnInvoice + Additionals), ratio, txtNotes);

        //ايراد مبيعات
        dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, (this.Total_ReturnInvoice) * ratio, 0, (this.Total_ReturnInvoice), 0, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        //ايراد الاضافات
        if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, Additionals * ratio, 0, Additionals, 0, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());


        //Customer
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotal_ReturnInvoice) * ratio, 0, (this.GrossTotal_ReturnInvoice), null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        //Discount
        if (this.TotalDiscount_ReturnInvoice > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, 0, this.TotalDiscount_ReturnInvoice * ratio, 0, this.TotalDiscount_ReturnInvoice, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes_ReturnInvoice.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        }

        if (this.CalculatedReturnSalesCost_ReturnInvoice > 0)
        {
            dc.usp_ReturnSalesCostOperation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), this.CalculatedReturnSalesCost_ReturnInvoice, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        //CostCenter debit
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), (this.Total_ReturnInvoice + Additionals) * ratio, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), txtNotes);
        //CostCenter credit
        dc.usp_CostCenterOperation_Insert(acBranch.ToNullableInt(), acCostCenter.ToNullableInt(), txtOperationDate.ToDate(), (this.CalculatedReturnSalesCost_ReturnInvoice + (this.TotalDiscount_ReturnInvoice * ratio)) * -1, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), txtNotes);
        this.InsertCashOutReturnInvoice(txtFirstPaid, acCustomer, acCashAccount, txtUserRefNo, acBranch, txtOperationDate, txtNotes);
    }
    private void InsertCashOutReturnInvoice(string txtFirstPaid, string acCustomer, string acCashAccount, string txtUserRefNo, string acBranch, string txtOperationDate, string txtNotes)
    {
        string Serial = string.Empty;
        decimal ratio = 1;
        int? CashOut_ID = null;
        if (txtFirstPaid.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.ToInt()).Value;
        CashOut_ID = dc.usp_Payments_Insert(txtOperationDate.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo, ref Serial, DocSerials.CashOut.ToInt(), txtNotes, txtFirstPaid.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashOut.ToByte(), acBranch.ToNullableInt(), 1, 15, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), this.DocRandomString_ReturnInvoice + "_FromReturnInvoice");
        if (!CashOut_ID.HasValue || CashOut_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashOut_ID, ContactChartOfAccount_ID, acCashAccount.ToInt(), txtFirstPaid.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashOut.ToInt(), 15, txtFirstPaid.ToDecimal() * ratio, txtFirstPaid.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.ToInt(), 0, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, txtFirstPaid.ToDecimal() * ratio, 0, txtFirstPaid.ToDecimal(), 0, null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_SetCashDocForBills(this.ReturnInvoice_ID, CashOut_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
    }
    private bool InsertICJReturnInvoice(int Detail_ID, DataRow row, string txtOperationDate, string acBranch)
    {
        int result = 0;
        decimal? ReturnSalesCost = 0;
        result = dc.usp_ICJ_ReturnInvoice(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * 1, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, Detail_ID, DocumentsTableTypes.Invoice.ToInt(), row["InvoiceDetail_ID"].ToNullableInt(), ref ReturnSalesCost);
        this.CalculatedReturnSalesCost_ReturnInvoice += ReturnSalesCost.Value;
        //if (result == -5)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.CantReturnMoreOriginal + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
        //    return false;
        //}
        //if (result == -10)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.CantReturnNegativeQty + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
        //    return false;
        //}
        //if (result == -7 && !this.ConfirmationAnswered)
        //{
        //    this.ConfirmationMessage += "<br>\u2022 " + Resources.UserInfoMessages.ReturnQtyMoreThanSold + " (" + row["StoreName"] + " : " + row["ItemName"] + ")";
        //}
        return true;
    }
    private void CalculateReturnInvoice(string txtPercentageDiscount, string txtCashDiscount, string txtAdditionals)
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal DocTaxValue = 0;
        decimal Additionals = 0;
        this.Total_ReturnInvoice = 0;
        this.GrossTotal_ReturnInvoice = 0;
        this.TotalDiscount_ReturnInvoice = 0;
        this.TotalTax_ReturnInvoice = 0;
        this.TotalDebitTax_ReturnInvoice = 0;
        this.TotalCreditTax_ReturnInvoice = 0;
        this.dtAllTaxes_ReturnInvoice.Rows.Clear();
        this.dtAllTaxes_ReturnInvoice.AcceptChanges();
        foreach (DataRow r in this.dtItems_ReturnInvoice.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total_ReturnInvoice += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount_ReturnInvoice += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax_ReturnInvoice += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                else
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax_ReturnInvoice += r["TotalTax"].ToDecimal();
                }
                this.TotalTax_ReturnInvoice += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotal_ReturnInvoice += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal_ReturnInvoice * txtPercentageDiscount.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.ToDecimalOrDefault();
        foreach (DataRow r in this.dtTaxes_ReturnInvoice.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (r["OnInvoiceType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal_ReturnInvoice - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax_ReturnInvoice += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                else
                {
                    this.dtAllTaxes_ReturnInvoice.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax_ReturnInvoice += DocTaxValue;
                }
                DocTax += DocTaxValue;
            }
        }
        Additionals = txtAdditionals.ToDecimalOrDefault();
        this.TotalDiscount_ReturnInvoice += DocDiscount;
        this.TotalTax_ReturnInvoice += DocTax;
        this.GrossTotal_ReturnInvoice = this.GrossTotal_ReturnInvoice - DocDiscount + DocTax + Additionals;
        //lblTotal.Text = this.Total.ToString("0.####");
        //lblTotalDiscount.Text = this.TotalDiscount.ToString("0.####");
        //lblAdditionals.Text = Additionals.ToString("0.####");
        //lblTotalTax.Text = this.TotalTax.ToString("0.####");
        //lblGrossTotal.Text = this.GrossTotal.ToString("0.####");
        //if (acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid)
        //{
        //    txtFirstPaid.Text = lblGrossTotal.Text;
        //    this.txtFirstPaid_TextChanged(null, null);
        //}
        //this.ConfirmationAnswered = false;
        //this.ConfirmationMessage = string.Empty;
    }
    #endregion

    #region Correct

    #region Properties


    private int InventoryDocument_ID
    {
        get
        {
            if (ViewState["InventoryDocument_ID"] == null) return 0;
            return (int)ViewState["InventoryDocument_ID"];
        }

        set
        {
            ViewState["InventoryDocument_ID"] = value;
        }
    }


    private DataTable dtItems_InventoryCorr
    {
        get
        {
            if (Session["dtItems_InventoryCorr" + this.WinID] == null)
            {
                Session["dtItems_InventoryCorr" + this.WinID] = dc.usp_InventoryDocumentDetails_Select(null, null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_InventoryCorr" + this.WinID];
        }

        set
        {
            Session["dtItems_InventoryCorr" + this.WinID] = value;
        }
    }


    private decimal Total_InventoryCorr
    {
        get
        {
            if (ViewState["Total_InventoryCorr"] == null) return 0;
            return (decimal)ViewState["Total_InventoryCorr"];
        }

        set
        {
            ViewState["Total_InventoryCorr"] = value;
        }
    }

    private string DocRandomString_InventoryCorr
    {
        get
        {
            if (ViewState["DocRandomString_InventoryCorr"] == null)
            {
                ViewState["DocRandomString_InventoryCorr"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString_InventoryCorr"];
        }

        set
        {
            ViewState["DocRandomString_InventoryCorr"] = value;
        }
    }

    private decimal CalculatedSalesCost_InventoryCorr
    {
        get
        {
            if (ViewState["CalculatedSalesCost_InventoryCorr"] == null) return 0;
            return (decimal)ViewState["CalculatedSalesCost_InventoryCorr"];
        }

        set
        {
            ViewState["CalculatedSalesCost_InventoryCorr"] = value;
        }
    }

    private decimal IncomingCost_InventoryCorr
    {
        get
        {
            if (ViewState["IncomingCost_InventoryCorr"] == null) return 0;
            return (decimal)ViewState["IncomingCost_InventoryCorr"];
        }

        set
        {
            ViewState["IncomingCost_InventoryCorr"] = value;
        }
    }

    #endregion

    private bool SaveCorrect(bool IsApproving, int ID)
    {

        this.InventoryDocument_ID = ID;
        var InventoryDoc = dc.usp_InventoryDocument_SelectByID(this.InventoryDocument_ID).FirstOrDefault();
        string acBranch = InventoryDoc.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = InventoryDoc.OperationDate.Value.ToString("d/M/yyyy");
        string txtUserRefNo = InventoryDoc.UserRefNo;
        string lblTotal = InventoryDoc.TotalAmount.ToExpressString();
        string txtNotes = InventoryDoc.Notes;
        string txtSerial = InventoryDoc.Serial;
        string acOppositeAccount = InventoryDoc.OppositeAccount_ID.ToExpressString();
        this.DocRandomString_InventoryCorr = InventoryDoc.DocRandomString;
        string lblCreatedBy = InventoryDoc.CreatedByName;
        string lblApprovedBy = InventoryDoc.ApprovedBYName;

        this.dtItems_InventoryCorr = dc.usp_InventoryDocumentDetails_Select(this.InventoryDocument_ID, null).CopyToDataTable();


        this.Total_InventoryCorr = 0;
        foreach (DataRow r in this.dtItems_InventoryCorr.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * Math.Abs(r["Differrence"].ToDecimal());
            this.Total_InventoryCorr += r["UnitCost"].ToDecimal() * Math.Abs(r["Differrence"].ToDecimal());
        }
        lblTotal = this.Total_InventoryCorr.ToString("0.####");


        this.CalculatedSalesCost_InventoryCorr = 0;
        this.IncomingCost_InventoryCorr = 0;

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 0;
        int Serial_ID = DocSerials.InventoryCorrection.ToInt();
        int Detail_ID = 0;


        //int Result = dc.usp_InventoryDocument_Update(this.InventoryDocument_ID, acBranch.ToNullableInt(), txtOperationDate.ToDate(),
        //                                            ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
        //                                            approvedBY_ID, null, null,
        //                                            txtNotes, lblTotal.ToDecimal(), 0, 0,
        //                                            txtUserRefNo, acOppositeAccount.ToInt(), null);
        //if (Result > 0)
        //{
            foreach (DataRow r in this.dtItems_InventoryCorr.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ActualQty"].ToDecimal(), r["Differrence"].ToDecimal(), null, r["QtyInNumber"].ToDecimalOrDefault(),null);
                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_InventoryDocumentDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ActualQty"].ToDecimal(), r["Differrence"].ToDecimal(), null, r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_InventoryDocumentDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}

                if (IsApproving && r.RowState != DataRowState.Deleted)
                {
                    Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                    if (!this.InsertICJCorrect(Detail_ID, r, txtOperationDate, acBranch))
                    {
                    }
                }
            }

            if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.ToDate(), acBranch.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID);
            if (IsApproving) this.InsertOperationCorrect(acBranch, txtOperationDate, acOppositeAccount, txtNotes);
       // }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        return true;
    }

    private void InsertOperationCorrect(string acBranch, string txtOperationDate, string acOppositeAccount, string txtNotes)
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();

        if (this.CalculatedSalesCost_InventoryCorr > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryCorrectionOut.ToInt(), company.Currency_ID, this.CalculatedSalesCost_InventoryCorr, this.CalculatedSalesCost_InventoryCorr, 1, txtNotes);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost_InventoryCorr, 0, this.CalculatedSalesCost_InventoryCorr, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, acOppositeAccount.ToInt(), this.CalculatedSalesCost_InventoryCorr, 0, this.CalculatedSalesCost_InventoryCorr, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());
        }

        if (this.IncomingCost_InventoryCorr > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryCorrectionIN.ToInt(), company.Currency_ID, this.IncomingCost_InventoryCorr, this.IncomingCost_InventoryCorr, 1, txtNotes);

            //المخزون مدين
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.IncomingCost_InventoryCorr, 0, this.IncomingCost_InventoryCorr, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, acOppositeAccount.ToInt(), 0, this.IncomingCost_InventoryCorr, 0, this.IncomingCost_InventoryCorr, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());
        }


    }

    private bool InsertICJCorrect(int Detail_ID, DataRow row, string txtOperationDate, string acBranch)
    {
        decimal? Cost = 0;
        int result = 0;
        if (row["Differrence"].ToDecimal() < 0)
        {
            result = dc.usp_ICJ_InvCorr_Out(txtOperationDate.ToDate(), row["Differrence"].ToDecimal() * -1, row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID, Detail_ID, ref Cost);
            this.CalculatedSalesCost_InventoryCorr += Cost.Value;
        }
        else if (row["Differrence"].ToDecimal() > 0)
        {
            result = dc.usp_ICJ_InvCorr_IN(txtOperationDate.ToDate(), row["Differrence"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID, Detail_ID, null);
            this.IncomingCost_InventoryCorr += row["Total"].ToDecimal();
        }


        return true;
    }


    #endregion

    #region Transfer
    #region Properties


    private int InventoryDocumentTransfer_ID
    {
        get
        {
            if (ViewState["InventoryDocumentTransfer_ID"] == null) return 0;
            return (int)ViewState["InventoryDocumentTransfer_ID"];
        }

        set
        {
            ViewState["InventoryDocumentTransfer_ID"] = value;
        }
    }

    private DataTable dtItems_InventoryTrans
    {
        get
        {
            if (Session["dtItems_InventoryTrans" + this.WinID] == null)
            {
                Session["dtItems_InventoryTrans" + this.WinID] = dc.usp_InventoryDocumentDetails_Select(null, null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_InventoryTrans" + this.WinID];
        }

        set
        {
            Session["dtItems_InventoryTrans" + this.WinID] = value;
        }
    }

    private string DocRandomString_InventoryTrans
    {
        get
        {
            if (ViewState["DocRandomString_InventoryTrans"] == null)
            {
                ViewState["DocRandomString_InventoryTrans"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString_InventoryTrans"];
        }

        set
        {
            ViewState["DocRandomString"] = value;
        }
    }

    private decimal CalculatedSalesCost_InventoryTrans
    {
        get
        {
            if (ViewState["CalculatedSalesCost_InventoryTrans"] == null) return 0;
            return (decimal)ViewState["CalculatedSalesCost_InventoryTrans"];
        }

        set
        {
            ViewState["CalculatedSalesCost_InventoryTrans"] = value;
        }
    }

    #endregion

    private bool SaveTransfer(bool IsApproving, int ID)
    {


        this.InventoryDocumentTransfer_ID = ID;
        var InventoryDoc = dc.usp_InventoryDocument_SelectByID(this.InventoryDocumentTransfer_ID).FirstOrDefault();
        string acBranch = InventoryDoc.Branch_ID.ToStringOrEmpty();
        string txtOperationDate = InventoryDoc.OperationDate.Value.ToString("d/M/yyyy");
        string txtUserRefNo = InventoryDoc.UserRefNo;
        string txtNotes = InventoryDoc.Notes;
        string txtSerial = InventoryDoc.Serial;
        string acToBranch = InventoryDoc.ToBranch_ID.ToStringOrEmpty();
        this.DocRandomString_InventoryTrans = InventoryDoc.DocRandomString;
        string lblCreatedBy = InventoryDoc.CreatedByName;
        string lblApprovedBy = InventoryDoc.ApprovedBYName;
        this.dtItems_InventoryTrans = dc.usp_InventoryDocumentDetails_Select(this.InventoryDocumentTransfer_ID, null).CopyToDataTable();


        this.CalculatedSalesCost_InventoryTrans = 0;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 1;
        int Serial_ID = DocSerials.InventoryTransfer.ToInt();
        int Detail_ID = 0;


        //int Result = dc.usp_InventoryDocument_Update(this.InventoryDocumentTransfer_ID, acBranch.ToNullableInt(), txtOperationDate.ToDate(),
        //                                            ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
        //                                            approvedBY_ID, null, null,
        //                                            txtNotes, 0, 0, 0,
        //                                            txtUserRefNo, null, acToBranch.ToNullableInt());
        //if (Result > 0)
        //{
            foreach (DataRow r in this.dtItems_InventoryTrans.Rows)
            {
                //if (r.RowState == DataRowState.Added)
                //{
                //    Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocumentTransfer_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), null, r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, r["ToStore_ID"].ToInt(), r["QtyInNumber"].ToDecimalOrDefault(),null);
                //}
                //if (r.RowState == DataRowState.Modified)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //    dc.usp_InventoryDocumentDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), null, r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, r["ToStore_ID"].ToInt(), r["QtyInNumber"].ToDecimalOrDefault());
                //}
                //if (r.RowState == DataRowState.Deleted)
                //{
                //    dc.usp_InventoryDocumentDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                //}
                //if (r.RowState == DataRowState.Unchanged)
                //{
                //    Detail_ID = r["ID"].ToInt();
                //}
                if (!this.InsertICJTransfer(Detail_ID, r, txtOperationDate, acBranch, acToBranch))
                    if (IsApproving && r.RowState != DataRowState.Deleted)
                    {
                        Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();

                    }
            }

            if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.ToDate(), null, DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocumentTransfer_ID);
            if (IsApproving && acBranch.ToStringOrEmpty() != acToBranch.ToStringOrEmpty()) this.InsertOperationTransfer(acBranch, acToBranch, txtOperationDate, txtNotes);

      //  }


        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        return true;
    }
    private decimal GetQantity(DataRow r, string acBranch)
    {

        decimal qty = 0;

        if (r["ItemDescribed_ID"].ToInt() > 0)
        {


            var funItemQty = dc.fun_ItemDQty(r["Item_ID"].ToInt(), r["Store_ID"].ToInt(), null, r["Uom_iD"].ToInt(), null, r["ItemDescribed_ID"].ToInt(), acBranch.ToNullableInt());

            if (funItemQty != null)
            {
                qty = funItemQty.Value;

            }
        }
        else
        {


            var funItemQty = dc.fun_ItemQty(r["Item_ID"].ToInt(), r["Store_ID"].ToInt(), null, r["Uom_iD"].ToInt(), null);
            if (funItemQty != null)
            {
                qty = funItemQty.Value;
            }
        }

        return qty;
    }
    private bool InsertICJTransfer(int Detail_ID, DataRow row, string txtOperationDate, string acBranch, string acToBranch)
    {
        decimal? Cost = 0;
        int result = 0;
        //if (GetQantity(row, acBranch) <= 0)
        //{
        //    result = dc.usp_ICJ_InvTransferException(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), row["ToStore_ID"].ToInt(), acToBranch.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocumentTransfer_ID, Detail_ID, row["ItemDescribed_ID"].ToInt(), ref Cost);
        //}
        //else
        //{
        result = dc.usp_ICJ_InvTransfer(txtOperationDate.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.ToNullableInt(), row["ToStore_ID"].ToInt(), acToBranch.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocumentTransfer_ID, Detail_ID,null, ref Cost);

        // }
        this.CalculatedSalesCost_InventoryTrans += Cost.Value;

        return true;
    }
    private void InsertOperationTransfer(string acBranch, string acToBranch, string txtOperationDate, string txtNotes)
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var Branches = dc.usp_Branchs_Select(string.Empty, null).ToList();
        var FromBranchAccount_ID = Branches.Where(x => x.ID == acBranch.ToInt()).First().BranchChartOfAccount_ID;
        var ToBranchAccount_ID = Branches.Where(x => x.ID == acToBranch.ToInt()).First().BranchChartOfAccount_ID;

        if (this.CalculatedSalesCost_InventoryTrans > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryTransfer.ToInt(), company.Currency_ID, this.CalculatedSalesCost_InventoryTrans, this.CalculatedSalesCost_InventoryTrans, 1, txtNotes);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost_InventoryTrans, 0, this.CalculatedSalesCost_InventoryTrans, null, this.InventoryDocumentTransfer_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            //حساب الفرع
            dc.usp_OperationDetails_Insert(Result, ToBranchAccount_ID, this.CalculatedSalesCost_InventoryTrans, 0, this.CalculatedSalesCost_InventoryTrans, 0, null, this.InventoryDocumentTransfer_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            Result = dc.usp_Operation_Insert(acToBranch.ToNullableInt(), txtOperationDate.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryTransfer.ToInt(), company.Currency_ID, this.CalculatedSalesCost_InventoryTrans, this.CalculatedSalesCost_InventoryTrans, 1, txtNotes);

            //المخزون مدين
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.CalculatedSalesCost_InventoryTrans, 0, this.CalculatedSalesCost_InventoryTrans, 0, null, this.InventoryDocumentTransfer_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            //حساب الفرع
            dc.usp_OperationDetails_Insert(Result, FromBranchAccount_ID, 0, this.CalculatedSalesCost_InventoryTrans, 0, this.CalculatedSalesCost_InventoryTrans, null, this.InventoryDocumentTransfer_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());
        }
    }

    #endregion
}