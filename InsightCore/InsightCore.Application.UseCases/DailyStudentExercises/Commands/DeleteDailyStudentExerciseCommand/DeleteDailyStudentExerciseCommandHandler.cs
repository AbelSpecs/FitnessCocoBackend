using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.DeleteDailyStudentExerciseCommand
{
    public class DeleteDailyStudentExerciseCommandHandler : IRequestHandler<DeleteDailyStudentExerciseCommand, Response<bool>>
    {
        private readonly IDailyStudentExercisesRepository _repository;
        public DeleteDailyStudentExerciseCommandHandler(IDailyStudentExercisesRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<bool>> Handle(DeleteDailyStudentExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _repository.DeleteAsync(request.Id); // using DeleteAsync for soft-delete
                response.IsSuccess = deleted;
                response.Data = deleted;
                response.Message = deleted ? "DailyStudentExercise deleted." : "DailyStudentExercise not found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}


