// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getconsulta/0',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'HowFoung');

        data.addColumn('number', 'cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.howFoung, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
           // title: 'HOW FOUND',
            width: 350,
            height: 250,
            pieHole: 0.4,
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_div'));
        piechart.draw(data, piechart_options);

        
    }
});

$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdataset/0',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Category');

        data.addColumn('number', 'Cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.category, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
          //  title: 'CATEGORY',
            width: 350,
            height: 250,
            pieHole: 0.4,
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_divcate'));
        piechart.draw(data, piechart_options);

        
    }
});


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdatasetvisited',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);

                var suma = 0;
                for (var i = 0; i < result.length; i++) {

                    suma += result[i].cases;

                }
                //const sumValues = result => Object.values(result).reduce((result.cases) => result.cases + result.cases);

                document.getElementById("visited").innerText = suma;
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'assignedTo');

        data.addColumn('number', 'cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.assignedTo, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
           // title: 'VISITED BY SALER',
            width: 350,
            height: 250,
            
        };
        var piechart = new google.visualization.ColumnChart(document.getElementById('visited_div'));
        piechart.draw(data, piechart_options);


    }
});

$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdatasetvisitedday',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string','date');

        data.addColumn('number', 'cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.date, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            //title: 'VISITED BY DATE',
            width: 350,
            height: 250,

        };
        var piechart = new google.visualization.ColumnChart(document.getElementById('visiteddate_div'));
        piechart.draw(data, piechart_options);


    }
});


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getcurrentmonth',
        success: function (result) {
            document.getElementById("Goal").innerText = result[0].goalYear;
            var mes = result[0].goalMonth;
            var month;
            switch (mes) {

                case 1:
                    month = "January";
                    break;
                case 2:
                    month = "February";
                    break;
                case 3:
                    month = "March";
                    break;

                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "June";
                    break;
                case 7:
                    month = "July";
                    break;
                case 8:
                    month = "August";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "October";
                    break;
                case 11:
                    month = "November";
                    break;
                case 3:
                    month = "December";
                    break;
                default:
            }

            document.getElementById("Month").innerText = month;
            document.getElementById("GoalNew").innerText = result[0].goalNewContracts;
            document.getElementById("Quantity").innerText = result[0].quantityReal;
            var calcu;
            calcu = result[0].goalNewContracts - result[0].quantityReal;
            document.getElementById("Calcu").innerText = calcu;

            document.getElementById("contract").innerText = result[0].quantityReal;


            var monthby = result[0].goalMonth;
            var yearby = result[0].goalYear;

            GetByCurrentMonth(monthby, yearby);


           
        }
    });

    function GetByCurrentMonth(month, year) {
        var row = '';

        $.ajax({
            type: 'GET',
            dataType: "json",
            contentType: "application/json",
            url: '/api/Dashboard/getbycurrentmonth/' + month + '/' + year + '',
           
            success: function (response) {
                var result = response;

                $.each(result, function (index, val) {
                    row += '<tr>';
                    row += '<td>' + val.salesMan + '</td>';
                    row += '<td>' + val.cases + '</td>'
                    row += '</tr>';
                });


                $('#bycurrent').html(row);
            }
        });
    }

    
});


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getcurrentmonthlast',
        success: function (result) {
            document.getElementById("GoalLast").innerText = result[0].goalYear;
            var mes = result[0].goalMonth;
            var month;
            switch (mes) {

                case 1:
                    month = "January";
                    break;
                case 2:
                    month = "February";
                    break;
                case 3:
                    month = "March";
                    break;

                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "June";
                    break;
                case 7:
                    month = "July";
                    break;
                case 8:
                    month = "August";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "October";
                    break;
                case 11:
                    month = "November";
                    break;
                case 12:
                    month = "December";
                    break;
                default:
            }

            document.getElementById("MonthLast").innerText = month;
            document.getElementById("GoalNewLast").innerText = result[0].goalNewContracts;
            document.getElementById("QuantityLast").innerText = result[0].quantityReal;
            var calcu;
            calcu = result[0].goalNewContracts - result[0].quantityReal;
            document.getElementById("CalcuLast").innerText = calcu;


            var monthby = result[0].goalMonth;
            var yearby = result[0].goalYear;

           GetByCurrentMonthLast(monthby, yearby);



        }
    });

    function GetByCurrentMonthLast(month, year) {
        var row = '';

        $.ajax({
            type: 'GET',
            dataType: "json",
            contentType: "application/json",
            url: '/api/Dashboard/getbycurrentmonthlast/' + month + '/' + year + '',

            success: function (response) {
                var result = response;

                $.each(result, function (index, val) {
                    row += '<tr>';
                    row += '<td>' + val.salesMan + '</td>';
                    row += '<td>' + val.cases + '</td>'
                    row += '</tr>';
                });


                $('#bycurrentlast').html(row);
            }
        });
    }


});

