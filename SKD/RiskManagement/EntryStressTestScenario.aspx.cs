using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class WebUI_RiskManagement_EntryStressTestScenario : System.Web.UI.Page
{
    private string eType
    {
        get { 
            return Request.QueryString["eType"]; 
        }
    }

    private decimal eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(Request.QueryString["eID"]);
            }
        }
        set { ViewState["eID"] = value; }
    }

    private string RowType
    {
        get { return ViewState["RowType"].ToString(); }
        set { ViewState["RowType"] = value; }
    }

    private string TempStressTest
    {
        get { return ViewState["TempStressTest"].ToString(); }
        set { ViewState["TempStressTest"] = value; }
    }

    private int EditRowIndex
    {
        get { return (int)ViewState["EditRowIndex"]; }
        set { ViewState["EditRowIndex"] = value; }
    }

    private ScenarioContractData.ScenarioContractDataTable dtScenarioContract
    {
        get { return (ScenarioContractData.ScenarioContractDataTable)ViewState["dtScenarioContract"]; }
        set { ViewState["dtScenarioContract"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uiBLError.Visible = false;

            if (!Page.IsPostBack)
            {
                dtScenarioContract = new ScenarioContractData.ScenarioContractDataTable();
                FillScenarioContractDataGrid();
            }

            if (eType == "add")
            {
               // uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                bindData();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtScenarioContract.Count == 0)
            {
                AddRow();
            }
            else
            {
                if (uiDgScenarioContract.EditIndex == -1)
                {
                    AddRow();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    private void FillScenarioContractGrid()
    {
        uiDgScenarioContract.DataSource = ObjectDataSourceScenarioContract;
        IEnumerable dv = (IEnumerable)ObjectDataSourceScenarioContract.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgScenarioContract.DataSource = dve;
        uiDgScenarioContract.DataBind();
    }


    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RiskManagement/ViewStressTestScenario.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                StressTestScenario.UpdateStressTestScenario(uiTxtScenarioName.Text, DateTime.Parse(CtlCalendarPickUp1.Text),
                        eID, User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, dtScenarioContract);

                 Response.Redirect("~/RiskManagement/ViewStressTestScenario.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }


    }

    protected void uiBtnRun_Click(object sender, EventArgs e)
    {
        decimal scenarioId = eID;
        if (scenarioId == 0)
        {
            throw new ApplicationException("Invalid Scenario");
        }

        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter taScenario =
            new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();

        StressTestScenarioDataTableAdapters.QueriesTableAdapter ta =
            new StressTestScenarioDataTableAdapters.QueriesTableAdapter();

        int? scenarioExists = taScenario.CountScenario(scenarioId);

        if (!scenarioExists.HasValue)
        {
            throw new ApplicationException("Invalid Scenario");
        }
        else if (scenarioExists == 0)
        {
            throw new ApplicationException("Invalid Scenario");
        }

        ta.CommandTimeOut = 120000;
        ta.EOD_ProcessEOD(DateTime.Now.Date, "STRESS", "GLOBAL", null, null,
                            scenarioId, User.Identity.Name, Request.UserHostAddress);

        Response.Redirect("StressTest.aspx?id=" + scenarioId.ToString());
    }


    #region " ---- Scenario Contract ----"

    private void BindViewStateToDataGrid()
    {
        try
        {
            uiDgScenarioContract.DataSource = dtScenarioContract;
            uiDgScenarioContract.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void DisableDataGridRowEnabled(int rowIndex)
    {
            for (int ii = 0; ii < dtScenarioContract.Count; ii++)
            {
                if (ii != rowIndex)
                {
                    uiDgScenarioContract.Rows[ii].Enabled = false;
                }
            }
    }

    protected void uiDgScenarioContract_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            
            if (RowType == "add")
            {
                dtScenarioContract.Rows[e.RowIndex].RejectChanges();
                uiBLErrorDetail.Visible = false;
                uiBLErrorDetail.Items.Clear();
            }

            uiDgScenarioContract.EditIndex = -1;

            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgScenarioContract_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            RowType = "edit";
            EditRowIndex = e.NewEditIndex;  
            ScenarioContractData.ScenarioContractRow dr = null;
            // get lastprice from tradefeed
            ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();
            uiDgScenarioContract.EditIndex = e.NewEditIndex;
            Label scenarioType = (Label)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiLblScenarioType");
            Label contractID = (Label)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiLblContractID");
            Label bestPrice = (Label)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiLblBasePrice");

            dr = dtScenarioContract[e.NewEditIndex];
            TempStressTest = dr.ContractID.ToString();

            BindViewStateToDataGrid();

            Lookup_CtlContractCommodityStressTestLookup contract = (Lookup_CtlContractCommodityStressTestLookup)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("CtlContractCommodityLookup");
            contract.SetContractCommodityValue(contractID.Text, ScenarioContract.GetContractDataByContractID(decimal.Parse(contractID.Text)));
            TextBox basePrice = (TextBox)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiTxtBestPrice");
            TextBox min = (TextBox)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiTxtLow");
            TextBox max = (TextBox)uiDgScenarioContract.Rows[e.NewEditIndex].FindControl("uiTxtHigh");
            contract.uiTxtLookupHighID = max.ClientID;
            contract.uiTxtLookupLowID = min.ClientID;
            
            if (dr.ScenarioType == "P")
            {
                basePrice.Text = dr.BasePrice.ToString("#,##0.###");
                if (!dr.IsLowNull() && !dr.IsHighNull())
                {
                    dr.Low = decimal.Parse(min.Text);
                    dr.High = decimal.Parse(max.Text);
                }
                else
                {
                    min.Text = "";
                    max.Text = "";
                }
            }
            else if(dr.ScenarioType == "V")
            {
                basePrice.Enabled = false;
                dr.BasePrice = 0;
                if (!dr.IsLowNull() && !dr.IsHighNull())
                {
                    dr.Low = decimal.Parse(min.Text);
                    dr.High = decimal.Parse(max.Text);
                }
                else
                {
                    min.Text = "";
                    max.Text = "";
                }
            }

            DisableDataGridRowEnabled(e.NewEditIndex);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgScenarioContract_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (IsValidDetailEntry(e.RowIndex) == true)
            {
                ScenarioContractData.ScenarioContractRow dr = null;

                Lookup_CtlContractCommodityStressTestLookup contract = (Lookup_CtlContractCommodityStressTestLookup)uiDgScenarioContract.Rows[e.RowIndex].FindControl("CtlContractCommodityLookup");
                DropDownList scenarioType = (DropDownList)uiDgScenarioContract.Rows[e.RowIndex].FindControl("uiDdlScenarioType");
                TextBox bestPrice = (TextBox)uiDgScenarioContract.Rows[e.RowIndex].FindControl("uiTxtBestPrice");
                TextBox min = (TextBox)uiDgScenarioContract.Rows[e.RowIndex].FindControl("uiTxtLow");
                TextBox max = (TextBox)uiDgScenarioContract.Rows[e.RowIndex].FindControl("uiTxtHigh");
                HiddenField uiHiddenHigh = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenHigh");
                HiddenField uiHiddenLow = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenLow");
                contract.uiTxtLookupHighID = max.ClientID;
                contract.uiTxtLookupLowID = min.ClientID;

                // get lastprice from tradefeed
                ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();


                if (scenarioType.SelectedValue == "P")
                {
                    dr = dtScenarioContract[e.RowIndex];
                    dr.ContractID = decimal.Parse(contract.LookupTextBoxID);
                    dr.ScenarioType = scenarioType.SelectedValue;
                    decimal lastprice = (decimal)taTradeFeed.GetLastPrice(Convert.ToDecimal(dr.ContractID), DateTime.Parse(CtlCalendarPickUp1.Text));
                    dr.BasePrice = decimal.Parse(bestPrice.Text);
                    bestPrice.Enabled = true;
                    uiHiddenHigh.Value = max.Text;
                    uiHiddenLow.Value = min.Text;
                    if (!dr.IsLowNull() && !dr.IsHighNull())
                    {
                        dr.Low = decimal.Parse(min.Text);
                        dr.High = decimal.Parse(max.Text);
                    }
                    else if (min.Text != "" && min.Text != "")
                    {
                        min.Text = uiHiddenLow.Value;
                        max.Text = uiHiddenHigh.Value;
                        dr.Low = decimal.Parse(min.Text);
                        dr.High = decimal.Parse(max.Text);
                    }
                    else 
                    {
                        min.Text = "";
                        max.Text = "";
                    }
                }
                else if (scenarioType.SelectedValue == "V")
                {
                    dr = dtScenarioContract[e.RowIndex];
                    dr.ContractID = decimal.Parse(contract.LookupTextBoxID);
                    dr.ScenarioType = scenarioType.SelectedValue;
                    dr.BasePrice = 0;

                    if (!dr.IsLowNull() && !dr.IsHighNull())
                    {
                        dr.Low = Convert.ToDecimal(min.Text);
                        dr.High = Convert.ToDecimal(max.Text);
                    }
                    uiHiddenHigh.Value = max.Text;
                    uiHiddenLow.Value = min.Text;
                    if (min.Text == "" && max.Text == "")
                    {
                        min.Text = "";
                        max.Text = "";
                    }
                    min.Text = uiHiddenLow.Value;
                    max.Text = uiHiddenHigh.Value;
                    dr.Low = decimal.Parse(min.Text);
                    dr.High = decimal.Parse(max.Text);
                }
                uiDgScenarioContract.EditIndex = -1;

                BindViewStateToDataGrid();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgScenarioContract_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtScenarioContract.Rows[e.RowIndex].Delete();
            dtScenarioContract.AcceptChanges();

            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private bool IsValidDetailEntry(int rowIndex)
    {
        try
        {
            bool isValid = true;
            uiBLErrorDetail.Visible = false;
            uiBLErrorDetail.Items.Clear();

            Lookup_CtlContractCommodityStressTestLookup contractId = (Lookup_CtlContractCommodityStressTestLookup)uiDgScenarioContract.Rows[rowIndex].FindControl("CtlContractCommodityLookup");
            DropDownList scenarioType = (DropDownList)uiDgScenarioContract.Rows[rowIndex].FindControl("uiDdlScenarioType");
            TextBox bestPrice = (TextBox)uiDgScenarioContract.Rows[rowIndex].FindControl("uiTxtBestPrice");
            TextBox min = (TextBox)uiDgScenarioContract.Rows[rowIndex].FindControl("uiTxtLow");
            TextBox max = (TextBox)uiDgScenarioContract.Rows[rowIndex].FindControl("uiTxtHigh");
            HiddenField uiHiddenHigh = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenHigh");
            HiddenField uiHiddenLow = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenLow");
            contractId.uiTxtLookupHighID = max.ClientID;
            contractId.uiTxtLookupLowID = min.ClientID;
                 
            if (string.IsNullOrEmpty(contractId.LookupTextBoxID) != null)
            {
                if (contractId.LookupTextBoxID == "0")
                {
                    isValid = false;
                    uiBLErrorDetail.Items.Add(string.Format("Row {0} : Contract is required.", rowIndex + 1));
                }
                else
                {
                    foreach (ScenarioContractData.ScenarioContractRow dr in dtScenarioContract)
                    {
                        if (contractId.LookupTextBoxID == dr.ContractID.ToString() && contractId.LookupTextBoxID != TempStressTest)
                        {
                            uiBLErrorDetail.Items.Add(string.Format("Row {0} : contract already exist.", rowIndex + 1));
                            break;
                        }
                    }
                }
            }

            if (uiBLErrorDetail.Items.Count > 0)
            {
                isValid = false;
                uiBLErrorDetail.Visible = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    protected void uiBtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtScenarioContract.Count == 0)
            {
                AddRow();
            }
            else
            {
                if (uiDgScenarioContract.EditIndex == -1)
                {
                    AddRow();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillScenarioContractDataGrid()
    {
        try
        {
            uiDgScenarioContract.DataSource = ObjectDataSourceScenarioContract;
            IEnumerable dv = (IEnumerable)ObjectDataSourceScenarioContract.Select();
            DataView dve = (DataView)dv;
            
            dtScenarioContract = (ScenarioContractData.ScenarioContractDataTable)dve.Table;
            dtScenarioContract.Columns.Add("ScenarioTypeDesc", typeof(string), "iif(ScenarioType = 'V', 'Value', 'Percent')");
           

            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

  
    private void AddRow()
    {
        try
        {
            RowType = "add";
            
            //Reserve data
            //Temporary clear constraint
            // get lastprice from tradefeed

            ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();
            dtScenarioContract.Constraints.Clear();
            ScenarioContractData.ScenarioContractRow dr = dtScenarioContract.NewScenarioContractRow();

            dr.ContractID = 0;
            dr.ScenarioID = eID;
            dr.ScenarioType = "V";
            dr.BasePrice = 0;

            dtScenarioContract.AddScenarioContractRow(dr);

            TempStressTest = "";

            int newEditIndex = uiDgScenarioContract.Rows.Count;
            uiDgScenarioContract.EditIndex = newEditIndex;
            EditRowIndex = newEditIndex;
            

            BindViewStateToDataGrid();
            DisableDataGridRowEnabled(newEditIndex);

            TextBox bestPrice = (TextBox)uiDgScenarioContract.Rows[newEditIndex].FindControl("uiTxtBestPrice");
            bestPrice.Enabled = false;
            bestPrice.Text = "";
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    
    protected void uiDdlScenarioType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RowType == "add")
            {

                if (uiDgScenarioContract.Rows[EditRowIndex].FindControl("CtlContractCommodityLookup") != null)
                {
                    Lookup_CtlContractCommodityStressTestLookup contractId = (Lookup_CtlContractCommodityStressTestLookup)uiDgScenarioContract.Rows[EditRowIndex].FindControl("CtlContractCommodityLookup");
                    DropDownList scenarioType = (DropDownList)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiDdlScenarioType");
                    TextBox bestPrice = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtBestPrice");
                    TextBox min = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtLow");
                    TextBox max = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtHigh");
                    HiddenField uiHiddenHigh = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenHigh");
                    HiddenField uiHiddenLow = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenLow");
                    
                   // get lastprice from tradefeed
                    ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();



                    if (contractId.LookupTextBoxID.ToString() == "")
                    {
                        //get min and max price from tradefeed and baseprice disabled
                        if (scenarioType.SelectedValue == "V")
                        {
                            bestPrice.Enabled = false;
                            bestPrice.Text = "";
                            max.Text = uiHiddenHigh.Value;
                            min.Text = uiHiddenLow.Value;
                        }
                        else if (scenarioType.SelectedValue == "P")
                        {
                            if (contractId.LookupTextBoxID.ToString() == "")
                            {
                                uiBLError.Visible = true;
                                uiBLError.Items.Add("contract is required.");
                                return;
                            }
                            else
                            {
                                decimal lastprice = (decimal)taTradeFeed.GetLastPrice(Convert.ToDecimal(contractId.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text));
                                bestPrice.Text = lastprice.ToString("#,##0.###");
                                bestPrice.Enabled = true;
                                uiHiddenHigh.Value = max.Text;
                                uiHiddenLow.Value = min.Text;
                                min.Text = "";
                                max.Text = "";
                            }
                        }
                    }
                    else
                    {
                        //get min and max price from tradefeed and baseprice disabled
                        if (scenarioType.SelectedValue == "V")
                        {
                            bestPrice.Enabled = false;
                            bestPrice.Text = "";
                            max.Text = uiHiddenHigh.Value;
                            min.Text = uiHiddenLow.Value;
                           
                        }
                        else if (scenarioType.SelectedValue == "P")
                        {
                            decimal lastprice = (decimal)taTradeFeed.GetLastPrice(Convert.ToDecimal(contractId.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text));
                            bestPrice.Text = lastprice.ToString("#,##0.###");
                            bestPrice.Enabled = true;
                            uiHiddenHigh.Value = max.Text;
                            uiHiddenLow.Value = min.Text;
                            min.Text = "";
                            max.Text = "";
                        }
                    }
                }
             }
            else if(RowType == "edit")
            {
                Lookup_CtlContractCommodityStressTestLookup contract = (Lookup_CtlContractCommodityStressTestLookup)uiDgScenarioContract.Rows[EditRowIndex].FindControl("CtlContractCommodityLookup");
                DropDownList scenarioType = (DropDownList)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiDdlScenarioType");
                TextBox bestPrice = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtBestPrice");
                TextBox min = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtLow");
                TextBox max = (TextBox)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiTxtHigh");
                HiddenField uiHiddenHigh = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenHigh");
                HiddenField uiHiddenLow = (HiddenField)uiDgScenarioContract.Rows[EditRowIndex].FindControl("uiHiddenLow");

                // get lastprice from tradefeed
                ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();

                if (scenarioType.SelectedValue == "P")
                {
                    decimal lastprice = (decimal)taTradeFeed.GetLastPrice(Convert.ToDecimal(contract.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text));
                    bestPrice.Text = lastprice.ToString("#,##0.###");
                    bestPrice.Enabled = true;
                    uiHiddenHigh.Value = max.Text;
                    uiHiddenLow.Value = min.Text;
                    min.Text = "";
                    max.Text = "";
                }
                else if (scenarioType.SelectedValue == "V")
                {
                    //get min and max price from tradefeed and baseprice disabled
                    bestPrice.Enabled = false;
                    bestPrice.Text = "";
                    max.Text = uiHiddenHigh.Value;
                    min.Text = uiHiddenLow.Value;
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
       }

   
    #endregion


    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (string.IsNullOrEmpty(uiTxtScenarioName.Text))
            {
                uiBLError.Items.Add("Scenario name is required.");
            }


            if (dtScenarioContract.Count == 0)
            {
                isValid = false;
                uiBLError.Items.Add("Add contract is required.");
            }
            else
            {
                for (int ii = 0; ii < dtScenarioContract.Count; ii++)
                {
                    //isValid = IsValidDetailEntry(ii);
                }
            }

            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    private void bindData()
    {
        try
        {
            StressTestScenarioData.StressTestScenarioRow dr = StressTestScenario.SelectAllByScenarioId(Convert.ToDecimal(eID));

            uiTxtScenarioName.Text = dr.ScenarioName;
            CtlCalendarPickUp1.SetCalendarValue(dr.ScenarioDate.ToString("dd-MMM-yyyy"));
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            if (eType == "edit")
            {
                //uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiDgScenarioContract.Columns[0].Visible = mp.IsMaker;
            uiBtnCreate.Visible = mp.IsMaker;

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    protected void uiDgScenarioContract_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Lookup_CtlContractCommodityStressTestLookup contract = (Lookup_CtlContractCommodityStressTestLookup)e.Row.FindControl("CtlContractCommodityLookup");
        TextBox min = (TextBox)e.Row.FindControl("uiTxtLow");
        TextBox max = (TextBox)e.Row.FindControl("uiTxtHigh");

        if (max != null)
        {
            contract.uiTxtLookupHighID = max.ClientID;

        }
        if (min != null)
        {
            contract.uiTxtLookupLowID = min.ClientID;
        }

    }
}
