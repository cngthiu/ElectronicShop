﻿using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        //Validate file hinh anh 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg" };
                bool result = extensions.Any(x=>extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Allowed extensions are jpg or png or jpeg");
                }

            }
            return ValidationResult.Success;
        }
    }
}
