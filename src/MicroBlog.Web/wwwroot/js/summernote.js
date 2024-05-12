SummernoteJs = (function () {
    function init(selector, imageUploadUrl) {
        $(selector).summernote({
            callbacks: {
                onImageUpload: function (files) {
                    uploadImages(files, selector, imageUploadUrl);
                }
            }
        });
    }

    function uploadImages(files, selector, url) {
        for (const file of files) {
            uploadImage(file, selector, url);
        }
    }

    function uploadImage(file, selector, url) {
        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: url,
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (imageUrl) {
                $(selector).summernote('insertImage', imageUrl);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        })
    }

    return {
        init
    };
})();