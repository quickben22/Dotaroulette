
using PortableSteam;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using WebApplication5.Framework;


namespace WebApplication5.Controllers
{
    public class TaskController : ProgressBarController
    {
        public string DoWork(string repeat)
        {
            var taskId = GetTaskId();
            ProgressManager.SetCompleted(taskId, "Starting...");

            string[] pom = repeat.Split(',');
            if (pom.Length == 3) 
            {
                if (pretvori(pom[0]) <= 0 || pretvori(pom[1]) <= 0) 
                {
                    ProgressManager.RequestTermination(taskId);
                    return "MMR and number of matches has to be greater than 0.";
                }
                if (pretvori(pom[2]) == 0 || pom[2].Length != 17) 
                {
                    
                    ProgressManager.RequestTermination(taskId);
                    return "Steam64 not found";
                }
            }
            else 
            {
                ProgressManager.RequestTermination(taskId);
                return "MMR, number of matches and Steam64 required!";
            }


         

            
           //ristovski "FBC63DE006DC15DB344036DACD6F0E37"
           //dotaroulette "F400C3AD46ECD33060F1499A93380BB7"
           SteamWebAPI.SetGlobalKey("FBC63DE006DC15DB344036DACD6F0E37");

           ////var json = SteamWebAPI.General()
           ////             .IPlayerService()
           ////             .GetCommunityBadgeProgress(SteamIdentity.FromSteamID(76561198049927741))
           ////.GetResponseString();

            //string ProviderKey = "http://steamcommunity.com/openid/id/76561198026710641";
            long steamid = Convert.ToInt64(pom[2]);
            var identity = SteamIdentity.FromSteamID(steamid);
            if (identity == null) return "User not found";
            var playerSummaries = SteamWebAPI.General()
                                 .ISteamUser()
                                 .GetPlayerSummaries(identity)
                                 .GetResponse();
            string avatar = playerSummaries.Data.Players[0].Avatar;


            var friendSummaries = SteamWebAPI.General()
                                 .ISteamUser()
                                 .GetFriendList(identity, RelationshipType.Friend)
                                 .GetResponse();

            List<long> pajdasi = new List<long>();
            
            if (friendSummaries.Data != null)
                foreach (var v in friendSummaries.Data.Friends)
                {
                    pajdasi.Add(v.Identity.AccountID);
                }


            var response = SteamWebAPI.Game()
                               .Dota2()
                               .IDOTA2Match()
                               .GetMatchHistory()
                               .Account(identity)
                               .GetResponse();


            int zbroj = Convert.ToInt32(pretvori(pom[0]));
            int meceva = Convert.ToInt32(pretvori(pom[1]));
            Dictionary<DateTime, int> zbrojeva =   new Dictionary<DateTime, int> { { DateTime.Now, zbroj } };
            int brojac2 = 0;
            int brojac3 = 0;
            while (response.Data.NumberOfResults > 1)
            {
               
                int brojac = 0;
                foreach (var v in response.Data.Matches)
                {
                    brojac3++;
                    if (brojac3 > meceva) break;
             
                    ProgressManager.SetCompleted(taskId, String.Format("Progress: Matches analyzed {0}/{1}", brojac3, meceva));


                    brojac++;
                    if (brojac == 100) break;

          
                    if (v.LobbyType == Dota2LobbyType.Ranked)
                    {
                        bool solo = true;

                        int b = v.Players.IndexOf(v.Players.FirstOrDefault(x => x.Identity.SteamID == identity.SteamID));


                        if (b < 5)
                            for (int i = 0; i < 5; i++)
                            {
                                if (pajdasi.Contains(v.Players[i].Identity.AccountID)) { solo = false; break; }
                            }
                        else
                            for (int i = 5; i < 10; i++)
                            {
                                if (pajdasi.Contains(v.Players[i].Identity.AccountID)) { solo = false; break; }
                            }
                        if (solo)
                        {

                            var data = SteamWebAPI.Game().Dota2().IDOTA2Match().GetMatchDetails(v.MatchID).GetResponse();
                            var nesto = new TimeSpan(0, 10, 0);

                            if (data.Data.Duration > new TimeSpan(0, 10, 0))
                            {

                                if (b < 5 && data.Data.RadiantWin) zbroj -= 24;
                                else if (b > 4 && !data.Data.RadiantWin) zbroj -= 24;
                                else zbroj += 24;
                                brojac2++;

                                zbrojeva.Add(data.Data.StartTime, zbroj);
                            }
                        }

                    }

                }
                if (brojac3 > meceva) break;
                response = SteamWebAPI.Game()
                               .Dota2()
                               .IDOTA2Match()
                               .GetMatchHistory()
                               .Account(identity).StartAtMatchID(response.Data.Matches.Last().MatchID)
                               .GetResponse();
            }

    
          //System.IO.File.WriteAllLines(ProviderKey.Replace("http://steamcommunity.com/openid/id/", "") + ".txt",
          //    zbrojeva.Reverse().Select(x => x.Key + "\t" + x.Value).ToArray());
           

            //for (var i = 1; i <= repeat; i++)
            //{
               
            //    Thread.Sleep(1000);
            //}

            // Some return value
            if (zbrojeva.Count > 0)
            {
                Session["zbrojeva"] = zbrojeva;
                return "Search complete!";
            }
            else return "No solo ranked matches found!";
            //return  RedirectToAction("Index", "Home");
        }

   

        private long pretvori(object b)
        {
            double rezultat;
            string a = b.ToString();
            bool isNum = double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out rezultat);

            if (isNum)
                rezultat = double.Parse(a.Replace(",", "."), System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-us"));
            else
                rezultat = 0;

            return Convert.ToInt64(Math.Round(rezultat,0));
        }

    }
}