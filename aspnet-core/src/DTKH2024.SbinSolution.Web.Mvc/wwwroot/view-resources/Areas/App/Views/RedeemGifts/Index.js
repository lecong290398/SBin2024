﻿$(function () {
    var _productPromotionsService = abp.services.app.redeemGifts;
    const popupModal = document.querySelector('.js-Modalpopup');
    const popupCloseBtnList = document.querySelectorAll('.js-CloseModalpopup');

    // Define the input parameters
    var input = {
        brandID: null,
        filter: null,
        maxPointFilter: null,
        minPointFilter: null,
        maxStartDateFilter: null,
        minStartDateFilter: null,
        maxEndDateFilter: null,
        minEndDateFilter: null,
        promotionCodeFilter: null,
        productProductNameFilter: null,
        categoryPromotionNameFilter: null,
        sorting: null,
        skipCount: 0,
        maxResultCount: 10
    };

    // Fetch and display the voucher list
    const getVoucher = {
        api: _productPromotionsService.getAllProduct,
        start() {
            this.api(input).done(function (response) {
                if (Array.isArray(response.items)) {
                    const voucherContainer = document.querySelector(".js-Voucher");
                    if (voucherContainer) {
                        voucherContainer.innerHTML = response.items.map((content) => `
                            <div class="col l-4 m-4 c-6" data-index="${content.productPromotion.id}" data-product-id="${content.productPromotion.productId}">
                                <div class="voucher__item js-popup">
                                    <div class="voucher__value flex-center">
                                        <span>${content.productPromotion.point}</span>
                                        <img alt="${content.productPromotion.description}" class="coin" src="/view-resources/Areas/App/Views/Sbin/assets/img/coin.png">
                                    </div>
                                    <div class="voucher__img" style="background-image: url('/view-resources/Areas/App/Views/Sbin/assets/img/coupons/coupons-blue.png');"></div>
                                    <div class="voucher__describe">
                                        <p class="voucher__name">${content.brandName}</p>
                                        <p class="voucher__name">${content.productProductName}</p>
                                        <p class="voucher__date">Out of date: ${new Date(content.productPromotion.endDate).toLocaleDateString()}</p>
                                        <p class="voucher__date">Quantity statistics: ${content.productPromotion.quantityCurrent}/${content.productPromotion.quantityInStock}</p>
                                    </div>
                                </div>
                            </div>
                        `).join("");

                        attachPopupEvent();
                    } else {
                        console.error("Element with class .js-Voucher not found");
                    }
                } else {
                    console.error("API did not return a valid array", response);
                }
            }).fail(function () {
                console.error("Error calling API");
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

    //POPUP
    function openPopup(event) {
        const voucherItem = event.currentTarget.closest('[data-index]');
        const productPromotionID = voucherItem.getAttribute('data-index');
        const productID = voucherItem.getAttribute('data-product-id');

        popupModal.classList.add('open');
        document.body.style.overflow = 'hidden';
        getVoucherDetails.start({ productID, productPromotionID });
    }

    function closePopup() {
        popupModal.classList.remove('open');
        document.body.style.overflow = 'auto';
    }

    // Make cancelExchange globally accessible
    window.cancelExchange = function () {
        closePopup();
    };

    window.seachVoucher = function () {
        alert("aloo");
    }

    // Make exchangeVoucher globally accessible
    window.exchangeVoucher = function (productPromotionId) {
        closePopup();
        const preload = document.getElementById("preload");
        preload.style.display = "block";

        abp.services.app.redeemGifts.createRedeemGift(productPromotionId)
            .done(function (points) {
                closePopup();
                preload.style.display = "none";
                abp.message.success("You have used " + points + " points to redeem gifts", "Reward redemption successful").done(function () {
                    window.location.reload();
                });
                abp.notify.success('Reward redemption successful');
            })
            .fail(function (error) {
                closePopup();
                preload.style.display = "none";
                abp.message.error(error.message);
                console.log(error.message);
                abp.notify.error("Redemption failed");
            });
    };



    // Fetch and display voucher details
    const getVoucherDetails = {
        api: _productPromotionsService.getProductPromotionDetail,
        start(input) {
            this.api(input).done(function (response) {
                if (response && typeof response === 'object') {
                    const product = response.informationProduct;
                    const jsVoucherDetails = document.querySelector(".js-VoucherDetails");

                    if (jsVoucherDetails) {
                        jsVoucherDetails.innerHTML = `
                            <h1 class="popup__header exchange__voucher--heading">${product.productName}</h1>
                            <p class="popup__describe exchange__voucher--sub-heading">${product.timeDescription}</p>
                            <hr class="dashed-line">
                            <div class="flex-center" style="justify-content: space-between;">
                                <div class="flex-center">
                                    <img class="exchange__voucher--brand" src="/view-resources/Areas/App/Views/Sbin/assets/img/brand/${product.id}.png" alt="${product.productName}">
                                    <p class="exchange__voucher--brand-sub">${product.brandName}</p>
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
                                <button style="font-size: 13px;" onclick="exchangeVoucher(${product.id})" class="_btn">Đổi Ngay</button>
                            </div>
                        `;
                    } else {
                        console.error("Không tìm thấy phần tử với class .js-VoucherDetails");
                    }
                } else {
                    console.error("Kết quả trả về không hợp lệ", response);
                }
            }).fail(function () {
                console.error("Lỗi khi lấy dữ liệu từ API");
            });
        }
    };

    // Handle filtering vouchers by brand
    document.querySelectorAll('.js-Sorting').forEach((radio) => {
        radio.addEventListener('change', function () {
            const brandId = this.getAttribute('data-index');
            if (brandId) {
                input.brandID = brandId; // Update the input with the selected brandID
                getVoucher.start(); // Fetch vouchers with the updated brandID
            }
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
                <div class="skeleton skeleton-text skeleton-text__large"></div>`;
        });
    });
});
