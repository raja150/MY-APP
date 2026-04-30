using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
    public interface IUpdateProfileService : IBaseService<Employee>
    {
        Task<Result<Employee>> Verification(Guid empId);
        Task<Result<Employee>> UpdateFromProfile(Guid empId, EmpProfileModel item);
    }
    public class UpdateProfileService : BaseService<Employee>, IUpdateProfileService
    {
        public UpdateProfileService(IUnitOfWork uow) : base(uow)
        {

        }
        public async Task<Result<Employee>> Verification(Guid empId)
        {
            Result<Employee> result = new Result<Employee>();
            Employee employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == empId);
            if (employee == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Employee_Details));
                return result;
            }
            return result;
        }
        public async Task<Result<Employee>> UpdateFromProfile(Guid empId, EmpProfileModel item)
        {
            var result = new Result<Employee>();
            var employee = await UOW.GetRepositoryAsync<Employee>().SingleAsync(predicate: x => x.ID == empId,
                include: i => i.Include(x => x.Department).Include(x => x.Designation).Include(x => x.WorkLocation).Include(x => x.Team));

            if (employee == null)
            {
                result.AddMessageItem(new MessageItem("Invalid Employee Details"));
            }

            else
            {
                if (item.Name != null)
                {
                    employee.Name = item.Name;

                    return await base.UpdateAsync(employee);
                }
                if (item.DateOfBirth != null)
                {
                    if (item.MobileNumber != "" && !Regex.IsMatch(item.MobileNumber, Resource.Regx_Mobile_Number))
                    {
                        result.AddMessageItem(new MessageItem(
                           nameof(Employee.MobileNumber), Resource.Contact_Number_Must_Be_Number_And_10_Digits_Only));
                    }
                    else
                    {
                        employee.DateOfBirth = (DateTime)item.DateOfBirth;
                        employee.Gender = item.Gender;
                        employee.MaritalStatus = item.MaritalStatus;
                        employee.MobileNumber = item.MobileNumber;
                        employee.FatherName = item.FatherName;
                        employee.PersonalEmail = item.PersonalEmail;
                        return await base.UpdateAsync(employee);
                    }
                }

            }
            return result;
        }
    }
}
