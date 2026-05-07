using AutoMapper;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.UseCases.Students.Queries.GetStudentQuery;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.Qrs.Queries
{
    public class RedirectToCoachQueryHandler : IRequestHandler<RedirectToCoachQuery, Response<QRTokenRegistroDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQrsRepository _qrsRepository;

        public RedirectToCoachQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IQrsRepository qrsRepository )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _qrsRepository = qrsRepository;
        }

        public async Task<Response<QRTokenRegistroDto>> Handle(RedirectToCoachQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dataDto = new QRTokenRegistroDto();

                var qrData = await _qrsRepository.GetCoachTokensByTokenAsync(request.Token);
                if (qrData == null)
                {
                    return new Response<QRTokenRegistroDto> { IsSuccess = false, Message = "qr expiried." };
                }

                dataDto.CoachId = qrData.CoachId;
                dataDto.Url = $"https://insightcore.com/coach/{qrData.CoachId}";
                return new Response<QRTokenRegistroDto> { IsSuccess = true, Data = dataDto };

            }
            catch (Exception ex)
            {
                return new Response<QRTokenRegistroDto> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
