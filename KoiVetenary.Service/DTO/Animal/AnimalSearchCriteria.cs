using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service.DTO.Animal
{
    public class AnimalSearchCriteria
    {
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? OwnerFirstName { get; set; }
        public string? OwnerLastName { get; set; }
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public decimal? WeightFrom { get; set; }
        public decimal? WeightTo { get; set; }

    }
}
