using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Language
    {
        public int LanguageID { get; set; }
        public string Jezik { get; set; }
        public virtual ICollection<UserProfileInfo> UserProfiles { get; set; }
    }
}