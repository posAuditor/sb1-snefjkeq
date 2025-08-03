using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Sales_SettingsPayment : UICulturePage
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



    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                Fill();

            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    private void Fill()
    {
        this.dtSettingPointOS = dc.usp_Payment_Methode().CopyToDataTable();
        gvSettingPointOS.DataSource = this.dtSettingPointOS;
        gvSettingPointOS.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);

    }

    private void LoadControls()
    {
        acAtm.ContextKey = acVisa.ContextKey = acMaster.ContextKey = acCash.ContextKey = MyContext.CurrentCulture.ToByte().ToExpressString();
        acBranch.ContextKey = string.Empty;
    }
    protected void btnSaveNew_Click(object sender, EventArgs e)
    {

        if (EditID == 0)
        {
            PaymentMethode payMethode = new PaymentMethode ();
            payMethode.Account_Atm_Id = acAtm.Value.ToIntOrDefault();
            payMethode.Account_Master_Id = acMaster.Value.ToIntOrDefault();
            payMethode.Account_Treasury_Id = acCash.Value.ToIntOrDefault();
            payMethode.Account_Visa_Id = acVisa.Value.ToIntOrDefault();
            payMethode.Branch_ID = acBranch.Value.ToIntOrDefault();
            dc.PaymentMethodes.InsertOnSubmit(payMethode);
        }
        else
        {
            var payMethode = dc.PaymentMethodes.Where(x => x.Id == this.EditID).FirstOrDefault();
            if (payMethode!=null)
            {
                payMethode.Account_Atm_Id = acAtm.Value.ToIntOrDefault();
                payMethode.Account_Master_Id = acMaster.Value.ToIntOrDefault();
                payMethode.Account_Treasury_Id = acCash.Value.ToIntOrDefault();
                payMethode.Account_Visa_Id = acVisa.Value.ToIntOrDefault();
                payMethode.Branch_ID = acBranch.Value.ToIntOrDefault();
            }
        }

        dc.SubmitChanges();
        Fill();
        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);


    }



    protected void gvSettingPointOS_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSettingPointOS.PageIndex = e.NewPageIndex;
            gvSettingPointOS.DataSource = this.dtSettingPointOS;
            gvSettingPointOS.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSettingPointOS_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtSettingPointOS.Select("ID=" + gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];



            acAtm.Value = dr["Account_Atm_Id"].ToExpressString();
            acVisa.Value = dr["Account_Visa_Id"].ToExpressString();
            acMaster.Value = dr["Account_Master_Id"].ToExpressString();
            acCash.Value = dr["Account_Treasury_Id"].ToExpressString();
            acBranch.Value = dr["Branch_ID"].ToExpressString();




            this.EditID = gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSettingPointOS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            
           var  result = dc.PaymentMethodes.Where( x=>x.Id== gvSettingPointOS.DataKeys[e.RowIndex]["ID"].ToInt()).FirstOrDefault();
           dc.PaymentMethodes.DeleteOnSubmit(result);
            //if (result == -6)
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
            //    return;
            //}
            LogAction(Actions.Delete, gvSettingPointOS.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


}