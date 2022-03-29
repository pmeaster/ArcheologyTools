using System;
using System.Collections.Generic;
using FractalSource.Mapping.Geodesy;
using FractalSource.Mapping.Projection.Utm;

namespace FractalSource.Mapping.Projection
{
    /// <summary>
    ///     This class overlays the globe with a mesh of squares.
    ///     The algorithm is based on subdividing the UTM grids into
    ///     finer cell structures, so the coverage is for latitudes
    ///     between 80° South and 84° North.
    /// </summary>
    public class GlobalMesh
    {
        private const int MinimumMeshSize = 1;

        // The maximum number of cells required for any UTM Grid 

        // The maximum vertical number of cells in any UTM Grid
        private readonly long _maxVerticalMeshes;

        /// <summary>
        ///     Instantiate the Mesh with the given nuber of meters as the size
        ///     of the mesh squares. We do not support squares less than 1m.
        ///     Please note that the actual mesh size used is a derived value
        ///     that approximates the requested mesh size in order to provide
        ///     better computational efficiency.
        /// </summary>
        /// <param name="meshSizeInMeters">The size of the squares in meter. The default value is 1000m.</param>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh size is specified</exception>
        public GlobalMesh(int meshSizeInMeters = 1000)
        {
            if (meshSizeInMeters < MinimumMeshSize)
            {
                throw new ArgumentOutOfRangeException(nameof(meshSizeInMeters),
                    "The mesh size must be at least 1 meter.");
            }

            MeshSize = meshSizeInMeters;
            var dblSquareSize = (double)meshSizeInMeters;
            var maxWidth = double.MinValue;
            var maxHeight = double.MinValue;

            for (var ord = 0; ord < UtmGrid.NumberOfGrids; ord++)
            {
                if (!UtmGrid.IsValidOrdinal(ord)) continue;
                var theGrid = new UtmGrid(Projection, ord);
                maxWidth = Math.Max(maxWidth, theGrid.MapWidth);
                maxHeight = Math.Max(maxHeight, theGrid.MapHeight);
            }

            var maxHorizontalMeshes =
                (long)Math.Round((maxWidth + dblSquareSize - 1.0) / dblSquareSize, MidpointRounding.AwayFromZero);

            _maxVerticalMeshes =
                (long)Math.Round((maxHeight + dblSquareSize - 1.0) / dblSquareSize, MidpointRounding.AwayFromZero);

            if (maxHorizontalMeshes < 2 || _maxVerticalMeshes < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHorizontalMeshes), 
                    "The mesh size is to big, it doesn't divide all UTMGrids into multiple cells.");

            }

