using BlogProject.Data;
using BlogProject.Enums;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class DataService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManagedDataAsync()
        {
            //Task: Create the DB from the Migration 
            await _dbContext.Database.MigrateAsync();
            //Task 1: Seed a few Roles into the system
            await SeedRolesAsync();


            //Task 2: Seed a few Users into the system 
            await SeedUsersAsync();
        }


        private async Task SeedRolesAsync()
        {
            //If there are already Roles in the system, do nothing 
            if (_dbContext.Roles.Any())
            {
                return;
            }

            //otherwise, we want to create a few roles 
            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                //I need to use the role manager to create Roles 
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            //If there are already any Users in the system, do nothing
            if (_dbContext.Users.Any())
            {
                return;
            }

            //otherwise, introduce a user 
            //Create a new instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "mbergy27@gmail.com",
                UserName = "mbergy27@gmail.com",
                FirstName = "Michelle",
                LastName = "Joseph",
                PhoneNumber = "(800) 555-1212",
                EmailConfirmed = true
            };

            //Use the UserManager to create a new user that is defined by the adminUser variable 
            await _userManager.CreateAsync(adminUser, "Abc&123!");

            //add the new user to the Administrator role 
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


            //Create a Moderator user 
            var modUser = new BlogUser()
            {
                Email = "abloguser27@gmail.com",
                UserName = "abloguser27@gmail.com",
                FirstName = "Amanda",
                LastName = "Peet",
                PhoneNumber = "(800) 555-1213",
                EmailConfirmed = true
            };

            //use the blog manager to create a new moderator 
            await _userManager.CreateAsync(modUser, "Abc&123!");

            //add the new moderate to the moderator role 
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }



    }
}
