using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            this._context = context;
            this._context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
         
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        
         public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c=>c.Lotes)
                .Include(c=>c.RedesSociais);

            if (includePalestrantes){
                query=query
                    .Include(pe=>pe.PalestrantesEventos)
                    .ThenInclude(p=>p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c=>c.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c=>c.Lotes)
                .Include(c=>c.RedesSociais);

            if (includePalestrantes){
                query=query
                    .Include(pe=>pe.PalestrantesEventos)
                    .ThenInclude(p=>p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c=>c.DataEvento).Where(c=>c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(c=>c.Lotes)
                .Include(c=>c.RedesSociais);

            if (includePalestrantes){
                query=query
                    .Include(pe=>pe.PalestrantesEventos)
                    .ThenInclude(p=>p.Palestrante);
            }

            query = query.AsNoTracking().OrderByDescending(c=>c.DataEvento).Where(c=>c.EventoId == EventoId);

            return await query.FirstOrDefaultAsync();
        }        

        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c=>c.RedesSociais);

            if (includeEventos){
                query=query
                    .Include(pe=>pe.PalestrantesEventos)
                    .ThenInclude(e=>e.Evento);
            }

            query = query.AsNoTracking().Where(c=>c.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteByNomeAsync(string nome, bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
                .Include(c=>c.RedesSociais);

            if (includeEventos){
                query=query
                    .Include(pe=>pe.PalestrantesEventos)
                    .ThenInclude(e=>e.Evento);
            }

            query = query.AsNoTracking().OrderBy(p=>p.Nome)
                .Where(c=>c.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

       
    }
}