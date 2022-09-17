using System;

namespace Katoot
{

    public partial class Info
    {
        public Guid Uuid { get; set; }
        public string Language { get; set; }
        public Guid Creator { get; set; }
        public string CreatorUsername { get; set; }
        public long CompatibilityLevel { get; set; }
        public string CreatorPrimaryUsage { get; set; }
        public Guid FolderId { get; set; }
        public long Visibility { get; set; }
        public string Audience { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string QuizType { get; set; }
        public Uri Cover { get; set; }
        public Metadata CoverMetadata { get; set; }
        public LobbyVideo LobbyVideo { get; set; }
        public Question[] Questions { get; set; }
        public MetadataClass Metadata { get; set; }
        public string Resources { get; set; }
        public string Slug { get; set; }
        public object[] InventoryItemIds { get; set; }
        public bool IsValid { get; set; }
        public string Type { get; set; }
        public long Created { get; set; }
        public long Modified { get; set; }
    }

    public partial class Metadata
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public string Resources { get; set; }
        public string AltText { get; set; }
    }
    

    public partial class LobbyVideo
    {
        public Youtube Youtube { get; set; }
    }

    public partial class Youtube
    {
        public string Id { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string Service { get; set; }
        public string FullUrl { get; set; }
    }

    public partial class MetadataClass
    {
        public string Resolution { get; set; }
        public Moderation Moderation { get; set; }
        public Access Access { get; set; }
        public bool DuplicationProtection { get; set; }
        public LastEdit LastEdit { get; set; }
    }

    public partial class Access
    {
        public Guid[] GroupRead { get; set; }
        public object[] Features { get; set; }
    }

    public partial class LastEdit
    {
        public Guid EditorUserId { get; set; }
        public string EditorUsername { get; set; }
        public long EditTimestamp { get; set; }
    }

    public partial class Moderation
    {
        public long FlaggedTimestamp { get; set; }
        public long TimestampResolution { get; set; }
        public string Resolution { get; set; }
    }

    public partial class Question
    {
        public string Type { get; set; }
        public string QuestionQuestion { get; set; }
        public long Time { get; set; }
        public bool Points { get; set; }
        public long PointsMultiplier { get; set; }
        public Choice[] Choices { get; set; }
        public string Layout { get; set; }
        public Uri Image { get; set; }
        public Metadata ImageMetadata { get; set; }
        public string Resources { get; set; }
        public Youtube Video { get; set; }
        public long QuestionFormat { get; set; }
        public object[] Media { get; set; }
    }

    public partial class Choice
    {
        public string Answer { get; set; }
        public bool Correct { get; set; }
    }
}
