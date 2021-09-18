  
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PicNic.Models
{
    public partial class Instructor
    {
        public int InstructorId { get; set; }
        public String UserId { get; set; }
       public String Title { get; set; }
       public bool IsActive { get; set; }
       public DateTime CreatedDateTime { get; set; }
       public int CreatedBy { get; set; }
       public DateTime ModifiedDateTime { get; set; }
       public int ModifiedBy { get; set; }



       
    }
}