#nullable enable
using System.Xml.Linq;
using FractalSource.Mapping.Keyhole;
using SharpKml.Base;
using SharpKml.Dom;

namespace FractalSource.Mapping
{

    public static class KmlExtensions
    {
        public static Feature ToFeature(this KmlFeatureContainer kmlFeatureContainer)
        {
            var feature
                = (kmlFeatureContainer as ExtendedKmlFeatureContainer)?.Feature
                  ?? new Folder
                  {
                      Name = kmlFeatureContainer.Name
                  };

            return feature;
        }

        public static KmlFeatureContainer ToFeatureContainer(this Feature feature)
        {
            return new ExtendedKmlFeatureContainer
            {
                Name = feature.Name,
                Description = feature.Description?.Text
                              ?? feature.Name,
                Feature = feature
            };
        }

        public static XDocument ToXDocument(this Feature feature)
        {
            var serializer = new Serializer();
            serializer.Serialize(new Kml
            {
                Feature = feature,
            });

            return XDocument.Parse(serializer.Xml);
        }

        public static Document ToDocument(this KmlDocument document)
        {
            return new Document
            {
                Id = document.InstanceId.ToString(),
                Name = document.Name,
                Description = new Description
                {
                    Text = document.Description
                }
            };
        }

        public static Folder AddFolder(this Container container, string folderName)
        {
            return container
                .AddFolder(folderName, string.Empty, null);
        }

        public static Folder AddFolder(this Container container, string folderName, bool? visibility)
        {
            return container
                .AddFolder(folderName, string.Empty, visibility);
        }

        public static Folder AddFolder(this Container container, string folderName, string description, bool? visibility)
        {
            var folder = new Folder
            {
                Name = folderName,
                Description
                    = !string.IsNullOrWhiteSpace(description)
                        ? new Description
                        {
                            Text = description
                        }
                        : null,
                Visibility = visibility
            };

            container.AddFeature(folder);

            return folder;
        }


        public static GeoCoordinates ToGeoCoordinates(this Vector? vector)
        {
            return
                new GeoCoordinates(
                    vector?.Latitude ?? 0,
                    vector?.Longitude ?? 0);
        }

        //public static void MarkVisibilityRecursive(this Container container, bool visible)
        //{
        //    foreach (var feature in container.Features)
        //    {
        //        feature.Visibility = visible;

        //        if (feature is Container containerFeature)
        //        {
        //            containerFeature.MarkVisibilityRecursive(visible);
        //        }
        //    }
        //}

        public static TContainer MarkVisibilityRecursive<TContainer>(this TContainer container, bool visible)
        where TContainer : Container
        {
            foreach (var feature in container.Features)
            {
                feature.Visibility = visible;

                if (feature is Container containerFeature)
                {
                    containerFeature.MarkVisibilityRecursive(visible);
                }
            }

            return container;
        }
    }
}