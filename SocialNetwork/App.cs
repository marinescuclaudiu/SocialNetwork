using Microsoft.EntityFrameworkCore;
using SocialNetworkDb.DbContext;
using SocialNetworkDB.Model;

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
            dbContext.Database.EnsureCreated();
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
                user = new User { Username = _username };
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

            var user = dbContext.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == _userId);

            Console.WriteLine($"You have {user.Posts.Count} posts.");

            foreach (var post in user.Posts)
            {
                Console.WriteLine($"Post id = {post.Id} Text = {post.Text}");
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

            if(post != null)
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

        private static void ShowMenu()
        {
            Console.WriteLine("1 - Create a post");
            Console.WriteLine("2 - Show all my posts.");
            Console.WriteLine("3 - Modify my first post.");
            Console.WriteLine("4 - Delete a post.");
            Console.WriteLine("0 - Exit program.");
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
