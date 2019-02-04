
"use strict";
function getMorris(type, element, dataStr) {
    var newStr;
    newStr = dataStr.replace(/"/g, '\'');

    var dataParced = JSON.stringify(newStr);
    var objJson = new Object();
    objJson = $.parseJSON(dataParced);
    
    if (type === 'bar') {
        Morris.Bar({
            element: element,
            data: [
                    {
                    x: 'Jan',
                    r: 200,
                    b: 300
                }, {
                    x: 'Feb',
                    r: 800,
                    b: 100
                }, {
                    x: 'Mar',
                    r: 300,
                    b: 500
                }, {
                    x: 'Apr',
                    r: 100,
                    b: 600
                }],
            xkey: 'x',
            ykeys: ['r', 'b'],
            labels: ['Residual', 'Bonus'],
            barColors: ['rgb(233, 30, 99)', 'rgb(0, 188, 212)'],
        });
    } else if (type === 'donut') {
        Morris.Donut({
            element: element,
            data: [
                objJson
            ],
            colors: ['rgb(233, 30, 99)', 'rgb(0, 188, 212)'],
            formatter: function (y) {
                return y + '%'
            }
        });
    } else if (type === 'accounts') {
        Morris.Bar({
            element: element,
            data: [
                {
                    x: 'Jan',
                    a: 2
                }, {
                    x: 'Feb',
                    a: 1
                }, {
                    x: 'Mar',
                    a: 3
                }, {
                    x: 'Apr',
                    a: 5
                }],
            xkey: 'x',
            ykeys: ['a'],
            labels: ['Accounts'],
            barColors: ['#0b62a4']
        });
    }
}

function donutChart(element, strResBon) {
    var valResidual = 0;
    var valBonus = 0;

    //50|50
    strResBon = strResBon.split("|");
    valResidual = strResBon[0];
    valBonus = strResBon[1];

    Morris.Donut({
        element: element,
        data: [
            { label: "Residual", value: valResidual },
            { label: "Bonus", value: valBonus }
        ],
        colors: ['rgb(69,127,202)', 'rgb(255,152,0)'],
        formatter: function (y) {
            return y + '%'
        }
    });
}

function barChart(element, strResBon) {

    //"Jan",500,200|"Feb",200,500
    strResBon = strResBon.split("|");
    Morris.Bar({
        element: element,
        data: [
            {
                x: strResBon[0].split(",")[0],
                r: strResBon[0].split(",")[1],
                b: strResBon[0].split(",")[2]
            }, {
                x: strResBon[1].split(",")[0],
                r: strResBon[1].split(",")[1],
                b: strResBon[1].split(",")[2]
            }, {
                x: strResBon[2].split(",")[0],
                r: strResBon[2].split(",")[1],
                b: strResBon[2].split(",")[2]
            }, {
                x: strResBon[3].split(",")[0],
                r: strResBon[3].split(",")[1],
                b: strResBon[3].split(",")[2]
            }, {
                x: strResBon[4].split(",")[0],
                r: strResBon[4].split(",")[1],
                b: strResBon[4].split(",")[2]
            }, {
                x: strResBon[5].split(",")[0],
                r: strResBon[5].split(",")[1],
                b: strResBon[5].split(",")[2]
            }, {
                x: strResBon[6].split(",")[0],
                r: strResBon[6].split(",")[1],
                b: strResBon[6].split(",")[2]
            }, {
                x: strResBon[7].split(",")[0],
                r: strResBon[7].split(",")[1],
                b: strResBon[7].split(",")[2]
            }, {
                x: strResBon[8].split(",")[0],
                r: strResBon[8].split(",")[1],
                b: strResBon[8].split(",")[2]
            }, {
                x: strResBon[9].split(",")[0],
                r: strResBon[9].split(",")[1],
                b: strResBon[9].split(",")[2]
            }, {
                x: strResBon[10].split(",")[0],
                r: strResBon[10].split(",")[1],
                b: strResBon[10].split(",")[2]
            }, {
                x: strResBon[11].split(",")[0],
                r: strResBon[11].split(",")[1],
                b: strResBon[11].split(",")[2]
            }
        ],
        xkey: 'x',
        ykeys: ['r', 'b'],
        labels: ['Residual', 'Bonus'],
        barColors: ['rgb(69,127,202)', 'rgb(255,152,0)'],
        //barColors: ['rgb(233, 30, 99)', 'rgb(0, 188, 212)'],
    });

}


function barAccount(element, strAccount, maxAccount) {

    
    strAccount = strAccount.split("|");
    Morris.Bar({
        element: element,
        data: [
            {
                x: strAccount[0].split(",")[0],
                a: strAccount[0].split(",")[1]
            }, {
                x: strAccount[1].split(",")[0],
                a: strAccount[1].split(",")[1]
            }, {
                x: strAccount[2].split(",")[0],
                a: strAccount[2].split(",")[1]
            }, {
                x: strAccount[3].split(",")[0],
                a: strAccount[3].split(",")[1]
            }, {
                x: strAccount[4].split(",")[0],
                a: strAccount[4].split(",")[1]
            }, {
                x: strAccount[5].split(",")[0],
                a: strAccount[5].split(",")[1]
            }, {
                x: strAccount[6].split(",")[0],
                a: strAccount[6].split(",")[1]
            }, {
                x: strAccount[7].split(",")[0],
                a: strAccount[7].split(",")[1]
            }, {
                x: strAccount[8].split(",")[0],
                a: strAccount[8].split(",")[1]
            }, {
                x: strAccount[9].split(",")[0],
                a: strAccount[9].split(",")[1]
            }, {
                x: strAccount[10].split(",")[0],
                a: strAccount[10].split(",")[1]
            }, {
                x: strAccount[11].split(",")[0],
                a: strAccount[11].split(",")[1]
            }
        ],
        xkey: 'x',
        ykeys: ['a'],
        labels: ['Accounts'], 
        barColors: ['#78B83E'],
        ymin: 0,
        ymax: maxAccount,
        //barColors: ['#0b62a4'],
    });

}