public enum PackageStatus
{
    Pending,           // created, not assigned
    Assigned,          // assigned to courier
    PickupInProgress,  // picked from supplier
    OutForDelivery,    // going to customer
    Delivered,
    Cancelled,
    Failed
}