function validateTimeToLearn (sender, e) {
    var years = jQuery ("[id $= '_textTimeToLearnYears']").val ();
    var months = jQuery ("[id $= '_textTimeToLearnMonths']").val ();
    var hours = jQuery ("[id $= '_textTimeToLearnHours']").val ();
    if (years == 0 && months == 0 && hours == 0) {
        e.IsValid = false;
    }
}