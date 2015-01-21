namespace WebApplication5.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;
    using WebApplication5.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication5.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication5.Models.ApplicationDbContext context)
        {

            //base.Seed(context);

            //var Role1 = new Position { PositionID = 1, PositionNumber = 1 };
            //var Role2 = new Position { PositionID = 2, PositionNumber = 2 };
            //var Role3 = new Position { PositionID = 3, PositionNumber = 3 };
            //var Role4 = new Position { PositionID = 4, PositionNumber = 4 };
            //var Role5 = new Position { PositionID = 5, PositionNumber = 5 };

            //context.Pozitions.Add(Role1);
            //context.Pozitions.Add(Role2);
            //context.Pozitions.Add(Role3);
            //context.Pozitions.Add(Role4);
            //context.Pozitions.Add(Role5);

            //IEnumerable<CultureInfo> regionalLanguages = CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(x => !x.EnglishName.Contains("("));

            //int i = 0;

            //foreach (CultureInfo c in regionalLanguages)
            //{
            //    i++;
            //    context.Languages.Add(new Language { LanguageID = i, Jezik = c.EnglishName });
            //}

            //context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
