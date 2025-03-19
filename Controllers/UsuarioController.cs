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
                clsUsuario objUsuario = new clsUsuario(modelo.nombre,
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


        // endpoint para validación para usuarios vwrtpusuario
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
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario();

                // Aplicar filtro si se proporciona
                DataTable dt = ds.Tables[0];
                if (!string.IsNullOrEmpty(filtro))
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = $"nombre LIKE '%{filtro}%'"; // Filtra por coincidencias parciales en 'nombre'
                    dt = dv.ToTable(); // Convierte el DataView filtrado en un DataTable
                }

                //configuracion del objeto de salida 
                objRespuesta.statusExec = true;
                objRespuesta.ban = dt.Rows.Count;
                objRespuesta.msg = "Reporte consultado exitosamente";

                // Convertir el DataTable filtrado a JSON
                string jsonString = JsonConvert.SerializeObject(dt, Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{dt.TableName}\": {jsonString}}}");

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



    }
}
