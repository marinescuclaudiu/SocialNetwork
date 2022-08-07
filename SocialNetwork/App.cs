using Microsoft.EntityFrameworkCore;
using SocialNetworkDb.DbContext;
using SocialNetworkDb.Model;

namespace SocialNetwork
{
    public static class App
    {
        private static string _username;
        private static int _userId;
        public static void StartApp()
        {
            InitializeDatabase();
            UserLogin();
            ExecuteCommands();
        }
        private static void InitializeDatabase()
        {
            using var dbContext = new SocialNetworkDbContext();
            dbContext.Database.Migrate();
        }

        //Check if a user is already in DB, if not create it
        private static void UserLogin()
        {
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();

            using var dbContext = new SocialNetworkDbContext();

            var user = dbContext.Users.SingleOrDefault(u => u.Username == _username);

            if (user == null)
            {
                Console.Write("Account has been registered successfully. ");
                user = new User { Username = _username, CreationDate = DateTime.Now };
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            _userId = user.Id;

            Console.WriteLine("You are now connected.");

        }

        //Create post
        private static void CreatePost()
        {
            var dbContext = new SocialNetworkDbContext();

            var user = dbContext.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == _userId);

            Console.WriteLine("Enter the text: ");

            var post = new Post { UserId = _userId, Text = Console.ReadLine() };

            user.Posts.Add(post);

            dbContext.SaveChanges();

            Console.WriteLine("You have successfully posted.");

        }

        //Print user's posts
        private static void PrintPosts()
        {
            using var dbContext = new SocialNetworkDbContext();

            var user = dbContext.Users.Include(u => u.Posts).ThenInclude(u => u.Reactions).SingleOrDefault(u => u.Id == _userId);

            Console.WriteLine($"You have {user.Posts.Count} posts.");

            foreach (var post in user.Posts)
            {
                Console.Write($"Post id = {post.Id} Text = {post.Text} Reaction count = {post.Reactions.Count} ");
                Console.Write("Reactions: ");
                foreach(var reaction in post.Reactions)
                {
                    Console.Write(reaction.ReactionType.ToString() + ' ');
                }
                Console.WriteLine();
            }
        }

        //Update text for user's first post
        private static void UpdateFirstPost()
        {
            using var dbContext = new SocialNetworkDbContext();

            var user = dbContext.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == _userId);

            var firstPost = user.Posts.FirstOrDefault();

            if (firstPost != null)
            {
                Console.WriteLine("Enter the new text for the first post: ");
                string newText = Console.ReadLine();
                firstPost.Text = newText;
                dbContext.SaveChanges();
                Console.WriteLine("You have successfully updated the post.");
            }
            else
            {
                Console.WriteLine("You don't have any post.");
            }
        }

        //Delete post
        private static void DeletePost()
        {
            PrintPosts();

            Console.Write("Enter id for the post you want to delete: ");
            int postId;
            bool successfullyParsed = int.TryParse(Console.ReadLine(), out postId);

            if (successfullyParsed == false)
            {
                Console.WriteLine("Your input is not correct.");
                return;
            }

            using var dbContext = new SocialNetworkDbContext();

            var post = dbContext.Posts.SingleOrDefault(p => p.Id == postId && p.UserId == _userId);

            bool successfullyRemoved = false;

            if (post != null)
            {
                dbContext.Posts.Remove(post);
                dbContext.SaveChanges();
                successfullyRemoved = true;
            }

            if (successfullyRemoved == false)
            {
                Console.WriteLine("Post's id does not exist.");
            }
            else
            {
                Console.WriteLine("Successfully removed.");
            }

        }

        private static void AddRemoveReaction()
        {
            using var dbContext = new SocialNetworkDbContext();

            Console.WriteLine("Enter the id of the post to which you want to add a reaction: ");

            int postId = int.Parse(Console.ReadLine());

            var post = dbContext.Posts.SingleOrDefault(p => p.Id == postId && p.UserId != _userId);

            if(post == null)
            {
                Console.WriteLine("Post's id does not correct.");
                return;
            }

            Reaction existingReaction = dbContext.Reactions.SingleOrDefault(r => r.PostId == postId && r.UserId == _userId);

            if(existingReaction != null)
            {
                dbContext.Reactions.Remove(existingReaction);

                Console.WriteLine("You already reacted to this post. So now, we delete your reaction.");
            }
            else
            {
                Console.WriteLine("Enter a number for the type of reaction:");
                ShowReactionMenu();
                Reaction.Type reactionType = (Reaction.Type)int.Parse(Console.ReadLine());

                if(Enum.IsDefined(typeof(Reaction.Type), reactionType) || reactionType == 0)
                {
                    Console.WriteLine("Success!");

                    existingReaction = new Reaction { UserId = _userId, PostId = post.Id, ReactionType = reactionType };
                    dbContext.Reactions.Add(existingReaction);
                }
                else
                {
                    Console.WriteLine("Your input is not correct");
                }
            }

            dbContext.SaveChanges();
        }

        private static void ShowReactionMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Like");
            Console.WriteLine("2 - Dislike");
            Console.WriteLine("3 - Anger");
            Console.WriteLine("4 - Sadness");
            Console.WriteLine();
        }

        private static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Create a post");
            Console.WriteLine("2 - Show all my posts.");
            Console.WriteLine("3 - Modify my first post.");
            Console.WriteLine("4 - Delete a post.");
            Console.WriteLine("5 - Add/Remove a reaction.");
            Console.WriteLine("0 - Exit program.");
            Console.WriteLine();
        }

        private static void ExecuteCommands()
        {
            int option;
            do
            {
                ShowMenu();
                Console.Write("Option: ");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        CreatePost();
                        break;

                    case 2:
                        PrintPosts();
                        break;

                    case 3:
                        UpdateFirstPost();
                        break;

                    case 4:
                        DeletePost();
                        break;
                    case 5:
                        AddRemoveReaction();
                        break;
                        
                    case 0:
                        break;

                    default:
                        Console.WriteLine("I don't have this option.");
                        break;
                }
            } while (option != 0);
        }
    }
}
