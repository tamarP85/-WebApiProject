const uri = '/IceCream';
let iceCreams = [];
let token = localStorage.getItem('token');

function checkValidity() {
    const userType = checkValidity1();
    if (!userType) {
        window.location.href = '../login.html';
    } else {
        if (userType === "Admin") {
            addAdminButton();
        }
        getItems();
    }
}

const checkValidity1 = () => {
    if (!token) return false;

    const parts = token.split('.');
    if (parts.length !== 3) return false;

    const payload = JSON.parse(atob(parts[1]));
    const currentDate = Math.floor(Date.now() / 1000);

    if (payload.exp && payload.exp < currentDate) return false;

    if (payload.type !== "Agent" && payload.type !== "Admin") return false;

    return payload.type;
};

function addAdminButton() {
    const adminToolsDiv = document.getElementById('admin-tools');
    const adminButton = document.createElement('button');
    adminButton.innerText = "View All Customers";
    adminButton.setAttribute('onclick', "window.location.href='/users.html';");
    adminButton.style.margin = "10px";
    adminButton.style.padding = "10px";
    adminButton.style.backgroundColor = "#4CAF50";
    adminButton.style.color = "white";
    adminButton.style.border = "none";
    adminButton.style.cursor = "pointer";

    adminToolsDiv.appendChild(adminButton);
}

function getItems() {
    fetch(uri, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
        .then(response => response.json())
        .then(data => {
            _displayItems(data);
        })
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addCodeTextbox = document.getElementById('add-code');
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');
    const addPriceTextbox = document.getElementById('add-price');
    const item = {
        Id: addCodeTextbox.value.trim(),
        Name: addNameTextbox.value.trim(),
        Price: addPriceTextbox.value.trim(),
        Description: addDescriptionTextbox.value.trim(),
        AgentId: -1,
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(() => {
            getItems();
            addCodeTextbox.value = '';
            addNameTextbox.value = '';
            addDescriptionTextbox.value = '';
            addPriceTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token,
            },
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = iceCreams.find(item => item.id === id);

    document.getElementById('edit-code').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-description').value = item.description;
    document.getElementById('edit-price').value = item.price;

    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-code').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        description: document.getElementById('edit-description').value.trim(),
        price: document.getElementById('edit-price').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayItems(data) {
    const tBody = document.getElementById('iceCreams');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let td2 = tr.insertCell(1);
        let td3 = tr.insertCell(2);
        let td4 = tr.insertCell(3);

        let textNodeId = document.createTextNode(item.id);
        let textNodeName = document.createTextNode(item.name);
        let textNodeDescription = document.createTextNode(item.description);
        let textNodePrice = document.createTextNode(item.price);

        td1.appendChild(textNodeId);
        td2.appendChild(textNodeName);
        td3.appendChild(textNodeDescription);
        td4.appendChild(textNodePrice);

        let td5 = tr.insertCell(4);
        let td6 = tr.insertCell(5);

        td5.appendChild(deleteButton);
        td6.appendChild(editButton);
    });

    iceCreams = data;
}