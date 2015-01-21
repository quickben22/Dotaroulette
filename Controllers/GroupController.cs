using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using PagedList;
namespace WebApplication5.Controllers
{
    public class GroupController : Controller
    {
      
          private ApplicationDbContext db;


          public GroupController(ApplicationDbContext _db)
    {
        db = _db;
    }

        //
        // GET: /Group/
        [Authorize]
        public ActionResult Index()
        {
            int currentUserId = LoggedInUserKey;
            Models.GroupHomeModel model = new Models.GroupHomeModel();
            model.myGroup = db.MemberGroupsSet.Include("FKGrpMmbrsGrp").FirstOrDefault(p => p.FKGrpMmbrsGrp.FirstOrDefault(x => x.MemberDetails.Id == currentUserId) != null);

            if (model.myGroup != null)
            {
                IEnumerable<MemberGroupmembers> membri = db.MemberGroupmembersSet.Include("MemberDetails").Include("Vote").Where(p => p.Group.Id == model.myGroup.Id);
                if (membri.Any())
                {
                    model.myMembers = membri.ToList();
                    model.member = model.myMembers.FirstOrDefault(x => x.MemberDetails.Id == currentUserId);
                    //Dictionary<int,int> votes = new Dictionary<int,int>();
                    //foreach (MemberGroupmembers m in model.myMembers)
                    //{
                    //    if (votes.ContainsKey(m.Vote)) votes[m.Vote] += 1;
                    //    else if(m.Vote!=0) votes.Add(m.Vote, 1);
                    //    if (!votes.ContainsKey(m.Id)) votes.Add(m.Id, 0);
                    //}
                    //model.votes = votes;
                }
            }
            return View(model);
        }

