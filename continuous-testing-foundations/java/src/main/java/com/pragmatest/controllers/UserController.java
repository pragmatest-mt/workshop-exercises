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

    UserResponse newUser(@RequestBody UserRequest userRequest) {
        //TODO
        return null;
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