            Count = maxHorizontalMeshes * _maxVerticalMeshes;
        }

        /// <summary>
        ///     The size of the mesh squares in meters. We only support full meters.
        /// </summary>
        public int MeshSize { get; private set; }

        /// <summary>
        ///     The UTM Projection for the Globe we cover with the mesh.
        /// </summary>
        public UtmProjection Projection { get; } = new();

        /// <summary>
        ///     The (maximum) total number of meshes used to cover an UTM Grid.
        ///     Individual Grids may actually be covered by fewer mesh-cells.
        /// </summary>
        public long Count { get; }

        /// <summary>
        ///     The maximum number of a mesh
        /// </summary>
        public long GlobalCount => Count * UtmGrid.NumberOfGrids;

        /// <summary>
        ///     Return the UtmGrid this mesh belongs to
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UtmGrid in which this mesh cell is located</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmGrid Grid(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / Count);
            return new UtmGrid(Projection, ord);
        }

        /// <summary>
        ///     Get the globally unique Mesh number of a coordinate
        /// </summary>
        /// <param name="coordinate">The UTM coordinate to convert</param>
        /// <returns>The mesh number to which the coordinate belongs</returns>
        public long MeshNumber(UtmCoordinate coordinate)
        {
            var relX = (long)Math.Round(coordinate.X - coordinate.Grid.SouthernWest.X, MidpointRounding.AwayFromZero) / MeshSize;
            var relY = (long)Math.Round(coordinate.Y - coordinate.Grid.SouthernWest.Y, MidpointRounding.AwayFromZero) / MeshSize;
            var res = coordinate.Grid.Ordinal * Count + relX * _maxVerticalMeshes + relY;
            return res;
        }

        /// <summary>
        ///     Get the globally unique Mesh number of a location given by
        ///     latitude and longitude.
        /// </summary>
        /// <param name="coordinate">The location to convert</param>
        /// <returns>The mesh number to which the location belongs</returns>
        public long MeshNumber(GeoCoordinates coordinate)
        {
            return MeshNumber((UtmCoordinate)Projection.ToEuclidean(coordinate));
        }

        /// <summary>
        ///     Get the globally unique Mesh number of a location given by
        ///     latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude (in degrees)</param>
        /// <param name="longitude">The longitude (in degrees)</param>
        /// <returns>The mesh number to which the location belongs</returns>
        public long MeshNumber(Angle latitude, Angle longitude)
        {
            return MeshNumber(new GeoCoordinates(latitude, longitude));
        }

        private void ValidateMeshNumber(long meshNumber)
        {
            if (meshNumber < 0 || meshNumber >= GlobalCount)
            {
                throw new ArgumentOutOfRangeException(nameof(meshNumber),
                    "The mesh number is invalid.");
            }
        }

        private void MeshOrigin(long meshNumber, out long relX, out long relY)
        {
            var local = meshNumber % (Count);
            relX = (local / _maxVerticalMeshes) * MeshSize;
            relY = (local % _maxVerticalMeshes) * MeshSize;
        }

        /// <summary>
        ///     Return the central coordinates of a Mesh given by its number.
        ///     Please note that this center is on the UTM map, but at the borders
        ///     of a grid this coordinate may actually overlap and belong to another
        ///     UTM grid. So if you convert them to a Latitude/Longitude and then back
        ///     to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber">The number of the mesh</param>
        /// <returns>The UTM coordinates of the center of the square</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmCoordinate CenterOf(long meshNumber)
        {
            var theGrid = Grid(meshNumber);
            MeshOrigin(meshNumber, out var relX, out var relY);

            var nx = relX + 0.5 * MeshSize;
            var ny = relY + 0.5 * MeshSize;

            return 
                new UtmCoordinate(theGrid, theGrid.SouthernWest.X + nx, theGrid.SouthernWest.Y + ny);
        }

        /// <summary>
        ///     Return the lower left corner coordinates of a Mesh given by its number.
        ///     Please note that this point is on the UTM map, but at the borders
        ///     of a grid this coordinate may actually overlap and belong to another
        ///     UTM grid. So if you convert them to a Latitude/Longitude and then back
        ///     to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the lower left corner of the Mesh</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmCoordinate LowerLeft(long meshNumber)
        {
            var theGrid = Grid(meshNumber);
            MeshOrigin(meshNumber, out var relX, out var relY);
            return new UtmCoordinate(theGrid, theGrid.SouthernWest.X + relX, theGrid.SouthernWest.Y + relY);
        }

        /// <summary>
        ///     Return the lower right corner coordinates of a Mesh given by its number.
        ///     Please note that this point is on the UTM map, but at the borders
        ///     of a grid this coordinate may actually overlap and belong to another
        ///     UTM grid. So if you convert them to a Latitude/Longitude and then back
        ///     to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the lower right corner of the Mesh</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmCoordinate LowerRight(long meshNumber)
        {
            var theGrid = Grid(meshNumber);
            MeshOrigin(meshNumber, out var relX, out var relY);
            relX += MeshSize;
            return new UtmCoordinate(theGrid, theGrid.SouthernWest.X + relX, theGrid.SouthernWest.Y + relY);
        }

        /// <summary>
        ///     Return the upper left corner coordinates of a Mesh given by its number.
        ///     Please note that this point is on the UTM map, but at the borders
        ///     of a grid this coordinate may actually overlap and belong to another
        ///     UTM grid. So if you convert them to a Latitude/Longitude and then back
        ///     to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the upper left corner of the Mesh</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmCoordinate UpperLeft(long meshNumber)
        {
            var theGrid = Grid(meshNumber);
            MeshOrigin(meshNumber, out var relX, out var relY);
            relY += MeshSize;
            return new UtmCoordinate(theGrid, theGrid.SouthernWest.X + relX, theGrid.SouthernWest.Y + relY);
        }

        /// <summary>
        ///     Return the upper right corner coordinates of a Mesh given by its number.
        ///     Please note that this point is on the UTM map, but at the borders
        ///     of a grid this coordinate may actually overlap and belong to another
        ///     UTM grid. So if you convert them to a Latitude/Longitude and then back
        ///     to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the upper right corner of the Mesh</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public UtmCoordinate UpperRight(long meshNumber)
        {
            var theGrid = Grid(meshNumber);
            MeshOrigin(meshNumber, out var relX, out var relY);
            relX += MeshSize;
            relY += MeshSize;
            return new UtmCoordinate(theGrid, theGrid.SouthernWest.X + relX, theGrid.SouthernWest.Y + relY);
        }

        /// <summary>
        ///     Get the list of neighbor meshes in a specified "distance". Distance 1 means
        ///     direct neighbors, 2 means neighbors that are 2 meshes away etc.
        /// </summary>
        /// <param name="meshNumber">The mesh number</param>
        /// <param name="distance">The distance (0-3 currently supported)</param>
        /// <returns>The list of mesh numbers of the neighbors</returns>
        /// <exception cref="ArgumentOutOfRangeException">Raised if an invalid mesh number is specified</exception>
        public List<long> Neighborhood(long meshNumber, int distance)
        {
            const int maxDistance = 3;

            if (distance is < 0 or > maxDistance)
            {
                throw new ArgumentOutOfRangeException(nameof(distance),
                    "Invalid distance.");
            }

            if (distance == 0)
            {
                return new List<long> { meshNumber };
            }

            var center = Projection.FromEuclidean(CenterOf(meshNumber));
            var calc = new GeodeticCalculator(Projection.ReferenceGlobe);
            var result = new List<long>();

            for (var y = -distance; y <= distance; y++)
            {
                var bearing = Math.Sign(y) < 0 ? 180.0 : 0.0;
                var vertical = (y != 0) ?
                    calc.CalculateEndingGeoCoordinates(center, bearing, Math.Abs(y) * MeshSize)
                    : center;

                for (var x = -distance; x <= distance; x++)
                {
                    if ((x == 0 && y == 0))
                    {
                        continue;
                    }

                    var add = false;
                    if (Math.Abs(y) == distance)
                    {
                        add = true;
                    }
                    else
                    {
                        if (Math.Abs(x) == distance)
                            add = true;
                    }

                    if (!add) continue;
                    bearing = Math.Sign(x) < 0 ? 270.0 : 90.0;
                    var horizontal = (x != 0) ?
                        calc.CalculateEndingGeoCoordinates(vertical, bearing, Math.Abs(x) * MeshSize)
                        : vertical;
                    try
                    {
                        result.Add(MeshNumber(horizontal));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            return result;
        }
    }
}