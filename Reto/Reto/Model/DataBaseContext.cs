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

        public MetricasDash getMetricas(){
            MetricasDash metricas = new MetricasDash(0,0,0,0,0,0);
            using(MySqlConnection connection = GetConnection()){
                connection.Open();
                string query = "Select * from metricasusuario where id_usuario = 1";

                using (MySqlCommand cmd = new MySqlCommand (query,connection)){
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            metricas = new MetricasDash(
                            
                                reader.GetInt32("idmetricasUsuario"),
                                reader.GetInt32("id_usuario"),
                                reader.GetFloat("ingresos"),
                                reader.GetFloat("ganancias"),
                                reader.GetInt32("ordenes"),
                                reader.GetInt32("visitas")
                            );
                        }
                    }

                }
            }
            return metricas;
            
        }
    }
}