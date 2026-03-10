package com.dam2.InterfacesJson.controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class AdvicerController {

    @ExceptionHandler(RuntimeException.class)
    public ResponseEntity<String> runtimeHandler(RuntimeException re) {
        return ResponseEntity.status(HttpStatus.NOT_FOUND)
                .body(re.getMessage());
    }
}
