# Use a base image with Java
FROM openjdk:17-jdk-slim

# Set the working directory
WORKDIR /app

# Copy the Maven-built JAR file to the container
COPY target/UserPortal-1.0-SNAPSHOT.jar app.jar

# Expose the application port (change if your app uses a different port)
EXPOSE 8081

# Run the application
CMD ["java", "-jar", "app.jar"]