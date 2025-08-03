using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Sales_SalesRep : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtSalesRepAreas
    {
        get
        {
            return (DataTable)Session["dtSalesRepAreas" + this.WinID];
        }

        set
        {
            Session["dtSalesRepAreas" + this.WinID] = value;
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

    #region Page events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.Fill();
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


    protected void FilterReps(object sender, EventArgs e)
    {
        try
        {
            acRepNameSrch.ContextKey = "E," + acBranchSrch.Value + ",,";
            if (sender != null) this.FocusNextControl(sender);
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
            this.Fill();
            acRepNameSrch.AutoCompleteFocus();
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
            acRepNameSrch.Clear();
            acAreaSrch.Clear();
            if (acBranchSrch.Enabled) acBranchSrch.Clear();
            this.FilterReps(null, null);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSalesRepAreas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSalesRepAreas.PageIndex = e.NewPageIndex;
            gvSalesRepAreas.DataSource = this.dtSalesRepAreas;
            gvSalesRepAreas.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSalesRepAreas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtSalesRepAreas.Select("ID=" + gvSalesRepAreas.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acRepName.Value = dr["Contact_ID"].ToExpressString();
            acArea.Value = dr["Area_ID"].ToExpressString();
            chkIsDefault.Checked = dr["IsDefault"].ToBoolean();
            this.EditID = gvSalesRepAreas.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSalesRepAreas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_SalesRepAreas_Delete(gvSalesRepAreas.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvSalesRepAreas.DataKeys[e.RowIndex]["ContactName"].ToExpressString() + ":" + gvSalesRepAreas.DataKeys[e.RowIndex]["AreaName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            this.ClearForm();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            if (this.EditID == 0) //insert
            {
                result = dc.usp_SalesRepAreas_Insert(acRepName.Value.ToInt(), acArea.Value.ToInt(), chkIsDefault.Checked);
            }
            else
            {
                result = dc.usp_SalesRepAreas_Update(this.EditID, acRepName.Value.ToInt(), acArea.Value.ToInt(), chkIsDefault.Checked);
                //result = dc.usp_Areas_Update(this.EditID, txtName.TrimmedText);
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.RepAreaExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            if (result == -3)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.AreaDefaultExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acRepName.Text + ":" + acArea.Text, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        acArea.Clear();
        acRepName.Clear();
        chkIsDefault.Checked = false;
    }

    private void LoadControls()
    {
        acBranchSrch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranchSrch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranchSrch.Enabled = false;
        }
        acArea.ContextKey = acAreaSrch.ContextKey = string.Empty;
        acRepName.ContextKey = "E," + MyContext.UserProfile.Branch_ID.ToNullableInt() + ",,";
        this.FilterReps(null, null);
    }

    private void Fill()
    {
        this.dtSalesRepAreas = dc.usp_SalesRepAreas_Select(acRepNameSrch.Value.ToNullableInt(), acAreaSrch.Value.ToNullableInt(), acBranchSrch.Value.ToNullableInt()).CopyToDataTable();
        gvSalesRepAreas.DataSource = this.dtSalesRepAreas;
        gvSalesRepAreas.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        //gvSalesRepAreas.Columns[3].Visible = MyContext.PageData.IsDelete;
        gvSalesRepAreas.Columns[4].Visible = MyContext.PageData.IsDelete;

        gvSalesRepAreas.Columns[3].Visible = MyContext.PageData.IsEdit;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acBranchSrch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
}
