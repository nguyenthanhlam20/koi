using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Business
{

    public interface IKoiVetenaryResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }

    public class KoiVetenaryResult : IKoiVetenaryResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public KoiVetenaryResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public KoiVetenaryResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public KoiVetenaryResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
