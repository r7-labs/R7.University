function ed_shorten (n, text) {
    return (text.length > n) ? text.substring (0, n-1).trim () + "\u2026" : text;
}

function ed_treeNodeClicked (sender, eventArgs) {
    var nodeText = eventArgs.get_node ().get_text ();
    $(".employeeDirectory #linkDivisions").text (ed_shorten (35, nodeText));
    $(".employeeDirectory #linkDivisions").attr ("title", nodeText);
    $(".employeeDirectory #hiddenDivisions").hide ();
}

function ed_treeLoad (sender, eventArgs) {
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0) {
        var nodeText = nodes [0].get_text ();
         $(".employeeDirectory #linkDivisions").text (ed_shorten (35, nodeText));
         $(".employeeDirectory #linkDivisions").attr ("title", nodeText);
    }
}