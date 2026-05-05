namespace Avina.Models;

public class DailyChallenge
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string BorderColor { get; set; } = "#aac7ff";
    public int RewardPoints { get; set; } = 20;
    public int PassingPercentage { get; set; } = 50;
    public int TimeLimitMinutes { get; set; } = 20;
    public bool IsActive { get; set; } = true;
    public DateTime PublishAt { get; set; } = DateTime.UtcNow;

    public ICollection<DailyChallengeQuestion> Questions { get; set; } = new List<DailyChallengeQuestion>();
    public ICollection<UserDailyChallengeAttempt> Attempts { get; set; } = new List<UserDailyChallengeAttempt>();
}

public class DailyChallengeQuestion
{
    public int Id { get; set; }
    public int DailyChallengeId { get; set; }
    public int Order { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = "A";
    public int Points { get; set; } = 1;

    public DailyChallenge DailyChallenge { get; set; } = null!;
}

public class UserDailyChallengeAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DailyChallengeId { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public int ScorePercent { get; set; }
    public int RewardEarned { get; set; }
    public bool Passed { get; set; }
    public int AttemptNumber { get; set; } = 1;
    public DateTime TakenAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public DailyChallenge DailyChallenge { get; set; } = null!;
}
