using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebBlog.ApplicationCore.Attributes
{
    public class ValidateImageAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int maxContentLength = 1024 * 1024;
            string[] allowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };

            var file = value as IFormFile;

            if (file == null)
            {
                ErrorMessage = "Please upload your image!";
                return false;
            } 
            else if (!allowedFileExtensions.Contains((file != null) ?
                file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()
                : string.Empty))
            {
                ErrorMessage = "Please upload image of type: " +
                    string.Join(", ", allowedFileExtensions);
                return false;
            }
            else if (file.Length > maxContentLength)
            {
                ErrorMessage = "Your image is too large, maximum allowed size is : "
                    + (maxContentLength / 1024).ToString() + "KB";
                return false;
            }
            else
            {
                return true;
            }
                
        }
    }
}
