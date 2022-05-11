using System;
namespace SharedModel.Helpers
{
	public class Settings
	{
		static public string databaseString = "Server=tcp:kmn-agri.database.windows.net,1433;Initial Catalog=thenazrana;Persist Security Info=False;User ID=kmn-master-user;Password=Password123/;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        static public string baseUrl="";

        #region MailCredential
        static public string MailServer = "mail.dealsonopenbox.com";
        static public string MailUserName = "no-reply@dealsonopenbox.com";
        static public string MailName = "Deals On OpenBox";
        static public string MailPassword = "bombom123/B@";
        static public int MailPort = 25;
        #endregion

    }
}

