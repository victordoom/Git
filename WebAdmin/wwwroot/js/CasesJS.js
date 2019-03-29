$(document).ready(function () {

    //mantener menu desplegado
    var si = "";
    var url = window.location.pathname;

    if (url == "/") {
        localStorage.setItem('memomenu', 'no')
    }

    if (localStorage.getItem('memomenu') == 'activotech') {
        $("#tech").addClass("menu-open");
        document.getElementById("techsubmenu").style.display = 'block';
    } else {
        if (localStorage.getItem('memomenu') == 'activosales') {
            $("#sales").addClass("menu-open");
            document.getElementById("salessubmenu").style.display = 'block';
        } else {
            if (localStorage.getItem('memomenu') == 'activoadmin') {
                $("#admin").addClass("menu-open");
                document.getElementById("adminsubmenu").style.display = 'block';
            }
        }
    }
    
    
});

function sitech() {
    if ($("#tech").hasClass("menu-open") == true) {
        localStorage.setItem('memomenu', 'no');
    } else {
        localStorage.setItem('memomenu', 'activotech');
    }
}

function sisales() {
    if ($("#sales").hasClass("menu-open") == true) {
        localStorage.setItem('memomenu', 'no');
    } else {
        localStorage.setItem('memomenu', 'activosales');
    }
}

function siadminis() {
    if ($("#admin").hasClass("menu-open") == true) {
        localStorage.setItem('memomenu', 'no');
    } else {
        localStorage.setItem('memomenu', 'activoadmin');
    }
}