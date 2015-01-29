using System.Net;
using System.Net.Mail;
using System.Text;

namespace ClassLib.DataStructures
{
    public class EmailOrderProcessor : IOrderProcessor
    {

        public void ProcessOrder(Cart cart, OrderDetails orderDetails)
        {
            var fromAddress = new MailAddress("testdotnetbar@gmail.com", "From Bar");
            var toAddress = new MailAddress("testdotnetbar@gmail.com", "To Me");
            const string fromPassword = "1qazXSW@3edc";
            const string subject = "New Order";
            var body = new StringBuilder()
                .AppendLine(string.Format("New Order from {0}", orderDetails.Name))
                .AppendLine(string.Format("Number {0}", orderDetails.Number))
                .AppendLine(string.Format("On {0}", orderDetails.Date))
                .AppendLine();

            foreach (var line in cart.Lines)
            {
                var subtotal = line.MenuItem.Price.Amount * line.Quantity;
                body.AppendLine(string.Format("{0} x {1} (subtotal: {2:c}", line.Quantity,
                line.MenuItem.Name,
                subtotal));
            }

            body.AppendLine()
                .AppendLine(string.Format("Total order value: {0:c}", cart.ComputeTotalValue()));

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body.ToString()
            })
            {
                smtp.Send(message);
            }
        }
    }
}