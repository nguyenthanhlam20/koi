namespace KoiVetenary.MVCWebApp.Models
{
    public class ServiceListViewModel
    {
        public IEnumerable<Data.Models.Service> Services { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
