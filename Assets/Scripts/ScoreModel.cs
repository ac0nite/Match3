using Match3.Context;

namespace ID
{
    public interface IScoreModel
    {
        int Round { get; set; }
        int Attempted { get; set; }
    }
    
    public class ScoreModel : IScoreModel
    {
        public ScoreModel(ApplicationContext context)
        {
            Round = 0;
            Attempted = 0;
        }
        
        public int Round { get; set; }
        public int Attempted { get; set; }
    }
}