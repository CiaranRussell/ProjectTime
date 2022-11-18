using ProjectTime.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Utility
{
    public class CustomValidation
    {
        public class CheckBoxRequired : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                var userRole = (UserRolesViewModel)validationContext.ObjectInstance;

                if (!userRole.IsSelected)
                {
                    return new ValidationResult(ErrorMessage ?? "User must have a Role");
                }
                return ValidationResult.Success;
            }
        }
    }
}
