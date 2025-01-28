async function add(productId) {
    var userNameEl = document.getElementById('username')
    if (userNameEl == null) {
        window.location.href = "/Identity/Account/Login"
    }
    try {
        var response = await fetch(`/Cart/AddProduct?productId=${productId}`)
        if (response.status === 200) {
            var result = await response.json()
            var cartCountEl = document.getElementById('cartCount')
            cartCountEl.innerHTML = result
            var quantityInCartRes = await fetch(`/Cart/GetProductQuantityInCart?productId=${productId}`)
            var quantityInCart = await quantityInCartRes.json()
            SetButton(productId)
        }
    }
    catch (err) {
        console.log(err)
    }
}

async function SetButton(productId) {
    var userNameEl = document.getElementById('username')
    if (userNameEl == null) {
        return
    }
    else {
        var button = document.getElementById(`add-to-card-${productId}`)
        var quantityInCartRes = await fetch(`/Cart/GetProductQuantityInCart?productId=${productId}`)
        var quantityInCart = await quantityInCartRes.json()
        if (quantityInCart > 0) {
            button.innerHTML = `Added ${quantityInCart}`
            button.classList.remove('btn-success')
            button.classList.add('btn-outline-success')
        }
    }
}