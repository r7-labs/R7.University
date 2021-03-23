using System.Collections.Generic;
using R7.Dnn.Extensions.Models;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateEduProgramDivisionsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateEduProgramDivisionsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void Update (IEnumerable<IEditModel<EduProgramDivisionInfo>> epDivs, ModelType modelType, int itemId)
        {
            foreach (var epDiv in epDivs) {
                var epd = epDiv.CreateModel ();
                switch (epDiv.EditState) {
                    case ModelEditState.Added:
                        epd.SetModelId (modelType, itemId);
                        ModelContext.Add (epd);
                        break;
                    case ModelEditState.Modified:
                        ModelContext.UpdateExternal (epd);
                        break;
                    case ModelEditState.Deleted:
                        ModelContext.RemoveExternal (epd);
                        break;
                }
            }
        }
    }
}

