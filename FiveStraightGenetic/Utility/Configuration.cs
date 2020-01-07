namespace FiveStraightGenetic.Utility
{
    public static class Configuration
    {

        // Chromosome
        public const int _MinimumNumberOfMutations = 1;

        public const int _MaxNumberOfMutations = 6;

        public const int _NumberOfChromosomesInGeneration = 30;

        public const int _NumberOfGamesPlayedPerChromosomePerGeneration = 10;

        public const double _MutationRate = .09;

        // Location Valuator
        public const int _LocationValueBump = 10;

        // Play Valuator
        public const int _HandSize = 4;

        public const int _LargeCardPreference = 50;

        // Generation creation constants
        public const int _NumOfTopChromosomesToCopyOver = 10;

        public const int _NumOfCrossOverForFirstPlace = 5;

        public const int _NumOfCrossOverForSecondPlace = 4;

        public const int _NumOfCrossOverForThirdPlace = 3;

        public const int _NumOfCrossOverForFourthPlace = 2;

        public const int _NumOfCrossOverForFifthPlace = 1;

        public const int _NumOfRandomNewChromosomes = 5;

        public static int GenerationSize()
        {
            return _NumOfTopChromosomesToCopyOver
                + _NumOfCrossOverForFirstPlace
                + _NumOfCrossOverForSecondPlace
                + _NumOfCrossOverForThirdPlace
                + _NumOfCrossOverForFourthPlace
                + _NumOfCrossOverForFifthPlace
                + _NumOfRandomNewChromosomes;
        }
    }
}
