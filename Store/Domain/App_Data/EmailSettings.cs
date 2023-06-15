using Domain.Abstract;
using System.Text;
using Domain.Entities;
using System.Net.Mail;
using System.Net;

namespace Domain.App_Data
{
    public  class EmailSettings
    {
        public string MailToAdress = "Order@.com";
        public string MailFromAdress = "store.com";
        public bool UseSSL = true;
        public string UserName = "USName";
        public string ServerName = "smtp.gmail.com";
        public string Pass = "MyPass";
        public int ServerPort = 587;
        public bool WriteToFile = true;
        public string Location = @"d:\Store";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSet;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSet = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shipDet)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSet.UseSSL;
                smtpClient.Host = emailSet.ServerName;
                smtpClient.Port = emailSet.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSet.UserName,emailSet.Pass);

                if (emailSet.WriteToFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSet.Location;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder().
                    AppendLine("Новый заказ")
                    .AppendLine("---")
                    .AppendLine("Товары");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Good.Price * line.Count;
                    body.AppendFormat("{0} * {1} (итого:{2:c})", line.Count, line.Good.Name, subtotal);
                }

                body.AppendFormat("Общая стоимость", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Доставка")
                .AppendLine(shipDet.Name)
                .AppendLine(shipDet.Line1)
                .AppendLine(shipDet.Line2 ?? "")
                .AppendLine(shipDet.Line3 ?? "")
                .AppendLine(shipDet.City)
                .AppendLine(shipDet.Country)
                .AppendLine("---")
                .AppendFormat("Подарочная упаковка:{0}", shipDet.GiftWrap ? "Да" : "Нет");

                MailMessage mailMessge = new MailMessage(
                    emailSet.MailFromAdress,
                    emailSet.MailToAdress,
                    "New order",
                    body.ToString()
                    );
                smtpClient.Send(mailMessge);
            }
        }
    }
}
