using T_Speich_CPT_206_Lab_5.Models;

namespace T_Speich_CPT_206_Lab_5
{
    public interface IStateRepository
    {
        Task<State?> CreateAsync(State state);
        Task<IEnumerable<State>> RetrieveAllAsync();
        Task<State?> RetrieveAsync(int id);
        Task<State?> UpdateAsync(int id, State state);
        Task<bool?> DeleteAsync(int id);    
    }
}
