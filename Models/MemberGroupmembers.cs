using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class MemberGroupmembers
    {
        public System.Int32 Id { get; set; }
        public System.Int32 Aktivnost { get; set; }
        public UserProfileInfo MemberDetails { get; set; }
        public MemberGroups Group { get; set; }
        public System.Int32 Role { get; set; }
        public MemberGroupmembers Vote { get; set; }
        public System.Int32 Votes { get; set; }
    }
}