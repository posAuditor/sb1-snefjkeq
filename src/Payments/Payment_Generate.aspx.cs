using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Payments_Payment_Generate : UICulturePage
{

    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        ifViewer.Attributes.Remove("src");
        if (!Page.IsPostBack)
        {
            if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
            this.LoadControls();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    protected void btnShow_click(object sender, EventArgs e)
    {
        try
        {
            if (int.Parse(ddlDocType.SelectedValue) == 0 || int.Parse(ddlDocType.SelectedValue) == 1 || int.Parse(ddlDocType.SelectedValue) == 4 || int.Parse(ddlDocType.SelectedValue) == 5)
            {
                dc.usp_ReCalulatePaymentIn(acBranch.Value.ToNullableInt(),
                                      txtDateFrom.Text.ToDate() ?? MyContext.FiscalYearStartDate,
                                      txtDateTo.Text.ToDate() ?? DateTime.Now.Date,
                                      acnameEmp.Value.ToNullableInt(), ddlDocType.SelectedValue.ToNullableInt());
            }

            if (int.Parse(ddlDocType.SelectedValue) == 2 || int.Parse(ddlDocType.SelectedValue) == 3 || int.Parse(ddlDocType.SelectedValue) == 6 || int.Parse(ddlDocType.SelectedValue) == 7)
            {
                dc.usp_ReCalulatePaymentOut(acBranch.Value.ToNullableInt(),
                                      txtDateFrom.Text.ToDate() ?? MyContext.FiscalYearStartDate,
                                      txtDateTo.Text.ToDate() ?? DateTime.Now.Date,
                                      acnameEmp.Value.ToNullableInt(), ddlDocType.SelectedValue.ToNullableInt());
            }

            
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnExportToExcel_click(object sender, EventArgs e)
    {
        try
        {
          
            
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
            
            txtDateFrom.Clear();
            txtDateTo.Clear();
             
            if (acBranch.Enabled) acBranch.Clear();
            
            
           
            ifViewer.Attributes.Remove("src");

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
 
  
    private void LoadControls()
    {
        
       
        acBranch.ContextKey = string.Empty;
        
        acnameEmp.ContextKey = ",,";
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToString();
            acBranch.Enabled = false;
        }
        
    }

 

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;

         
    }
}