        // GET: /Group/Create
        [Authorize]
        public ActionResult Create()
        {
            int currentUserId = LoggedInUserKey;
            ViewBag.naslov = "You can't create a team!";
            var originalUserProfile = db.UserProfileInfo.Find(currentUserId);
            TempData["message"] = Models.Helpers.Trazenje.moze_trazit(new List<UserProfileInfo> { originalUserProfile }, db); // provjera da li je unjeo sve podatke
            if (((List<string>)TempData["message"]).Count > 0) return (View("RequestStatus"));
            TempData["message"] = null;

           if( db.MemberGroupmembersSet.FirstOrDefault(x=>x.MemberDetails.Id==currentUserId)!=null)
            {
            TempData["message"]="You are already in a group!";
                return (View("RequestStatus"));
            }
            
            MemberGroups model = new MemberGroups();
            model.OwnerId = currentUserId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MemberGroups model)
        {
            try
            {

                // TODO: Add insert logic here
                model.Broj_clanova = 1;
                if (model.Name == null)
                {
                    string[] pom = Models.Helpers.Trazenje.generiraj(); // generiraj random ime ekipe
                    model.Name = pom[0];
                    model.Url = pom[1];
                }
                model.Broj_clanova = 1;
                db.MemberGroupsSet.Add(model);
                MemberGroupmembers member = new MemberGroupmembers();
                string currentUserId = User.Identity.GetUserId();
                member.MemberDetails = db.Users.FirstOrDefault(x => x.Id == currentUserId).UserProfileInfo;
                member.Group = model;
                db.MemberGroupmembersSet.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            Models.GroupDetailsModel model = new Models.GroupDetailsModel();
            model.groupDetails = db.MemberGroupsSet.FirstOrDefault(p => p.Id == id);
            IEnumerable<MemberGroupmembers> membri = db.MemberGroupmembersSet.Include("MemberDetails").Where(p => p.Group.Id == id);
            if (membri.Any())
                model.groupMembers = membri.ToList();
            return View(model);
        }

        public ActionResult Update(int id)
        {
            MemberGroups itm = db.MemberGroupsSet.FirstOrDefault(p => p.Id == id);
            if (itm.OwnerId != LoggedInUserKey)
            {
                TempData["message"] = "You do not have Edit access to the requested team. Activity Logged.";
                return RedirectToAction("Index");
            }


            Models.GroupHomeModel model = new Models.GroupHomeModel();
            model.myGroup = itm;
            ViewBag.userid = LoggedInUserKey;
            model.myMembers = db.MemberGroupmembersSet.Include("MemberDetails").Where(p => p.Group.Id == model.myGroup.Id).ToList();

            return View(model);
        }

        //
        // POST: /Group/Edit/5
        [HttpPost]
        public ActionResult Update(GroupHomeModel model)
        {
            try
            {
                // TODO: Add update logic here
                MemberGroups itm = db.MemberGroupsSet.FirstOrDefault(p => p.Id == model.myGroup.Id);

                if (itm.OwnerId != LoggedInUserKey)
                {
                    TempData["message"] = "You do not have Edit access to the requested team. Activity Logged.";

                    return RedirectToAction("Index");
                }
                itm.OwnerId = LoggedInUserKey;
                itm.Name = model.myGroup.Name;
                itm.Url = model.myGroup.Url;
                itm.About = model.myGroup.About;
                db.Entry(itm).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message2"] = "The requested team is updated now.";
            }
            catch (Exception excp)
            {
                TempData["message"] = "Error Updating the team: " + excp.Message + excp.StackTrace;
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Find(string givenUsername, GroupHomeModel model)
        {
            int currentUserId = LoggedInUserKey;







            MemberGroups itm = db.MemberGroupsSet.FirstOrDefault(p => p.Id == model.myGroup.Id);
            if (itm.OwnerId != currentUserId)
            {
                TempData["message"] = "You do not have Edit access to the requested team. Activity Logged.";
                return RedirectToAction("Index");
            }

            model.myGroup = itm;
            ViewBag.userid = currentUserId;

            model.myMembers = db.MemberGroupmembersSet.Include("MemberDetails").Where(p => p.Group.Id == model.myGroup.Id).ToList();


            if (!String.IsNullOrEmpty(givenUsername))
            {
                IQueryable<UserProfileInfo> rtrn = from m in db.UserProfileInfo

                                                   select m;

                rtrn = rtrn.Where(s => s.UserName == givenUsername);

                ViewBag.searchResults = rtrn.ToList<UserProfileInfo>();


                List<int> pomocna = new List<int>();

                foreach (UserProfileInfo u in rtrn)
                {
                    if (model.myMembers.FirstOrDefault(x => x.MemberDetails.Id == u.Id) != null) pomocna.Add(u.Id);
                }
                ViewBag.searchResults2 = pomocna;
            }
            return View("Update", model);
        }

        [HttpPost]
        public ActionResult Leave(int? userId, int groupId)
        {
            try
            {
                int uId = userId.HasValue ? userId.Value : LoggedInUserKey;

                MemberGroupmembers itm = db.MemberGroupmembersSet.Include("MemberDetails").FirstOrDefault(x => x.MemberDetails.Id == uId && x.Group.Id == groupId);
                MemberGroups gr = db.MemberGroupsSet.FirstOrDefault(x => x.Id == groupId);
                if (itm != null && gr != null)
                {

                    gr.Broj_clanova = Math.Max(gr.Broj_clanova - 1, 0); ;
                    db.Entry(gr).State = EntityState.Modified;
                    obavijest(gr, itm);
                    glasovi(gr, itm);
                    if (gr.Broj_clanova == 4) { grupu_mjenjaj(gr); }
                    db.MemberGroupmembersSet.Remove(itm);
                    db.SaveChanges();
                }

            }
            catch (Exception exc)
            {

                TempData["message"] = "Error Leaving the requested team: " + exc.Message + exc.StackTrace;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListTeams(int page = 1)
        {


            var model = db.MemberGroupsSet.OrderBy(r => r.Name).ToPagedList(page, 10);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Listpart", model);
            }

            return View(model);
        }


        private void glasovi(MemberGroups group, MemberGroupmembers member)
        {
            Dictionary<MemberGroupmembers, int> glas = new Dictionary<MemberGroupmembers, int>();
            IEnumerable<MemberGroupmembers> members=db.MemberGroupmembersSet.Include("Vote").Where(x => x.Group.Id == group.Id);
            foreach (MemberGroupmembers mg in members)
            {
               
                    if (mg.Vote != null)
                    {
                        if (!glas.ContainsKey(mg.Vote)) glas.Add(mg.Vote, 1);
                        else glas[mg.Vote]++;

                        if (mg.Id == member.Id) glas[mg.Vote]--;
                    }
            }
            int max = 0;
            int broj = 0;
            foreach (KeyValuePair<MemberGroupmembers, int> k in glas)
            {
                if (k.Value > max) { max = k.Value; broj = k.Key.MemberDetails.Id; }
                k.Key.Votes = k.Value;
                db.Entry(k.Key).State = EntityState.Modified;
            }
        
                group.OwnerId = broj;
                db.Entry(group).State = EntityState.Modified;
        }

        private void obavijest(MemberGroups group, MemberGroupmembers member)
        {
            foreach (MemberGroupmembers mg in db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group.Id))
            {
                if (mg.Aktivnost == 0 && mg.Id != member.Id)
                {
                    string strSender = "Hello " + mg.MemberDetails.UserName + ", " + member.MemberDetails.UserName + " has left your team. ";
                    Alert(mg.MemberDetails.Id, strSender);
                }
            }
        }

        private void obavijest3(MemberGroups group, MemberGroupmembers member)
        {
            foreach (MemberGroupmembers mg in db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group.Id))
            {
                if (mg.Aktivnost == 0 && mg.Id != member.Id)
                {
                    string strSender = "Hello " + mg.MemberDetails.UserName + ", " + member.MemberDetails.UserName + " has been kicked from your team. ";
                    Alert(mg.MemberDetails.Id, strSender);
                }
            }
        }

        private void Alert(int to, string message)
        {
            try
            {
                MemberAlert obj = new MemberAlert();
                obj.Ishidden = 0;
                obj.Message = message;
                obj.PUser = db.UserProfileInfo.FirstOrDefault(x => x.Id == to);
                db.MemberAlerts.Add(obj);
                //db.SaveChanges();

                //TempData["message2"] = "Requested User Notified.";
            }
            catch (Exception ex1)
            {
                TempData["message"] = "Unable to Notify. Error: " + ex1.Message;
            }
        }


        [HttpPost]
        public ActionResult Vote(int? Vote, int group_id)
        {
            try
            {
                int uId = LoggedInUserKey;
                MemberGroupmembers memb = db.MemberGroupmembersSet.Include("Vote").FirstOrDefault(x => x.MemberDetails.Id == uId);
                MemberGroupmembers stari_vote = memb.Vote;
                memb.Vote = db.MemberGroupmembersSet.FirstOrDefault(x => x.Id == Vote);
                if (memb.Vote != null)
                {

                    if (memb.Vote != stari_vote) // promijenjen je glas
                    {

                        if (memb.Vote.Id == memb.Id) //glasa sam za sebe
                        {
                            memb.Votes += 1;
                        }
                        else
                        {
                            MemberGroupmembers memb2 = db.MemberGroupmembersSet.FirstOrDefault(x => x.Id == memb.Vote.Id);
                            memb2.Votes += 1;
                            db.Entry(memb2).State = EntityState.Modified;
                        }
                        if (stari_vote == memb)
                        {
                            memb.Votes -= 1;
                        }
                        else if (stari_vote != null) //oduzet za jedan
                        {
                            MemberGroupmembers memb2 = db.MemberGroupmembersSet.FirstOrDefault(x => x.Id == stari_vote.Id);
                            memb2.Votes -= 1;
                            db.Entry(memb2).State = EntityState.Modified;
                        }
                        int max = 0;
                        int pom = 0;
                        int broji = 0;
                        foreach (MemberGroupmembers mb in db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group_id))
                        {
                            if (mb.Votes > max) { max = mb.Votes; pom = mb.MemberDetails.Id; broji = 1; }
                            else if (mb.Votes == max) broji++;
                        }
                        if (pom != 0)
                        {


                            MemberGroups grupa = db.MemberGroupsSet.FirstOrDefault(x => x.Id == group_id);
                            if (broji > 1)
                                grupa.OwnerId = 0;
                            else if(max>=2 || db.MemberGroupmembersSet.Where(x=>x.Group.Id==grupa.Id).Count()==1)
                                grupa.OwnerId = pom;
                            db.Entry(grupa).State = EntityState.Modified;
                        }


                    }
                    db.Entry(memb).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message2"] = "Vote successfully changed!";
                }


            }
            catch (Exception exc)
            {

                TempData["message"] = "Error Voting: " + exc.Message + exc.StackTrace;
            }

            return RedirectToAction("Index");
        }

        private void grupu_mjenjaj(MemberGroups group)
        {
            IEnumerable<UserProfileInfo> membri = db.UserProfileInfo.Where(x => x.Team.Id == group.Id);
            foreach (UserProfileInfo m in membri)
            {
                m.Team = null;
                db.Entry(m).State = EntityState.Modified;
            }
        }


        [HttpPost]
        public ActionResult Disband(int? userId, int groupId)
        {
            try
            {
                int uId = userId.HasValue ? userId.Value : LoggedInUserKey;

                IEnumerable<MemberGroupmembers> itm = db.MemberGroupmembersSet.Where(x => x.Group.Id == groupId);
                MemberGroups itm2 = db.MemberGroupsSet.FirstOrDefault(x => x.Id == groupId);
                obavijest2(itm2);
                if (itm2.Broj_clanova == 5) { grupu_mjenjaj(itm2); }
                db.MemberGroupmembersSet.RemoveRange(itm);
                db.MemberGroupsSet.Remove(itm2);
                db.SaveChanges();
            }
            catch (Exception exc)
            {

                TempData["message"] = "Error disbanding the requested team: " + exc.Message + exc.StackTrace;
            }
            return RedirectToAction("Index");
        }

        private void obavijest2(MemberGroups group)
        {
            foreach (MemberGroupmembers mg in db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group.Id))
            {
                string strSender = "Hello " + mg.MemberDetails.UserName + ", Team " + group.Name + " has been disbanded. ";
                Alert(mg.MemberDetails.Id, strSender);
            }
        }


