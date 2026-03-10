package com.dam2.InterfacesJson.sevices;

import com.dam2.InterfacesJson.dtos.CreateCancionDTO;
import com.dam2.InterfacesJson.dtos.GetCancionesDTO;
import com.dam2.InterfacesJson.models.Cancion;
import com.dam2.InterfacesJson.repositories.MyCancionesRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class CancionesService {

    private final MyCancionesRepository repository;

    public List<Cancion> getAllCanciones() {
        return repository.findAll();
    }

    public GetCancionesDTO createCancion(CreateCancionDTO createCancionDTO) {
        Cancion cancionDesdeDTO = Cancion.builder()
                .codigo(createCancionDTO.codigo())
                .titulo(createCancionDTO.titulo())
                .artistas(createCancionDTO.artistas())
                .urlPortada(createCancionDTO.urlPortada())
                .duracion(createCancionDTO.duracion())
                .esFavorita(createCancionDTO.esFavorita())
                .build();
        Cancion creada = repository.save(cancionDesdeDTO);
        return GetCancionesDTO.builder()
                .codigo(creada.getCodigo())
                .titulo(creada.getTitulo())
                .artistas(creada.getArtistas())
                .urlPortada(createCancionDTO.urlPortada())
                .duracion(createCancionDTO.duracion())
                .esFavorita(creada.isEsFavorita())
                .build();
    }

    public Cancion findCancionByCodigo(String codigo) {
        return repository.findByCodigo(codigo)
                .orElseThrow(() -> new RuntimeException("No se ha encontrado la canción "+codigo));
    }

    public Cancion deleteCancionByCodigo(String codigo) {
        return repository.deleteByCodigo(codigo);
    }

    public Cancion updateCancion(CreateCancionDTO createCancionDTO) {

        String idExistente = repository.findByCodigo(createCancionDTO.codigo())
                .map(Cancion::getId)
                .orElseThrow(() -> new RuntimeException("No se ha encontrado la canción "+createCancionDTO.codigo()));

        Cancion cancionAActualizar = Cancion.builder()
                .id(idExistente)
                .codigo(createCancionDTO.codigo())
                .titulo(createCancionDTO.titulo())
                .artistas(createCancionDTO.artistas())
                .urlPortada(createCancionDTO.urlPortada())
                .duracion(createCancionDTO.duracion())
                .esFavorita(createCancionDTO.esFavorita())
                .build();

        return repository.save(cancionAActualizar);
    }
}
