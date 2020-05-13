using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using GigTracker.Models;
using System;
using System.Linq;
using GigTracker.Repositories;
using GigTracker.Services;
using GigTracker.Entities;

namespace GigTracker.Models {
    public static class SeedData {

        public static void EnsurePopulated(IApplicationBuilder app, ApplicationDbContext context, IAccountService accountService) {

            //context.Database.Migrate();
                
            // Look for any movies.
            if (context.User.Any() == false) {
                context.User.AddRange(
                    new User {
                        UserName = "kirkhawley",
                        Password = accountService.HashPwd("password"),
                        FirstName = "Kirk",
                        LastName = "Hawley",
                        Email = "leekirkhawley@gmail.com",
                        Role = Role.Admin
                    },
                    new User {
                        UserName = "leon",
                        Password = accountService.HashPwd("password"),
                        FirstName = "Leon",
                        LastName = "Redbone",
                        Email = "leon@somedomain.com",
                        Role = Role.User
                    }
                );

                context.SaveChanges();
            }

            if(context.Gig.Any() == false) {
                context.Gig.AddRange(
                    new Gig {
                        UserId = 1,
                        VenueName = "Rhythm Room",
                        VenueAddress = "somewhere",
                        VenuePhone = "666-66-6666",
                        Date = DateTime.Today,
                        ArtistName = "Hub Capp and the Wheels"
                    },
                    new Gig {
                        UserId = 2,
                        VenueName = "Ripps",
                        VenueAddress = "somewhere else",
                        VenuePhone = "222-222-2222",
                        Date = DateTime.Today,
                        ArtistName = "Stark Naked and the Car Thieves"
                    }
                );

                context.SaveChanges();

            }
        }
    }
}