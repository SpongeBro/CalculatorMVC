const displayElement = document.getElementById("display");
document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener("keydown", handleKeyDown);
    document.getElementById("mainForm").addEventListener("submit", submitAjax);
});
function displayVal(val) {
    //clear single 0
    if (displayElement.value == "0" && !isKeyValidOperation(val)) {
        clearDisplay();
    }
    displayElement.value += val;
}
function clearDisplay() {
    displayElement.value = "";
}
function removeLastCh() {
    displayElement.value = displayElement.value.slice(0, -1);
}
function isKeyValidOperation(key) {
    return key == "." || key == "+" ||
        key == "-" || key == "*" || key == "/";
}
function handleKeyDown(event) {

    switch (event.key) {
        case "1": case "2": case "3": case "4":
        case "5": case "6": case "7": case "8":
        case "9": case "0": case "+": case "-":
        case "*": case "/": case ".":
            displayVal(event.key);
            break;
        case "Backspace": case "Delete":
            removeLastCh();
            break;
        case "Escape":
            clearDisplay();
            break;
        case "Enter":
            submitAjax(event);
            break;
    }
}
function submitAjax(event) {
    event.preventDefault();
    const errorElement = document.getElementById("validationError");
    var expression = displayElement.value;
    var decimal = document.getElementById("decimalCheck").checked;
    fetch("/Home/Calculate", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Expression: expression, Decimal: !decimal })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            errorElement.innerHTML = "";
            displayElement.value = data.result;
            var historyHTML = "<p>";
            data.history.forEach(function (item) {
                historyHTML += "<div>" + item + "</div>";
            });
            historyHTML += "</p>";
            document.getElementById("history").innerHTML = historyHTML;
        }
        else {
            errorElement.innerHTML = data.errors;
        }
    })
    .catch(error => {
        console.error("Error:", error);
    });
}