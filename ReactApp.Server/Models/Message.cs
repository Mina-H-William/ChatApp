using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp.Server.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

        public string Content { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
