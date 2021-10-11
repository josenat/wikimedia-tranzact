# Wikimedia Tranzact

## Description
The Wikimedia Foundation provides all pageviews for Wikipedia site since 2015 in machine-readable format. The pageviews can be downloaded in gzip format and are aggregated per hour per page.

## Assignment
-	Create a command line application.
-	Get data for last "x" hours. 
-	Get "x" number of records (top) grouped by domain name, page title and ordered by the total sum of pageviews of all downloaded files.

## Observation
Don't use use database system to make the query.

## Run with docker
If you run the application with docker, it is recommended to do it in the foreground. Example:
- $ docker run -ti <image_name>