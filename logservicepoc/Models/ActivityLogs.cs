namespace logservicepoc.Models
{
    public class ActivityLogs
    {
        public int LogId { get; set; }
        public DateTime Timestamp { get; set; }
        public int? UserId { get; set; }
        public string ActionType { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public bool IsProcessed { get; set; }
    }
}