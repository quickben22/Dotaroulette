using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class MemberAlert
    {
        public System.Int32 Id { get; set; }
        [Column(TypeName = "DateTime2")]
        public System.DateTime Version { get; set; }
        public Nullable<System.Int32> Ishidden { get; set; }
        public System.String Message { get; set; }
        public virtual ICollection<Alerttype> AlertType { get; set; }
        public UserProfileInfo PUser { get; set; }

    }
}