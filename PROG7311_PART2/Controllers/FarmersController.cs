using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PROG7311_PART2.Models;

namespace PROG7311_PART2.Controllers
{
    public class FarmersController : Controller
    {
        //database context
        private ApplicationDbContext db = new ApplicationDbContext();
        //user manager
        private ApplicationUserManager _userManager;

        #region INNIT
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Blank constructor
        /// </summary>
        public FarmersController()
        {

        }
        /// <summary>
        /// user manager constructor
        /// </summary>
        /// <param name="userManager"></param>
        public FarmersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        /// <summary>
        /// get user manager
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Index
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Farmers
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            //get user
            string userID = User.Identity.GetUserId();
            //if user role is admin or employee
            if (UserManager.IsInRole(userID, "Admin") || UserManager.IsInRole(userID, "Employee"))
            {
                List<FarmerViewModel> farmerViews = new List<FarmerViewModel>();
                //get farmers
                var farmerlist = db.Farmers.ToList();
                foreach (var farmer in farmerlist)
                {
                    var user = UserManager.FindById(farmer.UserID);
                    farmerViews.Add(new FarmerViewModel
                    {
                        FarmerID = farmer.FarmerID,
                        Username = user.UserName,
                        Email = user.Email
                    });
                }
                return View(farmerViews);
            }
            else
            {
                //return to home page
                return RedirectToAction("Index", "Home", new { area = "" }); ;
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Details
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Farmers Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = db.Farmers.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            else
            {
                //get farmer user
                var user = UserManager.FindById(farmer.UserID);
                FarmerDetailModel model = new FarmerDetailModel 
                {
                    FarmerID = farmer.FarmerID,
                    Username = user.UserName,
                    Email = user.Email,
                    Firstname = farmer.FarName,
                    Surname = farmer.FarSurname,
                    Address = farmer.FarAdress
                };
                return View(model);
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Create
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Farmers Create
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        // POST: Farmers Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateFarmerViewModel model)
        {
            if (ModelState.IsValid)
            {
                //create user
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                };
                //add user to database
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //add farmer role
                    UserManager.AddToRole(user.Id, "Farmer");
                    //create farmer
                    var farmer = new Farmer()
                    {
                        UserID = user.Id,
                        FarName = model.Name,
                        FarSurname = model.Surname,
                        FarAdress = model.Address
                    };
                    //save farmer to database
                    db.Farmers.Add(farmer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Edit
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Farmers Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = db.Farmers.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }
        /// <summary>
        /// POST: Farmers Edit
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(farmer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(farmer);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Delete
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Farmers Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = db.Farmers.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }
        /// <summary>
        /// POST: Farmers Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //get farmer
            Farmer farmer = db.Farmers.Find(id);
            //get user
            var user = UserManager.FindById(farmer.UserID);
            //remove farmer products
            List<Product> products = new List<Product>();
            foreach (var product in products)
            {
                if (product.FarmerID == farmer.FarmerID)
                { 
                    db.Products.Remove(product);
                }
            }
            //delete user
            UserManager.Delete(user);
            //delete farmer
            db.Farmers.Remove(farmer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Dispose
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to dispose of db connection
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//