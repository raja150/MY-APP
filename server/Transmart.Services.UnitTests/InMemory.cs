using TranSmart.Core;
using TranSmart.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TranSmart.Services.UnitTests
{
	public class InMemory
	{
		public IUnitOfWork _UOW;
		public TranSmartContext libraryDbContext;

		public IUnitOfWork GetUnitWorkObject(string InMemoryDBName)
		{
			libraryDbContext = GetInMemoryDB(InMemoryDBName);
			_UOW = new UnitOfWork<TranSmartContext>(libraryDbContext);
			return _UOW;
		}
		public TranSmartContext GetInMemoryDB(string InmemoryDBName)
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: InmemoryDBName);

			var dbContextOptions = builder.Options;
			var services = new ServiceCollection();
			//var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
			//httpContextAccessorMock.HttpContext = new DefaultHttpContext
			//{
			//    RequestServices = services.BuildServiceProvider()
			//};
			services.AddScoped<IApplicationUser, ApplicationUser>();

			libraryDbContext = new TranSmartContext(dbContextOptions, new ApplicationUser());
			// libraryDbContext.Database.EnsureDeleted();
			// libraryDbContext.Database.EnsureCreated();
			return libraryDbContext;
		}
	}

	public class ApplicationUser : IApplicationUser
	{
		public string UserId => "InMemory";

		public Guid EmpId => Guid.NewGuid();

		public Guid Id => Guid.NewGuid();
	}

	public class InMemoryFixture : IDisposable
	{
		public IUnitOfWork UnitOfWork;
		public TranSmartContext DbContext;

		public InMemoryFixture()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

			var dbContextOptions = builder.Options;
			var services = new ServiceCollection();
			services.AddScoped<IApplicationUser, ApplicationUser>();
			services.AddSingleton(dbContextOptions);
			DbContext = new TranSmartContext(dbContextOptions, new ApplicationUser());
			DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
			DbContext.Database.EnsureDeleted();
			DbContext.Database.EnsureCreated();
			UnitOfWork = new UnitOfWork<TranSmartContext>(DbContext);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			DbContext.Dispose();
		}
	}
}
