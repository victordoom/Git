using Microsoft.AspNetCore.Http;

namespace WebAdmin.Models
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}
