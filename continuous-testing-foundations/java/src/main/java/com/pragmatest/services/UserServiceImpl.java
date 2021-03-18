package com.pragmatest.services;

import com.pragmatest.models.User;
import com.pragmatest.models.UserEntity;
import com.pragmatest.repositories.UserRepository;
import com.pragmatest.utils.UserUtils;
import org.modelmapper.ModelMapper;
import org.modelmapper.TypeToken;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.lang.reflect.Type;
import java.util.List;
import java.util.Optional;

@Service
public class UserServiceImpl implements UserService {

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private UserUtils userUtils;

    @Autowired
    private ModelMapper modelMapper;

    @Override
    public User saveUser(User user) {

        int age = user.getAge();

        if (age < 18) {
            user.setIsActive(false);
        } else {
            user.setIsActive(true);
        }

        UserEntity userEntity = modelMapper.map(user, UserEntity.class);
        userEntity = userRepository.save(userEntity);

        User savedUser = modelMapper.map(userEntity, User.class);

        return savedUser;
    }

    @Override
    public User getUserById(Long id) {
        //TODO
        return null;
    }

    @Override
    public List<User> getAllUsers() {
        //TODO
        return null;
    }

    @Override
    public void deleteUserById(Long id) {
        //TODO
    }
}
