using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class HR_UnderRequestEmployees : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private int Employee_ID
    {
        get
        {
            if (ViewState["Employee_ID"] == null) return 0;
            return (int)ViewState["Employee_ID"];
        }

        set
        {
            ViewState["Employee_ID"] = value;
        }
    }

    private bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null) return false;
            return (bool)ViewState["EditMode"];
        }

        set
        {
            ViewState["EditMode"] = value;
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

    #region Conrtol Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUploadImage);
            this.SetEditMode();
            this.CheckSecurity();

            if (!Page.IsPostBack)
            {
                this.LoadControls();
                if (this.EditMode) this.FillEmployeeData();
            }
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
            if (!fpLogo.HasFile) return;
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
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveEmp_click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            if (!this.EditMode) //insert
            {
                this.Employee_ID = dc.usp_HR_EmployeesUnderRequest_Insert(txtName.TrimmedText, acQualification.Value.ToNullableInt(), acDegree.Value.ToNullableInt(), txtUniversity.TrimmedText, txtNationalID.TrimmedText, txtPassportID.TrimmedText, txtTestDate.Text.ToDate(), txtTestDegreeQuality.TrimmedText, txtTestDegreeSpeed.TrimmedText, acDepartment.Value.ToNullableInt(), acPosition.Value.ToNullableInt(), ddlEmploymentStatus.SelectedValue.ToByte(), this.ImageUrl, txtTel1.TrimmedText, txtTel2.TrimmedText, txtEmail.TrimmedText, txtAddress.TrimmedText);
            }
            else
            {
                result = dc.usp_HR_EmployeesUnderRequest_Update(this.Employee_ID, txtName.TrimmedText, acQualification.Value.ToNullableInt(), acDegree.Value.ToNullableInt(), txtUniversity.TrimmedText, txtNationalID.TrimmedText, txtPassportID.TrimmedText, txtTestDate.Text.ToDate(), txtTestDegreeQuality.TrimmedText, txtTestDegreeSpeed.TrimmedText, acDepartment.Value.ToNullableInt(), acPosition.Value.ToNullableInt(), ddlEmploymentStatus.SelectedValue.ToByte(), this.ImageUrl, txtTel1.TrimmedText, txtTel2.TrimmedText, txtEmail.TrimmedText, txtAddress.TrimmedText);
            }
            if (result == -2 || this.Employee_ID == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return;
            }
            LogAction(this.EditMode ? Actions.Edit : Actions.Add, txtName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.EmployeeUnderRequest + "?ID=" + this.Employee_ID.ToExpressString(), PageLinks.EmployeeUnderRequestList, PageLinks.EmployeeUnderRequest);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(PageLinks.EmployeeUnderRequestList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acPosition.ContextKey = string.Empty + acDepartment.Value;
            if (sender != null) this.FocusNextControl(sender);
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
        acDepartment.ContextKey = string.Empty;
        acPosition.ContextKey = string.Empty;
        acQualification.ContextKey = MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Qualification.ToInt().ToExpressString();
        acDegree.ContextKey = MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Degree.ToInt().ToExpressString();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Employee_ID = Request["ID"].ToInt();
        }
    }

    private void FillEmployeeData()
    {
        var employee = dc.usp_HR_EmployeesUnderRequest_Select(this.Employee_ID, string.Empty, null, null, null).FirstOrDefault();

        txtName.Text = employee.Name;
        acQualification.Value = employee.Qual_ID.ToStringOrEmpty();
        acDegree.Value = employee.Degree_ID.ToStringOrEmpty();
        txtUniversity.Text = employee.University;
        txtNationalID.Text = employee.NationalID;
        txtPassportID.Text = employee.PassportID;
        txtTestDate.Text = employee.TestDate.Value.ToString("d/M/yyyy");
        txtTestDegreeQuality.Text = employee.TestDegreeQuality;
        txtTestDegreeSpeed.Text = employee.TestDegreeSpeed;
        acDepartment.Value = employee.Department_ID.ToStringOrEmpty();
        this.acDepartment_SelectedIndexChanged(null, null);
        acPosition.Value = employee.Position_ID.ToStringOrEmpty();
        txtTel1.Text = employee.Tel1;
        txtTel2.Text = employee.Tel2;
        txtEmail.Text = employee.Email;
        txtAddress.Text = employee.Address;
        ddlEmploymentStatus.SelectedValue = employee.EmploymentStatus.ToExpressString();
        if (File.Exists(Server.MapPath("~/Uploads/" + employee.PhotoUrl))) imgLogo.ImageUrl = "~/Uploads/" + employee.PhotoUrl;
    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc && this.EditMode) Response.Redirect(PageLinks.Authorization, true);
        btnSaveEmp.Visible = (this.EditMode && this.MyContext.PageData.IsEdit) || (!this.EditMode && this.MyContext.PageData.IsAdd);
    }



    #endregion
}