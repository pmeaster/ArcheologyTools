using System;
using System.Globalization;

namespace FractalSource.Mapping.Projection.Utm
{
    /// <summary>
    ///     The globe is partitioned into Grids by the UTM projection.
    ///     This structure represents such a grid.
    /// </summary>
    public struct UtmGrid : IEquatable<UtmGrid>
    {
        private const double Delta = 1e-12;
        private const int MinZone = 1;
        private const int MaxZone = 60;
        private const int MinBand = 0;
        private const int MaxBand = 19;
        private const string BandChars = "CDEFGHJKLMNPQRSTUVWX";

        /// <summary>
        ///     The number of (horizontal) zones
        /// </summary>
        public const int NumberOfZones = MaxZone;

        /// <summary>
        ///     The number of (vertical) bands.
        /// </summary>
        public const int NumberOfBands = 1 + MaxBand;

        /// <summary>
        ///     The theoretical number of UTM Grids on the globe. Actually there are
        ///     fewer ones, because there are exceptions.  <seealso cref="NumberOfUsedGrids"/>
        /// </summary>
        public const int NumberOfGrids = MaxZone * (1 + MaxBand);

        /// <summary>
        ///     The number of UTM Grids really used. There are 3 potential combinations
        ///     in the X-Band that are an exception and not used (32X, 34X, 36X)
        /// </summary>
        public const int NumberOfUsedGrids = NumberOfGrids - 3;

        /*  Please note for the next two constant that they rely on the implement<tion of a
            default conversion from type "double" to type "Angle", assuming that the double
            is a degree value.
         */
        /// <summary>
        ///     Horizontal step width for the Grids
        /// </summary>
        public static readonly Angle XStep = 6.0;

        /// <summary>
        ///     Vertical step width for the Grids
        /// </summary>
        public static readonly Angle YStep = 8.0;

        private int _band;
        private GeoCoordinates _llCoordinates;
        private double _mapHeight;
        private double _mapWidth;
        private UtmCoordinate _origin;
        private UtmCoordinate _southernWest;
        private int _zone;

        private UtmGrid(UtmProjection projection)
        {
            Projection = projection ?? throw new ArgumentNullException(nameof(projection), "The projection argument must not be null.");
            _origin = null;
            _southernWest = null;
            _mapHeight = 0.0;
            _mapWidth = 0.0;
            Width = XStep;
            Height = YStep;
            _zone = 0;
            _band = 0;
            _llCoordinates = new GeoCoordinates();
        }

        /// <summary>
        ///     Instantiate a new UTM Grid object
        /// </summary>
        /// <param name="projection">The UTM projection this grid belongs to</param>
        /// <param name="zone">The zone of the grid</param>
        /// <param name="band">The band of the grid</param>
        /// <exception cref="ArgumentOutOfRangeException">Throw if zone or band are invalid</exception>
        /// <exception cref="ArgumentNullException">Thrown if the projection is null</exception>
        public UtmGrid(UtmProjection projection, int zone, int band) : this(projection)
        {
            SetZoneAndBandInConstructor(zone, band);
        }

        /// <summary>
        ///     Instantiate a new UTM Grid object
        /// </summary>
        /// <param name="projection">The UTM projection this grid belongs to</param>
        /// <param name="zone">The zone of the grid</param>
        /// <param name="band">The band of the grid</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if zone or band are requested</exception>
        /// <exception cref="ArgumentNullException">Thrown if the projection is null</exception>
        public UtmGrid(UtmProjection projection, int zone, char band)
            : this(projection, zone, BandChars.IndexOf(band))
        { }

        /// <summary>
        ///     Instantiate a grid by its ordinal number.
        /// </summary>
        /// <param name="projection">The UTM projection this Grid belongs to</param>
        /// <param name="ordinal">The unique ordinal number of the grid</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the ordinal number is invalid</exception>
        public UtmGrid(UtmProjection projection, int ordinal) : this(projection)
        {
            if (ordinal is < 0 or >= NumberOfGrids)
                throw new ArgumentOutOfRangeException(nameof(ordinal), "The ordinal number of the Grid is invalid.");

            SetZoneAndBandInConstructor(1 + ordinal / NumberOfBands, ordinal % NumberOfBands);
        }

