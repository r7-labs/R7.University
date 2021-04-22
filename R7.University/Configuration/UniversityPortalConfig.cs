namespace R7.University.Configuration
{
    public class UniversityPortalConfig
    {
        public EmployeePhotoConfig EmployeePhoto { get; set; } = new EmployeePhotoConfig ();

        public BarcodeConfig Barcode { get; set; } = new BarcodeConfig ();

        public VocabulariesConfig Vocabularies { get; set; } = new VocabulariesConfig ();

        public int DataCacheTime { get; set; } = 60;

        public int CuHours { get; set; } = 36;

        public EduProgramProfilesConfig EduProgramProfiles { get; set; } = new EduProgramProfilesConfig ();

        public RecaptchaConfig Recaptcha { get; set; } = new RecaptchaConfig ();
    }

    public class EmployeePhotoConfig
    {
        public string DefaultPath { get; set; } = "Images/";

        public int DefaultWidth { get; set; } = 192;

        public int ListDefaultWidth { get; set; } = 113;
    }

    public class BarcodeConfig
    {
        public int DefaultWidth { get; set; } = 192;
    }

    public class VocabulariesConfig
    {
        public string OrgStructure { get; set; } = "University_Structure";

        public string WorkingHours { get; set; } = "University_WorkingHours";
    }

    public class EduProgramProfilesConfig
    {
        public string DefaultLanguages { get; set; } = "en";
    }

    public class RecaptchaConfig
    {
        public string SiteKey { get; set; }
    }
}

