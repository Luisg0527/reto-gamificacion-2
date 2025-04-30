using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Data;


namespace Reto.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }
        public DataBaseContext()
        {
             ConnectionString = "Server=addServer;Port=14683;Database=OxxoDB;Uid=avnadmin;password='ADDPASSWORD';";   }    
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public MetricasDash getMetricas(DateTime selectedDate)
        {
            MetricasDash metricas = new MetricasDash(0, 0, 0, 0, 0, 0, DateTime.MinValue);

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para obtener la SUMA de cada columna donde la fecha sea menor a selectedDate
                string query = @"SELECT 
                                    SUM(ingresos) AS totalIngresos, 
                                    SUM(ganancias) AS totalGanancias, 
                                    SUM(ordenes) AS totalOrdenes, 
                                    SUM(visitas) AS totalVisitas
                                FROM metricasUsuario 
                                WHERE id_usuario = 1 AND fecha < @SelectedDate";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            metricas = new MetricasDash(
                                0,
                                1,
                                reader.GetFloat("totalIngresos"),
                                reader.GetFloat("totalGanancias"),
                                reader.GetInt32("totalOrdenes"),
                                reader.GetInt32("totalVisitas"),
                                selectedDate
                            );
                        }
                    }
                }
            }
            return metricas;
        }

        public bool verificarUsuario(string _nombre)
        {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT COUNT(*) AS existe FROM Usuario WHERE nombre_usuario = '" + _nombre + "'";
            bool existe = false;
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    existe = reader.GetInt32("existe") > 0;
                }
            }
            conexion.Close();
            return existe;
        }

        public bool verificarContra(string _nombre, string _contra)
        {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT COUNT(*) AS existe FROM Usuario WHERE nombre_usuario = '" + _nombre + "' AND password = '" + _contra + "'";
            bool ContraExiste = false;
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    ContraExiste = reader.GetInt32("existe") > 0;
                }
            }
            conexion.Close();
            return ContraExiste;
        }

        public string getTipoEmpleado(string nomUsuario) {
            string tipoEmpleado = "SIN PUESTO";

            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Web_getPuesto";
            cmd.Connection = conexion;

            cmd.Parameters.AddWithValue("@nomUsuario", nomUsuario);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    tipoEmpleado = reader.GetString("puesto");
                }
            }
            conexion.Close();
            return tipoEmpleado;
        }

        public void crearUsuario(int _empId, string _nombre, string _contra, string _correo)
        {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT MAX(id_usuario) AS last FROM Usuario";
            int lastId = 0;

            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    lastId = reader.GetInt32("last");
                }
            }
            int myUsersId = lastId + 1;

            cmdString = "INSERT INTO Usuario (id_empleado, id_usuario, nombre_usuario, password, correo) VALUES (@emId, @usId, @nombre, @contra, @correo)";
            cmd = new MySqlCommand(cmdString, conexion);
            cmd.Parameters.AddWithValue("@emId", _empId);
            cmd.Parameters.AddWithValue("@usId", myUsersId);
            cmd.Parameters.AddWithValue("@nombre", _nombre);
            cmd.Parameters.AddWithValue("@contra", _contra);
            cmd.Parameters.AddWithValue("@correo", _correo);

            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public void cambiarContra(string _usuario, string _nuevaContra)
        {
            MySqlConnection conexion = GetConnection();
            conexion.Open();

            string cmdString = "UPDATE Usuario SET password = @nuevaContraseña WHERE nombre_usuario = '" + _usuario + "'";
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);
            cmd.Parameters.AddWithValue("@nuevaContraseña", _nuevaContra);

            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        //Obtener la informacion del usuario para la pagina de home
        public Usuario GetUserByName(string nomUsuario) {
            Usuario usuario = new Usuario();
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Web_getUsuario";
            cmd.Connection = conexion;

            cmd.Parameters.AddWithValue("@nomUsuario", nomUsuario);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario.id_usuario = reader.GetInt32("id_usuario");
                    usuario.nombre_usuario = reader.GetString("nombre_usuario");
                    usuario.password = reader.GetString("password");
                    usuario.monedas = reader.GetInt32("monedas");
                    usuario.nivel = reader.GetInt32("nivel");
                    usuario.retos_completados = reader.GetInt32("retos_completados");
                    usuario.correo = reader.GetString("correo");
                    usuario.id_empleado = reader.GetInt32("id_empleado");
                    usuario.rol = reader.IsDBNull(reader.GetOrdinal("rol")) ? "" : reader.GetString("rol");
                    usuario.quote = reader.IsDBNull(reader.GetOrdinal("quote")) ? "" : reader.GetString("quote");
                    usuario.ubicacion = reader.IsDBNull(reader.GetOrdinal("ubicacion")) ? "" : reader.GetString("ubicacion");
                    usuario.telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? "" : reader.GetString("telefono");
                    usuario.imagen = reader.IsDBNull(reader.GetOrdinal("imagen")) ? "" : reader.GetString("imagen");
                }
            }
            conexion.Close();

            return usuario;
        }

        public List<Usuario> GetLeaderboard()
        {
            List<Usuario> leaderboard = new List<Usuario>();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Usuario WHERE rol = 'Asesor de Tienda' ORDER BY nivel DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leaderboard.Add(new Usuario(
                            reader.GetInt32("id_usuario"),
                            reader.GetString("nombre_usuario"),
                            reader.GetString("password"),
                            reader.GetInt32("monedas"),
                            reader.GetInt32("nivel"),
                            reader.GetInt32("retos_completados"),
                            reader.GetString("correo"),
                            reader.GetInt32("id_empleado"),
                            reader.GetString("rol"),
                            reader.GetString("quote"),
                            reader.GetString("imagen"),
                            reader.GetString("ubicacion"),
                            reader.GetString("telefono")
                        ));
                    }
                }
            }

            return leaderboard;
        }

        public Usuario GetUsuarioByNombre(string nombre)
        {
            Usuario usuario = null;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Usuario WHERE nombre_usuario = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario(
                               reader.GetInt32("id_usuario"),
                               reader.GetString("nombre_usuario"),
                               reader.GetString("password"),
                               reader.GetInt32("monedas"),
                               reader.GetInt32("nivel"),
                               reader.GetInt32("retos_completados"),
                               reader.GetString("correo"),
                               reader.GetInt32("id_empleado"),
                               reader.GetString("rol"),
                               reader.GetString("quote"),
                               reader.GetString("imagen"),
                               reader.GetString("ubicacion"),
                               reader.GetString("telefono")
                           );
                        }
                    }
                }
            }
            return usuario;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE Usuario SET telefono = @telefono, correo = @correo, ubicacion = @ubicacion, quote = @quote WHERE id_usuario = @id_usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@telefono", string.IsNullOrEmpty(usuario.telefono) ? DBNull.Value : (object)usuario.telefono);
                    cmd.Parameters.AddWithValue("@correo", string.IsNullOrEmpty(usuario.correo) ? DBNull.Value : (object)usuario.correo);
                    cmd.Parameters.AddWithValue("@ubicacion", string.IsNullOrEmpty(usuario.ubicacion) ? DBNull.Value : (object)usuario.ubicacion);
                    cmd.Parameters.AddWithValue("@quote", string.IsNullOrEmpty(usuario.quote) ? DBNull.Value : (object)usuario.quote);
                    cmd.Parameters.AddWithValue("@id_usuario", usuario.id_usuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Pregunta> obtenerPregunta()
        {
            Pregunta pregunta;
            List<Pregunta> ListaPreguntas = new List<Pregunta>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para obtener la SUMA de cada columna donde la fecha sea menor a selectedDate
                string query = @"SELECT 
                                p.id_pregunta,
                                u.nombre_usuario,
                                p.texto_pregunta,
                                p.date,
                                p.likes
                            FROM 
                                Pregunta p
                            JOIN 
                                Usuario u ON p.id_usuario = u.id_usuario;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            pregunta = new Pregunta();
                            pregunta.id_pregunta = reader.GetInt32("id_pregunta");
                            pregunta.nombre_usuario = reader.GetString("nombre_usuario");
                            pregunta.texto_pregunta = reader.GetString("texto_pregunta");
                            pregunta.likes = reader.GetInt32("likes");
                            pregunta.date = reader.GetDateTime("date");
                            pregunta.Respuestas = obtenerRespuesta(pregunta.id_pregunta);
                            ListaPreguntas.Add(pregunta);
                        }
                    }
                }
            }
            return ListaPreguntas;
        }

        //Obtener lista de Respuestas
        public List<Respuesta> obtenerRespuesta(int id_pregunta)
        {
            Respuesta respuesta;
            List<Respuesta> ListaRespuestas = new List<Respuesta>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para obtener la SUMA de cada columna donde la fecha sea menor a selectedDate
                string query = @"SELECT 
                                    r.id_respuesta,
                                    u.nombre_usuario ,
                                    r.texto_respuesta,
                                    r.date,
                                    r.likes
                                FROM 
                                    Foro f
                                JOIN 
                                    Respuesta r ON f.id_respuesta = r.id_respuesta
                                JOIN 
                                    Usuario u ON r.id_usuario = u.id_usuario
                                WHERE 
                                    f.id_pregunta = @id_pregunta;";


                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id_pregunta", id_pregunta);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            respuesta = new Respuesta();
                            respuesta.id_respuesta = reader.GetInt32("id_respuesta");
                            respuesta.nombre_usuario = reader.GetString("nombre_usuario");
                            respuesta.texto_respuesta = reader.GetString("texto_respuesta");
                            respuesta.likes = reader.GetInt32("likes");
                            respuesta.date = reader.GetDateTime("date");
                            ListaRespuestas.Add(respuesta);
                        }
                    }
                }
            }
            return ListaRespuestas;
        }

        public void agregarLikePregunta(int id_pregunta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para actualizar los likes de una pregunta
                string cmdString = @"UPDATE Pregunta
                                SET likes = likes + 1
                                WHERE id_pregunta = @id_pregunta;";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@id_pregunta", id_pregunta);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void agregarLikeRespuesta(int id_respuesta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para actualizar los likes de una pregunta
                string cmdString = @"UPDATE Respuesta
                                SET likes = likes + 1
                                WHERE id_respuesta = @id_respuesta;";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@id_respuesta", id_respuesta);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void quitarLikePregunta(int id_pregunta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para actualizar los likes de una pregunta
                string cmdString = @"UPDATE Pregunta
                                SET likes = likes - 1
                                WHERE id_pregunta = @id_pregunta;";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@id_pregunta", id_pregunta);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void quitarLikeRespuesta(int id_respuesta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para actualizar los likes de una pregunta
                string cmdString = @"UPDATE Respuesta
                                SET likes = likes - 1
                                WHERE id_respuesta = @id_respuesta;";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@id_respuesta", id_respuesta);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void agregarPregunta(string texto_pregunta, int? id_usuario)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Query para actualizar los likes de una pregunta
                string cmdString = @"INSERT INTO Pregunta (id_usuario, texto_pregunta, likes, date)
                                    VALUES (@id_usuario,@texto_pregunta,@likes ,@date );";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@texto_pregunta", texto_pregunta);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@likes", 0);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void agregarRespuesta(string texto_respuesta, int? id_usuario, int id_pregunta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // Comenzamos con la inserción en la tabla Respuesta
                string cmdString = @"INSERT INTO Respuesta (id_usuario, texto_respuesta, likes, date)
                             VALUES (@id_usuario, @texto_respuesta, @likes, @date);";

                MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                cmd.Parameters.AddWithValue("@texto_respuesta", texto_respuesta);
                cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@likes", 0);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();

                string cmdString2 = @"SELECT LAST_INSERT_ID();";
                MySqlCommand idCmd = new MySqlCommand(cmdString2, connection);
                int id_res = Convert.ToInt32(idCmd.ExecuteScalar());

                // Ahora insertamos en la tabla Foro (o la tabla que tengas para asociar la pregunta con la respuesta)
                string cmdForoString = @"INSERT INTO Foro (id_pregunta, id_respuesta)
                                  VALUES (@id_pregunta, @id_respuesta);";

                MySqlCommand cmdForo = new MySqlCommand(cmdForoString, connection);
                cmdForo.Parameters.AddWithValue("@id_pregunta", id_pregunta); // El id de la pregunta que ya tienes
                cmdForo.Parameters.AddWithValue("@id_respuesta", id_res); // El id de la respuesta recién insertada
                cmdForo.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool UsuarioDioLikePregunta(int idUsuario, int idPregunta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT 1
                         FROM LikesPreguntas
                         WHERE id_usuario = @idUsuario AND id_pregunta = @idPregunta
                         LIMIT 1;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idPregunta", idPregunta);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool liked = reader.Read();
                        return liked;
                    }
                }
            }
        }


        public void likePregunta(int idUsuario, int idPregunta, bool liked)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                if (liked)
                {
                    quitarLikePregunta(idPregunta);
                    string cmdString = @"DELETE FROM LikesPreguntas
                                    WHERE id_usuario = @idUsuario AND id_pregunta = @idPregunta;";

                    MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idPregunta", idPregunta);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    agregarLikePregunta(idPregunta);
                    string cmdString = @"INSERT INTO LikesPreguntas (id_usuario, id_pregunta)
                                        VALUES (@idUsuario, @idPregunta);";

                    MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                    cmd.Parameters.AddWithValue("@idPregunta", idPregunta);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        public bool UsuarioDioLikeRespuesta(int idUsuario, int idRespuesta)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"SELECT 1
                         FROM LikesRespuestas
                         WHERE id_usuario = @idUsuario AND id_respuesta = @idRespuesta
                         LIMIT 1;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idRespuesta", idRespuesta);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool liked = reader.Read();
                        return liked;
                    }
                }
            }
        }


        public void likeRespuesta(int idUsuario, int idRespuesta, bool liked)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                if (liked)
                {
                    quitarLikeRespuesta(idRespuesta);
                    string cmdString = @"DELETE FROM LikesRespuestas
                                    WHERE id_usuario = @idUsuario AND id_respuesta = @idRespuesta;";

                    MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idRespuesta", idRespuesta);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    agregarLikeRespuesta(idRespuesta);
                    string cmdString = @"INSERT INTO LikesRespuestas (id_usuario, id_respuesta)
                                        VALUES (@idUsuario, @idRespuesta);";

                    MySqlCommand cmd = new MySqlCommand(cmdString, connection);
                    cmd.Parameters.AddWithValue("@idRespuesta", idRespuesta);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        //Obtener videojuego
        public List<Videojuego> obtenerVideojuego()
        {
            Videojuego vj;
            List<Videojuego> ListaVideojuegos = new List<Videojuego>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string queryVideojuegos = @"SELECT * FROM videojuegos;";


                using (MySqlCommand cmd = new MySqlCommand(queryVideojuegos, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            vj = new Videojuego();
                            vj.Id = reader.GetInt32("Id");
                            vj.Nombre = reader.GetString("Nombre");
                            vj.Descripcion = reader.GetString("Descripcion");
                            vj.Historia = reader.GetString("Historia");
                            ListaVideojuegos.Add(vj);
                        }
                    }
                }
            }
            return ListaVideojuegos;
        }

        //Obtener Controles
        public List<Controles> obtenerControles()
        {

            Controles ctrl;
            List<Controles> ListaControles = new List<Controles>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string queryControles = @"SELECT * FROM controles;";


                using (MySqlCommand cmd = new MySqlCommand(queryControles, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            ctrl = new Controles();
                            ctrl.Id = reader.GetInt32("Id");
                            ctrl.VideojuegoId = reader.GetInt32("videojuego_id");
                            ctrl.Accion = reader.GetString("accion");
                            ctrl.Tecla = reader.GetString("tecla");
                            ListaControles.Add(ctrl);
                        }
                    }
                }
            }
            return ListaControles;
        }

        //Obtener Creditos
        public List<Creditos> obtenerCreditos()
        {
            Creditos crd;
            List<Creditos> ListaCreditos = new List<Creditos>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string queryCreditos = @"SELECT * FROM creditos;";


                using (MySqlCommand cmd = new MySqlCommand(queryCreditos, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            crd = new Creditos();
                            crd.Id = reader.GetInt32("Id");
                            crd.VideojuegoId = reader.GetInt32("videojuego_id");
                            crd.NombrePersona = reader.GetString("nombre_persona");
                            crd.Rol = reader.GetString("rol");
                            ListaCreditos.Add(crd);
                        }
                    }
                }
            }
            return ListaCreditos;
        }

        //Obtener Licencias
        public List<Licencias> obtenerLicencias()
        {

            Licencias lcn;
            List<Licencias> ListaLicencias = new List<Licencias>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string queryLicencias = @"SELECT * FROM licencias;";


                using (MySqlCommand cmd = new MySqlCommand(queryLicencias, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            lcn = new Licencias();
                            lcn.Id = reader.GetInt32("Id");
                            lcn.NombreRecurso = reader.GetString("nombre_recurso");
                            lcn.Tipo = reader.GetString("Tipo");
                            lcn.FuenteUrl = reader.GetString("fuente_url");
                            lcn.Atribucion = reader.GetString("atribucion");
                            ListaLicencias.Add(lcn);
                        }
                    }
                }
            }
            return ListaLicencias;
        }

        //Obtener Objetivo
        public List<Objetivo> obtenerObjetivo()
        {

            Objetivo obj;
            List<Objetivo> ListaObjetivo = new List<Objetivo>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string queryObjetivo = @"SELECT * FROM objetivos;";


                using (MySqlCommand cmd = new MySqlCommand(queryObjetivo, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            obj = new Objetivo();
                            obj.Id = reader.GetInt32("Id");
                            obj.VideojuegoId = reader.GetInt32("videojuego_id");
                            obj.Tipo = reader.GetString("tipo");
                            obj.Descripcion = reader.GetString("Descripcion");
                            ListaObjetivo.Add(obj);
                        }
                    }
                }
            }
            return ListaObjetivo;
        }

        public int? getEmpleadoId(string nombreUsuario)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT id_empleado FROM Usuario WHERE nombre_usuario = @nombreUsuario";
                
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32("id_empleado");
                        }
                    }
                }
            }
            return null;
        }

    }
}