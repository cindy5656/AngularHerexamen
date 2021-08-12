using AngularProjectAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class DBInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            // Look for any user.
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            context.Roles.AddRange(
              new Role { Name = "Gebruiker" },
              new Role { Name = "Superadmin" },
              new Role { Name = "Beheerder" },
              new Role { Name = "Werknemer" },
              new Role { Name = "Groepsbeheerder" },
              new Role { Name = "Groepslid" });
            context.SaveChanges();

            context.Users.AddRange(
                new User {  RoleID = 1, Username = "Cindy", Password = "test", FirstName = "Cindy", LastName = "Knapen", Email = "r0767301@thomasmore.be" }
                );
            context.SaveChanges();

            context.Groups.AddRange(
                new Group {  NameGroup = "IT", FotoURL=null, Theme = null }
                );
            context.SaveChanges();
            context.Companies.AddRange(
                new Company {  CompanyManagerID = 1, Description = "Labo", NameCompany = "Lavetan", Location = "Turnhout" }
                );
            context.CompanyUserGroup.AddRange(
                new CompanyUserGroup { CompanyID = 1, UserID = 1, RoleID = 2, GroupID = 1 }
                );

            /*context.Articles.AddRange(
                new Article { UserID = 1, Title = "Messi verlaat FC Barçelona", SubTitle = "Messi stuurde een fax met de boodschap dat hij wilt vertrekken.", ArticleStatusID = 1, TagID = 1, Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consequat non justo dignissim varius. Morbi finibus magna non neque bibendum efficitur. Aliquam eu auctor sem, ut mollis erat. Donec ornare dolor ex, tincidunt blandit purus sodales id. Phasellus a hendrerit libero. Nunc eu ultrices libero. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer consequat egestas dui sit amet dignissim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In sit amet cursus elit, eu dignissim elit. Ut aliquam cursus urna ultricies rhoncus. Proin vitae neque erat. Sed mollis consectetur diam eget vestibulum." }
                );*/

            context.SaveChanges();
        }
    }
}
