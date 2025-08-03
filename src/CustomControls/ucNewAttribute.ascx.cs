using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class CustomControls_ucNewAttribute : System.Web.UI.UserControl
{
    XpressDataContext dc = new XpressDataContext();

    public delegate void NewAttributeEventHandler(string AttName, int AttID);

    public event NewAttributeEventHandler NewAttributeCreated;

    public string TargetControlID
    {
        get
        {
            return mpeCreateAtt.TargetControlID;
        }
        set
        {
            mpeCreateAtt.TargetControlID = value;
        }
    }

    public string Title
    {
        get;

        set;

    }

    public int AttributeType_ID
    {
        get;

        set;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.GeneralAttributes, string.Empty);
        btnSaveAtt.Visible = con.PageData.IsAdd;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.mpeCreateAtt.TargetControlID = this.TargetControlID;
        txtAttName.ValidationGroup = ValidationSummary1.ValidationGroup = btnSaveAtt.ValidationGroup = this.ID + "_NewAtt";
    }

    protected void btnSaveAtt_click(object sender, EventArgs e)
    {
        try
        {
            int result = dc.usp_GeneralAttributesCustomControl_insert(txtAttName.TrimmedText, txtAttName.TrimmedText, this.AttributeType_ID);
            txtAttName.Clear();
            NewAttributeCreated(txtAttName.TrimmedText, result);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearAtt_click(object sender, EventArgs e)
    {
        try
        {
            txtAttName.Clear();
            mpeCreateAtt.Show();
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
            txtAttName.Clear();
            mpeCreateAtt.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}