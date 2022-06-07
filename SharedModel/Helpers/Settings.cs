using System;
namespace SharedModel.Helpers
{
    public class Settings
    {
        static public string databaseString = "Server=tcp:kmn-agri.database.windows.net,1433;Initial Catalog=thenazrana;Persist Security Info=False;User ID=kmn-master-user;Password=Password123/;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        static public string imageKitUrl = "https://ik.imagekit.io/3jw2q3z4w7h";
        static public string baseUrl = "https://api.thenazrana.in";
        static public string frontendUrl = "https://thenazrana.in";

        static public string blobConnection = "DefaultEndpointsProtocol=https;AccountName=advasset;AccountKey=vMn7ZVSWVi0NbY7T2X52MaS7f/Qwe2zqaNLDBHvbxGdJY9zyJVV9DJtkPuHJtStHOWucamsjk4T7rHAqTOuSJg==;EndpointSuffix=core.windows.net";

        #region MailCredential
        static public string MailServer = "mail.dealsonopenbox.com";
        static public string MailUserName = "no-reply@dealsonopenbox.com";
        static public string MailName = "Deals On OpenBox";
        static public string MailPassword = "bombom123/B@";
        static public int MailPort = 25;
        #endregion

        #region payment credential
        static public string paymentKeyId = "rzp_test_BHHx5IhiDeCyl8";
        static public string paymentSecretId = "9IlCmjrON4rAnMuYUfQKO7hn";
        #endregion


        #region whatsappCredentials
        static public string apiKey360 = "HHFdSiFcPXItQWh0dfCBjkH1AK";
        static public string apiEndpoint = "https://waba.360dialog.io";
        static public string nameSpace = "e99ab1db_c756_4b15_967b_5381a4ee36bd";
        #endregion

    }
}

