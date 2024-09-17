const popupModal = document.querySelector('.js-Modalpopup');
const popupCloseBtnList = document.querySelectorAll('.js-CloseModalpopup');

// Hàm mở popup và gọi API
function openPopup(event) {
    // Lấy giá trị của thuộc tính data-index từ phần tử được click
    const voucherItem = event.currentTarget.closest('[data-index]');
    const id = voucherItem.getAttribute('data-index'); // Lấy giá trị data-index

    // Mở popup
    popupModal.classList.add('open');
    document.body.style.overflow = 'hidden';

    // Gọi API với id lấy từ data-index
    getVoucherDetails.start(id);
}

// Hàm đóng popup
function closePopup() {
    popupModal.classList.remove('open');
    document.body.style.overflow = 'auto';
}

// Hàm gán sự kiện cho nút đóng popup
popupCloseBtnList.forEach((btn) => {
    btn.addEventListener('click', closePopup);
});
// Hủy chọn
function cancelExchange() {
    popupModal.classList.remove('open');
    document.body.style.overflow = 'auto';
}
function exchangeVoucher() {
    
}
// Hàm gọi API lấy chi tiết voucher
const getVoucherDetails = {
    api: API_ENDPOINTS.GET_PRODUCTS, // Gốc của API
    render(id) {
        // Tạo URL với id từ data-index
        const apiUrl = `${this.api}?id=${id}`;

        $.ajax({
            url: apiUrl,  // Sử dụng URL với id
            type: 'GET',
            success: function(response) {
                // Kiểm tra xem kết quả trả về là một đối tượng hoặc một mảng
                if (response.result && typeof response.result === 'object' && !Array.isArray(response.result)) {
                    // Trường hợp kết quả là đối tượng
                    const product = response.result.product;
                    const jsVoucherDetails = document.querySelector(".js-VoucherDetails");

                    if (jsVoucherDetails) {
                        // Tạo HTML cho đối tượng sản phẩm đơn lẻ
                        const html = `
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
                        jsVoucherDetails.innerHTML = html;
                    } else {
                        console.error("Không tìm thấy phần tử với class .js-VoucherDetails");
                    }
                } else {
                    console.error("Kết quả trả về không phải là đối tượng hợp lệ", response);
                }
            },
            error: function(xhr, status, error) {
                console.error("Lỗi khi lấy dữ liệu từ API:", error);
            }
        });
    },
    start(id) {
        this.render(id); // Truyền id vào khi gọi hàm
    },
};


// Hàm gọi API và hiển thị voucher
const getVoucher = {
    api: API_ENDPOINTS.GET_VOUCHER,
    render() {
        $.ajax({
            url: this.api,
            type: 'GET',
            success: function(response) {
                if (Array.isArray(response.result.items)) {
                    const jsVoucherDetails = document.querySelector(".js-Voucher");
                    if (jsVoucherDetails) {
                        const html = response.result.items.map((content) => {
                            return `
                                <div class="col l-4 m-4 c-6" data-index="${content.productPromotion.id}">
                                    <div class="voucher__item js-popup">
                                        <div class="voucher__value flex-center">
                                            <span>${content.productPromotion.point}</span>
                                            <img  alt="${content.productPromotion.description}" class="coin" src="/view-resources/Areas/App/Views/Sbin/assets/img/coin.png">
                                        </div>
                                        <div class="voucher__img" style="background-image: url('/view-resources/Areas/App/Views/Sbin/assets/img/coupons/coupons-blue.png');"></div>
                                        <div class="voucher__describe">
                                            <p class="voucher__name">${content.productProductName}</p>
                                            <p class="voucher__date">Out of date: ${content.productPromotion.endDate}</p>
                                        </div>
                                    </div>
                                </div>
                            `;
                        }).join("");
                        jsVoucherDetails.innerHTML = html;

                        // Gán sự kiện click cho các phần tử .js-popup
                        attachPopupEvent();
                    } else {
                        console.error("Không tìm thấy phần tử với class .js-Voucher");
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

// Hàm gán sự kiện mở popup
function attachPopupEvent() {
    const popupBtnList = document.querySelectorAll('.js-popup');
    popupBtnList.forEach((btn) => {
        btn.addEventListener('click', openPopup);
    });
}

// Gọi API để hiển thị danh sách voucher ban đầu
getVoucher.start();
