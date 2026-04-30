using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Organization
{
    public partial class EmployeeFamily
    {
        public void UpdateContactDetails(EmployeeFamily other)
        {
            this.PersonName = other.PersonName;
            this.HumanRelation = other.HumanRelation;
            this.ContactNo = other.ContactNo;
        }
    }
}
