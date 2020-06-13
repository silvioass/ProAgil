using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T: class;
         void Update<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
        Task<bool> SaveChangesAsync();

        Task<Evento[]> GetAllEventoByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
        Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes);

        Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteByNomeAsync(string nome, bool includeEventos);
    }
}