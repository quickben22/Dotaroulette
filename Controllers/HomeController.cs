using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using Microsoft.AspNet.Identity;
using WebApplication5.Models.ViewModels;
using WebApplication5.Models.Helpers;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {

     



         private ApplicationDbContext db;


         public HomeController(ApplicationDbContext _db)
    {
        db = _db;
    }


        public ActionResult Index()
        {

            ViewBag.imagrupu = false;
            ViewBag.logiran = false;
       
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.logiran = true;





                    string currentUserId = User.Identity.GetUserId();
                    var userProfile = db.Users.FirstOrDefault(x => x.Id == currentUserId).UserProfileInfo;
                    var member = db.MemberGroupmembersSet.Include("MemberDetails").FirstOrDefault(x => x.MemberDetails.Id == userProfile.Id);
                    if (member != null)
                        ViewBag.imagrupu = true;
                    var group = db.MemberGroupsSet.FirstOrDefault(x => x.OwnerId == userProfile.Id);
                    ViewBag.owner = false;
                    ViewBag.trazi = false;
                    if (userProfile.Search == 1) ViewBag.trazi = true;

                    if (group != null)
                    {
                        int hlp=db.MemberGroupmembersSet.Where(x => x.Group.Id == group.Id).Count();
                        if ( hlp<5 && hlp>1)
                            ViewBag.owner = true;
                    }

                }
                IEnumerable<UserProfileInfo> korisnici = db.Set<UserProfileInfo>().Include("Positions").Include("Team");

                int[] pozicija = new int[5];
                int timova = 0;
                int upotrazi = 0;
                foreach (UserProfileInfo k in korisnici)
                {
                    if (k.Team != null) timova++;
                    else if (k.Search == 1)
                    {
                        upotrazi++;
                        foreach (Position p in k.Positions)
                        {
                            if (p.PositionNumber != 0)
                                pozicija[p.PositionNumber - 1] += 1;
                        }
                    }

                }
                ViewBag.pozicija = pozicija;
                ViewBag.timova = timova / 5;
                ViewBag.korisnika = korisnici.Count();
                ViewBag.upotrazi = upotrazi;
            
            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "A little bit about this page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact for suggestions, questions, etc.";

            return View();
        }

        public ActionResult Questions()
        {
          

            return View();
        }


        //[Authorize]
        //public ActionResult Statistic()
        //{
        //    IEnumerable<UserProfileInfo> korisnici = db.Set<UserProfileInfo>().Include("Positions").Include("Languages").Include("Team");

        //    StatisticsViewModel model = new StatisticsViewModel();
        //    model.Positions = new Dictionary<int, int>();
        //    model.Languages = new Dictionary<string, int>();
        //    model.MinRating = new Dictionary<string, int>();
        //    model.Rating = new Dictionary<string, int>();
        //    model.MaxRating = new Dictionary<string, int>();
        //    foreach (UserProfileInfo u in korisnici)
        //    {
        //        if (u.Search == 1)
        //        {
        //            foreach (Position p in u.Positions)
        //            {
        //                if (model.Positions.ContainsKey(p.PositionNumber)) model.Positions[p.PositionNumber] += 1;
        //                else model.Positions.Add(p.PositionNumber, 1);
        //            }
        //            foreach (Language l in u.Languages)
        //            {
        //                if (model.Languages.ContainsKey(l.Jezik)) model.Languages[l.Jezik] += 1;
        //                else model.Languages.Add(l.Jezik, 1);
        //            }
                    
        //            string MMRmin=AppConstants.MMRs.FirstOrDefault(x => x.MMRID == u.MinRating).Opis;
        //            if (model.MinRating.ContainsKey(MMRmin)) model.MinRating[MMRmin] += 1;
        //            else model.MinRating.Add(MMRmin, 1);

        //            string MMR = AppConstants.MMRs.FirstOrDefault(x => x.MMRID == u.Rating).Opis;
        //            if (model.Rating.ContainsKey(MMR)) model.Rating[MMR] += 1;
        //            else model.Rating.Add(MMR, 1);

        //            string MMRmax = AppConstants.MMRs.FirstOrDefault(x => x.MMRID == u.MaxRating).Opis;
        //            if (model.MaxRating.ContainsKey(MMRmax)) model.MaxRating[MMRmax] += 1;
        //            else model.MaxRating.Add(MMRmax, 1);

        //        }
        //    }
 

        //    return View(model);
        //}

        //public Dictionary<string, int> MinRating { get; set; }
        //public Dictionary<string, int> Rating { get; set; }
        //public Dictionary<string, int> MaxRating { get; set; }
        //public Dictionary<int, int> Positions { get; set; }
        //public Dictionary<int, int> FavPosition { get; set; }
        //public Dictionary<string, int> TimeZone { get; set; }
        //public Dictionary<string, int> Languages { get; set; }
        //public Dictionary<string, int> MinTime { get; set; }
        //public Dictionary<string, int> MaxTime { get; set; }
        //public Dictionary<string, int> Microphone { get; set; }
        //public Dictionary<string, int> Goal { get; set; }
        //public Dictionary<string, int> MinAge { get; set; }
        //public Dictionary<string, int> Age { get; set; }
        //public Dictionary<string, int> MaxAge { get; set; }

    }
}