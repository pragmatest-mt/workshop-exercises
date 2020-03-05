package com.pragmatest.repository;

import com.pragmatest.repository.UserPortalRepository;
import org.springframework.stereotype.Repository;

@Repository
public class UserPortalRepositoryImpl implements UserPortalRepository {

    @Override
    public String getName() {
        return "Clyde Vassallo";
    }
}
