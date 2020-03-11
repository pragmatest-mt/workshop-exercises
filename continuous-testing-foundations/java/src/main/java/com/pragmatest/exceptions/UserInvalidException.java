package com.pragmatest.exceptions;

public class UserInvalidException extends RuntimeException {

    public UserInvalidException() {
        super("Invalid User");
    }
}
