ImagePreviewJs = (function () {
    let _previewId;

    function init(inputId, previewId) {
        _previewId = previewId;

        $(inputId).change(function (e) {
            const file = e.target.files[0];

            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();

                reader.onload = function () {
                    setImage(reader.result);
                }

                reader.readAsDataURL(file);
            }
        });
    }

    function setImage(url) {
        const preview = $(_previewId);
        const image = $('<img>');
        image.attr('src', url);
        preview.empty();
        preview.append(image);
    }

    return {
        init, setImage
    };
})();