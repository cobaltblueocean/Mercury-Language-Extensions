using NodaTime;

namespace Mercury.Language.Time
{
    public interface ITemporalUnit
    {
        Duration Duration { get; }
        bool IsDateBased { get; }
        bool IsDurationEstimated { get; }
        bool IsTimeBased { get; }
        string Name { get; }

        dynamic AddTo(Temporal dateTime, long periodToAdd);
        bool IsSupportedBy(Temporal temporal);
    }
}