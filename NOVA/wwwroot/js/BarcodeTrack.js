var PAGE = { "BARCODE_NUMBER": null };

var PAGE_HANDLER = ObservableSlim.create(PAGE, true, function (changes) {
    for (const Change of changes) {
        if (Object.hasOwn(PAGE, Change.property)) {
            GetDataButtonState();
        }
    }
});

$('#BARCODE_NUMBER').on('input paste', function (e) {
    var Value = e.currentTarget.value;

    e.currentTarget.value = Value.toLocaleUpperCase('tr-TR');

    PAGE_HANDLER.BARCODE_NUMBER = Value?.trim().length > 14 ? Value : null;
});

function GetDataButtonState() {
    $('#GET_DATA_BUTTON').prop('disabled', PAGE.BARCODE_NUMBER == null ? true : false);
}

var elem = document.getElementById('panzoom-tree');
var panzoom2 = panzoom(elem, {
    maxZoom: 1.8,
    minZoom: 0.7,
    zoomDoubleClickSpeed: 1,
    startTransform: 'scale(0.8)',
    increment: 0.1,
    contain: 'invert'
});

$('body').on('click touchend', function (e) {
    if (e.target.className !== 'node') {
        $('.node').removeClass('hover');
    }
});

function TouchHandle(Elements) {
    var TouchHandler = new Hammer.Manager(Elements);

    var Tap = new Hammer.Tap({ event: 'tap' });
    var DoubleTap = new Hammer.Tap({ event: 'doubletap', taps: 2 });

    TouchHandler.add([DoubleTap, Tap]);

    TouchHandler.get('tap').requireFailure('doubletap');

    TouchHandler.on('tap doubletap', function (ev) {
        if (ev.target.className === 'node') {
            $(ev.target).addClass('hover');
        }

        $('.node').not(ev.target).removeClass('hover');

        if (ev.type == 'doubletap') {
            alert($(ev.target).data('barcode'));
        }
    });
}


var Nodes = $('.node');

for (const BarcodeNode of Nodes) {
    TouchHandle(BarcodeNode);
}

var parent = elem.parentElement;
parent.addEventListener('wheel', panzoom2.zoomWithWheel);


function QRScan(Barcode) {
    Swal.fire({
        title: Barcode,
        text: "Seri bilgisi izlensin mi?",
        icon: "success",
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'EVET',
        denyButtonText: 'HAYIR',
        target: document.getElementById('scanner-controls')
    }).then((result) => {
        if (result.isConfirmed)
        {
            $('#BARCODE_NUMBER').val(Barcode).trigger('paste');

            StopQRScanner();
        }

        IsBusy = false;
    })
}