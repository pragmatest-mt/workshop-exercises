package com.pragmatest.services;

import com.pragmatest.models.User;

import java.util.List;
import java.util.Optional;

public interface UserService {

    User saveUser(User user);

    List<User> getAllUsers();

    User getUserById(Long id);

    void deleteUserById(Long id);
}
