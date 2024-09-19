const popupModal = document.querySelector('.js-Modalpopup');
const popupCloseBtnList = document.querySelectorAll('.js-CloseModalpopup');

function openPopup(event) {
    const voucherItem = event.currentTarget.closest('[data-index]');
    const id = voucherItem.getAttribute('data-index');
    
    popupModal.classList.add('open');
    document.body.style.overflow = 'hidden';
    
    getVoucherDetails.start(id);
}

function closePopup() {
    popupModal.classList.remove('open');
    document.body.style.overflow = 'auto';
}

// Attach event listeners to popup close buttons
popupCloseBtnList.forEach((btn) => btn.addEventListener('click', closePopup));

function cancelExchange() {
    closePopup();
}

function exchangeVoucher() {
    // Add logic for voucher exchange
}

// Fetch and display voucher details
const getVoucherDetails = {
    api: API_ENDPOINTS.GET_PRODUCTS,
    start(id) {
        const apiUrl = `${this.api}?id=${id}`;
        $.ajax({
            url: apiUrl,
            type: 'GET',
            success: function(response) {
                if (response.result && typeof response.result === 'object') {
                    const { product } = response.result;
                    const jsVoucherDetails = document.querySelector(".js-VoucherDetails");

                    if (jsVoucherDetails) {
                        jsVoucherDetails.innerHTML = `
                            <h1 class="popup__header exchange__voucher--heading">${product.productName}</h1>
                            <p class="popup__describe exchange__voucher--sub-heading">${product.timeDescription}</p>
                            <hr class="dashed-line">
                            <div class="flex-center" style="justify-content: space-between;">
                                <div class="flex-center">
                                    <img class="exchange__voucher--brand" src="/view-resources/Areas/App/Views/Sbin/assets/img/brand/${product.id}.png" alt="${product.productName}">
                                    <p class="exchange__voucher--brand-sub">${response.result.brandName}</p>
                                </div>
                                <p class="exchange__voucher--brand-sub">${product.supportAndComplaints}</p>
                            </div>
                            <div class="exchange__voucher--description">
                                <p class="bold">${product.userManual}</p>
                                <ul>
                                    <li>Giá trị: ${product.scopeOfApplication} đồng</li>
                                    <li>${product.description}</li>
                                    <li>${product.applicableSubjects}</li>
                                    <li>${product.regulations}</li>
                                </ul>
                            </div>
                            <div class="flex-center" style="justify-content: space-between;">
                                <button style="font-size: 13px;" onclick="cancelExchange()" class="_btn">Hủy Chọn</button>
                                <button style="font-size: 13px;" onclick="exchangeVoucher()" class="_btn">Đổi Ngay</button>
                            </div>
                        `;
                    } else {
                        console.error("Không tìm thấy phần tử với class .js-VoucherDetails");
                    }
                } else {
                    console.error("Kết quả trả về không hợp lệ", response);
                }
            },
            error: function() {
                console.error("Lỗi khi lấy dữ liệu từ API");
            }
        });
    }
};

// Fetch and display the voucher list
const getVoucher = {
    api: API_ENDPOINTS.GET_VOUCHER,
    start() {
        $.ajax({
            url: this.api,
            type: 'GET',
            success: function(response) {
                if (Array.isArray(response.result.items)) {
                    const voucherContainer = document.querySelector(".js-Voucher");

                    if (voucherContainer) {
                        voucherContainer.innerHTML = response.result.items.map((content) => `
                            <div class="col l-4 m-4 c-6" data-index="${content.productPromotion.id}">
                                <div class="voucher__item js-popup">
                                    <div class="voucher__value flex-center">
                                        <span>${content.productPromotion.point}</span>
                                        <img alt="${content.productPromotion.description}" class="coin" src="/view-resources/Areas/App/Views/Sbin/assets/img/coin.png">
                                    </div>
                                    <div class="voucher__img" style="background-image: url('/view-resources/Areas/App/Views/Sbin/assets/img/coupons/coupons-blue.png');"></div>
                                    <div class="voucher__describe">
                                        <p class="voucher__name">${content.productProductName}</p>
                                        <p class="voucher__date">Out of date: ${content.productPromotion.endDate}</p>
                                    </div>
                                </div>
                            </div>
                        `).join("");

                        attachPopupEvent();
                    } else {
                        console.error("Không tìm thấy phần tử với class .js-Voucher");
                    }
                } else {
                    console.error("API không trả về mảng hợp lệ", response);
                }
            },
            error: function() {
                console.error("Lỗi khi gọi API");
            }
        });
    }
};

// Attach event listener to voucher items to open popup
function attachPopupEvent() {
    document.querySelectorAll('.js-popup').forEach((btn) => {
        btn.addEventListener('click', openPopup);
    });
}

getVoucher.start();

// Handle filtering vouchers by brand
document.querySelectorAll('.js-Sorting').forEach((radio) => {
    radio.addEventListener('change', function() {
        const brandId = this.getAttribute('data-index');
        if (brandId) fetchVoucherData(brandId);
        const voucherContainer = document.querySelector(".js-Voucher");
        voucherContainer.innerHTML = `
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>
        <div class="skeleton skeleton-text skeleton-text__large"></div>`
    });
});

// Fetch vouchers for a specific brand
function fetchVoucherData(brandId) {
    const apiUrl = `${API_ENDPOINTS.GET_VOUCHER_FOR_VIEW}?id=${brandId}`;
    
    $.ajax({
        url: apiUrl,
        type: 'GET',
        success: function(response) {
            if (response.result) {
                const voucherContainer = document.querySelector(".js-Voucher");
                if (voucherContainer && response.result) {
                    const content = response.result;
                    const html = `
                        <div class="col l-4 m-4 c-6" data-index="${content.productPromotion.id}">
                            <div class="voucher__item js-popup">
                                <div class="voucher__value flex-center">
                                    <span>${content.productPromotion.point}</span>
                                    <img alt="${content.productPromotion.description}" class="coin" src="/view-resources/Areas/App/Views/Sbin/assets/img/coin.png">
                                </div>
                                <div class="voucher__img" style="background-image: url('/view-resources/Areas/App/Views/Sbin/assets/img/coupons/coupons-blue.png');"></div>
                                <div class="voucher__describe">
                                    <p class="voucher__name">${content.productProductName}</p>
                                    <p class="voucher__date">Out of date: ${content.productPromotion.endDate}</p>
                                </div>
                            </div>
                        </div>
                    `;
                    voucherContainer.innerHTML = html;
                    attachPopupEvent();
                } else {
                    console.error("Không tìm thấy container để hiển thị voucher");
                }
            } else {
                console.error("API không trả về mảng hợp lệ", response);
            }
        },
        error: function() {
            const voucherContainer = document.querySelector(".js-Voucher");
            voucherContainer.innerHTML = `
            <div class="flex-center" style="height: 400px">
                <img src="https://bizweb.dktcdn.net/100/363/701/themes/735188/assets/empty-cart.png?1715162023244"width="330">
            </div>
            `
        }
    });
}

