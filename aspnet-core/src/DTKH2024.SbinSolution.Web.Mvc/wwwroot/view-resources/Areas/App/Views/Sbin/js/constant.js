// constants.js

const API_BASE_URL = "https://localhost:44302/api/services/app";

const API_ENDPOINTS = {
    GET_ALL_BRANDS: `${API_BASE_URL}/Brands/GetAllForClient?SkipCount=0&MaxResultCount=100`,
    GET_TOP_CUSTOMERS: `${API_BASE_URL}/Customers/GetAllForClient?Sorting=positivePoint%20desc&SkipCount=0&MaxResultCount=10`,
    GET_PRODUCTS: `${API_BASE_URL}/Products/GetProductForView`,
    GET_VOUCHER: `${API_BASE_URL}/ProductPromotions/GetAll`
};
