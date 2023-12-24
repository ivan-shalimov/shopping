import { sleep, check } from 'k6'
import http from 'k6/http';
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { UUIDv4 } from '../shared/utils.js';

// 1. init code
const baseURL = 'http://shopping.api/api/products';
// delays to emulate user activity in seconds
const typingDelay = 10;
const simpleActionDelay = 5;

export function setup() {
    // 2. setup code
    const id = UUIDv4();
    const productKind = { id: id, name: `Test product #${id}` };
    const response = http.post(
        'http://shopping.api/api/products/kinds',
        JSON.stringify(productKind),
        { headers: { 'content-type': 'application/json' } }
    );
    if (response.status !== 200) {
        throw new Error(`Request to ${response.request.url} failed with code ${response.status} and message '${response.error}'`);
    }

    return { productKind };
}

export function teardown(data) {
    const response = http.del(`http://shopping.api/api/products/kinds/${data.productKind.id}`);
   
    if (response.status !== 200) {
        throw new Error(`Request to ${response.request.url} failed with code ${response.status} and message '${response.error}'`);
    }
}

export function productTests(data) {
    const id = UUIDv4();
    const product = { id, productKindId: data.productKind.id, type: 'test type', name: `Test product #${id}` };

    describe('Fetch products', () => {
        const response = http.get(baseURL);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Add new product', () => {
        sleep(typingDelay); // emulate typing

        const response = http.post(
            baseURL,
            JSON.stringify(product),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Update product', () => {
        sleep(typingDelay); // emulate typing

        const request = {
            productKindId: data.productKind.id,
            type: 'test type Updated',
            name: `Test product #${id} Updated`
        }
        const response = http.put(
            `${baseURL}/${product.id}`,
            JSON.stringify(request),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Delete product', () => {
        sleep(simpleActionDelay); // emulate action

        const response = http.del(`${baseURL}/${product.id}`);

        expect(response.status, 'response status').to.equal(200);
    });
}