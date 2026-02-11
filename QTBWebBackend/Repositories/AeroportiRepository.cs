using Microsoft.EntityFrameworkCore;
using QTBWebBackend.Interfaces;
using QTBWebBackend.Models;
using QTBWebBackend.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QTBWebBackend.Repositories
{
    public class AeroportiRepository(QTBWebDBContext contesto) : IAeroportiRepository
    {
        public async Task<List<AeroportoViewModel>> GetAeroportiAsync()
        {
            return await contesto.Aeroporti
                .Select(aeroporto => new AeroportoViewModel
                {
                    Id = aeroporto.Id,
                    Nome = aeroporto.Nome,
                    Denominazione = aeroporto.Denominazione,
                    IdTipoAeroporto = aeroporto.Tipo.Id,
                    TipoAeroporto = aeroporto.Tipo.Descrizione,
                    Identificativo = aeroporto.Identificativo,
                    Coordinate = aeroporto.Coordinate,
                    Icao = aeroporto.Icao,
                    Iata = aeroporto.Iata,
                    QNH = aeroporto.Qnh,
                    QFU = aeroporto.Qfu,
                    Lunghezza = aeroporto.Lunghezza,
                    Asfalto = aeroporto.Asfalto,
                    Radio = aeroporto.Radio,
                    Indirizzo = aeroporto.Indirizzo,
                    Telefono = aeroporto.Telefono,
                    Email = aeroporto.Email,
                    Web = aeroporto.Web,
                    Note = aeroporto.Note
                })
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto)
        {
            return await contesto.Aeroporti
                .Where(a => a.Id == idAeroporto)
                .Select(aeroporto => new AeroportoViewModel
                {
                    Id = aeroporto.Id,
                    Nome = aeroporto.Nome,
                    Denominazione = aeroporto.Denominazione,
                    IdTipoAeroporto = aeroporto.Tipo.Id,
                    TipoAeroporto = aeroporto.Tipo.Descrizione,
                    Identificativo = aeroporto.Identificativo,
                    Coordinate = aeroporto.Coordinate,
                    Icao = aeroporto.Icao,
                    Iata = aeroporto.Iata,
                    QNH = aeroporto.Qnh,
                    QFU = aeroporto.Qfu,
                    Lunghezza = aeroporto.Lunghezza,
                    Asfalto = aeroporto.Asfalto,
                    Radio = aeroporto.Radio,
                    Indirizzo = aeroporto.Indirizzo,
                    Telefono = aeroporto.Telefono,
                    Email = aeroporto.Email,
                    Web = aeroporto.Web,
                    Note = aeroporto.Note
                })
                .SingleOrDefaultAsync();
        }

        public async Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync()
        {
            return await contesto.TipiAeroporti
                .Select(tipoAeroporto => new TipoAeroportoViewModel
                {
                    Id = tipoAeroporto.Id,
                    Descrizione = tipoAeroporto.Descrizione
                })
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto)
        {
            return await contesto.TipiAeroporti
                .Where(t => t.Id == idTipoAeroporto)
                .Select(tipoAeroporto => new TipoAeroportoViewModel
                {
                    Id = tipoAeroporto.Id,
                    Descrizione = tipoAeroporto.Descrizione
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Aeroporti?> AddAeroportoAsync(AeroportoViewModel aeroportoModel)
        {
            var nuovoAeroporto = new Aeroporti
            {
                Nome = aeroportoModel.Nome,
                Denominazione = aeroportoModel.Denominazione,
                TipoId = aeroportoModel.IdTipoAeroporto,
                Identificativo = aeroportoModel.Identificativo,
                Coordinate = aeroportoModel.Coordinate,
                Icao = aeroportoModel.Icao,
                Iata = aeroportoModel.Iata,
                Qnh = aeroportoModel.QNH,
                Qfu = aeroportoModel.QFU,
                Asfalto = aeroportoModel.Asfalto,
                Lunghezza = aeroportoModel.Lunghezza,
                Radio = aeroportoModel.Radio,
                Indirizzo = aeroportoModel.Indirizzo,
                Telefono = aeroportoModel.Telefono,
                Email = aeroportoModel.Email,
                Web = aeroportoModel.Web,
                Note = aeroportoModel.Note
            };

            try
            {
                contesto.Aeroporti.Add(nuovoAeroporto);
                var changes = await contesto.SaveChangesAsync();
                return changes > 0 ? nuovoAeroporto : null;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }
    }
}
