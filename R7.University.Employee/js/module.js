/*$(function () {
    // paint tables with dnnGrid classes
    var table = $(".employeeDetails #employeeDisciplines table:not(.dnnGrid)");
    if (table)
    {
        table.addClass ("dnnGrid").css ("border-collapse", "collapse");
        // with tbody
        table.find ("tbody tr:nth-child(odd) td").parent ().addClass ("dnnGridItem")
            .next ().addClass ("dnnGridAltItem");
        // without tbody
        table.children ("tr:nth-child(even) td").parent ().addClass ("dnnGridItem")
            .next ().addClass ("dnnGridAltItem");
        // paint headers
        table.find ("tr th").parent ().addClass ("dnnGridHeader").attr ("align", "left");
    }
});*/