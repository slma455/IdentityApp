namespace webApplication.DTO.Account
{
    public class EmailSendDTO
    {
        public EmailSendDTO(string to, string body, string subject)
        {
            To = to;
            Body = body;
            Subject = subject;
        }

        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
