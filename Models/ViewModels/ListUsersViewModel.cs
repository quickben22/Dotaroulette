using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebApplication5.Models.ViewModels
{
    public class ListUsersViewModel
    {

        public IPagedList<UserProfileInfo> users { get; set; }
        public int MinRating { get; set; }
        public int MaxRating { get; set; }
        public int Position { get; set; }
        public int Language { get; set; }
        public int TimeZone { get; set; }
        public int MinTime { get; set; }
        public int MaxTime { get; set; }
        public int Goal { get; set; }
        public bool Search { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}