﻿
namespace SocialNetworkDB.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}