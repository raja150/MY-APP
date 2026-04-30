using System;
using System.ComponentModel.DataAnnotations;

namespace TranSmart.Domain.Models
{
    public class LookUpValues : BaseModel
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
    }
}