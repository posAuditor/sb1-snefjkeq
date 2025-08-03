using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Inv_Stores : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtStores
    {
        get
        {
            return (DataTable)Session["dtStores" + this.WinID];
        }

        set
        {
            Session["dtStores" + this.WinID] = value;
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtNameSrch.Focus();
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
            txtNameSrch.Clear();
            acEmployeeNameSrch.Clear();
            if (acBranchSrch.Enabled) acBranchSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvStores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvStores.PageIndex = e.NewPageIndex;
            gvStores.DataSource = this.dtStores;
            gvStores.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvStores_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtStores.Select("ID=" + gvStores.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtAddress.Text = dr["Address"].ToExpressString();
            txtNotes.Text = dr["Notes"].ToExpressString();
            acBranch.Value = dr["Branch_ID"].ToExpressString();
            acBranch.Enabled = !dr["Islocked"].ToBoolean();
            acEmployeeName.Value = dr["Employee_ID"].ToExpressString();
            this.EditID = gvStores.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvStores_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Stores_Delete(gvStores.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvStores.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
            acBranch.Enabled = (MyContext.UserProfile.Branch_ID == null);
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToStringOrEmpty();
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
                result = dc.usp_Stores_Insert(txtName.TrimmedText, txtAddress.TrimmedText, acEmployeeName.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), txtNotes.TrimmedText, true);
            }
            else
            {
                result = dc.usp_Stores_update(this.EditID, txtName.TrimmedText, txtAddress.TrimmedText, acEmployeeName.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), txtNotes.TrimmedText, true);
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
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

    protected void acBranchSrch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acEmployeeNameSrch.ContextKey = "E," + acBranchSrch.Value + ",,";
            if (sender != null) this.FocusNextControl(sender);
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
            acEmployeeName.ContextKey  = "E," + acBranch.Value + ",,";
            if (sender != null) mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
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
        txtName.Clear();
        txtAddress.Clear();
        txtNotes.Clear();
        acEmployeeName.Clear();
        if (acBranch.Enabled) acBranch.Clear();
    }

    private void LoadControls()
    {
        acBranchSrch.ContextKey = acBranch.ContextKey = string.Empty;

        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = acBranchSrch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = acBranchSrch.Enabled = false;
        }
        this.acBranch_SelectedIndexChanged(null, null);
        this.acBranchSrch_SelectedIndexChanged(null, null);
    }

    private void Fill()
    {
        this.dtStores = dc.usp_Stores_Select(txtNameSrch.TrimmedText, acEmployeeNameSrch.Value.ToNullableInt(), acBranchSrch.Value.ToNullableInt()).CopyToDataTable();
        gvStores.DataSource = this.dtStores;
        gvStores.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvStores.Columns[3].Visible = MyContext.PageData.IsEdit;
        gvStores.Columns[4].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acBranch.IsRequired = acBranchSrch.Visible = acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvStores.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion
}
