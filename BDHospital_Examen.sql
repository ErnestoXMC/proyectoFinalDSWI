create database DB_Hospital
use DB_Hospital

-----------------------------------------------------------------------------------
--TABLA AREA
create table area(
cod_area int primary key identity(1,1) not null,
nom_area varchar(30)
)
--Insertando Valores
INSERT INTO area (nom_area) VALUES ('Emergencias');
INSERT INTO area (nom_area) VALUES ('Pediatría');
INSERT INTO area (nom_area) VALUES ('Neurología');
INSERT INTO area (nom_area) VALUES ('Cardiologia');
INSERT INTO area (nom_area) VALUES ('Psicologia');

--SP Listar Areas
CREATE OR ALTER PROCEDURE sp_listarArea
as
select * from Area

exec sp_listarArea

--SP Insertar Area
CREATE OR ALTER PROCEDURE sp_insertarArea
    @nom_area varchar(30)
AS
BEGIN
    INSERT INTO area (nom_area)
    VALUES (@nom_area);
END;
exec sp_insertarArea 'Prueba'

--SP Eliminar Area
CREATE PROCEDURE sp_eliminarArea
    @cod_area int
AS
BEGIN
    DELETE FROM area
    WHERE cod_area = @cod_area;
END

EXEC sp_eliminarArea @cod_area = 7;

--SP Actualizar Area
CREATE OR ALTER PROCEDURE sp_actualizarArea
    @cod_area int,
    @nom_area varchar(30)
AS
BEGIN
    UPDATE area
    SET nom_area = @nom_area
    WHERE cod_area = @cod_area;
END

EXEC sp_actualizarArea @cod_area = 18, @nom_area = 'Nueva Área';




-----------------------------------------------------------------------------------
--TABLA MEDICO
create table medico(
cod_medico int primary key identity(1,1) not null,
nom_med varchar(50) not null,
edad_med int,
sexo_med char(1),
cod_area int references Area(cod_area)
)
go

INSERT INTO Medico (nom_med, edad_med, sexo_med, cod_area)
VALUES 
('Juan Perez', 40, 'M', 1), -- Área: Emergencias
('Carlos Gomez', 35, 'M', 2), -- Área: Pediatría
('Ana Garcia', 30, 'F', 1), -- Área: Emergencias
('Luis Rodriguez', 45, 'M', 3), -- Área: Neurología
('María Lopez', 25, 'F', 2), -- Área: Pediatría
('Sofía Diaz', 35, 'F', 3), -- Área: Neurología
('Roberto Martinez', 50, 'M', 4), -- Área: Cardiología
('Elena Fernandez', 45, 'F', 5), -- Área: Psicología
('Pablo Hernandez', 55, 'M', 5), -- Área: Psicología 
('Laura Sanchez', 40, 'F', 4); -- Área: Cardiología

--SP LISTAR MEDICOS
CREATE OR ALTER PROCEDURE sp_listarMedico
AS
BEGIN
    SELECT 
        m.cod_medico,
        m.nom_med,
        m.edad_med,
        m.sexo_med,
        a.nom_area AS nom_area
    FROM 
        Medico m
    INNER JOIN 
        Area a ON m.cod_area = a.cod_area;
END;

exec sp_listarMedico

--SP INSERTAR MEDICO
CREATE OR ALTER PROCEDURE sp_insertarMedico
    @nom_med varchar(50),
    @edad_med int,
    @sexo_med char(1),
    @cod_area int
AS
BEGIN
    INSERT INTO medico (nom_med, edad_med, sexo_med, cod_area)
    VALUES (@nom_med, @edad_med, @sexo_med, @cod_area);
END
GO
EXEC sp_insertarMedico @nom_med = 'Probando', @edad_med = 45, @sexo_med = 'M', @cod_area = 2;

--SP ELIMINAR MEDICO
CREATE OR ALTER PROCEDURE sp_eliminarMedico
    @cod_medico int
AS
BEGIN
    DELETE FROM medico
    WHERE cod_medico = @cod_medico;
END

EXEC sp_eliminarMedico @cod_medico = 10;

--SP ACTUALIZAR MEDICO
CREATE OR ALTER PROCEDURE sp_actualizarMedico
    @cod_medico int,
    @nom_med varchar(50),
    @edad_med int,
    @sexo_med char(1),
	@cod_area int
AS
BEGIN
    UPDATE medico
    SET nom_med = @nom_med,
        edad_med = @edad_med,
        sexo_med = @sexo_med,
		cod_area = @cod_area
    WHERE cod_medico = @cod_medico;
END
GO	

EXEC sp_actualizarMedico @cod_medico = 23, @nom_med = 'NUEVO MEDICO', @edad_med = 45, @sexo_med = 'M', @cod_area = 1;






-----------------------------------------------------------------------------------
--TABLA PACIENTE
create table Paciente(
cod_paciente int primary key identity(1,1) not null,
nom_pac varchar(50) not null,
ape_pac varchar(80) not null,
edad_pac int not null,
dni int not null unique,
sexo_pac char(1) not null,
fec_nac date not null,
contac_emer int not null,
cod_medico int references Medico(cod_medico),
cod_area int references Area(cod_area)
)
go

