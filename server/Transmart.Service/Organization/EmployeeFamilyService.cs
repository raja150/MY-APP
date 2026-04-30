using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
    public partial interface IEmployeeFamilyService : IBaseService<EmployeeFamily>
    {

        Task<Result<EmployeeFamily>> UpdateProfileContact(EmployeeFamilyModel item);

        Task<Result<EmployeeFamily>> DeleteProfileContact(Guid id);
    }


    public partial class EmployeeFamilyService : BaseService<EmployeeFamily>, IEmployeeFamilyService
    {
       
        public async Task<Result<EmployeeFamily>> UpdateProfileContact(EmployeeFamilyModel item)
        {

            Result<EmployeeFamily> result = new Result<EmployeeFamily>();
            if (item.ContactNo != "" && !Regex.IsMatch(item.ContactNo, Resource.Regx_Mobile_Number))
            {
                result.AddMessageItem(new MessageItem(
                   nameof(EmployeeFamily.ContactNo), Resource.Contact_Number_Must_Be_Number_And_10_Digits_Only));
                return result;
            }
            if (item.ID == Guid.Empty)
            {
                EmployeeFamily employeeFamily = new EmployeeFamily();
				employeeFamily.ID = Guid.NewGuid();
                employeeFamily.PersonName = item.PersonName;
                employeeFamily.EmployeeId = item.EmployeeId;
                employeeFamily.ContactNo = item.ContactNo;
                employeeFamily.HumanRelation = item.HumanRelation;
                await AddOnlyAsync(employeeFamily);
                return result;
            }
            var entity = await UOW.GetRepositoryAsync<EmployeeFamily>().SingleAsync(x => x.EmployeeId == item.EmployeeId);

            entity.EmployeeId = item.EmployeeId;
            entity.PersonName = item.PersonName;
            entity.ContactNo = item.ContactNo;
            entity.HumanRelation = item.HumanRelation;
            entity.ID = item.ID;
            _ = await UpdateOnlyAsync(entity);
            result.ReturnValue = entity;
            result.IsSuccess = true;
            return result;
        }
        public async Task<Result<EmployeeFamily>> DeleteProfileContact(Guid id)
        {
            Result<EmployeeFamily> result = new Result<EmployeeFamily>();
            var entity = await UOW.GetRepositoryAsync<EmployeeFamily>().SingleAsync(x => x.ID == id);
            if (entity == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
                return result;
            }
            UOW.GetRepositoryAsync<EmployeeFamily>().DeleteAsync(entity);
            await UOW.SaveChangesAsync();
            result.ReturnValue = entity;
            result.IsSuccess = true;
            return result;
        }
    }
}
