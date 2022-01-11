using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Common.Interfaces;

public interface IQiniuService
{
    Task<string> Upload(byte[] data,string fileName);
}
