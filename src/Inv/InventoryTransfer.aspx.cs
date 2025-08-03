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

public partial class Inv_InventoryTransfer : UICulturePage
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

    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvItems.FormatNumber = MyContext.FormatNumber;
            this.SetEditMode();
            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                this.SetDefaults();
                if (EditMode) this.FillInventoryDocument();
            }

            ucNav.SourceDocTypeType_ID = 7;
            ucNav.EntryType = 1;
            ucNav.Res_ID = this.InventoryDocument_ID;
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
        Response.Redirect(PageLinks.InventoryTransfer);
    }
    void ucNav_btnHandlerSearch(string strValue)
    {
        RefillForm(strValue);
    }


    private void RefillForm(string strValue)
    {
        if (!string.IsNullOrEmpty(strValue))
        {
            this.InventoryDocument_ID = strValue.ToInt();
            // this.EditMode = strValue.ToInt();
            this.EditMode = true;

            this.DocRandomString.ToString();
            this.CheckSecurity();
            this.LoadControls();
            this.SetDefaults();
            if (EditMode) this.FillInventoryDocument();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void acItemDescribed_OnSelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        try
        {

            this.ShowAvailableQty();

            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }
    protected void acStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (this.EditID == 0) this.FilterItems();
            this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acToStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FilterItems();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FilterItemsDescribed();
            this.FilterItemsData();
            this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acItem.Clear();
            if (txtBarcode.IsNotEmpty)
            {
                var item = dc.usp_Items_Select(txtBarcode.TrimmedText, string.Empty, null, null, null, true).FirstOrDefault();
                if (item != null) acItem.Value = item.ID.ToExpressString();
            }
            this.acItem_SelectedIndexChanged(null, null);
            if (acItem.HasValue) acToStore.AutoCompleteFocus(); else this.FocusNextControl(acStore);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddItem_click(object sender, EventArgs e)
    {
        try
        {
            if (acStore.Value == acToStore.Value)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.SameFromToStore, string.Empty);
                return;
            }
            if (txtQty.Text.ToDecimal() > lblAvailableQty.Text.ToDecimal())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough, string.Empty);
                return;
            }

            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");

            }
            else
            {
                r = this.dtItems.Select("ID=" + this.EditID)[0];
            }

            r["Store_ID"] = acStore.Value;
            r["ToStore_ID"] = acToStore.Value;
            r["Category_ID"] = acCategory.Value;
            r["Item_ID"] = acItem.Value;
            r["Quantity"] = txtQty.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
            r["BatchName"] = acBatchID.Text;
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            r["ToStoreName"] = acToStore.Text;
            r["ItemName"] = acItem.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
            r["ItemDescribed_ID"] = acItemDescribed.Value.ToInt();
            r["Total"] = 0;
            r["GrossTotal"] = 0;
            r["ActualQty"] = lblAvailableQty.Text.ToDecimalOrDefault();
            if (this.EditID == 0) this.dtItems.Rows.Add(r);

            this.ClearItemForm();
            this.BindItemsGrid();
            this.FocusNextControl(acStore);
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearItem_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearItemForm();
            acStore.AutoCompleteFocus();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItems.PageIndex = e.NewPageIndex;
            this.BindItemsGrid();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditID = gvItems.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtItems.Select("ID=" + this.EditID.ToExpressString())[0];

            acStore.Value = r["Store_ID"].ToExpressString();
            acToStore.Value = r["ToStore_ID"].ToExpressString();
            acCategory.Value = r["Category_ID"].ToExpressString();
            this.acCategory_SelectedIndexChanged(null, null);
            acItem.Value = r["Item_ID"].ToExpressString();
            this.acItem_SelectedIndexChanged(null, null);
            txtQty.Text = r["Quantity"].ToExpressString();
            txtQtyInNumber.Text = r["QtyInNumber"].ToExpressString();
            acUnit.Value = r["Uom_ID"].ToExpressString();
            acBatchID.Value = r["Batch_ID"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
            if (r["ProductionDate"].ToExpressString() != string.Empty) txtProductionDate.Text = r["ProductionDate"].ToDate().Value.ToString("d/M/yyyy"); ;
            if (r["ExpirationDate"].ToExpressString() != string.Empty) txtExpirationDate.Text = r["ExpirationDate"].ToDate().Value.ToString("d/M/yyyy");
            this.ShowAvailableQty(); //Called Twice but important
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvItems.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtItems.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            this.ClearItemForm();
            this.BindItemsGrid();
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
            if (sender != null)
            {
                this.FocusNextControl(sender);
                if (((Control)sender).ID == "acToBranch") acStore.AutoCompleteFocus();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acBatchID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtProductionDate.Clear();
            txtExpirationDate.Clear();
            if (acBatchID.HasValue)
            {
                var batch = dc.usp_ItemsBatch_Select(acBatchID.Value.ToInt(), acItem.Value.ToInt(), null, null, false).FirstOrDefault();
                if (batch.ProductionDate.HasValue) txtProductionDate.Text = batch.ProductionDate.Value.ToString("d/M/yyyy");
                if (batch.ExpirationDate.HasValue) txtExpirationDate.Text = batch.ExpirationDate.Value.ToString("d/M/yyyy");
            }
            this.ShowAvailableQty();
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
            dc.usp_InventoryDocument_Cancel(this.InventoryDocument_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InventoryTransfer + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryTransfersList, PageLinks.InventoryTransfer);
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
            doc.Load(Server.MapPath("~\\Reports\\InventoryTransfer_Print.rpt"));
            doc.SetParameterValue("@Doc_ID", this.InventoryDocument_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "InventoryTransfer"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var Branches = dc.usp_Branchs_Select(string.Empty, null).ToList();
        var FromBranchAccount_ID = Branches.Where(x => x.ID == acBranch.Value.ToInt()).First().BranchChartOfAccount_ID;
        var ToBranchAccount_ID = Branches.Where(x => x.ID == acToBranch.Value.ToInt()).First().BranchChartOfAccount_ID;

        if (this.CalculatedSalesCost > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryTransfer.ToInt(), company.Currency_ID, this.CalculatedSalesCost, this.CalculatedSalesCost, 1, txtNotes.Text);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            //حساب الفرع
            dc.usp_OperationDetails_Insert(Result, ToBranchAccount_ID, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            Result = dc.usp_Operation_Insert(acToBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryTransfer.ToInt(), company.Currency_ID, this.CalculatedSalesCost, this.CalculatedSalesCost, 1, txtNotes.Text);

            //المخزون مدين
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());

            //حساب الفرع
            dc.usp_OperationDetails_Insert(Result, FromBranchAccount_ID, 0, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Transfer.ToInt());
        }
    }

    private void FilterByBranchAndCurrency()
    {
        try
        {
            acStore.ContextKey = string.Empty + acBranch.Value;
            acToStore.ContextKey = string.Empty + acToBranch.Value;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = "," + acCategory.Value + ",";//" + acStore.Value;
        acItem.Clear();
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        acItemDescribed.Clear();
        this.FilterItemsData();
    }

    private void FilterItemsData()
    {
        txtProductionDate.Clear();
        txtExpirationDate.Clear();
        acUnit.Clear();
        txtBarcode.Clear();
        txtItemNotes.Clear();
        txtQty.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + ",";
        acUnit.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtBarcode.Text = item.Barcode;
            acUnit.Value = item.UOM_ID.Value.ToExpressString();
            acCategory.Value = item.Category_ID.ToExpressString();
        }
    }

    private void LoadControls()
    {
        this.dtItems = null;
        acBranch.ContextKey = string.Empty;
        acToBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acToBranch.Value = acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
            acToBranch.Enabled = false;
        }

        acdrivers.ContextKey = string.Empty;
        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }

    private void BindItemsGrid()
    {
        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();
        acToBranch.Enabled = acBranch.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        // acStore.Clear();
        //acToStore.Clear();
        acCategory.Clear();
        this.FilterItems();
        this.acItem_SelectedIndexChanged(null, null);
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.InventoryDocument_ID = Request["ID"].ToInt();
        }

    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }
        this.CalculatedSalesCost = 0;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 1;
        int Serial_ID = DocSerials.InventoryTransfer.ToInt();
        int Detail_ID = 0;
        if (!this.EditMode)
        {

            this.InventoryDocument_ID = dc.usp_InventoryDocument_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, 0, 0, 0,
                                                        txtUserRefNo.Text, this.DocRandomString, EntryType, null, acToBranch.Value.ToNullableInt(), acdrivers.Value.ToNullableInt(),null);
            if (this.InventoryDocument_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), null, r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, r["ToStore_ID"].ToInt(), r["QtyInNumber"].ToDecimalOrDefault(), null);
                    if (IsApproving)
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
                }
                //if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), null, DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocument_ID);
                if (IsApproving && acBranch.Value.ToStringOrEmpty() != acToBranch.Value.ToStringOrEmpty()) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_InventoryDocument_Update(this.InventoryDocument_ID, acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, 0, 0, 0,
                                                        txtUserRefNo.Text, null, acToBranch.Value.ToNullableInt(), acdrivers.Value.ToNullableInt(),null);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), null, r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, r["ToStore_ID"].ToInt(), r["QtyInNumber"].ToDecimalOrDefault(), null);
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_InventoryDocumentDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), null, r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, r["ToStore_ID"].ToInt(), r["QtyInNumber"].ToDecimalOrDefault());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_InventoryDocumentDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }

                    if (IsApproving && r.RowState != DataRowState.Deleted)
                    {
                        Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }

                // if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), null, DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocument_ID);
                if (IsApproving && acBranch.Value.ToStringOrEmpty() != acToBranch.Value.ToStringOrEmpty()) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }


        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InventoryTransfer + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryTransfersList, PageLinks.InventoryTransfer);
        return true;
    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        decimal? Cost = 0;
        int result = dc.usp_ICJ_InvTransfer(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), row["ToStore_ID"].ToInt(), acToBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Transfer.ToInt(), this.InventoryDocument_ID, Detail_ID,null, ref Cost);
        this.CalculatedSalesCost += Cost.Value;
        if (result == -32)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -4)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
            return false;
        }
        if (result == -5)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantMoveDocCreditsItems + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
            return false;
        }
        return true;
    }

    private void FillInventoryDocument()
    {

        var InventoryDoc = dc.usp_InventoryDocument_SelectByID(this.InventoryDocument_ID).FirstOrDefault();
        acBranch.Value = InventoryDoc.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = InventoryDoc.OperationDate.Value.ToString("d/M/yyyy");
        txtUserRefNo.Text = InventoryDoc.UserRefNo;
        txtNotes.Text = InventoryDoc.Notes;
        txtSerial.Text = InventoryDoc.Serial;

        ucNav.SetText = InventoryDoc.Serial;
        acdrivers.Value = InventoryDoc.Driver_ID.ToStringOrEmpty();

        acToBranch.Value = InventoryDoc.ToBranch_ID.ToStringOrEmpty();
        this.DocRandomString = InventoryDoc.DocRandomString;
        lblCreatedBy.Text = InventoryDoc.CreatedByName;
        lblApprovedBy.Text = InventoryDoc.ApprovedBYName;
        this.ImgStatus = ((DocStatus)InventoryDoc.DocStatus_ID).ToExpressString();
        btnPrint.Visible = MyContext.PageData.IsPrint;
        pnlAddItem.Visible = (InventoryDoc.DocStatus_ID == 1);
        btnCancel.Visible = (InventoryDoc.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (InventoryDoc.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = !btnApprove.Visible && (InventoryDoc.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;

        btnSave.Visible = (InventoryDoc.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (InventoryDoc.DocStatus_ID == 1);

        this.dtItems = dc.usp_InventoryDocumentDetails_Select(this.InventoryDocument_ID, null).CopyToDataTable();
        this.BindItemsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;


        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
    }

    private void ShowAvailableQty()
    {
        //lblAvailableQty.Text = string.Empty;
        //if (acItem.HasValue)
        //{
        //    decimal Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null).Value;
        //    lblAvailableQty.Text = Qty.ToExpressString();
        //    lblAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;

        //    Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acToStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null).Value;
        //    lblToStoreAvailableQty.Text = Qty.ToExpressString();
        //    lblToStoreAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        //}


        lblAvailableQty.Text = string.Empty;


        if (acItemDescribed.HasValue)
        {
            var itemQtyFromInput = acItem.Value.ToInt();

            // var funItemQty = dc.fun_ItemDescribedQty(itemQtyFromInput, acItemDescribed.Value, acStore.Value.ToNullableInt(), null, null, null, MyContext.UserProfile.Branch_ID);
            var funItemQty = dc.fun_ItemDQty(itemQtyFromInput, acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null, acItemDescribed.Value.ToNullableInt(), acBranch.Value.ToNullableInt());

            if (funItemQty != null)
            {
                decimal qty = funItemQty.Value;
                lblAvailableQty.Text = qty.ToExpressString();
                lblAvailableQty.ForeColor = qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            }
        }
        else if (acItem.HasValue)
        {
            var itemQtyFromInput = acItem.Value.ToInt();

            var funItemQty = dc.fun_ItemQty(itemQtyFromInput, acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null);
            if (funItemQty != null)
            {
                decimal qty = funItemQty.Value;
                lblAvailableQty.Text = qty.ToExpressString();
                lblAvailableQty.ForeColor = qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            }
        }


    }

    private void CustomPage()
    {
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        acToBranch.IsRequired = acBranch.IsRequired = acToBranch.Visible = acBranch.Visible = MyContext.Features.BranchesEnabled;

        foreach (DataControlField col in gvItems.Columns)
        {
            if (col.ItemStyle.CssClass == "BatchCol") col.Visible = MyContext.Features.BatchIDEnabled;
        }
    }

    private void SetDefaults()
    {
        if (Page.IsPostBack) return;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var defaults = dc.usp_CashBillDefaults_Select(acBranch.Value.ToNullableInt(), MyContext.UserProfile.Contact_ID).First();

        if (company.AutoDate.Value) txtOperationDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");



        txtOperationDate.Enabled = !company.LockAutoDate.Value;
    }


    private void FilterItemsDescribed()
    {
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        acItemDescribed.Clear();
        this.FilterItemsData();


    }

    #endregion
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            dc.CancelInventoryDocumentTransfer_Approvel(this.InventoryDocument_ID);
            //trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InventoryTransfer + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryTransfersList, PageLinks.InventoryTransfer);

        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}