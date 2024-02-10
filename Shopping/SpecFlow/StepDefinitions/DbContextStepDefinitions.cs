using Bogus;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.SpecFlow.Contexts;
using Shopping.SpecFlow.Extensions;
using System.Collections.ObjectModel;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;
using Product = Shopping.Models.Domain.Product;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class DbContextStepDefinitions
    {
        private readonly ShoppingContext _stactisticContext;
        private readonly ScenarioContext _scenarioContext;

        public ShoppingDbContext _context { get; }

        public DbContextStepDefinitions(ScenarioContext scenarioContext, ShoppingContext stactisticContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _stactisticContext = stactisticContext;
            _context = context;
        }

        [Given(@"The Db has the following products")]
        public void GivenTheDbHasTheFollowingProducts(Table table)
        {
            foreach (var row in table.Rows)
            {
                var kind = row["Kind name"];
                var product = row["Product Name"];

                if (!_stactisticContext.Kinds.ContainsKey(kind))
                {
                    var kindEntity = _context.ProductKinds.FirstOrDefault(e => e.Name == kind);
                    if (kindEntity == null)
                    {
                        kindEntity = new Models.Domain.ProductKind { Id = Guid.NewGuid(), Name = kind, IsMain = true, };
                        _context.ProductKinds.Add(kindEntity);
                        _context.SaveChanges();
                    }

                    _stactisticContext.Kinds.Add(kindEntity.Name, kindEntity.Id);
                }

                if (!_stactisticContext.Prodcuts.ContainsKey(product))
                {
                    var productEntity = _context.Products.FirstOrDefault(e => e.Name == product);
                    if (productEntity == null)
                    {
                        var kindId = _stactisticContext.Kinds[kind];
                        productEntity = new Models.Domain.Product { Id = Guid.NewGuid(), Name = product, ProductKindId = kindId };
                        _context.Products.Add(productEntity);
                        _context.SaveChanges();
                    }

                    _stactisticContext.Prodcuts.Add(productEntity.Name, productEntity.Id);
                }
            }
        }

        [Given(@"The Db has receipt for '([^']*)' shop on (.*) with the following items")]
        public void GivenTheDbHasReceiptForShopOnStOfThisMonthWithTheFollowingItems(string shop, DateTime date, Table table)
        {
            var receipt = _context.Receipts.FirstOrDefault(e => e.Description == shop && e.Date == date);
            if (receipt != null)
            {
                // data has been already added to DB
                return;
            }

            receipt = new Models.Domain.Receipt { Id = Guid.NewGuid(), Date = date, Description = shop };
            _context.Add(receipt);
            foreach (var row in table.Rows)
            {
                var product = row["Product Name"];
                var productId = _stactisticContext.Prodcuts.TryGetValue(product, out var id) ? id
                    : throw new InvalidOperationException($"The product {product} is not in context");
                var price = decimal.TryParse(row["Price"], out var priceValue) ? priceValue
                    : throw new InvalidOperationException($"The Price {row["Price"]} is not valid");
                var amount = decimal.TryParse(row["Amount"], out var amountValue) ? amountValue
                    : throw new InvalidOperationException($"The Amount {row["Amount"]} is not valid");

                var reseiptItem = new Models.Domain.ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = receipt.Id,
                    ProductId = productId,
                    Price = price,
                    Amount = amount
                };
                _context.Add(reseiptItem);
            }

            _context.SaveChanges();
        }


        [Given(@"The DB has the price change projection with the following items")]
        public void GivenTheDBHasThePriceChangeProjectionWithTheFollowingItems(Table table)
        {
            _context.RemoveRange(_context.PriceChangeProjections);
            foreach (var row in table.Rows)
            {
                var previousCost = decimal.TryParse(row["PreviousCost"], out var previousCostValue) ? previousCostValue
                    : throw new InvalidOperationException($"The PreviousCost {row["PreviousCost"]} is not valid");
                var lastCost = decimal.TryParse(row["LastCost"], out var lastCostValue) ? lastCostValue
                    : throw new InvalidOperationException($"The LastCost {row["LastCost"]} is not valid");
                var changePercent = decimal.TryParse(row["ChangePercent"], out var changePercentValue) ? changePercentValue
                    : throw new InvalidOperationException($"The ChangePercent {row["ChangePercent"]} is not valid");

                var item = new PriceChangeProjection
                {
                    Id = Guid.NewGuid(),
                    ProductName = row["Product Name"],
                    ProductKindName = row["Kind Name"],
                    Shop = row["Shop"],
                    PreviousPrice = previousCost,
                    LastPrice = lastCost,
                    ChangePercent = changePercent,
                    ChangedDate = DateTime.UtcNow,
                };

                _context.Add(item);
            }

            _context.SaveChanges();
        }


        [Given(@"The DB has the product kind")]
        public void GivenTheDBHasAProductKind()
        {
            var productKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word(), IsMain = true };
            _scenarioContext[TheProductKind] = productKind;
            _scenarioContext[TheProductKindId] = productKind.Id;
            _scenarioContext[TheProductKindName] = productKind.Name;
            _context.ProductKinds.Add(productKind);
            _context.SaveChanges();
        }

        [Given(@"The DB has another product kind")]
        public void GivenTheDBHasAnotherProductKind()
        {
            var productKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word(), IsMain = true };
            _scenarioContext[AnotherProductKind] = productKind;
            _context.ProductKinds.Add(productKind);
            _context.SaveChanges();
        }

        [Given(@"The DB has the product")]
        public void GivenTheDBHasTheProduct()
        {
            var productKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word(), IsMain = true };
            _scenarioContext[TheProductKind] = productKind;
            _scenarioContext[TheProductKindId] = productKind.Id;
            _scenarioContext[TheProductKindName] = productKind.Name;
            _context.ProductKinds.Add(productKind);

            var product = new Product { Id = Guid.NewGuid(), ProductKindId = productKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[TheProduct] = product;
            _scenarioContext[ProductId] = product.Id;
            _scenarioContext[TheProductName] = product.Name;
            _context.Products.Add(product);

            _context.SaveChanges();
        }

        [Given(@"The DB has another product")]
        public void GivenTheDBHasAnotherProduct()
        {
            var anotherProductKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word(), IsMain = true };
            _scenarioContext[AnotherProductKind] = anotherProductKind;
            _context.ProductKinds.Add(anotherProductKind);

            var anotherProduct = new Product { Id = Guid.NewGuid(), ProductKindId = anotherProductKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[AnotherProduct] = anotherProduct;
            _scenarioContext[AnotherProductId] = anotherProduct.Id;
            _context.Products.Add(anotherProduct);
            _context.SaveChanges();
        }

        [Given(@"The DB has a product for the product kind")]
        public void GivenTheDBHasAProductForTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var product = new Product { Id = Guid.NewGuid(), ProductKindId = productKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[TheProduct] = product;
            _scenarioContext[ProductId] = product.Id;
            _scenarioContext[TheProductName] = product.Name;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [Given(@"The DB has the product with hidden flag")]
        public void GivenTheDBHasTheProductWithHiddenFlag()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var product = new Product { Id = Guid.NewGuid(), ProductKindId = productKind.Id, Name = new Faker().Name.Random.Word(), Hidden = true };
            _scenarioContext[TheProduct] = product;
            _scenarioContext[ProductId] = product.Id;
            _scenarioContext[TheProductName] = product.Name;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [Given(@"The DB has another product for another product kind")]
        public void GivenTheDBHasAnotherProductForAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var anotherProduct = new Product { Id = Guid.NewGuid(), ProductKindId = anotherProductKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[AnotherProduct] = anotherProduct;
            _scenarioContext[AnotherProductId] = anotherProduct.Id;
            _context.Products.Add(anotherProduct);
            _context.SaveChanges();
        }

        [Given(@"The product is used")]
        public void GivenTheProductIsUsed()
        {
            var theProdcut = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var receipt = new Receipt { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Description = "test" };
            _context.Receipts.Add(receipt);
            var receiptItem = new ReceiptItem { Id = Guid.NewGuid(), ReceiptId = receipt.Id, ProductId = theProdcut.Id };
            _context.ReceiptItems.Add(receiptItem);
            _context.SaveChanges();
            _scenarioContext[TheProductReceiptItemId] = receiptItem.Id;
        }

        [Given(@"The another product is used")]
        public void GivenTheAnotherProductIsUsed()
        {
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);
            var receipt = new Receipt { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Description = "test" };
            _context.Receipts.Add(receipt);
            var anotherProductReceiptItem = new ReceiptItem { Id = Guid.NewGuid(), ReceiptId = receipt.Id, ProductId = anotherProduct.Id };
            _context.ReceiptItems.Add(anotherProductReceiptItem);
            _context.SaveChanges();
            _scenarioContext[AnotherProductReceiptItemId] = anotherProductReceiptItem.Id;
        }

        [Given(@"The DB has the receipt created in this month")]
        public void GivenTheDBHasTheReceiptCreatedInThisMonth()
        {
            var receipt = new Receipt { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Description = "test" };
            _scenarioContext[TheReceipt] = receipt;
            _scenarioContext[TheReceiptId] = receipt.Id;
            _context.Receipts.Add(receipt);
            _context.SaveChanges();
        }

        [Given(@"The DB has the receipt")]
        public void GivenTheDBHasTheReceipt()
        {
            GivenTheDBHasTheReceiptCreatedInThisMonth();
        }

        [Given(@"The DB has the item with the product of the receipt")]
        public void GivenTheDBHasTheItemOfTheReceipt()
        {
            var receipt = _scenarioContext.Get<Receipt>(TheReceipt);
            var theProdcut = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var receiptItem = new ReceiptItem { Id = Guid.NewGuid(), ReceiptId = receipt.Id, ProductId = theProdcut.Id };
            _context.ReceiptItems.Add(receiptItem);

            _scenarioContext[TheReceiptItem] = receiptItem;
            _scenarioContext[TheReceiptItemId] = receiptItem.Id;
            _context.SaveChanges();
        }

        [Then(@"The DB should contain the product")]
        public void ThenTheDBShouldContainTheProduct()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            _context.Products.AsNoTracking()
                .Should().Contain(p => p.Id == product.Id)
                .Which.Should().BeEquivalentTo(product);
        }

        [Then(@"The DB should not contain the product")]
        public void ThenTheDBShouldNotContainTheProduct()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            _context.Products.AsNoTracking()
                .Should().NotContainEquivalentOf(product);
        }

        [Then(@"The DB should not contain another product")]
        public void ThenTheDBShouldNotContainAnotherProduct()
        {
            var notherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);
            _context.Products.AsNoTracking()
                .Should().NotContainEquivalentOf(notherProduct);
        }

        [Then(@"The DB should contain the product with updated info")]
        public void ThenTheDBShouldContainTheProductForTheAnotherProductKindWithTheNewName()
        {
            var productId = _scenarioContext.GetValueOrDefault<Guid>(ProductId);
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var newName = _scenarioContext.GetValueOrDefault<string>(TheNewName);
            var newType = _scenarioContext.GetValueOrDefault<string>(TheNewType);

            var product = new Product { Id = productId, Type = newType, Name = newName, ProductKindId = anotherProductKind.Id };
            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(product);
        }

        [Then(@"The DB should contain the product kind")]
        public void ThenTheDBShouldContainTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().ContainEquivalentOf(new ProductKind { Id = productKind.Id, Name = productKind.Name });
        }

        [Then(@"The DB should not contain the product kind")]
        public void ThenTheDBShouldNotHaveTheProduct()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().NotContainEquivalentOf(new ProductKind { Id = productKind.Id, Name = productKind.Name });
        }

        [Then(@"The DB should not contain another product kind")]
        public void ThenTheDBShouldNotContainAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().NotContainEquivalentOf(new ProductKind { Id = anotherProductKind.Id, Name = anotherProductKind.Name });
        }

        [Then(@"The DB should contain the product kind with the new name")]
        public void ThenTheDBShouldContainTheProductKindWithTheNewName()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var productKindId = _scenarioContext.GetValueOrDefault<string>(ProductKindId);
            var newName = _scenarioContext.GetValueOrDefault<string>(TheNewName);

            var product = new ProductKind { Id = Guid.Parse(productKindId), Name = newName, IsMain = productKind.IsMain };
            _context.ProductKinds.AsNoTracking()
                .Should().ContainEquivalentOf(product);
        }

        [Then(@"The DB should contain both products for the product kind")]
        public void ThenTheDBShouldContainBothProductsForTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);

            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(new Product { Id = product.Id, Name = product.Name, ProductKindId = productKind.Id });
            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(new Product { Id = anotherProduct.Id, Name = anotherProduct.Name, ProductKindId = productKind.Id });
        }

        [Then(@"The DB should contain the product with hidden flag")]
        public void ThenTheDBShouldContainTheProductWithHiddenFlag()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);

            _context.Products.AsNoTracking()
                .Should().Contain(e => e.Id == product.Id)
                .Which.Hidden.Should().BeTrue();
        }

        [Then(@"The DB should contain the product without hidden flag")]
        public void ThenTheDBShouldContainTheProductWithoutHiddenFlag()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);

            _context.Products.AsNoTracking()
                .Should().Contain(e => e.Id == product.Id)
                .Which.Hidden.Should().BeFalse();
        }

        [Then(@"The DB should contain the receipt")]
        public void ThenTheDBShouldContainTheReceipt()
        {
            var receipt = _scenarioContext.GetValueOrDefault<Receipt>(TheReceipt);

            _context.Receipts.AsNoTracking()
                .Should().Contain(r => r.Id == receipt.Id)
                .Which.Should().BeEquivalentTo(receipt);
        }

        [Then(@"The DB should contain the receipt with the new description and date")]
        public void ThenTheDBShouldContainTheReceiptWithTheNewDescriptionAndDate()
        {
            var receipt = _scenarioContext.GetValueOrDefault<Receipt>(TheReceipt);

            var expectedReceipt = new { Description = receipt.Description, Date = receipt.Date };
            _context.Receipts.AsNoTracking()
                .Should().Contain(r => r.Id == receipt.Id)
                .Which.Should().BeEquivalentTo(expectedReceipt, config => config.ExcludingMissingMembers());
        }

        [Then(@"The DB should contain the item of the receipt")]
        public void ThenTheDBShouldContainTheItemOfTheReceipt()
        {
            var receiptItem = _scenarioContext.GetValueOrDefault<ReceiptItem>(TheReceiptItem);

            _context.ReceiptItems.AsNoTracking()
                .Should().Contain(r => r.Id == receiptItem.Id)
                .Which.Should().BeEquivalentTo(receiptItem);
        }

        [Then(@"The DB should not contain the item of the receipt")]
        public void ThenTheDBShouldNotContainTheItemOfTheReceipt()
        {
            var receiptItemId = _scenarioContext.GetValueOrDefault<Guid>(TheReceiptItemId);

            _context.ReceiptItems.AsNoTracking()
                .Should().NotContain(r => r.Id == receiptItemId);
        }

        [Then(@"The DB should contain receipt's items of the product assigned to the product")]
        public void ThenTheDBShouldContainReceiptsItemsOfTheProductAssignedToTheProduct()
        {
            var receiptItemId = _scenarioContext.GetValueOrDefault<Guid>(TheProductReceiptItemId);
            var productId = _scenarioContext.GetValueOrDefault<Guid>(ProductId);

            _context.ReceiptItems.AsNoTracking()
                .Should().Contain(r => r.Id == receiptItemId)
                .Which.ProductId.Should().Be(productId);
        }

        [Then(@"The DB should contain receipt's items of another product assigned to the product")]
        public void ThenTheDBShouldContainReceiptsItemsOfAnotherProductAssignedToTheProduct()
        {
            var receiptItemId = _scenarioContext.GetValueOrDefault<Guid>(AnotherProductReceiptItemId);
            var productId = _scenarioContext.GetValueOrDefault<Guid>(ProductId);

            _context.ReceiptItems.AsNoTracking()
                .Should().Contain(r => r.Id == receiptItemId)
                .Which.ProductId.Should().Be(productId);
        }
    }
}