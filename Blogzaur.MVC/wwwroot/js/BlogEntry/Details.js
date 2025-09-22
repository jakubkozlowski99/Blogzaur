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
});