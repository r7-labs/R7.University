using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace R7.University.Launchpad.ViewModels
{
    public class WorkbookConverterUploadResult
    {
        [JsonProperty (PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty (PropertyName = "tempFilePath")]
        public string TempFilePath { get; set; }
    }
}
