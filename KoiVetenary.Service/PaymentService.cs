using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;

namespace KoiVetenary.Service;
public interface IPaymentService
{
    Task<IKoiVetenaryResult> SearchPaymentAsync(int ownerId, string searchTerm);
    Task<IKoiVetenaryResult> GetPaymentByIdAsync(int id);
}

public class PaymentService : IPaymentService
{

    private readonly UnitOfWork _unitOfWork;

    public PaymentService()
    {
        _unitOfWork ??= new UnitOfWork();
    }


    public async Task<IKoiVetenaryResult> GetPaymentByIdAsync(int id)
    { 
        try
        {

            var result = await _unitOfWork.PaymentRepository.GetByIdAsync(id);

            if (result != null)
            {
                return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            else
            {
                return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        catch (Exception ex)
        {
            return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
        }
    }

    public async Task<IKoiVetenaryResult> SearchPaymentAsync(int ownerId, string searchTerm)
    {
        try
        {

            var result = await _unitOfWork.PaymentRepository.SearchAsync(ownerId, searchTerm);

            if (result != null)
            {
                return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            else
            {
                return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        catch (Exception ex)
        {
            return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
        }
    }
}
