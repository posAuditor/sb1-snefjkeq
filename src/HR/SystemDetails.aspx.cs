using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_SystemDetails : UICulturePage
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

    private int System_ID
    {
        get
        {
            if (ViewState["System_ID"] == null) return 0;
            return (int)ViewState["System_ID"];
        }

        set
        {
            ViewState["System_ID"] = value;
        }
    }

    private DataTable dtSystemDetails
    {
        get
        {
            if (Session["dtSystemDetails" + this.WinID] == null)
            {
                Session["dtSystemDetails" + this.WinID] = dc.usp_HR_SystemDetails_Select(this.System_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtSystemDetails" + this.WinID];
        }

        set
        {
            Session["dtSystemDetails" + this.WinID] = value;
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

    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.SetEditMode();
            if (!Page.IsPostBack)
            {
                this.CheckSecurity();
                this.LoadControls();
                this.FillSystem();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region Control Events

    protected void btnAddDetail_click(object sender, EventArgs e)
    {
        try
        {
            if (txtValueFrom.Text.ToIntOrDefault() > txtValueTo.Text.ToIntOrDefault())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.InvalidFromTo, string.Empty);
                return;
            }
            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtSystemDetails.NewRow();
                r["ID"] = this.dtSystemDetails.GetID("ID");

            }
            else
            {
                r = this.dtSystemDetails.Select("ID=" + this.EditID)[0];
            }

            r["FromValue"] = txtValueFrom.Text.ToIntOrDefault();
            r["ToValue"] = txtValueTo.Text.ToIntOrDefault();
            r["ResultValue"] = txtResultValue.Text.ToDecimalOrDefault();
            r["ResultValueType"] = ddlResultResultValueType.SelectedValue.ToByte();
            r["ResultValueTypeName"] = ddlResultResultValueType.SelectedItem.Text;

            if (this.EditID == 0) this.dtSystemDetails.Rows.Add(r);

            this.ClearDetailForm();
            this.BindDetailsGrid();
            txtValueFrom.Focus();
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
            txtValueFrom.Focus();
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
            DataRow r = this.dtSystemDetails.Select("ID=" + this.EditID.ToExpressString())[0];

            txtValueFrom.Text = r["FromValue"].ToExpressString();
            txtValueTo.Text = r["ToValue"].ToExpressString();
            txtResultValue.Text = r["ResultValue"].ToExpressString();
            ddlResultResultValueType.SelectedValue = r["ResultValueType"].ToExpressString();

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
            DataRow dr = this.dtSystemDetails.Select("ID=" + ID.ToExpressString())[0];
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


    protected void BtnSave_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(trans))
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
            Response.Redirect(PageLinks.HRSystemsList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods


    private void LoadControls()
    {

    }

    private void BindDetailsGrid()
    {
        gvDetails.DataSource = this.dtSystemDetails;
        gvDetails.DataBind();
    }

    private void ClearDetailForm()
    {
        txtValueFrom.Clear();
        txtValueTo.Clear();
        txtResultValue.Clear();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.System_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(System.Data.Common.DbTransaction trans)
    {
        if (dtSystemDetails.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0 && ddlSystemTypeSrch.SelectedValue != HRSystems.Allowances.ToByte().ToExpressString())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.SystemDetailsRequired, string.Empty);
            trans.Rollback();
            return false;
        }

        dc.usp_HR_Systems_Update(this.System_ID, null, txtFromalVacationAs.Text.ToDecimalOrDefault(), txtDailyAllowance.Text.ToDecimalOrDefault(), txtMonthlyAllowance.Text.ToDecimalOrDefault());
        foreach (DataRow r in this.dtSystemDetails.Rows)
        {
            if (r.RowState == DataRowState.Added)
            {
                dc.usp_HR_SystemDetails_Insert(this.System_ID, r["fromValue"].ToIntOrDefault(), r["ToValue"].ToIntOrDefault(), r["ResultValue"].ToDecimalOrDefault(), r["ResultValueType"].ToByte());
            }
            if (r.RowState == DataRowState.Modified)
            {
                dc.usp_HR_SystemDetails_Update(r["ID"].ToInt(), r["fromValue"].ToIntOrDefault(), r["ToValue"].ToIntOrDefault(), r["ResultValue"].ToDecimalOrDefault(), r["ResultValueType"].ToByte());
            }
            if (r.RowState == DataRowState.Deleted)
            {
                dc.usp_HR_SystemDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
            }
        }

        LogAction(Actions.Edit, lblName.Text, dc);
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.HRSystemDetails + "?ID=" + this.System_ID.ToExpressString(), PageLinks.HRSystemsList);
        return true;
    }

    private void FillSystem()
    {
        var System = dc.usp_HR_Systems_Select(this.System_ID, string.Empty, null).FirstOrDefault();
        txtFromalVacationAs.Text = System.FormalVacationAs.ToExpressString();
        txtDailyAllowance.Text = System.DailyAllowance.ToExpressString();
        txtMonthlyAllowance.Text = System.MonthlyAllowance.ToExpressString();
        lblName.Text = System.Name;
        ddlSystemTypeSrch.SelectedValue = System.SystemType.ToExpressString();

         txtDailyAllowance.Visible = txtMonthlyAllowance.Visible = System.SystemType == HRSystems.Allowances.ToByte();
        txtFromalVacationAs.Visible = System.SystemType == HRSystems.OverTime.ToByte();
        pnlAddItem.Visible = System.SystemType != HRSystems.Allowances.ToByte();

        this.dtSystemDetails = dc.usp_HR_SystemDetails_Select(this.System_ID).CopyToDataTable();
        this.BindDetailsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = MyContext.PageData.IsEdit;
    }

    #endregion
}