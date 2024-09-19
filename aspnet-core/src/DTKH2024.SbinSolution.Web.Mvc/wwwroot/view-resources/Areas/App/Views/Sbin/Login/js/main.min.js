import Validator from './validation.js'

const defaultPassword = {
    email: "admin@gmail.com",
    password: "admin123"
}

var setPassword = {};

function compareObjects(obj1, obj2) {
    return obj1.email === obj2.email && obj1.password === obj2.password;
}
Validator({
    form: "#form-1",
    errorSelector: ".form-message",
    formGroupSelector: ".form-group",
    rules: [
        Validator.isRequired(".email"),
        Validator.isEmail(".email"),

        Validator.isRequired(".password"),
        Validator.isMinLength(".password", 6),
    ],
    onSubmit: (data) => {
        console.log(data);
        if(compareObjects(data, defaultPassword) || compareObjects(data, setPassword)) {
            window.location.href = '/';
        } else {
            alert("Email or password is incorrect");
        }
    }
})

Validator({
    form: "#form-2",
    errorSelector: ".form-message",
    formGroupSelector: ".form-group",
    rules: [
        Validator.isRequired(".fullname"),

        Validator.isRequired(".email"),
        Validator.isEmail(".email"),

        Validator.isRequired(".password"),
        Validator.isMinLength(".password", 6),
    ],
    onSubmit: (data) => {
        console.log(data)
        setPassword = data;
    }
})

// DOM

const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});

// Handle preload
const preLoadingPage = document.getElementById('preload')
window.addEventListener('load', () => {
    preLoadingPage.style.opacity = 0;
    preLoadingPage.style.visibility = 'hidden';
});
