# Pragmatest & Co Workshop Exercises

In this repository, you will find all the exercises that are used in the different courses offered by Pragmatest & Co. 

## Structure

The folder structure indicates which material belongs to which course. A typical structure is {coursename}/{technology_used}, for example, continuous-testing-foundations/java

## Continuous Testing Foundations
This is a course aimed at enabling teams to start introducing testing into their development process. The guarantees given by tests at different levels are discussed and the course finishes with a practical example on unit tests using industry-standard mocking and testing frameworks. 

### Java
A sample Maven structured application is used for demo purposes. This uses technologies such as Springboot, JUnit 5 and Mockito. 

#### Usage 
The application can be deployed using the following command. Once initialized, the application is running on localhost on port 8080.

```bash
mvn spring-boot:run
```
Thanks to Swagger configuration, the API Controllers can be accessed on http://localhost:8080/swagger-ui.html

All the unit tests can be executed using the command below.

```bash
mvn test
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[Pragmatest & Co](https://www.pragmatest.com)