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

public partial class Inv_BegingingInventory : UICulturePage
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
            if (Session["dtItems_InventoryBegin" + this.WinID] == null)
            {
                Session["dtItems_InventoryBegin" + this.WinID] = dc.usp_InventoryDocumentDetails_Select(null, null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_InventoryBegin" + this.WinID];
        }

        set
        {
            Session["dtItems_InventoryBegin" + this.WinID] = value;
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
                if (EditMode) this.FillInventoryDocument();
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
        ucNewBatchID.TargetControlID = pnlAddItem.Visible && lnkNewBatch.Visible ? lnkNewBatch.UniqueID : hfNewBatch.UniqueID;
        ucNewItemDescribed.TargetControlID = lnkNewDescribed.Visible ? lnkNewDescribed.UniqueID : hfNewDescribed.UniqueID;
    }

    #endregion

    #region Control Events

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (acStore.HasValue)
        {
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            string fileName = "InvDocDetails" + DateTime.Now.Ticks + Extension;
            FileUpload1.SaveAs(Server.MapPath("~\\uploads\\Excel\\" + fileName));
            string ExcelFilePath = Server.MapPath("~\\uploads\\Excel\\" + fileName);

            int counter = 0;
            string line;

            List<InvDocDetails> lst = new List<InvDocDetails>();


            System.IO.StreamReader file =
                new System.IO.StreamReader(@ExcelFilePath);
            while ((line = file.ReadLine()) != null)
            {
                var its = line.Split(',');
                lst.Add(new InvDocDetails()
                {
                    Barcode = its[0],
                    Qty = its[1].ToDecimalOrDefault()
                });
                counter++;
            }

            file.Close();

            var s = lst.GroupBy(f => f.Barcode).Select(x => x.Key).Aggregate((x, y) => x + "," + y).ToString();
            var lstItems = dc.GetItemsSpecifique(s).ToList();            
            foreach (var item in lstItems)
            {
                DataRow r = null;
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = acStore.Value;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.Id;
                r["UnitCost"] = item.CostPrice;
                r["Quantity"] = lst.Where(x => x.Barcode == item.Barcode).Sum(c => c.Qty);
                r["QtyInNumber"] = DBNull.Value;
                r["Uom_ID"] = item.UOM_ID;
                r["UOMName"] = item.AttName;
                r["Batch_ID"] = DBNull.Value;
                r["BatchName"] = string.Empty;
                r["TotalTax"] = 0;
                r["Notes"] = string.Empty;
                r["StoreName"] = acStore.Text;
                r["ItemName"] = item.Name;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = item.NameCategory;
                r["Total"] = 0;
                r["GrossTotal"] = 0;

                r["StoreBranch_ID"] = DBNull.Value;


                this.dtItems.Rows.Add(r);




            }

            this.BindItemsGrid();

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
            if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
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
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
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
            //r["ItemDescribed_ID"] = acItemDescribed.Value;
            //r["ItemDescribed"] = acItemDescribed.Text;

            r["StoreBranch_ID"] = DBNull.Value;
            var StoreBranch_ID = (from data in dc.usp_Stores_Select(string.Empty, null, null) where data.ID == acStore.Value.ToInt() select data.Branch_ID).FirstOrDefault();
            if (StoreBranch_ID != null) r["StoreBranch_ID"] = StoreBranch_ID;

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

    protected void btnAddItemGroup_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.EditID != 0) return;

            if (acBatchID.HasValue)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NoGroupWithBatch, string.Empty);
                return;
            }

            var Items = dc.usp_Items_SelectByCategory(acCategory.Value.ToInt(), false);

            foreach (var item in Items)
            {

                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = acStore.Value;
                r["Category_ID"] = acCategory.Value;
                r["Item_ID"] = item.ID;
                r["UnitCost"] = txtCost.Text;
                r["Quantity"] = txtQty.Text;
                r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
                r["Uom_ID"] = acUnit.Value;
                r["UOMName"] = acUnit.Text;
                r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
                r["BatchName"] = acBatchID.Text;
                r["TotalTax"] = 0;
                r["Notes"] = txtItemNotes.Text;
                r["StoreName"] = acStore.Text;
                r["ItemName"] = item.Name;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = acCategory.Text;
                r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
                r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
                r["Total"] = 0;
                r["GrossTotal"] = 0;

                r["StoreBranch_ID"] = DBNull.Value;
                var StoreBranch_ID = (from data in dc.usp_Stores_Select(string.Empty, null, null) where data.ID == acStore.Value.ToInt() select data.Branch_ID).FirstOrDefault();
                if (StoreBranch_ID != null) r["StoreBranch_ID"] = StoreBranch_ID;

                this.dtItems.Rows.Add(r);
            }

            this.ClearItemForm();
            this.BindItemsGrid();
            if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            DataView dv = new DataView(this.dtItems);
            dv.RowFilter = string.Format("ItemName like '%{0}%'", txtItemNameSrch.TrimmedText.EscapeLikeValue());
            gvItems.DataSource = dv;
            gvItems.DataBind();
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
            doc.Load(Server.MapPath("~\\Reports\\InventoryBegin_Print.rpt"));
            doc.SetParameterValue("@Doc_ID", this.InventoryDocument_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(null, "InventoryBegin"), false);
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
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        acItemDescribed.Clear();
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
        ucNewItemDescribed.ItemID = acItem.Value;
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
        acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppositeAccount.Value = COA.Capital.ToInt().ToExpressString();

        acBranch.ContextKey = string.Empty;
        //acBranch.Enabled = false;

        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
        }

        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        txtOperationDate.Text = MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
    }

    private void BindItemsGrid()
    {
        this.Calculate();
        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();
    }

    private void ClearItemForm()
    {
        //acStore.Clear();
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
        //else
        //{
        //    var doc = dc.usp_InventoryDocument_Select(MyContext.UserProfile.Branch_ID, string.Empty, null, null, string.Empty, null, 0, 2).FirstOrDefault();
        //    if (doc != null)
        //    {
        //        Response.Redirect(PageLinks.BeginingInventory + "?ID=" + doc.ID.ToExpressString(), false);
        //    }
        //}

    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
            trans.Rollback();
            return false;
        }
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

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 2;
        int Serial_ID = 0;
        int Detail_ID = 0;
        if (!this.EditMode)
        {

            this.InventoryDocument_ID = dc.usp_InventoryDocument_Insert(null, txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, lblTotal.Text.ToDecimal(), 0, 0,
                                                        txtUserRefNo.Text, this.DocRandomString, EntryType, acOppositeAccount.Value.ToInt(), null,null,null);
            if (this.InventoryDocument_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, null, r["QtyInNumber"].ToDecimalOrDefault(), r["ItemDescribed_ID"].ToNullableInt());
                    if (IsApproving)
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
                }
                if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), null, DocumentsTableTypes.InventoryDocument_Begining.ToInt(), this.InventoryDocument_ID);
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_InventoryDocument_Update(this.InventoryDocument_ID, null, txtOperationDate.Text.ToDate(),
                                                        ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                        approvedBY_ID, null, null,
                                                        txtNotes.Text, lblTotal.Text.ToDecimal(), 0, 0,
                                                        txtUserRefNo.Text, acOppositeAccount.Value.ToInt(), null,null,null);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_InventoryDocumentDetails_Insert(this.InventoryDocument_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, null, r["QtyInNumber"].ToDecimalOrDefault(), r["ItemDescribed_ID"].ToNullableInt());
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_InventoryDocumentDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), null, null, null, r["QtyInNumber"].ToDecimalOrDefault());
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

                if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), null, DocumentsTableTypes.InventoryDocument_Begining.ToInt(), this.InventoryDocument_ID);
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, string.Empty, dc);
            }
        }

        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.BeginingInventory + "?ID=" + this.InventoryDocument_ID.ToExpressString());
        return true;
    }

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();

        var GroupedBegingingInv = from data in this.dtItems.AsEnumerable()
                                  where data.RowState != DataRowState.Deleted
                                  group data by data.Field<int?>("StoreBranch_ID") into GroupedInv
                                  select new { Branch_ID = GroupedInv.Key, BracnhTotal = GroupedInv.Sum(x => x.Field<decimal>("Total")) };

        foreach (var Branch in GroupedBegingingInv)
        {
            int Result = dc.usp_Operation_Insert(Branch.Branch_ID, txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.BeginingInventory.ToInt(), company.Currency_ID, Branch.BracnhTotal, Branch.BracnhTotal, 1, txtNotes.Text);

            //Opposite Account
            dc.usp_OperationDetails_Insert(Result, acOppositeAccount.Value.ToInt(), 0, Branch.BracnhTotal, 0, Branch.BracnhTotal, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Begining.ToInt());

            //Inventory
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, Branch.BracnhTotal, 0, Branch.BracnhTotal, 0, null, this.InventoryDocument_ID, DocumentsTableTypes.InventoryDocument_Begining.ToInt());
        }
    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        int result = 0;
        decimal ItemTotal = row["Total"].ToDecimal();
        result = dc.usp_ICJ_BegingingInventory(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), ItemTotal, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.InventoryDocument_Begining.ToInt(), this.InventoryDocument_ID, Detail_ID, row["ItemDescribed_ID"].ToNullableInt());
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
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = InventoryDoc.OperationDate.Value.ToString("d/M/yyyy");
        txtUserRefNo.Text = InventoryDoc.UserRefNo;
        lblTotal.Text = InventoryDoc.TotalAmount.ToExpressString();
        txtNotes.Text = InventoryDoc.Notes;
        acOppositeAccount.Value = InventoryDoc.OppositeAccount_ID.ToExpressString();
        this.DocRandomString = InventoryDoc.DocRandomString;
        lblCreatedBy.Text = InventoryDoc.CreatedByName;
        lblApprovedBy.Text = InventoryDoc.ApprovedBYName;
        this.ImgStatus = ((DocStatus)InventoryDoc.DocStatus_ID).ToExpressString();
        pnlAddItem.Visible = (InventoryDoc.DocStatus_ID == 1);
        btnApprove.Visible = (InventoryDoc.DocStatus_ID == 1) && MyContext.PageData.IsApprove && MyContext.UserProfile.Branch_ID == null;
        btnCancelApprove.Visible = !btnApprove.Visible && (InventoryDoc.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;


        btnSave.Visible = (InventoryDoc.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        btnPrint.Visible = MyContext.PageData.IsPrint;
        gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (InventoryDoc.DocStatus_ID == 1);

        this.dtItems = dc.usp_InventoryDocumentDetails_Select(this.InventoryDocument_ID, acBranch.Value.ToNullableInt()).CopyToDataTable();
        this.BindItemsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove && MyContext.UserProfile.Branch_ID == null;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
    }

    private void Calculate()
    {
        this.Total = 0;
        foreach (DataRow r in this.dtItems.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
        }
        lblTotal.Text = this.Total.ToString(NbrHashNeerDecimal);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        lnkNewBatch.Visible = MyContext.Features.BatchIDEnabled;

        foreach (DataControlField col in gvItems.Columns)
        {
            if (col.ItemStyle.CssClass == "BatchCol") col.Visible = MyContext.Features.BatchIDEnabled;
        }
    }

    #endregion

    private void FilterItemsDescribed()
    {
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        acItemDescribed.Clear();
        this.FilterItemsData();


    }


    protected void ucNewItemDescribed_OnNewItemDescribedCreated(string itemdescribedid, string price, int attid)
    {
        try
        {
            acItemDescribed.Refresh();
            acItemDescribed.Value = itemdescribedid.ToExpressString();
            this.FocusNextControl(lnkNewDescribed);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        { 
            dc.CancelInventoryDocumentBS_Approvel(this.InventoryDocument_ID);
           // trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.BeginingInventory + "?ID=" + this.InventoryDocument_ID.ToExpressString(), PageLinks.BeginingInventory, PageLinks.BeginingInventory);

        }
        catch (Exception ex)
        {
            //trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}

public class InvDocDetails
{
    public decimal Qty { get; set; }
    public string Barcode { get; set; }
}