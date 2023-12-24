import * as dashboard from './tests/dashboard.js';
import * as productKind from './tests/product-kind.js';
import * as product from './tests/product.js';
import * as receipt from './tests/receipt.js';

export function setup() {
  // 2. setup code
  const productData = product.setup();
  const receiptData = receipt.setup();
  return { productData, receiptData };
}

export function teardown(data) {
  product.teardown(data.productData);
}

export function dashboardTests() {
  dashboard.dashboardTests();
}

export function productKindTests() {
  productKind.productKindTests();
}

export function productTests({ productData }) {
  product.productTests(productData);
}

export function receiptTests({ receiptData }) {
  receipt.receiptTests(receiptData);
}
