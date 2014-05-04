using System;

using MasterServer.Interfaces;

namespace MasterServer.TesterFiles
{
    public class Problem1Tester : ITester
    {
        public bool Test(Object soln)
        {
            IProblem1 solution = soln as IProblem1;
            int sum = solution.Add(1, 2);

            return sum == 3;
        }
    }
}
