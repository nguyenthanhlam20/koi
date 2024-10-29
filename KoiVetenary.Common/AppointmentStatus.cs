using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Common
{
    public class AppointmentStatus
    {
        public static readonly string Pending = "Pending";
        public static readonly string Confirmed = "Confirmed";
        public static readonly string InProgress = "InProgress";
        public static readonly string Completed = "Completed";
        public static readonly string Canceled = "Canceled";

        // Optional: Validation method to check if a string is a valid status
        public static bool IsValidStatus(string status)
        {
            return status == Pending || status == Confirmed || status == InProgress ||
                   status == Completed || status == Canceled;
        }
    }

}
