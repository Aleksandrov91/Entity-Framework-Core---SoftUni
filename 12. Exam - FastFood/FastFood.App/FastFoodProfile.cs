namespace FastFood.App
{
    using AutoMapper;
    using DataProcessor.Dto.Export;
    using Models;

    public class FastFoodProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public FastFoodProfile()
        {
            this.CreateMap<Item, ItemDto>();
        }
    }
}
