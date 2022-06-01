namespace CRUD.Models.ViewModel
{
    public class ProductUserVm
    {
        public ProductUserVm()
        {
            ProductList = new List<Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList <Product> ProductList { get; set; }
    }
}
