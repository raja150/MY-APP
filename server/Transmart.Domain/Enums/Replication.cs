using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Enums
{
	public enum ReplicationType
	{
		Add = 1,
		Update,
		Delete,
	}
	public enum ReplicationCategory
	{
		Designation = 1,
		Empployee,
		Salary,
		User
	}
}
