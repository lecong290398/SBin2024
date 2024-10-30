$(function () {
    var _scanQR = abp.services.app.scanQr;

    // QR Scanner
    var resultContainer = document.getElementById('qr-reader-results');
    var lastResult = null;
    var debounceTimeout = null;

    function onScanSuccess(decodedText, decodedResult) {
        if (decodedText !== lastResult) {
            lastResult = decodedText;

            // Debounce to avoid multiple scans of the same QR code
            if (debounceTimeout) {
                clearTimeout(debounceTimeout);
            }

            debounceTimeout = setTimeout(() => {
                // Handle on success condition with the decoded message.
                console.log(`Scan result: ${decodedText}`, decodedResult);
                const preload = document.getElementById("preload");
                preload.style.display = "block";

                // Call the API
                _scanQR.handleScanQR({ TransactionCode: decodedText })
                    .done(function (points) {
                        preload.style.display = "none";
                        abp.message.success("You are awarded " + points + " points for this transaction", "Points accumulated successfully").done(function () {
                            window.location.reload();
                        });;
                        abp.notify.success('Points accumulated successfully.');
                    })
                    .fail(function (error) {
                        preload.style.display = "none";
                        abp.message.error(error.message);
                        abp.notify.error("QR code scanning failed");
                    });
            }, 300); // Adjust the debounce delay as needed
        }
    }

    var html5QrcodeScanner = new Html5QrcodeScanner(
        "qr-reader", { fps: 10, qrbox: 250 }
    );
    html5QrcodeScanner.render(onScanSuccess);
});