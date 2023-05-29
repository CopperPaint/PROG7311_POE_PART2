using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PROG7311_PART2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace PROG7311_PART2
{
    /*This class is created to hard code the database data in event of database failure
      This is for prototype and demonstration purposes only
      Delete this file when no longer needed 
    */
    public class PopulateDatabase
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public void Populate()
        {
            CreateRoles();
        }


        #region Roles
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Method to create user roles
        /// </summary>
        private void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Create Admin Role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Create Default Admin - change password after creation
                var admin = new ApplicationUser();
                admin.UserName = "Admin";
                string AdminPass = "Password@123";
                var createAdmin = UserManager.Create(admin, AdminPass);
                if (createAdmin.Succeeded)
                {
                    UserManager.AddToRole(admin.Id, "Admin");
                }
            }

            //Create Employee Role
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }

            //Create Farmer Role
            if (!roleManager.RoleExists("Farmer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Farmer";
                roleManager.Create(role);

                CreateEmployees();
                CreateTypes();
                CreateFarmers();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Create Employees
        //---------------------------------------------------------------------------------------//
        private void CreateEmployees()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            //EMP1
            var EMP1 = new ApplicationUser();
            EMP1.UserName = "Employee1";
            EMP1.Email = "emp1@gmail.com";
            string password1 = "Password@123";
            var createEMP1 = UserManager.Create(EMP1, password1);
            if (createEMP1.Succeeded)
            {
                UserManager.AddToRole(EMP1.Id, "Employee");
            }

            //EMP2
            var EMP2 = new ApplicationUser();
            EMP2.UserName = "Employee2";
            EMP2.Email = "emp2@gmail.com";
            string password2 = "Password@123";
            var createEMP2 = UserManager.Create(EMP2, password2);
            if (createEMP2.Succeeded)
            {
                UserManager.AddToRole(EMP2.Id, "Employee");
            }

            //EMP3
            var EMP3 = new ApplicationUser();
            EMP3.UserName = "Employee3";
            EMP3.Email = "emp3@gmail.com";
            string password3 = "Password@123";
            var createEMP3 = UserManager.Create(EMP3, password3);
            if (createEMP3.Succeeded)
            {
                UserManager.AddToRole(EMP3.Id, "Employee");
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion



        #region Create Product Types
        //---------------------------------------------------------------------------------------//
        private void CreateTypes()
        {
            var milk = new ProductType
            {
                PTName = "Milk",
                PTDescription = "White liqud form cows",
                PTCategory = "Dairy"
            };
            context.ProductTypes.Add(milk);
            context.SaveChanges();

            var pototo = new ProductType
            {
                PTName = "Potato",
                PTDescription = "Round brownish veggatble",
                PTCategory = "Vegetable"
            };
            context.ProductTypes.Add(pototo);
            context.SaveChanges();

            var apple = new ProductType
            {
                PTName = "Apple",
                PTDescription = "Road red/green fruit",
                PTCategory = "Fruit"
            };
            context.ProductTypes.Add(apple);
            context.SaveChanges();

            var flour = new ProductType
            {
                PTName = "Flour",
                PTDescription = "White powder used for baking",
                PTCategory = "Baking"
            };
            context.ProductTypes.Add(flour);
            context.SaveChanges();

            var bread = new ProductType
            {
                PTName = "Bread",
                PTDescription = "Cooked loaf of dough",
                PTCategory = "Grains"
            };
            context.ProductTypes.Add(bread);
            context.SaveChanges();
        }
        //---------------------------------------------------------------------------------------//
        #endregion



        #region Create Farmers and Products
        //---------------------------------------------------------------------------------------//
        private void CreateFarmers()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Farmer 1
            var user1 = new ApplicationUser
            {
                UserName = "Farmer1",
                Email = "Farmer1@gmail.com"
            };
            string password1 = "Password@123";
            var createF1 = UserManager.Create(user1, password1);
            if (createF1.Succeeded)
            {
                UserManager.AddToRole(user1.Id, "Farmer");
            }
            var farmer1 = new Farmer()
            {
                UserID = user1.Id,
                FarName = "John",
                FarSurname = "Smith",
                FarAdress = "56th Mountain Road"
            };
            context.Farmers.Add(farmer1);
            var almondmilk = new Product()
            {
                FarmerID = 1,
                PName = "Almond milk",
                PDescription = "milk made form almond nuts",
                DateAdded = DateTime.Now,
                PTypeID = 1,
                Quantity = 20
            };
            context.Products.Add(almondmilk);
            context.SaveChanges();

            //Farmer 2
            var user2 = new ApplicationUser
            {
                UserName = "Farmer2",
                Email = "Farmer2@gmail.com"
            };
            string password2 = "Password@123";
            var createF2 = UserManager.Create(user2, password2);
            if (createF2.Succeeded)
            {
                UserManager.AddToRole(user2.Id, "Farmer");
            }
            var farmer2 = new Farmer()
            {
                UserID = user2.Id,
                FarName = "Robert",
                FarSurname = "Davis",
                FarAdress = "10th Seaside Road"
            };
            context.Farmers.Add(farmer2);
            var selfflour = new Product()
            {
                FarmerID = 2,
                PName = "Self-Rising Flour",
                PDescription = "flour with baking powder mixed in",
                DateAdded = DateTime.Now,
                PTypeID = 4,
                Quantity = 10
            };
            context.Products.Add(selfflour);
            context.SaveChanges();

            //Farmer 3
            var user3 = new ApplicationUser
            {
                UserName = "Farmer3",
                Email = "Farmer3@gmail.com"
            };
            string password3 = "Password@123";
            var createF3 = UserManager.Create(user3, password3);
            if (createF3.Succeeded)
            {
                UserManager.AddToRole(user3.Id, "Farmer");
            }
            var farmer3 = new Farmer()
            {
                UserID = user3.Id,
                FarName = "Jaccob",
                FarSurname = "Doe",
                FarAdress = "34th Creek Road"
            };
            context.Farmers.Add(farmer3);
            var redapples = new Product()
            {
                FarmerID = 3,
                PName = "Red Delicious",
                PDescription = "red apples",
                DateAdded = DateTime.Now,
                PTypeID = 3,
                Quantity = 30
            };
            context.Products.Add(redapples);
            context.SaveChanges();
        }
        //---------------------------------------------------------------------------------------//
        #endregion





    }
}