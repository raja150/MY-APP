using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities
{
  [Table("SS_Images")]
    public class EmpImage : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public string ImageName { get; set; }
        public byte ImageFlag { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] ResizeImageData { get; set; } 
    }
}
