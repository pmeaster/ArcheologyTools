using System.Collections.Generic;

namespace FractalSource.Mapping.Services.Poles
{
    internal static class PoleParallels
    {
        public static readonly IReadOnlyDictionary<string, double> Parallels = new SortedDictionary<string, double>
        {
            { nameof(ArcticCircle), ArcticCircle },
            { nameof(SeventyDegreesNorth), SeventyDegreesNorth },
            { nameof(SixtyDegreesNorth), SixtyDegreesNorth },
            { nameof(FortyDegreesNorth), FortyDegreesNorth },
            { nameof(TropicOfCancer), TropicOfCancer },
            { nameof(TenDegreesNorth), TenDegreesNorth },
            { nameof(Equator), Equator },
            { nameof(TenDegreesSouth), TenDegreesSouth },
            { nameof(TropicOfCapricorn), TropicOfCapricorn },
            { nameof(FortyDegreesSouth), FortyDegreesSouth },
            { nameof(SixtyDegreesSouth), SixtyDegreesSouth },
            { nameof(SeventyDegreesSouth), SeventyDegreesSouth },
            { nameof(AntarcticCircle), AntarcticCircle }
        };

        public const double GreatCircle = 10000000;

        public const double ArcticCircle = 2600000;

        public const double SeventyDegreesNorth = 1941000;

        public const double SixtyDegreesNorth = 3333000;

        public const double FortyDegreesNorth = 5575000;

        public const double TropicOfCancer = 7400000;

        public const double TenDegreesNorth = 8700000;

        public const double Equator = GreatCircle;

        public const double TenDegreesSouth = 11300000;

        public const double TropicOfCapricorn = 12600000;

        public const double FortyDegreesSouth = 14425000;

        public const double SixtyDegreesSouth = 16667000;

        public const double SeventyDegreesSouth = 15460000;

        public const double AntarcticCircle = 17400000;
    }
}