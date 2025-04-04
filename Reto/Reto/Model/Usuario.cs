public class Usuario
{
	public int id_usuario {get;set;}
	public string nombre_usuario {get;set;}
	public string password {get;set;}
	public int puntuacion {get;set;}
	public int nivel {get;set;}
	public int retos_completados {get;set;}
	public string correo {get;set;}
	public int id_empleado {get;set;}
	public string imagen { get; set; }

	public Usuario(int id_usuario_,string nombre_usuario_,string password_,int puntuacion_,int nivel_,int retos_completados_,string correo_,int id_empleado_, string imagen_)
	{
		this.id_usuario = id_usuario_;
		this.nombre_usuario = nombre_usuario_;
		this.password = password_;
		this.puntuacion = puntuacion_;
		this.nivel = nivel_;
		this.retos_completados = retos_completados_;
		this.correo = correo_;
		this.id_empleado = id_empleado_;
		this.imagen = imagen_;
	}
}