using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_DataService.Repositories
{
    public class UserRepository(SqlConnectionFactory factory) : IUserRepository
    {
        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            await using var conn = factory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = SqlConnectionFactory.CreateProcedureCommand(conn, "dbo.GetUserByUsername");
            cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 100).Value = username;

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct))
                return null;

            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = reader.GetString(reader.GetOrdinal("Role"))
            };
        }
    }
}
