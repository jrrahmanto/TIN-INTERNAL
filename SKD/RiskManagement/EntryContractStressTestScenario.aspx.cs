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

public partial class WebUI_FinanceAndAccounting_EntryManagePostingCode : System.Web.UI.Page
{

    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
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

    private ScenarioContractData.ScenarioContractDataTable dtScenarioContract
    {
        get { return (ScenarioContractData.ScenarioContractDataTable)ViewState["dtScenarioContract"]; }
        set { ViewState["dtScenarioContract"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
            if (!Page.IsPostBack)
            {
                dtScenarioContract = new ScenarioContractData.ScenarioContractDataTable();

                if (eType == "add")
                {
                    //uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    bindData();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                StressTestScenarioData.StressTestScenarioDataTable ds = new StressTestScenarioData.StressTestScenarioDataTable();
                ds = (StressTestScenarioData.StressTestScenarioDataTable)Session["dtStressTestScenario"];
                decimal strScenarioID = ds[0].ScenarioID; 

                if (eID != 0)
                {
                    ScenarioContract.UpdateScenarioContract(Convert.ToDecimal(CtlContractCommodityLookup.LookupTextBoxID), strScenarioID,
                                                    uiDdlScenarioType.SelectedValue, Convert.ToDecimal(uiTxtBestPrice.Text),
                                               Convert.ToDecimal(uiTxtLow.Text), Convert.ToDecimal(uiTxtHigh.Text), User.Identity.Name);
                }
                else
                {

                    AddRow();
                    //ScenarioContract.AddScenarioContract(Convert.ToDecimal(CtlContractCommodityLookup.LookupTextBoxID), strScenarioID,
                    //                                uiDdlScenarioType.SelectedValue, Convert.ToDecimal(uiTxtBestPrice.Text),
                    //                           Convert.ToDecimal(uiTxtLow.Text), Convert.ToDecimal(uiTxtHigh.Text), User.Identity.Name);
                }
                Response.Redirect("EntryStressTestScenario.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }  
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryStressTestScenario.aspx");
    }


    #region SupportingMethod

    private void AddRow()
    {
        try
        {
            StressTestScenarioData.StressTestScenarioDataTable ds = new StressTestScenarioData.StressTestScenarioDataTable();
                    ds = (StressTestScenarioData.StressTestScenarioDataTable)Session["dtStressTestScenario"];
                    decimal strScenarioID = ds[0].ScenarioID; 

            //save to datatable
            //get Seconario Id in table and then + 1
             RowType = "add";
            //Reserve data
            //Temporary clear constraint
            dtScenarioContract.Constraints.Clear();
            ScenarioContractData.ScenarioContractRow dr = dtScenarioContract.NewScenarioContractRow();
            dtScenarioContract.ContractIDColumn.AllowDBNull = true;
            dtScenarioContract.ScenarioIDColumn.AllowDBNull = true;
            dtScenarioContract.ScenarioTypeColumn.AllowDBNull = true;
            dtScenarioContract.BasePriceColumn.AllowDBNull = true;
            dtScenarioContract.LowColumn.AllowDBNull = true;
            dtScenarioContract.HighColumn.AllowDBNull = true;

            dr.ContractID = Convert.ToDecimal(CtlContractCommodityLookup.LookupTextBoxID);
            dr.ScenarioID = strScenarioID;
            dr.ScenarioType = uiDdlScenarioType.SelectedValue;
            dr.BasePrice = Convert.ToDecimal(uiTxtBestPrice.Text);
            dr.Low = Convert.ToDecimal(uiTxtLow.Text);
            dr.High = Convert.ToDecimal(uiTxtHigh.Text);
            dtScenarioContract.AddScenarioContractRow(dr);
            dtScenarioContract.AcceptChanges();

            //insert ke session
            Session["dtScenarioContract"] = dtScenarioContract;
            //BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (string.IsNullOrEmpty(CtlContractCommodityLookup.LookupTextBox))
            {
                uiBLError.Items.Add("Contract commodity is required.");
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
        //ScenarioContractData.ScenarioContractRow dr = ScenarioContract.FillByContractID(Convert.ToDecimal(eID));

        //CtlContractCommodityLookup.LookupTextBox = dr.ContractID.ToString();
        //uiDdlScenarioType.SelectedValue = dr.ScenarioType;
        //uiTxtBestPrice.Text = dr.BasePrice.ToString();
        //uiTxtLow.Text = dr.Low.ToString();
        //uiTxtHigh.Text = dr.High.ToString();

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
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    #endregion
}
