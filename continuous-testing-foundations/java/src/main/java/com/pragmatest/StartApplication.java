package com.pragmatest;

import com.pragmatest.models.UserEntity;
import com.pragmatest.repositories.UserRepository;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Profile;

@SpringBootApplication
public class StartApplication {
    public static void main(String[] args) {

        SpringApplication.run(StartApplication.class, args);
    }
}
