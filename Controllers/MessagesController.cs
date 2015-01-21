using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WebApplication5.Models;
using WebApplication5.Models.ViewModels;

namespace WebApplication5.Controllers
{
    public class MessagesController : Controller
    {
          private ApplicationDbContext db;


          public MessagesController(ApplicationDbContext _db)
    {
        db = _db;
    }
        //
        // GET: /Messages/
        [Authorize]
        public ActionResult Index()
        {
            MessagesViewModel model = new MessagesViewModel();
            int id = LoggedInUserKey;
            ViewBag.Userident = id;
            model.myAlerts = getAlertsByUId(id);
            model.myRequests = getRequestsByUId(id);
            return View(model);
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
                                                     where (p.Senderid.Id == uId && p.Status != 0) || p.Recipientid.Id == uId
                                                     select p;
            return myRequests.ToList();
        }

        private int LoggedInUserKey
        {
            //[DebuggerStepThrough]
            get
            {
                string currentUserId = User.Identity.GetUserId();
                var userProfile = db.Users.FirstOrDefault(x => x.Id == currentUserId).UserProfileInfo;
                return userProfile.Id;
            }

        }
    }
}




