async function add(productId) {
    var userNameEl = document.getElementById('username')
    if (userNameEl == null) {
        window.location.href = "/Identity/Account/Login" 
    }

    var index = document.querySelectorAll('.add-to-card') 

    for (let el of index) 
    {
        if (Number(el.id.substring(12, el.id.length)) === (Number(productId))) 
        {
            var quantityInCartRes = await fetch(`/Cart/GetProductQuantityInCartByProductId?productId=${productId}`) 
            var quantityRes = await fetch(`/Stock/GetQuantityInStockByProductId?productId=${productId}`) 
            quantityInStock = await quantityRes.json()
            quantityInCart = await quantityInCartRes.json()

            if (quantityInCart < quantityInStock) 
            {
                try {
                    var response = await fetch(`/Cart/AddProduct?productId=${productId}`) 
                    if (response.status === 200) {
                        var result = await response.json()
                        var cartCountEl = document.getElementById('cartCount')  
                        cartCountEl.innerHTML = result
                        SetBtnCard(el, quantityInCart, quantityInStock) 
                    }
                }
                catch (err) {
                    console.log(err)
                }
            }
        }
    }
}

async function SetBtnCard(el, quantityInCart, quantityInStock) {
    if (quantityInCart+1 === quantityInStock) {
        el.classList.remove('btn-success')
        el.classList.add('btn-outline-success')
        el.innerHTML = `All in cart`
    }
    else if (quantityInCart < quantityInStock) {
        el.classList.remove('btn-success')
        el.classList.add('btn-outline-success')
        el.innerHTML = `Added ${quantityInCart + 1} of ${quantityInStock}`
    }
}

//bag
async function SetAllBtnCards(productId) {
    const index = document.getElementById(`add-to-card-${productId}`)

    var quantityInCartRes = await fetch(`/Cart/GetProductQuantityInCartByProductId?productId=${productId}`) 
    var quantityRes = await fetch(`/Stock/GetQuantityInStockByProductId?productId=${productId}`)
    quantityInStock = await quantityRes.json()
    quantityInCart = await quantityInCartRes.json()

    if (Number(quantityInCart) === Number(quantityInStock)) {
        index.classList.remove('btn-success')
        index.classList.add('btn-outline-success')
        index.innerHTML = `All in cart`
    }
    else if (quantityInCart === 0) {
        return;
    }
    else{
        
        index.classList.remove('btn-success')
        index.classList.add('btn-outline-success')
        index.innerHTML = `Added ${quantityInCart} of ${quantityInStock}`
    }
}