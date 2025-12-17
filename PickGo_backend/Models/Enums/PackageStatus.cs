public enum PackageStatus
{
    Pending,           // created, not yet assigned
    Assigned,          // courier assigned but not picked up yet
    PickupInProgress,  // courier is picking up
    Delivered,
    Cancelled,
    Failed
}