package com.pragmatest.exceptions;

import java.util.Set;

public class UserUnsupportedPatchException extends RuntimeException {

    public UserUnsupportedPatchException(Set<String> keys) {
        super("No update is allowed on fields '" + keys.toString() + "'");
    }
}
