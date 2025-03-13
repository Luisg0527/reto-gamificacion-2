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
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxodb;Uid=root;password=Paolo2004;";
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

    }
}