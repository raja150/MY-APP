using System;
using System.Text.Json.Serialization;

namespace TranSmart.Domain.Models.Cache
{
    public class RolePrivilegeCache
    {
        public Guid RoleId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupIcon { get; set; }
        public string GroupName { get; set; }
        public Guid PageId { get; set; }
        public string PageIcon { get; set; }
        public string PageName { get; set; }
        [JsonPropertyName("Path")]
        public string PagePath { get; set; }
        public int Privilege { get; set; }
    }
}
