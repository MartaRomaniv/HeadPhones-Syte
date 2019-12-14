angular.module('myApp', [])
  .controller('AuthCtrl', function ($scope) {
    var au = this;

    if (localStorage.getItem('token'))
      window.location = 'index.html';


    $scope.switchForm = function () {
      au.signIn = !au.signIn;
    }

    $scope.sendRegistryRequest = function () {
      if (au.regPassword !== au.regRepeat) {
        alert('Passwords is not the same');
        return false;
      }

      var data = {
        model: {
          Login: au.regUsername,
          Password: au.regPassword,
          Email: au.regEmail,
          FirstName: au.regFirstname,
          LastName: au.regLastname
        }
      };
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/Register',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        if (response.status === 200) {
          localStorage.setItem('token', response.data);
          window.location = 'index.html';
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
      });
      return false;
    }

    $scope.sendLoginRequest = function () {
      var data = {
        model: {
          Login: au.loginUsername,
          Password: au.loginPassword
        }
      };
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/Login',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        if (response.status === 200) {
          localStorage.setItem('token', response.data);
          window.location = 'index.html';
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
      });
      return false;
    }
  });