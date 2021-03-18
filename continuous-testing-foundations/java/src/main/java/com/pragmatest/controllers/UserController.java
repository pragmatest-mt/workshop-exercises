package com.pragmatest.controllers;

import com.pragmatest.models.User;
import com.pragmatest.models.UserRequest;
import com.pragmatest.models.UserResponse;
import com.pragmatest.services.UserService;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import javax.validation.Valid;
import java.util.List;

@RestController
public class UserController {

    @Autowired
    UserService userService;

    @Autowired
    ModelMapper modelMapper;

    List<UserResponse> findAll() {
        //TODO
        return null;
    }

    @PostMapping("/users")
    @ResponseStatus(HttpStatus.CREATED)
    UserResponse newUser(@Valid @RequestBody UserRequest userRequest) {

        User user = modelMapper.map(userRequest, User.class);

        User savedUser = userService.saveUser(user);

        UserResponse userResponse = modelMapper.map(savedUser, UserResponse.class);

        return userResponse;
    }

    UserResponse findOne(@PathVariable Long id) {
        //TODO
        return null;
    }

    UserResponse saveOrUpdate(@RequestBody UserRequest userRequest, @PathVariable Long id) {
        //TODO
        return null;
    }

    void deleteUser(@PathVariable Long id) {
        //TODO
    }
}