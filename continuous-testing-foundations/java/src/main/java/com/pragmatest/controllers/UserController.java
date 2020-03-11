package com.pragmatest.controllers;

import com.pragmatest.exceptions.UserInvalidException;
import com.pragmatest.exceptions.UserNotFoundException;
import com.pragmatest.models.User;
import com.pragmatest.services.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
public class UserController {

    @Autowired
    UserService userService;

    @GetMapping("/users")
    List<User> findAll() {

        return userService.getAllUsers();
    }

    @PostMapping("/users")
    @ResponseStatus(HttpStatus.CREATED)
    User newUser(@RequestBody User newUser) {

        return userService.saveUser(newUser).orElseThrow(() -> new UserInvalidException());
    }

    @GetMapping("/users/{id}")
    User findOne(@PathVariable Long id) {
        return userService.getUserById(id).orElseThrow(() -> new UserNotFoundException(id));
    }

    @PutMapping("/users/{id}")
    User saveOrUpdate(@RequestBody User newUser, @PathVariable Long id) {

        Optional<User> user = userService.getUserById(id);

        return user.map(x -> {
            x.setFullName(newUser.getFullName());
            x.setAge(newUser.getAge());
            x.setLocality(newUser.getLocality());

            return userService.saveUser(newUser).orElseThrow(() -> new UserInvalidException());
        }).orElseGet(() -> {
            newUser.setId(id);
            return userService.saveUser(newUser).orElseThrow(() -> new UserInvalidException());
        });
    }

    @DeleteMapping("/users/{id}")
    void deleteUser(@PathVariable Long id) {

        userService.deleteUserById(id);
    }
}
