package com.pragmatest.controllers;

import com.pragmatest.exceptions.UserNotFoundException;
import com.pragmatest.exceptions.UserUnsupportedPatchException;
import com.pragmatest.models.User;
import com.pragmatest.repositories.jpa.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
public class UserController {

    @Autowired
    private UserRepository userRepository;

    //Find
    @GetMapping("/users")
    List<User> findAll() {
        return userRepository.findAll();
    }

    //Save
    @PostMapping("/users")
    //return 201 instead of 200
    @ResponseStatus(HttpStatus.CREATED)
    User newUser(@RequestBody User newUser) {
        return userRepository.save(newUser);
    }

    @GetMapping("/users/{id}")
    User findOne(@PathVariable Long id) {
        return userRepository.findById(id).orElseThrow(() -> new UserNotFoundException(id));
    }

    @PutMapping("/users/{id}")
    User saveOrUpdate(@RequestBody User newUser, @PathVariable Long id) {
        return userRepository.findById(id).map(x -> {
            x.setFullName(newUser.getFullName());
            x.setAge(newUser.getAge());
            x.setLocality(newUser.getLocality());
            return userRepository.save(newUser);
        }).orElseGet(() -> {
            newUser.setId(id);
            return userRepository.save(newUser);
        });
    }

    //update locality only
    @PatchMapping("/users/{id}")
    User patch(@RequestBody Map<String, String> update, @PathVariable Long id) {
        return userRepository.findById(id).map(x -> {
            String locality = update.get("locality");

            if (!StringUtils.isEmpty(locality)) {
                x.setLocality(locality);
                return userRepository.save(x);
            } else {
                throw new UserUnsupportedPatchException(update.keySet());
            }
        }).orElseGet(() -> {
            throw new UserNotFoundException(id);
        });
    }

    @DeleteMapping("/users/{id}")
    void deleteUser(@PathVariable Long id) {
        userRepository.deleteById(id);
    }
}
