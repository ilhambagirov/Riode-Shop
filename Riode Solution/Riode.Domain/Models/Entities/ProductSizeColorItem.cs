namespace Riode.Domain.Models.Entities
{
    public class ProductSizeColorItem : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public int ColorId { get; set; }
        public virtual Colors Color { get; set; }
        public int SizeId { get; set; }
        public virtual Size Size { get; set; }
    }
}
