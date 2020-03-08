package com.pragmatest;

import com.pragmatest.utils.UserUtils;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;

import static org.junit.jupiter.api.Assertions.*;

public class UserUtilsTest {

    UserUtils userUtils;

    @BeforeEach
    void init() {
        userUtils = new UserUtils();
    }

    @Test
    void testGenerateFullName() {
        String expectedName = "John Smith";

        assertEquals(expectedName, userUtils.generateFullName("John", "Smith"));
    }

    @Test
    void testIsOver18() {
        assertAll(
                () -> assertTrue(userUtils.isOver18(20)),
                () -> assertFalse(userUtils.isOver18(15)),
                () -> assertThrows(IllegalArgumentException.class, () -> userUtils.isOver18(0))

        );
    }

    @ParameterizedTest
    @ValueSource(ints = {18, 40, 50})
    void testOver18(int age){
        assertTrue(userUtils.isOver18(age));
    }
}
