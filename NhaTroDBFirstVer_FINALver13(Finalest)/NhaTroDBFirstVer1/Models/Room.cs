//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NhaTroDBFirstVer1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        public Room()
        {
            this.Comments = new HashSet<Comment>();
            this.QLTDDs = new HashSet<QLTDD>();
        }
    
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Address { get; set; }
        public Nullable<double> Price { get; set; }
        public string Description { get; set; }
        public Nullable<double> Area { get; set; }
        public string Image { get; set; }
        public string Moreinfo { get; set; }
        public int UserID { get; set; }
        public int RoomStatus { get; set; }
        public Nullable<double> Long { get; set; }
        public Nullable<double> Lat { get; set; }
        public string HoVaTen { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
    
        public virtual RoomStatus RoomStatus1 { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<QLTDD> QLTDDs { get; set; }
    }
}