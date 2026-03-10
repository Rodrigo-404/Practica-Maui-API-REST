package com.dam2.InterfacesJson.dtos;

import lombok.*;
import org.springframework.data.annotation.Id;

import java.util.List;

@AllArgsConstructor
@Builder
@Getter
@Setter
@ToString
public class GetCancionesDTO {

    private String codigo;
    private String titulo;
    private List<String> artistas;
    private String duracion;
    private String urlPortada;
    private boolean esFavorita;

}
