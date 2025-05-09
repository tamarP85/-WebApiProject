const uri = '/User';
let users = [];
let tokenCookie = document.cookie.split('; ').find(row => row.startsWith('token='));
let token = tokenCookie ? tokenCookie.split('=')[1] : null;

function getUsers() {
    fetch(uri, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Accept': 'application/json'
            }
        })
        .then(response => {
            if (!response.ok) {
                alert("שגיאה בקבלת הלקוחות")
            }
            return response.json();

        })
        .then(data => _displayUsers(data))
        .catch((error) => {
            alert(error)
        });
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
        Type: userTypeTextbox.value,
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
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה בהוספת המשתמש ");
            }
            return response.json();
        })
        .then(() => {
            getUsers();
            userIdTextbox.value = '';
            userNameTextbox.value = '';
            userPasswordTextbox.value = '';
            userEmailTextbox.value = '';
            userTypeTextbox.value = '';
        })
        .catch((error) => {
            alert(error);
        });
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token,
            },
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה במחיקת המשתמש");
            }
        })
        .then(() => getUsers())
        .catch((error) => {
            alert(error)
        });
}

function displayEditForm(id) {
    id = Number(id);
    const user = users.find(user => user.id === id);
    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-password').value = user.password;
    document.getElementById('edit-email').value = user.email;
    document.getElementById('edit-type').selected = user.type;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id').value;

    const user = {
        Id: userId,
        Name: document.getElementById('edit-name').value.trim(),
        Password: document.getElementById('edit-password').value.trim(),
        Email: document.getElementById('edit-email').value.trim(),
        Type: document.getElementById('edit-type').value,
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
        .then(response => {
            if (!response.ok) {
                throw new Error("שגיאה בעדכון המשתמש");
            }
        })
        .then(() => getUsers())
        .catch((error) => {
            alert(error)
        });

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
        editButton.setAttribute('onclick', `displayEditForm('${user.id}')`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.className = 'delete';
        deleteButton.setAttribute('onclick', `deleteUser('${user.id}')`);

        const tr = tBody.insertRow();

        const td1 = tr.insertCell(0);
        const td2 = tr.insertCell(1);
        const td3 = tr.insertCell(2);
        const td4 = tr.insertCell(3);
        const td5 = tr.insertCell(4);
        const td6 = tr.insertCell(5);
        const td7 = tr.insertCell(6);

        td1.textContent = user.id;
        td2.textContent = user.name;
        td3.textContent = user.password;
        td4.textContent = user.email;
        td5.textContent = user.type;

        td6.appendChild(editButton);
        td7.appendChild(deleteButton);
    });

    users = data;
}

function goToIceCream() {
    window.location.href = './index.html';
}