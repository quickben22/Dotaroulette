using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity.Design.PluralizationServices;
using System.Data.Entity;
using System.Text;

namespace WebApplication5.Models.Helpers
{
    public static class Trazenje
    {

        public static List<string> moze_trazit(List<UserProfileInfo> Users, ApplicationDbContext db)
        {
            List<string> povrat = new List<string>();
            foreach (UserProfileInfo u in Users)
            {
                string povrat2 = "";
                if (u.Age == 0) povrat2 += " Age missing!";
                if (u.FavPosition == 0) povrat2 += " Favorite position missing!";
                if (u.Goal == 0) povrat2 += " Goal missing!";
                if (u.Languages.Count() == 0) povrat2 += " Languages missing!";
                if (u.MaxAge == 0) povrat2 += " MaxAge missing!";
                if (u.MaxRating == 0) povrat2 += " MaxRating missing!";
                if (u.MaxTime == 0) povrat2 += " MaxTime missing!";
                if (u.Microphone == 0) povrat2 += " Microphone missing!";
                if (u.MinAge == 0) povrat2 += " MinAge missing!";
                if (u.MinRating == 0) povrat2 += " MinRating missing!";
                if (u.MinTime == 0) povrat2 += " MinTime missing!";
                if (u.Positions.Count() == 0) povrat2 += " Positions missing!";
                if (u.Rating == 0) povrat2 += " Ratings missing!";
                if (u.Team != null) povrat2 += " Already in a team!";
                if (u.TimeZone == 0) povrat2 += " TimeZone missing!";
                if (povrat2.Length > 0)
                {
                    povrat.Add(u.UserName + ":" + povrat2);
                }
            }
            return povrat;
        }

        public static bool trazi_group(List<UserProfileInfo> Users, ApplicationDbContext db, int u_id)
        {

            return trazi(stvori_usera(Users, db, u_id), db);
        }

        public static UserProfileInfo stvori_usera(List<UserProfileInfo> Users, ApplicationDbContext db, int u_id)
        {
            int rating = 6000;
            int minrating = 0;
            int maxrating = 6000;
            int age = 7;
            int minage = 0;
            int maxage = 7;
            int mintime = 0;
            int maxtime = 58;
            int mircrophone = 0;
            int goal = 0;
            int timezone = Users[0].TimeZone;
            List<Language> languages = new List<Language>(Users[0].Languages);

            foreach (UserProfileInfo u in Users)
            {
                if (u.Rating < rating)
                    rating = u.Rating;
                if (u.MinRating > minrating)
                    minrating = u.MinRating;
                if (u.MaxRating < maxrating)
                    maxrating = u.MaxRating;
                if (u.Age < age)
                    age = u.Age;
                if (u.Age < age)
                    age = u.Age;
                if (u.MinAge > minage)
                    minage = u.MinAge;
                if (u.MaxAge < maxage)
                    maxage = u.MaxAge;
                if (u.MinTime > mintime)
                    mintime = u.MinTime;
                if (u.MaxTime < maxtime)
                    maxtime = u.MaxTime;
                mircrophone = u.Microphone;
                goal = u.Goal;
                List<Language> l_pom = new List<Language>();
                foreach (Language l in languages)
                {
                    if (!u.Languages.Contains(l)) l_pom.Add(l);
                }
                foreach (Language l in l_pom)
                {
                    languages.Remove(l);
                }

            }
            UserProfileInfo imag_user = new UserProfileInfo();
            imag_user.Age = age;
            imag_user.Goal = goal;
            imag_user.MaxAge = maxage;
            imag_user.MaxRating = maxrating;
            imag_user.MaxTime = maxtime;
            imag_user.Microphone = mircrophone;
            imag_user.MinAge = minage;
            imag_user.MinRating = minrating;
            imag_user.MinTime = mintime;
            imag_user.Rating = rating;
            imag_user.Search = 1;
            imag_user.TimeZone = timezone;
            imag_user.Languages = languages;

            imag_user.Id = u_id;
            return imag_user;
        }

