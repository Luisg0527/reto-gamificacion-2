using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;


namespace Reto.Model
{
    public class DataBaseContext{
        public string ConnectionString { get; set; }
        public DataBaseContext(){
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=OxxoDB;Uid=root;password=Rayo2008;";
        }

        private MySqlConnection GetConnection(){
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
                                FROM metricasusuario 
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

        public bool verificarUsuario(string _nombre) {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT COUNT(*) AS existe FROM Usuario WHERE nombre_usuario = '" + _nombre + "'";
            bool existe = false;
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);

            using (MySqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read())
                {
                    existe = reader.GetInt32("existe") > 0;
                }
            }
            conexion.Close();
            return existe;
        }

        public bool verificarContra(string _nombre, string _contra) {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT COUNT(*) AS existe FROM Usuario WHERE nombre_usuario = '" + _nombre + "' AND password = '" + _contra + "'";
            bool ContraExiste = false;
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);

            using (MySqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read())
                {
                    ContraExiste = reader.GetInt32("existe") > 0;
                }
            }
            conexion.Close();
            return ContraExiste;
        }

        public void crearUsuario(int _empId, string _nombre, string _contra, string _correo) {
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            string cmdString = "SELECT MAX(id_usuario) AS last FROM Usuario";
            int lastId = 0;
            
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);
            using (MySqlDataReader reader = cmd.ExecuteReader()) {
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

        public void cambiarContra(string _usuario, string _nuevaContra) {
            MySqlConnection conexion = GetConnection();
            conexion.Open();

            string cmdString = "UPDATE Usuario SET password = @nuevaContraseña WHERE nombre_usuario = '" + _usuario + "'";
            MySqlCommand cmd = new MySqlCommand(cmdString, conexion);
            cmd.Parameters.AddWithValue("@nuevaContraseña", _nuevaContra);

            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}