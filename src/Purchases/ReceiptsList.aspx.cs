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

public partial class Purchases_ReceiptsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtReceiptsList
    {
        get
        {
            return (DataTable)Session["dtReceiptsList" + this.WinID];
        }

        set
        {
            Session["dtReceiptsList" + this.WinID] = value;
        }
    }

    #endregion

    #region ViewState

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

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvReceiptsList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvReceiptsList.Columns[8].Visible = this.MyContext.PageData.IsViewDoc;
                gvReceiptsList.Columns[9].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillReceiptsList();
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


            this.FillReceiptsList();
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
            acVendorName.Clear();
            this.FilterVendors(null, null);
            this.FillReceiptsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterVendors(object sender, EventArgs e)
    {
        try
        {
            acVendorName.ContextKey = "V," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvReceiptsList.PageIndex = e.NewPageIndex;
            gvReceiptsList.DataSource = this.dtReceiptsList;
            gvReceiptsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvReceiptsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkPay_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.Receipt_ID = gvReceiptsList.DataKeys[Index]["ID"].ToInt();
            lblReceiptSerial.Text = gvReceiptsList.DataKeys[Index]["Serial"].ToExpressString();
            lblCollected.Text = gvReceiptsList.DataKeys[Index]["PaidAmount"].ToExpressString();
            lblGrossTotal.Text = gvReceiptsList.DataKeys[Index]["GrossTotalAmount"].ToExpressString();
            mpePay.Show();
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
            mpePay.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnOkPay_click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(ddlPayWith.SelectedValue + "?RelatedDocTableType_ID=2&RelatedDoc_ID=" + Receipt_ID.ToExpressString(), false);
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
            XpressDataContext dataContext = new XpressDataContext();
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            this.Receipt_ID = gvReceiptsList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvReceiptsList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            string SaveName = Request.PathInfo == "/Receipt" ? "Receipt" : "PurchaseOrder";
            ReportDocument doc = new ReportDocument();
            var databaseName = dataContext.Connection.Database;
            if (databaseName == "TWFL")
            {

                doc.Load(Server.MapPath("~\\Reports\\ReceiptPrint.rpt"));
                doc.SetParameterValue("@Receipt_ID", this.Receipt_ID);

                doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
                doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
                doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
                Response.Redirect(
                    PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Receipt"), false);
            }
            else
            {
                var receipt = dc.usp_Receipt_SelectByID(this.Receipt_ID);
                //var totalAmount = receipt.Select(x => x.GrossTotalAmount).FirstOrDefault();

                //doc.Load(Server.MapPath("~\\Reports\\Receipt_Print ssb.rpt"));
                //doc.SetParameterValue("@Receipt_ID", this.Receipt_ID);
                //doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault().ToString(), "Tafkit.rpt");
                //doc.SetParameterValue("@TheNo2", "0", "Tafkit.rpt");
                //Response.Redirect(
                //    PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Receipt"), false);

                Response.Redirect("~/Report_Dev/PrintReceiptDev.aspx?Invoice_ID=" + Receipt_ID + "&IsMaterla=1", false);

            }








        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillReceiptsList()
    {
        byte EntryType = Request.PathInfo == "/Receipt" ? (byte)1 : (byte)0;
        lnkadd.NavigateUrl = Request.PathInfo == "/Receipt" ? PageLinks.ReceiptShortcut  : PageLinks.PurchaseOrder;
        gvReceiptsList.Columns[11].Visible = Request.PathInfo != "/Receipt";
        gvReceiptsList.Columns[12].Visible = Request.PathInfo == "/Receipt";
        gvReceiptsList.Columns[13].Visible = Request.PathInfo == "/Receipt";
        divHasBill.Visible = Request.PathInfo != "/Receipt";
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        bool? HasReceipt = ddlIsHasBill.SelectedIndex == 0 ? (bool?)null : ddlIsHasBill.SelectedValue.ToBoolean();
        // this.dtReceiptsList = dc.usp_Receipt_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acVendorName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), HasReceipt, EntryType).CopyToDataTable();


        #region save Search
        //List<string> s = new List<string>();

        //string ret = LoggerNavigation.GetLogNav(MyContext.UserProfile.Contact_ID);
        //var js = new JavaScriptSerializer();
        //if (ret != Environment.NewLine && ret != string.Empty)
        //{
        //    var lt = js.Deserialize<List<InvoiceListSearchable>>(ret).ToList();

        //    var list = lt.Where(x => x.Page_ID == MyContext.PageData.PageID && x.UserID == MyContext.UserProfile.Contact_ID);

        //    if (lt != null && lt.Any())
        //    {
        //        var objectNav = list.FirstOrDefault();
        //        if (objectNav != null)
        //        {
        //            if (InvoicesSearchable == null)
        //            {
        //                if (objectNav.acBranch != null) acBranch.Value = objectNav.acBranch.ToExpressString();
        //                if (objectNav.DocStatus != null) { DocStatus_ID = objectNav.DocStatus.ToByteOrDefault(); ddlStatus.SelectedValue = objectNav.DocStatus.ToExpressString(); }
        //                if (objectNav.FromDate != null) { txtDateFromSrch.Text = (objectNav.FromDate != null ? objectNav.FromDate.Value.ToLocalTime().ToExpressString() : string.Empty); }
        //                if (objectNav.ToDate != null) { txtDateToSrch.Text = (objectNav.ToDate != null ? objectNav.ToDate.Value.ToLocalTime().ToExpressString() : string.Empty); }
        //            }
        //            else
        //            {
        //                if (InvoicesSearchable.acBranch != null) acBranch.Value = InvoicesSearchable.acBranch.ToExpressString();
        //                if (InvoicesSearchable.DocStatus != null) DocStatus_ID = InvoicesSearchable.DocStatus.ToByteOrDefault();
        //                if (InvoicesSearchable.FromDate != null) txtDateFromSrch.Text = (InvoicesSearchable.FromDate == DateTime.MinValue ? null : InvoicesSearchable.FromDate.ToExpressString());
        //                if (InvoicesSearchable.ToDate != null) txtDateToSrch.Text = (InvoicesSearchable.ToDate == DateTime.MinValue ? null : InvoicesSearchable.ToDate.ToExpressString());
        //                if (!list.Any())
        //                {
        //                    lt.Add(new InvoiceListSearchable()
        //                    {
        //                        acBranch = InvoicesSearchable.acBranch.ToNullableInt(),
        //                        DocStatus = InvoicesSearchable.DocStatus.ToByteOrDefault(),
        //                        FromDate = (InvoicesSearchable.FromDate == DateTime.MinValue ? null : InvoicesSearchable.FromDate.ToDate()),
        //                        ToDate = (InvoicesSearchable.ToDate == DateTime.MinValue ? null : InvoicesSearchable.ToDate.ToDate()),
        //                        Page_ID = MyContext.PageData.PageID,
        //                        UserID = MyContext.UserProfile.Contact_ID
        //                    });
        //                }
        //                else
        //                {
        //                    var xx = lt.Where(x => x.Page_ID == MyContext.PageData.PageID).FirstOrDefault();
        //                    lt.Remove(xx);
        //                    lt.Add(new InvoiceListSearchable()
        //                    {
        //                        acBranch = InvoicesSearchable.acBranch.ToNullableInt(),
        //                        DocStatus = InvoicesSearchable.DocStatus.ToByteOrDefault(),
        //                        FromDate = (InvoicesSearchable.FromDate == DateTime.MinValue ? null : InvoicesSearchable.FromDate.ToDate()),
        //                        ToDate = (InvoicesSearchable.ToDate == DateTime.MinValue ? null : InvoicesSearchable.ToDate.ToDate()),
        //                        Page_ID = MyContext.PageData.PageID,
        //                        UserID = MyContext.UserProfile.Contact_ID
        //                    });
        //                }
        //            }
        //        }

        //        if (!list.Any() && InvoicesSearchable != null)
        //        {

        //            lt.Add(new InvoiceListSearchable()
        //            {
        //                acBranch = InvoicesSearchable.acBranch.ToNullableInt(),
        //                DocStatus = InvoicesSearchable.DocStatus.ToByteOrDefault(),
        //                FromDate = (InvoicesSearchable.FromDate == DateTime.MinValue ? null : InvoicesSearchable.FromDate.ToDate()),
        //                ToDate = (InvoicesSearchable.ToDate == DateTime.MinValue ? null : InvoicesSearchable.ToDate.ToDate()),
        //                Page_ID = MyContext.PageData.PageID,
        //                UserID = MyContext.UserProfile.Contact_ID
        //            });
        //        }

        //        foreach (var item in lt)
        //        {
        //            s.Add(js.Serialize(new InvoiceListSearchable()
        //            {
        //                acBranch = item.acBranch.ToNullableInt(),
        //                DocStatus = item.DocStatus,
        //                FromDate = item.FromDate != null ? item.FromDate.Value.ToLocalTime() : (DateTime?)null,
        //                ToDate = item.ToDate != null ? item.ToDate.Value.ToLocalTime() : (DateTime?)null,
        //                Page_ID = item.Page_ID,
        //                UserID = item.UserID
        //            }));
        //        }
        //        LoggerNavigation.LogNav(s, MyContext.UserProfile.Contact_ID);
        //    }
        //}
        //else
        //{
        //    if (InvoicesSearchable != null)
        //        s.Add(js.Serialize(new InvoiceListSearchable()
        //        {
        //            acBranch = InvoicesSearchable.acBranch.ToNullableInt(),
        //            DocStatus = InvoicesSearchable.DocStatus.ToByteOrDefault(),
        //            FromDate = (InvoicesSearchable.FromDate == DateTime.MinValue ? null : InvoicesSearchable.FromDate.ToDate()),
        //            ToDate = (InvoicesSearchable.ToDate == DateTime.MinValue ? null : InvoicesSearchable.ToDate.ToDate()),
        //            Page_ID = MyContext.PageData.PageID,
        //            UserID = MyContext.UserProfile.Contact_ID
        //        }));
        //    LoggerNavigation.LogNav(s, MyContext.UserProfile.Contact_ID);
        //}


        #endregion

        
        
        var lstInvoices = dc.usp_ReceiptWithFirstPaid_Select(acBranch.Value.ToNullableInt(), Currency_ID,
            txtSerialsrch.TrimmedText, acVendorName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(),
            txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), HasReceipt,
            EntryType, (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID)).ToList();
        foreach (var item in lstInvoices)
        {
            item.PageName = item.PageName.Replace("Receipt", "ReceiptForm");
            item.ToReceiptPage = item.ToReceiptPage.Replace("Receipt", "ReceiptForm");
        }

        this.dtReceiptsList = lstInvoices.CopyToDataTable();
        gvReceiptsList.DataSource = this.dtReceiptsList;
        gvReceiptsList.DataBind();
    }

    private void LoadControls()
    {
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
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
        this.FilterVendors(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;


        foreach (DataControlField col in gvReceiptsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

    protected void lnkGenerate_Click(object sender, EventArgs e)
    { 
        try
        {
            var ID = (sender as LinkButton).CommandArgument;


            Response.Redirect("~/Sales/InvoiceForm.aspx?GenerateInoice_ID=" + ID, false);

        }
        catch (Exception ex)
        {
            
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }






    } 
}