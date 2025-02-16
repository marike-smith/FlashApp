namespace LulaPay.Domain.Entities.Buyers
{
    public sealed record BuyerId(Guid Value)
    {
        public static BuyerId New() => new(Guid.NewGuid());
    }
}