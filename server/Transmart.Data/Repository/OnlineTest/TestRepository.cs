using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.OnlineTest;

namespace TranSmart.Data.Repository.OnlineTest
{
	public interface ITestRepository
	{
		Task<IEnumerable<dynamic>> Test(Guid empId, Guid depId, Guid desgnId);
	}
	public class TestRepository : ITestRepository
	{
		private readonly TranSmartContext _context;

		public TestRepository(TranSmartContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<dynamic>> Test(Guid empId, Guid depId, Guid desgnId)
		{
			return await (from td in _context.TestDepartment
						  join p in _context.Paper on td.PaperId equals p.ID
						  join rq in _context.ResultQuestion.Where(x => x.EmployeeId == empId) on p.ID equals rq.PaperId into results
						  from rq in results.DefaultIfEmpty()
						  where td.DepartmentId == depId && !td.IsDelete && td.Paper.MoveToLive &&
						  p.StartAt.Date <= DateTime.Now.Date && DateTime.Now.Date <= p.EndAt.Date
						  select new
						  {
							  p.Name,
							  p.ID,
							  p.Duration,
							  p.EndAt,
							  p.StartAt,
							  p.Status,
							  p.MoveToLive,
							  //rq.ReTake,
							  reTake = rq != null && rq.ReTake,
							  isStarted = rq != null && rq.EmployeeId == empId,
						  }).Union(from tds in _context.TestDesignation
								   join p in _context.Paper on tds.PaperId equals p.ID
								   join rq in _context.ResultQuestion.Where(x => x.EmployeeId == empId) on p.ID equals rq.PaperId into results
								   from rq in results.DefaultIfEmpty()
								   where tds.DesignationId == desgnId && !tds.IsDelete && tds.Paper.MoveToLive &&
								   p.StartAt.Date <= DateTime.Now.Date && DateTime.Now.Date <= p.EndAt.Date
								   select new
								   {
									   p.Name,
									   p.ID,
									   p.Duration,
									   p.EndAt,
									   p.StartAt,
									   p.Status,
									   p.MoveToLive,
									   reTake = rq != null && rq.ReTake,
									   isStarted = rq != null && rq.EmployeeId == empId,

								   }).Union(from te in _context.TestEmployee
											join p in _context.Paper on te.PaperId equals p.ID
											join rq in _context.ResultQuestion.Where(x => x.EmployeeId == empId) on p.ID equals rq.PaperId into results
											from rq in results.DefaultIfEmpty()
											where te.EmployeeId == empId && !te.IsDelete && te.Paper.MoveToLive &&
											p.StartAt.Date <= DateTime.Now.Date && DateTime.Now.Date <= p.EndAt.Date
											select new
											{
												p.Name,
												p.ID,
												p.Duration,
												p.EndAt,
												p.StartAt,
												p.Status,
												p.MoveToLive,
												reTake = rq != null && rq.ReTake,
												isStarted = rq != null && rq.EmployeeId == empId,
											}).OrderByDescending(x => x.StartAt).ToListAsync();
		}
	}
}
