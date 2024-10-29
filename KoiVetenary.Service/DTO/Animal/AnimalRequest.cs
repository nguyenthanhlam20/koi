using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service.DTO.Animal
{
    public class AnimalRequest
    {
        public int AnimalId { get; set; }

        [Required(ErrorMessage = "OwnerId is required.")]
        public int? OwnerId { get; set; }

        [Required(ErrorMessage = "TypeId is required.")]
        public int? TypeId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Species is required.")]
        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "Species must be at least 2 characters long.")]
        public string Species { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "Origin must be at least 2 characters long.")]
        public string Origin { get; set; }

        [CustomValidation(typeof(AnimalRequest), "ValidateDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Age must be greater than 0.")]
        public int? Age { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public decimal? Weight { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Length must be greater than 0.")]
        public decimal? Length { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "Color must be at least 2 characters long.")]
        public string Color { get; set; }

        [StringLength(255, MinimumLength = 2, ErrorMessage = "Distinguishing Marks must be at least 2 characters long.")]
        public string DistinguishingMarks { get; set; }

        //[Url(ErrorMessage = "Please enter a valid URL.")]
        public string ImageUrl { get; set; }

        [RegularExpression("Male|Female|Other", ErrorMessage = "Gender must be either 'Male' or 'Female' or 'Other'.")]
        public string Gender { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateTime? dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth.HasValue && dateOfBirth.Value.Date > DateTime.Now.Date)
            {
                return new ValidationResult("Date of birth cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
