using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaPasantia.Models
{
	public class RepositorioAuto : RepositorioBase, IRepositorioAuto
	{
		public RepositorioAuto(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Auto e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Autos (Patente, Marca, Modelo, Año, Kms, Foto1, Foto2) " +
					$"VALUES (@Patente,@Marca, @Modelo, @Año, @Kms, @Foto1, @Foto2);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Patente", e.Patente);
					command.Parameters.AddWithValue("@Marca", e.Marca);
					command.Parameters.AddWithValue("@Modelo", e.Modelo);
					
					command.Parameters.AddWithValue("@Año", e.Año);
					command.Parameters.AddWithValue("@Kms", e.Kms);

					if (String.IsNullOrEmpty(e.Foto1))
						command.Parameters.AddWithValue("@Foto1", DBNull.Value);
					else
						command.Parameters.AddWithValue("@Foto1", e.Foto1);

					if (String.IsNullOrEmpty(e.Foto2))
						command.Parameters.AddWithValue("@Foto2", DBNull.Value);
					else
						command.Parameters.AddWithValue("@Foto2", e.Foto2);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.IdAuto = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Autos WHERE IdAuto = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Auto e)
		{
			int res = -1;

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Autos SET Patente=@Patente, Marca=@Marca, Modelo=@Modelo, Kms=@Kms, Año=@Año, Foto1=@Foto1, Foto2=@Foto2 " +
					"WHERE IdAuto = @IdAuto";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Patente", e.Patente);
					command.Parameters.AddWithValue("@Marca", e.Marca);
					command.Parameters.AddWithValue("@Modelo", e.Modelo);
					command.Parameters.AddWithValue("@Kms", e.Kms);
					command.Parameters.AddWithValue("@Año", e.Año);
					command.Parameters.AddWithValue("@Foto1", e.Foto1);
					command.Parameters.AddWithValue("@Foto2", e.Foto2);
					command.Parameters.AddWithValue("@IdAuto", e.IdAuto);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Auto> ObtenerTodos()
		{
			IList<Auto> res = new List<Auto>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdAuto, Patente, Marca, Modelo, Año, Kms, Foto1, Foto2" +
					$" FROM Autos";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Auto e = new Auto
						{
							IdAuto = reader.GetInt32(0),
							Patente = reader.GetString(1),
							Marca = reader.GetString(2),
							Modelo = reader.GetString(3),
							Año = reader.GetInt32(4),
							Kms = reader.GetInt32(5),
							Foto1 = reader["Foto1"].ToString(),
							Foto2 = reader["Foto2"].ToString(),
							
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Auto ObtenerPorId(int id)
		{
			Auto e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdAuto, Patente, Marca, Modelo, Año, Kms, Foto1,Foto2 FROM Autos" +
					$" WHERE IdAuto=@IdAuto";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@IdAuto", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Auto
						{
							IdAuto = reader.GetInt32(0),
							Patente = reader.GetString(1),
							Marca = reader.GetString(2),
							Modelo = reader.GetString(3),

							Año = reader.GetInt32(4),
							Kms = reader.GetInt32(5),
							Foto1 = reader["Foto1"].ToString(),
							Foto2 = reader["Foto2"].ToString(),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		
	}
}
