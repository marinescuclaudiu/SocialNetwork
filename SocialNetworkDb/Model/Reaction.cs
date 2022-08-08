using static SocialNetworkDb.Utility.Enumerations;

namespace SocialNetworkDb.Model
{
    public class Reaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public ReactionType ReactionType { get; set; }

    }
}
