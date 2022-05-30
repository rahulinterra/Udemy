namespace CRUD.Models.ViewModel
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new Product();

        }
        public Product Product { get; set; }

        public bool ExitsInCart { get; set; }
    }
}
