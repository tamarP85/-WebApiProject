document.getElementById('loginButton').addEventListener('click', function() {
    // קבלת ערכי הקלט
    const nameInput = document.getElementById('name').value;
    const passwordInput = document.getElementById('password').value;
    var user = {
        Name: nameInput,
        Password: passwordInput,
        Email: "",
        Type: "",
        Id: -1,
    };
    // קריאה לפונקציה POST
    login(user);


});

function login(user) {
    fetch('/Login', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json(); // מחזירים את התגובה בפורמט JSON
        })
        .then(data => {
            // שמור את הטוקן ב-localStorage או ב-sessionStorage
            console.log(data);
            localStorage.setItem('token', data);
            window.location.href = '../index.html';
        })
        .catch(error => {
            console.error('Error:', error);
        });
}