using System.Globalization;
using System.IO;
using R7.University.Components;

namespace R7.University.Templates
{
    public static class UniversityTemplateHelper
    {
        public static string GetLocalizedTemplatePath (string templateFileName, CultureInfo culture)
        {
            var templateBasePath = UniversityGlobals.GetAbsoluteTemplatesPath ();
            var cultureCode = culture.TwoLetterISOLanguageName;

            var templatePath = templateBasePath + $"/{Path.GetFileNameWithoutExtension (templateFileName)}_{cultureCode}{Path.GetExtension (templateFileName)}";
            if (!File.Exists (templatePath)) {
                templatePath = templateBasePath + "/" + templateFileName;
            }

            return templatePath;
        }

        public static string GetLocalizedEmployeeTemplatePath ()
        {
            return GetLocalizedTemplatePath ("employee_template.xls", CultureInfo.CurrentUICulture);
        }
    }
}
