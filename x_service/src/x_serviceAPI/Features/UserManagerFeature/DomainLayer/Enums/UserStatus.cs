namespace Domain.UserManagement.Enums;

public enum UserStatus
{
    Active,
    Inactive,
    Pending, //(e.g., AuthSuccessEvent, email verification)
    Suspended, //(e.g., due to violations of terms of service)
    Deleted //(soft delete)
}