# cs.Weightlifting
A solution for managing weightlifting training data.

This has several aims:

* to provide athletes a platform for adding and tracking training data
* to provide coaches with visibility on athlete's training data
* to provide coaches with the ability to manage multiple training programs and assign them to their athletes

## Structure
The project will be split into two main parts

* An API to provide authenticated access to the database
* A web application for the user interface 

### API
The API is a .NET 6 Core API, using Entity Framework to interact with a MSSQL database.

## Testing
Unit tests are written using XUnit and NSubstitute, and are run on commit to the main branch by Github's Actions. 