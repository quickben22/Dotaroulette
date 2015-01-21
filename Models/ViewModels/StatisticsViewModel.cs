using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models.ViewModels
{
    public class StatisticsViewModel
    {

        public Dictionary<string, int> MinRating { get; set; }
        public Dictionary<string, int> Rating { get; set; }
        public Dictionary<string, int> MaxRating { get; set; }
        public Dictionary<int, int> Positions { get; set; }
        public Dictionary<int, int> FavPosition { get; set; }
        public Dictionary<string, int> TimeZone { get; set; }
        public Dictionary<string, int> Languages { get; set; }
        public Dictionary<string, int> MinTime { get; set; }
        public Dictionary<string, int> MaxTime { get; set; }
        public Dictionary<string, int> Microphone { get; set; }
        public Dictionary<string, int> Goal { get; set; }
        public Dictionary<string, int> MinAge { get; set; }
        public Dictionary<string, int> Age { get; set; }
        public Dictionary<string, int> MaxAge { get; set; }

    }
}