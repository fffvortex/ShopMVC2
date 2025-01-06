function setTheme() {
    document.documentElement.setAttribute('data-bs-theme', localStorage.getItem('theme'))
}
let check = document.querySelector('.fa-circle-half-stroke')
let checkbox = document.getElementById("checkbox")
checkbox.addEventListener("click", () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.documentElement.setAttribute('data-bs-theme', 'light')

        check.classList.add('fa-spin')
        setTimeout(() => {
            check.classList.remove('fa-spin')
        }, 2000)
        localStorage.setItem('theme', 'light')
    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        check.classList.add('fa-spin')
        setTimeout(() => {
            check.classList.remove('fa-spin')
        }, 2000)
        localStorage.setItem('theme', 'dark')
    }
})