package com.pragmatest.services;

import com.pragmatest.repositories.UserPortalRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class UserPortalServiceImpl implements UserPortalService {

    @Autowired
    UserPortalRepository userPortalRepository;

    @Override
    public String getName() {
        return userPortalRepository.getName();
    }
}
