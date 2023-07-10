using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Presistence.IProvider
{
    public interface IPdfProvider
    {
        Task<byte[]> Get(string htmlContent, bool landscape = false);
    }
}
