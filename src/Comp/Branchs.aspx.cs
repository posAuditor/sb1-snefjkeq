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

public partial class Comp_Branchs : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtBranches
    {
        get
        {
            return (DataTable)Session["dtBranches" + this.WinID];
        }

        set
        {
            Session["dtBranches" + this.WinID] = value;
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
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUploadImage);
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

    protected void gvBranches_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvBranches.PageIndex = e.NewPageIndex;
            gvBranches.DataSource = this.dtBranches;
            gvBranches.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBranches_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            var DefaultCurrency_ID = dc.usp_Company_Select().First().Currency_ID;

            DataRow dr = this.dtBranches.Select("ID=" + gvBranches.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtAddress.Text = dr["Address"].ToExpressString();
            txtTelephone.Text = dr["Phone"].ToExpressString();
            txtMobile.Text = dr["Mobile"].ToExpressString();
            txtFax.Text = dr["Fax"].ToExpressString();
            if (dr["logourl"] != null) imgLogo.ImageUrl = dr["logourl"].ToExpressString();
            this.EditID = gvBranches.DataKeys[e.NewSelectedIndex]["ID"].ToInt();

            acDefaultCashAccount_ID.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + this.EditID + ",," + COA.CashOnHand.ToInt().ToExpressString() + ",true";
            acDefaultCustomer.ContextKey = "C," + this.EditID + "," + DefaultCurrency_ID.ToExpressString() + ",";
            acDefaultStore.ContextKey = string.Empty + this.EditID;
            acDefaultCashAccount_ID.Enabled = acDefaultCustomer.Enabled = acDefaultStore.Enabled = true;
            acDefaultCashAccount_ID.Value = dr["DefaultCashAccount_ID"].ToStringOrEmpty();
            acDefaultCustomer.Value = dr["DefaultCustomer_ID"].ToStringOrEmpty();
            acDefaultStore.Value = dr["DefaultStore_ID"].ToStringOrEmpty();



            var id = gvBranches.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            var branches = dc.Branches.Where(x => x.ID == id).ToList();
            if (branches.Any())
            {
                var brch = branches.First();
                txtPrinterName.Text = brch.PrinterName;
                txtNbrPrint.Text = brch.NbrPrint != null ? brch.NbrPrint.Value.ToString() : "1";
                txtPassword.Text = brch.PasswordSupervisor.ToExpressString();
                txtConfirmPassword.Text = brch.PasswordSupervisor.ToExpressString();
                txtBranchNbrTax.Text = brch.NbrTax.ToExpressString();
                txtTradeRegistration.Text = brch.TradeRegistration.ToExpressString();
                acBrancheGroup.Value = brch.ParentGroup_Id.ToStringOrEmpty();

            }


            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBranches_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Branchs_delete(gvBranches.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvBranches.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
            acDefaultCashAccount_ID.Enabled = acDefaultCustomer.Enabled = acDefaultStore.Enabled = false;
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
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            System.Data.Linq.Binary imageLogo = null;

            if (this.ImageUrl != null) imageLogo = new System.Data.Linq.Binary(System.IO.File.ReadAllBytes(Server.MapPath("~\\uploads\\" + this.ImageUrl)));


            if (this.EditID == 0) //insert
            {
                result = dc.usp_Branchs_Insert(txtName.TrimmedText, txtAddress.TrimmedText, txtTelephone.TrimmedText, txtMobile.TrimmedText, txtFax.TrimmedText, imageLogo, true, this.ImageUrl);

                var branches = dc.Branches.Where(x => x.ID == result).ToList();
                if (branches.Any())
                {
                    var brch = branches.First();
                    brch.PrinterName = txtPrinterName.Text.ToExpressString();
                    brch.NbrPrint = txtNbrPrint.Text.ToInt();
                    brch.PasswordSupervisor = txtPassword.Text;
                    brch.NbrTax = txtBranchNbrTax.Text;
                    brch.TradeRegistration = txtTradeRegistration.Text;
                    brch.ParentGroup_Id = acBrancheGroup.Value.ToNullableInt();
                    dc.SubmitChanges();
                }

            }
            else
            {
                result = dc.usp_Branchs_Update(this.EditID, txtName.TrimmedText, txtAddress.TrimmedText, txtTelephone.TrimmedText, txtMobile.TrimmedText, txtFax.TrimmedText, imageLogo, this.ImageUrl, acDefaultCashAccount_ID.Value.ToNullableInt(), acDefaultCustomer.Value.ToNullableInt(), acDefaultStore.Value.ToNullableInt());

                var branches = dc.Branches.Where(x => x.ID == this.EditID).ToList();
                if (branches.Any())
                {
                    var brch = branches.First();
                    brch.PrinterName = txtPrinterName.Text.ToExpressString();
                    brch.NbrPrint = txtNbrPrint.Text.ToInt();
                    brch.PasswordSupervisor = txtPassword.Text;
                    brch.NbrTax = txtBranchNbrTax.Text;
                    brch.TradeRegistration = txtTradeRegistration.Text;
                    brch.ParentGroup_Id = acBrancheGroup.Value.ToNullableInt();
                    dc.SubmitChanges();

                }

            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }


            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
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

    protected void btnUploadImage_Click(object sender, EventArgs e)
    {
        try
        {
            if (fpLogo.HasFile)
            {
                string fileName = null;
                do
                {
                    fileName = Guid.NewGuid() + Path.GetExtension(fpLogo.PostedFile.FileName);
                }
                while (File.Exists(Server.MapPath("~\\uploads\\" + fileName)));

                Bitmap originalBMP = new Bitmap(fpLogo.FileContent);
                Bitmap newBMP = new Bitmap(originalBMP, 200, 150);
                Graphics objGraphics = Graphics.FromImage(newBMP);
                objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                objGraphics.DrawImage(originalBMP, 0, 0, 200, 150);
                newBMP.Save(Server.MapPath("~\\uploads\\" + fileName)); ;
                imgLogo.ImageUrl = "~/uploads/" + fileName;
                this.ImageUrl = fileName;
            }
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
        txtAddress.Clear();
        txtMobile.Clear();
        txtTelephone.Clear();
        txtFax.Clear();
        acDefaultCashAccount_ID.Clear();
        acDefaultCustomer.Clear();
        acDefaultStore.Clear();
        imgLogo.ImageUrl = "~/Images/no_photo.png";
        this.ImageUrl = null;
    }

    private void LoadControls()
    {
        acBrancheGroup.ContextKey = string.Empty;
    }

    private void Fill()
    {
        this.dtBranches = dc.usp_Branchs_Select(txtNameSrch.TrimmedText, MyContext.UserProfile.Branch_ID).CopyToDataTable();
        gvBranches.DataSource = this.dtBranches;
        gvBranches.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvBranches.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvBranches.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd && MyContext.UserProfile.Branch_ID == null;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acDefaultCashAccount_ID.Visible = acDefaultCustomer.Visible = acDefaultStore.Visible = MyContext.Features.WorkingMode != WorkingMode.HR.ToByte();
    }

    protected void btnDeleteImage_Click(object sender, EventArgs e)
    {
        var brn = dc.Branches.Where(c => c.ID == this.EditID).FirstOrDefault();
        brn.Logo = null;
        brn.LogoURL = string.Empty;
        dc.SubmitChanges();
        this.Fill();
    }
    #endregion
}