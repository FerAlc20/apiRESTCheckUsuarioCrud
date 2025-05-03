using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;//conectarnos a web config
using System.Data;//recibir y enviar objetos contenedores de datos
using MySql.Data.MySqlClient;

namespace apiRESTCheckUsuario.Models
{
    public class clsUsuario {

        // Definición de atributos
        public string cve { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public string tipo { get; set; }

        //Métodos y atributos de funcionalidad y seguridad
        string cadConn = ConfigurationManager.ConnectionStrings["bd_control_acceso"].ConnectionString;

        //Constructores de la clase que se usan en los endpoints
        //primer constructor para las vistas
        public clsUsuario(){

        }


        //segundo constructor para acceso
        public clsUsuario(string usuario,string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }

        

        //tercer constructor para todos los datos
        public clsUsuario(
                          string cve,
                          string nombre,
                          string apellidoPaterno,
                          string apellidoMaterno,
                          string usuario,
                          string contrasena,
                          string ruta,
                          string tipo)
        {
            this.cve = cve;
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoPaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        //Metodo para la ejecución del spInUsuario
        public DataSet spInsUsuario() {

            string cadSql = "CALL spInsUsuario('" + this.nombre +
                                                "', '" + this.apellidoPaterno +
                                                "', '" + this.apellidoMaterno + 
                                                "', '" + this.usuario +
                                                "', '" + this.contrasena + 
                                                "', '" + this.ruta + 
                                                "',  " + this.tipo + ")";


            // Configuración de los objetos de conexión a datos
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();

            //Ejecucion del adaptador de datos (retorna un DataSet)
            da.Fill(ds, "spInsUsuario");

            //retorno de los datos recibidos
            return ds;
        }
        // Proceso de validación de usuarios (spValidarAcceso)
        public DataSet spvalidarAcceso()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spvalidarAcceso('" + this.usuario + "','"
                                              + this.contrasena + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spvalidarAcceso");
            return ds;
        }


        // Proceso de consulta de usuarios (viewrtpusuario)
        public DataSet vwRptUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "SELECT * FROM vwRptUsuario";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwRptUsuario");
            return ds;
        }

        // Proceso de consulta de Tipo de Usuarios (vwtipousuario)
        public DataSet vwTipoUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "SELECT * FROM vwTipoUsuario";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwTipoUsuario");
            return ds;
        }

        public DataSet spDelUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spDelUsuario('" + this.cve + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spDelUsuario");
            return ds;
        }

        public DataSet vwRptUsuariocve(string filtro)
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "select * from control_acceso.usuario where USU_CVE_USUARIO like @filtro";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            da.SelectCommand.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwRptUsuario");
            return ds;
        }


        public DataSet spUpdUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spUpdUsuario(" + this.cve + ", '" + this.nombre + "', '" + this.apellidoPaterno +
        "', '" + this.apellidoMaterno + "', '" + this.usuario +
        "', '" + this.contrasena + "', '" + this.ruta +
        "', " + this.tipo + ")";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spUpdUsuario");
            return ds;
        }
    }
}