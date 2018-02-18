namespace FastFood.DataProcessor.Dto.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EmployeeOrdersDto
    {
        public string Name { get; set; }

        public ICollection<OrderDto> Orders { get; set; }

        public decimal TotalMade => this.Orders.Sum(o => o.TotalPrice);
    }
}
