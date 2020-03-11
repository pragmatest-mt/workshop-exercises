package com.pragmatest.services;

import com.pragmatest.models.User;
import com.pragmatest.repositories.UserRepository;
import com.pragmatest.utils.UserUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class UserServiceImpl implements UserService {

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private UserUtils userUtils;

    @Override
    public Optional<User> getUserById(Long id) {

        return userRepository.findById(id);
    }

    @Override
    public List<User> getAllUsers() {

        return userRepository.findAll();
    }

    @Override
    public Optional<User> saveUser(User newUser) {
        int age = newUser.getAge();
        boolean isAdult = userUtils.isAdult(age);

        Optional<User> savedUser = Optional.empty();

        if (isAdult) {
            savedUser = Optional.of(userRepository.save(newUser));
        }

        return savedUser;
    }

    @Override
    public void deleteUserById(Long id) {

        userRepository.deleteById(id);
    }
}
