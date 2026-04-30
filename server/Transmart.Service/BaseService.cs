using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;

namespace TranSmart.Service
{
	public interface IBaseService<T> where T : BaseEntity
	{
		Task<IEnumerable<T>> GetList(string orderBy);
		Task<IPaginate<T>> GetPaginate(BaseSearch baseSearch);
		Task<T> GetById(Guid id);
		IEnumerable<T> GetBySpecification(ISpecification<T> specification);
		Task<Result<T>> AddAsync(T item);
		Task<Result<T>> AddOnlyAsync(T item);
		Task<Result<T>> UpdateAsync(T item);
		Task<Result<T>> UpdateOnlyAsync(T item);
		Task CustomValidation(T item, Result<T> result);
		Task<IEnumerable<T>> Search(string name);
		DateTime TimeStamp();
		Task<Tuple<int, string>> GetNextSequenceNo(string entity, string attribute);
	}

	public class BaseService<T> : IBaseService<T> where T : BaseEntity
	{
		private readonly IUnitOfWork _uow;
		private readonly ISequenceNoService _sequence;

		public IUnitOfWork UOW { get { return _uow; } }
		public ISequenceNoService Sequence { get { return _sequence; } }	

		public BaseService(IUnitOfWork uow)
		{
			_uow = uow;
		}
		public BaseService(IUnitOfWork uow, ISequenceNoService sequenceNoService)
		{
			_uow = uow;
			_sequence = sequenceNoService;
		}
		public DateTime TimeStamp() => DateTime.Now;
		public virtual async Task<IEnumerable<T>> GetList(string orderBy)
		{
			return await UOW.GetRepositoryAsync<T>().GetListAsync(orderBy);
		}
		/// <summary>
		/// Get pagination entity. Old name is "GetAll"
		/// </summary>
		/// <param name="baseSearch"></param>
		/// <returns></returns>
		public async virtual Task<IPaginate<T>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<T>().GetPaginateAsync(index: baseSearch.Page, size: baseSearch.Size);
		}

		public virtual Task Validation(T item, Result<T> result)
		{
			return Task.CompletedTask;
		}

		public virtual Task CustomValidation(T item, Result<T> result)
		{
			return Task.CompletedTask;
		}

		public async Task<Result<T>> AddOnlyAsync(T item)
		{
			Result<T> result = new(false);
			if (!result.HasError)
			{
				try
				{
					await UOW.GetRepositoryAsync<T>().AddAsync(item);
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
					result.ReturnValue = item;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			return result;
		}
		public virtual async Task<Result<T>> AddAsync(T item)
		{
			Result<T> result = new(false);
			await OnBeforeAdd(item, result);
			if (!result.HasError)
			{
				try
				{
					await UOW.GetRepositoryAsync<T>().AddAsync(item);
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
					result.ReturnValue = item;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			await OnAfterAdd(item, result);
			return result;
		}
		public virtual async Task OnBeforeAdd(T item, Result<T> executionResult)
		{
			await Validation(item, executionResult);
			await CustomValidation(item, executionResult);
		}
		public virtual Task OnAfterAdd(T item, Result<T> executionResult)
		{
			return Task.CompletedTask;
		}

		public virtual async Task<Result<T>> UpdateOnlyAsync(T item)
		{
			var result = new Result<T>(false);
			try
			{
				UOW.GetRepositoryAsync<T>().UpdateAsync(item);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = item;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public virtual async Task<Result<T>> UpdateAsync(T item)
		{
			var result = new Result<T>(false);
			await OnBeforeUpdate(item, result);
			if (!result.HasError)
			{
				try
				{
					UOW.GetRepositoryAsync<T>().UpdateAsync(item);
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
					result.ReturnValue = item;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			await OnAfterUpdate(item, result);
			return result;
		}
		public virtual async Task OnBeforeUpdate(T item, Result<T> executionResult)
		{
			await Validation(item, executionResult);
			await CustomValidation(item, executionResult);
		}

		public virtual Task OnAfterUpdate(T item, Result<T> executionResult)
		{
			return Task.CompletedTask;
		}
		public virtual async Task<T> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<T>().SingleAsync(x => x.ID == id);
		}
		public virtual IEnumerable<T> GetBySpecification(ISpecification<T> specification)
		{
			return UOW.GetRepository<T>().GetAll(specification);
		}
		public async Task<Tuple<int, string>> GetNextSequenceNo(string entity, string attribute)
		{
			SequenceNo sequenceNo = await UOW.GetRepositoryAsync<SequenceNo>().SingleAsync(x => x.EntityName == entity && x.Attribute == attribute);

			int seq = sequenceNo.NextNo++;
			UOW.GetRepository<SequenceNo>().Update(sequenceNo);
			return new Tuple<int, string>(seq, $"{(string.IsNullOrEmpty(sequenceNo.Prefix) ? "A" : sequenceNo.Prefix)}{seq.ToString().PadLeft(5, '0')}");
		}

		public virtual Task<IEnumerable<T>> Search(string name)
		{
			throw new NotImplementedException();
		}
	}
}
