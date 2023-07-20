using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasculasMPrimaNQ
{
    class Conexion
    {
        private MySqlConnection conexion;
        private string server = "10.56.0.54";
        private string database = "BasculasAlmacenNQ";
        private string user = "remote";
        private string password = "remoto";
        private string port = "3306";
        private string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = "Database=" + database +
            "; DataSource= " + server +
            "; Port= " + port +
            "; User Id= " + user +
            "; Password= " + password;


        }
        public MySqlConnection getConexion()
        {
            if (conexion == null)
            {

                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }
            return conexion;
        }
        public MySqlConnection close()
        {

            conexion.Close();

            return conexion;
        }
    }
}
