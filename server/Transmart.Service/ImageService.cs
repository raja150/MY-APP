using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;

namespace TranSmart.Service
{

    public partial interface IImageService : IBaseService<EmpImage>
    {
		Task<IEnumerable<EmpImage>> GetImg();
        Task<Result<EmpImage>> AddandUpdate(EmpImage item);


    }
    public class ImageService : BaseService<EmpImage>, IImageService
    {

        public ImageService(IUnitOfWork uow) : base(uow)
        {

        }

        public async Task<IEnumerable<EmpImage>> GetImg()
        {
            return await UOW.GetRepositoryAsync<EmpImage>().GetAsync(x => x.Employee.Status == 1); 
        }
        
        public  async Task<Result<EmpImage>> AddandUpdate(EmpImage item)
        {
            EmpImage entity =await UOW.GetRepositoryAsync<EmpImage>().SingleAsync(x => x.EmployeeId == item.EmployeeId);
            Result<EmpImage> result = new();
            if (entity != null)
            {
                entity.ImageData = item.ImageData;
                entity.ResizeImageData = item.ResizeImageData;
                UOW.GetRepositoryAsync<EmpImage>().UpdateAsync(entity);
                await UOW.SaveChangesAsync();
                result.ReturnValue = entity;
                return result;
            }
            else 
            {
                await UOW.GetRepositoryAsync<EmpImage>().AddAsync(item);
                await UOW.SaveChangesAsync();
            }
            return await base.AddAsync(item);
        }
    }
}