$(document).ready(function () {
    var row = '';
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getopportunitiesonline',
        success: function (response) {
            
            var result = response;

            $.each(result, function (index, val) {

                row += '<tr>';
                row += '<td colspan="9">';
                row += '<table  width="100%">';
                row += '<thead style="background: blue; ">';
                row += '<tr>';
                row += '<th >' + val.date + '</th>';
                row += '<th>' + val.salesman + '</th>';
                row += '<th>' + val.howFoundName + '</th>';
                row += '<th>' + val.numberLead + '</th>';
                row += '<th colspan="7">' + val.programName + '</th>';
                row += '</tr>';
                row += '</thead>';
                row += '<tbody>';
                row += '<tr style="border: #E6E6E6 3px solid;">';
                row += '<td colspan="3">' + val.company + '</td>';
                row += '<td>' + val.phoneNumber + '</td>';
                //row += '</tr>';
               // row += '<tr>';
                row += '<td>' + val.vrfd1 + '</td>';
                row += '<td>' + val.vrfd2 + '</td>';
                row += '<td>' + val.vrfd3 + '</td>';
                row += '<td>' + val.vrfd4 + '</td>';
                row += '<td>' + val.vrfd5 + '</td>';
                row += '<td>R</td>';
                row += '</tr>';
                row += '<tr style="border: #E6E6E6 3px solid;">';
                row += '<td colspan="3">' + val.email + '</td>';
                row += '<td>' + val.date + '</td>';

                if (val.vrfd1 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="15"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="15"/></td>';
                }
                if (val.vrfd2 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="15"/></td>';
                }
                else {
                    row += '<td><img  src="../images/close.png" width="15" height="15"/></td>';
                }
                if (val.vrfd3 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="15"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="15"/></td>';
                }
                if (val.vrfd4 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="15"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="15"/></td>';
                }
                if (val.vrfd5 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="15"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="15"/></td>';
                }
                
                if (val.rating == "Cold") {
                    row += '<td><img  src="../images/Cold.png" width="15" height="15"/></td>';
                }
                if (val.rating == "Warm") {
                    row += '<td><img  src="../images/Warm.png" width="15" height="15"/></td>';
                }
                 row += '</tr>';
                 row += '<tr>';
                row += '<td colspan="10">' + val.lastFollowup + '</td>';
                row += '</tr>';
                row += '</tbody>';
                row += '</table>';
                row += '</td>';
                row += '</tr>';
                
            });


            $('#opponline').html(row);


        }
    });

    


});

function filtropiechart() {
    var x = document.getElementById("Select").value;

    filtrar(x);
    filtrardos(x);

    


    
}

function filtrar(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getconsulta/' + x + '',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'HowFoung');

        data.addColumn('number', 'cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.howFoung, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            // title: 'HOW FOUND',
            width: 350,
            height: 250,
            pieHole: 0.4,
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_div'));
        piechart.draw(data, piechart_options);


    }
}
function filtrardos(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdataset/' + x + '',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Category');

        data.addColumn('number', 'Cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.category, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            //  title: 'CATEGORY',
            width: 350,
            height: 250,
            pieHole: 0.4,
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_divcate'));
        piechart.draw(data, piechart_options);


    }
}