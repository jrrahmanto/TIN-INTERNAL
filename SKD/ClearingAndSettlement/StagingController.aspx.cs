using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_StagingController : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnBgr_Click(object sender, EventArgs e)
    {
        try
        {
            //MySqlConnection conn = new MySqlConnection(connectionString);
            //conn.Open();

            //try
            //{
            //    MySqlCommand cmd = new MySqlCommand();
            //    cmd.Connection = conn;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "prc_rollover_ctd";
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //}
            
            using (OdbcConnection connection = new OdbcConnection("DSN=BGR"))
            {
                OdbcCommand command = new OdbcCommand();

                command.Connection = connection;
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "prc_rollover_ctd";
                command.ExecuteNonQuery();
            }
            
            ApplicationLog.Insert(DateTime.Now, "Run Command Staging BGR", "I", "Execute", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnTks_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnectionStringBuilder builder2 = new SqlConnectionStringBuilder();
            builder2.DataSource = ConfigurationManager.AppSettings["sqlserv_datasource"];
            builder2.UserID = ConfigurationManager.AppSettings["sqlserv_username"];
            builder2.Password = ConfigurationManager.AppSettings["sqlserv_password"];
            builder2.InitialCatalog = ConfigurationManager.AppSettings["sqlserv_database"];
            builder2.ConnectTimeout = 0;
            builder2.MultipleActiveResultSets = true;

            using (SqlConnection connection2 = new SqlConnection(builder2.ConnectionString))
            {
                connection2.Open();

                String sql2 = "dbo.uspReStockWarehoueTKS";

                using (SqlCommand command2 = new SqlCommand(sql2, connection2))
                {
                    command2.CommandType = CommandType.StoredProcedure;

                    command2.ExecuteNonQuery();
                }

                connection2.Close();
            }

            ApplicationLog.Insert(DateTime.Now, "Run Command Staging TKS", "I", "Execute", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnLme_Click(object sender, EventArgs e)
    {
        try
        {
            GetDataKLTM();
            ApplicationLog.Insert(DateTime.Now, "Run Command Staging LME", "I", "Execute", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnKltm_Click(object sender, EventArgs e)
    {
        try
        {
            GetDataLME();
            ApplicationLog.Insert(DateTime.Now, "Run Command Staging KLTM", "I", "Execute", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    private void GetDataKLTM()
    {
        WebClient web = new WebClient();
        String text = "";

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

        System.IO.Stream stream = web.OpenRead("https://www.lme.com/Metals/Non-ferrous/Tin#tabIndex=0");

        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
        {
            text = reader.ReadToEnd();
        }

        int pointstart = text.IndexOf("<td>Cash</td>");

        double bid = double.Parse((text.Substring(pointstart + 43, 17)).Replace("<td>", "").Replace("</td>", "").Replace(".00", ""));
        double ask = double.Parse((text.Substring(pointstart + 90, 17)).Replace("<td>", "").Replace("</td>", "").Replace(".00", ""));

        Console.WriteLine("Price LME bid : " + bid + " dan ask : " + ask);

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = ConfigurationSettings.AppSettings["sqlserv_datasource"];
        builder.UserID = ConfigurationSettings.AppSettings["sqlserv_username"];
        builder.Password = ConfigurationSettings.AppSettings["sqlserv_password"];
        builder.InitialCatalog = ConfigurationSettings.AppSettings["sqlserv_database_kbi"];
        builder.ConnectTimeout = 0;
        builder.MultipleActiveResultSets = true;

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            string sqlRawTradefeedInsert = "SKD.uspMergingKLTM";

            using (SqlCommand command = new SqlCommand(sqlRawTradefeedInsert, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@BusinessDate", SqlDbType.DateTime).Value = DateTime.Now.AddDays(-1);
                command.Parameters.Add("@Bid", SqlDbType.Money).Value = bid;
                command.Parameters.Add("@Ask", SqlDbType.Money).Value = ask;
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private void GetDataLME()
    {
        WebClient web = new WebClient();
        String text = "";
        var culture = new CultureInfo("en-US");

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

        System.IO.Stream stream = web.OpenRead("https://www.mtpma.org.my/index.php/statistic/2014-07-07-04-56-57");

        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
        {
            text = reader.ReadToEnd();
        }

        int pointstart = text.IndexOf(DateTime.Now.ToString("MMMM yyyy", culture).ToUpper() + " TIN PRICES");
        int pointend = text.IndexOf(DateTime.Now.AddMonths(-1).ToString("MMMM yyyy", culture).ToUpper() + " TIN PRICES");

        text = text.Substring(pointstart, pointend - pointstart);

        int pointdate = 0;
        int dayadd = 0;

        if (DateTime.Now.ToString("dddd", culture).ToUpper() == "MONDAY")
        {
            dayadd = -3;
        }
        else if (DateTime.Now.ToString("dddd", culture).ToUpper() == "SUNDAY")
        {
            dayadd = -2;
        }
        else
        {
            dayadd = -1;
        }

        string tanggal = int.Parse(DateTime.Now.AddDays(dayadd).ToString("dd")).ToString();

        pointdate = text.IndexOf("<span style=\"font-size: 12pt; color: #333333;\">" + int.Parse(DateTime.Now.AddDays(dayadd).ToString("dd")).ToString() + "</span></td>");

        while (pointdate == -1)
        {
            dayadd--;
            pointdate = text.IndexOf("<span style=\"font-size: 12pt; color: #333333;\">" + int.Parse(DateTime.Now.AddDays(dayadd).ToString("dd")).ToString() + "</span></td>");
        }

        double usd = 0;

        if (tanggal.Length > 1)
        {
            usd = double.Parse((text.Substring(pointdate + 186, 6)).Replace(",", ""));
        }
        else
        {
            usd = double.Parse((text.Substring(pointdate + 185, 6)).Replace(",", ""));
        }

        Console.WriteLine("Price LME USD : " + usd);

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = ConfigurationSettings.AppSettings["sqlserv_datasource"];
        builder.UserID = ConfigurationSettings.AppSettings["sqlserv_username"];
        builder.Password = ConfigurationSettings.AppSettings["sqlserv_password"];
        builder.InitialCatalog = ConfigurationSettings.AppSettings["sqlserv_database_kbi"];
        builder.ConnectTimeout = 0;
        builder.MultipleActiveResultSets = true;

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            string sqlRawTradefeedInsert = "SKD.uspMergingLME";

            using (SqlCommand command = new SqlCommand(sqlRawTradefeedInsert, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@BusinessDate", SqlDbType.DateTime).Value = DateTime.Now.AddDays(-1);
                command.Parameters.Add("@Price", SqlDbType.Money).Value = usd;
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}