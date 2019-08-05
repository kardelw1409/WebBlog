using System;

namespace WebBlog.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool IsRequestIdVisible => !string.IsNullOrEmpty(RequestId);
    }
}