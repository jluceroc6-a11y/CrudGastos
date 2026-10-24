using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CrudGastos
{
    internal class RepositorioGastos
    {
        private string conexion =
            "Server=(localdb)\\MSSQLLocalDB;Database=GastosBD;Trusted_Connection=True;";

        public void Insertar(Gastos gasto)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = "INSERT INTO gastos (descripcion, monto, categoria) " +
                             "VALUES (@descripcion, @monto, @categoria)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@descripcion", gasto.Descripcion);
                    cmd.Parameters.AddWithValue("@monto", gasto.Monto);
                    cmd.Parameters.AddWithValue("@categoria", gasto.Categoria);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Gastos> Listar()
        {
            List<Gastos> lista = new List<Gastos>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = "SELECT id, descripcion, monto, categoria FROM gastos";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Gastos gasto = new Gastos();

                        gasto.Id = Convert.ToInt32(reader["id"]);
                        gasto.Descripcion = reader["descripcion"].ToString();
                        gasto.Monto = Convert.ToDecimal(reader["monto"]);
                        gasto.Categoria = reader["categoria"].ToString();

                        lista.Add(gasto);
                    }
                }
            }

            return lista;
        }

        public Gastos BuscarPorId(int id)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = "SELECT id, descripcion, monto, categoria " +
                             "FROM gastos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Gastos gasto = new Gastos();

                            gasto.Id = Convert.ToInt32(reader["id"]);
                            gasto.Descripcion = reader["descripcion"].ToString();
                            gasto.Monto = Convert.ToDecimal(reader["monto"]);
                            gasto.Categoria = reader["categoria"].ToString();

                            return gasto;
                        }
                    }
                }
            }

            return null;
        }

        public bool ActualizarMonto(int id, decimal nuevoMonto)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = "UPDATE gastos SET monto = @monto WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@monto", nuevoMonto);
                    cmd.Parameters.AddWithValue("@id", id);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();

                string sql = "DELETE FROM gastos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
        }
    }
}