        public static bool trazi(UserProfileInfo User, ApplicationDbContext db)
        {
            Dictionary<int, UserProfileInfo> ekipa = new Dictionary<int, UserProfileInfo>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };
            bool nasao = false;
            string jezik = null;
            string goal = null;
            IQueryable<UserProfileInfo> reza =
                from p in db.UserProfileInfo
                where (p.Rating >= User.MinRating && User.Rating >= p.MinRating && p.Rating <= User.MaxRating && User.Rating <= p.MaxRating) // ratinge podjednake 
                && (p.Age >= User.MinAge && User.Age >= p.MinAge && p.Age <= User.MaxAge && User.Age <= p.MaxAge)  // godine iste
                && (p.MinTime <= User.MaxTime) && (User.MinTime <= p.MaxTime)  // podjednako vremena moraju imat za igrat
                && (p.Microphone == User.Microphone)
                && (p.Goal == User.Goal)  // isti cilj svi moraju imat
                && (p.Search == 1)
                && (p.Team == null) // nesmije imat tim
                select p;


            if (User.Goal > 0)
                goal = AppConstants.Ciljs[User.Goal - 1].Opis;
            Dictionary<string, List<UserProfileInfo>> nesto2 = new Dictionary<string, List<UserProfileInfo>>();
            //List<UserProfileInfo> davidim = reza.ToList();
            if (reza.ToList().Count() >= 5) // osnovni filtar
            {
                IEnumerable<MemberGroupmembers> svi_u_grupi = db.Set<MemberGroupmembers>().Include("MemberDetails").Include("Group"); // svi igraci koju ne traze samostalno,vec u grupi
                List<MemberGroupmembers> svi_u_grupi2 = svi_u_grupi.ToList();
                nesto2 = jezici(User, reza.ToList()); // podijela ljudi po jezicima
                var sorted1 = from entry in nesto2 orderby entry.Value.Count() descending select entry;
                nesto2 = sorted1.ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (KeyValuePair<string, List<UserProfileInfo>> nesto in nesto2)
                {
                    jezik = nesto.Key;
                    List<List<UserProfileInfo>> nesto3 = new List<List<UserProfileInfo>>();
                    nesto3 = zone(User, nesto.Value);  // podijela ljudi po zonama,max 2
                    var sorted2 = from entry in nesto3 orderby entry.Count() descending select entry;
                    nesto3 = sorted2.ToList();


                    foreach (List<UserProfileInfo> nestoa in nesto3)
                    {

                        Dictionary<int, List<UserProfileInfo>> nesto4 = new Dictionary<int, List<UserProfileInfo>>();
                        nesto4 = vremena(User, nestoa); // podijela ljudi po slobodnom vremenu
                        var sorted3 = from entry in nesto4 orderby entry.Value.Count() descending select entry;
                        nesto4 = sorted3.ToDictionary(pair => pair.Key, pair => pair.Value);
                        if (nesto4.Count() != 0)
                            if (nesto4.First().Value.Count() == nestoa.Count()) nesto4 = new Dictionary<int, List<UserProfileInfo>>() { { nesto4.First().Key, nesto4.First().Value } };


                        // filtar dnevno vremena
                        foreach (KeyValuePair<int, List<UserProfileInfo>> nestob in nesto4)
                        {
                            Dictionary<int, List<UserProfileInfo>> nesto5 = new Dictionary<int, List<UserProfileInfo>>();
                            nesto5 = ratingzi(User, nestob.Value); // podijela ljudi po ratinzima
                            var sorted4 = from entry in nesto5 orderby entry.Key descending select entry;
                            nesto5 = sorted4.ToDictionary(pair => pair.Key, pair => pair.Value);




                            // filtar ratinzi
                            foreach (KeyValuePair<int, List<UserProfileInfo>> nestoca in nesto5)
                            {
                                Dictionary<int, List<UserProfileInfo>> nesto5a = new Dictionary<int, List<UserProfileInfo>>();




                                nesto5a = godine(User, nestoca.Value); // podijela ljudi po godinama
                                var sorted4a = from entry in nesto5a orderby entry.Key descending select entry;
                                nesto5a = sorted4a.ToDictionary(pair => pair.Key, pair => pair.Value);

                                // filtar godina
                                foreach (KeyValuePair<int, List<UserProfileInfo>> nestoc in nesto5a)
                                {

                                    Tuple<List<UserProfileInfo>, List<List<UserProfileInfo>>> nesto_pom = groupe(User, nestoc.Value, svi_u_grupi);
                                    List<UserProfileInfo> nestoca_pom = nesto_pom.Item1;  //filtar da li ima igraca koji su u grupi, izbaci igrace ako su grupi, a kolege nisu preživjele filtre
                                    if (nestoca_pom.Count < 5) continue;
                                    Dictionary<int, int> kolko = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };
                                    foreach (UserProfileInfo u in nestoca_pom)
                                    {
                                        bool fav = true;
                                        foreach (Position r in u.Positions)
                                        {

                                            switch (r.PositionNumber)
                                            {
                                                case (1):
                                                    kolko[1] += 1;
                                                    break;
                                                case (2):
                                                    kolko[2] += 1;
                                                    break;
                                                case (3):
                                                    kolko[3] += 1;
                                                    break;
                                                case (4):
                                                    kolko[4] += 1;
                                                    break;
                                                case (5):
                                                    kolko[5] += 1;
                                                    break;
                                            }
                                            if (r.PositionNumber == u.FavPosition)
                                                fav = false;
                                        }
                                        if (fav && u.FavPosition != 0) kolko[u.FavPosition] += 1; // ako slucajno igrac nije stavio favposition medju pozicije koje igra
                                    }
                                    if (kolko.Values.Contains(0)) break; // nitko ne zeli igrat neki role
                                    var sorted5 = from entry in kolko orderby entry.Value ascending select entry;
                                    kolko = sorted5.ToDictionary(pair => pair.Key, pair => pair.Value); // kolko kojih pozicija sortirano od manje zastupljenosti prema većoj



                                    if (nesto_pom.Item2.Count < 1) //sam, odnosno ne postoje grupe ljudi koji su tražili
                                    {
                                        if (User.UserName == null) continue;
                                        //---------------------------------ovo je kada igrac sam traži----------------
                                        List<int> raspored = new List<int> { User.FavPosition };
                                        foreach (Position r in User.Positions)
                                        {
                                            if (r.PositionNumber != User.FavPosition) raspored.Add(r.PositionNumber);
                                        }

                                        foreach (int i in raspored) // role glavnog igraca koji trazi, prvo mu omiljenu rolu, pa ostale
                                        {
                                            ekipa = new Dictionary<int, UserProfileInfo>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };
                                            ekipa[i] = User;
                                            foreach (int k in kolko.Keys) // glavnom igracu smo dali rolu, sad vrtimo ostale role od najmanje zastupljene
                                            {
                                                if (k == i) continue;
                                                for (int j = 0; j < nestoca_pom.Count; j++) // prvo gledamo da li itko je postavio rolu "k" kao omiljenu
                                                {
                                                    if (nestoca_pom[j].FavPosition == k && ekipa[k] == null && !ekipa.Values.Contains(nestoca_pom[j]))
                                                    {
                                                        ekipa[k] = nestoca_pom[j];
                                                        break;
                                                    }
                                                }
                                                if (ekipa[k] == null) // ako je nitko nije stavio kao omiljenu onda je nekome uvaliti, prvom mogucem koji je moze igrat
                                                {
                                                    for (int j = 0; j < nestoca_pom.Count; j++)
                                                    {
                                                        foreach (Position r in nestoca_pom[j].Positions)
                                                        {
                                                            if (r.PositionNumber == k && !ekipa.Values.Contains(nestoca_pom[j])) { ekipa[k] = nestoca_pom[j]; break; }
                                                        }
                                                        if (ekipa[k] != null) break;
                                                    }

                                                }
                                            }
                                            if (!ekipa.Values.Contains(null))  //nasao je nesto
                                            {

                                                break;
                                            }
                                        }


                                        //suma sumarum ekipa je rjesenje
                                    }
                                    else // u ekipi, ili ima ekipi
                                    {

                                        //---------------------------------ovo je kada igrac traži u ekipi----------------

                                        foreach (List<UserProfileInfo> lista in nesto_pom.Item2)
                                        {
                                            List<UserProfileInfo> lista_pom = new List<UserProfileInfo>(lista);
                                            if (User.UserName == null) // User ne postoji, grupni search
                                            {

                                            }
                                            else if (lista.FirstOrDefault(x => x.Id == User.Id) != null) // user je u grupi
                                            {

                                            }
                                            else // treba usera "dodat" grupi
                                            {
                                                lista_pom.Add(User);
                                            }

                                            List<List<int>> raspored = stvori_raspored(lista_pom);


                                            foreach (List<int> i in raspored) // role glavnog igraca koji trazi, prvo mu omiljenu rolu, pa ostale
                                            {
                                                ekipa = new Dictionary<int, UserProfileInfo>() { { 1, null }, { 2, null }, { 3, null }, { 4, null }, { 5, null } };

                                                for (int ii = 0; ii < i.Count; ii++)
                                                    ekipa[i[ii]] = lista_pom[ii];

                                                foreach (int k in kolko.Keys) // glavnom igracu smo dali rolu, sad vrtimo ostale role od najmanje zastupljene
                                                {
                                                    if (i.Contains(k)) continue;
                                                    for (int j = 0; j < nestoca_pom.Count; j++) // prvo gledamo da li itko je postavio rolu "k" kao omiljenu
                                                    {
                                                        if (nestoca_pom[j].FavPosition == k && ekipa[k] == null && !ekipa.Values.Contains(nestoca_pom[j]))
                                                        {
                                                            ekipa[k] = nestoca_pom[j];
                                                            break;
                                                        }
                                                    }
                                                    if (ekipa[k] == null) // ako je nitko nije stavio kao omiljenu onda je nekome uvaliti, prvom mogucem koji je moze igrat
                                                    {
                                                        for (int j = 0; j < nestoca_pom.Count; j++)
                                                        {
                                                            foreach (Position r in nestoca_pom[j].Positions)
                                                            {
                                                                if (r.PositionNumber == k && !ekipa.Values.Contains(nestoca_pom[j])) { ekipa[k] = nestoca_pom[j]; break; }
                                                            }
                                                            if (ekipa[k] != null) break;
                                                        }

                                                    }
                                                }
                                                if (!ekipa.Values.Contains(null))  //nasao je nesto
                                                {

                                                    break;
                                                }
                                            }
                                            if (!ekipa.Values.Contains(null))  //nasao je nesto
                                            {

                                                break;
                                            }
                                            //suma sumarum ekipa je rjesenje
                                        }

                                        //if() treba dodati uvijet ako se ekipa moze sloziti od pojedinaca, a ne može se složiti od grupa + pojedinci

                                    }

                                    if (!ekipa.Values.Contains(null)) { nasao = true; break; }
                                }
                                if (nasao) break;
                            }
                            if (nasao) break;
                        }
                        if (nasao) break;
                    }
                    if (nasao) break;
                }
            }


            if (nasao)
            {
                // ukloni prijasnje grupe,ako postoje
                if (User.UserName == null) User = db.UserProfileInfo.FirstOrDefault(x => x.Id == User.Id);
                grupe_ukloni(db, User);
                // stvori tim i dodaj clana koji je trazio u tim, on je sigurno aktivan
                MemberGroups model = new MemberGroups();
                string[] pom = generiraj(); // generiraj random ime ekipe
                model.Broj_clanova = 1;
                model.Name = pom[0];
                model.Url = pom[1];
                model.Goal = goal;
                model.Language = jezik;
                db.MemberGroupsSet.Add(model);
                MemberGroupmembers member = new MemberGroupmembers();
                member.MemberDetails = User;
                member.Group = model;


                User.Search = 0;
                db.Entry(User).State = EntityState.Modified;

                // ostalima posalji zahtjev za clanstvom i dodaj ih u tim ali se stavlja zastavica aktivnost==1, to je za provjeru da li su aktivni prije daljnih akcija
                foreach (KeyValuePair<int, UserProfileInfo> u in ekipa)
                {
                    if (u.Value.Id == User.Id) { member.Role = u.Key; db.MemberGroupmembersSet.Add(member); continue; }

                    pozivi(db, User, u.Value, model, u.Key);
                    grupe_ukloni(db, u.Value);
                    UserProfileInfo pom_U = db.UserProfileInfo.FirstOrDefault(x => x.Id == u.Value.Id);
                    pom_U.Search = 0;
                    db.Entry(pom_U).State = EntityState.Modified;

                    MemberGroupmembers memberP = new MemberGroupmembers();
                    memberP.MemberDetails = u.Value;
                    memberP.Group = model;
                    memberP.Role = u.Key;
                    memberP.Aktivnost = 1; // Mora se provjeriti da li je osoba koju smo našli aktivna, ili se registrirala i ošla
                    db.MemberGroupmembersSet.Add(memberP);
                    //System.Diagnostics.Debug.WriteLine(u.Key + "  " + u.Value.UserName);
                }

                db.SaveChanges();
            }



            return nasao;
        }


        private static void pozivi(ApplicationDbContext db, UserProfileInfo sender, UserProfileInfo recipient, MemberGroups model, int pozicija)
        {
            MemberRequests itm = new MemberRequests();

            Guid val;

            val = System.Guid.NewGuid();

            itm.Guid = val.ToString();
            itm.Senderid = sender;
            itm.Recipientid = recipient;
            itm.TargetPageid = model.Id;

            itm.Type = 2;
            itm.Status = 1;
            itm.Rola = pozicija;
            IEnumerable<MemberRequests> items = db.MemberRequestsSet.Where(x => x.Senderid.Id == sender.Id && x.Recipientid.Id == recipient.Id && x.Status == 1); // ako vec postoji takav poziv
            if (items.Count() > 0) { db.MemberRequestsSet.RemoveRange(items); }
            db.MemberRequestsSet.Add(itm);
            try
            {
                if (recipient.Email != null)
                    SendMail(recipient.Email);
            }
            catch { }
            //db.SaveChanges();
        }


        public static void SendMail(string to)
        {




            System.Net.Mail.MailMessage eMail = new System.Net.Mail.MailMessage();
            eMail.IsBodyHtml = true;
            eMail.Body = "Hello, you have been invited to the team! Please check your messages at www.dotaroulette.com.";
            eMail.From = new System.Net.Mail.MailAddress("contact@dotaroulette.com");
            eMail.Sender = new System.Net.Mail.MailAddress("contact@dotaroulette.com");
            eMail.To.Add(new System.Net.Mail.MailAddress(to));
            eMail.Subject = "Team invite!";
            System.Net.Mail.SmtpClient SMTP = new System.Net.Mail.SmtpClient();
            SMTP.Host = "localhost";
            SMTP.Credentials = new System.Net.NetworkCredential("contact@dotaroulette.com", "dalibor32167");

            SMTP.Send(eMail);

        }

        private static void grupe_ukloni(ApplicationDbContext db, UserProfileInfo user)
        {
            MemberGroupmembers group_m = db.MemberGroupmembersSet.Include("Group").FirstOrDefault(x => x.MemberDetails.Id == user.Id);
            if (group_m != null)
            {
                MemberGroups group = group_m.Group;
                if (group != null)
                {
                    db.MemberGroupsSet.Remove(group);
                    db.MemberGroupmembersSet.RemoveRange(db.MemberGroupmembersSet.Include("Group").Where(x => x.Group.Id == group.Id));
                }
            }
        }

        private static Dictionary<string, List<UserProfileInfo>> jezici(UserProfileInfo User, List<UserProfileInfo> reza)
        {
            Dictionary<string, List<UserProfileInfo>> nesto2 = new Dictionary<string, List<UserProfileInfo>>();
            foreach (Language l in User.Languages)
            {
                List<UserProfileInfo> nesto = new List<UserProfileInfo>();
                foreach (var v in reza)
                {
                    string s = v.Search.ToString();
                    if (v.Languages.Contains(l)) nesto.Add(v);


                }
                if (nesto.Count >= 5) nesto2.Add(l.Jezik, nesto);
            }

            return nesto2;
        }

        private static List<List<UserProfileInfo>> zone(UserProfileInfo User, List<UserProfileInfo> reza)
        {
            List<List<UserProfileInfo>> nesto2 = new List<List<UserProfileInfo>>();
            List<UserProfileInfo> nestoa = new List<UserProfileInfo>();
            List<UserProfileInfo> nestob = new List<UserProfileInfo>();
            int pocetna_z = zone_sati(User.TimeZone);
            foreach (UserProfileInfo u in reza)
            {
                int pom = zone_sati(u.TimeZone);
                if (pom == pocetna_z) { nestoa.Add(u); nestob.Add(u); }
                else if (pom - 1 == pocetna_z) nestoa.Add(u);
                else if (pom + 1 == pocetna_z) nestob.Add(u);
            }
            if (nestoa.Count >= 5) nesto2.Add(nestoa);
            if (nestob.Count >= 5) nesto2.Add(nestob);
            return nesto2;
        }

        private static Dictionary<int, List<UserProfileInfo>> vremena(UserProfileInfo User, List<UserProfileInfo> reza)
        {
            Dictionary<int, List<UserProfileInfo>> nesto2 = new Dictionary<int, List<UserProfileInfo>>();

            int min = AppConstants.Sats.IndexOf((
       from p in AppConstants.Sats
       where p.SatID == User.MinTime
       select p).First());
            int max = AppConstants.Sats.IndexOf((
       from p in AppConstants.Sats
       where p.SatID == User.MaxTime
       select p).First());

            for (int i = min; i <= max; i++)
            {
                List<UserProfileInfo> nesto = new List<UserProfileInfo>();
                foreach (UserProfileInfo u in reza)
                {
                    if (u.MinTime <= AppConstants.Sats[i].SatID && u.MaxTime >= AppConstants.Sats[i].SatID) nesto.Add(u);
                }
                if (nesto.Count >= 5) nesto2.Add(AppConstants.Sats[i].SatID, nesto);
            }

            return nesto2;
        }

        private static Dictionary<int, List<UserProfileInfo>> ratingzi(UserProfileInfo User, List<UserProfileInfo> reza)
        {
            Dictionary<int, List<UserProfileInfo>> nesto2 = new Dictionary<int, List<UserProfileInfo>>();
            int min = AppConstants.MMRs.IndexOf((
    from p in AppConstants.MMRs
    where p.MMRID == User.MinRating
    select p).First());
            int max = AppConstants.MMRs.IndexOf((
       from p in AppConstants.MMRs
       where p.MMRID == User.Rating
       select p).First());

            for (int i = min; i <= max; i++)
            {
                List<UserProfileInfo> nesto = new List<UserProfileInfo>();
                foreach (UserProfileInfo u in reza)
                {
                    if (u.MinRating <= AppConstants.MMRs[i].MMRID && u.Rating >= AppConstants.MMRs[i].MMRID) nesto.Add(u);
                }
                if (nesto.Count >= 5) nesto2.Add(AppConstants.MMRs[i].MMRID, nesto);
            }

            return nesto2;
        }


        private static Dictionary<int, List<UserProfileInfo>> godine(UserProfileInfo User, List<UserProfileInfo> reza)
        {
            Dictionary<int, List<UserProfileInfo>> nesto2 = new Dictionary<int, List<UserProfileInfo>>();
            int min = AppConstants.godine.IndexOf((
    from p in AppConstants.godine
    where p.GodinaID == User.MinAge
    select p).First());
            int max = AppConstants.godine.IndexOf((
       from p in AppConstants.godine
       where p.GodinaID == User.Age
       select p).First());

            for (int i = min; i <= max; i++)
            {
                List<UserProfileInfo> nesto = new List<UserProfileInfo>();
                foreach (UserProfileInfo u in reza)
                {
                    if (u.MinAge <= AppConstants.godine[i].GodinaID && u.Age >= AppConstants.godine[i].GodinaID) nesto.Add(u);
                }
                if (nesto.Count >= 5) nesto2.Add(AppConstants.MMRs[i].MMRID, nesto);
            }

            return nesto2;
        }


        private static Tuple<List<UserProfileInfo>, List<List<UserProfileInfo>>> groupe(UserProfileInfo User, List<UserProfileInfo> reza, IEnumerable<MemberGroupmembers> grupe)
        {



            Tuple<List<UserProfileInfo>, List<List<UserProfileInfo>>> povrat = new Tuple<List<UserProfileInfo>, List<List<UserProfileInfo>>>(new List<UserProfileInfo>(reza), new List<List<UserProfileInfo>>());
            List<int> pamcenje = new List<int>();
            foreach (UserProfileInfo u in reza)
            {

                if (u.Id == User.Id) { continue; } // onaj tko traži solo ne provjeravamo da li ima grupu
                MemberGroupmembers pom = grupe.FirstOrDefault(x => x.MemberDetails.Id == u.Id);
                if (pom == null) continue; // igrac nije ni u jednoj grupi
                else // igrac ima grupu
                {
                    if (pamcenje.Contains(pom.Group.Id)) continue;
                    pamcenje.Add(pom.Group.Id); // da se ne ponavljaju iste grupe, otprilike objasnjenje
                    bool brisi = false;
                    IEnumerable<MemberGroupmembers> pom2 = grupe.Where(x => x.Group.Id == pom.Group.Id);
                    List<UserProfileInfo> popis = new List<UserProfileInfo>();
                    foreach (MemberGroupmembers m in pom2)
                    {
                        popis.Add(m.MemberDetails);
                        if (reza.FirstOrDefault(x => x.Id == m.MemberDetails.Id) == null) { brisi = true; break; } // ako jedan od clanova grupe ne postoji u pretrazi brisi sve clanove
                    }

                    if (brisi)
                    {
                        foreach (MemberGroupmembers m in pom2)
                        {
                            if (povrat.Item1.Contains(m.MemberDetails))
                                povrat.Item1.Remove(m.MemberDetails);
                        }
                    }
                    else
                    {
                        povrat.Item2.Add(popis);
                    }
                }
            }


            //if (povrat.Item1.Count < 5)
            //    return new Tuple<List<UserProfileInfo>, List<List<UserProfileInfo>>>(new List<UserProfileInfo>(reza), new List<List<UserProfileInfo>>());
            return povrat;
        }


        private static List<List<int>> stvori_raspored(List<UserProfileInfo> membri)
        {
            List<List<int>> pomoc = new List<List<int>>();
            for (int i = 0; i < membri.Count; i++)
            {
                List<int> pom = new List<int>();
                foreach (Position p in membri[i].Positions)
                    pom.Add(p.PositionNumber);

                if (!pom.Contains(membri[i].FavPosition)) { pom.Insert(0, membri[i].FavPosition); }
                else
                {
                    int index = pom.IndexOf(membri[i].FavPosition);
                    int tmp = pom[index];
                    pom[index] = pom[0];
                    pom[0] = tmp;
                }
                pomoc.Add(pom);
            }

            return stvori_raspored_pom(pomoc);
        }

        private static List<List<int>> stvori_raspored_pom(List<List<int>> membri)
        {
            List<List<int>> grupa = new List<List<int>>();
            for (int i = 0; i < membri[0].Count; i++)
            {
                grupa.Add(new List<int> { membri[0][i] });
            }
            for (int broj = 1; broj < membri.Count; broj++)
            {
                List<List<int>> grupa2 = new List<List<int>>();
                List<string> pamcenje2 = new List<string>();
                for (int i = 0; i < grupa.Count; i++)
                {
                    for (int j = 0; j < membri[broj].Count; j++)
                    {

                        //Console.WriteLine("{ " + grupa[i][0] + ", " + membri[broj][j] + " }");
                        List<int> pomoc = new List<int>(grupa[i]);
                        if (pomoc.Contains(membri[broj][j])) continue;
                        pomoc.Add(membri[broj][j]);
                        int[] pom = pomoc.ToArray();

                        Array.Sort(pom);
                        var builder = new StringBuilder();
                        Array.ForEach(pom, x => builder.Append(x));
                        var res = builder.ToString();
                        if (pamcenje2.Contains(res)) continue;
                        pamcenje2.Add(res);

                        grupa2.Add(pomoc);
                    }
                }
                grupa = new List<List<int>>(grupa2);
            }

            return grupa;
        }

        public static int zone_sati(int br)
        {
            if (br == 0) return 20;
            ReadOnlyCollection<TimeZoneInfo> timeZonest = TimeZoneInfo.GetSystemTimeZones();
            return timeZonest[br-1].BaseUtcOffset.Hours;

        }

        private static bool slicno(ICollection<Language> a, ICollection<Language> b)
        {
            bool hasCommonElements = a.Intersect(b).Count() > 0;
            return hasCommonElements;
        }

        public static string[] generiraj()
        {

            string[] povrat = new string[2];
            Random rnd = new Random();
            int prvi = rnd.Next(AppConstants.imenice.Count);
            int drugi = rnd.Next(AppConstants.pridjevi.Count);

            povrat[0] = UppercaseFirst(AppConstants.pridjevi[drugi]) + " " + UppercaseFirst(PluralizationService
        .CreateService(new CultureInfo("en-US"))
        .Pluralize(AppConstants.imenice[prvi]));

            string[] pomoc = povrat[0].Split(' ');

            string ispis = "";

            foreach (string s in pomoc)
                ispis += s[0];
            povrat[1] = ispis;

            return povrat;
        }

        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        // Returns maximum number of matching from M to N
        public static int maxBPM(int[,] bpGraph, int M, int N)
        {
            // An array to keep track of the applicants assigned to
            // jobs. The value of matchR[i] is the applicant number
            // assigned to job i, the value -1 indicates nobody is
            // assigned.

            int[] matchR = new int[N];

            // Initially all jobs are available
            for (int i = 0; i < matchR.Length; i++) matchR[i] = -1;


            int result = 0; // Count of jobs assigned to applicants
            for (int u = 0; u < M; u++)
            {
                // Mark all jobs as not seen for next applicant.
                int[] seen = new int[N];
                //for (int i = 0; i < seen.Length; i++) seen[i] = 0;

                // Find if the applicant 'u' can get a job
                if (bpm(bpGraph, u, seen, matchR, M, N))
                    result++;
            }
            return result;
        }
        // A DFS based recursive function that returns true if a
        // matching for vertex u is possible
        private static bool bpm(int[,] bpGraph, int u, int[] seen, int[] matchR, int M, int N)
        {

            // Try every job one by one
            for (int v = 0; v < N; v++)
            {
                // If applicant u is interested in job v and v is
                // not visited
                if (bpGraph[u, v] == 1 && seen[v] == 0)
                {
                    seen[v] = 1; // Mark v as visited

                    // If job 'v' is not assigned to an applicant OR
                    // previously assigned applicant for job v (which is matchR[v]) 
                    // has an alternate job available. 
                    // Since v is marked as visited in the above line, matchR[v] 
                    // in the following recursive call will not get job 'v' again
                    if (matchR[v] < 0 || bpm(bpGraph, matchR[v], seen, matchR, M, N))
                    {
                        matchR[v] = u;
                        return true;
                    }
                }
            }
            return false;
        }


    }


}