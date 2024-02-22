using Company.DAL.Model;
using System.Net;
using System.Net.Mail;

namespace Company.PL.settings
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient(" smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("zyadamarwa6@gmail.com", "tcfinsxligweyyph");
			Client.Send("zyadamarwa6@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
