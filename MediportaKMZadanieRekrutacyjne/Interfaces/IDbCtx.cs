using MediportaKMZadanieRekrutacyjne.Models;
using Microsoft.EntityFrameworkCore;

namespace MediportaKMZadanieRekrutacyjne.Interfaces
{
    /// <summary>
    /// Implement interface in database context class, to provide compliance with generic methods which uses generic types
    /// </summary>
    public interface IDbCtx
    {
        public DbSet<Tag> Tags { get; set; }
    }
}
