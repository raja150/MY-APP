//using AutoMapper;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TranSmart.Service;
//using TranSmart.Service.Helpdesk;

//namespace TranSmart.API.Services.Extensions
//{
//    public class ExtensionService
//    {
//        public ExtensionService(IMapper mapper, IMemoryCache memoryCache, ISearchService service
//			,ISsoService ssoService, IDepartmentEmployeeService departmentEmployee)
//        {
//			IMapper _mapper = mapper;
//			IMemoryCache _memoryCache = memoryCache;
//			ISearchService _service = service;
//			ISsoService _ssoService = ssoService;
//			IDepartmentEmployeeService _departmentEmployee = departmentEmployee;
//			_sharedObjects.Add(new KeyValuePair<string, ExtensionBaseService>("Employee",
//                new EmployeeService(
//                    cacheService: new CacheService
//            (
//                 _mapper, _memoryCache, _service, _ssoService, _departmentEmployee)
//            )));
//        }
//        private readonly List<KeyValuePair<string, ExtensionBaseService>> _sharedObjects = new();

//        public ExtensionBaseService GetObject(string key)
//        {
//            return _sharedObjects.SingleOrDefault(c => c.Key == key).Value;
//        }
//    }
//}
