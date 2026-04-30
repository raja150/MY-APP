using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.API.ManageServices.Organization
{
    public interface IManageEmployeeService : IManageBaseService<Employee>
    {

    }
    public class ManageEmployeeService : ManageBaseService<Employee>, IManageEmployeeService
    {

    }
}
