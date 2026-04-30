using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Enums;

namespace TranSmart.Data.SeedData
{
	public static class TranSmartContextSeed
	{
		static TranSmartContext _transmartContext;
		public static async System.Threading.Tasks.Task SeedAsync(TranSmartContext transmartContext, int? retry = 0)
		{
			_transmartContext = transmartContext;

			Role role = _transmartContext.Roles.Where(x => x.Name == "Administrator").FirstOrDefault();
			if (role == null)
			{
				Guid roleId = Guid.NewGuid();
				_transmartContext.Roles.Add(new Role { ID = roleId, Name = "Administrator", Cards = "1,2,3,4,5,6,7,8,9,10,11,12,13", CanEdit = false, AddedAt = DateTime.Now });
				foreach (Page page in _transmartContext.Pages.ToList())
				{
					_transmartContext.RolePrivileges.Add(new RolePrivilege { ID = Guid.NewGuid(), PageId = page.ID, RoleId = roleId, Privilege = page.Privilege, AddedAt = DateTime.Now });
				}
			}
			else
			{
				foreach (Page page in _transmartContext.Pages.ToList())
				{
					RolePrivilege rolePrivilege = _transmartContext.RolePrivileges.Where(x => x.RoleId == role.ID
					&& x.PageId == page.ID).FirstOrDefault();
					if (rolePrivilege == null)
					{
						_transmartContext.RolePrivileges.Add(new RolePrivilege { ID = Guid.NewGuid(), PageId = page.ID, RoleId = role.ID, Privilege = page.Privilege, AddedAt = DateTime.Now });
					}
				}
			}
			if (!_transmartContext.Payroll_EarningComponent.Any())
			{
				AddPayrollHeaders();
			}
			await _transmartContext.SaveChangesAsync();


			if (!_transmartContext.Users.Any(x => x.Name == "Administrator"))
			{
				role = _transmartContext.Roles.Single(x => x.Name == "Administrator");
				_transmartContext.Users.Add(new User
				{
					ID = Guid.NewGuid(),
					Name = "Administrator",
					RoleId = role.ID,
					Type = 1,
					Password = Core.Util.Encrypt.HashPassword("password"),
					NoOfWrongs = 0,
					ExpireOn = DateTime.Now.AddDays(-30),
					AddedAt = DateTime.Now
				});

				await _transmartContext.SaveChangesAsync();
			}

			if (!_transmartContext.Payroll_Section80C.Any())
			{
				AddITSections();
				await _transmartContext.SaveChangesAsync();
			}
		}
		private static void AddPayrollHeaders()
		{
			_transmartContext.Payroll_EarningComponent.Add(new Domain.Entities.Payroll.EarningComponent
			{
				ID = Guid.NewGuid(),
				Name = "Basic",
				EarningType = (int)EarningType.Basic,
				PartOfSalary = true,
				ProrataBasis = true,
				EPFContribution = 1,
				ESIContribution = true,
				ShowInPayslip = true,
				PartEmployerCTC = true,
				HideWhenZero = false,
				DisplayOrder = 1,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});

			_transmartContext.Payroll_EarningComponent.Add(new Domain.Entities.Payroll.EarningComponent
			{
				ID = Guid.NewGuid(),
				Name = "HRA",
				EarningType = (int)EarningType.HRA,
				PartOfSalary = true,
				ProrataBasis = true,
				EPFContribution = 3,
				ESIContribution = true,
				ShowInPayslip = true,
				PartEmployerCTC = true,
				HideWhenZero = false,
				DisplayOrder = 2,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});
			_transmartContext.Payroll_EarningComponent.Add(new Domain.Entities.Payroll.EarningComponent
			{
				ID = Guid.NewGuid(),
				Name = "Medical & Transport Allowance",
				EarningType = (int)EarningType.MedicalAllowance,
				PartOfSalary = true,
				ProrataBasis = true,
				EPFContribution = 3,
				ESIContribution = true,
				ShowInPayslip = true,
				PartEmployerCTC = false,
				HideWhenZero = false,
				DisplayOrder = 3,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});

			Guid foodId = Guid.NewGuid();
			_transmartContext.Payroll_EarningComponent.Add(new Domain.Entities.Payroll.EarningComponent
			{
				ID = foodId,
				Name = "Food Coupons",
				EarningType = (int)EarningType.FoodCoupon,
				PartOfSalary = true,
				ProrataBasis = true,
				EPFContribution = 3,
				ESIContribution = true,
				ShowInPayslip = true,
				PartEmployerCTC = false,
				HideWhenZero = true,
				DisplayOrder = 4,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});
			_transmartContext.Payroll_DeductionComponent.Add(new Domain.Entities.Payroll.DeductionComponent
			{
				ID = Guid.NewGuid(),
				EarningId = foodId,
				IsEditable = false,
				Name = "Food Coupons",
				Deduct = 2,
				ProrataBasis = true,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});

			_transmartContext.Payroll_EarningComponent.Add(new Domain.Entities.Payroll.EarningComponent
			{
				ID = Guid.NewGuid(),
				Name = "Special Allowance",
				PartOfSalary = true,
				ProrataBasis = true,
				EPFContribution = 3,
				ESIContribution = true,
				ShowInPayslip = true,
				PartEmployerCTC = false,
				HideWhenZero = false,
				DisplayOrder = 5,
				AddedAt = DateTime.Now,
				Status = true,
				CreatedBy = "DataSeed"
			});
		}

