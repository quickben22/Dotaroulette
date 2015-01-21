using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class UserProfileInfo
    {
        
        public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string avatar_url { get; set; }
        public string avatar_url2 { get; set; }
        public string steam_url { get; set; }
        public int MinRating { get; set; }
        public int Rating { get; set; }
        public int MaxRating { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public int FavPosition { get; set; }
        public virtual List<Language> Languages { get; set; }
        public int TimeZone { get; set; }
        public int MinTime { get; set; }
        public int MaxTime { get; set; }
        public int Microphone { get; set; }
        public int Goal { get; set; }
        public int Search { get; set; }
        public int MinAge { get; set; }
        public int Age { get; set; }
        public int MaxAge { get; set; }
        public string About { get; set; }
        //public virtual ICollection<Friend> Friends { get; set; }
        public MemberGroups Team { get; set; }
    }
}