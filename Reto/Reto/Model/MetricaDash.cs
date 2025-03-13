
namespace Reto.Model
{
    public class MetricasDash{
        
        public int idmetricasUsuario {get; set;}
        public int id_usuario {get; set;}
        public float ingresos { get; set; }
        public float ganancias { get; set; } 
        public int ordenes { get; set; } 
        public int visistas { get; set; } 
        public DateTime fecha {get;set;}
        

        public MetricasDash(int idmetricasUsuario_ , int id_usuario_, float ingresos_, float ganancias_, int ordenes_, int visitas_, DateTime fecha_){
            this.idmetricasUsuario = idmetricasUsuario_;
            this.id_usuario = id_usuario_;
            this.ingresos = ingresos_;
            this.ganancias = ganancias_;
            this.ordenes = ordenes_;
            this.visistas = visitas_;
            this.fecha = fecha_;
        }
    }
    
}