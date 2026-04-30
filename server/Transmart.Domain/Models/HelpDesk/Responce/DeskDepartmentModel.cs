using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.HelpDesk.Responce
{
    public class DeskDepartmentModel:BaseModel
    {
        public string Department { get; set; }
        public Guid DepartmentId { get; set; }  

    }
}
