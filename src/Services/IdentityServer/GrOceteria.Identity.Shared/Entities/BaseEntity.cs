﻿namespace Groceteria.Identity.Shared.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = "Default";
        public string UpdatedBy { get; set; } = "Default";
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
