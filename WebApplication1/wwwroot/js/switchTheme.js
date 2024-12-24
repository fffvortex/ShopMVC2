function setTheme() {
    document.documentElement.setAttribute('data-bs-theme',localStorage.getItem('theme'))
}

document.getElementById('btnSwitch').addEventListener('click', () => {
    if (document.documentElement.getAttribute('data-bs-theme') == 'dark') {
        document.documentElement.setAttribute('data-bs-theme', 'light')
        localStorage.setItem('theme','light')
    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        localStorage.setItem('theme', 'dark')
    }
})