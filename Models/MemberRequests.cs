using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class MemberRequests
    {
        public System.Int32 Id { get; set; }
        public System.String Guid { get; set; }
        public System.String Email { get; set; }
        public UserProfileInfo Senderid { get; set; }
        public System.Int32 Status { get; set; }
        public UserProfileInfo Recipientid { get; set; }
        public System.Int32 TargetPageid { get; set; }
        public System.Int32 Type { get; set; }
        public int Rola { get; set; }

    }
}