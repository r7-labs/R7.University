function dd_shorten (n, text) {
    return (text.length > n) ? text.substring (0, n-1).trim () + "\u2026" : text;
}

function dd_treeNodeClicked (sender, eventArgs) {
    var nodeText = eventArgs.get_node ().get_text ();
    $(".division-directory #linkDivisions").text (dd_shorten (35, nodeText));
    $(".division-directory #linkDivisions").attr ("title", nodeText);
    $(".division-directory #hiddenDivisions").hide ();
}

function dd_treeLoad (sender, eventArgs) {
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0) {
        var nodeText = nodes [0].get_text ();
         $(".division-directory #linkDivisions").text (dd_shorten (35, nodeText));
         $(".division-directory #linkDivisions").attr ("title", nodeText);
    }
}