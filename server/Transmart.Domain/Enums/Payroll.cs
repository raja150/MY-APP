namespace TranSmart.Domain.Enums
{
	/// <summary>
	/// Payroll month status
	/// </summary>
	public enum PayMonthStatus
	{
		InActive = 0,
		Active,
		Open,
		InProcess,
		Released,
		Hold
	}

	public enum PaySheetSts
	{
		AttendanceMiss = 1,
		SalaryMissing = 2,
		EPFDisabled = 4,
		ESIDisabled = 8,
	}

	public enum EarningType
	{
		Basic = 1,
		HRA = 2,
		ConveyanceAllowance = 3,
		TransportAllowance = 4,
		EducationAllowance = 5,
		LeaveEncashment = 6,
		Gratuity = 7,
		NoticePay = 8,
		FoodCoupon = 9,
		MedicalAllowance = 10,
		SodexoMultiBenefit = 11,
	}
	public enum DeductionType
	{
		Pre = 1,
		Post = 2
	}
}
