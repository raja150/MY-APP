using TranSmart.Domain;
using Microsoft.EntityFrameworkCore;
using TranSmart.Domain.Entities;
using System.Linq;
using System;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Core;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Payroll;
using System.Threading.Tasks;
using System.Threading;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Entities.HelpDesk;

namespace TranSmart.Data
{
	public partial class TranSmartContext : DbContext
	{
		readonly IApplicationUser _applicationUser;
		public TranSmartContext(DbContextOptions<TranSmartContext> options, IApplicationUser applicationUser) : base(options)
		{
			_applicationUser = applicationUser;
		}
		public System.Data.IDbConnection Connection => Database.GetDbConnection();
		public DbSet<AppForm> AppForms { get; set; }
		public DbSet<SequenceNo> SequenceNo { get; set; }
		public DbSet<LookUpMaster> LookUpMaster { get; set; }
		public DbSet<LookUpValues> LookUpValues { get; set; }
		public DbSet<ValueText> ValueText { get; set; }

		public DbSet<UserLoginLog> LoginLog { get; set; }
		public DbSet<UserLoginFail> LoginFail { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Page> Pages { get; set; }
		public DbSet<Report> Reports { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<RolePrivilege> RolePrivileges { get; set; }
		public DbSet<RoleReportPrivilege> RoleReportPrivileges { get; set; }

		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}

			//Employee Code is unique
			modelBuilder.Entity<Employee>().HasIndex(b => b.No).IsUnique();
			modelBuilder.Entity<PaySheet>().HasIndex(p => new { p.PayMonthId, p.EmployeeID }).IsUnique();

			modelBuilder.Entity<PaySheet>().HasMany(t => t.Earnings).WithOne(c => c.PaySheet).HasForeignKey(c => c.PaySheetId)
			  .OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<PaySheet>().HasMany(t => t.Deductions).WithOne(c => c.PaySheet).HasForeignKey(c => c.PaySheetId)
			  .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<EmpStatutory>().HasIndex(u => u.EmpId).IsUnique();
			modelBuilder.Entity<EmployeePayInfo>().HasIndex(u => u.EmployeeId).IsUnique();
			modelBuilder.Entity<ApplyLeaveDetails>().HasIndex(p => new { p.ApplyLeaveId, p.LeaveTypeId, p.LeaveDate }).IsUnique();
		}

		public override int SaveChanges()
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			AddTimestamps();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		private string CreatedBy(string creadBy)
		{
			if (!string.IsNullOrEmpty(creadBy))
			{
				return creadBy;
			}
			return _applicationUser == null ? string.Empty
							: _applicationUser.UserId;
		}
		private void AddTimestamps()
		{
			//https://www.koskila.net/how-to-add-creator-modified-info-to-all-of-your-ef-models-at-once-in-net-core/
			var entities = ChangeTracker.Entries().Where(x => (x.Entity is AuditEntity
			|| x.Entity is AuditLogEntity) && (x.State == EntityState.Added || x.State == EntityState.Modified));


			foreach (var entity in entities)
			{
				if (entity.State == EntityState.Added)
				{
					if (entity.Entity is AdjustLeave adjustLeave)
					{
						adjustLeave.AddedOn = DateTime.Now;
						adjustLeave.Addedby = _applicationUser == null ? string.Empty : _applicationUser.UserId;
					}
					if (entity.Entity is AuditEntity auditEntity)
					{
						auditEntity.AddedAt = DateTime.Now;
						auditEntity.CreatedBy = CreatedBy(auditEntity.CreatedBy);
					}
					else if (entity.Entity is AuditLogEntity auditLog)
					{
						auditLog.ModifiedAt = DateTime.Now;
						auditLog.ModifiedBy = _applicationUser == null ? string.Empty : _applicationUser.UserId;
					}
				}
				else if (entity.State == EntityState.Modified)
				{
					if (entity.Entity is AuditEntity auditEntity)
					{
						Entry(auditEntity).Property(p => p.AddedAt).IsModified = false;
						Entry(auditEntity).Property(p => p.CreatedBy).IsModified = false;

						auditEntity.ModifiedAt = DateTime.Now;
						auditEntity.ModifiedBy = _applicationUser == null ? string.Empty : _applicationUser.UserId;
					}
				}
			}
		}
	}
}
