using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models.Helpers;

namespace WebApplication5.Models.ViewModels
{
    public class UserProfileEditViewModel
    {
        public UserProfileEditViewModel()
        {
            Positions = new Collection<AssignedPositionData>();
        }


        public int UserProfileID { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string avatar_url { get; set; }
        public string avatar_url2 { get; set; }
        public string steam_url { get; set; }
        public int MinRating { get; set; }
        [IsNumberAfterAttribute("MinRating", true, ErrorMessage = "Your rating has to be greater or equal to minimum rating!")]
        public int Rating { get; set; }
        [IsNumberAfterAttribute("Rating", true, ErrorMessage = "Maximum rating has to be greater or equal to your rating!")]
        public int MaxRating { get; set; }

        public virtual ICollection<AssignedPositionData> Positions { get; set; }
        public int FavPosition { get; set; }
        public List<int> Languages { get; set; }
        public MultiSelectList LangPopis { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The 'Time Zone' attribute is required")]
        public int TimeZone { get; set; }
        public int MinTime { get; set; }
        [IsNumberAfterAttribute("MinTime", true, ErrorMessage = "Maximum time has to be greater or equal to your Minumum time!")]
        public int MaxTime { get; set; }
        public int Microphone { get; set; }
        public int Goal { get; set; }
        public int MinAge { get; set; }
        [IsNumberAfterAttribute("MinAge", true, ErrorMessage = "Your age has to be greater or equal to minimum age!")]
        public int Age { get; set; }
        [IsNumberAfterAttribute("Age", true, ErrorMessage = "Maximum age has to be greater or equal to your age!")]
        public int MaxAge { get; set; }
        public string About { get; set; }
        public int Search { get; set; }
       
    }

    public class AssignedPositionData
    {
        public int PositionID { get; set; }
        public int PositionNumber { get; set; }
        public bool Assigned { get; set; }
    }


}