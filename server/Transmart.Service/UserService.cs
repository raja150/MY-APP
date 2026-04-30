using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;

namespace TranSmart.Service
{
	public interface IUserService : IBaseService<User>
	{
		Task<User> Validate(string username, string password);
		Task<Result<User>> Refresh(Guid id, string refreshToken);
		Task<Result<UserLoginLog>> AddUserLoginLog(UserLoginLog item);
		Task AddUserLoginFail(UserLoginFail item);
		Task<User> ChangePassword(string username);
		Task<User> ChangePassword(Guid id);
		Task<Result<User>> UpdatePassword(Guid id, string oldPassword, string newPassword);
		Task<User> CheckUser(string user);
		Task<User> GetUserById(Guid id);

		Task<IEnumerable<UserAudit>> GetAuditHistory(Guid id);
		Task<Result<User>> UpdateUserPwd(User item);
		Task UpdateLastLogin(Guid id);
		Task UpdateWebPunchIn(Guid? empId);
	}


	public class UserService : BaseService<User>, IUserService
	{

		private readonly DateTime expireAfter = DateTime.Now.AddDays(30);
		public UserService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<UserLoginLog>> AddUserLoginLog(UserLoginLog item)
		{
			Result<UserLoginLog> result = new(false);
			try
			{
				await UOW.GetRepositoryAsync<UserLoginLog>().AddAsync(item);
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
		public async Task AddUserLoginFail(UserLoginFail item)
		{
			await UOW.GetRepositoryAsync<UserLoginFail>().AddAsync(item);
			UOW.SaveChanges();
		}

		public override async Task<Result<User>> UpdateAsync(User item)
		{
			var result = new Result<User>();
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == item.ID
				//include: i => i.Include(x => x.Role),
				//disableTracking: false
				);

			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Employee));
				return result;
			}
			Role entityRole1 = await UOW.GetRepositoryAsync<Role>().SingleAsync(x => x.ID == entity.RoleId);

			UserAudit userAudit = new()
			{
				UserId = entity.ID,
				AddedAt = DateTime.Now
			};

