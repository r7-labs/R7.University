//
//  EditScienceRecords.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Web.UI.WebControls.Extensions;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Controls.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditScienceRecords : GridAndFormControlBase<ScienceRecordInfo, ScienceRecordEditModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<ScienceRecordTypeInfo> scienceRecordTypes)
        {
            Module = module;

            comboScienceRecordType.DataSource = scienceRecordTypes
                .Select (srt => new ListItemViewModel (
                    srt.ScienceRecordTypeId,
                    LocalizeString ("SystemScienceRecordType_" + srt.Type + ".Text"))
                );
            comboScienceRecordType.DataBind ();

            StoreScienceRecordTypes (scienceRecordTypes);
        }

        void StoreScienceRecordTypes (IEnumerable<ScienceRecordTypeInfo> scienceRecordTypes)
        {
            ViewState ["scienceRecordTypes"] = Json.Serialize (scienceRecordTypes.ToList ());
        }

        ScienceRecordTypeInfo GetScienceRecordType (int scienceRecordTypeId)
        {
            var scienceRecordTypes = Json.Deserialize<List<ScienceRecordTypeInfo>> ((string) ViewState ["scienceRecordTypes"]);
            return scienceRecordTypes.Single (srt => srt.ScienceRecordTypeId == scienceRecordTypeId);
        }

        protected void comboScienceRecordType_SelectedIndexChanged (object sender, EventArgs e)
        {
            SetupFormForType (int.Parse (((DropDownList) sender).SelectedValue));
        }

        void SetupFormForType (int scienceRecordTypeId)
        {
            SetupFormForType (GetScienceRecordType (scienceRecordTypeId));
        }
            
        void SetupFormForType (IScienceRecordType scienceRecordType)
        {
            if (scienceRecordType.DescriptionIsRequired) {
                panelDescription.AddCssClass ("dnnFormRequired");
                valDescriptionRequired.Enabled = true;
            }
            else {
                panelDescription.RemoveCssClass ("dnnFormRequired");
                valDescriptionRequired.Enabled = false;
            }

            panelValue1.Visible = scienceRecordType.NumOfValues >= 1;
            panelValue2.Visible = scienceRecordType.NumOfValues >= 2;

            // TODO: Introduce ScienceRecordType.ValueType or ScienceRecordType.NumOfDecimalPoints?
            if (scienceRecordType.GetSystemScienceRecordType () == SystemScienceRecordType.Finances) {
                if (scienceRecordType.NumOfValues >= 1) {
                    SetupRangeValidator (valValue1Range, ValidationDataType.Double);
                }
                if (scienceRecordType.NumOfValues >= 2) {
                    SetupRangeValidator (valValue2Range, ValidationDataType.Double);
                }
            }
            else {
                if (scienceRecordType.NumOfValues >= 1) {
                    SetupRangeValidator (valValue1Range, ValidationDataType.Integer);
                }
                if (scienceRecordType.NumOfValues >= 2) {
                    SetupRangeValidator (valValue2Range, ValidationDataType.Integer);
                }
            }

            LocalizeLabels (scienceRecordType);
        }

        void SetupRangeValidator (RangeValidator validator, ValidationDataType valDataType)
        {
            if (valDataType == ValidationDataType.Integer || valDataType == ValidationDataType.Double) {
                validator.Type = valDataType;
                // the range for Integer also sufficent for finances values
                validator.MaximumValue = int.MaxValue.ToString (); 
            }
            else {
                throw new ArgumentException ("valDataType argument should be either Integer or Double.");
            }
        }

        void LocalizeLabels (IScienceRecordType scienceRecordType)
        {
            var baseKey = "SystemScienceRecordType_" + scienceRecordType.Type;
            labelScienceRecordTypeHelp.Text = LocalizeString (baseKey + ".Help");

            if (scienceRecordType.NumOfValues >= 1) {
                labelValue1.Text = LocalizeString (baseKey + ".Value1");
            }

            if (scienceRecordType.NumOfValues >= 2) {
                labelValue2.Text = LocalizeString (baseKey + ".Value2");
            }
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            LocalizeLabels (GetScienceRecordType (int.Parse (comboScienceRecordType.SelectedValue)));
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (ScienceRecordEditModel item)
        {
            var scienceRecordType = GetScienceRecordType (item.ScienceRecordTypeId);
            comboScienceRecordType.SelectByValue (item.ScienceRecordTypeId);

            textDescription.Text = item.Description;

            if (scienceRecordType.NumOfValues >= 1) {
                textValue1.Text = item.Value1.ToString ();
            }
          
            if (scienceRecordType.NumOfValues >= 2) {
                textValue2.Text = item.Value2.ToString ();
            }
          
            hiddenScienceRecordID.Value = item.ScienceRecordId.ToString ();
        }

        protected override void OnUpdateItem (ScienceRecordEditModel item)
        {
            item.ScienceRecordTypeId = int.Parse (comboScienceRecordType.SelectedValue);
            var scienceRecordType = GetScienceRecordType (item.ScienceRecordTypeId);
            item.ScienceRecordType_Type = scienceRecordType.Type;

            item.Description = textDescription.Text;

            if (scienceRecordType.NumOfValues >= 1) {
                item.Value1 = TypeUtils.ParseToNullable<decimal> (textValue1.Text, false);
            }
            else {
                item.Value1 = null;
            }

            if (scienceRecordType.NumOfValues >= 2) {
                item.Value2 = TypeUtils.ParseToNullable<decimal> (textValue2.Text, false);
            }
            else {
                item.Value2 = null;
            }
        }

        protected override void OnResetForm ()
        {
            comboScienceRecordType.SelectedIndex = 0;
            SetupFormForType (int.Parse (comboScienceRecordType.SelectedValue));

            OnPartialResetForm ();
        }

        protected override void OnPartialResetForm ()
        {
            textDescription.Text = string.Empty;
            textValue1.Text = string.Empty;
            textValue2.Text = string.Empty;
        }

        protected override void BindItems (IEnumerable<ScienceRecordEditModel> items)
        {
            base.BindItems (items);

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
