using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public class EmployeeContactReportModel : BaseModel
    {
        public string PersonName { get; set; }
        public int HumanRelation { get; set; }
        public string Department { get; set; }
        public string ContactNo { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string No { get; set; }

        public string HumanRelationTxt
		{
			get
			{
				if (HumanRelation == 1)
				{
					return "Father";
				}
				else if (HumanRelation == 2)
				{
					return "Mother";
				}
				else if (HumanRelation == 3)
				{
					return "Spouse";
				}
				else if (HumanRelation == 4)
				{
					return "Child1";
				}
				else if (HumanRelation == 5)
				{
					return "Child2";
				}
				else
				{
					return HumanRelation == 6 ? "Child3" : "";
				}
			}
		}

    }
}

