using System.ComponentModel.DataAnnotations;

namespace Pustok.Attributes.ValidationAttributes
{
    public class AllowedFileTypesAttribute:ValidationAttribute
    {
        private string[] _types;
        public AllowedFileTypesAttribute(params string[] types)
        {
            _types = types;
        }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            IFormFile? file = value as IFormFile;

            if (file != null)
            {
                if (!_types.Contains(file.ContentType))
                {
                    string errorMessage = "File must be one of the types: " + String.Join(",", _types);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
