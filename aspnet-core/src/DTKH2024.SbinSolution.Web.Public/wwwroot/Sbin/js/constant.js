// constants.js

const API_BASE_URL = "https://localhost:44301/api/services/app";

const API_ENDPOINTS = {
    GET_ALL_BRANDS: `${API_BASE_URL}/Brands/GetAllForClient?SkipCount=0&MaxResultCount=100`,
    GET_TOP_CUSTOMERS: `${API_BASE_URL}/Customers/GetAllForClient?Sorting=positivePoint%20desc&SkipCount=0&MaxResultCount=10`
};
