function myFunction() {
    if (checkEmpty() === false) {
        return;
    } else {
        var table = document.getElementById("table");
        var row = table.insertRow(table.rows.length);
        for (i = 0; i < 6; i++) {
            row.insertCell(row.cells.length);
        }
        row.cells[0].innerHTML = "Size: ";
        row.cells[2].innerHTML = "Color: ";
        row.cells[4].innerHTML = "Price: ";
        for (i = 1; i < 6; i += 2) {
            var input = document.createElement("input");
            input.type = "text";
            input.setAttribute("class", "input");
            row.cells[i].appendChild(input);
        }
    }
}

function checkEmpty() {
    var check = true;
    var tags = document.getElementsByClassName("input");
    for (i = 0; i < tags.length; i++) {
        if (tags[i].value.length === 0) {
            check = false;
            break;
        }
    }
    return check;
}