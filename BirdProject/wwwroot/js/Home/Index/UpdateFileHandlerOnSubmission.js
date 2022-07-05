
//window.addEventListener('load', (event) => {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            Swal.fire({
//                position: 'top-end',
//                icon: 'success',
//                title: 'Your work has been saved',
//                showConfirmButton: false,
//                timer: 1500
//            })
//        }
//    })
//});



var imageInput = document.getElementById("fileimageinput");


var imageHolder = document.createElement('img');

imageHolder.setAttribute("class", "card-img-top");




function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}

var photoButton = document.getElementById("exampleUploadButton");
var imageOnTop = document.getElementById("imageOnTop");

photoButton.addEventListener('click', () => {
    imageInput.click();
})


function changeImage(input) {
    var reader;

    if (input.files && input.files[0]) {
        reader = new FileReader();

        reader.onload = function (e) {
            preview.setAttribute('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

imageInput.addEventListener('change', () => {
    if (imageInput.files.length > 0) {
        console.log("File selected");
        removeAllChildNodes(photoButton);
        photoButton.appendChild(imageHolder);
        const [file] = imageInput.files;
        imageHolder.src = URL.createObjectURL(file);
    }
})


var datePicker = document.getElementById("date");

datePicker.max = new Date().toISOString().split("T")[0];

