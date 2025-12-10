public enum RequestStatus
{
    Pending,    // لسه متطلبش كورير
    Assigned,   // اتعيّن لكورير (في انتظار ردّه)
    Accepted,   // الكورير قبِل
    Rejected,   // الكورير رفَض
    InProgress, // استلم الشحنة
    Delivered
}
