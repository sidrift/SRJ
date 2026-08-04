let codeReader;
let scriptLoadingPromise;

function loadZXing() {
    if (window.ZXing) return Promise.resolve();

    if (!scriptLoadingPromise) {
        scriptLoadingPromise = new Promise((resolve, reject) => {
            const script = document.createElement("script");
            script.src = "lib/zxing/umd/index.min.js"; // from libman above
            script.onload = () => resolve();
            script.onerror = () => reject(new Error("Failed to load barcode scanner library."));
            document.head.appendChild(script);
        });
    }
    return scriptLoadingPromise;
}
window.startBarcodeScanner = async function (dotnetRef) {
    try {
        await loadZXing();

        if (!codeReader) {
            const hints = new Map();

            // Restrict to Code128 only — stops ZXing from
            // occasionally misreading it as another symbology
            hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, [
                ZXing.BarcodeFormat.DATA_MATRIX
            ]);

            // Try harder = slower per-frame but more accurate,
            // worth it since you're scanning a screen/small label
            hints.set(ZXing.DecodeHintType.TRY_HARDER, true);

            codeReader = new ZXing.BrowserMultiFormatReader(hints);
        }

        const video = document.getElementById("barcode-video");
        if (!video) throw new Error("Video element not ready.");

        const devices = await codeReader.listVideoInputDevices();
        if (devices.length === 0) throw new Error("No camera found.");

        const camera = devices.find(x => x.label.toLowerCase().includes("back")) ?? devices[0];

        await codeReader.decodeFromVideoDevice(camera.deviceId, video, (result, err) => {
            if (result) {
                console.log("DECODED:", result.text, result.format);
                codeReader.reset();
                dotnetRef.invokeMethodAsync("BarcodeDetected", result.text);
            }

            // Suppress the expected "nothing found this frame" noise
            if (err && !/not.*able to detect|no multiformat/i.test(err.message ?? "")) {
                console.error("Scan error:", err);
            }
        });
    } catch (e) {
        console.error("Barcode scanner error:", e);
        await dotnetRef.invokeMethodAsync("OnScannerError", e.message ?? String(e));
    }
};

window.stopBarcodeScanner = function () {
    codeReader?.reset();
};