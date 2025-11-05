using ProdutividadeApp.Models;
using SQLite;

public class GoalService
{
    private readonly SQLiteAsyncConnection _db;

    public GoalService(SQLiteAsyncConnection db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _db.CreateTableAsync<MoodEntry>().Wait();
        _db.CreateTableAsync<Goal>().Wait();
    }

    public Task<List<Goal>> GetAllGoalsAsync()
    {
        return _db.Table<Goal>().ToListAsync();
    }

    public Task<int> AddGoalAsync(Goal goal)
    {
        return _db.InsertAsync(goal);
    }

    public Task<int> DeleteGoalAsync(Goal goal)
    {
        return _db.DeleteAsync(goal);
    }

    public Task<int> UpdateGoalAsync(Goal goal)
    {
        return _db.UpdateAsync(goal);
    }
}
