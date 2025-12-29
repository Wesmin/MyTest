var ImageDownloaderPlugin = {
    ImageDownloader: function (str, fn) {
        console.log("start jslib download");
        var msg = UTF8ToString(str);
        var fname = UTF8ToString(fn);
        var contentType = 'image/jpeg';

        function fixBinary(bin) {
            var length = bin.length;
            var buf = new ArrayBuffer(length);
            var arr = new Uint8Array(buf);
            for (var i = 0; i < length; i++) {
                arr[i] = bin.charCodeAt(i);
            }
            return buf;
        }

        var binary = fixBinary(atob(msg));
        var data = new Blob([binary], { type: contentType });
        var link = document.createElement('a');
        link.download = fname;
        link.href = window.URL.createObjectURL(data);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(link.href);
    },

    UploadImage: function () {
        var input = document.createElement('input');
        input.type = 'file';
        input.accept = '.png,.jpg,.jpeg';
        input.onchange = function (e) {
            var file = e.target.files[0];
            if (!file) return;

            var reader = new FileReader();
            reader.onload = function (evt) {
                var base64 = evt.target.result.split(',')[1]; // remove header
                unityInstance.SendMessage('ScreenshotManager', 'OnImageLoadedFromJS', base64);
            };
            reader.readAsDataURL(file);
        };
        input.click();
    }
};

mergeInto(LibraryManager.library, ImageDownloaderPlugin);