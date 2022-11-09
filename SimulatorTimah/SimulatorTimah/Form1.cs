using MySql.Data.MySqlClient;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulatorTimah
{
    public partial class Form1 : Form
    {
        static string sqlsrv_hostname = ConfigurationSettings.AppSettings["sqlsvr_hostname"];
        static string sqlsvr_username = ConfigurationSettings.AppSettings["sqlsvr_username"];
        static string sqlsvr_password = ConfigurationSettings.AppSettings["sqlsvr_password"];
        static string sqlsvr_db = ConfigurationSettings.AppSettings["sqlsvr_db"];
        static string sqlsvr_db_staging = ConfigurationSettings.AppSettings["sqlsvr_db_staging"];

        static string mysql_hostname = ConfigurationSettings.AppSettings["mysql_hostname"];
        static string mysql_username = ConfigurationSettings.AppSettings["mysql_username"];
        static string mysql_password = ConfigurationSettings.AppSettings["mysql_password"];
        static string mysql_db = ConfigurationSettings.AppSettings["mysql_db"];
        static string mysql_port = ConfigurationSettings.AppSettings["mysql_port"];

        static string connectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};SslMode=none", mysql_hostname, mysql_port, mysql_db, mysql_username, mysql_password);

        public Form1()
        {
            InitializeComponent();
            disableAllObject();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            disableAllObject();
            if (cboSimulator.SelectedIndex == 0)
            {
                enableGroupCtdDaily();
            }
            else if(cboSimulator.SelectedIndex == 2)
            {
                enableGroupFullDelivery();
            }
            else
            {
                enableGroupTradeFeed();
            }
        }

        private void disableAllObject()
        {
            #region group CTD Daily
            txtJumlahBst.Enabled = false;
            txtSellerName.Enabled = false;
            txtProduct.Enabled = false;
            txtBrand.Enabled = false;
            cboWarehouse.Enabled = false;
            txtFormatBst.Enabled = false;
            txtFormatCoa.Enabled = false;
            txtBusinessDate.Enabled = false;
            txtSequence.Enabled = false;
            btnSimulateCTD.Enabled = false;
            #endregion

            #region Full Delivery
            txtFullDelivery.Enabled = false;
            btnSimulatorFD.Enabled = false;
            #endregion

            #region Trade Feed
            txtJumlahTrade.Enabled = false;
            txtStartSeq.Enabled = false;
            txtTradeTime.Enabled = false;
            txtProductCode.Enabled = false;
            txtBuyerCode.Enabled = false;
            txtSellerCode.Enabled = false;
            txtFormatExRef.Enabled = false;
            txtFormatBSTTrade.Enabled = false;
            txtSeqBST.Enabled = false;
            txtCIF.Enabled = false;
            txtJumlahTrade.Enabled = false;
            btnSimulateTF.Enabled = false;
            txtSeqExReff.Enabled = false;
            #endregion
        }

        private void enableGroupCtdDaily()
        {
            txtJumlahBst.Enabled = true;
            txtSellerName.Enabled = true;
            txtProduct.Enabled = true;
            txtBrand.Enabled = true;
            cboWarehouse.Enabled = true;
            txtFormatBst.Enabled = true;
            txtFormatCoa.Enabled = true;
            txtBusinessDate.Enabled = true;
            txtSequence.Enabled = true;
            btnSimulateCTD.Enabled = true;
        }

        private void enableGroupFullDelivery()
        {
            txtFullDelivery.Enabled = true;
            btnSimulatorFD.Enabled = true;
        }

        private void enableGroupTradeFeed()
        {
            txtJumlahTrade.Enabled = true;
            txtStartSeq.Enabled = true;
            txtTradeTime.Enabled = true;
            txtProductCode.Enabled = true;
            txtBuyerCode.Enabled = true;
            txtSellerCode.Enabled = true;
            txtFormatExRef.Enabled = true;
            txtFormatBSTTrade.Enabled = true;
            txtSeqBST.Enabled = true;
            txtCIF.Enabled = true;
            txtJumlahTrade.Enabled = true;
            btnSimulateTF.Enabled = true;
            txtSeqExReff.Enabled = true;
        }

        private void btnSimulateCTD_Click(object sender, EventArgs e)
        {
            ctdDaily ctd = new ctdDaily();

            ctd.receiveNumber = "RCV\\00005";
            ctd.ctdReff = 10000;
            ctd.ctdStatus = 5;
            ctd.idSeller = 1;
            ctd.tradeAccount = "TTPCM00001";
            ctd.brandCode = "1";
            ctd.whNumber = 2;
            ctd.sellerName = txtSellerName.Text;
            ctd.brandName = txtBrand.Text;
            ctd.whLocation = cboWarehouse.SelectedItem.ToString();
            ctd.qualityCode = txtProduct.Text;

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            for (int i=0; i<int.Parse(txtJumlahBst.Text); i++)
            {
                int sequence = int.Parse(txtSequence.Text) + i + 1;
                string textSeq = "";
                if(sequence.ToString().Length == 1)
                {
                    textSeq = "000" + sequence;
                }
                else if (sequence.ToString().Length == 2)
                {
                    textSeq = "00" + sequence;
                }
                else if (sequence.ToString().Length == 3)
                {
                    textSeq = "0" + sequence;
                }
                else
                {
                    textSeq = "" + sequence;
                }

                String bulan = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();

                Random random = new Random();
                ctd.ctdNumber = txtFormatBst.Text.Replace("{tahun}", DateTime.Now.Year.ToString()).Replace("{bulan}", bulan).Replace("{sequence}", textSeq);
                ctd.coaNumber = txtFormatCoa.Text.Replace("{sequence}", textSeq);
                ctd.ctdDate = DateTime.Parse(txtBusinessDate.Text);
                ctd.volume = random.Next(4997, 5000);

                try
                {
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText =   "INSERT INTO ctdDaily (receiveNumber, ctdReff, ctdNumber, ctdDate, ctdStatus, idSeller, sellerName, tradeAccount, brandCode, brandName, volume, whNumber, whLocation, coaNumber, qualityCode, createDate, updateDate) " +
                                        "VALUES (@receiveNumber, @ctdReff, @ctdNumber, @ctdDate, @ctdStatus, @idSeller, @sellerName, @tradeAccount, @brandCode, @brandName, @volume, @whNumber, @whLocation, @coaNumber, @qualityCode, @createDate, @updateDate)";

                    cmd.Parameters.AddWithValue("@receiveNumber", ctd.receiveNumber);
                    cmd.Parameters.AddWithValue("@ctdReff", ctd.ctdReff);
                    cmd.Parameters.AddWithValue("@ctdNumber", ctd.ctdNumber);
                    cmd.Parameters.AddWithValue("@ctdDate", ctd.ctdDate);
                    cmd.Parameters.AddWithValue("@ctdStatus", ctd.ctdStatus);
                    cmd.Parameters.AddWithValue("@idSeller", ctd.idSeller);
                    cmd.Parameters.AddWithValue("@sellerName", ctd.sellerName);
                    cmd.Parameters.AddWithValue("@tradeAccount", ctd.tradeAccount);
                    cmd.Parameters.AddWithValue("@brandCode", ctd.brandCode);
                    cmd.Parameters.AddWithValue("@brandName", ctd.brandName);
                    cmd.Parameters.AddWithValue("@volume", ctd.volume);
                    cmd.Parameters.AddWithValue("@whNumber", ctd.whNumber);
                    cmd.Parameters.AddWithValue("@whLocation", ctd.whLocation);
                    cmd.Parameters.AddWithValue("@coaNumber", ctd.coaNumber);
                    cmd.Parameters.AddWithValue("@qualityCode", ctd.qualityCode);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);

                    int x = cmd.ExecuteNonQuery();
                    appendConsole(ctd.ctdNumber + " : created!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        private void btnSimulatorFD_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText =   "INSERT INTO delivery (ctdReff, ctdNumber, ctdStatus, deliveryDate, deliveryNumber, vhcNumber, driverName, asal, tujuan, volume) " + 
                                    "VALUES (@ctdReff, @ctdNumber, @ctdStatus, @deliveryDate, @deliveryNumber, @vhcNumber, @driverName, @asal, @tujuan, @volume)";

                Delivery delivery = new Delivery();

                delivery.ctdNumber = txtFullDelivery.Text;
                delivery.ctdReff = 10000;
                delivery.ctdStatus = 10;
                delivery.deliveryDate = DateTime.Now;
                delivery.deliveryNumber = "";
                delivery.vhcNumber = "";
                delivery.driverName = "";
                delivery.asal = "";
                delivery.tujuan = "";
                delivery.volume = 5000;

                cmd.Parameters.AddWithValue("@ctdReff", delivery.ctdReff);
                cmd.Parameters.AddWithValue("@ctdNumber", delivery.ctdNumber);
                cmd.Parameters.AddWithValue("@ctdStatus", delivery.ctdStatus);
                cmd.Parameters.AddWithValue("@deliveryDate", delivery.deliveryDate);
                cmd.Parameters.AddWithValue("@deliveryNumber", delivery.deliveryNumber);
                cmd.Parameters.AddWithValue("@vhcNumber", delivery.vhcNumber);
                cmd.Parameters.AddWithValue("@driverName", delivery.driverName);
                cmd.Parameters.AddWithValue("@asal", delivery.asal);
                cmd.Parameters.AddWithValue("@tujuan", delivery.tujuan);
                cmd.Parameters.AddWithValue("@volume", delivery.volume);

                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    appendConsole("Nomor BST : " + txtFullDelivery.Text + " : status 10");
                }
                else
                {
                    appendConsole("Nomor BST : " + txtFullDelivery.Text + " : gagal update");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                appendConsole(ex.StackTrace);
            }

            conn.Close();
            clearGroupFullDelivery();
        }

        private void clearGroupFullDelivery()
        {
            txtFullDelivery.Text = "";
        }

        private void appendConsole(string text)
        {
            txtConsole.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss : ") + text + "\r\n" + txtConsole.Text;
        }

        private void btnSimulateTF_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder2 = new SqlConnectionStringBuilder();
            builder2.DataSource = sqlsrv_hostname;
            builder2.UserID = sqlsvr_username;
            builder2.Password = sqlsvr_password;
            builder2.InitialCatalog = sqlsvr_db_staging;
            builder2.ConnectTimeout = 0;
            builder2.MultipleActiveResultSets = true;

            TradeFeed trade = new TradeFeed();
            Random random = new Random();

            trade.BusinessDate = DateTime.Parse(txtTradeTime.Text);
            trade.TradeTime = DateTime.Parse(txtTradeTime.Text);
            trade.TradeTimeOffset = 420;
            trade.ProductCode = txtProductCode.Text;
            trade.ContractMonth = "209912";
            trade.OptionCode = null;
            trade.StrikePriceOption = 0;
            trade.EntitasTradeVersion = 0;
            trade.Price = random.Next(18000, 22000);
            trade.Quantity = 1;
            trade.ContraIndicator = null;
            trade.SellerClearingCode = "899";
            trade.SellerExchangeCode = "899";
            trade.SellerTraderType = null;
            trade.SellerAccountCode = txtSellerCode.Text;
            trade.SellerGiveUpCode = null;
            trade.SellerCommisionGiveUp = 0;
            trade.SellerTradeTypeCode = null;
            trade.SellerTotalLegs = 0;
            trade.BuyerClearingCode = "899";
            trade.BuyerExchangeCode = "899";
            trade.BuyerTraderType = null;
            trade.BuyerAccountCode = txtBuyerCode.Text;
            trade.BuyerGiveUpCode = null;
            trade.BuyerCommisionGiveUp = 0;
            trade.BuyerTradeTypeCode = null;
            trade.BuyerTotalLegs = 0;
            trade.BuyerRef = txtCIF.Text;
            trade.Flag = null;
            trade.trade_id_detail = null;

            int seqExchangeRef = 1 + int.Parse(txtSeqExReff.Text);
            int seqExReff = 0;

            for(int i=0; i<int.Parse(txtJumlahTrade.Text); i++)
            {
                int sequence = int.Parse(txtSeqBST.Text) + i + 1;
                string textSeq = "";
                if (sequence.ToString().Length == 1)
                {
                    textSeq = "000" + sequence;
                }
                else if (sequence.ToString().Length == 2)
                {
                    textSeq = "00" + sequence;
                }
                else if (sequence.ToString().Length == 3)
                {
                    textSeq = "0" + sequence;
                }

                string date = DateTime.Now.Date.ToString("dd").Length < 2 ? "0" + DateTime.Now.Date.ToString("dd") : DateTime.Now.Date.ToString("dd");
                string bulan = DateTime.Now.Date.ToString("MM").Length < 2 ? "0" + DateTime.Now.Date.ToString("MM") : DateTime.Now.Date.ToString("MM");

                trade.Sequence = i + int.Parse(txtStartSeq.Text);
                trade.SellerRef = txtFormatBSTTrade.Text.Replace("{tahun}", DateTime.Now.Year.ToString()).Replace("{bulan}", bulan).Replace("{sequence}", textSeq);

                Console.WriteLine(trade.SellerRef);

                string textSegRef = "";

                if (seqExchangeRef.ToString().Length == 1)
                {
                    textSegRef = "000" + seqExchangeRef;
                }
                else if (seqExchangeRef.ToString().Length == 2)
                {
                    textSegRef = "00" + seqExchangeRef;
                }
                else if (seqExchangeRef.ToString().Length == 3)
                {
                    textSegRef = "0" + seqExchangeRef;
                }
                else
                {
                    textSegRef = "" + seqExchangeRef;
                }

                if (seqExReff < 5)
                {
                    seqExReff++;
                }
                else
                {
                    seqExReff = 0;
                    seqExchangeRef++;
                }

                string formatExRef = txtFormatExRef.Text.Replace("{tanggal}", date).Replace("{sequence}", textSegRef);
                trade.ExchangeRef = formatExRef;

                using (SqlConnection connection = new SqlConnection(builder2.ConnectionString))
                {
                    connection.Open();

                    string sqlRawTradeFeed = "INSERT INTO RawTradeFeed(BusinessDate,Sequence,TradeTime,TradeTimeOffset,ProductCode,ContractMonth," +
                                            "Price, Quantity, ExchangeRef, SellerClearingCode, SellerExchangeCode " +
                                            ", SellerAccountCode " +
                                            ", SellerRef, BuyerClearingCode, BuyerExchangeCode, BuyerAccountCode, " +
                                            "BuyerRef)" +
                                            "VALUES (@BusinessDate,@Sequence,@TradeTime,@TradeTimeOffset,@ProductCode,@ContractMonth," + 
                                            " @Price, @Quantity, @ExchangeRef, @SellerClearingCode, @SellerExchangeCode " +
                                            ",@SellerAccountCode " +
                                            ", @SellerRef, @BuyerClearingCode, @BuyerExchangeCode, @BuyerAccountCode, " +
                                            "@BuyerRef)";

                    using (SqlCommand command = new SqlCommand(sqlRawTradeFeed, connection))
                    {
                        command.Parameters.AddWithValue("@BusinessDate", trade.BusinessDate);
                        command.Parameters.AddWithValue("@Sequence", trade.Sequence);
                        command.Parameters.AddWithValue("@TradeTime", trade.TradeTime);
                        command.Parameters.AddWithValue("@TradeTimeOffset", trade.TradeTimeOffset);
                        command.Parameters.AddWithValue("@ProductCode", trade.ProductCode);
                        command.Parameters.AddWithValue("@ContractMonth", trade.ContractMonth);
                        command.Parameters.AddWithValue("@Price", trade.Price);
                        command.Parameters.AddWithValue("@Quantity", trade.Quantity);
                        command.Parameters.AddWithValue("@ExchangeRef", trade.ExchangeRef);
                        command.Parameters.AddWithValue("@SellerClearingCode", trade.SellerClearingCode);
                        command.Parameters.AddWithValue("@SellerExchangeCode", trade.SellerExchangeCode);
                        command.Parameters.AddWithValue("@SellerAccountCode", trade.SellerAccountCode);
                        command.Parameters.AddWithValue("@SellerRef", trade.SellerRef);
                        command.Parameters.AddWithValue("@BuyerClearingCode", trade.BuyerClearingCode);
                        command.Parameters.AddWithValue("@BuyerExchangeCode", trade.BuyerExchangeCode);
                        command.Parameters.AddWithValue("@BuyerAccountCode", trade.BuyerAccountCode);
                        command.Parameters.AddWithValue("@BuyerRef", trade.BuyerRef);

                        try
                        {
                            int x = command.ExecuteNonQuery();

                            if(x > 0)
                            {
                                appendConsole("Insert RawTradefeed Success!");
                            }else
                            {
                                appendConsole("Insert RawTradefeed Failed!");
                            }
                        }
                        catch(Exception ex)
                        {
                            appendConsole(ex.Message);
                        }
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = "10.15.10.87";
            int port = 22;
            string username = "hasto";
            string password = "Jakarta01";
            string directoryBst = "/home/hasto/ftp/bgr";
            string filedirectory = "D:/PDFKosong.pdf";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "KBIDRC-TIMAH-DBMS.PTKBI.COM";
            builder.UserID = "sa";
            builder.Password = "P@ssw0rd2019";
            builder.InitialCatalog = "TIN_KBI";
            builder.ConnectTimeout = 0;
            builder.MultipleActiveResultSets = true;

            using (var client = new SftpClient(host, port, username, password))
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    client.Connect();
                    if (client.IsConnected)
                    {
                        client.ChangeDirectory(directoryBst);

                        string nobst = "";
                        string sql = "SELECT DISTINCT nobst FROM SKD.StagingSellerAllocation WHERE flag = 1";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    nobst = reader.GetString(0).Replace("/", "_");
                                    if (IsDirectoryExists(client, nobst))
                                    {
                                        client.CreateDirectory(nobst);
                                    }

                                    client.ChangeDirectory(directoryBst + "/" + nobst);

                                    using (var fileStream = new FileStream(filedirectory, FileMode.Open))
                                    {
                                        client.BufferSize = 50 * 1024;
                                        client.UploadFile(fileStream, "/");
                                    }
                                }
                            }
                        }

                        
                    }
                }
            }
        }

        private static bool IsDirectoryExists(SftpClient sftp, string path)
        {
            bool isDirectoryExist = true;

            try
            {
                sftp.ChangeDirectory(path);
                isDirectoryExist = false;
            }
            catch (SftpPathNotFoundException)
            {
                return true;
            }
            return isDirectoryExist;
        }

        private static void copyFile(string source, string destination, string path)
        {
            if (!Directory.Exists(destination.Substring(0, 38)))
            {
                Directory.CreateDirectory(destination.Substring(0, 38));
            }

            File.Copy(source, destination, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'tIN_KBIDataSet.ClearingMember' table. You can move, or remove it, as needed.
            this.clearingMemberTableAdapter.Fill(this.tIN_KBIDataSet.ClearingMember);

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
