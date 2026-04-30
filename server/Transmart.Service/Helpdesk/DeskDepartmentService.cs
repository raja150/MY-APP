using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Helpdesk
{
    public partial interface IDeskDepartmentService : IBaseService<DeskDepartment>
    {
        Task<IEnumerable<DeskDepartment>> GetDepartments();

    }
    public partial class DeskDepartmentService : BaseService<DeskDepartment>, IDeskDepartmentService
    {
        public async Task<IEnumerable<DeskDepartment>> GetDepartments()
        {
            return await UOW.GetRepositoryAsync<DeskDepartment>().GetAsync(x => x.Status);
        }
        public override async Task CustomValidation(DeskDepartment item, Result<DeskDepartment> result)
        {
            if (await UOW.GetRepositoryAsync<DeskDepartment>().HasRecordsAsync(x => x.Department == item.Department && x.ID != item.ID))
            {
                result.AddMessageItem(new MessageItem(nameof(DeskDepartmentModel.Department), Resource.Name_already_exists));
            }
			if (await UOW.GetRepositoryAsync<DeskGroupEmployee>().HasRecordsAsync(x => x.EmployeeId == item.ManagerId && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(nameof(DeskDepartmentModel.ManagerId), Resource.Group_Emp_Not_Allowed_As_Manager));
			}

		}
	}

}

