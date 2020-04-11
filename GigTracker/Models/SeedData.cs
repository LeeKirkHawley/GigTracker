﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using GigTracker.Models;
using System;
using System.Linq;
using GigTracker.Data;
using GigTracker.Services;
using GigTracker.Entities;

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
                new GigTrackerUser {
                    UserName = "kirkhawley",
                    Password = accountService.HashPwd("password"),
                    FirstName = "Kirk",
                    LastName = "Hawley",
                    Email = "leekirkhawley@gmail.com",
                    Role = Role.Admin
                },
                new GigTrackerUser {
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
    }
}