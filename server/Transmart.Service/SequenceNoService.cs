using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TranSmart.Data;
using Ref = TranSmart.Domain.Entities;
using TranSmart.Data.Paging;
using TranSmart.Domain.Models;
using System.Linq;
using System;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using System.Threading.Tasks;

namespace TranSmart.Service
{
    public interface ISequenceNoService : IBaseService<Ref.SequenceNo>
    {
        Task<IEnumerable<SequenceNo>> UpdateRange(IEnumerable<SequenceNo> sequenceNos);
        Task<Tuple<int, string>> NextSequenceNo(string entity, string attribute);
        Task<string> DisplaySeqNo(string entity, string attribute);
        Task AddSequenceAttribute(string entity, string attribute);
    }

    public class SequenceNoService : BaseService<Ref.SequenceNo>, ISequenceNoService
    {
        public SequenceNoService(IUnitOfWork uow) : base(uow)
        {

        }

        //public override IEnumerable<SequenceNo> GetList(string OrderBy)
        //{
        //    return _UOW.GetRepository<SequenceNo>().GetById(orderBy: o => o.OrderBy(x => x.EntityName));
        //}

        public async Task<IEnumerable<SequenceNo>> UpdateRange(IEnumerable<SequenceNo> sequenceNos)
        {
            foreach (SequenceNo item in sequenceNos)
            {
                SequenceNo sequenceNo = await UOW.GetRepositoryAsync<SequenceNo>().SingleAsync(x => x.ID == item.ID);
                if (sequenceNo != null)
                {
                    sequenceNo.NextNo = item.NextNo;
                    sequenceNo.Prefix = item.Prefix;
                    UOW.GetRepositoryAsync<SequenceNo>().UpdateAsync(sequenceNo);
                }
            }
            UOW.SaveChanges();
            return UOW.GetRepository<SequenceNo>().GetAllList("");
        }

        public virtual async Task<Tuple<int, string>> NextSequenceNo(string entity, string attribute)
        {
            SequenceNo sequenceNo = await UOW.GetRepositoryAsync<SequenceNo>().SingleAsync(x => x.EntityName == entity
                                                                && x.Attribute == attribute);
            if (sequenceNo != null)
            {
                int seq = sequenceNo.NextNo++;
                UOW.GetRepositoryAsync<SequenceNo>().UpdateAsync(sequenceNo);
                return new Tuple<int, string>(seq, GetNo(sequenceNo.Prefix, seq));
            }
            else { throw new InvalidOperationException("Unable to generate next sequenceNo"); }
        }

        public async Task<string> DisplaySeqNo(string entity, string attribute)
        {
            SequenceNo sequenceNo = await UOW.GetRepositoryAsync<SequenceNo>().SingleAsync(x => x.EntityName == entity && x.Attribute == attribute);
            if (sequenceNo != null)
            {
                int seq = sequenceNo.NextNo++;
                return GetNo(sequenceNo.Prefix, seq);
            }
            return "00000";
        }

        public async Task AddSequenceAttribute(string entity, string attribute)
        {
            await UOW.GetRepositoryAsync<SequenceNo>().AddAsync(new SequenceNo { EntityName = entity, Attribute = attribute });
        }
        public string GetNo(string Prefix, int seq)
        {
            return $"{(string.IsNullOrEmpty(Prefix) ? "A" : Prefix)}{seq.ToString().PadLeft(5, '0')}";
        }
    }
}
