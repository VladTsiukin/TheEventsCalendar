/* site.js */
'use strict';


/*-------------------- DATEPICKER ------------------*/

$('#datepicker').datepicker({
    language: 'ru',
    todayHighlight: true,
    todayBtn: true
});


$('#datepicker').on('changeDate', function () {
    $('#my_hidden_input').val(
        $('#datepicker').datepicker('getFormattedDate')
    );
});

/*------------------------------------------------- */


/* Container d-picker */
$('.container-fields').height(($(document).height() - 180));

/* d-picker-btn */
 $('#d-picker-btn').click((e) => {
     console.log('d-picker-btn go!');
});