﻿$(function () {
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

                // Call the API
                _scanQR.handleScanQR({ TransactionCode: decodedText })
                    .done(function () {
                        abp.notify.success('QR code processed successfully.');
                        window.location.href = decodedText;
                        resultContainer.innerHTML = `<a style="color: var(--dark-blue);" href="${decodedText}">Your QR Code is: ${decodedText}</a>`;
                    })
                    .fail(function (error) {
                        abp.notify.error('Failed to process QR code: ' + error.message);
                    });
            }, 300); // Adjust the debounce delay as needed
        }
    }

    var html5QrcodeScanner = new Html5QrcodeScanner(
        "qr-reader", { fps: 10, qrbox: 250 }
    );
    html5QrcodeScanner.render(onScanSuccess);
});