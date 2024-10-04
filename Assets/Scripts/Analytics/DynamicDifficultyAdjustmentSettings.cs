namespace Analytics
{
    public static class DynamicDifficultyAdjustmentSettings
    {
        // Weight settings
        public const float InitialWeight = 50f;
        public const float WeightIncrement = 5f;
        public const float WeightDecrement = 5f;
        public const float MaxWeight = 99f;
        public const float MinWeight = 0f;
        public const float LevelUpThreshold = 45f;
        
        // Time weight settings
        public const float TimeWeightFactor = 0.1f;
        public const float TimeWeightIncrement = 0.1f;
        public const float TimeWeightDecrement = 0.1f;
        
        // Composite weight settings
        
        // Dynamic Difficulty settings
    }
}