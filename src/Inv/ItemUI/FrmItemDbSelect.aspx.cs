using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Inv_ItemUI_FrmItemDbSelect : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            XpressDataContext dc = new XpressDataContext();
            var results = dc.usp_ItemsCategoriesPadded_Select().ToList();
            acCategorySearch.DataSource = results;
            acCategorySearch.DataTextField = "Name";
            acCategorySearch.DataValueField = "ID";
            acCategorySearch.DataBind();
            acCategorySearch.Items.Insert(0, new ListItem { Text= Resources.Labels.SelectProductCategory, Value="-1"});
            acCategorySearch.SelectedIndex = -1;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    public string GeneralAttributesUOM()
    {
        string ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString();
        return ContextKey;
    }

    public string GetAccountRelatedContext()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,,,true";
    }

    public string GetPriceNameContextKey()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Prices.ToInt().ToExpressString();
    }

    public string GetUOMContextKey()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString(); ;
    }
}