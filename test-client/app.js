let list = document.getElementById("list");
const form = document.getElementById("form");
const newBtn = document.getElementById("btn-new");
const nameText = document.getElementById("name-text");
const deleteBtns = document.getElementsByTagName("li");

let index = 0;
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7210/listHub", { accessTokenFactory: () => "testing" })
    // .withUrl("https://bag-it-api.azurewebsites.net/listHub", { accessTokenFactory: () => this.loginToken })
    .withAutomaticReconnect()
    .build();

connection
    .start()
    .then(() => {
        console.log("connected");
        // invoke a get all list items function on load
        connection.invoke("GetList", "listId");
    })
    .catch((e) => console.log(e));

connection.on("ItemsUpdated", (listArray) => {
    
    appendList(listArray);
});

newBtn.addEventListener("click", (e) => {
    if (nameText.value !== "") {
        connection
            .invoke("AddItemToList", nameText.value, "1")
            .catch((e) => console.log(e));
    }
});
const _token = () => connection.invoke('GetList', "listId").then(r => console.log(r));
const _testToken = (token) => (token ? fetch('https://localhost:7210/token?access_token=' + token) : fetch('https://localhost:7210/token'))
    .then(r => r.text()).then(t => console.log(t));

function deleteSelf(button) {
    console.log(button.value);
    connection.invoke("RemoveItemFromList", button.value);
}

function appendList(listArray) {

    list.innerHTML = "";
    listArray = JSON.parse(listArray);
    
    listArray.forEach((item) => {
        const li = document.createElement("li");
        li.innerHTML = `${item.Name}---<button onclick="deleteSelf(this)" value="${item.Name}">X</button>`;
        list.appendChild(li);
    });
}
