namespace FastFood.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using FastFood.Data;
    using FastFood.DataProcessor.Dto.Import;
    using FastFood.Models;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            EmployeeDto[] employeesDto = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            List<Employee> validEmployees = new List<Employee>();
            List<Position> validPositions = new List<Position>();

            foreach (var employeeDto in employeesDto)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position employeePosition = context.Positions
                    .Where(p => p.Name == employeeDto.Position)
                    .SingleOrDefault();

                if (employeePosition == null)
                {
                    employeePosition = new Position
                    {
                        Name = employeeDto.Position
                    };

                    context.Positions.Add(employeePosition);
                    context.SaveChanges();
                }

                Employee employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = employeePosition
                };

                validEmployees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, employeeDto.Name));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

            string result = sb.ToString();
            return result;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ItemDto[] itemsDto = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            List<Item> validItems = new List<Item>();

            foreach (var itemDto in itemsDto)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool isItemExists = validItems.Any(i => i.Name == itemDto.Name);

                if (isItemExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = context.Categories
                    .Where(c => c.Name == itemDto.Category)
                    .SingleOrDefault();

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDto.Category
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                }

                Item item = new Item
                {
                    Name = itemDto.Name,
                    Category = category,
                    Price = itemDto.Price
                };

                validItems.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
            }

            context.AddRange(validItems);
            context.SaveChanges();

            string result = sb.ToString();
            return result;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            OrderDto[] deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            List<Order> orders = new List<Order>();
            StringBuilder sb = new StringBuilder();

            foreach (var orderDto in deserializedOrders)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Employee employee = context.Employees
                    .Where(e => e.Name == orderDto.Employee)
                    .SingleOrDefault();
                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool isItemsExists = orderDto.Items.All(i => context.Items.Any(di => di.Name == i.Name));
                if (!isItemsExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime dateTime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                List<OrderItem> orderedItems = new List<OrderItem>();

                foreach (var itemDto in orderDto.Items)
                {
                    Item item = context.Items
                        .Where(i => i.Name == itemDto.Name)
                        .SingleOrDefault();

                    OrderItem orderItem = new OrderItem
                    {
                        Item = item,
                        Quantity = itemDto.Quantity
                    };

                    orderedItems.Add(orderItem);
                }

                Order order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = dateTime,
                    Type = orderDto.Type,
                    OrderItems = orderedItems
                };

                sb.AppendLine($"Order for {orderDto.Customer} on {dateTime.ToString("dd/MM/yyyy HH:mm")} added");
                orders.Add(order);
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();

            string result = sb.ToString();
            return result;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);

            return isValid;
        }
    }
}