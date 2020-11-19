using System.Diagnostics.Contracts;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Text;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DivisionDnnExtensions
    {
        public static VCard VCard (this IDivision division)
        {
            var vcard = new VCard ();

            if (!string.IsNullOrWhiteSpace (division.Title)) {
                vcard.OrganizationName = division.Title;
            }

            if (!string.IsNullOrWhiteSpace (division.Email)) {
                vcard.Emails.Add (division.Email);
            }

            if (!string.IsNullOrWhiteSpace (division.SecondaryEmail)) {
                vcard.Emails.Add (division.SecondaryEmail);
            }

            if (!string.IsNullOrWhiteSpace (division.Phone)) {
                vcard.Phones.Add (new VCardPhone () { Number = division.Phone, Type = VCardPhoneType.Work });
            }

            if (!string.IsNullOrWhiteSpace (division.Fax)) {
                vcard.Phones.Add (new VCardPhone () { Number = division.Fax, Type = VCardPhoneType.Fax });
            }

            if (!string.IsNullOrWhiteSpace (division.WebSite)) {
                vcard.Url = division.WebSite;
            }

            if (!string.IsNullOrWhiteSpace (division.Location)) {
                // TODO: Add organization address
                vcard.DeliveryAddress = division.Location;
            }

            vcard.LastRevision = division.LastModifiedOnDate;

            return vcard;
        }

        public static IDivision GetParentDivision (this IDivision division, IModelContext modelContext)
        {
            Contract.Ensures (Contract.Result<IDivision> () != null);
            if (division.ParentDivisionID != null) {
                return modelContext.Get<DivisionInfo, int> (division.ParentDivisionID.Value);
            }

            return null;
        }

        public static string SearchText (this IDivision division)
        {
            return FormatHelper.JoinNotNullOrEmpty (
                ", ",
                division.Title,
                UniversityModelHelper.HasUniqueShortTitle (division.ShortTitle, division.Title) ? division.ShortTitle : null,
                division.Phone,
                division.Fax,
                division.Email,
                division.SecondaryEmail,
                division.WebSite,
                division.Location,
                division.WorkingHours
            );
        }

        public static string GetSearchUrl (this IDivision division, ModuleInfo module, PortalSettings portalSettings)
        {
            if (!string.IsNullOrEmpty (division.HomePage)) {
                return Globals.NavigateURL (int.Parse (division.HomePage), false, portalSettings, "",
                    portalSettings.PortalAlias.CultureCode);
            }

            return Globals.NavigateURL (module.TabID, false, portalSettings, "",
                portalSettings.PortalAlias.CultureCode, "", "mid", module.ModuleID.ToString ());
        }
    }
}
