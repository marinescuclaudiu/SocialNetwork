using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkDb.Model
{
    public enum Type
    {
        Undefined,
        Like,
        Dislike,
        Anger,
        Sadness
    }
    public class Reaction
    {
        public enum Type
        {
            Undefined,
            Like,
            Dislike,
            Anger,
            Sadness
        }
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public Type ReactionType { get; set; }

    }
}
