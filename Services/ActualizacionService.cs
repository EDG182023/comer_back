using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TarifarioBackend.Data;
using TarifarioBackend.Models;
using System.Data;
using Microsoft.AspNetCore.Http; // Necesario para IFormFile
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using ClosedXML.Excel;
using System.IO;
using TarifarioBackend.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarifarioBackend.Services
{
    public class ActualizacionService
    {
        private readonly SaadisNuevoDbContext _saadisNuevoContext;
        private readonly GestionTarifasDbContext _context;
        private readonly SqlHelper _sqlHelper;
        private readonly ILogger<ActualizacionService> _logger;

        public ActualizacionService(SaadisNuevoDbContext saadisNuevoContext, GestionTarifasDbContext context, SqlHelper sqlHelper, ILogger<ActualizacionService> logger)
        {
            _saadisNuevoContext = saadisNuevoContext;
            _context = context;
            _sqlHelper = sqlHelper;
            _logger = logger;
        }

        public async Task<object> PerformMassUpdateAsync(ActualizacionMasivaRequest request)
        {
            if (request.FechaInicio == default)
            {
                throw new InvalidOperationException("FechaInicio es requerida.");
            }

            IQueryable<TarifarioGeneral1> queryGeneral1 = _context.TarifarioGeneral1;
            IQueryable<TarifarioGeneral2> queryGeneral2 = _context.TarifarioGeneral2;
            IQueryable<TarifarioEcommerce> queryEcommerce = _context.TarifarioEcommerce;

            if (request.FechaFin.HasValue)
            {
                queryGeneral1 = queryGeneral1.Where(t => t.FechaVigenciaInicio >= request.FechaInicio && t.FechaVigenciaInicio <= request.FechaFin.Value);
                queryGeneral2 = queryGeneral2.Where(t => t.FechaVigenciaInicio >= request.FechaInicio && t.FechaVigenciaInicio <= request.FechaFin.Value);
                queryEcommerce = queryEcommerce.Where(t => t.FechaVigenciaInicio >= request.FechaInicio && t.FechaVigenciaInicio <= request.FechaFin.Value);
            }
            else
            {
                queryGeneral1 = queryGeneral1.Where(t => t.FechaVigenciaInicio >= request.FechaInicio);
                queryGeneral2 = queryGeneral2.Where(t => t.FechaVigenciaInicio >= request.FechaInicio);
                queryEcommerce = queryEcommerce.Where(t => t.FechaVigenciaInicio >= request.FechaInicio);
            }

            if (request.IncluirCliente && request.ClienteId.HasValue)
            {
                queryGeneral1 = queryGeneral1.Where(t => t.ClienteId == request.ClienteId.Value);
                queryGeneral2 = queryGeneral2.Where(t => t.ClienteId == request.ClienteId.Value);
                queryEcommerce = queryEcommerce.Where(t => t.ClienteId == request.ClienteId.Value);
            }

            int updatedCount = 0;

            // Update TarifarioGeneral1
            var tarifariosGeneral1 = await queryGeneral1.ToListAsync();
            foreach (var tarifario in tarifariosGeneral1)
            {
                // Create historical record
                _context.TarifarioGeneralHistorico.Add(new TarifarioGeneralHistorico
                {
                    TarifarioGeneralId = tarifario.Id,
                    ClienteId = tarifario.ClienteId,
                    UnidadId = tarifario.UnidadId,
                    ItemId = tarifario.ItemId,
                    Precio = tarifario.Precio,
                    Minimo = tarifario.Minimo,
                    Incremento = tarifario.Incremento,
                    FechaVigenciaInicio = tarifario.FechaVigenciaInicio,
                    FechaVigenciaFinal = tarifario.FechaVigenciaFinal,
                    UsuarioActualizacion = request.Usuario,
                    FechaCreacion = DateTime.Now
                });

                tarifario.Precio *= (1 + request.Porcentaje / 100);
                tarifario.Incremento *= (1 + request.Porcentaje / 100);
                tarifario.FechaActualizacion = DateTime.Now;
                updatedCount++;
            }

            // Update TarifarioGeneral2
            var tarifariosGeneral2 = await queryGeneral2.ToListAsync();
            foreach (var tarifario in tarifariosGeneral2)
            {
                // Create historical record
                _context.TarifarioGeneralHistorico.Add(new TarifarioGeneralHistorico
                {
                    TarifarioGeneralId = tarifario.Id, // Assuming this maps to TarifarioGeneral2 for history
                    ClienteId = tarifario.ClienteId,
                    UnidadId = tarifario.UnidadId,
                    ItemId = tarifario.ItemId,
                    Precio = tarifario.Precio,
                    Minimo = null, // Not applicable for General2
                    Incremento = tarifario.Incremento,
                    FechaVigenciaInicio = tarifario.FechaVigenciaInicio,
                    FechaVigenciaFinal = tarifario.FechaVigenciaFinal,
                    UsuarioActualizacion = request.Usuario,
                    FechaCreacion = DateTime.Now
                });

                tarifario.Precio *= (1 + request.Porcentaje / 100);
                tarifario.Incremento *= (1 + request.Porcentaje / 100);
                tarifario.FechaActualizacion = DateTime.Now;
                updatedCount++;
            }

            // Update TarifarioEcommerce
            var tarifariosEcommerce = await queryEcommerce.ToListAsync();
            foreach (var tarifario in tarifariosEcommerce)
            {
                // Create historical record
                _context.TarifarioEcommerceHistorico.Add(new TarifarioEcommerceHistorico
                {
                    TarifarioEcommerceId = tarifario.Id,
                    Localidad = tarifario.Localidad,
                    UnidadId = tarifario.UnidadId,
                    ClienteId = tarifario.ClienteId,
                    PlazosEntrega = tarifario.PlazosEntrega,
                    RangoMin = tarifario.RangoMin,
                    RangoMax = tarifario.RangoMax,
                    Precio = tarifario.Precio,
                    FechaVigenciaInicio = tarifario.FechaVigenciaInicio,
                    FechaVigenciaFinal = tarifario.FechaVigenciaFinal,
                    Incremento = tarifario.Incremento,
                    UsuarioActualizacion = request.Usuario,
                    FechaCreacion = DateTime.Now
                });

                tarifario.Precio *= (1 + request.Porcentaje / 100);
                tarifario.Incremento *= (1 + request.Porcentaje / 100);
                tarifario.FechaActualizacion = DateTime.Now;
                updatedCount++;
            }

            await _context.SaveChangesAsync();

            return new { Message = $"Actualización masiva completada. {updatedCount} registros actualizados." };
        }

        public async Task<object> UpdateValPrepKilosAsync(ValPrepKilosRequest request)
        {
            var cliente = await _context.Clientes.FindAsync(request.ClienteId);
            if (cliente == null)
            {
                throw new InvalidOperationException($"Cliente con ID {request.ClienteId} no encontrado.");
            }

            // Find the specific item for "VAL PREP KILOS"
            var valPrepKilosItem = await _context.Items
                                                .FirstOrDefaultAsync(i => i.Nombre == "VAL PREP KILOS");

            if (valPrepKilosItem == null)
            {
                throw new InvalidOperationException("Item 'VAL PREP KILOS' no encontrado. Asegúrese de que existe en la tabla Items.");
            }

            // Find existing TarifarioGeneral1 for this client and item
            var existingTarifario = await _context.TarifarioGeneral1
                                                .FirstOrDefaultAsync(t => t.ClienteId == request.ClienteId &&
                                                                          t.ItemId == valPrepKilosItem.Id &&
                                                                          t.FechaVigenciaInicio == request.FechaInicio &&
                                                                          t.FechaVigenciaFinal == request.FechaFinal);

            if (existingTarifario != null)
            {
                // Update existing record
                existingTarifario.Precio = request.Valor;
                existingTarifario.Minimo = request.Minimo;
                existingTarifario.FechaActualizacion = DateTime.Now;
                _context.Entry(existingTarifario).State = EntityState.Modified;
            }
            else
            {
                // Add new record
                var newTarifario = new TarifarioGeneral1
                {
                    ClienteId = request.ClienteId,
                    UnidadId = 1, // Assuming a default unit ID, adjust as needed
                    ItemId = valPrepKilosItem.Id,
                    Precio = request.Valor,
                    Minimo = request.Minimo,
                    Incremento = 0, // Assuming no increment for this specific tariff
                    FechaVigenciaInicio = request.FechaInicio,
                    FechaVigenciaFinal = request.FechaFinal,
                    FechaCreacion = DateTime.Now
                };
                _context.TarifarioGeneral1.Add(newTarifario);
            }

            await _context.SaveChangesAsync();
            return new { Message = "Valores de preparación por kilos actualizados correctamente." };
        }

        public async Task<bool> UpdateClienteEmailsAsync(int clienteId, List<string> emails)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                return false;
            }

            cliente.Email = string.Join(";", emails); // Store multiple emails as a semicolon-separated string
            cliente.FechaActualizacion = DateTime.Now;

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ActualizacionMasivaTarifas(ActualizacionMasivaRequest datos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var criterio = datos.Criterio;
                var seleccionId = datos.SeleccionId;
                var incluirCliente = datos.IncluirCliente;
                var clienteId = datos.ClienteId;
                var fechaInicio = datos.FechaInicio;
                var fechaFin = datos.FechaFin;
                var porcentaje = datos.Porcentaje;
                var usuarioStr = datos.Usuario;

                // Mapeo de usuario string a int
                var usuarioMap = new Dictionary<string, int>
                {
                    { "admin", 0 },
                    { "Diego", 1 },
                    { "Thiago", 2 }
                };
                var usuario = usuarioMap.GetValueOrDefault(usuarioStr, -1); // -1 en caso de usuario desconocido

                var tarifasAfectadas = new List<dynamic>();

                // Process tarifariogeneral_1
                var queryGeneral = new System.Text.StringBuilder(@"
                    SELECT t.id, t.cliente_id, t.precio, t.fecha_vigencia_inicio, t.fecha_vigencia_final, 'general' as origen, t.item_id, t.unidad_id, t.minimo, t.Incremento
                    FROM tarifariogeneral_1 t
                    WHERE 1=1
                ");
                var paramsGeneral = new List<SqlParameter>();

                if (criterio == "cliente")
                {
                    queryGeneral.Append(" AND t.cliente_id = @seleccionId");
                    paramsGeneral.Add(new SqlParameter("@seleccionId", seleccionId));
                }
                if (incluirCliente && clienteId.HasValue)
                {
                    queryGeneral.Append(" AND t.cliente_id = @clienteId");
                    paramsGeneral.Add(new SqlParameter("@clienteId", clienteId.Value));
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = queryGeneral.ToString();
                    command.Parameters.AddRange(paramsGeneral.ToArray());
                    _context.Database.OpenConnection();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tarifasAfectadas.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("cliente_id")),
                                Precio = reader.GetDouble(reader.GetOrdinal("precio")),
                                FechaVigenciaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_inicio")),
                                FechaVigenciaFinal = reader.IsDBNull(reader.GetOrdinal("fecha_vigencia_final")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_final")),
                                Origen = "general",
                                ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                                UnidadId = reader.GetInt32(reader.GetOrdinal("unidad_id")),
                                Minimo = reader.IsDBNull(reader.GetOrdinal("minimo")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("minimo")),
                                Incremento = reader.GetDouble(reader.GetOrdinal("Incremento"))
                            });
                        }
                    }
                }

                // Process tarifarioecommerce
                var queryEcommerce = new System.Text.StringBuilder(@"
                    SELECT t.id, t.cliente_id, t.precio, t.fecha_vigencia_inicio, t.fecha_vigencia_final, 'ecommerce' as origen, t.localidad, t.unidad_id, t.plazos_entrega, t.rango_min, t.rango_max, t.incremento
                    FROM tarifarioecommerce t
                    WHERE 1=1
                ");
                var paramsEcommerce = new List<SqlParameter>();

                if (criterio == "cliente")
                {
                    queryEcommerce.Append(" AND t.cliente_id = @seleccionId");
                    paramsEcommerce.Add(new SqlParameter("@seleccionId", seleccionId));
                }
                if (incluirCliente && clienteId.HasValue)
                {
                    queryEcommerce.Append(" AND t.cliente_id = @clienteId");
                    paramsEcommerce.Add(new SqlParameter("@clienteId", clienteId.Value));
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = queryEcommerce.ToString();
                    command.Parameters.AddRange(paramsEcommerce.ToArray());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tarifasAfectadas.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("cliente_id")),
                                Precio = reader.GetDouble(reader.GetOrdinal("precio")),
                                FechaVigenciaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_inicio")),
                                FechaVigenciaFinal = reader.IsDBNull(reader.GetOrdinal("fecha_vigencia_final")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_final")),
                                Origen = "ecommerce",
                                Localidad = reader.GetString(reader.GetOrdinal("localidad")),
                                UnidadId = reader.GetInt32(reader.GetOrdinal("unidad_id")),
                                PlazosEntrega = reader.GetString(reader.GetOrdinal("plazos_entrega")),
                                RangoMin = reader.GetDouble(reader.GetOrdinal("rango_min")),
                                RangoMax = reader.GetDouble(reader.GetOrdinal("rango_max")),
                                Incremento = reader.GetDouble(reader.GetOrdinal("incremento"))
                            });
                        }
                    }
                }

                var factor = 1 + (porcentaje / 100);

                foreach (var tarifa in tarifasAfectadas)
                {
                    var nuevoPrecio = tarifa.Precio * factor;

                    if (tarifa.Origen == "general")
                    {
                        // Insert into tarifariogeneralhistorico
                        var insertHistoryQuery = @"
                            INSERT INTO tarifariogeneralhistorico (
                                [OriginalId], [item_id], [unidad_id], [cliente_id], [minimo], [precio], [fecha_vigencia_inicio], [fecha_vigencia_final], [Usuario], [Fecha_movimiento], [Accion], [Incremento]
                            )
                            VALUES (
                                @originalId, @item_id, @unidad_id, @cliente_id, @minimo, @precio, @fecha_vigencia_inicio, @fecha_vigencia_final, @usuario, GETDATE(), 1, @incremento
                            )
                        ";
                        var historyParams = new[]
                        {
                            new SqlParameter("@originalId", tarifa.Id),
                            new SqlParameter("@item_id", tarifa.ItemId),
                            new SqlParameter("@unidad_id", tarifa.UnidadId),
                            new SqlParameter("@cliente_id", tarifa.ClienteId),
                            new SqlParameter("@minimo", (object)tarifa.Minimo ?? DBNull.Value),
                            new SqlParameter("@precio", tarifa.Precio),
                            new SqlParameter("@fecha_vigencia_inicio", tarifa.FechaVigenciaInicio),
                            new SqlParameter("@fecha_vigencia_final", (object)tarifa.FechaVigenciaFinal ?? DBNull.Value),
                            new SqlParameter("@usuario", usuario),
                            new SqlParameter("@incremento", tarifa.Incremento)
                        };
                        await _context.Database.ExecuteSqlRawAsync(insertHistoryQuery, historyParams);

                        // Update tarifariogeneral_1
                        var updateQuery = @"
                            UPDATE tarifariogeneral_1
                            SET precio = @precio, fecha_vigencia_inicio = @fecha_vigencia_inicio, fecha_vigencia_final = @fecha_vigencia_final, Incremento = @incremento
                            WHERE id = @id
                        ";
                        var updateParams = new[]
                        {
                            new SqlParameter("@precio", nuevoPrecio),
                            new SqlParameter("@fecha_vigencia_inicio", fechaInicio),
                            new SqlParameter("@fecha_vigencia_final", fechaFin),
                            new SqlParameter("@incremento", porcentaje),
                            new SqlParameter("@id", tarifa.Id)
                        };
                        await _context.Database.ExecuteSqlRawAsync(updateQuery, updateParams);
                    }
                    else if (tarifa.Origen == "ecommerce")
                    {
                        // Insert into tarifarioecommerce_historico
                        var insertHistoryQuery = @"
                            INSERT INTO tarifarioecommerce_historico (
                                [OriginalId], [localidad], [unidad_id], [cliente_id], [plazos_entrega], [rango_min], [rango_max], [precio], [fecha_vigencia_inicio], [fecha_vigencia_final], [usuario], [fecha_movimiento], [accion], [incremento]
                            )
                            VALUES (
                                @originalId, @localidad, @unidad_id, @cliente_id, @plazos_entrega, @rango_min, @rango_max, @precio, @fecha_vigencia_inicio, @fecha_vigencia_final, @usuario, GETDATE(), 1, @incremento
                            )
                        ";
                        var historyParams = new[]
                        {
                            new SqlParameter("@originalId", tarifa.Id),
                            new SqlParameter("@localidad", tarifa.Localidad),
                            new SqlParameter("@unidad_id", tarifa.UnidadId),
                            new SqlParameter("@cliente_id", tarifa.ClienteId),
                            new SqlParameter("@plazos_entrega", tarifa.PlazosEntrega),
                            new SqlParameter("@rango_min", tarifa.RangoMin),
                            new SqlParameter("@rango_max", tarifa.RangoMax),
                            new SqlParameter("@precio", tarifa.Precio),
                            new SqlParameter("@fecha_vigencia_inicio", tarifa.FechaVigenciaInicio),
                            new SqlParameter("@fecha_vigencia_final", (object)tarifa.FechaVigenciaFinal ?? DBNull.Value),
                            new SqlParameter("@usuario", usuario),
                            new SqlParameter("@incremento", tarifa.Incremento)
                        };
                        await _context.Database.ExecuteSqlRawAsync(insertHistoryQuery, historyParams);

                        // Update tarifarioecommerce
                        var updateQuery = @"
                            UPDATE tarifarioecommerce
                            SET precio = @precio, fecha_vigencia_inicio = @fecha_vigencia_inicio, fecha_vigencia_final = @fecha_vigencia_final, incremento = @incremento
                            WHERE id = @id
                        ";
                        var updateParams = new[]
                        {
                            new SqlParameter("@precio", nuevoPrecio),
                            new SqlParameter("@fecha_vigencia_inicio", fechaInicio),
                            new SqlParameter("@fecha_vigencia_final", fechaFin),
                            new SqlParameter("@incremento", porcentaje),
                            new SqlParameter("@id", tarifa.Id)
                        };
                        await _context.Database.ExecuteSqlRawAsync(updateQuery, updateParams);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public async Task<bool> UpdateTarifasFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // Assuming data is in the first worksheet
                    var firstRowUsed = worksheet.FirstRowUsed();
                    var lastRowUsed = worksheet.LastRowUsed();

                    // Assuming headers are in the first row
                    var headerRow = firstRowUsed;

                    // Example: Read and update TarifarioGeneral1
                    // This is a simplified example. You'll need robust error handling and mapping.
                    for (int rowNum = headerRow.RowNumber() + 1; rowNum <= lastRowUsed.RowNumber(); rowNum++)
                    {
                        var row = worksheet.Row(rowNum);
                        try
                        {
                            var id = row.Cell("A").GetValue<int>(); // Assuming ID is in column A
                            var precio = row.Cell("B").GetValue<double>(); // Assuming Precio is in column B
                            var fechaVigenciaInicio = row.Cell("C").GetValue<DateTime>();
                            var fechaVigenciaFinal = row.Cell("D").GetValue<DateTime?>();
                            var incremento = row.Cell("E").GetValue<double>();

                            var tarifa = await _context.TarifarioGeneral1.FindAsync(id);
                            if (tarifa != null)
                            {
                                tarifa.Precio = precio;
                                tarifa.FechaVigenciaInicio = fechaVigenciaInicio;
                                tarifa.FechaVigenciaFinal = fechaVigenciaFinal;
                                tarifa.Incremento = incremento;
                                _context.Entry(tarifa).State = EntityState.Modified;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error for specific row, continue processing other rows
                            Console.WriteLine($"Error processing row {rowNum}: {ex.Message}");
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<IEnumerable<Cliente>> GetTarifasVencidas()
        {
            var query = @"
                SELECT DISTINCT
                    c.id,
                    c.nombre,
                    c.email,
                    c.acuerdo,
                    c.observaciones
                FROM tarifariogeneral_1 t
                JOIN cliente c ON t.cliente_id = c.id
                WHERE MONTH(t.fecha_vigencia_final) <= MONTH(GETDATE())-1

                UNION

                SELECT DISTINCT
                    c.id,
                    c.nombre,
                    c.email,
                    c.acuerdo,
                    c.observaciones
                FROM TarifarioEcommerce te
                JOIN cliente c ON te.cliente_id = c.id
                WHERE MONTH(te.fecha_vigencia_final) <=MONTH(GETDATE())-1
            ";

            var result = new List<Cliente>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                _context.Database.OpenConnection();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                            Acuerdo = reader.IsDBNull(reader.GetOrdinal("acuerdo")) ? null : reader.GetString(reader.GetOrdinal("acuerdo")),
                            Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones")) ? null : reader.GetString(reader.GetOrdinal("observaciones"))
                        });
                    }
                }
            }
            _context.Database.CloseConnection();
            return result;
        }

        public async Task<IEnumerable<TarifarioGeneralHistorico>> GetTarifasHistoricas(
            int? clienteId, string? categoria, int? unidadId, int? itemId, DateTime? fechaInicio, DateTime? fechaFin, DateTime? fechaMovimiento)
        {
            var query = new System.Text.StringBuilder(@"
                SELECT
                       H.id, H.OriginalId, H.item_id, H.unidad_id, H.cliente_id, H.minimo, H.Incremento, H.precio,
                       H.fecha_vigencia_inicio, H.fecha_vigencia_final, H.Usuario, H.Fecha_movimiento, H.Accion,
                       cl.nombre as cliente_nombre,
                       c.nombre as categoria_nombre,
                       u.nombre as unidad_nombre,
                       i.nombre as item_nombre
                FROM tarifariogeneralhistorico H
                JOIN item i ON H.item_id = i.id
                JOIN categoria c ON i.categoria = c.id
                JOIN cliente cl ON H.cliente_id = cl.id
                JOIN Unidades u ON H.unidad_id = u.id
            ");

            var conditions = new List<string>();
            var parameters = new List<SqlParameter>();

            if (clienteId.HasValue)
            {
                conditions.Add("H.cliente_id = @clienteId");
                parameters.Add(new SqlParameter("@clienteId", clienteId.Value));
            }
            if (!string.IsNullOrEmpty(categoria))
            {
                conditions.Add("c.nombre = @categoria"); // Filter by category name
                parameters.Add(new SqlParameter("@categoria", categoria));
            }
            if (unidadId.HasValue)
            {
                conditions.Add("H.unidad_id = @unidadId");
                parameters.Add(new SqlParameter("@unidadId", unidadId.Value));
            }
            if (itemId.HasValue)
            {
                conditions.Add("H.item_id = @itemId");
                parameters.Add(new SqlParameter("@itemId", itemId.Value));
            }
            if (fechaInicio.HasValue)
            {
                conditions.Add("H.fecha_vigencia_inicio >= @fechaInicio");
                parameters.Add(new SqlParameter("@fechaInicio", fechaInicio.Value));
            }
            if (fechaFin.HasValue)
            {
                conditions.Add("H.fecha_vigencia_final <= @fechaFin");
                parameters.Add(new SqlParameter("@fechaFin", fechaFin.Value));
            }
            if (fechaMovimiento.HasValue)
            {
                conditions.Add("CAST(H.fecha_movimiento AS DATE) = CAST(@fechaMovimiento AS DATE)");
                parameters.Add(new SqlParameter("@fechaMovimiento", fechaMovimiento.Value));
            }

            if (conditions.Any())
            {
                query.Append(" WHERE " + string.Join(" AND ", conditions));
            }

            var result = new List<TarifarioGeneralHistorico>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query.ToString();
                command.Parameters.AddRange(parameters.ToArray());
                _context.Database.OpenConnection();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new TarifarioGeneralHistorico
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            OriginalId = reader.GetInt32(reader.GetOrdinal("OriginalId")),
                            ClienteId = reader.GetInt32(reader.GetOrdinal("cliente_id")),
                            ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                            UnidadId = reader.GetInt32(reader.GetOrdinal("unidad_id")),
                            Minimo = reader.IsDBNull(reader.GetOrdinal("minimo")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("minimo")),
                            Incremento = reader.GetDouble(reader.GetOrdinal("incremento")),
                            Precio = reader.GetDouble(reader.GetOrdinal("precio")),
                            FechaVigenciaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_inicio")),
                            FechaVigenciaFinal = reader.IsDBNull(reader.GetOrdinal("fecha_vigencia_final")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_vigencia_final")),
                            Usuario = reader.GetInt32(reader.GetOrdinal("Usuario")),
                            FechaMovimiento = reader.GetDateTime(reader.GetOrdinal("Fecha_movimiento")),
                            Accion = reader.GetInt32(reader.GetOrdinal("Accion")),
                            Cliente = reader.GetString(reader.GetOrdinal("cliente_nombre")),
                            Categoria = reader.GetString(reader.GetOrdinal("categoria_nombre")),
                            Unidad = reader.GetString(reader.GetOrdinal("unidad_nombre")),
                            Item = reader.GetString(reader.GetOrdinal("item_nombre"))
                        });
                    }
                }
            }
            _context.Database.CloseConnection();
            return result;
        }

        public async Task ActualizarMaestrosDesdeOrigenExterno()
        {
            // This is a placeholder for actual logic to fetch data from an external source
            // and update the local database.
            // Example:
            // var externalData = await _externalApiService.GetMaestrosData();
            // foreach (var item in externalData)
            // {
            //     // Logic to update or insert into _context
            // }
            // await _context.SaveChangesAsync();

            await Task.CompletedTask; // Simulate async operation
        }

        public async Task ActualizarTarifasMasivoAsync(string usuarioModificacion)
        {
            _logger.LogInformation($"Iniciando actualización masiva de tarifas por {usuarioModificacion}...");

            try
            {
                // Ejemplo de cómo podrías llamar a un SP en Saadis_nuevo para obtener datos
                // DataTable dataFromSaadis = await _saadisNuevoContext.ExecuteStoredProcedureAsync("SP_ObtenerTarifasActualizadas");

                // Aquí iría la lógica para:
                // 1. Obtener los datos más recientes de las tarifas de la base de datos externa (Saadis_nuevo).
                //    Esto podría implicar llamar a un procedimiento almacenado o una consulta directa.
                // 2. Comparar estos datos con los existentes en GestionTarifas.
                // 3. Actualizar, insertar o desactivar tarifas según sea necesario en GestionTarifas.

                // Ejemplo de una operación de actualización ficticia en GestionTarifas
                // (Reemplaza con tu lógica real de actualización masiva)
                var tarifariosToUpdate = await _context.Tarifarios.Where(t => t.Activo).ToListAsync();
                foreach (var tarifario in tarifariosToUpdate)
                {
                    // Simula una actualización
                    tarifario.Valor += 0.01m; // Incrementa el valor ligeramente
                    tarifario.FechaModificacion = DateTime.Now;
                    tarifario.UsuarioModificacion = usuarioModificacion;
                }
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Actualización masiva de tarifas completada por {usuarioModificacion}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante la actualización masiva de tarifas por {usuarioModificacion}.");
                throw; // Re-lanza la excepción para que el controlador la maneje
            }
        }

        public async Task<IEnumerable<Actualizacion>> GetAllActualizacionesAsync()
        {
            return await _context.Actualizaciones.Where(a => a.Activo).ToListAsync();
        }

        public async Task<Actualizacion?> GetActualizacionByIdAsync(int id)
        {
            return await _context.Actualizaciones.FirstOrDefaultAsync(a => a.Id == id && a.Activo);
        }

        public async Task<Actualizacion> CreateActualizacionAsync(ActualizacionRequest request)
        {
            var actualizacion = new Actualizacion
            {
                TipoActualizacion = request.TipoActualizacion,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Resultado = request.Resultado,
                Exitosa = request.Exitosa,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.Actualizaciones.Add(actualizacion);
            await _context.SaveChangesAsync();
            return actualizacion;
        }

        public async Task<bool> UpdateActualizacionAsync(int id, ActualizacionRequest request)
        {
            var actualizacion = await _context.Actualizaciones.FindAsync(id);
            if (actualizacion == null)
            {
                return false;
            }

            actualizacion.TipoActualizacion = request.TipoActualizacion;
            actualizacion.FechaInicio = request.FechaInicio;
            actualizacion.FechaFin = request.FechaFin;
            actualizacion.Resultado = request.Resultado;
            actualizacion.Exitosa = request.Exitosa;
            actualizacion.FechaModificacion = DateTime.Now;

            _context.Actualizaciones.Update(actualizacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteActualizacionAsync(int id)
        {
            var actualizacion = await _context.Actualizaciones.FindAsync(id);
            if (actualizacion == null)
            {
                return false;
            }

            actualizacion.Activo = false; // Soft delete
            actualizacion.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class Actualizacion : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string TipoActualizacion { get; set; } = string.Empty; // e.g., "Tarifas", "Maestros"
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        [StringLength(500)]
        public string? Resultado { get; set; }
        public bool Exitosa { get; set; }
    }

    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
