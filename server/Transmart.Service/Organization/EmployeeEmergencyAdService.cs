using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TranSmart.Core.Result;

namespace TranSmart.Service.Organization
{
    public partial interface IEmployeeEmergencyAdService : IBaseService<EmployeeEmergencyAd>
    {

    }

    public partial class EmployeeEmergencyAdService : BaseService<EmployeeEmergencyAd>, IEmployeeEmergencyAdService
    {
       
        public override async Task CustomValidation(EmployeeEmergencyAd item, Result<EmployeeEmergencyAd> result)
        {
            await base.CustomValidation(item, result);
            if (item.EmergencyConNo != "" && !Regex.IsMatch(item.EmergencyConNo, Resource.Regx_Mobile_Number))
            {
                result.AddMessageItem(new MessageItem(nameof(item.EmergencyConNo),
                   Resource.Contact_Number_Must_Be_Number_And_10_Digits_Only));

            }
        }
       
    }
}

