using NodaTime;

namespace Mercury.Language.Time
{
    public interface ITemporalAdjuster
    {
        Temporal AdjustInto(Temporal temporal);
    }
}