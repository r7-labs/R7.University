using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.EFCore;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class ScienceConfiguration:  IEntityTypeConfiguration<ScienceInfo>
    {
        public void Configure (EntityTypeBuilder<ScienceInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<ScienceInfo> ("University", t => "Science"));
            entityBuilder.HasKey (m => m.ScienceId);
            entityBuilder.Property (m => m.Directions).IsRequired (false);
            entityBuilder.Property (m => m.Base).IsRequired (false);
            //entityBuilder.HasOne (m => m.EduProgram).WithOne (ep => ep.Science).HasForeignKey<ScienceInfo> (m => m.ScienceId);
        }
    }
}
