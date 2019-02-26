$('#opportunitiesclosed').on('shown.bs.modal', function () {

});

function CerrarOppor(id, byclose, action) {
    var closedcomment = $('textarea[name=ClosedComment]')[0].value;

    if (closedcomment == "") {
        $('#ClosedComment').focus();
    }

}