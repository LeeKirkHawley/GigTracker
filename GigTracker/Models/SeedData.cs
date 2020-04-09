using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using GigTracker.Models;
using System;
using System.Linq;
using GigTracker.Data;

namespace GigTracker.Models {
    public static class SeedData {

        //AccountService _accountService;

        //public SeedData(AccountService accountService) {
        //    _accountService = accountService;
        //}

        public static void EnsurePopulated(IApplicationBuilder app, ApplicationDbContext context, IAccountService accountService) {

            //context.Database.Migrate();
                
            // Look for any movies.
            if (context.User.Any()) {
                return;   // DB has been seeded
            }

            context.User.AddRange(
                new User {
                    UserName = "kirkhawley",
                    Password = accountService.HashPwd("password"),
                    FirstName = "Kirk",
                    LastName = "Hawley",
                    Email = "leekirkhawley@gmail.com"
                },
                new User {
                    UserName = "leon",
                    Password = accountService.HashPwd("password"),
                    FirstName = "Leon",
                    LastName = "Redbone",
                    Email = "leon@somedomain.com"
                }
            );

            context.SaveChanges();
        }
    }
}