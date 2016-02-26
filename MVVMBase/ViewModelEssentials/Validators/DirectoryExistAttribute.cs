using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MyCommander.Validators
{
    public class DirectoryExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var strValue = value as string;

            return Directory.Exists(strValue) ? ValidationResult.Success :
                    new ValidationResult("Path should exist");
        }
    }
}
