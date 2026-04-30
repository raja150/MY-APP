using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Service.Organization
{
    public partial interface IOrganizationsService : IBaseService<Organizations>
    {
        Task<Organizations> GetOrg();
    }
    public partial class OrganizationsService
    {
        public async Task<Organizations> GetOrg()
        {
            return await UOW.GetRepositoryAsync<Organizations>().SingleAsync();
        }

        public override async Task CustomValidation(Organizations item, Result<Organizations> result)
        {
            if (item.PAN != "" && !Regex.IsMatch(item.PAN, Resource.Regx_Pan))
            {
                result.AddMessageItem(new MessageItem
                     (nameof(Organizations.PAN), Resource.Invalid_PAN_Format));
            }
            else
            {
                if (await UOW.GetRepositoryAsync<Organizations>().HasRecordsAsync(x => x.PAN == item.PAN && x.ID != item.ID))
                {
                    result.AddMessageItem(new MessageItem
                          (nameof(Organizations.PAN), Resource.Pan_Number_Already_Exists));
                }
            }

            if (item.TAN != "" && !Regex.IsMatch(item.TAN, Resource.Regx_Tan))
            {
                result.AddMessageItem(new MessageItem
                     (nameof(Organizations.TAN), Resource.Invalid_TAN_Format));

            }
            else
            {
                if (await UOW.GetRepositoryAsync<Organizations>().HasRecordsAsync(x => x.TAN == item.TAN && x.ID != item.ID))
                {
                    result.AddMessageItem(new MessageItem
                          (nameof(Organizations.TAN), Resource.Tan_Number_Already_Exists));
                }
            }
        }
    }
}