        /// <summary>
        ///     The UTM Grid for a given latitude/longitude
        /// </summary>
        /// <param name="projection">The projection to use</param>
        /// <param name="coordinates">Latitude/Longitude of the location</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the latitude is out of the limits for the UTM projection</exception>
        public UtmGrid(UtmProjection projection, GeoCoordinates coordinates) : this(projection)
        {
            if (coordinates.Latitude < projection.MinLatitude || coordinates.Latitude > projection.MaxLatitude)
            {
                throw new ArgumentOutOfRangeException(nameof(coordinates),
                    "The UTM projection only allows latitudes from -80° to +84°");
            }

            var longitude = MercatorProjection.NormalizeLongitude(coordinates.Longitude).Degrees + 180.0;
            var latitude = projection.NormalizeLatitude(coordinates.Latitude);
            var band = (int)((latitude - projection.MinLatitude).Degrees / YStep.Degrees);

            if (band == NumberOfBands)
            {
                var northernLimit = projection.MinLatitude + NumberOfBands * YStep;
                if (latitude >= northernLimit && latitude <= projection.MaxLatitude)
                    band--;
            }
            var zone = (int)(longitude / XStep.Degrees) + 1;
            SetZoneAndBandInConstructor(zone, band, true);

            if (_zone == 31 && Band == 'V')
            {
                var delta = coordinates.Longitude.Degrees - _llCoordinates.Longitude.Degrees - Width.Degrees;
                if (Math.Sign(delta) != -1)
                {
                    Zone = _zone + 1;
                }
            }
            else if (Band == 'X')
            {
                if (_zone is 32 or 34 or 36)
                {
                    var delta = coordinates.Longitude.Degrees - CenterMeridian.Degrees;
                    if (Math.Sign(delta) == -1)
                        Zone = _zone - 1;
                    else
                        Zone = _zone + 1;
                }
            }
        }

        /// <summary>
        ///     The projection this grid belongs to
        /// </summary>
        public UtmProjection Projection { get; }

        /// <summary>
        ///     The UTM coordinates of the left corner of the wider latitude of the zone
        ///     which is the latitude closer to the equator.
        /// </summary>
        public UtmCoordinate Origin
        {
            get
            {
                if (null == _origin)
                    ComputeFlatSize();
                return _origin;
            }
        }

        /// <summary>
        ///     The UTM coordinates of the most western corner of the w latitude of the zone
        ///     which is the latitude most southern
        /// </summary>
        public UtmCoordinate SouthernWest
        {
            get
            {
                if (null == _southernWest)
                    ComputeFlatSize();
                return _southernWest;
            }
        }

        /// <summary>
        ///     The width of this grid (in meters)
        /// </summary>
        public double MapWidth
        {
            get
            {
                if (_origin == null)
                    ComputeFlatSize();
                return _mapWidth;
            }
        }

        /// <summary>
        ///     The height of this grid (in meters)
        /// </summary>
        public double MapHeight
        {
            get
            {
                if (_origin == null)
                    ComputeFlatSize();
                return _mapHeight;
            }
        }

        /// <summary>
        ///     The UTM zone the point belongs to.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid UTM zone number is specified</exception>
        public int Zone
        {
            get => _zone;
            set
            {
                if (value is < MinZone or > MaxZone)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid UTM Zone specified.");
                }

                _zone = value;
                ComputeSizes();
                if (_origin != null)
                    ComputeFlatSize();
            }
        }

