using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class GroupDetailsModel
    {
        public MemberGroups groupDetails { get; set; }
        public IList<MemberGroupmembers> groupMembers { get; set; }
        public string submitButton { get; set; }
        public IList<UserProfileInfo> UserSrchResult { get; set; }
        public string txtSearchUserNm { get; set; }
        public int loggedInUserId { get; set; }
        public bool showInvite { get; set; }
    }
    public class GroupHomeModel
    {
        public MemberGroups myGroup { get; set; }
        public IList<MemberGroupmembers> myMembers { get; set; }
        public MemberGroupmembers member { get; set; }
        //public Dictionary<int,int> votes {get;set;}
    }
}