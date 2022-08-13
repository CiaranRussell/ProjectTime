using System.ComponentModel.DataAnnotations;

namespace ProjectTime.Models
{
    public class CustomValidation
    {
        public class CheckBoxRequired : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                var userRole = (UserRolesViewModel)validationContext.ObjectInstance;

                if (userRole.IsSelected == false)
                {
                    return new ValidationResult(ErrorMessage == null ? "User must have a Role" : ErrorMessage);
                }
                return ValidationResult.Success;
            }
        }
    }
}
