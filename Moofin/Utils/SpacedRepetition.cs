using System;

namespace Moofin.Core.Utils
{
    public static class SpacedRepetition
    {
        public static (int interval, double easinessFactor) Calculate(int repetitions, double easinessFactor, bool wasCorrect)
        {
            if (!wasCorrect)
            {
                easinessFactor = Math.Max(1.3, easinessFactor - 0.2);
                return (1, easinessFactor);
            }

            easinessFactor += 0.1 - (0.52) * (1 / easinessFactor);
            easinessFactor = Math.Max(1.3, Math.Min(2.5, easinessFactor));

            int interval;
            if (repetitions == 0) interval = 1;
            else if (repetitions == 1) interval = 3;
            else if (repetitions == 2) interval = 7;
            else interval = (int)Math.Round((repetitions - 1) * easinessFactor);

            return (interval, easinessFactor);
        }
    }
}