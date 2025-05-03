using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//----------------------
using apiRESTCheckUsuario.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;


namespace apiRESTCheckUsuario.Controllers
{
    public class UsuarioController : ApiController {

        [HttpPost]
        [Route("check/usuario/spinsusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo) {
            //Definicion de los objetos de modelos
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            //---------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario(modelo.cve,
                                                       modelo.nombre,
                                                       modelo.apellidoPaterno,
                                                       modelo.apellidoMaterno,
                                                       modelo.usuario,
                                                       modelo.contrasena,
                                                       modelo.ruta,
                                                       modelo.tipo);
                ds = objUsuario.spInsUsuario();

                //configuracion del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse
                                        (ds.Tables[0].Rows[0][0].ToString());

                //Validar valor de ban
                if (objRespuesta.ban == 0){

                    objRespuesta.msg = "Usuario registrado exitosamente";
                    jsonResp.Add("msgData", "Usuario registrado exitosamente");
                }

                else {
                    objRespuesta.msg = "Usuario no registrado, verificar...";
                    jsonResp.Add("msgData", "Usuario no registrado, verificar...");

                }

                objRespuesta.datos = jsonResp;
            }

            catch(Exception ex){
                //configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Falló la inserción de usuario, verificar...";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            //salida del objeto ObjRespuesta
            return objRespuesta;
        }

        // endpoint para validación de acceso spValidarAcceso
        [HttpPost]
        [Route("check/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                // Creación del objeto del modelo clsUsuario
                clsUsuario objUsuario = new clsUsuario(modelo.usuario,
                                                       modelo.contrasena);
                ds = objUsuario.spvalidarAcceso();

                // Configuración del objeto de salida

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                //validar el valor recibido en bandera
                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Acceso denegado, verificar...";
                    jsonResp.Add("msgData", "Acceso denegado, verificar...");
                    objRespuesta.datos = jsonResp;
                }

            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida

                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;

            }

            //retorno del obj de salida objespuesta
            return objRespuesta;

        }//fin del endpoint


        // Endpoint para los usuarios (filtro nombre y usuario)
        [HttpGet]
        [Route("check/usuario/vwRptUsuario")]
        public clsApiStatus vwRptUsuario(string filtro = "")
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();

            try
            {
                // Crear el objeto del modelo clsUsuario
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario(); // Obtener todos los usuarios

                // Si se ha proporcionado un filtro, aplicamos el filtro en nombre y usuario
                DataTable dt = ds.Tables[0];
                if (!string.IsNullOrEmpty(filtro))
                {
                    // Filtro para buscar en 'nombre' o 'usuario'
                    string filtroFinal = $"nombre LIKE '%{filtro}%' OR usuario LIKE '%{filtro}%'";

                    // Aplicar el filtro al DataView
                    DataView dv = new DataView(dt);
                    dv.RowFilter = filtroFinal;
                    dt = dv.ToTable(); // Convertir el DataView filtrado en DataTable
                }

                // Configuración del objeto de salida 
                objRespuesta.statusExec = true;
                objRespuesta.ban = dt.Rows.Count; // Número de registros filtrados
                objRespuesta.msg = "Reporte consultado exitosamente";

                // Convertir el DataTable filtrado a JSON
                string jsonString = JsonConvert.SerializeObject(dt, Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{dt.TableName}\": {jsonString}}}");

                objRespuesta.datos = jsonResp; // Asignar los datos a la respuesta
            }
            catch (Exception ex)
            {
                // En caso de error, configurar el objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            // Retorno del objeto de salida
            return objRespuesta;
        }



        // endpoint para validación para usuarios vwrtpusuario
        [HttpGet]
            [Route("check/usuario/vwTipoUsuario")]
            public clsApiStatus vwTipoUsuario()
            {
                // -----------------------------------------
                clsApiStatus objRespuesta = new clsApiStatus();
                JObject jsonResp = new JObject();
                // -----------------------------------------
                DataSet ds = new DataSet();

                try
                {
                    clsUsuario objUsuario = new clsUsuario();
                    ds = objUsuario.vwTipoUsuario();
                    //configuracion del objeto de salida 
                    objRespuesta.statusExec = true;
                    objRespuesta.ban = ds.Tables[0].Rows.Count;
                    objRespuesta.msg = "Reporte consultado exitosamente";
                    //formatear los datos recibidos del dataset para
                    //enviarlos de salida json
                    // Migración del ds(DataSet) al objeto Json
                    string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");


                    objRespuesta.datos = jsonResp;
                }
                catch (Exception ex)
                {
                    // Configuración del objeto de salida

                    objRespuesta.statusExec = false;
                    objRespuesta.ban = -1;
                    objRespuesta.msg = "Error de conexión con el servicio de datos";
                    jsonResp.Add("msgData", ex.Message.ToString());
                    objRespuesta.datos = jsonResp;
                }
                //retorno del objeto de salida
                return objRespuesta;

            }


        //endpoint ELIMINAR
        [HttpPost]
        [Route("check/usuario/spDelUsuario")]
        public clsApiStatus deleteUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Usa directamente el modelo que ya tiene la cve del usuario
                ds = modelo.spDelUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario No eliminado";
                    jsonResp.Add("usu_cve", ds.Tables[0].Rows[0][1].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Usuario Eliminado exitosamente";
                    jsonResp.Add("msgData", "Usuario no Eliminado, verificar");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }


        //CONSULTA
        [HttpGet]
        [Route("check/usuario/vwRptUsuariocve")]
        public clsApiStatus vwRptUsuariocveo(string filtro)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuariocve(filtro);
                //Configuracion del objSalida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";
                //Formatear los datos recibidos (Data set) para 
                //enviarlos de salida(JSON)
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                //Configuracion del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexion con el servicio de datos";
                jsonResp.Add("msData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            return objRespuesta;
        }

        //MODIFICAR
        [HttpPost]
        [Route("check/usuario/spUpdUsuario")]
        public clsApiStatus UpdUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                ds = modelo.spUpdUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario actualizado exitosamente";
                    jsonResp.Add("usu_cve", ds.Tables[0].Rows[0][1].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "No se actualizó el usuario, verificar datos";
                    jsonResp.Add("msgData", "Usuario no modificado");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }
    }
}
