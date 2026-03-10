package com.dam2.InterfacesJson.models;

import lombok.*;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.index.Indexed;
import org.springframework.data.mongodb.core.mapping.Document;
import org.springframework.stereotype.Controller;

import java.util.List;

@AllArgsConstructor
@Builder
@Getter
@Setter
@ToString
@Document("canciones")
public class Cancion {

    @Id
    private String id;

    @Indexed(unique = true)
    private String codigo;

    private String titulo;
    private List<String> artistas;
    private String duracion;
    private String urlPortada;
    private boolean esFavorita;

}
