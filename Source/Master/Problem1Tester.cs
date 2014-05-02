using MasterServer.Interfaces;

namespace MasterServer
{
    public class Problem1Tester : ITester
    {
        public bool Test(ISolution solution)
        {
            Problem1 soln = solution as Problem1;
            int sum = soln.Add(1, 2);

            return sum == 3;
        }
    }
}
