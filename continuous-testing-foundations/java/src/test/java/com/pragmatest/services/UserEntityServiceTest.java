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

    // Under 18 --> Inactive, Over 18 --> Active
    @Test
    public void testSaveUserUnder18(){

        User userToBeSaved = new User("Joe Borg", "Mosta", 17);

        User expectedSavedUser = new User(1L, "Joe Borg", "Mosta", 17);
        expectedSavedUser.setIsActive(false);

        UserEntity outputUserEntity = new UserEntity(1L, "Joe Borg", "Mosta", 17, false);
        when(userMockRepository.save(any(UserEntity.class))).thenReturn(outputUserEntity);


        User savedUser = userService.saveUser(userToBeSaved);

        assertThat(savedUser).isEqualToComparingFieldByField(expectedSavedUser);
        verify(userMockRepository, times(1)).save(any(UserEntity.class));
    }

    @Test
    public void testSaveUserOver18() {

        User userToBeSaved = new User("Joe Borg", "Mosta", 18);

        User expectedSavedUser = new User(1L, "Joe Borg", "Mosta", 18);
        expectedSavedUser.setIsActive(true);

        UserEntity outputUserEntity = new UserEntity(1L, "Joe Borg", "Mosta", 18, true);
        when(userMockRepository.save(any(UserEntity.class))).thenReturn(outputUserEntity);

        User savedUser = userService.saveUser(userToBeSaved);

        assertThat(savedUser).isEqualToComparingFieldByField(expectedSavedUser);

        verify(userMockRepository, times(1)).save(any(UserEntity.class));
    }
}