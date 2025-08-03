using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Accounting_FrmJournalEntrySelect : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string GetCurrentCulture()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString();
    }

    public int GetContactID()
    {
        return (MyContext.UserProfile.HasPermissionShow == false ? 0 : MyContext.UserProfile.Contact_ID);
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }
}