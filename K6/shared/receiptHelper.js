import { UUIDv4 } from './utils.js';

export const testProducts = [
    'Carrots',
    'Tomatoes',
    'Beetroot',
    'Onion',
    'Potato',
    'Garlic',

    'Apple',
    'Banana',
    'Orange',
    'Strawberry',
    'Raspberry',
    'Cherries',
];

export const defaultPrices = {
    'Carrots': 1.25,
    'Tomatoes': 3,
    'Beetroot': 1,
    'Onion': 1.5,
    'Potato': 1.2,
    'Garlic': 5,

    'Apple': 5.5,
    'Banana': 8,
    'Orange': 15,
    'Strawberry': 11,
    'Raspberry': 12,
    'Cherries': 9,
}

export function generateReceiptItems(receiptId, productIds) {
    const items = [];

    function addItemForProductWithAmount(product, amount) {

        const productId = productIds[product];
        const defaultPrice = defaultPrices[product];
        const price = Math.random() < 0.2 ? defaultPrice : defaultPrice + defaultPrice * Math.random();

        items.push({ id: UUIDv4(), productId, receiptId, amount, price });
    }

    for (const product of testProducts) {
        if (items.length == 5) {
            // avoid adding to many items
            return items;
        }

        if (Math.random() < 0.3) {
            const amount = Math.trunc(Math.random() * 10);
            if (amount == 0) {
                continue;
            }

            addItemForProductWithAmount(product, amount);
        }
    }

    if (items.length < 2) {
        // add at least two items
        addItemForProductWithAmount('Apple', 1);
        addItemForProductWithAmount('Cherries', 1);
    }

    return items;
}