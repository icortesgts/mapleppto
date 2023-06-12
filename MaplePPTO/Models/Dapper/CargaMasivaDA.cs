using Dapper;
using MaplePPTO.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MaplePPTO.Models.Dapper
{
    public class CargaMasivaDA : ICargaMasivaDA
    {
        protected readonly IConnectionFactory _connectionFactory;

        public CargaMasivaDA()
        {
            _connectionFactory = new ConnectionFactory();
        }
        /* fin connection using Micro ORM Dapper*/

        public async Task<RespuestaDapperViewModel> CargaMasivaPacientes(CargaMasivaViewModel model)
        {
            var oResponse = new RespuestaDapperViewModel();
            var dt = GetTableTypePacientes(model.Data);
            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id_escenario", model.id_escenario);
                parameters.Add("@id_ppto", model.id_ppto);
                parameters.Add("@tipoInformacion", model.tipoInformacion);
                parameters.Add("@id_usuario", model.id_usuario);
                parameters.Add("@pacientes", dt.AsTableValuedParameter("dbo.tvp_Pacientes"));
                oResponse = await connection.QueryFirstOrDefaultAsync<RespuestaDapperViewModel>("dbo.usp_CargaMasivaPresupuesto", parameters, commandTimeout:300, commandType: System.Data.CommandType.StoredProcedure);

            }
            return oResponse;
        }



        public async Task<RespuestaDapperViewModel> GetReports(ParametroViewModel model)
        {
            var response = new RespuestaDapperViewModel();
            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@tipoReporteId", model.TipoReporteId);
                parameters.Add("@presupuestoId", model.PresupuestoId);
                parameters.Add("@escenarioID", model.EscenarioId);
                var oResponse = await connection.QueryAsync<ResultPpto>("dbo.sp_GenerarReportes", parameters, commandType: System.Data.CommandType.StoredProcedure);

                response.reports = oResponse.ToList();
            }
            return response;
        }        
        public async Task<RespuestaDapperViewModel> GetFinancialReports(ParametroViewModel model)
        {
            var response = new RespuestaDapperViewModel();
            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@tipoReporteId", model.TipoReporteId);
                parameters.Add("@presupuestoId", model.PresupuestoId);
                parameters.Add("@escenarioID", model.EscenarioId);
                var oResponse = await connection.QueryAsync<ResultPpto>("dbo.sp_GenerarReportesFinanciero", parameters, commandType: System.Data.CommandType.StoredProcedure);

                response.reports = oResponse.ToList();
            }
            return response;
        }

        private DataTable GetTableTypePacientes(IEnumerable<bulkData> Data)
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add("Cliente");
            tabla.Columns.Add("Ciudad");
            tabla.Columns.Add("Contrato");
            tabla.Columns.Add("Valor");
            tabla.Columns.Add("Anio");
            tabla.Columns.Add("Mes");

            foreach (var item in Data)
            {
                DataRow dr = tabla.NewRow();
                dr[0] = item.Cliente;
                dr[1] = item.Ciudad;
                dr[2] = item.Contrato;
                dr[3] = Math.Round(item.ValorPeriodo);
                dr[4] = item.Anio;
                dr[5] = item.Mes;

                tabla.Rows.Add(dr);
            }
            return tabla;
        }

        public async Task<RespuestaDapperViewModel> BulkInsertOrUpdateParametro(List<Parametro> parametros, long userID)
        {
            



            var xmlData = new XElement("DATAPARENT",
             from e in parametros
             select new XElement("DATA",
                 new XElement("id", e.id),
                 new XElement("Nombre", e.Nombre),
                 new XElement("ValorNum", e.ValorNum),
                 new XElement("id_cliente", e.id_cliente),
                 new XElement("id_ppto", e.id_ppto),
                 new XElement("id_escenario", e.id_escenario),
                 new XElement("usuario_crea", userID)
             ));

            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dataParametro", "<?xml version='1.0' ?>" + xmlData, System.Data.DbType.Xml);
                var data = await connection.QueryFirstOrDefaultAsync<RespuestaDapperViewModel>("dbo.usp_BulkInsertOrUpdateParametro", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return data;
            }
           
        }

        public async Task<int> CargaMasivaProyeccion(List<BulkPptoViewModel> listPpto, long userID)
        {
            var dt = GetTableTypeCtoPpto(listPpto);

            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id_usuario", userID);
                parameters.Add("@listData", dt.AsTableValuedParameter("dbo.tvp_Conceptoppto"));
                var result = await connection.QueryFirstOrDefaultAsync<int>("dbo.usp_CargaMasivaPresupuestoDesdeFormulario", parameters, commandTimeout: 300, commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }

        }

        private DataTable GetTableTypeCtoPpto(IEnumerable<BulkPptoViewModel> Data)
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add("id_concepto");
            tabla.Columns.Add("id_cliente");
            tabla.Columns.Add("id_localizacion");
            tabla.Columns.Add("id_escenario");
            tabla.Columns.Add("id_ppto");
            tabla.Columns.Add("Mes");
            tabla.Columns.Add("Vigencia");
            tabla.Columns.Add("valor");

            foreach (var item in Data)
            {
                DataRow dr = tabla.NewRow();
                dr[0] = item.id_concepto;
                dr[1] = item.id_cliente;
                dr[2] = item.id_localizacion;
                dr[3] = item.id_escenario;
                dr[4] = item.id_ppto;
                dr[5] = item.Mes;
                dr[6] = item.Anio;
                dr[7] = item.Valor;

                tabla.Rows.Add(dr);
            }
            return tabla;
        }

        public async Task<RespuestaDapperViewModel> GetSummaryReports()
        {
            var response = new RespuestaDapperViewModel();
            using (var connection = _connectionFactory.GetConnection)
            {
                var query = @"	SELECT 
	                            c.Nombre,
	                            r.fecha,
	                            r.cop,
	                            r.usd,
	                            r.porcentaje,
                                ct.orden
	                            FROM reportSummary r
	                            INNER join ConceptosPorTipoReporte ct on r.id_concepto = ct.conceptoId 
	                            inner join Concepto c on ct.conceptoId = c.id
	                            where ct.tipoReporteId  = 38 and c.id not in (376,377)
	                            order by ct.orden asc";

                var oResponse = await connection.QueryAsync<ResultSummary>(query);

                response.resultSummaries = oResponse.ToList();
            }
            return response;
        }

        public async Task<int> ExecCalculateBase(long PresupuestoId, long EscenarioId)
        {
            var response = 0;
            using (var connection = _connectionFactory.GetConnection)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id_ppto", PresupuestoId);
                parameters.Add("@id_escenario", EscenarioId);
                var oResponse = await connection.ExecuteAsync("dbo.SP_CalculoBase", parameters, commandTimeout:1200, commandType: System.Data.CommandType.StoredProcedure);
                response = oResponse;
            }
            return response;
        }

        public class ListDataConceptopPptoSqlData : List<BulkPptoViewModel>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("id_concepto", SqlDbType.BigInt),
                    new SqlMetaData("id_cliente", SqlDbType.BigInt),
                    new SqlMetaData("id_localizacion", SqlDbType.BigInt),
                    new SqlMetaData("id_escenario", SqlDbType.BigInt),
                    new SqlMetaData("id_ppto", SqlDbType.BigInt),
                    new SqlMetaData("Mes", SqlDbType.Int),
                    new SqlMetaData("Vigencia", SqlDbType.Int),
                    new SqlMetaData("valor", SqlDbType.Float)
                    );
                foreach (BulkPptoViewModel item in this)
                {
                    sqlDataRecord.SetInt64(0, item.id_concepto);
                    sqlDataRecord.SetInt64(1, item.id_cliente);
                    sqlDataRecord.SetInt64(2, item.id_localizacion);
                    sqlDataRecord.SetInt64(3, item.id_escenario);
                    sqlDataRecord.SetInt64(4, item.id_ppto);
                    sqlDataRecord.SetInt32(5, item.Mes);
                    sqlDataRecord.SetInt32(6, item.Anio);
                    sqlDataRecord.SetSqlDouble(7, item.Valor);
                    yield return sqlDataRecord;
                }
            }
        }


    }
}