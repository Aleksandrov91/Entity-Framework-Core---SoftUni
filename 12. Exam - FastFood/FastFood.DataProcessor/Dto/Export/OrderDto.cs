namespace FastFood.DataProcessor.Dto.Export
{
    using System.Collections.Generic;
    using System.Linq;

    public class OrderDto
    {
        public string Customer { get; set; }

        public List<OrderItemDto> Items { get; set; }

        public decimal TotalPrice => this.Items.Sum(o => (o.Price * o.Quantity));
    }
}
