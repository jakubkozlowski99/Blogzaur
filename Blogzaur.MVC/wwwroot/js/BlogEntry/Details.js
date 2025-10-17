$(document).ready(function () {

    LoadComments()

    $("#createComment form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Comment added")
                var frm = document.getElementsByName('comment-form')[0];
                frm.reset();
                LoadComments()
            },
            error: function () {
                toastr["error"]("Something went wrong")
            }
        })
    });

    // Helper: read antiforgery token from form or page
    function getAntiForgeryToken($form) {
        var token = $form.find('input[name="__RequestVerificationToken"]').val();
        if (token) return token;
        token = $('input[name="__RequestVerificationToken"]').first().val();
        if (token) return token;
        token = $('meta[name="csrf-token"]').attr('content');
        return token || null;
    }

    // Refresh only the likeBlogEntry fragment by fetching the current page and extracting the fragment
    function refreshLikeFragment() {
        return $.get(window.location.href).then(function (html) {
            // parse returned HTML and extract the fragment
            var $parsed = $('<div>').append($.parseHTML(html));
            var $newFragment = $parsed.find('#likeBlogEntry');
            if ($newFragment.length) {
                $('#likeBlogEntry').replaceWith($newFragment);
            } else {
                console.warn('Could not find #likeBlogEntry in returned HTML.');
            }
        }).fail(function () {
            console.warn('Failed to refresh like fragment.');
        });
    }

    // Optionally update DOM if server returns lightweight JSON (keeps UX snappy)
    function applyJsonLikeResponse($form, result) {
        if (result.likeCount !== undefined) {
            $('#blog-like-count').text(result.likeCount);
        }
        if (result.liked !== undefined) {
            var $icon = $form.find('i.fa-heart');
            if (!$icon.length) $icon = $('#likeBlogEntry').find('i.fa-heart');
            if ($icon.length) {
                if (result.liked) {
                    $icon.removeClass('fa-regular').addClass('fa-solid');
                } else {
                    $icon.removeClass('fa-solid').addClass('fa-regular');
                }
            }
        }
    }

    // Intercept like/unlike form submissions and do AJAX; then refresh fragment
    $(document).on('submit', '#like-form, #unlike-form, form.like-form', function (e) {
        e.preventDefault();
        var $form = $(this);
        var url = $form.attr('action');
        var method = ($form.attr('method') || 'POST').toUpperCase();
        var data = $form.serialize();
        var token = getAntiForgeryToken($form);

        var headers = { 'X-Requested-With': 'XMLHttpRequest' };
        if (token) headers['RequestVerificationToken'] = token;

        // Disable submit button to prevent double submits
        var $btn = $form.find('button[type="submit"]');
        $btn.prop('disabled', true);

        $.ajax({
            url: url,
            type: method,
            data: data,
            headers: headers,
            success: function (result) {
                // If server returns JSON with success flag, try to use it to update UI
                if (result && typeof result === 'object' && result.success !== undefined) {
                    if (result.success) {
                        // Prefer incremental DOM update if provided
                        applyJsonLikeResponse($form, result);

                        // Still refresh fragment to ensure server-rendered state (keeps markup logic on server)
                        refreshLikeFragment();

                        // Show feedback with requested phrasing
                        toastr["success"](result.message || (result.liked ? "Blog liked" : "Blog unliked"));
                    } else {
                        toastr["error"](result.message || "Action failed");
                    }
                } else {
                    // If server returned HTML (redirect or full page), refresh the fragment
                    refreshLikeFragment().then(function () {
                        // Inspect replaced fragment to determine new state and show the exact message
                        var liked = $('#likeBlogEntry').find('i.fa-solid.fa-heart').length > 0 || $('#likeBlogEntry').find('#unlike-form').length > 0;
                        toastr["success"](liked ? "Blog liked" : "Blog unliked");
                    });
                }
            },
            error: function (xhr) {
                // If server redirects to login (302 -> HTML), the above path may return HTML; treat as failure otherwise
                toastr["error"]("Server error");
            },
            complete: function () {
                $btn.prop('disabled', false);
            }
        });
    });
});