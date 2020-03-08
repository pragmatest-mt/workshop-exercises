package com.pragmatest.repositories;

import org.springframework.stereotype.Repository;

@Repository
public class UserPortalRepositoryImpl implements UserPortalRepository {

    @Override
    public String getName() {
        return "John Smith";
    }
}
