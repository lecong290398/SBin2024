const popupBtnList = document.querySelectorAll('.js-popup')
const popupModal = document.querySelector('.js-Modalpopup')
const popupCloseBtnList = document.querySelectorAll('.js-CloseModalpopup')

popupBtnList.forEach((btn) => {
    btn.addEventListener('click', () => {
        popupModal.classList.add('open');
        document.body.style.overflow = 'hidden';
        // Calls API here
        const appRedeemGift = {
            api: API_ENDPOINTS.GET_PRODUCTS,
            render() {
                $.ajax({
                    url: this.api,
                    type: 'GET',
                    success: function(response) {
                        if (Array.isArray(response.result.items)) {
                            const jsVoucherDetails = document.querySelector(".js-VoucherDetails");
                            if (jsVoucherDetails) {
                                const html = response.result.items.map((content) => {
                                    return `
                                        <h1 class="popup__header exchange__voucher--heading">${content.product.productName}</h1>
                                        <p class="popup__describe exchange__voucher--sub-heading">${content.product.timeDescription}</p>
                                        <hr class="dashed-line">
                                        <div class="flex-center" style="justify-content: space-between;">
                                            <div class="flex-center">
                                                <img class="exchange__voucher--brand" src="/view-resources/Areas/App/Views/Sbin/assets/img/brand/${content.product.id}.png" alt="${content.product.productName}">
                                                <p class="exchange__voucher--brand-sub">${content.brandName}</p>
                                            </div>
                                            <p class="exchange__voucher--brand-sub">${content.product.supportAndComplaints}</p>
                                        </div>
                                        <div class="exchange__voucher--description">
                                            <p class="bold">${content.product.userManual}</p>
                                            <ul>
                                                <li>Giá trị: ${content.product.scopeOfApplication} đồng</li>
                                                <li>${content.product.description}</li>
                                                <li>${content.product.applicableSubjects}</li>
                                                <li>${content.product.regulations}</li>
                                            </ul>
                                        </div>
                                        <div class="flex-center" style="justify-content: space-between;">
                                            <button style="font-size: 13px;" onclick="cancelExchange()" class="_btn">Hủy Chọn</button>
                                            <button style="font-size: 13px;" onclick="exchangeVoucher()" class="_btn">Đổi Ngay</button>
                                        </div>
                                    `;
                                }).join("");  // Sử dụng join để ghép chuỗi HTML
                                jsVoucherDetails.innerHTML = html;
                            } else {
                                console.error("Không tìm thấy phần tử với class .js-VoucherDetails");
                            }
                        } else {
                            console.error("Kết quả trả về không phải là mảng", response);
                        }
        
                        console.log(response);
                    },
                    error: function(xhr, status, error) {
                        console.error("Lỗi khi lấy dữ liệu từ API:", error);
                    }
                });
            },
            start() {
                this.render();
            },
        };
        appRedeemGift.start();
    });
})

popupCloseBtnList.forEach((btn) => {
    btn.addEventListener('click', () => {
        popupModal.classList.remove('open');
        document.body.style.overflow = 'auto';
    });
});

function cancelExchange() {
    popupModal.classList.remove('open');
    document.body.style.overflow = 'auto';
}
function exchangeVoucher() {
    
}