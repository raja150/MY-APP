using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models
{
    public class ImageModel
    {
        public Guid EmployeeId { get; set; }
        public string ImageName { get; set; }   
        public byte ImageFlag { get; set; }
        public byte[] ImagePicture { get; set; }
        public byte[] ResizeImagePicture { get; set; }
        public IFormFile File { get; set; } 
    }
}
