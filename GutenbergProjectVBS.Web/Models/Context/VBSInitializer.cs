using System.Data.Entity;

namespace GutenbergProjectVBS.Web.Models.Context
{
    public class VBSInitializer : CreateDatabaseIfNotExists<VBSContext>
    {
        protected override void Seed(VBSContext context)
        {
            User user = new User()
            {
                Name = "System",
                Surname = "Administrator",
                Email = "gutenbergvbs@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("1357911"),
                IsAdmin = true,
                CreatedAt = System.DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}