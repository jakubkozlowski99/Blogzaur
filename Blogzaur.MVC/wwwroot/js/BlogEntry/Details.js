$(document).ready(function () {

    $("#createComment form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Comment added")
                LoadCarWorkshopServices()
            },
            error: function () {
                toastr["error"]("Something went wrong")
            }
        })
    });
});