namespace Contas.Core.ViewModels
{
    public class ServiceSettings
    {
        public SendGridModel sendGrid { get; set; }
        public string fcmUrl { get; set; }
        public string fcmKey { get; set; }
        public string fcmSenderId { get; set; }

    }
    public class SendGridModel
    {
        public string key { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

}
