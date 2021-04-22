using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.ViewModels;

namespace R7.University.Controls.EditModels
{
    public abstract class EditModelBase<TModel>: IEditModel<TModel>
    {
        public int ViewItemID { get; set; } = ViewNumerator.GetNextItemID ();

        [JsonIgnore]
        public ViewModelContext Dnn { get; set; }

        [JsonConverter (typeof (StringEnumConverter))]
        public ModelEditState PrevEditState { get; set; } = ModelEditState.Unchanged;

        [JsonConverter (typeof (StringEnumConverter))]
        public ModelEditState EditState { get; set; } = ModelEditState.Unchanged;

        public virtual bool IsPublished => true;

        public abstract TModel CreateModel ();

        public abstract IEditModel<TModel> Create (TModel model, ViewModelContext dnn);

        public abstract void SetTargetItemId (int targetItemId, string targetItemKey);

        public virtual void SetEditState (ModelEditState value)
        {
            PrevEditState = EditState;
            EditState = value;
        }

        public virtual void RestoreEditState ()
        {
            EditState = PrevEditState;
            PrevEditState = ModelEditState.Unchanged;
        }
    }
}
