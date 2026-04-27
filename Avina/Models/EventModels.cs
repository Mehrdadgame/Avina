namespace Avina.Models;

/// <summary>
/// ایونت حضوری: «هر ایونت ادامهٔ یک مسیر است.»
/// طبق سند: ایونت نباید جدا از اپ باشد؛ کاربر باید در مسیر مشخصی به سطح خاص رسیده باشد تا دعوت شود.
/// </summary>
public enum EventStatus
{
    Draft = 1,
    Published = 2,
    RegistrationOpen = 3,
    RegistrationClosed = 4,
    Completed = 5,
    Cancelled = 6
}

public enum EventRegistrationStatus
{
    Pending = 1,
    Confirmed = 2,
    Attended = 3,
    NoShow = 4,
    Cancelled = 5,
    Rejected = 6
}

public class GrowthEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int Capacity { get; set; } = 30;

    /// <summary>
    /// مسیر مرتبط (مثلاً مسیر «خلاق» ⇒ کارگاه داستان‌سازی).
    /// </summary>
    public int? RelatedPathId { get; set; }
    public virtual GrowthPath? RelatedPath { get; set; }

    /// <summary>
    /// حداقل سطح مهارت یا XP موردنیاز برای ثبت‌نام.
    /// </summary>
    public int RequiredLevel { get; set; } = 1;

    /// <summary>
    /// هزینه به کوین (اختیاری). اگر صفر باشد، رایگان است.
    /// </summary>
    public int CoinCost { get; set; } = 0;

    /// <summary>
    /// آیا نیاز به تأیید ادمین برای ثبت‌نام دارد.
    /// </summary>
    public bool RequiresAdminApproval { get; set; } = false;

    /// <summary>
    /// XP و کوین جایزه پس از حضور (Online → Real → Online).
    /// </summary>
    public int RewardXP { get; set; } = 50;
    public int RewardCoin { get; set; } = 25;

    /// <summary>
    /// مأموریت پس از ایونت (اختیاری) — کاربر بعد از حضور باید این را تکمیل کند تا چرخه بسته شود.
    /// </summary>
    public int? PostEventMissionId { get; set; }
    public virtual Mission? PostEventMission { get; set; }

    public string? CoverImageUrl { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
}

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public EventRegistrationStatus Status { get; set; } = EventRegistrationStatus.Pending;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? AttendedAt { get; set; }
    public string? Notes { get; set; }
    /// <summary>
    /// آیا جوایز XP/کوین حضور پرداخت شده.
    /// </summary>
    public bool RewardGranted { get; set; }

    public virtual GrowthEvent Event { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

// ====== DTOs ======

public sealed class GrowthEventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int Capacity { get; set; }
    public int RegisteredCount { get; set; }
    public string? RelatedPathTitle { get; set; }
    public int RequiredLevel { get; set; }
    public int CoinCost { get; set; }
    public int RewardXP { get; set; }
    public int RewardCoin { get; set; }
    public string? CoverImageUrl { get; set; }
    public EventStatus Status { get; set; }
    public bool UserCanRegister { get; set; }
    public string? UserCannotRegisterReason { get; set; }
    public EventRegistrationStatus? UserRegistrationStatus { get; set; }
}

public sealed class GrowthEventUpsertRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int Capacity { get; set; } = 30;
    public int? RelatedPathId { get; set; }
    public int RequiredLevel { get; set; } = 1;
    public int CoinCost { get; set; } = 0;
    public bool RequiresAdminApproval { get; set; } = false;
    public int RewardXP { get; set; } = 50;
    public int RewardCoin { get; set; } = 25;
    public int? PostEventMissionId { get; set; }
    public string? CoverImageUrl { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
}

public sealed class EventRegistrationRequest
{
    public int EventId { get; set; }
    public int UserId { get; set; }
}
