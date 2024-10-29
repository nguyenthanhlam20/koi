using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service.DTO.Appointment
{
    public class AppointmentSearchCriteria
    {
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Status { get; set; }
        public decimal? TotalCostFrom { get; set; }
        public decimal? TotalCostTo { get; set; }
    }
}
