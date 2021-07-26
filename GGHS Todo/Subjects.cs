using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGHS
{
    interface ISubjects
    {
        int Grade { get; }
    }
    namespace Grade1
    {
        class Subjects : ISubjects
        {
            int ISubjects.Grade { get; } = 1;
        }
    }
    namespace Grade2
    {
        class Subjects : ISubjects
        {
            int ISubjects.Grade { get; } = 2;
        }
    }
    namespace Grade3
    {
        class Subjects : ISubjects
        {
            int ISubjects.Grade { get; } = 3;
        }
    }
}
