// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2,
    });

    const search = document.querySelector('.search'),
        searchBtn = document.getElementById('searchBtn'),
        searchBox = document.querySelector('.search_box'),
        searchReset = document.querySelector('.search_reset'),
        total = document.querySelector('#total'),
        totalNumber = document.getElementById('total_number'),
        cart = document.getElementById('cart'),
        cartItems = document.querySelector('.cart_items'),
        clear = document.querySelector('#clear'),
        plusButtons = document.querySelectorAll('#plus'),
        minusButtons = document.querySelectorAll('#minus'),
        deleteButton = document.querySelectorAll('.cart_item_delete'),
        clearButton = document.getElementById('btn_clear');

    window.addEventListener('mouseup', event => {
        if (
            event.target !== search &&
            event.target.parentNode !== search &&
            searchBox.className === 'search_box search_box_open'
        ) {
            searchReset.style.opacity = 0;
            searchBox.classList.toggle('search_box_open');
        }
    });

    searchBtn.onclick = e => {
        if (searchBox.className === 'search_box') {
            e.preventDefault();
            // searchReset.style.opacity = 1;
            searchBox.classList.toggle('search_box_open');
            searchBox.focus();
        } else if (searchBox.value === '') {
            e.preventDefault();
            searchBox.classList.toggle('search_box_open');
            searchBox.blur();
        }
    };

    searchBox.oninput = () => {
        if (searchBox.value === '') {
            searchReset.style.opacity = 0;
        } else {
            searchReset.style.opacity = 1;
        }
    };

    searchReset.onclick = e => {
        e.preventDefault();
        if (searchBox.value !== '') {
            searchBox.value = '';
            searchBox.focus();
            searchReset.style.opacity = 0;
        }
    };

    searchBox.onfocus = () => {
        if (
            search.className === 'search' ||
            search.className === 'search search_slided'
        ) {
            search.classList.toggle('search_focus');
            if (searchBox.value !== '') {
                searchReset.style.opacity = 1;
            }
        }
    };
    searchBox.onblur = () => {
        if (
            search.classList.contains('search_focus') &&
            (search.classList.contains('search') ||
                search.classList.contains('search_slided'))
        ) {
            search.classList.toggle('search_focus');
            searchReset.style.opacity = 0;
        }
    };

    function removeItem(button) {
        button.onclick = function (e) {
            e.preventDefault();

            let itemToDelete = button.dataset.id;
            let cartItem = button.closest(`#item_${itemToDelete}`);
            let quantity = button.nextElementSibling;
            let xhr = new XMLHttpRequest();

            xhr.open('POST', `/Cart/RemoveFromCart/`, true);
            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            xhr.onload = function () {
                if (this.status == 200 && quantity.innerHTML >= '1') {
                    if (quantity.innerHTML == '1' && cartItems.children.length > 1) {
                        cartItem.style.opacity = 0;
                        setTimeout(() => {
                            cartItem.parentNode.removeChild(cartItem);
                        }, 400);
                    } else if (
                        quantity.innerHTML == '1' &&
                        cartItems.children.length <= 1
                    ) {
                        total.style.opacity = 0;
                        cartItem.style.opacity = 0;
                        clear.style.opacity = 0;
                        cart.style.opacity = 0;
                        setTimeout(() => {
                            cartItem.parentNode.removeChild(cartItem);
                            clear.parentNode.removeChild(clear);
                            total.parentNode.removeChild(total);
                            cart.parentNode.removeChild(cart);
                        }, 500);
                        setTimeout(() => {
                            document.querySelector('#main').innerHTML +=
                                '<h2 id="no_items">No Items</h2 >';
                            document.getElementById('no_items').style.opacity = 1;
                        }, 510);
                    } else {
                        quantity.innerHTML = parseInt(quantity.innerHTML) - 1;
                    }
                    xhr.open('GET', `/Cart/Total`, true);
                    xhr.onload = function () {
                        totalNumber.innerHTML = `${formatter.format(this.responseText)}`;
                    };
                    xhr.send();
                }
            };
            xhr.send(`id=${itemToDelete}`);
        };
    }

    function addItem(button) {
        button.onclick = function (e) {
            e.preventDefault();

            let itemToAdd = button.dataset.id;
            let xhr = new XMLHttpRequest();

            xhr.open('POST', `/Cart/AddToCart/`, true);
            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            xhr.onload = function () {
                if (this.status == 200) {
                    button.previousElementSibling.innerHTML =
                        parseInt(button.previousElementSibling.innerHTML) + 1;
                    xhr.open('GET', `/Cart/Total`, true);
                    xhr.onload = function () {
                        totalNumber.innerHTML = `${formatter.format(this.responseText)}`;
                    };
                    xhr.send();
                }
            };
            xhr.send(`id=${itemToAdd}`);
        };
    }

    function removeItemFromCart(button) {
        button.onclick = function (e) {
            e.preventDefault();

            let itemToDelete = button.dataset.id;
            let cartItem = button.closest(`#item_${itemToDelete}`);
            let xhr = new XMLHttpRequest();

            xhr.open('POST', `/Cart/RemoveFromCartAtAll/`, true);
            xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            xhr.onload = function () {
                if (this.status == 200) {
                    if (cartItems.children.length <= 1) {
                        total.style.opacity = 0;
                        cartItem.style.opacity = 0;
                        clear.style.opacity = 0;
                        cart.style.opacity = 0;
                        setTimeout(() => {
                            cartItem.parentNode.removeChild(cartItem);
                            clear.parentNode.removeChild(clear);
                            total.parentNode.removeChild(total);
                            cart.parentNode.removeChild(cart);
                        }, 500);
                        setTimeout(() => {
                            document.querySelector('#main').innerHTML +=
                                '<h2 id="no_items">No Items</h2 >';
                            document.getElementById('no_items').style.opacity = 1;
                        }, 510);
                    } else {
                        cartItem.style.opacity = 0;
                        setTimeout(() => {
                            cartItem.parentNode.removeChild(cartItem);
                        }, 400);
                    }
                    xhr.open('GET', `/Cart/Total`, true);
                    xhr.onload = function () {
                        totalNumber.innerHTML = `${formatter.format(this.responseText)}`;
                    };
                    xhr.send();
                }
            };
            xhr.send(`id=${itemToDelete}`);
        };
    }

    function removeAll(button) {
        button.onclick = function (e) {
            e.preventDefault();

            let xhr = new XMLHttpRequest();

            xhr.open('POST', '/Cart/RemoveAllItems/', true);
            xhr.onload = function () {
                if (this.status == 200 && cartItems.children.length > 0) {
                    total.style.opacity = 0;
                    clear.style.opacity = 0;
                    cart.style.opacity = 0;
                    setTimeout(() => {
                        clear.parentNode.removeChild(clear);
                        total.parentNode.removeChild(total);
                        cart.parentNode.removeChild(cart);
                    }, 500);
                    setTimeout(() => {
                        document.querySelector('#main').innerHTML +=
                            '<h2 id="no_items">No Items</h2 >';
                        document.getElementById('no_items').style.opacity = 1;
                    }, 510);
                }
                xhr.open('GET', `/Cart/Total`, true);
                xhr.onload = function () {
                    totalNumber.innerHTML = `${formatter.format(this.responseText)}`;
                };
                xhr.send();
            };
            xhr.send();
        };
    }

    plusButtons.forEach(addItem);
    minusButtons.forEach(removeItem);
    deleteButton.forEach(removeItemFromCart);
    removeAll(clearButton);
});
