package com.pragmatest;

import com.pragmatest.utils.UserUtils;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class UserUtilsTest {

    UserUtils userPortal;

    @BeforeEach
    void init() {
        userPortal = new UserUtils();
    }

    @Test
    void testGenerateFullName() {
        String expectedName = "Andrea Mangion";

        assertEquals(expectedName, userPortal.generateFullName("Andrea", "Mangion"));
    }


    @Test
    void testIsOver18() {

        assertAll(
                () -> assertTrue(userPortal.isOver18(20)),
                () -> assertFalse(userPortal.isOver18(15)),
                () -> assertThrows(IllegalArgumentException.class, () -> userPortal.isOver18(0))

        );
    }

}
