using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MyCommander.Validators
{
    public class DirectoryExistAttribute : ValidationAttribute
    {
        private readonly static ValidationResult PathShouldExist = new ValidationResult("Path should exist");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return Directory.Exists(value as string) ? ValidationResult.Success : PathShouldExist;
        }
    }
}
