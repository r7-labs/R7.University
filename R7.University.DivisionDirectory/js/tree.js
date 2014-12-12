function dd_shorten (n, text) {
    return (text.length > n) ? text.substring (0, n-1).trim () + "\u2026" : text;
}

function dd_treeNodeClicked (sender, eventArgs) {
    var nodeText = eventArgs.get_node ().get_text ();
    $(".divisionDirectory #linkDivisions").text (dd_shorten (35, nodeText));
    $(".divisionDirectory #linkDivisions").attr ("title", nodeText);
    $(".divisionDirectory #hiddenDivisions").hide ();
}

function dd_treeLoad (sender, eventArgs) {
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0) {
        var nodeText = nodes [0].get_text ();
         $(".divisionDirectory #linkDivisions").text (dd_shorten (35, nodeText));
         $(".divisionDirectory #linkDivisions").attr ("title", nodeText);
    }
}