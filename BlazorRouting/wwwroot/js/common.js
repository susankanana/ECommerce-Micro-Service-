//const Swal = require('sweetalert2')
//window.ShowMessage = (type, message) => {
//    if (type === 'success') {
//        toastr.success(message, "Operation was Success!")
//    }

//    if (type === 'error') {
//        toastr.error(message, "Operation was Failed!")

//    }
//}

//window.ShowMessage = (type, message) => {
//    if (type === 'success') {
//        Swal.fire({
//            title: 'Error!',
//            text: message,
//            icon: 'error',
//            confirmButtonText: 'Cool'

//        });
//    }
//}
window.ShowMessage = (type, message) => {
    if (type === 'success') {
        Swal.fire({
            title: 'Error!',
            text: message,
            icon: 'error',
            confirmButtonText: 'Cool'

        });
    }
}
window.ShowMessage = (type) => {
    if (type === 'success') {
        Swal.fire({
            title: "Custom width, padding, color, background.",
            width: 600,
            padding: "3em",
            color: "#716add",
            background: "#fff url(/images/trees.png)",
            backdrop: `
    rgba(0,0,123,0.4)
    url("/images/nyan-cat.gif")
    left top
    no-repeat
  `
        });
    }
}