        /// <summary>
        ///     Get the numeric representation of the band (0 based)
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid UTM band number is specified</exception>
        public int BandNr
        {
            get => _band;
            private set
            {
                if (value is < MinBand or > MaxBand)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid UTMBand specified.");
                }

                _band = value;
                ComputeSizes();
                if (_origin != null)
                    ComputeFlatSize();
            }
        }

        /// <summary>
        ///     The UTM band the point belongs to.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If the band character is out of its limits</exception>
        public char Band
        {
            get => BandChars[_band];
            set => BandNr = BandChars.IndexOf(value);
        }

        /// <summary>
        ///     Width of the Grid (as an Angle)
        /// </summary>
        public Angle Width { get; private set; }

        /// <summary>
        ///     Height of the Grid (as an Angle)
        /// </summary>
        public Angle Height { get; private set; }

        /// <summary>
        ///     Unique numbering of the Grids. The most western, most southern
        ///     gets #0. Then we go north continue counting, when reaching the
        ///     northern limit we go to the lowest south of the next zone to the
        ///     east of the current one.
        /// </summary>
        public int Ordinal => (_zone - 1) * NumberOfBands + _band;

        /// <summary>
        ///     Return true is this is a northern band
        /// </summary>
        public bool IsNorthern => (_band >= NumberOfBands / 2);

        private void ComputeFlatSize()
        {
            UtmCoordinate other;
            UtmCoordinate right;

            _southernWest = (UtmCoordinate)Projection.ToEuclidean(LowerLeftCorner);
            if (IsNorthern)
            {
                _origin = _southernWest;
                other = (UtmCoordinate)Projection.ToEuclidean(UpperLeftCorner);
                right = (UtmCoordinate)Projection.ToEuclidean(LowerRightCorner);
            }
            else
            {
                _origin = (UtmCoordinate)Projection.ToEuclidean(UpperLeftCorner);
                other = _southernWest;
                right = (UtmCoordinate)Projection.ToEuclidean(UpperRightCorner);
            }
            _mapHeight = Math.Abs(_origin.Y - other.Y);
            _mapWidth = Math.Abs(_origin.X - right.X);
        }

        /// <summary>
        ///     Check whether or not an Ordinal number is valid
        /// </summary>
        /// <param name="ordinal">The ordinal to check</param>
        /// <returns>True if this is a valid ordinal number</returns>
        public static bool IsValidOrdinal(int ordinal)
        {
            if (ordinal is < 0 or >= NumberOfGrids)
                return false;

            var zone = 1 + ordinal / NumberOfBands;
            var band = ordinal % NumberOfBands;
            return band != MaxBand || (zone != 32 && zone != 34 && zone != 36);
        }

        private void ComputeSizes()
        {
            // Initial position of the grid
            _llCoordinates = new GeoCoordinates(
                _band * YStep + Projection.MinLatitude,
                (_zone - 1) * XStep + MercatorProjection.MinLongitude);

            if (_band == MaxBand)
                Height = YStep.Degrees + 4.0;

            var latitude = _llCoordinates.Latitude;
            var longitude = _llCoordinates.Longitude;
            
            switch (_zone)
            {
                case 32 when Band == 'V':
                    Width += 3.0;
                    longitude -= 3.0;
                    break;

                case 31 when Band == 'V':
                    Width -= 3.0;
                    break;

                default:
                    {
                        if (_band == MaxBand)
                        {
                            if (_zone is 31 or 37)
                            {
                                Width += 3.0;
                                if (_zone == 37)
                                    longitude -= 3.0;
                            }
                            else if (_zone is 33 or 35)
                            {
                                Width += 6.0;
                                longitude -= 3.0;
                            }
                        }

                        break;
                    }
            }

            _llCoordinates = new GeoCoordinates(latitude, longitude);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void SetZoneAndBandInConstructor(int zone, int band, bool noGridException = false)
        {
            if (!noGridException && (band == MaxBand) && zone is 32 or 34 or 36)
            {
                throw new ArgumentOutOfRangeException(nameof(band),
                    "The UTM grids 32X, 34X, and 36X are not assigned.");
            }

            if (zone is < MinZone or > MaxZone)
            {
                throw new ArgumentOutOfRangeException(nameof(zone),
                    "Invalid UTM Zone specified");
            }

            _zone = zone;
            if (band is < MinBand or > MaxBand)
            {
                throw new ArgumentOutOfRangeException(nameof(band), 
                    "Invalid UTMBand specified.");
            }

            _band = band;
            ComputeSizes();
        }

        /// <summary>
        ///     Check whether a point is in the grid
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <returns>True if the point is inside</returns>
        public bool IsInside(GeoCoordinates point)
        {
            if (point.Longitude < LowerLeftCorner.Longitude || point.Longitude > LowerRightCorner.Longitude)
                return false;
            return point.Latitude >= LowerLeftCorner.Latitude && point.Latitude <= UpperLeftCorner.Latitude;
        }

        /// <summary>
        ///     Compare the grid to another grid for equality.
        /// </summary>
        /// <param name="other">The other grid</param>
        /// <returns>True if they are equal</returns>
        public bool Equals(UtmGrid other)
        {
            return (other.Projection.Equals(Projection) &&
                    (_band == other._band) && (_zone == other._zone));
        }

        /// <summary>
        ///     Compare these coordinates to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is UtmGrid))
                return false;
            var other = (UtmGrid)obj;
            return Equals(other);
        }

        /// <summary>
        ///     Test two Grids for equality
        /// </summary>
        /// <param name="lhs">The first grid</param>
        /// <param name="rhs">The second grid</param>
        /// <returns>True if the first equals the second grid</returns>
        public static bool operator ==(UtmGrid lhs, UtmGrid rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        ///     Test two Grids for inequality
        /// </summary>
        /// <param name="lhs">The first grid</param>
        /// <param name="rhs">The second grid</param>
        /// <returns>True if the first is not equal to the second grid</returns>
        public static bool operator !=(UtmGrid lhs, UtmGrid rhs)
        {
            return !(lhs.Equals(rhs));
        }

        /// <summary>
        ///     The Hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Ordinal;
        }

        /// <summary>
        ///     The culture invariant string representation of the Grid
        /// </summary>
        /// <returns>ZThe name of the Grid</returns>
        public override string ToString()
        {
            return _zone.ToString(NumberFormatInfo.InvariantInfo) + Band;
        }

        /// <summary>
        ///     The latitude/longitude of the lower left corner of this grid
        /// </summary>
        public GeoCoordinates LowerLeftCorner => _llCoordinates;

        /// <summary>
        ///     The latitude/longitude of the upper right corner of this grid
        /// </summary>
        public GeoCoordinates UpperRightCorner =>
            new(
                _llCoordinates.Latitude + Height - Delta,
                _llCoordinates.Longitude + Width - Delta);

        /// <summary>
        ///     The latitude/longitude of the upper left corner of this grid
        /// </summary>
        public GeoCoordinates UpperLeftCorner =>
            new(
                _llCoordinates.Latitude + Height - Delta,
                _llCoordinates.Longitude);

        /// <summary>
        ///     The latitude/longitude of the lower right corner of this grid
        /// </summary>
        public GeoCoordinates LowerRightCorner =>
            new(
                _llCoordinates.Latitude,
                _llCoordinates.Longitude + Width - Delta);

        /// <summary>
        ///     The longitude of the center of this Grid
        /// </summary>
        public Angle CenterMeridian => _llCoordinates.Longitude + Width * 0.5;

        /// <summary>
        ///     The western neighbor of the grid
        /// </summary>
        public UtmGrid West
        {
            get
            {
                var newZone = _zone - 1;
                if (newZone < MinZone)
                {
                    newZone = MaxZone;
                }

                if (_band == MaxBand && newZone is 32 or 34 or 36)
                {
                    newZone--;
                }

                return new UtmGrid(Projection, newZone, _band);
            }
        }

        /// <summary>
        ///     The eastern neighbor of the grid
        /// </summary>
        public UtmGrid East
        {
            get
            {
                var newZone = _zone + 1;
                if (newZone > MaxZone)
                {
                    newZone = MinZone;
                }

                if (_band == MaxBand && newZone is 32 or 34 or 36)
                {
                    newZone++;
                }

                return new UtmGrid(Projection, newZone, _band);
            }
        }

        /// <summary>
        ///     The northern neighbor of the grid
        /// </summary>
        /// <exception cref="Exception">If there is no northern neighbor</exception>
        public UtmGrid North
        {
            get
            {
                if ((Band == 'U' && _zone == 31) ||
                    ((Band == 'W') && _zone is 32 or 34 or 36))
                {
                    throw new Exception("The UTM grid has no unique northern neighbor (exceptional grids in northern hemisphere).");
                }

                var newBand = _band + 1;
                if (newBand > MaxBand)
                {
                    throw new Exception("The grid is in the most northern band, there is no northern neighbor.");
                }

                return new UtmGrid(Projection, _zone, newBand);
            }
        }

        /// <summary>
        ///     The southern neighbor of the grid
        /// </summary>
        /// <exception cref="Exception">If there is no southern neighbor</exception>
        public UtmGrid South
        {
            get
            {
                if ((Band == 'W' && _zone == 31) || ((Band == 'X') && _zone >= 31 && _zone <= 37))
                {
                    throw new Exception("The UTM grid has no unique southern neighbor (exceptional grids in northern hemisphere).");
                }

                var newBand = _band - 1;
                if (newBand < MinBand)
                {
                    throw new Exception("The grid is in the most southern band, there is no southern neighbor.");
                }

                return new UtmGrid(Projection, _zone, newBand);
            }
        }
    }
}