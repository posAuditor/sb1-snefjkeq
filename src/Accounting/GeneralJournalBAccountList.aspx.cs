using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Reflection;

public partial class Accounting_GeneralJournalBAccountList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtGeneralJournalList
    {
        get
        {
            return (DataTable)Session["dtGeneralJournalList" + this.WinID];
        }

        set
        {
            Session["dtGeneralJournalList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);

                //  gvGeneralJournalList.Columns[8].Visible = this.MyContext.PageData.IsPrint;
                this.LoadControls();
                this.FillJournalEntriesList();
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

    protected void chkHide_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow Row = ((GridViewRow)((CheckBox)sender).Parent.Parent);
            int Operation_ID = gvGeneralJournalList.DataKeys[Row.RowIndex]["ID"].ToInt();
            bool chk = ((CheckBox)Row.FindControl("chkHide")).Checked;
            dc.usp_Operation_Hide(Operation_ID, chk);
            //this.FillJournalEntriesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int Operation_ID = gvGeneralJournalList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvGeneralJournalList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\GeneralJournalEntry_Print.rpt"));
            doc.SetParameterValue("@Operation_ID", Operation_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "GeneralJournalEntry"), false);
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
            this.FillJournalEntriesList();
            acOperationType.AutoCompleteFocus();
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
            txtDateFromSrch.Focus();
            if (acBranch.Enabled) acBranch.Clear();
            acOperationType.Clear();
            this.FillJournalEntriesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvGeneralJournalList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvGeneralJournalList.PageIndex = e.NewPageIndex;
            gvGeneralJournalList.DataSource = this.dtGeneralJournalList;
            gvGeneralJournalList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvGeneralJournalList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            GridViewRow row = gvGeneralJournalList.Rows[e.NewSelectedIndex];
            HtmlTableRow tdOperationDetails = (HtmlTableRow)row.FindControl("tdOperationDetails");
            tdOperationDetails.Visible = !tdOperationDetails.Visible;
            ((LinkButton)row.FindControl("lnkbtnimgSelect")).CssClass = tdOperationDetails.Visible ? "grid-collapse" : "grid-expand";
            if (tdOperationDetails.Visible)
            {
                GridView gvDetails = (GridView)row.FindControl("gvOperationDetails");
                gvDetails.DataSource = dc.usp_JournalEntryDetails_Select(gvGeneralJournalList.DataKeys[e.NewSelectedIndex]["ID"].ToInt(), MyContext.CurrentCulture.ToByte()).OrderBy(x => x.CreditAmount);
                gvDetails.DataBind();
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillJournalEntriesList()
    {
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        this.dtGeneralJournalList = dc.usp_GeneralJournal_Select(acBranch.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtSerialsrch.TrimmedText, Currency_ID, 1, MyContext.CurrentCulture.ToByte(),null).CopyToDataTable();
        gvGeneralJournalList.DataSource = this.dtGeneralJournalList;
        gvGeneralJournalList.DataBind();
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        acOperationType.ContextKey = string.Empty;
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
        txtDateFromSrch.Text = DateTime.Now.ToString("d/M/yyyy");
        txtDateToSrch.Text = DateTime.Now.ToString("d/M/yyyy");
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvGeneralJournalList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }

    }

    #endregion


    protected void lnkbtnimgSelect_Click(object sender, EventArgs e)
    {

    }

    private void MapProp(object sourceObj, object targetObj)
    {
        Type T1 = sourceObj.GetType();
        Type T2 = targetObj.GetType();

        PropertyInfo[] sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        PropertyInfo[] targetProprties = T2.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var sourceProp in sourceProprties)
        {
            object osourceVal = sourceProp.GetValue(sourceObj, null);
            int entIndex = Array.IndexOf(targetProprties, sourceProp);
            if (entIndex >= 0)
            {
                var targetProp = targetProprties[entIndex];
                targetProp.SetValue(targetObj, osourceVal);
            }
        }
    }
    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        var id = int.Parse((sender as LinkButton).CommandArgument);
        var tnd = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
        var op = dc.Operations.Where(x => x.ID == id).FirstOrDefault();
        var opdetail = dc.OperationDetails.Where(x => x.Operation_ID == id).ToList();
        string Serial = string.Empty;
        var IsApproving = false;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        decimal ZERO_Foreign = acOperationType.Value.ToIntOrDefault() == OperationTypes.CurrenciesDiff.ToInt() ? 0 : 1;


        var Operation_ID = dc.usp_JournalEntry_Insert(op.Branch_ID, DateTime.Now.Date, ref Serial, DocStatus_ID, op.OperationType_ID, op.Currency_ID, op.Amount * op.Ratio.ToDecimal(), op.Amount * ZERO_Foreign, op.Ratio.ToDecimal(), op.Description, this.MyContext.UserProfile.Contact_ID, approvedBY_ID, null, DateTime.Now, ApproveDate, null, op.CostCenter_ID, op.UserRefNo, tnd);
        if (Operation_ID > 0)
        {
            foreach (var item in opdetail)
            {

                dc.usp_JournalEntryDetails_Insert(Operation_ID, item.Account_ID, item.DebitAmount * op.Ratio.ToDecimal(), item.CreditAmount * op.Ratio.ToDecimal(), item.DebitForeignAmount * ZERO_Foreign, item.CreditForeignAmount.ToDecimal() * ZERO_Foreign, item.Description.ToExpressString(), item.CostCenter_ID);
                //if (IsApproving)
                //{
                //    decimal Amount = (r[this.DebitColumnName].ToDecimal() != 0 ? r[this.DebitColumnName].ToDecimal() : r[this.CreditColumnName].ToDecimal() * -1) * txtRatio.Text.ToDecimal();
                //    dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), r["CostCenter_ID"].ToNullableInt(), txtOperationDate.Text.ToDate(), Amount, this.Operation_ID, DocumentsTableTypes.Operation.ToInt(), r["Description"].ToExpressString());
                //}
            }
            // if (IsApproving) dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.TotalDebit * txtRatio.Text.ToDecimal(), this.Operation_ID, DocumentsTableTypes.Operation.ToInt(), txtNotes.Text);
            LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
        }
        UserMessages.Message(null, "تم إنشاء نسخة من ذا القيد مسودة يمكنك التعديل عليه من قائمة القيود اليومية", string.Empty);

        Response.Redirect("JournalEntry.aspx?ID=" + Operation_ID.ToExpressString());

        //this.FillJournalEntriesList();
    }
    protected void Unnamed_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int Operation_ID = gvGeneralJournalList.DataKeys[Index]["ID"].ToInt();
            var lstOpDetail = dc.OperationDetails.Where(x => x.Operation_ID == Operation_ID).ToList();
            var Op = dc.Operations.Where(x => x.ID == Operation_ID).FirstOrDefault();
            if (Op != null)
            {
                if (lstOpDetail.Any())
                {
                    dc.OperationDetails.DeleteAllOnSubmit(lstOpDetail.AsEnumerable());
                    dc.Operations.DeleteOnSubmit(Op);
                    dc.SubmitChanges();
                }
            }

            UserMessages.Message(this.MyContext.PageData.PageTitle, "تم الحذف", string.Empty);
            this.FillJournalEntriesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}