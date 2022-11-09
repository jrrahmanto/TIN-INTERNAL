using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for SMTPHelper
/// </summary>
public class SMTPHelper
{
    public SMTPHelper()
    {
        
    }

    public static void SendInvoice(string emailTo, string pathInvoice, string pathFakturPajak)
    {
        MailMessage mail = new MailMessage("infotimah@ptkbi.com", emailTo);
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Invoice dan Faktur Pajak";
        mail.Body = "Terlampir";
        mail.Attachments.Add(new Attachment(pathInvoice));
        mail.Attachments.Add(new Attachment(pathFakturPajak));
        client.Send(mail);
        mail.Dispose();
    }

    public static void SendApprove(string emailTo, string description)
    {
        MailMessage mail = new MailMessage("infotimah@ptkbi.com", emailTo);
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Approval of Registration for TIN Participant Approved";
        mail.Body = "Dear Board of Director," + 
                    "\n" +
                    "\n" +
                    "\n" +
                    "Registration of membership of the Tin Exchange has been approved. To continue the activity can access the participants dashboard again." +
                    "\n" +
                    "\n" +
                    "\n" +
                    "\n" +
                    "Thank you for your attention." + 
                    "\n" +
                    "\n" +
                    "TIN Exchange System" + 
                    "Call Center : (021) 39833066 ext 84/86";
        client.Send(mail);
        mail.Dispose();
    }

    public static void SendReject(string emailTo, string description)
    {
        MailMessage mail = new MailMessage("infotimah@ptkbi.com", emailTo);
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Approval of Registration for TIMAH Participant Denied";
        mail.Body = "Dear Board of Director," +
                    "\n" +
                    "\n" +
                    "\n" +
                    "Registration for the membership of the Tin Exchange has been rejected, please log in to the participant's dashboard to update the data on the tin exchange registration requirements." +
                    "\n" +
                    "\n" +
                    "\n" +
                    "\n" +
                    "Thank you for your attention." +
                    "\n" +
                    "\n" +
                    "TIN Exchange System" +
                    "Call Center : (021) 39833066 ext 84/86";
        client.Send(mail);
        mail.Dispose();
    }

    public static void SendInformasiVirtualAccount(string emailTo, BankData.BankAccountDataTable bankDataTable)
    {
        MailMessage mail = new MailMessage("infotimah@ptkbi.com", emailTo);
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Virtual Account Information";

        string account = "";
        
        account = account + "<table>";
        account = account + "<tr>";
        account = account + "<td>Account</td>";
        account = account + "<td>Account Type</td>";
        account = account + "<td>Virtual Account Number</td>";
        account = account + "</tr>";

        foreach (BankData.BankAccountRow row in bankDataTable)
        {
            if(row.AccountType != "RC")
            {
                account = account + "<tr>";
                account = account + "<td>" + row.InvestorCode + "</td>";
                account = account + "<td>" + row.AccountType + "</td>";
                account = account + "<td>" + row.AccountNo + "</td>";
                account = account + "</tr>";
            }
        }

        account = account + "</table>";

        mail.Body = "Dear Board of Director," +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "Together with this e-mail, We inform the virtual account number to be used in your transaction." +
                    "<br />" +
                    "<br />" +
                    account +
                    "<br />" +
                    "<br />" +
                    "Thank you for your attention." +
                    "<br />" +
                    "<br />" +
                    "TIN Exchange System<br />" +
                    "Call Center : (021) 39833066 ext 84/86";
        mail.IsBodyHtml = true;
        client.Send(mail);
        mail.Dispose();
    }

    public static void SendInformasiNoticeOfShipment(string emailTo, List<string> nosList)
    {
        MailMessage mail = new MailMessage();
        SmtpClient client = new SmtpClient();

        mail.From = new MailAddress("infotimah@ptkbi.com");

        if (emailTo.Contains(";"))
        {
            foreach(string email in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(email);
            }
        }
        else
        {
            mail.To.Add(new MailAddress(emailTo));
        }

        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Notice Of Shipment Information";

        mail.Body = "Dear Operational BGR," +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "Together with this e-mail, We inform the document Notice of Shipment." +
                    "<br />" +
                    "Please check attachment with this email." +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "Thank you for your attention." +
                    "<br />" +
                    "<br />" +
                    "TIN Exchange System<br />" +
                    "Call Center : (021) 39833066 ext 84/86";

        foreach (string path in nosList)
        {
            mail.Attachments.Add(new Attachment(path));
        }        
        mail.IsBodyHtml = true;
        client.Send(mail);
        mail.Dispose();
    }

    public static void SendInformasiNoticeOfShipmentForBuyerSeller(string emailTo, string path)
    {
        MailMessage mail = new MailMessage("infotimah@ptkbi.com", emailTo);
        SmtpClient client = new SmtpClient();
        client.Port = 25;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Host = "10.10.10.2";
        mail.Subject = "Notice Of Shipment Information";

        mail.Body = "Dear Buyer / Seller," +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "Together with this e-mail, We inform the document Notice of Shipment." +
                    "<br />" +
                    "Please check attachment with this email." +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "<br />" +
                    "Thank you for your attention." +
                    "<br />" +
                    "<br />" +
                    "TIN Exchange System<br />" +
                    "Call Center : (021) 39833066 ext 84/86";

        mail.Attachments.Add(new Attachment(path));
        mail.IsBodyHtml = true;
        client.Send(mail);
        mail.Dispose();
    }
}