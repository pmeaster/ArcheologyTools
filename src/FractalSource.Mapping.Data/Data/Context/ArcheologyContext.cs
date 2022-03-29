#nullable disable
using FractalSource.Data;
using FractalSource.Mapping.Configuration;
using FractalSource.Mapping.Data.Entities;
using FractalSource.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable InvocationIsSkipped
// ReSharper disable PartialMethodWithSinglePart

namespace FractalSource.Mapping.Data.Context;

public partial class ArcheologyContext : DbContext, IService<IEntity>
{
    private readonly ISqlDataConfiguration _configuration;

    public ArcheologyContext(ILoggerFactory loggerFactory, ISqlDataConfiguration configuration)
    {
        _configuration = configuration;
        Logger = loggerFactory.CreateLogger<ArcheologyContext>();
        InstanceId = Guid.NewGuid();
    }

    public ILogger Logger { get; }

    public Guid InstanceId { get; }
    
    public ServiceKeys<IEntity> ServiceKeys => default;

    public virtual DbSet<AlignmentTypeEntity> AlignmentType { get; set; }

    public virtual DbSet<AxisPositionEntity> AxisPosition { get; set; }

    public virtual DbSet<AxisPositionTypeEntity> AxisPositionType { get; set; }

    public virtual DbSet<AxisRadiusTypeEntity> AxisRadiusType { get; set; }

    public virtual DbSet<AxisTypeEntity> AxisType { get; set; }

    public virtual DbSet<DistanceMatrixEntity> DistanceMatrix { get; set; }

    public virtual DbSet<EquatorSiteEntity> EquatorSite { get; set; }

    public virtual DbSet<EquatorSiteLocationMatrixEntity> EquatorSiteLocationMatrix { get; set; }

    public virtual DbSet<EquatorSiteLocationMatrixSummaryEntity> EquatorSiteLocationMatrixSummary { get; set; }

    public virtual DbSet<LantisLocationEntity> LantisLocation { get; set; }

    public virtual DbSet<LantisZoneEntity> LantisZone { get; set; }

    public virtual DbSet<LantisZoneTypeEntity> LantisZoneType { get; set; }

    public virtual DbSet<MeasurementSystemEntity> MeasurementSystem { get; set; }

    public virtual DbSet<MeasurementSystemExpandedEntity> MeasurementSystemExpanded { get; set; }

    public virtual DbSet<MeasurementSystemObjectRadiusEntity> MeasurementSystemObjectRadius { get; set; }

    public virtual DbSet<MeasurementSystemObjectShapeEntity> MeasurementSystemObjectShape { get; set; }

    public virtual DbSet<MeasurementSystemTypeEntity> MeasurementSystemType { get; set; }

    public virtual DbSet<PoleEquatorLocationMatrixEntity> PoleEquatorLocationMatrix { get; set; }

    public virtual DbSet<PoleEquatorLocationMatrixSummaryEntity> PoleEquatorLocationMatrixSummary { get; set; }

    public virtual DbSet<PoleLocationEntity> PoleLocation { get; set; }

    public virtual DbSet<PoleLocationMatrixEntity> PoleLocationMatrix { get; set; }

    public virtual DbSet<PoleLocationMatrixSummaryEntity> PoleLocationMatrixSummary { get; set; }

    public virtual DbSet<RegionEntity> Region { get; set; }

    public virtual DbSet<SiteLocationEntity> SiteLocation { get; set; }

    public virtual DbSet<SitePoleLocationAlignmentTypeEntity> SitePoleLocationAlignmentType { get; set; }

    public virtual DbSet<SolarSystemObjectEntity> SolarSystemObject { get; set; }

    public virtual DbSet<SolarSystemConfigurationEntity> SolarSystemConfiguration { get; set; }

    public virtual DbSet<SolarSystemObjectTypeEntity> SolarSystemObjectType { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.ConnectionString, (options) =>
            {
                options?.EnableRetryOnFailure();
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlignmentTypeEntity>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<AxisPositionEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

        });

        modelBuilder.Entity<AxisPositionTypeEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();
        });

        modelBuilder.Entity<AxisRadiusTypeEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.ObjectName)
                .IsRequired();

            entity.Property(e => e.OrbitalName)
                .IsRequired();
        });

        modelBuilder.Entity<AxisTypeEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();
        });

        modelBuilder.Entity<EquatorSiteEntity>(entity =>
        {
            entity.Property(e => e.East)
                .HasDefaultValueSql("((90))");

            entity.Property(e => e.SiteName)
                .HasColumnName("Site Name")
                .HasComputedColumnSql("('Site '+CONVERT([nvarchar](max),[ID]))", false);
        });

        modelBuilder.Entity<MeasurementSystemEntity>(entity =>
        {
            entity.Property(e => e.Abbreviation)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.SystemTypeID)
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MeasurementSystemExpandedEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

        });

        modelBuilder.Entity<MeasurementSystemTypeEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();
        });

        modelBuilder.Entity<PoleLocationEntity>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<RegionEntity>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<SitePoleLocationAlignmentTypeEntity>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever();

        });
        
        modelBuilder.Entity<SolarSystemObjectEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.ObjectTypeID)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.ParentObjectID)
                .HasDefaultValueSql("((1))");

        });

        modelBuilder.Entity<SolarSystemConfigurationEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

        });

        modelBuilder.Entity<SolarSystemObjectTypeEntity>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    [DbFunction("GetSolarSystemObjectRadii", "dbo")]
    public IQueryable<SolarSystemObjectRadiusEntity> GetSolarSystemObjectRadii(
        long? solarSystemConfigurationID, long? axisTypeID)
    {
        return FromExpression(() => GetSolarSystemObjectRadii(solarSystemConfigurationID, axisTypeID));
    }

}