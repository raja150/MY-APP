using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;

namespace TranSmart.API.Services
{

	public static class MailService
	{
		public static async Task SendPaySlips(MemoryStream PdfStream, string FromMail, string ToMail, string EmpCode, string Month)
		{
			try
			{
				var ms = new MemoryStream(PdfStream.ToArray());
				var builder = new BodyBuilder();
				ms.Position = 0;
				MimeMessage message = MimeMsg(FromMail, ToMail, "Salary Statement for the month of " + Month, "");
				string text = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailBody.txt"));

				builder.TextBody = string.Format(text, Month);
				builder.Attachments.Add(EmpCode.ToUpper() + "_" + Month + ".pdf", ms);
				message.Body = builder.ToMessageBody();
				await Smtp(message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task MailSending(string id, string token, string mailId, string appUrl)
		{
			var message = new MimeMessage();
			message.From.Add(MailboxAddress.Parse(mailId));
			message.To.Add(MailboxAddress.Parse(mailId));
			message.Subject = "Reset Your Password using below link";
			var bodyBuilder = new BodyBuilder
			{
				HtmlBody =
				$"<html>" +
				  $"<body>" +
						$"<p>You recently requested to reset the password for your account. Click the button below to proceed." +
						$"</p>" +
						$"<a href='{$"{appUrl}/#/reset/" + id + "?r=" + token}' target=_blank>Click here to reset your password" +
						$"</a> " +
						$"<p>If you did not request a password reset, please ignore this email or reply to let us know. This password reset link is only valid for the next 30 minutes." +
						$"</p>" +
						$"<h3>Thanks,</h3>" +
						$"<h3>Avontix</h3>" +
				$"</html>"
			};
			message.Body = bodyBuilder.ToMessageBody();
			await Smtp(message);
		}
		public static async Task SendMail(string fromAddress, string toAddress, string msg, string subject)
		{
			await Smtp(MimeMsg(fromAddress, toAddress, msg, subject));
		}
		public static async Task Smtp(MimeMessage message)
		{
			using (var client = new SmtpClient())
			{
				// Note: only needed if the SMTP server requires authentication
				await client.ConnectAsync("172.16.0.1", 25, SecureSocketOptions.None);
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
		}
		public static MimeMessage MimeMsg(string fromAddress, string toAddress, string subject, string msg)
		{
			var message = new MimeMessage();
			message.From.Add(MailboxAddress.Parse(fromAddress));
			message.To.Add(MailboxAddress.Parse(toAddress));
			message.Subject = subject;
			message.Body = new TextPart(@msg);
			return message;
		}
		public static MimeMessage HelpDeskResponse(string fromAddress, string toAddress, string subject, string msg)
		{
			var message = new MimeMessage();
			message.From.Add(MailboxAddress.Parse(fromAddress));
			message.To.Add(MailboxAddress.Parse(toAddress));
			message.Subject = subject;
			var bodyBuilder = new BodyBuilder
			{
				HtmlBody =
				$"<html>" +
				  $"<body>" +
						$"<p>{msg}" +
						$"</p>" +
						$"<h3>Thanks,</h3>" +
						$"<h3>Avontix</h3>" +
				$"</html>"
			};
			message.Body = bodyBuilder.ToMessageBody();
			return message;
		}
		public static async Task SendMailFromHelpDesk(string fromAddress, string toAddress, string msg, string subject)		
		{
			await Smtp(HelpDeskResponse(fromAddress, toAddress, subject, msg));
		}
		public static void ReceivingMail()
		{
			using (var client = new ImapClient())
			{
				client.Connect("imap.friends.com", 993, true);

				client.Authenticate("rajasekharg@mail.avontixindia.com", "Avontix@gr");
				// The Inbox folder is always available on all IMAP servers...
				var inbox = client.Inbox;
				inbox.Open(FolderAccess.ReadOnly);

				for (int i = 0; i < inbox.Count; i++)
				{
					_ = inbox.GetMessage(i);
				}

				client.Disconnect(true);
			}
		}

	}
}
