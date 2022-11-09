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
using System.IO;
using System.Net;
using System.Drawing;
//using Renci.SshNet;

public partial class WebUI_MemberManagement_EntryClearingMember : System.Web.UI.Page
{
    
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    private string currentProfileID
    {
        get
        {
            return Request.QueryString["profileID"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPageClearingMemberSection();

        if (!Page.IsPostBack)
        {
            if (currentProfileID != null)
            {
                bindData();
                
                ExchangeData.ExchangeDdlDataTable dt = new ExchangeData.ExchangeDdlDataTable();
                dt = Exchange.SelectExchange();
                uiDdlProductExchange.DataSource = dt;
                uiDdlProductExchange.DataBind();
                uiDdlProductExchange.DataTextField = "ExchangeCode";
                uiDdlProductExchange.DataValueField = "ExchangeId";
                uiDdlProductExchange.Items.Insert(0, new ListItem("", String.Empty));

                uiDdlCMExchange.DataSource = dt;
                uiDdlCMExchange.DataBind();
                uiDdlCMExchange.DataTextField = "ExchangeCode";
                uiDdlCMExchange.DataValueField = "ExchangeId";
                
                uiDdlEMexch.DataSource = dt;
                uiDdlEMexch.DataBind();
                uiDdlEMexch.DataTextField = "ExchangeCode";
                uiDdlEMexch.DataValueField = "ExchangeId";

                uiDdlEMexchSearch.DataSource = dt;
                uiDdlEMexchSearch.DataBind();
                uiDdlEMexchSearch.DataTextField = "ExchangeCode";
                uiDdlEMexchSearch.DataValueField = "ExchangeId";

                

                uiTabsClearingMember.ActiveTabIndex = 0;

                //uiDtpStartDate.Enabled = false;
                ShowMgtPanel(false);
                ShowCMExchangePanel(false);
                ShowProductPanel(false);
                ShowEMPanel(false);
                uiBlCMError.Visible = false;
                uiBlBODError.Visible = false;
                uiBlProductError.Visible = false;
                uiBlCMExchError.Visible = false;
                uiBlEMError.Visible = false;
            }
            else
            {
               

                uiPnlManagementStructure.Visible = false;
                uiPanelCluster.Visible = false;
                uiPanelExchangeMember.Visible = false;
                uiPanelExchange.Visible = false;

            }

            RegionData.RegionDataTable dtDdlRegion = new RegionData.RegionDataTable();
            dtDdlRegion = Region.SelectDdlRegion();
            uiDdlRegion.DataSource = dtDdlRegion;
            uiDdlRegion.DataBind();
            uiDdlRegion.DataTextField = "Code";
            uiDdlRegion.DataValueField = "RegionID";
            uiDdlRegion.Items.Insert(0, new ListItem("", String.Empty));

        }
    }

    #region "   ClearingMember   "

    protected void uiDownloadLogo_Click(object sender, EventArgs e)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        
        if (uiUploadFileLogo.HasFile)
        {
            if (uiUploadFileLogo.FileBytes != null)
            {
                ShowFile(uiUploadFileLogo.PostedFile.FileName);
            }
        }
        else
        {
            if (currentID != null)
            {
                dt = ClearingMember.GetClearingMemberByCMID(currentID);
                if (dt.Count > 0)
                {
                    if (!dt[0].IsCompImageIDNull())
                    {
                        ShowFile(dt[0].Image);
                    }
                }
            }
        }
    }

    protected void uiBtnAktaPendiri_Click(object sender, EventArgs e)
    {
        if (uiLblAktaPendiri.Text != "" && uiLblAktaPendiri.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblAktaPendiri.Text);
        }
    }

    protected void uiBtnAktaPendiri2_Click(object sender, EventArgs e)
    {
        if (uiLblAktaPendiri.Text != "" && uiLblAktaPendiri.Text != null)
        {
            ClearingMember.UpdateDocument(
                "", uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblAktaPendiri.Text = "";
        }
    }

    protected void uiBtnPerubahan_Click(object sender, EventArgs e)
    {
        if (uiLblPerubahan.Text != "" && uiLblPerubahan.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblPerubahan.Text);
        }
    }

    protected void uiBtnPerubahan2_Click(object sender, EventArgs e)
    {
        if (uiLblPerubahan.Text != "" && uiLblPerubahan.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, "", uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblPerubahan.Text = "";
        }
    }