        [HttpPost]
        public ActionResult RemoveFromGroup(int? userId, int groupId)
        {
            if (userId == null)
            {
                TempData["message"] = "Error has occured!";
                return RedirectToAction("Index");
            }
            try
            {
                int currentUserId = LoggedInUserKey;
                MemberGroupmembers fr = db.MemberGroupmembersSet.Include("MemberDetails").FirstOrDefault(x => x.MemberDetails.Id == userId && x.Group.Id == groupId);
                MemberGroups gr = db.MemberGroupsSet.FirstOrDefault(x => x.Id == groupId);
                if (fr != null && gr != null)
                {

                    gr.Broj_clanova = Math.Max(gr.Broj_clanova - 1, 0);
                    db.Entry(gr).State = EntityState.Modified;
                    if (gr.Broj_clanova == 4) { grupu_mjenjaj(gr); }
                    obavijest3(gr, fr);
                    glasovi(gr, fr);
                    db.MemberGroupmembersSet.Remove(fr);
                    MemberRequests req = db.MemberRequestsSet.FirstOrDefault(x => x.Recipientid.Id == userId && x.Type == 2);
                    if (req != null)
                        db.MemberRequestsSet.Remove(req);
                    db.SaveChanges();
                }


                TempData["message2"] = "Requested User is now removed from your team.";
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchSolo()
        {
            ViewBag.naslov = "Search not possible!";
            var originalUserProfile = db.UserProfileInfo.Find(LoggedInUserKey);
            TempData["message"] = Models.Helpers.Trazenje.moze_trazit(new List<UserProfileInfo> { originalUserProfile }, db); // provjera da li je unjeo sve podatke
            if (((List<string>)TempData["message"]).Count > 0) return (View("RequestStatus"));
            TempData["message"] = null;
            if (originalUserProfile.Search != 1)
            {
                originalUserProfile.Search = 1;
                db.Entry(originalUserProfile).State = EntityState.Modified;
                db.SaveChanges();
            }
            //TempData["message"] = Models.Helpers.Trazenje.moze_trazit(new List<UserProfileInfo> { originalUserProfile }, db);
            //if (((string)TempData["message"]).Length > 0) return (View("RequestStatus"));
            bool nasao = Models.Helpers.Trazenje.trazi(originalUserProfile, db);
            if (nasao) TempData["message2"] = "Team has been found :)! Please wait for other players to confirm they are active.";
            else TempData["message"] = "Not possible to form a team at this moment. You are in the search pool, and you will be informed when enough players for forming a team are available.";
            return RedirectToAction("Index");
            //return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult StopSearchSolo()
        {
            var originalUserProfile = db.UserProfileInfo.Find(LoggedInUserKey);

            if (originalUserProfile.Search == 1)
            {
                originalUserProfile.Search = 0;
                db.Entry(originalUserProfile).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult StopSearchasGroup()
        {
            MemberGroupmembers m = db.MemberGroupmembersSet.Include("Group").Include("MemberDetails").FirstOrDefault(x => x.MemberDetails.Id == LoggedInUserKey);
            IEnumerable<MemberGroupmembers> mb = db.MemberGroupmembersSet.Include("Group").Include("MemberDetails").Where(x => x.Group.Id == m.Group.Id);

            List<UserProfileInfo> lista = new List<UserProfileInfo>();
            int i = 0;
            foreach (MemberGroupmembers mg in mb)
            {
                lista.Add(mg.MemberDetails);
                if (mg.MemberDetails.Search != 0)
                {
                    lista[i].Search = 0;
                    db.Entry(lista[i]).State = EntityState.Modified;
                }
                i++;

            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchasGroup()
        {
            MemberGroupmembers m = db.MemberGroupmembersSet.Include("Group").Include("MemberDetails").FirstOrDefault(x => x.MemberDetails.Id == LoggedInUserKey);
            IEnumerable<MemberGroupmembers> mb = db.MemberGroupmembersSet.Include("Group").Include("MemberDetails").Where(x => x.Group.Id == m.Group.Id);
            List<UserProfileInfo> lista = new List<UserProfileInfo>();
            int i = 0;
            foreach (MemberGroupmembers mg in mb)
            {
                lista.Add(mg.MemberDetails);
                if (mg.MemberDetails.Search != 1)
                {
                    lista[i].Search = 1;
                    db.Entry(lista[i]).State = EntityState.Modified;
                }
                i++;

            }
            TempData["message"] = Models.Helpers.Trazenje.moze_trazit(lista, db);
            if (((List<string>)TempData["message"]).Count > 0) { return (View("RequestStatus")); }
            TempData["message"] = null;
            db.SaveChanges();

            bool nasao = Models.Helpers.Trazenje.trazi_group(lista, db, LoggedInUserKey);

            if (nasao) TempData["message2"] = "Team has been found :)!  Please wait for other players to confirm they are active.";
            else TempData["message"] = "Not possible to form a team at this moment. Your team is in the search pool, and you will be informed when enough players for forming a team are available.";


            return RedirectToAction("Index");
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