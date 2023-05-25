using Groceteria.SalesOrder.Application.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples.CheckoutOrder
{
    public class CheckoutOrderRequestExample : IExamplesProvider<CheckoutOrderRequest>
    {
        public CheckoutOrderRequest GetExamples()
        {
            return new CheckoutOrderRequest
            {
                UserName = "exampleuser",
                TotalPrice = 19.98m,
                OrderedItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest
                    {
                        ProductId = "P001",
                        Name = "Example Product",
                        Category = "Electronics",
                        Brand = "Example Brand",
                        Summary = "Example summary",
                        Description = "Example description",
                        Price = 9.99,
                        Color = "Black",
                        Image = "example-image.jpg",
                        Quantity = 2
                    },
                    new OrderItemRequest
                    {
                        ProductId = "P002",
                        Name = "Another Product",
                        Category = "Clothing",
                        Brand = "Example Brand",
                        Summary = "Another summary",
                        Description = "Another description",
                        Price = 19.99,
                        Color = "Blue",
                        Image = "another-image.jpg",
                        Quantity = 1
                    }
                },
                BillingAddress = new BillingDetailsRequest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    AddressLine = "123 Example Street",
                    City = "Example City",
                    State = "Example State",
                    ZipCode = "12345"
                },
                PaymentDetails = new PaymentDetailsRequest
                {
                    CardName = "John Doe",
                    CardNumber = "**** **** **** 1234",
                    Expiration = "12/23",
                    CVV = "***",
                    PaymentMethod = 1
                }
            };

        }
    }
}