		private static void AddITSections()
		{
			Dictionary<Guid, string> section80c = new()
			{
				{ Guid.Parse("AA2D9E87-F5D8-4968-A74F-3DD254B54127"), "Life insurance Premium" },
				{ Guid.Parse("327F2056-8540-4187-A345-3FC23A4656E7"), "Tuition Fee" },
				{ Guid.Parse("DA3E1652-47C1-490C-9342-3FC2756AE202"), "Public Provident Fund" },
				{ Guid.Parse("0589AB6E-9695-4B7B-81EB-40C60E4D47D2"), "Employee Contribution Recognized / Statutory Provident Fund" },
				{ Guid.Parse("ACBF40D0-BC3C-4B6A-BF72-4109B3FFE7E7"), "National Savings Certificate- VIII Issue and IX Issue" },
				{ Guid.Parse("1AEC565A-2648-47CE-B0C4-424056FE877C"), "Interest accrued on NSC (Reinvested)" },
				{ Guid.Parse("12468415-E5ED-4B4B-9E79-4287A2EF47D8"), "Deposit in Sukanya Samriddhi Account" },
				{ Guid.Parse("B97DF190-17A4-48F7-AC7A-42A7BD77A05C"), "Bank Fixed Deposit (Minimum Five Years)" },
				{ Guid.Parse("EC206766-65A7-4C7E-BBBB-452D219C2274"), "Post Office Term Deposit (Minimum Five Year)" },

				{ Guid.Parse("C886320A-28F2-4BD8-8119-6527C20B2B09"), "Non-commutable Deferred Annuity Plan of LIC or any other insurer" },
				{ Guid.Parse("92EE9D70-C0BC-4EB0-9553-67761DCED2EF"), "Subscription to Security / Scheme" },
				{ Guid.Parse("F3451878-2DB8-4C46-B0B8-6A3BB0A7F41C"), "Annuity Plan (Jeevan Dhara and Jeevan Akshay) of LIC" },
				{ Guid.Parse("FCC2CDF1-99D9-4461-97B5-6A734F9372AC"), "Annuity Plan any Other Insurer" },
				{ Guid.Parse("B0A7337A-2C16-4937-BF36-6ACBDB6934D8"), "Contribution to Pension Fund set up by any Mutual Fund" },
				{ Guid.Parse("A838DC7E-1FA2-4203-AD95-6AF9E8EF9D2E"), "Subscription to Deposit Scheme of National Housing Bank" },
				{ Guid.Parse("68A5C723-49B5-46AD-A7C4-6B1D9C33099A"), "Contribution to Pension Fund of National Housing Bank" },
				{ Guid.Parse("53203F57-C6F8-485A-B71D-6C14003F6EC5"), "Subscription to Deposit Scheme of Public Sector Housing Finance Company" },
				{ Guid.Parse("1B78B9CA-88A5-479C-8141-6E64742D5576"), "Subscription to Deposit Scheme of Housing Authority" },
				{ Guid.Parse("2B90EB2F-44CA-42B5-AAAC-6F1C2DDEA259"), "Contribution to UTI Mutual Fund" },
				{ Guid.Parse("147C079D-62CF-403C-9959-6F1E06653F1A"), "Subscription to NABARD Rural Development Bonds of NABARD" },
				{ Guid.Parse("F241DC87-E243-4142-823C-7046D3237878"), "Subscription to Eligible Equity Shares or Debentures" },
				{ Guid.Parse("F6970446-2BFF-42D9-8780-709F3DF0CAF4"), "Contribution Approved Superannuation Fund" },
				{ Guid.Parse("E1093E65-EABA-41D9-9560-71CE1332E8D4"), "Contribution to ULIP of UTI" },
				{ Guid.Parse("0372ACFC-8779-4768-B97C-733B383FF323"), "Contribution to ULIP of LIC" },
				{ Guid.Parse("904D3BBF-D6BD-4DD5-BC84-735B551BF575"), "Contribution to Pension Fund" }
			};

			foreach (var item in section80c)
			{
				_transmartContext.Payroll_Section80C.Add(new Domain.Entities.Payroll.Section80C
				{
					ID = item.Key,
					Name = item.Value,
					AddedAt = DateTime.Now,
					CreatedBy = "Data Seed",
					Status = true
				});
			}

			_transmartContext.Payroll_Section80D.Add(new Domain.Entities.Payroll.Section80D
			{
				ID = Guid.Parse("D741FF86-1451-4F4A-9FF4-461E5BBBF192"),
				Name = "Medical Insurance Premium",
				AddedAt = DateTime.Now,
				CreatedBy = "Data Seed",
				Status = true,
				Limit = 25000
			});
			_transmartContext.Payroll_Section80D.Add(new Domain.Entities.Payroll.Section80D
			{
				ID = Guid.Parse("3F9DC8EC-E938-4426-9787-471C7DA31D6F"),
				Name = "Medical Bills",
				AddedAt = DateTime.Now,
				CreatedBy = "Data Seed",
				Status = true,
				Limit = 5000
			});

			Dictionary<Guid, string> otherSection = new()
			{
				{ Guid.Parse("3160C514-53A5-41C7-8131-609599854556"), "Donation to Prime Minister’s National Relief Fund" },
				{ Guid.Parse("BB7C20AB-53AC-4B82-963B-60CEC543E796"), "Donation to Chief Minister’s Relief Fund" },
				{ Guid.Parse("1A9504AC-48B4-4C76-B500-640C8F29414E"), "Donation to the Lieutenant Governor’s Relief Fund" },
				{ Guid.Parse("3352889F-D15D-48F9-B609-74CA4B56A6A9"), "Contribution to National Pension System" }
			};

			foreach (var item in otherSection)
			{
				_transmartContext.Payroll_OtherSections.Add(new Domain.Entities.Payroll.OtherSections
				{
					ID = item.Key,
					Name = item.Value,
					AddedAt = DateTime.Now,
					CreatedBy = "Data Seed",
					Status = true,
					Limit = 100000,
				});
			}
		}
		public static Tuple<int, string> GetSeqNo(string EntityName, string Attribute)
		{
			SequenceNo sequenceNo = _transmartContext.SequenceNo.Single(x => x.EntityName == EntityName && x.Attribute == Attribute);
			int seq = sequenceNo.NextNo++;
			return new Tuple<int, string>(seq, $"{(string.IsNullOrEmpty(sequenceNo.Prefix) ? "A" : sequenceNo.Prefix)}{seq}");
		}
	}
}
