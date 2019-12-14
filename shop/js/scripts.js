angular.module('myApp', [])
  .controller('MyCtrl', function ($scope, $sce) {
    var my = this;
    my.goods = [];
    my.orders = [];
    my.cartIds = [];
    my.basketTotal = 0;
    my.searchTimer;
    my.userSearch = '';
    my.mainPage = window.location.href.indexOf('index.html') > -1;
    my.initPagination = true;
    my.lastSearch = '';
    my.PaginationPages = 1;

    getCartItems();
    if (my.mainPage) {
      getProducts(1, true);
      $('#search').bind('keydown blur change', function (e) {
        var _this = $(this);
        clearTimeout(my.searchTimer);
        my.searchTimer = setTimeout(function () {
          if (my.lastSearch != my.userSearch)
            getProducts(1, true);
          my.lastSearch = my.userSearch;
        }, 500);
      });
    } else if (window.location.href.indexOf('basket.html') > -1)
      getBasket();
    else if (window.location.href.indexOf('history.html') > -1)
      getOrders(1, true);
    else if (window.location.href.indexOf('mailing.html') > -1)
      getMailings();

    checkToken();

    function initPagination(totalPages, startPage, init) {
      my.paginationPages = totalPages;
      $('#pagination-demo').twbsPagination('destroy');
      $('#pagination-demo').twbsPagination({
        totalPages: totalPages,
        startPage: startPage,
        visiblePages: 5,
        initiateStartPageClick: true,
        href: false,
        hrefVariable: '{{number}}',
        first: 'Перша',
        prev: 'Попередня',
        next: 'Наступна',
        last: 'Остання',
        loop: false,
        onPageClick: function (event, page) {
          if (!my.initPagination && my.mainPage)
            getProducts(page);
          else if (!my.initPagination && window.location.href.indexOf('history.html') > -1)
            getOrders(page);
          my.initPagination = false;
        },

        paginationClass: 'pagination',
        nextClass: 'next',
        prevClass: 'prev',
        lastClass: 'last',
        firstClass: 'first',
        pageClass: 'page',
        activeClass: 'active',
        disabledClass: 'disabled'

      });
    }

    function checkToken() {
      var token = localStorage.getItem('token');
      if (!token) {
        my.tokenVerified = true;
        return;
      }
      var data = {
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/CheckToken',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          my.userName = response.data.FirstName + ' ' + response.data.LastName;
          my.isLogedIn = true;
          my.isAdmin = response.role;
          my.tokenVerified = true;
        } else {
          alert(response.data);
          localStorage.removeItem('token');
          window.location = 'index.html';
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    function getCartItems() {
      var cart = JSON.parse(localStorage.getItem('cartIds'));
      my.cartIds = cart && cart.length > 0 ? cart : [];
    }

    $scope.logout = function () {
      var token = localStorage.getItem('token');
      var data = {
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/Logout',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          localStorage.removeItem('token');
          window.location = 'index.html';
        } else {
          alert(response.data);
          localStorage.removeItem('token');
          window.location = 'index.html';
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.changeProductModalState = function (value) {
      //0-hide
      //1-add
      //2-edit
      if (value != 2) {
        my.productId = 0;
        my.productName = '';
        my.productPrice = '';
        my.productUrl = '';
        my.productDiscount = '';
        my.productDetails = '';
      }
      my.showProductModal = value;
    }

    $scope.showEditProductModal = function (id) {
      my.productId = id;
      var product = my.goods.find(x => x.Id === id);
      my.productName = product.Name;
      my.productPrice = product.Price;
      my.productUrl = product.Url;
      my.productDiscount = product.Discount;
      my.productDetails = product.Details;
      $scope.changeProductModalState(2);
    }

    $scope.showProductDetailsModal = function (id) {
      my.productId = id;
      var product = my.goods.find(x => x.Id === id);
      my.productName = product.Name;
      my.productPrice = product.Price;
      my.productUrl = product.Url;
      my.productDiscount = product.Discount;
      my.productDetails = product.Details;
      my.showProductDetailsModal = true;
    }


    $scope.showRemoveProductModal = function (id) {
      my.productId = id;
      my.showRemoveProductModal = true;
    }

    $scope.showRemoveBasketProductModal = function (index) {
      my.basketProductIndex = index;
      my.showRemoveProductModal = true;
    }

    $scope.addToCart = function (id) {
      my.cartIds.push(id);
      localStorage.setItem("cartIds", JSON.stringify(my.cartIds));
    }

    $scope.saveProduct = function () {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        model: {
          Id: my.productId,
          Name: my.productName,
          Price: parseFloat(my.productPrice),
          Url: my.productUrl,
          Discount: parseInt(my.productDiscount),
          Details: my.productDetails
        },
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/SaveProduct',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          window.location = 'index.html';
        } else {
          alert(response.data);
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.createMailing = function () {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        model: {
          Name: my.mailingName,
          Mail: my.mailingHTML,
        },
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/CreateMailing',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          window.location = 'mailing.html';
        } else {
          alert(response.data);
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.removeProductFromBasket = function () {
      my.cartIds.splice(my.basketProductIndex, 1);
      localStorage.setItem("cartIds", JSON.stringify(my.cartIds));
      my.showRemoveProductModal = false;

      getBasket();
    }

    $scope.showMailDetails = function (mail) {
      my.mailingHTML = $sce.trustAsHtml(mail);
      my.showMailingDetailsModal = true;
    }


    $scope.removeProduct = function () {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        id: my.productId,
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/RemoveProduct',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          window.location = 'index.html';
        } else {
          alert(response.data);
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }


    function getProducts(page, init) {
      var data = {
        page: page,
        search: JSON.stringify(my.userSearch)
      }
      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetProducts',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        my.goods = response.data.Products;
        if (init) {
          my.initPagination = true;
          initPagination(response.data.Pages, page, true);
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    function getOrders(page, init) {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        page: page,
        token: JSON.stringify(token)
      }

      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetOrders',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        console.log(data);
        var response = $.parseJSON(data.d);
        console.log(response);
        my.orders = response.data.Orders;
        if (init) {
          my.initPagination = true;
          initPagination(response.data.Pages, page, true);
        }
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    function getMailings() {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        token: JSON.stringify(token)
      }

      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetMailings',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        my.mailings = response.data;
        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }


    function getBasket() {
      var data = {
        ids: JSON.stringify(my.cartIds)
      }
      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetProductsByIds',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        my.goods = response.data;
        my.basketTotal = parseFloat((0).toFixed(2));
        my.goods.forEach(element => {
          my.basketTotal += parseFloat((element.Discount != 0 ? element.Price * (100 - element.Discount) / 100 : element.Price).toFixed(2));
        });

        $scope.$apply();
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.createOrder = function () {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        ids: my.cartIds,
        token: token,
        price: my.basketTotal
      }

      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/CreateOrder',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          localStorage.setItem("cartIds", JSON.stringify([]));
          alert(response.data);
          window.location = 'index.html';
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.changeOrderStatus = function (orderId, status) {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        orderId: orderId,
        status: status,
        token: token
      }
      $.ajax({
        type: 'POST',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/ChangeOrderStatus',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          window.location = 'history.html';
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

    $scope.getProductsByOrder = function (orderId) {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        orderId: orderId,
        token: JSON.stringify(token)
      }
      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetProductsByOrder',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          my.goods = response.data;
          my.showOrderDetailsModal = true;
          $scope.$apply();
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }


    $scope.getUsersByMailingId = function (mailingId) {
      var token = localStorage.getItem('token');
      if (!token) {
        window.location = 'index.html';
        return;
      }

      var data = {
        mailingId: mailingId,
        token: JSON.stringify(token)
      }
      $.ajax({
        type: 'GET',
        url: 'http://localhost:50279/HeadPhonesAPI.asmx/GetUsersByMailing',
        contentType: 'application/json; charset=utf-8',
        data: data
      }).done(function (data) {
        var response = $.parseJSON(data.d);
        console.log(response);
        if (response.status === 200) {
          my.mailingUsers = response.data;
          my.showMailingUsersDetailsModal = true;
          $scope.$apply();
        } else {
          alert(response.data);
        }
        return;
      }).fail(function (response) {
        alert("Internal Server Error");
        window.location = 'authorization.html';
      });
    }

  });