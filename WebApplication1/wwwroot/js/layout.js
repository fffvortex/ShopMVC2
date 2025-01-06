let navLinks = document.querySelectorAll('.nav-link')
let windowPathName = window.location.pathname

navLinks.forEach(nav => {
    let navPath = new URL(nav.href).pathname
    console.log(navPath)

    if ((windowPathName === navPath) || (windowPathName === '/Home/Index') && navPath === '/' ) {
        nav.classList.add('active')
    }
})