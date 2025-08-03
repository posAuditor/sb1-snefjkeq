using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Accounting_COAMergeAccount : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
               
              
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private DataTable dtCOATree
    {
        get
        {
            return (DataTable)Session["dtCOATree" + this.WinID];
        }

        set
        {
            Session["dtCOATree" + this.WinID] = value;
        }
    }

    private void FillCOATree(string name,string parentName)
    {
        

       

    }


    private void LoadControls()
    {
       
        acAccountFrom.ContextKey = this.MyContext.CurrentCulture.ToInt().ToExpressString();
        acAccountTo.ContextKey = this.MyContext.CurrentCulture.ToInt().ToExpressString();
        

       

       

    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }

    private void CustomPage()
    {
        
    }

    protected void gvchartOfAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {


        try
        {
            
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }




        //try
        //{
        //    gvchartOfAccount.PageIndex = e.NewPageIndex;
        //    this.FillCOATree();
        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        //}
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


    protected void gvchartOfAccount_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        
        
         
    }
     
    

    private int ChartOfAccount_ID
    {
        get
        {
            if (ViewState["ChartOfAccount_ID"] == null) return 0;
            return (int)ViewState["ChartOfAccount_ID"];
        }

        set
        {
            ViewState["ChartOfAccount_ID"] = value;
        }
    }

    
   
    protected void btnMerge_Click(object sender, EventArgs e)
    {

        try
        {
             
            switch (dc.MergeTowAccount(acAccountFrom.Value.ToInt(), acAccountTo.Value.ToInt()))
            {
                case 1:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي عميل", string.Empty);
                    return;
                case 2:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي مورد", string.Empty);
                    return;
                case 3:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي أصل", string.Empty);
                    return;
                case 4:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي تسوية او حركة مخزنية", string.Empty);
                    return;
                case 5:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي مشتريات", string.Empty);
                    return;
                case 6:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي مردود مشتريات", string.Empty);
                    return;
                case 7:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي  مبيعات", string.Empty);
                    return;
                case 8:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي مردود مبيعات", string.Empty);
                    return;
                case 9:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي  شيك", string.Empty);
                    return;
                case 10:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي قيد", string.Empty);
                    return;
                case 11:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي السلف", string.Empty);
                    return;
                case 12:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي  إعدادات عامة", string.Empty);
                    return;
                case 13:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي فئة الاصول  ", string.Empty);
                    return;
                case 14:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي  إعدادات نقطة البيع", string.Empty);
                    return;
                case 16:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي  بنك", string.Empty);
                    return;
                case 15:
                    UserMessages.Message(null, "لا يمكن دمج لانة مربوط بي حساب ابن", string.Empty);
                    return;
            }
             
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
    }
    protected void btnClearSrch_Click(object sender, EventArgs e)
    {

    }
}