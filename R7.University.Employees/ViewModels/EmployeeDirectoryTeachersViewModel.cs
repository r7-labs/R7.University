using System.Collections.Generic;
using R7.Dnn.Extensions.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class EmployeeDirectoryTeachersViewModel
    {
        public IEnumerable<EduProfileViewModel> EduProfiles { get; set; }

        public ViewModelContext Context { get; protected set; }

        public EmployeeDirectoryTeachersViewModel SetContext (ViewModelContext context)
        {
            Context = context;
            return this;
        }
    }
}

