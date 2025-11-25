use bd_hoteleria;

create table if not exists tbl_paciente(
pk_idPaciente int not null,
nombrePaciente varchar(50),
apellidoPaciente varchar(50),
fechaNacimientoPaciente date,
sexoPaciente varchar(15),
direccionPaciente varchar(30),
telefonoPaciente varchar(20),
estadoPaciente int );

select * from tbl_paciente;