-- Creación de pacientes masculinos
INSERT INTO Paciente (nom_pac, ape_pac, edad_pac, dni, sexo_pac, fec_nac, contac_emer, cod_medico, cod_area)
VALUES 
('Joel Angel', 'Gonzales Abarca', 35, 12345678, 'M', '1989-05-15', 999888777, 1, 1), -- Área: Emergencias, Médico: Juan Perez
('Renzo Matias', 'Mendoza Alvares', 6, 23456789, 'M', '2018-08-25', 987654321, 2, 2), -- Área: Pediatría, Médico: Carlos Gomez
('Luisa Jovana', 'Cardenas Felix', 64, 34567890, 'F', '1960-03-10', 876543210, 3, 3), -- Área: Neurología, Médico: Luis Rodriguez
('Josefina', 'Cabellos Alvarez', 28, 45678901, 'F', '1996-11-20', 765432109, 4, 4), -- Área: Cardiología, Médico: Roberto Martinez
('Enrique Franco', 'Ceballos Gaveira', 24, 56789012, 'M', '2000-07-08', 654321098, 5, 5), -- Área: Psicología, Médico: Pablo Hernandez
('Analia Sofia', 'cabrera Zapata', 21, 67890123, 'F', '2003-09-05', 543210987, 6, 1), -- Área: Emergencias, Médico: Ana Garcia
('Katherine Lucia', 'Perez Trejo', 10, 78901234, 'F', '2014-04-18', 432109876, 7, 2), -- Área: Pediatría, Médico: María Lopez
('Jhon Luis', 'Rodriguez Sarmiento', 74, 89012345, 'M', '1950-12-30', 321098765, 8, 3), -- Área: Neurología, Médico: Sofía Diaz
('Lautaro Julian', 'Martinez Zabaleta', 55, 90123456, 'M', '1969-10-03', 210987654, 9, 4), -- Área: Cardiología, Médico: Laura Sanchez
('Elena Luz', 'Valdez Palomino', 27, 01234567, 'F', '1997-06-12', 109876543, 10, 5); -- Área: Psicología, Médico: Elena Fernandez


select * from Paciente


--SP INSERTAR PACIENTES
CREATE OR ALTER PROCEDURE sp_ingresarPacientes
    @nom_pac varchar(50),
    @ape_pac varchar(80),
    @edad_pac int,
    @dni int,
    @sexo_pac char(1),
    @fec_nac date,
    @contac_emer int,
    @cod_medico int,
    @cod_area int
AS
BEGIN
    INSERT INTO Paciente (nom_pac, ape_pac, edad_pac, dni, sexo_pac, fec_nac, contac_emer, cod_medico, cod_area)
    VALUES (@nom_pac, @ape_pac, @edad_pac, @dni, @sexo_pac, @fec_nac, @contac_emer, @cod_medico, @cod_area);
END;

EXEC sp_ingresarPacientes
    @nom_pac = 'Juan',
    @ape_pac = 'Pérez',
    @edad_pac = 30,
    @dni = 58632145,
    @sexo_pac = 'M',
    @fec_nac = '1993-04-29',
    @contac_emer = 987654321,
    @cod_medico = 1,
    @cod_area = 5;


--SP LISTAR PACIENTES

CREATE OR ALTER PROCEDURE sp_listarPacientes
AS
BEGIN
    SELECT 
        p.cod_paciente,
        p.nom_pac,
        p.ape_pac,
        p.edad_pac,
        p.dni,
        p.sexo_pac,
        p.fec_nac,
        p.contac_emer,
        m.nom_med AS nom_medico,
        a.nom_area AS nom_area
    FROM 
        Paciente p
    INNER JOIN 
        Medico m ON p.cod_medico = m.cod_medico
    INNER JOIN 
        Area a ON p.cod_area = a.cod_area;
END;


exec sp_listarPacientes

--SP ACTUALIZAR PACIENTES

CREATE OR ALTER PROCEDURE sp_actualizarPaciente
    @cod_paciente int,
    @nom_pac varchar(50),
    @ape_pac varchar(80),
    @edad_pac int,
    @dni int,
    @sexo_pac char(1),
    @fec_nac date,
    @contac_emer int,
    @cod_medico int,
    @cod_area int
AS
BEGIN
    UPDATE Paciente
    SET nom_pac = @nom_pac,
        ape_pac = @ape_pac,
        edad_pac = @edad_pac,
        dni = @dni,
        sexo_pac = @sexo_pac,
        fec_nac = @fec_nac,
        contac_emer = @contac_emer,
        cod_medico = @cod_medico,
        cod_area = @cod_area
    WHERE cod_paciente = @cod_paciente;
END;

EXEC sp_actualizarPaciente
	@cod_paciente = 17,
    @nom_pac = 'Jhoni',
    @ape_pac = 'Pérez',
    @edad_pac = 30,
    @dni = 58632145,
    @sexo_pac = 'M',
    @fec_nac = '1993-04-29',
    @contac_emer = 987654321,
    @cod_medico = 1,
    @cod_area = 1;


--SP ELIMINAR PACIENTES
CREATE OR ALTER PROCEDURE sp_eliminarPaciente
    @cod_paciente int
AS
BEGIN
    DELETE FROM Paciente
    WHERE cod_paciente = @cod_paciente;
END;

exec sp_eliminarPaciente 17



-----------------------------------------------------------------------------------
--TABLA USUARIO

create table usuario(
idUsuario int primary key identity,
nombreUsuario varchar(60) not null,
correo varchar(60),
clave varchar(100)
)

select * from usuario

CREATE OR ALTER PROCEDURE sp_eliminarUsuario
 @idUsuario int
AS
BEGIN
    DELETE FROM usuario WHERE idUsuario = @idUsuario;
END
















