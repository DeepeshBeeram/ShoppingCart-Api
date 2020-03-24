
var url = "https://localhost:44376/v1.0/product"
var products = document.getElementById("products-list");

if (products) {

    fetch(url)
        .then(response => response.json())
        .then(data => showProducts(data))
        .catch(ex => {
            alert("something went wrong...");
            console.log(ex);
        });


    var showProducts = function (data) {

        data.forEach(data => {

            let li = document.createElement("li");
            let text = `${data.name} ($${data.price})`;

            li.appendChild(document.createTextNode(text));
            products.appendChild(li);
        });

    }

    
}