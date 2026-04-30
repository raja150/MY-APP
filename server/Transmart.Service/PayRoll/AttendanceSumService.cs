using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Service.PayRoll
{
	public interface IAttendanceSumService : IBaseService<AttendanceSum>
	{
		Task<Result<AttendanceSum>> AddBulk(List<AttendanceSum> items, int year, int month);
	}
	public class AttendanceSumService : BaseService<AttendanceSum>,IAttendanceSumService
	{
		public AttendanceSumService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<AttendanceSum>> AddBulk(List<AttendanceSum> items, int year, int month)
		{
			Result<AttendanceSum> result = new();
			//Conditions will Update
			var entityList = await UOW.GetRepositoryAsync<AttendanceSum>().GetAsync(x => x.Month == month && x.Year == year);

			//Add or Update Components  
			CollectionCompareResult<AttendanceSum> compareList = entityList.Compare(items, (x, y) => x.EmployeeId.Equals(y.EmployeeId));
			foreach (AttendanceSum entity in compareList.Same)
			{
				AttendanceSum editItem = items.FirstOrDefault(x => x.EmployeeId == entity.EmployeeId);
				if (editItem != null && !entity.Equals(editItem))
				{
					entity.Update(editItem);
					UOW.GetRepositoryAsync<AttendanceSum>().UpdateAsync(entity);
				}
			}
			foreach (AttendanceSum comp in compareList.Added)
			{
				await UOW.GetRepositoryAsync<AttendanceSum>().AddAsync(comp);
			}

			foreach (AttendanceSum comp in compareList.Deleted)
			{
				UOW.GetRepositoryAsync<AttendanceSum>().DeleteAsync(comp);
			}

			await UOW.SaveChangesAsync();
			return result;
		}
	}
}
