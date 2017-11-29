using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using R7.University.Models;
using System.Web.UI.WebControls;

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