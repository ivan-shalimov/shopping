import { sleep } from 'k6'
import http from 'k6/http';
import { describe, expect } from 'https://jslib.k6.io/k6chaijs/4.3.4.3/index.js';
import { UUIDv4 } from '../shared/utils.js';

// 1. init code
const baseURL = 'http://shopping.api/api/car-costs';
// delays to emulate user activity in seconds
const typingDelay = 10;
const simpleActionDelay = 5;
const currentMonth = new Date(Date.now()).getMonth() + 1;

export function setup() {
    // 2. setup code
}

export function carCostsTests() {
    const id = UUIDv4();
    const price = 50 + 50 * Math.random();
    const amount = 10 + 10 * Math.random();
    const carCost = { id: id, description: 'Petrol A95', price, amount, date: new Date(Date.now()).toISOString() };

    describe('Fetch car costs', () => {
        const response = http.get(`${baseURL}?month=${currentMonth}`);

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Add new car cost', () => {
        sleep(typingDelay); // emulate typing

        const response = http.post(
            baseURL,
            JSON.stringify(carCost),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Update car cost', () => {
        sleep(typingDelay); // emulate typing

        const request = { description: 'Petrol A95 Updated', price, amount, date: new Date(Date.now()).toISOString() };
        const response = http.put(
            `${baseURL}/${carCost.id}`,
            JSON.stringify(request),
            { headers: { 'content-type': 'application/json' } }
        );

        expect(response.status, 'response status').to.equal(200);
        expect(response).to.have.validJsonBody();
    });

    describe('Delete car cost', () => {
        sleep(simpleActionDelay); // emulate action

        const response = http.del(`${baseURL}/${carCost.id}`);

        expect(response.status, 'response status').to.equal(200);
    });
}