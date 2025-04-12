using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using T_Speich_CPT_206_Lab_5.Models;

namespace T_Speich_CPT_206_Lab_5.Repositories
{
    public class StateRepository : IStateRepository
    {
        private static ConcurrentDictionary<int, State>? stateCache;
        //instance of the datacontext 
        private StateDbContext db;

        public StateRepository(StateDbContext injectedContext)
        {
            db = injectedContext;
            //load the states from the database to a dictionary
            if (stateCache == null)
            {
                
                stateCache = new ConcurrentDictionary<int, State>(
                    db.State.ToDictionary(x => x.State_ID));
            }

        }
        public async Task<State?> CreateAsync(State s)
        {
            //add to db user ef core
            EntityEntry<State> addedState = await db.State.AddAsync(s);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (stateCache == null) return s;
                //if state is new add to cache else call update
                return stateCache.AddOrUpdate(s.State_ID, s, UpdateCache);
            }
            else
            {
                return null;
            }
        }
        public Task<IEnumerable<State>> RetrieveAllAsync()
        {
            return Task.FromResult(stateCache is null ? Enumerable.Empty<State>() : stateCache.Values);
        }
        public Task<State?> RetrieveAsync(int id)
        {
            if (stateCache == null) return null!;
            stateCache.TryGetValue(id, out State? state);
            return Task.FromResult(state);
        }

        private State UpdateCache(int id, State state)
        {
            State? old;
            if (stateCache is not null)
            {
                if (stateCache.TryGetValue(id, out old))
                {
                    if (stateCache.TryUpdate(id, state, old))
                    {
                        return state;
                    }
                }
            }
            return null!;
        }
        public async Task<State?> UpdateAsync(int id, State state)
        {

            //update the database
            db.State.Update(state);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                //update the cache
                return UpdateCache(id, state);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            //remove from db
            State? state = db.State.Find(id);
            if (state == null)
            {
                return null;//nothing to delete
            }
            db.State.Remove(state);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (stateCache is null) return null;
                //else remove
                return stateCache.TryRemove(id, out state);
            }
            else { return null; }
        }
    }
}

