using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Presistence.Seed
{
    public interface ISeedProvider
    {
        void InitProduction();
        void InitDevelopment();
    }
}
