function PrintElem(elem) {
    //var x = window.screenX, y = window.screenY;
    //var height = document.body.getBoundingClientRect().height;
    //var width = document.body.getBoundingClientRect().width;
    //var mywindow = window.open('', 'my div', 'top=' + y + ', left=' + x + ',height=' + height + ',width=' + width);
    ////var mywindow = window.open('', 'PRINT', 'height=400,width=600');

    //mywindow.document.write('<html><head><title>' + document.title + '</title>');
    //mywindow.document.write('</head><body >');
    //mywindow.document.write('<h1>' + document.title + '</h1>');
    //mywindow.document.write(document.getElementById(elem).innerHTML);
    //mywindow.document.write('</body></html>');

    //mywindow.document.close(); // necessary for IE >= 10
    //mywindow.focus(); // necessary for IE >= 10*/

    //mywindow.print();
    //mywindow.close();
}

function printContent(el) {
    var restorepage = $('body').html();
    var printcontent = $('#' + el).clone();
    $('body').empty().html(printcontent);
    window.print();
    $('body').html(restorepage);
}