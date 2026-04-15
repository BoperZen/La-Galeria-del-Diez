using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace La_Galeria_del_Diez.Infraestructure.Repository.Implementations
{
    public class RepositoryObject : IRepositoryObject
    {
        private readonly La_Galeria_del_Diez_Context _context;

        public RepositoryObject(La_Galeria_del_Diez_Context context)
        {
            _context = context;
        }

        public async Task<AuctionableObject> FindByIdAsync(int id)
        {
            var @object = await _context.Set<AuctionableObject>()
                                        .Include(o => o.Image)
                                        .Include(o => o.IdCategory)
                                        .Include(o => o.IdStateNavigation)
                                        .Include(o => o.IdUserNavigation)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(o => o.Id == id);

            if (@object != null)
            {
                @object.Auction = await _context.Set<Auction>()
                    .Where(a => a.IdObject == id)
                    .Include(a => a.IdUserNavigation)
                    .Include(a => a.IdStateNavigation)
                    .Include(a => a.Bidding)
                        .ThenInclude(b => b.IdUserNavigation)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return @object!;
        }

        public async Task<ICollection<AuctionableObject>> ListAsync()
        {
            var collection = await _context.Set<AuctionableObject>()
                                        .Include(x => x.Image)
                                        .Include(x => x.IdCategory)
                                        .Include(x => x.IdStateNavigation)
                                        .Include(x => x.IdUserNavigation)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .ToListAsync();

            var objectIds = collection.Select(x => x.Id).ToList();

            if (objectIds.Any())
            {
                var auctions = await _context.Set<Auction>()
                    .Where(a => objectIds.Contains(a.IdObject))
                    .Include(a => a.IdUserNavigation)
                    .Include(a => a.IdStateNavigation)
                    .Include(a => a.Bidding)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var obj in collection)
                {
                    obj.Auction = auctions.Where(a => a.IdObject == obj.Id).ToList();
                }
            }

            return collection;
        }

        public async Task<ICollection<AuctionableObject>> ListAsyncNoAuction()
        {
            var collection = await _context.Set<AuctionableObject>()
                                        .Include(x => x.Image)
                                        .Include(x => x.IdCategory)
                                        .Include(x => x.IdStateNavigation)
                                        .Include(x => x.IdUserNavigation)
                                        .Where(x => x.IdState == 6)
                                        .Where(x => !_context.Set<Auction>()
                                            .Any(a => a.IdObject == x.Id &&
                                                          (a.IdState == 1 ||
                                                           a.IdState == 2 ||
                                                           a.IdState == 5 ||
                                                           a.IdState == 7 ||
                                                           a.IdState == 6)))
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .ToListAsync();
            return collection;
        }

        public async Task AddAsync(AuctionableObject auctionableObject)
        {
            var categoryIds = auctionableObject.IdCategory?.Select(c => c.Id).Distinct().ToList() ?? new List<int>();
            var imagePayload = auctionableObject.Image?
                .Where(i => i.Data != null && i.Data.Length > 0)
                .Select(i => new Image
                {
                    Data = i.Data,
                    RegistrationDate = i.RegistrationDate == default ? DateTime.Now : i.RegistrationDate
                })
                .ToList() ?? new List<Image>();

            auctionableObject.IdCategory = new List<Category>();
            auctionableObject.Image = new List<Image>();

            _context.Set<AuctionableObject>().Add(auctionableObject);
            await _context.SaveChangesAsync();

            if (categoryIds.Any())
            {
                var categories = await _context.Set<Category>()
                    .Where(c => categoryIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var category in categories)
                {
                    auctionableObject.IdCategory.Add(category);
                }
            }

            foreach (var image in imagePayload)
            {
                image.IdObject = auctionableObject.Id;
                _context.Set<Image>().Add(image);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AuctionableObject auctionableObject)
        {
            var existing = await _context.Set<AuctionableObject>()
                .Include(o => o.IdCategory)
                .Include(o => o.Image)
                .FirstOrDefaultAsync(o => o.Id == auctionableObject.Id);

            if (existing == null)
            {
                return;
            }

            existing.Name = auctionableObject.Name;
            existing.Description = auctionableObject.Description;
            existing.Condition = auctionableObject.Condition;
            existing.IdState = auctionableObject.IdState;

            var hasActiveAuction = await HasActiveAuctionAsync(existing.Id);
            if (hasActiveAuction || existing.IdState != 6)
            {
                throw new InvalidOperationException("El objeto no puede editarse por su estado actual o por tener subasta activa.");
            }

            var categoryIds = auctionableObject.IdCategory?.Select(c => c.Id).Distinct().ToList() ?? new List<int>();
            var categories = await _context.Set<Category>()
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            existing.IdCategory.Clear();
            foreach (var category in categories)
            {
                existing.IdCategory.Add(category);
            }

            var newImages = auctionableObject.Image?
                .Where(i => i.Data != null && i.Data.Length > 0)
                .Select(i => new Image
                {
                    Data = i.Data,
                    RegistrationDate = i.RegistrationDate == default ? DateTime.Now : i.RegistrationDate,
                    IdObject = existing.Id
                })
                .ToList() ?? new List<Image>();

            if (newImages.Any())
            {
                _context.Set<Image>().AddRange(newImages);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<AuctionableObject>()
                .Include(o => o.Image)
                .Include(o => o.IdCategory)
                .Include(o => o.Auction)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null)
            {
                return;
            }

            existing.IdState = 4;
            await _context.SaveChangesAsync();
        }

        public Task<bool> HasActiveAuctionAsync(int id)
        {
            return _context.Set<Auction>()
                .AnyAsync(a => a.IdObject == id && a.EndDate > DateTime.Now);
        }

        public async Task<bool> CanEditAsync(int id)
        {
            var obj = await _context.Set<AuctionableObject>()
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obj == null)
            {
                return false;
            }

            var hasActiveAuction = await HasActiveAuctionAsync(id);
            return obj.IdState == 6 && !hasActiveAuction;
        }
    }
}