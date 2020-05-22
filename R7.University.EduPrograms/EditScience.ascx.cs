//
//  EditScience.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Text.RegularExpressions;
using System.Web;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.Text;
using R7.University.Commands;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Modules;
using R7.University.ViewModels;

namespace R7.University.EduPrograms
{
    public partial class EditScience : UniversityEditPortalModuleBase<ScienceInfo>, IActionable
    {
        protected EditScience () : base ("science_id")
        {
        }

        protected int? GetEduProgramId () =>
            ParseHelper.ParseToNullable<int> (Request.QueryString [Key])
        			 ?? ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogram_id"]);

        protected IEduProgram GetEduProgram (int eduProgramId)
        {
        	return ModelContext.Get<EduProgramInfo,int> (eduProgramId);
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        #region UniversityEditPortalModuleBase implementation

        protected override string GetItemTitle (ScienceInfo item)
        {
            return GetEduProgram (item.ScienceId).FormatTitle ();
        }

        protected override void LoadItem (ScienceInfo item)
        {
            base.LoadItem (item);

            textDirections.Text = item.Directions;
            textBase.Text = item.Base;
            textScientists.Text = item.Scientists.ToString ();
            textStudents.Text = item.Students.ToString ();
            textMonographs.Text = item.Monographs.ToString ();
            textArticles.Text = item.Articles.ToString ();
            textArticlesForeign.Text = item.ArticlesForeign.ToString ();
            textPatents.Text = item.Patents.ToString ();
            textPatentsForeign.Text = item.PatentsForeign.ToString ();
            textCertificates.Text = item.Certificates.ToString ();
            textCertificatesForeign.Text = item.CertificatesForeign.ToString ();
            textFinancingByScientist.Text = item.FinancingByScientist.ToDecimalString ();
        }

        protected override void BeforeUpdateItem (ScienceInfo item, bool isNew)
        {
            item.Directions = HttpUtility.HtmlEncode (StripScripts (HttpUtility.HtmlDecode (textDirections.Text)));
            item.Base = HttpUtility.HtmlEncode (StripScripts (HttpUtility.HtmlDecode (textBase.Text)));
            item.Scientists = ParseHelper.ParseToNullable<int> (textScientists.Text);
            item.Students = ParseHelper.ParseToNullable<int> (textStudents.Text);
            item.Monographs = ParseHelper.ParseToNullable<int> (textMonographs.Text);
            item.Articles = ParseHelper.ParseToNullable<int> (textArticles.Text);
            item.ArticlesForeign = ParseHelper.ParseToNullable<int> (textArticlesForeign.Text);
            item.Patents = ParseHelper.ParseToNullable<int> (textPatents.Text);
            item.PatentsForeign = ParseHelper.ParseToNullable<int> (textPatentsForeign.Text);
            item.Certificates = ParseHelper.ParseToNullable<int> (textCertificates.Text);
            item.CertificatesForeign = ParseHelper.ParseToNullable<int> (textCertificatesForeign.Text);
            item.FinancingByScientist = ParseHelper.ParseToNullable<decimal> (textFinancingByScientist.Text);
        }

        string StripScripts (string html)
        {
        	html = Regex.Replace (html, @"<script.*>.*</script>", string.Empty, RegexOptions.Singleline);
        	html = Regex.Replace (html, @"<script.*/>", string.Empty, RegexOptions.Singleline);

        	return html;
        }

        protected override int GetItemId (ScienceInfo item) => item.ScienceId;

        protected ScienceInfo GetItemWithDependencies (int itemId)
        {
            return ModelContext.Get<ScienceInfo, int> (itemId);
        }

        protected override void UpdateItem (ScienceInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges (true);
        }

        protected override void AddItem (ScienceInfo item)
        {
            var scienceId = ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogram_id"]);
            if (scienceId != null) {
                item.ScienceId = scienceId.Value;
            }

            new AddCommand<ScienceInfo> (ModelContext, SecurityContext).Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (ScienceInfo item)
        {
            new DeleteCommand<ScienceInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var actions = new ModuleActionCollection ();
                var eduProgramId = GetEduProgramId ();
                if (eduProgramId != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgram.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprogram_id", eduProgramId.ToString (), "EditEduProgram"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
