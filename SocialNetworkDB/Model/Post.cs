
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkDB.Model
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [MaxLength(200)]
        public string Text { get; set; }
    }
}
