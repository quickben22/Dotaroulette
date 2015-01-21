using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class MemberGroups
    {
        public System.Int32 Id { get; set; }
        public Int32 OwnerId { get; set; }
        public System.Int32 Broj_clanova { get; set; }
        public virtual IList<MemberGroupmembers> FKGrpMmbrsGrp { get; set; }
        public System.String Url { get; set; }
        public System.String Name { get; set; }
        public System.String Language { get; set; }
        public System.String About { get; set; }
        public System.String Goal { get; set; }

    }
}