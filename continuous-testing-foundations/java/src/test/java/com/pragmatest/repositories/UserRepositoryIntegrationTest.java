package com.pragmatest.repositories;

import com.pragmatest.models.User;
import com.pragmatest.models.UserEntity;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.autoconfigure.orm.jpa.TestEntityManager;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

@DataJpaTest
public class UserRepositoryIntegrationTest {

    @Autowired
    private TestEntityManager testEntityManager;

    @Autowired
    private UserRepository userRepository;

    @Test
    void testFindUserById() {
        UserEntity userEntity = new UserEntity();
        userEntity.setFullName("John Smith");
        userEntity.setLocality("London");
        userEntity.setAge(20);

        testEntityManager.persistAndFlush(userEntity);

        assertNotNull(userEntity.getId());

        Optional<UserEntity> retrievedUser = userRepository.findById(userEntity.getId());

        assertFalse(retrievedUser.isEmpty());
        assertEquals(userEntity, retrievedUser.get());
    }

    @Test
    void testFindUsersById() {
        UserEntity userEntity1 = new UserEntity();
        userEntity1.setFullName("John Smith");
        userEntity1.setLocality("London");
        userEntity1.setAge(20);

        UserEntity userEntity2 = new UserEntity();
        userEntity2.setFullName("John Smith");
        userEntity2.setLocality("London");
        userEntity2.setAge(20);

    }

    @Test
    void testFindUserByNonExistentId() {

    }

    @Test
    void testDeleteUser() {
        UserEntity userEntity = new UserEntity();
        userEntity.setFullName("John Smith");
        userEntity.setLocality("London");
        userEntity.setAge(20);

        testEntityManager.persistAndFlush(userEntity);

        assertNotNull(userEntity.getId());

        testEntityManager.find(UserEntity.class, 1L);
    }



}
