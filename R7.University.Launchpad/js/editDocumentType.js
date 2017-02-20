function validateFilenameFormat (sender, e) {
    try {
        var regex = new RegExp (e.Value);
    }
    catch (err) {
        e.IsValid = false;
    }
}