$(document).ready(function () {

    var x = $('input[name=Esadmin]')[0].value;
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
            height: 240,
            chartArea: { width: '98%', height: '80%' },
            hAxis: { showTextEvery: 7, textStyle: { fontSize: '10' } },
            legend: { position: 'top', textStyle: { color: 'blue', fontSize: 12 } },
            lineWidth: 4,
            pointShape: 'circle',
            pointSize: 6,
            vAxis: { textPosition: 'in', gridlines: { count: 3 }, minorGridlines: { count: 2 }, textStyle: { fontSize: 12 } },
            is3D: true,
            slices: { 0: { color: '#578CCF' }, 1: { color: '#85BF51' }, 2: { color: '#FFA219' }, 3: { color: '#1CB1F5' }, 4: { color: '#C5D2DF' }, 5: { color: '#9D96D1' }, 6: { color: '#C9C9C9' }, 7: { color: '#2B7BE4' } },
        };

        var piechart = new google.visualization.PieChart(document.getElementById('piechart_div'));
        piechart.draw(data, piechart_options);


    }
});

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;
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
            height: 240,
            chartArea: { width: '98%', height: '80%' },
            hAxis: { showTextEvery: 7, textStyle: { fontSize: '10' } },
            legend: { position: 'top', textStyle: { color: 'blue', fontSize: 12 } },
            lineWidth: 4,
            pointShape: 'circle',
            pointSize: 6,
            vAxis: { textPosition: 'in', gridlines: { count: 3 }, minorGridlines: { count: 2 }, textStyle: { fontSize: 12 } },
            is3D: true,
            slices: { 0: { color: '#578CCF' }, 1: { color: '#85BF51' }, 2: { color: '#FFA219' }, 3: { color: '#1CB1F5' }, 4: { color: '#C5D2DF' }, 5: { color: '#9D96D1' }, 6: { color: '#C9C9C9' }, 7: { color: '#2B7BE4' } },


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
        data.addColumn({ type: 'string', role: 'style' });


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.assignedTo, obj.cases, 'color: #1CB1F5']);
        });

        data.addRows(dataArray);

        var piechart_options = {
            // title: 'VISITED BY SALER',
            bar: { groupWidth: "90%" },
            legend: { position: "none" },


        };
        var piechart = new google.visualization.ColumnChart(document.getElementById('visited_div'));
        piechart.draw(data, piechart_options);


    }
});

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdatasetvisitedday/' + x + '',
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
        data.addColumn('string', 'date');

        data.addColumn('number', 'cases');
        data.addColumn({ type: 'string', role: 'style' });

        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.date, obj.cases, 'color: #1CB1F5']);
        });

        data.addRows(dataArray);

        var piechart_options = {
            //title: 'VISITED BY DATE',
            bar: { groupWidth: "90%" },
            legend: { position: "none" },


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
            var porcent;
            var calpor;
            porcent = (result[0].quantityReal) / (result[0].goalNewContracts);
            calpor = porcent * 100;

            var admin = $('input[name=Esadmin]')[0].value;
            if (admin == 1) {
                document.getElementById("contract").innerText = result[0].quantityReal;
            }

            document.getElementById("porcentcurrent").value = calpor;

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

                    if (val.cases == 0) {
                        row += '<tr>';
                        row += '<td style="text-align:left">' + val.salesMan + '</td>';
                        row += '<td>' + val.cases + '</td>'
                        row += '</tr>';
                    } else {
                        row += '<tr style="background:#AECAF9;">';
                        row += '<td style="text-align:left">' + val.salesMan + '</td>';
                        row += '<td>' + val.cases + '</td>'
                        row += '</tr>';
                    }


                    var admin = $('input[name=Esadmin]')[0].value;
                    if (admin >= 1) {
                        var user = $('input[name=UserID]')[0].value;
                        if (val.salesID == user) {
                            document.getElementById("contract").innerText = val.cases;
                        }
                    }


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

            var porcent;
            var calpor;
            porcent = (result[0].quantityReal) / (result[0].goalNewContracts);
            calpor = porcent * 100;


            document.getElementById("porcentlast").value = calpor;

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
                    if (val.cases == 0) {
                        row += '<tr>';
                        row += '<td style="text-align:left">' + val.salesMan + '</td>';
                        row += '<td>' + val.cases + '</td>'
                        row += '</tr>';
                    } else {
                        row += '<tr style="background:#C0C0C0;">';
                        row += '<td style="text-align:left">' + val.salesMan + '</td>';
                        row += '<td>' + val.cases + '</td>'
                        row += '</tr>';
                    }
                });


                $('#bycurrentlast').html(row);
            }
        });
    }


});

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;

    var row = '';
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getopportunitiesonline/' + x + '',
        success: function (response) {

            var result = response;

            $.each(result, function (index, val) {

                row += '<tr>';
                row += '<td colspan="2">';
                row += '<table  width="100%" class="table-bordered">';
                row += '<thead style="background: #4C8BF5; ">';

                row += '<tr>';
                row += '<th>' + val.correlativo + '</th>';
                row += '<th>' + val.date + '</th>';

                row += '<th>' + val.salesman + '</th>';

                row += '<th >' + val.howFoundName + '</th>';
                row += '<th>' + val.numberLead + '</th>';
                row += '<th colspan="6">' + val.programName + '</th>';
                row += '</tr>';


                row += '</thead>';
                row += '<tbody>';

                row += '<tr >';
                row += '<td colspan="4"><b>' + val.company + '</b></td>';
                row += '<td style="text-align:center">' + val.phoneNumber + '</td>';
                if (val.vrfd1 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd1 + '</td>';
                }
                if (val.vrfd2 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd2 + '</td>';
                }
                if (val.vrfd3 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd3 + '</td>';
                }
                if (val.vrfd4 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd4 + '</td>';
                }
                if (val.vrfd5 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd5 + '</td>';
                }
                row += '<td>R</td>';
                row += '</tr>';
                // row += '<td colspan="6" style="text-align:right; background: #4C8BF5; color: white;">' + val.programName + '</td>';
                row += '</tr>';

                row += '<tr>';
                row += '<td colspan="4" style="text-align:center">' + val.email + '</td>';
                row += '<td style="text-align:center">' + val.date + '</td>';
                if (val.vrfd1 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd2 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                }
                else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd3 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd4 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd5 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }

                if (val.rating == "Cold") {
                    row += '<td><img  src="../images/Cold.png" width="15" height="20"/></td>';
                }
                if (val.rating == "Warm") {
                    row += '<td><img  src="../images/Warm.png" width="15" height="20"/></td>';
                }

                row += '<tr>';
                row += '<td  colspan="11">' + val.lastFollowup + '</td>';
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

$(document).ready(function () {

    
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/opporstatusquantity',
        success: function (result) {
            google.charts.load('current', { packages: ['corechart', 'bar'] });

           
            var Resultado = GetArray(result, 0, 1, 2);
            google.charts.setOnLoadCallback(function () {
                drawChart(Resultado);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.arrayToDataTable(result);
        

        var piechart_options = {
           
           
            legend: { position: 'top', maxLines: 2 },
            colors: ['#1b9e77', '#d95f02'],
            bar: { groupWidth: '75%' },

            
            
        
        };

        var barchart = new google.visualization.ColumnChart(document.getElementById('barstatus_div'));
        barchart.draw(data, google.charts.Bar.convertOptions(piechart_options));


    }
});


$(document).ready(function () {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/opporonlinesaler',
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
        data.addColumn({ type: 'string', role: 'style' });


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.assignedTo, obj.cases, 'color: #1CB1F5']);
        });

        data.addRows(dataArray);

        var piechart_options = {
            // title: 'VISITED BY SALER',
            bar: { groupWidth: "90%" },
            legend: { position: "none" },


        };
        var piechart = new google.visualization.ColumnChart(document.getElementById('opporonlinesaler_div'));
        piechart.draw(data, piechart_options);


    }
});


function GetArray(dataArray, rowIndex, colIndex, dataIndex) {

    var result = {}, ret = [];
    var newCols = [];
    for (var i = 0; i < dataArray.length; i++) {

        if (!result[dataArray[i][rowIndex]]) {
            result[dataArray[i][rowIndex]] = {};
        }
        result[dataArray[i][rowIndex]][dataArray[i][colIndex]] = dataArray[i][dataIndex];

        //To get column names
        if (newCols.indexOf(dataArray[i][colIndex]) == -1) {
            newCols.push(dataArray[i][colIndex]);
        }
    }

    newCols.sort();
    var item = [];

    //Add Header Row
    item.push('Follows');
    item.push.apply(item, newCols);
    ret.push(item);

    //Add content 
    for (var key in result) {
        item = [];
        item.push(key);
        for (var i = 0; i < newCols.length; i++) {
            item.push(parseInt(result[key][newCols[i]]) || 0);
        }
        ret.push(item);
    }
    return ret;

}