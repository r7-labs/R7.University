using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;

namespace R7.University.EditModels
{
    public interface IEditModel<TModel>
    {
        int ViewItemID { get; set; }

        ViewModelContext Dnn { get; set; }

        ModelEditState EditState { get; set; }

        ModelEditState PrevEditState { get; set; }

        bool IsPublished { get; }

        TModel CreateModel ();

        IEditModel<TModel> Create (TModel model, ViewModelContext dnn);

        void SetTargetItemId (int targetItemId, string targetItemKey);

        void SetEditState (ModelEditState value);

        void RestoreEditState ();
    }
}
