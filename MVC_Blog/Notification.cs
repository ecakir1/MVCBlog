namespace MVC_Blog.Models
{
    public class Notification
    {
        public string NotificationID { get; set; }
        public string Message { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
