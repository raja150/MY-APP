using System;
using TranSmart.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranSmart.Domain.Entities.Payroll
{
    public sealed partial class EmpStatutory : IEquatable<EmpStatutory>
    {
        public bool Equals(EmpStatutory other)
        {
            if (other == null) return false;
            return this.EnablePF.Equals(other.EnablePF)
              && this.UAN.Equals(other.UAN)
              && this.EmployeeContrib.Equals(other.EmployeeContrib)
              && this.EnableESI.Equals(other.EnableESI)
              && this.ESINo.Equals(other.ESINo)
              && this.EmployeesProvid.Equals(other.EmployeesProvid);
        }
        public void Update(EmpStatutory other)
        {
            EnablePF = other.EnablePF; 
            EmployeesProvid = other.EmployeesProvid; 
            UAN = other.UAN;
            EmployeeContrib = other.EmployeeContrib; 
            EnableESI = other.EnableESI; 
            ESINo = other.ESINo;
        }
    }
}
