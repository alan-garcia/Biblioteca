function changeLanguageToggle() {
    const dropdown = document.querySelector("#languageDropdown");
    dropdown.classList.toggle("hidden");
}

function menuResponsiveToggle(e) {
    const menu = document.querySelector("#navbar-sticky");
    menu.classList.toggle("hidden");
}

function Eliminar(url) {
    Swal.fire({
        title: "¿Seguro que quieres eliminar este registro?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí",
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url, {
                headers: {
                    "Content-Type": "application/json",
                    'Accept': 'application/json',
                },
                method: "POST"
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    window.location.reload();
                }
            })
            .catch(error => {
                Swal.fire({
                    title: "Error",
                    text: error.message,
                    icon: "error"
                });
            });
        }
    });
}

function EliminarSeleccionados(url) {
    var idsSeleccionados = [];
    var checkboxes = document.querySelectorAll('tbody input[type="checkbox"]:checked');
    checkboxes.forEach((checkbox) => {
        idsSeleccionados.push(checkbox.value);
    });

    if (idsSeleccionados.length === 0) {
        Swal.fire({
            title: "¡Cuidado!",
            text: "Por favor, selecciona al menos un registro para eliminar.",
            icon: "warning"
        });
    }
    else {
        Swal.fire({
            title: "¿Seguro que quieres eliminar los registros seleccionados?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí",
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                if (idsSeleccionados.length > 0) {
                    fetch(url, {
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json"
                        },
                        method: "POST",
                        body: JSON.stringify(idsSeleccionados)
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            window.location.reload();
                        }
                    })
                    .catch(error => {
                        Swal.fire({
                            title: "Error",
                            text: error.message,
                            icon: "error"
                        });
                    });
                }

                checkboxes.forEach((checkbox) => checkbox.checked = false);
            }
        });
    }
}

function Search(url) {
    let term = document.querySelector("input[name=filterSearch]").value;
    term = term ? term : "all";

    fetch(`${url}/${term}`)
        .then(response => response.text()) // Obtenemos el HTML parcial como texto
        .then(html => {
            // Reemplaza el contenido del cuerpo de la tabla con los resultados filtrados
            document.getElementById('dataBody').innerHTML = html;
        })
        .catch(error => {
            console.error('Error en la petición:', error);
        });
}
