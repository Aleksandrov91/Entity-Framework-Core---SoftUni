namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Models.Enums;

    [XmlType("Order")]
    public class OrderDto
    {
        [Required]
        public string Customer { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Employee { get; set; }

        [Required]
        public string DateTime { get; set; }

        [Required]
        public OrderType Type { get; set; }

        public OrderItemsDto[] Items { get; set; }
    }
}
