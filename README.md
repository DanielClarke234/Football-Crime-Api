# Football-Crime-Api

There is one endpoint /FootballCrime/CrimesAtClubs that will return a list of all the clubs in the given competition and a list of all the crimes on record that occured nearby.

NOTE: I could only ever get the police API to return crimes that happened on 05/2021? I hope my attempt at ordering is apparent even with the lack of orderable dates.

The project is using .NET version 5.0.
An install can be found here: https://dotnet.microsoft.com/download/dotnet/5.0

The API is only using standard http so make sure the calls are http and not https!

To call the endpoint:
* Pull the project and run in Visual Studio 19 using IIS.
* Call to endpoint: http://localhost:5000/FootballCrime/CrimesAtClubs via preferred means ( I have been using PostMan to make calls: https://www.postman.com/ )

To run the tests:
* Pull the project in Visual Studio 19
* Go to the test explorer (ctrl e, then t) and click the "Run All Tests" button
