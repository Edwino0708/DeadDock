import swal from 'sweetalert2'


export const ConfirmDeletAlert = (): Promise<boolean> => {
    return swal
        .fire({
            title: "¿Esta seguro que desea eliminar ese dato?",
            text: "¡No podrás revertir esto!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Si, deseo eliminarlo!",
        })
        .then((result) => {
            if (result.value) {
                return true;
            }
        });
};

export const SuccessAlert = (messange:string) : Promise<any> =>{
    return swal.fire({
      icon: 'success',
      title: 'Datos Guardados',
      text: `${messange}`
    })

}

export const ErrorAlert = (message) : Promise<any> =>{
    return swal.fire({
      icon: 'error',
      title: '¡Ohh error!',
      text: `${message}`
  //    text: '¡Ha intentar de guardar datos!',
    })

}