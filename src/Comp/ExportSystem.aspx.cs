using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Comp_ExportSystem : UICulturePage
{

    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtFile
    {
        get
        {
            return (DataTable)Session["dtFile" + this.WinID];
        }

        set
        {
            Session["dtFile" + this.WinID] = value;
        }
    }

    private DataTable dtAllAdded
    {
        get
        {
            if (Session["dtAllAdded" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("ID", typeof(int));
                dtallTaxes.Columns.Add("Name", typeof(string));
                dtallTaxes.Columns.Add("Name_ID", typeof(int));
                dtallTaxes.Columns.Add("Value", typeof(string));
                dtallTaxes.Columns.Add("Value_ID", typeof(int));
                Session["dtAllAdded" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllAdded" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Invoice" + this.WinID] = value;
        }
    }

    private string NameFile
    {
        get
        {
            if (ViewState["NameFile"] == null) return string.Empty;
            return (string)ViewState["NameFile"];
        }

        set
        {
            ViewState["NameFile"] = value;
        }
    }

    #endregion

    #region Page events

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUpload);
        if (!IsPostBack)
        {
            ddlIsHasBill_SelectedIndexChanged(null, null);
        }

    }
    #endregion

    #region Control Events

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        var lstHeader = new List<ExportClass>();

        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        string fileName = "ExcelData" + DateTime.Now.Ticks + Extension;
        FileUpload1.SaveAs(Server.MapPath("~\\uploads\\Excel\\" + fileName));
        string ExcelFilePath = Server.MapPath("~\\uploads\\Excel\\" + fileName);

        dtFile = new DataTable();
        byte[] bin = File.ReadAllBytes(ExcelFilePath);
        using (MemoryStream stream = new MemoryStream(bin))

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        try
                        {
                            var rowExcp = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
                            int oo = 0;
                            foreach (var cell in rowExcp)
                            {
                                var t = new ExportClass() { ID = oo++, Name = cell.Text };
                                lstHeader.Add(t);
                            }

                            if (rbHDR.SelectedIndex == 0)
                            {
                                foreach (var cell in rowExcp)
                                {
                                    dtFile.Columns.Add(new DataColumn(cell.Text));
                                }
                            }
                            else
                            {
                                for (int k = worksheet.Dimension.Start.Column; k <= worksheet.Dimension.End.Column; k++)
                                {
                                    dtFile.Columns.Add(new DataColumn("F" + k.ToString()));

                                }
                            }

                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];

                                DataRow newRow = dtFile.NewRow();

                                foreach (var cell in row)
                                {
                                    newRow[cell.Start.Column - 1] = cell.Text;
                                }

                                dtFile.Rows.Add(newRow);
                            }
                        }
                        catch (Exception exDetails)
                        {

                            UserMessages.Message(null, "يجب ان يكون شيت واحد", string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                UserMessages.Message(null, "يجب ان يكون نوع الاكسل .xlsx", string.Empty);
            }

        ddlPropertiesValue.DataSource = lstHeader;
        ddlPropertiesValue.DataValueField = "ID";
        ddlPropertiesValue.DataTextField = "Name";
        ddlPropertiesValue.DataBind();


        GridView1.DataSource = dtFile;
        GridView1.DataBind();

    }
    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.DataSource = dtFile;

        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
    protected void ddlIsHasBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        var lst = new List<ExportClass>();
        switch (ddlType.SelectedValue)
        {
            case "0":
                lst.Add(new ExportClass() { ID = 0, Name = "إسم الفرع", Type_ID = 0 });

                break;
            case "1":
                lst.Add(new ExportClass() { ID = 0, Name = "إسم المخزن", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "إسم الفرع", Type_ID = 1 });

                break;
            case "2":

                lst.Add(new ExportClass() { ID = 0, Name = "إسم الصنف بالعربية * ", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "إسم الصنف بالانكليزية", Type_ID = 1 });
                lst.Add(new ExportClass() { ID = 2, Name = "إسم الفئة *", Type_ID = 2 });
                lst.Add(new ExportClass() { ID = 3, Name = "التكلفة *", Type_ID = 3 });

                lst.Add(new ExportClass() { ID = 4, Name = "سعر البيع *", Type_ID = 4 });
                lst.Add(new ExportClass() { ID = 5, Name = "الباركود *", Type_ID = 5 });
                lst.Add(new ExportClass() { ID = 6, Name = "الرمز", Type_ID = 6 });
                lst.Add(new ExportClass() { ID = 7, Name = "الوحدة *", Type_ID = 7 });

                lst.Add(new ExportClass() { ID = 8, Name = "الوحدة الاولى", Type_ID = 8 });
                lst.Add(new ExportClass() { ID = 9, Name = "باركود الوحدة الاولى", Type_ID = 9 });
                lst.Add(new ExportClass() { ID = 10, Name = "النسبة الوحدة الاولى", Type_ID = 10 });
                lst.Add(new ExportClass() { ID = 11, Name = "السعر الوحدة الاولى", Type_ID = 11 });

                lst.Add(new ExportClass() { ID = 12, Name = "الوحدة الثانية", Type_ID = 12 });
                lst.Add(new ExportClass() { ID = 13, Name = "باركود الوحدة الثانية", Type_ID = 13 });
                lst.Add(new ExportClass() { ID = 14, Name = "النسبة الوحدة الثانية", Type_ID = 14 });
                lst.Add(new ExportClass() { ID = 15, Name = "السعر الوحدة الثانية", Type_ID = 15 });

                break;
            case "3":

                lst.Add(new ExportClass() { ID = 0, Name = "الاسم", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "الفرع", Type_ID = 1 });
                lst.Add(new ExportClass() { ID = 2, Name = "المنطقة", Type_ID = 2 });
                lst.Add(new ExportClass() { ID = 3, Name = "الرصيد الافتتاحى", Type_ID = 3 });
                lst.Add(new ExportClass() { ID = 4, Name = "تليفون", Type_ID = 4 });
                lst.Add(new ExportClass() { ID = 5, Name = "عنوان", Type_ID = 5 });
                break;
            case "4":
                lst.Add(new ExportClass() { ID = 0, Name = "الاسم", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "الفرع", Type_ID = 1 });
                lst.Add(new ExportClass() { ID = 3, Name = "الرصيد الافتتاحى", Type_ID = 3 });
                lst.Add(new ExportClass() { ID = 4, Name = "تليفون", Type_ID = 4 });
                lst.Add(new ExportClass() { ID = 5, Name = "عنوان", Type_ID = 5 });


                break;
            case "5":
                lst.Add(new ExportClass() { ID = 0, Name = "إسم البنك", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "الفرع", Type_ID = 1 });
                lst.Add(new ExportClass() { ID = 3, Name = "الرصيد الافتتاحى", Type_ID = 3 });
                lst.Add(new ExportClass() { ID = 4, Name = "تليفون", Type_ID = 4 });
                lst.Add(new ExportClass() { ID = 5, Name = "عنوان", Type_ID = 5 });
                break;

            case "7":
                lst.Add(new ExportClass() { ID = 0, Name = "رقم الحساب", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "مبلغ الدائن", Type_ID = 1 });
                lst.Add(new ExportClass() { ID = 2, Name = "مبلغ المدين", Type_ID = 2 });
                lst.Add(new ExportClass() { ID = 3, Name = "الملاحظات", Type_ID = 3 });
                lst.Add(new ExportClass() { ID = 4, Name = "التاريخ", Type_ID = 4 });
                lst.Add(new ExportClass() { ID = 5, Name = "مركز التكلفة", Type_ID = 5 });
                break;
            case "8":
                lst.Add(new ExportClass() { ID = 0, Name = "الحساب", Type_ID = 0 });
                lst.Add(new ExportClass() { ID = 1, Name = "حساب الاب", Type_ID = 1 });
                break;
            case "9":
                lst.Add(new ExportClass() { ID = 0, Name = "الاسم", Type_ID = 0 }); break;
            case "10":
                lst.Add(new ExportClass() { ID = 0, Name = "الاسم", Type_ID = 0 }); break;
            default:
                break;


        }

        ddlProperties.DataSource = lst;
        ddlProperties.DataValueField = "ID";
        ddlProperties.DataTextField = "Name";
        ddlProperties.DataBind();
    }

    #endregion

    #region Private Methods

    #endregion

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlPropertiesValue.SelectedValue))
        {

            var rs1 = this.dtAllAdded.Select("Name_ID='" + ddlProperties.SelectedValue + "'"); 
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID='" + ddlProperties.SelectedValue + "'");
            if (filteredRows.Length > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return;
            }


            DataRow r = null;
            r = this.dtAllAdded.NewRow();
            r["ID"] = this.dtAllAdded.GetID("ID");
            r["Name"] = ddlProperties.SelectedItem.Text; ;
            r["Name_ID"] = ddlProperties.SelectedValue.ToInt();
            r["Value"] = ddlPropertiesValue.SelectedItem.Text;
            r["Value_ID"] = ddlPropertiesValue.SelectedValue.ToInt();
            this.dtAllAdded.Rows.Add(r);
            this.BindItemsGrid();
        }
        else
        {
            UserMessages.Message(null, "بيانات غير مكتملة", string.Empty);
        }


    }
    private void BindItemsGrid()
    {

        gvExportList.DataSource = this.dtAllAdded;
        gvExportList.DataBind();

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            var lst = new List<ExportClass>();
            switch (ddlType.SelectedValue)
            {
                case "0":
                    if (!this.InsertBranches()) { trans.Rollback(); return; }
                    break;
                case "1":
                    if (!this.InsertStores()) { trans.Rollback(); return; }
                    break;
                case "2":
                    if (!this.InsertItems()) { trans.Rollback(); return; }
                    break;
                case "3":
                    if (!this.InsertCustomers()) { trans.Rollback(); return; }
                    break;
                case "4":
                    if (!this.InsertVendors()) { trans.Rollback(); return; }
                    break;
                case "5":
                    if (!this.InsertBanks()) { trans.Rollback(); return; }
                    break;
                case "7":
                    if (!this.InsertOperation()) { trans.Rollback(); return; }
                    break;
                case "8":
                    if (!this.InsertAccountinCOA()) { trans.Rollback(); return; }
                    break;
                case "9":
                    if (!this.InsertUnit()) { trans.Rollback(); return; }
                    break;
                case "10":
                    if (!this.InsertCategory()) { trans.Rollback(); return; }
                    break;
                default:
                    break;
            }


            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    private bool InsertAccountinCOA()
    {


        var lst = dc.ChartOfAccounts.Where(x => x.IsActive.Value).ToList();

        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            string nameParentAccount = string.Empty;
            string nameAccount = string.Empty;



            DataRow[] filtereAccountRows = this.dtAllAdded.Select("Name_ID=0");
            if (filtereAccountRows.Any())
            {
                var objCustomer = filtereAccountRows[0];
                nameAccount = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
            }

            DataRow[] filtereParentAccountRows = this.dtAllAdded.Select("Name_ID=1");
            if (filtereParentAccountRows.Any())
            {
                var objCustomer = filtereParentAccountRows[0];
                nameParentAccount = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
            }


            var cnt = lst.Count(x => x.Name == nameAccount);

            if (cnt > 0)
            {
                UserMessages.Message(null, nameAccount + " : الحساب موجود من قبل ", string.Empty);
                return false;
            }

            var parent_Object = lst.Where(x => x.Name == nameParentAccount).FirstOrDefault();
            if (parent_Object == null)
            {
                UserMessages.Message(null, nameParentAccount + " : حساب الاب غير موجود", string.Empty);
                return false;
            }
            dc.usp_ChartOfAccount_Insert(nameAccount, nameAccount, COA.Customers.ToInt(), false, null, 15, 1, 0, null, COA.Capital.ToInt());


        }
        return true;


    }

    private bool InsertOperation()
    {

        var list = new List<OperationCls>();

        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            if (row[0].ToExpressString() == string.Empty)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "القيود", i + 1), string.Empty);
                return false;
            }

            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=0");
            var obj = filteredRows[0];
            var index = (obj["Value_ID"].ToExpressString()).ToInt();

            DataRow[] filteredRowsCredit = this.dtAllAdded.Select("Name_ID=1");

            DataRow objCredit = filteredRowsCredit.Length > 0 ? filteredRowsCredit[0] : null;
            var indexCredit = 0;
            if (objCredit != null)
            {
                indexCredit = (objCredit["Value_ID"].ToExpressString()).ToInt();
            }
            //var indexCredit = (objCredit["Value_ID"].ToExpressString()).ToInt();


            DataRow[] filteredRowsDebit = this.dtAllAdded.Select("Name_ID=2");

            var objDebit = filteredRowsDebit.Length > 0 ? filteredRowsDebit[0] : null;

            var indexDebit = 0;
            if (objDebit != null)
            {
                indexDebit = (objDebit["Value_ID"].ToExpressString()).ToInt();
            }

            DataRow[] filteredRowsNote = this.dtAllAdded.Select("Name_ID=3");
            var objNote = filteredRowsNote[0];
            var indexNote = (objNote["Value_ID"].ToExpressString()).ToInt();






            DataRow[] filteredRowsDateOperation = this.dtAllAdded.Select("Name_ID=4");
            var objDateOperation = filteredRowsDateOperation[0];
            var indexDateOperation = (objDateOperation["Value_ID"].ToExpressString()).ToInt();

            DataRow[] filteredRowsCostCenter = this.dtAllAdded.Select("Name_ID=5");
            var objCostCenter = filteredRowsCostCenter[0];
            var indexCostCenter = (objCostCenter["Value_ID"].ToExpressString()).ToInt();



            OperationCls objx = new OperationCls();
            objx.cachedNumber = row[index].ToExpressString();
            objx.CostCenter = row[indexCostCenter].ToExpressString();
            decimal amt = 0;
            objx.Debit = indexDebit != 0 ? row[indexDebit].ToExpressString().ToDecimalOrDefault() : amt;

            objx.Credit = row[indexCredit].ToExpressString().ToDecimalOrDefault();
            objx.DateOperation = row[indexDateOperation].ToExpressString().ToDate().Value;
            objx.Note = row[indexNote].ToExpressString();

            list.Add(objx);


        }

        var listCoa = dc.ChartOfAccounts.Where(x => x.IsActive == true).ToList();
        var listCostCenter = dc.usp_CostCenters_select(0, false, null).ToList();
        foreach (var item in list)
        {
            var Accounts = listCoa.Where(x => x.CachedNumber == item.cachedNumber).ToList();
            if (Accounts.Any())
            {
                item.Account_id = Accounts.FirstOrDefault().ID;
            }
            else
            {
                UserMessages.Message(null, string.Format(item.cachedNumber + " غير موجود ", "الحساب", 0), string.Empty);
                return false;
            }

            var CostCenters = listCostCenter.Where(x => x.Name == item.CostCenter).ToList();
            if (CostCenters.Any())
            {
                item.CostCenter_ID = CostCenters.FirstOrDefault().ID;
            }
            else
            {
                UserMessages.Message(null, string.Format(item.CostCenter + "  غير موجود  ", "مركز التكلفة", 0), string.Empty);
                return false;
            }
        }
        var cur_ID = dc.usp_Company_Select().FirstOrDefault().Currency_ID;
        var listGrouping = list.GroupBy(x => x.DateOperation).ToList();
        foreach (var item in listGrouping)
        {
            var dateOperation = item.Key;
            string serial = string.Empty;
            byte DocStatus_ID = DocStatus.Current.ToByte();
            decimal ratio = 1;
            var Op_ID = dc.usp_JournalEntry_Insert(null, dateOperation, ref serial, DocStatus_ID, 8, cur_ID, item.Sum(f => f.Debit), item.Sum(f => f.Debit), ratio, string.Empty, null, null, null, null, null, null, null, string.Empty, DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString());
            foreach (var itemDetails in item.ToList())
            {
                dc.usp_JournalEntryDetails_Insert(Op_ID, itemDetails.Account_id, itemDetails.Debit, itemDetails.Credit, itemDetails.Debit, itemDetails.Credit, itemDetails.Note, itemDetails.CostCenter_ID);
            }
        }


        return true;

    }

    private bool InsertBranches()
    {


        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            if (row[0].ToExpressString() == string.Empty)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "الفروع", i + 1), string.Empty);
                return false;
            }


            // if (r.RowState == DataRowState.Deleted) continue;

            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=0");
            var obj = filteredRows[0];
            var index = (obj["Value_ID"].ToExpressString()).ToInt();

            var result = dc.usp_Branchs_Insert(row[index].ToExpressString(), string.Empty, string.Empty, string.Empty, string.Empty, null, true, null);
            if (result == -2)
            {
                UserMessages.Message(null, row[0].ToExpressString() + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return false;
            }
        }


        return true;
    }

    private bool InsertStores()
    {



        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            //if (row[0].ToExpressString() == string.Empty)
            //{
            //    UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "المخازن", i + 1), string.Empty);
            //    return false;
            //}

            //Branch Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");

            var obj = filteredRows[0];
            var indexBranch = (obj["Value_ID"].ToExpressString()).ToInt();

            //Store Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=0");

            var objStore = filteredStoreRows[0];
            var index = (objStore["Value_ID"].ToExpressString()).ToInt();


            var Branch = dc.usp_Branchs_Select(string.Empty, null).Where(x => x.Name == row[indexBranch].ToExpressString()).FirstOrDefault();
            int? Branch_ID = (Branch != null) ? Branch.ID : (int?)null;

            if (row[index].ToExpressString() == string.Empty || (MyContext.Features.BranchesEnabled && Branch == null))
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "المخازن", index + 1), string.Empty);
                return false;
            }
            var result = dc.usp_Stores_Insert(row[index].ToExpressString(), string.Empty, null, Branch_ID, string.Empty, true);
            if (result == -2)
            {
                UserMessages.Message(null, row[index].ToExpressString() + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return false;
            }



        }
        return true;


        //var worksheet = Workbook.Worksheets(ExcelFilePath).Skip(1).Take(1).First();
        //int index = 0;
        //int result = 0;
        //foreach (var row in worksheet.Rows)
        //{
        //    if (index == 0) { index++; continue; }

        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (row.Cells[i].TextValue() == null)
        //        {
        //            UserMessages.Message(null, string.Format(Resources.UserInfoMessages.CorruptedData, "المخازن", index + 1), string.Empty);
        //            return false;
        //        }
        //    }

        //    var Branch = dc.usp_Branchs_Select(string.Empty, null).Where(x => x.Name == row.Cells[1].TextValue()).FirstOrDefault();
        //    int? Branch_ID = (Branch != null) ? Branch.ID : (int?)null;

        //    if (row.Cells[0].TextValue() == string.Empty || (MyContext.Features.BranchesEnabled && Branch == null))
        //    {
        //        UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "المخازن", index + 1), string.Empty);
        //        return false;
        //    }
        //    result = dc.usp_Stores_Insert(row.Cells[0].TextValue(), string.Empty, null, Branch_ID, string.Empty, true);
        //    if (result == -2)
        //    {
        //        UserMessages.Message(null, row.Cells[0].TextValue() + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
        //        return false;
        //    }
        //    index++;
        //}
        //return true;
    }


    private bool InsertCustomers()
    {
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            string nameCustomer = string.Empty;
            //Customer
            DataRow[] filtereCustomerdRows = this.dtAllAdded.Select("Name_ID=0");
            if (filtereCustomerdRows.Any())
            {
                var objCustomer = filtereCustomerdRows[0];
                nameCustomer = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
            }

            string nameBranch = string.Empty;
            //Branch Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");
            if (filteredRows.Any())
            {
                var obj = filteredRows[0];
                var indexBranch = (obj["Value_ID"].ToExpressString()).ToInt();
                nameBranch = row[indexBranch].ToExpressString();
            }
            string nameArea = string.Empty;
            //Area Propertie
            DataRow[] filteredAreaRows = this.dtAllAdded.Select("Name_ID=2");
            if (filteredAreaRows.Any())
            {
                var objArea = filteredAreaRows[0];
                var indexArea = (objArea["Value_ID"].ToExpressString()).ToInt();
                nameArea = row[indexArea].ToExpressString();
            }

            decimal valueBalance = 0;
            //Area Propertie
            DataRow[] filteredBalanceRows = this.dtAllAdded.Select("Name_ID=3");

            if (filteredBalanceRows.Any())
            {
                var objBalance = filteredBalanceRows[0];
                var indexdBalance = (objBalance["Value_ID"].ToExpressString()).ToInt();
                valueBalance = row[indexdBalance].ToExpressString().ToDecimalOrDefault();
            }

            string nameTelephon = string.Empty;
            //Telephone Propertie
            DataRow[] filteredTelephonRows = this.dtAllAdded.Select("Name_ID=4");
            if (filteredTelephonRows.Any())
            {
                var objTelephon = filteredTelephonRows[0];
                var indexTelephon = (objTelephon["Value_ID"].ToExpressString()).ToInt();
                nameTelephon = row[indexTelephon].ToExpressString();
            }


            string nameAddress = string.Empty;
            //Address Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=5");
            if (filteredStoreRows.Any())
            {
                var objAddress = filteredStoreRows[0];
                var indexAddress = (objAddress["Value_ID"].ToExpressString()).ToInt();
                nameAddress = (objAddress["Value"].ToExpressString());

            }

            string nameParentAccount = string.Empty;
            //Address Propertie
            DataRow[] filteredParentAccountRows = this.dtAllAdded.Select("Name_ID=6");
            if (filteredParentAccountRows.Any())
            {
                var objParentAccount = filteredParentAccountRows[0];
                var indexParentAccount = (objParentAccount["Value_ID"].ToExpressString()).ToInt();
                nameParentAccount = row[indexParentAccount].ToExpressString(); ;

            }


            var Branch = dc.usp_Branchs_Select(string.Empty, null).Where(x => x.Name == nameBranch).FirstOrDefault();
            int? Branch_ID = (Branch != null) ? Branch.ID : (int?)null;

            var company = dc.usp_Company_Select().FirstOrDefault();
            int? Currency_ID = company.Currency_ID;

            var Area = dc.usp_Areas_Select(string.Empty).Where(x => x.Name == nameArea).FirstOrDefault();
            int? Area_ID = (Area != null) ? Area.ID : (int?)null;

            decimal OpenBalance = valueBalance;
            decimal Ratio = 1;// row.Cells[5].TextValue().ToDecimalOrDefault();

            if (nameBranch == string.Empty || (Branch_ID == null && MyContext.Features.BranchesEnabled && OpenBalance != 0) || (OpenBalance != 0 && Ratio == 0) || Currency_ID == null)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "العملاء", i + 1), string.Empty);
                return false;
            }

            int coa_ID = 0;
            var coa = dc.ChartOfAccounts.Where(x => x.Name == nameParentAccount).FirstOrDefault();
            if (coa != null)
            {
                coa_ID = coa.ID;
            }
            if (coa_ID == 0)
            {
                UserMessages.Message(null, " حساب الاب غير موجود اوتأكد من الاسم  ", string.Empty);
                return false;
            }
            int Contact_ID = dc.usp_Contact_Insert(Branch_ID, Currency_ID, DocSerials.Customer.ToInt(), nameCustomer, 'C', string.Empty, null);
            int ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(nameCustomer, nameCustomer, (coa_ID > 0 ? coa_ID : COA.Customers.ToInt()), true, Branch_ID, Currency_ID, Ratio, OpenBalance, MyContext.FiscalYearStartDate, COA.Capital.ToInt());

            if (Contact_ID == -2 || ChartofAccount_ID == -2)
            {
                UserMessages.Message(null, nameCustomer + " : " + Resources.UserInfoMessages.NameAlreadyExists + " (العملاء) ", string.Empty);
                return false;
            }

            dc.usp_Customers_Insert(Contact_ID, ChartofAccount_ID, Area_ID, null, company.UseCustomerCreditLimit, company.CustomerCreditLimit);
            if (nameTelephon != string.Empty) dc.usp_ContactDetails_insert(Contact_ID, 4, nameTelephon);
            if (nameAddress != string.Empty) dc.usp_ContactDetails_insert(Contact_ID, 3, nameAddress);

        }
        return true;


    }

    private bool InsertVendors()
    {
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            string nameCustomer = string.Empty;
            //Customer
            DataRow[] filtereCustomerdRows = this.dtAllAdded.Select("Name_ID=0");
            if (filtereCustomerdRows.Any())
            {
                var objCustomer = filtereCustomerdRows[0];
                nameCustomer = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
            }

            string nameBranch = string.Empty;
            //Branch Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");
            if (filteredRows.Any())
            {
                var obj = filteredRows[0];
                var indexBranch = (obj["Value_ID"].ToExpressString()).ToInt();
                nameBranch = row[indexBranch].ToExpressString();
            }


            decimal valueBalance = 0;
            //Area Propertie
            DataRow[] filteredBalanceRows = this.dtAllAdded.Select("Name_ID=3");

            if (filteredBalanceRows.Any())
            {
                var objBalance = filteredBalanceRows[0];
                var indexdBalance = (objBalance["Value_ID"].ToExpressString()).ToInt();
                valueBalance = row[indexdBalance].ToExpressString().ToDecimalOrDefault();
            }

            string nameTelephon = string.Empty;
            //Telephone Propertie
            DataRow[] filteredTelephonRows = this.dtAllAdded.Select("Name_ID=4");
            if (filteredTelephonRows.Any())
            {
                var objTelephon = filteredTelephonRows[0];
                var indexTelephon = (objTelephon["Value_ID"].ToExpressString()).ToInt();
                nameTelephon = row[indexTelephon].ToExpressString();
            }


            string nameAddress = string.Empty;
            //Address Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=5");
            if (filteredStoreRows.Any())
            {
                var objAddress = filteredStoreRows[0];
                var indexAddress = (objAddress["Value_ID"].ToExpressString()).ToInt();
                nameAddress = (objAddress["Value"].ToExpressString());

            }


            var Branch = dc.usp_Branchs_Select(string.Empty, null).Where(x => x.Name == nameBranch).FirstOrDefault();
            int? Branch_ID = (Branch != null) ? Branch.ID : (int?)null;

            var company = dc.usp_Company_Select().FirstOrDefault();
            int? Currency_ID = company.Currency_ID;


            decimal OpenBalance = valueBalance;
            decimal Ratio = 1;// row.Cells[5].TextValue().ToDecimalOrDefault();

            if (nameBranch == string.Empty || (Branch_ID == null && MyContext.Features.BranchesEnabled && OpenBalance != 0) || (OpenBalance != 0 && Ratio == 0) || Currency_ID == null)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "الموردين", i + 1), string.Empty);
                return false;
            }

            int Contact_ID = dc.usp_Contact_Insert(Branch_ID, Currency_ID, DocSerials.Vendor.ToInt(), nameCustomer, 'V', string.Empty, null);
            int ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(nameCustomer, nameCustomer, COA.Vendors.ToInt(), true, Branch_ID, Currency_ID, Ratio, OpenBalance, MyContext.FiscalYearStartDate, COA.Capital.ToInt());

            if (Contact_ID == -2 || ChartofAccount_ID == -2)
            {
                UserMessages.Message(null, nameCustomer + " : " + Resources.UserInfoMessages.NameAlreadyExists + " (الموردين) ", string.Empty);
                return false;
            }

            dc.usp_Vendors_Insert(Contact_ID, ChartofAccount_ID, null, company.UseCustomerCreditLimit, company.CustomerCreditLimit);
            if (nameTelephon != string.Empty) dc.usp_ContactDetails_insert(Contact_ID, 4, nameTelephon);
            if (nameAddress != string.Empty) dc.usp_ContactDetails_insert(Contact_ID, 3, nameAddress);
        }
        return true;


    }

    private bool InsertItems()
    {
        if (dtAllAdded.Rows.Count == 0)
        {
            UserMessages.Message(null, "بيانات غير مكتملة", string.Empty);
            return false; ;
        }
        var listCategory = dc.usp_ItemsCategories_Select(string.Empty).ToList();
        var listGeneralAttribute = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).ToList();

        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            int? Category_ID = null;
            int? UOM_ID = null;
            string nameCategory = string.Empty,
                   nameItem = string.Empty,
                   nameItemEn = string.Empty,
                   BarcodeItem = string.Empty,
                   CodeItem = string.Empty,
                   UnitItem = string.Empty,

                   UnitFirstItem = string.Empty,
                   BarcodeFirstItem = string.Empty,



                   UnitSecondItem = string.Empty,
                   BarcodeSecondItem = string.Empty;



            decimal CostItem = 0,
              PriceItem = 0,
              PriceFirstItem = 0,
              PriceSecondItem = 0,
              RatioFirstItem = 1,
              RatioSecondItem = 1;


            #region Name Item

            //Item Arabic
            DataRow[] filtereItemRows = this.dtAllAdded.Select("Name_ID=0");
            if (filtereItemRows.Any())
            {
                var objItem = filtereItemRows[0];
                nameItem = row[objItem["Value_ID"].ToInt()].ToExpressString();
            }


            #endregion

            #region Name Item En

            //Item Arabic
            DataRow[] filtereItemEnRows = this.dtAllAdded.Select("Name_ID=1");
            if (filtereItemEnRows.Any())
            {
                var objItemEn = filtereItemEnRows[0];
                nameItemEn = row[objItemEn["Value_ID"].ToInt()].ToExpressString();
            }


            #endregion

            #region Category

            //Category
            DataRow[] filtereCustomerdRows = this.dtAllAdded.Select("Name_ID=2");
            if (filtereCustomerdRows.Any())
            {
                var objCustomer = filtereCustomerdRows[0];
                nameCategory = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
            }
            #endregion

            #region Name Item En

            //Item Cost
            DataRow[] filtereItemCostRows = this.dtAllAdded.Select("Name_ID=3");
            if (filtereItemCostRows.Any())
            {
                var objItemCost = filtereItemCostRows[0];
                CostItem = row[objItemCost["Value_ID"].ToInt()].ToDecimalOrDefault();
            }


            #endregion

            #region Price

            //Item Price
            DataRow[] filtereItemPriceRows = this.dtAllAdded.Select("Name_ID=4");
            if (filtereItemPriceRows.Any())
            {
                var objItemPrice = filtereItemPriceRows[0];
                PriceItem = row[objItemPrice["Value_ID"].ToInt()].ToDecimalOrDefault();
            }

            #endregion

            #region Barcode

            //Item Price
            DataRow[] filtereItemBarcodeRows = this.dtAllAdded.Select("Name_ID=5");
            if (filtereItemBarcodeRows.Any())
            {
                var objItemBarcode = filtereItemBarcodeRows[0];
                BarcodeItem = row[objItemBarcode["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Code

            //Item Price
            DataRow[] filtereItemCodeRows = this.dtAllAdded.Select("Name_ID=6");
            if (filtereItemCodeRows.Any())
            {
                var objItemCode = filtereItemCodeRows[0];
                CodeItem = row[objItemCode["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Unit

            //Item Price
            DataRow[] filtereItemUnitRows = this.dtAllAdded.Select("Name_ID=7");
            if (filtereItemUnitRows.Any())
            {
                var objItemUnit = filtereItemUnitRows[0];
                UnitItem = row[objItemUnit["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion


            #region *First*

            #region Price

            //Item Price
            DataRow[] filtereItemUnitFirstRows = this.dtAllAdded.Select("Name_ID=8");
            if (filtereItemUnitFirstRows.Any())
            {
                var objItemUnitFirst = filtereItemUnitFirstRows[0];
                UnitFirstItem = row[objItemUnitFirst["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Barcode

            //Item Price
            DataRow[] filtereItemBarcodeFirstRows = this.dtAllAdded.Select("Name_ID=9");
            if (filtereItemBarcodeFirstRows.Any())
            {
                var objItemBarcodeFirst = filtereItemBarcodeFirstRows[0];
                BarcodeFirstItem = row[objItemBarcodeFirst["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Code

            //Item Price
            DataRow[] filtereItemRatioRows = this.dtAllAdded.Select("Name_ID=10");
            if (filtereItemRatioRows.Any())
            {
                var objItemRatio = filtereItemRatioRows[0];
                RatioFirstItem = row[objItemRatio["Value_ID"].ToInt()].ToDecimalOrDefault();
            }

            #endregion

            #region Unit

            //Item Price
            DataRow[] filtereItemPriceFirstRows = this.dtAllAdded.Select("Name_ID=11");
            if (filtereItemPriceFirstRows.Any())
            {
                var objItemPriceFirst = filtereItemPriceFirstRows[0];
                PriceFirstItem = row[objItemPriceFirst["Value_ID"].ToInt()].ToDecimalOrDefault();
            }

            #endregion
            #endregion


            #region *Second*

            #region Unit

            //Item Unit
            DataRow[] filtereItemUnitSecondRows = this.dtAllAdded.Select("Name_ID=12");
            if (filtereItemUnitSecondRows.Any())
            {
                var objItemUnitSecond = filtereItemUnitSecondRows[0];
                UnitSecondItem = row[objItemUnitSecond["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Barcode

            //Item Barcode
            DataRow[] filtereItemBarcodeSecondRows = this.dtAllAdded.Select("Name_ID=13");
            if (filtereItemBarcodeSecondRows.Any())
            {
                var objItemBarcodeSecond = filtereItemBarcodeSecondRows[0];
                BarcodeSecondItem = row[objItemBarcodeSecond["Value_ID"].ToInt()].ToExpressString();
            }

            #endregion

            #region Ratio

            //Item Ratio
            DataRow[] filtereItemRatioSecondRows = this.dtAllAdded.Select("Name_ID=14");
            if (filtereItemRatioSecondRows.Any())
            {
                var objItemRatioSecond = filtereItemRatioSecondRows[0];
                RatioSecondItem = row[objItemRatioSecond["Value_ID"].ToInt()].ToDecimalOrDefault();
            }

            #endregion

            #region Price

            //Item Price
            DataRow[] filtereItemPriceSecondRows = this.dtAllAdded.Select("Name_ID=15");
            if (filtereItemPriceSecondRows.Any())
            {
                var objItemPriceSecond = filtereItemPriceSecondRows[0];
                PriceSecondItem = row[objItemPriceSecond["Value_ID"].ToInt()].ToDecimalOrDefault();
            }

            #endregion
            #endregion


            #region Category Insert Test

            if (!string.IsNullOrEmpty(nameCategory))
            {
                var Category = listCategory.Where(x => x.Name.Trim() == nameCategory.Trim()).FirstOrDefault();
                Category_ID = (Category != null) ? Category.ID : (int?)null;

                if (Category_ID == null)
                {
                    UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + " غير موجوده " + " (الفئة) " + " **" + nameCategory + "**", string.Empty);
                }
                //if (Category_ID == null)
                //{
                //    Category_ID = dc.usp_ItemsCategories_Insert(nameCategory, null, false, null, false, false);
                //}
            }
            else
            {
                UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + "بيانات غير مكتملة" + " (الفئة) " + " **" + nameCategory + "**", string.Empty);
                return false; ;
            }

            #endregion


            #region Unit Insert With Test
            if (!string.IsNullOrEmpty(UnitItem))
            {
                var UOM = listGeneralAttribute.Where(x => x.Name.Trim() == UnitItem.Trim()).FirstOrDefault();
                UOM_ID = (UOM != null) ? UOM.ID : (int?)null;
                if (UOM_ID == null)
                {
                    UserMessages.Message(null, (i +1) + "رقم السطر" + "/" + " غير موجوده " + " (الوحدة) " + " **" + UnitItem + "**", string.Empty);
                }
            }
            else
            {
                UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + "بيانات غير مكتملة" + " (الوحدة) " + " **" + UnitItem + "**", string.Empty);
                return false; ;
            }
            #endregion


            decimal Cost = CostItem;
            decimal Price = PriceItem;

            //if (row.Cells[2].TextValue() == string.Empty || Category_ID == null || Cost < 0 || row.Cells[14].TextValue() == string.Empty || Price < 0 || UOM_ID == null)
            //{
            //    UserMessages.Message(null, string.Format(row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.IncompleteData, "الاصناف", i + 1), string.Empty);
            //    return false;
            //}

            //if (Price < Cost)
            //{
            //    UserMessages.Message(null, nameItem + " : " + Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
            //    return false;
            //}
            var result = dc.usp_Items_insert(nameItem.Trim(), Category_ID, 'i', Cost, UOM_ID, null, null, null, null, string.Empty, BarcodeItem.Trim(), Price, 0, CodeItem.Trim(), nameItemEn.Trim(), 0, 0,
                false, false, false);

            if (result == -2)
            {
                UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + nameItem + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return false;
            }

            if (result == -3)
            {
                UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + BarcodeItem + " : " + Resources.UserInfoMessages.BarcodeExists, string.Empty);
                return false;
            }


            if (result == -4)
            {
                UserMessages.Message(null, (i + 1) + "رقم السطر" + "/" + nameItem + " : " + Resources.UserInfoMessages.CodeItemAlreadyExist, string.Empty);
                return false;
            }
            //unit1
            if (!string.IsNullOrEmpty(UnitFirstItem))
            {

                var UOM1 = listGeneralAttribute.Where(x => x.Name == UnitFirstItem).FirstOrDefault();
                int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
                if (UOM1 == null)
                {
                    UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(UnitFirstItem, UnitFirstItem, null);
                }
                try
                {
                    dc.usp_ItemsUnits_Insert(result, UOM_ID1, RatioFirstItem, PriceFirstItem, BarcodeFirstItem, null);
                }
                catch
                {
                    UserMessages.Message(null, i + "رقم السطر" + "/unit1" + nameItem, string.Empty);
                    return false;
                }
            }

            //unit2
            if (!string.IsNullOrEmpty(UnitSecondItem))
            {

                var UOM1 = listGeneralAttribute.Where(x => x.Name == UnitSecondItem).FirstOrDefault();
                int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
                if (UOM1 == null)
                {
                    UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(UnitSecondItem, UnitSecondItem, null);
                }
                try
                {
                    dc.usp_ItemsUnits_Insert(result, UOM_ID1, RatioSecondItem, PriceSecondItem, BarcodeSecondItem, null);
                }
                catch
                {
                    UserMessages.Message(null, i + "رقم السطر" + "/unit2" + nameItem, string.Empty);
                    return false;
                }
            }




        }




        //int index = 0;
        //int result = 0;
        // foreach (var row in worksheet.Rows)
        //{
        //    if (index == 0) { index++; continue; }

        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (row.Cells[i].TextValue() == null)
        //        {
        //            UserMessages.Message(null, string.Format(Resources.UserInfoMessages.CorruptedData, "البضاعة", index + 1), string.Empty);
        //            return false;
        //        }
        //    }

        //    var Category = dc.usp_ItemsCategories_Select(string.Empty).Where(x => x.Name == row.Cells[16].TextValue()).FirstOrDefault();
        //    int? Category_ID = (Category != null) ? Category.ID : (int?)null;


        //    if (Category_ID == null)
        //    {
        //        Category_ID = dc.usp_ItemsCategories_Insert(row.Cells[16].TextValue(), null, false, null);
        //    }



        //    var UOM = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[4].TextValue()).FirstOrDefault();
        //    int? UOM_ID = (UOM != null) ? UOM.ID : (int?)null;





        //    decimal Cost = row.Cells[7].TextValue().ToDecimalOrDefault();
        //    decimal Price = row.Cells[8].TextValue().ToDecimalOrDefault();

        //    if (row.Cells[2].TextValue() == string.Empty || Category_ID == null || Cost < 0 || row.Cells[14].TextValue() == string.Empty || Price < 0 || UOM_ID == null)
        //    {
        //        UserMessages.Message(null, string.Format(row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.IncompleteData, "البضاعة", index + 1), string.Empty);
        //        return false;
        //    }

        //    if (Price < Cost)
        //    {
        //        UserMessages.Message(null, row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
        //        return false;
        //    }
        //    //result = dc.usp_Items_insert(row.Cells[2].TextValue().ToString(), Category_ID, 'i', Cost, UOM_ID, null, 0, 0, null, row.Cells[23].TextValue().ToString(), row.Cells[14].TextValue().ToString(), Price, 0, row.Cells[1].TextValue().ToString(), row.Cells[2].TextValue().ToString());
        //    result = dc.usp_Items_insert(row.Cells[2].TextValue().ToString(), Category_ID, 'i', Cost, UOM_ID, null, row.Cells[24].TextValue().ToDecimalOrDefault(), row.Cells[23].TextValue().ToDecimalOrDefault(), null, row.Cells[25].TextValue().ToString(), row.Cells[14].TextValue().ToString(), Price, 0, row.Cells[1].TextValue().ToString(), row.Cells[3].TextValue().ToString());

        //    if (result == -2)
        //    {
        //        UserMessages.Message(null, row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
        //        return false;
        //    }

        //    if (result == -3)
        //    {
        //        UserMessages.Message(null, row.Cells[14].TextValue() + " : " + Resources.UserInfoMessages.BarcodeExists, string.Empty);
        //        return false;
        //    }


        //    //unit2
        //    if (!string.IsNullOrEmpty(row.Cells[31].TextValue()))
        //    {

        //        var UOM1 = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[31].TextValue()).FirstOrDefault();
        //        int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
        //        if (UOM1 == null)
        //        {
        //            UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(row.Cells[31].TextValue(), row.Cells[31].TextValue(), null);
        //        }
        //        try
        //        {
        //            dc.usp_ItemsUnits_Insert(result, UOM_ID1, row.Cells[32].TextValue().ToDecimal(), row.Cells[33].TextValue().ToDecimalOrDefault(), row.Cells[35].TextValue(), null);
        //        }
        //        catch
        //        {

        //            UserMessages.Message(null, row.Cells[1].TextValue(), string.Empty);
        //            return false;
        //        }


        //    }





        //    //unit3
        //    if (!string.IsNullOrEmpty(row.Cells[36].TextValue()))
        //    {

        //        var UOM1 = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[36].TextValue()).FirstOrDefault();
        //        int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
        //        if (UOM1 == null)
        //        {
        //            UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(row.Cells[31].TextValue(), row.Cells[36].TextValue(), null);
        //        }
        //        try
        //        {
        //            dc.usp_ItemsUnits_Insert(result, UOM_ID1, row.Cells[37].TextValue().ToDecimal(), row.Cells[38].TextValue().ToDecimalOrDefault(), row.Cells[40].TextValue(), null);

        //        }
        //        catch
        //        {

        //            UserMessages.Message(null, row.Cells[2].TextValue() + " : " + row.Cells[1].TextValue(), string.Empty);
        //            return false;
        //        }


        //    }



        //    //سعر الشراء
        //    try
        //    {
        //        decimal Price1 = row.Cells[6].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 88, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }


        //    //سعر التكلفة
        //    try
        //    {
        //        decimal Price1 = row.Cells[7].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 89, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }

        //    // سعر المبيع
        //    try
        //    {
        //        decimal Price1 = row.Cells[8].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 90, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }



        //    // سعر الجملة
        //    try
        //    {
        //        decimal Price1 = row.Cells[9].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 91, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }




        //    //  سعر ن/الجملة
        //    try
        //    {
        //        decimal Price1 = row.Cells[10].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 92, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }




        //    //  سعر التصدير
        //    try
        //    {
        //        decimal Price1 = row.Cells[11].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 93, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }


        //    //  سعر المستهلك
        //    try
        //    {
        //        decimal Price1 = row.Cells[12].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 94, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    }



        //    //آخر شراء
        //    try
        //    {
        //        decimal Price1 = row.Cells[13].TextValue().ToDecimalOrDefault();
        //        if (Price1 > 0)
        //        {
        //            dc.usp_ItemsPrices_Insert(result, 95, Price1, null);
        //        }
        //    }
        //    catch
        //    {

        //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
        //        return false;
        //    } 

        //    index++;
        //}
        return true;
    }

    //private bool InsertItems()
    //{
    //    if (dtAllAdded.Rows.Count == 0)
    //    {
    //        UserMessages.Message(null, "بيانات غير مكتملة", string.Empty);
    //        return false; ;
    //    }
    //    var listCategory = dc.usp_ItemsCategories_Select(string.Empty).ToList();
    //    var listGeneralAttribute = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).ToList();

    //    for (int i = 0; i < dtFile.Rows.Count; i++)
    //    {
    //        var row = dtFile.Rows[i];
    //        int? Category_ID = null;
    //        int? UOM_ID = null;
    //        string nameCategory = string.Empty,
    //               nameItem = string.Empty,
    //               nameItemEn = string.Empty,
    //               BarcodeItem = string.Empty,
    //               CodeItem = string.Empty,
    //               UnitItem = string.Empty,

    //               UnitFirstItem = string.Empty,
    //               BarcodeFirstItem = string.Empty,



    //               UnitSecondItem = string.Empty,
    //               BarcodeSecondItem = string.Empty;



    //        decimal CostItem = 0,
    //          PriceItem = 0,
    //          PriceFirstItem = 0,
    //          PriceSecondItem = 0,
    //          RatioFirstItem = 1,
    //          RatioSecondItem = 1;


    //        #region Name Item

    //        //Item Arabic
    //        DataRow[] filtereItemRows = this.dtAllAdded.Select("Name_ID=0");
    //        if (filtereItemRows.Any())
    //        {
    //            var objItem = filtereItemRows[0];
    //            nameItem = row[objItem["Value_ID"].ToInt()].ToExpressString();
    //        }


    //        #endregion

    //        #region Name Item En

    //        //Item Arabic
    //        DataRow[] filtereItemEnRows = this.dtAllAdded.Select("Name_ID=1");
    //        if (filtereItemEnRows.Any())
    //        {
    //            var objItemEn = filtereItemEnRows[0];
    //            nameItemEn = row[objItemEn["Value_ID"].ToInt()].ToExpressString();
    //        }


    //        #endregion

    //        #region Category

    //        //Category
    //        DataRow[] filtereCustomerdRows = this.dtAllAdded.Select("Name_ID=2");
    //        if (filtereCustomerdRows.Any())
    //        {
    //            var objCustomer = filtereCustomerdRows[0];
    //            nameCategory = row[objCustomer["Value_ID"].ToInt()].ToExpressString();
    //        }
    //        #endregion

    //        #region Name Item En

    //        //Item Cost
    //        DataRow[] filtereItemCostRows = this.dtAllAdded.Select("Name_ID=3");
    //        if (filtereItemCostRows.Any())
    //        {
    //            var objItemCost = filtereItemCostRows[0];
    //            CostItem = row[objItemCost["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }


    //        #endregion

    //        #region Price

    //        //Item Price
    //        DataRow[] filtereItemPriceRows = this.dtAllAdded.Select("Name_ID=4");
    //        if (filtereItemPriceRows.Any())
    //        {
    //            var objItemPrice = filtereItemPriceRows[0];
    //            PriceItem = row[objItemPrice["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }

    //        #endregion

    //        #region Barcode

    //        //Item Price
    //        DataRow[] filtereItemBarcodeRows = this.dtAllAdded.Select("Name_ID=5");
    //        if (filtereItemBarcodeRows.Any())
    //        {
    //            var objItemBarcode = filtereItemBarcodeRows[0];
    //            BarcodeItem = row[objItemBarcode["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Code

    //        //Item Price
    //        DataRow[] filtereItemCodeRows = this.dtAllAdded.Select("Name_ID=6");
    //        if (filtereItemCodeRows.Any())
    //        {
    //            var objItemCode = filtereItemCodeRows[0];
    //            CodeItem = row[objItemCode["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Unit

    //        //Item Price
    //        DataRow[] filtereItemUnitRows = this.dtAllAdded.Select("Name_ID=7");
    //        if (filtereItemUnitRows.Any())
    //        {
    //            var objItemUnit = filtereItemUnitRows[0];
    //            UnitItem = row[objItemUnit["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion


    //        #region *First*

    //        #region Price

    //        //Item Price
    //        DataRow[] filtereItemUnitFirstRows = this.dtAllAdded.Select("Name_ID=8");
    //        if (filtereItemUnitFirstRows.Any())
    //        {
    //            var objItemUnitFirst = filtereItemUnitFirstRows[0];
    //            UnitFirstItem = row[objItemUnitFirst["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Barcode

    //        //Item Price
    //        DataRow[] filtereItemBarcodeFirstRows = this.dtAllAdded.Select("Name_ID=9");
    //        if (filtereItemBarcodeFirstRows.Any())
    //        {
    //            var objItemBarcodeFirst = filtereItemBarcodeFirstRows[0];
    //            BarcodeFirstItem = row[objItemBarcodeFirst["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Code

    //        //Item Price
    //        DataRow[] filtereItemRatioRows = this.dtAllAdded.Select("Name_ID=10");
    //        if (filtereItemRatioRows.Any())
    //        {
    //            var objItemRatio = filtereItemRatioRows[0];
    //            RatioFirstItem = row[objItemRatio["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }

    //        #endregion

    //        #region Unit

    //        //Item Price
    //        DataRow[] filtereItemPriceFirstRows = this.dtAllAdded.Select("Name_ID=11");
    //        if (filtereItemPriceFirstRows.Any())
    //        {
    //            var objItemPriceFirst = filtereItemPriceFirstRows[0];
    //            PriceFirstItem = row[objItemPriceFirst["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }

    //        #endregion
    //        #endregion


    //        #region *Second*

    //        #region Unit

    //        //Item Unit
    //        DataRow[] filtereItemUnitSecondRows = this.dtAllAdded.Select("Name_ID=12");
    //        if (filtereItemUnitSecondRows.Any())
    //        {
    //            var objItemUnitSecond = filtereItemUnitSecondRows[0];
    //            UnitSecondItem = row[objItemUnitSecond["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Barcode

    //        //Item Barcode
    //        DataRow[] filtereItemBarcodeSecondRows = this.dtAllAdded.Select("Name_ID=13");
    //        if (filtereItemBarcodeSecondRows.Any())
    //        {
    //            var objItemBarcodeSecond = filtereItemBarcodeSecondRows[0];
    //            BarcodeSecondItem = row[objItemBarcodeSecond["Value_ID"].ToInt()].ToExpressString();
    //        }

    //        #endregion

    //        #region Ratio

    //        //Item Ratio
    //        DataRow[] filtereItemRatioSecondRows = this.dtAllAdded.Select("Name_ID=14");
    //        if (filtereItemRatioSecondRows.Any())
    //        {
    //            var objItemRatioSecond = filtereItemRatioSecondRows[0];
    //            RatioSecondItem = row[objItemRatioSecond["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }

    //        #endregion

    //        #region Price

    //        //Item Price
    //        DataRow[] filtereItemPriceSecondRows = this.dtAllAdded.Select("Name_ID=15");
    //        if (filtereItemPriceSecondRows.Any())
    //        {
    //            var objItemPriceSecond = filtereItemPriceSecondRows[0];
    //            PriceSecondItem = row[objItemPriceSecond["Value_ID"].ToInt()].ToDecimalOrDefault();
    //        }

    //        #endregion
    //        #endregion


    //        #region Category Insert Test

    //        if (!string.IsNullOrEmpty(nameCategory))
    //        {
    //            var Category = listCategory.Where(x => x.Name == nameCategory).FirstOrDefault();
    //            Category_ID = (Category != null) ? Category.ID : (int?)null;

    //            if (UOM_ID == null)
    //            {
    //                UserMessages.Message(null, " غير موجوده " + " (الفئة) " + " **" + nameCategory + "**", string.Empty);
    //            }
    //            //if (Category_ID == null)
    //            //{
    //            //    Category_ID = dc.usp_ItemsCategories_Insert(nameCategory, null, false, null, false, false);
    //            //}
    //        }
    //        else
    //        {
    //            UserMessages.Message(null, "بيانات غير مكتملة" + " (الفئة) " + " **" + nameCategory + "**", string.Empty);
    //            return false; ;
    //        }

    //        #endregion


    //        #region Unit Insert With Test
    //        if (!string.IsNullOrEmpty(UnitItem))
    //        {
    //            var UOM = listGeneralAttribute.Where(x => x.Name == UnitItem).FirstOrDefault();
    //            UOM_ID = (UOM != null) ? UOM.ID : (int?)null;
    //            if (UOM_ID == null)
    //            {
    //                UserMessages.Message(null, " غير موجوده " + " (الوحدة) " + " **" + UnitItem + "**", string.Empty);
    //            }
    //        }
    //        else
    //        {
    //            UserMessages.Message(null, "بيانات غير مكتملة" + " (الوحدة) " + " **" + UnitItem + "**", string.Empty);
    //            return false; ;
    //        }
    //        #endregion


    //        decimal Cost = CostItem;
    //        decimal Price = PriceItem;

    //        //if (row.Cells[2].TextValue() == string.Empty || Category_ID == null || Cost < 0 || row.Cells[14].TextValue() == string.Empty || Price < 0 || UOM_ID == null)
    //        //{
    //        //    UserMessages.Message(null, string.Format(row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.IncompleteData, "الاصناف", i + 1), string.Empty);
    //        //    return false;
    //        //}

    //        if (Price < Cost)
    //        {
    //            UserMessages.Message(null, nameItem + " : " + Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
    //            return false;
    //        }
    //        var result = dc.usp_Items_insert(nameItem, Category_ID, 'i', Cost, UOM_ID, null, null, null, null, string.Empty, BarcodeItem, Price, 0, CodeItem, nameItemEn, 0, 0);

    //        if (result == -2)
    //        {
    //            UserMessages.Message(null, nameItem + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
    //            return false;
    //        }

    //        if (result == -3)
    //        {
    //            UserMessages.Message(null, BarcodeItem + " : " + Resources.UserInfoMessages.BarcodeExists, string.Empty);
    //            return false;
    //        }



    //        //unit1
    //        if (!string.IsNullOrEmpty(UnitFirstItem))
    //        {

    //            var UOM1 = listGeneralAttribute.Where(x => x.Name == UnitFirstItem).FirstOrDefault();
    //            int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
    //            if (UOM1 == null)
    //            {
    //                UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(UnitFirstItem, UnitFirstItem, null);
    //            }
    //            try
    //            {
    //                dc.usp_ItemsUnits_Insert(result, UOM_ID1, RatioFirstItem, PriceFirstItem, BarcodeFirstItem, null);
    //            }
    //            catch
    //            {
    //                UserMessages.Message(null, nameItem, string.Empty);
    //                return false;
    //            }
    //        }

    //        //unit2
    //        if (!string.IsNullOrEmpty(UnitSecondItem))
    //        {

    //            var UOM1 = listGeneralAttribute.Where(x => x.Name == UnitSecondItem).FirstOrDefault();
    //            int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
    //            if (UOM1 == null)
    //            {
    //                UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(UnitSecondItem, UnitSecondItem, null);
    //            }
    //            try
    //            {
    //                dc.usp_ItemsUnits_Insert(result, UOM_ID1, RatioSecondItem, PriceSecondItem, BarcodeSecondItem, null);
    //            }
    //            catch
    //            {
    //                UserMessages.Message(null, nameItem, string.Empty);
    //                return false;
    //            }
    //        }




    //    }




    //    //int index = 0;
    //    //int result = 0;
    //    // foreach (var row in worksheet.Rows)
    //    //{
    //    //    if (index == 0) { index++; continue; }

    //    //    for (int i = 0; i < 6; i++)
    //    //    {
    //    //        if (row.Cells[i].TextValue() == null)
    //    //        {
    //    //            UserMessages.Message(null, string.Format(Resources.UserInfoMessages.CorruptedData, "البضاعة", index + 1), string.Empty);
    //    //            return false;
    //    //        }
    //    //    }

    //    //    var Category = dc.usp_ItemsCategories_Select(string.Empty).Where(x => x.Name == row.Cells[16].TextValue()).FirstOrDefault();
    //    //    int? Category_ID = (Category != null) ? Category.ID : (int?)null;


    //    //    if (Category_ID == null)
    //    //    {
    //    //        Category_ID = dc.usp_ItemsCategories_Insert(row.Cells[16].TextValue(), null, false, null);
    //    //    }



    //    //    var UOM = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[4].TextValue()).FirstOrDefault();
    //    //    int? UOM_ID = (UOM != null) ? UOM.ID : (int?)null;





    //    //    decimal Cost = row.Cells[7].TextValue().ToDecimalOrDefault();
    //    //    decimal Price = row.Cells[8].TextValue().ToDecimalOrDefault();

    //    //    if (row.Cells[2].TextValue() == string.Empty || Category_ID == null || Cost < 0 || row.Cells[14].TextValue() == string.Empty || Price < 0 || UOM_ID == null)
    //    //    {
    //    //        UserMessages.Message(null, string.Format(row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.IncompleteData, "البضاعة", index + 1), string.Empty);
    //    //        return false;
    //    //    }

    //    //    if (Price < Cost)
    //    //    {
    //    //        UserMessages.Message(null, row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
    //    //        return false;
    //    //    }
    //    //    //result = dc.usp_Items_insert(row.Cells[2].TextValue().ToString(), Category_ID, 'i', Cost, UOM_ID, null, 0, 0, null, row.Cells[23].TextValue().ToString(), row.Cells[14].TextValue().ToString(), Price, 0, row.Cells[1].TextValue().ToString(), row.Cells[2].TextValue().ToString());
    //    //    result = dc.usp_Items_insert(row.Cells[2].TextValue().ToString(), Category_ID, 'i', Cost, UOM_ID, null, row.Cells[24].TextValue().ToDecimalOrDefault(), row.Cells[23].TextValue().ToDecimalOrDefault(), null, row.Cells[25].TextValue().ToString(), row.Cells[14].TextValue().ToString(), Price, 0, row.Cells[1].TextValue().ToString(), row.Cells[3].TextValue().ToString());

    //    //    if (result == -2)
    //    //    {
    //    //        UserMessages.Message(null, row.Cells[2].TextValue() + " : " + Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
    //    //        return false;
    //    //    }

    //    //    if (result == -3)
    //    //    {
    //    //        UserMessages.Message(null, row.Cells[14].TextValue() + " : " + Resources.UserInfoMessages.BarcodeExists, string.Empty);
    //    //        return false;
    //    //    }


    //    //    //unit2
    //    //    if (!string.IsNullOrEmpty(row.Cells[31].TextValue()))
    //    //    {

    //    //        var UOM1 = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[31].TextValue()).FirstOrDefault();
    //    //        int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
    //    //        if (UOM1 == null)
    //    //        {
    //    //            UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(row.Cells[31].TextValue(), row.Cells[31].TextValue(), null);
    //    //        }
    //    //        try
    //    //        {
    //    //            dc.usp_ItemsUnits_Insert(result, UOM_ID1, row.Cells[32].TextValue().ToDecimal(), row.Cells[33].TextValue().ToDecimalOrDefault(), row.Cells[35].TextValue(), null);
    //    //        }
    //    //        catch
    //    //        {

    //    //            UserMessages.Message(null, row.Cells[1].TextValue(), string.Empty);
    //    //            return false;
    //    //        }


    //    //    }





    //    //    //unit3
    //    //    if (!string.IsNullOrEmpty(row.Cells[36].TextValue()))
    //    //    {

    //    //        var UOM1 = dc.usp_GeneralAttributes_Select(0, GeneralAttributes.UOM.ToInt()).Where(x => x.Name == row.Cells[36].TextValue()).FirstOrDefault();
    //    //        int? UOM_ID1 = (UOM1 != null) ? UOM1.ID : (int?)null;
    //    //        if (UOM1 == null)
    //    //        {
    //    //            UOM_ID1 = dc.usp_GeneralAttributesCustomControl_insert(row.Cells[31].TextValue(), row.Cells[36].TextValue(), null);
    //    //        }
    //    //        try
    //    //        {
    //    //            dc.usp_ItemsUnits_Insert(result, UOM_ID1, row.Cells[37].TextValue().ToDecimal(), row.Cells[38].TextValue().ToDecimalOrDefault(), row.Cells[40].TextValue(), null);

    //    //        }
    //    //        catch
    //    //        {

    //    //            UserMessages.Message(null, row.Cells[2].TextValue() + " : " + row.Cells[1].TextValue(), string.Empty);
    //    //            return false;
    //    //        }


    //    //    }



    //    //    //سعر الشراء
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[6].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 88, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }


    //    //    //سعر التكلفة
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[7].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 89, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }

    //    //    // سعر المبيع
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[8].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 90, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }



    //    //    // سعر الجملة
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[9].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 91, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }




    //    //    //  سعر ن/الجملة
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[10].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 92, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }




    //    //    //  سعر التصدير
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[11].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 93, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }


    //    //    //  سعر المستهلك
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[12].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 94, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    }



    //    //    //آخر شراء
    //    //    try
    //    //    {
    //    //        decimal Price1 = row.Cells[13].TextValue().ToDecimalOrDefault();
    //    //        if (Price1 > 0)
    //    //        {
    //    //            dc.usp_ItemsPrices_Insert(result, 95, Price1, null);
    //    //        }
    //    //    }
    //    //    catch
    //    //    {

    //    //        UserMessages.Message(null, row.Cells[0].TextValue(), string.Empty);
    //    //        return false;
    //    //    } 

    //    //    index++;
    //    //}
    //    return true;
    //}

    private bool InsertBanks()
    {

        var cmp = dc.usp_Company_Select().FirstOrDefault();
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            string nameBank = string.Empty;
            //Customer
            DataRow[] filtereBankdRows = this.dtAllAdded.Select("Name_ID=0");
            if (filtereBankdRows.Any())
            {
                var objBank = filtereBankdRows[0];
                nameBank = row[objBank["Value_ID"].ToInt()].ToExpressString();
            }

            string nameBranch = string.Empty;
            //Branch Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");
            if (filteredRows.Any())
            {
                var obj = filteredRows[0];
                var indexBranch = (obj["Value_ID"].ToExpressString()).ToInt();
                nameBranch = row[indexBranch].ToExpressString();
            }


            decimal valueBalance = 0;
            //Balance Propertie
            DataRow[] filteredBalanceRows = this.dtAllAdded.Select("Name_ID=3");

            if (filteredBalanceRows.Any())
            {
                var objBalance = filteredBalanceRows[0];
                var indexdBalance = (objBalance["Value_ID"].ToExpressString()).ToInt();
                valueBalance = row[indexdBalance].ToExpressString().ToDecimalOrDefault();
            }

            string nameTelephon = string.Empty;
            //Telephone Propertie
            DataRow[] filteredTelephonRows = this.dtAllAdded.Select("Name_ID=4");
            if (filteredTelephonRows.Any())
            {
                var objTelephon = filteredTelephonRows[0];
                var indexTelephon = (objTelephon["Value_ID"].ToExpressString()).ToInt();
                nameTelephon = row[indexTelephon].ToExpressString();
            }


            string nameAddress = string.Empty;
            //Address Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=5");
            if (filteredStoreRows.Any())
            {
                var objAddress = filteredStoreRows[0];
                var indexAddress = (objAddress["Value_ID"].ToExpressString()).ToInt();
                nameAddress = (objAddress["Value"].ToExpressString());

            }

            var Branch = dc.usp_Branchs_Select(string.Empty, null).Where(x => x.Name == nameBranch).FirstOrDefault();
            int? Branch_ID = (Branch != null) ? Branch.ID : (int?)null;

            var company = dc.usp_Company_Select().FirstOrDefault();
            int? Currency_ID = company.Currency_ID;

            decimal OpenBalance = valueBalance;
            decimal Ratio = 1;

            if (nameBranch == string.Empty || (Branch_ID == null && MyContext.Features.BranchesEnabled && OpenBalance != 0) || (OpenBalance != 0 && Ratio == 0) || Currency_ID == null)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "الموردين", i + 1), string.Empty);
                return false;
            }

            int result = 0;
            result = dc.usp_Bank_Insert(nameBank, COA.BankExpensesAndBenefits.ToInt(), Branch_ID, cmp.Currency_ID, 1, valueBalance, cmp.FiscalYearEndDate, COA.Capital.ToInt(), nameAddress, nameTelephon, string.Empty);

            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);

                return false;
            }
            if (result == -35)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.ParentAccountNoChange, string.Empty);

                return false;
            }
            if (result == -30)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                return false;
            }

        }
        return true;


        return true;
    }


    private bool InsertUnit()
    {

        List<string> lst = new List<string>();
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            if (row[0].ToExpressString() == string.Empty)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "الوحدات", i + 1), string.Empty);
                return false;
            }

            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=0");
            var obj = filteredRows[0];
            var index = (obj["Value_ID"].ToExpressString()).ToInt();

            //var result = dc.usp_ItemsCategories_Insert(row[index].ToExpressString(), null, true, null, false, false);

            //if (result == -2)
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
            //    return false;
            //}
            //  }

            lst.Add(row[index].ToExpressString());
            //  int result = dc.usp_GeneralAttributesCustomControl_insert(row[index].ToExpressString(), row[index].ToExpressString(), 14);
        }

        foreach (var item in lst.Distinct())
        {
            // var result = dc.usp_ItemsCategories_Insert(item, null, true, null, false, false);
            // int result = dc.usp_GeneralAttributesCustomControl_insert(row[index].ToExpressString(), row[index].ToExpressString(), 14);
            int result = dc.usp_GeneralAttributesCustomControl_insert(item, item, 14);
        }


        return true;
    }
    private bool InsertCategory()
    {

        List<string> lst = new List<string>();
        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];
            if (row[0].ToExpressString() == string.Empty)
            {
                UserMessages.Message(null, string.Format(Resources.UserInfoMessages.IncompleteData, "الفئات", i + 1), string.Empty);
                return false;
            }

            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=0");
            var obj = filteredRows[0];
            var index = (obj["Value_ID"].ToExpressString()).ToInt();
            lst.Add(row[index].ToExpressString());
            //  int result = dc.usp_GeneralAttributesCustomControl_insert(row[index].ToExpressString(), row[index].ToExpressString(), 14);
        }

        foreach (var item in lst.Distinct())
        {
            dc.usp_ItemsCategories_Insert(item, null, true, null, false, false,null,null,null,null);
            //  int result = dc.usp_GeneralAttributesCustomControl_insert(item, item, 14);
        }


        return true;
    }


    protected void imgButtonDelete_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void gvExportList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataRow dr = this.dtAllAdded.Select("ID=" + gvExportList.DataKeys[e.RowIndex]["ID"].ToExpressString())[0];
            dr.Delete();
            this.dtAllAdded.AcceptChanges();
            gvExportList.DataSource = this.dtAllAdded;
            gvExportList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}



public class OperationCls
{
    public string cachedNumber { get; set; }
    public int Account_id { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string CostCenter { get; set; }
    public int CostCenter_ID { get; set; }
    public DateTime DateOperation { get; set; }
    public string Note { get; set; }

}