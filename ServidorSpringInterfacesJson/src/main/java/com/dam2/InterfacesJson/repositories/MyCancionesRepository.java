package com.dam2.InterfacesJson.repositories;

import com.dam2.InterfacesJson.models.Cancion;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.Optional;

public interface MyCancionesRepository extends MongoRepository<Cancion, String> {

    Cancion deleteByCodigo(String codigo);
    Optional<Cancion> findByCodigo(String codigo);

}
