using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Position
    {
        public int PositionID { get; set; }
        public int PositionNumber { get; set; }
        public virtual ICollection<UserProfileInfo> UserProfiles { get; set; }
    }
}