const uri = '/Users';
let users = [];
const token = localStorage.getItem('token');

function getUsers() {
    fetch(uri, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json'
            }
        })
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function addUser() {
    const userIdTextbox = document.getElementById('add-id');
    const userNameTextbox = document.getElementById('add-name');
    const userPasswordTextbox = document.getElementById('add-password');
    const userEmailTextbox = document.getElementById('add-email');
    const userTypeTextbox = document.getElementById('add-type');

    const user = {
        Id: userIdTextbox.value.trim(),
        Name: userNameTextbox.value.trim(),
        Password: userPasswordTextbox.value.trim(),
        Email: userEmailTextbox.value.trim(),
        Type: userTypeTextbox.value.trim()
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .then(() => {
            getUsers();
            userIdTextbox.value = '';
            userNameTextbox.value = '';
            userPasswordTextbox.value = '';
            userEmailTextbox.value = '';
            userTypeTextbox.value = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token,
            },
        })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(user => user.Id === id);

    document.getElementById('edit-id').value = user.Id;
    document.getElementById('edit-name').value = user.Name;
    document.getElementById('edit-password').value = user.Password;
    document.getElementById('edit-email').value = user.Email;
    document.getElementById('edit-type').value = user.Type;

    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id').value;

    const user = {
        Id: userId,
        Name: document.getElementById('edit-name').value.trim(),
        Password: document.getElementById('edit-password').value.trim(),
        Email: document.getElementById('edit-email').value.trim(),
        Type: document.getElementById('edit-type').value.trim()
    };

    fetch(`${uri}/${userId}`, {
            method: 'PUT',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .then(() => getUsers())
        .catch(error => console.error('Unable to update user.', error));

    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(user => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.className = 'edit';
        editButton.setAttribute('onclick', `displayEditForm('${user.Id}')`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.className = 'delete';
        deleteButton.setAttribute('onclick', `deleteUser('${user.Id}')`);

        const tr = tBody.insertRow();

        const td1 = tr.insertCell(0);
        const td2 = tr.insertCell(1);
        const td3 = tr.insertCell(2);
        const td4 = tr.insertCell(3);
        const td5 = tr.insertCell(4);
        const td6 = tr.insertCell(5);
        const td7 = tr.insertCell(6);

        td1.textContent = user.Id;
        td2.textContent = user.Name;
        td3.textContent = '****'; // Hide password
        td4.textContent = user.Email;
        td5.textContent = user.Type;

        td6.appendChild(editButton);
        td7.appendChild(deleteButton);
    });

    users = data;
}