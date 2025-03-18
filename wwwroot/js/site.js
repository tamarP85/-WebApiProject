const uri = '/IceCream';
let iceCreams = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addCodeTextbox = document.getElementById('add-code');
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');
    const addPriceTextbox = document.getElementById('add-price');
    const item = {
        code:addCodeTextbox.value.trim(),
        name: addNameTextbox.value.trim(),
        description: addDescriptionTextbox.value.trim(),
        price: addPriceTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: {
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
        method: 'DELETE'
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
  console.log("bcngf"+itemId);  
  const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        description:document.getElementById('edit-description').value.trim(),
        price:document.getElementById('edit-price').value.trim()
    };
   

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
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
    const tBody = document.getElementById('iceCreams');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

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