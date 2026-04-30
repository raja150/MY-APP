using TranSmart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain
{
    /// <summary>
    /// Return service executed status
    /// </summary>
    public class ServiceResult
    { 
        /// <summary>
        /// True - Executed
        /// False - Failed
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Entity object
        /// </summary>
        public DataGroupEntity Entity { get; set; }
    }
}
