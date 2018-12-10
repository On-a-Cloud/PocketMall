﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using PocketMall.Domain.Abstract;
using PocketMall.Domain.Entities;
using System.Net;

namespace PocketMall.Domain.Concrete
{
    public class EmailSetting
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "pocketmall@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\pocket_mall_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSetting emailSettings;

        public EmailOrderProcessor(EmailSetting settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username,
                    emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("_ _ _")
                    .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity,
                                       line.Product.Name,
                                       subtotal);
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                     .AppendLine("---")
                     .AppendLine("Ship to:")
                     .AppendLine(shippingInfo.Name)
                     .AppendLine(shippingInfo.Line1)
                     .AppendLine(shippingInfo.Line2 ?? "")
                     .AppendLine(shippingInfo.Line3 ?? "")
                     .AppendLine(shippingInfo.City)
                     .AppendLine(shippingInfo.State ?? "")
                     .AppendLine(shippingInfo.Country)
                     .AppendLine(shippingInfo.Zip)
                     .AppendLine("---")
                     .AppendFormat("Gift wrap: {0}", shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                         emailSettings.MailFromAddress,
                                         emailSettings.MailToAddress,
                                         "New order submitted!",
                                         body.ToString());

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);

            }
        }
    }
}