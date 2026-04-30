using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Security.Claims;
using TranSmart;
using TranSmart.Core;
using TranSmart.Data;

namespace Transmart.Controller.UnitTests
{
    public class ControllerTestBase
    {
        public readonly Mock<TranSmartContext> DbContext;
        public readonly Mock<UnitOfWork<TranSmartContext>> UOW;
        public readonly IMapper Mapper;
        public readonly ClaimsPrincipal Claim;
        public readonly Guid EmployeeId;
		public readonly Guid DepartmentId;
		public readonly Guid DesignationId;
		public readonly Guid UserId;
		public ControllerTestBase()
        {
            var builder = new DbContextOptionsBuilder<TranSmartContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
            var app = new Mock<IApplicationUser>();
            var dbContextOptions = builder.Options;
            DbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
            UOW = new Mock<UnitOfWork<TranSmartContext>>(DbContext.Object);

            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.LeaveProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.PayrollProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.HelpDeskProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.ApprovalProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapperProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.AttendanceProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.MappingProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.OrganizationProfile());
                cfg.AddProfile(new TranSmart.API.MappingProfile());
				cfg.AddProfile(new TranSmart.API.AutoMapper.OnlineTestProfile());
                cfg.AddProfile(new TranSmart.API.AutoMapper.HelpDeskProfile());
            });

            Mapper = config.CreateMapper();
            EmployeeId = Guid.NewGuid();
            DepartmentId = Guid.NewGuid();
			UserId = Guid.NewGuid();
            Claim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
          {
                new Claim("eid", EmployeeId.ToString()),
                new Claim("did" , DepartmentId.ToString()),
				new Claim("id" , EmployeeId.ToString()),
				new Claim("uid", UserId.ToString())
		  }, "mock"));
		}
    }
}
