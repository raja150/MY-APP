using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.API.Services.Extensions
{
    public class EmployeeService : ExtensionBaseService
    {
        private readonly ICacheService _cacheService;
        public EmployeeService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public override void Post(Guid id)
        {
            _cacheService.UpdateEmployees(id);
        }
        public override void Put(Guid id)
        {
            _cacheService.UpdateEmployees(id);
        }
    }
}
