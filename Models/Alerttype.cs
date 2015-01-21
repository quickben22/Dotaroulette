using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication5.Models
{
    public class Alerttype
    {
        public System.Int32 Id { get; set; }
        public System.Int32 Paramcount { get; set; }
        public System.String SupportEmail { get; set; }
        public System.String Template { get; set; }
        public virtual ICollection<MemberAlert> MemberAlerts { get; set; }
        public System.String Name { get; set; }
    }
}
