function validatePassword() {
    var newPass = document.getElementById('password').value;
    var confirmPass = document.getElementById('confirmPassword').value;
    if (newPass == confirmPass) {
        return true;
    } else {
        document.getElementById('txtError').innerHTML = 'Confirm password does not match new password';
        return false;
    }
}