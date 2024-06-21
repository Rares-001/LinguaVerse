LinguaVerse is an innovative hybrid app designed to make learning Italian or English engaging and enjoyable. The app features a variety of interactive games such as flashcards, memory cards, tests, and quizzes, ensuring a dynamic and fun learning experience. Whether you're a beginner or looking to enhance your language skills, LinguaVerse provides a comprehensive and entertaining way to master a new language.

To ensure the functionality of the application, a valid database is essential. LinguaVerse utilizes PostgreSQL for its database management. Please follow the detailed instructions below to set up the required database:

Install pgAdmin:
- Download and install the specified version of pgAdmin from the following link: https://www.postgresql.org/ftp/pgadmin/pgadmin4/v7.8/windows/ .

Create a New Database:
- Open pgAdmin and create a new database. The process involves accessing the "Create Database" option as shown in the screenshot below:

<img width="604" alt="Screenshot 2024-06-21 at 22 47 25" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/40ecafeb-9d93-4275-9233-2dd5d72a38c0">

Name the Database:
- Name the newly created database "LinguaVerseDB". This name is crucial for the application to recognize and interact with the database correctly. Refer to the screenshot below for guidance:

<img width="859" alt="Screenshot 2024-06-21 at 22 47 57" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/d14e2088-61cc-4910-93df-99f2c3c9a7d6">

Restore the Provided Backup:
- First locate the backup, currentlry is in DAL -> LinguaVerseDB_Backup. This is the link for it: LinguaVerse/DAL/LinguaVerseDB_Backup.sql
- Once the database is created, you need to restore the provided backup file to import the necessary data. This step is critical for pre-loading the database with the required schema and initial data. The following screenshot illustrates this process:

<img width="725" alt="Screenshot 2024-06-21 at 22 48 25" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/d0335624-fdaf-4c8f-8b94-18797fea75fa">

Select the Backup File Location:
- In the restore dialog, navigate to the location of the backup file on your computer and select it. Ensuring the correct file is chosen is imperative for a successful database setup.


To establish a connection between the database and the app, you need to modify a specific line of code with your PostgreSQL username and password. Follow these steps:
- Locate the MauiProgram.cs file
- Navigate to line 32, where you will find the following line of code: ```
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";```
  <img width="1010" alt="Screenshot 2024-06-21 alle 23 05 38" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/e304c035-8b48-44a3-9931-4caa4a40920a">

- Change this username with yours ``` Username=postgres ``` and ``` Password=admin"```
- Save the file and run the app

- If everything is configured correctly, you will see the following message:
  
<img width="248" alt="Screenshot 2024-06-21 alle 23 14 30" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/813b5f00-a927-46ed-862e-10a89ee8d59b">

in the Login Page:

<img width="1256" alt="Screenshot 2024-06-21 alle 23 03 50" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/5c70568c-fa90-4a6f-b354-adc2bcff7e04">
