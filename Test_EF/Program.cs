using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

//  ** Defintions of database :

//CREATE TABLE `Blogs` (
//	`BlogId` INT(11) NOT NULL AUTO_INCREMENT,
//	`Url` TEXT NOT NULL DEFAULT '',
//	PRIMARY KEY(`BlogId`)
//)
//COLLATE='utf8mb4_general_ci'
//ENGINE=InnoDB
//AUTO_INCREMENT = 4
//;

//CREATE TABLE `Posts` (
//	`PostId` INT(11) NOT NULL AUTO_INCREMENT,
//	`BlogId` INT(11) NOT NULL DEFAULT 0,
//	`Title` VARCHAR(50) NOT NULL DEFAULT '',
//	`Content` VARCHAR(50) NOT NULL DEFAULT '',
//	PRIMARY KEY(`PostId`),
//	INDEX `FK_Blog` (`BlogId`),
//	CONSTRAINT `FK_Blog` FOREIGN KEY(`BlogId`) REFERENCES `Blogs` (`BlogId`)
//)
//COLLATE='utf8mb4_general_ci'
//ENGINE=InnoDB
//AUTO_INCREMENT = 3
//;



namespace Test_EF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var db = new BloggingContext())
            {
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    .OrderBy(b => b.BlogId)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://devblogs.microsoft.com/dotnet";
                blog.Posts.Add(
                    new Post
                    {
                        Title = "Hello World",
                        Content = "I wrote an app using EF Core!"
                    });
                db.SaveChanges();

                Console.ReadKey();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(blog);
                db.SaveChanges();
            }
        }
    }

    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("Server=rasp4;User Id=EF;Password=Navion151;Database=test_EF");
        }
    }

}
