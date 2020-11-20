function GridAndFormUniqueValidator (uniqueField) {
    this.uniqueField = uniqueField;
}

GridAndFormUniqueValidator.prototype.getSelectedFieldId = function (valContext) { throw "Override in a child class"; }
GridAndFormUniqueValidator.prototype.getEditedFieldId = function (valContext) { throw "Override in a child class"; }

GridAndFormUniqueValidator.prototype.validate = function (sender, e) {
    var valContext = jQuery(sender).closest (".dnnForm");
    if (valContext.length === 0) {
        e.IsValid = false;
        throw "Could not find validation context for " + sender;
    }
    var itemsData = valContext.find ("[id $= '_gridItems']").attr ("data-items");
    if (!!itemsData) {
        var items = JSON.parse (itemsData);
        if (items.length > 0) {
            var selectedFieldId = this.getSelectedFieldId (valContext);
            var addCmd = valContext.find ("[id $= '_buttonAddItem']").length === 1;
            var count = items.filter (function (i) { return i[this.uniqueField] == selectedFieldId && i.EditState != "Deleted"; }, this).length;
            if (addCmd && count === 0) { return; }
            var editedFieldId = this.getEditedFieldId (valContext);
            if (!addCmd && count === ((selectedFieldId === editedFieldId)? 1 : 0)) { return; }
            e.IsValid = false;
        }
    }
}
