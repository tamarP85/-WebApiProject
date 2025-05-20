document.addEventListener("DOMContentLoaded", checkValidity);
const uri = '/IceCream';
let iceCreams = [];
let tokenCookie = document.cookie.split('; ').find(row => row.startsWith('token='));
let token = tokenCookie ? tokenCookie.split('=')[1] : null;

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
                throw new Error("שגיאה בקבלת המוצרים ")
            }
            return response.json();
        })
        .then(data => {
            if (!data) {
                throw new Error('Response is empty');
            }
            _displayItems(data);
        })
        .catch((error) => {
            console.error('Unable to get items.', error), alert(error)
        });

}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');
    const addPriceTextbox = document.getElementById('add-price');
    const item = {
        Id: -1,
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
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה בקבלת המוצרים ")
            }
        })
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addDescriptionTextbox.value = '';
            addPriceTextbox.value = '';
        })
        .catch((error) => {
            console.error('Unable to add item.', error),
                alert(error)
        });
}

function deleteItem(itemId) {
    fetch(`${uri}/${itemId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
            },
            credentials: 'include'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה במחיקת המוצרים")
            }
        })
        .then(() => getItems())
        .catch((error) => {
            console.error('Unable to delete item.', error),
                alert(error)
        });
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
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה בעדכון המוצרים")
            }
        })
        .then(() => getItems())
        .catch((error) => {
            console.error('Unable to update item.', error), alert(error)
        });

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function closeUpdate(){
    document.getElementById('userModal').style.display = 'none';
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
const checkValidity1 = () => {
    if (!token)
        return false;
    const parts = token.split('.');
    if (parts.length !== 3) {
        return false;
    }
    const payload = JSON.parse(atob(parts[1]));
    const currentDate = Math.floor(Date.now() / 1000);
    if (payload.exp && payload.exp < currentDate) {
        return false;
    }
    if (payload.type !== "Agent" && payload.type !== "Admin") {
        return false;
    }
    return true;
}
document.getElementById('updateUserButton').addEventListener('click', function() {
    document.getElementById('userModal').style.display = 'block';
    const parts = token.split('.');
    let userId = '-1';
    let payload;
    if (parts.length === 3) {
        payload = JSON.parse(atob(parts[1]));
        userId = payload.Id;
    }
    fetch(`/User/${userId}`, {
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
            throw new Error("שגיאה בקבלת המוצרים ")
        }
        return response.json();
    })
    .then(data => {
        if (!data) {
            throw new Error('Response is empty');
        }
         document.getElementById('name').value=data.name,
         document.getElementById('password').value=data.password,
         document.getElementById('email').value=data.email
    })
    .catch((error) => {
        console.error('Unable to get items.', error), alert(error)
    });
});
document.getElementById('submitButton').addEventListener('click', function() {
    const parts = token.split('.');
    let userId = '-1';
    let payload;
    if (parts.length === 3) {
        payload = JSON.parse(atob(parts[1]));
        userId = payload.Id;
    }
    const user = {
        id: Number(userId),
        name: document.getElementById('name').value.trim(),
        password: document.getElementById('password').value.trim(),
        email: document.getElementById('email').value.trim(),
        type: payload.type,
    };

    userId = Number(userId);
    document.getElementById('userModal').style.display = 'none';

    fetch(`/User/${userId}`, {
            method: 'PUT',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .catch(error => console.error('Unable to update user.', error));

});

function checkAdmin() {
    const parts = token.split('.');
    if (parts.length === 3) {
        const payload = JSON.parse(atob(parts[1]));
        if (payload.type === "Admin") {
            addAdminButton();
        }
    }
}

function addAdminButton() {
    const adminToolsDiv = document.getElementById('admin-tools');
    const adminButton = document.createElement('button');
    adminButton.innerText = 'Go to Users';
    adminButton.onclick = function() {
        window.location.href = './users.html';
    };
    adminToolsDiv.appendChild(adminButton);
}

function checkValidity() {
    if (!checkValidity1())
        window.location.href = '../login.html';
    else {
        getItems();
        checkAdmin();
    }
}

function clearCookieAndRedirect() {
    document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

    window.location.href = 'login.html';
}