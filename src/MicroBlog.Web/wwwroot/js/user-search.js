UserSearchJs = (function () {
    let _pageNumber = 0;

    function init(container, searchUrl, pageSize, xsrfToken) {
        $(container).empty();
        $('#loading').addClass('d-none');

        $('#search').on('keyup', debounce(function () {
            _pageNumber = 0;
            $(container).empty();
            fetch(container, searchUrl, pageSize, xsrfToken)
        }, 300));

        const win = $(window);
        win.on('scroll', function () {
            if ($(document).height() - win.height() == win.scrollTop()) {
                fetch(container, searchUrl, pageSize, xsrfToken);
            }
        });
    }

    function fetch(container, searchUrl, pageSize, xsrfToken) {
        const search = $('#search').val();

        if (search !== '') {
            $('#loading').removeClass('d-none');

            $.ajax({
                url: searchUrl,
                headers: { RequestVerificationToken: xsrfToken },
                data: { search, skip: _pageNumber * pageSize, take: pageSize },
                dataType: 'html',
                success: function (html) {
                    if (!isEmpty(html)) {
                        $(container).append(html);
                        _pageNumber++;

                        BlockJs.init();
                        SubscribeJs.init();
                    }
                    $('#loading').addClass('d-none');
                }
            });
        }
    }

    function debounce(func, wait) {
        let timeout;
        return function (...args) {
            const context = this;
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(context, args), wait);
        };
    }

    return {
        init
    };
})();