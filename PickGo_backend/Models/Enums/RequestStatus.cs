public enum RequestStatus
{
    Pending,            // created, not assigned
    Assigned,           // supplier manually assigned courier
    Accepted,           // courier accepted the order
    PickupInProgress,   // courier picking up
    Delivered,
    Cancelled
}
