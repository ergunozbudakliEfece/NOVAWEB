
//#region Page Handle

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

function QRScan(Barcode) {
    $('#BARCODE_NUMBER').val(Barcode).trigger('paste');

    StopQRScanner();

    GetDataAction();

    IsBusy = false;
}

//#endregion

//#region Panzoom

var PanzoomElement = document.getElementById('panzoom-tree');

var PanzoomObject = panzoom(PanzoomElement, {
    maxZoom: 1.8,
    minZoom: 0.7,
    zoomDoubleClickSpeed: 1,
    increment: 0.1
});

PanzoomElement.parentElement.addEventListener('wheel', PanzoomObject.zoomWithWheel);

//#endregion

//#region Touch Events
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

        if (ev.type == 'tap') {
            ShowProgress();
            GetDetailData($(ev.target).data('barcode'));
        }
    });
}

$('body').on('click touchend', function (e) {
    if (e.target.className !== 'node') {
        $('.node').removeClass('hover');
    }
});

function SetTouchEvent()
{
    var Nodes = $('.node');

    for (const BarcodeNode of Nodes) {
        TouchHandle(BarcodeNode);
    }
}

//#endregion

//#region FetchData

var Connections = [];
var Connection_ID = 0;

async function GetData(Barcode, ParentBarcode = null) {
    var response = await axios.get(`http://192.168.2.13:83/api/uretim/diagram/${Barcode}`);

    var Connection = Connections.filter(x => x.Barcode == ParentBarcode);

    var Parent_ID = -1;

    if (Connection.length > 0) {
        Parent_ID = Connection[0].Connection_ID;
    }

    Connection = Connections.filter(x => x.Barcode == Barcode);

    if (Connection.length > 0) {
        Connections[Parent_ID].Connection_ID = Connection[0].Parent_ID;

        Parent_ID = Connection[0].Parent_ID;

        Connection_ID = Connection[0].Connection_ID;
    }

    Connections.push({
        "Connection_ID": Connection_ID++,
        "Parent_ID": Parent_ID,
        "Barcode": Barcode
    });

    for (const data of response.data) {
        await GetData(data.SERI_NO, Barcode);
    }
}

function BarcodeTree(Barcode) {

    Connections = [];
    Connection_ID = 0;

    GetData(Barcode).then(() => {
        var UniqueConnections = Array.from(new Set(Connections.map(JSON.stringify))).map(JSON.parse).sort((a, b) => a.Parent_ID - b.Parent_ID);

        var Branches = [...new Set(UniqueConnections.map(x => x.Parent_ID))];

        for (const Branch of Branches) {
            var BranchGroup = UniqueConnections.filter(x => x.Parent_ID == Branch);

            if (Branch == -1) {
                $('.tree').html(`<ul><li id="con_${BranchGroup[0].Connection_ID}"><div class="root nodes"><div class="node" data-barcode="${BranchGroup[0].Barcode}">${BranchGroup[0].Barcode}</div></div></li></ul>`)
            }
            else {
                var ChildBranches = [...new Set(BranchGroup.map(x => x.Connection_ID))];

                var Template = `<ul>`;

                for (const ChildBranch of ChildBranches) {
                    var Items = BranchGroup.filter(x => x.Connection_ID == ChildBranch);

                    Template += `<li id="con_${ChildBranch}"><div class="nodes">`;

                    for (const Item of Items) {
                        Template += `<div class="node" data-barcode="${Item.Barcode}">${Item.Barcode}</div>`;
                    }

                    Template += '</li>';
                }

                $(`#con_${BranchGroup[0].Parent_ID}`).append(Template + `</ul>`);

            }
        }

        SetTouchEvent();

        var Zoom = 1 - ($('#panzoom-tree').height() * $('#panzoom-tree').width()) / ($('#tree-view').height() * $('#tree-view').width());
        PanzoomObject.smoothZoomAbs(0, 0, Zoom);
    });
}

function GetDataAction()
{
    $('.tree').html(`Ağaç Oluşturuluyor...`);

    var Barcode = $('#BARCODE_NUMBER').val();

    axios.get(`http://192.168.2.13:83/api/seri/kontrol/detay/${Barcode}`)
        .then((response) =>
        {
            if (response.data.length > 0) {
                BarcodeTree(Barcode);
            }
            else {
                $('.tree').html('');

                Swal.fire({
                    icon: "error",
                    title: "Seri bilgilerine ulaşılamadı.",
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        });
}

async function GetDetailData(Barcode)
{
    var response = await axios.get(`http://192.168.2.13:83/api/seri/kontrol/detay/${Barcode}`);

    if (response.data.length > 0)
    {
        $('#barcode-title').html(`${Barcode} Detay`);

        $('#STOCK_NAME').html(`<b>${response.data[0].STOK_ADI}</b>`);
        $('#STOCK_CODE').html(`<b>${response.data[0].STOK_KODU}</b>`);

        $('#AMOUNT_1').html(`${NumberFormatter(response.data[0].MIKTAR1, response.data[0].OLCU_BR1)}`);
        $('#AMOUNT_2').html(`${NumberFormatter(response.data[0].MIKTAR2, response.data[0].OLCU_BR2)}`);

        $('#THICKNESS').html(`${response.data[0].KALINLIK}`);
        $('#WIDTH').html(`${response.data[0].GENISLIK}`);
        $('#HEIGHT').html(`${ZeroToHypen(response.data[0].BOY)}`);
        $('#QUANTITY').html(`${NumberFormatter(response.data[0].METRAJ, 'M')}`);
        $('#ACIK5').html(`${EmptyTextToHypen(response.data[0].ACIKLAMA_5)}`);

        $('#MACHINE_OP').html(`${EmptyTextToHypen(response.data[0].MAK_KODU)} / ${EmptyTextToHypen(response.data[0].KAYITYAPANKUL)}`);

        $('#CREATE_DATE').html(`${moment(response.data[0].KAYITTARIHI).format('DD/MM/YYYY HH:mm:ss')}`);

        HideProgress();

        $('#barcode-detail-modal').modal('show');
    }
    else
    {
        Swal.fire({
            icon: "error",
            title: "Seri bilgilerine ulaşılamadı.",
            showConfirmButton: false,
            timer: 1500
        });

        HideProgress();
    }
}

function NumberFormatter(Value, Unit = "")
{
    return (Value == null || Value == 0 || Value == undefined) ? "-" : `${Value.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 }) } ${Unit}`.trim();
}

function ZeroToHypen(Value) {
    return (Value == null || Value == 0 || Value == undefined) ? "-" : `${Value}`.trim();
}

function EmptyTextToHypen(Value)
{
    return (Value == null || Value.trim().length <= 0 || Value == undefined) ? "-" : Value;
}

//#endregion