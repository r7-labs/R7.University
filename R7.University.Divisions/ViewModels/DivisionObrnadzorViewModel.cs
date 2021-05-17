using System.Collections.Generic;
using System.Linq;
using System.Web;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Divisions.Models;
using R7.University.Divisions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Divisions
{
    internal class DivisionObrnadzorViewModel: DivisionViewModelBase
    {
        protected new ViewModelContext<DivisionDirectorySettings> Dnn { get; set; }

        #region Bindable properties

        public string Order { get; protected set; }

        public new int Level { get; protected set; }

        public IOccupiedPosition HeadEmployeePosition { get; set; }

        #endregion

        public DivisionObrnadzorViewModel (IDivision division, ViewModelContext<DivisionDirectorySettings> dnn): base (division, dnn)
        {
            Dnn = dnn;
        }

        public static IEnumerable<DivisionObrnadzorViewModel> Create (IEnumerable<DivisionInfo> divisions, ViewModelContext<DivisionDirectorySettings> viewModelContext)
        {
            var now = HttpContext.Current.Timestamp;

            // TODO: If parent division not published, ensure what child divisions also not
            var divisionViewModels = divisions.Select (d => new DivisionObrnadzorViewModel (d, viewModelContext))
                .Where (d => d.IsPublished (now) || viewModelContext.Module.IsEditable)
                .Where (d => !d.IsInformal || viewModelContext.Settings.ShowInformal || viewModelContext.Module.IsEditable)
                .ToList ();

            WithHeadEmployeePositions (divisionViewModels);
            CalculateOrder (divisionViewModels);

            return divisionViewModels;
        }

        protected static void WithHeadEmployeePositions (IEnumerable<DivisionObrnadzorViewModel> divisions)
        {
            var now = HttpContext.Current.Timestamp;
            foreach (var division in divisions) {
                division.HeadEmployeePosition = division.GetHeadEmployeePositions ().FirstOrDefault (hep => hep.Employee.IsPublished (now));
            }
        }

        /// <summary>
        /// Calculates the hierarchical order of divisions.
        /// </summary>
        /// <param name="divisions">Divisions that must be properly sorted before the call.</param>
        protected static void CalculateOrder (IList<DivisionObrnadzorViewModel> divisions)
        {
            // TODO: Get hierarchical data from DB without recalculating it?

            const string separator = ".";
            var orderCounter = 1;
            var orderStack = new List<int> ();
            var returnStack = new Stack<DivisionObrnadzorViewModel> ();

            DivisionObrnadzorViewModel prevDivision = null;

            foreach (var division in divisions) {
                if (prevDivision != null) {
                    if (division.ParentDivisionID == prevDivision.ParentDivisionID) {
                        // moving on same level
                        orderCounter++;
                    }
                    else if (division.ParentDivisionID == prevDivision.DivisionID) {
                        // moving down
                        orderStack.Add (orderCounter);
                        returnStack.Push (prevDivision);
                        orderCounter = 1;
                    }
                    else {
                        // moving up
                        while (returnStack.Count > 0 && orderStack.Count > 0) {
                            orderCounter = orderStack [orderStack.Count - 1];
                            orderStack.RemoveAt (orderStack.Count - 1);

                            if (division.ParentDivisionID == returnStack.Pop ().ParentDivisionID) {
                                break;
                            }
                        }

                        orderCounter++;
                    }
                }

                // format order value
                if (orderStack.Count == 0) {
                    division.Order = orderCounter + separator;
                    division.Level = 0;
                }
                else {
                    division.Order = FormatHelper.JoinNotNullOrEmpty (separator, orderStack.Select (o => o.ToString ())) + separator + orderCounter + separator;
                }

                prevDivision = division;
            }
        }

        // TODO: Split data into 2 columns?
        public string HeadEmployeeHtml {
            get {
                if (HeadEmployeePosition != null) {
                    var positionTitle = UniversityFormatHelper.FormatShortTitle (HeadEmployeePosition.Position.ShortTitle, HeadEmployeePosition.Position.Title);
                    var headEmployee =  HeadEmployeePosition.Employee;
                    return $"<a href=\"{Dnn.Module.EditUrl ("employee_id", headEmployee.EmployeeID.ToString (), "EmployeeDetails")}\"><span itemprop=\"fio\">{UniversityFormatHelper.FullName (headEmployee.FirstName, headEmployee.LastName, headEmployee.OtherName)}</span></a><br />"
                           + $"<span itemprop=\"post\">{FormatHelper.JoinNotNullOrEmpty (" ", positionTitle, HeadEmployeePosition.TitleSuffix)}</span>";
                }

                if (HeadPositionID != null) {
                    return Dnn.LocalizeString ("HeadPosition_IsVacant.Text");
                }
                return Dnn.LocalizeString ("HeadPosition_NotApplicable.Text");
            }
        }
    }
}

