using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Infrastructure.Configurations;

public class  QiniuSettings
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Bucket { get; set; }
    public string Domain { get; set; }
}
