import { sleep } from 'k6'
import http from 'k6/http';
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';

// 1. init code
const baseURL = 'http://shopping.api/api/statistic';

function dashboardTests() {
    describe('Fetch previous month expenses by product kind', () => {
        const response = http.get(
            `${baseURL}/expenses-by-kind/previous/month`,
            { tags: { name: 'expenses-by-kind' }, });

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Fetch current month expenses by product kind', () => {
        const response = http.get(
            `${baseURL}/expenses-by-kind/current/month`,
            { tags: { name: 'expenses-by-kind' }, });

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Fetch previous year expenses by product kind', () => {
        const response = http.get(
            `${baseURL}/expenses-by-month/previous/year`,
            { tags: { name: 'expenses-by-month' }, });

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Fetch current year expenses by month', () => {
        const response = http.get(
            `${baseURL}/expenses-by-month/current/year`,
            { tags: { name: 'expenses-by-month' }, });

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Fetch current month expenses by shop', () => {
        const response = http.get(`${baseURL}/expenses-by-shop/current/month`);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Fetch current month expenses by shop', () => {
        const response = http.get(            `${baseURL}/product-cost-change?page=1&pageSize=10&orderBy=percent`);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    sleep(5); // emulate navigating to another page
}

export { dashboardTests };