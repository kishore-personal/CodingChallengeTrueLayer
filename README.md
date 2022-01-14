# Pokemon
Pokemon repo for TrueLayer - This is a simple Rest API, when given a valid Pokemon name, returns the name along with some additional information and translated descriptions.

# Prerequisite
- Docker - https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=Windows-Server

# Architecture & Components
- This project is developed using Clean architecture where all the layers are decoupled.
- Swagger Implementation
- Error Handling Middleware
- Health Checks on all the external APIs used
- Docker Impementation
- Unit Tests
- Integration Tests
- Caching Enabled
- API Versioning Enabled
- Living Doc Enabled

# External Sources
- https://funtranslations.com/api/shakespeare
- https://pokeapi.co/
- https://funtranslations.com/api/yoda

# Endpoints
- Swagger: http://localhost:50444/swagger/index.html
- Pokemon: http://localhost:50444/pokemon/
- Translated Pokemon: http://localhost:50444/pokemon/translated/
- Health: http://localhost:50444/health

# Documentation
- https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/visual-studio-tools-for-docker?view=aspnetcore-5.0

# Frameworks:
- Dotnetcore 3.1
- Docker 20.10.6

# How to run the solution:
- Clone the repo into a folder of your choice: git clone git@github.com:kiskiphotography/TLPokemon.git
- Open powershell from the root folder eg:TLPokemon 
- Type "docker-compose up"

The endpoint will be up and running. 
curl --location --request GET 'http://localhost:50444/v1/Pokemon/mewtwo' \
--header 'Content-Type: application/json'

# Running Unit Tests 
- Navigate to the unit test folder: cd TLPokemon.UnitTests
- Type dotnet test, to run the 5 unit tests included.

# Living Doc
- Cd into TLPokemon\TLPokemon.IntegrationTests\bin\Debug\netcoreapp3.1 
- Run the command livingdoc test-assembly .\TLPokemon.IntegrationTests.dll -t TestExecution.json  to generate the living doc html
- LivingDoc.html can be viewd to run or glaze through the tests

# What else can be done to improve code quality
- More negative unit tests and improve code coverage 
- 443 needs to be exposed in docker and also certificate installation needs to be done
- Docker installation could be automated
- Proper branching strategy should be followed, eg: create a Release Candidate branch, create a new feature branch from it and raise pull requests on to the RC branch
- Remove in memory caching and replace with Redis/Equivalent caching
- Proper set up of intgration tests with configuration classes and Before and after Scenario
- Could also implement CQRS using Mediatr in case there are commands and queries involved in the future 
- More config transforms to be added for other environments