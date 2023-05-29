using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PROG7311_PART2.Models;

namespace PROG7311_PART2.Controllers
{
    public class ProductsController : Controller
    {
        //database context
        private ApplicationDbContext db = new ApplicationDbContext();
        //user manager
        private ApplicationUserManager _userManager;

        #region INNIT
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// blank constructor
        /// </summary>
        public ProductsController()
        {
            
        } 
        /// <summary>
        /// constuctor with user manager
        /// </summary>
        /// <param name="userManager"></param>
        public ProductsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        /// <summary>
        /// Usermanager
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
        /// GET: Products
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            //get userid
            string userID = User.Identity.GetUserId();
            //if user role is farmer
            if (UserManager.IsInRole(userID, "Farmer"))
            {
                return RedirectToAction("FarmerIndex");
            }
            else
            { 
                return RedirectToAction("EmployeeIndex");
            }
        }
        /// <summary>
        /// GET: Employee Index
        /// </summary>
        /// <param name="SortOrder"></param>
        /// <param name="SelectedFarmer"></param>
        /// <returns></returns>
        public ActionResult EmployeeIndex(string SortOrder, string SelectedFarmer)
        {
            //Filter order
            ViewBag.DateAdded = String.IsNullOrEmpty(SortOrder) ? "DateAdded_desc" : "";
            ViewBag.Type = SortOrder == "Type" ? "Type_desc" : "Type";

            //create list of farmer users
            List<FarmerUsers> farmerUsers = new List<FarmerUsers>();
            var farmers = db.Farmers.ToList();
            foreach (var farmer in farmers)
            {
                var user = UserManager.FindById(farmer.UserID);
                farmerUsers.Add(new FarmerUsers 
                { 
                    UserID = user.Id,
                    FarmerID = farmer.FarmerID,
                    Username = user.UserName
                });
            }
            //select list of farmer users
            ViewBag.Farmers = farmerUsers.Select(f => new SelectListItem
            {
                Value = f.FarmerID.ToString(),
                Text = f.Username
            });

            //selected farmer
            ViewBag.SelectedFarmer = SelectedFarmer;
            var products = db.Products.ToList();
            if (!String.IsNullOrEmpty(SelectedFarmer))
            {
                int farmID = int.Parse(SelectedFarmer);
                products = db.Products.Where(p => p.FarmerID == farmID).ToList();
            }

            //create product view models
            List<EmpProductIndexModel> models = new List<EmpProductIndexModel>();
            foreach (var product in products)
            {
                ProductType type = db.ProductTypes.Find(product.PTypeID);
                models.Add(new EmpProductIndexModel 
                { 
                    ProductID = product.ProductID,
                    PName = product.PName,
                    Type = type.PTName,
                    DateAdded = product.DateAdded,
                    Quantity = product.Quantity
                });
            }

            ViewBag.CurrentFilter = "Date Added ▲";
            //sort product view models according to filter
            switch (SortOrder)
            {
                case "DateAdded_desc":
                    models = models.OrderByDescending(n => n.DateAdded).ToList();
                    ViewBag.CurrentFilter = "Date Added ▼";
                    break;
                case "Type":
                    models = models.OrderBy(n => n.Type).ToList();
                    ViewBag.CurrentFilter = "Type ▲";
                    break;
                case "Type_desc":
                    models = models.OrderByDescending(n => n.Type).ToList();
                    ViewBag.CurrentFilter = "Type ▼";
                    break;
                default:
                    models = models.OrderBy(n => n.DateAdded).ToList();
                    ViewBag.CurrentFilter = "Date Added ▲";
                    break;
            }
            return View(models);
        }
        /// <summary>
        /// GET: Farmer Product Index
        /// </summary>
        /// <returns></returns>
        public ActionResult FarmerIndex()
        {
            //get user
            string userID = User.Identity.GetUserId();
            //get farmer
            var farmers = db.Farmers.Where(f => f.UserID == userID).ToList();
            Farmer farmer = farmers[0];
            //get farmer products
            return View(db.Products.Where(p => p.FarmerID == farmer.FarmerID));
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Details
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Products Details
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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                int farmerID = product.FarmerID;
                Farmer farmer = db.Farmers.Find(farmerID);
                var user = UserManager.FindById(farmer.UserID);
                ProductType type = db.ProductTypes.Find(product.PTypeID);
                ProductDetailsModel detailsModel = new ProductDetailsModel 
                {
                    ProductID = product.ProductID,
                    Username = user.UserName,
                    PName = product.PName,
                    Description = product.PDescription,
                    Type = type.PTName,
                    DateAdded = product.DateAdded,
                    Quantity = product.Quantity
                };
                return View(detailsModel);
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Create
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Products Create
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create()
        {
            string userID = User.Identity.GetUserId();
            if (UserManager.IsInRole(userID, "Farmer"))
            {
                IEnumerable<ProductType> types = db.ProductTypes.ToList();
                ViewBag.TypeList = new SelectList(types, "PTypeID", "PTName");
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// POST: Products Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                var farmers = db.Farmers.Where(f => f.UserID == userID).ToList();
                Farmer farmer = farmers[0];
                Product product = new Product
                {
                    FarmerID = farmer.FarmerID,
                    PTypeID = model.TypeID,
                    PName = model.Name,
                    PDescription = model.Description,
                    DateAdded = model.DateAdded,
                    Quantity = model.Quantity
                };
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Edit
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Products Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int? id)
        {
            string userID = User.Identity.GetUserId();
            if (UserManager.IsInRole(userID, "Farmer"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    IEnumerable<ProductType> types = db.ProductTypes.ToList();
                    ViewBag.TypeList = new SelectList(types, "PTypeID", "PTName");
                    return View(product);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// POST: Products Edit
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,FarmerID,PName,PDescription,PTypeID,DateAdded,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Delete
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: Products Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // 
        public ActionResult Delete(int? id)
        {
            string userID = User.Identity.GetUserId();
            if (UserManager.IsInRole(userID, "Farmer"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    ProductType type = db.ProductTypes.Find(product.PTypeID);
                    ProductDetailsModel detailsModel = new ProductDetailsModel
                    {
                        ProductID = product.ProductID,
                        PName = product.PName,
                        Description = product.PDescription,
                        Type = type.PTName,
                        DateAdded = product.DateAdded,
                        Quantity = product.Quantity
                    };
                    return View(detailsModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// POST: Products Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Dispose
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Method to dispose of DB Connection
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
