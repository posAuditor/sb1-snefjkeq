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
using System.IO;
using OfficeOpenXml;

public partial class Inv_InventoryCorrection : UICulturePage
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

    private decimal IncomingCost
    {
        get
        {
            if (ViewState["IncomingCost"] == null) return 0;
            return (decimal)ViewState["IncomingCost"];
        }

        set
        {
            ViewState["IncomingCost"] = value;
        }
    }

    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvItems.FormatNumber = MyContext.FormatNumber;
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUpload);
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(Button2);
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
            ucNav.EntryType = 0;

            ucNav.Res_ID = this.InventoryDocument_ID;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
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
        ucNewBatchID.TargetControlID = pnlAddItem.Visible && lnkNewBatch.Visible ? lnkNewBatch.UniqueID : hfNewBatch.UniqueID;
    }

    #endregion

    #region Control Events


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
            txtCost.Text = (dc.fun_GetItemDefaultCostByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt())).Value.ToString(NbrHashNeerDecimal);
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
            if (acItem.HasValue) this.FocusNextControl(txtCost); else this.FocusNextControl(acStore);
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

            if (acStore.HasValue && acBranch.HasValue)
            {



                decimal Difference = txtQty.Text.ToDecimal() - lblAvailableQty.Text.ToDecimalOrDefault();

                if ((lblAvailableQty.Text.ToDecimalOrDefault() + Difference) < 0)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough, string.Empty);
                    return;
                }


                if (acBatchID.Value != null)
                {
                    int Batch_ID = Convert.ToInt32(acBatchID.Value.ToIntOrDBNULL());
                    DateTime ProductionDate = Convert.ToDateTime(txtProductionDate.Text.ToDateOrDBNULL());
                    DateTime ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text.ToDateOrDBNULL());
                    dc.usp_ItemsBatch_Update(Batch_ID, ProductionDate, ExpirationDate);

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
                r["Category_ID"] = acCategory.Value;
                r["Item_ID"] = acItem.Value;
                r["UnitCost"] = txtCost.Text;
                r["Quantity"] = txtQty.Text;
                r["Uom_ID"] = acUnit.Value;
                r["UOMName"] = acUnit.Text;
                r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
                r["BatchName"] = acBatchID.Text;
                r["TotalTax"] = 0;
                r["Notes"] = txtItemNotes.Text;
                r["StoreName"] = acStore.Text;
                r["ItemName"] = acItem.Text;
                r["Barcode"] = txtBarcode.Text;
                r["CategoryName"] = acCategory.Text;
                r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
                r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
                r["Total"] = 0;
                r["GrossTotal"] = 0;
                r["ActualQty"] = lblAvailableQty.Text.ToDecimalOrDefault();
                r["Differrence"] = Difference;
                r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
                if (this.EditID == 0) this.dtItems.Rows.Add(r);

                this.ClearItemForm();
                this.BindItemsGrid();
                this.FocusNextControl(acStore);
                this.EditID = 0;
            }
            else
            {
                UserMessages.Message(null, "لا يصلح الاستراد الا بعد اختيار الفرع والمخزن", string.Empty);

            }
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
            acCategory.Value = r["Category_ID"].ToExpressString();
            this.acCategory_SelectedIndexChanged(null, null);
            acItem.Value = r["Item_ID"].ToExpressString();
            this.acItem_SelectedIndexChanged(null, null);
            txtCost.Text = r["UnitCost"].ToExpressString();
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
            if (sender != null) this.FocusNextControl(sender);
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
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InventoryCorrection + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryCorrectionsList, PageLinks.InventoryCorrection);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text.ToDecimalOrDefault() <= 0)
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                else
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewBatchID_NewBatchCreated(string BatchName, string ProductionDate, string ExiprationDate, int Batch_ID)
    {
        try
        {
            if (!acBatchID.Enabled) return;
            acBatchID.Refresh();
            acBatchID.Value = Batch_ID.ToExpressString();
            this.acBatchID_SelectedIndexChanged(null, null);
            this.FocusNextControl(lnkNewBatch);
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
            doc.Load(Server.MapPath("~\\Reports\\InventoryCorrection_Print.rpt"));
            doc.SetParameterValue("@Doc_ID", this.InventoryDocument_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "InventoryCorrection"), false);
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
            acStore.ContextKey = string.Empty + acBranch.Value;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = "," + acCategory.Value + ",";
        acItem.Clear();
        this.FilterItemsData();
    }

    private void FilterItemsData()
    {
        txtCost.Clear();
        txtProductionDate.Clear();
        txtExpirationDate.Clear();
        acUnit.Clear();
        txtBarcode.Clear();
        txtItemNotes.Clear();
        txtQty.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + ",";
        ucNewBatchID.ItemID = acItem.Value;
        acUnit.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = item.Cost.ToString(NbrHashNeerDecimal);
            txtBarcode.Text = item.Barcode;
            acUnit.Value = item.UOM_ID.Value.ToExpressString();
            acCategory.Value = item.Category_ID.ToExpressString();
        }
    }

    private void LoadControls()
    {
        this.dtItems = null;
        acBranch.ContextKey = string.Empty;
        acOppositeAccount.ContextKey = MyContext.CurrentCulture.ToByte().ToExpressString();
        acOppositeAccount.Value = COA.CostOfSoldGoods.ToInt().ToExpressString();
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }

        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }

    private void BindItemsGrid()
    {
        this.Calculate();
        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();
        acBranch.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        //acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
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
        //if (this.Total <= 0)
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}

        if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        this.CalculatedSalesCost = 0;
        this.IncomingCost = 0;

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 0;
        int Serial_ID = DocSerials.InventoryCorrection.ToInt();
        int Detail_ID = 0;
        if (!this.EditMode)
        {

            this.InventoryDocument_ID = dc.usp_InventoryDocument_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, lblTotal.Text.ToDecimal(), 0, 0,
                                                        txtUserRefNo.Text, this.DocRandomString, EntryType, acOppositeAccount.Value.ToInt(), null, null,null);
            if (this.InventoryDocument_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ActualQty"].ToDecimal(), r["Differrence"].ToDecimal(), null, r["QtyInNumber"].ToDecimalOrDefault(), null);
                    if (IsApproving)
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
                }
                //if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID);
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_InventoryDocument_Update(this.InventoryDocument_ID, acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, lblTotal.Text.ToDecimal(), 0, 0,
                                                        txtUserRefNo.Text, acOppositeAccount.Value.ToInt(), null, null,null);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ActualQty"].ToDecimal(), r["Differrence"].ToDecimal(), null, r["QtyInNumber"].ToDecimalOrDefault(), null);
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_InventoryDocumentDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ActualQty"].ToDecimal(), r["Differrence"].ToDecimal(), null, r["QtyInNumber"].ToDecimalOrDefault());
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

                //if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID);
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }


        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InventoryCorrection + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryCorrectionsList, PageLinks.InventoryCorrection);
        return true;
    }

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();

        if (this.CalculatedSalesCost > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryCorrectionOut.ToInt(), company.Currency_ID, this.CalculatedSalesCost, this.CalculatedSalesCost, 1, txtNotes.Text);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, acOppositeAccount.Value.ToInt(), this.CalculatedSalesCost, 0, this.CalculatedSalesCost, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());
        }

        if (this.IncomingCost > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.InventoryCorrectionIN.ToInt(), company.Currency_ID, this.IncomingCost, this.IncomingCost, 1, txtNotes.Text);

            //المخزون مدين
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.IncomingCost, 0, this.IncomingCost, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, acOppositeAccount.Value.ToInt(), 0, this.IncomingCost, 0, this.IncomingCost, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Correction.ToInt());
        }


    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        decimal? Cost = 0;
        int result = 0;
        if (row["Differrence"].ToDecimal() < 0)
        {
            result = dc.usp_ICJ_InvCorr_Out(txtOperationDate.Text.ToDate(), row["Differrence"].ToDecimal() * -1, row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID, Detail_ID, ref Cost);
            this.CalculatedSalesCost += Cost.Value;
        }
        else if (row["Differrence"].ToDecimal() > 0)
        {
            result = dc.usp_ICJ_InvCorr_IN(txtOperationDate.Text.ToDate(), row["Differrence"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Correction.ToInt(), this.InventoryDocument_ID, Detail_ID, null);
            this.IncomingCost += row["Total"].ToDecimal();
        }
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

        if (result == -666)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.VeryLowCost + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
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
        lblTotal.Text = InventoryDoc.TotalAmount.ToExpressString();
        txtNotes.Text = InventoryDoc.Notes;
        txtSerial.Text = InventoryDoc.Serial;
        ucNav.SetText = InventoryDoc.Serial;
        acOppositeAccount.Value = InventoryDoc.OppositeAccount_ID.ToExpressString();
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

    private void Calculate()
    {
        this.Total = 0;
        foreach (DataRow r in this.dtItems.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * Math.Abs(r["Differrence"].ToDecimal());
            this.Total += r["UnitCost"].ToDecimal() * Math.Abs(r["Differrence"].ToDecimal());
        }
        lblTotal.Text = this.Total.ToString(NbrHashNeerDecimal);
    }

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;
        if (acItem.HasValue)
        {
            decimal Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null).Value;
            lblAvailableQty.Text = Qty.ToExpressString();
            lblAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        }
    }

    private void CustomPage()
    {
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        lnkNewBatch.Visible = MyContext.Features.BatchIDEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;

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


        if (defaults.UserHasStore.Value && defaults.DefaultStore_ID.HasValue) acStore.Value = defaults.DefaultStore_ID.ToExpressString();
        if (acStore.HasValue) this.acStore_SelectedIndexChanged(null, null);

        txtOperationDate.Enabled = !company.LockAutoDate.Value;

        acStore.Enabled = !((MyContext.UserProfile.Store_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
    }


    #endregion
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            // dc.usp_CancelInvoice_Approvel(this.Invoice_ID);
            //trans.Commit();
            dc.CancelInventoryDocumentCorrect_Approvel(this.InventoryDocument_ID);
            trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InventoryCorrection + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.InventoryCorrectionsList, PageLinks.InventoryCorrection);

        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnGetItemsDataNegative_Click(object sender, EventArgs e)
    {

        if (acStore.HasValue)
        {

            var list = dc.usp_GetQuantityNegativeForCLoseYear(acStore.Value.ToInt()).ToList();
            foreach (var item in list)
            {

                decimal Difference = 0 - item.Quantity.Value;

                DataRow r = null;
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = item.Store_ID;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.Item_ID;
                r["UnitCost"] = item.UnitCost;
                r["Quantity"] = 0;
                r["Uom_ID"] = item.UOM_ID;
                r["UOMName"] = item.UOMName;
                r["Batch_ID"] = DBNull.Value;
                r["BatchName"] = "";
                r["TotalTax"] = 0;
                r["Notes"] = string.Empty;
                r["StoreName"] = item.StoreName;
                r["ItemName"] = item.ItemName;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = item.CategoryName;
                r["ProductionDate"] = DBNull.Value;
                r["ExpirationDate"] = DBNull.Value;
                r["Total"] = 0;
                r["GrossTotal"] = 0;
                r["ActualQty"] = item.Quantity.Value;
                r["Differrence"] = Difference;
                r["QtyInNumber"] = DBNull.Value;
                if (this.EditID == 0) this.dtItems.Rows.Add(r);
                this.Calculate();
            }
            this.BindItemsGrid();
        }
        else
        {
            UserMessages.Message(null, "لا يصلح الاستراد الا بعد اختيار الفرع ", string.Empty);

        }
    }


    protected void btnGetItemsDatain_Click(object sender, EventArgs e)
    {

        if (acStore.HasValue)
        {

            var list = dc.usp_GetQuantityINForCLoseYear(acStore.Value.ToInt()).ToList();
            foreach (var item in list)
            {

                decimal Difference = 0 - item.Quantity.Value;

                DataRow r = null;
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = item.Store_ID;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.Item_ID;
                r["UnitCost"] = item.UnitCost;
                r["Quantity"] = 0;
                r["Uom_ID"] = item.UOM_ID;
                r["UOMName"] = item.UOMName;
                r["Batch_ID"] = DBNull.Value;
                r["BatchName"] = "";
                r["TotalTax"] = 0;
                r["Notes"] = string.Empty;
                r["StoreName"] = item.StoreName;
                r["ItemName"] = item.ItemName;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = item.CategoryName;
                r["ProductionDate"] = DBNull.Value;
                r["ExpirationDate"] = DBNull.Value;
                r["Total"] = 0;
                r["GrossTotal"] = 0;
                r["ActualQty"] = item.Quantity.Value;
                r["Differrence"] = Difference;
                r["QtyInNumber"] = DBNull.Value;
                if (this.EditID == 0) this.dtItems.Rows.Add(r);
                this.Calculate();
            }
            this.BindItemsGrid();
        }
        else
        {
            UserMessages.Message(null, "لا يصلح الاستراد الا بعد اختيار الفرع ", string.Empty);

        }
    }







    private DataTable dtFile
    {
        get
        {
            return (DataTable)Session["dtFile" + this.WinID];
        }

        set
        {
            Session["dtFile" + this.WinID] = value;
        }
    }

    private DataTable dtAllAdded
    {
        get
        {
            if (Session["dtAllAdded" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("ID", typeof(int));
                dtallTaxes.Columns.Add("Name", typeof(string));
                dtallTaxes.Columns.Add("Name_ID", typeof(int));
                dtallTaxes.Columns.Add("Value", typeof(string));
                dtallTaxes.Columns.Add("Value_ID", typeof(int));
                Session["dtAllAdded" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllAdded" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Invoice" + this.WinID] = value;
        }
    }

    private string NameFile
    {
        get
        {
            if (ViewState["NameFile"] == null) return string.Empty;
            return (string)ViewState["NameFile"];
        }

        set
        {
            ViewState["NameFile"] = value;
        }
    }

    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.DataSource = dtFile;

        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        var lstHeader = new List<ExportClass>();

        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        string fileName = "ExcelData" + DateTime.Now.Ticks + Extension;
        FileUpload1.SaveAs(Server.MapPath("~\\uploads\\Excel\\" + fileName));
        string ExcelFilePath = Server.MapPath("~\\uploads\\Excel\\" + fileName);

        dtFile = new DataTable();
        byte[] bin = File.ReadAllBytes(ExcelFilePath);
        using (MemoryStream stream = new MemoryStream(bin))

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        try
                        {
                            var rowExcp = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
                            int oo = 0;
                            foreach (var cell in rowExcp)
                            {
                                var t = new ExportClass() { ID = oo++, Name = cell.Text };
                                lstHeader.Add(t);
                            }

                            if (rbHDR.SelectedIndex == 0)
                            {
                                foreach (var cell in rowExcp)
                                {
                                    dtFile.Columns.Add(new DataColumn(cell.Text));
                                }
                            }
                            else
                            {
                                for (int k = worksheet.Dimension.Start.Column; k <= worksheet.Dimension.End.Column; k++)
                                {
                                    dtFile.Columns.Add(new DataColumn("F" + k.ToString()));

                                }
                            }

                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];

                                DataRow newRow = dtFile.NewRow();

                                foreach (var cell in row)
                                {
                                    newRow[cell.Start.Column - 1] = cell.Text;
                                }

                                dtFile.Rows.Add(newRow);
                            }
                        }
                        catch (Exception exDetails)
                        {

                            UserMessages.Message(null, "يجب ان يكون شيت واحد", string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                UserMessages.Message(null, "يجب ان يكون نوع الاكسل .xlsx", string.Empty);
            }

        ddlPropertiesValue.DataSource = lstHeader;
        ddlPropertiesValue.DataValueField = "ID";
        ddlPropertiesValue.DataTextField = "Name";
        ddlPropertiesValue.DataBind();


        GridView1.DataSource = dtFile;
        GridView1.DataBind();
    }

    private void BindItemsGrid1()
    {

        gvExportList.DataSource = this.dtAllAdded;
        gvExportList.DataBind();

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlPropertiesValue.SelectedValue))
        {
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID='" + ddlProperties.SelectedValue + "'");
            if (filteredRows.Length > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return;
            }

            DataRow r = null;
            r = this.dtAllAdded.NewRow();
            r["ID"] = this.dtAllAdded.GetID("ID");
            r["Name"] = ddlProperties.SelectedItem.Text; ;
            r["Name_ID"] = ddlProperties.SelectedValue.ToInt();
            r["Value"] = ddlPropertiesValue.SelectedItem.Text;
            r["Value_ID"] = ddlPropertiesValue.SelectedValue.ToInt();
            this.dtAllAdded.Rows.Add(r);
            this.BindItemsGrid1();
        }
        else
        {
            UserMessages.Message(null, "بيانات غير مكتملة", string.Empty);
        }
    }

    protected void btnImportFromFile_Click(object sender, EventArgs e)
    {
        ImportCorrection();
    }


    private bool ImportCorrection()
    {

        var its = dc.Items.ToList();
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];

            //Qauntity Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");

            var obj = filteredRows[0];
            var indexQty = (obj["Value_ID"].ToExpressString()).ToInt();

            //Barcode Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=0");

            var objStore = filteredStoreRows[0];
            var index = (objStore["Value_ID"].ToExpressString()).ToInt();

            var valueBarcode = row[index].ToExpressString();
            var valueQty = row[indexQty].ToDecimalOrDefault();


            var item = its.Where(c => c.Barcode == valueBarcode || c.Name.Contains(valueBarcode)).FirstOrDefault();


            if (item != null)
            {
                var AvailableQty = dc.fun_ItemQty(item.ID, acStore.Value.ToIntOrDefault(), null, item.UOM_ID, null).ToDecimalOrDefault();
                decimal Difference = valueQty.ToDecimalOrDefault() - AvailableQty;


                //decimal Difference = 0;
                //if (ActQty > valueQty.ToDecimalOrDefault())
                //{
                //    Difference = -(ActQty - valueQty.ToDecimalOrDefault());
                //}
                //else
                //{
                //    Difference = (ActQty - valueQty.ToDecimalOrDefault());
                //}


                DataRow r = null;
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = acStore.Value; ;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.ID;
                r["UnitCost"] = item.DefaultPrice;
                r["Quantity"] = valueQty.ToDecimalOrDefault();
                r["Uom_ID"] = item.UOM_ID;
                //r["UOMName"] = item.UOMName;
                r["Batch_ID"] = DBNull.Value;
                r["BatchName"] = "";
                r["TotalTax"] = 0;
                r["Notes"] = string.Empty;
                r["StoreName"] = acStore.Text.ToExpressString();
                r["ItemName"] = item.Name;
                r["Barcode"] = item.Barcode;
                //r["CategoryName"] = item.CategoryName;
                r["ProductionDate"] = DBNull.Value;
                r["ExpirationDate"] = DBNull.Value;
                r["Total"] = 0;
                r["GrossTotal"] = 0;
                r["ActualQty"] = AvailableQty;
                r["Differrence"] = Difference;
                r["QtyInNumber"] = DBNull.Value;
                if (this.EditID == 0) this.dtItems.Rows.Add(r);

            }
            this.Calculate();
            BindItemsGrid();
        }
        return true;


    }


    protected void Button2_Click(object sender, EventArgs e)
    {

        if (acStore.HasValue)
        {
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            string fileName = "InvDocDetails" + DateTime.Now.Ticks + Extension;
            FileUpload1.SaveAs(Server.MapPath("~\\uploads\\Excel\\" + fileName));
            string ExcelFilePath = Server.MapPath("~\\uploads\\Excel\\" + fileName);

            int counter = 0;
            string line;

            List<InvDocDetails1> lst = new List<InvDocDetails1>();



            //  var itemms = dc.Items.ToList();

            System.IO.StreamReader file =
                new System.IO.StreamReader(@ExcelFilePath);
            while ((line = file.ReadLine()) != null)
            {
                var its = line.Split(',');
                lst.Add(new InvDocDetails1()
                {
                    Barcode = its[0],
                    Qty = its[1].ToDecimalOrDefault()
                });
                counter++;
            }



            file.Close();
            if (lst==null || !lst.Any())
            {
                UserMessages.Message(null, "لا يوجد اي  باركود من ملف الاستراد في النظام", string.Empty);
                return;

            }
            var s = lst.GroupBy(f => f.Barcode).Select(x => x.Key).Aggregate((x, y) => x + "," + y).ToString();
            // var lstItems = dc.GetItemsSpecifique(s).ToList();
            var lstItems = dc.usp_GetItemsQithQuantityAvailbale(s, acStore.Value.ToInt()).ToList();






            foreach (var item in lstItems)
            {



                //var item = its.Where(c => c.Barcode == valueBarcode || c.Name.Contains(valueBarcode)).FirstOrDefault();


                if (item != null)
                {

                    var AvailableQty = item.Qty.ToDecimalOrDefault();
                    //  var AvailableQty = dc.fun_ItemQty(item.Id, 2, null, item.UOM_ID, null).ToDecimalOrDefault();
                    decimal Difference = lst.Where(x => x.Barcode == item.Barcode).Sum(c => c.Qty).ToDecimalOrDefault() - AvailableQty;



                    DataRow r = null;
                    r = this.dtItems.NewRow();
                    r["ID"] = this.dtItems.GetID("ID");
                    r["Store_ID"] = 2;
                    r["Category_ID"] = item.Category_ID;
                    r["Item_ID"] = item.Id;
                    r["UnitCost"] = item.defaultPrice;
                    r["Quantity"] = lst.Where(x => x.Barcode == item.Barcode).Sum(c => c.Qty);
                    r["Uom_ID"] = item.UOM_ID;
                    //r["UOMName"] = item.UOMName;
                    r["Batch_ID"] = DBNull.Value;
                    r["BatchName"] = "";
                    r["TotalTax"] = 0;
                    r["Notes"] = string.Empty;
                    r["StoreName"] = acStore.Text.ToExpressString();
                    r["ItemName"] = item.Name;
                    r["Barcode"] = item.Barcode;
                    //r["CategoryName"] = item.CategoryName;
                    r["ProductionDate"] = DBNull.Value;
                    r["ExpirationDate"] = DBNull.Value;
                    r["Total"] = 0;
                    r["GrossTotal"] = 0;
                    r["ActualQty"] = AvailableQty;
                    r["Differrence"] = Difference;
                    r["QtyInNumber"] = DBNull.Value;
                    if (this.EditID == 0) this.dtItems.Rows.Add(r);

                }
            }

            this.Calculate();
            BindItemsGrid();

            var ListBarcodeNotContains = lst.Where(c => !lstItems.Select(f => f.Barcode).Contains(c.Barcode)).Select(v => v.Barcode).ToList();
            if (ListBarcodeNotContains.Any())
            {

                BarcodeNC.Text = ListBarcodeNotContains.Aggregate((x, y) => x + "\n" + y);
                mpexxx.Show();
            }


        }
        else
        {
            UserMessages.Message(null, "لا يصلح الاستراد الا بعد اختيار الفرع والمخزن", string.Empty);

        }
    }

}

public class InvDocDetails1
{
    public decimal Qty { get; set; }
    public string Barcode { get; set; }
}