using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Organization
{
	public partial interface IEmployeeDeviceService : IBaseService<EmployeeDevice>
	{

	}
	public class EmployeeDeviceService : BaseService<EmployeeDevice>, IEmployeeDeviceService
	{
		public EmployeeDeviceService(IUnitOfWork uow) : base(uow)
		{

		}
		public override async Task CustomValidation(EmployeeDevice item, Result<EmployeeDevice> result)
		{
			if ((item.IsActZeroInstalled || item.IsK7Installed) && item.InstalledOn == null)
			{
				result.AddMessageItem(new MessageItem(nameof(EmployeeDevice.InstalledOn), Resource.Installed_On_Is_Required));
			}
			if (item.IsUninstalled && item.UninstalledOn == null)
			{
				result.AddMessageItem(new MessageItem(nameof(EmployeeDevice.UninstalledOn), Resource.Uninstalled_On_Is_Required));
			}
			if (await UOW.GetRepositoryAsync<EmployeeDevice>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
			&& x.ComputerType == item.ComputerType && x.ID != item.ID))
			{
				result.AddMessageItem(new MessageItem(Resource.Employee_Already_Exists, FeedbackType.Warning));
			}
		}
		public override async Task<EmployeeDevice> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<EmployeeDevice>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
							   .Include(x => x.Employee).ThenInclude(x => x.Designation));
		}
		public override async Task<IPaginate<EmployeeDevice>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<EmployeeDevice>().GetPageListAsync(
				 predicate: x => string.IsNullOrEmpty(baseSearch.Name)
					 || x.Employee.Name.Contains(baseSearch.Name),
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
								.Include(x => x.Employee).ThenInclude(x => x.Designation),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Employee.No", ascending: !baseSearch.IsDescend);
		}
	}
}
