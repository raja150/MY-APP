using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Entities.Organization;
using System.Threading.Tasks;

namespace TranSmart.Service.Payroll
{
    public partial interface ILoanService : IBaseService<Loan>
    {

    }

    public partial class LoanService : BaseService<Loan>, ILoanService
    {
		private readonly ISequenceNoService _sequenceNoService;
		public LoanService(IUnitOfWork uow, ISequenceNoService sequenceNoService) : base(uow)
        {
			_sequenceNoService = sequenceNoService ?? throw new NullReferenceException();
		}
        public override async Task OnBeforeAdd(Loan item, Result<Loan> executionResult)
        {
            item.Due = item.LoanAmount;
            Tuple<int, string> tuple = await _sequenceNoService.NextSequenceNo("PayRoll_Loan", "No");
            item.LoanNo = tuple.Item2; 
            await base.OnBeforeAdd(item, executionResult);
        }
        public override async Task CustomValidation(Loan item, Result<Loan> result)
        {
            await base.CustomValidation(item, result);

            if (await UOW.GetRepositoryAsync<LoanDeduction>().GetCountAsync(x => x.LoanID == item.ID && x.ID != item.ID
                && x.PayMonth.Status == (int)PayMonthStatus.Released) > 0)
            {
                result.AddMessageItem(new MessageItem(Resource.Sorry____You_Can_Not_Update_The_Loan_Once_Recovery_Is_Started));
            }
        }
        public override async Task<Result<Loan>> UpdateAsync(Loan item)
        {
            //Stop overwriting due amount which is calculated by pay run
            Loan entity = await UOW.GetRepositoryAsync<Loan>().SingleAsync(x => x.ID == item.ID);
            entity.Update(item);
            return await base.UpdateAsync(entity);
        }

    }
}
