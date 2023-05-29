using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PROG7311_PART2.Models;

namespace PROG7311_PART2.Controllers
{
    public class ProductTypesController : Controller
    {
        //Database context
        private ApplicationDbContext db = new ApplicationDbContext();

        #region User Manager
        //---------------------------------------------------------------------------------------//
        //Usermanger for controller
        private ApplicationUserManager _userManager;
        /// <summary>
        /// method to get user manager
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
        /// GET: ProductTypes - default view
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            //get user
            string userID = User.Identity.GetUserId();
            //if user is farmer
            if (UserManager.IsInRole(userID, "Farmer"))
            {
                return RedirectToAction("FarmerIndex");
            }
            else
            { 
                return View(db.ProductTypes.ToList());
            } 
        }
        /// <summary>
        /// GET: ProductTypes - farmer view
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult FarmerIndex()
        {
            return View(db.ProductTypes.ToList());
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Details
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: ProductTypes Details
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Create
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: ProductTypes Create
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// POST: ProductTypes Create
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PTypeID,PTName,PTDescription,PTCategory")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productType);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Edit
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: ProductTypes Edit
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }
        /// <summary>
        /// POST: ProductTypes Edit
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PTypeID,PTName,PTDescription,PTCategory")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productType);
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Delete
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// GET: ProductTypes Delete
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }
        /// <summary>
        /// POST: ProductTypes Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //get product type
            ProductType productType = db.ProductTypes.Find(id);
            //get products
            List<Product> products = db.Products.ToList();
            //remove all products of type
            foreach (var product in products)
            {
                if (product.PTypeID == productType.PTypeID)
                { 
                    db.Products.Remove(product);
                }
            }
            db.ProductTypes.Remove(productType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Dispose
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to for disposing of database connection
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
