using System;
using System.ComponentModel.DataAnnotations.Schema;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Entities.OnlineTest
{
	[Table("OT_Paper")]
	public class Paper : AuditEntity
	{
		public string Name { get; set; }
		public Guid OrganiserId { get; set; }
		public Employee Organiser { get; set; }
		public short Duration { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public bool IsJumbled { get; set; }
		public bool MoveToLive { get; set; }
		public bool Status { get; set; }
		public bool ShowResult { get; set; }
		public void Update(Paper paper)
		{
			Name = paper.Name;
			Duration = paper.Duration;
			StartAt = paper.StartAt;
			EndAt = paper.EndAt;
			IsJumbled = paper.IsJumbled;
			MoveToLive = paper.MoveToLive;
			Status = paper.Status;
			OrganiserId = paper.OrganiserId;
			ShowResult = paper.ShowResult;
		}
		public bool IsEqual(Paper other)
		{
			return Name.Equals(other.Name.Trim()) && Duration.Equals(other.Duration) && IsJumbled.Equals(other.IsJumbled)
				&& OrganiserId.Equals(other.OrganiserId);
		}
	}
}
