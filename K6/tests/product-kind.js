import { sleep } from 'k6'
import http from 'k6/http';
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { UUIDv4 } from '../shared/utils.js';

// 1. init code
const baseURL = 'http://shopping.api/api/products/kinds';
// delays to emulate user activity in seconds
const typingDelay = 10;
const simpleActionDelay = 5;

export function setup() {
    // 2. setup code
}

export function productKindTests() {
    const id = UUIDv4();
    const productKind = { id: id, name: `Test product kind #${id}` };

    describe('Fetch product kinds', () => {
        const response = http.get(baseURL);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Add new product kind', () => {
        sleep(typingDelay); // emulate typing

        const response = http.post(
            baseURL,
            JSON.stringify(productKind),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Update product kind', () => {
        sleep(typingDelay); // emulate typing

        const request = { name: `Test product kind #${id} Updated` };
        const response = http.put(
            `${baseURL}/${productKind.id}`,
            JSON.stringify(request),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Delete product kind', () => {
        sleep(simpleActionDelay); // emulate action

        const response = http.del(`${baseURL}/${productKind.id}`);

        expect(response.status, 'response status').to.equal(200);
    });
}