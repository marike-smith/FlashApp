namespace FlashApp.Domain.Entities.Abstractions
{
    public interface ISoftDelete
    {
        DateTime? DeletedDate { get; set; }
        Guid? DeletedBy { get; set; }
    }
}
