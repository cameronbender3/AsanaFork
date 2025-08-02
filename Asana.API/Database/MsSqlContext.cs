
using Asana.Library.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.SqlTypes;

namespace Asana.API.Database
{
	public class MsSqlContext
	{
		private string _connectionString;
		public MsSqlContext()
		{
			_connectionString = $"Server=localhost,1433;Database=ASANADB;User Id=sa;Password=;TrustServerCertificate=True;";
		}

		private List<Project> _projects;


		public List<Project> Projects
		{
			get
			{
				_projects = new List<Project>();
				using (var sqlConnection = new SqlConnection(_connectionString))
				{
					using (var sqlCmd = sqlConnection.CreateCommand())
					{
						sqlCmd.CommandText = "select * from Project";
						sqlCmd.CommandType = System.Data.CommandType.Text;

						sqlConnection.Open();
						var reader = sqlCmd.ExecuteReader();
						while (reader.Read())
						{
							Project project = new Project
							{
								Id = int.Parse(reader["ID"].ToString()),
								Name = reader["NAME"].ToString(),
								Description = reader["DESCRIPTION"].ToString(),
								CompletePercent = int.Parse(reader["COMPLETE_PERCENT"].ToString()),

							};
							_projects.Add(project);
						}

						sqlConnection.Close();
						sqlCmd.Dispose();
						sqlConnection.Dispose();
						return _projects;
					}
				}
			}
		}
		private List<ToDo> _toDos;
		public List<ToDo> ToDos
		{
			get
			{
				_toDos = new List<ToDo>();
				using (var sqlConnection = new SqlConnection(_connectionString))
				{
					using (var sqlCmd = sqlConnection.CreateCommand())
					{
						sqlCmd.CommandText = "select * from ToDo";
						sqlCmd.CommandType = System.Data.CommandType.Text;

						sqlConnection.Open();
						var reader = sqlCmd.ExecuteReader();
						while (reader.Read())
						{
							ToDo toDo = new ToDo
							{
								Id = int.Parse(reader["ID"].ToString()),
								Name = reader["NAME"].ToString(),
								Description = reader["DESCRIPTION"].ToString(),
								IsCompleted = bool.Parse(reader["IS_COMPLETED"].ToString()),
								Priority = int.Parse(reader["PRIORITY"].ToString()),

							};
							_toDos.Add(toDo);
						}

						sqlConnection.Close();
						sqlCmd.Dispose();
						sqlConnection.Dispose();
						return _toDos;
					}
				}
			}
		}


