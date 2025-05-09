document.getElementById('loginButton').addEventListener('click', function() {
    const nameInput = document.getElementById('name').value;
    const passwordInput = document.getElementById('password').value;
    var user = {
        Name: nameInput,
        Password: passwordInput,
        Email: "",
        Type: "",
        Id: -1,
    };
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
            return response.json();
        })
        .then(data => {
            document.cookie = "token=" + encodeURIComponent(data) + "; path=/; max-age=" + 7 * 24 * 60 * 60;
            window.location.href = '../index.html';
        })
        .catch(error => {
            console.error('Error:', error);
        });
}