package com.pragmatest.services;

import com.pragmatest.models.User;
import com.pragmatest.models.UserEntity;
import com.pragmatest.repositories.UserRepository;
import org.junit.jupiter.api.Test;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.context.ActiveProfiles;

import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.Mockito.*;

@SpringBootTest
@ActiveProfiles("test")
public class UserEntityServiceTest {

    @MockBean
    UserRepository userMockRepository;

    @Autowired
    UserService userService;

    @Autowired
    ModelMapper modelMapper;
}