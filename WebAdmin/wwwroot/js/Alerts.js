function GetTechStatusFn(companyid, locationid) {

    $.ajax({
        Type: 'POST',
        url: '/Cases/GetContractTechStaFn',
        dataType: 'json',
        data: { companyid, locationid },
        success: function (Result) {
            if (Result[0].hasTechnicalSpp == "1") {
                TechStatusSuccess();
                $('#TechSuccess').show();
                $('#TechDanger').hide();
                $('#SaveTechnical').show();
            } else {
                TechStatusDanger();
                $('#TechDanger').show();
                $('#TechSuccess').hide();
                $('#SaveTechnical').hide();
            }


        }
    })
}

function TechStatusSuccess() {
    $.notify({
        title: '<strong>Technical support Active.</strong>',
        message: ''
    }, {
            type: 'success',
            animate: {
                enter: 'animated flipInY',
                exit: 'animated flipOutX'
            }
            
        });

    
}

function TechStatusDanger() {
    $.notify({
        title: '<strong>This location has not active technical support. Please contact with your sales representative.</strong>',
        message: ''
    }, {
            type: 'danger',
            animate: {
                enter: 'animated flipInY',
                exit: 'animated flipOutX'
            }
        });
}

