public class Usuario
{
    public int id_usuario {get;set;}
    public string nombre_usuario {get;set;}
    public string password {get;set;}
    public int monedas {get;set;}
    public int nivel {get;set;}
    public int retos_completados {get;set;}
    public string correo {get;set;}
    public int id_empleado {get;set;}
    public string rol {get; set;}
    public string quote {get; set;}
    public string imagen { get; set;}
    public string ubicacion { get; set;}
    public string telefono { get; set;}



    public Usuario(){}
    

    public Usuario(int id_usuario_,string nombre_usuario_,string password_,int monedas_,int nivel_,int retos_completados_,string correo_,int id_empleado_, string rol_, string quote_, string imagen_, string ubicacion_, string telefono_)
    {
        this.id_usuario = id_usuario_;
        this.nombre_usuario = nombre_usuario_;
        this.password = password_;
        this.monedas = monedas_;
        this.nivel = nivel_;
        this.retos_completados = retos_completados_;
        this.correo = correo_;
        this.id_empleado = id_empleado_;
        this.rol = rol_;
        this.quote = quote_;
        this.ubicacion = ubicacion_;
        this.telefono = telefono_;
        this.imagen = imagen_;
    }
}