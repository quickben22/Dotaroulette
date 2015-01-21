using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace WebApplication5.Controllers
{
    public class AlertController : Controller
    {
      
         private ApplicationDbContext db;


         public AlertController(ApplicationDbContext _db)
    {
        db = _db;
    }

        [Authorize]
        public ActionResult Index(int? id, int? v)
        {
            int currentUserId = LoggedInUserKey;
            if ((v.HasValue) && (v.Value == 1))
            {
                ViewBag.myAlerts = (from p in db.MemberAlerts
                                    where p.PUser.Id == currentUserId
                                    select p).ToList();
            }
            else
            {
                ViewBag.myAlerts = (from p in db.MemberAlerts
                                    where p.PUser.Id == currentUserId && p.Ishidden == 0
                                    select p).ToList();
            }

            return View();
        }

        [Authorize]
        public ActionResult Delete(int id, int? rUrl)
        {
            try
            {
                MemberAlert model = db.MemberAlerts.Find(id);
                db.MemberAlerts.Remove(model);
                db.SaveChanges();
                TempData["message2"] = "Requested alert is deleted now.";
            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to delete. Error: " + ex1.Message;
            }
            int rUrl2 = rUrl.HasValue ? rUrl.Value : 0;
            return RedirectToAction("Index", "Messages");
        }


        [Authorize]
        public ActionResult Hide(int id, int? rUrl)
        {
            try
            {
                MemberAlert model = db.MemberAlerts.Find(id);
                model.Ishidden = (model.Ishidden == 0) ? 1 : 0;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message2"] = "Requested alert is updated now.";
            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to toggle. Error: " + ex1.Message;

            }
            int rUrl2 = rUrl.HasValue ? rUrl.Value : 0;
            return RedirectToAction("Index", "Messages");
        }


        [Authorize]
        public ActionResult DisplayEnvelope()
        {
            // query the user photo then return the view
            int id = LoggedInUserKey;
            ViewBag.Envelope = getRequestsByUId(id).Count() + getAlertsByUId(id).Count();
            var user = db.UserProfileInfo.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                ViewBag.trazi = user.Search;
                ViewBag.slika = user.avatar_url;
                ViewBag.name = user.UserName;
            }
            return PartialView("_DisplayPhoto");
        }

        private IList<MemberAlert> getAlertsByUId(int uId)
        {
            IEnumerable<MemberAlert> lstAlerts = from p in db.MemberAlerts
                                                 where p.PUser.Id == uId && p.Ishidden == 0
                                                 select p;
            return (lstAlerts.ToList());
        }

        private IList<MemberRequests> getRequestsByUId(int uId)
        {
            IEnumerable<MemberRequests> myRequests = from p in db.MemberRequestsSet.Include("Recipientid").Include("Senderid")
                                                     where (p.Senderid.Id == uId && p.Status != 0 && p.Type != 2) || p.Recipientid.Id == uId
                                                     select p;
            return myRequests.ToList();
        }


        private int LoggedInUserKey
        {
            //[DebuggerStepThrough]
            get
            {
                string currentUserId = User.Identity.GetUserId();
                var userProfile = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                if (userProfile!=null)
                return userProfile.UserProfileInfo.Id;

                return 0;
            }

        }
    }
}