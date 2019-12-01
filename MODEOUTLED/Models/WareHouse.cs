//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onsoft.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WareHouse
    {
        public WareHouse()
        {
            this.Exports = new HashSet<Export>();
            this.Imports = new HashSet<Import>();
            this.MemberWareHouses = new HashSet<MemberWareHouse>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
    
        public virtual ICollection<Export> Exports { get; set; }
        public virtual ICollection<Import> Imports { get; set; }
        public virtual ICollection<MemberWareHouse> MemberWareHouses { get; set; }
    }
}
