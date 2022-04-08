
# Archaeological Renaissance (ArchaeologyTools)

This repository contains the source code for a set of mapping and analysis tools. These tools were used to explore the archeological discoveries presented in the four-part series ***Archeological Renaissance***, which is also contained within this repository.

## Documents:

**Archaeological Renaissance**

The four-part series ***Archaeological Renaissance*** is located in the "*docs\Archeological Renaissance*" repository folder. Each part of the series is contained in a Microsoft PowerPoint document. 

However, pdf versions can be found in the "*docs\Archeological Renaissance\pdf*" repository folder.

**Data Reference Spreadsheets**
Data reference Excel spreadsheets can be found in the "*docs\Data Reference*" repository folder. These spreadsheets contain the following data:

 - Solar System Object Properties
 - Measurement Systems
 - Ancient Site Locations
 - Pole Locations/Axis Positions
 - Lantis Locations

This data is directly referenced within the ***Archaeological Renaissance*** series.

**Research Papers**

Several research papers can be found in the "*docs\Research Papers*" repository folder.  Information from these research papers is directly referenced within the ***Archaeological Renaissance*** series.

## Tools:

**Database**

The tools are configured to use a Sql Server database by default. A backup file that contains the complete backup of the Archeology database is located in the "*data/Sql Server Backups*" repository folder. The name of the backup file is:

    Archeology.bak

The database can be restored to any Sql Server 2012 (or later) instance.

The database references one SqlClr assembly, which should already be registered. The source code for the assembly can be found in the "*src\FractalSource.Sql.Clr*" repository folder. The project file requires the .Net Framework 4.5.2 SDK, which Visual Studio 2022 (and later) no longer support. Therefore, Visual Studio 2019 is the latest version that can be used to open and/or compile the project.

The SqlClr assembly is only required if using the "Ancient Site Alignment and Location Analysis Tool" and populating (refreshing) the tables in the Sql database.

**KML Layout Generator**

The KML layout generator tool dynamically generates Keyhole Markup Language (KML) documents and outputs the resulting xml to the specified stream/location. 

KML is a file format used to display geographic data in an Earth browser such as Google Earth. You can create KML files to pinpoint locations, add image overlays, and expose rich data in new ways. KML is an international standard maintained by the [Open Geospatial Consortium, Inc. (OGC)](http://www.opengeospatial.org/standards/kml/).

A layout is a KML document that contains overlay geometry and/or location details (placemark and description). 

The focal point of a layout is the target location of the KML document. Any UTM coordinates can be used as the focal point.

Examples of kml layouts:

 - Location Placemarks 
 - Global Grid (Pole/Earth Grid) 
 - Solar System Orbits
 - Atlantis Zones
 
The KML layout generator can be configured to output individual KML files or KML document "fragments" that are used for the KML NetworkLink feature. The NetworkLink feature allows dynamic KML hierarchical content. I.e., data-driven folder structure with no limits on folder depth or content items/size.

The NetworkLink feature requires a "server" to listen on the configured TCP port. The "server" can be a process that runs locally, or a web server (such as IIS) that runs locally as well. It can also be hosted remotely.

The following is a snippet of KML output when configured for the NetworkLink feature:

    <NetworkLink>
    	<name>Archaeological Renaissance</name>
    	<visibility>0</visibility>
    	<open>1</open>
    	<description>The true history of humanity.</description>
    	<Link>
    		<href>https://localhost:8080/Archeology/NetworkLinkRoot</href>
    	</Link>
    </NetworkLink>

The tool can be configured to use any host or port. It is recommended that SSL is enabled, as some security policies restrict nonsecure http requests.

**Ancient Site Alignment and Location Analysis Tool**

The data and visuals used for the "Probability Matrix" (Part II: Pivots) are located in a Microsoft Power BI project file. The file is located in the "*visualization\Microsoft Power BI*" repository folder. 

Power BI Desktop is required to view the project file, which can be downloaded here:

[Microsoft Power BI Desktop](https://www.microsoft.com/en-us/download/details.aspx?id=58494)

All data has already been loaded within the project file. However, in order to refresh the data, the stored procedures that contain all the calculations that generate all the probability matrix data must be executed on the Sql Server (the data was truncated from the database to save repository space.

Execute the following Sql to re-populate the data:

    USE Archeology
    GO
    
    EXEC usp_PopulatePoleLocationMatrix
    GO
    
    EXEC usp_PopulatePoleEquatorLocationMatrix
    GO

Depending on the default parameter values configured within the stored procedures, the execution time may take several hours.

Once execution is complete, the data can be refreshed within the Power BI project.

## Notes:

All tools support (and use) .Net Core dependency injection.

There are **no unit tests** programmed or configured for any of the tools. 

Currently, there are no plans to implement unit tests nor make any changes/enhancements to the codebase located within this repository. However, this repository can be cloned/forked and updated/customized by anyone. 

Finally, if the development community generally agrees upon an "official" forked repository (which has an active admin and contributors), I will gladly endorse that repository as "official" (if such an endorsement is desired).
