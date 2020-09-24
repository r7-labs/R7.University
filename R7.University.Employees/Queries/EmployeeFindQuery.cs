using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Employees.Queries
{
    internal class EmployeeFindQuery: QueryBase
    {
        public EmployeeFindQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IEnumerable<EmployeeInfo> FindEmployees (string search, bool teachersOnly, int divisionId)
        {
            if (divisionId != -1) {
                return ModelContext.Query<EmployeeInfo> (
                    "EXECUTE {objectQualifier}University_FindEmployees {0}, {1}", divisionId, teachersOnly
                ).ToList ().Where (
                    e => e.LastName.Contains (search)
                    || e.FirstName.Contains (search)
                    || e.OtherName.Contains (search)
                    || (e.FirstName + " " + e.LastName).Contains (search)
                    || (e.FirstName + " " + e.OtherName + " " + e.LastName).Contains (search)
                    || (e.LastName + " " + e.FirstName + " " + e.OtherName).Contains (search)
                    || e.CellPhone.Contains (search)
                    || e.Phone.Contains (search)
                    || e.Email.Contains (search)
                    || e.SecondaryEmail.Contains (search)
                    || e.WorkingPlace.Contains (search))
                .Distinct (new EntityEqualityComparer<EmployeeInfo> (e => e.EmployeeID));
            }
            return ModelContext.Query<EmployeeInfo> ()
                .IncludePositionsWithDivision ()
                .Where (e => teachersOnly == false || e.Positions.Any (op => op.Position.IsTeacher))
                .Where (
                    e => e.LastName.Contains (search)
                    || e.FirstName.Contains (search)
                    || e.OtherName.Contains (search)
                    || (e.FirstName + " " + e.LastName).Contains (search)
                    || (e.FirstName + " " + e.OtherName + " " + e.LastName).Contains (search)
                    || (e.LastName + " " + e.FirstName + " " + e.OtherName).Contains (search)
                    || e.CellPhone.Contains (search)
                    || e.Phone.Contains (search)
                    || e.Email.Contains (search)
                    || e.SecondaryEmail.Contains (search)
                    || e.WorkingPlace.Contains (search)
                )
                .ToList ()
                .Distinct (new EntityEqualityComparer<EmployeeInfo> (e => e.EmployeeID));
        }
    }
}
