using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.DeleteDailyStudentExerciseCommand
{
    public class DeleteDailyStudentExerciseCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }

}
