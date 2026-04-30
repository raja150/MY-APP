using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Core.Util
{
	public static class ConstUtil
	{
		public static string Gender(int genderType)
		{
			if (genderType == 1)
			{
				return "Male";
			}
			else
			{
				return genderType == 2 ? "Female" : "Others";
			}
		}
		public static string MartialStatus(int status)
		{
			if (status == 1)
			{
				return "Married";
			}
			else
			{
				return status == 2 ? "Unmarried" : "Separated";
			}
		}
		public static byte QueType(string type)
		{
			return type.ToLower() switch
			{
				"single" => 1,
				"multiple" => 2,
				"numeric" => 3,
				"torf" => 4,
				"text" => 5,
				_ => 0
			};
		}

		public static string GetOption(int SNo)
		{
			return SNo switch
			{
				1 => "A",
				2 => "B",
				3 => "C",
				4 => "D",
				5 => "E",
				6 => "F",
				_ => "",
			};
		}

		public static byte GetSNo(string Key)
		{
			return Key switch
			{
				"A" => 1,
				"B" => 2,
				"C" => 3,
				"D" => 4,
				"E" => 5,
				"F" => 6,
				_ => 0
			};
		}

		public static string GetQuestionType(int type)
		{
			return type switch
			{
				1 => "Single",
				2 => "Multiple",
				3 => "Numeric",
				4 => "T or F",
				5 => "Text",
				_ => "",
			};
		}
	}
}
