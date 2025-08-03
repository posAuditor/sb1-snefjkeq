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


public partial class Comp_GeneratePaymentMethode : UICulturePage
{

    XpressDataContext dc = new XpressDataContext();

    private DataTable dtSettingPointOS
    {
        get
        {
            return (DataTable)Session["dtSettingPointOS" + this.WinID];
        }

        set
        {
            Session["dtSettingPointOS" + this.WinID] = value;
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
    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        //gvGPaymentMethode.Columns[4].Visible = MyContext.PageData.IsEdit;
        //gvGPaymentMethode.Columns[5].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

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
    protected void gvGPaymentMethode_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_GenerateMethodePayment_Delete(gvGPaymentMethode.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvGPaymentMethode.DataKeys[e.RowIndex]["NamePayment"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvGPaymentMethode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvGPaymentMethode.PageIndex = e.NewPageIndex;
            gvGPaymentMethode.DataSource = this.dtSettingPointOS;
            gvGPaymentMethode.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvGPaymentMethode_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtSettingPointOS.Select("ID=" + gvGPaymentMethode.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["NamePayment"].ToExpressString();
            acSalesAccount.Value = dr["Account_ID"].ToExpressString();
            acBranch.Value = dr["Branch_ID"].ToExpressString();
            this.EditID = gvGPaymentMethode.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            if (this.EditID == 0) //insert
            {
                result = dc.usp_GenerateMethodePayment_Insert(txtName.TrimmedText, acSalesAccount.Value.ToInt(), null, 0, false, acBranch.Value.ToNullableInt());
            }
            else
            {
                result = dc.usp_GenerateMethodePayment_Update(this.EditID, txtName.TrimmedText, acSalesAccount.Value.ToInt(), null, 0, false, acBranch.Value.ToNullableInt());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            this.ClearForm();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    private void ClearForm()
    {
        txtName.Clear();
        acBranch.Clear();
        acSalesAccount.Clear();
    }


    protected void BtnClearNew_Click(object sender, EventArgs e)
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
    protected void close_popup_Click(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
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


    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        acSalesAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + ",,,false,false";

    }


    private void Fill()
    {
        this.dtSettingPointOS = dc.usp_GenerateMethodePayment_Select(null, txtNameSrch.TrimmedText).CopyToDataTable();
        gvGPaymentMethode.DataSource = this.dtSettingPointOS;
        gvGPaymentMethode.DataBind();
    }


    protected void acBranch_SelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        acSalesAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + ",,,false,false";
        mpeCreateNew.Show();
    }
}