# GoodFood test


# Steps to run the application


- GoodFood API

    Dependencies:
  
      dotnet core 3.1
      SqlServer
      
    The api is configured to connect to a local SqlServer database. Make sure the app will have no problems connecting with this connection string:
       
       "Server=localhost; Database=RecipesDB; Trusted_Connection=True; MultipleActiveResultSets=true"
       
     Within the cloned repository directory, execute theses commands:

        cd .\GoodFood.Recipes.Api\
        dotnet build
        cd .\GoodFood.Recipes.Api\
        dotnet run
        
     The database will be automatically created and api will be running at http://localhost:59277
     
     
- GoodFood Client Application

    Dependencies:
  
      Node.js / npm
       
     Within the cloned repository directory, execute theses commands:

        cd .\GoodFood.Recipes.Client\
        cd .\client-app\
        npm install
        npm start
        
     The client web application will be running at http://localhost:3000




