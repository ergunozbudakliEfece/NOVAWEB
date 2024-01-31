var Scanner = null;
var IsBusy = false;

document.addEventListener("fullscreenchange", FullscreenChanged);

function FullscreenChanged(event) {
    if (!document.fullscreenElement) {
        Scanner?.stop().then(() => {
            Scanner = null;
        });
    }
}

function StopQRScanner() {
    $.fullscreen.exit();
}

function StartQRScanner(Func, CustomButtonFunc = null) {
    ShowProgress();

    Scanner = new Instascan.Scanner({
        video: document.getElementById('scanner-camera'),
        continuous: true,
        mirror: false,
    });

    $('#scanner-custom-button').css('display', CustomButtonFunc == null ? 'none' : 'block');

    Scanner.addListener('scan', function (content) {
        if (!IsBusy)
        {
            IsBusy = true;

            Func(content);
        }
    });

    Instascan.Camera.getCameras()
        .then(function (cameras, image) {
            this.Cameras = cameras;

            let MultipleCamera = cameras.length > 1;

            $('#scanner-camera-select').html('');

            for (let i = 0; i < cameras.length; i++) {
                var CameraName = (i == 0 ? "Ön Kamera" : (i == 1 ? "Arka Kamera" : cameras[i].name));
                $('#scanner-camera-select').append(new Option(CameraName, cameras[i].id));
            }

            if (MultipleCamera) {
                $('#scanner-camera-select').val(cameras[1].id);
            }

            Scanner.start(MultipleCamera ? cameras[1] : cameras[0]).then(() => { HideProgress(); $("#scanner").fullscreen(true); });

            $('#scanner-camera-select').on("change", event => {
                Scanner.start(this.Cameras.find(c => c.id == event.target.value));
            });
        })
        .catch((e) => {
            console.log(e);
            StopQRScanner();
            HideProgress();
        });
}

function OperationA(ScannedText) {
    console.log(`OperationA: ${ScannedText}`);
}
function CustomButton() {
    console.log(`CustomButton`);
}