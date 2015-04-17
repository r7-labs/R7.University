$(function () {
    // paint tables with dnnGrid classes
    var table = ".employeeDetails #employeeDisciplines table";
    $(table).addClass ("dnnGrid").css ("border-collapse", "collapse");
    // with tbody
    $(table + " tbody tr:nth-child(odd) td").parent ().addClass ("dnnGridItem")
        .next ().addClass ("dnnGridAltItem");
    // without tbody
    $(table + " > tr:nth-child(even) td").parent ().addClass ("dnnGridItem")
        .next ().addClass ("dnnGridAltItem");
    // paint headers
    $(table + " tr th").parent ().addClass ("dnnGridHeader").attr ("align", "left");
});