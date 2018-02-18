namespace FastFood.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("Item")]
    public class OrderItemsDto
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}