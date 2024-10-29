﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace KoiVetenary.Data.Models
{
    public partial class Service
    {
        public Service()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? Duration { get; set; }
        public decimal? BasePrice { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public string RequiredEquipment { get; set; }
        public string SpecialInstructions { get; set; }
        public string ServiceImg { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
    }
}