		public List<Project> Expanded()
		{
			var projects = new List<Project>();
			var projectDict = new Dictionary<int, Project>();
			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = connection.CreateCommand())
				{

					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "ProjectExpandAll";
					connection.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							int projId = Convert.ToInt32(reader["ID"]);
							if (!projectDict.ContainsKey(projId))
							{
								var project = new Project
								{
									Id = projId,
									Name = reader["NAME"].ToString(),
									Description = reader["DESCRIPTION"].ToString(),
									CompletePercent = Convert.ToInt32(reader["COMPLETE_PERCENT"]),
									ToDos = new List<ToDo>()
								};
								projectDict[projId] = project;
								projects.Add(project);
							}

							if (reader["ToDoID"] != DBNull.Value)
							{
								var todo = new ToDo
								{
									Id = Convert.ToInt32(reader["ToDoID"]),
									Name = reader["ToDoName"].ToString(),
									Description = reader["ToDoDescription"].ToString(),
									IsCompleted = Convert.ToBoolean(reader["IS_COMPLETED"]),
									Priority = Convert.ToInt32(reader["PRIORITY"]),
								};
								projectDict[projId]?.ToDos?.Add(todo);
							}
						}
					}

				}
			}
							return projects;
			}

		public Project? ExpandSingle(int projectId)
		{
			Project? project = null;

			using (var connection = new SqlConnection(_connectionString))
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.CommandText = "ProjectExpandById";
				cmd.Parameters.Add(new SqlParameter("@id", projectId));
				connection.Open();

				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						project = new Project
						{
							Id = Convert.ToInt32(reader["ID"]),
							Name = reader["NAME"].ToString(),
							Description = reader["DESCRIPTION"].ToString(),
							CompletePercent = Convert.ToInt32(reader["COMPLETE_PERCENT"]),
							ToDos = new List<ToDo>()
						};
					}

					if (reader.NextResult() && project != null)
					{
						while (reader.Read())
						{
							var todo = new ToDo
							{
								Id = Convert.ToInt32(reader["ID"]),
								Name = reader["NAME"].ToString(),
								Description = reader["DESCRIPTION"].ToString(),
								IsCompleted = Convert.ToBoolean(reader["IS_COMPLETED"]),
								Priority = Convert.ToInt32(reader["PRIORITY"]),
							};
							project?.ToDos?.Add(todo);
						}
					}
				}
			}
			return project;
		}


		public int DeleteProject(int projectId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = connection.CreateCommand())
				{

					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "ProjectDelete";
					cmd.Parameters.Add(new SqlParameter("@id", projectId));

					connection.Open();
					var result = cmd.ExecuteScalar();
				}
			}

			return projectId;
		}
		public Project? AddOrUpdateProject(Project? project)
		{
			if (project == null) return project;
			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					SqlParameter? idParameter = null;
					//set up the command
					if (project.Id <= 0)
					{
						//Insert command
						cmd.CommandText = "ProjectInsert";
						idParameter = new SqlParameter("@id", project.Id) { Direction = System.Data.ParameterDirection.Output };
						cmd.Parameters.Add(idParameter);

					}
					else
					{
						//update command
						cmd.CommandText = "ProjectUpdate";
						cmd.Parameters.Add(new SqlParameter("@id", project.Id));
					}

					cmd.Parameters.Add(new SqlParameter("@projectName", project.Name));
					cmd.Parameters.Add(new SqlParameter("@description", project.Description));
					cmd.Parameters.Add(new SqlParameter("@complete_percent", project.CompletePercent));

					connection.Open();
					var result = cmd.ExecuteScalar();
					if (idParameter != null)
					{
						project.Id = int.Parse(idParameter.Value?.ToString() ?? "0");
					}
				}
			}

			return project;
		}
		public ToDo? AddOrUpdateToDo(ToDo? toDo)
		{
			if (toDo == null) return toDo;

			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					SqlParameter? idParameter = null;
					//set up the command
					if (toDo.Id <= 0)
					{
						//Insert command
						cmd.CommandText = "ToDoInsert";
						idParameter = new SqlParameter("@id", toDo.Id) { Direction = System.Data.ParameterDirection.Output };
						cmd.Parameters.Add(idParameter);

					}
					else
					{
						//update command
						cmd.CommandText = "ToDoUpdate";
						cmd.Parameters.Add(new SqlParameter("@id", toDo.Id));
					}

					cmd.Parameters.Add(new SqlParameter("@todoName", toDo.Name));
					cmd.Parameters.Add(new SqlParameter("@description", toDo.Description));
					cmd.Parameters.Add(new SqlParameter("@isCompleted", toDo.IsCompleted));
					cmd.Parameters.Add(new SqlParameter("@priority", toDo.Priority));
					cmd.Parameters.Add(new SqlParameter("@DueDate", toDo.DueDate));
					cmd.Parameters.Add(new SqlParameter("@projectId", toDo.ProjectId));


					connection.Open();
					var result = cmd.ExecuteScalar();
					if (idParameter != null)
					{
						toDo.Id = int.Parse(idParameter.Value?.ToString() ?? "0");
					}
				}
			}

			return toDo;
		}

		public int DeleteToDo(int toDoId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				using (var cmd = connection.CreateCommand())
				{
					
					cmd.CommandType = System.Data.CommandType.StoredProcedure;
					cmd.CommandText = "ToDoDelete";
					cmd.Parameters.Add(new SqlParameter("@id", toDoId));

					connection.Open();
					var result = cmd.ExecuteScalar();
				}
			}

			return toDoId;
		}

	}
}