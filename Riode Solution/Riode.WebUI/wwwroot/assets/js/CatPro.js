let lis = document.querySelectorAll('.widget-body li')
let productwrapper = document.querySelector('.products-wrapper')
let products = document.querySelectorAll('.products-wrapper .product')
let listicon = document.querySelector('.mode-list')
let gridicon = document.querySelector('.mode-grid')
let anchor
lis.forEach(li => {
    li.addEventListener('click', e => {
        anchor = li.querySelector('a').innerText
        productForEach("block")
    })
    listicon.addEventListener('click', e => {
        productForEach("flex")
    })
    gridicon.addEventListener('click', e => {
        productForEach("block")
    })
})
function productForEach(display) {
    products.forEach(p => {
        p.style.display = "none"
        if (p.querySelector('.product-cat a').innerText == anchor) {
            p.style.display = display
        }
    })
}









