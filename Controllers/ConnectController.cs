using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.Models;
using System.Security.Principal;
using System.Web.Security;
using WebApplication5.Models.Helpers;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace WebApplication5.Controllers
{
    public class ConnectController : Controller
    {
       private ApplicationDbContext db;


       public ConnectController(ApplicationDbContext _db)
    {
        db = _db;
    }

        public ActionResult VRequest(int id, int tId, int type, string username)
        {
            int UserId = LoggedInUserKey;
            MemberRequests model = new MemberRequests()
            {
                Senderid = new UserProfileInfo { Id = UserId },
                Recipientid = new UserProfileInfo() { Id = id, UserName = username },
                TargetPageid = tId,

                Type = (int)type
            };
            ViewBag.poruka = mozes_zvati(id);
            return (View(model));
        }

        private List<string> mozes_zvati(int id)
        {
            List<string> povrat = new List<string>();
            IEnumerable<MemberGroupmembers> mb = db.MemberGroupmembersSet.Include("Group").Include("MemberDetails").Where(x => x.Group.OwnerId == LoggedInUserKey);
            List<UserProfileInfo> lista = new List<UserProfileInfo>();
            int N = 5;
            int M = mb.Count() + 1;
            int[,] igraci = new int[M, N];
            int i = 0;
            foreach (MemberGroupmembers mg in mb)
            {
                bool fav = true;
                foreach (Position p in mg.MemberDetails.Positions)
                {
                    igraci[i, p.PositionNumber - 1] = 1;
                    if (mg.MemberDetails.FavPosition == p.PositionNumber) fav = false;
                }
                if (fav && mg.MemberDetails.FavPosition!=0) igraci[i, mg.MemberDetails.FavPosition - 1] = 1;
                lista.Add(mg.MemberDetails);
                i++;
            }
            UserProfileInfo imag_user = Models.Helpers.Trazenje.stvori_usera(lista, db,LoggedInUserKey);
            UserProfileInfo user = db.UserProfileInfo.Include("Team").FirstOrDefault(y => y.Id == id);

            bool fav2 = true;
            foreach (Position p in user.Positions)
            {
                igraci[i, p.PositionNumber - 1] = 1;
                if (user.FavPosition == p.PositionNumber) fav2 = false;
            }
            if (fav2 && user.FavPosition!=0) igraci[i, user.FavPosition - 1] = 1;
            bool jezik = false;
            foreach (Language l in user.Languages)
            {
                if (imag_user.Languages.Contains(l)) { jezik = true; break; }
            }

            if (user.Age < imag_user.MinAge || imag_user.Age < user.MinAge || user.Age > imag_user.MaxAge || imag_user.Age > user.MaxAge) povrat.Add(" Years don't match!");
            if (user.Rating < imag_user.MinRating || imag_user.Rating < user.MinRating || imag_user.Rating > user.MaxRating || user.Rating > imag_user.MaxRating) povrat.Add(" Ratings don't match!");
            if (user.MinTime > imag_user.MaxTime || imag_user.MinTime > user.MaxTime) povrat.Add( " Available time doesnt match!");
            if (user.Microphone != imag_user.Microphone)povrat.Add( " Microphones don't match!");
            if (user.Team != null || db.MemberGroupmembersSet.FirstOrDefault(x=>x.MemberDetails.Id==id)!=null) povrat.Add(" User is already in a team!");
            if (Math.Abs(Trazenje.zone_sati(user.TimeZone) - Trazenje.zone_sati(imag_user.TimeZone)) > 1)povrat.Add( " Time zones don't match!");
            if (user.Goal != imag_user.Goal) povrat.Add( " Goals don't match!");
            if (Trazenje.maxBPM(igraci, M, N) < M) povrat.Add( " Positions don't match!");
            if (!jezik) povrat.Add(" Languages don't match!");

            return povrat;
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


        [HttpPost]
        public ActionResult VRequest(int id, int tId, int type, MemberRequests model)
        {
            int currentUserId = LoggedInUserKey;
            try
            {
                int _senderId = currentUserId;

                if ((model.Senderid != null) && (model.Senderid.Id != 0) && (model.Senderid.Id != -1))
                    _senderId = model.Senderid.Id;
                if (db.MemberRequestsSet.FirstOrDefault(x => x.Recipientid.Id == id && x.Senderid.Id == _senderId && x.Status == 1) != null)
                {
                    TempData["message"] = "Unable to invite friend. Error: You already invited this member!";
                    return (View("RequestStatus"));
                }

                UserProfileInfo sender = db.UserProfileInfo.FirstOrDefault(x => x.Id == _senderId);
                UserProfileInfo recipient = db.UserProfileInfo.FirstOrDefault(x => x.Id == id);
                MemberRequests itm = new MemberRequests();

                Guid val;
                val = System.Guid.NewGuid();

                itm.Guid = val.ToString();
                itm.Senderid = sender;
                itm.Recipientid = recipient;
                itm.TargetPageid = model.TargetPageid;
                itm.Type = (int)type;
                itm.Status = 1;

                try
                {
                    //Mail sending. 
                    //<-- To Do --> Add logic to check if email notifications for target user are enabled
                    //string[] msgParam = new string[4];

                    //string strMsg = "Hello,\n\n {0} has sent you a {1} request at Parichay. \n\n Please click on following link to acknowledge :\n {2} \n\n {3} \n\n Thanks & Regards,\n Webmasters \n WebAdmin - Parichay";

                    //msgParam[0] = itm.Senderid.UserName;
                    //msgParam[1] = "1";
                    //msgParam[2] = AppConstants.BaseSiteUrl.TrimEnd('/') + Url.Action("AckRequest", new { id = itm.Guid });
                    ////msgParam[3] = string.IsNullOrEmpty(model.UserMessage) ? "" : "\n----------\n" + model.UserMessage + "\n----------\n";


                    //strMsg = string.Format(strMsg, msgParam);

                    //SendMail(model.Email, itm.Senderid.Givennm + " : Parichay " + type.ToString() + " request", strMsg);
                    //Mail sending
                    if (recipient.Email != null)
                        Trazenje.SendMail(recipient.Email);
                }
                catch
                {
                    //Data.Helper.NHibernateHelper.Log(new Exception("Error sending Connect request Email=>", exc1));
                }
                db.MemberRequestsSet.Add(itm);
                db.SaveChanges();
                TempData["message2"] = "Invitation sent successfully.";
                model.TargetPageid = id;
            }
            catch (Exception exc1)
            {
                TempData["message"] = "Unable to invite to team. Error:" + exc1.Message;
            }
            return (View("RequestStatus"));
        }


      

        private int zone_sati(int br)
        {
            ReadOnlyCollection<TimeZoneInfo> timeZonest = TimeZoneInfo.GetSystemTimeZones();
            return timeZonest[br].BaseUtcOffset.Hours;
        }


        [Authorize]
        public ActionResult AckRequest(string id)
        {
            int currentUserId = LoggedInUserKey;
            MemberRequests source = db.MemberRequestsSet.Include("Senderid").Include("Recipientid").FirstOrDefault(x => x.Guid == id);
            MemberRequests model = source;
            ViewBag.IsRecipient = (source.Senderid.Id != currentUserId);
            return View(model);
        }


      


        [Authorize]
        [HttpPost]
        public ActionResult AckRequest(string id, MemberRequests model, string submitButton)
        {
            int currentUserId = LoggedInUserKey;
            MemberRequests source = db.MemberRequestsSet.Include("Senderid").Include("Recipientid").FirstOrDefault(x => x.Guid == model.Guid);

            switch (submitButton)
            {
                case ("Accept"):
                    {
                        if (source.Type == 1) // 1 je manualno stvorena grupa
                        {
                            return JoinGroup(source);
                        }
                        else if (source.Type == 2) //2 je automatski stvorena grupa
                        {
                            return JoinGroup2(source);
                        }
                        break;
                    }
                case ("Decline"):
                    {
                        db.MemberRequestsSet.Remove(source);
                        if (source.Type == 2) //2 je automatski stvorena grupa
                        {
                           MemberGroupmembers mb= db.MemberGroupmembersSet.FirstOrDefault(x => x.MemberDetails.Id == currentUserId);
                           db.MemberGroupmembersSet.Remove(mb);
                        }
                        db.SaveChanges();
                        break;
                    }
                case ("Hide"):
                    {

                        source.Status = 0;
                        db.Entry(source).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["message2"] = "The notification is now hidden from view. You will receive a Notification if the invitation is accepted.";
                        break;
                    }
                default:
                    {
                        TempData["message"] = "Un-Identified action.";
                        break;
                    }
            }

            if (source.Senderid != null)
                ViewBag.IsRecipient = (source.Senderid.Id != currentUserId);
            return RedirectToAction("Index", "Messages"); 
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
        
            TempData["message2"] = "Requested User Notified.";
            }
            catch (Exception ex1)
            {
            TempData["message"] = "Unable to Notify. Error: " + ex1.Message;
            }
        }

        private void Alerti(int group_id,int novi_clan_id,string novi_clan,string group)
        {
            foreach (MemberGroupmembers mg in db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group_id))
            {
                if (mg.Aktivnost == 0)
                {
                    string strSender = "Hello " + mg.MemberDetails.UserName + ", " + novi_clan + " has joined your team.";
                    Alert(mg.MemberDetails.Id, strSender);
                }
            }
            
            string strRecipient = "Hello " + novi_clan + ", you are now a member of the team " + group;
            Alert(novi_clan_id, strRecipient);
          

        }

        private ActionResult JoinGroup(MemberRequests thisReq) //osoba poziva
        {
            try
            {
                int currentUserId = LoggedInUserKey;
                if (thisReq.Senderid.Id == currentUserId)
                {
                    throw new Exception("You are already a member of your team.");
                }
                MemberGroupmembers itm = new MemberGroupmembers();
                itm.MemberDetails = db.UserProfileInfo.FirstOrDefault(x => x.Id == currentUserId);
                MemberGroupmembers pom_u = db.MemberGroupmembersSet.Include("Group").FirstOrDefault(x => x.MemberDetails.Id == thisReq.Senderid.Id);
                if (pom_u != null)
                    itm.Group = pom_u.Group;
                if (itm.Group == null)
                {
                    throw new Exception("Team unavailable.");
                }

                if (db.MemberGroupmembersSet.FirstOrDefault(x => x.MemberDetails.Id == thisReq.Recipientid.Id && x.Group.Id == thisReq.TargetPageid) != null)
                {
                    string erText = "This user is already member of this team.";
                    throw new Exception(erText);
                }

                Alerti(itm.Group.Id, thisReq.Recipientid.Id, thisReq.Recipientid.UserName, itm.Group.Name);

                db.MemberGroupmembersSet.Add(itm);

                itm.Group.Broj_clanova += 1;

                if (itm.Group.Broj_clanova == 5) grupu_mjenjaj(itm.Group, itm.MemberDetails);
                db.Entry(itm.Group).State = EntityState.Modified;

                db.MemberRequestsSet.Remove(thisReq);
                db.SaveChanges();
                TempData["message2"] = "You are now a member of requested team";

            }
            catch (Exception ex)
            {
                db.MemberRequestsSet.Remove(thisReq);
                db.SaveChanges();
                TempData["message"] = ex.Message + ex.StackTrace;
            }
            return RedirectToAction("Index", "Group");
        }

        private ActionResult JoinGroup2(MemberRequests thisReq) // program poziva
        {
            try
            {
                int currentUserId = LoggedInUserKey;
        
                MemberGroupmembers memb = db.MemberGroupmembersSet.Include("Group").FirstOrDefault(x => x.MemberDetails.Id == thisReq.Recipientid.Id);

                if (memb == null)
                {
                    throw new Exception("Team unavailable.");
                }
                if (memb.Group == null)
                {
                    throw new Exception("Team unavailable.");
                }

                if (memb.Aktivnost != 0)
                {
                    Alerti(memb.Group.Id, thisReq.Recipientid.Id, thisReq.Recipientid.UserName, memb.Group.Name); // obavjestit sve clanove da su dobili novoga
                    memb.Aktivnost = 0; // ovo znaci da smo postavili igrača da je aktivan
                    memb.Group.Broj_clanova =Math.Min(memb.Group.Broj_clanova+1,5) ;

                    if (memb.Group.Broj_clanova == 5) grupu_mjenjaj(memb.Group, memb.MemberDetails);
                    db.Entry(memb.Group).State = EntityState.Modified;
                    db.Entry(memb).State = EntityState.Modified;


                    db.MemberRequestsSet.Remove(thisReq);
                    db.SaveChanges();
                    TempData["message2"] = "You are now a member of requested team";
                }
                else
                {
                    TempData["message"] = "You are already a member of this team";
                }

            }
            catch (Exception ex)
            {
                db.MemberRequestsSet.Remove(thisReq);
                db.SaveChanges();
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("Index", "Group");
        }


        private void grupu_mjenjaj(MemberGroups group, UserProfileInfo user)
        {
            IEnumerable<MemberGroupmembers> membri = db.MemberGroupmembersSet.Include("MemberDetails").Where(x => x.Group.Id == group.Id);
            foreach (MemberGroupmembers m in membri)
            {
                UserProfileInfo u = m.MemberDetails;
                u.Team = group;
                db.Entry(u).State = EntityState.Modified;
            }
            user.Team = group;
            db.Entry(user).State = EntityState.Modified;
        }
    }
}