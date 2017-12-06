using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EduVolumeMapping: EntityTypeConfiguration<EduVolumeInfo>
    {
        public EduVolumeMapping ()
        {
            ToTable (UniversityMappingHelper.GetTableName<EduVolumeInfo> (pluralize: false));
            HasKey (m => m.EduVolumeId);

            Property (m => m.TimeToLearnHours).IsRequired ();
            Property (m => m.TimeToLearnMonths).IsRequired ();

            Property (m => m.Year1Cu).IsOptional ();
            Property (m => m.Year2Cu).IsOptional ();
            Property (m => m.Year3Cu).IsOptional ();
            Property (m => m.Year4Cu).IsOptional ();
            Property (m => m.Year5Cu).IsOptional ();
            Property (m => m.Year6Cu).IsOptional ();

            Property (m => m.PracticeType1Cu).IsOptional ();
            Property (m => m.PracticeType2Cu).IsOptional ();
            Property (m => m.PracticeType3Cu).IsOptional ();

            HasRequired (m => m.EduProgramProfileFormYear).WithOptional (x => x.EduVolume);
        }
    }
}