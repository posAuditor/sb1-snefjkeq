using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Comp_ExchangeRates : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtExchnageRates
    {
        get
        {
            return (DataTable)Session["dtExchnageRates" + this.WinID];
        }

        set
        {
            Session["dtExchnageRates" + this.WinID] = value;
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
    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtDateFromSrch.Focus();
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
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            ddlCurrencySrch.SelectedIndex = 0;
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExchangeRates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExchangeRates.PageIndex = e.NewPageIndex;
            gvExchangeRates.DataSource = this.dtExchnageRates;
            gvExchangeRates.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExchangeRates_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtExchnageRates.Select("ID=" + gvExchangeRates.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtDateFrom.Text = dr["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            txtDateTo.Text = dr["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            txtRatio.Text = dr["Ratio"].ToExpressString();
            ddlCurrency.SelectedValue = dr["Currency_ID"].ToExpressString();
            this.EditID = gvExchangeRates.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExchangeRates_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dc.usp_ExchangeRates_update(gvExchangeRates.DataKeys[e.RowIndex]["ID"].ToInt(), null, null, null, null, false);
            LogAction(Actions.Delete, gvExchangeRates.DataKeys[e.RowIndex]["CurrencyName"].ToExpressString(), dc);
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
            txtDateFrom.Clear();
            txtDateTo.Clear();
            txtRatio.Clear();
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
            if (txtDateFrom.Text.ToDate() > txtDateTo.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateFromTo, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            if (this.EditID == 0) //insert
            {
                result = dc.usp_ExchangeRates_Insert(txtDateFrom.Text.ToDate(), txtDateTo.Text.ToDate(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal());
            }
            else
            {
                result = dc.usp_ExchangeRates_update(this.EditID, txtDateFrom.Text.ToDate(), txtDateTo.Text.ToDate(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), null);
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PeriodExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, string.Format(" {0}  {1} - {2} : {3}", ddlCurrency.SelectedItem.Text, txtDateFrom.Text, txtDateTo.Text, txtRatio.Text), dc);
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
            txtDateFrom.Clear();
            txtDateTo.Clear();
            txtRatio.Clear();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void LoadControls()
    {
        var currencies = dc.usp_Currency_Select(false).ToList();
        ddlCurrency.DataSource = currencies;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        ddlCurrencySrch.DataSource = currencies;
        ddlCurrencySrch.DataTextField = "Name";
        ddlCurrencySrch.DataValueField = "ID";
        ddlCurrencySrch.DataBind();
        ddlCurrencySrch.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
    }

    private void Fill()
    {
        this.dtExchnageRates = dc.usp_ExchangeRates_Select(txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), ddlCurrencySrch.SelectedIndex == 0 ? (int?)null : ddlCurrencySrch.SelectedValue.ToInt()).CopyToDataTable();
        gvExchangeRates.DataSource = this.dtExchnageRates;
        gvExchangeRates.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvExchangeRates.Columns[4].Visible = MyContext.PageData.IsEdit;
        gvExchangeRates.Columns[5].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}