using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models.ViewModels
{
    public class MessagesViewModel
    {

        public IList<MemberAlert> myAlerts { get; set; }
        public IList<MemberRequests> myRequests { get; set; }
    }
}