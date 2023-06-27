using Groceteria.SalesOrder.Application.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples.CheckoutOrder
{
    public class CheckoutOrderResponseExample : IExamplesProvider<OrderDto>
    {
        public OrderDto GetExamples()
        {
            return new OrderDto
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "exampleuser",
                TotalPrice = 19.98m,
                OrderedItems = new List<OrderItemDto>
                {
                    new OrderItemDto
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
                    new OrderItemDto
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
                BillingAddress = new BillingAddressDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    AddressLine = "123 Example Street",
                    City = "Example City",
                    State = "Example State",
                    ZipCode = "12345"
                },
                MetaData = new MetadataDto
                {
                    CreatedAt = "24/05/2023 03:25:10 PM",
                    LastModifiedAt = "24/05/2023 03:25:10 PM",
                    CreatedBy = "default",
                    LastModifiedBy = "default"
                }
            };
        }
    }
}
