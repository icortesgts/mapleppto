using MaplePPTO.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaplePPTO.Models.Dapper
{
    public interface ICargaMasivaDA
    {
        Task<RespuestaDapperViewModel> CargaMasivaPacientes(CargaMasivaViewModel model);
        Task<int> CargaMasivaProyeccion(List<BulkPptoViewModel> listPpto, long userID);
        Task<RespuestaDapperViewModel> GetReports(ParametroViewModel model);
        Task<RespuestaDapperViewModel> GetFinancialReports(ParametroViewModel model);

        Task<RespuestaDapperViewModel> BulkInsertOrUpdateParametro(List<Parametro> parametros,long userID);

        Task<RespuestaDapperViewModel> GetSummaryReports();
        Task<int> ExecCalculateBase(long PresupuestoId, long EscenarioId);

    }
}