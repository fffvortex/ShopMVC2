let check = document.querySelector('.fa-circle-half-stroke')
let checkbox = document.getElementById("checkbox")
let header = document.getElementsByClassName('sticky-top')

function setTheme() {
    document.documentElement.setAttribute('data-bs-theme', localStorage.getItem('theme'))
    if (localStorage.getItem('theme') === 'dark') {
        for (let el of header) {
            el.classList.remove('top-light')
            el.classList.add('top-dark')
        }
    }
    else {
        for (let el of header) {
            el.classList.remove('top-dark')
            el.classList.add('top-light')
        }
    }
}

checkbox.addEventListener("click", () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.documentElement.setAttribute('data-bs-theme', 'light')
        localStorage.setItem('theme', 'light')

        check.classList.add('fa-spin')
        setTimeout(() => {
            check.classList.remove('fa-spin')
        }, 2000)
        setTheme()
    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        localStorage.setItem('theme', 'dark')
        check.classList.add('fa-spin')
        setTimeout(() => {
            check.classList.remove('fa-spin')
        }, 2000)
        setTheme()
    }
})