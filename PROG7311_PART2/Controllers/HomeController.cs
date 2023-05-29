using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROG7311_PART2.Controllers
{
    public class HomeController : Controller
    {
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
        /// Index action of home controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //check if user is signed in
            bool val = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (val)
            {
                //get user ID
                string userID = User.Identity.GetUserId();
                ViewBag.Welcome = "Welcome" + User.Identity.GetUserName() + "!";
                //if user role is farmer
                if (UserManager.IsInRole(userID, "Farmer"))
                {
                    return RedirectToAction("FarmerHome");
                }
                else
                {
                    return RedirectToAction("EmployeeHome");
                }
            }
            else
            { 
                return View();
            }
        }
        /// <summary>
        /// Action for employee home page
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployeeHome()
        {
            return View();
        }
        /// <summary>
        /// action for farmer home page
        /// </summary>
        /// <returns></returns>
        public ActionResult FarmerHome()
        {
            return View();
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region About
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// action for about page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//