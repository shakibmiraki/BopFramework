using Bop.Core.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bop.Data.Mapping.Security
{
    /// <summary>
    /// Represents a permission record-user role mapping configuration
    /// </summary>
    public class PermissionRecordUserRoleMap : BopEntityTypeConfiguration<PermissionRecordUserRoleMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PermissionRecordUserRoleMapping> builder)
        {
            builder.ToTable(BopMappingDefaults.PermissionRecordRoleTable);
            builder.HasKey(mapping => new { mapping.PermissionRecordId, mapping.UserRoleId});

            builder.Property(mapping => mapping.PermissionRecordId).HasColumnName("PermissionRecord_Id");
            builder.Property(mapping => mapping.UserRoleId).HasColumnName("UserRole_Id");

            builder.HasOne(mapping => mapping.UserRole)
                .WithMany(role => role.PermissionRecordUserRoleMappings)
                .HasForeignKey(mapping => mapping.UserRoleId)
                .IsRequired();

            builder.HasOne(mapping => mapping.PermissionRecord)
                .WithMany(record => record.PermissionRecordUserRoleMappings)
                .HasForeignKey(mapping => mapping.PermissionRecordId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }

        #endregion
    }
}
