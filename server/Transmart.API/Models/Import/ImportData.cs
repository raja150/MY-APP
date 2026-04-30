using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.API.Models
{
   public class ImportData
    {   
        public int Type { get; set; } 
        public IFormFile FormFile { get; set; }
        public DateTime? Date { get; set; }  
    }
}
