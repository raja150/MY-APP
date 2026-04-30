using System;

namespace TranSmart.Core.Result
{
	public class MessageItem
	{
		public string Field { get; set; }
		public string ErrorCode { get; set; }
		public string Description { get; set; }
		public string Source { get; set; }
		public FeedbackType FeedbackType { get; set; }

		public MessageItem(string description, FeedbackType feedbackType = FeedbackType.Error,
			string source = "Not Specified")
		{
			Description = description;
			Source = source;
			FeedbackType = feedbackType;
		}

		public MessageItem(string field, string description, FeedbackType feedbackType = FeedbackType.Error,
			string source = "Not Specified")
		{
			Field = field;
			Description = description;
			Source = source;
			FeedbackType = feedbackType;
		}

		public MessageItem(Exception ex)
		{
			Description = ex.Message + ex.InnerException + "\n" + ex.StackTrace;
			Source = ex.Source;
			FeedbackType = FeedbackType.Exception;
		}

		public string ToString(bool showSource)
		{
			string result = string.IsNullOrEmpty(Field) ? string.Empty : $"[Field]:{Field}\n";

			if (showSource)
			{
				result += $"[Description]:{Description}\n[Source]:{Source}";
			}
			else
			{
				result += $"[Description]:{Description}";
			}
			return result;
		}
	}
}
