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
    
    public partial class RoomStatus
    {
        public RoomStatus()
        {
            this.Room = new HashSet<Room>();
        }
    
        public int StatusID { get; set; }
        public string Contents { get; set; }
    
        public virtual ICollection<Room> Room { get; set; }
    }
}
