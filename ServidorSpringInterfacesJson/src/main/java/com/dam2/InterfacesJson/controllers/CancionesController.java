package com.dam2.InterfacesJson.controllers;

import com.dam2.InterfacesJson.dtos.CreateCancionDTO;
import com.dam2.InterfacesJson.dtos.GetCancionesDTO;
import com.dam2.InterfacesJson.mappers.CancionesMapper;
import com.dam2.InterfacesJson.models.Cancion;
import com.dam2.InterfacesJson.sevices.CancionesService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
public class CancionesController {

    private final CancionesService cancionesService;
    private final CancionesMapper cancionesMapper;

    @GetMapping("canciones")
    public ResponseEntity<List<GetCancionesDTO>> getAllCanciones() {
        List<Cancion> canciones = this.cancionesService.getAllCanciones();
        List<GetCancionesDTO> cancionesDTO = this.cancionesMapper.mapCancionesToGetCancionesDTO(canciones);
        return ResponseEntity.ok(cancionesDTO);

    }

    @GetMapping("cancion/{codigo}")
    public ResponseEntity<GetCancionesDTO> getCancion(@PathVariable("codigo") String codigo) {
        Cancion cancion = this.cancionesService.findCancionByCodigo(codigo);
        GetCancionesDTO cancionDTO = new GetCancionesDTO(cancion.getCodigo(), cancion.getTitulo(), cancion.getArtistas(), cancion.getDuracion(), cancion.getUrlPortada(), cancion.isEsFavorita());
        return ResponseEntity.status(HttpStatus.ACCEPTED).body(cancionDTO);
    }

    @PostMapping("cancion")
    public ResponseEntity<GetCancionesDTO> createCanciones(@RequestBody CreateCancionDTO createCancionDTO) {

        GetCancionesDTO cancion = this.cancionesService.createCancion(createCancionDTO);
        return ResponseEntity.ok().body(cancion);
    }

    @DeleteMapping("cancion/{codigo}")
    public ResponseEntity<GetCancionesDTO> deleteCancion(@PathVariable("codigo") String codigo) {
        Cancion cancion = this.cancionesService.deleteCancionByCodigo(codigo);
        GetCancionesDTO cancionDTO = new GetCancionesDTO(cancion.getCodigo(), cancion.getTitulo(), cancion.getArtistas(), cancion.getDuracion(), cancion.getUrlPortada(), cancion.isEsFavorita());
        return ResponseEntity.status(HttpStatus.ACCEPTED).body(cancionDTO);
    }

    @PatchMapping("cancion/update")
    public ResponseEntity<GetCancionesDTO> updateCancion(@RequestBody CreateCancionDTO createCancionDTO) {
        Cancion cancion = this.cancionesService.updateCancion(createCancionDTO);
        GetCancionesDTO cancionDTO = new GetCancionesDTO(cancion.getCodigo(), cancion.getTitulo(), cancion.getArtistas(), cancion.getDuracion(), cancion.getUrlPortada(), cancion.isEsFavorita());
        return ResponseEntity.status(418).body(cancionDTO);
    }

}
