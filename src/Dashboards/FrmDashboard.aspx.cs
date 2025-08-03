using System;
using XPRESS.Common;

public partial class Dashboards_FrmDashboard : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string GetCurrentCulture()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString();
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }
}