using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using WebApplication5.Models.ViewModels;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using WebApplication5.Models.Helpers;

namespace WebApplication5.Controllers
{
    public class UserProfileInfoController : Controller
    {
        //
        // GET: /UserProfileInfo/
        private ApplicationDbContext db;


        public UserProfileInfoController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public ActionResult Index(int? id)
        {
            UserProfileInfo u = db.UserProfileInfo.FirstOrDefault(x => x.Id == id);
            UserProfileEditViewModel model = null;
            if (u != null)
                model = u.ToViewModel(db.Pozitions.ToList(), db.Languages.ToList());
            return View(model);
        }
        [Authorize]
        public ActionResult Edit()
        {
            int currentUserId = LoggedInUserKey;
            var userProfile = db.UserProfileInfo.Find(currentUserId);
            UserProfileEditViewModel model = new UserProfileEditViewModel();
            model = userProfile.ToViewModel(db.Pozitions.ToList(), db.Languages.ToList());

            TempData["Message"] = null;

            if (db.MemberGroupmembersSet.FirstOrDefault(x => x.MemberDetails.Id == userProfile.Id) != null)
            {
                TempData["Message"] = "You can't change your profile data while in a team!";
            }
            else if (userProfile.Search == 1)
            {
                TempData["Message"] = "You can't change your profile data while searching!";
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserProfileEditViewModel userProfileViewModel)
        {

            if (ModelState.IsValid)
            {
                int currentUserId = LoggedInUserKey;
                var originalUserProfile = db.UserProfileInfo.Find(currentUserId);
                AddOrUpdateKeepExistingPositions(originalUserProfile, userProfileViewModel.Positions);
                AddOrUpdateKeepExistingLanguages(originalUserProfile, userProfileViewModel.Languages);

                db.Entry(originalUserProfile).CurrentValues.SetValues(userProfileViewModel);
                db.SaveChanges();

                return RedirectToAction("Index", "UserProfileInfo", new { id = currentUserId });
            }

            List<Language> Languagess = db.Languages.ToList();
            userProfileViewModel.LangPopis = new MultiSelectList(Languagess, "LanguageID", "Jezik", userProfileViewModel.Languages.ToList());

            ICollection<AssignedPositionData> allPositions = new List<AssignedPositionData>();
            int i = 1;
            foreach (AssignedPositionData c in userProfileViewModel.Positions)
            {
                allPositions.Add(new AssignedPositionData
                {
                    PositionID = i,
                    PositionNumber = i,
                    Assigned = c.Assigned
                });
                i++;
            }
            userProfileViewModel.Positions = allPositions;
            return View(userProfileViewModel);
        }


        public ActionResult ListUsers(int page = 1)
        {
            ViewBag.logiran = false;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.logiran = true;
            }

            ListUsersViewModel model = new ListUsersViewModel();
            model.users = db.UserProfileInfo.OrderBy(r => r.UserName).ToPagedList(page, 30);
            model.MaxRating = 6000;
            model.MaxAge = 7;
            model.MaxTime = 58;
            model.Search = true;
            ViewBag.stranica = page;
            ViewBag.jezici = db.Languages.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Listpart", model);
            }



            return View(model);
        }

        [HttpPost]
        public ActionResult ListUsers(ListUsersViewModel model)
        {

            int search = 0;
            ViewBag.logiran = true;
            if (model.Search) search = 1;
            var users = db.UserProfileInfo.Include("Positions").Include("Languages").Where(x => x.Search == search && x.Rating >= model.MinRating && x.Rating <= x.MaxRating && x.MinTime >= model.MinTime && x.MinTime <= model.MaxTime && x.Goal == model.Goal && x.Age >= model.MinAge && x.Age <= x.MaxAge).OrderBy(r => r.UserName);
            List<UserProfileInfo> profili = new List<UserProfileInfo>();

            foreach (UserProfileInfo u in users)
            {
                if (u.Positions.FirstOrDefault(x => x.PositionNumber == model.Position) != null && Trazenje.zone_sati(u.TimeZone) == Trazenje.zone_sati(model.TimeZone) && u.Languages.FirstOrDefault(x => x.LanguageID == model.Language)!=null)
                {
                    profili.Add(u);
                }
            }
            model.users = profili.ToPagedList(1, Math.Max(profili.Count,1));


            ViewBag.stranica = 1;
            ViewBag.jezici = db.Languages.ToList();
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("_Listpart", model);
            //}

            return View(model);
        }