    protected void uiBtnDomisili_Click(object sender, EventArgs e)
    {
        if (uiLblDomisili.Text != "" && uiLblDomisili.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblDomisili.Text);
        }
    }

    protected void uiBtnDomisili2_Click(object sender, EventArgs e)
    {
        if (uiLblDomisili.Text != "" && uiLblDomisili.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, "", uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblDomisili.Text = "";
        }
    }

    protected void uiBtnNpwp_Click(object sender, EventArgs e)
    {
        if (uiLblNpwp.Text != "" && uiLblNpwp.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblNpwp.Text);
        }
    }

    protected void uiBtnNpwp2_Click(object sender, EventArgs e)
    {
        if (uiLblNpwp.Text != "" && uiLblNpwp.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, "",
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblNpwp.Text = "";
        }
    }

    protected void uiBtnKepabeaan_Click(object sender, EventArgs e)
    {
        if (uiLblKepabeaan.Text != "" && uiLblKepabeaan.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblKepabeaan.Text);
        }
    }

    protected void uiBtnKepabeaan2_Click(object sender, EventArgs e)
    {
        if (uiLblKepabeaan.Text != "" && uiLblKepabeaan.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                "", uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblKepabeaan.Text = "";
        }
    }

    protected void uiBtnTerdaftar_Click(object sender, EventArgs e)
    {
        if (uiLblTerdaftar.Text != "" && uiLblTerdaftar.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblTerdaftar.Text);
        }
    }

    protected void uiBtnTerdaftar2_Click(object sender, EventArgs e)
    {
        if (uiLblTerdaftar.Text != "" && uiLblTerdaftar.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, "", uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblTerdaftar.Text = "";
        }
    }

    protected void uiBtnEksportir_Click(object sender, EventArgs e)
    {
        if (uiLblEksportir.Text != "" && uiLblEksportir.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblEksportir.Text);
        }
    }

    protected void uiBtnEksportir2_Click(object sender, EventArgs e)
    {
        if (uiLblEksportir.Text != "" && uiLblEksportir.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, "", uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblEksportir.Text = "";
        }
    }

    protected void uiBtnSiup_Click(object sender, EventArgs e)
    {
        if (uiLblSiup.Text != "" && uiLblSiup.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblSiup.Text);
        }
    }

    protected void uiBtnSiup2_Click(object sender, EventArgs e)
    {
        if (uiLblSiup.Text != "" && uiLblSiup.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, "",
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblSiup.Text = "";
        }
    }

    protected void uiBtnNib_Click(object sender, EventArgs e)
    {
        if (uiLblNib.Text != "" && uiLblNib.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblNib.Text);
        }
    }

    protected void uiBtnNib2_Click(object sender, EventArgs e)
    {
        if (uiLblNib.Text != "" && uiLblNib.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                "", uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblNib.Text = "";
        }
    }

    protected void uiBtnIdentitas_Click(object sender, EventArgs e)
    {
        if (uiLblIdentitas.Text != "" && uiLblIdentitas.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblIdentitas.Text);
        }
    }

    protected void uiBtnIdentitas2_Click(object sender, EventArgs e)
    {
        if (uiLblIdentitas.Text != "" && uiLblIdentitas.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, "", uiLblKeuangan.Text, uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblIdentitas.Text = "";
        }
    }

    protected void uiBtnKeuangan_Click(object sender, EventArgs e)
    {
        if (uiLblKeuangan.Text != "" && uiLblKeuangan.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblKeuangan.Text);
        }
    }

    protected void uiBtnKeuangan2_Click(object sender, EventArgs e)
    {
        if (uiLblKeuangan.Text != "" && uiLblKeuangan.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, "", uiLblBankLuar.Text,
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblKeuangan.Text = "";
        }
    }

    protected void uiBtnBankLuar_Click(object sender, EventArgs e)
    {
        if (uiLblBankLuar.Text != "" && uiLblBankLuar.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblBankLuar.Text);
        }
    }

    protected void uiBtnBankLuar2_Click(object sender, EventArgs e)
    {
        if (uiLblBankLuar.Text != "" && uiLblBankLuar.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, "",
                uiLblCProfile.Text, int.Parse(uiHiddenCMID.Value));

            uiLblBankLuar.Text = "";
        }
    }

    protected void uiBtnCProfile_Click(object sender, EventArgs e)
    {
        if (uiLblCProfile.Text != "" && uiLblCProfile.Text != null)
        {
            ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT].ToString() + uiLblCProfile.Text);
        }
    }

    protected void uiBtnCProfile2_Click(object sender, EventArgs e)
    {
        if (uiLblCProfile.Text != "" && uiLblCProfile.Text != null)
        {
            ClearingMember.UpdateDocument(
                uiLblAktaPendiri.Text, uiLblPerubahan.Text, uiLblDomisili.Text, uiLblNpwp.Text,
                uiLblKepabeaan.Text, uiLblTerdaftar.Text, uiLblEksportir.Text, uiLblSiup.Text,
                uiLblNib.Text, uiLblIdentitas.Text, uiLblKeuangan.Text, uiLblBankLuar.Text,
                "", int.Parse(uiHiddenCMID.Value));

            uiLblCProfile.Text = "";
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMError();
            if (uitxbApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
            ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
            ta.FillByCMID(dt, Convert.ToDecimal(currentID));

            if(dt[0].CMType == "B")
            {
                if (uiLblAktaPendiri.Text == "" || uiLblAktaPendiri.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'No Akta Pendirian' upload not found.");

                if (uiLblPerubahan.Text == "" || uiLblAktaPendiri.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'No Akta Perubahan' upload not found.");

                if (uiLblDomisili.Text == "" || uiLblDomisili.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Domisili Perusahaan' upload not found.");

                if (uiLblNpwp.Text == "" || uiLblNpwp.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'NPWP' upload not found.");

                if (uiLblEksportir.Text == "" || uiLblEksportir.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Perizinan Instansi Eksportir' upload not found.");
            }
            else
            {
                if (uiLblAktaPendiri.Text == "" || uiLblAktaPendiri.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'No Akta Pendirian' upload not found.");

                if (uiLblPerubahan.Text == "" || uiLblAktaPendiri.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'No Akta Perubahan' upload not found.");

                if (uiLblDomisili.Text == "" || uiLblDomisili.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Domisili Perusahaan' upload not found.");

                if (uiLblNpwp.Text == "" || uiLblNpwp.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'NPWP' upload not found.");

                if (uiLblKepabeaan.Text == "" || uiLblKepabeaan.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Identitas Kepabeaan' upload not found.");

                if (uiLblTerdaftar.Text == "" || uiLblTerdaftar.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Eksportir Terdaftar Timah' upload not found.");
            }

            if (uiTxtDomisili.Text == "")
            {
                if (uiLblNib.Text == "" || uiLblNib.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'NIB' upload not found.");

                if (uiLblIdentitas.Text == "" || uiLblIdentitas.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Identitas Kepabeaan' upload not found.");

                if (uiLblKeuangan.Text == "" || uiLblKeuangan.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Laporan Keuangan' upload not found.");
            }
            else if(uiTxtDomisili.Text != "")
            {
                //Luar Negeri
                if (uiLblKeuangan.Text == "" || uiLblKeuangan.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Laporan Keuangan' upload not found.");

                if (uiLblBankLuar.Text == "" || uiLblBankLuar.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Surat Referensi Bank Luar Negeri' upload not found.");

                if (uiLblCProfile.Text == "" || uiLblCProfile.Text == null)
                    throw new ApplicationException("Cannot Approve Application Registration, document 'Company Profile' upload not found.");
            }

            ClearingMember.Approve(Convert.ToDecimal(currentID), User.Identity.Name,
                                   uitxbApprovalDesc.Text, decimal.Parse(currentProfileID),uiTxtAddress.Text,
                                   uiTxtEmail.Text, uiTxtPhoneNumber.Text 
                                   ,uiTxtContactPerson.Text ,uiTxtContactPhone.Text,Convert.ToDateTime(uiDtpRegDate.Text));

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Approve update of clearing member", User.Identity.Name, Common.GetIPAddress(this.Request));

            Response.Redirect("~/PlanningAndDevelopment/MemberManagement/ViewClearingMember.aspx");
        }
        catch (Exception ex)
        {
            DisplayCMErrorMessage(ex);
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMError();
            if (uitxbApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            ClearingMember.Reject(Convert.ToDecimal(currentProfileID), User.Identity.Name, uitxbApprovalDesc.Text);

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Reject update of clearing member", User.Identity.Name, Common.GetIPAddress(this.Request));
            Response.Redirect("~/PlanningAndDevelopment/MemberManagement/ViewClearingMember.aspx");
        }
        catch (Exception ex)
        {
            DisplayCMErrorMessage(ex);
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PlanningAndDevelopment/MemberManagement/ViewClearingMember.aspx");
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMError();
            SetAccessPageClearingMemberSection();
            Response.Redirect("EntryExchangeMember.aspx");
        }
        catch (Exception ex)
        {
            DisplayCMErrorMessage(ex);
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        ClearCMError();
        if (!isValidEntryCM())
            return;

        try
        {
            Nullable<DateTime> AgreementDate = null;
            Nullable<DateTime> CertificateDate = null;
            Nullable<DateTime> SPATraderDate = null;
            Nullable<DateTime> SPABrokerDate = null;
            Nullable<DateTime> AnniversaryDate = null;
            Nullable<DateTime> EndDate = null;
            Nullable<DateTime> RegDate = null; 

            if (uiDTPAgreementDate.Text != "")
                AgreementDate = Convert.ToDateTime(uiDTPAgreementDate.Text);
            if (uiDtpCertificateDate.Text != "")
                CertificateDate = Convert.ToDateTime(uiDtpCertificateDate.Text);
            if (uiDtpSPATraderDate.Text != "")
                SPATraderDate = Convert.ToDateTime(uiDtpSPATraderDate.Text);
            if (uiDtpSPABrokerDate.Text != "")
                SPABrokerDate = Convert.ToDateTime(uiDtpSPABrokerDate.Text);
            if (uiDtpAnniversary.Text != "")
                AnniversaryDate = Convert.ToDateTime(uiDtpAnniversary.Text);
            if (uiDtpEndDate.Text != "")
                EndDate = Convert.ToDateTime(uiDtpEndDate.Text);
            if (uiDtpRegDate.Text != "")
                RegDate = Convert.ToDateTime(uiDtpRegDate.Text);

            if (currentProfileID != null)
            {
                bool isLogoUpdated = false;
                if (uiUploadFileLogo.FileName != "")
                {
                    isLogoUpdated = true;
                }
                var x = Convert.ToDecimal(currentProfileID);
                ClearingMember.ProposeInsertUpdate(uiTxbCMCode.Text, uiTxbCMName.Text,
                                            Convert.ToDateTime(uiDtpStartDate.Text).Date,
                                            uiTxbPPKP.Text, uiTxbWebsite.Text, uiDdlStatus.SelectedValue,
                                            uiTxbAgreementNo.Text, AgreementDate,
                                            uiDdlExchangeStatus.SelectedValue, uiTxbCertificateNO.Text,
                                            CertificateDate, uiTxbSpaTraderNo.Text,
                                            SPATraderDate, uiTxbSpaBrokerNo.Text,
                                            SPABrokerDate, uiTXbPALNLicense.Text,
                                            AnniversaryDate,
                                            uiUploadFileLogo.FileBytes,"A",
                                            User.Identity.Name, "U", Convert.ToDecimal(currentProfileID),
                                            "Proposed Update", isLogoUpdated, EndDate, 
                                            decimal.Parse(uiTxbCMInitialMarginMultiplier.Text),
                                            decimal.Parse(uiTxbMinReqInitialMarginIDR.Text),
                                            decimal.Parse(uiTxbMinReqInitialMarginUSD.Text), 
                                            decimal.Parse(currentID),uiTxtAddress.Text , uiTxtEmail.Text ,
                                            0, uiTxtPhoneNumber.Text , uiTxtContactPerson.Text ,
                                            uiTxtContactPhone.Text , RegDate,uiTxtCMAccNo.Text,uiTxtCMBankName.Text, uiTxtCMAccNm.Text,
                                            uiDdlRegion.SelectedItem.ToString());
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose update of clearing member", User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            else
            {
                ClearingMember.ProposeInsertUpdate(uiTxbCMCode.Text, uiTxbCMName.Text,
                                             Convert.ToDateTime(uiDtpStartDate.Text).Date,
                                             uiTxbPPKP.Text, uiTxbWebsite.Text, uiDdlStatus.SelectedValue,
                                             uiTxbAgreementNo.Text, AgreementDate,
                                             uiDdlExchangeStatus.SelectedValue, uiTxbCertificateNO.Text,
                                             CertificateDate, uiTxbSpaTraderNo.Text,
                                             SPATraderDate, uiTxbSpaBrokerNo.Text,
                                             SPABrokerDate, uiTXbPALNLicense.Text,
                                             AnniversaryDate,
                                             uiUploadFileLogo.FileBytes, "A",
                                             User.Identity.Name, "I", 0, AuditTrail.PROPOSE, true, EndDate,
                                            decimal.Parse(uiTxbCMInitialMarginMultiplier.Text),
                                            decimal.Parse(uiTxbMinReqInitialMarginIDR.Text),
                                            decimal.Parse(uiTxbMinReqInitialMarginUSD.Text),0, 
                                            uiTxtAddress.Text, uiTxtEmail.Text,
                                            decimal.Parse(uiDdlRegion.SelectedValue),uiTxtPhoneNumber.Text, uiTxtContactPerson.Text,
                                            uiTxtContactPhone.Text, RegDate,uiTxtCMAccNo.Text,uiTxtCMBankName.Text ,uiTxtCMAccNm.Text,uiDdlRegion.SelectedItem.ToString ());
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose insert of clearing member", User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            Response.Redirect("~/PlanningAndDevelopment/MemberManagement/ViewClearingMember.aspx");
        }
        catch (Exception ex)
        {
            DisplayCMErrorMessage(ex);
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMError();   
            if (uitxbApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            ClearingMember.ProposeDelete(Convert.ToDecimal(currentProfileID), User.Identity.Name);
            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose delete of clearing member", User.Identity.Name, Common.GetIPAddress(this.Request));
            Response.Redirect("~/PlanningAndDevelopment/MemberManagement/ViewClearingMember.aspx");
        }
        catch (Exception ex)
        {
            DisplayCMErrorMessage(ex);
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }

    private void bindData()
    {
        ClearingMemberData.CMProfileDataTable dt = new ClearingMemberData.CMProfileDataTable();
        dt = ClearingMember.GetClearingMemberByCMProfileID(currentProfileID);

        Session["CMIDLookup"] = currentID;
        if (dt.Count > 0)
        {
            uiTxbCMCode.Text = dt[0].Code;
            if (!dt[0].IsNameNull())
                uiTxbCMName.Text = dt[0].Name;
            else
                uiTxbCMName.Text = "";
            uiDtpStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsEffectiveEndDateNull())
            {
                uiDtpEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
            }
            else
                uiDtpEndDate.SetCalendarValue("");


            if (!dt[0].IsRegistrationDateNull())
            {
                uiDtpRegDate.SetCalendarValue(dt[0].RegistrationDate.ToString("dd-MMM-yyyy"));
            }
            else
                uiDtpRegDate.SetCalendarValue("");


            if (!dt[0].IsPPKPNull())
                uiTxbPPKP.Text = dt[0].PPKP;
            else
                uiTxbPPKP.Text = "";
            if (!dt[0].IsWebsiteNull())
            {
                uiTxbWebsite.Text = dt[0].Website;
            }
            else
                uiTxbWebsite.Text = "";
            if (!dt[0].IsExchangeStatusNull())
            {
                uiDdlExchangeStatus.SelectedValue = dt[0].ExchangeStatus;
            }
            if (!dt[0].IsAgreementNoNull())
            {
                uiTxbAgreementNo.Text = dt[0].AgreementNo;
            }
            else
                uiTxbAgreementNo.Text = "";
            if (!dt[0].IsAgreementDateNull())
            {
                uiDTPAgreementDate.SetCalendarValue(dt[0].AgreementDate.ToString("dd-MMM-yyyy"));
            }
            else
                uiDTPAgreementDate.SetCalendarValue("");

            if (!dt[0].IsCertNoNull())
            {
                uiTxbCertificateNO.Text = dt[0].CertNo;
            }
            else
                uiTxbCertificateNO.Text = "";
            if (!dt[0].IsCertDateNull())
            {
                uiDtpCertificateDate.SetCalendarValue(dt[0].CertDate.ToString("dd-MMM-yyyy"));
            }
            else
                uiDtpCertificateDate.SetCalendarValue("");

            if (!dt[0].IsSPATraderNoNull())
            {
                uiTxbSpaTraderNo.Text = dt[0].SPATraderNo;
            }
            else
                uiTxbSpaTraderNo.Text = "";
            if (!dt[0].IsSPATraderDateNull())
            {
                uiDtpSPATraderDate.SetCalendarValue(dt[0].SPATraderDate.ToString("dd-MMM-yyyy"));
            }
            else
                uiDtpSPATraderDate.SetCalendarValue("");

            if (!dt[0].IsSPABrokerNoNull())
            {
                uiTxbSpaBrokerNo.Text = dt[0].SPABrokerNo;
            }
            else
                uiTxbSpaBrokerNo.Text = "";
            if (!dt[0].IsSPABrokerDateNull())
            {
                uiDtpSPABrokerDate.SetCalendarValue(dt[0].SPABrokerDate.ToString("dd-MMM-yyyy"));
            }
            else

                uiDtpSPABrokerDate.SetCalendarValue("");

            if (!dt[0].IsPALNLicenseNull())
            {
                uiTXbPALNLicense.Text = dt[0].PALNLicense;
            }
            else
                uiTXbPALNLicense.Text = "";

            if (!dt[0].IsCompAnniversaryNull())
            {
                uiDtpAnniversary.SetCalendarValue(dt[0].CompAnniversary.ToString("dd-MMM-yyyy"));
            }
            else
                uiDtpAnniversary.SetCalendarValue("");

            uiDdlStatus.SelectedValue = dt[0].CMStatus.ToString(); 
            
            uiTxbCMInitialMarginMultiplier.Text = dt[0].InitialMarginMultiplier.ToString("#,##0.##");
            uiTxbMinReqInitialMarginIDR.Text = dt[0].MinReqInitialMarginIDR.ToString("#,##0.##");
            uiTxbMinReqInitialMarginUSD.Text = dt[0].MinReqInitialMarginUSD.ToString("#,##0.##");
            if (dt[0].IsAddressNull())
            {
                uiTxtAddress.Text = string.Empty;
            }
            else
            { 
                uiTxtAddress.Text = dt[0].Address;
            }

            if (dt[0].IsEmailNull())
            {
                uiTxtEmail.Text = string.Empty;
            }
            else
            {
                uiTxtEmail.Text = dt[0].Email;
            }

            if (dt[0].IsPhoneNumberNull())
            {
                uiTxtPhoneNumber.Text = string.Empty;
            }
            else
            {
                uiTxtPhoneNumber.Text = dt[0].PhoneNumber;
            }

            if (dt[0].IsContactPersonNameNull())
            {
                uiTxtContactPerson.Text = string.Empty;
            }
            else
            {
                uiTxtContactPerson.Text = dt[0].ContactPersonName;
            }

            if (dt[0].IsContactPersonPhoneNull())
            {
                uiTxtContactPhone.Text = string.Empty;
            }
            else
            {
                uiTxtContactPhone.Text = dt[0].ContactPersonPhone;
            }
            if (dt[0].IsRegionIDNull())
            {
                uiDdlRegion.SelectedValue = string.Empty; 
            }
            else
            {
                uiDdlRegion.SelectedValue = dt[0].RegionID.ToString();
            }

            if (dt[0].IsNoAktaPendiriNull())
            {
                uiLblAktaPendiri.Text = string.Empty;
                uiBtnAktaPendiri.Visible = false;
                uiBtnAktaPendiri2.Visible = false;
            }
            else
            {
                if (dt[0].NoAktaPendiri.ToString() == "")
                {
                    uiBtnAktaPendiri.Visible = false;
                    uiBtnAktaPendiri2.Visible = false;
                }else {
                    uiLblAktaPendiri.Text = dt[0].NoAktaPendiri.ToString();
                    uiLblAktaPendiri.Visible = true;
                }
            }

            if (dt[0].IsNoAktaPerubahanNull())
            {
                uiLblPerubahan.Text = string.Empty;
                uiBtnPerubahan.Visible = false;
                uiBtnPerubahan2.Visible = false;
            }
            else
            {
                if (dt[0].NoAktaPerubahan.ToString() == "")
                {
                    uiBtnPerubahan.Visible = false;
                    uiBtnPerubahan2.Visible = false;
                }
                else
                {
                    uiLblPerubahan.Text = dt[0].NoAktaPerubahan.ToString();
                    uiLblPerubahan.Visible = true;
                }
            }

            if (dt[0].IsDomisiliPerusahaanNull())
            {
                uiLblDomisili.Text = string.Empty;
                uiBtnDomisili.Visible = false;
                uiBtnDomisili2.Visible = false;
            }
            else
            {
                if (dt[0].DomisiliPerusahaan.ToString() == "")
                {
                    uiBtnDomisili.Visible = false;
                    uiBtnDomisili2.Visible = false;
                }
                else
                {
                    uiLblDomisili.Text = dt[0].DomisiliPerusahaan.ToString();
                    uiLblDomisili.Visible = true;
                }
            }

            if (dt[0].IsNPWPNull())
            {
                uiLblNpwp.Text = string.Empty;
                uiBtnNpwp.Visible = false;
                uiBtnNpwp2.Visible = false;
            }
            else
            {
                if (dt[0].NPWP.ToString() == "")
                {
                    uiBtnNpwp.Visible = false;
                    uiBtnNpwp2.Visible = false;
                }
                else
                {
                    uiLblNpwp.Text = dt[0].NPWP.ToString();
                    uiLblNpwp.Visible = true;
                }
            }

            if (dt[0].IsIdentitasKepabeanNull())
            {
                uiLblKepabeaan.Text = string.Empty;
                uiBtnKepabeaan.Visible = false;
                uiBtnKepabeaan2.Visible = false;
            }
            else
            {
                if (dt[0].IdentitasKepabean.ToString() == "")
                {
                    uiBtnKepabeaan.Visible = false;
                    uiBtnKepabeaan2.Visible = false;
                }
                else
                {
                    uiLblKepabeaan.Text = dt[0].IdentitasKepabean.ToString();
                    uiLblKepabeaan.Visible = true;
                }
            }

            if (dt[0].IsEksportirTerdaftarTimahNull())
            {
                uiLblTerdaftar.Text = string.Empty;
                uiBtnTerdaftar.Visible = false;
                uiBtnTerdaftar2.Visible = false;
            }
            else
            {
                if (dt[0].EksportirTerdaftarTimah.ToString() == "")
                {
                    uiBtnTerdaftar.Visible = false;
                    uiBtnTerdaftar2.Visible = false;
                }
                else
                {
                    uiLblTerdaftar.Text = dt[0].EksportirTerdaftarTimah.ToString();
                    uiLblTerdaftar.Visible = true;
                }
            }

            if (dt[0].IsPerizinanInstansiEksportirNull())
            {
                uiLblEksportir.Text = string.Empty;
                uiBtnEksportir.Visible = false;
                uiBtnEksportir2.Visible = false;
            }
            else
            {
                if (dt[0].PerizinanInstansiEksportir.ToString() == "")
                {
                    uiBtnEksportir.Visible = false;
                    uiBtnEksportir2.Visible = false;
                }
                else
                {
                    uiLblEksportir.Text = dt[0].PerizinanInstansiEksportir.ToString();
                    uiLblEksportir.Visible = true;
                }
            }

            if (dt[0].IssiupNull())
            {
                uiLblSiup.Text = string.Empty;
                uiBtnSiup.Visible = false;
                uiBtnSiup2.Visible = false;
            }
            else
            {
                if (dt[0].siup.ToString() == "")
                {
                    uiBtnSiup.Visible = false;
                    uiBtnSiup2.Visible = false;
                }
                else
                {
                    uiLblSiup.Text = dt[0].siup.ToString();
                    uiLblSiup.Visible = true;
                }
            }

            if (dt[0].IsnibNull())
            {
                uiLblNib.Text = string.Empty;
                uiBtnNib.Visible = false;
                uiBtnNib2.Visible = false;
            }
            else
            {
                if (dt[0].nib.ToString() == "")
                {
                    uiBtnNib.Visible = false;
                    uiBtnNib2.Visible = false;
                }
                else
                {
                    uiLblNib.Text = dt[0].nib.ToString();
                    uiLblNib.Visible = true;
                }
            }

            if (dt[0].IsidentitasDiriPengurusNull())
            {
                uiLblIdentitas.Text = string.Empty;
                uiBtnIdentitas.Visible = false;
                uiBtnIdentitas2.Visible = false;
            }
            else
            {
                if (dt[0].identitasDiriPengurus.ToString() == "")
                {
                    uiBtnIdentitas.Visible = false;
                    uiBtnIdentitas2.Visible = false;
                }
                else
                {
                    uiLblIdentitas.Text = dt[0].identitasDiriPengurus.ToString();
                    uiLblIdentitas.Visible = true;
                }
            }

            if (dt[0].IslaporanKeuanganNull())
            {
                uiLblKeuangan.Text = string.Empty;
                uiBtnKeuangan.Visible = false;
                uiBtnKeuangan2.Visible = false;
            }
            else
            {
                if (dt[0].laporanKeuangan.ToString() == "")
                {
                    uiBtnKeuangan.Visible = false;
                    uiBtnKeuangan2.Visible = false;
                }
                else
                {
                    uiLblKeuangan.Text = dt[0].laporanKeuangan.ToString();
                    uiLblKeuangan.Visible = true;
                }
            }

            if (dt[0].IssuratRefBankNegeriNull())
            {
                uiLblBankLuar.Text = string.Empty;
                uiBtnBankLuar.Visible = false;
                uiBtnBankLuar2.Visible = false;
            }
            else
            {
                if (dt[0].suratRefBankNegeri.ToString() == "")
                {
                    uiBtnBankLuar.Visible = false;
                    uiBtnBankLuar2.Visible = false;
                }
                else
                {
                    uiLblBankLuar.Text = dt[0].suratRefBankNegeri.ToString();
                    uiLblBankLuar.Visible = true;
                }
            }

            if (dt[0].IscompanyProfileNull())
            {
                uiLblCProfile.Text = string.Empty;
                uiBtnCProfile.Visible = false;
                uiBtnCProfile2.Visible = false;
            }
            else
            {
                if(dt[0].companyProfile.ToString() == "")
                {
                    uiBtnCProfile.Visible = false;
                    uiBtnCProfile2.Visible = false;
                }
                else
                {
                    uiLblCProfile.Text = dt[0].companyProfile.ToString();
                    uiLblCProfile.Visible = true;
                }
            }

            if (dt[0].IsdomisiliNull())
            {
                uiTxtDomisili.Text = string.Empty;
            }
            else
            {
                uiTxtDomisili.Text = dt[0].domisili.ToString();
            }

            uiTxtCMAccNm.Text = dt[0].CMAccountName;
            uiTxtCMAccNo.Text = dt[0].CMAccountNo;
            uiTxtCMBankName.Text = dt[0].CMBankName;

            uiDownloadLogo.Enabled = dt[0].IsCompImageIDNull();
            if (!dt[0].IsActionFlagNull())
            {
                if (dt[0].ActionFlag == "I")
                {
                    uiTXbAction.Text = "Insert";
                }
                else if (dt[0].ActionFlag == "U")
                {
                    uiTXbAction.Text = "Update";
                }
                else if (dt[0].ActionFlag == "D")
                {
                    uiTXbAction.Text = "Delete";
                }
                else if (dt[0].ActionFlag == "V")
                {
                    uiTXbAction.Text = "Update Version";
                }
            }
            else
                uiTXbAction.Text = "";
            if (!dt[0].IsApprovalDescNull())
                uitxbApprovalDesc.Text = dt[0].ApprovalDesc;
            else
                uitxbApprovalDesc.Text = "";


            //bind lbl cm code in exchange tab
            uiLblCMCodeExchange.Text = dt[0].Code;
            uiLblProductCMCode.Text = dt[0].Code + " - " + dt[0].Name;
            uiLblBODCMCode.Text = dt[0].Code;
            uiHiddenCMID.Value = dt[0].CMID.ToString();
            uiHiddenProductTraderCMID.Value = dt[0].CMID.ToString();
            uiHiddBODCMID.Value = dt[0].CMID.ToString();
            uiHiddenEMCMID.Value = dt[0].CMID.ToString();
        }

    }


    private void ShowFile(string filePath)
    {

        // Create New instance of FileInfo class to get the properties of the file being downloaded
        FileInfo file = new FileInfo(filePath);

        // Checking if file exists
        if (file.Exists)
        {
            // Clear the content of the response
            Response.ClearContent();

            // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            // Add the file size into the response header
            Response.AddHeader("Content-Length", file.Length.ToString());

            // Set the ContentType
            Response.ContentType = ReturnExtension(file.Extension.ToLower());

            // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
            Response.TransmitFile(file.FullName);

            // End the response
            Response.End();
        }
    }

    private void ShowFile(byte[] img)
    {


        // Clear the content of the response
        Response.ClearContent();

        // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
        Response.AddHeader("Content-Disposition", "attachment; filename=Logo.jpeg");

        // Add the file size into the response header
        Response.AddHeader("Content-Length", img.Length.ToString());

        // Set the ContentType
        Response.ContentType = "image/jpeg"; //ReturnExtension(file.Extension.ToLower());

        // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
        //Response.TransmitFile("Logo.jpeg");

        Response.BinaryWrite(img);

        // End the response
        Response.End();



    }

    private void SetAccessPageClearingMemberSection()
    {
        MasterPage mp = (MasterPage)this.Master;
        bool Maker, Checker, Viewer;

        Maker = mp.IsMaker;
        Checker = mp.IsChecker;
        Viewer = mp.IsViewer;

        // Always check for each roles to enabled control users that has multiple roles

        if (Viewer)
        {
            // Hide / Show control on clearing member section
            trAction.Visible = true;
            trApprovalDesc.Visible = true;
            uiBtnSave.Visible = false;
            uiBtnDelete.Visible = false;
            uiBtnApprove.Visible = false;
            uiBtnReject.Visible = false;

            // Enable / Disable control on clearing member section
            EnableClearingMemberCtl(false);
            uitxbApprovalDesc.Enabled = false;

            // Hide create new record of all data related to clearing member for viewer
            uiBtnCreateBOD.Visible = false;
            uiBtnCreateProduct.Visible = false;
            uiBtnCreateCMExchange.Visible = false;
            uiBtnCreateEM.Visible = false;
        }

        if (Checker)
        {
            // Hide / Show control on clearing member section
            trAction.Visible = true;
            trApprovalDesc.Visible = true;
            uiBtnSave.Visible = false;
            uiBtnDelete.Visible = false;
            uiBtnApprove.Visible = true;
            uiBtnReject.Visible = true;

            // Enable / Disable control on clearing member section
            EnableClearingMemberCtl(false);
            uitxbApprovalDesc.Enabled = true;

            // Hide create new record of all data related to clearing member for viewer
            uiBtnCreateBOD.Visible = false;
            uiBtnCreateProduct.Visible = false;
            uiBtnCreateCMExchange.Visible = false;
            uiBtnCreateEM.Visible = false;
        }

        if (Maker)
        {
            // Hide / Show control on clearing member section
            trAction.Visible = false;
            trApprovalDesc.Visible = false;
            uiBtnSave.Visible = true;
            uiBtnDelete.Visible = true;
            uiBtnApprove.Visible = false;
            uiBtnReject.Visible = false;

            // Enable / Disable control on clearing member section
            EnableClearingMemberCtl(true);
            uitxbApprovalDesc.Enabled = true;

            // Show create new record of all data related to clearing member for viewer
            uiBtnCreateBOD.Visible = true;
            uiBtnCreateProduct.Visible = true;
            uiBtnCreateCMExchange.Visible = true;
            uiBtnCreateEM.Visible = true;
        }

    }

    private void EnableClearingMemberCtl(bool isActive)
    {
        uiTxbCMCode.Enabled = isActive;
        uiTxbCMName.Enabled = isActive;
        uiTxbPPKP.Enabled = isActive;
        uiTxbWebsite.Enabled = isActive;
        uiTxbAgreementNo.Enabled = isActive;
        uiTxbCertificateNO.Enabled = isActive;
        uiTxbSpaTraderNo.Enabled = isActive;
        uiTxbSpaBrokerNo.Enabled = isActive;
        uiTXbPALNLicense.Enabled = isActive;
        uiTxbMinReqInitialMarginIDR.Enabled = isActive;
        uiTxbMinReqInitialMarginUSD.Enabled = isActive;
        uiTxbCMInitialMarginMultiplier.Enabled = isActive;
        uiTxbExchangeLicenseNo.Enabled = isActive;

        //uiDdlExchangeType.Enabled = isActive;
        uiDdlExchangeStatus.Enabled = isActive;
        
        uiDtpAnniversary.SetDisabledCalendar(!isActive);
        uiDtpCertificateDate.SetDisabledCalendar(!isActive);
        uiDtpStartDate.SetDisabledCalendar(!isActive);
        uiDtpEndDate.SetDisabledCalendar(!isActive);
        uiDTPAgreementDate.SetDisabledCalendar(!isActive);
        uiDtpSPABrokerDate.SetDisabledCalendar(!isActive);
        uiDtpSPATraderDate.SetDisabledCalendar(!isActive);

        // Status clearing member can only be set using Clearing Member Discipline module
        uiDdlStatus.Enabled = false;

        // Always set enabled to false
        uiTXbAction.Enabled = false;

        uiTxtAddress.Enabled = isActive;
        uiTxtEmail.Enabled = isActive;
        uiTxtPhoneNumber.Enabled = isActive;
        uiTxtContactPerson.Enabled = isActive;
        uiTxtContactPhone.Enabled = isActive;
        uiTxtCMAccNo.Enabled = isActive;
        uiTxtCMAccNm.Enabled = isActive;
        uiTxtCMBankName.Enabled = isActive;
        uiTxtDomisili.Enabled = isActive;
    }

    private void DisplayCMErrorMessage(Exception ec)
    {
        uiBlCMError.Items.Clear();
        uiBlCMError.Items.Add(ec.Message);
        uiBlCMError.Visible = true;
    }

    private void ClearCMError()
    {
        uiBlCMError.Items.Clear();
        uiBlCMError.Visible = false;
    }

    private bool isValidEntryCM()
    {
        bool isValid = true;

        uiBlCMError.Visible = false;
        uiBlCMError.Items.Clear();

        if (uiTxbCMCode.Text == "")
            uiBlCMError.Items.Add("Code is required.");

        if (uiTxbCMName.Text == "")
            uiBlCMError.Items.Add("Name is required.");

        if (uiDtpStartDate.Text == "")
            uiBlCMError.Items.Add("Effective start date is required.");

        if (uiTxbCMInitialMarginMultiplier.Text == "")
            uiBlCMError.Items.Add("Initial margin multiplier is required.");

        if (uiTxbMinReqInitialMarginIDR.Text == "")
            uiBlCMError.Items.Add("Minimum requirement for initial margin (IDR) is required.");

        if (uiTxbMinReqInitialMarginUSD.Text == "")
            uiBlCMError.Items.Add("Minimum requirement for initial margin (USD) is required.");

        if (uiBlCMError.Items.Count > 0)
        {
            uiBlCMError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

    #endregion

    #region "   ClearingMemberExchange   "

    private string SortOrderCMExchange
    {
        get
        {
            if (ViewState["SortOrderCMExchange"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrderCMExchange"].ToString();
            }
        }
        set { ViewState["SortOrderCMExchange"] = value; }
    }

    protected void Ecgridview8_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ClearCMExchangeError();
                //ClearCMExchCtl();
                ShowCMExchangePanel(true);
                SetAccessPageCMExchangeSection();

                int ii = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = uiDgCMExchMember.Rows[ii];
                Label id = (Label)selectedRow.FindControl("uiLblCMExchangeID");

                BindCMExchData(id.Text);
                uiHiddenCMExchangeStatus.Value = "Edit";
            }
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnCreateCMExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            ShowCMExchangePanel(true);
            ClearCMExchCtl();

            SetAccessPageCMExchangeSection();

            uiHiddenCMExchangeStatus.Value = "Create";
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnSaveExchange_Click(object sender, EventArgs e)
    {
        // Check for valid entry
        if (!isValidEntryCMExchange())
            return;

        try
        {
            ClearCMExchangeError();
            Nullable<DateTime> endDate = null;
            Nullable<DateTime> licenseDate = null;

            if (uiDtpExchangeEndDate.Text != "")
            {
                endDate = Convert.ToDateTime(uiDtpExchangeEndDate.Text);
            }
            if (uiDtpExchangeLicenseDate.Text != "")
            {
                licenseDate = Convert.ToDateTime(uiDtpExchangeLicenseDate.Text);
            }

            if (uiHiddenCMExchangeStatus.Value == "Create")
            {
                ClearingMemberExchange.ProposedCMExchange(Convert.ToDecimal(uiHiddenCMID.Value),
                                                      Convert.ToDecimal(uiDdlCMExchange.SelectedValue),
                                                      Convert.ToDateTime(uiDtpExchangeStartDate.Text), endDate,
                                                      uiTxbExchangeMemberCode.Text, uiDdlMembershipType.SelectedValue,
                                                      User.Identity.Name, "I", null, uiTxbExchangeLicenseNo.Text,
                                                      licenseDate);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose create of clearing member exchange", User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            else if (uiHiddenCMExchangeStatus.Value == "Edit")
            {
                ClearingMemberExchange.ProposedCMExchange(Convert.ToDecimal(uiHiddenCMID.Value),
                                                      Convert.ToDecimal(uiDdlCMExchange.SelectedValue),
                                                      Convert.ToDateTime(uiDtpExchangeStartDate.Text), endDate,
                                                      uiTxbExchangeMemberCode.Text, uiDdlMembershipType.SelectedValue,
                                                      User.Identity.Name, "U", 
                                                      Convert.ToDecimal(uiHiddenCMExchangeID.Value),
                                                      uiTxbExchangeLicenseNo.Text,
                                                      licenseDate);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose update of clearing member exchange", User.Identity.Name, Common.GetIPAddress(this.Request));
            }

            // Set display mode
            ShowCMExchangePanel(false);

            // Refresh grid to reflect changes
            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnDeleteExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            Nullable<DateTime> endDate = null;
            Nullable<DateTime> licenseDate = null;

            if (uiHiddenCMExchangeStatus.Value == "Edit")
            {
                if (uiDtpEndDate.Text != "")
                {
                    endDate = Convert.ToDateTime(uiDtpEndDate.Text);
                }
               
                if (uiDtpExchangeLicenseDate.Text != "")
                {
                    licenseDate = Convert.ToDateTime(uiDtpExchangeLicenseDate.Text);
                }

                ClearingMemberExchange.ProposedCMExchange(Convert.ToDecimal(uiHiddenCMID.Value),
                                                      Convert.ToDecimal(uiDdlCMExchange.SelectedValue),
                                                      Convert.ToDateTime(uiDtpStartDate.Text), endDate,
                                                      uiTxbExchangeMemberCode.Text, uiDdlMembershipType.SelectedValue,
                                                      User.Identity.Name, "D",
                                                      Convert.ToDecimal(uiHiddenCMExchangeID.Value),
                                                      uiTxbExchangeLicenseNo.Text,
                                                      licenseDate);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose delete of clearing member exchange", User.Identity.Name, Common.GetIPAddress(this.Request));
                // Set display mode
                ShowCMExchangePanel(false);

                // Refresh grid to reflect changes
                FillCMExchangeDataGrid();
            }
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex); 
        }

    }

    protected void uiBtnApproveExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            if (uiHiddenCMExchangeStatus.Value == "Edit")
            {
                if (uiTxbCMExchbApprovalDesc.Text == "")
                    throw new ApplicationException("Approval description is required.");

                // Approve proposed record
                ClearingMemberExchange.Approve(Convert.ToDecimal(uiHiddenCMExchangeID.Value), User.Identity.Name, uiTxbCMExchbApprovalDesc.Text);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Approve update of clearing member exchange", User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            // Set display mode
            ShowCMExchangePanel(false);

            // Refresh grid to reflect changes
            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnRejectExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            if (uiHiddenCMExchangeStatus.Value == "Edit")
            {
                if (uiTxbCMExchbApprovalDesc.Text == "")
                    throw new ApplicationException("Approval description is required.");

                ClearingMemberExchange.Reject(Convert.ToDecimal(uiHiddenCMExchangeID.Value), User.Identity.Name, uiTxbCMExchbApprovalDesc.Text);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Reject update of clearing member exchange", User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            // Set display mode
            ShowCMExchangePanel(false);

            // Refresh grid to reflect changes
            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnCancelExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            //ClearCMExchCtl();
            ShowCMExchangePanel(false);
            //EnableCMExchCtl(false);
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiBtnSearchCMExchange_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiDgCMExchMember_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ClearCMExchangeError();

            if (string.IsNullOrEmpty(SortOrderCMExchange))
            {
                SortOrderCMExchange = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrderCMExchange.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrderCMExchange = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrderCMExchange = e.SortExpression + " " + "ASC";
                }
            }

            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    protected void uiDgCMExchMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ClearCMExchangeError();
            uiDgCMExchMember.PageIndex = e.NewPageIndex;
            FillCMExchangeDataGrid();
        }
        catch (Exception ex)
        {
            DisplayCMExchangeError(ex);
        }
    }

    private void ClearCMExchCtl()
    {
        uiDdlMembershipType.SelectedIndex = 0;
        uiDdlCMExchange.SelectedIndex = 0;
        uiTxbExchangeMemberCode.Text = "";
        //uiDtpExchangeStartDate.SetCalendarValue("");
        uiTxbCMExchActionFlag.Text = "";
        uiTxbCMExchbApprovalDesc.Text = "";
        uiDtpExchangeEndDate.SetCalendarValue("");
        uiDtpExchangeStartDate.SetCalendarValue("");
    }

    private void BindCMExchData(string CMExchangeID)
    {
        string ActionFlag = null;

        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        ta.FillByCMExchangeID(dt, Convert.ToDecimal(CMExchangeID));

        uiHiddenCMExchangeID.Value = CMExchangeID;

        uiDdlCMExchange.SelectedValue = dt[0].ExchangeId.ToString();
        uiTxbExchangeMemberCode.Text = dt[0].CMExchangeCode;
        uiDdlMembershipType.SelectedValue = dt[0].CMType;
        if (!dt[0].IsExchangeLicenseNoNull())
            uiTxbExchangeLicenseNo.Text = dt[0].ExchangeLicenseNo;
        else uiTxbExchangeLicenseNo.Text = "";
        uiDtpExchangeStartDate.SetCalendarValue(Convert.ToDateTime(dt[0].EffectiveStartDate).ToString("dd-MMM-yyyy"));
        if (!dt[0].IsEffectiveEndDateNull())
            uiDtpExchangeEndDate.SetCalendarValue(Convert.ToDateTime(dt[0].EffectiveEndDate).ToString("dd-MMM-yyyy"));
        else
            uiDtpExchangeEndDate.SetCalendarValue("");

        ActionFlag = (dt[0].IsActionFlagNull()) ? "" : dt[0].ActionFlag;

        switch (ActionFlag)
        {
            case "I": ActionFlag = "Insert"; break;
            case "U": ActionFlag = "Update"; break;
            case "D": ActionFlag = "Delete"; break;
        }

        uiTxbCMExchActionFlag.Text = ActionFlag;
        if (!dt[0].IsApprovalDescNull())
            uiTxbCMExchbApprovalDesc.Text = dt[0].ApprovalDesc;
        else
            uiTxbCMExchbApprovalDesc.Text = "";
    }

    private void EnableCMExchCtl(bool isActive)
    {
        uiDdlCMExchange.Enabled = isActive;
        uiDdlMembershipType.Enabled = isActive;
        uiTxbExchangeMemberCode.Enabled = isActive;
        uiDtpExchangeStartDate.SetDisabledCalendar(!isActive);
        uiDtpExchangeEndDate.SetDisabledCalendar(!isActive);

        // always set enabled to false
        uiTxbCMExchActionFlag.Enabled = false;
    }

    private void SetAccessPageCMExchangeSection()
    {
        MasterPage mp = (MasterPage)this.Master;
        bool Maker, Checker, Viewer;

        // Get roles for current user
        Maker = mp.IsMaker;
        Checker = mp.IsChecker;
        Viewer = mp.IsViewer;

        // Always check for each roles to enabled control users that has multiple roles

        if (Viewer)
        {
            // Show / hide control on Clearing Member Exchange section
            trActionExchange.Visible = true;
            trApprovalDescExchange.Visible = true;
            uiBtnSaveExchange.Visible = false;
            uiBtnDeleteExchange.Visible = false;
            uiBtnApproveExchange.Visible = false;
            uiBtnRejectExchange.Visible = false;

            // Disable control on clearing member exchange section for viewer
            EnableCMExchCtl(false);
            uiTxbCMExchbApprovalDesc.Enabled = false;

            // Hide create new record for viewer
            uiBtnCreateCMExchange.Visible = false;
        }

        if (Checker)
        {
            // Show / hide control on Clearing Member Exchange section
            trActionExchange.Visible = true;
            trApprovalDescExchange.Visible = true;
            uiBtnSaveExchange.Visible = false;
            uiBtnDeleteExchange.Visible = false;
            uiBtnApproveExchange.Visible = true;
            uiBtnRejectExchange.Visible = true;

            // Disable control on clearing member exchange section for checker
            EnableCMExchCtl(false);
            uiTxbCMExchbApprovalDesc.Enabled = true;

            // Hide create new record for checker
            uiBtnCreateCMExchange.Visible = false;
        }

        if (Maker)
        {
            // Show / hide control on Clearing Member Exchange section
            trActionExchange.Visible = false;
            trApprovalDescExchange.Visible = false;
            uiBtnSaveExchange.Visible = true;
            uiBtnDeleteExchange.Visible = true;
            uiBtnApproveExchange.Visible = false;
            uiBtnRejectExchange.Visible = false;

            // Enable control on clearing member exchange section for viewer
            EnableCMExchCtl(true);
            uiTxbCMExchbApprovalDesc.Enabled = true;

            // Show create new record for maker
            uiBtnCreateCMExchange.Visible = true;
        }

    }

    private void ShowCMExchangePanel(bool hidePanel)
    {
        pnlEditExchange.Visible = hidePanel;

        uiDgCMExchMember.Enabled = !hidePanel;
        uiBtnSearchCMExchange.Enabled = !hidePanel;
        uiBtnCreateCMExchange.Enabled = !hidePanel;
    }

    private void DisplayCMExchangeError(Exception ex)
    {
        uiBlCMExchError.Items.Clear();
        uiBlCMExchError.Items.Add(ex.Message);
        uiBlCMExchError.Visible = true;
    }

    private void ClearCMExchangeError()
    {
        uiBlCMExchError.Items.Clear();
        uiBlCMExchError.Visible = false;
    }

    private void FillCMExchangeDataGrid()
    {
        uiDgCMExchMember.DataSource = odsBOD;
        IEnumerable dv = (IEnumerable)odsCMExchange.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrderCMExchange))
        {
            dve.Sort = SortOrderCMExchange;
        }

        uiDgCMExchMember.DataSource = dve;
        uiDgCMExchMember.DataBind();
    }

    private bool isValidEntryCMExchange()
    {
        bool isValidEntryCM = true;

        if (uiTxbExchangeMemberCode.Text == "")
            uiBlCMExchError.Items.Add("Exchange Member Code is required.");

        if (uiDtpExchangeStartDate.Text == "")
            uiBlCMExchError.Items.Add("Effective Start Date is required.");

        if (Exchange.GetExchangeType(decimal.Parse(uiDdlCMExchange.SelectedValue)) == "S")
        {
            string CMType = ClearingMemberExchange.GetCMType(decimal.Parse(uiHiddenCMID.Value));
            if (CMType == "B")
            {
                if (uiDdlMembershipType.SelectedValue != "B")
                {
                    uiBlCMExchError.Items.Add("You are already registered as broker in " + 
                                                ClearingMemberExchange.GetExchangeCode(decimal.Parse(uiHiddenCMID.Value)) + 
                                                " with SPA exchange type.");
                }
            }
        }

        if (uiBlCMExchError.Items.Count > 0)
        {
            uiBlCMExchError.Visible = true;
            isValidEntryCM = false;
        }

       
        return isValidEntryCM;
    }

    #endregion 

    #region "   BOD   "

    private string SortOrderBOD
    {
        get
        {
            if (ViewState["SortOrderBOD"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrderBOD"].ToString();
            }
        }
        set { ViewState["SortOrderBOD"] = value; }
    }

    protected void uiDGBOD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ClearBODError();
                //clearBODCtl();
                ShowMgtPanel(true);
                SetAccessPageBODSection();

                int ii = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = uiDGBOD.Rows[ii];
                Label id = (Label)selectedRow.FindControl("uiLblBODID");

                BindBODData(id.Text);
                uiHiddenBODStatus.Value = "Edit";
            }
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }

    }

    protected void uiBtnCreateBOD_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();
            uiHiddenBODStatus.Value = "Create";
            clearBODCtl();
            //enableBODCtl(true);

            ShowMgtPanel(true);
            SetAccessPageBODSection();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiBtnSaveBOD_Click(object sender, EventArgs e)
    {
        // Check for valid entry
        if (!isValidEntryBOD())
            return;

        Nullable<DateTime> EndDate = null;

        if (uiDtpBODEndDate.Text != "")
            EndDate = DateTime.Parse(uiDtpBODEndDate.Text);
        bool isSignatureImageUpdated = false;
        if (uiUploadSignatureMgm.FileName != "")
        {
            isSignatureImageUpdated = true;
        }
        bool isPhotoImageUpdated = false;
        if (uiUploadMgmPic.FileName != "")
        {
            isPhotoImageUpdated= true;
        }

        if (uiHiddenBODStatus.Value == "Create")
        {
            try
            {
                ClearBODError();
                BOD.ProposeBOD(uiTxbBODNo.Text, uiTxbBODName.Text, Convert.ToDateTime(uiDtpBODDOB.Text),
                                   uiTxbIDNo.Text, uiTxbLastEducation.Text, uiTxbBODWorkExperience.Text,
                                   uiDdlBODPosition.SelectedValue, uiTxbBODMobilePhoneNo.Text, uiTxbBODDirect.Text,
                                   uiTxbBODEmail.Text, uiTxbBODCertificateNo.Text,
                                   Convert.ToDateTime(uiDtpBODCertificateDate.Text), uiChkSignatureAuthor.Checked,
                                   Convert.ToDateTime(uiDtpBODStartDate.Text), EndDate, Convert.ToDecimal(uiHiddBODCMID.Value),
                                   uiTxbBODAddress.Text, uiTxbBODProvince.Text,
                                   uiTxbBODCity.Text, uiTxbPlaceOfBirth.Text, uiTxbPostalCode.Text,
                                   uiUploadMgmPic.FileBytes, uiUploadSignatureMgm.FileBytes,
                                   User.Identity.Name, "I", null, isSignatureImageUpdated,isPhotoImageUpdated);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose create of clearing member BOD", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Set display
                ShowMgtPanel(false);

                // Refresh data after inserting new record
                FillBODDataGrid();
            }
            catch (Exception ex)
            {
                DisplayBODError(ex);
            }
        }
        else if (uiHiddenBODStatus.Value == "Edit")
        {
            
            try
            {
                ClearBODError();
                BOD.ProposeBOD(uiTxbBODNo.Text, uiTxbBODName.Text, Convert.ToDateTime(uiDtpBODDOB.Text),
                                   uiTxbIDNo.Text, uiTxbLastEducation.Text, uiTxbBODWorkExperience.Text,
                                   uiDdlBODPosition.SelectedValue, uiTxbBODMobilePhoneNo.Text, uiTxbBODDirect.Text,
                                   uiTxbBODEmail.Text, uiTxbBODCertificateNo.Text,
                                   Convert.ToDateTime(uiDtpBODCertificateDate.Text), uiChkSignatureAuthor.Checked,
                                   Convert.ToDateTime(uiDtpBODStartDate.Text), EndDate, Convert.ToDecimal(uiHiddBODCMID.Value),
                                   uiTxbBODAddress.Text, uiTxbBODProvince.Text,
                                   uiTxbBODCity.Text, uiTxbPlaceOfBirth.Text, uiTxbPostalCode.Text,
                                   uiUploadMgmPic.FileBytes, uiUploadSignatureMgm.FileBytes,
                                   User.Identity.Name, "U", Convert.ToDecimal(uiHiddenBODID.Value), 
                                   isSignatureImageUpdated, isPhotoImageUpdated);
                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose update of clearing member BOD", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Set display
                ShowMgtPanel(false);

                // Refresh data after inserting new record
                FillBODDataGrid();
            }
            catch (Exception ex)
            {
                DisplayBODError(ex);
            }

        }
    }

    protected void uiBtnBODDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();
            Nullable<DateTime> EndDate = null;

            if (uiDtpBODEndDate.Text != "")
                EndDate = Convert.ToDateTime(uiDtpBODEndDate.Text);

            BOD.ProposeBOD(uiTxbBODNo.Text, uiTxbBODName.Text, Convert.ToDateTime(uiDtpBODDOB.Text),
                                   uiTxbIDNo.Text, uiTxbLastEducation.Text, uiTxbBODWorkExperience.Text,
                                   uiDdlBODPosition.SelectedValue, uiTxbBODMobilePhoneNo.Text, uiTxbBODDirect.Text,
                                   uiTxbBODEmail.Text, uiTxbBODCertificateNo.Text,
                                   Convert.ToDateTime(uiDtpBODCertificateDate.Text), uiChkSignatureAuthor.Checked,
                                   Convert.ToDateTime(uiDtpBODStartDate.Text),EndDate, Convert.ToDecimal(uiHiddBODCMID.Value),
                                   uiTxbBODAddress.Text, uiTxbBODProvince.Text,
                                   uiTxbBODCity.Text, uiTxbPlaceOfBirth.Text, uiTxbPostalCode.Text,
                                   uiUploadMgmPic.FileBytes, uiUploadSignatureMgm.FileBytes,
                                   User.Identity.Name, "D", Convert.ToDecimal(uiHiddenBODID.Value),false,false);
            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose delete of clearing member BOD", User.Identity.Name, Common.GetIPAddress(this.Request));

            // Set display
            ShowMgtPanel(false);

            // Refresh data after deleting record
            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiBtnBODApprove_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();
            if (uiTxbBODApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            BOD.Approve(Convert.ToDecimal(uiHiddenBODID.Value), User.Identity.Name, uiTxbBODApprovalDesc.Text);

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Approve update of clearing member BOD", User.Identity.Name, Common.GetIPAddress(this.Request));

            // Set display
            ShowMgtPanel(false);

            // Refresh data after approving record
            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiBtnBODReject_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();

            if (uiTxbBODApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            BOD.Reject(Convert.ToDecimal(uiHiddenBODID.Value), User.Identity.Name, uiTxbBODApprovalDesc.Text);

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Reject update of clearing member BOD", User.Identity.Name, Common.GetIPAddress(this.Request));

            // Set display
            ShowMgtPanel(false);

            // Refresh data after reject record
            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiBtnCancelBOD_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();
            //enableBODCtl(false);
            //clearBODCtl();
            ShowMgtPanel(false);
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiBtnSearchBOD_Click(object sender, EventArgs e)
    {
        try
        {
            ClearBODError();
            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiDGBOD_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ClearBODError();
            uiDGBOD.PageIndex = e.NewPageIndex;
            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    protected void uiDGBOD_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ClearBODError();

            if (string.IsNullOrEmpty(SortOrderBOD))
            {
                SortOrderBOD = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrderBOD.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrderBOD = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrderBOD = e.SortExpression + " " + "ASC";
                }
            }

            FillBODDataGrid();
        }
        catch (Exception ex)
        {
            DisplayBODError(ex);
        }
    }

    private void BindBODData(string BODID)
    {
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
        BODData.BODDataTable dt = new BODData.BODDataTable();

        ta.FillByBODID(dt, Convert.ToDecimal(BODID));

        if (dt.Count > 0)
        {
            uiHiddenBODID.Value = dt[0].BODID.ToString();
            uiHiddBODno.Value = dt[0].BODNo;

            uiTxbBODNo.Text = dt[0].BODNo;

            if (!dt[0].IsNameNull())
                uiTxbBODName.Text = dt[0].Name;
            else
                uiTxbBODName.Text = "";
            uiDtpBODDOB.SetCalendarValue(dt[0].DOB.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsPOBNull())
                uiTxbPlaceOfBirth.Text = dt[0].POB;
            else
                uiTxbPlaceOfBirth.Text = "";
            if (!dt[0].IsAddressNull())
                uiTxbBODAddress.Text = dt[0].Address;
            else
                uiTxbBODAddress.Text = "";
            if (!dt[0].IsCityNull())
                uiTxbBODCity.Text = dt[0].City;
            else
                uiTxbBODCity.Text = "";
            if (!dt[0].IsProvinceNull())
                uiTxbBODProvince.Text = dt[0].Province;
            else
                uiTxbBODProvince.Text = "";
            if (!dt[0].IsPostCodeNull())
                uiTxbPostalCode.Text = dt[0].PostCode;
            else
                uiTxbPostalCode.Text = "";
            if (!dt[0].IsIDNONull())
                uiTxbIDNo.Text = dt[0].IDNO;
            else
                uiTxbIDNo.Text = "";
            if (!dt[0].IsMobilePhoneNoNull())
                uiTxbBODMobilePhoneNo.Text = dt[0].MobilePhoneNo;
            else
                uiTxbBODMobilePhoneNo.Text = "";
            if (!dt[0].IsPhoneNoNull())
                uiTxbBODDirect.Text = dt[0].PhoneNo;
            else
                uiTxbBODDirect.Text = "";
            if (!dt[0].IsEmailNull())
                uiTxbBODEmail.Text = dt[0].Email;
            else
                uiTxbBODEmail.Text = "";
            uiDdlBODPosition.SelectedValue = dt[0].BoardPosition;
            if (!dt[0].IsEducationNull())
                uiTxbLastEducation.Text = dt[0].Education;
            else 
                uiTxbLastEducation.Text = "";
            if (!dt[0].IsWorkExpNull())
                uiTxbBODWorkExperience.Text = dt[0].WorkExp;
            else
                uiTxbBODWorkExperience.Text = "";
            if (!dt[0].IsSignatureAuthorNull())
                uiChkSignatureAuthor.Checked = dt[0].SignatureAuthor;
            if (!dt[0].IsSignatureImageIDNull())
            {
                uiBtnBODDownloadSigPic.Enabled = true;
            }
            else
            {
                uiBtnBODDownloadSigPic.Enabled = false;
            }
            if (!dt[0].IsCertNoNull())
                uiTxbBODCertificateNo.Text = dt[0].CertNo;
            else
                uiTxbBODCertificateNo.Text = "";
            uiDtpBODCertificateDate.SetCalendarValue(dt[0].CertDate.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsPhotoImageIDNull())
            {
                uiBtnBODDownloadMgmPic.Enabled = true;
            }
            else
            {
                uiBtnBODDownloadMgmPic.Enabled = false;
            }
                        
            uiDtpBODStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsEffectiveEndDateNull())
                uiDtpBODEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
            else
                uiDtpBODEndDate.SetCalendarValue("");
            
            if (!dt[0].IsActionFlagNull())
                uiTxbBODAction.Text = (dt[0].ActionFlag == "I") ? "Insert" : ((dt[0].ActionFlag == "U") ? "Update" : "Delete");
            else
                uiTxbBODAction.Text = "";
            if (!dt[0].IsApprovalDescNull())
                uiTxbBODApprovalDesc.Text = dt[0].ApprovalDesc;
            else
                uiTxbBODApprovalDesc.Text = "";
        }
    }
    
    private void ShowMgtPanel(bool hidePanel)
    {
        uiDGBOD.Enabled = !hidePanel;
        pnlEditMgt.Visible = hidePanel;

        uiBtnSearchBOD.Enabled = !hidePanel;
        uiBtnCreateBOD.Enabled = !hidePanel;
    }

    private void SetAccessPageBODSection()
    {
        MasterPage mp = (MasterPage)this.Master;
        bool Maker, Checker, Viewer;

        // Get roles for current user
        Maker = mp.IsMaker;
        Checker = mp.IsChecker;
        Viewer = mp.IsViewer;

        // Always check for each roles to enabled control users that has multiple roles
        if (Viewer)
        {
            // Show / hide control on BOD section
            trActionBOD.Visible = true;
            trApprovalDescBOD.Visible = true;
            uiBtnSaveBOD.Visible = false;
            uiBtnDeleteBOD.Visible = false;
            uiBtnApproveBOD.Visible = false;
            uiBtnRejectBOD.Visible = false;

            // Disable control on BOD section for viewer
            EnableBODCtl(false);
            uiTxbBODApprovalDesc.Enabled = false;

            // Hide create new record button for viewer
            uiBtnCreateBOD.Visible = false;
        }

        if (Checker)
        {
            // Show / hide control on BOD section
            trActionBOD.Visible = true;
            trApprovalDescBOD.Visible = true;
            uiBtnSaveBOD.Visible = false;
            uiBtnDeleteBOD.Visible = false;
            uiBtnApproveBOD.Visible = true;
            uiBtnRejectBOD.Visible = true;

            // Disable control on BOD section for checker
            EnableBODCtl(false);
            uiTxbBODApprovalDesc.Enabled = true;

            // Hide create new record button for checker
            uiBtnCreateBOD.Visible = false;
        }

        if (Maker)
        {
            // Show / hide control on BOD section
            trActionBOD.Visible = false;
            trApprovalDescBOD.Visible = false;
            uiBtnSaveBOD.Visible = true;
            uiBtnDeleteBOD.Visible = true;
            uiBtnApproveBOD.Visible = false;
            uiBtnRejectBOD.Visible = false;

            // Enable control on BOD section for maker
            EnableBODCtl(true);
            uiTxbBODApprovalDesc.Enabled = true;

            // Show create new record button for maker
            uiBtnCreateBOD.Visible = true;
        }

    }

    private void DisplayBODError(Exception ex)
    {
        uiBlBODError.Items.Clear();
        uiBlBODError.Items.Add(ex.Message);
        uiBlBODError.Visible = true;
    }

    private void ClearBODError()
    {
        uiBlBODError.Visible = false;
        uiBlBODError.Items.Clear();
    }

    private void EnableBODCtl(bool isActive)
    {
        uiTxbBODNo.Enabled = isActive;
        uiTxbBODName.Enabled = isActive;
        uiTxbPlaceOfBirth.Enabled = isActive;
        uiTxbBODAddress.Enabled = isActive;
        uiTxbBODCity.Enabled = isActive;
        uiTxbBODProvince.Enabled = isActive;
        uiTxbPostalCode.Enabled = isActive;
        uiTxbIDNo.Enabled = isActive;
        uiTxbBODMobilePhoneNo.Enabled = isActive;
        uiTxbBODDirect.Enabled = isActive;
        uiTxbBODEmail.Enabled = isActive;
        uiDdlBODPosition.Enabled = isActive;
        uiTxbLastEducation.Enabled = isActive;
        uiTxbBODWorkExperience.Enabled = isActive;
        uiChkSignatureAuthor.Enabled = isActive;
        uiTxbCertificateNO.Enabled = isActive;
        uiUploadMgmPic.Enabled = isActive;
        uiUploadSignatureMgm.Enabled = isActive;
        uiTxbBODCertificateNo.Enabled = isActive;
        uiTxbBODApprovalDesc.Enabled = isActive;

        uiDtpBODDOB.SetDisabledCalendar(!isActive);
        uiDtpBODStartDate.SetDisabledCalendar(!isActive);
        uiDtpBODEndDate.SetDisabledCalendar(!isActive);
        uiDtpBODCertificateDate.SetDisabledCalendar(!isActive);

        // Always set enabled to false
        uiTxbBODAction.Enabled = false;
    }

    private void clearBODCtl()
    {
        uiTxbBODNo.Text = "";
        uiTxbBODName.Text = "";
        uiTxbBODAddress.Text = "";
        uiTxbBODDirect.Text = "";
        uiTxbBODEmail.Text = "";
        uiTxbBODMobilePhoneNo.Text = "";
        uiTxbBODWorkExperience.Text = "";
        uiTxbCertificateNO.Text = "";
        uiTxbIDNo.Text = "";
        uiChkSignatureAuthor.Checked = false;
        uiDdlBODPosition.SelectedIndex = 0;
        uiTxbPlaceOfBirth.Text = "";
        uiTxbBODProvince.Text = "";
        uiTxbBODCity.Text = "";
        uiTxbPostalCode.Text = "";
        uiTxbPlaceOfBirth.Text = "";
        uiTxbLastEducation.Text = "";
        uiTxbBODCertificateNo.Text = "";

        uiDtpBODCertificateDate.SetCalendarValue("");
        uiDtpBODDOB.SetCalendarValue("");
        uiDtpBODEndDate.SetCalendarValue("");
        uiDtpBODStartDate.SetCalendarValue("");

        uiTxbBODAction.Text = "";
        uiTxbBODApprovalDesc.Text = "";
    }

    private void FillBODDataGrid()
    {
        uiDGBOD.DataSource = odsBOD;
        IEnumerable dv = (IEnumerable)odsBOD.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrderBOD))
        {
            dve.Sort = SortOrderBOD;
        }

        uiDGBOD.DataSource = dve;
        uiDGBOD.DataBind();
    }

    private bool isValidEntryBOD()
    {
        bool isValid = true;

        ClearBODError();

        if (uiTxbBODNo.Text == "")
            uiBlBODError.Items.Add("BOD No is required.");

        if (uiDtpBODDOB.Text == "")
            uiBlBODError.Items.Add("Date of Birth is required.");

        if (uiDtpBODCertificateDate.Text == "")
            uiBlBODError.Items.Add("Certificate date is required");

        if (uiDtpBODStartDate.Text == "")
            uiBlBODError.Items.Add("Effective start date is required");

        if (uiBlBODError.Items.Count > 0)
        {
            uiBlBODError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

    protected void uiBtnBODDownloadSigPic_Click(object sender, EventArgs e)
    {
        BODData.BODDataTable dt = new BODData.BODDataTable();
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();

        if (uiUploadFileLogo.HasFile)
        {
            if (uiUploadFileLogo.FileBytes != null)
            {
                ShowFile(uiUploadFileLogo.PostedFile.FileName);
            }
        }
        else
        {
            if (currentID != null)
            {
                dt = ta.GetDataByBODID(Convert.ToDecimal(uiHiddenBODID.Value));
                if (dt.Count > 0)
                {
                    if (!dt[0].IsSignatureImageIDNull())
                    {
                        ShowFile(dt[0].SignatureImage);
                    }
                }
            }
        }
    }

    protected void uiBtnBODDownloadMgmPic_Click(object sender, EventArgs e)
    {
        BODData.BODDataTable dt = new BODData.BODDataTable();
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();

        if (uiUploadFileLogo.HasFile)
        {
            if (uiUploadFileLogo.FileBytes != null)
            {
                ShowFile(uiUploadFileLogo.PostedFile.FileName);
            }
        }
        else
        {
            if (currentID != null)
            {
                dt = ta.GetDataByBODID(Convert.ToDecimal(uiHiddenBODID.Value));
                if (dt.Count > 0)
                {
                    if (!dt[0].IsPhotoImageIDNull())
                    {
                        ShowFile(dt[0].PhotoImage);
                    }
                }
            }
        }
    }

    #endregion

    #region "   Product   "

    private string SortOrderProduct
    {
        get
        {
            if (ViewState["SortOrderProduct"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrderProduct"].ToString();
            }
        }
        set { ViewState["SortOrderProduct"] = value; }
    }

    protected void uiBtnCreateProduct_Click(object sender, EventArgs e)
    {
        try
        {
            ClearProductError();
            uiHiddenProductStatus.Value = "Create";
            Session["CMIDLookup"] = uiHiddenProductTraderCMID.Value;
            //enableProductCTL(true);
            clearProductCTL();

            ShowProductPanel(true);
            SetAccessPageProductSection();
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    protected void uiDGProduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                CtlContractCommodityLookup1.DisabledLookupButton = true;
                CtlCMBroker.DisabledLookupButton = true;
                uiDtpProductStartDate.DisabledCalendar = true;

                ClearProductError();
                ShowProductPanel(true);
                SetAccessPageProductSection();

                Session["CMIDLookup"] = uiHiddenProductTraderCMID.Value;
                uiHiddenProductStatus.Value = "Edit";
                int ii = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = uiDGProduct.Rows[ii];
                string jj = uiDGProduct.DataKeys[ii].Value.ToString();
                uiHiddenProductClusterID.Value = jj;
                uiHiddenProductTraderCMID.Value = uiHiddenCMID.Value;

                BindProductData(uiHiddenProductClusterID.Value);
               
            }
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    protected void uiBtnSaveProduct_Click(object sender, EventArgs e)
    {
        // Check for valid entry
        if (!isValidEntryProduct())
            return;
        
        Nullable<DateTime> EndDate = null;

        if (uiDtpProductEndDate.Text != "")
            EndDate = Convert.ToDateTime(uiDtpProductEndDate.Text);

        if (uiHiddenProductStatus.Value == "Create")
        {
            try
            {
                ClearProductError();
                Cluster.ProposedCluster(Convert.ToDecimal(uiHiddenProductTraderCMID.Value),
                                       Convert.ToDecimal(CtlCMBroker.LookupTextBoxID),
                                       Convert.ToDecimal(CtlContractCommodityLookup1.LookupTextBoxID),
                                       Convert.ToDateTime(uiDtpProductStartDate.Text),
                                       EndDate,
                                       User.Identity.Name, "I", null);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose insert of cluster", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                // Change display mode
                ShowProductPanel(false);

                // Refresh grid to reflect changes
                FillProductDataGrid();
            }
            catch (Exception ex)
            {
                DisplayProductError(ex);
            }
        }
        else if (uiHiddenProductStatus.Value == "Edit")
        {
            try
            {
                ClearProductError();
                ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
                ClusterData.ClusterDataTable dt = new ClusterData.ClusterDataTable();
                ta.FillByClusterID(dt, Convert.ToDecimal(uiHiddenProductClusterID.Value));

                if (dt.Count > 0)
                {
                    ClearingMemberDataTableAdapters.ClearingMemberTableAdapter taCM = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
                    ClearingMemberData.ClearingMemberDataTable dtCM = new ClearingMemberData.ClearingMemberDataTable();
                    taCM.FillByCMID(dtCM, dt[0].BrokerCMID);
                    CtlCMBroker.SetClearingMemberValue(dt[0].BrokerCMID.ToString(), dtCM[0].Code);

                    CommodityDataTableAdapters.CommodityTableAdapter taCommodity = new CommodityDataTableAdapters.CommodityTableAdapter();
                    CommodityData.CommodityDataTable dtCommodity = new CommodityData.CommodityDataTable();
                    taCommodity.FillByCommodityId(dtCommodity, dt[0].CommodityID);
                    CtlContractCommodityLookup1.SetCommodityValue(dt[0].CommodityID.ToString(), dtCommodity[0].CommodityCode);

                    //ContractDataTableAdapters.ContractTableAdapter taContract = new ContractDataTableAdapters.ContractTableAdapter();
                    //ContractData.ContractDataTable dtContract = new ContractData.ContractDataTable();
                    //taContract.FillByContractID(dtContract, dt[0].ContractID);
                    //CtlContractCommodityLookup1.SetCommodityValue(dt[0].ContractID.ToString(), dtContract[0].CommodityCode + ' ' + dtContract[0].ContractYear + ' ' + dtContract[0].ContractMonth);

                    uiDtpProductStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
                   

                }

                Cluster.ProposedCluster(Convert.ToDecimal(uiHiddenProductTraderCMID.Value),
                                       dt[0].BrokerCMID,
                                       dt[0].CommodityID,
                                       dt[0].EffectiveStartDate,
                                       EndDate,
                                       User.Identity.Name, "U", Convert.ToDecimal(uiHiddenProductClusterID.Value));

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose update of cluster", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                // Change display mode
                ShowProductPanel(false);

                // Refresh grid to reflect changes
                FillProductDataGrid();
            }
            catch (Exception ex)
            {
                DisplayProductError(ex);
            }
        }
    }

    protected void uiBtnProductDelete_Click(object sender, EventArgs e)
    {
        if (uiHiddenProductStatus.Value == "Edit")
        {
            try
            {
                ClearProductError();
                Nullable<DateTime> EndDate = null;

                if (uiDtpProductEndDate.Text != "")
                    EndDate = Convert.ToDateTime(uiDtpProductEndDate.Text);

                Cluster.ProposedCluster(Convert.ToDecimal(uiHiddenProductTraderCMID.Value),
                                       Convert.ToDecimal(CtlCMBroker.LookupTextBoxID),
                                       Convert.ToDecimal(CtlContractCommodityLookup1.LookupTextBoxID),
                                       Convert.ToDateTime(uiDtpProductStartDate.Text),
                                       EndDate,
                                       User.Identity.Name, "D", 1);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose delete of cluster", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Change display mode
                ShowProductPanel(false);

                // Refresh grid to reflect changes
                FillProductDataGrid();
            }
            catch (Exception ex)
            {
                DisplayProductError(ex);
            }
        }
    }

    protected void uiBtnProductReject_Click(object sender, EventArgs e)
    {
        if (uiHiddenProductStatus.Value == "Edit")
        {
            try
            {
                ClearProductError();
                if (uiTxbProductApprovalDesc.Text == "")
                    throw new ApplicationException("Approval description is required.");

                Cluster.Reject(Convert.ToDecimal(uiHiddenProductClusterID.Value), User.Identity.Name, uiTxbProductApprovalDesc.Text);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Reject update of cluster", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Change display mode
                ShowProductPanel(false);

                // Refresh grid to reflect changes
                FillProductDataGrid();
            }
            catch (Exception ex)
            {
                DisplayProductError(ex);
            }
        }
    }

    protected void uiBtnProductApprove_Click(object sender, EventArgs e)
    {
        if (uiHiddenProductStatus.Value == "Edit")
        {
            try
            {
                ClearProductError();
                if (uiTxbProductApprovalDesc.Text == "")
                    throw new ApplicationException("Approval description is required.");

                Cluster.Approve(Convert.ToDecimal(uiHiddenProductClusterID.Value), User.Identity.Name, uiTxbProductApprovalDesc.Text);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Approve update of cluster", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Change display mode
                ShowProductPanel(false);

                // Refresh grid to reflect changes
                FillProductDataGrid();
            }
            catch (Exception ex)
            {
                DisplayProductError(ex);
            }
        }
    }

    protected void uiBtnCancelProduct_Click(object sender, EventArgs e)
    {
        try
        {
            ClearProductError();
            //ShowProductPanel(false);
            Response.Redirect("~/PlanningAndDevelopment/MemberManagement/EntryClearingMember.aspx?id=" + currentID + "&profileID=" + currentProfileID);
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    protected void uiBtnProductSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ClearProductError();
            FillProductDataGrid();
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    protected void uiDGProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ClearProductError();
            uiDGProduct.PageIndex = e.NewPageIndex;
            FillProductDataGrid();
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    protected void uiDGProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ClearProductError();

            if (string.IsNullOrEmpty(SortOrderProduct))
            {
                SortOrderProduct = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrderProduct.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrderProduct = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrderProduct = e.SortExpression + " " + "ASC";
                }
            }

            FillProductDataGrid();
        }
        catch (Exception ex)
        {
            DisplayProductError(ex);
        }
    }

    private void BindProductData(string ClusterID)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        ClusterData.ClusterDataTable dt = new ClusterData.ClusterDataTable();
        ta.FillByClusterID(dt, Convert.ToDecimal(ClusterID));

        if (dt.Count > 0)
        {
            ClearingMemberDataTableAdapters.ClearingMemberTableAdapter taCM = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
            ClearingMemberData.ClearingMemberDataTable dtCM = new ClearingMemberData.ClearingMemberDataTable();
            taCM.FillByCMID(dtCM, dt[0].BrokerCMID);
            CtlCMBroker.SetClearingMemberValue(dt[0].BrokerCMID.ToString(), dtCM[0].Code);

            CommodityDataTableAdapters.CommodityTableAdapter taCommodity = new CommodityDataTableAdapters.CommodityTableAdapter();
            CommodityData.CommodityDataTable dtCommodity = new CommodityData.CommodityDataTable();
            taCommodity.FillByCommodityId(dtCommodity, dt[0].CommodityID);
            CtlContractCommodityLookup1.SetCommodityValue(dt[0].CommodityID.ToString(), dtCommodity[0].CommodityCode);

            //ContractDataTableAdapters.ContractTableAdapter taContract = new ContractDataTableAdapters.ContractTableAdapter();
            //ContractData.ContractDataTable dtContract = new ContractData.ContractDataTable();
            //taContract.FillByContractID(dtContract, dt[0].ContractID);
            //CtlContractCommodityLookup1.SetCommodityValue(dt[0].ContractID.ToString(), dtContract[0].CommodityCode + ' ' + dtContract[0].ContractYear + ' ' + dtContract[0].ContractMonth);

            uiLblProductCMCode.Text = dt[0].TraderCMCode + " - " + dt[0].TraderCMName;
            uiLblTraderCMID.Text = dt[0].TraderCMCode;
            uiHiddenProductTraderCMID.Value = dt[0].TraderCMID.ToString();
            uiDtpProductStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsEffectiveEndDateNull())
                uiDtpProductEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
            else
                uiDtpProductEndDate.SetCalendarValue("");

            if (!dt[0].IsActionFlagNull())
                uiTxbProductActionFlag.Text = (dt[0].ActionFlag == "I") ? "Insert" : ((dt[0].ActionFlag == "U") ? "Update" : "Delete");
            else
                uiTxbProductActionFlag.Text = "";

            if (!dt[0].IsApprovalDescNull())
                uiTxbProductApprovalDesc.Text = dt[0].ApprovalDesc;
            else
                uiTxbProductApprovalDesc.Text = "";
        }
    }

    private void EnableProductCtl(bool isEnabled)
    {
        CtlCMBroker.SetDisabledClearingMember(!isEnabled);
        CtlContractCommodityLookup1.SetDisabledCommodity(!isEnabled);
        uiDtpProductEndDate.SetDisabledCalendar(!isEnabled);
        uiDtpProductStartDate.SetDisabledCalendar(!isEnabled);

        // Always set enabled to false
        uiTxbProductActionFlag.Enabled = false;
    }

    private void clearProductCTL()
    {
        CtlCMBroker.SetClearingMemberValue("", "");
        CtlContractCommodityLookup1.SetCommodityValue("", "");
        uiDtpProductStartDate.SetCalendarValue("");
        uiDtpProductEndDate.SetCalendarValue("");
        uiTxbProductActionFlag.Text = "";
        uiTxbProductApprovalDesc.Text = "";
    }

    private void ShowProductPanel(bool hidePanel)
    {
        pnlEditProduct.Visible = hidePanel;
        uiDGProduct.Enabled = !hidePanel;

        uiBtnProductSearch.Enabled = !hidePanel;
        uiBtnCreateProduct.Enabled = !hidePanel;
    }

    private void DisplayProductError(Exception ex)
    {
        uiBlProductError.Items.Clear();
        uiBlProductError.Items.Add(ex.Message);
        uiBlProductError.Visible = true;
    }

    private void ClearProductError()
    {
        uiBlProductError.Items.Clear();
        uiBlProductError.Visible = false;
    }

    private void SetAccessPageProductSection()
    {
        MasterPage mp = (MasterPage)this.Master;
        bool Maker, Checker, Viewer;

        // Get roles for current user
        Maker = mp.IsMaker;
        Checker = mp.IsChecker;
        Viewer = mp.IsViewer;

        // Always check for each roles to enable control users that has multiple roles

        if (Viewer)
        {
            // Show / hide control for product section
            trActionProduct.Visible = true;
            trApprovalDescProduct.Visible = true;
            uiBtnSaveProduct.Visible = false;
            uiBtnDeleteProduct.Visible = false;
            uiBtnApproveProduct.Visible = false;
            uiBtnRejectProduct.Visible = false;

            // Disable product control for viewer
            EnableProductCtl(false);
            uiTxbProductApprovalDesc.Enabled = false;

            // Hide create new record button for viewer
            uiBtnCreateProduct.Visible = false;
        }

        if (Checker)
        {
            // Show / hide control for product section
            trActionProduct.Visible = true;
            trApprovalDescProduct.Visible = true;
            uiBtnSaveProduct.Visible = false;
            uiBtnDeleteProduct.Visible = false;
            uiBtnApproveProduct.Visible = true;
            uiBtnRejectProduct.Visible = true;

            // Disable product control for checker
            EnableProductCtl(false);
            uiTxbProductApprovalDesc.Enabled = true;

            // Hide create new record button for checker
            uiBtnCreateProduct.Visible = false;
        }

        if (Maker)
        {
            // Show / hide control for product section
            trActionProduct.Visible = false;
            trApprovalDescProduct.Visible = false;
            uiBtnSaveProduct.Visible = true;
            uiBtnDeleteProduct.Visible = true;
            uiBtnApproveProduct.Visible = false;
            uiBtnRejectProduct.Visible = false;

            // Enable product control for maker
            EnableProductCtl(true);
            uiTxbProductApprovalDesc.Enabled = true;

            // Show create new record button for maker
            uiBtnCreateProduct.Visible = true;
        }

    }

    private void FillProductDataGrid()
    {
        uiDGProduct.DataSource = odsProduct;
        IEnumerable dv = (IEnumerable)odsProduct.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrderProduct))
        {
            dve.Sort = SortOrderProduct;
        }

        uiDGProduct.DataSource = dve;
        uiDGProduct.DataBind();
    }

    private bool isValidEntryProduct()
    {
        bool isValidEntryProd = true;

        ClearProductError();

        if (CtlContractCommodityLookup1.LookupTextBox == "")
            uiBlProductError.Items.Add("Contract is required.");

        if (CtlCMBroker.LookupTextBox == "")
            uiBlProductError.Items.Add("Clearing Member Broker is required.");

        if (uiDtpProductStartDate.Text == "")
            uiBlProductError.Items.Add("Effective start date is required");

        if (uiHiddenProductStatus.Value != "Edit")
        {
            ClearingMemberExchangeData.ClearingMemberExchangeDataTable cmExchDt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
            //creator cluster must be trader in membership type.

            //Trader CMID and Broker CMID must have the same ExchangeID selected Contract ID
            // get exchangeid Contract
            if (!string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBox) &&
                !string.IsNullOrEmpty(CtlCMBroker.LookupTextBoxID) &&
                !string.IsNullOrEmpty(uiDtpProductStartDate.Text))
            {
                decimal contractExchangeID = Contract.GetExchangeID(decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID));
                cmExchDt = ClearingMemberExchange.fillByCMID(uiHiddenCMID.Value);
                bool isTrader = false;
                foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in cmExchDt)
                {
                    if (dr.CMType == "T")
                    {
                        isTrader = true;
                    }
                }
                if (!isTrader)
                {
                    uiBlProductError.Items.Add("As Broker you are not authorized to create or modifiy cluster.");
                }
                cmExchDt.Clear();
                cmExchDt.AcceptChanges();

                cmExchDt = ClearingMemberExchange.fillByCMIDAndExchangeID(decimal.Parse(CtlCMBroker.LookupTextBoxID), contractExchangeID);
                bool isBroker = false;
                foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in cmExchDt)
                {
                    if (dr.CMType == "B")
                    {
                        isBroker = true;
                    }
                }
                if (!isBroker)
                {
                    uiBlProductError.Items.Add("Clearing Member type is not valid.");
                }
                //get Exchangeid in trader CMID
                cmExchDt.Clear();
                cmExchDt.AcceptChanges();

                cmExchDt = ClearingMemberExchange.fillByCMID(uiHiddenCMID.Value);
                bool isTraderCMIDexist = false;
                foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in cmExchDt)
                {
                    if (contractExchangeID == dr.ExchangeId)
                    {
                        isTraderCMIDexist = true;
                    }
                }
                if (!isTraderCMIDexist)
                {
                    uiBlProductError.Items.Add("Trader is not registered in " + Exchange.GetExchangeNameByExchangeId(contractExchangeID) + " Exchange.");
                }

                cmExchDt.Clear();
                cmExchDt.AcceptChanges();
                cmExchDt = ClearingMemberExchange.fillByCMID(CtlCMBroker.LookupTextBoxID);
                bool isBrokerCMIDexist = false;
                foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in cmExchDt)
                {
                    if (contractExchangeID == dr.ExchangeId)
                    {
                        isBrokerCMIDexist = true;
                    }
                }
                if (!isBrokerCMIDexist)
                {
                    uiBlProductError.Items.Add("Broker is not registered in " + Exchange.GetExchangeNameByExchangeId(contractExchangeID) + " Exchange.");
                }

                if (Cluster.CountBrokerContract(decimal.Parse(CtlCMBroker.LookupTextBoxID), 
                    decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID),
                    DateTime.Parse(uiDtpProductStartDate.Text)) > 0)
                {
                    uiBlProductError.Items.Add("Broker already trade the same contract with another trader.");
                }

               
            }
        }

        if (!string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBox) &&
               !string.IsNullOrEmpty(CtlCMBroker.LookupTextBoxID) &&
               !string.IsNullOrEmpty(uiDtpProductEndDate.Text))
        {
            if (Cluster.CheckOpenPosition(decimal.Parse(Session["CMIDLookup"].ToString()),
                   decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID),
                   DateTime.Parse(uiDtpProductEndDate.Text)) > 0)
            {
                uiBlProductError.Items.Add("There is still open position.");
            }
        }
        
        if (uiBlProductError.Items.Count > 0)
        {
            uiBlProductError.Visible = true;
            isValidEntryProd = false;
        }

        return isValidEntryProd;
    }
    
    #endregion

    #region "   ExchangeMember   "

    private string SortOrderEM
    {
        get
        {
            if (ViewState["SortOrderEM"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrderEM"].ToString();
            }
        }
        set { ViewState["SortOrderEM"] = value; }
    }

    protected void uiBtnSaveEM_Click(object sender, EventArgs e)
    {
        // Check for valid entry
        if (!isValidEntryEM())
            return;

        if (uiHiddenEMStatus.Value == "Create")
        {
            try
            {
                string CMRep = "N";
                Nullable<DateTime> EndDate = null;

                ClearEMError();
                if (uiChkCMRep.Checked)
                    CMRep = "Y";
                if (uiDtpEMEndDate.Text != "")
                    EndDate = Convert.ToDateTime(uiDtpEMEndDate.Text);

                string MiniLotFlag = "N";
                if (uiChkMiniLot.Checked)
                    MiniLotFlag = "Y";


                ExchangeMember.ProposedEM(uiTxbEMCode.Text,
                                              Convert.ToDecimal(uiDdlEMexch.SelectedValue),
                                              Convert.ToDateTime(uiDtpEMStartDate.Text),
                                              EndDate,
                                              Convert.ToDecimal(uiHiddenEMCMID.Value),
                                              uiTxbEMName.Text, CMRep, uiDdlEMStatus.SelectedValue,
                                              User.Identity.Name, "I", null, MiniLotFlag);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose insert of exchange member", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                // Set display mode
                ShowEMPanel(false);

                // Refresh grid to reflect changes
                FillEMDataGrid();
            }
            catch (Exception ex)
            {
                DisplayEMError(ex);
            }
        }
        else if (uiHiddenEMStatus.Value == "Edit")
        {
            try
            {
                string CMRep = "N";
                Nullable<DateTime> EndDate = null;

                ClearEMError();
                if (uiChkCMRep.Checked)
                    CMRep = "Y";
                if (uiDtpEMEndDate.Text != "")
                    EndDate = Convert.ToDateTime(uiDtpEMEndDate.Text);

                string MiniLotFlag = "N";
                if (uiChkMiniLot.Checked)
                    MiniLotFlag = "Y";

                
                ExchangeMember.ProposedEM(uiTxbEMCode.Text,
                                              Convert.ToDecimal(uiDdlEMexch.SelectedValue),
                                              Convert.ToDateTime(uiDtpEMStartDate.Text),
                                              EndDate,
                                              Convert.ToDecimal(uiHiddenEMCMID.Value),
                                              uiTxbEMName.Text, CMRep, uiDdlEMStatus.SelectedValue,
                                              User.Identity.Name, "U", Convert.ToDecimal(uiHiddenEMid.Value), MiniLotFlag);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose update of exchange member", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                // Set display mode
                ShowEMPanel(false);

                // Refresh grid to reflect changes
                FillEMDataGrid();
            }
            catch (Exception ex)
            {
                DisplayEMError(ex);
            }
        }

    }

    protected void uiBtnEMCreate_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();
            ClearEMCtl();
            uiHiddenEMStatus.Value = "Create";
            ShowEMPanel(true);
            SetAccessPageEMSection();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiBtnEMSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();
            FillEMDataGrid();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiDgExchangeMember_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ClearEMError();
                //ClearEMCtl();
                ShowEMPanel(true);
                SetAccessPageEMSection();

                uiHiddenEMStatus.Value = "Edit";
                int ii = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = uiDgExchangeMember.Rows[ii];
                string jj = uiDgExchangeMember.DataKeys[ii].Value.ToString();
                uiHiddenEMid.Value = jj;

                BindEMData(uiHiddenEMid.Value);
                
            }
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
        
    }

    protected void uiDgExchangeMember_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ClearEMError();

            if (string.IsNullOrEmpty(SortOrderEM))
            {
                SortOrderEM = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrderEM.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrderEM = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrderEM = e.SortExpression + " " + "ASC";
                }
            }

            FillEMDataGrid();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiDgExchangeMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ClearEMError();
            uiDgExchangeMember.PageIndex = e.NewPageIndex;
            FillEMDataGrid();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }
    
    protected void uiBtnDeleteEM_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();

            string CMRep = "N";
            if (uiChkCMRep.Checked)
                CMRep = "Y";

            string MiniLotFlag = "N";
            if (uiChkMiniLot.Checked)
                MiniLotFlag = "Y";

            if (uiHiddenEMStatus.Value == "Edit")
            {
                ExchangeMember.ProposedEM(uiTxbEMCode.Text,
                                             Convert.ToDecimal(uiDdlEMexch.SelectedValue),
                                             Convert.ToDateTime(uiDtpEMStartDate.Text),
                                             Convert.ToDateTime(uiDtpEMEndDate.Text),
                                             Convert.ToDecimal(uiHiddenEMCMID.Value),
                                             uiTxbEMName.Text, CMRep, uiDdlEMStatus.SelectedValue,
                                             User.Identity.Name, "D", Convert.ToDecimal(uiHiddenEMid.Value), MiniLotFlag);

                ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Propose delete of exchange member", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                // Change display mode
                ShowEMPanel(false);

                // Refresh grid to reflect changes
                FillEMDataGrid();
            }
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiBtnApproveEM_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();
            if (uiTxbEMApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            ExchangeMember.Approve(Convert.ToDecimal(uiHiddenEMid.Value), uiTxbEMApprovalDesc.Text,User.Identity.Name);

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Approve update of exchange member", User.Identity.Name, Common.GetIPAddress(this.Request));
            
            // Change display mode
            ShowEMPanel(false);

            // Refresh grid to reflect changes
            FillEMDataGrid();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiBtnRejectEM_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();
            if (uiTxbEMApprovalDesc.Text == "")
                throw new ApplicationException("Approval description is required.");

            ExchangeMember.Reject(Convert.ToDecimal(uiHiddenEMid.Value), uiTxbEMApprovalDesc.Text, User.Identity.Name);

            ApplicationLog.Insert(DateTime.Now, "Clearing Member", "I", "Reject update of exchange member", User.Identity.Name, Common.GetIPAddress(this.Request));
            
            // Change display mode
            ShowEMPanel(false);

            // Refresh grid to reflect changes
            FillEMDataGrid();
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    protected void uiBtnCancelEM_Click(object sender, EventArgs e)
    {
        try
        {
            ClearEMError();
            ShowEMPanel(false);
        }
        catch (Exception ex)
        {
            DisplayEMError(ex);
        }
    }

    private void BindEMData(string EMID)
    {
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();

        dt = ExchangeMember.FillByEMID(Convert.ToDecimal(EMID));
        if (dt.Count > 0)
        {
            uiTxbEMCode.Text = dt[0].Code;
            uiTxbEMName.Text = dt[0].Name;
            uiDdlEMexch.SelectedValue = dt[0].ExchangeId.ToString();

            uiDtpEMStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (!dt[0].IsEffectiveEndDateNull())
            {
                uiDtpEMEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
            }
            else
            {
                uiDtpEMEndDate.SetCalendarValue("");
            }

            uiChkCMRep.Checked = false;
            if (!dt[0].IsCMRepNull())
                uiChkCMRep.Checked = (dt[0].CMRep == "Y") ? true : false;

            uiChkMiniLot.Checked = false;
            if (!dt[0].IsMiniLotNull())
                uiChkMiniLot.Checked = (dt[0].MiniLot == "Y") ? true : false;

            uiDdlEMStatus.SelectedValue = dt[0].Status;
            if (!dt[0].IsActionFlagNull())
            {
                if (dt[0].ActionFlag == "I")
                {
                    uiTxbEMAction.Text = "Insert";
                }
                else if (dt[0].ActionFlag == "U")
                {
                    uiTxbEMAction.Text = "Update";
                }
                else if (dt[0].ActionFlag == "D")
                {
                    uiTxbEMAction.Text = "Delete";
                }
            }
            else
                uiTxbEMAction.Text = "";

            if (!dt[0].IsApprovalDescNull())
                uiTxbEMApprovalDesc.Text = dt[0].ApprovalDesc;
            else
                uiTxbEMApprovalDesc.Text = "";
        }
        
    }

    private void ShowEMPanel(bool hidePanel)
    {
        pnlEditExchangeMember.Visible = hidePanel;

        uiBtnEMSearch.Enabled = !hidePanel;
        uiBtnCreateEM.Enabled = !hidePanel;
        uiDgExchangeMember.Enabled = !hidePanel;
    }

    private void DisplayEMError(Exception ex)
    {
        uiBlEMError.Items.Clear();
        uiBlEMError.Items.Add(ex.Message);
        uiBlEMError.Visible = true;
    }

    private void ClearEMError()
    {
        uiBlEMError.Items.Clear();
        uiBlEMError.Visible = false;
    }

    private void SetAccessPageEMSection()
    {
        MasterPage mp = (MasterPage)this.Master;
        bool Maker, Checker, Viewer;

        // Get roles for current user
        Maker = mp.IsMaker;
        Checker = mp.IsChecker;
        Viewer = mp.IsViewer;

        // Always check for each roles to enable control users that has multiple roles

        if (Viewer)
        {
            // Show / hide control for product section
            trActionExchangeMember.Visible = true;
            trApprovalDescExchangeMember.Visible = true;
            uiBtnSaveEM.Visible = false;
            uiBtnDeleteEM.Visible = false;
            uiBtnApproveEM.Visible = false;
            uiBtnRejectEM.Visible = false;

            // Disable product control for viewer
            EnableEMCtl(false);
            uiTxbEMApprovalDesc.Enabled = false;

            // Hide create new record button for viewer
            uiBtnCreateEM.Visible = false;
        }

        if (Checker)
        {
            // Show / hide control for product section
            trActionExchangeMember.Visible = true;
            trApprovalDescExchangeMember.Visible = true;
            uiBtnSaveEM.Visible = false;
            uiBtnDeleteEM.Visible = false;
            uiBtnApproveEM.Visible = true;
            uiBtnRejectEM.Visible = true;

            // Disable product control for checker
            EnableEMCtl(false);
            uiTxbEMApprovalDesc.Enabled = true;

            // Hide create new record button for checker
            uiBtnCreateEM.Visible = false;
        }

        if (Maker)
        {
            // Show / hide control for product section
            trActionExchangeMember.Visible = false;
            trApprovalDescExchangeMember.Visible = false;
            uiBtnSaveEM.Visible = true;
            uiBtnDeleteEM.Visible = true;
            uiBtnApproveEM.Visible = false;
            uiBtnRejectEM.Visible = false;

            // Enable product control for maker
            EnableEMCtl(true);
            uiTxbEMApprovalDesc.Enabled = true;

            // Show create new record button for maker
            uiBtnCreateEM.Visible = true;
        }

    }

    private void EnableEMCtl(bool isEnabled)
    {
        uiTxbEMCode.Enabled = isEnabled;
        uiTxbEMName.Enabled = isEnabled;
        uiDdlEMexch.Enabled = isEnabled;
        uiDtpEMStartDate.SetDisabledCalendar(!isEnabled);
        uiDtpEMEndDate.SetDisabledCalendar(!isEnabled);
        uiChkCMRep.Enabled = isEnabled;
        uiChkMiniLot.Enabled = isEnabled;
        uiDdlEMStatus.Enabled = isEnabled;

        // Always set enabled to false
        uiTxbEMAction.Enabled = false;
    }

    private void ClearEMCtl()
    {
        uiTxbEMCode.Text = "";
        uiTxbEMName.Text = "";
        uiDtpEMStartDate.SetCalendarValue("");
        uiDtpEMEndDate.SetCalendarValue("");
        uiChkCMRep.Checked = false;
        uiChkMiniLot.Checked = false;

        // Always set enabled to false
        uiTxbEMAction.Text = "";
        uiTxbEMApprovalDesc.Text = "";
    }

    private void FillEMDataGrid()
    {
        uiDgExchangeMember.DataSource = odsExchangeMember;
        IEnumerable dv = (IEnumerable)odsExchangeMember.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrderEM))
        {
            dve.Sort = SortOrderEM;
        }

        uiDgExchangeMember.DataSource = dve;
        uiDgExchangeMember.DataBind();
    }

    private bool isValidEntryEM()
    {
        bool isValidEM = true;

        ClearEMError();

        if (uiTxbEMCode.Text == "")
            uiBlEMError.Items.Add("Exchange Member code is required.");

        if (uiTxbEMName.Text == "")
            uiBlEMError.Items.Add("Exchange Member name is required.");

        if (uiBlEMError.Items.Count > 0)
        {
            uiBlEMError.Visible = true;
            isValidEM = false;
        }

        return isValidEM;
    }
        
    #endregion

   
}
