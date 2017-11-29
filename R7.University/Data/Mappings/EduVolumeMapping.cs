using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EduVolumeMapping: EntityTypeConfiguration<EduVolumeInfo>
    {
        public EduVolumeMapping ()
        {
            HasKey (m => m.EduVolumeId);
            Property (m => m.EduVolumeId).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);

            Property (m => m.EduProgramProfileFormYearId).IsRequired ();

            Property (m => m.TimeToLearnHours).IsRequired ();
            Property (m => m.TimeToLearnMonths).IsRequired ();

            Property (m => m.VolumeYear1Cu).IsOptional ();
            Property (m => m.VolumeYear2Cu).IsOptional ();
            Property (m => m.VolumeYear3Cu).IsOptional ();
            Property (m => m.VolumeYear4Cu).IsOptional ();
            Property (m => m.VolumeYear5Cu).IsOptional ();
            Property (m => m.VolumeYear6Cu).IsOptional ();

            Property (m => m.VolumePracticeType1Cu).IsOptional ();
            Property (m => m.VolumePracticeType2Cu).IsOptional ();
            Property (m => m.VolumePracticeType3Cu).IsOptional ();

            HasRequired (m => m.EduProgramProfileFormYear).WithOptional ();
        }
    }
}