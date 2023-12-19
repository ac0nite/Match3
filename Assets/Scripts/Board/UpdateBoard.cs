using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.General;

namespace Match3.Board
{
    public interface IUpdateBoard
    {
        UniTask UpdateAsync();
    }
    
    public class UpdateBoard : IUpdateBoard
    {
        private readonly IMatching _matching;
        private readonly IClearing _cleaning;
        private readonly IShifting _shifting;
        private readonly ICheckResult _checkingResult;

        public UpdateBoard(ApplicationContext context)
        {
            _matching = context.Resolve<IMatching>();
            _cleaning = context.Resolve<IClearing>();
            _shifting = context.Resolve<IShifting>();
            _checkingResult = context.Resolve<ICheckResult>();
        }
        public async UniTask UpdateAsync()
        {
            await _shifting.AllShiftAsync();
            _matching.Find();

            do
            {
                await _cleaning.MatchExecuteAsync();
                //await UniTask.Delay(5000);
                await _shifting.AllShiftAsync();
            } 
            while (_matching.Find());
            
            _checkingResult.Check();
        }
    }
}