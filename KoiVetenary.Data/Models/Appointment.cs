﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace KoiVetenary.Data.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            AppointmentDetails = new HashSet<AppointmentDetail>();
            Payments = new HashSet<Payment>();
        }

        public int AppointmentId { get; set; }
        public int? OwnerId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? AppointmentTime { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Status { get; set; }
        public bool? IsPaid { get; set; }
        public string SpecialRequests { get; set; }
        public string Notes { get; set; }
        public int? TotalEstimatedDuration { get; set; }
        public decimal? TotalCost { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}