using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.EFCore;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class ContingentConfiguration: IEntityTypeConfiguration<ContingentInfo>
    {
        public void Configure (EntityTypeBuilder<ContingentInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<ContingentInfo> ("University", t => "Contingent"));
            entityBuilder.HasKey (m => m.ContingentId);

            entityBuilder.Property (m => m.AvgAdmScore).HasColumnType ("decimal(18,3)").IsRequired (false);

            entityBuilder.Property (m => m.AdmittedFB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedRB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedMB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedBC).IsRequired (false);

            entityBuilder.Property (m => m.ActualFB).IsRequired (false);
            entityBuilder.Property (m => m.ActualRB).IsRequired (false);
            entityBuilder.Property (m => m.ActualMB).IsRequired (false);
            entityBuilder.Property (m => m.ActualBC).IsRequired (false);

            entityBuilder.Property (m => m.VacantFB).IsRequired (false);
            entityBuilder.Property (m => m.VacantRB).IsRequired (false);
            entityBuilder.Property (m => m.VacantMB).IsRequired (false);
            entityBuilder.Property (m => m.VacantBC).IsRequired (false);

            entityBuilder.Property (m => m.MovedIn).IsRequired (false);
            entityBuilder.Property (m => m.MovedOut).IsRequired (false);
            entityBuilder.Property (m => m.Restored).IsRequired (false);
            entityBuilder.Property (m => m.Expelled).IsRequired (false);

            entityBuilder.HasOne (m => m.EduProgramProfileFormYear).WithOne (eppfy => eppfy.Contingent).HasForeignKey<ContingentInfo> (m => m.ContingentId);
        }
    }
}
