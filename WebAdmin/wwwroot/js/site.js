// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getconsulta',
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
        url: '/api/Dashboard/getdataset',
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


