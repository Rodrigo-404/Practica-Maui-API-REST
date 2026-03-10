package com.dam2.InterfacesJson.mappers;

import com.dam2.InterfacesJson.dtos.GetCancionesDTO;
import com.dam2.InterfacesJson.models.Cancion;
import com.dam2.InterfacesJson.sevices.CancionesService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Component;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.net.URI;
import java.util.List;

@Component
public class CancionesMapper {

    public List<GetCancionesDTO> mapCancionesToGetCancionesDTO(List<Cancion> canciones) {
        return canciones.stream()
                .map(cancion -> GetCancionesDTO.builder()
                        .codigo(cancion.getCodigo())
                        .artistas(cancion.getArtistas())
                        .duracion(cancion.getDuracion())
                        .titulo(cancion.getTitulo())
                        .esFavorita(cancion.isEsFavorita())
                        .urlPortada(cancion.getUrlPortada())
                        .build()
                ).toList();
    }
}
