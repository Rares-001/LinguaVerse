To ensure the functionality of the application, a valid database is essential. LinguaVerse utilizes PostgreSQL for its database management. Please follow the detailed instructions below to set up the required database:

Install pgAdmin:
Download and install the specified version of pgAdmin from the following link: pgAdmin 4 v7.8 for Windows.
Create a New Database:
Open pgAdmin and create a new database. The process involves accessing the "Create Database" option as shown in the screenshot below:
<img width="604" alt="Screenshot 2024-06-21 at 22 47 25" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/40ecafeb-9d93-4275-9233-2dd5d72a38c0">
Name the Database:
Name the newly created database "LinguaVerseDB". This name is crucial for the application to recognize and interact with the database correctly. Refer to the screenshot below for guidance:
<img width="859" alt="Screenshot 2024-06-21 at 22 47 57" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/d14e2088-61cc-4910-93df-99f2c3c9a7d6">
Restore the Provided Backup:
Once the database is created, you need to restore the provided backup file to import the necessary data. This step is critical for pre-loading the database with the required schema and initial data. The following screenshot illustrates this process:
<img width="725" alt="Screenshot 2024-06-21 at 22 48 25" src="https://github.com/Rares-001/LinguaVerse/assets/91318114/d0335624-fdaf-4c8f-8b94-18797fea75fa">
Select the Backup File Location:
In the restore dialog, navigate to the location of the backup file on your computer and select it. Ensuring the correct file is chosen is imperative for a successful database setup.