        [Authorize]
        public ActionResult SimilarUsers()
        {

            int currentUserId = LoggedInUserKey;
            var userProfile = db.UserProfileInfo.Find(currentUserId);
            SimilarViewModel model = new SimilarViewModel();
            model.User = userProfile;
            model.search = userProfile.Search;
            if (userProfile.Search == 1)
            {
                string strpom = AppConstants.Sats.FirstOrDefault(x => x.SatID == userProfile.MaxTime).Opis;
                if (strpom.Contains("+")) model.UserTime = userProfile.MinTime + " - " + "6000";
                else
                    model.UserTime = userProfile.MinTime + " - " + strpom.Substring(strpom.IndexOf("-") + 1);
                model.UserGoal = AppConstants.Ciljs.FirstOrDefault(x => x.CiljID == userProfile.Goal).Opis;
                model.UserTimezone = AppConstants.timeZones().FirstOrDefault(x => x.TimeZID == userProfile.TimeZone).Opis;
                strpom = AppConstants.godine.FirstOrDefault(x => x.GodinaID == userProfile.MinAge).Opis;
                string strpom2 = AppConstants.godine.FirstOrDefault(x => x.GodinaID == userProfile.MaxAge).Opis;
                model.UserAge = strpom.Substring(0, strpom.IndexOf("-")) + " - " + strpom2.Substring(strpom2.IndexOf("-") + 1);
                strpom = AppConstants.MMRs.FirstOrDefault(x => x.MMRID == userProfile.MaxRating).Opis;
                if (strpom.Contains("+")) model.UserRating = userProfile.MinRating + " - " + "6000";
                else model.UserRating = userProfile.MinRating + " - " + strpom.Substring(strpom.IndexOf("-") + 1);


                model.Rating = (db.UserProfileInfo.Where(p => p.Rating >= userProfile.MinRating && userProfile.Rating >= p.MinRating && p.Rating <= userProfile.MaxRating && userProfile.Rating <= p.MaxRating && p.Search == 1)).Count();
                model.Age = db.UserProfileInfo.Where(p => p.Age >= userProfile.MinAge && userProfile.Age >= p.MinAge && p.Age <= userProfile.MaxAge && userProfile.Age <= p.MaxAge && p.Search == 1).Count();
                model.Time = db.UserProfileInfo.Where(p => (p.MinTime <= userProfile.MaxTime) && (userProfile.MinTime <= p.MaxTime) && p.Search == 1).Count();
                model.Goal = db.UserProfileInfo.Where(p => p.Goal == userProfile.Goal && p.Search == 1).Count() - 1;

                int pocetna_z = Trazenje.zone_sati(userProfile.TimeZone);
                var countTZ = -1;
                foreach (UserProfileInfo u in db.UserProfileInfo.Where(p => p.Search == 1))
                {
                    int pom = Trazenje.zone_sati(u.TimeZone);
                    if (pocetna_z == pom + 1 || pocetna_z == pom - 1 || pocetna_z == pom) { countTZ++; }

                }
                model.TimeZone = countTZ;

                IQueryable<UserProfileInfo> reza =
                from p in db.UserProfileInfo
                where (p.Rating >= userProfile.MinRating && userProfile.Rating >= p.MinRating && p.Rating <= userProfile.MaxRating && userProfile.Rating <= p.MaxRating) // ratinge podjednake 
                && (p.Age >= userProfile.MinAge && userProfile.Age >= p.MinAge && p.Age <= userProfile.MaxAge && userProfile.Age <= p.MaxAge)  // godine iste
                && (p.MinTime <= userProfile.MaxTime) && (userProfile.MinTime <= p.MaxTime)  // podjednako vremena moraju imat za igrat
                && (p.Goal == userProfile.Goal)  // isti cilj svi moraju imat
                && (p.Search == 1)
                && (p.Team == null) // nesmije imat tim
                select p;

                var countSVE = -1;
                foreach (UserProfileInfo u in reza)
                {
                    int pom = Trazenje.zone_sati(u.TimeZone);
                    if (pocetna_z == pom + 1 || pocetna_z == pom - 1 || pocetna_z == pom) { countSVE++; }
                }
                model.Sve = countSVE;

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SimilarUsers(SimilarViewModel model)
        {

            string strpom = AppConstants.Sats.FirstOrDefault(x => x.SatID == model.User.MaxTime).Opis;
            if (strpom.Contains("+")) model.UserTime = model.User.MinTime + " - " + "6000";
            else
                model.UserTime = model.User.MinTime + " - " + strpom.Substring(strpom.IndexOf("-") + 1);
            model.UserGoal = AppConstants.Ciljs.FirstOrDefault(x => x.CiljID == model.User.Goal).Opis;
            model.UserTimezone = AppConstants.timeZones().FirstOrDefault(x => x.TimeZID == model.User.TimeZone).Opis;
            strpom = AppConstants.godine.FirstOrDefault(x => x.GodinaID == model.User.MinAge).Opis;
            string strpom2 = AppConstants.godine.FirstOrDefault(x => x.GodinaID == model.User.MaxAge).Opis;
            model.UserAge = strpom.Substring(0, strpom.IndexOf("-")) + " - " + strpom2.Substring(strpom2.IndexOf("-") + 1);
            strpom = AppConstants.MMRs.FirstOrDefault(x => x.MMRID == model.User.MaxRating).Opis;
            if (strpom.Contains("+")) model.UserRating = model.User.MinRating + " - " + "6000";
            else model.UserRating = model.User.MinRating + " - " + strpom.Substring(strpom.IndexOf("-") + 1);


            model.Rating = (db.UserProfileInfo.Where(p => p.Rating >= model.User.MinRating && model.User.Rating >= p.MinRating && p.Rating <= model.User.MaxRating && model.User.Rating <= p.MaxRating && p.Search == 1)).Count();
            model.Age = db.UserProfileInfo.Where(p => p.Age >= model.User.MinAge && model.User.Age >= p.MinAge && p.Age <= model.User.MaxAge && model.User.Age <= p.MaxAge && p.Search == 1).Count();
            model.Time = db.UserProfileInfo.Where(p => (p.MinTime <= model.User.MaxTime) && (model.User.MinTime <= p.MaxTime) && p.Search == 1).Count();
            model.Goal = db.UserProfileInfo.Where(p => p.Goal == model.User.Goal && p.Search == 1).Count();

            int pocetna_z = Trazenje.zone_sati(model.User.TimeZone);
            var countTZ = -1;
            foreach (UserProfileInfo u in db.UserProfileInfo.Where(p => p.Search == 1))
            {
                int pom = Trazenje.zone_sati(u.TimeZone);
                if (pocetna_z == pom + 1 || pocetna_z == pom - 1 || pocetna_z == pom) { countTZ++; }

            }
            model.TimeZone = countTZ;

            IQueryable<UserProfileInfo> reza =
              from p in db.UserProfileInfo
              where (p.Rating >= model.User.MinRating && model.User.Rating >= p.MinRating && p.Rating <= model.User.MaxRating && model.User.Rating <= p.MaxRating) // ratinge podjednake 
              && (p.Age >= model.User.MinAge && model.User.Age >= p.MinAge && p.Age <= model.User.MaxAge && model.User.Age <= p.MaxAge)  // godine iste
              && (p.MinTime <= model.User.MaxTime) && (model.User.MinTime <= p.MaxTime)  // podjednako vremena moraju imat za igrat
              && (p.Goal == model.User.Goal)  // isti cilj svi moraju imat
              && (p.Search == 1)
              && (p.Team == null) // nesmije imat tim
              select p;

            var countSVE = -1;
            foreach (UserProfileInfo u in reza)
            {
                int pom = Trazenje.zone_sati(u.TimeZone);
                if (pocetna_z == pom + 1 || pocetna_z == pom - 1 || pocetna_z == pom) { countSVE++; }
            }
            model.Sve = countSVE;


            return View(model);
        }


        private void AddOrUpdateKeepExistingPositions(UserProfileInfo userProfile, IEnumerable<AssignedPositionData> assignedPositons)
        {
            if (assignedPositons != null && userProfile != null)
            {
                var webPositionAssignedIDs = assignedPositons.Where(c => c.Assigned).Select(webPositon => webPositon.PositionID);
                var dbPositionIDs = userProfile.Positions.Select(dbPosition => dbPosition.PositionID);
                var PositionIDs = dbPositionIDs as int[] ?? dbPositionIDs.ToArray();
                var positionsToDeleteIDs = PositionIDs.Where(x => !webPositionAssignedIDs.Contains(x)).ToList();

                // Delete removed courses
                foreach (var id in positionsToDeleteIDs)
                {
                    userProfile.Positions.Remove(db.Pozitions.Find(id));
                }

                // Add courses that user doesn't already have
                foreach (var id in webPositionAssignedIDs)
                {
                    if (!PositionIDs.Contains(id))
                    {
                        userProfile.Positions.Add(db.Pozitions.Find(id));
                    }
                }
            }
        }

        private void AddOrUpdateKeepExistingLanguages(UserProfileInfo userProfile, List<int> Languages)
        {
            if (Languages != null && userProfile != null)
            {

                //var webLanguageAssignedIDs = Languages;
                var dbLanguageIDs = userProfile.Languages.Select(dbLanguage => dbLanguage.LanguageID);
                var LanguageIDs = dbLanguageIDs as int[] ?? dbLanguageIDs.ToArray();
                var LanguagesToDeleteIDs = LanguageIDs.Where(id => !Languages.Contains(id)).ToList();

                // Delete removed courses
                foreach (var id in LanguagesToDeleteIDs)
                {
                    userProfile.Languages.Remove(db.Languages.Find(id));
                }

                // Add courses that user doesn't already have
                foreach (var id in Languages)
                {
                    if (!LanguageIDs.Contains(id))
                    {
                        userProfile.Languages.Add(db.Languages.Find(id));
                    }
                }
            }
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