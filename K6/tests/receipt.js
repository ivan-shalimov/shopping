import { sleep, check } from 'k6'
import exec from 'k6/execution';
import http from 'k6/http';
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { UUIDv4 } from '../shared/utils.js';
import { testProducts, generateReceiptItems } from '../shared/receiptHelper.js';

// 1. init code
const baseURL = 'http://shopping.api/api/receipts';
// delays to emulate user activity in seconds
const typingDelay = 10;
const buttonClickDelay = 5;
const executionIdentificator = (new Date(Date.now()).getHours() + 1) * 100 + new Date(Date.now()).getMinutes();
const currentMonth = new Date(Date.now()).getMonth();

export function setup() {
    // 2. setup code
    //#region getting other product kind id
    const productkindsResponse = http.get('http://shopping.api/api/products/kinds');
    if (productkindsResponse.status !== 200) {
        throw new Error(`Request to ${productkindsResponse.request.url} failed with code ${productkindsResponse.status} and message '${productkindsResponse.error}'`);
    }

    let otherProductKindid = productkindsResponse.json('#(name=="Other").id');
    // console.log(`Product kind id from response ${otherProductKindid}`);
    if (otherProductKindid == undefined) {
        otherProductKindid = UUIDv4();
        const productKind = { id: otherProductKindid, name: `Other` };
        const addProductKindresponse = http.post(
            'http://shopping.api/api/products/kinds',
            JSON.stringify(productKind),
            { headers: { 'content-type': 'application/json' } }
        );
        if (addProductKindresponse.status !== 200) {
            throw new Error(`Request to ${addProductKindresponse.request.url} failed with code ${addProductKindresponse.status} and message '${addProductKindresponse.error}'`);
        }
    }
    //#endregion getting other product kind id

    //#region getting product's ids for receipt builder
    const productsResponse = http.get('http://shopping.api/api/products');
    if (productsResponse.status !== 200) {
        throw new Error(`Request to ${productsResponse.request.url} failed with code ${productsResponse.status} and message '${productsResponse.error}'`);
    }

    const products = {};
    for (const productName of testProducts) {
        const productId = productsResponse.json(`#(name=="${productName}").id`);
        // console.log(`Product id from response ${productId} for ${productName}`);
        if (productId === undefined) {
            const id = UUIDv4();
            const product = { id, productKindId: otherProductKindid, type: '', name: productName };
            const addProductResponse = http.post(
                'http://shopping.api/api/products',
                JSON.stringify(product),
                { headers: { 'content-type': 'application/json' } }
            );
            if (addProductKindresponse.status !== 200) {
                throw new Error(`Request to ${addProductResponse.request.url} failed with code ${addProductResponse.status} and message '${addProductResponse.error}'`);
            }

            // console.log(`Product id after adding ${id} for ${productName}`);
            products[productName] = id;
        } else {
            products[productName] = productId;
        }
    }
    //#endregion getting product's ids for receipt builder

    return { products };
}

export function receiptTests(data) {
    // throw new Error();
    const id = UUIDv4();
    const receipt = { id, description: `Wrong Test shop ${exec.vu.idInTest} #${executionIdentificator}`, date: new Date(Date.now()).toISOString() };
    const descriptionToUpdate = `Test shop ${exec.vu.idInTest} #${executionIdentificator}`;
    const receiptItems = generateReceiptItems(receipt.id, data.products);
    const receiptItemToUpdateId = receiptItems[1].id;
    const receiptItemToUpdateRequest = { amount: 3, price: receiptItems[1].price };
    const receiptItemToDeleteId = receiptItems[0].id;

    describe('Fetch receipts', () => {
        const response = http.get(`${baseURL}?month=${currentMonth}`);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Add new receipt', () => {
        sleep(typingDelay); // emulate typing

        const response = http.post(
            baseURL,
            JSON.stringify(receipt),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Update receipts', () => {
        sleep(typingDelay); // emulate typing

        const request = {
            date: receipt.date,
            description: descriptionToUpdate
        }
        const response = http.put(
            `${baseURL}/${receipt.id}`,
            JSON.stringify(request),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Get products before adding receipt items', () => {
        sleep(buttonClickDelay); // emulate action
        const response = http.get('http://localhost:19092/api/products');

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Load last prices for receipt items', () => {
        sleep(typingDelay); // emulate typing
        // Load last prices for selected
        const productIdsQueryPart = receiptItems.reduce((pv, cv) => pv + '&productIds=' + cv.productId, '');
        const response = http.get(
            `http://localhost:19092/api/prices/latest?receiptId=${receipt.id}${productIdsQueryPart}`
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    //#region receipt items

    describe('Fetch receipt items', () => {
        const response = http.get(`${baseURL}/${receipt.id}/items`);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    receiptItems.forEach((item, index) => {
        describe(`Add new receipt items ${index}`, () => {
            sleep(typingDelay); // emulate typing

            const response = http.post(
                `${baseURL}/${receipt.id}/items`,
                JSON.stringify(item),
                { headers: { 'content-type': 'application/json' } }
            );

            expect(response.status, 'response status').to.equal(200);
            expect(response).to.have.validJsonBody();
        });
    });

    describe('Update receipt items', () => {
        sleep(typingDelay); // emulate typing

        const response = http.put(
            `${baseURL}/${receipt.id}/items/${receiptItemToUpdateId}`,
            JSON.stringify(receiptItemToUpdateRequest),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Delete receipts item', () => {
        sleep(buttonClickDelay); // emulate action

        const response = http.del(`${baseURL}/${receipt.id}/items/${receiptItemToDeleteId}`);

        expect(response.status, 'response status').to.equal(200);
    });
    //#endregion receipt items
}