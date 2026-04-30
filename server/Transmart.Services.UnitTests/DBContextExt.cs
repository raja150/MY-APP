using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using TranSmart.Data;
using TranSmart.Domain.Entities;

namespace Transmart.Services.UnitTests
{
	public static class DBContextExt
	{
		public static Mock<RepositoryAsync<T>> GetRepositoryAsyncDbSet<T>(this Mock<DbContext> context,
			Mock<UnitOfWork<TranSmartContext>> uow, List<T> mockSet) where T : BaseEntity
		{
			context.Setup(x => x.Set<T>()).Returns(mockSet.AsQueryable().BuildMockDbSet().Object);
			var repository = new Mock<RepositoryAsync<T>>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<T>()).Returns(repository.Object);
			repository.Setup(m => m.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>())).Callback((T entity, CancellationToken cancellation) =>
			{
				mockSet.Add(entity);
			});
			repository.Setup(m => m.AddAsync(It.IsAny<T>())).Callback((T entity) =>
			{
				mockSet.Add(entity);
			});
			repository.Setup(m => m.GetAsync(It.IsAny<Expression<Func<T, bool>>>(),
				   It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
				   It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object>>>())).ReturnsAsync(mockSet);
			repository.Setup(m => m.SingleAsync(It.IsAny<Expression<Func<T, bool>>>(),
				   It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
				   It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object>>>(), true)).ReturnsAsync(mockSet.FirstOrDefault());

			repository.Setup(m => m.DeleteAsync(It.IsAny<T>())).Callback((T entity) =>
			{
				mockSet.Remove(entity);
			});
			//repository.Setup(m => m.SumOfDecimalAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, decimal>>>())).ReturnsAsync(mockSet.Sum(It.IsAny<Func<T, decimal>>()));
			return repository;
		}

		public static Mock<RepositoryAsync<T>> SetupSingleAsync<T>(this Mock<RepositoryAsync<T>> repository, List<T> mockSet) where T : BaseEntity
		{
			repository.Setup(m => m.SingleAsync(It.IsAny<Expression<Func<T, bool>>>(),
				  It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
				  It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object>>>(), true)).ReturnsAsync(mockSet.FirstOrDefault());
			return repository;
		}
	}
}
