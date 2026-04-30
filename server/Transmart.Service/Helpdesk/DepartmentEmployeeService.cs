using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Repository.HelpDesk;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models.Cache;

namespace TranSmart.Service.Helpdesk
{
	public interface IDepartmentEmployeeService : IBaseService<DeskGroup>
	{
		Task UpdateTempData();
		Task CheckGroupLinkedToDept(Guid id);
		Task<IEnumerable<DeskGroupEmpCache>> GetGroupEmps();
	}
	public class DepartmentEmployeeService : BaseService<DeskGroup>, IDepartmentEmployeeService
	{
		private readonly ITicketRepository _repository;
		public DepartmentEmployeeService(ITicketRepository ticketRepository, IUnitOfWork uow) : base(uow)
		{
			_repository = ticketRepository;
		}

		public async Task CheckGroupLinkedToDept(Guid id)
		{
			var emp = await UOW.GetRepositoryAsync<DeskGroupEmployee>().SingleAsync(x => x.ID == id);
			if (emp != null)
			{
				bool isLinked = await UOW.GetRepositoryAsync<DepartmentGroup>().HasRecordsAsync(x => x.GroupsId == emp.DeskGroupId);
				if (isLinked)
				{
					await _repository.UpdateTempData();
				}
			}
		}

		public async Task<IEnumerable<DeskGroupEmpCache>> GetGroupEmps()
		{
			return await _repository.GetGroupEmps();
		}

		public async Task UpdateTempData()
		{
			await _repository.UpdateTempData();
		}

	}
}
