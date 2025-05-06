document.addEventListener("DOMContentLoaded", checkValidity);
const uri = '/IceCream';
let iceCreams = [];
let token = localStorage.getItem('token');

function getItems() {
    fetch(uri, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok: ' + response.statusText);
            }
            return response.json(); // טען את התגובה כ-JSON
        })
        .then(data => {
            console.log(data);
            if (!data) {
                throw new Error('Response is empty');
            }
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
        .then(response => response.json())
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

    // document.getElementById('edit-id').value = item.id;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {

    const itemId = document.getElementById('edit-code').value;
    console.log("bcngf" + itemId);
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

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'IceCream' : 'iceCream kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {

    const button = document.createElement('button');
    const tBody = document.getElementById('iceCreams');
    tBody.innerHTML = '';

    _displayCount(data.length);



    data.forEach(item => {
        console.log(item);
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

        let textNodeName = document.createTextNode(item.name);
        let textNodeId = document.createTextNode(item.id);
        let textNodeDescpription = document.createTextNode(item.description);
        let textNodePrice = document.createTextNode(item.price);
        let td5 = tr.insertCell(4);
        let td6 = tr.insertCell(5);

        td2.appendChild(textNodeName);
        td1.appendChild(textNodeId);
        td3.appendChild(textNodeDescpription);
        td4.appendChild(textNodePrice);
        td5.appendChild(deleteButton);
        td6.appendChild(editButton);

    });

    iceCreams = data;
}

function checkValidity() {

    if (!checkValidity1())
        window.location.href = '../login.html';
    else
        getItems();
}
const checkValidity1 = () => {
    console.log(token);
    if (!token)
        return false;
    // פיצול הטוקן לשלושה חלקים
    const parts = token.split('.');
    if (parts.length !== 3) {
        return false; // טוקן לא תקין
    }
    // פענוח ה-payload
    const payload = JSON.parse(atob(parts[1]));

    // בדיקת תאריך התוקף
    const currentDate = Math.floor(Date.now() / 1000); // תאריך נוכחי בשניות
    if (payload.exp && payload.exp < currentDate) {
        return false; // הטוקן פג תוקף
    }

    // בדיקת ערך מותאם אישית
    if (payload.type !== "Agent" && payload.type !== "Admin") {
        return false; // הטוקן לא תקין לפי המשתנים המותאמים אישית
    }

    // בדיקות נוספות (כגון חתימה) יכולות להתבצע כאן

    return true; // הטוקן תקף
}