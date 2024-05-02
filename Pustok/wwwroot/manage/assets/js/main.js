
$(document).ready(function () {
    $("#imgInput").change(function (e) {
        var img = document.getElementById('previewImg');
        img.src = URL.createObjectURL(e.target.files[0]);
        img.onload = function () {
            URL.revokeObjectURL(img.src) // free memory
        }
    })

    $(".delete-btn").click(function (e) {
        e.preventDefault();

        let url = $(this).attr("href");


        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {

                fetch(url)
                    .then(response => {
                        if (response.ok) {
                            Swal.fire({
                                title: "Deleted!",
                                text: "Your file has been deleted.",
                                icon: "success"
                            }).then(() => {
                                window.location.reload();
                            })
                        }
                        else {
                            Swal.fire({
                                title: "Sorry!",
                                text: "Something went wrong",
                                icon: "error"
                            })
                        }
                    })
            }
        });
    })
})


