using Microsoft.EntityFrameworkCore;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) 
    {
        
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserRefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<UserEvent> UserEvents { get; set; }
    public virtual DbSet<UserEventDeliveryStatus> EventDeliveryStatuses { get; set; }
    public virtual DbSet<UserEventReadStatus> EventReadStatuses { get; set; }
    public virtual DbSet<UserHubConnection> UserHubConnections { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }
    public virtual DbSet<ShipmentMilestone> ShipmentMilestones { get; set; }
    public virtual DbSet<UserEventSubsSetting> UserEventSubsSettings { get; set; }
    public virtual DbSet<UserEventSubsSettingsItem> UserEventSubsSettingsItems { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }
}
