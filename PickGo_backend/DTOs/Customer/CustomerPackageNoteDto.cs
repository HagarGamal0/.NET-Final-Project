namespace PickGo_backend.DTOs.Customer
{
    public class CustomerPackageNoteDto
    {
        public int PackageId { get; set; }       // The package to update
        public string Notes { get; set; } = null!; // Delivery note from customer
    }
}