			if (entity.RoleId != item.RoleId)
			{
				Role entityRole = await UOW.GetRepositoryAsync<Role>().SingleAsync(x => x.ID == item.RoleId);
				userAudit.Description = $"Role updated from  {entityRole1.Name} to {entityRole.Name}";
				entity.RoleId = entityRole.ID;
			}
			if (!string.IsNullOrEmpty(item.Password.Trim()))
			{
				entity.Password = Encrypt.HashPassword(item.Password);
				entity.ExpireOn = DateTime.Now.AddDays(-1);
				entity.NoOfWrongs = 0;
				entity.Type = (int)UserSts.Active;
				userAudit.Description = userAudit.Description + "  " + " Password updated";
			}
			await UOW.GetRepositoryAsync<UserAudit>().AddAsync(userAudit);
			return await base.UpdateAsync(entity);
		}

		public async Task<User> Validate(string username, string password)
		{
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.Name == username);

			if (entity == null)
			{
				return null;
			}
			if (entity.Type == (int)UserSts.Locked)
			{
				return entity;
			}
			//if (!Encrypt.Verify(password, entity.Password))
			//{
			//	//Block the user if he/she enter wrong password greater than or equal to 5
			//	if (entity.NoOfWrongs >= 5)
			//	{
			//		entity.Type = (int)UserSts.Locked;
			//	}
			//	entity.NoOfWrongs += 1;
			//	UOW.GetRepositoryAsync<User>().UpdateAsync(entity);

			//	return null;
			//}
			entity.LastLogin = TimeStamp();
			entity.RefreshToken = GenerateRefreshToken();
			entity.RefreshTokenExpiryAt = DateTime.Now.AddDays(1);
			UOW.GetRepositoryAsync<User>().UpdateAsync(entity);
			await UOW.SaveChangesAsync();
			return entity;
		}

		public async Task<Result<User>> Refresh(Guid id, string refreshToken)
		{
			Result<User> result = new();
			User user = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == id);
			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryAt <= DateTime.Now)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_User_Or_Refresh_Token));
				return result;
			}

			user.RefreshToken = GenerateRefreshToken();
			user.RefreshTokenExpiryAt = DateTime.Now.AddHours(1);
			result = await base.UpdateAsync(user);
			return result;
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
		public async Task<User> ChangePassword(Guid id)
		{
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == id);
			return entity;
		}
		public async Task<User> ChangePassword(string username)
		{
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.Name == username);
			return entity;
		}

		public async Task<User> CheckUser(string user)
		{
			return await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.Name == user,
				include: x => x.Include(x => x.Employee));
		}

		public async Task<User> GetUserById(Guid id)
		{
			return await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.EmployeeID == id,
				include: x => x.Include(x => x.Employee));
		}

		public override async Task<IPaginate<User>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<User>().GetPageListAsync(
				predicate: x => x.Employee.Status == 1 && x.EmployeeID.HasValue && (string.IsNullOrEmpty(baseSearch.Name) || x.Name.Contains(baseSearch.Name)
				|| string.IsNullOrEmpty(baseSearch.Name) || x.Employee.Name.Contains(baseSearch.Name)),
				include: x => x.Include(x => x.Role)
				.Include(x => x.Employee).ThenInclude(x => x.Designation)
				.Include(x => x.Employee).ThenInclude(x => x.Department),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "Employee.Name", ascending: !baseSearch.IsDescend);
		}
		public override async Task<Result<User>> AddAsync(User item)
		{
			Result<User> result = new();
			item.AddedAt = TimeStamp();
			item.Password = Encrypt.HashPassword(item.Password);
			item.ExpireOn = DateTime.Now.AddDays(-1);
			item.Type = (int)UserSts.Active;
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.Name == item.Name);
			User entity1 = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.EmployeeID == item.EmployeeID);

			if (entity != null)
			{
				result.AddMessageItem(new MessageItem(nameof(User.Name), Resource.User_Name_Already_Exist));
				return result;
			}
			else if (entity1 != null)
			{
				result.AddMessageItem(new MessageItem(nameof(User.EmployeeID), Resource.Duplicate_User));
				return result;
			}
			return await base.AddAsync(item);
		}

		public async Task<Result<User>> UpdatePassword(Guid id, string oldPassword, string newPassword)
		{
			var result = new Result<User>();
			var user = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == id);

			if (!Encrypt.Verify(oldPassword, user.Password))
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Old_Password));
				return result;
			}
			if (await CheckOldPwds(user.ID, newPassword))
			{
				result.AddMessageItem(new MessageItem(Resource.New_Password_Should_Be_Different_From_Old_Password));
				return result;
			}
			user.Password = Encrypt.HashPassword(newPassword);
			user.ExpireOn = expireAfter;
			user.Type = (int)UserSts.Active;
			UOW.GetRepositoryAsync<User>().UpdateAsync(user);
			await AddUserPassword(user.ID, user.Password);
			result.ReturnValue = user;
			return result;
		}


		public async Task<IEnumerable<UserAudit>> GetAuditHistory(Guid id)
		{
			return await UOW.GetRepositoryAsync<UserAudit>().GetAsync(x => x.UserId == id);
		}

		private async Task<bool> CheckOldPwds(Guid userId, string newPwd)
		{
			var passwords = await UOW.GetRepositoryAsync<UserPasswords>().GetAsync(x => x.UserId == userId);

			var last5Pwds = passwords.OrderByDescending(x => x.ChangedAt).Take(5);
			return last5Pwds.Any(x => Encrypt.Verify(newPwd, x.Password));
		}
		private async Task AddUserPassword(Guid userId, string newPassword)
		{
			await UOW.GetRepositoryAsync<UserPasswords>().AddAsync(new UserPasswords
			{
				UserId = userId,
				Password = newPassword,
				ChangedAt = DateTime.Now,
			});
			await UOW.SaveChangesAsync();
		}

		public async Task<Result<User>> UpdateUserPwd(User item)
		{
			var result = new Result<User>();
			User entity = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == item.ID);

			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Employee));
				return result;
			}

			if (await CheckOldPwds(item.ID, item.Password))
			{
				result.AddMessageItem(new MessageItem(Resource.New_Password_Should_Be_Different_From_Old_Password));
				return result;
			}
			var userAudit = new UserAudit
			{
				UserId = entity.ID,
				CreatedBy = entity.Name
			};
			if (!string.IsNullOrEmpty(item.Password.Trim()))
			{
				entity.Password = Encrypt.HashPassword(item.Password);
				entity.ExpireOn = item.ExpireOn;
				entity.NoOfWrongs = 0;
				entity.Type = (int)UserSts.Active;
				userAudit.Description = "Password updated";
				await UOW.GetRepositoryAsync<UserAudit>().AddAsync(userAudit);
				await AddUserPassword(entity.ID, entity.Password);
			}
			UOW.GetRepositoryAsync<User>().UpdateAsync(entity);
			try
			{
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task UpdateLastLogin(Guid id)
		{
			var user = await UOW.GetRepositoryAsync<User>().SingleAsync(x => x.ID == id);
			user.LastLogin = DateTime.Now;
			UOW.GetRepositoryAsync<User>().UpdateAsync(user);
			await UOW.SaveChangesAsync();
		}
		public async Task UpdateWebPunchIn(Guid? empId)
		{
			var webAttDetails = await UOW.GetRepositoryAsync<WebAttendance>().SingleAsync(x => x.OutTime == null
									&& x.EmployeeId == empId);
			if (webAttDetails != null && (webAttDetails.InTime.Date - DateTime.Now.Date).TotalHours > 10)
			{
				webAttDetails.OutTime = webAttDetails.InTime;
				UOW.GetRepositoryAsync<WebAttendance>().UpdateAsync(webAttDetails);
				await UOW.SaveChangesAsync();
			}
		}
	}

}
