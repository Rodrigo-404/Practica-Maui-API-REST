package com.dam2.InterfacesJson.dtos;

import java.util.List;

public record CreateCancionDTO(
        String codigo,
        String titulo,
        List<String> artistas,
        String duracion,
        String urlPortada,
        boolean esFavorita
    ) {
}
