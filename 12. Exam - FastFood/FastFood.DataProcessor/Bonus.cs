namespace FastFood.DataProcessor
{
    using System.Linq;

    using Data;
    using Models;

    public static class Bonus
    {
        public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
        {
            Item item = context.Items
                .Where(i => i.Name == itemName)
                .SingleOrDefault();

            if (item == null)
            {
                return $"Item {itemName} not found!";
            }

            var oldPrice = item.Price;

            item.Price = newPrice;

            context.Update(item);
            context.SaveChanges();

            return $"{item.Name} Price updated from ${oldPrice:F2} to ${newPrice:F2}";
        }
    }
}
