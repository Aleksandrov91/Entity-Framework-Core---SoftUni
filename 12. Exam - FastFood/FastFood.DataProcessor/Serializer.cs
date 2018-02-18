namespace FastFood.DataProcessor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Data;
    using DataProcessor.Dto.Export;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            EmployeeOrdersDto employeeOrders = context.Employees
                .Where(e => e.Name == employeeName && e.Orders.Any(o => o.Type.ToString() == orderType))
                .Select(e => new EmployeeOrdersDto
                {
                    Name = e.Name,
                    Orders = e.Orders.Where(o => o.Type.ToString() == orderType).Select(o => new OrderDto
                    {
                        Customer = o.Customer,
                        Items = o.OrderItems.Select(oi => new OrderItemDto
                        {
                            Name = oi.Item.Name,
                            Price = oi.Item.Price,
                            Quantity = oi.Quantity
                        }).ToList(),
                    }).ToArray()
                }).SingleOrDefault();

            employeeOrders.Orders = employeeOrders.Orders
                .OrderByDescending(o => o.TotalPrice)
                .ThenByDescending(i => i.Items.Count)
                .ToList();

            var json = JsonConvert.SerializeObject(employeeOrders);

            return json.ToString();
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            string[] categoriesStringArr = categoriesString.Split(",");

            List<Category> categories = new List<Category>();

            foreach (var categoryString in categoriesStringArr)
            {
                Category category = context.Categories
                    .Include(c => c.Items)
                    .ThenInclude(oi => oi.OrderItems)
                    .Where(c => c.Name == categoryString)
                    .SingleOrDefault();

                categories.Add(category);
            }

            CategoryDto[] categoriesDto = categories.Select(c => new CategoryDto
            {
                Name = c.Name,
                MostPopularItem = c.Items
                .OrderByDescending(i => i.OrderItems.Sum(oi => (i.Price * oi.Quantity)))
                .Select(it => new ItemDto
                {
                    Name = it.Name,
                    TotalMade = it.Price * it.OrderItems.Sum(oi => oi.Quantity),
                    TimesSold = it.OrderItems.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(tm => tm.TotalMade)
                .ThenByDescending(ts => ts.TimesSold)
                .FirstOrDefault()
            })
            .ToArray();

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), categoriesDto, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            var result = sb.ToString();
            return result;
        }
    }
}