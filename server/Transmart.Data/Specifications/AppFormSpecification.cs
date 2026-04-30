using TranSmart.Data.Repository;
using TranSmart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Data.Specifications
{
    public class AppFormSpecification : BaseSpecification<AppForm>
    {
        public AppFormSpecification() : base()
        {
           
        }

        public AppFormSpecification(Guid id) :
            base(x => x.ID == id)
        { 

        }
    }
}
