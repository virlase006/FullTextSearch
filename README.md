# FullTextSearch
Full text search using C# .Net and SQLite
This solution involve two part.
1. Designign database
2. Search Appliication (full text search)

## 1. Desgining Database:
Main funcitonality of full text search is in desginging database. SQL queries used to desing this database are given in file databaseinitialization.
This file contains queries for creating tables and also creating indexes and syncing any changes to the data in tables to the indexes.
For keeping task simple I have used two tables of news:
 1. Sports News
 2. Weather News
Both table contains three fields ID, News and Heading. We have created index on heading and new text using SQLite FTS4 module.
Once indices are created we have written triggers on both tables so that when any item is inserted , updated or deleted the indecis are updated as well
so that search result always returns the latest results.

## 2. Search Application:
When you run the search application it prompts user to choose what action he wants to perform. He is following three optinos:
### 1. Initialize database:
When user choose this option He is asked choose whether he want to create tables or batch insert. The database queries will be run on database name TestNewsDB. 
If user choose to create tables , queries will be run that will create tables , indecis and triggers if not already created.
If user choose to batch insert, data will be inserted into tables. If you would like to insert your own data you can edit batchinsert file. 
Note: Running batch insert file twice will add same data twice as IDs are auto generated and news and heading fields are not unique. This will affect the search result as similar results would appear more than once.
### 2. Perform CRUD:
User can choose to insert update or delete data from tables. He would be systematically prompt by the console app to make sure right data is being inserted/modified/deleted.
### 3. Search Full Text:
When user choose to perform search. He will be prompt to enter the text he would like to search. Tables are search using indecis (Full text search) and list of result is shown on the console. Result may contains records from both table. Results will reflect the latest state of the records.
