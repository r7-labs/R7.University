function validateTimeToLearn (sender, e) {
    var years = jQuery ("[id $= 'textTimeToLearnYears']").val ();
    var months = jQuery ("[id $= 'textTimeToLearnMonths']").val ();
    var hours = jQuery ("[id $= 'textTimeToLearnHours']").val ();
    if (years == 0 && months == 0 && hours == 0) {
        e.IsValid = false;
    }
}

function EduFormUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["EduFormID"]);
}

EduFormUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
EduFormUniqueValidator.prototype.constructor = EduFormUniqueValidator;
EduFormUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("input[id *= '_radioEduForm_']:checked").val ();
};

EduFormUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= 'hiddenEduFormID']").val ();
};

eduFormUniqueValidator = new EduFormUniqueValidator ();