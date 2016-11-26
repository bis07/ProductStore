using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;

namespace ActivityLibrary1.Concrete
{
    public class EmailSettings
    {
        public string MailToAdress = "order@example.com";
        public string MailFromAdress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServarName = "smpt.example.com";
        public int SrverPort = 587;
        public bool WriteasFile = false;
        public string FileLocation = @"d:\sports_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings email)
        {
            emailSettings = email;
        }
        public void ProcessorOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServarName;
                smtpClient.Port = emailSettings.SrverPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials= new NetworkCredential(emailSettings.Username,emailSettings.Password);

                if (emailSettings.WriteasFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body =new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("___")
                    .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price*line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal : {2:c}", line.Quantity, line.Product.Name, subtotal);

                    body.AppendFormat("---")
                        .AppendLine("Ship to :")
                        .AppendLine(shippingDetails.Name)
                        .AppendLine(shippingDetails.Line1)
                        .AppendLine(shippingDetails.Line2)
                        .AppendLine(shippingDetails.Line3)
                        .AppendLine(shippingDetails.City)
                        .AppendLine(shippingDetails.State)
                        .AppendLine(shippingDetails.Country)
                        .AppendLine(shippingDetails.Zip)
                        .AppendLine("---")
                        .AppendFormat("Gift wrap : {0}", shippingDetails.GiftWrap ? "Yes" : "No");
                    MailMessage mailMessage= new MailMessage( emailSettings.MailFromAdress,
                        emailSettings.MailToAdress,
                        "New order submitted!",
                        body.ToString());

                    if (emailSettings.WriteasFile)
                    {
                        mailMessage.BodyEncoding= Encoding.ASCII;
                    }
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}
