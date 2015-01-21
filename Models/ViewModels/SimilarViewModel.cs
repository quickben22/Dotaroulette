using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models.ViewModels
{
    public class SimilarViewModel
    {

        public int Rating { get; set; }
        public int Age { get; set; }
        public int Time { get; set; }
        public int TimeZone { get; set; }
        public int Goal { get; set; }
        public int Sve { get; set; }
        public int search { get; set; }
        public string UserRating { get; set; }
        public string UserAge { get; set; }
        public string UserTime { get; set; }
        public string UserTimezone { get; set; }
        public string UserGoal { get; set; }
        public UserProfileInfo User { get; set; }
    }
}