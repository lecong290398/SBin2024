// Radio Check and Search Input handle
var searchInput = document.getElementById("filter__search");

searchInput.addEventListener('keydown',(e) => {
if(e.key == 'Enter') {
    filterSearch(searchInput.value)
}
})

var filterSearch = (valueInputCheck) => {
var filter = valueInputCheck ? valueInputCheck.trim().toUpperCase() : searchInput.value.trim().toUpperCase();
var voucherItems = document.querySelectorAll(".js-filterVoucherItem .voucher__item");

for (var i = 0; i < voucherItems.length; i++) {
    var voucherNameElement = voucherItems[i].querySelector(".voucher__name");
    var voucherPriceElement = voucherItems[i].querySelector(".voucher__price");

    var txtValue = voucherNameElement ? (voucherNameElement.textContent || voucherNameElement.innerText) : "";
    var priceValue = voucherPriceElement ? (voucherPriceElement.textContent || voucherPriceElement.innerText) : "";

    if (txtValue.toUpperCase().indexOf(filter) > -1 || priceValue.indexOf(filter) > -1) {
        voucherItems[i].parentElement.style.display = "block"; // Đảm bảo rằng phần tử hiển thị trên dòng mới
    } else {
        voucherItems[i].parentElement.style.display = "none";
    }
}
};
// Handle my voucher

const myVoucherBtnList = document.querySelectorAll('.js-MyVoucher')
const myVoucherModal = document.querySelector('.js-ModalMyVoucher')
const myVoucherCloseBtnList = document.querySelectorAll('.js-CloseModalMyVoucher')

myVoucherBtnList.forEach(btn => {
btn.addEventListener('click', () => {
    myVoucherModal.classList.add('open');
    document.body.style.overflow = 'hidden';
});
});

myVoucherCloseBtnList.forEach(btn => {
btn.addEventListener('click', () => {
    myVoucherModal.classList.remove('open');
    document.body.style.overflow = 'auto';
});
});
