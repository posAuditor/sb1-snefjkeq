using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public partial class Comp_BranchGroup : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtBrancheGroup
    {
        get
        {
            return (DataTable)Session["dtBrancheGroup" + this.WinID];
        }

        set
        {
            Session["dtBrancheGroup" + this.WinID] = value;
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

    private string ImageUrl
    {
        get
        {
            return (string)ViewState["ImageUrl"];
        }

        set
        {
            ViewState["ImageUrl"] = value;
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
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBrancheGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvBrancheGroup.PageIndex = e.NewPageIndex;
            gvBrancheGroup.DataSource = this.dtBrancheGroup;
            gvBrancheGroup.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBrancheGroup_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            var DefaultCurrency_ID = dc.usp_Company_Select().First().Currency_ID;

            DataRow dr = this.dtBrancheGroup.Select("ID=" + gvBrancheGroup.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["NameGroup"].ToExpressString();
            
            
            this.EditID = gvBrancheGroup.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
             
       
            


            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBrancheGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Branchs_delete(gvBrancheGroup.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvBrancheGroup.DataKeys[e.RowIndex]["NameGroup"].ToExpressString(), dc);
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
                BranchGroup bg = new BranchGroup();
                bg.DateCreated = DateTime.Now;
                bg.NameGroup = txtName.Text.ToExpressString();
                dc.BranchGroups.InsertOnSubmit(bg);
                dc.SubmitChanges();

            }
            else
            {
                var bg = dc.BranchGroups.Where(c => c.ID == this.EditID).FirstOrDefault();
                bg.DateCreated = DateTime.Now;
                bg.NameGroup = txtName.Text.ToExpressString();
                dc.SubmitChanges();

            }
            //if (result == -2)
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                
            //    mpeCreateNew.Show();
            //    return;
            //}


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

   

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        txtName.Clear();
        
    }

    private void LoadControls()
    {

    }

    private void Fill()
    {
        this.dtBrancheGroup = dc.BranchGroups.CopyToDataTable();
        gvBrancheGroup.DataSource = this.dtBrancheGroup;
        gvBrancheGroup.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvBrancheGroup.Columns[1].Visible = MyContext.PageData.IsEdit;
        gvBrancheGroup.Columns[2].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd && MyContext.UserProfile.Branch_ID == null;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
       
    }

    #endregion
    
}